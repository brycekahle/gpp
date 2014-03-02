'use strict';

// A little lame, but want this for easy debugging
var player1, playerShip, game, enemies, shipBullets, player2Bullets, player2, starsprite, music;
var sunsprite, rocksprite, gassprite;
var score = 0, scoreText, boom;

var musicVolume = 0.2;

window.onload = function() {
  game = new Phaser.Game(960, 600, Phaser.CANVAS, '', { preload: preload, create: create, update: update, render: render });

  var starSpeed = 10.0; // 100x / second

  function preload() {
    game.load.image('starfield', 'assets/starfield.png');
    game.load.image('gasgiant', 'assets/gas_giant.png');
    game.load.image('sun', 'assets/small_sun.png');
    game.load.image('rock', 'assets/foreground_rock.png');
    game.load.image('shield', 'assets/player-ship-sheild.png');
    game.load.image('logo', 'assets/logo.png');
    game.load.image('overlay', 'assets/overlay.png');
    
    player1 = game.load.spritesheet('player1', 'assets/player_ship.png', 256, 128);
    game.load.spritesheet('enemy', 'assets/enemy-small.png', 96, 59);
    game.load.spritesheet('reticle', 'assets/reticle.png', 230, 230);
    game.load.spritesheet('bullet', 'assets/side_missle.png', 128, 32);
    game.load.spritesheet('player2Missile', 'assets/top_down_missle.png', 256, 256);
    game.load.spritesheet('deathbits', 'assets/deathbits.png', 10, 10);
    game.load.audio('music1', ['assets/music1.mp3']);

    game.load.audio('laser1', ['assets/Laser 1.mp3']);
    game.load.audio('laser2', ['assets/Laser 2.mp3']);
    game.load.audio('laser3', ['assets/Laser 3.mp3']);
    game.load.audio('laser4', ['assets/Laser 4.mp3']);
    game.load.audio('laser5', ['assets/Laser 5.mp3']);
    game.load.audio('boom', ['assets/Explosion.mp3']);
    game.load.audio('missile', ['assets/Missile.mp3']);
  }

  function create() {
    starsprite = game.add.tileSprite(0, 0, 4096, 1024, 'starfield');
    sunsprite = game.add.tileSprite(0, 0, 2048, 1024, 'sun');
    gassprite = game.add.tileSprite(0, 0, 2048, 1024, 'gasgiant');

    var logosprite = game.add.sprite(480, 0, 'logo');
    logosprite.x -= logosprite.width/2;

    boom = game.add.audio('boom');

    music = game.add.audio('music1', musicVolume, true);
    music.play('', 0, musicVolume, true);
    //game.world.setBounds(0, 0, 80000, 600);

    enemies = game.add.group();
    shipBullets = game.add.group();
    shipBullets.createMultiple(50, 'bullet');
    shipBullets.setAll('autoCull', true);
    shipBullets.setAll('scale.x', 0.7);
    shipBullets.setAll('scale.y', 0.7);
    shipBullets.setAll('anchor.x', 0.5);
    shipBullets.setAll('anchor.y', 0.5);
    shipBullets.callAll('events.onOutOfBounds.add', 'events.onOutOfBounds', resetBullet, this);

    player2Bullets = game.add.group();
    player2Bullets.createMultiple(10, 'player2Missile');
    player2Bullets.setAll('autoCull', true);
    player2Bullets.callAll('events.onOutOfBounds.add', 'events.onOutOfBounds', resetBullet, this);

    game.input.gamepad.start();

    playerShip = new PlayerShip(game, shipBullets);
    player2 = new PlayerAssist(game, player2Bullets);

    for (var i=0; i < 200; i++) {
      enemies.add(new Enemy(game, 'enemy', 10));
    }

    game.input.onDown.add(pauseToggle, this);
    game.stage.fullScreenScaleMode = Phaser.StageScaleMode.SHOW_ALL;
    var fKey = game.input.keyboard.addKey(Phaser.Keyboard.F);
    fKey.onDown.add(goFull, this);
    var mKey = game.input.keyboard.addKey(Phaser.Keyboard.M);
    mKey.onDown.add(toggleMute, this);

    rocksprite = game.add.tileSprite(0, 0, 4096, 1024, 'rock');
    game.add.sprite(0, 0, 'overlay');
    scoreText = game.add.text(16, 16, 'Score: 0', { font: '32px arial', fill: '#fff' });  
  }

  function update() {
    player2.update();
    
    var xdiff = (starSpeed * (game.time.elapsed / 1000));
    starsprite.tilePosition.x -= xdiff;
    gassprite.tilePosition.x -= xdiff * 8;
    rocksprite.tilePosition.x -= xdiff * 40;
    sunsprite.tilePosition.x -= xdiff * 2;

    game.physics.overlap(playerShip, enemies, enemyCollide);
    game.physics.overlap(shipBullets, enemies, bulletCollide);
    game.physics.overlap(player2Bullets, enemies, bullet2Collide, bulletBeforeCollide);
    game.physics.overlap(playerShip, player2Bullets, healPlayer, beforeCollideHeal);

    shipBullets.forEach(function (bullet) {
      if (bullet.alive && !bullet.renderable) {
        bullet.kill();
      }
    });

    player2Bullets.forEach(function (bullet) {
      if (bullet.alive){
        if (!bullet.renderable || bullet.scale.x <= 0) {
          bullet.kill();
        } else{
          bullet.update();
        }
      }
    });
  }

  function render() {
    //game.debug.renderCameraInfo(game.camera, 32, 32);
    //game.debug.renderSpriteCoords(playerShip, 32, 100);
  }

  function pauseToggle() {
    game.paused = !game.paused;
  }
  function goFull() {
    game.stage.scale.startFullScreen();
  }
  function toggleMute() {
    music.volume = music.volume ? 0 : musicVolume;
  }

  function enemyCollide(player, enemy) {
    if (player.shield.alive) {
      player.shield.kill();
    } else {
      boom.play();
      player.kill();
      setTimeout(function () {
        window.location.reload();
      }, 1500);
    }
    enemy.shield.kill();
    enemy.kill();
  }
  function bulletBeforeCollide(bullet, enemy){
    // player 2 bullets only hit when 0.2 or less
    return bullet.scale.x <= 0.2;
  }
  function beforeCollideHeal(player, bullet){
    // player 2 bullets only hit when 0.2 or less
    return bullet.scale.x <= 0.2;
  }
  function bulletCollide(bullet, enemy) {
    bullet.kill();
    if (enemy.shield.alive) {
      return;
    };
    enemy.kill();
    enemy.reset(game.rnd.integerInRange(5000, 10000), game.rnd.integerInRange(25, 575), 1);
    score += enemy.score;
    scoreText.content = 'Score: ' + score; 
    boom.play();
  }
  function bullet2Collide(bullet, enemy) {
    bullet.kill();
    if (enemy.shield.alive) {
      enemy.shield.kill();
      return;
    };
    enemy.kill();
    score += enemy.score;
    scoreText.content = 'Score: ' + score; 
  }
  function healPlayer(player, bullet) {
    bullet.kill();
    if (!player.shield.alive) {
      player.shield.reset();
      player.shield.scale.x = 0.25;
      player.shield.scale.y = 0.25;
    }
  }
  //  Called if the bullet goes out of the screen
  function resetBullet (bullet) {
    bullet.kill();
  }
};

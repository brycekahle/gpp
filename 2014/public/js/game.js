'use strict';

// A little lame, but want this for easy debugging
var player1, playerShip, game, enemies, shipBullets, player2Bullets, player2, starsprite, music;

var musicVolume = 0.5;

window.onload = function() {
  game = new Phaser.Game(960, 600, Phaser.CANVAS, '', { preload: preload, create: create, update: update, render: render });

  var starSpeed = 10.0; // 100x / second

  function preload() {
    game.load.image('starfield', 'assets/starfield.png');
    player1 = game.load.spritesheet('player1', 'assets/player_ship.png', 256, 128);
    game.load.spritesheet('enemy', 'assets/enemy-small.png', 96, 59);
    game.load.spritesheet('reticle', 'assets/reticle.png', 230, 230);
    game.load.spritesheet('bullet', 'assets/bullet.png', 200, 200);
    game.load.spritesheet('deathbits', 'assets/deathbits.png', 10, 10);
    game.load.audio('music1', ['assets/music1.mp3']);
  }

  function create() {
    starsprite = game.add.tileSprite(0, 0, 4096, 1024, 'starfield');
    music = game.add.audio('music1', musicVolume, true);
    music.play('', 0, 0, true);
    //game.world.setBounds(0, 0, 80000, 600);

    enemies = game.add.group();
    shipBullets = game.add.group();
    shipBullets.createMultiple(50, 'bullet');
    shipBullets.setAll('autoCull', true);
    shipBullets.setAll('scale.x', 0.2);
    shipBullets.setAll('scale.y', 0.2);
    shipBullets.setAll('anchor.x', 0.5);
    shipBullets.setAll('anchor.y', 0.5);
    shipBullets.callAll('events.onOutOfBounds.add', 'events.onOutOfBounds', resetBullet, this);

    player2Bullets = game.add.group();
    player2Bullets.createMultiple(50, 'bullet');
    player2Bullets.setAll('autoCull', true);
    player2Bullets.callAll('events.onOutOfBounds.add', 'events.onOutOfBounds', resetBullet, this);

    game.input.gamepad.start();

    playerShip = new PlayerShip(game, shipBullets);
    player2 = new PlayerAssist(game, player2Bullets);

    for (var i=0; i < 20; i++) {
      enemies.add(new Enemy(game));
    }

    game.input.onDown.add(pauseToggle, this);
    game.stage.fullScreenScaleMode = Phaser.StageScaleMode.SHOW_ALL;
    var fKey = game.input.keyboard.addKey(Phaser.Keyboard.F);
    fKey.onDown.add(goFull, this);
    var mKey = game.input.keyboard.addKey(Phaser.Keyboard.M);
    mKey.onDown.add(toggleMute, this);
  }

  function update() {
    player2.update();
    
    var xdiff = (starSpeed * (game.time.elapsed / 1000));
    starsprite.tilePosition.x -= xdiff;

    game.physics.overlap(playerShip, enemies, enemyCollide);
    game.physics.overlap(shipBullets, enemies, bulletCollide);
    game.physics.overlap(player2Bullets, enemies, bulletCollide, bulletBeforeCollide);

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
    game.debug.renderCameraInfo(game.camera, 32, 32);
    game.debug.renderSpriteCoords(playerShip, 32, 100);
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
    player.kill();
  }
  function bulletBeforeCollide(bullet, enemy){
    // player 2 bullets only hit when 0.2 or less
    return bullet.scale.x <= 0.2;
  }
  function bulletCollide(bullet, enemy) {
    bullet.kill();
    enemy.kill();
  }
  //  Called if the bullet goes out of the screen
  function resetBullet (bullet) {
    bullet.kill();
  }
};

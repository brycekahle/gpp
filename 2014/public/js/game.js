'use strict';

// A little lame, but want this for easy debugging
var player1, playerShip, game, enemies, bullets, player2, starsprite, music;

var musicVolume = 0.5;

window.onload = function() {
  game = new Phaser.Game(960, 600, Phaser.CANVAS, '', { preload: preload, create: create, update: update, render: render });

  var starSpeed = 10.0; // 100x / second

  function preload() {
    game.load.image('starfield', 'assets/starfield.png');
    player1 = game.load.spritesheet('player1', 'assets/player_ship.png', 227, 176);
    game.load.spritesheet('enemy', 'assets/enemy.png', 25, 25);
    game.load.spritesheet('reticle', 'assets/reticle.png', 230, 230);
    game.load.spritesheet('bullet', 'assets/bullet.png', 200, 200);
    game.load.audio('music1', ['assets/music1.mp3']);
  }

  function create() {
    starsprite = game.add.tileSprite(0, 0, 4096, 1024, 'starfield');
    music = game.add.audio('music1', musicVolume, true);
    music.play('', 0, 0, true);
    //game.world.setBounds(0, 0, 80000, 600);

    enemies = game.add.group();
    bullets = game.add.group();
    bullets.createMultiple(50, 'bullet');
    bullets.setAll('autoCull', true);
    bullets.callAll('events.onOutOfBounds.add', 'events.onOutOfBounds', resetBullet, this);

    game.input.gamepad.start();

    playerShip = new PlayerShip(game, bullets);
    player2 = new PlayerAssist(game, bullets);

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

    game.physics.collide(playerShip, enemies, enemyCollide);
    game.physics.collide(bullets, enemies, bulletCollide, bulletBeforeCollide);

    bullets.forEach(function (bullet) {
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
    // Normal bullets are 0.2 in size.
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

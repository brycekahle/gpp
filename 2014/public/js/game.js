'use strict';

// A little lame, but want this for easy debugging
var player1, playerShip, game, enemies, bullets, player2, starsprite;

window.onload = function() {
  game = new Phaser.Game(800, 600, Phaser.CANVAS, '', { preload: preload, create: create, update: update, render: render });

  var starSpeed = 10.0; // 100x / second

  function preload() {
    game.load.image('starfield', 'assets/starfield.png');
    player1 = game.load.spritesheet('player1', 'assets/player1.png', 100, 100);
    game.load.spritesheet('enemy', 'assets/enemy.png', 25, 25);
    game.load.spritesheet('reticle', 'assets/reticle.png', 217, 206);
  }

  function create() {
    starsprite = game.add.tileSprite(0, 0, 4096, 1024, 'starfield');
    //game.world.setBounds(0, 0, 80000, 600);

    enemies = game.add.group();
    bullets = game.add.group();
    bullets.createMultiple(50, 'bullet');
    bullets.setAll('autoCull', true);
    bullets.callAll('events.onOutOfBounds.add', 'events.onOutOfBounds', resetBullet, this);

    game.input.gamepad.start();

    playerShip = new PlayerShip(game, bullets);
    player2 = new PlayerAssist(game);

    for (var i=0; i < 20; i++) {
      enemies.add(new Enemy(game));
    }

    game.input.onDown.add(pauseToggle, this);
  }

  function update() {
    playerShip.update();
    player2.update();
    
    var xdiff = (starSpeed * (game.time.elapsed / 1000));
    starsprite.tilePosition.x -= xdiff;

    game.physics.collide(playerShip.sprite, enemies, enemyCollide);
    game.physics.collide(bullets, enemies, bulletCollide);

    bullets.forEach(function (bullet) {
      if (bullet.alive && !bullet.renderable) bullet.kill();
    });
  }

  function render() {
    game.debug.renderCameraInfo(game.camera, 32, 32);
    game.debug.renderSpriteCoords(playerShip.sprite, 32, 100);
  }

  function pauseToggle() {
    game.paused = !game.paused;
  }

  function enemyCollide(player, enemy) {
    player.kill();
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

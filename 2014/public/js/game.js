'use strict';

// A little lame, but want this for easy debugging
var player1, playerShip, game, enemies;

window.onload = function() {
  game = new Phaser.Game(800, 600, Phaser.CANVAS, '', { preload: preload, create: create, update: update, render: render });

  var cameraSpeed = 200.0; // 100x / second

  function preload() {
    player1 = game.load.spritesheet('player1', 'assets/player1.png', 100, 100);
    game.load.spritesheet('enemy', 'assets/enemy.png', 25, 25);
  }

  function create() {
    game.world.setBounds(0, 0, 80000, 600);

    enemies = game.add.group();
    game.input.gamepad.start();

    playerShip = new PlayerShip(game);

    for (var i=0; i < 20; i++) {
      enemies.create(game.rnd.integerInRange(200, 10000), game.rnd.integerInRange(25, 575), 'enemy');
    }

    game.input.onDown.add(pauseToggle, this);
  }

  function update() {
    playerShip.update();
    var xdiff = (cameraSpeed * (game.time.elapsed / 1000));
    game.camera.x += xdiff;

    game.physics.collide(playerShip.sprite, enemies, enemyCollide);
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
};

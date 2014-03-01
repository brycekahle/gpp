'use strict';

// A little lame, but want this for easy debugging
var player1, playerShip, game;

window.onload = function() {
  game = new Phaser.Game(800, 600, Phaser.CANVAS, '', { preload: preload, create: create, update: update, render: render });

  var cameraSpeed = 100.0; // 100x / second

  function preload() {
    player1 = game.load.spritesheet('player1', 'assets/player1.png', 100, 100);
  }

  function create() {
    game.input.gamepad.start();

    playerShip = new PlayerShip(game);

    game.world.setBounds(0, 0, 80000, 600);
    game.input.onDown.add(pauseToggle, this);
  }

  function update() {
    playerShip.update();
    var xdiff = (cameraSpeed * (game.time.elapsed / 1000));
    game.camera.x += xdiff;
  }

  function render() {
    game.debug.renderCameraInfo(game.camera, 32, 32);
    game.debug.renderSpriteCoords(playerShip.sprite, 32, 100);
  }

  function pauseToggle() {
    game.paused = !game.paused;
  }
};

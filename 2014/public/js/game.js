'use strict';

window.onload = function() {
  var game = new Phaser.Game(800, 600, Phaser.CANVAS, '', { preload: preload, create: create, update: update, render: render });
  var player1, ship;

  var cameraSpeed = 100.0; // 100x / second

  function preload() {
    player1 = game.load.spritesheet('player1', 'assets/player1.png', 100, 100);
  }

  function create() {
    ship = game.add.sprite(32, game.world.height / 2, 'player1');
    ship.body.collideWorldBounds = true;

    game.world.setBounds(0, 0, 80000, 600);
    game.input.onDown.add(pauseToggle, this);

    ship.fixedToCamera = true;
  }

  function update() {
    var xdiff = (cameraSpeed * (game.time.elapsed / 1000));
    game.camera.x += xdiff;
  }

  function render() {
    game.debug.renderCameraInfo(game.camera, 32, 32);
    game.debug.renderSpriteCoords(ship, 32, 100);
  }

  function pauseToggle() {
    game.paused = !game.paused;
  }
};
'use strict';

// A little lame, but want this for easy debugging
var player1, playerShip, game;

window.onload = function() {
  game = new Phaser.Game(800, 600, Phaser.AUTO, '', { preload: preload, create: create, update: update, render: render });

  function preload() {
    player1 = game.load.spritesheet('player1', 'assets/player1.png', 100, 100);
  }

  function create() {
    game.input.gamepad.start();

    playerShip = new PlayerShip(game);
  }

  function update() {
    playerShip.update();
  }

  function render() {

  }
};
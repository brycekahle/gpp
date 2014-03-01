'use strict';

window.onload = function() {
  var game = new Phaser.Game(800, 600, Phaser.AUTO, '', { preload: preload, create: create, update: update, render: render });
  var player1, ship;

  function preload() {
    player1 = game.load.spritesheet('player1', 'assets/player1.png', 100, 100);
  }

  function create() {
    ship = game.add.sprite(32, game.world.height / 2, 'player1');
    ship.body.collideWorldBounds = true;
    
  }

  function update() {
    
  }

  function render() {

  }
};
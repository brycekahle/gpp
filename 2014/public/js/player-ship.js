'use strict';
function PlayerShip(game){
	this.ship = game.add.sprite(32, game.world.height / 2, 'player1');
    this.ship.body.collideWorldBounds = true;
}
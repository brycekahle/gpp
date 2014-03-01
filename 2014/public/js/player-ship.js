'use strict';
function PlayerShip(game){
	this.sprite = game.add.sprite(32, game.world.height / 2, 'player1');
    this.sprite.body.collideWorldBounds = true;

    this.update = function(){
    	this.sprite.x++;
    }
}
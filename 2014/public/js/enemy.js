'use strict';

var Enemy = function (game) {
  Phaser.Sprite.call(this, game, game.rnd.integerInRange(200, 10000), game.rnd.integerInRange(25, 575), 'enemy');
  this.body.velocity.x = -50;
  game.add.existing(this);
};

Enemy.prototype = Object.create(Phaser.Sprite.prototype);
Enemy.prototype.constructor = Enemy;

Enemy.prototype.update = function () {

};
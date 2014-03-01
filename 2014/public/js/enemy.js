'use strict';

var Enemy = function (game) {
  Phaser.Sprite.call(this, game, game.rnd.integerInRange(200, 10000), game.rnd.integerInRange(25, 575), 'enemy');
  this.body.velocity.x = -50;
  this.phase = game.rnd.angle();
  game.add.existing(this);
};

Enemy.prototype = Object.create(Phaser.Sprite.prototype);
Enemy.prototype.constructor = Enemy;

Enemy.prototype.update = function () {
  this.body.velocity.y = Math.sin(2 * Math.PI * (this.game.time.now / 1000) + (this.phase / 360) * (2*Math.PI)) * 200;
};
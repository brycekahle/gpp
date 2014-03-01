'use strict';

var Enemy = function (game) {
  Phaser.Sprite.call(this, game, game.rnd.integerInRange(800, 10000), game.rnd.integerInRange(25, 575), 'enemy');
  this.body.velocity.x = -50;
  this.phase = game.rnd.angle();
  game.add.existing(this);
  this.swayScale = game.rnd.integerInRange(50, 400);
};

Enemy.prototype = Object.create(Phaser.Sprite.prototype);
Enemy.prototype.constructor = Enemy;

Enemy.prototype.update = function () {
  this.body.velocity.y = Math.sin(2 * Math.PI * (this.game.time.now / 1000) + (this.phase / 360) * (2*Math.PI)) * this.swayScale;
};

Enemy.prototype.kill = function() {
  Phaser.Sprite.prototype.kill.call(this);
  var deathEmitter = this.game.add.emitter(this.x, this.y, 50);
  deathEmitter.makeParticles('deathbits', 0, 100);
  deathEmitter.minParticleSpeed.setTo(-800, -800);
  deathEmitter.maxParticleSpeed.setTo(800, 800);
  deathEmitter.gravity = 0;
  deathEmitter.start(true, 500, 0, 50);
};
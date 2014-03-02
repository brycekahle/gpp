'use strict';

var Enemy = function (game, spriteName, score) {
  Phaser.Sprite.call(this, game, game.rnd.integerInRange(800, 10000), game.rnd.integerInRange(25, 575), spriteName);
  this.body.velocity.x = -game.rnd.integerInRange(50, 100);
  this.phase = game.rnd.angle();
  game.add.existing(this);
  this.swayScale = game.rnd.integerInRange(50, 400);
  this.score = score;
  this.shield = game.add.sprite(64, 300, 'shield');
  this.shield.angle = 180;
  this.shield.scale.x = 0.25;
  this.shield.scale.y = 0.25;
  if (game.rnd.integerInRange(1,10) > 4) {
  	this.shield.kill();
  };
};

Enemy.prototype = Object.create(Phaser.Sprite.prototype);
Enemy.prototype.constructor = Enemy;

Enemy.prototype.update = function () {
  this.body.velocity.y = Math.sin(2 * Math.PI * (this.game.time.now / 1000) + (this.phase / 360) * (2*Math.PI)) * this.swayScale;
  this.shield.body.y = this.y + 96;
  this.shield.body.x = this.x + 20;
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
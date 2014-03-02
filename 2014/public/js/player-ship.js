'use strict';


function PlayerShip(game, bullets){
  Phaser.Sprite.call(this, game, 64, 300, 'player1');
  game.add.existing(this);
  
  this.scale.setTo(0.5, 0.5);
  this.anchor.setTo(0.5, 0.5);
  this.body.collideWorldBounds = true;
  this.fixedToCamera = true;
  this.cursors = game.input.keyboard.createCursorKeys();
  this.maxTiltAngle = 20;
  this.bulletTime = 0;
  this.moveSpeed = 10;
  this.pad1 = game.input.gamepad.pad1;
  this.bullets = bullets;
  this.shield = game.add.sprite(64, 300, 'shield');
  this.shield.scale.x = 0.25;
  this.shield.scale.y = 0.25;
}

PlayerShip.prototype = Object.create(Phaser.Sprite.prototype);
PlayerShip.prototype.constructor = PlayerShip;

PlayerShip.prototype.update = function(){
  this.move();
  this.shoot();
  this.shield.body.y = this.y - 64;
};

PlayerShip.prototype.kill = function () {
  Phaser.Sprite.prototype.kill.call(this);
  var deathEmitter = this.game.add.emitter(this.x, this.y, 100);
  deathEmitter.makeParticles('deathbits', 0, 100);
  deathEmitter.minParticleSpeed.setTo(-400, -400);
  deathEmitter.maxParticleSpeed.setTo(400, 400);
  deathEmitter.gravity = 0;
  deathEmitter.start(true, 1000, 0, 100);
};

PlayerShip.prototype.move = function() {
  var yAxis = this.pad1.connected ? this.pad1.axis(Phaser.Gamepad.XBOX360_STICK_LEFT_Y) : 0;

  if (this.cursors.up.isDown || yAxis < -0.3) {
    yAxis = yAxis || -1.0;
    var ydiff = yAxis * this.moveSpeed;
    this.cameraOffset.y = this.game.math.clamp(this.cameraOffset.y + ydiff, this.height/2, 
      this.game.world.height - this.height/2);

    this.frame = 1;
  }
  else if (this.cursors.down.isDown || yAxis > 0.3) {
    yAxis = yAxis || 1.0;
    var ydiff = yAxis * this.moveSpeed;
    this.cameraOffset.y = this.game.math.clamp(this.cameraOffset.y + ydiff, this.height/2, this.game.world.height - this.height/2);

    this.frame = 2;
  }
  else{
    this.frame = 0;
  }
};

PlayerShip.prototype.shoot = function(){
  if (this.game.input.keyboard.isDown(Phaser.Keyboard.SPACEBAR) ||
   (this.pad1.connected && this.pad1.buttonValue(Phaser.Gamepad.XBOX360_A) > 0)){
    if (this.game.time.now > this.bulletTime)
    {
      var bullet = this.bullets.getFirstExists(false);

      if (bullet)
      {
        bullet.reset(this.x, this.y);
        bullet.body.velocity.x = 500;
        this.bulletTime = this.game.time.now + 250;
      }
    }
  }
};

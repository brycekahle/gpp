'use strict';

function PlayerAssist(game, bullets) {
  this.sprite = game.add.sprite(game.camera.width / 2, game.world.height / 2, 'reticle');
  this.sprite.anchor.setTo(0.5, 0.5);
  this.sprite.fixedToCamera = true;
  this.sprite.scale.setTo(0.5, 0.5);
  var bulletTime = 0;

  var pad = game.input.gamepad.pad2;
  this.moveSpeed = 10;

  this.lasers = [
    game.add.audio('laser1'),
    game.add.audio('laser2'),
    game.add.audio('laser3'),
    game.add.audio('laser4'),
    game.add.audio('laser5'),
  ];

  this.update = function() {
    if (!pad.connected) return;

    this.move();
    this.shoot();
  };

  this.move = function(){
    var yAxis = pad.axis(Phaser.Gamepad.XBOX360_STICK_LEFT_Y);
    var xAxis = pad.axis(Phaser.Gamepad.XBOX360_STICK_LEFT_X);
    if (Math.abs(yAxis) > 0.1) {
      this.sprite.cameraOffset.y = game.math.clamp(this.sprite.cameraOffset.y + yAxis * this.moveSpeed, 0, game.world.height);
    }
    if (Math.abs(xAxis) > 0.1) {
      this.sprite.cameraOffset.x = game.math.clamp(this.sprite.cameraOffset.x + xAxis * this.moveSpeed, 0, game.world.width);
    }
  };

  this.shoot = function(){
    if (pad.buttonValue(Phaser.Gamepad.XBOX360_A) > 0){
      if (game.time.now > bulletTime)
      {
        var bullet = bullets.getFirstExists(false);

        if (bullet) {
          this.lasers[game.rnd.integerInRange(0, 4)].play();

          bullet.reset(this.sprite.x, this.sprite.y);
          bullet.scale.x = 0.49;
          bullet.scale.y = 0.49;
          bullet.anchor.setTo(0.5, 0.5);
          bullet.alpha = 0.2;
          bullet.update = function(){
            bullet.scale.x -= 0.01;
            bullet.scale.y -= 0.01;
            bullet.alpha += 0.01;
            bullet.angle++;
          };
          bulletTime = game.time.now + 500;
        }
      }
    }
  };
}
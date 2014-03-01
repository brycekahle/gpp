'use strict';

function PlayerAssist(game) {
  this.sprite = game.add.sprite(game.camera.width / 2, game.world.height / 2, 'reticle');
  this.sprite.anchor.setTo(0.5, 0.5);
  this.sprite.fixedToCamera = true;

  var pad = game.input.gamepad.pad1;
  this.moveSpeed = 10;

  this.update = function() {
    if (!pad.connected) return;

    var yAxis = pad.axis(Phaser.Gamepad.XBOX360_STICK_LEFT_Y);
    var xAxis = pad.axis(Phaser.Gamepad.XBOX360_STICK_LEFT_X);
    if (Math.abs(yAxis) > 0.3) {
      this.sprite.cameraOffset.y += yAxis * this.moveSpeed;
    }
    if (Math.abs(xAxis) > 0.3) {
      this.sprite.cameraOffset.x += xAxis * this.moveSpeed;
    }
  };
}
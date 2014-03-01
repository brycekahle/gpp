'use strict';
function PlayerShip(game){
    this.sprite = game.add.sprite(64, game.world.height / 2, 'player1');
    this.sprite.anchor.setTo(0.5, 0.5);
    this.sprite.body.collideWorldBounds = true;
    this.sprite.fixedToCamera = true;
    var pad1 = game.input.gamepad.pad1;
    var cursors = game.input.keyboard.createCursorKeys();
    var maxTiltAngle = 20;

    this.update = function(){
      if (cursors.up.isDown || (pad1.connected && pad1.axis(Phaser.Gamepad.XBOX360_STICK_LEFT_Y) < -0.3)) {
        //  Move up
        this.sprite.cameraOffset.y -= 4;
        if (this.sprite.angle > -maxTiltAngle){
          this.sprite.angle--;
        }
      }
      else if (cursors.down.isDown || (pad1.connected && pad1.axis(Phaser.Gamepad.XBOX360_STICK_LEFT_Y) > 0.3)) {
        //  Move down
        this.sprite.cameraOffset.y += 4;
        if (this.sprite.angle < maxTiltAngle){
          this.sprite.angle++;
        }
      }
      else{
        if (this.sprite.angle > 0){
          this.sprite.angle -= 2;
        } else if (this.sprite.angle < 0){
          this.sprite.angle += 2;
        }
      }
    }
}
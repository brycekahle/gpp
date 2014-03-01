'use strict';
function PlayerShip(game, bullets){
    this.sprite = game.add.sprite(64, game.world.height / 2, 'player1');
    this.sprite.anchor.setTo(0.5, 0.5);
    this.sprite.body.collideWorldBounds = true;
    this.sprite.fixedToCamera = true;
    var pad1 = game.input.gamepad.pad1;
    var cursors = game.input.keyboard.createCursorKeys();
    var maxTiltAngle = 20;
    var bulletTime = 0;
    this.moveSpeed = 10;

    this.update = function(){
      this.move();
      this.shoot();
    };

    this.move = function() {
      var yAxis = pad1.connected ? pad1.axis(Phaser.Gamepad.XBOX360_STICK_LEFT_Y) : 0;

      if (cursors.up.isDown || yAxis < -0.3) {
        yAxis = yAxis || -1.0;
        var ydiff = yAxis * this.moveSpeed;
        this.sprite.cameraOffset.y = game.math.clamp(this.sprite.cameraOffset.y + ydiff, this.sprite.height/2, game.world.height - this.sprite.height/2);

        if (this.sprite.angle > -maxTiltAngle){
          this.sprite.angle--;
        }
      }
      else if (cursors.down.isDown || yAxis > 0.3) {
        yAxis = yAxis || 1.0;
        var ydiff = yAxis * this.moveSpeed;
        this.sprite.cameraOffset.y = game.math.clamp(this.sprite.cameraOffset.y + ydiff, this.sprite.height/2, game.world.height - this.sprite.height/2);

        if (this.sprite.angle < maxTiltAngle){
          this.sprite.angle++;
        }
      }
      else{
        if (this.sprite.angle > 1){
          this.sprite.angle -= 2;
        } else if (this.sprite.angle < -1){
          this.sprite.angle += 2;
        } else{
          this.sprite.angle = 0;
        }
      }
    };

    this.shoot = function(){
      if (game.input.keyboard.isDown(Phaser.Keyboard.SPACEBAR) ||
       (pad1.connected && pad1.buttonValue(Phaser.Gamepad.XBOX360_RIGHT_TRIGGER) > 0)){
        if (game.time.now > bulletTime)
        {
          var bullet = bullets.getFirstExists(false);

          if (bullet)
          {
            bullet.reset(this.sprite.x + 6, this.sprite.y - 8);
            bullet.body.velocity.x = 500;
            bullet.scale.x = 1;
            bullet.scale.y = 1;
            bullet.update = function(){};
            bulletTime = game.time.now + 250;
          }
        }
      }
    };
}
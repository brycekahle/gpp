'use strict';
function PlayerShip(game){
    this.sprite = game.add.sprite(32, game.world.height / 2, 'player1');
    this.sprite.body.collideWorldBounds = true;
    var pad1 = game.input.gamepad.pad1;
    var cursors = game.input.keyboard.createCursorKeys();


    this.update = function(){
        if (cursors.up.isDown || (pad1.connected && pad1.axis(Phaser.Gamepad.XBOX360_STICK_LEFT_Y) < -0.3)) {
            //  Move up
            this.sprite.y -= 4;
        }
        else if (cursors.down.isDown || (pad1.connected && pad1.axis(Phaser.Gamepad.XBOX360_STICK_LEFT_Y) > 0.3)) {
            //  Move down
            this.sprite.y += 4;
        }
    }
}
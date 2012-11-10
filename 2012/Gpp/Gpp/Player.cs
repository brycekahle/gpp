using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gpp
{
    class Player : MovableObject
    {
        private PlayerIndex _controlIndex;
        private bool _isCharging;
        private float _chargeAmount;

        private Vector2 _aimingVector;

        private const float ChargeRate = 1.0f; // m/s
        private const float AimingRotateRate = 1.5f; // radians / s
        private const float MaxCharge = 5.0f;
        private const float MovementSpeed = 1.0f; // m/s

        public Player(SupermassiveGame game, PlayerIndex controlIndex)
            : base(game)
        {
            _controlIndex = controlIndex;
        }

        private void ReadGamepad(TimeSpan elapsedTime)
        {
            var state = GamePad.GetState(_controlIndex);
            ReadTrigger(state, elapsedTime);
            ReadSticks(state, elapsedTime);
        }

        private void ReadSticks(GamePadState state, TimeSpan elapsedTime)
        {
            var leftStick = state.ThumbSticks.Left.X;
            var leftMovementVector = Vector2.TransformNormal(Heading, Matrix.CreateRotationZ((float)Math.PI / 2.0f));
            var rightMovementVector = Vector2.TransformNormal(Heading, Matrix.CreateRotationZ((float)-Math.PI / 2.0f));

            if (_isCharging || leftStick == 0)
            {
                // cancel out left and right movement velocities
                Velocity -= Vector2.Dot(Velocity, leftMovementVector) * leftMovementVector;
                Velocity -= Vector2.Dot(Velocity, rightMovementVector) * rightMovementVector;
            }
            else
            {
                var movementVector = leftStick > 0 ? rightMovementVector : leftMovementVector;
                Velocity += movementVector * (MovementSpeed * (float)elapsedTime.TotalSeconds);
            }

            var rightStickX = state.ThumbSticks.Right.X;
            // left is negative, but that means positive radians
            _aimingVector = Vector2.TransformNormal(_aimingVector, Matrix.CreateRotationZ(-rightStickX * (AimingRotateRate * (float)elapsedTime.TotalSeconds)));
        }

        private void ReadTrigger(GamePadState state, TimeSpan elapsedTime)
        {
            var rightTrigger = state.Triggers.Right;
            if (rightTrigger == 0)
            {
                if (_isCharging)
                {
                    Fire();
                }
            }
            else
            {
                _isCharging = true;
                _chargeAmount = Math.Min(_chargeAmount + (ChargeRate * (float)elapsedTime.TotalSeconds), MaxCharge);
            }
        }

        private void Fire()
        {
            var proj = new Projectile(Game, this, _aimingVector, _chargeAmount);
            Game.GameObjects.Add(proj);

            _isCharging = false;
            _chargeAmount = 1;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            ReadGamepad(elapsedTime);
            base.Update(elapsedTime);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}

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

        private const float ChargeRate = 0.2f; // % /s
        private const float AimingRotateRate = 1.5f; // radians / s

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
            if (!_isCharging) // can't move and charge at same time
            {
                var leftStick = state.ThumbSticks.Left;
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
                _chargeAmount = Math.Min(_chargeAmount + (ChargeRate * (float)elapsedTime.TotalSeconds), 1.0f);
            }
        }

        private void Fire()
        {
            _isCharging = false;
            _chargeAmount = 0;
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

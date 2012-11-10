using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private bool _isJumping;

        private Vector2 _aimingVector;
        private Vector2 _planetCenter;
        private float _planetRadius;

        private const float ChargeRate = 100.0f; // m/s
        private const float AimingRotateRate = 3.0f; // radians / s
        private const float MaxCharge = 500.0f;
        private const float MovementSpeed = 100.0f; // m/s
        private const float MovementRotateRate = 3.0f; // radian/s
        private const float JumpAcceleration = 5.0f; //m/s/s
        private const float ReticleDistance = 70.0f;

        public float Health { get; private set; }

        public Player(SupermassiveGame game, PlayerIndex controlIndex, Texture2D texture, float planetRadius, Vector2 heading, Vector2 planetCenter)
            : base(game, texture, planetCenter + heading * planetRadius, 0.3f, 5000000)
        {
            Heading = heading;
            _controlIndex = controlIndex;
            _aimingVector = Heading;
            _planetCenter = planetCenter;
            _planetRadius = planetRadius;
            Health = 100f;
        }

        private void ReadGamepad(TimeSpan elapsedTime)
        {
            var state = GamePad.GetState(_controlIndex);
            ReadTrigger(state, elapsedTime);
            ReadSticks(state, elapsedTime);
            ReadButtons(state, elapsedTime);
        }

        private void ReadButtons(GamePadState state, TimeSpan elapsedTime)
        {
            // handle jumping
            var jump = state.Buttons.A == ButtonState.Pressed;
            if (jump)
            {
                if (!_isCharging && !_isJumping && 
                    Vector2.Dot(Velocity, Heading) <= 0f) // not moving upwards
                {
                    Acceleration += Heading * (JumpAcceleration * (float)elapsedTime.TotalSeconds);
                    _isJumping = true;
                }
            }
            else
            {
                _isJumping = false;
            }
        }

        private void ReadSticks(GamePadState state, TimeSpan elapsedTime)
        {
            var leftStickX = state.ThumbSticks.Left.X;
            var leftMovementVector = Vector2.TransformNormal(Heading, Matrix.CreateRotationZ((float)Math.PI / 2.0f));
            var rightMovementVector = Vector2.TransformNormal(Heading, Matrix.CreateRotationZ((float)-Math.PI / 2.0f));

            float movementRotate = 0.0f;
            if (_isCharging || leftStickX == 0)
            {
                // cancel out left and right movement velocities
                //Velocity -= Vector2.Dot(Velocity, leftMovementVector) * leftMovementVector;
                //Velocity -= Vector2.Dot(Velocity, rightMovementVector) * rightMovementVector;
            }
            else
            {
                movementRotate = leftStickX * (MovementRotateRate * (float)elapsedTime.TotalSeconds);
                var newHeading = Vector2.TransformNormal(Heading, Matrix.CreateRotationZ(movementRotate));
                
                // if crossing vertical boundary
                if (Math.Abs(newHeading.X) < 0.3)
                {
                    newHeading = Heading;
                }
                Heading = newHeading;

                Position = _planetCenter + _planetRadius * Heading;
                //var movementVector = leftStick > 0 ? rightMovementVector : leftMovementVector;
                //Velocity += movementVector * (MovementSpeed * (float)elapsedTime.TotalSeconds);
            }

            var rightStickX = state.ThumbSticks.Right.X;
            // left is negative, but that means positive radians
            var aimRotate = rightStickX * (AimingRotateRate * (float)elapsedTime.TotalSeconds);
            _aimingVector = Vector2.TransformNormal(_aimingVector, Matrix.CreateRotationZ(aimRotate + movementRotate));
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
            _chargeAmount = 0;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            ReadGamepad(elapsedTime);
            base.Update(elapsedTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            var reticlePosition = Position + (_aimingVector * ReticleDistance);
            var reticleTexture = Game.Content.Load<Texture2D>("aiming-reticle");

            var reticleAngle = -(float)AngleBetweenVectors2(new Vector2(0, 1), _aimingVector);

            var reticleColor = new Color((_chargeAmount / MaxCharge), 0, 0);
            batch.Draw(reticleTexture, reticlePosition, null, reticleColor, reticleAngle,
                       new Vector2((float)reticleTexture.Width / 2, (float)reticleTexture.Height / 2),
                       1.0f, SpriteEffects.None, 0);
            base.Draw(batch);
        }

        protected override void UpdateAcceleration(TimeSpan elapsedTime)
        {
        }

        public void TakeDamage(Projectile projectile)
        {
            Health -= projectile.Damage;
        }
    }
}

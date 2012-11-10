using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Gpp
{
    public class MovableObject : GameObject
    {
        /// <summary>
        /// m/s
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// m/s/s
        /// </summary>
        public Vector2 Acceleration { get; set; }

        public MovableObject(SupermassiveGame game)
            : base(game)
        {
        }

        public override void Update(TimeSpan elapsedTime)
        {
            UpdateAcceleration(elapsedTime);
            UpdateVelocity(elapsedTime);
            UpdatePosition(elapsedTime);
            base.Update(elapsedTime);
        }

        protected void UpdateAcceleration(TimeSpan elapsedTime)
        {
            foreach (var gameObject in Game.GameObjects.Where(o => o != this))
            {
                // kg*m/s/s = ((m*m*m)/kg/s/s)*(kg*kg)/(m*m)
                var force = GravitationConstant*(gameObject.Mass*Mass)/(Vector2.DistanceSquared(gameObject.Position, Position));
            }
        }

        protected void UpdateVelocity(TimeSpan elapsedTime)
        {
            // m/s = (m/s/s) * s
            Velocity += Acceleration * (float)elapsedTime.TotalSeconds;
        }

        protected void UpdatePosition(TimeSpan elapsedTime)
        {
            // m = (m/s) * s
            Position += Velocity * (float)elapsedTime.TotalSeconds;
        }
    }
}
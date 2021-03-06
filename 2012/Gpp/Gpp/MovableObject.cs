using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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

        public MovableObject(SupermassiveGame game, Texture2D texture, Vector2 position, float scale, float mass)
            : base(game, texture, position, scale, mass)
        {
        }

        public override void Update(TimeSpan elapsedTime)
        {
            UpdateAcceleration(elapsedTime);
            UpdateVelocity(elapsedTime);
            UpdatePosition(elapsedTime);
            BoundingSphere = GetBoundingSphere();
            base.Update(elapsedTime);
        }

        protected virtual void UpdateAcceleration(TimeSpan elapsedTime)
        {
            var resultDirectionalAcceleration = Vector2.Zero;
            foreach (var gameObject in Game.GameObjects.Where(o => o != this))
            {
                // m/s/s = ((m*m*m)/kg/s/s)*(kg)/(m*m)
                var acceleration = GravitationConstant*(gameObject.Mass)/(Vector2.DistanceSquared(gameObject.Position, Position));
                //var acceleration = GravitationConstant * (gameObject.Mass) / ((float)Math.Atan(Vector2.Distance(gameObject.Position, Position)));
                var direction = (gameObject.Position - Position);
                direction.Normalize();

                resultDirectionalAcceleration += acceleration*direction;
            }
            Acceleration += resultDirectionalAcceleration;
        }

        protected void UpdateVelocity(TimeSpan elapsedTime)
        {
            // m/s = (m/s/s) * s
            Velocity += Acceleration * (float)elapsedTime.TotalSeconds;
        }

        protected void UpdatePosition(TimeSpan elapsedTime)
        {
            if (Game.GameObjects.ToList().Any(o => !(o is MovableObject) && o.BoundingSphere.Intersects(BoundingSphere)))
            {
                //return;
            }
            // m = (m/s) * s
            Position += Velocity * (float)elapsedTime.TotalSeconds;
            if (Position.X < 0)
            {
                Position.X = Game._width;
            }
            if (Position.X > Game._width) Position.X = 0;
            if (Position.Y < 0) Position.Y = Game._height;
            if (Position.Y > Game._height) Position.Y = 0;
        }
    }
}
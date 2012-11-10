﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Gpp
{
    public class GameObject
    {
        /// <summary>
        /// (m*m*m)/kg/s/s
        /// </summary>
        public const float GravitationConstant = 6.673E-11f;

        public SupermassiveGame Game { get; private set; }

        /// <summary>
        /// Meters from origin
        /// </summary>
        public Vector2 Position { get; protected set; }

        public Vector2 Heading { get; protected set; }

        /// <summary>
        /// KG
        /// </summary>
        public float Mass { get; protected set; }

        protected readonly float _scale;

        private readonly Texture2D _texture;

        public BoundingSphere BoundingSphere { get; protected set; }

        public GameObject(SupermassiveGame game, Texture2D texture, Vector2 position, float scale, float mass)
        {
            Game = game;
            _texture = texture;
            Position = position;
            _scale = scale;
            Mass = mass;
            BoundingSphere = GetBoundingSphere();
        }

        public virtual void Update(TimeSpan elapsedTime)
        {
            
        }

        public virtual void Draw(SpriteBatch batch)
        {
            var headingAngle = (float)AngleBetweenVectors2(new Vector2(0, -1), Heading);
            if (float.IsNaN(headingAngle)) headingAngle = 0;

            batch.Draw(_texture, Position, null, Color.White, headingAngle,
                       new Vector2((float) _texture.Width/2, (float) _texture.Height/2),
                       _scale, SpriteEffects.None, 0);
        }


        public double AngleBetweenVectors2(Vector2 v1, Vector2 v2)
        {
            return ((v1.X - v2.X) > 0 ? -1 : 1) * (float)Math.Acos((double)Vector2.Dot(Vector2.Normalize(v1), Vector2.Normalize(v2)));
        }

        protected BoundingSphere GetBoundingSphere()
        {
            return new BoundingSphere(new Vector3(Position.X, Position.Y, 0), _scale*_texture.Width/2f);
        }
    }
}

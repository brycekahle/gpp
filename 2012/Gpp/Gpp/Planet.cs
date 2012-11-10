using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gpp
{
    class Planet : GameObject
    {
        private float _boundingPercentage;

        public Planet(SupermassiveGame game, Texture2D texture, Vector2 position, float scale, float mass, float boundingPercentage)
            : base(game, texture, position, scale, mass)
        {
            _boundingPercentage = boundingPercentage;
            BoundingSphere = GetBoundingSphere();
        }

        protected override BoundingSphere GetBoundingSphere()
        {
            return new BoundingSphere(new Vector3(Position.X, Position.Y, 0), (_scale * _texture.Height * _boundingPercentage) / 2f);
        }

        public void TakeDamage(Projectile projectile)
        {
            Mass *= 1.1f;
            _scale *= 1.1f;
            BoundingSphere = GetBoundingSphere();
        }
    }
}

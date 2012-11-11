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
        private float _originalScale;
        private const float PlanetShrinkRate = -0.01f; // 

        public Planet(SupermassiveGame game, Texture2D texture, Vector2 position, float scale, float mass, float boundingPercentage)
            : base(game, texture, position, scale, mass)
        {
            _boundingPercentage = boundingPercentage;
            BoundingSphere = GetBoundingSphere();
            _originalScale = scale;
        }

        protected override BoundingSphere GetBoundingSphere()
        {
            return new BoundingSphere(new Vector3(Position.X, Position.Y, 0), (_scale * _texture.Height * _boundingPercentage) / 2f);
        }

        public void TakeDamage(Projectile projectile)
        {
            UpdateScale(0.1f);
        }

        private void UpdateScale(float change)
        {
            var newScale = _scale * (1.0f + change);
            if (newScale < _originalScale) newScale = _originalScale;

            if (newScale < _originalScale * 2.0f)
            {
                _scale = newScale;
                Mass *= (1.15f + change);
                BoundingSphere = GetBoundingSphere();
            }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            //UpdateScale(PlanetShrinkRate * (float)elapsedTime.TotalSeconds);
            base.Update(elapsedTime);
        }
    }
}

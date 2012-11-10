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
        public Planet(SupermassiveGame game, Texture2D texture, Vector2 position, float scale, float mass, BoundingSphere bounds)
            : base(game, texture, position, scale, mass)
        {
            BoundingSphere = bounds;
        }
    }
}

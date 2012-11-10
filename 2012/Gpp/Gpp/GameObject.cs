using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Gpp
{
    public class GameObject
    {
        /// <summary>
        /// (m*m*m)/kg/s/s
        /// </summary>
        public const float GravitationConstant = 6.673E-11f;

        public Game1 Game1 { get; private set; }

        /// <summary>
        /// Meters from origin
        /// </summary>
        public Vector2 Position { get; protected set; }

        /// <summary>
        /// KG
        /// </summary>
        public float Mass { get; protected set; }

        public GameObject(Game1 game1)
        {
            Game1 = game1;
        }

        public virtual void Update(TimeSpan elapsedTime)
        {
            
        }

        public virtual void Draw()
        {
            
        }
    }
}

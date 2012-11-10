using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Gpp
{
    class Projectile : MovableObject
    {
        private Player _firingPlayer;

        public float BaseDamage { get; private set; }
        public float Damage { get { return BaseDamage; } }

        public Projectile(SupermassiveGame game, Player firingPlayer, Vector2 initialHeading, float firingPower)
            : base(game, firingPlayer.ProjectileTexture, firingPlayer.Position + initialHeading * 100, 0.1f, 100)
        {
            _firingPlayer = firingPlayer;
            Heading = initialHeading;
            Velocity = Heading * firingPower;
            BaseDamage = 10f;
        }
    }
}

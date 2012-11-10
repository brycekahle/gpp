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

        public Projectile(SupermassiveGame game, Player firingPlayer, Vector2 initialHeading, float firingPower)
            : base(game, game.ProjectileTexture, firingPlayer.Position, 1, 100)
        {
            _firingPlayer = firingPlayer;
            Heading = initialHeading;
            Velocity = Heading * firingPower;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
        }

        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}

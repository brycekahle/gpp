using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gpp
{
    class Projectile : MovableObject
    {
        private Player _firingPlayer;

        public Projectile(SupermassiveGame game, Player firingPlayer, Vector2 initialHeading, float firingPower)
            : base(game)
        {
            _firingPlayer = firingPlayer;
            Heading = initialHeading;
            Velocity = Heading * firingPower;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
        }

        public override void Draw()
        {
            base.Draw();
        }
    }
}

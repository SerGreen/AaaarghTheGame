using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace MultiplayerLib.Projectiles
{
    class ExplosionDamageable : Projectile
    {
        public ExplosionDamageable(float x, float y, int team, int ownerID)
            : base(x, y, 1, 10, team, ownerID, 5)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Explosion, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/explosion.png"), 96, 1);
            sprite.y -= sprite.Height + 4;
            sprite.x -= sprite.Width / 2;
            sprite.setCollisionBox(23, 28, 50, 68);
        }
    }
}

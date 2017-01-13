using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace MultiplayerLib.Particles
{
    public class FireballExplosion : LastFrameDieParticle
    {
        public FireballExplosion(float x, float y, int face)
            : base(0, 0, face, 300, 2)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Fireball_Explosion, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/fireball_explosion.png"), 40, 1);
            sprite.y -= sprite.Height / 2;
        }
    }
}

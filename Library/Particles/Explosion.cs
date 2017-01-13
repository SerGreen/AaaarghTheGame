using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace MultiplayerLib.Particles
{
    public class Explosion : LastFrameDieParticle
    {
        public Explosion(float x, float y, int face)
            : base(0, 0, face, 300, 1)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Explosion, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/explosion.png"), 96, 1);
            sprite.y -= sprite.Height + 4;
            sprite.x -= sprite.Width / 2;
        }
    }
}

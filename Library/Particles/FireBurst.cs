using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Drawing;

namespace MultiplayerLib.Particles
{
    public class FireBurst : LastFrameDieParticle
    {
        public FireBurst(float x, float y)
            : base(0, 0, 1, 300, 1)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.FireBurst, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/fire.png"), 30, 1);
            sprite.y -= sprite.Height;
            sprite.x -= sprite.Width / 2;
        }
    }
}

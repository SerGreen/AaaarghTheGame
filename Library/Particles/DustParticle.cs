using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections;

namespace MultiplayerLib.Particles
{
    public class DustParticle : LastFrameDieParticle
    {
        public DustParticle(float x, float y, float vx, float vy, int face, int lifetime)
            : base(vx, vy, face, lifetime)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Dust, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/dust.png"), 8, 1);
        }
    }
}

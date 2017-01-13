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
    class BloodArrowParticle : LastFrameDieParticle
    {
        public BloodArrowParticle(float x, float y, float vx, float vy, int face, int lifetime)
            : base(vx, vy, face, lifetime, 2)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Blood_Arrow, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/blood_arrow.png"), 32, 2);
        }
    }
}

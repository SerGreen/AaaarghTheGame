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
    class BloodSlashParticle : LastFrameDieParticle
    {
        public BloodSlashParticle(float x, float y, float vx, float vy, int face, int lifetime)
            : base(vx, vy, face, lifetime, 2)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Blood_Slash, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/blood_slash.png"), 32, 2);
        }
    }
}

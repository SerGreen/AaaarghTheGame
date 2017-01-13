using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.IO;

namespace MultiplayerLib.Particles
{
    class ScoreUpParticle : Particle
    {
        public ScoreUpParticle(int value, float x, float y, int lifetime)
            : base(0, -2, 1, lifetime)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            SpriteIndex index = SpriteIndex.Popup_minus1;
            string fileName = null;

            if (value < 0)
            {
                index = SpriteIndex.Popup_minus1;
                fileName = "-1";
            }
            else if (value > 1)
            {
                index = SpriteIndex.Popup_plus2;
                fileName = "+2";
            }
            else
            {
                index = SpriteIndex.Popup_plus1;
                fileName = "+1";
            }

            sprite = new Sprite(x, y, index, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/" + fileName + ".png"), 22, 1);
        }
    }
}

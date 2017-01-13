using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace MultiplayerLib.Particles
{
    class TombstoneCrush : LastFrameDieParticle
    {
        public TombstoneCrush(float x, float y)
            : base(0, 0, 1, 300, 1)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Tombstone_crush, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/tombstone_crush.png"), 26, 1);
            sprite.setCollisionBox(8, 28, 10, 4);
        }
    }
}

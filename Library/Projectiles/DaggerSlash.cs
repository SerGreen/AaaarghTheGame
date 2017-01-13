using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections;

namespace MultiplayerLib
{
    public class DaggerSlash : Projectile
    {
        public DaggerSlash(float x, float y, int face, int damage, int team, int ownerID, int lifetime)
            : base(x, y, face, damage, team, ownerID, lifetime)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Slash_Dagger, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/slash_dagger.png"), 15, 2);
        }
    }
}

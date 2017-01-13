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
    public class SwordSlash : Projectile
    {
        public SwordSlash(float x, float y, int face, int damage, int team, int ownerID, int lifetime)
            : base(x, y, face, damage, team, ownerID, lifetime)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Slash, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/slash.png"), 22, 2);
            sprite.setCollisionBox(0, 2, 22, 17);
        }
    }
}

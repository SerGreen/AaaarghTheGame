using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Collections;

namespace MultiplayerLib
{
    class Bolt : Arrow
    {
        public Bolt(float x, float y, int face, int damage, int team, int ownerID, int lifetime, float vx, float vy, float gravity)
            : base(x, y, face, damage, team, ownerID, lifetime, vx,vy,gravity)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Bolt, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/bolt.png"), 14, 2);
            sprite.setCollisionBox(0, 1, 14, 1);
        }
    }
}

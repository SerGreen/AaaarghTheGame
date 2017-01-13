using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Collections;

namespace MultiplayerLib.Projectiles
{
    public class PoisonedArrow : Arrow
    {
        public PoisonedArrow(float x, float y, int face, int damage, int team, int ownerID, int lifetime, float vx, float vy, float gravity)
            : base(x, y, face, damage, team, ownerID, lifetime, vx, vy, gravity)
        { }
    }
}

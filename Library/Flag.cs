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
    public class Flag : GravityInfluencedObject
    {
        public int team;
        public int returnTimeout;
        public int maxReturnTimeout;
        public bool timeToReturn { get; set; }
        public bool captured { get; set; }

        public Flag(float x, float y, int team)
            : base(0.5f)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Flag, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/flag.png"), 12, 2);
            this.team = team;
            returnTimeout = -1;
            maxReturnTimeout = 1000;
            timeToReturn = false;
            captured = false;
            sprite.setFrame(team);
        }

        public Sprite GetSprite
        { get { return sprite; } }

        public void tick(ArrayList solidObjects, ref ArrayList particles)
        {
            if (!captured)
            {
                checkGroundUnderneath(solidObjects, ref particles);
                move(solidObjects);
            }
            doCountersStuff();
        }

        private void doCountersStuff()
        {
            if (returnTimeout > 0)
                returnTimeout--;

            if (returnTimeout == 0)
                timeToReturn = true;
        }

        public void moveTo(float x, float y)
        {
            sprite.x = x;
            sprite.y = y;
        }
    }
}

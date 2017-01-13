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
    public class Tombstone : GravityInfluencedObject
    {
        public int disappearTimeout { get; set; }
        public bool TimeToDisappear { get; set; }

        public Tombstone(float x, float y, int team, int disappearTime) : base(0.5f)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Tombstone, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/tombstone.png"), 26, 1);
            sprite.setCollisionBox(8, 28, 10, 4);
            sprite.setFrame(team);
            disappearTimeout = disappearTime;
            TimeToDisappear = false;
        }

        public void tick(ArrayList solidObjects, ref ArrayList particles)
        {
            checkGroundUnderneath(solidObjects, ref particles);
            move(solidObjects);
            doCountersStuff();
        }

        private void doCountersStuff()
        {
            if (disappearTimeout > 0)
                disappearTimeout--;

            if (disappearTimeout == 0)
                TimeToDisappear = true;
        }
        
        protected override void doStuffCaseGroundUnderneath(ref System.Collections.ArrayList particles)
        {
            if (vx != 0)
            {
                vx *= 0.75f;
                if (Math.Abs(vx) < 0.01f)
                    vx = 0;
            }
        }
    }
}

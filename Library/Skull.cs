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
    public class Skull : GravityInfluencedObject
    {
        public int team;
        public bool captured { get; set; }
        public int disappearTimeout { get; set; }
        public bool TimeToDisappear { get; set; }
        public int face { get; set; }

        public Skull(float x, float y, float vx, float vy, int team)
            : base(0.5f)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Skull, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/skulls.png"), 20, 1);
            sprite.setFrame(team);
            this.vx = vx;
            this.vy = vy;
            this.team = team;
            captured = false;
            disappearTimeout = 3000;
            TimeToDisappear = false;
            face = 1;
        }

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

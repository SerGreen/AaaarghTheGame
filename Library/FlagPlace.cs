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
    public class FlagPlace : MarshalByRefObject
    {
        private Sprite sprite;
        public int team;
        public Flag flag;
        public bool FlagOnPlace { get; set; }

        public FlagPlace(float x, float y, int team)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.FlagPlace, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/flag_place.png"), 16, 2);
            this.team = team;
            flag = new Flag(x + 4, y - 56, team);
            FlagOnPlace = true;
        }

        public void tick(ArrayList solidObjects, ref ArrayList particles)
        {
            flag.tick(solidObjects, ref particles);

            if (!FlagOnPlace && flag.timeToReturn)
            {
                flag.moveTo(sprite.x + 4, sprite.y - 56);
                flag.returnTimeout = -1;
                flag.timeToReturn = false;
                FlagOnPlace = true;
                flag.captured = false;
            }
        }

        public Sprite GetSprite
        { get { return sprite; } }

        public Flag Flag
        { get { return flag; } }
    }
}

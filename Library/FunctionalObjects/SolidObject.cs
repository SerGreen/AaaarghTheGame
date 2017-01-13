using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MultiplayerLib
{
    public class SolidObject : MarshalByRefObject
    {
        public Rectangle box { get; set; }
        private Brush bB = new SolidBrush(Color.Black);
        private Brush bG = new SolidBrush(Color.DarkGray);
        public bool semiSolid { get; set; }

        public SolidObject(int X, int Y, int Width, int Height, bool semiSolid)
        {
            box = new Rectangle(X, Y, Width, Height);
            this.semiSolid = semiSolid;
        }

        public virtual void paint(Graphics g)
        {
            if (!semiSolid)
                g.FillRectangle(bB, box);
            else
                g.FillRectangle(bG, box);
        }
    }
}

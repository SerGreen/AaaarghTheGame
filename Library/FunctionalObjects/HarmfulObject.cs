using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MultiplayerLib.FunctionalObjects
{
    public class HarmfulObject : MarshalByRefObject
    {
        public Rectangle box { get; set; }
        public int Damage { get; set; }
        private Brush bR = new SolidBrush(Color.Red);

        public HarmfulObject(int X, int Y, int Width, int Height, int damage)
        {
            box = new Rectangle(X, Y, Width, Height);
            Damage = damage;
        }

        public void paint(Graphics g)
        {
            g.FillRectangle(bR, box);
        }
    }
}

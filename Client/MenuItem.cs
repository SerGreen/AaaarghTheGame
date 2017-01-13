using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Client
{
    class MenuItem
    {
        private Bitmap icon;
        private Brush grayA;
        private Brush greenA;
        private Brush white;
        private Font font18;
        private Rectangle Box { get; set; }
        public string Label { get; set; }
        public bool hovered { get; set; }

        public MenuItem(int x, int y, int width, int height, string label)
        {
            Box = new Rectangle(x, y, width, height);
            Label = label;
            grayA = new SolidBrush(Color.FromArgb(128, Color.LightGray));
            white = new SolidBrush(Color.White);
            greenA = new SolidBrush(Color.FromArgb(128, Color.Lime));
            font18 = new Font("Calibri", 18);
            hovered = false;
        }

        public Color selectedColor
        {
            set { greenA = new SolidBrush(Color.FromArgb(128, value)); }
        }

        public MenuItem(int x, int y, int width, int height, string label, Bitmap icon)
            : this(x, y, width, height, label)
        {
            this.icon = icon;
        }

        public void tick(int mouseX, int mouseY)
        {
            Rectangle point = new Rectangle(mouseX, mouseY, 1, 1);
            hovered = Box.IntersectsWith(point);
        }

        public void paint(Graphics g)
        {
            if (hovered)
                g.FillRectangle(greenA, Box);
            else
                g.FillRectangle(grayA, Box);

            if (icon != null)
                g.DrawImage(icon, Box.X - 10, Box.Y - icon.Height + 32, icon.Width * 1.5f, icon.Height * 1.5f);
            
            g.DrawString(Label, font18, white, Box.X + (icon == null ? 10 : 64), Box.Y + 10);
        }
    }
}

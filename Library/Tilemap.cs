using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MultiplayerLib
{
    [Serializable]
    public class Tilemap
    {
        public Bitmap[] tileset;
        public short[,] map;
        public int shiftX;
        public int shiftY;
        public int tileWidth;
        public int tileHeight;

        public Tilemap(int width, int height, int tileWidth, Bitmap bmp)
        {
            tileset = new Bitmap[bmp.Width / tileWidth];

            this.tileWidth = tileWidth;
            tileHeight = bmp.Height;

            for (int i = 0; i < tileset.Length; i++)
            {
                tileset[i] = bmp.Clone(new Rectangle(i * tileWidth, 0, tileWidth, tileHeight), bmp.PixelFormat);
            }

            map = new short[width, height];
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    map[i, j] = -1;
        }

        public void setTile(int x, int y, short value)
        {
            if (x < 0 || x > map.GetLength(0) || y < 0 || y > map.GetLength(1))
            {
                return;
            }
            else
            {
                if (value > tileset.Length - 1)
                    map[x, y] = -1;
                else
                    map[x, y] = value;
            }
        }

        public void setShiftX(int value)
        { shiftX = value; }

        public void setShiftY(int value)
        { shiftY = value; }

        public void paint(Graphics g)
        {
            for(int i=0;i<map.GetLength(0);i++)
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    short index = map[i,j];
                    if (index >= 0)
                        g.DrawImage(tileset[index], i * tileWidth + shiftX, j * tileHeight + shiftY, tileWidth, tileHeight);
                }
        }

        public void paint(Graphics g, int vpX, int vpY, int vpWidth, int vpHeight)
        {
            int startX = Math.Max(0, vpX / tileWidth);
            int stopX = Math.Min(map.GetLength(0), startX + vpWidth / tileWidth + 3);
            int startY = Math.Max(0, vpY / tileHeight);
            int stopY = Math.Min(map.GetLength(1), startY + vpHeight / tileHeight + 3);

            for (int i = startX; i < stopX; i++)
                for (int j = startY; j < stopY; j++)
                {
                    short index = map[i, j];
                    if (index >= 0)
                        g.DrawImage(tileset[index], i * tileWidth + shiftX, j * tileHeight + shiftY, tileWidth, tileHeight);
                }
        }
    }
}

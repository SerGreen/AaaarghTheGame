using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MultiplayerLib
{
    public enum SpriteIndex { Arrow, Slash, Slash_Axe, Slash_Dagger, Bolt, Melon, MelonCrack, Knife, Arbalest_Smash, Fireball, 
        Dust, Blood_Arrow, Blood_Slash, Popup_plus1, Popup_plus2, Popup_minus1, Fireball_Explosion, Explosion, FireBurst, 
        DominPlate, Flag, FlagPlace, Crown, Skull, Skull_Front, Statue, Tombstone, Tombstone_crush, 
        war_Stand, war_Run, war_Jump, war_Swing, war_Jump_Swing, war_Run_Swing, 
        ber_Stand, ber_Run, ber_Jump, ber_Swing, ber_Jump_Swing, ber_Run_Swing, 
        bow_Stand, bow_Run, bow_Jump, bow_Swing, bow_Jump_Swing, bow_Run_Swing, 
        arb_Stand, arb_Run, arb_Jump, arb_Swing, arb_Jump_Swing, arb_Run_Swing, arb_Reload, arb_Smack, arb_Run_Smack, arb_Jump_Smack, 
        kni_Stand, kni_Run, kni_Jump, kni_Swing, kni_Jump_Swing, kni_Run_Swing,
        thi_Stand, thi_Run, thi_Jump, thi_Swing, thi_Jump_Swing, thi_Run_Swing, thi_Throw, thi_Jump_Throw, thi_Run_Throw,
        mag_Stand, mag_Run, mag_Jump, mag_Swing, mag_Jump_Swing, mag_Run_Swing
    };

    public class Sprite : MarshalByRefObject
    {
        public float x { get; set; }
        public float y { get; set; }
        private int width;
        private int height;
        private Bitmap bmp;
        public int spriteIndex;
        public int frames;
        public int currentFrame;
        private Rectangle collisionBox;
        public int frameWidth;
        private Point origin;
        private float scale;

        public Point getOrigin()
        { return origin; }

        public float getScale()
        { return scale; }

        public int Width
        { get { return width; } }

        public int Height
        { get { return height; } }

        public Rectangle getCollisionBox()
        { return collisionBox; }

        public float Scale
        { get { return scale; } }

        public Sprite(float x, float y, Bitmap bmp, int frameWidth)
        {
            this.x = x;
            this.y = y;
            origin = new Point(0, 0);
            this.frameWidth = frameWidth;
            this.bmp = bmp;
            this.width = frameWidth;
            this.height = bmp.Height;
            currentFrame = 0;
            collisionBox = new Rectangle(0, 0, frameWidth, bmp.Height);

            frames = bmp.Width / frameWidth;

            if (bmp.Width % frameWidth != 0)
            {
                throw new Exception("FATAL ERROR! GO TO HELL!");
            }
        }

        public Sprite(float x, float y, SpriteIndex spriteInd, Bitmap bmp, int frameWidth, float scale)
        {
            this.x = x;
            this.y = y;
            this.width = frameWidth;
            this.height = bmp.Height;
            origin = new Point(0, 0);
            this.frameWidth = frameWidth;
            this.spriteIndex = (int) spriteInd;
            currentFrame = 0;
            collisionBox = new Rectangle(0, 0, frameWidth, bmp.Height);

            frames = bmp.Width / frameWidth;

            if (bmp.Width % frameWidth != 0)
            {
                throw new Exception("FATAL ERROR! GO TO HELL!");
            }

            this.scale = scale;
            this.collisionBox = new Rectangle(0, 0, (int)(frameWidth * scale), (int)(bmp.Height * scale));
        }

        public Sprite(float x, float y, Bitmap bmp, int frameWidth, float scale)
            : this(x, y, bmp, frameWidth)
        {
            this.scale = scale;
            this.collisionBox = new Rectangle(0, 0, (int) (frameWidth * scale), (int) (bmp.Height * scale));
        }
        
        public void setOriginPoint(int x, int y)
        {
            origin = new Point(x, y);
        }

        public void paint(Graphics g)
        {
            paint(g, 1, 1);
        }

        public void paint(Graphics g, float scaleX, float scaleY)
        {
            Bitmap frame = bmp.Clone(new Rectangle(currentFrame * frameWidth, 0, frameWidth, bmp.Height), bmp.PixelFormat);
            int shiftX = 0;
            int shiftY = 0;
            if (scaleX < 0)
                shiftX = (int) (frame.Width * scale * scaleX);
            if (scaleY < 0)
                shiftY = (int)(frame.Height * scale * scaleY);

            g.DrawImage(frame, x + origin.X * scaleX * scale - shiftX, y + origin.Y * scaleY * scale - shiftY, frame.Width * scaleX * scale, frame.Height * scaleY * scale);
        }

        public void nextFrame()
        {
            currentFrame++;
            if (currentFrame == frames)
                currentFrame = 0;
        }

        public void prevFrame()
        {
            currentFrame--;
            if (currentFrame < 0)
                currentFrame = frames - 1;
        }

        public void setFrame(int number)
        {
            if (number >= 0)
                currentFrame = number % frames;
        }

        public int getCurrentFrame()
        { return currentFrame; }

        public void setCollisionBox(int x, int y, int width, int height)
        {
            collisionBox = new Rectangle((int)(x * scale), (int)(y * scale), (int)(width * scale), (int)(height * scale));
        }

        public bool checkCollision(Sprite sprite)
        {
            Rectangle me = new Rectangle((int) (x + collisionBox.X), (int) (y + collisionBox.Y), collisionBox.Width, collisionBox.Height);
            Rectangle other = new Rectangle((int)(sprite.x + sprite.getCollisionBox().X), (int)(sprite.y + sprite.getCollisionBox().Y), sprite.getCollisionBox().Width, sprite.getCollisionBox().Height);

            return me.IntersectsWith(other);
        }

        public bool checkCollision(Rectangle rect)
        {
            Rectangle me = new Rectangle((int)(x + collisionBox.X), (int)(y + collisionBox.Y), collisionBox.Width, collisionBox.Height);
            return me.IntersectsWith(rect);
        }

        public bool checkCollision(Rectangle rect, float dx, float dy)
        {
            Rectangle me = new Rectangle((int)(x + collisionBox.X + dx), (int)(y + collisionBox.Y + dy), collisionBox.Width, collisionBox.Height);
            return me.IntersectsWith(rect);
        }
    }
}

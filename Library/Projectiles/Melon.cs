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
    public class Melon : Arrow
    {
        private int animationSlowdown;
        private Sprite crack;

        public Melon(float x, float y, int face, int damage, int team, int ownerID, int lifetime, float vx, float vy, float gravity)
            : base(x, y, face, damage, team, ownerID, lifetime, vx, vy, gravity)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Melon, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/melon.png"), 22, 1);
            sprite.setCollisionBox(1, 1, 20, 20);
            crack = new Sprite(x, y, SpriteIndex.MelonCrack, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/melon_crack.png"), 22, 1);
            crack.setCollisionBox(1, 1, 20, 20);
            disarmed = true;
            animationSlowdown = 0;
        }

        public void addSpeed(float dx, float dy)
        {
            vx += dx;
            vy += dy;
        }

        public override void tick(ArrayList solidObjects, ArrayList projectiles, ref ArrayList particles)
        {
            base.tick(solidObjects, projectiles, ref particles);
            doSpriteStuff();
        }

        private void doSpriteStuff()
        {
            animationSlowdown++;
            if (animationSlowdown == 4)
            {
                animationSlowdown = 0;

                sprite.nextFrame();

                if (sprite == crack && sprite.getCurrentFrame() == 0)
                    destroy();
            }
        }

        protected override void move(ArrayList solidObjects)
        {
            bool noCollisions = true;

            if (vx != 0 || vy != 0)
            {
                for (int i = 0; i < solidObjects.Count; i++)
                {
                    SolidObject so = (SolidObject)solidObjects[i];

                    if (!so.semiSolid && sprite.checkCollision(so.box, vx, vy) == true)
                    {
                        noCollisions = false;
                        break;
                    }
                }

                if (!noCollisions)
                {
                    vx = 0;
                    vy = 0;
                    gravity = 0;
                    disarmed = true;
                    if (sprite != crack)
                    {
                        crack.x = sprite.x;
                        crack.y = sprite.y;
                        sprite = crack;
                    }
                }
                else
                {
                    sprite.x += vx;
                    sprite.y += vy;
                }

                vy += gravity;
            }
        }
    }
}

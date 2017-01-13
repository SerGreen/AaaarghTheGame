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
    public class Knife : Arrow
    {
        protected int animationSlowdown;

        public Knife(float x, float y, int face, int damage, int team, int ownerID, int lifetime, float vx, float vy, float gravity)
            : base(x, y, face, damage, team, ownerID, lifetime, vx, vy, gravity)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Knife, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/knife.png"), 16, 2);
            sprite.setCollisionBox(3, 3, 10, 10);

            animationSlowdown = 0;
        }

        protected override void decreaseDamage()
        {
            if (activated && !disarmed && damage > 2.0f)
                damage -= 0.1f;
        }

        public override void tick(ArrayList solidObjects, ArrayList projectiles, ref ArrayList particles)
        {
            animationSlowdown++;
            if (animationSlowdown == 1)
            {
                animationSlowdown = 0;

                if (!disarmed)
                    sprite.nextFrame();
                else
                    sprite.setFrame(0);
            }

            base.tick(solidObjects, projectiles, ref particles);
        }

        protected override void checkCollisionWithSlash(ArrayList projectiles)
        { }
    }
}

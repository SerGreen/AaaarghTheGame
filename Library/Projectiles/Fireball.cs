using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Collections;
using MultiplayerLib.Particles;

namespace MultiplayerLib.Projectiles
{
    public class Fireball : Arrow
    {
        protected int animationSlowdown;

        public Fireball(float x, float y, int face, int damage, int team, int ownerID, int lifetime, float vx, float vy, float scale)
            : base(x, y, face, damage, team, ownerID, lifetime, vx, vy, 0)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Fireball, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/fireball.png"), 32, scale);
            sprite.setCollisionBox(3, 3, 10, 10);

            animationSlowdown = 0;
        }

        protected override void decreaseDamage()
        { }

        public override void tick(ArrayList solidObjects, ArrayList projectiles, ref ArrayList particles)
        {
            animationSlowdown++;
            if (animationSlowdown == 1)
            {
                animationSlowdown = 0;

                if (!disarmed)
                    sprite.nextFrame();
                else
                {
                    Particle part = (Particle) destroy();
                    if (part != null)
                        particles.Add(part);
                }
            }

            base.tick(solidObjects, projectiles, ref particles);
        }

        public override object destroy()
        {
            base.destroy();
            return new FireballExplosion(X, Y + sprite.Height / 2, face);
        }

        protected override void checkCollisionWithSlash(ArrayList projectiles)
        { }
    }
}

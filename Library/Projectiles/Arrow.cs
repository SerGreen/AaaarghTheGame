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
    public class Arrow : Projectile
    {
        public float vx { get; set; }
        public float vy { get; set; }
        public float gravity { get; set; }
        public Point relativePositionInPlayer { get; set; }
        public bool faceMatchPlayerFace { get; set; }

        public Arrow(float x, float y, int face, int damage, int team, int ownerID, int lifetime, float vx, float vy, float gravity)
            : base(x, y, face, damage, team, ownerID, lifetime)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Arrow, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/arrow.png"), 21, 2);
            sprite.setCollisionBox(0, 1, 21, 1);

            this.vx = vx;
            this.vy = vy;
            this.gravity = gravity;
        }

        public override void tick(ArrayList solidObjects, ArrayList projectiles, ref ArrayList particles)
        {
            move(solidObjects);
            checkCollisionWithSlash(projectiles);
            base.tick(solidObjects, projectiles, ref particles);
        }

        protected virtual void checkCollisionWithSlash(ArrayList projectiles)
        {
            foreach (Projectile pr in projectiles)
            {
                if (pr is SwordSlash || pr is AxeSlash)
                {
                    if (pr.getSprite.checkCollision(sprite))
                    {
                        destroy();
                        break;
                    }
                }
            }
        }

        protected virtual void decreaseDamage()
        {
            if (activated && !disarmed && damage > 0.5f)
                damage -= 0.25f;
        }

        protected virtual void move(ArrayList solidObjects)
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

                sprite.x += vx;
                sprite.y += vy;

                if (!noCollisions)
                {
                    vx = 0;
                    vy = 0;
                    gravity = 0;
                    disarmed = true;
                }
            }

            vy += gravity;
        }
    }
}

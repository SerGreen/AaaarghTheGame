using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections;

namespace MultiplayerLib
{
    public enum ProjectyleType { Slash, Slash_Axe, Arrow, Bolt };

    public class VeryOldProjectile : MarshalByRefObject
    {
        public Sprite sprite;
        public float vx;
        public float vy;
        public int lifetime;
        public float gravity;
        public int team;
        public int ownerID;
        public ProjectyleType type;
        public bool timeToDie;
        public bool activated;
        public bool disarmed;
        public int face;
        public float damage;

        public VeryOldProjectile(ProjectyleType type, int x, int y, float vx, float vy, int face, int lifetime, float gravity, int damage, int team)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            this.vx = vx;
            this.vy = vy;
            this.lifetime = lifetime;
            this.gravity = gravity;
            this.face = face;
            timeToDie = false;
            activated = false;
            disarmed = false;
            this.damage = damage;
            this.team = team;
            ownerID = -1;

            if (type == ProjectyleType.Slash)
            {
                sprite = new Sprite(x, y, SpriteIndex.Slash, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/slash.png"), 22, 2);
                sprite.setCollisionBox(0, 2, 22, 17);
                this.type = type;
            }
            else if (type == ProjectyleType.Slash_Axe)
            {
                sprite = new Sprite(x, y, SpriteIndex.Slash_Axe, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/slash_axe.png"), 37, 2);
                sprite.setCollisionBox(1, 4, 34, 20);
                this.type = type;
            }
            else if (type == ProjectyleType.Arrow)
            {
                sprite = new Sprite(x, y, SpriteIndex.Arrow, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/arrow.png"), 21, 2);
                sprite.setCollisionBox(0, 1, 21, 1);
                this.type = type;
            }
            else if (type == ProjectyleType.Bolt)
            {
                sprite = new Sprite(x, y, SpriteIndex.Bolt, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/bolt.png"), 14, 2);
                sprite.setCollisionBox(0, 1, 14, 1);
                this.type = type;
            }
        }

        public float getX()
        { return sprite.x; }

        public float getY()
        { return sprite.y; }

        public void tick(ArrayList solidObjects)
        {
            move(solidObjects);

            if ((type == ProjectyleType.Arrow || type == ProjectyleType.Bolt) && activated)
                damage -= 0.25f;

            lifetime--;
            if (lifetime < 0)
            {
                timeToDie = true;
            }
        }

        private void move(ArrayList solidObjects)
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

                    if (type == ProjectyleType.Slash || type == ProjectyleType.Slash_Axe)
                    {
                        timeToDie = true;
                    }
                    else
                    {
                        disarmed = true;
                    }
                }
            }

            vy += gravity;
        }
    }
}

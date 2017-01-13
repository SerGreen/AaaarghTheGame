using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MultiplayerLib
{
    public abstract class GravityInfluencedObject : MarshalByRefObject
    {
        protected Sprite sprite;
        public float vx { get; set; }
        public float vy { get; set; }
        public float maxGravity { get; set; }
        protected float gravity;
        protected float speedMultiplier;
        public bool fallThroughSemiSolid { get; set; }

        public GravityInfluencedObject(float maxGravity)
        {
            vx = 0;
            vy = 0;
            gravity = 0;
            this.maxGravity = maxGravity;
            speedMultiplier = 1;
            fallThroughSemiSolid = false;
        }

        public Sprite GetSprite
        { get { return sprite; } }

        public float X
        {
            get { return sprite.x; }
            set { sprite.x = value; }
        }

        public float Y
        {
            get { return sprite.y; }
            set { sprite.y = value; }
        }

        public virtual void move(ArrayList solidObjects)
        {
            bool noCollisions = true;
            for (int i = 0; i < solidObjects.Count; i++)
            {
                SolidObject so = (SolidObject)solidObjects[i];

                if (sprite.checkCollision(so.box, vx * speedMultiplier, 0) == true && so.semiSolid == false)
                {
                    noCollisions = false;
                    break;
                }
            }

            if (noCollisions)
            {
                X += vx * speedMultiplier;
            }

            noCollisions = true;
            for (int i = 0; i < solidObjects.Count; i++)
            {
                SolidObject so = (SolidObject)solidObjects[i];

                if (sprite.checkCollision(so.box, 0, vy) == true)                                         //if next step will be collision with obj.                    
                {
                    if (vy < 0 && so.semiSolid == false)
                    {
                        noCollisions = false;
                        break;
                    }
                    else
                    {
                        if (so.semiSolid == false ||                                                                                         //if obj. is solid
                           (so.semiSolid == true && vy >= 0 && sprite.checkCollision(so.box, 0, 0) == false && !fallThroughSemiSolid))       //or it is semiSolid, while now there is no collisions with obj. and player moving down
                        {
                            moveToObject(so, 0, 1);
                            noCollisions = false;
                        }
                    }
                }
            }

            if (noCollisions)
                Y += vy;
            else
                vy = 0;

            vy += gravity;
        }

        public virtual void moveToObject(SolidObject so, int dx, int dy)
        {
            while (!sprite.checkCollision(so.box, dx, dy))
            {
                Y += dy;
                X += dx;
            }
        }

        public void moveTo(float x, float y)
        {
            sprite.x = x;
            sprite.y = y;
        }

        protected virtual void checkGroundUnderneath(ArrayList solidObjects, ref ArrayList particles)
        {
            bool noGroungUnderneath = true;
            for (int i = 0; i < solidObjects.Count; i++)
            {
                SolidObject so = (SolidObject)solidObjects[i];
                if (sprite.checkCollision(so.box, 0, 1) == true &&                                     //if one bit below is obj.
                    (so.semiSolid == false ||                                                          //and it is solid
                    so.semiSolid == true && sprite.checkCollision(so.box, 0, 0) == false))             //or it is semi solid but now there is no collision with it
                {                                                                                      //then we have ground under foots
                    gravity = 0;
                    vy = 0;
                    noGroungUnderneath = false;

                    doStuffCaseGroundUnderneath(ref particles);
                }
            }

            if (noGroungUnderneath)
            {
                gravity = maxGravity;
                doStuffCaseNoGroundUnderneath();
            }
        }

        protected virtual void doStuffCaseNoGroundUnderneath()
        { }

        protected virtual void doStuffCaseGroundUnderneath(ref ArrayList particles)
        { }
    }
}

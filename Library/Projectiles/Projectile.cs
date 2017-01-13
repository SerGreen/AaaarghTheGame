using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace MultiplayerLib
{
    public abstract class Projectile : MarshalByRefObject
    {
        protected Sprite sprite;
        public int team;
        public int ownerID;
        private bool timeToDie;
        public bool activated;
        public bool disarmed;
        public int face;
        public float damage;
        public int lifetime;
        protected ArrayList damagedPlayersID;

        public Projectile(float x, float y, int face, int damage, int team, int ownerID, int lifetime)
        {
            this.face = face;
            this.damage = damage;
            this.team = team;
            this.ownerID = ownerID;
            this.lifetime = lifetime;
            activated = false;
            disarmed = false;
            timeToDie = false;
            sprite = null;
            damagedPlayersID = new ArrayList();
        }

        public float X
        { get { return sprite.x; } }

        public float Y
        { get { return sprite.y; } }

        public Sprite getSprite
        { get { return sprite; } }

        public void addDamagedPlayer(int ID)
        {
            damagedPlayersID.Add(ID);
        }

        public bool TimeToDie
        { get { return timeToDie; } }

        public bool isPlayerDamaged(int ID)
        {
            foreach (int id in damagedPlayersID)
                if (id == ID)
                    return true;

            return false;
        }

        public void moveTo(float x, float y)
        {
            sprite.x = x;
            sprite.y = y;
        }

        public ArrayList DamagedPlayersID
        { get { return damagedPlayersID; } }

        public virtual void tick(ArrayList solidObjects, ArrayList projectiles, ref ArrayList particles)
        {
            lifetime--;
            if (lifetime < 0)
                timeToDie = true;
        }

        public virtual object destroy()
        {
            timeToDie = true;
            return null;
        }
    }
}

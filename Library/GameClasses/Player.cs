using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections;
using MultiplayerLib.Particles;
using MultiplayerLib.Projectiles;
using MultiplayerLib.FunctionalObjects;

namespace MultiplayerLib
{
    public abstract class Player : GravityInfluencedObject
    {
        private int id;
        protected Sprite run;
        protected Sprite stand;
        protected Sprite jump;
        protected Sprite swing;
        protected Sprite runSwing;
        protected Sprite jumpSwing;
        public bool running { get; set; }
        public float movingSpeedX;
        public float jumpSpeedY;
        private int team;
        public int lastHitByPlayer;
        public int Frags { get; set; }
        public int Assists { get; set; }
        public int Deaths { get; set; }
        public float Health { get; set; }
        public float healthMax;
        public float poison { get; set; }
        public bool Dead { get; set; }
        public int TimeToRespawn { get; set; }
        private PlayerDamageStats damageStats;
        public int healCooldown;
        public int weaponCooldown;
        public int weaponStrikeDelay;
        public int weaponCharge;
        public int weaponChargeLimit { get; set; }
        public bool weaponReadyToAction { get; set; }
        public bool weaponInAction;
        private string name;
        public bool inAir { get; set; }
        public int face { get; set; }
        public string className { get; set; }
        public string classAfterRespawn;
        public int teamAfterRespawn;
        public Flag flag;
        public bool king { get; set; }
        public int hitDamage { get; set; }
        private ArrayList arrowsInMe;
        private ArrayList mySkulls;
        protected bool stunnable;
        protected int stunTimeout;
        protected float stunPower;
        protected int maxStunTime;

        protected int animationSlowdown = 0;

        public Player(int health, float speedX, float jumpY, int damage, int team, int face, string name, int id)
            : base(0.5f)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = run = stand = jump = swing = runSwing = jumpSwing = null;
            this.id = id;
            movingSpeedX = speedX;
            jumpSpeedY = jumpY;
            this.face = face;
            this.team = team;
            teamAfterRespawn = team;
            lastHitByPlayer = -1;
            Frags = 0;
            Assists = 0;
            Deaths = 0;
            inAir = false;
            this.Health = health;
            this.healthMax = health;
            poison = 0;
            Dead = false;
            TimeToRespawn = 0;
            damageStats = new PlayerDamageStats();
            arrowsInMe = new ArrayList();
            mySkulls = new ArrayList();
            healCooldown = 0;
            this.name = name;
            hitDamage = damage;
            weaponCooldown = 0;
            weaponCharge = 0;
            weaponStrikeDelay = -1;
            weaponReadyToAction = false;
            weaponInAction = false;
            className = "Undefined";
            flag = null;
            king = false;
            stunnable = true;
            stunPower = 1;
            stunTimeout = 0;
            maxStunTime = 10;
        }

        public float Width
        { get { return sprite.Width; } }

        public float Height
        { get { return sprite.Height; } }

        public Sprite getSprite()
        { return sprite; }

        public string Name
        { get { return name; } }

        public int ID
        { get { return id; } }

        public int Team
        { 
            get { return team; }
            set { team = value; }
        }

        public ArrayList ArrowsInMe
        { get { return arrowsInMe; } }

        public ArrayList MySkulls
        { get { return mySkulls; } }

        public PlayerDamageStats DamageStats
        { get { return damageStats; } }

        public virtual void respawn(float x, float y)
        {
            Health = healthMax;
            Dead = false;
            poison = 0;
            damageStats = new PlayerDamageStats();
            weaponCooldown = 0;
            weaponCharge = 0;
            weaponReadyToAction = false;
            weaponInAction = false;
            lastHitByPlayer = -1;
            team = teamAfterRespawn;
            king = false;
            X = x;
            Y = y;
            if (flag != null)
            {
                flag.timeToReturn = true;
                flag = null;
            }
            removeMyArrows();
            removeMySkulls(-1);
        }

        public virtual void respawn(float x, float y, ref ArrayList projectiles)
        {
            respawn(x, y);
        }

        public virtual void setSprite(String spriteName)
        {
            float x = X;
            float y = Y;

            if (spriteName.CompareTo("run") == 0)
                sprite = run;

            else if (spriteName.CompareTo("swing") == 0)
            {
                int frame = 0;
                if (sprite == jumpSwing)
                    frame = sprite.getCurrentFrame();

                sprite = swing;
                sprite.setFrame(frame);
            }

            else if (spriteName.CompareTo("stand") == 0)
                sprite = stand;

            else if (spriteName.CompareTo("jump") == 0)
                sprite = jump;

            else if (spriteName.CompareTo("jumpSwing") == 0)
            {
                int frame = 0;
                if (sprite == swing)
                    frame = sprite.getCurrentFrame();

                sprite = jumpSwing;
                sprite.setFrame(frame);
            }

            else if (spriteName.CompareTo("runSwing") == 0)
                sprite = runSwing;

            X = x;
            Y = y;
        }

        public virtual void doJump()
        {
            if (sprite == swing)
            {
                swing.setFrame(0);
                setSprite("jumpSwing");
            }

            push(0, -1);
            vy -= jumpSpeedY;
        }

        public virtual void doJumpDown()
        {
            if (sprite == swing)
            {
                swing.setFrame(0);
                setSprite("jumpSwing");
            }

            push(0, 1);
        }

        public void push(float dx, float dy)
        {
            X += dx;
            Y += dy;
        }

        protected virtual void applyDamage(float damage, bool causeStun, int ID)
        {
            lastHitByPlayer = ID;
            damageStats.addDamage(ID, damage);
            Health -= damage;

            if (causeStun)
            {
                breakSwing();

                if (stunnable)
                {
                    float newStunPower = 1 - (damage / healthMax) * 1.5f;

                    if (stunTimeout > 0)
                    {
                        float stronger = stunPower < newStunPower ? stunPower : newStunPower;
                        float weaker = stunPower > newStunPower ? stunPower : newStunPower;

                        stunPower = stronger * weaker * 0.95f;
                    }
                    else
                    {
                        stunPower = newStunPower;
                    }

                    if (stunPower < 0.1f)
                        stunPower = 0.1f;

                    stunTimeout = maxStunTime;
                }
            }
        }

        protected virtual void breakSwing()
        {
            if (weaponReadyToAction && !weaponInAction)
            {
                weaponReadyToAction = false;
            }
        }

        public override void move(ArrayList solidObjects)
        {
            float stunSpeedMultiplier = stunTimeout > 0 ? stunPower : 1;
            
            speedMultiplier = flag != null ? 0.6f : 1;
            speedMultiplier = king ? 1.2f : 1;

            speedMultiplier *= stunSpeedMultiplier;

            base.move(solidObjects);
        }

        public void pushOut(SolidObject so, int dx, int dy)
        {
            do
            {
                Y += dy;
                X += dx;
            }
            while (sprite.checkCollision(so.box) == true);
        }

        public virtual void tick(ArrayList solidObjects, ArrayList projectiles, ref ArrayList particles, ArrayList harmfulObjects)
        {
            checkGroundUnderneath(solidObjects, ref particles);
            doFlagStuff();
            move(solidObjects);
            moveMyArrowsInsideOfMe();
            moveMySkulls();
            checkSpikeTrapsInsideOfSelf(harmfulObjects);

            if (!Dead)
                checkProjectilesInsideOfSelf(projectiles, ref particles);
            
            doSpriteStuff();
            doCountersStuff();
        }

        private void moveMyArrowsInsideOfMe()
        {
            float midX = X + Width * sprite.Scale / 2;

            for (int i = 0; i < arrowsInMe.Count; i++)
            {
                Arrow arr = (Arrow)arrowsInMe[i];
                arr.face = arr.faceMatchPlayerFace ? face : face * -1;
                arr.moveTo(midX - arr.relativePositionInPlayer.X, Y - arr.relativePositionInPlayer.Y);
            }
        }

        public void removeMyArrows()
        {
            for (int i = 0; i < arrowsInMe.Count; i++)
            {
                Arrow arr = (Arrow)arrowsInMe[i];
                arr.lifetime = 0;
            }
        }

        private void moveMySkulls()
        {
            float midX = X + Width * sprite.Scale / 2;

            for (int i = mySkulls.Count - 1; i >= 0; i--)
            {
                Skull sk = (Skull)mySkulls[i];
                sk.face = face;
                sk.moveTo(midX - 20 * face - sk.GetSprite.Width / 2, Y - 4 * i);
            }
        }

        public int removeMySkulls(int teamToRemove)
        {
            int removedSkulls = 0;
            for (int i = mySkulls.Count - 1; i >= 0; i--)
            {
                Skull sk = (Skull)mySkulls[i];
                if (teamToRemove < 0 || teamToRemove == sk.team)
                {
                    sk.TimeToDisappear = true;
                    mySkulls.RemoveAt(i);
                    removedSkulls++;
                }
            }
            return removedSkulls;
        }

        public void addSkull(Skull skull)
        {
            mySkulls.Add(skull);
        }

        private void doFlagStuff()
        {
            if (flag != null)
            {
                flag.moveTo(sprite.x + sprite.Width*sprite.Scale / 2 - flag.GetSprite.Width*flag.GetSprite.Scale/2 - 6*face, sprite.y + sprite.Height - flag.GetSprite.Height - 8);
            }
        }

        private void checkSpikeTrapsInsideOfSelf(ArrayList harmfulObjects)
        {
            foreach (HarmfulObject ho in harmfulObjects)
            {
                if (!Dead && sprite.checkCollision(ho.box))
                {
                    applyDamage(ho.Damage, false, -1);
                }
            }
        }

        protected virtual void doCountersStuff()
        {
            if (!Dead)
            {
                if (weaponCooldown > 0)
                    weaponCooldown--;

                doChargeTick();

                if (weaponStrikeDelay >= 0)
                    weaponStrikeDelay--;


                if (healCooldown > 0)
                    healCooldown--;

                if (poison > 0)
                {
                    poison--;
                    if (poison <= 0)
                        poison = 0;

                    applyDamage(0.1f, false, lastHitByPlayer);
                    healCooldown = 100;
                }

                doHealTick();
            }
            else
            {
                if (TimeToRespawn > 0)
                    TimeToRespawn--;
            }

            if (stunnable && stunTimeout > 0 && !inAir)
                stunTimeout--;

            if (vy < -jumpSpeedY * 1.0f)
                vy = -jumpSpeedY * 1.0f;
        }

        protected virtual void doHealTick()
        {
            if (healCooldown == 0 && Health < healthMax * (king ? 1.5f : 1))
                Health += 0.08f * (king ? 1.5f : 1);
        }

        protected virtual void doChargeTick()
        {
            if (weaponReadyToAction)
            {
                if (weaponCharge < weaponChargeLimit)
                    weaponCharge++;
            }
        }

        private void checkProjectilesInsideOfSelf(ArrayList projectiles, ref ArrayList particles)
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                Projectile pr = (Projectile)projectiles[i];
                if (pr.activated)
                {
                    if (!pr.disarmed)
                    {
                        if (pr.team != team && sprite.checkCollision(pr.getSprite))
                        {
                            /*
                            if (pr is Fireball)
                            {
                                pr.disarmed = true;
                            }
                            else  // Enables fireball damage */
                            {
                                applyDamage(pr.damage > Health ? Health : pr.damage, true, pr.ownerID);
                                healCooldown = 200;

                                if (pr is Arrow)
                                {
                                    if (pr is PoisonedArrow)
                                        poison += 100 + (pr.damage * 4.65f);

                                    int shiftX = 0;
                                    if (pr.face == 1)
                                        shiftX = 0;
                                    else
                                        shiftX = -64;

                                    particles.Add(new BloodArrowParticle(X + sprite.Width * sprite.Scale / 2 + shiftX, pr.Y - 16, ((Arrow)pr).vx / 10, 0, pr.face, 100));

                                    if (!(pr is Knife) && !(pr is Fireball))
                                    {
                                        //pr.destroy();
                                        Arrow arr = (Arrow)pr;
                                        arr.vx = 0;
                                        arr.vy = 0;
                                        arr.gravity = 0;
                                        arr.lifetime = 500;

                                        float midX = X + Width * sprite.Scale / 2;
                                        arr.faceMatchPlayerFace = (arr.face == face);
                                        arr.relativePositionInPlayer = new Point((int)(arr.getSprite.Width * arr.getSprite.Scale / 2), (int)(Y - arr.Y));

                                        arrowsInMe.Add(pr);
                                    }

                                    pr.disarmed = true;
                                }
                                else if (pr is DaggerSlash)
                                {
                                    if (!pr.isPlayerDamaged(ID))
                                    {
                                        int shiftX = 0;
                                        if (pr.face == 1)
                                            shiftX = 0;
                                        else
                                            shiftX = -64;

                                        particles.Add(new BloodArrowParticle(X + sprite.Width * sprite.Scale / 2 + shiftX, pr.Y - 16, 2 * pr.face, 0, pr.face, 100));
                                    }
                                }
                                else if (!pr.isPlayerDamaged(ID))
                                {
                                    int shiftX = 0;
                                    if (pr.face == 1)
                                        shiftX = 0;
                                    else
                                        shiftX = -64;

                                    particles.Add(new BloodSlashParticle(X + sprite.Width * sprite.Scale / 2 + shiftX, pr.Y, 2 * pr.face, 0, pr.face, 100));
                                }

                                pr.addDamagedPlayer(ID);
                            }
                        }
                    }
                    else
                    {
                        doStuffForDisarmedProjectile(pr);
                    }
                }
            }
        }

        protected virtual void doStuffForDisarmedProjectile(Projectile pr)
        { }

        protected override void doStuffCaseGroundUnderneath(ref ArrayList particles)
        {
            if (inAir)
            {
                inAir = false;
                if (!Dead)
                    particles.Add(new DustParticle(X + sprite.Width * sprite.Scale / 2 - 16 * face, Y + sprite.Height * sprite.Scale - 8, 0, -1.5f, face, 100));

                if (weaponReadyToAction)
                {
                    if (vx == 0)
                    {
                        setSprite("swing");
                        sprite.setFrame(0);
                    }
                    else
                        setSprite("runSwing");
                }
                else
                {
                    if (vx == 0)
                        setSprite("stand");
                    else
                        setSprite("run");
                }
            }

            if (!running && vx != 0)
            {
                //vx *= 0.6f;
                //if (Math.Abs(vx) < 0.01f)
                {
                    vx = 0;
                    running = false;
                    setSprite("stand");
                }
            }
        }

        protected override void doStuffCaseNoGroundUnderneath()
        {
            inAir = true;
            if (sprite != jump || sprite != jumpSwing)
            {
                if (weaponReadyToAction)
                {
                    if (sprite != jumpSwing)
                    {
                        setSprite("jumpSwing");
                        sprite.setFrame(0);
                    }
                }
                else
                    setSprite("jump");
            }
        }

        protected virtual void doSpriteStuff()
        {
            animationSlowdown++;
            if (animationSlowdown == 4)
            {
                animationSlowdown = 0;

                if (!inAir && sprite != swing && sprite != jumpSwing)
                {
                    sprite.nextFrame();
                }
            }

            if (animationSlowdown % 2 == 0)
            {
                if (weaponInAction)
                {
                    if (sprite == runSwing)
                    {
                        setSprite("swing");
                        sprite.setFrame(0);
                    }

                    if ((sprite == swing || sprite == jumpSwing))
                    {
                        if (sprite.currentFrame == sprite.frames - 1)
                        {
                            weaponInAction = false;

                            if (inAir)
                            {
                                setSprite("jump");
                            }
                            else
                            {
                                if (vx == 0)
                                    setSprite("stand");
                                else
                                    setSprite("run");
                            }
                        }

                        sprite.nextFrame();
                    }
                }
            }

            if (sprite == jump)
            {
                if (vy < -2)
                    sprite.setFrame(0);
                else if (vy > 2)
                    sprite.setFrame(2);
                else
                    sprite.setFrame(1);
            }
        }

        public void paint(Graphics g)
        {
            sprite.paint(g, face, 1);
        }

        public virtual void startSwing()
        {
            weaponInAction = true;
        }

        public abstract Projectile spawnProjectile();
    }
}

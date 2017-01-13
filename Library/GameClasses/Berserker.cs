using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections;

namespace MultiplayerLib.GameClasses
{
    public class Berserker:Player
    {
        public float Rage { get; set; }
        private bool inRage;

        public Berserker(float x, float y, int team, int face, string name, int id)
            : base(120, 3.5f, 6.7f, 6, team, face, name, id)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            stand = new Sprite(0, 0, SpriteIndex.ber_Stand, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/berserker_standing.png"), 50, 2);
            stand.setCollisionBox(19, 9, 12, 26);
            run = new Sprite(0, 0, SpriteIndex.ber_Run, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/berserker_run.png"), 50, 2);
            run.setCollisionBox(19, 9, 12, 26);
            jump = new Sprite(0, 0, SpriteIndex.ber_Jump, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/berserker_jump.png"), 50, 2);
            jump.setCollisionBox(19, 9, 12, 26);
            swing = new Sprite(0, 0, SpriteIndex.ber_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/berserker_swing.png"), 50, 2);
            swing.setCollisionBox(19, 9, 12, 26);
            jumpSwing = new Sprite(0, 0, SpriteIndex.ber_Jump_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/berserker_jump_swing.png"), 50, 2);
            jumpSwing.setCollisionBox(19, 9, 12, 26);
            runSwing = new Sprite(0, 0, SpriteIndex.ber_Run_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/berserker_run_swing.png"), 50, 2);
            runSwing.setCollisionBox(19, 9, 12, 26);

            weaponChargeLimit = 30;
            Rage = 0;
            inRage = false;
            stunnable = false;
            className = "Berserker";
            classAfterRespawn = className;

            sprite = stand;
            this.X = x;
            this.Y = y;
        }

        public bool InRage
        { get { return inRage; } }

        public void addRage(float dRage)
        {
            Rage += dRage;
            if (Rage >= weaponChargeLimit)
            {
                inRage = true;
                Rage = weaponChargeLimit;
            }
        }

        protected override void applyDamage(float damage, bool causeStun, int ID)
        {
            addRage(damage / 25);
            base.applyDamage(damage, causeStun, ID);
        }

        protected override void doSpriteStuff()
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

            if (animationSlowdown % (inRage ? 2 : 4) == 0)
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

        protected override void doCountersStuff()
        {
            if (!Dead)
            {
                if (inRage)
                {
                    Rage -= 0.05f;
                    if (Rage <= 0)
                    {
                        Rage = 0;
                        inRage = false;
                    }
                }
            }

            base.doCountersStuff();
        }

        protected override void doChargeTick()
        {
            if (weaponReadyToAction)
            {
                if ((weaponCharge + Rage) < weaponChargeLimit)
                    weaponCharge++;
            }
        }

        protected override void doHealTick()
        {
            if (healCooldown == 0 && Health < healthMax)
                Health += (inRage ? 0.16f : 0.08f) * (king ? 1.5f : 1f);
        }

        public override void move(ArrayList solidObjects)
        {
            if (vx != 0)
            {
                if (inRage)
                    vx = movingSpeedX * 1.2f * face;
                else
                    vx = movingSpeedX * face;
            }

            base.move(solidObjects);
        }

        public override void startSwing()
        {
            base.startSwing();
            weaponStrikeDelay = inRage ? 7 : 13;
        }

        public override Projectile spawnProjectile()
        {
            int damage = king ? (int)(hitDamage * 1.5f) : hitDamage;

            weaponCooldown = inRage ? 9 : 14;
            weaponReadyToAction = false;

            int shiftX = 0;
            float chargePercent = inRage ? 1 : (weaponCharge + Rage) / (float)weaponChargeLimit;

            if (face == 1)
                shiftX = 20;
            else
                shiftX = -7;

            Projectile pr = new AxeSlash(X + shiftX * 2, Y + 7, face, 10 + (int)(damage * chargePercent), Team, ID, 3);
            weaponCharge = 0;

            return pr;
        }

        public override void respawn(float x, float y)
        {
            inRage = false;
            Rage = 0;
            base.respawn(x, y);
        }
    }
}

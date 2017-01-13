using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections;
using MultiplayerLib.Projectiles;

namespace MultiplayerLib.GameClasses
{
    public class Arbalester : Player
    {
        private Sprite reload;
        private Sprite smack;
        private Sprite runSmack;
        private Sprite jumpSmack;
        public bool weaponReloading { get; set; }

        public Arbalester(float x, float y, int team, int face, string name, int id)
            : base(105, 4.2f, 7.1f, 70, team, face, name, id)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            stand = new Sprite(0, 0, SpriteIndex.arb_Stand, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/arbalester_standing.png"), 50, 2);
            stand.setCollisionBox(19, 9, 12, 26);
            run = new Sprite(0, 0, SpriteIndex.arb_Run, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/arbalester_run.png"), 50, 2);
            run.setCollisionBox(19, 9, 12, 26);
            jump = new Sprite(0, 0, SpriteIndex.arb_Jump, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/arbalester_jump.png"), 50, 2);
            jump.setCollisionBox(19, 9, 12, 26);
            swing = new Sprite(0, 0, SpriteIndex.arb_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/arbalester_swing.png"), 50, 2);
            swing.setCollisionBox(19, 9, 12, 26);
            jumpSwing = new Sprite(0, 0, SpriteIndex.arb_Jump_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/arbalester_jump_swing.png"), 50, 2);
            jumpSwing.setCollisionBox(19, 9, 12, 26);
            runSwing = new Sprite(0, 0, SpriteIndex.arb_Run_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/arbalester_run_swing.png"), 50, 2);
            runSwing.setCollisionBox(19, 9, 12, 26);
            smack = new Sprite(0, 0, SpriteIndex.arb_Smack, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/arbalester_smack.png"), 50, 2);
            smack.setCollisionBox(19, 9, 12, 26);
            jumpSmack = new Sprite(0, 0, SpriteIndex.arb_Jump_Smack, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/arbalester_jump_smack.png"), 50, 2);
            jumpSmack.setCollisionBox(19, 9, 12, 26);
            runSmack = new Sprite(0, 0, SpriteIndex.arb_Run_Smack, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/arbalester_run_smack.png"), 50, 2);
            runSmack.setCollisionBox(19, 9, 12, 26);
            reload = new Sprite(0, 0, SpriteIndex.arb_Reload, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/arbalester_reload.png"), 50, 2);
            reload.setCollisionBox(19, 9, 12, 26);

            weaponChargeLimit = 45;
            weaponCharge = weaponChargeLimit;
            className = "Arbalester";
            classAfterRespawn = className;
            weaponReloading = false;

            sprite = stand;
            this.X = x;
            this.Y = y;
        }

        public override void respawn(float x, float y)
        {
            breakReload();
            base.respawn(x, y);
            weaponCharge = weaponChargeLimit;
        }

        public override void setSprite(string spriteName)
        {
            float x = X;
            float y = Y;

            if (spriteName.CompareTo("reload") == 0)
            {
                if (sprite != reload)
                {
                    sprite = reload;
                    if (weaponCharge > 0)
                        sprite.setFrame((int)(sprite.frames * 0.65f));
                    else
                        sprite.setFrame(0);
                }
            }
            else if (spriteName.CompareTo("swing") == 0)
            {
                int frame = 0;
                if (weaponCharge != weaponChargeLimit && sprite == jumpSmack)
                    frame = sprite.getCurrentFrame();

                sprite = weaponCharge == weaponChargeLimit ? swing : smack;
                sprite.setFrame(frame);
            }
            else if (spriteName.CompareTo("jumpSwing") == 0)
            {
                int frame = 0;
                if (sprite == smack)
                    frame = sprite.getCurrentFrame();
                
                sprite = weaponCharge == weaponChargeLimit ? jumpSwing : jumpSmack;
                sprite.setFrame(frame);
            }
            else if (spriteName.CompareTo("runSwing") == 0)
                sprite = weaponCharge == weaponChargeLimit ? runSwing : runSmack;

            else
                base.setSprite(spriteName);

            X = x;
            Y = y;
        }

        public void breakReload()
        {
            weaponReloading = false;
            if (vx == 0)
                setSprite("stand");
            else
                setSprite("run");
            
            if (weaponCharge < weaponChargeLimit)
            {
                if (weaponCharge < (int)(0.75f * weaponChargeLimit))
                    weaponCharge = 0;
                else
                    weaponCharge = (int)(0.75f * weaponChargeLimit);
            }
        }

        public void startReload()
        {
            if (vx == 0 && !inAir)
            {
                if (!weaponReloading)
                {
                    weaponReloading = true;
                    setSprite("reload");
                }
            }
        }

        protected override void doChargeTick()
        {
            if (weaponReloading)
            {
                if (weaponCharge < weaponChargeLimit)
                    weaponCharge++;
                else
                    breakReload();
            }
        }

        protected override void doSpriteStuff()
        {
            animationSlowdown++;
            if (animationSlowdown == 8)
            {
                animationSlowdown = 0;

                if (sprite == reload)
                {
                    sprite.nextFrame();

                    if (sprite.getCurrentFrame() == 0)
                        setSprite("stand");

                }
            }

            if (animationSlowdown % 4 == 0)
            {
                if (!inAir && sprite != swing && sprite != jumpSwing && sprite != reload && sprite != smack && sprite != jumpSmack)
                {
                    sprite.nextFrame();
                }
            }
            else if (animationSlowdown % 2 == 0)
            {
                if (weaponInAction)
                {
                    if (sprite == runSwing || sprite == runSmack)
                    {
                        setSprite("swing");
                    }

                    if (sprite == swing || sprite == jumpSwing || sprite == smack || sprite == jumpSmack)
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

        public override void startSwing()
        {
            base.startSwing();
            weaponStrikeDelay = weaponCharge == weaponChargeLimit ? 2 : 6;
        }

        public override Projectile spawnProjectile()
        {
            int damage = king ? (int)(hitDamage * 1.5f) : hitDamage;

            weaponCooldown = weaponCharge == weaponChargeLimit ? 12 : 16;
            weaponReadyToAction = false;

            int shiftX = 0;
            float chargePercent = weaponCharge / (float)weaponChargeLimit;

            if (face == 1)
                shiftX = weaponCharge == weaponChargeLimit ? 20 : 34;
            else
                shiftX = weaponCharge == weaponChargeLimit ? 16 : 6;

            Projectile pr = null;
            if (weaponCharge == weaponChargeLimit)
                pr = new Bolt(X + shiftX * 2, Y + 40, face, (int)(damage * chargePercent), Team, ID, 300, (int)(35 * chargePercent) * face, 0, 0.2f);
            else
                pr = new ArbalestSmash(X + shiftX * 2, Y + 10, face, damage / 11, Team, ID, 2);

            if (weaponCharge == weaponChargeLimit)
                weaponCharge = 0;

            return pr;
        }
    }
}

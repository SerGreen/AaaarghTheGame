using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Collections;
using MultiplayerLib.Projectiles;
using MultiplayerLib.Particles;

namespace MultiplayerLib.GameClasses
{
    public class Thief : Player
    {
        private Sprite throwing;
        private Sprite runThrowing;
        private Sprite jumpThrowing;
        public bool IsThrowing { get; set; }
        private ArrayList myKnives;
        public int knivesLeft { get; set; }

        public Thief(float x, float y, int team, int face, string name, int id)
            : base(80, 4.6f, 7.8f, 5, team, face, name, id)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            stand = new Sprite(0, 0, SpriteIndex.thi_Stand, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/thief_standing.png"), 50, 2);
            stand.setCollisionBox(19, 9, 12, 26);
            run = new Sprite(0, 0, SpriteIndex.thi_Run, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/thief_run.png"), 50, 2);
            run.setCollisionBox(19, 9, 12, 26);
            jump = new Sprite(0, 0, SpriteIndex.thi_Jump, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/thief_jump.png"), 50, 2);
            jump.setCollisionBox(19, 9, 12, 26);
            swing = new Sprite(0, 0, SpriteIndex.thi_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/thief_swing.png"), 50, 2);
            swing.setCollisionBox(19, 9, 12, 26);
            jumpSwing = new Sprite(0, 0, SpriteIndex.thi_Jump_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/thief_jump_swing.png"), 50, 2);
            jumpSwing.setCollisionBox(19, 9, 12, 26);
            runSwing = new Sprite(0, 0, SpriteIndex.thi_Run_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/thief_run_swing.png"), 50, 2);
            runSwing.setCollisionBox(19, 9, 12, 26);
            throwing = new Sprite(0, 0, SpriteIndex.thi_Throw, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/thief_throw.png"), 50, 2);
            throwing.setCollisionBox(19, 9, 12, 26);
            jumpThrowing = new Sprite(0, 0, SpriteIndex.thi_Jump_Throw, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/thief_jump_throw.png"), 50, 2);
            jumpThrowing.setCollisionBox(19, 9, 12, 26);
            runThrowing = new Sprite(0, 0, SpriteIndex.thi_Run_Throw, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/thief_run_throw.png"), 50, 2);
            runThrowing.setCollisionBox(19, 9, 12, 26);

            weaponChargeLimit = 8;
            className = "Thief";
            classAfterRespawn = className;
            IsThrowing = false;
            knivesLeft = 3;
            myKnives = new ArrayList();

            sprite = stand;
            this.X = x;
            this.Y = y;
        }

        public override void setSprite(string spriteName)
        {
            float x = X;
            float y = Y;

            if (spriteName.CompareTo("run") == 0)
                sprite = run;

            else if (spriteName.CompareTo("swing") == 0)
            {
                int frame = 0;
                if (sprite == (IsThrowing ? jumpThrowing : jumpSwing))
                    frame = sprite.getCurrentFrame();

                sprite = IsThrowing ? throwing : swing;
                sprite.setFrame(frame);
            }

            else if (spriteName.CompareTo("stand") == 0)
                sprite = stand;

            else if (spriteName.CompareTo("jump") == 0)
                sprite = jump;

            else if (spriteName.CompareTo("jumpSwing") == 0)
            {
                int frame = 0;
                if (sprite == (IsThrowing ? throwing : swing))
                    frame = sprite.getCurrentFrame();

                sprite = IsThrowing ? jumpThrowing : jumpSwing;
                sprite.setFrame(frame);
            }

            else if (spriteName.CompareTo("runSwing") == 0)
                sprite = IsThrowing ? runThrowing : runSwing;

            X = x;
            Y = y;
        }

        protected override void doSpriteStuff()
        {
            animationSlowdown++;
            if (animationSlowdown == 4)
            {
                animationSlowdown = 0;

                if (!inAir && sprite != swing && sprite != jumpSwing && sprite != throwing && sprite != jumpThrowing)
                {
                    sprite.nextFrame();
                }
            }

            if (animationSlowdown % 2 == 0)
            {
                if (weaponInAction)
                {
                    if (sprite == runSwing || sprite == runThrowing)
                    {
                        setSprite("swing");
                        sprite.setFrame(0);
                    }

                    if (sprite == swing || sprite == jumpSwing || sprite == throwing || sprite == jumpThrowing)
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
            weaponStrikeDelay = IsThrowing ? 4 : 3;
        }

        public override Projectile spawnProjectile()
        {
            int damage = king ? (int)(hitDamage * 1.5f) : hitDamage;

            weaponCooldown = IsThrowing ? 8 : 5;
            weaponReadyToAction = false;

            int shiftX = 0;
            float chargePercent = weaponCharge / (float)weaponChargeLimit;

            if (face == 1)
                shiftX = 30;
            else
                shiftX = 4;

            int shiftY = IsThrowing ? 8 : 36;

            Projectile pr = null;
            if (IsThrowing)
            {
                pr = new Knife(X + shiftX * 2, Y + shiftY, face, 20 + (int)(3 * damage * chargePercent), Team, ID, 5400, (8 + (int)(7 * chargePercent)) * face, -1 - (int)(2 * chargePercent), 0.5f);
                knivesLeft--;
                myKnives.Add(pr);
            }
            else
                pr = new DaggerSlash(X + shiftX * 2, Y + shiftY, face, 4 + (int)(damage * chargePercent), Team, ID, 2);
            
            weaponCharge = 0;
            IsThrowing = false;

            return pr;
        }

        protected override void doStuffForDisarmedProjectile(MultiplayerLib.Projectile pr)
        {
            if (pr is Knife)
            {
                if (sprite.checkCollision(pr.getSprite))
                {
                    for (int i = myKnives.Count - 1; i >= 0; i--)
                    {
                        if (pr == myKnives[i])
                        {
                            pr.destroy();
                            myKnives.RemoveAt(i);
                            knivesLeft++;
                            break;
                        }
                    }
                }
            }
        }

        public override void respawn(float x, float y, ref ArrayList projectiles)
        {
            foreach (Knife myK in myKnives)
                foreach (Projectile pr in projectiles)
                    if (pr == myK)
                        pr.destroy();

            knivesLeft = 3;
            myKnives = new ArrayList();

            base.respawn(x, y);
        }

        public void doWallJumpIfCan(ArrayList solidObjects)
        {
            int wallJump = canWallJump(solidObjects);

            if (wallJump != 0)
            {
                vy -= jumpSpeedY / 2.5f;
                vx = movingSpeedX * wallJump;
                face = wallJump;
            }
        }

        private int canWallJump(ArrayList solidObjects)     //returns 1 if can jump right, -1 if can jump left and 0 if can't wall jump
        {
            if (inAir)
            {
                foreach (SolidObject so in solidObjects)
                {
                    if (!so.semiSolid)
                    {
                        if (sprite.checkCollision(so.box, -movingSpeedX, 0))
                            return 1;
                        else if (sprite.checkCollision(so.box, movingSpeedX, 0))
                            return -1;
                    }
                }
            }

            return 0;
        }
    }
}

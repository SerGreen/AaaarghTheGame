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
    public class Bowman : Player
    {
        public bool poisonedShoot { get; set; }
        public int arrowsLeft { get; set; }

        public Bowman(float x, float y, int team, int face, string name, int id)
            : base(80, 4.2f, 7.4f, 43, team, face, name, id)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            stand = new Sprite(0, 0, SpriteIndex.bow_Stand, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/bowman_standing.png"), 50, 2);
            stand.setCollisionBox(19, 9, 12, 26);
            run = new Sprite(0, 0, SpriteIndex.bow_Run, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/bowman_run.png"), 50, 2);
            run.setCollisionBox(19, 9, 12, 26);
            jump = new Sprite(0, 0, SpriteIndex.bow_Jump, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/bowman_jump.png"), 50, 2);
            jump.setCollisionBox(19, 9, 12, 26);
            swing = new Sprite(0, 0, SpriteIndex.bow_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/bowman_swing.png"), 50, 2);
            swing.setCollisionBox(19, 9, 12, 26);
            jumpSwing = new Sprite(0, 0, SpriteIndex.bow_Jump_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/bowman_jump_swing.png"), 50, 2);
            jumpSwing.setCollisionBox(19, 9, 12, 26);
            runSwing = new Sprite(0, 0, SpriteIndex.bow_Run_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/bowman_run_swing.png"), 50, 2);
            runSwing.setCollisionBox(19, 9, 12, 26);

            weaponChargeLimit = 40;
            poisonedShoot = false;
            arrowsLeft = 3;
            className = "Bowman";
            classAfterRespawn = className;

            sprite = stand;
            this.X = x;
            this.Y = y;
        }

        public override void move(ArrayList solidObjects)
        {
            if (vx != 0)
            {
                if (weaponReadyToAction)
                {
                    if (!inAir)
                        vx = movingSpeedX * 0.55f * face;
                    else
                        vx = movingSpeedX * 0.8f * face;
                }
                else
                    vx = movingSpeedX * face;
            }

            base.move(solidObjects);
        }

        public override void startSwing()
        {
            base.startSwing();
            weaponStrikeDelay = 1;
        }

        public override Projectile spawnProjectile()
        {
            int damage = king ? (int)(hitDamage * 1.5f) : hitDamage;

            weaponCooldown = 19;
            weaponReadyToAction = false;

            int shiftX = 0;
            float chargePercent = weaponCharge / (float)weaponChargeLimit;

            if (face == 1)
                shiftX = 20;
            else
                shiftX = 9;

            Projectile pr = null;
            if (poisonedShoot)
            {
                pr = new PoisonedArrow(X + shiftX * 2, Y + 36, face, (int)(damage * chargePercent), Team, ID, 300, (5 + (int)(25 * chargePercent)) * face, 0, 0.2f);
                arrowsLeft--;
                poisonedShoot = false;
            }
            else
                pr = new Arrow(X + shiftX * 2, Y + 36, face, (int)(damage * chargePercent), Team, ID, 300, (5 + (int)(25 * chargePercent)) * face, 0, 0.2f);
            
            weaponCharge = 0;
            return pr;
        }

        public override void respawn(float x, float y)
        {
            poisonedShoot = false;
            arrowsLeft = 3;
            base.respawn(x, y);
        }
    }
}

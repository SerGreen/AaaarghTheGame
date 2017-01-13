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
    public class Warrior : Player
    {
        public float Dash { get; set; }
        private int dashMax;
        public bool InDash { get; set; }

        public Warrior(float x, float y, int team, int face, string name, int id)
            : base(100, 4, 7, 5, team, face, name, id)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            stand = new Sprite(0, 0, SpriteIndex.war_Stand, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/warrior_standing.png"), 50, 2);
            stand.setCollisionBox(19, 9, 12, 26);
            run = new Sprite(0, 0, SpriteIndex.war_Run, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/warrior_run.png"), 50, 2);
            run.setCollisionBox(19, 9, 12, 26);
            jump = new Sprite(0, 0, SpriteIndex.war_Jump, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/warrior_jump.png"), 50, 2);
            jump.setCollisionBox(19, 9, 12, 26);
            swing = new Sprite(0, 0, SpriteIndex.war_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/warrior_swing.png"), 50, 2);
            swing.setCollisionBox(19, 9, 12, 26);
            jumpSwing = new Sprite(0, 0, SpriteIndex.war_Jump_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/warrior_jump_swing.png"), 50, 2);
            jumpSwing.setCollisionBox(19, 9, 12, 26);
            runSwing = new Sprite(0, 0, SpriteIndex.war_Run_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/warrior_run_swing.png"), 50, 2);
            runSwing.setCollisionBox(19, 9, 12, 26);

            weaponChargeLimit = 15;
            dashMax = 200;
            Dash = dashMax;
            InDash = false;
            className = "Warrior";
            classAfterRespawn = className;

            sprite = stand;
            this.X = x;
            this.Y = y;
        }

        public int DashMax
        { get { return dashMax; } }

        public override void startSwing()
        {
            base.startSwing();
            weaponStrikeDelay = 5;
        }

        public override Projectile spawnProjectile()
        {
            int damage = king ? (int)(hitDamage * 1.5f) : hitDamage;

            weaponCooldown = 8;
            weaponReadyToAction = false;

            int shiftX = 0;
            float chargePercent = weaponCharge / (float)weaponChargeLimit;

            if (face == 1)
                shiftX = 30;
            else
                shiftX = -1;

            Projectile pr = new SwordSlash(X + shiftX * 2, Y + 5, face, 6 + (int)(damage * chargePercent), Team, ID, 3);
            weaponCharge = 0;

            return pr;
        }

        protected override void doCountersStuff()
        {
            if (InDash)
            {
                Dash -= 30;
                if (Dash <= 0)
                {
                    Dash = 0;
                    stopDash();
                }
            }
            else
            {
                if (Dash < dashMax)
                {
                    if (running)
                        Dash += 0.25f;
                    else
                        Dash += 1;

                    if (Dash >= dashMax)
                        Dash = dashMax;
                }
            }

            base.doCountersStuff();
        }

        public override void move(ArrayList solidObjects)
        {
            if (InDash)
                vx = movingSpeedX * 4 * face;
            else
            {
                if (vx != 0)
                    vx = movingSpeedX * face;
            }

            base.move(solidObjects);
        }

        public void stopDash()
        {
            if (InDash)
            {
                InDash = false;
                if (running)
                    vx = movingSpeedX * face;
                else
                    vx = 0;
            }
        }

        public override void respawn(float x, float y)
        {
            InDash = false;
            Dash = dashMax;
            base.respawn(x, y);
        }
    }
}

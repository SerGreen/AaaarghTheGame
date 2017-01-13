using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;
using MultiplayerLib.Projectiles;
using MultiplayerLib.Particles;

namespace MultiplayerLib.GameClasses
{
    public class Mage : Player
    {
        public float Mana { get; set; }
        private int manaMax;

        public Mage(float x, float y, int team, int face, string name, int id)
            : base(71, 3.4f, 6.7f, 48, team, face, name, id)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            stand = new Sprite(0, 0, SpriteIndex.mag_Stand, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/mage_standing.png"), 50, 2);
            stand.setCollisionBox(19, 9, 12, 26);
            run = new Sprite(0, 0, SpriteIndex.mag_Run, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/mage_run.png"), 50, 2);
            run.setCollisionBox(19, 9, 12, 26);
            jump = new Sprite(0, 0, SpriteIndex.mag_Jump, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/mage_jump.png"), 50, 2);
            jump.setCollisionBox(19, 9, 12, 26);
            swing = new Sprite(0, 0, SpriteIndex.mag_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/mage_swing.png"), 50, 2);
            swing.setCollisionBox(19, 9, 12, 26);
            jumpSwing = new Sprite(0, 0, SpriteIndex.mag_Jump_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/mage_jump_swing.png"), 50, 2);
            jumpSwing.setCollisionBox(19, 9, 12, 26);
            runSwing = new Sprite(0, 0, SpriteIndex.mag_Run_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/mage_run_swing.png"), 50, 2);
            runSwing.setCollisionBox(19, 9, 12, 26);

            weaponChargeLimit = 45;
            manaMax = 200;
            Mana = manaMax;            
            className = "Mage";
            classAfterRespawn = className;

            sprite = stand;
            this.X = x;
            this.Y = y;
        }

        public int ManaMax
        { get { return manaMax; } }

        public override void startSwing()
        {
            base.startSwing();
            weaponStrikeDelay = 2;
        }

        public override Projectile spawnProjectile()
        {
            int damage = king ? (int)(hitDamage * 1.5f) : hitDamage;

            weaponCooldown = 13;
            weaponReadyToAction = false;

            int shiftX = 0;
            float chargePercent = weaponCharge / (float)weaponChargeLimit;

            if (chargePercent > 0.25)
            {
                if (face == 1)
                    shiftX = 20;
                else
                    shiftX = 9;

                Projectile pr = new Fireball(X + shiftX * 2, Y + 25 - 5 * chargePercent, face, (int)(damage * chargePercent), Team, ID, 300, (10 + (int)(5 * chargePercent)) * face, 0, 2 * chargePercent);
                weaponCharge = 0;

                return pr;
            }
            else
            {
                Mana += weaponCharge;
                weaponCharge = 0;
            }

            return null;
        }

        public Particle suicide()
        {
            applyDamage(999, false, -1);
            return new Explosion(X + Width * sprite.Scale / 2, Y + Height * sprite.Scale, face);
        }

        protected override void doChargeTick()
        {
            if (weaponReadyToAction)
            {
                if (weaponCharge < weaponChargeLimit && Mana > 0)
                {
                    weaponCharge++;
                    Mana--;
                }
            }
            else
            {
                if (Mana < manaMax)
                {
                    if (running)
                        Mana += 0.4f;
                    else
                        Mana += 1f;

                    if (Mana >= manaMax)
                        Mana = manaMax;
                }
            }
        }

        public override void respawn(float x, float y)
        {
            Mana = manaMax;
            base.respawn(x, y);
        }
    }
}

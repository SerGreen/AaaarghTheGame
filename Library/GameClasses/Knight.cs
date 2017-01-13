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
    public class Knight : Player
    {
        public Knight(float x, float y, int team, int face, string name, int id)
            : base(130, 3.4f, 6.7f, 5, team, face, name, id)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            stand = new Sprite(0, 0, SpriteIndex.kni_Stand, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/knight_standing.png"), 56, 2);
            stand.setCollisionBox(22, 14, 12, 26);
            run = new Sprite(0, 0, SpriteIndex.kni_Run, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/knight_run.png"), 56, 2);
            run.setCollisionBox(22, 14, 12, 26);
            jump = new Sprite(0, 0, SpriteIndex.kni_Jump, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/knight_jump.png"), 56, 2);
            jump.setCollisionBox(22, 14, 12, 26);
            swing = new Sprite(0, 0, SpriteIndex.kni_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/knight_swing.png"), 56, 2);
            swing.setCollisionBox(22, 14, 12, 26);
            jumpSwing = new Sprite(0, 0, SpriteIndex.kni_Jump_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/knight_jump_swing.png"), 56, 2);
            jumpSwing.setCollisionBox(22, 14, 12, 26);
            runSwing = new Sprite(0, 0, SpriteIndex.kni_Run_Swing, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/knight_run_swing.png"), 56, 2);
            runSwing.setCollisionBox(22, 14, 12, 26);

            weaponChargeLimit = 35;
            className = "Knight";
            classAfterRespawn = className;

            sprite = stand;
            this.X = x;
            this.Y = y;
        }

        public override void startSwing()
        {
            base.startSwing();
            weaponStrikeDelay = 7;
        }

        public override Projectile spawnProjectile()
        {
            int damage = king ? (int)(hitDamage * 1.5f) : hitDamage;

            weaponCooldown = 18;
            weaponReadyToAction = false;

            int shiftX = 0;
            float chargePercent = weaponCharge / (float)weaponChargeLimit;

            if (face == 1)
                shiftX = 23;
            else
                shiftX = -4;

            Projectile pr = new AxeSlash(X + shiftX * 2, Y + 12, face, 5 + (int)(damage * chargePercent), Team, ID, 3);
            weaponCharge = 0;

            return pr;
        }
    }
}

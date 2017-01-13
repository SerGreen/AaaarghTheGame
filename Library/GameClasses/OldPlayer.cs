using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace MultiplayerLib
{
    //enum State { Stand, Run, Jump, Swing, JumpSwing };

    public enum GameClass { Warrior, Berserker, Bowman, Arbalester }

    public class VeryOldPlayer : MarshalByRefObject
    {
        private string[] gameClassPath = new string[] { "warrior", "berserker", "bowman", "arbalester" };

        private int id;
        //public float x;
        //public float y;
        private Sprite sprite;     //NEVER! NEVER EVER USE DIRECT ASSIGN! ONLY SetSprite("sprite_name")!!!!
        Sprite run;
        Sprite stand;
        Sprite swing;
        Sprite jump;
        Sprite jumpSwing;
        Sprite runSwing;
        public float vx { set; get; }
        public float vy { set; get; }
        public float movingSpeedX;
        public float jumpSpeedY;
        float gravity;
        public GameClass gameClass;
        private int team;
        public int lastHitByPlayer;
        public int Frags { get; set; }
        public int Deaths { get; set; }
        public float Health { get; set; }
        public float healthMax;
        public int healCooldown;
        public int weaponCooldown;
        public int weaponStrikeDelay;
        public int weaponCharge;
        public int weaponChargeLimit { get; set; }
        public bool weaponStrikeOnRelease;
        public bool weaponReadyToAction;
        public bool weaponInAction;
        private string name;
        public bool inAir { get; set; }
        public int face { get; set; }
        //public State state;
        int slow = 0;
        //bool beforeJumpStand = true;    //false = run

        public VeryOldPlayer(float x, float y, int health, float speedX, float jumpY, int team, int face, string name, GameClass gameClass, int id)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            int spriteIndexShift = 4;

            stand = new Sprite(0, 0, (SpriteIndex)(spriteIndexShift + (int)gameClass * 6 + 0), (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/" + gameClassPath[(int)gameClass] + "_standing.png"), 50, 2);
            stand.setCollisionBox(19, 9, 12, 26);
            run = new Sprite(0, 0, (SpriteIndex)(spriteIndexShift + (int)gameClass * 6 + 1), (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/" + gameClassPath[(int)gameClass] + "_run.png"), 50, 2);
            run.setCollisionBox(19, 9, 12, 26);
            jump = new Sprite(0, 0, (SpriteIndex)(spriteIndexShift + (int)gameClass * 6 + 2), (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/" + gameClassPath[(int)gameClass] + "_jump.png"), 50, 2);
            jump.setCollisionBox(19, 9, 12, 26);
            swing = new Sprite(0, 0, (SpriteIndex)(spriteIndexShift + (int)gameClass * 6 + 3), (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/" + gameClassPath[(int)gameClass] + "_swing.png"), 50, 2);
            swing.setCollisionBox(19, 9, 12, 26);
            jumpSwing = new Sprite(0, 0, (SpriteIndex)(spriteIndexShift + (int)gameClass * 6 + 4), (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/" + gameClassPath[(int)gameClass] + "_jump_swing.png"), 50, 2);
            jumpSwing.setCollisionBox(19, 9, 12, 26);
            runSwing = new Sprite(0, 0, (SpriteIndex)(spriteIndexShift + (int)gameClass * 6 + 5), (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/" + gameClassPath[(int)gameClass] + "_run_swing.png"), 50, 2);
            runSwing.setCollisionBox(19, 9, 12, 26);

            sprite = stand;
            this.id = id;
            this.gameClass = gameClass;
            this.X = x;
            this.Y = y;
            movingSpeedX = speedX;
            jumpSpeedY = jumpY;
            this.face = face;
            this.team = team;
            lastHitByPlayer = -1;
            Frags = 0;
            Deaths = 0;
            vx = 0;
            vy = 0;
            gravity = 0;
            inAir = false;
            this.Health = health;
            this.healthMax = health;
            healCooldown = 0;
            this.name = name;
            weaponCooldown = 0;
            weaponCharge = 0;
            weaponStrikeDelay = -1;
            weaponStrikeOnRelease = true;
            weaponReadyToAction = false;
            weaponInAction = false;

            if (gameClass == GameClass.Bowman)
                weaponChargeLimit = 40;
            else if (gameClass == GameClass.Berserker)
                weaponChargeLimit = 25;
            else
                weaponChargeLimit = 15;
        }

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

        public Sprite getSprite()
        {
            return sprite;
        }

        public string Name
        { get { return name; } }

        public int ID
        { get { return id; } }

        public GameClass Class
        {  get { return gameClass; } }

        public int Team
        { get { return team; } }

        public void setSprite(String spriteName)
        {
            float x = X;
            float y = Y;

            if (spriteName.CompareTo("run") == 0)
                sprite = run;

            else if (spriteName.CompareTo("swing") == 0)
                sprite = swing;

            else if (spriteName.CompareTo("stand") == 0)
                sprite = stand;

            else if (spriteName.CompareTo("jump") == 0)
                sprite = jump;

            else if (spriteName.CompareTo("jumpSwing") == 0)
                sprite = jumpSwing;

            else if (spriteName.CompareTo("runSwing") == 0)
                sprite = runSwing;

            X = x;
            Y = y;
        }

        public void doJump()
        {
            if (sprite == swing)
            {
                swing.setFrame(0);
                setSprite("jumpSwing");
            }

            push(0, -1);
            vy -= jumpSpeedY;
        }

        public void doJumpDown()
        {
            if (sprite == swing)
            {
                swing.setFrame(0);
                setSprite("jumpSwing");
            }

            push(0, 1);
        }

        public void move(ArrayList solidObjects)
        {
            if (gameClass == GameClass.Bowman && vx != 0)
            {
                if (weaponReadyToAction )
                    if(!inAir)
                        vx = movingSpeedX * 0.55f * face;
                    else
                        vx = movingSpeedX * 0.8f * face;
                else
                    vx = movingSpeedX * face;
            }

            bool noCollisions = true;
            for (int i = 0; i < solidObjects.Count; i++)
            {
                SolidObject so = (SolidObject)solidObjects[i];

                if (sprite.checkCollision(so.box, vx, 0) == true && so.semiSolid == false)
                {
                    noCollisions = false;
                    break;
                }
            }

            if (noCollisions)
            {
                X += vx;
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
                        if (so.semiSolid == false ||                                                                //if obj. is solid
                           (so.semiSolid == true && vy >= 0 && sprite.checkCollision(so.box, 0, 0) == false))       //or it is semiSolid, while now there is no collisions with obj. and player moving down
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

        public void push(float dx, float dy)
        {
            X += dx;
            Y += dy;
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

        public void moveToObject(SolidObject so, int dx, int dy)
        {
            while (!sprite.checkCollision(so.box, dx, dy))
            {
                Y += dy;
                X += dx;
            }
        }

        public void tick(ArrayList solidObjects, ArrayList projectiles)
        {
            checkGroundUnderneath(solidObjects);
            move(solidObjects);
            checkProjectilesInsideOfSelf(projectiles);
            doSpriteStuff();
            doCountersStuff();
        }

        private void doCountersStuff()
        {
            if (weaponCooldown > 0)
                weaponCooldown--;

            if (weaponReadyToAction)
            {
                if (weaponCharge < weaponChargeLimit)
                    weaponCharge++;
            }

            if (weaponStrikeDelay >= 0)
                weaponStrikeDelay--;

            if (healCooldown > 0)
                healCooldown--;

            if (healCooldown == 0 && Health < healthMax)
                Health += 0.08f;
        }

        private void checkProjectilesInsideOfSelf(ArrayList projectiles)
        {
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                Projectile pr = (Projectile)projectiles[i];
                if (pr.activated && !pr.disarmed)
                {
                    if (sprite.checkCollision(pr.getSprite))
                    {
                        if (pr.team != team)
                        {
                            Health -= (int) pr.damage;
                            healCooldown = 200;
                            lastHitByPlayer = pr.ownerID;

                            if (pr is Arrow || pr is Bolt)
                            {
                                //projectiles.RemoveAt(i);
                                pr.destroy();
                            }
                        }
                    }
                }
            }
        }

        private void checkGroundUnderneath(ArrayList solidObjects)
        {
            bool noGroungUnderneath = true;
            for (int i = 0; i < solidObjects.Count; i++)
            {
                SolidObject so = (SolidObject)solidObjects[i];
                if (sprite.checkCollision(so.box, 0, 1) == true &&                           //if one bit below is obj.
                    (so.semiSolid == false ||                                                //and it is solid
                    so.semiSolid == true && sprite.checkCollision(so.box, 0, 0) == false))   //or it is semi solid but now there is no collision with it
                {                                                                            //then we have ground under foots
                    gravity = 0;
                    vy = 0;
                    noGroungUnderneath = false;

                    if (inAir)
                    {
                        inAir = false;

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
                }
            }

            if (noGroungUnderneath)
            {
                inAir = true;
                gravity = 0.5f;
                if (sprite != jump || sprite != jumpSwing)
                {
                    if (weaponReadyToAction)
                    {
                        setSprite("jumpSwing");
                        sprite.setFrame(0);
                    }
                    else
                        setSprite("jump");
                }
            }
        }

        private void doSpriteStuff()
        {
            slow++;
            if (slow == 4)
            {
                slow = 0;

                if (!inAir && sprite != swing && sprite != jumpSwing)
                {
                    sprite.nextFrame();
                }
            }

            if (slow % 2 == 0)
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

                        if(gameClass != GameClass.Berserker || (gameClass == GameClass.Berserker && slow % 4 == 0))
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
            //g.DrawRectangle(new Pen(Color.Red), sprite.getCollisionBox().X + x, sprite.getCollisionBox().Y + y, sprite.getCollisionBox().Width, sprite.getCollisionBox().Height);
        }
    }
}

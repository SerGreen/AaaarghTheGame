using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MultiplayerLib.Particles
{
    public abstract class Particle : MarshalByRefObject
    {
        protected Sprite sprite;
        protected int lifetime;
        protected float vx;
        protected float vy;
        protected int animationSlowdown;
        protected int animationSlowdownMax;
        public bool timeToDie { get; set; }
        public int face { get; set; }

        public Particle(float vx, float vy, int face, int lifetime)
        {
            sprite = null;
            this.vx = vx;
            this.vy = vy;
            this.lifetime = lifetime;
            this.face = face;
            timeToDie = false;
            animationSlowdownMax = 4;
        }

        public Particle(float vx, float vy, int face, int lifetime, int animationPeriod)
            : this(vx, vy, face, lifetime)
        {
            animationSlowdownMax = animationPeriod;
        }

        public Sprite getSprite()
        { return sprite; }

        public virtual void tick()
        {
            animationSlowdown++;
            if (animationSlowdown == 4)
            {
                animationSlowdown = 0;
                flipSpriteFrame();
            }

            move();

            lifetime--;
            if (lifetime < 0)
                timeToDie = true;
        }

        protected virtual void move()
        {
            sprite.x += vx;
            sprite.y += vy;
        }

        protected virtual void flipSpriteFrame()
        {
            sprite.nextFrame();
        }
    }
}

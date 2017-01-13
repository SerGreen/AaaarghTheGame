using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiplayerLib.Particles
{
    public abstract class LastFrameDieParticle : Particle
    {
        public LastFrameDieParticle(float vx, float vy, int face, int lifetime)
            : base(vx, vy, face, lifetime)
        { }

        public LastFrameDieParticle(float vx, float vy, int face, int lifetime, int animationPeriod)
            : base(vx, vy, face, lifetime, animationPeriod)
        { }

        protected override void flipSpriteFrame()
        {
            base.flipSpriteFrame();
            if (sprite.getCurrentFrame() == 0)
                timeToDie = true;
        }
    }
}

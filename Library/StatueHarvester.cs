using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;
using System.Collections;
using MultiplayerLib.Particles;

namespace MultiplayerLib
{
    public class StatueHarvester : MarshalByRefObject
    {
        private Sprite sprite;
        private Random rnd;

        public StatueHarvester(float x, float y)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Statue, (Bitmap)Bitmap.FromFile(assemblyFolder + "/../res/statue.png"), 32, 1);
            rnd = new Random();
        }

        public Sprite getSprite
        { get { return sprite; } }

        public Skull spawnSkull(int team, ref ArrayList particles)
        {
            float skullX = sprite.x + sprite.Width / 2;
            float skullY = sprite.y + sprite.Height / 2;

            particles.Add(new FireBurst(skullX, sprite.y + sprite.Height));
            return new Skull(skullX, skullY, (float)(rnd.NextDouble() * 8 - 4), (float)(rnd.NextDouble() * -5), team);
        }
    }
}

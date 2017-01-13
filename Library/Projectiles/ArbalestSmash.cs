using System.IO;
using System.Reflection;
using System.Drawing;

namespace MultiplayerLib.Projectiles
{
    class ArbalestSmash : SwordSlash
    {
        public ArbalestSmash(float x, float y, int face, int damage, int team, int ownerID, int lifetime)
            : base(x, y, face, damage, team, ownerID, lifetime)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Arbalest_Smash, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/arbalest_smash.png"), 11, 2);
            sprite.setCollisionBox(1, 4, 10, 17);
        }
    }
}

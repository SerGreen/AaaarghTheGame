using System.Reflection;
using System.IO;
using System.Drawing;

namespace MultiplayerLib
{
    public class AxeSlash : SwordSlash
    {
        public AxeSlash(float x, float y, int face, int damage, int team, int ownerID, int lifetime)
            : base(x, y, face, damage, team, ownerID, lifetime)
        {
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            sprite = new Sprite(x, y, SpriteIndex.Slash_Axe, (Bitmap)Bitmap.FromFile(assemblyFolder + "/res/slash_axe.png"), 37, 2);
            sprite.setCollisionBox(1, 4, 34, 20);
        }
    }
}

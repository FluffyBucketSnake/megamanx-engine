using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.Graphics
{
    public class SpriteSheet(Texture2D texture, SpriteFrame[] frames)
    {
        public Texture2D Texture => texture;
        public SpriteFrame[] Frames => frames;
        public Vector2 Origin { get; set; }

        public SpriteSheet(Texture2D texture, int frameCount) : this(texture, new SpriteFrame[frameCount]) { }

        public Vector2 GetOrigin(int index, SpriteEffects effects)
        {
            return Origin - Frames[index].GetOffset(effects);
        }
    }
}

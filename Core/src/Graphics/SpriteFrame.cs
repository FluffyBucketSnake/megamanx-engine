using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.Graphics
{
    public record struct SpriteFrame(Rectangle Source, Thickness Offset)
    {
        public SpriteFrame(Rectangle source) : this(source, Thickness.Zero) { }
        public SpriteFrame(int x, int y, int width, int height) : this(new Rectangle(x, y, width, height)) { }
        public SpriteFrame(int x, int y, int width, int height, Thickness offset) : this(new Rectangle(x, y, width, height), offset) { }

        public readonly Vector2 GetOffset(SpriteEffects effects)
        {
            float offsetX = (effects & SpriteEffects.FlipHorizontally) == 0 ? Offset.Left : Offset.Right;
            float offsetY = (effects & SpriteEffects.FlipVertically) == 0 ? Offset.Top : Offset.Bottom;

            return new Vector2(offsetX, offsetY);
        }
    }
}

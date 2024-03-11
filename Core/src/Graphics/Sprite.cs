using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.Graphics
{
    public class Sprite(SpriteSheet template)
    {
        public SpriteSheet Template { get => template; set => template = value; }
        public int FrameIndex { get; set; }
        public SpriteFrame CurrentFrame => Template.Frames[FrameIndex];
        public float CurrentTime { get; set; }
        public Vector2 Position { get; set; }
        public SpriteEffects Effects { get; set; }
        public float Depth { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Template.Texture, Position, CurrentFrame.Source, Color.White, 0.0f,
            Template.GetOrigin(FrameIndex, Effects), Vector2.One, Effects, Depth);
        }
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.Graphics
{
    public class SpriteSheet
    {
        public Texture2D Texture { get; }
        public SpriteFrame[] Frames { get; }
        public AnimationCollection Animations { get; } = new AnimationCollection();
        public Vector2 Origin { get; set; }

        public SpriteSheet(Texture2D texture, int frameCount)
        {
            Texture = texture ?? throw new System.ArgumentNullException(nameof(texture));
            Frames = new SpriteFrame[frameCount];
        }

        public Vector2 GetOrigin(int index, SpriteEffects effects)
        {
            return Origin - Frames[index].GetOffset(effects);
        }
    }
}
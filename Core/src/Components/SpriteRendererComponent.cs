using MegamanX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.Components
{
    public class SpriteRendererComponent(Entity entity, Sprite sprite) : IComponent
    {
        public Sprite Sprite { get; } = sprite;

        private readonly TransformComponent transform = entity.GetComponent<TransformComponent>();

        int? IComponent.DrawPriority => 50;
        void IComponent.Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Position = transform.Position;
            Sprite.Draw(spriteBatch);
        }
    }
}

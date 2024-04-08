using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX
{
    public interface IComponent
    {
        int? UpdatePriority => null;
        void Update(GameTime gameTime) { }

        int? PostUpdatePriority => null;
        void PostUpdate(GameTime gameTime) { }

        int? DrawPriority => null;
        void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}

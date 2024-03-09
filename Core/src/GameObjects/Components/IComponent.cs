using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.GameObjects.Components
{
    public interface IComponent
    {
        void Update(GameTime gameTime) { }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}

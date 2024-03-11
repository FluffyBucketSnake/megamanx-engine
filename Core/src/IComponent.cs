using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX
{
    public interface IComponent
    {
        void Update(GameTime gameTime) { }

        void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }
    }
}

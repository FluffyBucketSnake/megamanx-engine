using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.GameObjects
{
    public class BattonBone : Entity
    {
        public Texture2D Texture;

        public override void LoadContent(ContentManager content)
        {
            content.Load<Texture2D>("textures/enemy-battonbone");
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }
    }
}
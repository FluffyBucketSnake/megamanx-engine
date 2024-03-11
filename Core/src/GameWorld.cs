using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX
{
    public class GameWorld
    {
        private readonly List<Entity> entities = [];

        public IEnumerable<Entity> Entities => new ReadOnlyCollection<Entity>(entities);

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                entity.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                entity.Draw(gameTime, spriteBatch);
            }
        }
    }
}

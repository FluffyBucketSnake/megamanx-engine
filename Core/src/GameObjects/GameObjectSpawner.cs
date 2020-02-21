using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MegamanX.GameObjects
{
    public class GameObjectSpawner : GameObject
    {
        private ContentManager _content;

        public Type SpawnType = null;

        public GameObject Spawn;

        public GameObjectSpawner()
        {
            IsPersistent = true;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch) { }

        public override void Update(GameTime gameTime) { }

        public override void LoadContent(ContentManager content)
        {
            _content = content;
        }

        protected override void OnActivation()
        {
            if (Map != null && SpawnType != null && (Spawn == null || !Spawn.IsAlive))
            {
                Spawn = Activator.CreateInstance(SpawnType) as GameObject;
                Spawn.Position = Position;
                Spawn.LoadContent(_content);

                Map.Objects.Add(Spawn);
            }
        }
    }
}

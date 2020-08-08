using MegamanX.Data;
using MegamanX.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace MegamanX.GameStates
{
    public class PlayState : GameState
    {
        SpriteBatch spriteBatch;

        public GraphicsDevice GraphicsDevice { get; }
        public ContentManager Content { get; }
        public string MapFilePath { get; private set;}
        public GameWorld GameWorld { get; private set; }

        public PlayState(GraphicsDevice graphicsDevice, ContentManager content)
        {
            UpdateOrder = 0;
            DrawOrder = 1000;

            GraphicsDevice = graphicsDevice ?? throw new System.ArgumentNullException(nameof(graphicsDevice));
            Content = content ?? throw new System.ArgumentNullException(nameof(content));
        }

        public override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }
        
        public void LoadLevel(string path)
        {
            if (path == null)
            {
                throw new System.ArgumentNullException(nameof(path));
            }
            
            //Load level.
            MapFilePath = path;
            GameWorld = MapLoader.LoadFromTMX(MapFilePath, Content, Content.ServiceProvider);

            //Load entities content.
            foreach (var entity in GameWorld.Objects)
            {
                entity.LoadContent(Content);
            }

            //Change music.
            MediaPlayer.Stop();
            if (GameWorld.Music != null)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(GameWorld.Music);
            }
        }

        public void ReloadLevel()
        {
            if (MapFilePath == null)
            {
                return;
            }

            GameWorld = MapLoader.LoadFromTMX(MapFilePath, Content, Content.ServiceProvider);

            //Load entities content.
            foreach (var entity in GameWorld.Objects)
            {
                entity.LoadContent(Content);
            }

            //Change music.
            MediaPlayer.Stop();
            if (GameWorld.Music != null)
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Play(GameWorld.Music);
            }
        }

        public override void Update(GameTime gameTime)
        {
            GameWorld?.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GameWorld?.Draw(gameTime, spriteBatch);
        }
    }
}
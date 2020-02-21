using MegamanX.Input.Keyboard;
using MegamanX.Data;
using MegamanX.GameObjects;
using MegamanX.GameObjects.Debug;
using MegamanX.GameObjects.Playable;
using MegamanX.GameStates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MegamanX
{
    public class MegamanX : Game
    {
        KeyboardComponent keyboard;
        GraphicsDeviceManager graphics;

        public MegamanX()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 512;
            graphics.PreferredBackBufferHeight = 448;

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = true;
        }

        protected override void Initialize()
        {
            //Initialize keyboard input.
            keyboard = new KeyboardComponent(this);
            Components.Add(keyboard);
            
            //Call base class's initialization.
            base.Initialize();
        }

        protected override void LoadContent()
        {
            //Create play state.
            var playState = new PlayState(GraphicsDevice, Content);
            playState.Initialize();
            playState.LoadLevel("Maps/debug0.tmx");

            keyboard.KeyDown += e => {
                if (e.Key == Keys.F2)
                {
                    playState.ReloadLevel();
                }
            };

            //screen.DebugProfilers.RegisterProfiler(typeof(Entity), new EntityDebugProfiler());
            //screen.DebugProfilers.RegisterProfiler(typeof(Player), new PlayerDebugProfiler());

            playState.Enabled = true;
            playState.Visible = true;

            //Load it into the manager.
            GameStateManager.AddState(playState);
        }

        protected override void Update(GameTime gameTime)
        {
            //Use default exit input from the Monogame template.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Call base class's Update method.
            base.Update(gameTime);

            //Render the screens.
            GameStateManager.UpdateStates(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //Clear screen.
            GraphicsDevice.Clear(Color.Black);

            //Render the screens.
            GameStateManager.DrawStates(gameTime);

            //Call base class's Draw method.
            base.Draw(gameTime);
        }
    }
}

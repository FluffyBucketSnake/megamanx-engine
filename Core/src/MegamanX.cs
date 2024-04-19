using MegamanX.Components;
using MegamanX.GameStates;
using MegamanX.Graphics;
using MegamanX.Input.Keyboard;
using MegamanX.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MegamanX
{
    public class MegamanX : Game
    {
        private readonly KeyboardComponent keyboard;
        private readonly GraphicsDeviceManager graphics;

        public MegamanX()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 512,
                PreferredBackBufferHeight = 448
            };

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
            IsFixedTimeStep = true;

            keyboard = new KeyboardComponent(this);
            Components.Add(keyboard);
        }

        protected override void LoadContent()
        {
            Texture2DPrefabs.Initialize(GraphicsDevice);

            TileMap tileMap = BuildBoxTileMap(Content);

            GameWorld gameWorld = new(tileMap);

            Entity camera = new();
            camera.AddComponent(new TransformComponent(new Vector2(128, 114)));
            camera.AddComponent(new CameraComponent(gameWorld, camera));
            gameWorld.AddEntity(camera);

            PlayerContent playerContent = PlayerContent.LoadDefault(Content);
            Sprite playerSprite = playerContent.CreateSprite();
            Entity player = new();
            player.AddComponent(new TransformComponent(new Vector2(128, 114)));
            player.AddComponent(new SpriteRendererComponent(player, playerSprite));
            player.AddComponent(new SpriteAnimatorComponent(playerSprite, playerContent.animations));
            player.AddComponent(new LivingComponent(player, 20, 20));
            player.AddComponent(new PhysicBodyComponent(player, gameWorld.PhysicWorld, new(-8, -16, 16, 32)));
            player.AddComponent(new PlayerComponent(player, keyboard, playerContent));
            gameWorld.AddEntity(player);

            PlayState playState = new(GraphicsDevice, Content, gameWorld)
            {
                Enabled = true,
                Visible = true
            };

            // playState.Initialize();
            // playState.LoadLevel("Maps/debug0.tmx");
            //
            // keyboard.KeyDown += e =>
            // {
            //     if (e.Key == Keys.F2)
            //     {
            //         playState.ReloadLevel();
            //     }
            // };

            //Load it into the manager.
            GameStateManager.AddState(playState);
        }

        private static TileMap BuildBoxTileMap(ContentManager content)
        {
            Texture2D tileset = content.Load<Texture2D>("textures/tileset-dev0");

            Tile floorTile = new(TileType.Solid, tileset, new(1, 0));
            Tile ceilingTile = new(TileType.Solid, tileset, new(1, 2));
            Tile rightWallTile = new(TileType.Solid, tileset, new(0, 1));
            Tile leftWallTile = new(TileType.Solid, tileset, new(2, 1));
            Tile tlCornerTile = new(TileType.Solid, tileset, new(4, 0));
            Tile trCornerTile = new(TileType.Solid, tileset, new(5, 0));
            Tile blCornerTile = new(TileType.Solid, tileset, new(4, 1));
            Tile brCornerTile = new(TileType.Solid, tileset, new(5, 1));

            int width = 16;
            int height = 14;
            TileMap tileMap = new(width, height);
            for (int x = 1; x < width - 1; x++)
            {
                tileMap[x, 0] = ceilingTile;
                tileMap[x, height - 1] = floorTile;
            }
            for (int y = 1; y < height - 1; y++)
            {
                tileMap[0, y] = leftWallTile;
                tileMap[width - 1, y] = rightWallTile;
            }
            tileMap[0, 0] = tlCornerTile;
            tileMap[width - 1, 0] = trCornerTile;
            tileMap[0, height - 1] = blCornerTile;
            tileMap[width - 1, height - 1] = brCornerTile;
            return tileMap;
        }

        protected override void Update(GameTime gameTime)
        {
            //Use default exit input from the Monogame template.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

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

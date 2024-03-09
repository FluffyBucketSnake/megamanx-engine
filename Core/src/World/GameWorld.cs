using System.Collections.Generic;
using System.Linq;
using MegamanX.GameObjects;
using MegamanX.GameObjects.Playable;
using MegamanX.Graphics;
using MegamanX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace MegamanX.World
{
    public class GameWorld
    {
        private static Texture2D whitePixel = null;

        public Point Size { get; set; }

        public Rectangle Bounds => new Rectangle(Point.Zero, Size);

        public Camera2D Camera = Camera2D.Default;

        public GameObjectCollection Objects { get; }

        public PhysicWorld World { get; private set; }

        public Song Music;

        public GameWorld()
        {
            Objects = new GameObjectCollection(this);
            World = new PhysicWorld();
            Size = Point.Zero;
        }

        public GameWorld(int width, int height)
        {
            Objects = new GameObjectCollection(this);
            World = new PhysicWorld();
            Size = new Point(width, height);
        }

        public void Update(GameTime gameTime)
        {
            //Cache the state.
            ICollection<LegacyGameObject> currentFrame = Objects.ToList();

            //Update level physics.
            World.Update(gameTime);

            //Update each object.
            foreach (var gameObject in currentFrame)
            {
                //Check if within bounds.
                if (gameObject.WorldBounds.Intersects(Camera.WorldBounds))
                {
                    if (!gameObject.IsActive)
                    {
                        gameObject.Activate();
                    }
                }
                else
                {
                    if (gameObject.IsActive)
                    {
                        gameObject.Deactivate();
                        if (!gameObject.IsPersistent)
                        {
                            gameObject.Destroy();
                            continue;
                        }
                    }
                }

                //Call object's update method.
                gameObject.Update(gameTime);
            }

            //Update camera's position.
            var player = currentFrame.FirstOrDefault(e => e.GetType() == typeof(Player));
            if (player != null)
            {
                Camera.WorldPosition = player.Position;
            }

            //Clamp camera to the world.
            var cameraMinX = -Camera.SourceBounds.Left;
            var cameraMinY = -Camera.SourceBounds.Top;
            var cameraMaxX = Size.X - Camera.SourceBounds.Right;
            var cameraMaxY = Size.Y - Camera.SourceBounds.Bottom;
            Camera.WorldPosition = Vector2.Clamp(Camera.WorldPosition, new Vector2(cameraMinX, cameraMinY),
                new Vector2(cameraMaxX, cameraMaxY));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Create white pixel.
            if (whitePixel == null)
            {
                whitePixel = Texture2DPrefabs.CreateWhitePixel(spriteBatch.GraphicsDevice);
            }

            spriteBatch.Begin(samplerState: SamplerState.PointWrap,
                transformMatrix: Camera.GetTransformation(spriteBatch.GraphicsDevice.Viewport),
                blendState: BlendState.AlphaBlend);

            DrawWorldTiles(spriteBatch);

            //Render all entities.
            foreach (var gameObject in Objects)
            {
                gameObject.Draw(gameTime, spriteBatch);
                //spriteBatch.Draw(whitePixel, gameObject.WorldBounds, new Color(Color.Red, 0.5f));
            }

            //DrawPhysicWorldDebug(spriteBatch);

            spriteBatch.End();
        }

        private void DrawPhysicWorldDebug(SpriteBatch spriteBatch)
        {
            HashSet<Point> testedTilesCoordinates = new HashSet<Point>();

            foreach (var body in World.Bodies)
            {
                testedTilesCoordinates.UnionWith(World.Tiles.SelectAll(body.WorldBounds));

                spriteBatch.Draw(whitePixel, body.WorldBounds, new Color(Color.Blue, 0.5f));

                foreach (var sensor in body.Sensors)
                {
                    var worldArea = sensor.Area;
                    worldArea.Offset(body.Position);
                    worldArea.Offset(body.Bounds.Location);
                    testedTilesCoordinates.UnionWith(World.Tiles.SelectAll(worldArea));

                    var color = sensor.State ? new Color(Color.Green, 0.25f) : new Color(Color.Red, 0.25f);
                    spriteBatch.Draw(whitePixel, worldArea, color);
                }
            }

            foreach (var sensor in World.Sensors)
            {
                testedTilesCoordinates.UnionWith(World.Tiles.SelectAll(sensor.Area));

                var color = sensor.State ? new Color(Color.Green, 0.25f) : new Color(Color.Red, 0.25f);
                spriteBatch.Draw(whitePixel, sensor.Area, color);
            }

            foreach (var tileCoordinate in testedTilesCoordinates)
            {
                int x = (int)World.Tiles.GetWorldX(tileCoordinate.X);
                int y = (int)World.Tiles.GetWorldY(tileCoordinate.Y);
                int width = Tile.Width;
                int height = Tile.Height;

                spriteBatch.Draw(whitePixel,
                new Rectangle(x + 1, y + 1, width - 2, height - 2),
                new Color(Color.Gray, 0.125f));
            }
        }

        private void DrawWorldTiles(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < World.Tiles.GridHeight; y++)
            {
                for (int x = 0; x < World.Tiles.GridWidth; x++)
                {
                    Tile tile = World.Tiles[x, y];
                    if (tile.Texture != null)
                    {
                        spriteBatch.Draw(tile.Texture, new Rectangle(x * Tile.Width,
                        y * Tile.Height, Tile.Width, Tile.Height),
                        new Rectangle(tile.UV.X * Tile.Width, tile.UV.Y * Tile.Height,
                        Tile.Width, Tile.Height), Color.White, 0f, Vector2.Zero,
                        SpriteEffects.None, 0f);
                    }
                }
            }
        }
    }
}

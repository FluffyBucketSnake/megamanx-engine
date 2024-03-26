using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MegamanX.Components;
using MegamanX.Graphics;
using MegamanX.Map;
using MegamanX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX
{
    public class GameWorld(TileMap tiles)
    {
        private readonly List<Entity> entities = [];

        public TileMap Tiles => tiles;
        public PhysicWorld PhysicWorld { get; } = new(tiles);
        public IEnumerable<Entity> Entities => new ReadOnlyCollection<Entity>(entities);
        public CameraComponent? CurrentCamera { get; set; }

        public void AddEntity(Entity entity)
        {
            entities.Add(entity);
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                entity.Update(gameTime);
            }
            PhysicWorld.Update(gameTime);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(
                samplerState: SamplerState.PointWrap,
                transformMatrix: CurrentCamera?.GetTransformation(spriteBatch.GraphicsDevice.Viewport),
                blendState: BlendState.AlphaBlend);
            RenderTileMap(spriteBatch);
            RenderEntities(gameTime, spriteBatch);
            RenderDebug(gameTime, spriteBatch);
            spriteBatch.End();
        }

        private void RenderTileMap(SpriteBatch spriteBatch)
        {
            for (int y = 0; y < tiles.GridHeight; y++)
            {
                for (int x = 0; x < tiles.GridWidth; x++)
                {
                    Tile tile = tiles[x, y];
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

        private void RenderEntities(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < entities.Count; i++)
            {
                Entity entity = entities[i];
                entity.Draw(gameTime, spriteBatch);
            }
        }

        private void RenderDebug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (PhysicBody body in PhysicWorld.Bodies)
            {
                spriteBatch.Draw(Texture2DPrefabs.WhitePixel, body.WorldBounds, new Color(Color.Green, 0.5f));
            }
        }
    }
}

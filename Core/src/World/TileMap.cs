using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MegamanX.World
{
    public class TileMap
    {
        public int GridWidth { get; }

        public int GridHeight { get; }

        Tile[,] tiles;

        public TileMap(int gridWidth, int gridHeight)
        {
            //Set tilemap size.
            GridWidth = gridWidth;
            GridHeight = gridHeight;
            //Create tile array.
            tiles = new Tile[gridHeight, gridWidth];
        }

        public Tile this[int gridX, int gridY]
        {
            get
            {
                //Boundary check.
                if (gridX < 0 || gridX >= GridWidth)
                {
                    return new Tile(TileType.Empty);
                }
                if (gridY < 0 || gridY >= GridHeight)
                {
                    return new Tile(TileType.Empty);
                }
                //Return tile.
                return tiles[gridY, gridX];
            }
            set
            {
                //Boundary check.
                if (gridX < 0 || gridX >= GridWidth)
                {
                    throw new ArgumentOutOfRangeException(nameof(gridX));
                }
                if (gridY < 0 || gridY >= GridHeight)
                {
                    throw new ArgumentOutOfRangeException(nameof(gridY));
                }
                //Set tile value.
                tiles[gridY, gridX] = value;
            }
        }

        public float GetWorldX(int x) => x * Tile.Width;

        public float GetWorldY(int y) => y * Tile.Height;

        public Vector2 GetWorldPosition(Point tilePosition) =>
        new Vector2(GetWorldX(tilePosition.X), GetWorldY(tilePosition.Y));

        public int GetTileX(float x)
        {
            return (int)(x / Tile.Width);
        }

        public int GetTileY(float y)
        {
            return (int)(y / Tile.Height);
        }

        public Point GetTilePosition(Vector2 position)
        {
            return new Point(GetTileX(position.X), GetTileY(position.Y));
        }

        public bool Any(Rectangle area, Predicate<Tile> predicate)
        {
            int gridAreaLeft = GetTileX(area.Left);
            int gridAreaTop = GetTileY(area.Top);
            int gridAreaRight = GetTileX(Math.Max(area.Left,area.Right - 1));
            int gridAreaBottom = GetTileY(Math.Max(area.Top,area.Bottom - 1));

            for (int x = gridAreaLeft; x <= gridAreaRight; x++)
            {
                for (int y = gridAreaTop; y <= gridAreaBottom; y++)
                {
                    if (predicate(this[x, y]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool AnySolid(Rectangle area)
        {
            return Any(area, e => e.Type == TileType.Solid);
        }

        public IEnumerable<Point> Select(Rectangle area, Predicate<Tile> predicate)
        {
            ICollection<Point> results = new List<Point>();

            int gridAreaLeft = GetTileX(area.Left);
            int gridAreaTop = GetTileY(area.Top);
            int gridAreaRight = GetTileX(Math.Max(area.Left,area.Right - 1));
            int gridAreaBottom = GetTileY(Math.Max(area.Top,area.Bottom - 1));

            for (int x = gridAreaLeft; x <= gridAreaRight; x++)
            {
                for (int y = gridAreaTop; y <= gridAreaBottom; y++)
                {
                    if (predicate(this[x, y]))
                    {
                        results.Add(new Point(x, y));
                    }
                }
            }
            
            return results;
        }

        public IEnumerable<Point> SelectAll(Rectangle area)
        {
            return Select(area, e => true);
        }

        public IEnumerable<Point> SelectSolid(Rectangle area)
        {
            return Select(area, e => e.Type == TileType.Solid);
        }

        public bool QueryFloor(Rectangle area, out int floorY)
        {
            int gridAreaLeft = GetTileX(area.Left);
            int gridAreaRight = GetTileX(Math.Max(area.Left,area.Right - 1));
            int gridAreaBottom = GetTileY(Math.Max(area.Top,area.Bottom - 1));

            floorY = 0;
            for (int y = gridAreaBottom; y < GridHeight; y++)
            {
                for (int x = gridAreaLeft; x <= gridAreaRight; x++)
                {
                    if (this[x, y].Type == TileType.Solid)
                    {
                        floorY = y;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool QueryWallRight(Rectangle area, out int wallX)
        {
            int gridAreaTop = GetTileY(area.Top);
            int gridAreaBottom = GetTileY(area.Bottom - 1);
            int gridAreaRight = GetTileX(area.Right - 1);

            wallX = 0;
            for (int x = gridAreaRight; x < GridWidth; x++)
            {
                for (int y = gridAreaTop; y <= gridAreaBottom; y++)
                {
                    if (this[x, y].Type == TileType.Solid)
                    {
                        wallX = x;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool QueryCeiling(Rectangle area, out int ceilingY)
        {
            int gridAreaLeft = GetTileX(area.Left);
            int gridAreaRight = GetTileX(area.Right - 1);
            int gridAreaTop = GetTileY(area.Top);

            ceilingY = 0;
            for (int y = gridAreaTop; y >= 0; y--)
            {
                for (int x = gridAreaLeft; x <= gridAreaRight; x++)
                {
                    if (this[x, y].Type == TileType.Solid)
                    {
                        ceilingY = y;
                        return true;
                    }
                }
            }

            return false;
        }

        public bool QueryWallLeft(Rectangle area, out int wallX)
        {
            int gridAreaTop = GetTileY(area.Top);
            int gridAreaBottom = GetTileY(area.Bottom - 1);
            int gridAreaLeft = GetTileX(area.Left);

            wallX = 0;
            for (int x = gridAreaLeft; x >= 0; x--)
            {
                for (int y = gridAreaTop; y <= gridAreaBottom; y++)
                {
                    if (this[x, y].Type == TileType.Solid)
                    {
                        wallX = x;
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
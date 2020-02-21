using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MegamanX.Physics;

namespace MegamanX.World
{
    public enum TileType
    {
        Empty,
        Solid
    }

    public struct Tile
    {
        public static Tile Empty => new Tile(TileType.Empty,null,Point.Zero);
        
        public const int Width = 16;

        public const int Height = 16;

        public TileType Type;

        public Texture2D Texture;

        public Point UV;

        public Tile(TileType type)
        {
            Type = type;
            Texture = null;
            UV = Point.Zero;
        }

        public Tile(TileType type, Texture2D texture)
        {
            Type = type;
            Texture = texture;
            UV = Point.Zero;
        }

        public Tile(TileType type, Texture2D texture, Point uv)
        {
            Type = type;
            Texture = texture;
            UV = uv;
        }
    }
}
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.Map
{
    public enum TileType
    {
        Empty,
        Solid
    }

    public record struct Tile(TileType Type, Texture2D? Texture, Point UV)
    {
        public static Tile Empty => new(TileType.Empty, null, Point.Zero);

        public const int Width = 16;

        public const int Height = 16;

        public Tile(TileType type) : this(type, null, Point.Zero) { }

        public Tile(TileType type, Texture2D? texture) : this(type, texture, Point.Zero) { }
    }
}

using System;
using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects
{
    /// <summary>
    /// Represents the 4 possible directions in 2D.
    /// </summary>
    [Flags]
    public enum Direction2D : byte
    {
        None = 0,
        Top = 1,
        Left = 2,
        Bottom = 4,
        Right = 8
    }

    public static class Direction2DHelper
    {
        public static Direction2D GetDirection(Vector2 value)
        {
            Direction2D result = Direction2D.None;

            if (value.X > 0)
            {
                result = Direction2D.Right;
            }
            else if (value.X < 0)
            {
                result = Direction2D.Left;
            }

            if (value.Y > 0)
            {
                result |= Direction2D.Bottom;
            }
            else if (value.Y < 0)
            {
                result |= Direction2D.Top;
            }

            return result;
        }
    }
}
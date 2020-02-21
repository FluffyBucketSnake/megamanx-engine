using Microsoft.Xna.Framework;

namespace MegamanX
{
    public static class RectangleExtensions
    {
        public static Rectangle Translate(this Rectangle rectangle, Vector2 translation)
        {
            return new Rectangle(
                rectangle.X + (int)translation.X,
                rectangle.Y + (int)translation.Y,
                rectangle.Width,
                rectangle.Height
                );
        }

        public static Rectangle GetTop(this Rectangle rectangle, int height)
        {
            return new Rectangle(rectangle.Left,rectangle.Top - height, 
            rectangle.Width, height);
        }

        public static Rectangle GetLeft(this Rectangle rectangle, int width)
        {
            return new Rectangle(rectangle.Left - width,rectangle.Top, 
            width, rectangle.Height);
        }

        public static Rectangle GetBottom(this Rectangle rectangle, int height)
        {
            return new Rectangle(rectangle.Left,rectangle.Bottom, 
            rectangle.Width, height);
        }

        public static Rectangle GetRight(this Rectangle rectangle, int width)
        {
            return new Rectangle(rectangle.Right,rectangle.Top, 
            width, rectangle.Height);
        }

        public static bool Intersects(this Rectangle me, Rectangle other, out Vector2 penetration)
        {
            if (me.Intersects(other))
            {
                float pX,pY;

                int dX1 = me.Right - other.Left;
                int dX2 = other.Right - me.Left;
                pX = (dX1 <= dX2) ? dX1 : -dX2;

                int dY1 = me.Bottom - other.Top;
                int dY2 = other.Bottom - me.Top;
                pY = (dY1 <= dY2) ? dY1 : -dY2;

                penetration = new Vector2(pX,pY);
                return true;
            }
            else
            {
                penetration = Vector2.Zero;
                return false;
            }
        }
    }
}
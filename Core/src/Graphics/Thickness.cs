namespace MegamanX.Graphics
{
    public struct Thickness
    {
        public float Top;
        public float Left;
        public float Bottom;
        public float Right;

        public Thickness(float width)
        {
            Top = Left = Bottom = Right = width;
        }

        public static readonly Thickness Zero = new(0.0f);

        public Thickness(float vwidth, float hwidth)
        {
            Top = Bottom = vwidth;
            Left = Right = hwidth;
        }

        public Thickness(float top, float left, float bottom, float right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }
    }
}

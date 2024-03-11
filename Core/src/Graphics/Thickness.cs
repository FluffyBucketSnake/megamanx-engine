namespace MegamanX.Graphics
{
    public record struct Thickness(float Top, float Left, float Bottom, float Right)
    {
        public static readonly Thickness Zero = new(0.0f);

        public Thickness(float width) : this(width, width, width, width) { }
        public Thickness(float horizontal, float vertical) : this(vertical, horizontal, vertical, horizontal) { }
    }
}

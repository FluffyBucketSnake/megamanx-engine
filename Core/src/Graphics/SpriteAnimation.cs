namespace MegamanX.Graphics
{
    public record struct SpriteAnimation(string Name, int[] Frames, bool Loops = false)
    {
        public SpriteAnimation(string name, int frameCount) : this(name, new int[frameCount]) { }
    }
}

namespace MegamanX.Graphics
{
    public class SpriteAnimation
    {
        public bool IsLooping { get; set; }
        public string Name { get; private set; }
        public float FPS { get; set; } = 60.0f;
        public int[] Frames { get; }

        // public SpriteAnimation(int frameCount)
        // {
        //     Frames = new int[frameCount];
        // }

        public SpriteAnimation(string name, params int[] frames)
        {
            Name = name;
            Frames = frames;
        }
    }
}
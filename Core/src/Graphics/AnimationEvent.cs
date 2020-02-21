namespace MegamanX.Graphics
{
    public class AnimationEventArgs
    {
        public SpriteAnimation Animation;
        public float Time;

        public AnimationEventArgs(SpriteAnimation animation, float time)
        {
            Animation = animation;
            Time = time;
        }
    }
}
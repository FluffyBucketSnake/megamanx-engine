using System.Collections.Generic;
using System.Linq;
using MegamanX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.Components
{
    public class SpriteAnimatorComponent(Sprite sprite, IEnumerable<SpriteAnimation> animations) : IComponent
    {
        public int FPS { get; set; } = 60;
        public int CurrentFrame { get; private set; }
        public SpriteAnimation? CurrentAnimation { get; private set; }
        public Dictionary<string, SpriteAnimation> Animations { get; } = animations.ToDictionary(a => a.Name);

        public event ComponentEvent<SpriteAnimatorComponent>? AnimationEnded;

        private int FrameDuration => 1000 / FPS;
        private int currentFrameTime;

        public void Play(string name)
        {
            CurrentAnimation = Animations[name];
            CurrentFrame = 0;
            currentFrameTime = 0;
        }

        int? IComponent.DrawPriority => 49;
        void IComponent.Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (CurrentAnimation == null)
            {
                return;
            }
            SpriteAnimation animation = CurrentAnimation.Value;

            currentFrameTime += gameTime.ElapsedGameTime.Milliseconds;
            CurrentFrame += currentFrameTime / FrameDuration;
            currentFrameTime %= FrameDuration;

            if (CurrentFrame == animation.FrameCount)
            {
                if (CurrentAnimation.Value.Loops)
                {
                    CurrentFrame %= animation.FrameCount;

                }
                else
                {
                    CurrentAnimation = null;
                    AnimationEnded?.Invoke(this);
                }

            }

            if (CurrentAnimation != null)
            {
                sprite.FrameIndex = animation.Frames[CurrentFrame];
            }
        }
    }
}

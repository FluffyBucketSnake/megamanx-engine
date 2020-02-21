using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.Graphics
{
    public class Sprite
    {
        public Sprite(SpriteSheet template)
        {
            Template = template ?? throw new ArgumentNullException(nameof(template));
        }

        public event EventHandler<AnimationEventArgs> OnAnimationComplete;
        
        public SpriteSheet Template { get; }
        public int FrameIndex { get; set; }
        public SpriteFrame CurrentFrame => Template.Frames[FrameIndex];
        public SpriteAnimation CurrentAnimation { get; set; }
        public AnimationState State { get; private set; } = AnimationState.Stopped;
        public float CurrentTime { get; set; }
        public Vector2 Position { get; set; }
        public SpriteEffects Effects { get; set; }
        public float Depth { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Template.Texture, Position, CurrentFrame.Source, Color.White, 0.0f, 
            Template.GetOrigin(FrameIndex, Effects), Vector2.One, Effects, Depth);
        }

        public bool HasAnimation(string animationName)
        {
            return Template.Animations.Contains(animationName);
        }

        public void Play(SpriteAnimation animation)
        {
            CurrentTime = 0;
            CurrentAnimation = animation ?? throw new ArgumentNullException(nameof(animation));
            State = CurrentAnimation != null ? AnimationState.Playing : AnimationState.Stopped;
        }

        public void Play(string animationName)
        {
            Play(Template.Animations[animationName]);
        }

        public bool TryPlay(string animationName)
        {
            if (HasAnimation(animationName))
            {
                Play(animationName);
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Stop()
        {
            CurrentAnimation = null;
            State = AnimationState.Stopped;
        }

        public void Update(GameTime gameTime)
        {
            if (State != AnimationState.Playing)
            {
                return;
            }

            CurrentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            int frameTime = (int)(CurrentTime * CurrentAnimation.FPS);
            int animationLenght = CurrentAnimation.Frames.Length;
            if (frameTime >= animationLenght)
            {
                var animation = CurrentAnimation;
                if (CurrentAnimation.IsLooping)
                {
                    CurrentTime %= (animationLenght / animation.FPS);
                    FrameIndex = animation.Frames[(int)(CurrentTime * animation.FPS)];
                }
                else
                {
                    FrameIndex = animation.Frames[animationLenght - 1];
                    Stop();
                    OnAnimationComplete?.Invoke(this, new AnimationEventArgs(animation, CurrentTime));
                }
            }
            else
            {
                FrameIndex = CurrentAnimation.Frames[frameTime];
            }
        }
    }
}
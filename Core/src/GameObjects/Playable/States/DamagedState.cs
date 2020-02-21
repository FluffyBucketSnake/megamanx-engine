using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable.States
{
    public class DamagedState : PlayerState
    {
        int timer;
        Vector2 _knockback;

        public DamagedState(Vector2 knockback, int duration)
        {
            _knockback = knockback;
            timer = duration;
        }

        public override void OnStateEnter(StateChangeInfo info)
        {
            Parent.AnimationController.State = PlayerAnimationStates.Hurt;
            Parent.Physics.Speed = new Vector2(0, _knockback.Y);
        }

        public override void Update(GameTime gameTime)
        {
            Parent.Physics.Move(new Vector2(_knockback.X, 0) *
            gameTime.ElapsedGameTime.Milliseconds);

            timer -= gameTime.ElapsedGameTime.Milliseconds;

            if (timer <= 0)
            {
                if (Parent.Physics.GroundSensor)
                {
                    if (Parent.CurrentInput.IsMoving)
                    {
                        Parent.CurrentState = new WalkingState();
                    }
                    else
                    {
                        Parent.CurrentState = new StandingState();
                    }
                }
                else
                {
                    Parent.CurrentState = new FallState(false);
                }
            }
        }
    }
}
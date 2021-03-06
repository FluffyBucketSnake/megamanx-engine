using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable.States
{
    public class DamagedState : PlayerState
    {
        int timer;

        public DamagedState(Player parent, int duration) : base(parent)
        {
            Duration = duration;
        }

        public int Duration { get; set; }
        public Vector2 Knockback { get; set; }

        public override void OnStateEnter(StateChangeInfo info)
        {
            Parent.AnimationController.State = PlayerAnimationStates.Hurt;
            Parent.Physics.Speed = new Vector2(0, Knockback.Y);
            timer = 0;
        }

        public override void Update(GameTime gameTime)
        {
            Parent.Physics.Move(new Vector2(Knockback.X, 0) *
            gameTime.ElapsedGameTime.Milliseconds);

            timer += gameTime.ElapsedGameTime.Milliseconds;

            if (timer > Duration)
            {
                if (Parent.Physics.GroundSensor)
                {
                    if (Parent.CurrentInput.IsMoving)
                    {
                        Parent.ChangeState<WalkingState>();
                    }
                    else
                    {
                        Parent.ChangeState<StandingState>();
                    }
                }
                else
                {
                    Parent.GetState<FallState>().IsDashing = false;
                    Parent.ChangeState<FallState>();
                }
            }
        }
    }
}
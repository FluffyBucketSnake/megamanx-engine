using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable.States
{
    public class DashState : PlayerState
    {
        public const int DefaultDuration = 500;

        int timer;

        public DashState(Player parent, int duration) : base(parent)
        {
            timer = duration;
        }

        public override void OnStateEnter(StateChangeInfo info)
        {
            Parent.AnimationController.State = PlayerAnimationStates.Dash;
        }

        public override void OnInputEnter(PlayerInput inputType)
        {
            switch (inputType)
            {
                case PlayerInput.Jump:
                    Parent.ChangeState<JumpState>();
                    break;
                case PlayerInput.Fire:
                    if ((Parent.CurrentWeapon?.Fire()).GetValueOrDefault())
                    {
                        Parent.AnimationController.Shoot();
                    }
                    break;
            }
        }

        public override void OnInputLeave(PlayerInput inputType)
        {
            switch (inputType)
            {
                case PlayerInput.Left:
                    if (Parent.IsLeft)
                    {
                        Stop();
                    }
                    break;
                case PlayerInput.Right:
                    if (!Parent.IsLeft)
                    {
                        Stop();
                    }
                    break;
                case PlayerInput.Dash:
                    Stop();
                    break;
                case PlayerInput.Fire:
                    if ((Parent.CurrentWeapon?.ReleaseCharge()).GetValueOrDefault())
                    {
                        Parent.AnimationController.Shoot();
                    }
                break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            //Dashing.
            int milliseconds = gameTime.ElapsedGameTime.Milliseconds;
            float speed = Parent.Physics.Parameters.DashingSpeed;
            if (Parent.IsLeft)
            {
                Parent.Physics.Move(new Vector2(-speed * milliseconds, 0));
            }
            else
            {
                Parent.Physics.Move(new Vector2(speed * milliseconds, 0));
            }

            timer -= milliseconds;

            //Check if the player is on the ground and if the timer ran out.
            if (!Parent.Physics.GroundSensor)
            {
                Parent.GetState<FallState>().IsDashing = true;
                Parent.ChangeState<FallState>();
            }
            else if (timer <= 0)
            {
                Stop();
            }
        }

        private void Stop()
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
    }
}
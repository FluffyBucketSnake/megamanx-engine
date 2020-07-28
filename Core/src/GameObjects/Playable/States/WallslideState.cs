using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable.States
{
    public class WallslideState : PlayerState
    {
        int dashInputTimer = 0;

        public WallslideState(Player parent) : base(parent) {}

        public override void OnStateEnter(StateChangeInfo info)
        {
            Parent.AnimationController.State = PlayerAnimationStates.Wallslide;
            Parent.Physics.Body.GravityScale = 0;
            Parent.IsLeft = !Parent.IsLeft;
            Parent.Physics.Speed = new Vector2(Parent.Physics.Speed.X,0);
        }

        public override void OnStateLeave(StateChangeInfo info)
        {
            Parent.Physics.GravityScale = 1;
        }

        public override void OnInputEnter(PlayerInput inputType)
        {
            switch(inputType)
            {
                case PlayerInput.Left:
                if (Parent.IsLeft)
                {
                    Parent.GetState<FallState>().IsDashing = false;
                    Parent.ChangeState<FallState>();
                }
                break;
                case PlayerInput.Right:
                if (!Parent.IsLeft)
                {
                    Parent.GetState<FallState>().IsDashing = false;
                    Parent.ChangeState<FallState>();
                }
                break;
                case PlayerInput.Jump:
                if ((Parent.IsLeft && Parent.Physics.RightWalljumpSensor) ||
                (!Parent.IsLeft && Parent.Physics.LeftWalljumpSensor))
                {
                    Parent.IsLeft = !Parent.IsLeft;
                    Parent.ChangeState<WalljumpState>();
                }
                break;
                case PlayerInput.Fire:
                if ((Parent.CurrentWeapon?.Fire()).GetValueOrDefault())
                {
                    Parent.AnimationController.Shoot();
                }
                break;
                case PlayerInput.Dash:
                    dashInputTimer = 66;
                break;
            }
        }

        public override void OnInputLeave(PlayerInput inputType)
        {
            switch(inputType)
            {
                case PlayerInput.Left:
                if (!Parent.IsLeft)
                {
                    Parent.GetState<FallState>().IsDashing = false;
                    Parent.ChangeState<FallState>();
                }
                break;
                case PlayerInput.Right:
                if (Parent.IsLeft)
                {
                    Parent.GetState<FallState>().IsDashing = false;
                    Parent.ChangeState<FallState>();
                }
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
            //Slide down.
            float speed = Parent.Physics.Parameters.WallslidingSpeed;
            Parent.Physics.Move(new Vector2(0, speed * gameTime.ElapsedGameTime.Milliseconds));

            //Reduce dash command timer.
            if (dashInputTimer > 0)
            {
                dashInputTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }

            //Check for falling out.
            if ((Parent.IsLeft && !Parent.Physics.RightWallSensor) || 
            (!Parent.IsLeft && !Parent.Physics.LeftWallSensor))
            {
                Parent.ChangeState<FallState>();
            }
            else if (Parent.Physics.GroundSensor)
            {
                Parent.IsLeft = !Parent.IsLeft;
                Parent.ChangeState<StandingState>();
            }
        }
    }
}
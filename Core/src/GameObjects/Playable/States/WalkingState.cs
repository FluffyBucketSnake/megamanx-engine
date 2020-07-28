using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable.States
{
    public class WalkingState : PlayerState
    {
        public WalkingState(Player parent) : base(parent) {}

        public override void OnStateEnter(StateChangeInfo info)
        {
            Parent.AnimationController.State = PlayerAnimationStates.Walk;
        }

        public override void OnInputEnter(PlayerInput inputType)
        {
            switch (inputType)
            {
                case PlayerInput.Jump:
                    if (Parent.Physics.GroundSensor)
                    {
                        Parent.ChangeState<JumpState>();
                    }
                    break;
                case PlayerInput.Fire:
                    if ((Parent.CurrentWeapon?.Fire()).GetValueOrDefault())
                    {
                        Parent.AnimationController.Shoot();
                    }
                    break;
                case PlayerInput.Dash:
                    Parent.ChangeState<DashState>();
                    break;
            }
        }

        public override void OnInputLeave(PlayerInput inputType)
        {
            switch(inputType)
            {
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
            //Walking.
            float speed = Parent.Physics.Parameters.WalkingSpeed;
            if (Parent.CurrentInput.IsMovingLeft)
            {
                Parent.IsLeft = true;
                Parent.Physics.Move(new Vector2(-speed *
                    gameTime.ElapsedGameTime.Milliseconds, 0));
            }
            else if (Parent.CurrentInput.IsMovingRight)
            {
                Parent.IsLeft = false;
                Parent.Physics.Move(new Vector2(speed *
                    gameTime.ElapsedGameTime.Milliseconds, 0));
            }
            else
            {
                Parent.ChangeState<StandingState>();
            }

            //Check if falling.
            if (!Parent.Physics.GroundSensor)
            {
                Parent.GetState<FallState>().IsDashing = false;
                Parent.ChangeState<FallState>();
            }
        }
    }
}
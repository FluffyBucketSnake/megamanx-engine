using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable.States
{
    public class WalkingState : PlayerState
    {
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
                        Parent.CurrentState = new JumpState(false, Parent.Physics.Parameters.JumpSpeed);
                    }
                    break;
                case PlayerInput.Fire:
                    if ((Parent.CurrentWeapon?.Fire()).GetValueOrDefault())
                    {
                        Parent.AnimationController.Shoot();
                    }
                    break;
                case PlayerInput.Dash:
                    Parent.CurrentState = new DashState(DashState.DefaultDuration);
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
                Parent.CurrentState = new StandingState();
            }

            //Check if falling.
            if (!Parent.Physics.GroundSensor)
            {
                Parent.CurrentState = new FallState(false);
            }
        }
    }
}
using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable.States
{
    public class StandingState : PlayerState
    {
        public StandingState(Player parent) : base(parent) {}

        public override void OnStateEnter(StateChangeInfo info)
        {
            Parent.AnimationController.State = PlayerAnimationStates.Idle;
        }

        public override void OnInputEnter(PlayerInput inputType)
        {
            switch (inputType)
            {
                case PlayerInput.Jump:
                    if (Parent.Physics.GroundSensor)
                    {
                        Parent.GetState<FallState>().IsDashing = false;
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
            //Check if walking.
            if (Parent.CurrentInput.IsMoving)
            {
                Parent.ChangeState<WalkingState>();
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
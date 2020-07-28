using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable.States
{
    public class FallState : PlayerState
    {
        int dashInputTimer = 0;

        public FallState(Player parent) : base(parent) {}

        public bool IsDashing { get; set; }

        public override void OnInputEnter(PlayerInput inputType)
        {
            switch (inputType)
            {
                case PlayerInput.Fire:
                    if ((Parent.CurrentWeapon?.Fire()).GetValueOrDefault())
                    {
                        Parent.AnimationController.Shoot();
                    }
                    break;
                case PlayerInput.Jump:
                    if ((!Parent.IsLeft && Parent.Physics.RightWalljumpSensor) ||
                    (Parent.IsLeft && Parent.Physics.LeftWalljumpSensor))
                    {
                        Parent.Physics.Speed = new Vector2(Parent.Physics.Speed.X, 0);

                        Parent.ChangeState<WallslideState>();
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
                case PlayerInput.Fire:
                    if ((Parent.CurrentWeapon?.ReleaseCharge()).GetValueOrDefault())
                    {
                        Parent.AnimationController.Shoot();
                    }
                break;
            }
        }

        public override void OnStateEnter(StateChangeInfo info)
        {
            Parent.AnimationController.State = PlayerAnimationStates.Fall;
        }

        public override void Update(GameTime gameTime)
        {
            //Air strafe.
            float speed = IsDashing ? Parent.Physics.Parameters.DashingSpeed :
            Parent.Physics.Parameters.WalkingSpeed;
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

            //Reduce dash command timer.
            if (dashInputTimer > 0)
            {
                dashInputTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }

            //Check if the player is on the ground or sliding down the wall.
            if (Parent.Physics.Speed.X > 0 || Parent.Physics.GroundSensor)
            {
                //Play landing animation.
                if (Parent.CurrentInput.IsMoving)
                {
                    Parent.ChangeState<WalkingState>();
                }
                else
                {
                    Parent.ChangeState<StandingState>();
                }
            }
            else if ((Parent.Physics.LeftWallSensor && Parent.CurrentInput.IsMovingLeft) ||
            (Parent.Physics.RightWallSensor && Parent.CurrentInput.IsMovingRight))
            {
                Parent.ChangeState<WallslideState>();
            }
        }
    }
}
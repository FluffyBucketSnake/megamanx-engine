using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable.States
{
    public class WalljumpState : PlayerState
    {
        float _jumpingSpeed;
        int animationTimer;
        int timer;

        public const int DefaultDuration = 112;

        public bool IsDashing { get; set; }

        public WalljumpState(bool isDashing, int duration, float jumpingSpeed)
        {
            IsDashing = isDashing;
            _jumpingSpeed = jumpingSpeed;
            animationTimer = 66;
            timer = duration;
        }

        public override void OnInputEnter(PlayerInput inputType)
        {
            switch(inputType)
            {
                case PlayerInput.Fire:
                if ((Parent.CurrentWeapon?.Fire()).GetValueOrDefault())
                {
                    Parent.AnimationController.Shoot();
                }
                break;
                case PlayerInput.Jump:
                if (animationTimer > 0 && ((!Parent.IsLeft && Parent.Physics.RightWalljumpSensor) ||
                (Parent.IsLeft && Parent.Physics.LeftWalljumpSensor)))
                {
                    Parent.Physics.Speed += new Vector2(0, _jumpingSpeed);
                    Parent.CurrentState = new WalljumpState(false,
                    WalljumpState.DefaultDuration,
                    Parent.Physics.Parameters.JumpSpeed);
                }
                break;
                case PlayerInput.Dash:
                if (animationTimer > 0)
                {
                    IsDashing = true;
                }
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
            Parent.AnimationController.State = PlayerAnimationStates.Walljump;
            Parent.Physics.GravityScale = 0;
        }

        public override void OnStateLeave(StateChangeInfo info)
        {
            Parent.Physics.GravityScale = 1f;
        }

        public override void Update(GameTime gameTime)
        {
            if (animationTimer > 0)
            {
                animationTimer -= gameTime.ElapsedGameTime.Milliseconds;
                if (animationTimer <= 0)
                {
                    Parent.Physics.GravityScale = 1f;
                    Parent.Physics.Speed -= new Vector2(0, _jumpingSpeed);
                }
            }
            else
            {
                float xSpeed = IsDashing ? Parent.Physics.Parameters.DashingSpeed :
                Parent.Physics.Parameters.WalkingSpeed;
                if (Parent.IsLeft)
                {
                    Parent.IsLeft = true;
                    Parent.Physics.Move(new Vector2(xSpeed, 0) * 
                    gameTime.ElapsedGameTime.Milliseconds);
                }
                else
                {
                    Parent.IsLeft = false;
                    Parent.Physics.Move(new Vector2(-xSpeed, 0) * 
                    gameTime.ElapsedGameTime.Milliseconds);
                }

                _jumpingSpeed -= Parent.Map.World.Gravity.Y * 
                gameTime.ElapsedGameTime.Milliseconds;
                timer -= gameTime.ElapsedGameTime.Milliseconds;

                if (timer <= 0)
                {
                    timer = 0;
                    Parent.Physics.Speed += new Vector2(0, _jumpingSpeed);
                    Parent.CurrentState = new JumpState(IsDashing,_jumpingSpeed);
                }
                else if (Parent.Physics.CeilingSensor)
                {
                    Parent.CurrentState = new FallState(IsDashing);
                }
            }
        }
    }
}
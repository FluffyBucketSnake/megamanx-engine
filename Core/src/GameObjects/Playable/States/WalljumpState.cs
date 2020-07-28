using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable.States
{
    public class WalljumpState : PlayerState
    {
        float _accSpeed;
        int animationTimer;
        int timer;

        public const int DefaultDuration = 112;

        public WalljumpState(Player parent, int duration, float jumpingSpeed) : base(parent)
        {
            JumpingSpeed = jumpingSpeed;
            Duration = duration;
        }

        public bool IsDashing { get; set; }
        public int Duration { get; set; }
        public float JumpingSpeed { get; set; }

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
                if (animationTimer <= 66 && ((!Parent.IsLeft && Parent.Physics.RightWalljumpSensor) ||
                (Parent.IsLeft && Parent.Physics.LeftWalljumpSensor)))
                {
                    Parent.Physics.Speed += new Vector2(0, _accSpeed);
                    OnStateEnter(null);
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
            animationTimer = 0;
            _accSpeed = Parent.Physics.Parameters.JumpSpeed;
        }

        public override void OnStateLeave(StateChangeInfo info)
        {
            Parent.Physics.GravityScale = 1f;
        }

        public override void Update(GameTime gameTime)
        {
            if (animationTimer <= 66)
            {
                animationTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (animationTimer > 66)
                {
                    Parent.Physics.GravityScale = 1f;
                    Parent.Physics.Speed -= new Vector2(0, _accSpeed);
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

                _accSpeed -= Parent.Map.World.Gravity.Y * 
                gameTime.ElapsedGameTime.Milliseconds;
                timer += gameTime.ElapsedGameTime.Milliseconds;

                if (timer > Duration)
                {
                    timer = 0;
                    var jumpState = Parent.GetState<JumpState>();
                    jumpState.IsDashing = IsDashing;
                    Parent.Physics.Speed += new Vector2(0, jumpState.InitialJumpingSpeed);
                    Parent.ChangeState<JumpState>();
                }
                else if (Parent.Physics.CeilingSensor)
                {
                    Parent.GetState<FallState>().IsDashing = IsDashing;
                    Parent.ChangeState<FallState>();
                }
            }
        }
    }
}
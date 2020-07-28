using MegamanX.Physics;
using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable.States
{
    public class JumpState : PlayerState
    {
        float jumpingSpeed;

        int dashInputTimer = 0;

        public bool IsDashing { get; set; }

        public JumpState(Player parent, float initalJumpingSpeed) : base(parent)
        {
            jumpingSpeed = initalJumpingSpeed;
        }

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
                        Parent.Physics.Speed += new Vector2(0, jumpingSpeed);
                        Parent.ChangeState<WalljumpState>();
                    }
                    break;
                case PlayerInput.Dash:
                    dashInputTimer = 66;
                    break;
            }
        }

        public override void OnStateEnter(StateChangeInfo info)
        {
            Parent.AnimationController.State = PlayerAnimationStates.Jump;
            Parent.Physics.Speed -= new Vector2(0, jumpingSpeed);
            Parent.Physics.Body.TileMapCollisionEvent += OnPlayerTilemapCollision;
        }

        public override void OnStateLeave(StateChangeInfo info)
        {
            Parent.Physics.Body.TileMapCollisionEvent -= OnPlayerTilemapCollision;
        }

        public override void OnInputLeave(PlayerInput inputType)
        {
            switch (inputType)
            {
                case PlayerInput.Jump:
                    Parent.Physics.Speed += new Vector2(0, jumpingSpeed);
                    Fall();
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
            //Air strafe.
            float speed = IsDashing ? Parent.Physics.Parameters.DashingSpeed :
            Parent.Physics.Parameters.WalkingSpeed;
            if (Parent.CurrentInput.IsMovingLeft)
            {
                Parent.IsLeft = true;
                Parent.Physics.Move(new Vector2(-speed * gameTime.ElapsedGameTime.Milliseconds, 0));
            }
            else if (Parent.CurrentInput.IsMovingRight)
            {
                Parent.IsLeft = false;
                Parent.Physics.Move(new Vector2(speed * gameTime.ElapsedGameTime.Milliseconds, 0));
            }

            //Calculate the current state of the jumping speed.
            if (Parent.Physics.World != null)
            {
                jumpingSpeed -= gameTime.ElapsedGameTime.Milliseconds * Parent.Physics.Body.GravityScale *
                Parent.Map.World.Gravity.Y;
            }
            //Reduce dash command timer.
            if (dashInputTimer > 0)
            {
                dashInputTimer -= gameTime.ElapsedGameTime.Milliseconds;
            }

            //Check if player is now falling.
            if (jumpingSpeed <= 0)
            {
                Fall();
            }
        }

        private void Fall()
        {
            Parent.GetState<FallState>().IsDashing = IsDashing;
            Parent.ChangeState<FallState>();
        }

        private void OnPlayerTilemapCollision(TileMapCollisionInfo info)
        {
            if (info.Penetration.Y < 0)
            {
                Fall();
            }
        }

        private void OnPlayerBodyCollision(BodyCollisionInfo info)
        {
            if (info.Penetration.Y < 0)
            {
                Fall();
            }
        }
    }
}
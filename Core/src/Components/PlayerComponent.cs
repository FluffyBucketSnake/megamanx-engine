using MegamanX.Graphics;
using MegamanX.Input.Keyboard;
using MegamanX.Math;
using MegamanX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MegamanX.Components
{
    public record struct PlayerInputFrame(bool Left, bool Right, bool Jump, bool Dash, bool Fire);

    public class PlayerInput(IKeyboardDevice keyboard)
    {
        private const Keys LEFT_KEY = Keys.Left;
        private const Keys RIGHT_KEY = Keys.Right;
        private const Keys JUMP_KEY = Keys.Z;
        private const Keys DASH_KEY = Keys.X;
        // private const Keys FIRE_KEY = Keys.A;

        public bool Left => keyboard.IsKeyDown(LEFT_KEY);
        public bool Right => keyboard.IsKeyDown(RIGHT_KEY);
        public bool Jump => keyboard.IsKeyDown(JUMP_KEY);
        public bool Dash => keyboard.IsKeyDown(DASH_KEY);

        public bool IsMovingLeft => keyboard.IsKeyDown(LEFT_KEY) && !keyboard.IsKeyDown(RIGHT_KEY);
        public bool IsMovingRight => !keyboard.IsKeyDown(LEFT_KEY) && keyboard.IsKeyDown(RIGHT_KEY);
        public bool IsMoving => IsMovingLeft || IsMovingRight;

        public bool ShouldJump => keyboard.IsKeyPressed(JUMP_KEY);
        public bool ShouldDash => keyboard.IsKeyPressed(DASH_KEY);
    }

    public enum PlayerState
    {
        Standing,
        Walking,
        Jumping,
        Falling,
        Dashing,
    }

    public record PlayerContent(
        SpriteSheet SpriteSheet, SoundEffect JumpSoundEffect, SoundEffect LandSoundEffect, SoundEffect DashSoundEffect,
        SoundEffect HurtSoundEffect, SoundEffect ChargingIntroSoundEffect, SoundEffect ChargingLoopSoundEffect)
    {
        public static PlayerContent LoadDefault(ContentManager content)
        {
            Texture2D texture = content.Load<Texture2D>("textures/player-x");
            SpriteFrame[] frames =
            [
                // Standing
                new SpriteFrame(225, 28, 30, 34, new Thickness(-2, 1, 0, 1)),
                new SpriteFrame(260, 28, 30, 34, new Thickness(-2, 1, 0, 1)),
                new SpriteFrame(294, 28, 30, 34, new Thickness(-2, 1, 0, 1)),
                // Shooting
                new SpriteFrame(364, 28, 30, 34, new Thickness(-2, 1, 0, 1)),
                new SpriteFrame(401, 28, 30, 34, new Thickness(-2, 1, 0, 1)),
                // Step
                new SpriteFrame(4, 66, 30, 34, new Thickness(-2, 1, 0, 1)),
                // Walking
                new SpriteFrame(49, 66, 20, 34, new Thickness(-2, 7, 0, 5)),
                new SpriteFrame(74, 66, 23, 35, new Thickness(-2, 4, -1, 5)),
                new SpriteFrame(104, 67, 32, 34, new Thickness(-1, 0, -1, 0)),
                new SpriteFrame(144, 67, 34, 33, new Thickness(-1, -2, 0, 0)),
                new SpriteFrame(189, 67, 26, 33, new Thickness(-1, 2, 0, 4)),
                new SpriteFrame(221, 66, 22, 34, new Thickness(-2, 5, 0, 5)),
                new SpriteFrame(247, 66, 25, 35, new Thickness(-2, 3, -1, 4)),
                new SpriteFrame(279, 66, 30, 34, new Thickness(-2, 2, 0, 0)),
                new SpriteFrame(317, 67, 34, 33, new Thickness(-1, -1, 0, -1)),
                new SpriteFrame(358, 67, 30, 33, new Thickness(-1, 1, 0, 2)),
                // Walking(Shooting)
                new SpriteFrame(40, 106, 29, 34, new Thickness(-2, 7, 0, -4)),
                new SpriteFrame(75, 106, 32, 35, new Thickness(-2, 4, -1, -4)),
                new SpriteFrame(114, 107, 35, 34, new Thickness(-1, 0, -1, -3)),
                new SpriteFrame(158, 107, 38, 33, new Thickness(-1, -2, 0, -4)),
                new SpriteFrame(203, 107, 34, 33, new Thickness(-1, 2, 0, -4)),
                new SpriteFrame(245, 106, 31, 34, new Thickness(-2, 5, 0, -4)),
                new SpriteFrame(283, 106, 33, 35, new Thickness(-2, 3, -1, -4)),
                new SpriteFrame(325, 106, 35, 34, new Thickness(-2, 2, 0, -5)),
                new SpriteFrame(368, 107, 37, 33, new Thickness(-1, -1, 0, -4)),
                new SpriteFrame(412, 107, 35, 33, new Thickness(-1, 1, 0, -4)),
                // Jumping
                new SpriteFrame(5, 147, 24, 37, new Thickness(-5, 11, 0, -3)),
                new SpriteFrame(36, 147, 15, 41, new Thickness(-5, 14, -4, 3)),
                new SpriteFrame(55, 145, 19, 46, new Thickness(-7, 10, -7, 3)),
                // Falling
                new SpriteFrame(79, 149, 23, 41, new Thickness(-3, 8, -6, 1)),
                new SpriteFrame(107, 149, 27, 42, new Thickness(-3, 5, -7, 0)),
                // Landing
                new SpriteFrame(138, 150, 24, 38, new Thickness(-2, 9, -1, -4)),
                new SpriteFrame(165, 152, 30, 32, new Thickness(0, 2, 0, 0)),
                // Jumping(Shooting)
                new SpriteFrame(200, 147, 29, 37, new Thickness(-5, 11, 0, -8)),
                new SpriteFrame(239, 147, 24, 41, new Thickness(-5, 14, -4, -6)),
                new SpriteFrame(270, 145, 27, 46, new Thickness(-7, 10, -7, -5)),
                // Falling(Shooting)
                new SpriteFrame(303, 149, 31, 41, new Thickness(-3, 8, -6, 1)),
                new SpriteFrame(340, 149, 31, 42, new Thickness(-3, 5, -7, 0)),
                // Landing(Shooting)
                new SpriteFrame(138, 150, 24, 38, new Thickness(-2, 9, -1, -10)),
                new SpriteFrame(165, 152, 30, 32, new Thickness(0, 2, 0, -6)),
                // Dashing
                new SpriteFrame(3, 334, 28, 31, new Thickness(1, 6, 0, -2)),
                new SpriteFrame(33, 340, 38, 26, new Thickness(7, 1, 0, -7)),
                // Dashing(Shooting)
                new SpriteFrame(75, 334, 37, 31, new Thickness(1, 6, 0, -11)),
                new SpriteFrame(114, 340, 48, 26, new Thickness(7, 1, 0, -17)),
                // Wallsliding
                new SpriteFrame(4, 196, 24, 42, new Thickness(-4, 0, -6, 7)),
                new SpriteFrame(32, 195, 27, 43, new Thickness(-6, 2, -5, 3)),
                new SpriteFrame(63, 195, 28, 42, new Thickness(-5, 1, -5, 3)),
                // Walljumping
                new SpriteFrame(94, 198, 30, 39, new Thickness(-1, -2, -6, 3)),
                new SpriteFrame(127, 194, 27, 44, new Thickness(-1, -6, -6, 6)),
                // Wallsliding(Shooting)
                new SpriteFrame(157, 199, 31, 39, new Thickness(-4, 0, -6, 1)),
                new SpriteFrame(200, 195, 32, 43, new Thickness(-6, 2, -5, -2)),
                new SpriteFrame(237, 195, 32, 42, new Thickness(-5, 1, -5, -1)),
                // Walljumping(Shooting)
                new SpriteFrame(94, 198, 30, 39, new Thickness(-1, -2, -6, 3)),
                new SpriteFrame(127, 194, 27, 44, new Thickness(-1, -6, -6, 6)),
                // Damaged
                new SpriteFrame(4, 378, 26, 36, new Thickness(-2, 3, -2, 3)),
                new SpriteFrame(35, 380, 29, 34, new Thickness(0, -2, 0, 5)),
                new SpriteFrame(70, 380, 29, 34, new Thickness(0, -2, 0, 5)),
                new SpriteFrame(107, 370, 32, 48, new Thickness(-10, -3, -4, 3)),
                new SpriteFrame(146, 380, 29, 34, new Thickness(0, -2, 0, 5)),
                new SpriteFrame(183, 370, 32, 48, new Thickness(-10, -3, -4, 3)),
                new SpriteFrame(221, 380, 29, 34, new Thickness(0, -2, 0, 5)),
                new SpriteFrame(256, 370, 32, 48, new Thickness(-10, -3, -4, 3)),
                new SpriteFrame(293, 380, 34, 34, new Thickness(0, -2, 0, 0)),
                new SpriteFrame(332, 379, 29, 35, new Thickness(-1, -2, 0, 5)),
                // Teleporting
                // TODO: Organize margins
                // TODO: Reorganize frames.
                new SpriteFrame(4, 14, 8, 48),
                new SpriteFrame(18, 33, 22, 29),
                new SpriteFrame(45, 20, 30, 42),
                new SpriteFrame(83, 23, 30, 39),
                new SpriteFrame(119, 26, 30, 36),
                new SpriteFrame(155, 28, 30, 34),
                new SpriteFrame(190, 30, 30, 32),
                // Winning stance
                new SpriteFrame(220, 255, 28, 45),
                new SpriteFrame(258, 255, 28, 45),
                new SpriteFrame(296, 252, 34, 48),
                new SpriteFrame(334, 255, 29, 45),
                new SpriteFrame(372, 225, 31, 45),
            ];
            SpriteSheet spriteSheet = new(texture, frames) { Origin = new(16, 16) };

            SoundEffect jumpSfx = content.Load<SoundEffect>("sfx/x-jump");
            SoundEffect landSfx = content.Load<SoundEffect>("sfx/x-land");
            SoundEffect dashSfx = content.Load<SoundEffect>("sfx/x-dash");
            SoundEffect hurtSfx = content.Load<SoundEffect>("sfx/x-hurt");

            SoundEffect chargingIntroSfx = content.Load<SoundEffect>("sfx/x-charge0");
            SoundEffect chargingLoopSfx = content.Load<SoundEffect>("sfx/x-charge1");


            return new(
                SpriteSheet: spriteSheet,
                JumpSoundEffect: jumpSfx,
                LandSoundEffect: landSfx,
                DashSoundEffect: dashSfx,
                HurtSoundEffect: hurtSfx,
                ChargingIntroSoundEffect: chargingIntroSfx,
                ChargingLoopSoundEffect: chargingLoopSfx
            );
        }
    }

    public class PlayerComponent : IComponent
    {
        public const int DASH_DURATION = 500;
        public const float WALKING_SPEED = 0.088125f;
        public const float JUMP_SPEED = 0.319453125f;
        public const float DASHING_SPEED = 0.207421875f;

        public Sprite Sprite { get; set; }
        public PhysicBody Body => physicsBody.Body;

        public PhysicSensor GroundSensor { get; }
        public PhysicSensor CeilingSensor { get; }
        public PhysicSensor LeftWallSensor { get; }
        public PhysicSensor RightWallSensor { get; }
        public PhysicSensor LeftWalljumpSensor { get; }
        public PhysicSensor RightWalljumpSensor { get; }

        public PlayerContent Content { get; }
        public PlayerInput Input { get; private set; }

        public PlayerState State { get; private set; } = PlayerState.Standing;
        public Vector2 Position { get => transform.Position; set => transform.Position = value; }
        public bool IsLeft { get; set; }

        private readonly LivingComponent living;
        private readonly TransformComponent transform;
        private readonly PhysicBodyComponent physicsBody;

        private bool isDashing;
        private int dashTimer;

        public PlayerComponent(Entity entity, IKeyboardDevice keyboard, PlayerContent content)
        {
            //Setup character's properties.
            // Health = MaxHealth = 10;
            // Bounds = new Rectangle(-8, -16, 16, 32);
            Content = content;

            Input = new PlayerInput(keyboard);

            transform = entity.GetComponent<TransformComponent>();
            living = entity.GetComponent<LivingComponent>();
            living.Damaged += OnDamage;
            physicsBody = entity.GetComponent<PhysicBodyComponent>();

            Sprite = new Sprite(content.SpriteSheet);

            // Setup PhysicsBody
            physicsBody.Body.UserData = this;
            physicsBody.Body.MaskBits = (ushort)CollisionFlags.Player;
            physicsBody.Body.CategoryBits = (ushort)CollisionFlags.All;

            GroundSensor = physicsBody.CreateSensor(new Rectangle(0, 32, 16, 1));
            CeilingSensor = physicsBody.CreateSensor(new Rectangle(0, -1, 16, 1));
            LeftWallSensor = physicsBody.CreateSensor(new Rectangle(-1, 0, 1, 32));
            RightWallSensor = physicsBody.CreateSensor(new Rectangle(16, 0, 1, 32));
            LeftWalljumpSensor = physicsBody.CreateSensor(new Rectangle(-7, 0, 7, 32));
            RightWalljumpSensor = physicsBody.CreateSensor(new Rectangle(16, 0, 7, 32));
        }

        int? IComponent.UpdatePriority => 0;
        void IComponent.Update(GameTime gameTime)
        {
            switch (State)
            {
                case PlayerState.Standing:
                    isDashing = false;
                    if (Input.IsMoving)
                    {
                        State = PlayerState.Walking;
                    }
                    break;
                case PlayerState.Walking:
                    isDashing = false;
                    if (!Input.IsMoving)
                    {
                        State = PlayerState.Standing;
                    }
                    break;
                case PlayerState.Jumping:
                    if (Body.Velocity.Y >= 0)
                    {
                        State = PlayerState.Falling;
                    }
                    break;
                case PlayerState.Falling:
                    if (GroundSensor)
                    {
                        State = Input.IsMoving ? PlayerState.Walking : PlayerState.Standing;
                        Content.LandSoundEffect.Play();
                    }
                    break;
                case PlayerState.Dashing:
                    dashTimer -= gameTime.ElapsedGameTime.Milliseconds;
                    if (dashTimer <= 0)
                    {
                        State = Input.IsMoving ? PlayerState.Walking : PlayerState.Standing;
                    }

                    if (IsLeft)
                    {
                        Body.Move(new Vector2(-DASHING_SPEED *
                            gameTime.ElapsedGameTime.Milliseconds, 0));
                        if (Input.Right)
                        {
                            State = Input.IsMoving ? PlayerState.Walking : PlayerState.Standing;

                        }
                    }
                    else
                    {
                        Body.Move(new Vector2(DASHING_SPEED *
      gameTime.ElapsedGameTime.Milliseconds, 0));
                        if (Input.Left)
                        {
                            State = Input.IsMoving ? PlayerState.Walking : PlayerState.Standing;

                        }
                    }

                    if (!Input.Dash)
                    {
                        State = Input.IsMoving ? PlayerState.Walking : PlayerState.Standing;
                    }
                    break;
            }

            if (CanStrafe)
            {
                float speed = isDashing ? DASHING_SPEED : WALKING_SPEED;
                if (Input.IsMovingLeft)
                {
                    IsLeft = true;
                    Body.Move(new Vector2(-speed *
                        gameTime.ElapsedGameTime.Milliseconds, 0));
                }
                else if (Input.IsMovingRight)
                {
                    IsLeft = false;
                    Body.Move(new Vector2(speed *
                        gameTime.ElapsedGameTime.Milliseconds, 0));
                }
            }

            if (CanDash && Input.ShouldDash)
            {
                State = PlayerState.Dashing;
                Content.DashSoundEffect.Play();
                dashTimer = DASH_DURATION;
                isDashing = true;
            }

            if (CanFall && !GroundSensor)
            {
                State = PlayerState.Falling;
            }

            if (CanJump && Input.ShouldJump)
            {
                State = PlayerState.Jumping;
                Content.JumpSoundEffect.Play();
                Body.Velocity -= new Vector2(0, JUMP_SPEED);
            }
        }

        int? IComponent.DrawPriority => 0;
        void IComponent.Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw sprite.
            Sprite.Position = Position;
            Sprite.Effects = IsLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Sprite.Draw(spriteBatch);
        }

        private void OnDamage(LivingComponent living, Entity _, DamageInfo info)
        {
            //Apply invencibility timer.
            if (living.InvincibilityTime <= 0 && !info.IgnoreInvincibility)
            {
                living.InvincibilityTime = 1000 + 533;
                //Change state.
                if ((info.Direction & Direction2D.Left) != 0)
                {
                    IsLeft = true;
                }
                else if ((info.Direction & Direction2D.Right) != 0)
                {
                    IsLeft = false;
                }
                else
                {
                }
            }
        }

        private bool CanStrafe => State is not PlayerState.Dashing;
        private bool CanFall => State is PlayerState.Standing or PlayerState.Walking or PlayerState.Dashing;
        private bool CanJump => State is PlayerState.Standing or PlayerState.Walking or PlayerState.Dashing;
        private bool CanDash => State is PlayerState.Standing or PlayerState.Walking;
    }
}

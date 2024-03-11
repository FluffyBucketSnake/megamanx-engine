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
    public enum PlayerInput
    {
        Left,
        Right,
        Jump,
        Dash,
        Fire
    }

    public record struct PlayerInputData(bool Left, bool Right, bool Jump, bool Dash, bool Fire)
    {
        public readonly bool IsMovingLeft => Left && !Right;
        public readonly bool IsMovingRight => !Left && Right;
        public readonly bool IsMoving => IsMovingLeft || IsMovingRight;
    }

    public enum PlayerState
    {
        Standing,
        Walking,
        Jumping,
        Falling
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
            SpriteSheet spriteSheet = new(texture, frames);

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
        public Sprite Sprite { get; set; }
        public PhysicBody Body { get; } = new PhysicBody(new Rectangle(-8, -16, 16, 32));

        public PhysicSensor GroundSensor { get; }
        public PhysicSensor CeilingSensor { get; }
        public PhysicSensor LeftWallSensor { get; }
        public PhysicSensor RightWallSensor { get; }
        public PhysicSensor LeftWalljumpSensor { get; }
        public PhysicSensor RightWalljumpSensor { get; }

        public PlayerContent Content { get; }
        public PlayerInputData CurrentInput => currentInput;

        public PlayerState State { get; private set; } = PlayerState.Standing;
        public Vector2 Position { get => transform.Position; set => transform.Position = value; }
        public bool IsLeft { get; set; }

        private PlayerInputData currentInput;

        private readonly LivingComponent living;
        private readonly TransformComponent transform;
        private readonly PhysicsBodyComponent physicsBody;

        public PlayerComponent(Entity entity, IKeyboardDevice keyboard, PlayerContent content)
        {
            //Setup character's properties.
            // Health = MaxHealth = 10;
            // Bounds = new Rectangle(-8, -16, 16, 32);
            Content = content;

            keyboard.KeyDown += OnKeyboardKeyDown;
            keyboard.KeyUp += OnKeyboardKeyUp;

            transform = entity.GetComponent<TransformComponent>();
            living = entity.GetComponent<LivingComponent>();
            living.Damaged += OnDamage;
            physicsBody = entity.GetComponent<PhysicsBodyComponent>();

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

        private void OnKeyboardKeyDown(KeyboardEventArgs args)
        {
            switch (args.Key)
            {
                case Keys.Left:
                    currentInput.Left = true;
                    break;
                case Keys.Right:
                    currentInput.Right = true;
                    break;
                case Keys.Z:
                    currentInput.Jump = true;
                    break;
                case Keys.A:
                    currentInput.Fire = true;
                    break;
                case Keys.X:
                    currentInput.Dash = true;
                    break;
            }
        }

        private void OnKeyboardKeyUp(KeyboardEventArgs args)
        {
            switch (args.Key)
            {
                case Keys.Left:
                    currentInput.Left = false;
                    break;
                case Keys.Right:
                    currentInput.Right = false;
                    break;
                case Keys.Z:
                    currentInput.Jump = false;
                    break;
                case Keys.A:
                    currentInput.Fire = false;
                    break;
                case Keys.X:
                    currentInput.Dash = false;
                    break;
            }
        }

        // public void LoadContent(ContentManager content)
        // {
        //     //Load sprites.
        //     BuildPlayerSprites(content);
        //     BuildBusterSprites(content);
        //
        //     //Load sound effects.
        //
        //     CurrentWeapon.ShootingSoundEffects[0] = content.Load<SoundEffect>("sfx/x-shoot0");
        //     CurrentWeapon.ShootingSoundEffects[1] = content.Load<SoundEffect>("sfx/x-shoot1");
        //     CurrentWeapon.ShootingSoundEffects[2] = content.Load<SoundEffect>("sfx/x-shoot2");
        //
        //     //Create main player controller.
        //     AnimationController = new PlayerAnimationController(this);
        //     AnimationController.State = PlayerAnimationStates.Idle;
        //
        // }

        // private void BuildPlayerSprites(ContentManager content)
        // {
        //     // Animations
        //     playerSheet.Animations.Add(new SpriteAnimation("idle", 0) { IsLooping = true });
        //     playerSheet.Animations.Add(new SpriteAnimation("idle-blink1",
        //     1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1));
        //     playerSheet.Animations.Add(new SpriteAnimation("idle-blink2",
        //     1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 1, 1, 1, 1));
        //
        //     playerSheet.Animations.Add(new SpriteAnimation("shoot",
        //     3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4, 4));
        //
        //     playerSheet.Animations.Add(new SpriteAnimation("step",
        //     5, 5, 5, 5, 5));
        //     playerSheet.Animations.Add(new SpriteAnimation("walk",
        //     6, 7, 7, 8, 8, 8, 9, 9, 9, 10, 10, 10, 11, 11, 12, 12, 13, 13, 13, 14, 14, 14, 15, 15, 15)
        //     { IsLooping = true });
        //     playerSheet.Animations.Add(new SpriteAnimation("walk-shoot",
        //     16, 17, 17, 18, 18, 18, 19, 19, 19, 20, 20, 20, 21, 21, 22, 22, 23, 23, 23, 24, 24, 24, 25, 25, 25)
        //     { IsLooping = true });
        //
        //     playerSheet.Animations.Add(new SpriteAnimation("jump-intro",
        //     26, 26, 26, 27, 27, 27, 27));
        //     playerSheet.Animations.Add(new SpriteAnimation("jump",
        //     28));
        //     playerSheet.Animations.Add(new SpriteAnimation("fall-intro",
        //     28, 28, 29, 29, 29));
        //     playerSheet.Animations.Add(new SpriteAnimation("fall",
        //     30));
        //     playerSheet.Animations.Add(new SpriteAnimation("land",
        //     30, 31, 31, 32));
        //
        //     playerSheet.Animations.Add(new SpriteAnimation("jump-intro-shoot",
        //     33, 33, 33, 34, 34, 34, 34));
        //     playerSheet.Animations.Add(new SpriteAnimation("jump-shoot",
        //     35));
        //     playerSheet.Animations.Add(new SpriteAnimation("fall-intro-shoot",
        //     35, 35, 36, 36, 36));
        //     playerSheet.Animations.Add(new SpriteAnimation("fall-shoot",
        //     37));
        //     playerSheet.Animations.Add(new SpriteAnimation("land-shoot",
        //     37, 38, 38, 39));
        //
        //     playerSheet.Animations.Add(new SpriteAnimation("dash-intro",
        //     40, 40, 40));
        //     playerSheet.Animations.Add(new SpriteAnimation("dash",
        //     41));
        //     playerSheet.Animations.Add(new SpriteAnimation("dash-outro",
        //     41, 40, 40, 40, 40, 40, 40, 40, 40));
        //     playerSheet.Animations.Add(new SpriteAnimation("dash-intro-shoot",
        //     42, 42, 42));
        //     playerSheet.Animations.Add(new SpriteAnimation("dash-shoot",
        //     43));
        //     playerSheet.Animations.Add(new SpriteAnimation("dash-outro-shoot",
        //     43, 42, 42, 42, 42, 42, 42, 42, 42));
        //
        //     playerSheet.Animations.Add(new SpriteAnimation("wallslide-intro",
        //     44, 44, 44, 44, 44, 45, 45, 45, 45, 45, 45));
        //     playerSheet.Animations.Add(new SpriteAnimation("wallslide",
        //     46));
        //     playerSheet.Animations.Add(new SpriteAnimation("walljump",
        //     47, 47, 47, 48));
        //
        //     playerSheet.Animations.Add(new SpriteAnimation("wallslide-intro-shoot",
        //     49, 49, 49, 49, 49, 50, 50, 50, 50, 50, 50));
        //     playerSheet.Animations.Add(new SpriteAnimation("wallslide-shoot",
        //     51));
        //     playerSheet.Animations.Add(new SpriteAnimation("walljump-shoot",
        //     52, 52, 52, 53));
        //
        //     playerSheet.Animations.Add(new SpriteAnimation("hurt",
        //     54, 54, 54, 54, 55, 56, 56, 57, 57, 58, 58, 59, 59, 60, 60, 61, 61, 62, 62, 62, 62, 63, 55, 55, 55, 55, 55, 55, 55, 55, 54, 54));
        // }

        // private void BuildBusterSprites(ContentManager content)
        // {
        //     CurrentWeapon.ProjectileSpriteSheets[0] = new SpriteSheet(content.Load<Texture2D>("textures/projectile-default0"), 1)
        //     {
        //         Origin = new Vector2(4, 3)
        //     };
        //     CurrentWeapon.ProjectileSpriteSheets[0].Frames[0] = new SpriteFrame(new Rectangle(0, 4, 8, 6));
        //
        //     CurrentWeapon.ProjectileSpriteSheets[1] = new SpriteSheet(content.Load<Texture2D>("textures/projectile-default1"), 3)
        //     {
        //         Origin = new Vector2(8, 6)
        //     };
        //     CurrentWeapon.ProjectileSpriteSheets[1].Frames[0] = new SpriteFrame(new Rectangle(100, 1, 40, 19), new Thickness(-5, -1, -2, -23));
        //     CurrentWeapon.ProjectileSpriteSheets[1].Frames[1] = new SpriteFrame(new Rectangle(140, 1, 36, 22), new Thickness(-4, 0, -6, -20));
        //     CurrentWeapon.ProjectileSpriteSheets[1].Frames[2] = new SpriteFrame(new Rectangle(176, 6, 38, 12), new Thickness(0, 0, 0, -23));
        //     CurrentWeapon.ProjectileSpriteSheets[1].Animations.Add(new SpriteAnimation("default", 2, 0, 0, 2, 1, 1) { IsLooping = true });
        //
        //     CurrentWeapon.ProjectileSpriteSheets[2] = new SpriteSheet(content.Load<Texture2D>("textures/projectile-default2"), 3)
        //     {
        //         Origin = new Vector2(12)
        //     };
        //     CurrentWeapon.ProjectileSpriteSheets[2].Frames[0] = new SpriteFrame(new Rectangle(37, 0, 32, 32), new Thickness(-4, -8, -4, 0));
        //     CurrentWeapon.ProjectileSpriteSheets[2].Frames[1] = new SpriteFrame(new Rectangle(69, 0, 40, 32), new Thickness(-3, -9, -4, -7));
        //     CurrentWeapon.ProjectileSpriteSheets[2].Frames[2] = new SpriteFrame(new Rectangle(109, 4, 27, 24));
        //     CurrentWeapon.ProjectileSpriteSheets[2].Animations.Add(new SpriteAnimation("default", 0, 0, 2, 1, 1, 2) { IsLooping = true });
        // }

        void IComponent.Update(GameTime gameTime)
        {
        }

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
    }
}

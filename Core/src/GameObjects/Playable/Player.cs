// using CatalysisEngine.Data;
using MegamanX.Input.Keyboard;
using MegamanX.GameObjects.Playable.States;
using MegamanX.Graphics;
using MegamanX.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MegamanX.GameObjects.Playable
{
    public class Player : Entity
    {
        private PlayerState currentState;
        private PlayerWeapon currentWeapon;

        public SoundEffect JumpSoundEffect;
        public SoundEffect LandSoundEffect;
        public SoundEffect DashSoundEffect;
        public SoundEffect HurtSoundEffect;
        IKeyboardDevice keyboard;

        public Player()
        {
            //Setup character's properties.
            Health = MaxHealth = 10;
            Bounds = new Rectangle(-8, -16, 16, 32);
            Physics = new PlayerPhysics(this);

            //Setup state machine.
            currentState = new StandingState();
            currentState.Parent = this;

            //Setup default buster.
            CurrentWeapon = new PlayerWeapon();
        }

        public Player(IKeyboardDevice keyboard) : this()
        {
            //Setup input.
            this.keyboard = keyboard;
            keyboard.KeyDown += OnKeyboardKeyDown;
            keyboard.KeyUp += OnKeyboardKeyUp;
        }

        public PlayerPhysics Physics { get; }
        public PlayerInputData CurrentInput = new PlayerInputData();
        public PlayerState CurrentState
        {
            get => currentState;
            set
            {
                if (value != currentState)
                {
                    States.PlayerState oldState = currentState;
                    States.PlayerState newState = value;

                    if (currentState != null)
                    {
                        oldState.OnStateLeave(new StateChangeInfo(oldState, newState));
                    }

                    currentState = newState;

                    newState.Parent = this;
                    newState.OnStateEnter(new StateChangeInfo(oldState, newState));
                }
            }
        }
        public PlayerWeapon CurrentWeapon
        {
            get => currentWeapon;
            set
            {
                if (currentWeapon != null)
                {
                    currentWeapon.Parent = null;
                }
                currentWeapon = value;
                if (value != null)
                {
                    value.Parent = this;
                }
            }
        }
        public PlayerAnimationController AnimationController { get; private set; }
        public Sprite Sprite { get; private set; }
        public bool IsLeft { get; set; } = false;

        private void OnKeyboardKeyDown(KeyboardEventArgs args)
        {
            switch (args.Key)
            {
                case Keys.Left:
                    CurrentInput.Left = true;
                    CurrentState.OnInputEnter(PlayerInput.Left);
                    break;
                case Keys.Right:
                    CurrentInput.Right = true;
                    CurrentState.OnInputEnter(PlayerInput.Right);
                    break;
                case Keys.Z:
                    CurrentInput.Jump = true;
                    CurrentState.OnInputEnter(PlayerInput.Jump);
                    break;
                case Keys.A:
                    CurrentInput.Fire = true;
                    CurrentState.OnInputEnter(PlayerInput.Fire);
                    break;
                case Keys.X:
                    CurrentInput.Dash = true;
                    CurrentState.OnInputEnter(PlayerInput.Dash);
                    break;
            }
        }

        private void OnKeyboardKeyUp(KeyboardEventArgs args)
        {
            switch (args.Key)
            {
                case Keys.Left:
                    CurrentInput.Left = false;
                    CurrentState.OnInputLeave(PlayerInput.Left);
                    break;
                case Keys.Right:
                    CurrentInput.Right = false;
                    CurrentState.OnInputLeave(PlayerInput.Right);
                    break;
                case Keys.Z:
                    CurrentInput.Jump = false;
                    CurrentState.OnInputLeave(PlayerInput.Jump);
                    break;
                case Keys.A:
                    CurrentInput.Fire = false;
                    CurrentState.OnInputLeave(PlayerInput.Fire);
                    break;
                case Keys.X:
                    CurrentInput.Dash = false;
                    CurrentState.OnInputLeave(PlayerInput.Dash);
                    break;
            }
        }

        public override void LoadContent(ContentManager content)
        {
            //Load sprites.
            BuildPlayerSprites(content);
            BuildBusterSprites(content);

            //Load sound effects.
            JumpSoundEffect = content.Load<SoundEffect>("sfx/x-jump");
            LandSoundEffect = content.Load<SoundEffect>("sfx/x-land");
            DashSoundEffect = content.Load<SoundEffect>("sfx/x-dash");
            HurtSoundEffect = content.Load<SoundEffect>("sfx/x-hurt");

            CurrentWeapon.ShootingSoundEffects[0] = content.Load<SoundEffect>("sfx/x-shoot0");
            CurrentWeapon.ShootingSoundEffects[1] = content.Load<SoundEffect>("sfx/x-shoot1");
            CurrentWeapon.ShootingSoundEffects[2] = content.Load<SoundEffect>("sfx/x-shoot2");
            CurrentWeapon.ChargingSoundEffects[0] = content.Load<SoundEffect>("sfx/x-charge0");
            CurrentWeapon.ChargingSoundEffects[1] = content.Load<SoundEffect>("sfx/x-charge1");

            //Create main player controller.
            AnimationController = new PlayerAnimationController(this);
            AnimationController.State = PlayerAnimationStates.Idle;
        }

        private void BuildPlayerSprites(ContentManager content)
        {
            var playerSheet = new SpriteSheet(content.Load<Texture2D>("textures/player-x"), 64);
            playerSheet.Origin = new Vector2(16, 16);
            
            // Standing
            playerSheet.Frames[0] = new SpriteFrame(225, 28, 30, 34, new Thickness(-2, 1, 0, 1));
            playerSheet.Frames[1] = new SpriteFrame(260, 28, 30, 34, new Thickness(-2, 1, 0, 1));
            playerSheet.Frames[2] = new SpriteFrame(294, 28, 30, 34, new Thickness(-2, 1, 0, 1));

            // Shooting
            playerSheet.Frames[3] = new SpriteFrame(364, 28, 30, 34, new Thickness(-2, 1, 0, 1));
            playerSheet.Frames[4] = new SpriteFrame(401, 28, 30, 34, new Thickness(-2, 1, 0, 1));

            // Step
            playerSheet.Frames[5] = new SpriteFrame(4, 66, 30, 34, new Thickness(-2, 1, 0, 1));

            // Walking
            playerSheet.Frames[6] = new SpriteFrame(  49, 66, 20, 34, new Thickness(-2,  7,  0,  5));
            playerSheet.Frames[7] = new SpriteFrame(  74, 66, 23, 35, new Thickness(-2,  4, -1,  5));
            playerSheet.Frames[8] = new SpriteFrame( 104, 67, 32, 34, new Thickness(-1,  0, -1,  0));
            playerSheet.Frames[9] = new SpriteFrame( 144, 67, 34, 33, new Thickness(-1, -2,  0,  0));
            playerSheet.Frames[10] = new SpriteFrame(189, 67, 26, 33, new Thickness(-1,  2,  0,  4));
            playerSheet.Frames[11] = new SpriteFrame(221, 66, 22, 34, new Thickness(-2,  5,  0,  5));
            playerSheet.Frames[12] = new SpriteFrame(247, 66, 25, 35, new Thickness(-2,  3, -1,  4));
            playerSheet.Frames[13] = new SpriteFrame(279, 66, 30, 34, new Thickness(-2,  2,  0,  0));
            playerSheet.Frames[14] = new SpriteFrame(317, 67, 34, 33, new Thickness(-1, -1,  0, -1));
            playerSheet.Frames[15] = new SpriteFrame(358, 67, 30, 33, new Thickness(-1,  1,  0,  2));

            // Walking(Shooting)
            playerSheet.Frames[16] = new SpriteFrame( 40, 106, 29, 34, new Thickness(-2,  7,  0, -4));
            playerSheet.Frames[17] = new SpriteFrame( 75, 106, 32, 35, new Thickness(-2,  4, -1, -4));
            playerSheet.Frames[18] = new SpriteFrame(114, 107, 35, 34, new Thickness(-1,  0, -1, -3));
            playerSheet.Frames[19] = new SpriteFrame(158, 107, 38, 33, new Thickness(-1, -2,  0, -4));
            playerSheet.Frames[20] = new SpriteFrame(203, 107, 34, 33, new Thickness(-1,  2,  0, -4));
            playerSheet.Frames[21] = new SpriteFrame(245, 106, 31, 34, new Thickness(-2,  5,  0, -4));
            playerSheet.Frames[22] = new SpriteFrame(283, 106, 33, 35, new Thickness(-2,  3, -1, -4));
            playerSheet.Frames[23] = new SpriteFrame(325, 106, 35, 34, new Thickness(-2,  2,  0, -5));
            playerSheet.Frames[24] = new SpriteFrame(368, 107, 37, 33, new Thickness(-1, -1,  0, -4));
            playerSheet.Frames[25] = new SpriteFrame(412, 107, 35, 33, new Thickness(-1,  1,  0, -4));

            // Jumping
            playerSheet.Frames[26] = new SpriteFrame( 5, 147, 24, 37, new Thickness(-5, 11,  0, -3));
            playerSheet.Frames[27] = new SpriteFrame(36, 147, 15, 41, new Thickness(-5, 14, -4,  3));
            playerSheet.Frames[28] = new SpriteFrame(55, 145, 19, 46, new Thickness(-7, 10, -7,  3));

            // Falling
            playerSheet.Frames[29] = new SpriteFrame( 79, 149, 23, 41, new Thickness(-3, 8, -6, 1));
            playerSheet.Frames[30] = new SpriteFrame(107, 149, 27, 42, new Thickness(-3, 5, -7, 0));
           
            // Landing
            playerSheet.Frames[31] = new SpriteFrame(138, 150, 24, 38, new Thickness(-2, 9, -1, -4));
            playerSheet.Frames[32] = new SpriteFrame(165, 152, 30, 32, new Thickness( 0, 2,  0,  0));

            // Jumping(Shooting)
            playerSheet.Frames[33] = new SpriteFrame(200, 147, 29, 37, new Thickness(-5, 11,  0, -8));
            playerSheet.Frames[34] = new SpriteFrame(239, 147, 24, 41, new Thickness(-5, 14, -4, -6));
            playerSheet.Frames[35] = new SpriteFrame(270, 145, 27, 46, new Thickness(-7, 10, -7, -5));

            // Falling(Shooting)
            playerSheet.Frames[36] = new SpriteFrame(303, 149, 31, 41, new Thickness(-3, 8, -6, 1));
            playerSheet.Frames[37] = new SpriteFrame(340, 149, 31, 42, new Thickness(-3, 5, -7, 0));

            // Landing(Shooting)
            playerSheet.Frames[38] = new SpriteFrame(138, 150, 24, 38, new Thickness(-2, 9, -1, -10));
            playerSheet.Frames[39] = new SpriteFrame(165, 152, 30, 32, new Thickness( 0, 2,  0,  -6));

            // Dashing
            playerSheet.Frames[40] = new SpriteFrame( 3, 334, 28, 31, new Thickness(1, 6, 0, -2));
            playerSheet.Frames[41] = new SpriteFrame(33, 340, 38, 26, new Thickness(7, 1, 0, -7));

            // Dashing(Shooting)
            playerSheet.Frames[42] = new SpriteFrame( 75, 334, 37, 31, new Thickness(1, 6, 0, -11));
            playerSheet.Frames[43] = new SpriteFrame(114, 340, 48, 26, new Thickness(7, 1, 0, -17));

            // Wallsliding
            playerSheet.Frames[44] = new SpriteFrame( 4, 196, 24, 42, new Thickness(-4, 0, -6, 7));
            playerSheet.Frames[45] = new SpriteFrame(32, 195, 27, 43, new Thickness(-6, 2, -5, 3));
            playerSheet.Frames[46] = new SpriteFrame(63, 195, 28, 42, new Thickness(-5, 1, -5, 3));

            // Walljumping
            playerSheet.Frames[47] = new SpriteFrame( 94, 198, 30, 39, new Thickness(-1, -2, -6, 3));
            playerSheet.Frames[48] = new SpriteFrame(127, 194, 27, 44, new Thickness(-1, -6, -6, 6));

            // Wallsliding(Shooting)
            playerSheet.Frames[49] = new SpriteFrame(157, 199, 31, 39, new Thickness(-4, 0, -6,  1));
            playerSheet.Frames[50] = new SpriteFrame(200, 195, 32, 43, new Thickness(-6, 2, -5, -2));
            playerSheet.Frames[51] = new SpriteFrame(237, 195, 32, 42, new Thickness(-5, 1, -5, -1));

            // Walljumping(Shooting)
            playerSheet.Frames[52] = new SpriteFrame( 94, 198, 30, 39, new Thickness(-1, -2, -6, 3));
            playerSheet.Frames[53] = new SpriteFrame(127, 194, 27, 44, new Thickness(-1, -6, -6, 6));

            // Damaged
            playerSheet.Frames[54] = new SpriteFrame(  4, 378, 26, 36, new Thickness( -2,  3, -2, 3));
            playerSheet.Frames[55] = new SpriteFrame( 35, 380, 29, 34, new Thickness(  0, -2,  0, 5));
            playerSheet.Frames[56] = new SpriteFrame( 70, 380, 29, 34, new Thickness(  0, -2,  0, 5));
            playerSheet.Frames[57] = new SpriteFrame(107, 370, 32, 48, new Thickness(-10, -3, -4, 3));
            playerSheet.Frames[58] = new SpriteFrame(146, 380, 29, 34, new Thickness(  0, -2,  0, 5));
            playerSheet.Frames[59] = new SpriteFrame(183, 370, 32, 48, new Thickness(-10, -3, -4, 3));
            playerSheet.Frames[60] = new SpriteFrame(221, 380, 29, 34, new Thickness(  0, -2,  0, 5));
            playerSheet.Frames[61] = new SpriteFrame(256, 370, 32, 48, new Thickness(-10, -3, -4, 3));
            playerSheet.Frames[62] = new SpriteFrame(293, 380, 34, 34, new Thickness(  0, -2,  0, 0));
            playerSheet.Frames[63] = new SpriteFrame(332, 379, 29, 35, new Thickness( -1, -2,  0, 5));

            // Animations
            playerSheet.Animations.Add(new SpriteAnimation("idle", 0) { IsLooping = true });
            playerSheet.Animations.Add(new SpriteAnimation("idle-blink1",
            1,1,1,1,2,2,2,2,2,2,2,2,1,1,1,1));
            playerSheet.Animations.Add(new SpriteAnimation("idle-blink2",
            1,1,1,1,2,2,2,2,1,1,1,1,0,0,0,0,1,1,1,1,2,2,2,2,1,1,1,1));

            playerSheet.Animations.Add(new SpriteAnimation("shoot",
            3,3,3,4,4,4,4,4,4,4,4,4,4,4));

            playerSheet.Animations.Add(new SpriteAnimation("step",
            5,5,5,5,5));
            playerSheet.Animations.Add(new SpriteAnimation("walk",
            6,7,7,8,8,8,9,9,9,10,10,10,11,11,12,12,13,13,13,14,14,14,15,15,15) { IsLooping = true });
            playerSheet.Animations.Add(new SpriteAnimation("walk-shoot",
            16,17,17,18,18,18,19,19,19,20,20,20,21,21,22,22,23,23,23,24,24,24,25,25,25) { IsLooping = true });

            playerSheet.Animations.Add(new SpriteAnimation("jump-intro",
            26,26,26,27,27,27,27));
            playerSheet.Animations.Add(new SpriteAnimation("jump",
            28));
            playerSheet.Animations.Add(new SpriteAnimation("fall-intro",
            28,28,29,29,29));
            playerSheet.Animations.Add(new SpriteAnimation("fall",
            30));
            playerSheet.Animations.Add(new SpriteAnimation("land",
            30,31,31,32));

            playerSheet.Animations.Add(new SpriteAnimation("jump-intro-shoot",
            33,33,33,34,34,34,34));
            playerSheet.Animations.Add(new SpriteAnimation("jump-shoot",
            35));
            playerSheet.Animations.Add(new SpriteAnimation("fall-intro-shoot",
            35,35,36,36,36));
            playerSheet.Animations.Add(new SpriteAnimation("fall-shoot",
            37));
            playerSheet.Animations.Add(new SpriteAnimation("land-shoot",
            37,38,38,39));

            playerSheet.Animations.Add(new SpriteAnimation("dash-intro",
            40,40,40));
            playerSheet.Animations.Add(new SpriteAnimation("dash",
            41));
            playerSheet.Animations.Add(new SpriteAnimation("dash-outro",
            41,40,40,40,40,40,40,40,40));
            playerSheet.Animations.Add(new SpriteAnimation("dash-intro-shoot",
            42,42,42));
            playerSheet.Animations.Add(new SpriteAnimation("dash-shoot",
            43));
            playerSheet.Animations.Add(new SpriteAnimation("dash-outro-shoot",
            43,42,42,42,42,42,42,42,42));

            playerSheet.Animations.Add(new SpriteAnimation("wallslide-intro",
            44,44,44,44,44,45,45,45,45,45,45));
            playerSheet.Animations.Add(new SpriteAnimation("wallslide",
            46));
            playerSheet.Animations.Add(new SpriteAnimation("walljump",
            47,47,47,48));

            playerSheet.Animations.Add(new SpriteAnimation("wallslide-intro-shoot",
            49,49,49,49,49,50,50,50,50,50,50));
            playerSheet.Animations.Add(new SpriteAnimation("wallslide-shoot",
            51));
            playerSheet.Animations.Add(new SpriteAnimation("walljump-shoot",
            52,52,52,53));

            playerSheet.Animations.Add(new SpriteAnimation("hurt",
            54,54,54,54,55,56,56,57,57,58,58,59,59,60,60,61,61,62,62,62,62,63,55,55,55,55,55,55,55,55,54,54));

            Sprite = new Sprite(playerSheet);
        }

        private void BuildBusterSprites(ContentManager content)
        {
            CurrentWeapon.ProjectileSpriteSheets[0] = new SpriteSheet(content.Load<Texture2D>("textures/projectile-default0"), 1)
            {
                Origin = new Vector2(4, 3)
            };
            CurrentWeapon.ProjectileSpriteSheets[0].Frames[0] = new SpriteFrame(new Rectangle(0, 4, 8, 6));

            CurrentWeapon.ProjectileSpriteSheets[1] = new SpriteSheet(content.Load<Texture2D>("textures/projectile-default1"), 3)
            {
                Origin = new Vector2(8, 6)
            };
            CurrentWeapon.ProjectileSpriteSheets[1].Frames[0] = new SpriteFrame(new Rectangle(100, 1, 40, 19), new Thickness(-5, -1, -2, -23));
            CurrentWeapon.ProjectileSpriteSheets[1].Frames[1] = new SpriteFrame(new Rectangle(140, 1, 36, 22), new Thickness(-4, 0, -6, -20));
            CurrentWeapon.ProjectileSpriteSheets[1].Frames[2] = new SpriteFrame(new Rectangle(176, 6, 38, 12), new Thickness(0, 0, 0, -23));
            CurrentWeapon.ProjectileSpriteSheets[1].Animations.Add(new SpriteAnimation("default", 2, 0, 0, 2, 1, 1) { IsLooping = true });

            CurrentWeapon.ProjectileSpriteSheets[2] = new SpriteSheet(content.Load<Texture2D>("textures/projectile-default2"), 3)
            {
                Origin = new Vector2(12)
            };
            CurrentWeapon.ProjectileSpriteSheets[2].Frames[0] = new SpriteFrame(new Rectangle(37, 0, 32, 32), new Thickness(-4, -8, -4, 0));
            CurrentWeapon.ProjectileSpriteSheets[2].Frames[1] = new SpriteFrame(new Rectangle(69, 0, 40, 32), new Thickness(-3, -9, -4, -7));
            CurrentWeapon.ProjectileSpriteSheets[2].Frames[2] = new SpriteFrame(new Rectangle(109, 4, 27, 24));
            CurrentWeapon.ProjectileSpriteSheets[2].Animations.Add(new SpriteAnimation("default", 0, 0, 2, 1, 1, 2) { IsLooping = true });
        }

        public override void Update(GameTime gameTime)
        {
            // Update position.
            Position = Physics.Body.Position;

            // Call base method.
            base.Update(gameTime);

            // Update state machine.
            CurrentState?.Update(gameTime);

            // Update weapon.
            if (CurrentInput.Fire)
            {
                CurrentWeapon?.Charge(gameTime);
            }
            else
            {
                CurrentWeapon.ResetCharge();
            }

            // Update animation controller.
            AnimationController.Update(gameTime);

            // Update sprite.
            Sprite.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draw sprite.
            Sprite.Position = Position;
            Sprite.Effects = IsLeft ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
            Sprite.Draw(spriteBatch);
        }

        protected override void OnCreation(object sender)
        {
            base.OnCreation(sender);
            Physics.RegisterWorld(Map.World);
        }

        protected override void OnDestruction(object sender)
        {
            base.OnDestruction(sender);
            Physics.UnregisterWorld(Map.World);
            keyboard.KeyDown -= OnKeyboardKeyDown;
            keyboard.KeyUp -= OnKeyboardKeyUp;
        }

        protected override void OnPositionChange(Physics.PositionChangedArgs e)
        {
            Physics.Body.Position = e.NewPosition;
        }

        protected override void OnDamage(DamageInfo info)
        {
            //Apply invencibility timer.
            if (InvincibilityTime <= 0 && !info.IgnoreInvincibility)
            {
                InvincibilityTime = 1000 + 533;
                //Change state.
                if ((info.Direction & Direction2D.Left) != 0)
                {
                    IsLeft = true;
                    CurrentState = new DamagedState(Physics.Parameters.LeftKnockbackSpeed,
                        533);
                }
                else if ((info.Direction & Direction2D.Right) != 0)
                {
                    IsLeft = false;
                    CurrentState = new DamagedState(Physics.Parameters.RightKnockbackSpeed,
                        533);
                }
                else
                {
                    CurrentState = new DamagedState(Physics.Parameters.CenterKnockbackSpeed,
                        533);
                }
            }
        }
    }
}
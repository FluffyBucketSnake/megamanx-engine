using System;
using MegamanX.Graphics;
using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable
{
    public enum PlayerAnimationStates
    {
        Idle,
        Walk,
        Jump,
        Fall,
        Dash,
        Wallslide,
        Walljump,
        Hurt
    }

    public class PlayerAnimationController
    {
        private PlayerAnimationStates _currentState = PlayerAnimationStates.Idle;
        private bool _isPlayingIntro = false;
        private float _shootTimer = 0.0f;

        public PlayerAnimationController(Player parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Parent.Sprite.OnAnimationComplete += OnAnimationComplete;
        }

        public PlayerAnimationStates State
        {
            get => _currentState;
            set
            {
                if (_currentState != value)
                {
                    switch (value)
                    {
                        case PlayerAnimationStates.Idle:
                            if (State == PlayerAnimationStates.Dash)
                            {
                                _isPlayingIntro = true;
                                PlayAnimation("dash-outro");
                            }
                            else if (State == PlayerAnimationStates.Jump)
                            {
                                _isPlayingIntro = true;
                                PlayAnimation("land");
                                Parent.LandSoundEffect?.Play();
                            }
                            else if (IsShooting)
                            {
                                Parent.Sprite.TryPlay("shoot");
                                Parent.Sprite.CurrentTime = Math.Max(0, ShootAnimationDuration - _shootTimer);
                            }
                            else
                            {
                                Parent.Sprite.TryPlay("idle");
                            }
                            break;
                        case PlayerAnimationStates.Walk:
                            if (IsShooting)
                            {
                                Parent.Sprite.TryPlay("walk-shoot");
                            }
                            else if (_currentState == PlayerAnimationStates.Idle)
                            {
                                _isPlayingIntro = true;
                                Parent.Sprite.TryPlay("step");
                            }
                            else
                            {
                                Parent.Sprite.TryPlay("walk");
                            }
                            break;
                        case PlayerAnimationStates.Jump:
                            _isPlayingIntro = true;
                            PlayAnimation("jump-intro");
                            if (_currentState != PlayerAnimationStates.Walljump)
                            {
                                Parent.JumpSoundEffect?.Play();
                            }
                            break;
                        case PlayerAnimationStates.Fall:
                            _isPlayingIntro = true;
                            PlayAnimation("fall-intro");
                            break;
                        case PlayerAnimationStates.Dash:
                            _isPlayingIntro = true;
                            PlayAnimation("dash-intro");
                            Parent.DashSoundEffect?.Play();
                            break;
                        case PlayerAnimationStates.Wallslide:
                            _isPlayingIntro = true;
                            PlayAnimation("wallslide-intro");
                            break;
                        case PlayerAnimationStates.Walljump:
                            PlayAnimation("walljump");
                            Parent.JumpSoundEffect?.Play();
                            break;
                        case PlayerAnimationStates.Hurt:
                            _shootTimer = 0.0f;
                            Parent.Sprite.TryPlay("hurt");
                            Parent.HurtSoundEffect?.Play();
                            break;
                    }

                    PreviousState = _currentState;
                    _currentState = value;
                }
            }
        }
        public bool IsShooting => _shootTimer > 0;
        public Player Parent { get; }
        public PlayerAnimationStates PreviousState { get; private set; }
        public float ShootAnimationDuration = 233.0f;

        public void Shoot()
        {
            _shootTimer = ShootAnimationDuration;
            float time = Parent.Sprite.CurrentTime;

            switch (State)
            {
                case PlayerAnimationStates.Idle:
                    if (_isPlayingIntro && PreviousState == PlayerAnimationStates.Dash)
                    {
                        Parent.Sprite.TryPlay("dash-outro-shoot");
                        break;
                    }
                    else
                    {
                        Parent.Sprite.TryPlay("shoot");
                        return;
                    }
                case PlayerAnimationStates.Walk:
                    Parent.Sprite.TryPlay("walk-shoot");
                    break;
                case PlayerAnimationStates.Jump:
                    if (_isPlayingIntro)
                    {
                        Parent.Sprite.TryPlay("jump-intro-shoot");
                    }
                    else
                    {
                        Parent.Sprite.TryPlay("jump-shoot");
                    }
                    break;
                case PlayerAnimationStates.Fall:
                    if (_isPlayingIntro)
                    {
                        Parent.Sprite.TryPlay("fall-intro-shoot");
                    }
                    else
                    {
                        Parent.Sprite.TryPlay("fall-shoot");
                    }
                    break;
                case PlayerAnimationStates.Dash:
                    if (_isPlayingIntro)
                    {
                        Parent.Sprite.TryPlay("dash-intro-shoot");
                    }
                    else
                    {
                        Parent.Sprite.TryPlay("dash-shoot");
                    }
                    break;
                case PlayerAnimationStates.Wallslide:
                    if (_isPlayingIntro)
                    {
                        Parent.Sprite.TryPlay("wallslide-intro-shoot");
                    }
                    else
                    {
                        Parent.Sprite.TryPlay("wallslide-shoot");
                    }
                    break;
                case PlayerAnimationStates.Walljump:
                    Parent.Sprite.TryPlay("walljump-shoot");
                    break;
            }

            Parent.Sprite.CurrentTime = time;
        }

        public void Update(GameTime gameTime)
        {
            if (IsShooting)
            {
                _shootTimer -= gameTime.ElapsedGameTime.Milliseconds;
                if (_shootTimer <= 0)
                {
                    StopShootAnimation();
                }
            }
        }

        private void PlayAnimation(string animationName)
        {
            Parent.Sprite.TryPlay(!IsShooting ? animationName : $"{animationName}-shoot");
        }

        private void OnAnimationComplete(object sender, AnimationEventArgs e)
        {
            if (_isPlayingIntro)
            {
                switch (_currentState)
                {
                    case PlayerAnimationStates.Idle:
                        if (IsShooting)
                        {
                            Parent.Sprite.TryPlay("shoot");
                            Parent.Sprite.CurrentTime = Math.Max(0, ShootAnimationDuration - _shootTimer);
                        }
                        else
                        {
                            Parent.Sprite.TryPlay("idle");
                        }
                        break;
                    case PlayerAnimationStates.Walk:
                        PlayAnimation("walk");
                        break;
                    case PlayerAnimationStates.Jump:
                        PlayAnimation("jump");
                        break;
                    case PlayerAnimationStates.Fall:
                        PlayAnimation("fall");
                        break;
                    case PlayerAnimationStates.Dash:
                        PlayAnimation("dash");
                        break;
                    case PlayerAnimationStates.Wallslide:
                        PlayAnimation("wallslide");
                        break;
                }

                _isPlayingIntro = false;
            }
        }

        private void StopShootAnimation()
        {
            _shootTimer = 0.0f;
            float time = Parent.Sprite.CurrentTime;

            switch (State)
            {
                case PlayerAnimationStates.Idle:
                    if (_isPlayingIntro && PreviousState == PlayerAnimationStates.Dash)
                    {
                        Parent.Sprite.TryPlay("dash-outro");
                        break;
                    }
                    else
                    {
                        Parent.Sprite.TryPlay("idle");
                        return;
                    }
                case PlayerAnimationStates.Walk:
                    Parent.Sprite.TryPlay("walk");
                    break;
                case PlayerAnimationStates.Jump:
                    if (_isPlayingIntro)
                    {
                        Parent.Sprite.TryPlay("jump-intro");
                    }
                    else
                    {
                        Parent.Sprite.TryPlay("jump");
                    }
                    break;
                case PlayerAnimationStates.Fall:
                    if (_isPlayingIntro)
                    {
                        Parent.Sprite.TryPlay("fall-intro");
                    }
                    else
                    {
                        Parent.Sprite.TryPlay("fall");
                    }
                    break;
                case PlayerAnimationStates.Dash:
                    if (_isPlayingIntro)
                    {
                        Parent.Sprite.TryPlay("dash-intro");
                    }
                    else
                    {
                        Parent.Sprite.TryPlay("dash");
                    }
                    break;
                case PlayerAnimationStates.Wallslide:
                    if (_isPlayingIntro)
                    {
                        Parent.Sprite.TryPlay("wallslide-intro");
                    }
                    else
                    {
                        Parent.Sprite.TryPlay("wallslide");
                    }
                    break;
                case PlayerAnimationStates.Walljump:
                    Parent.Sprite.TryPlay("walljump");
                    break;
            }

            Parent.Sprite.CurrentTime = time;
        }
    }
}

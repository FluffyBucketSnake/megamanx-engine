using MegamanX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.GameObjects.Playable
{
    public class PlayerWeapon
    {
        public bool IsEnabled = true;

        public bool IsChargeAllowed = true;

        public Player Parent { get; internal set; }

        public int ProjectileCount { get; private set; }

        public int ChargeTimer { get; private set; } = 0;

        public SpriteSheet[] ProjectileSpriteSheets { get; } = new SpriteSheet[3];

        public SoundEffect[] ShootingSoundEffects { get; } = new SoundEffect[3];

        public SoundEffect[] ChargingSoundEffects { get; } = new SoundEffect[2];

        private SoundEffectInstance chargingSFXInstance = null;

        public bool Fire()
        {
            if (IsEnabled && ProjectileCount < 3)
            {
                Parent.Map.Objects.Add(CreatePellet());
                ProjectileCount++;

                ShootingSoundEffects[0]?.Play();

                return true;
            }
            return false;
        }

        public bool ReleaseCharge()
        {
            if (ChargeTimer > 1777)
            {
                Parent.Map.Objects.Add(CreateLevel2ChargeShot());
                ProjectileCount++;
                IsChargeAllowed = false;

                ShootingSoundEffects[1]?.Play();

                return true;
            }
            else if (ChargeTimer > 500)
            {
                Parent.Map.Objects.Add(CreateLevel1ChargeShot());
                ProjectileCount++;
                IsChargeAllowed = false;

                ShootingSoundEffects[2]?.Play();

                return true;
            }
            else
            {
                return false;
            }
        }

        private Projectile CreateLevel2ChargeShot()
        {
            //Create a new bullet instance.
            Projectile bullet = new Projectile(ProjectileSpriteSheets[2], new Rectangle(-21, -17, 43, 35), CollisionFlags.PlayerProjectile, CollisionFlags.Enemy);

            //Graphics.
            bullet.Sprite.Effects = (Parent.IsLeft) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            //Physics.
            float sign = (Parent.IsLeft ? -1f : 1f);
            bullet.Position = Parent.Position + new Vector2(16 * sign, -2);
            bullet.Speed = new Vector2(0.48f * sign, 0);


            //Misc.
            bullet.Damage = 4;
            bullet.Destroyed += (o) =>
            {
                ProjectileCount--;
                IsChargeAllowed = true;
            };

            return bullet;
        }

        private Projectile CreateLevel1ChargeShot()
        {
            //Create a new bullet instance.
            Projectile bullet = new Projectile(ProjectileSpriteSheets[1], new Rectangle(-22, -12, 45, 25), CollisionFlags.PlayerProjectile, CollisionFlags.Enemy);

            //Graphics.
            bullet.Sprite.Effects = (Parent.IsLeft) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            //Physics.
            float sign = (Parent.IsLeft ? -1f : 1f);
            bullet.Position = Parent.Position + new Vector2(16 * sign, -2);
            bullet.Speed = new Vector2(0.36f * sign, 0);

            //Misc.
            bullet.Damage = 3;
            bullet.Destroyed += (o) =>
            {
                ProjectileCount--;
                IsChargeAllowed = true;
            };

            return bullet;
        }

        private Projectile CreatePellet()
        {
            //Create a new bullet instance.
            Projectile bullet = new Projectile(ProjectileSpriteSheets[0], new Rectangle(-4, -4, 9, 9), CollisionFlags.PlayerProjectile, CollisionFlags.Enemy);

            //Graphics.
            bullet.Sprite.Effects = (Parent.IsLeft) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            //Physics.
            float sign = (Parent.IsLeft ? -1f : 1f);
            bullet.Position = Parent.Position + new Vector2(16 * sign, -2);
            bullet.Speed = new Vector2(0.24f * sign, 0);

            //Misc.
            bullet.Damage = 2;
            bullet.Destroyed += (o) => ProjectileCount--;

            return bullet;
        }

        public void Charge(GameTime gameTime)
        {
            if (IsEnabled && IsChargeAllowed)
            {
                ChargeTimer += gameTime.ElapsedGameTime.Milliseconds;

                if (ChargeTimer >= 500)
                {
                    if (chargingSFXInstance == null)
                    {
                        chargingSFXInstance = ChargingSoundEffects[0].CreateInstance();
                        chargingSFXInstance.Play();
                    }
                    else if (chargingSFXInstance.State == SoundState.Stopped)
                    {
                        chargingSFXInstance = ChargingSoundEffects[1].CreateInstance();
                        chargingSFXInstance.IsLooped = true;
                        chargingSFXInstance.Play();
                    }
                }
            }
            else
            {
                ResetCharge();
            }
        }

        internal void ResetCharge()
        {
            ChargeTimer = 0;

            chargingSFXInstance?.Stop();
            chargingSFXInstance = null;
        }
    }
}
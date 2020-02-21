using MegamanX.World;
using MegamanX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using MegamanX.GameObjects.Playable;
using System.Linq;
using MegamanX.Graphics;

namespace MegamanX.GameObjects
{
    public class DigLabour : Entity
    {
        PhysicBody body = new PhysicBody(new Rectangle(-11, -16, 22, 34));

        Sprite mainSprite;

        SpriteSheet pickaxeSpriteSheet;

        SoundEffect hitSound;

        SoundEffect deathSound;

        float timer;

        bool isThrowing = false;

        Projectile projectile;

        public DigLabour()
        {
            //Setup character's properties.
            Health = MaxHealth = 8;
            IsPersistent = false;
            Bounds = new Rectangle(-11, -16, 22, 34);

            //Setup physic body.
            body.UserData = this;
            body.MaskBits = (ushort)CollisionFlags.Enemy;
            body.CategoryBits = (ushort)CollisionFlags.All;
            body.BodyCollisionEvent += OnBodyCollision;
        }

        public float VPickaceLaunchSpeed { get; set; } = -0.36f; // 6 p/f
        public float ThrowBreak { get; set; } = 500.0f;

        public override void LoadContent(ContentManager content)
        {
            var mainSpriteSheet = new SpriteSheet(content.Load<Texture2D>("textures/enemy-diglabour"), 6);
            mainSpriteSheet.Origin = new Vector2(11, 17);

            mainSpriteSheet.Frames[0] = new SpriteFrame(0, 2, 32, 37, new Thickness(-2, -4, 0, -5));
            mainSpriteSheet.Frames[1] = new SpriteFrame(32, 2, 31, 37, new Thickness(-2, -3, 0, -6));
            mainSpriteSheet.Frames[2] = new SpriteFrame(63, 2, 34, 37, new Thickness(-2, -2, 0, -9));
            mainSpriteSheet.Frames[3] = new SpriteFrame(97, 1, 38, 38, new Thickness(-3, -6, 0, -9));
            mainSpriteSheet.Frames[4] = new SpriteFrame(135, 0, 40, 39, new Thickness(-4, -9, 0, -8));
            mainSpriteSheet.Frames[5] = new SpriteFrame(175, 2, 41, 37, new Thickness(-2, -13, 0, -5));

            mainSpriteSheet.Animations.Add(new SpriteAnimation("idle", 0));
            mainSpriteSheet.Animations.Add(new SpriteAnimation("throw",
            1, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4,
            5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5));

            mainSprite = new Sprite(mainSpriteSheet);
            mainSprite.OnAnimationComplete += (sender, e) =>
            {
                mainSprite.TryPlay("idle");
            };

            pickaxeSpriteSheet = new SpriteSheet(content.Load<Texture2D>("textures/projectile-pickaxe"), 1);
            pickaxeSpriteSheet.Origin = new Vector2(11, 12);
            pickaxeSpriteSheet.Frames[0] = new SpriteFrame(new Rectangle(0, 0, 24, 24), new Thickness(-1.0f));

            hitSound = content.Load<SoundEffect>("sfx/enemy-hurt");
            deathSound = content.Load<SoundEffect>("sfx/enemy-death");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            mainSprite.Position = Position;
            mainSprite.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            //Update physics.
            Position = body.Position;

            //Call base method.
            base.Update(gameTime);

            //Update sprite.
            mainSprite.Update(gameTime);

            //Attempt to throw a pickaxe.
            if (IsActive && Map != null && (projectile == null || !projectile.IsAlive))
            {
                timer += gameTime.ElapsedGameTime.Milliseconds;

                if (!isThrowing && timer >= ThrowBreak)
                {
                    isThrowing = true;
                    mainSprite.TryPlay("throw");
                    timer = 0;
                }
                else if (isThrowing && timer >= 15f * 100f / 6f)
                {
                    Vector2 launchSpeed = CalculateThrowSpeed();
                    ThrowPickaxe(launchSpeed);
                    timer = 0;
                    isThrowing = false;
                }
            }
        }

        private Vector2 CalculateThrowSpeed()
        {
            Vector2 launchSpeed = new Vector2(0, VPickaceLaunchSpeed);

            Player player = (Player)Map.Objects.FirstOrDefault(e => e.GetType() == typeof(Player));
            if (player != null)
            {
                Vector2 distanceToPlayer = player.Position - (Position - new Vector2(20, 12));
                float yGravity = Map.World.Gravity.Y;
                if (yGravity == 0)
                {
                    launchSpeed.X = distanceToPlayer.X * VPickaceLaunchSpeed / distanceToPlayer.Y;
                }
                else
                {
                    float delta = (VPickaceLaunchSpeed * VPickaceLaunchSpeed) + (2 * distanceToPlayer.Y * yGravity);
                    if (delta > 0)
                    {
                        double deltaSqrt = System.Math.Sqrt(delta);
                        double t0 = -(deltaSqrt + VPickaceLaunchSpeed) / yGravity;
                        double t1 = (deltaSqrt - VPickaceLaunchSpeed) / yGravity;
                        double estimatedCollisionTime = (t0 > t1) ? t0 : t1;
                        if (estimatedCollisionTime > 0)
                        {
                            launchSpeed.X = (float)(distanceToPlayer.X / estimatedCollisionTime);
                        }
                    }
                }
            }

            return launchSpeed;
        }

        private void ThrowPickaxe(Vector2 launchSpeed)
        {
            projectile = new Projectile(pickaxeSpriteSheet, new Rectangle(-11, -12, 22, 24), CollisionFlags.EnemyProjectile, CollisionFlags.Player);
            projectile.Position = Position - new Vector2(20, 12);
            projectile.DestroyOnCollide = false;
            projectile.Body.GravityScale = 1f;
            projectile.Speed = launchSpeed;

            Map?.Objects.Add(projectile);
        }

        protected override void OnCreation(object sender)
        {
            base.OnCreation(sender);
            Map.World.Bodies.Add(body);
        }

        protected override void OnDestruction(object sender)
        {
            base.OnDestruction(sender);
            Map.World.Bodies.Remove(body);
        }

        protected override void OnDamage(DamageInfo info)
        {
            if (info.Damage > 0)
            {
                hitSound.Play();
            }
        }

        protected override void OnDeath(object source)
        {
            deathSound.Play();
            base.OnDeath(source);
        }

        protected override void OnPositionChange(PositionChangedArgs e)
        {
            body.Position = e.NewPosition;
        }

        private void OnBodyCollision(BodyCollisionInfo info)
        {
            var player = info.CollidingBody.UserData as Player;
            if (player != null)
            {
                player.Damage(new DamageInfo(this, 1, Direction2DHelper.GetDirection(-info.Penetration)));
            }
        }
    }
}
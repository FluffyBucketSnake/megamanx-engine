using MegamanX.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MegamanX.Physics;
using MegamanX.Graphics;

namespace MegamanX.GameObjects
{
    public class Projectile : GameObject
    {
        public Sprite Sprite { get; set; } = null;

        public int Damage { get; set; } = 1;

        public PhysicBody Body = new PhysicBody(Rectangle.Empty);

        public bool DestroyOnCollide = true;

        public Vector2 Speed
        {
            get => Body.Speed;
            set => Body.Speed = value;
        }

        public Projectile(SpriteSheet spriteSheet, Rectangle hitbox, CollisionFlags Mask, CollisionFlags Category)
        {
            //General properties
            Bounds = hitbox;

            //Sprite.
            Sprite = new Sprite(spriteSheet);
            Sprite.TryPlay("default");

            //Set hitbox.
            Body.UserData = this;
            Body.Bounds = hitbox;
            Body.IsTangible = false;
            Body.GravityScale = 0f;
            Body.MaskBits = (ushort)Mask;
            Body.CategoryBits = (ushort)Category;
            Body.BodyCollisionEvent += OnBodyCollide;
        }

        protected override void OnCreation(object sender)
        {
            base.OnCreation(sender);
            Map.World.Bodies.Add(Body);
        }

        protected override void OnDestruction(object sender)
        {
            base.OnDestruction(sender);
            Map.World.Bodies.Remove(Body);
        }

        protected override void OnPositionChange(PositionChangedArgs e)
        {
            Body.Position = e.NewPosition;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Sprite.Position = Position;
            Sprite.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            Position = Body.Position;

            Sprite.Update(gameTime);
        }

        private void OnBodyCollide(BodyCollisionInfo info)
        {
            if (info.CollidingBody.UserData is Entity entity)
            {
                entity.Damage(new DamageInfo(this, Damage, Direction2DHelper.GetDirection(-info.Penetration)));
                if (DestroyOnCollide)
                {
                    Destroy();
                }
            }
        }
    }
}
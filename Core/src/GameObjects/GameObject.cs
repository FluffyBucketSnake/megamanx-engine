using MegamanX.World;
using MegamanX.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MegamanX.GameObjects
{
    public delegate void GameObjectStateChangedEventHandler(object sender);

    public abstract class GameObject
    {
        private GameWorld world;

        private Vector2 _position;

        public string Name { get; set; }

        public GameWorld Map
        {
            get => world;
            internal set
            {
                if (value != null)
                {
                    world = value;
                    OnCreation(null);
                }
                else
                {
                    OnDestruction(null);
                    world = null;
                }
            }
        }

        public bool IsPersistent { get; set; }

        public bool IsActive { get; private set; } = false;

        public bool IsAlive => Map != null;

        public Vector2 Position
        {
            get => _position;
            set
            {
                var e = new PositionChangedArgs(_position, value);
                OnPositionChange(e);
                _position = e.NewPosition;
            }
        }

        public Rectangle Bounds { get; protected set; } = Rectangle.Empty;

        public Rectangle WorldBounds => Bounds.Translate(Position);

        public event GameObjectStateChangedEventHandler Created;
        public event GameObjectStateChangedEventHandler Destroyed;

        public virtual void LoadContent(ContentManager content) { }

        public void Destroy()
        {
            //Call for the removal of the object.
            Map?.Objects.Remove(this);
        }

        public void Activate()
        {
            IsActive = true;
            OnActivation();
        }

        public void Deactivate()
        {
            OnDectivation();
            IsActive = false;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        protected virtual void OnPositionChange(PositionChangedArgs e) { }

        protected virtual void OnCreation(object sender)
        {
            Created?.Invoke(sender);
        }

        protected virtual void OnDestruction(object sender)
        {
            Destroyed?.Invoke(sender);
        }

        protected virtual void OnActivation() { }

        protected virtual void OnDectivation() { }
    }
}
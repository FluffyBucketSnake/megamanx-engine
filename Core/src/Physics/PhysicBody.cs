using Microsoft.Xna.Framework;

namespace MegamanX.Physics
{
    public enum CollisionTypes
    {
        None,
        Solid,
        Platform
    }

    public class PhysicBody : IPhysicSensorParent
    {
        public Vector2 Position;

        public Vector2 Speed;

        public Rectangle Bounds;

        public Rectangle WorldBounds => Bounds.Translate(Position);

        public CollisionTypes Type = CollisionTypes.None;

        public float GravityScale = 1f;

        public bool IsTangible = true;

        public bool IsCollidable = true;

        public ushort CategoryBits = 0x0001;

        public ushort MaskBits = 0xFFFF;

        public object UserData;

        public PhysicWorld World { get; internal set; }

        public PhysicSensorCollection Sensors { get; }

        public event BodyCollisionEventHandler BodyCollisionEvent;

        public event TilemapCollisionEventHandler TileMapCollisionEvent;

        public PhysicBody(Rectangle bounds)
        {
            Bounds = bounds;
            Sensors = new PhysicSensorCollection(this);
        }

        public PhysicBody(Rectangle bounds, CollisionTypes type)
        {
            Bounds = bounds;
            Type = type;
            Sensors = new PhysicSensorCollection(this);
        }

        public PhysicBody(Rectangle bounds, Vector2 position)
        {
            Bounds = bounds;
            Position = position;
        }

        internal void TileMapCollision(TileMapCollisionInfo info)
        {
            TileMapCollisionEvent?.Invoke(info);
        }

        internal void BodyCollision(BodyCollisionInfo info)
        {
            BodyCollisionEvent?.Invoke(info);
        }
    }
}
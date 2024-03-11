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
        public Rectangle Bounds { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }

        public Rectangle WorldBounds => Bounds.Translate(Position);

        public CollisionTypes Type { get; set; } = CollisionTypes.None;

        public float GravityScale { get; set; } = 1f;

        public bool IsTangible { get; set; } = true;

        public bool IsCollidable { get; set; } = true;

        public ushort CategoryBits { get; set; } = 0x0001;

        public ushort MaskBits { get; set; } = 0xFFFF;

        public object? UserData { get; set; }

        public PhysicWorld? World { get; internal set; }

        public PhysicSensorCollection Sensors { get; }

        public event BodyCollisionEventHandler? BodyCollisionEvent;
        public event TilemapCollisionEventHandler? TileMapCollisionEvent;

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
            Sensors = new PhysicSensorCollection(this);
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

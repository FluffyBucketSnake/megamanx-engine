using MegamanX.Physics;
using Microsoft.Xna.Framework;

namespace MegamanX.Components
{
    public class PhysicsBodyComponent : IComponent
    {
        public PhysicBody Body { get; }

        public PhysicWorld PhysicWorld { get; }

        public Vector2 Position { get => Body.Position; set => Body.Position = value; }
        public Vector2 Velocity { get => Body.Velocity; set => Body.Velocity = value; }

        public PhysicsBodyComponent(Entity entity, PhysicWorld physicWorld, Rectangle bounds)
        {
            PhysicWorld = physicWorld;

            Body = new(bounds)
            {
                UserData = entity
            };
            PhysicWorld.Bodies.Add(Body);
        }

        public PhysicSensor CreateSensor(Rectangle bounds)
        {
            PhysicSensor sensor = new(bounds);
            Body.Sensors.Add(sensor);
            return sensor;
        }
    }
}

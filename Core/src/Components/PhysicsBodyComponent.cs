using MegamanX.Physics;
using Microsoft.Xna.Framework;

namespace MegamanX.Components
{
    public class PhysicBodyComponent : IComponent
    {
        public PhysicBody Body { get; }

        public PhysicWorld PhysicWorld { get; }

        public Vector2 Position { get => Body.Position; set => Body.Position = value; }
        public Vector2 Velocity { get => Body.Velocity; set => Body.Velocity = value; }

        private readonly TransformComponent transformComponent;

        public PhysicBodyComponent(Entity entity, PhysicWorld physicWorld, Rectangle bounds)
        {
            transformComponent = entity.GetComponent<TransformComponent>();

            PhysicWorld = physicWorld;

            Body = new(bounds)
            {
                UserData = entity,
                GravityScale = 1,
                Position = transformComponent.Position
            };
            PhysicWorld.Bodies.Add(Body);
        }

        public PhysicSensor CreateSensor(Rectangle bounds)
        {
            PhysicSensor sensor = new(bounds);
            Body.Sensors.Add(sensor);
            return sensor;
        }

        void IComponent.Update(GameTime gameTime)
        {
            transformComponent.Position = Body.Position;
        }
    }
}

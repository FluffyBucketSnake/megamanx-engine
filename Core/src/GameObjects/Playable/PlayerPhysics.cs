using MegamanX.Physics;
using Microsoft.Xna.Framework;

namespace MegamanX.GameObjects.Playable
{
    public class PlayerPhysics
    {
        public PlayerPhysicsParameters Parameters = PlayerPhysicsParameters.Default;

        public PhysicWorld World  { get; private set; }

        public PhysicBody Body { get; } = new PhysicBody(new Rectangle(-8, -16, 16, 32));

        public PhysicSensor GroundSensor { get; } = new PhysicSensor(0, 32, 16, 1);
        public PhysicSensor CeilingSensor { get; } = new PhysicSensor(0, -1, 16, 1);
        public PhysicSensor LeftWallSensor { get; } = new PhysicSensor(-1, 0, 1, 32);
        public PhysicSensor RightWallSensor { get; } = new PhysicSensor(16, 0, 1, 32);
        public PhysicSensor LeftWalljumpSensor { get; } = new PhysicSensor(-7, 0, 7, 32);
        public PhysicSensor RightWalljumpSensor { get; } = new PhysicSensor(16, 0, 7, 32);

        public Vector2 Speed
        {
            get => Body.Speed;
            set
            {
                Body.Speed = value;
            }
        }

        public float GravityScale
        {
            get => Body.GravityScale;
            set
            {
                Body.GravityScale = value;
            }
        }

        public PlayerPhysics(Player player)
        {
            Body.UserData = player;
            Body.MaskBits = (ushort)CollisionFlags.Player;
            Body.CategoryBits = (ushort)CollisionFlags.All;

            Body.Sensors.Add(GroundSensor);
            Body.Sensors.Add(CeilingSensor);
            Body.Sensors.Add(LeftWalljumpSensor);
            Body.Sensors.Add(RightWalljumpSensor);
            Body.Sensors.Add(LeftWallSensor);
            Body.Sensors.Add(RightWallSensor);
        }

        public void Move(Vector2 translation)
        {
            if (World != null)
            {
                World.Translate(Body, translation);
            }
            else
            {
                Body.Position += translation;
            }
        }

        public void RegisterWorld(PhysicWorld world)
        {
            world.Bodies.Add(Body);
            World = world;
        }

        public void UnregisterWorld(PhysicWorld world)
        {
            world.Bodies.Remove(Body);
            World = null;
        }
    }
}
using Microsoft.Xna.Framework;

namespace MegamanX.Physics
{
    public class PhysicSensor
    {
        public Rectangle Area { get; set; }
        public ushort MaskBits { get; set; } = 0x0001;
        public ushort CategoryBits { get; set; } = 0xFFFF;

        public IPhysicSensorParent? Parent { get; internal set; }

        public PhysicSensor(Rectangle area)
        {
            Area = area;
        }

        public PhysicSensor(int x, int y, int width, int height)
        {
            Area = new Rectangle(x, y, width, height);
        }

        public bool State { get; internal set; }

        public static implicit operator bool(PhysicSensor sensor)
        {
            return sensor.State;
        }
    }
}

using System.Collections.Generic;

namespace MegamanX.Physics
{
    public interface IPhysicSensorParent
    {
        IEnumerable<PhysicSensor> Sensors { get; }
    }
}

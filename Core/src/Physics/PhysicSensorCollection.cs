using System;
using System.Collections;
using System.Collections.Generic;

namespace MegamanX.Physics
{
    public class PhysicSensorCollection : ICollection<PhysicSensor>
    {
        private List<PhysicSensor> sensors = new List<PhysicSensor>();

        public int Count => sensors.Count;

        public bool IsReadOnly => false;

        public IPhysicSensorParent Parent { get; }

        public PhysicSensorCollection(IPhysicSensorParent owner)
        {
            Parent = owner;
        }

        public void Add(PhysicSensor item)
        {
            if (Parent != null && item.Parent != null)
            {
                throw new ArgumentException("The given PhysicSensor already has a parent.");
            }
            else
            {
                sensors.Add(item);
            }
        }

        public void Clear()
        {
            foreach (var sensor in sensors)
            {
                sensor.Parent = null;
            }
            sensors.Clear();
        }

        public bool Contains(PhysicSensor item)
        {
            return sensors.Contains(item);
        }

        public void CopyTo(PhysicSensor[] array, int arrayIndex)
        {
            sensors.CopyTo(array, arrayIndex);
        }

        public IEnumerator<PhysicSensor> GetEnumerator()
        {
            return sensors.GetEnumerator();
        }

        public bool Remove(PhysicSensor item)
        {
            if (sensors.Remove(item))
            {
                item.Parent = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return sensors.GetEnumerator();
        }
    }
}
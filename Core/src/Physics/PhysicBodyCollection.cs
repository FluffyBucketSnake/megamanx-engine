using System;
using System.Collections;
using System.Collections.Generic;

namespace MegamanX.Physics
{
    public class PhysicBodyCollection : ICollection<PhysicBody>
    {
        private List<PhysicBody> bodies = new List<PhysicBody>();

        public int Count => bodies.Count;

        public bool IsReadOnly => false;

        public PhysicWorld World { get; }

        public PhysicBodyCollection(PhysicWorld world)
        {
            World = world;
        }

        public void Add(PhysicBody item)
        {
            if (World != null && item.World != null)
            {
                throw new ArgumentException("The given PhysicBody already has a PhysicWorld. Remove it from the other world.");
            }
            else
            {
                bodies.Add(item);
            }
        }

        public void Clear()
        {
            foreach (var body in bodies)
            {
                body.World = null;
            }
            bodies.Clear();
        }

        public bool Contains(PhysicBody item)
        {
            return bodies.Contains(item);
        }

        public void CopyTo(PhysicBody[] array, int arrayIndex)
        {
            bodies.CopyTo(array, arrayIndex);
        }

        public IEnumerator<PhysicBody> GetEnumerator()
        {
            return bodies.GetEnumerator();
        }

        public bool Remove(PhysicBody item)
        {
            if (bodies.Remove(item))
            {
                item.World = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return bodies.GetEnumerator();
        }
    }
}
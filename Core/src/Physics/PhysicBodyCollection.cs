using System;
using System.Collections;
using System.Collections.Generic;

namespace MegamanX.Physics
{
    public class PhysicBodyCollection(PhysicWorld world) : ICollection<PhysicBody>
    {
        private readonly List<PhysicBody> bodies = [];

        public int Count => bodies.Count;
        public bool IsReadOnly => false;
        public PhysicWorld World => world;

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
            foreach (PhysicBody body in bodies)
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

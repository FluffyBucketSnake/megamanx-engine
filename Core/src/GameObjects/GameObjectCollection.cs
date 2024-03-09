using System.Collections;
using System.Collections.Generic;
using MegamanX.World;

namespace MegamanX.GameObjects
{
    public class GameObjectCollection : ICollection<LegacyGameObject>
    {
        private ICollection<LegacyGameObject> internalCollection;

        public GameWorld World { get; private set; }

        public int Count => internalCollection.Count;

        public bool IsReadOnly => internalCollection.IsReadOnly;

        public GameObjectCollection(GameWorld level)
        {
            World = level;
            internalCollection = new List<LegacyGameObject>();
        }

        public void Add(LegacyGameObject gameObject)
        {
            if (gameObject.Map == null)
            {
                gameObject.Map = World;
            }
            else
            {
                throw new System.Exception("GameObject is already contained by a map.");
            }
            internalCollection.Add(gameObject);
        }

        public void Clear()
        {
            foreach (var entity in this)
            {
                entity.Map = null;
            }
            internalCollection.Clear();
        }

        public bool Contains(LegacyGameObject item)
        {
            return internalCollection.Contains(item);
        }

        public void CopyTo(LegacyGameObject[] array, int arrayIndex)
        {
            internalCollection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<LegacyGameObject> GetEnumerator()
        {
            return internalCollection.GetEnumerator();
        }

        public bool Remove(LegacyGameObject item)
        {
            if (internalCollection.Remove(item))
            {
                item.Map = null;
                return true;
            }
            else
            {
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return internalCollection.GetEnumerator();
        }
    }
}

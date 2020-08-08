using MegamanX.World;
using System.Collections;
using System.Collections.Generic;

namespace MegamanX.GameObjects
{
    public class GameObjectCollection : ICollection<GameObject>
    {
        private ICollection<GameObject> internalCollection;

        public GameWorld World { get; private set; }

        public int Count => internalCollection.Count;

        public bool IsReadOnly => internalCollection.IsReadOnly;

        public GameObjectCollection(GameWorld level)
        {
            World = level;
            internalCollection = new List<GameObject>();
        }

        public void Add(GameObject gameObject)
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

        public bool Contains(GameObject item)
        {
            return internalCollection.Contains(item);
        }

        public void CopyTo(GameObject[] array, int arrayIndex)
        {
            internalCollection.CopyTo(array,arrayIndex);
        }

        public IEnumerator<GameObject> GetEnumerator()
        {
            return internalCollection.GetEnumerator();
        }

        public bool Remove(GameObject item)
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
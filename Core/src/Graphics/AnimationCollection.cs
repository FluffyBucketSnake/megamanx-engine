using System.Collections;
using System.Collections.Generic;

namespace MegamanX.Graphics
{
    public class AnimationCollection : ICollection<SpriteAnimation>
    {
        private readonly Dictionary<string, SpriteAnimation> _innerTable = new Dictionary<string, SpriteAnimation>();

        public int Count => _innerTable.Count;
        public bool IsReadOnly => false;
        public SpriteAnimation this[string name]
        {
            get => _innerTable[name];
        }

        public void Add(SpriteAnimation item)
        {
            if (item == null)
            {
                throw new System.ArgumentNullException(nameof(item));
            }
            else
            {
                _innerTable.Add(item.Name, item);
            }
        }

        public void Clear()
        {
            _innerTable.Clear();
        }

        public bool Contains(SpriteAnimation item)
        {
            return _innerTable.ContainsValue(item);
        }

        public bool Contains(string animationName)
        {
            return _innerTable.ContainsKey(animationName);
        }

        public void CopyTo(SpriteAnimation[] array, int arrayIndex)
        {
            _innerTable.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<SpriteAnimation> GetEnumerator()
        {
            return _innerTable.Values.GetEnumerator();
        }

        public bool Remove(SpriteAnimation item)
        {
            return _innerTable.Remove(item.Name);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerTable.Values.GetEnumerator();
        }
    }
}
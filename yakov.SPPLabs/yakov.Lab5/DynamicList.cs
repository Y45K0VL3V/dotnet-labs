using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace yakov.Lab5
{
    public class DynamicList<T> : IEnumerable<T>, IList<T>
    {
        private T[] _items = new T[0];
        public T this[int index]
        {
            get
            {
                try
                {
                    var gg = new List<int>();
                    return _items[index];
                }
                catch
                {
                    throw;
                }
            }
            set
            {
                _items[index] = value;
            }
        }

        public int Count => _items.Length;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            Array.Resize(ref _items, _items.Length + 1);
            _items[_items.Length] = item;
        }

        public void Clear()
        {
            Array.Clear(_items);
            _items = new T[0];
        }

        public bool Contains(T item)
        {
            return IndexOf(item) == -1 ? false : true;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _items.Length; i++)
            {
                if (Object.Equals(item, _items[i]))
                    return i;
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            Array.Resize(ref _items, _items.Length + 1);

            for (int i = _items.Length - 1; i > index; i--)
                _items[i] = _items[i - 1];

            _items[index] = item;
        }

        public bool Remove(T item)
        {
            var itemIndex = IndexOf(item);
            if (itemIndex == -1)
            {
                return false;
            }
            else
            {
                RemoveAt(itemIndex);
                return true;
            }
        }

        public void RemoveAt(int index)
        {
            for (int i = index; i < _items.Length - 1; i++)
                _items[i] = _items[i + 1];

            Array.Resize(ref _items, _items.Length - 1);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}

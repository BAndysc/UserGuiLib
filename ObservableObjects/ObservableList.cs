using System;
using System.Collections;
using System.Collections.Generic;

namespace ObservableObjects
{
    public class ObservableList<T> : IObservableList<T>
    {
        private List<T> list = new List<T>();

        public event Action<T> OnRemoveItem = delegate { };
        public event Action<T, int> OnInsertItem = delegate { };

        public T this[int index] { get => list[index]; set => list[index] = value; }

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            list.Add(item);
            OnInsertItem(item, Count - 1);
        }

        public void Clear()
        {
            while (Count > 0)
                RemoveAt(Count - 1);
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
            OnInsertItem(item, index);
        }

        public bool Remove(T item)
        {
            bool result = list.Remove(item);
            OnRemoveItem(item);
            return result;
        }

        public void RemoveAt(int index)
        {
            var item = this[index];
            list.RemoveAt(index);
            OnRemoveItem(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }
    }
}

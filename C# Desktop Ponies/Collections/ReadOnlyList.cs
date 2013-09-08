namespace CSDesktopPonies.Collections
{
    using System;
    using System.Collections.Generic;
    using CSDesktopPonies.Core;

    public static class ReadOnlyList
    {
        public static ReadOnlyList<T> AsReadOnly<T>(this IList<T> list)
        {
            return new ReadOnlyList<T>(list);
        }
    }
    public class ReadOnlyList<T> : IList<T>
    {
        private IList<T> list;
        public ReadOnlyList(IList<T> list)
        {
            this.list = Argument.EnsureNotNull(list, "list");
        }
        public T this[int index]
        {
            get { return list[index]; }
        }

        public int Count
        {
            get { return list.Count; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }
        public bool Contains(T item)
        {
            return list.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }
        private NotSupportedException ReadOnlyException()
        {
            return new NotSupportedException("Collection is read-only.");
        }
        void IList<T>.Insert(int index, T item)
        {
            throw ReadOnlyException();
        }
        void IList<T>.RemoveAt(int index)
        {
            throw ReadOnlyException();
        }

        T IList<T>.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                throw ReadOnlyException();
            }
        }

        void ICollection<T>.Add(T item)
        {
            throw ReadOnlyException();
        }

        void ICollection<T>.Clear()
        {
            throw ReadOnlyException();
        }
        bool ICollection<T>.IsReadOnly
        {
            get { return true; }
        }
        bool ICollection<T>.Remove(T item)
        {
            throw ReadOnlyException();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

namespace CSDesktopPonies.Collections
{
    using System;
    using System.Collections.Generic;
    using CSDesktopPonies.Core;

    /// <summary>
    /// Wraps a mutable collection in order to provide a read-only interface.
    /// </summary>
    public static class ReadOnlyCollection
    {
        /// <summary>
        /// Creates a read-only wrapper around a collection.
        /// </summary>
        /// <typeparam name="T">The type of the elements.</typeparam>
        /// <param name="collection">The mutable collection to wrap.</param>
        /// <returns>A <see cref="T:CSDesktopPonies.Collections.ReadOnlyCollection`1"/> that wraps the specified collection.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> is null.</exception>
        public static ReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> collection)
        {
            return new ReadOnlyCollection<T>(collection);
        }
    }

    /// <summary>
    /// Wraps a mutable collection in order to provide a read-only interface.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    public struct ReadOnlyCollection<T> : ICollection<T>
    {
        /// <summary>
        /// The wrapped collection.
        /// </summary>
        private ICollection<T> collection;
        /// <summary>
        /// Creates a read-only wrapper around a collection.
        /// </summary>
        /// <param name="collection">The mutable collection to wrap.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> is null.</exception>
        public ReadOnlyCollection(ICollection<T> collection)
        {
            this.collection = Argument.EnsureNotNull(collection, "collection");
        }
        public bool Contains(T item)
        {
            return collection.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            collection.CopyTo(array, arrayIndex);
        }
        public int Count
        {
            get { return collection.Count; }
        }
        public IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator();
        }
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        private NotSupportedException ReadOnlyException()
        {
            return new NotSupportedException("Collection is read-only.");
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
    }
}

namespace DesktopSprites.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DesktopSprites.Core;

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
        /// <returns>A <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> that wraps the specified collection.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> is null.</exception>
        public static ReadOnlyCollection<T> AsReadOnly<T>(this ICollection<T> collection)
        {
            if (collection is ReadOnlyCollection<T>)
                return (ReadOnlyCollection<T>)collection;
            return new ReadOnlyCollection<T>(collection);
        }
    }

    /// <summary>
    /// Wraps a mutable collection in order to provide a read-only interface.
    /// </summary>
    /// <typeparam name="T">The type of the elements.</typeparam>
    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(ReadOnlyCollection<>.DebugView))]
    public struct ReadOnlyCollection<T> : ICollection<T>
    {
        /// <summary>
        /// The wrapped collection.
        /// </summary>
        private ICollection<T> collection;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> structure around the
        /// specified collection.
        /// </summary>
        /// <param name="collection">The mutable collection to wrap.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> is null.</exception>
        public ReadOnlyCollection(ICollection<T> collection)
        {
            this.collection = Argument.EnsureNotNull(collection, "collection");
        }
        /// <summary>
        /// Determines whether the <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/>.</param>
        /// <returns>Returns true if item is found in the <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/>; otherwise,
        /// false.</returns>
        public bool Contains(T item)
        {
            return collection.Contains(item);
        }
        /// <summary>
        /// Copies the elements of the <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> to an <see cref="T:System.Array"/>,
        /// starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from
        /// <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/>. The <see cref="T:System.Array"/> must have zero-based
        /// indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source
        /// <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> is greater than the available space from
        /// <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            collection.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/>.
        /// </summary>
        public int Count
        {
            get { return collection.Count; }
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return collection.GetEnumerator();
        }
        /// <summary>
        /// Tests whether two specified <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> that is to the left of the equality
        /// operator.</param>
        /// <param name="right">The <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> that is to the right of the equality
        /// operator.</param>
        /// <returns>Returns true if the two <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> structures are equal;
        /// otherwise, false.</returns>
        public static bool operator ==(ReadOnlyCollection<T> left, ReadOnlyCollection<T> right)
        {
            return left.collection.Equals(right.collection);
        }
        /// <summary>
        /// Tests whether two specified <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> structures are different.
        /// </summary>
        /// <param name="left">The <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> that is to the left of the inequality
        /// operator.</param>
        /// <param name="right">The <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> that is to the right of the inequality
        /// operator.</param>
        /// <returns>Returns true if the two <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> structures are different;
        /// otherwise, false.</returns>
        public static bool operator !=(ReadOnlyCollection<T> left, ReadOnlyCollection<T> right)
        {
            return !(left == right);
        }
        /// <summary>
        /// Tests whether the specified object is a <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> structure and is
        /// equivalent to this <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> structure.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true if <paramref name="obj"/> is a <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> structure
        /// equivalent to this <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> structure; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ReadOnlyCollection<T>))
                return false;
            return this == (ReadOnlyCollection<T>)obj;
        }
        /// <summary>
        /// Returns a hash code for this <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/> structure.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this
        /// <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/>.</returns>
        public override int GetHashCode()
        {
            return unchecked(collection.GetHashCode() * 7);
        }
        /// <summary>
        /// Returns a new <see cref="T:System.NotSupportedException"/> with a message about the collection being read-only.
        /// </summary>
        /// <returns>A new <see cref="T:System.NotSupportedException"/> with a message about the collection being read-only.</returns>
        private static NotSupportedException ReadOnlyException()
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
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #region DebugView class
        /// <summary>
        /// Provides a debugger view for a <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1"/>.
        /// </summary>
        private sealed class DebugView
        {
            /// <summary>
            /// The collection for which an alternate view is being provided.
            /// </summary>
            private ReadOnlyCollection<T> readOnlyCollection;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:DesktopSprites.Collections.ReadOnlyCollection`1.DebugView"/> class.
            /// </summary>
            /// <param name="readOnlyCollection">The collection to proxy.</param>
            public DebugView(ReadOnlyCollection<T> readOnlyCollection)
            {
                this.readOnlyCollection = Argument.EnsureNotNull(readOnlyCollection, "readOnlyCollection");
            }

            /// <summary>
            /// Gets a view of the items in the collection.
            /// </summary>
            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public T[] Items
            {
                get { return readOnlyCollection.collection as T[] ?? readOnlyCollection.collection.ToArray(); }
            }
        }
        #endregion
    }
}

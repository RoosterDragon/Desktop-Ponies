namespace DesktopSprites.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DesktopSprites.Core;

    /// <summary>
    /// Represents an immutable collection of objects backed by an array.
    /// </summary>
    public static class ImmutableArray
    {
        /// <summary>
        /// Creates an <see cref="T:DesktopSprites.Collections.ImmutableArray`1"/> from an
        /// <see cref="T:System.Collections.Generic.IEnumerable`1"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> to create an immutable array from.</param>
        /// <returns>An immutable array that contains the elements from the input sequence.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static ImmutableArray<T> ToImmutableArray<T>(this IEnumerable<T> source)
        {
            return source as ImmutableArray<T> ?? new ImmutableArray<T>(source);
        }
    }

    /// <summary>
    /// Represents an immutable collection of elements that can be accessed by index.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the immutable array.</typeparam>
    [System.Diagnostics.DebuggerDisplay("Length = {Length}")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(ImmutableArray<>.DebugView))]
    public sealed class ImmutableArray<T> : IList<T>
    {
        /// <summary>
        /// The underlying array.
        /// </summary>
        private T[] array;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.Collections.ImmutableArray`1"/> class by making an copy of the
        /// specified sequence.
        /// </summary>
        /// <param name="source">The sequence to copy.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        public ImmutableArray(IEnumerable<T> source)
        {
            array = Argument.EnsureNotNull(source, "source").ToArray();
        }
        /// <summary>
        /// Gets the element at the specified index in the immutable array.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index in the immutable array.</returns>
        public T this[int index]
        {
            get { return array[index]; }
        }
        /// <summary>
        /// Gets the number of elements in the immutable array.
        /// </summary>
        public int Length
        {
            get { return array.Length; }
        }
        /// <summary>
        /// Gets the number of elements in the immutable array.
        /// </summary>
        int ICollection<T>.Count
        {
            get { return array.Length; }
        }
        /// <summary>
        /// Returns an enumerator that iterates through the immutable array.
        /// </summary>
        /// <returns>A <see cref="T:DesktopSprites.Collections.ImmutableArray`1.Enumerator"/> that can be used to iterate through the
        /// collection.</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        /// <summary>
        /// Returns an enumerator that iterates through the immutable array.
        /// </summary>
        /// <returns>A <see cref="T:DesktopSprites.Collections.ImmutableArray`1.Enumerator"/> that can be used to iterate through the
        /// collection.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Supports iteration over an immutable array.
        /// </summary>
        public struct Enumerator : IEnumerator<T>
        {
            /// <summary>
            /// The array being enumerated.
            /// </summary>
            private readonly ImmutableArray<T> immutableArray;
            /// <summary>
            /// The current index into the array.
            /// </summary>
            private int index;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:DesktopSprites.Collections.ImmutableArray`1.Enumerator"/> structure.
            /// </summary>
            /// <param name="immutableArray">The immutable array to enumerate.</param>
            internal Enumerator(ImmutableArray<T> immutableArray)
            {
                this.immutableArray = Argument.EnsureNotNull(immutableArray, "immutableArray");
                this.index = -1;
            }
            /// <summary>
            /// Advances the enumerator to the next element of the immutable array.
            /// </summary>
            /// <returns>Return true if the enumerator was successfully advanced to the next element; false if the enumerator has passed
            /// the end of the collection.</returns>
            public bool MoveNext()
            {
                return ++index < immutableArray.Length;
            }
            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            public T Current
            {
                get { return immutableArray[index]; }
            }
            /// <summary>
            /// Releases all resources allocated by the object.
            /// </summary>
            public void Dispose()
            {
            }
            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }
            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            void System.Collections.IEnumerator.Reset()
            {
                index = -1;
            }
        }
        /// <summary>
        /// Determines the index of a specific item in the <see cref="T:DesktopSprites.Collections.ImmutableArray`1"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:DesktopSprites.Collections.ImmutableArray`1"/>.</param>
        /// <returns>The index of item if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item)
        {
            return array.IndexOf(item);
        }
        /// <summary>
        /// Determines whether the <see cref="T:DesktopSprites.Collections.ImmutableArray`1"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:DesktopSprites.Collections.ImmutableArray`1"/>.</param>
        /// <returns>Returns true if item is found in the <see cref="T:DesktopSprites.Collections.ImmutableArray`1"/>; otherwise, false.
        /// </returns>
        public bool Contains(T item)
        {
            return array.Contains(item);
        }
        /// <summary>
        /// Copies the elements of the <see cref="T:DesktopSprites.Collections.ImmutableArray`1"/> to an <see cref="T:System.Array"/>,
        /// starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from
        /// <see cref="T:DesktopSprites.Collections.ImmutableArray`1"/>. The <see cref="T:System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source
        /// <see cref="T:DesktopSprites.Collections.ImmutableArray`1"/> is greater than the available space from
        /// <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            this.array.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Returns a new <see cref="T:System.NotSupportedException"/> with a message about the collection being read-only.
        /// </summary>
        /// <returns>A new <see cref="T:System.NotSupportedException"/> with a message about the collection being read-only.</returns>
        private static NotSupportedException ReadOnlyException()
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
            get { return this[index]; }
            set { throw ReadOnlyException(); }
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
        /// Provides a debugger view for an <see cref="T:DesktopSprites.Collections.ImmutableArray`1"/>.
        /// </summary>
        private sealed class DebugView
        {
            /// <summary>
            /// The array for which an alternate view is being provided.
            /// </summary>
            private ImmutableArray<T> immutableArray;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:DesktopSprites.Collections.ImmutableArray`1.DebugView"/> class.
            /// </summary>
            /// <param name="immutableArray">The array to proxy.</param>
            public DebugView(ImmutableArray<T> immutableArray)
            {
                this.immutableArray = Argument.EnsureNotNull(immutableArray, "immutableArray");
            }

            /// <summary>
            /// Gets a view of the items in the array.
            /// </summary>
            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public T[] Items
            {
                get { return immutableArray.array; }
            }
        }
        #endregion
    }
}

namespace CSDesktopPonies.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CSDesktopPonies.Core;

    /// <summary>
    /// Represents an immutable collection of objects backed by an array.
    /// </summary>
    public static class ImmutableArray
    {
        /// <summary>
        /// Creates an <see cref="T:CSDesktopPonies.Collections.ImmutableArray`1"/> from an
        /// <see cref="T:System.Collections.Generic.IEnumerable`1"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
        /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> to create an immutable array from.</param>
        /// <returns>An immutable array that contains the elements from the input sequence.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static ImmutableArray<T> ToImmutableArray<T>(this IEnumerable<T> source)
        {
            return new ImmutableArray<T>(source);
        }
    }

    /// <summary>
    /// Represents an immutable collection of elements that can be accessed by index.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the immutable array.</typeparam>
    public class ImmutableArray<T> : IList<T>
    {
        /// <summary>
        /// The underlying array.
        /// </summary>
        private T[] array;

        /// <summary>
        /// Creates an immutable array by making an copy of the specified sequence.
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
        /// <returns>A <see cref="T:CSDesktopPonies.Collections.ImmutableArray`1.ArrayEnumerator"/> that can be used to iterate through the
        /// collection.</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        /// <summary>
        /// Returns an enumerator that iterates through the immutable array.
        /// </summary>
        /// <returns>A <see cref="T:CSDesktopPonies.Collections.ImmutableArray`1.ArrayEnumerator"/> that can be used to iterate through the
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
            /// Creates an enumerator for the specified immutable array.
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

            public void Dispose()
            {
            }
            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }
            void System.Collections.IEnumerator.Reset()
            {
                index = -1;
            }
        }

        public int IndexOf(T item)
        {
            return array.IndexOf(item);
        }
        public bool Contains(T item)
        {
            return array.Contains(item);
        }
        public void CopyTo(T[] array, int arrayIndex)
        {
            array.CopyTo(array, arrayIndex);
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

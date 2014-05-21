namespace DesktopSprites.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DesktopSprites.Core;

    /// <summary>
    /// Wraps a mutable collection in order to provide a read-only interface.
    /// </summary>
    public static class ReadOnlyDictionary
    {
        /// <summary>
        /// Creates a read-only wrapper around a collection.
        /// </summary>
        /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
        /// <param name="dictionary">The mutable collection to wrap.</param>
        /// <returns>A <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> that wraps the specified collection.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="dictionary"/> is null.</exception>
        public static ReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary is ReadOnlyDictionary<TKey, TValue>)
                return (ReadOnlyDictionary<TKey, TValue>)dictionary;
            return new ReadOnlyDictionary<TKey, TValue>(dictionary);
        }
    }

    /// <summary>
    /// Wraps a mutable collection in order to provide a read-only interface.
    /// </summary>
    /// <typeparam name="TKey">The type of keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of values in the dictionary.</typeparam>
    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(ReadOnlyDictionary<,>.DebugView))]
    public struct ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// The wrapped dictionary.
        /// </summary>
        private IDictionary<TKey, TValue> dictionary;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> structure around the
        /// specified collection.
        /// </summary>
        /// <param name="dictionary">The mutable collection to wrap.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="dictionary"/> is null.</exception>
        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            this.dictionary = Argument.EnsureNotNull(dictionary, "dictionary");
        }
        /// <summary>
        /// Gets a collection containing the keys in the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return dictionary.Keys; }
        }
        /// <summary>
        /// Gets a collection containing the values in the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>.
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return dictionary.Values; }
        }
        /// <summary>
        /// Determines whether the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>.</param>
        /// <returns>Returns true if the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> contains an element with the
        /// specified key; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key whose value to get.</param>
        /// <param name="value">When this method returns, the value associated with the specified key, if the key is found; otherwise, the
        /// default value for the type of the value parameter. This parameter is passed uninitialized.</param>
        /// <returns>Returns true if the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> contains an element with the
        /// specified key; otherwise, false.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }
        /// <summary>
        /// Gets the element with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get.</param>
        /// <returns>The element with the specified key.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and key is not found.</exception>
        public TValue this[TKey key]
        {
            get { return dictionary[key]; }
        }
        /// <summary>
        /// Determines whether the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>.</param>
        /// <returns>Returns true if item is found in the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>; otherwise,
        /// false.</returns>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return dictionary.Contains(item);
        }
        /// <summary>
        /// Copies the elements of the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> to an <see cref="T:System.Array"/>,
        /// starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from
        /// <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>. The <see cref="T:System.Array"/> must have zero-based
        /// indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source
        /// <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> is greater than the available space from
        /// <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            dictionary.CopyTo(array, arrayIndex);
        }
        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>.
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }
        /// <summary>
        /// Tests whether two specified <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> that is to the left of the equality
        /// operator.</param>
        /// <param name="right">The <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> that is to the right of the equality
        /// operator.</param>
        /// <returns>Returns true if the two <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> structures are equal;
        /// otherwise, false.</returns>
        public static bool operator ==(ReadOnlyDictionary<TKey, TValue> left, ReadOnlyDictionary<TKey, TValue> right)
        {
            return object.Equals(left.dictionary, right.dictionary);
        }
        /// <summary>
        /// Tests whether two specified <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> structures are different.
        /// </summary>
        /// <param name="left">The <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> that is to the left of the inequality
        /// operator.</param>
        /// <param name="right">The <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> that is to the right of the inequality
        /// operator.</param>
        /// <returns>Returns true if the two <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> structures are different;
        /// otherwise, false.</returns>
        public static bool operator !=(ReadOnlyDictionary<TKey, TValue> left, ReadOnlyDictionary<TKey, TValue> right)
        {
            return !(left == right);
        }
        /// <summary>
        /// Tests whether the specified object is a <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> structure and is
        /// equivalent to this <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> structure.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true if <paramref name="obj"/> is a <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> structure
        /// equivalent to this <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> structure; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ReadOnlyDictionary<TKey, TValue>))
                return false;
            return this == (ReadOnlyDictionary<TKey, TValue>)obj;
        }
        /// <summary>
        /// Returns a hash code for this <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> structure.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this
        /// <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>.</returns>
        public override int GetHashCode()
        {
            return unchecked(dictionary.GetHashCode() * 7);
        }
        /// <summary>
        /// Returns a new <see cref="T:System.NotSupportedException"/> with a message about the collection being read-only.
        /// </summary>
        /// <returns>A new <see cref="T:System.NotSupportedException"/> with a message about the collection being read-only.</returns>
        private static NotSupportedException ReadOnlyException()
        {
            return new NotSupportedException("Collection is read-only.");
        }
        /// <summary>
        /// Not supported by <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>.
        /// </summary>
        /// <param name="key">The parameter is not used.</param>
        /// <param name="value">The parameter is not used.</param>
        /// <exception cref="T:System.NotSupportedException">Thrown when the method is invoked.</exception>
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw ReadOnlyException();
        }
        /// <summary>
        /// Not supported by <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>.
        /// </summary>
        /// <param name="key">The parameter is not used.</param>
        /// <returns>The method does not return.</returns>
        /// <exception cref="T:System.NotSupportedException">Thrown when the method is invoked.</exception>
        bool IDictionary<TKey, TValue>.Remove(TKey key)
        {
            throw ReadOnlyException();
        }
        /// <summary>
        /// Gets the element with the specified key.
        /// </summary>
        /// <param name="key">The key of the element to get.</param>
        /// <returns>The element with the specified key. A set operation throws a <see cref="T:System.NotSupportedException"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="T:System.Collections.Generic.KeyNotFoundException">The property is retrieved and key is not found.</exception>
        /// <exception cref="T:System.NotSupportedException">The operation is set.</exception>
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get { return this[key]; }
            set { throw ReadOnlyException(); }
        }
        /// <summary>
        /// Not supported by <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>.
        /// </summary>
        /// <param name="item">The parameter is not used.</param>
        /// <exception cref="T:System.NotSupportedException">Thrown when the method is invoked.</exception>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw ReadOnlyException();
        }
        /// <summary>
        /// Not supported by <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>.
        /// </summary>
        /// <exception cref="T:System.NotSupportedException">Thrown when the method is invoked.</exception>
        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
        {
            throw ReadOnlyException();
        }
        /// <summary>
        /// Gets a value indicating whether the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/> is read-only. Returns true.
        /// </summary>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return true; }
        }
        /// <summary>
        /// Not supported by <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>.
        /// </summary>
        /// <param name="item">The parameter is not used.</param>
        /// <returns>The method does not return.</returns>
        /// <exception cref="T:System.NotSupportedException">Thrown when the method is invoked.</exception>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw ReadOnlyException();
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #region DebugView class
        /// <summary>
        /// Provides a debugger view for a <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2"/>.
        /// </summary>
        private sealed class DebugView
        {
            /// <summary>
            /// The dictionary for which an alternate view is being provided.
            /// </summary>
            private ReadOnlyDictionary<TKey, TValue> readOnlyDictionary;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:DesktopSprites.Collections.ReadOnlyDictionary`2.DebugView"/> class.
            /// </summary>
            /// <param name="readOnlyDictionary">The dictionary to proxy.</param>
            public DebugView(ReadOnlyDictionary<TKey, TValue> readOnlyDictionary)
            {
                this.readOnlyDictionary = Argument.EnsureNotNull(readOnlyDictionary, "readOnlyDictionary");
            }

            /// <summary>
            /// Gets a view of the items in the collection.
            /// </summary>
            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public KeyValuePair<TKey, TValue>[] Items
            {
                get { return readOnlyDictionary.dictionary.ToArray(); }
            }
        }
        #endregion
    }
}

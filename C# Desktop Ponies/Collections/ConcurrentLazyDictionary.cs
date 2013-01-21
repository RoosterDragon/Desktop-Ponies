namespace CsDesktopPonies.Collections
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    // TODO: Add documentation.

    /// <summary>
    /// Represents a thread-safe collection of keys and lazily-initialized values that can be accessed by multiple threads concurrently.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the lazily-initialized values in the dictionary.</typeparam>
    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    public sealed class ConcurrentLazyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// The underlying dictionary.
        /// </summary>
        private ConcurrentDictionary<TKey, Lazy<TValue>> dictionary;

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        public ConcurrentLazyDictionary()
        {
            dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        public ConcurrentLazyDictionary(IEqualityComparer<TKey> comparer)
        {
            Comparer = comparer ?? EqualityComparer<TKey>.Default;
            dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>(comparer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="concurrencyLevel"></param>
        /// <param name="capacity"></param>
        public ConcurrentLazyDictionary(int concurrencyLevel, int capacity)
        {
            dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>(concurrencyLevel, capacity);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="concurrencyLevel"></param>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        public ConcurrentLazyDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer)
        {
            Comparer = comparer ?? EqualityComparer<TKey>.Default;
            dictionary = new ConcurrentDictionary<TKey, Lazy<TValue>>(concurrencyLevel, capacity, comparer);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueFactory"></param>
        public ConcurrentLazyDictionary(Func<TKey, TValue> valueFactory)
            : this()
        {
            ValueFactory = valueFactory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparer"></param>
        /// <param name="valueFactory"></param>
        public ConcurrentLazyDictionary(IEqualityComparer<TKey> comparer, Func<TKey, TValue> valueFactory)
            : this(comparer)
        {
            ValueFactory = valueFactory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="concurrencyLevel"></param>
        /// <param name="capacity"></param>
        /// <param name="valueFactory"></param>
        public ConcurrentLazyDictionary(int concurrencyLevel, int capacity, Func<TKey, TValue> valueFactory)
            : this(concurrencyLevel, capacity)
        {
            ValueFactory = valueFactory;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="concurrencyLevel"></param>
        /// <param name="capacity"></param>
        /// <param name="comparer"></param>
        /// <param name="valueFactory"></param>
        public ConcurrentLazyDictionary(int concurrencyLevel, int capacity, IEqualityComparer<TKey> comparer, Func<TKey, TValue> valueFactory)
            : this(concurrencyLevel, capacity, comparer)
        {
            ValueFactory = valueFactory;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> that is used to determine equality of keys for the
        /// dictionary.
        /// </summary>
        public IEqualityComparer<TKey> Comparer { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<TValue> InitializedValues
        {
            get { return dictionary.Values.Where(lazy => lazy.IsValueCreated).Select(lazy => lazy.Value).ToArray(); }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return dictionary.Keys; }
        }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<KeyValuePair<TKey, Lazy<TValue>>> LazyItems
        {
            get { return new Enumerator<KeyValuePair<TKey, Lazy<TValue>>>(GetLazyEnumerator()); }
        }
        /// <summary>
        /// Gets the <see cref="T:System.Func`2"/> delegate that is used to lazily initialize values based on their key.
        /// </summary>
        public Func<TKey, TValue> ValueFactory { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<TValue> Values
        {
            get { return dictionary.Values.Select(lazy => lazy.Value).ToArray(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue this[TKey key]
        {
            get { return dictionary.GetOrAdd(key, CreateLazy).Value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get { return this[key]; }
            set { throw new NotSupportedException(); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Add(TKey key)
        {
            dictionary.GetOrAdd(key, CreateLazy);
        }
        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            dictionary.Clear();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            Lazy<TValue> lazy;
            bool found = dictionary.TryGetValue(item.Key, out lazy);
            return found ? EqualityComparer<TValue>.Default.Equals(item.Value, lazy.Value) : false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            Argument.EnsureNotNull(array, "array");
            Argument.EnsureNonnegative(arrayIndex, "arrayIndex");
            if (array.Length < arrayIndex)
                throw new ArgumentException("The length of array must be greater than or equal to arrayIndex.");

            KeyValuePair<TKey, Lazy<TValue>>[] lazyArray = new KeyValuePair<TKey, Lazy<TValue>>[array.Length - arrayIndex];
            ((IDictionary<TKey, Lazy<TValue>>)dictionary).CopyTo(lazyArray, 0);
            for (int i = 0; i < lazyArray.Length; i++)
                array[arrayIndex + i] = GetKeyValuePair(lazyArray[i]);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private Lazy<TValue> CreateLazy(TKey key)
        {
            if (ValueFactory == null)
                return new Lazy<TValue>();
            else
                return new Lazy<TValue>(() => ValueFactory(key));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lazyPair"></param>
        /// <returns></returns>
        private static KeyValuePair<TKey, TValue> GetKeyValuePair(KeyValuePair<TKey, Lazy<TValue>> lazyPair)
        {
            return new KeyValuePair<TKey, TValue>(lazyPair.Key, lazyPair.Value.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, Lazy<TValue>>> GetLazyEnumerator()
        {
            return dictionary.GetEnumerator();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(TKey key)
        {
            Lazy<TValue> lazy;
            return dictionary.TryRemove(key, out lazy);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            Lazy<TValue> lazy;
            bool success = dictionary.TryGetValue(key, out lazy);
            value = success ? lazy.Value : default(TValue);
            return success;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var kvp in dictionary)
                yield return GetKeyValuePair(kvp);
        }
        #endregion

        #region Unsupported Methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}

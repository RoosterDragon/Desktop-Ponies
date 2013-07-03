namespace CSDesktopPonies.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using CSDesktopPonies.Core;

    /// <summary> 
    /// Represents a collection of keys and lazily-initialized values.
    /// </summary>
    /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
    /// <typeparam name="TValue">The type of the lazily-initialized values in the dictionary.</typeparam>
    [System.Diagnostics.DebuggerDisplay("Count = {Count} InitializedCount = {InitializedCount}")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(LazyDictionary<,>.DebugView))]
    public sealed class LazyDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        /// <summary>
        /// The underlying dictionary.
        /// </summary>
        private Dictionary<TKey, Lazy<TValue>> dictionary;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> class that is empty, has the
        /// default initial capacity, uses the default equality comparer for the key type, and uses the default constructor of
        /// <typeparamref name="TValue"/> for initialization.
        /// </summary>
        public LazyDictionary()
        {
            dictionary = new Dictionary<TKey, Lazy<TValue>>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> class that is empty, has the
        /// specified initial capacity, uses the default equality comparer for the key type, and uses the default constructor of
        /// <typeparamref name="TValue"/> for initialization.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> can
        /// contain.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public LazyDictionary(int capacity)
        {
            dictionary = new Dictionary<TKey, Lazy<TValue>>(capacity);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> class that is empty, has the
        /// default initial capacity, uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/>, and uses the
        /// default constructor of <typeparamref name="TValue"/> for initialization.
        /// </summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> implementation to use when comparing
        /// keys, or null to use the default <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> for the type of the key.
        /// </param>
        public LazyDictionary(IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, Lazy<TValue>>(comparer);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> class that is empty, has the
        /// specified initial capacity, uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/>, and uses the
        /// default constructor of <typeparamref name="TValue"/> for initialization.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> can
        /// contain.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> implementation to use when comparing
        /// keys, or null to use the default <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> for the type of the key.
        /// </param>
        public LazyDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            dictionary = new Dictionary<TKey, Lazy<TValue>>(capacity, comparer);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> class that is empty, has the
        /// default initial capacity, uses the default equality comparer for the key type, and uses the specified initialization function
        /// for initialization of <typeparamref name="TValue"/>.
        /// </summary>
        /// <param name="valueFactory">The delegate that is invoked to produce the lazily initialized value, based on the key, when the
        /// value is needed, or null to invoke the default constructor for the type of value.</param>
        public LazyDictionary(Func<TKey, TValue> valueFactory)
            : this()
        {
            ValueFactory = valueFactory;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> class that is empty, has the
        /// specified initial capacity, uses the default equality comparer for the key type, and uses the specified initialization function
        /// for initialization of <typeparamref name="TValue"/>.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> can
        /// contain.</param>
        /// <param name="valueFactory">The delegate that is invoked to produce the lazily initialized value, based on the key, when the
        /// value is needed, or null to invoke the default constructor for the type of value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public LazyDictionary(int capacity, Func<TKey, TValue> valueFactory)
            : this(capacity)
        {
            ValueFactory = valueFactory;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> class that is empty, has the
        /// default initial capacity, uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/>, and uses the
        /// specified initialization function for initialization of <typeparamref name="TValue"/>.
        /// </summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> implementation to use when comparing
        /// keys, or null to use the default <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> for the type of the key.
        /// </param>
        /// <param name="valueFactory">The delegate that is invoked to produce the lazily initialized value, based on the key, when the
        /// value is needed, or null to invoke the default constructor for the type of value.</param>
        public LazyDictionary(IEqualityComparer<TKey> comparer, Func<TKey, TValue> valueFactory)
            : this(comparer)
        {
            ValueFactory = valueFactory;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> class that is empty, has the
        /// specified initial capacity, uses the specified <see cref="T:System.Collections.Generic.IEqualityComparer`1"/>, and uses the
        /// specified initialization function for initialization of <typeparamref name="TValue"/>.
        /// </summary>
        /// <param name="capacity">The initial number of elements that the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> can
        /// contain.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> implementation to use when comparing
        /// keys, or null to use the default <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> for the type of the key.
        /// </param>
        /// <param name="valueFactory">The delegate that is invoked to produce the lazily initialized value, based on the key, when the
        /// value is needed, or null to invoke the default constructor for the type of value.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="capacity"/> is less than 0.</exception>
        public LazyDictionary(int capacity, IEqualityComparer<TKey> comparer, Func<TKey, TValue> valueFactory)
            : this(capacity, comparer)
        {
            ValueFactory = valueFactory;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the <see cref="T:System.Collections.Generic.IEqualityComparer`1"/> that is used to determine equality of keys for the
        /// dictionary.
        /// </summary>
        public IEqualityComparer<TKey> Comparer
        {
            get { return dictionary.Comparer; }
        }
        /// <summary>
        /// Gets the number of key/value pairs contained in the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        public int Count
        {
            get { return dictionary.Count; }
        }
        /// <summary>
        /// Gets the number of key/value pairs, whose values are initialized, in the
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        public int InitializedCount { get; private set; }
        /// <summary>
        /// Gets an enumerator which iterates over only key/value pairs in the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>
        /// whose values are initialized. If any items are initialized during the enumeration, the enumerator is invalidated.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The collection was modified during enumeration.-or-An item was initialized
        /// during the enumeration.</exception>
        public IEnumerable<KeyValuePair<TKey, TValue>> InitializedItems
        {
            get { return Enumerable.From(GetInitializedEnumerator()); }
        }
        /// <summary>
        /// Gets a collection containing the only the initialized values in the
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        public ICollection<TValue> InitializedValues
        {
            get
            {
                TValue[] values = new TValue[InitializedCount];

                int index = 0;
                foreach (var kvp in InitializedItems)
                    values[index++] = kvp.Value;

                return values;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> is read-only. Returns false.
        /// </summary>
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// Gets a collection containing the keys in the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get { return dictionary.Keys; }
        }
        /// <summary>
        /// Gets the number of key/value pairs, whose values are uninitialized, in the
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        public int UninitializedCount
        {
            get { return Count - InitializedCount; }
        }
        /// <summary>
        /// Gets an enumerator which iterates over only keys in the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> whose
        /// values are uninitialized. If any items are initialized during the enumeration, the enumerator is invalidated.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified during enumeration.-or-An item was initialized
        /// during the enumeration.</exception>
        public IEnumerable<TKey> UninitializedKeys
        {
            get { return Enumerable.From(GetUninitializedEnumerator()); }
        }
        /// <summary>
        /// Gets the <see cref="T:System.Func`2"/> delegate that is used to lazily initialize values based on their key.
        /// </summary>
        public Func<TKey, TValue> ValueFactory { get; private set; }
        /// <summary>
        /// Gets a collection containing the values in the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                TValue[] values = new TValue[Count];

                int index = 0;
                foreach (var kvp in this)
                    values[index++] = kvp.Value;

                return values;
            }
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key. The value will be initialized if it has not yet been created. Any
        /// exception thrown trying to initialize the value will be propagated. If the specified key has not yet been added, it is added
        /// automatically and then the value is generated.</returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="key"/> is null.</exception>
        public TValue this[TKey key]
        {
            get
            {
                Add(key);
                Lazy<TValue> lazy = dictionary[key];
                if (!lazy.IsValueCreated)
                    InitializedCount++;
                return lazy.Value;
            }
        }
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key. The value will be initialized if it has not yet been created. Any
        /// exception thrown trying to initialize the value will be propagated. If the specified key has not yet been added, it is added
        /// automatically and then the value is generated. A set operation throws a <see cref="T:System.NotSupportedException"/>.</returns>
        /// <exception cref="T:System.ArgumentException"><paramref name="key"/> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">The operation is set.</exception>
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get { return this[key]; }
            set { throw new NotSupportedException("Not supported by " + GetType().Name); }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds the specified key to the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        /// <param name="key">The key of the element to add. If the key already exists, this method does nothing.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public void Add(TKey key)
        {
            if (!dictionary.ContainsKey(key))
                CreateEntry(key);
        }
        /// <summary>
        /// Creates an entry for the internal dictionary.
        /// </summary>
        /// <param name="key">The key of the element to add, for which a lazily initialized value will be created. The key should not yet
        /// exist.</param>
        private void CreateEntry(TKey key)
        {
            Lazy<TValue> lazy;
            if (ValueFactory == null)
                lazy = new Lazy<TValue>(LazyThreadSafetyMode.None);
            else
                lazy = new Lazy<TValue>(() => ValueFactory(key), LazyThreadSafetyMode.None);

            dictionary.Add(key, lazy);
        }
        /// <summary>
        /// Removes all keys and values from the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        public void Clear()
        {
            InitializedCount = 0;
            dictionary.Clear();
        }
        /// <summary>
        /// Determines whether the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.</param>
        /// <returns>Returns true if item is found in the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="item"/> is null.</exception>
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            Lazy<TValue> lazy;
            bool found = dictionary.TryGetValue(item.Key, out lazy);
            return found ? EqualityComparer<TValue>.Default.Equals(item.Value, lazy.Value) : false;
        }
        /// <summary>
        /// Determines whether the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> contains the specified key.
        /// </summary>
        /// <param name="key">The key to locate in the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.</param>
        /// <returns>Returns true if the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> contains an element with the
        /// specified key; otherwise, false.</returns>
        /// <exception cref="System.ArgumentException"><paramref name="key"/> is null.</exception>
        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }
        /// <summary>
        /// Copies the elements of the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> to an <see cref="T:System.Array"/>,
        /// starting at a particular <see cref="T:System.Array"/> index. Values will be initialized if they have not yet been created.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from the
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>. The <see cref="T:System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0 or greater than or equal to
        /// the size of the array.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> is greater than the available space from
        /// <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            CopyTo(array, arrayIndex, true);
        }
        /// <summary>
        /// Copies the elements of the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> to an <see cref="T:System.Array"/>,
        /// starting at a particular <see cref="T:System.Array"/> index. Uninitialized values will either be created or skipped, in
        /// accordance with the initialize parameter.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from the
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>. The <see cref="T:System.Array"/> must have zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="initialize">Specifies whether values should be initialized if they have not yet been created. If true, this method
        /// will initialize the values before adding the item to the array. If false, uninitialized items will not be copied to the array,
        /// and their values will remain uninitialized.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements to be copied from the
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> is greater than the available space from
        /// <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex, bool initialize)
        {
            Argument.EnsureNotNull(array, "array");
            Argument.EnsureNonnegative(arrayIndex, "arrayIndex");
            int count = initialize ? Count : InitializedCount;
            if (array.Length - arrayIndex < count)
                throw new ArgumentException("The size of the collection to copy is too small for the given array and arrayIndex.");

            IEnumerable<KeyValuePair<TKey, TValue>> items = initialize ? this : InitializedItems;
            foreach (var kvp in items)
                array[arrayIndex++] = kvp;
        }
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>. Uninitialized
        /// values will be created as required.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1"/> for the
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">The collection was modified during enumeration.</exception>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var kvp in dictionary)
            {
                if (!kvp.Value.IsValueCreated)
                    InitializedCount++;

                yield return KeyValuePair.From(kvp.Key, kvp.Value.Value);
            }
        }
        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>. Uninitialized
        /// values will be created as required.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> for the
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </returns>
        /// <exception cref="System.InvalidOperationException">The collection was modified during enumeration.</exception>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Returns an enumerator that iterates through only the initialized items in the
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1"/> for initialized items in the the
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.</returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified during enumeration.-or-An item was initialized
        /// during the enumeration.</exception>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetInitializedEnumerator()
        {
            int startingInitializedCount = InitializedCount;
            foreach (var kvp in dictionary)
            {
                if (startingInitializedCount != InitializedCount)
                    throw new InvalidOperationException("Values were initialized during enumeration of the collection.");

                if (kvp.Value.IsValueCreated)
                    yield return KeyValuePair.From(kvp.Key, kvp.Value.Value);
            }
        }
        /// <summary>
        /// Returns an enumerator that iterates through only the uninitialized keys in the
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1"/> for uninitialized keys in the the 
        /// <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.</returns>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified during enumeration.-or-An item was initialized
        /// during the enumeration.</exception>
        public IEnumerator<TKey> GetUninitializedEnumerator()
        {
            int startingInitializedCount = InitializedCount;
            foreach (var kvp in dictionary)
            {
                if (startingInitializedCount != InitializedCount)
                    throw new InvalidOperationException("Values were initialized during enumeration of the collection.");

                if (!kvp.Value.IsValueCreated)
                    yield return kvp.Key;
            }
        }
        /// <summary>
        /// Initializes all of the uninitialized items in the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        /// <param name="parallelize">Indicates if initialization may be done in parallel on worker threads. Pass true for faster loading,
        /// or pass false to load on the current thread.</param>
        /// <exception cref="T:System.AggregateException">Collection of all exceptions thrown by the initialization of items.</exception>
        public void InitializeAll(bool parallelize)
        {
            InitializeAll(parallelize, null);
        }
        /// <summary>
        /// Initializes all of the uninitialized items in the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        /// <param name="parallelize">Indicates if initialization may be done in parallel on worker threads. Pass true for faster loading,
        /// or pass false to load on the current thread.</param>
        /// <param name="itemInitializedHandler">An <see cref="T:System.EventHandler`1"/> that is raised when an item is initialized.
        /// </param>
        /// <exception cref="T:System.AggregateException">Collection of all exceptions thrown by the initialization of items.</exception>
        public void InitializeAll(bool parallelize, EventHandler<ItemInitializedEventArgs> itemInitializedHandler)
        {
            if (UninitializedCount == 0)
                return;

            int initalRemaining = UninitializedCount;
            LinkedList<Exception> initializeExceptions = null;
            object syncInitialize = new object();
            lock (syncInitialize)
            {
                foreach (Lazy<TValue> loopLazy in dictionary.Values)
                    if (!loopLazy.IsValueCreated)
                    {
                        // C# 4.0 - The anonymous method captures the final value of the foreach loop. Therefore a local copy must be made.
                        // This will be fixed with a breaking change in C# 5.0. If you're reading this and it is C# 5.0, fix me please :).
                        Lazy<TValue> lazy = loopLazy;
                        WaitCallback load = o =>
                        {
                            try
                            {
                                TValue value = lazy.Value;
                            }
                            catch (Exception ex)
                            {
                                if (initializeExceptions == null)
                                    initializeExceptions = new LinkedList<Exception>();
                                initializeExceptions.AddLast(ex);
                            }

                            if (parallelize)
                                Monitor.Enter(syncInitialize);
                            try
                            {
                                InitializedCount++;
                                itemInitializedHandler.Raise(this, () => new ItemInitializedEventArgs(
                                    initalRemaining - UninitializedCount, UninitializedCount));
                                if (parallelize && UninitializedCount == 0)
                                    Monitor.Pulse(syncInitialize);
                            }
                            finally
                            {
                                if (parallelize)
                                    Monitor.Exit(syncInitialize);
                            }
                        };

                        if (parallelize)
                            ThreadPool.QueueUserWorkItem(load);
                        else
                            load(null);
                    }
                if (parallelize)
                    Monitor.Wait(syncInitialize);
            }

            if (initializeExceptions != null)
                throw new AggregateException(initializeExceptions);
        }
        /// <summary>
        /// Removes the value with the specified key from the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        /// <param name="key">The key of the element to remove.</param>
        /// <returns>Returns true if the element is successfully found and removed; otherwise, false. This method returns false if key is
        /// not found in the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">The collection was modified whilst attempting to remove the item. The
        /// internal state of the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> is now unreliable.</exception>
        public bool Remove(TKey key)
        {
            Lazy<TValue> lazy;
            bool exists = dictionary.TryGetValue(key, out lazy);
            if (exists && lazy.IsValueCreated)
                InitializedCount--;
            bool removed = dictionary.Remove(key);
            if (exists != removed)
                throw new InvalidOperationException("Collection was modified whilst attempting to remove an item.");
            return removed;
        }
        /// <summary>
        /// Removes all of the uninitialized items in the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        public void RemoveUninitialized()
        {
            TKey[] uninitializedKeys = new TKey[UninitializedCount];
            int i = 0;
            foreach (TKey key in UninitializedKeys)
                uninitializedKeys[i++] = key;

            foreach (TKey key in uninitializedKeys)
                dictionary.Remove(key);
        }
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value, which is initialized if it has not yet been created,
        /// associated with the specified key, if the key is found; otherwise, the default value for the type of the value parameter. This
        /// parameter is passed uninitialized.</param>
        /// <returns>Returns true if the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> contains an element with the
        /// specified key; otherwise, false.</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        bool IDictionary<TKey, TValue>.TryGetValue(TKey key, out TValue value)
        {
            return TryGetValue(key, out value, true);
        }
        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found and is
        /// initialized or initialize is true; otherwise, the default value for the type of the value parameter. This parameter is passed
        /// uninitialized.</param>
        /// <param name="initialize">Specifies whether the value should be initialized if it has not yet been created. If true, this method
        /// will initialize the value. If false, this method will return false even if the key is located, and the value will remain
        /// uninitialized.</param>
        /// <returns>Returns true if the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/> contains an element with the
        /// specified key whose value is already initialized or was made to initialize by setting initialize to true; otherwise, false.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="key"/> is null.</exception>
        public bool TryGetValue(TKey key, out TValue value, bool initialize)
        {
            Lazy<TValue> lazy;
            bool exists = dictionary.TryGetValue(key, out lazy);

            if (exists && initialize && !lazy.IsValueCreated)
                InitializedCount++;

            bool result = exists && (initialize || lazy.IsValueCreated);
            value = result ? lazy.Value : default(TValue);

            return result;
        }
        #endregion

        #region Unsupported Methods
        /// <summary>
        /// Not supported by <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        /// <param name="key">The parameter is not used.</param>
        /// <param name="value">The parameter is not used.</param>
        /// <exception cref="T:System.NotSupportedException">Thrown when the method is invoked.</exception>
        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            throw new NotSupportedException("Not supported by " + GetType().Name);
        }
        /// <summary>
        /// Not supported by <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        /// <param name="item">The parameter is not used.</param>
        /// <exception cref="T:System.NotSupportedException">Thrown when the method is invoked.</exception>
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("Not supported by " + GetType().Name);
        }
        /// <summary>
        /// Not supported by <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        /// <param name="item">The parameter is not used.</param>
        /// <returns>The method does not return.</returns>
        /// <exception cref="T:System.NotSupportedException">Thrown when the method is invoked.</exception>
        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new NotSupportedException("Not supported by " + GetType().Name);
        }
        #endregion

        #region DebugView class
        /// <summary>
        /// Provides a debugger view for a <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2"/>.
        /// </summary>
        private sealed class DebugView
        {
            /// <summary>
            /// The dictionary for which an alternate view is being provided.
            /// </summary>
            private LazyDictionary<TKey, TValue> lazyDictionary;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.LazyDictionary`2.DebugView"/>
            /// class.
            /// </summary>
            /// <param name="lazyDictionary">The dictionary to proxy.</param>
            public DebugView(LazyDictionary<TKey, TValue> lazyDictionary)
            {
                Argument.EnsureNotNull(lazyDictionary, "lazyDictionary");
                this.lazyDictionary = lazyDictionary;
            }

            /// <summary>
            /// Provides for display of an uninitialized item by displaying only its key.
            /// </summary>
            [System.Diagnostics.DebuggerDisplay("\\{{Key,nq}\\} (Value uninitialized)")]
            public struct KeyUnit
            {
                /// <summary>
                /// Gets or sets the key in the key unit.
                /// </summary>
                public TKey Key { get; set; }
            }

            /// <summary>
            /// Gets a view of the items in the dictionary.
            /// </summary>
            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public object[] Items
            {
                get
                {
                    var array = new object[lazyDictionary.dictionary.Count];
                    int i = 0;
                    foreach (var kvp in lazyDictionary.dictionary)
                        if (kvp.Value.IsValueCreated)
                            array[i++] = KeyValuePair.From(kvp.Key, kvp.Value.Value);
                        else
                            array[i++] = new KeyUnit() { Key = kvp.Key };
                    return array;
                }
            }
        }
        #endregion
    }
}

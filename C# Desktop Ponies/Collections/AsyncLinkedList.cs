namespace CSDesktopPonies.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    // TODO: Fix documentation.

    /// <summary>
    /// Represents a thread-safe doubly linked list.
    /// </summary>
    /// <remarks>
    /// A thread-safe version of <see cref="T:System.Collections.Generic.LinkedList`1"/>. Methods that add or remove items in the list are
    /// performed asynchronously and will return immediately. Use the events provided to respond when the items are actually added or
    /// removed. Enumerating or sorting the list is safe and will block changes until those operations complete. Outside the enumeration of
    /// items, all properties are susceptible to time-of-check to time-of-use errors, as the collection may have operations queued which
    /// will execute between those times. This includes event handlers.
    /// </remarks>
    /// <typeparam name="T">The type of items in the linked list.</typeparam>
    [System.Diagnostics.DebuggerDisplay("Count = {Count}")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(AsyncLinkedList<>.DebugView))]
    public sealed class AsyncLinkedList<T> : ICollection<T>
    {
        #region Enumerator struct
        /// <summary>
        /// Enumerates the elements of a <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </summary>
        public struct Enumerator : IEnumerator<T>
        {
            /// <summary>
            /// The enumerator of the underlying collection.
            /// </summary>
            private LinkedList<T>.Enumerator enumerator;
            /// <summary>
            /// The synchronization object used to lock the collection and prevent the enumerator becoming invalidated.
            /// </summary>
            private object syncObject;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1.Enumerator"/> struct
            /// for the given <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
            /// </summary>
            /// <param name="collection">The <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> to enumerate.</param>
            internal Enumerator(AsyncLinkedList<T> collection)
            {
                syncObject = collection.syncObject;
                Monitor.Enter(syncObject);
                enumerator = collection.list.GetEnumerator();
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            void System.Collections.IEnumerator.Reset()
            {
                ((System.Collections.IEnumerator)enumerator).Reset();
            }
            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>Returns true if the enumerator was successfully advanced to the next element; false if the enumerator has passed
            /// the end of the collection.</returns>
            public bool MoveNext()
            {
                return enumerator.MoveNext();
            }
            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            public T Current
            {
                get { return enumerator.Current; }
            }
            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            object System.Collections.IEnumerator.Current
            {
                get { return Current; }
            }
            /// <summary>
            /// Releases all resources used by the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1.Enumerator"/>
            /// object.
            /// </summary>
            public void Dispose()
            {
                enumerator.Dispose();
                Monitor.Exit(syncObject);
            }
        }
        #endregion

        /// <summary>
        /// The underlying linked list.
        /// </summary>
        private LinkedList<T> list;
        /// <summary>
        /// Synchronization object for use when mutating the collection.
        /// </summary>
        private object syncObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> class that is empty.
        /// </summary>
        public AsyncLinkedList()
        {
            list = new LinkedList<T>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> class that contains elements
        /// copied from the specified <see cref="T:System.Collections.Generic.IEnumerable`1"/> and has sufficient capacity to accommodate
        /// the number of elements copied.
        /// </summary>
        /// <param name="collection">The <see cref="T:System.Collections.Generic.IEnumerable`1"/> whose elements are copied to the new
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="collection"/> is null.</exception>
        public AsyncLinkedList(IEnumerable<T> collection)
        {
            list = new LinkedList<T>(collection);
        }

        /// <summary>
        /// Occurs when a single item is added to the collection.
        /// </summary>
        public event EventHandler<CollectionItemChangedEventArgs<T>> ItemAdded;
        /// <summary>
        /// Occurs when a single item is successfully removed from the collection.
        /// </summary>
        public event EventHandler<CollectionItemChangedEventArgs<T>> ItemRemoved;
        /// <summary>
        /// Occurs when multiple items are added to the collection.
        /// </summary>
        public event EventHandler<CollectionItemsChangedEventArgs<T>> ItemsAdded;
        /// <summary>
        /// Occurs when multiple items are successfully removed from the collection.
        /// </summary>
        public event EventHandler<CollectionItemsChangedEventArgs<T>> ItemsRemoved;

        /// <summary>
        /// Gets the number of nodes actually contained in the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </summary>
        public int Count
        {
            get
            {
                lock (syncObject)
                    return list.Count;
            }
        }
        /// <summary>
        /// Gets a value indicating whether the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> is read-only.
        /// </summary>
        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }
        /// <summary>
        /// Gets the first node of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </summary>
        public LinkedListNode<T> First
        {
            get
            {
                lock (syncObject)
                    return list.First;
            }
        }
        /// <summary>
        /// Gets the last node of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </summary>
        public LinkedListNode<T> Last
        {
            get
            {
                lock (syncObject)
                    return list.Last;
            }
        }

        /// <summary>
        /// Runs <paramref name="addMethod"/> to add an item to the collection, and raises the
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemAdded"/> event using <paramref name="value"/>.
        /// </summary>
        /// <param name="addMethod">The method which adds an item to the underlying collection.</param>
        /// <param name="value">The value that will be added to the collection.</param>
        private void Add(Action addMethod, T value)
        {
            ThreadPool.QueueUserWorkItem(o => AddSync(addMethod, value));
        }
        /// <summary>
        /// Runs <paramref name="addMethod"/> to add an item to the collection synchronously, and raises the
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemAdded"/> event using <paramref name="value"/>.
        /// </summary>
        /// <param name="addMethod">The method which adds an item to the underlying collection.</param>
        /// <param name="value">The value that will be added to the collection.</param>
        private void AddSync(Action addMethod, T value)
        {
            lock (syncObject)
                addMethod();
            ItemAdded.Raise(this, () => new CollectionItemChangedEventArgs<T>(value));
        }
        /// <summary>
        /// Runs <paramref name="addMethod"/> on the first node in <paramref name="nodes"/> and then inserts remaining nodes after that in
        /// order to add a range of items to the collection. The <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemsAdded"/>
        /// event is then raised.
        /// </summary>
        /// <param name="addMethod">The method which adds an item to the underlying collection.</param>
        /// <param name="nodes">The nodes that will be added to the collection.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="nodes"/> is null.</exception>
        private void AddRange(Action<LinkedListNode<T>> addMethod, IEnumerable<LinkedListNode<T>> nodes)
        {
            Argument.EnsureNotNull(nodes, "nodes");

            ThreadPool.QueueUserWorkItem(o =>
            {
                var itemsAdded = ItemsAdded;
                LinkedList<T> valuesAdded = null;
                if (itemsAdded != null)
                    valuesAdded = new LinkedList<T>();
                lock (syncObject)
                {
                    LinkedListNode<T> first = null;
                    foreach (LinkedListNode<T> node in nodes)
                    {
                        if (first == null)
                        {
                            addMethod(node);
                            first = node;
                        }
                        else
                        {
                            list.AddAfter(first, node);
                        }
                        if (itemsAdded != null)
                            valuesAdded.AddLast(node.Value);
                    }
                }
                if (itemsAdded != null)
                    itemsAdded(this, new CollectionItemsChangedEventArgs<T>(valuesAdded));
            });
        }
        /// <summary>
        /// Runs <paramref name="addMethod"/> on the first value in <paramref name="values"/> and then inserts remaining values after that
        /// in order to add a range of items to the collection. The
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemsAdded"/> event is then raised.
        /// </summary>
        /// <param name="addMethod">The method which adds an item to the underlying collection.</param>
        /// <param name="values">The values that will be added to the collection.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="values"/> is null.</exception>
        private void AddRange(Func<T, LinkedListNode<T>> addMethod, IEnumerable<T> values)
        {
            Argument.EnsureNotNull(values, "values");
            
            ThreadPool.QueueUserWorkItem(o =>
            {
                var itemsAdded = ItemsAdded;
                LinkedList<T> valuesAdded = itemsAdded != null ? new LinkedList<T>() : null;
                lock (syncObject)
                {
                    LinkedListNode<T> first = null;
                    foreach (T value in values)
                    {
                        if (first == null)
                            first = addMethod(value);
                        else
                            list.AddAfter(first, value);
                        if (itemsAdded != null)
                            valuesAdded.AddLast(value);
                    }
                }
                if (itemsAdded != null)
                    itemsAdded(this, new CollectionItemsChangedEventArgs<T>(valuesAdded));
            });
        }
        /// <summary>
        /// Runs <paramref name="removeMethod"/> to remove an item from the collection, and raises the
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemRemoved"/> event using <paramref name="value"/>.
        /// </summary>
        /// <param name="removeMethod">The method which removes an item from the underlying collection.</param>
        /// <param name="value">The value that will be removed from the collection.</param>
        private void Remove(Action removeMethod, T value)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                lock (syncObject)
                    removeMethod();
                ItemRemoved.Raise(this, () => new CollectionItemChangedEventArgs<T>(value));
            });
        }
        /// <summary>
        /// Removes the first occurrence of the specified value from the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> synchronously. The
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemRemoved"/> event will be raised once the value is
        /// removed. The event will not be raised if the value was not removed, or if the value was not found in the original
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </param>
        /// <returns>Return true if <paramref name="item"/> was successfully removed from the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>; otherwise, false. This method also returns false if
        /// <paramref name="item"/> is not found in the original <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </returns>
        private bool RemoveSync(T item)
        {
            bool removed;
            lock (syncObject)
                removed = list.Remove(item);
            if (removed)
                ItemRemoved.Raise(this, () => new CollectionItemChangedEventArgs<T>(item));
            return removed;
        }
        /// <summary>
        /// Removes the all the elements that match the conditions defined by the specified predicate synchronously. The
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemsRemoved"/> event will be raised once the elements are
        /// removed.
        /// </summary>
        /// <param name="match">The <see cref="T:System.Predicate`1"/> delegate that defines the conditions of the elements to remove.
        /// </param>
        private void RemoveAllSync(Predicate<T> match)
        {
            var itemsRemoved = ItemsRemoved;
            LinkedList<T> valuesRemoved = itemsRemoved != null ? new LinkedList<T>() : null;
            lock (syncObject)
            {
                LinkedListNode<T> node = list.First;
                while (node != null)
                {
                    LinkedListNode<T> nextNode = node.Next;
                    if (match(node.Value))
                    {
                        list.Remove(node);
                        if (itemsRemoved != null)
                            valuesRemoved.AddLast(node);
                    }
                    node = nextNode;
                }
            }
            if (itemsRemoved != null && valuesRemoved.Count != 0)
                itemsRemoved(this, new CollectionItemsChangedEventArgs<T>(valuesRemoved));
        }

        /// <summary>
        /// Adds a new node containing the specified value at the start of the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>. This method is performed asynchronously. The
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemAdded"/> event will be raised once the value is added.
        /// </summary>
        /// <param name="value">The value to add at the start of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.</param>
        public void AddFirst(T value)
        {
            Add(() => list.AddFirst(value), value);
        }
        /// <summary>
        /// Adds a new node containing the specified value at the end of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// This method is performed asynchronously. The <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemAdded"/> event will
        /// be raised once the value is added.
        /// </summary>
        /// <param name="value">The value to add at the end of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.</param>
        public void AddLast(T value)
        {
            Add(() => list.AddLast(value), value);
        }
        /// <summary>
        /// Adds the specified nodes at the start of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>. This method is
        /// performed asynchronously. The <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemsAdded"/> event will be raised
        /// once the nodes are added.
        /// </summary>
        /// <param name="nodes">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> of the new
        /// <see cref="T:System.Collections.Generic.LinkedListNode`1"/> to add at the start of the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="nodes"/> is null.-or-Any node in <paramref name="nodes"/> is
        /// null.</exception>
        /// <exception cref="T:System.InvalidOperationException">A node in <paramref name="nodes"/> belongs to another
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.</exception>
        public void AddRangeFirst(IEnumerable<LinkedListNode<T>> nodes)
        {
            AddRange(list.AddFirst, nodes);
        }
        /// <summary>
        /// Adds new nodes containing the specified values at the start of the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>. This method is performed asynchronously. The
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemsAdded"/> event will be raised once the values are
        /// added.
        /// </summary>
        /// <param name="values">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> of the values to add at the start of the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="values"/> is null.</exception>
        public void AddRangeFirst(IEnumerable<T> values)
        {
            AddRange(list.AddFirst, values);
        }
        /// <summary>
        /// Adds the specified new nodes at the end of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>. This
        /// method is performed asynchronously. The <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemsAdded"/>
        /// event will be raised once the nodes are added.
        /// </summary>
        /// <param name="nodes">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> of the new
        /// <see cref="T:System.Collections.Generic.LinkedListNode`1"/> to add at the end of the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="nodes"/> is null.-or-Any node in <paramref name="nodes"/> is
        /// null.</exception>
        /// <exception cref="T:System.InvalidOperationException">A node in <paramref name="nodes"/> belongs to another
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.</exception>
        public void AddRangeLast(IEnumerable<LinkedListNode<T>> nodes)
        {
            AddRange(list.AddLast, nodes);
        }
        /// <summary>
        /// Adds new nodes containing the specified values at the end of the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>. This method is performed asynchronously. The
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemsAdded"/> event will be raised once the values are
        /// added.
        /// </summary>
        /// <param name="values">The values to add at the end of the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="values"/> is null.</exception>
        public void AddRangeLast(IEnumerable<T> values)
        {
            AddRange(list.AddLast, values);
        }
        /// <summary>
        /// Removes all nodes from the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> asynchronously. This method
        /// is performed asynchronously. The <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemsRemoved"/> event
        /// will be raised once the collection is cleared.
        /// </summary>
        public void Clear()
        {
            RemoveAll(value => true);
        }
        /// <summary>
        /// Determines whether a value is in the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </summary>
        /// <param name="value">The value to locate in the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>. The
        /// value can be null for reference types.</param>
        /// <returns>Returns true if <paramref name="value"/> is found in the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>; otherwise, false.</returns>
        public bool Contains(T value)
        {
            lock (syncObject)
                return list.Contains(value);
        }
        /// <summary>
        /// Copies the entire <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> to a compatible one-dimensional
        /// <see cref="T:System.Array"/>, starting at the specified index of the target array.
        /// </summary>
        /// <param name="arrayFactory">A <see cref="T:System.Func`2"/> that specifies the one-dimensional <see cref="T:System.Array"/>
        /// that is the destination of the elements copied from <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>. The
        /// <see cref="T:System.Array"/> must have zero-based indexing. The input parameter is the count of elements in the collection.
        /// This is to prevent a time-of-check to time-of-use problem they may result by checking the count outside locking the collection.
        /// </param>
        /// <param name="indexFactory">A <see cref="T:System.Func`2"/> that specifies the zero-based index in the resulting array at which
        /// copying begins. The input parameter is the count of elements in the collection.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="arrayFactory"/> is null.-or-<paramref name="indexFactory"/> is
        /// null.-or-The array produced by <paramref name="arrayFactory"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The index produced by <paramref name="indexFactory"/> is less than zero.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> is greater than the available space from the index produced by 
        /// <paramref name="indexFactory"/> to the end of the destination array produced by <paramref name="arrayFactory"/>.</exception>
        /// <returns>Returns the destination array produced by <paramref name="arrayFactory"/> to which elements were copied.</returns>
        public T[] CopyTo(Func<int, T[]> arrayFactory, Func<int, int> indexFactory)
        {
            Argument.EnsureNotNull(arrayFactory, "arrayFactory");
            Argument.EnsureNotNull(indexFactory, "indexFactory");

            lock (syncObject)
            {
                T[] array = arrayFactory(list.Count);
                int index = indexFactory(list.Count);
                list.CopyTo(array, index);
                return array;
            }
        }
        /// <summary>
        /// Finds the first node that contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.</param>
        /// <returns>The first <see cref="T:System.Collections.Generic.LinkedListNode`1"/> that contains the specified value, if found;
        /// otherwise, null.</returns>
        public LinkedListNode<T> Find(T value)
        {
            lock (syncObject)
                return list.Find(value);
        }
        /// <summary>
        /// Finds the last node that contains the specified value.
        /// </summary>
        /// <param name="value">The value to locate in the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.</param>
        /// <returns>The last <see cref="T:System.Collections.Generic.LinkedListNode`1"/> that contains the specified value, if found;
        /// otherwise, null.</returns>
        public LinkedListNode<T> FindLast(T value)
        {
            lock (syncObject)
                return list.FindLast(value);
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator"/> that can be used to iterate through the collection.</returns>
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
        /// <summary>
        /// Removes the first occurrence of the specified value from the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>. This method is performed asynchronously. The
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemRemoved"/> event will be raised once the value is
        /// removed. The event will not be raised if the value was not removed, or if the value was not found in the original
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </summary>
        /// <param name="value">The value to remove from the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </param>
        public void Remove(T value)
        {
            ThreadPool.QueueUserWorkItem(o => RemoveSync(value));
        }
        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified predicate. This method is performed asynchronously.
        /// The <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemsRemoved"/> event will be raised once the elements are
        /// removed. The event will be raised with an empty collection if no items were removed.
        /// </summary>
        /// <param name="match">The <see cref="T:System.Predicate`1"/> delegate that defines the conditions of the elements to remove.
        /// </param>
        public void RemoveAll(Predicate<T> match)
        {
            ThreadPool.QueueUserWorkItem(o => RemoveAllSync(match));
        }
        /// <summary>
        /// Removes the node at the start of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>. This method is
        /// performed asynchronously. The <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemRemoved"/> event will be
        /// raised once the node is removed.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> is empty.</exception>
        public void RemoveFirst()
        {
            Remove(list.RemoveFirst, list.First.Value);
        }
        /// <summary>
        /// Removes the node at the end of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>. This method is
        /// performed asynchronously. The <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemRemoved"/> event will be
        /// raised once the node is removed.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> is empty.</exception>
        public void RemoveLast()
        {
            Remove(list.RemoveLast, list.Last.Value);
        }
        /// <summary>
        /// Sorts the elements in the entire <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> using the default
        /// comparer.
        /// </summary>
        public void Sort()
        {
            lock (syncObject)
                list.Sort();
        }
        /// <summary>
        /// Sorts the elements in the entire <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> using the specified
        /// <see cref="T:System.Comparison"/>.
        /// </summary>
        /// <param name="comparison">The <see cref="T:System.Comparison"/> to use when comparing elements.</param>
        public void Sort(Comparison<T> comparison)
        {
            lock (syncObject)
                list.Sort(comparison);
        }
        /// <summary>
        /// Sorts the elements in the entire <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> using the specified
        /// comparer.
        /// </summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer"/> implementation to use when comparing elements,
        /// or null to use the default comparer
        /// <see cref="T:System.Collections.Generic.Comparer.Default"/>.</param>
        public void Sort(IComparer<T> comparer)
        {
            lock (syncObject)
                list.Sort(comparer);
        }

        /// <summary>
        /// Adds an item to the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> synchronously. The
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemAdded"/> event will be raised once the item is added.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.</param>
        void ICollection<T>.Add(T item)
        {
            AddSync(() => ((ICollection<T>)list).Add(item), item);
        }
        /// <summary>
        /// Removes all nodes from the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> synchronously. The
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemsRemoved"/> event will be raised once the collection
        /// is cleared.
        /// </summary>
        void ICollection<T>.Clear()
        {
            RemoveAllSync(value => true);
        }
        /// <summary>
        /// Copies the entire <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> to a compatible one-dimensional
        /// <see cref="T:System.Array"/>, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="T:System.Array"/>
        /// that is the destination of the elements copied from <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// The
        /// <see cref="T:System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        /// <exception cref="T:System.ArgumentException">The number of elements in the source
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> is greater than the available space from
        /// <paramref name="index"/> to the end of the destination array.</exception>
        void ICollection<T>.CopyTo(T[] array, int index)
        {
            lock (syncObject)
                list.CopyTo(array, index);
        }
        /// <summary>
        /// Removes the first occurrence of the specified value from the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/> synchronously. The
        /// <see cref="E:CSDesktopPonies.Collections.AsyncLinkedList`1.ItemRemoved"/> event will be raised once the value is
        /// removed. The event will not be raised if the value was not removed, or if the value was not found in the original
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </param>
        /// <returns>Return true if <paramref name="item"/> was successfully removed from the
        /// <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>; otherwise, false. This method also returns false if
        /// <paramref name="item"/> is not found in the original <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </returns>
        bool ICollection<T>.Remove(T item)
        {
            return RemoveSync(item);
        }
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>A <see cref="T:System.Collections.Generic.IEnumerator"/> that can be used to iterate through the collection.</returns>
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return GetEnumerator();
        }
        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region DebugView class
        /// <summary>
        /// Provides a debugger view for an <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1"/>.
        /// </summary>
        private sealed class DebugView
        {
            /// <summary>
            /// The list for which an alternate view is being provided.
            /// </summary>
            private AsyncLinkedList<T> asyncLinkedList;

            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.AsyncLinkedList`1.DebugView"/>
            /// class.
            /// </summary>
            /// <param name="asyncLinkedList">The list to proxy.</param>
            public DebugView(AsyncLinkedList<T> asyncLinkedList)
            {
                Argument.EnsureNotNull(asyncLinkedList, "asyncLinkedList");
                this.asyncLinkedList = asyncLinkedList;
            }

            /// <summary>
            /// Gets a view of the items in the list.
            /// </summary>
            [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.RootHidden)]
            public T[] Items
            {
                get
                {
                    var array = new T[asyncLinkedList.list.Count];
                    asyncLinkedList.list.CopyTo(array, 0);
                    return array;
                }
            }
        }
        #endregion
    }
}
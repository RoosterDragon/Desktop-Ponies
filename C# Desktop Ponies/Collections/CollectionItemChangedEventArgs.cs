namespace CsDesktopPonies.Collections
{
    using System;

    /// <summary>
    /// Provides data for when an item is added or removed from a collection.
    /// </summary>
    /// <typeparam name="T">The type of the item that was added or removed.</typeparam>
    public class CollectionItemChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the item that was added or removed from the collection.
        /// </summary>
        public T Item { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.Collections.CollectionItemChangedEventArgs`1"/> class.
        /// </summary>
        /// <param name="item">The item that was added or removed from the collection.</param>
        public CollectionItemChangedEventArgs(T item)
        {
            Item = item;
        }
    }
}

namespace CsDesktopPonies.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides data for when multiple items are added or removed from a collection.
    /// </summary>
    /// <typeparam name="T">The type of the items that were added or removed.</typeparam>
    public class CollectionItemsChangedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the items that were added or removed from the collection.
        /// </summary>
        public ICollection<T> Items { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.Collections.CollectionItemsChangedEventArgs`1"/> class.
        /// </summary>
        /// <param name="items">The items that were added or removed from the collection.</param>
        public CollectionItemsChangedEventArgs(ICollection<T> items)
        {
            Items = items;
        }
    }
}

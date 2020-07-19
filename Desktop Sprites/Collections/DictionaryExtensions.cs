namespace DesktopSprites.Collections
{
    using System;
    using System.Collections.Generic;
    using DesktopSprites.Core;

    /// <summary>
    /// Provides general extension methods to the <see cref="T:System.Collections.Generic.IDictionary`2"/> interface.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Adds a key/value pair to the <see cref="T:System.Collections.Generic.IDictionary`2"/> if the key does not already exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to query.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="value">The value to be added, if the key does not already exist.</param>
        /// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or
        /// the new value if the key was not in the dictionary.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="dictionary"/> is null.-or-<paramref name="key"/> is null.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">An attempt at adding was made, but the
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        {
            Argument.EnsureNotNull(dictionary, nameof(dictionary));
            if (!dictionary.TryGetValue(key, out TValue currentValue))
                dictionary.Add(key, currentValue = value);
            return currentValue;
        }

        /// <summary>
        /// Adds a key/value pair to the <see cref="T:System.Collections.Generic.IDictionary`2"/> by using the specified function, if the
        /// key does not already exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
        /// <typeparam name="TValue">The type of the values in the dictionary.</typeparam>
        /// <param name="dictionary">The dictionary to query.</param>
        /// <param name="key">The key of the element to add.</param>
        /// <param name="valueFactory">The function used to generate a value for the key.</param>
        /// <returns>The value for the key. This will be either the existing value for the key if the key is already in the dictionary, or
        /// the new value for the key as returned by <paramref name="valueFactory"/> if the key was not in the dictionary.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="dictionary"/> is null.-or-<paramref name="key"/> is null.-or-
        /// <paramref name="valueFactory"/> is null.</exception>
        /// <exception cref="T:System.NotSupportedException">An attempt at adding was made, but the
        /// <see cref="T:System.Collections.Generic.IDictionary`2"/> is read-only.</exception>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> valueFactory)
        {
            Argument.EnsureNotNull(dictionary, nameof(dictionary));
            Argument.EnsureNotNull(valueFactory, nameof(valueFactory));
            if (!dictionary.TryGetValue(key, out TValue currentValue))
                dictionary.Add(key, currentValue = valueFactory(key));
            return currentValue;
        }
    }
}

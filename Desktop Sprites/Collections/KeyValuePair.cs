namespace DesktopSprites.Collections
{
    using System.Collections.Generic;
    using DesktopSprites.Core;

    /// <summary>
    /// Defines a key/value pair that can be set or retrieved.
    /// </summary>
    public static class KeyValuePair
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Collections.Generic.KeyValuePair`2"/> structure with the specified key
        /// and value.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="key">The object defined in each key/value pair.</param>
        /// <param name="value">The definition associated with key.</param>
        /// <returns>A new <see cref="T:System.Collections.Generic.KeyValuePair`2"/>.</returns>
        public static KeyValuePair<TKey, TValue> From<TKey, TValue>(TKey key, TValue value)
        {
            return new KeyValuePair<TKey, TValue>(key, value);
        }

        /// <summary>
        /// Enumerates the keys from a sequence of key-value pairs.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The sequence to enumerate.</param>
        /// <returns>An enumerable of the keys from a sequence of key-value pairs.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEnumerable<TKey> Keys<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            Argument.EnsureNotNull(source, "source");
            foreach (var kvp in source)
                yield return kvp.Key;
        }

        /// <summary>
        /// Enumerates the values from a sequence of key-value pairs.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="source">The sequence to enumerate.</param>
        /// <returns>An enumerable of the values from a sequence of key-value pairs.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static IEnumerable<TValue> Values<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            Argument.EnsureNotNull(source, "source");
            foreach (var kvp in source)
                yield return kvp.Value;
        }
    }
}

namespace DesktopSprites.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Provides extra LINQ style extension methods.
    /// </summary>
    public static class Linq
    {
        /// <summary>
        /// Returns the only element of a sequence, or a default value if the sequence is empty or contains more than one item.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> to return the only element of.</param>
        /// <returns>The single element of the input sequence, or default(TSource) if the sequence contains no elements or more than one
        /// element.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        public static TSource OnlyOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            Argument.EnsureNotNull(source, nameof(source));
            TSource result;
            using (var enumerator = source.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return default(TSource);
                result = enumerator.Current;
                if (enumerator.MoveNext())
                    result = default(TSource);
            }
            return result;
        }

        /// <summary>
        /// Returns the only element of a sequence that satisfies a specified condition or a default value if no such element exists or
        /// more than one element satisfies the condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> to return the only element of.</param>
        /// <param name="predicate">A function to test an element for a condition.</param>
        /// <returns>The single element of the input sequence that satisfies the condition, or default(TSource) if no such element is found
        /// or more than one such element is found.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="predicate"/> is null.
        /// </exception>
        public static TSource OnlyOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            Argument.EnsureNotNull(source, nameof(source));
            Argument.EnsureNotNull(predicate, nameof(predicate));
            var count = 0;
            var result = default(TSource);
            foreach (var element in source)
                if (predicate(element))
                {
                    if (++count >= 2)
                        return default(TSource);
                    result = element;
                }
            return result;
        }

        /// <summary>
        /// Returns an element uniformly selected at random from all elements of a non-empty sequence.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1"/> to return a random element from.</param>
        /// <returns>A random element of the sequence.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="source"/> contains no elements.</exception>
        public static TSource RandomElement<TSource>(this IEnumerable<TSource> source)
        {
            Argument.EnsureNotNull(source, nameof(source));
            if (source is ICollection<TSource> collection)
            {
                if (collection.Count == 0)
                    throw new ArgumentException("source must contain at least one element.", nameof(source));
                return collection.ElementAt(Rng.Next(collection.Count));
            }
            var array = source.ToArray();
            if (array.Length == 0)
                throw new ArgumentException("source must contain at least one element.", nameof(source));
            return array[Rng.Next(array.Length)];
        }
    }
}

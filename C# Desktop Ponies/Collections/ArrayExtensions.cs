namespace CSDesktopPonies.Collections
{
    using System;

    /// <summary>
    /// Provides extension methods for arrays that access the static methods provided by <see cref="T:System.Array"/>.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Searches for the specified object and returns the index of the first occurrence within the entire array.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <param name="array">The one-dimensional, zero-based array to search.</param>
        /// <param name="value">The object to locate in array.</param>
        /// <returns>The zero-based index of the first occurrence of value within the entire array, if found; otherwise, –1.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        public static int IndexOf<T>(this T[] array, T value)
        {
            return Array.IndexOf(array, value);
        }
    }
}

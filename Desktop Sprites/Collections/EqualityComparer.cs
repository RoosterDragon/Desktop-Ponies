namespace DesktopSprites.Collections
{
    using System;
    using System.Collections.Generic;
    using DesktopSprites.Core;

    /// <summary>
    /// Defines methods to support the comparison of objects for equality.
    /// </summary>
    public static class EqualityComparer
    {
        /// <summary>
        /// Creates an equality comparer by using the specified equality and hash functions.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="equals">A function that determines whether the specified objects are equal.</param>
        /// <param name="getHashCode">A function that returns a hash code for the specified object.</param>
        /// <returns>The new equality comparer.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="equals"/> is null.-or-<paramref name="getHashCode"/> is null.
        /// </exception>
        public static EqualityComparer<T> Create<T>(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            return new EqualityComparerFromDelegates<T>(equals, getHashCode);
        }
        /// <summary>
        /// Exposes some equality delegates through the <see cref="T:System.Collections.Generic.IEqualityComparer"/> interface.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        private class EqualityComparerFromDelegates<T> : EqualityComparer<T>
        {
            /// <summary>
            /// The equality function.
            /// </summary>
            private readonly Func<T, T, bool> equals;
            /// <summary>
            /// The hashing function.
            /// </summary>
            private readonly Func<T, int> getHashCode;
            /// <summary>
            /// Initializes a new instance of the
            /// <see cref="T:DesktopSprites.Collections.EqualityComparer.EqualityComparerFromDelegates`1"/> class.
            /// </summary>
            /// <param name="equals">The function to use when determining whether the specified objects are equal.</param>
            /// <param name="getHashCode">The function that returns a hash code for the specified object.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="equals"/> is null.-or-<paramref name="getHashCode"/> is null.
            /// </exception>
            public EqualityComparerFromDelegates(Func<T, T, bool> equals, Func<T, int> getHashCode)
            {
                this.equals = Argument.EnsureNotNull(equals, "equals");
                this.getHashCode = Argument.EnsureNotNull(getHashCode, "getHashCode");
            }
            /// <summary>
            /// Determines whether the specified objects are equal.
            /// </summary>
            /// <param name="x">The first object of type <typeparamref name="T"/> to compare.</param>
            /// <param name="y">The second object of type <typeparamref name="T"/> to compare.</param>
            /// <returns>Returns true if the specified objects are equal; otherwise, false.</returns>
            public override bool Equals(T x, T y)
            {
                return equals(x, y);
            }
            /// <summary>
            /// Returns a hash code for the specified object.
            /// </summary>
            /// <param name="obj">The object for which a hash code is to be returned.</param>
            /// <returns>A hash code for the specified object.</returns>
            /// <exception cref="T:System.ArgumentException">The type of <paramref name="obj"/> is a reference type and
            /// <paramref name="obj"/> is null.</exception>
            public override int GetHashCode(T obj)
            {
                return getHashCode(obj);
            }
        }
    }
}

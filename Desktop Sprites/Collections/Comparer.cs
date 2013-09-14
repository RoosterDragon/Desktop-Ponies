namespace DesktopSprites.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a method that a type implements to compare two objects.
    /// </summary>
    public static class Comparer
    {
        /// <summary>
        /// Creates a comparer by using the specified comparison.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        /// <param name="comparison">The comparison to use.</param>
        /// <returns>The new comparer.</returns>
        public static Comparer<T> Create<T>(Comparison<T> comparison)
        {
            return new ComparerFromComparison<T>(comparison);
        }
        /// <summary>
        /// Exposes a <see cref="T:System.Comparison"/> through the <see cref="T:System.Collections.Generic.IComparer"/> interface.
        /// </summary>
        /// <typeparam name="T">The type of objects to compare.</typeparam>
        private class ComparerFromComparison<T> : Comparer<T>
        {
            /// <summary>
            /// The comparison function.
            /// </summary>
            private readonly Comparison<T> comparison;
            /// <summary>
            /// Initializes a new instance of the <see cref="T:DesktopSprites.Collections.Comparer.ComparerFromComparison`1"/> class.
            /// </summary>
            /// <param name="comparison">The comparison function to invoke when comparing objects.</param>
            public ComparerFromComparison(Comparison<T> comparison)
            {
                this.comparison = comparison;
            }
            /// <summary>
            /// Performs a comparison of two objects of the same type and returns a value indicating whether one object is less than, equal
            /// to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>A signed integer that indicates the relative values of x and y.</returns>
            public override int Compare(T x, T y)
            {
                return comparison(x, y);
            }
        }
    }
}

namespace CSDesktopPonies.Collections
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
        private class ComparerFromComparison<T> : Comparer<T>
        {
            private readonly Comparison<T> comparison;
            public ComparerFromComparison(Comparison<T> comparison)
            {
                this.comparison = comparison;
            }
            public override int Compare(T x, T y)
            {
                return comparison(x, y);
            }
        }
    }
}

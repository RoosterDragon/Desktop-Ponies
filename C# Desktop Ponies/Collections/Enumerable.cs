namespace CSDesktopPonies.Collections
{
    using System.Collections.Generic;

    /// <summary>
    /// Creates a wrapper around an <see cref="T:System.Collections.Generic.IEnumerator`1"/> to expose it through the
    /// <see cref="T:System.Collections.Generic.IEnumerable`1"/> interface.
    /// </summary>
    public static class Enumerable
    {
        /// <summary>
        /// Exposes the specified <see cref="T:System.Collections.Generic.IEnumerator`1"/> through the
        /// <see cref="T:System.Collections.Generic.IEnumerable`1"/> interface.
        /// </summary>
        /// <typeparam name="T">The type of elements to enumerate.</typeparam>
        /// <param name="enumerator">An <see cref="T:System.Collections.Generic.IEnumerator`1"/> that enumerates the elements of a generic
        /// collection.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1"/> which returns the specified
        /// <see cref="T:System.Collections.Generic.IEnumerator`1"/> when its
        /// <see cref="M:System.Collections.Generic.IEnumerable`1.GetEnumerator()"/> method is called.</returns>
        public static IEnumerable<T> For<T>(IEnumerator<T> enumerator)
        {
            return new EnumeratorWrapper<T>(enumerator);
        }

        /// <summary>
        /// Exposes an enumerator for elements of a generic collection.
        /// </summary>
        /// <typeparam name="T">The type of elements to enumerate.</typeparam>
        private struct EnumeratorWrapper<T> : IEnumerable<T>
        {
            /// <summary>
            /// The enumerator to return.
            /// </summary>
            private IEnumerator<T> enumerator;
            /// <summary>
            /// Initializes a new instance of the <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/> struct from
            /// the given enumerator.
            /// </summary>
            /// <param name="enumerator">An <see cref="T:System.Collections.Generic.IEnumerator`1"/> that enumerates the elements of a
            /// generic collection.</param>
            public EnumeratorWrapper(IEnumerator<T> enumerator)
            {
                this.enumerator = enumerator;
            }
            /// <summary>
            /// Returns an enumerator that iterates through the generic collection.
            /// </summary>
            /// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1"/> for the generic collection.</returns>
            public IEnumerator<T> GetEnumerator()
            {
                return enumerator;
            }
            /// <summary>
            /// Returns an enumerator that iterates through the generic collection.
            /// </summary>
            /// <returns>An <see cref="T:System.Collections.Generic.IEnumerator`1"/> for the generic collection.</returns>
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            /// <summary>
            /// Tests whether two specified <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/> structures are
            /// equivalent.
            /// </summary>
            /// <param name="left">The <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/> that is to the left of
            /// the equality operator.
            /// </param>
            /// <param name="right">The <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/> that is to the right of
            /// the equality operator.
            /// </param>
            /// <returns>Returns true if the two <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/> structures are
            /// equal; otherwise, false.</returns>
            public static bool operator ==(EnumeratorWrapper<T> left, EnumeratorWrapper<T> right)
            {
                return left.enumerator == right.enumerator;
            }
            /// <summary>
            /// Tests whether two specified <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/> structures are
            /// different.
            /// </summary>
            /// <param name="left">The <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/> that is to the left of
            /// the inequality operator.
            /// </param>
            /// <param name="right">The <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/> that is to the right of
            /// the inequality operator.</param>
            /// <returns>Returns true if the two <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/> structures are
            /// different; otherwise, false.</returns>
            public static bool operator !=(EnumeratorWrapper<T> left, EnumeratorWrapper<T> right)
            {
                return !(left == right);
            }
            /// <summary>
            /// Tests whether the specified object is a <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/>
            /// structure and is equivalent to this <see cref="T:CSDesktopPonies.Collections.Enumerable`1"/> structure.
            /// </summary>
            /// <param name="obj">The object to test.</param>
            /// <returns>Returns true if <paramref name="obj"/> is a
            /// <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/> structure equivalent to this
            /// <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/> structure; otherwise, false.
            /// </returns>
            public override bool Equals(object obj)
            {
                if (obj == null || !(obj is EnumeratorWrapper<T>))
                    return false;

                return this == (EnumeratorWrapper<T>)obj;
            }
            /// <summary>
            /// Returns a hash code for this <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/> structure.
            /// </summary>
            /// <returns>An integer value that specifies the hash code for this
            /// <see cref="T:CSDesktopPonies.Collections.Enumerable.EnumeratorWrapper`1"/>.
            /// </returns>
            public override int GetHashCode()
            {
                return int.MaxValue ^ enumerator.GetHashCode();
            }
        }
    }
}

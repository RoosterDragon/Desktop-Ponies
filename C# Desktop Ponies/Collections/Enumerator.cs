namespace CsDesktopPonies.Collections
{
    using System.Collections.Generic;

    /// <summary>
    /// Enumerates the elements of a generic collection.
    /// </summary>
    /// <typeparam name="T">The type of elements to enumerate.</typeparam>
    public struct Enumerator<T> : IEnumerable<T>
    {
        /// <summary>
        /// The enumerator to return.
        /// </summary>
        private IEnumerator<T> enumerator;
        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> struct from the given enumerator.
        /// </summary>
        /// <param name="enumerator">An <see cref="T:System.Collections.Generic.IEnumerator`1"/> that enumerates the elements of a generic
        /// collection.</param>
        public Enumerator(IEnumerator<T> enumerator)
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
        /// Tests whether two specified <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> structures are equivalent.
        /// </summary>
        /// <param name="left">The <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> that is to the left of the equality operator.
        /// </param>
        /// <param name="right">The <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> that is to the right of the equality operator.
        /// </param>
        /// <returns>Returns true if the two <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> structures are equal; otherwise,
        /// false.</returns>
        public static bool operator ==(Enumerator<T> left, Enumerator<T> right)
        {
            return left.enumerator == right.enumerator;
        }
        /// <summary>
        /// Tests whether two specified <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> structures are different.
        /// </summary>
        /// <param name="left">The <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> that is to the left of the inequality operator.
        /// </param>
        /// <param name="right">The <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> that is to the right of the inequality
        /// operator.</param>
        /// <returns>Returns true if the two <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> structures are different; otherwise,
        /// false.</returns>
        public static bool operator !=(Enumerator<T> left, Enumerator<T> right)
        {
            return !(left == right);
        }
        /// <summary>
        /// Tests whether the specified object is a <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> structure and is equivalent
        /// to this <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> structure.
        /// </summary>
        /// <param name="obj">The object to test.</param>
        /// <returns>Returns true if <paramref name="obj"/> is a <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> structure
        /// equivalent to this <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> structure; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Enumerator<T>))
                return false;

            return this == (Enumerator<T>)obj;
        }
        /// <summary>
        /// Returns a hash code for this <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/> structure.
        /// </summary>
        /// <returns>An integer value that specifies the hash code for this <see cref="T:CsDesktopPonies.Collections.Enumerator`1"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return enumerator.GetHashCode();
        }
    }
}

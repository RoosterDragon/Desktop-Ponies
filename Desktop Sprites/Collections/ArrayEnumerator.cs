namespace DesktopSprites.Collections
{
    using System.Collections.Generic;
    using DesktopSprites.Core;

    /// <summary>
    /// Supports iteration over an array.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the array.</typeparam>
    public struct ArrayEnumerator<T> : IEnumerator<T>
    {
        /// <summary>
        /// The array being enumerated.
        /// </summary>
        private readonly T[] array;
        /// <summary>
        /// The current index into the array.
        /// </summary>
        private int index;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.Collections.ArrayEnumerator`1"/> structure.
        /// </summary>
        /// <param name="array">The array to enumerate.</param>
        public ArrayEnumerator(T[] array)
        {
            this.array = Argument.EnsureNotNull(array, "array");
            index = -1;
        }
        /// <summary>
        /// Advances the enumerator to the next element of the array.
        /// </summary>
        /// <returns>Return true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the
        /// end of the collection.</returns>
        public bool MoveNext()
        {
            return ++index < array.Length;
        }
        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        public T Current
        {
            get { return array[index]; }
        }
        /// <summary>
        /// Releases all resources allocated by the object.
        /// </summary>
        public void Dispose()
        {
        }
        /// <summary>
        /// Gets the current element in the collection.
        /// </summary>
        object System.Collections.IEnumerator.Current
        {
            get { return Current; }
        }
        /// <summary>
        /// Sets the enumerator to its initial position, which is before the first element in the collection.
        /// </summary>
        void System.Collections.IEnumerator.Reset()
        {
            index = -1;
        }
    }
}

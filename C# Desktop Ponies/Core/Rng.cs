namespace CSDesktopPonies.Core
{
    using System;

    /// <summary>
    /// Provides shared, thread-safe access to an instance of <see cref="T:System.Random"/>.
    /// </summary>
    public static class Rng
    {
        /// <summary>
        /// Random Number Generator.
        /// </summary>
        private static readonly Random Generator = new Random();

        /// <summary>
        /// Returns a nonnegative random number.
        /// </summary>
        /// <returns>A 32-bit signed integer greater than or equal to zero and less than <see cref="P:System.Int32.MaxValue"/>.</returns>
        public static int Next()
        {
            lock (Generator)
                return Generator.Next();
        }
        /// <summary>
        /// Returns a nonnegative random number less than the specified maximum.
        /// </summary>
        /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be
        /// greater than or equal to zero.</param>
        /// <returns>A 32-bit signed integer greater than or equal to zero, and less than <paramref name="maxValue"/>; that is, the range
        /// of return values ordinarily includes zero but not <paramref name="maxValue"/>. However, if <paramref name="maxValue"/> equals
        /// zero, <paramref name="maxValue"/> is returned.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="maxValue"/> is less than zero.</exception>
        public static int Next(int maxValue)
        {
            lock (Generator)
                return Generator.Next(maxValue);
        }
        /// <summary>
        /// Returns a random number within a specified range.
        /// </summary>
        /// <param name="minValue">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxValue">The exclusive upper bound of the random number returned. <paramref name="maxValue"/> must be greater
        /// than or equal to <paramref name="minValue"/>.</param>
        /// <returns>A 32-bit signed integer greater than or equal to <paramref name="minValue"/> and less than
        /// <paramref name="maxValue"/>; that is, the range of return values includes minValue but not <paramref name="maxValue"/>. If
        /// <paramref name="minValue"/> equals <paramref name="maxValue"/>, <paramref name="minValue"/> is returned.</returns>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="minValue"/> is greater than <paramref name="maxValue"/>.
        /// </exception>
        public static int Next(int minValue, int maxValue)
        {
            lock (Generator)
                return Generator.Next(minValue, maxValue);
        }
        /// <summary>
        /// Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="buffer"/> is null.</exception>
        public static void NextBytes(byte[] buffer)
        {
            lock (Generator)
                Generator.NextBytes(buffer);
        }
        /// <summary>
        /// Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
        public static double NextDouble()
        {
            lock (Generator)
                return Generator.NextDouble();
        }
    }
}

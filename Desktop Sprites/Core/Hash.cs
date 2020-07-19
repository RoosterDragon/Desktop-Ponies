namespace DesktopSprites.Core
{
    /// <summary>
    /// Provides hash generation functions.
    /// </summary>
    public static class Hash
    {
        /// <summary>
        /// Gets the 32-bit FNV-1a hash code for a sequence of bytes.
        /// </summary>
        /// <param name="input">The sequence of bytes which should be hashed.</param>
        /// <returns>A 32-bit integer that is the hash code for the input bytes.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="input"/> is null.</exception>
        public static int Fnv1A32(byte[] input)
        {
            const int Fnv1AOffsetBasis32 = unchecked((int)2166136261);
            return Fnv1A32(input, Fnv1AOffsetBasis32);
        }
        /// <summary>
        /// Gets the 32-bit FNV-1a hash code for a sequence of bytes.
        /// </summary>
        /// <param name="input">The sequence of bytes which should be hashed.</param>
        /// <param name="hash">The initial hash value to be hashed further.</param>
        /// <returns>A 32-bit integer that is the hash code for the input bytes.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="input"/> is null.</exception>
        public static int Fnv1A32(byte[] input, int hash)
        {
            Argument.EnsureNotNull(input, nameof(input));
            const int FnvPrime32 = 16777619;
            foreach (var octet in input)
            {
                hash ^= octet;
                hash *= FnvPrime32;
            }
            return hash;
        }
    }
}

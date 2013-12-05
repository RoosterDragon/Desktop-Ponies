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
        public static int Fnv1A32(byte[] input)
        {
            const int OffsetBasis32 = unchecked((int)2166136261);
            return Fnv1A32Continue(input, OffsetBasis32);
        }
        /// <summary>
        /// Gets the 32-bit FNV-1a hash code for a sequence of bytes, starting from a hash value generated from a previous sequence.
        /// </summary>
        /// <param name="input">The sequence of bytes which should be hashed.</param>
        /// <param name="hash">A 32-bit FNV-1a hash which should be hashed further.</param>
        /// <returns>A 32-bit integer that is the hash code for the input bytes, plus those of the previous sequences.</returns>
        public static int Fnv1A32Continue(byte[] input, int hash)
        {
            const int FnvPrime32 = 16777619;
            foreach (byte octet in input)
            {
                hash ^= octet;
                hash *= FnvPrime32;
            }
            return hash;
        }
    }
}

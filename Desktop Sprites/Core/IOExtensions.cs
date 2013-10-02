namespace DesktopSprites.Core
{
    using System.IO;

    /// <summary>
    /// Defines extension methods to members of the System.IO namespace.
    /// </summary>
    public static class IOExtensions
    {
        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values
        /// between offset and (offset + count - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in buffer at which to begin storing the data read from the current stream.
        /// </param>
        /// <param name="count">The number of bytes to be read from the current stream.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="stream"/> is null.-or-<paramref name="buffer"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">The sum of offset and count is larger than the buffer length.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">offset or count is negative.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.-or-count was greater than the number of bytes available.
        /// </exception>
        /// <exception cref="T:System.NotSupportedException">The stream does not support reading.</exception>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
        public static void ReadExact(this Stream stream, byte[] buffer, int offset, int count)
        {
            if (Argument.EnsureNotNull(stream, "stream").Read(buffer, offset, count) != count)
                throw InsufficientDataAvailableException();
        }
        /// <summary>
        /// Reads the specified number of bytes from the stream, starting from a specified point in the byte array.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        /// <param name="buffer">The buffer to read data into.</param>
        /// <param name="index">The starting point in the buffer at which to begin reading into the buffer.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="reader"/> is null.-or-<paramref name="buffer"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">The buffer length minus index is less than count.-or-The number of decoded
        /// characters to read is greater than count. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index or count is negative.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.-or-count was greater than the number of bytes available.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
        public static void ReadExact(this BinaryReader reader, byte[] buffer, int index, int count)
        {
            if (Argument.EnsureNotNull(reader, "reader").Read(buffer, index, count) != count)
                throw InsufficientDataAvailableException();
        }
        /// <summary>
        /// Reads the specified number of characters from the stream, starting from a specified point in the character array.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        /// <param name="buffer">The buffer to read data into.</param>
        /// <param name="index">The starting point in the buffer at which to begin reading into the buffer.</param>
        /// <param name="count">The number of characters to read.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="reader"/> is null.-or-<paramref name="buffer"/> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">The buffer length minus index is less than count.-or-The number of decoded
        /// characters to read is greater than count. This can happen if a Unicode decoder returns fallback characters or a surrogate pair.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">index or count is negative.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.-or-count was greater than the number of characters available.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
        public static void ReadExact(this BinaryReader reader, char[] buffer, int index, int count)
        {
            if (Argument.EnsureNotNull(reader, "reader").Read(buffer, index, count) != count)
                throw InsufficientDataAvailableException();
        }
        /// <summary>
        /// Reads the specified number of bytes from the current stream into a byte array and advances the current position by that number
        /// of bytes.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>A byte array containing data read from the underlying stream.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="reader"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">The number of decoded characters to read is greater than count. This can happen if
        /// a Unicode decoder returns fallback characters or a surrogate pair.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.-or-count was greater than the number of bytes available.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">count is negative.</exception>
        public static byte[] ReadBytesExact(this BinaryReader reader, int count)
        {
            byte[] result = Argument.EnsureNotNull(reader, "reader").ReadBytes(count);
            if (result.Length < count)
                throw InsufficientDataAvailableException();
            return result;
        }
        /// <summary>
        /// Reads the specified number of characters from the current stream, returns the data in a character array, and advances the
        /// current position in accordance with the Encoding used and the specific character being read from the stream.
        /// </summary>
        /// <param name="reader">The stream to read from.</param>
        /// <param name="count">The number of characters to read.</param>
        /// <returns>A character array containing data read from the underlying stream.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="reader"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">The number of decoded characters to read is greater than count. This can happen if
        /// a Unicode decoder returns fallback characters or a surrogate pair.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.-or-count was greater than the number of bytes available.
        /// </exception>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">count is negative.</exception>
        public static char[] ReadCharsExact(this BinaryReader reader, int count)
        {
            char[] result = Argument.EnsureNotNull(reader, "reader").ReadChars(count);
            if (result.Length < count)
                throw InsufficientDataAvailableException();
            return result;
        }
        /// <summary>
        /// Creates an exception that represents the error were more data was requested to be read into a buffer than was available.
        /// </summary>
        /// <returns>A new <see cref="T:System.IO.IOException"/>.</returns>
        private static IOException InsufficientDataAvailableException()
        {
            return new IOException("Attempted to read more data into a buffer than were available.");
        }
    }
}

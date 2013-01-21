namespace CSDesktopPonies
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Provides methods to get the size of an image without having to load the whole file.
    /// </summary>
    /// <remarks>
    /// Modified from http://stackoverflow.com/questions/111345/getting-image-dimensions-without-reading-the-entire-file by user ICR
    /// http://stackoverflow.com/users/214/icr.
    /// Modified DecodeJfif method from http://www.codeproject.com/Articles/35978/Reading-Image-Headers-to-Get-Width-and-Height by user
    /// andywilsonuk http://www.codeproject.com/script/Membership/View.aspx?mid=1231652.
    /// </remarks>
    public static class ImageSize
    {
        /// <summary>
        /// Standard error message.
        /// </summary>
        private const string ErrorMessage = "Could not recognize image format.";

        /// <summary>
        /// Contains a collection of known image formats based on the magic bytes in the header of those formats. Each sequence of magic
        /// bytes maps to a decoding function which reads the image and returns its size.
        /// </summary>
        private static readonly Dictionary<byte[], Func<BinaryReader, Size>> ImageDecoders =
            new Dictionary<byte[], Func<BinaryReader, Size>>
        {
            { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }, DecodePng },
            { new byte[] { 0xFF, 0xD8 }, DecodeJfif },
            { new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, DecodeGif },
            { new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, DecodeGif },
            { new byte[] { 0x42, 0x4D }, DecodeBitmap },
        };

        /// <summary>
        /// The length of the longest sequence of magic bytes in the collection of image decoders.
        /// </summary>
        private static readonly int MaxMagicBytesLength = ImageDecoders.Keys.Max(magicBytes => magicBytes.Length);

        /// <summary>
        /// Gets the width and height of an image, in pixels.
        /// </summary>
        /// <param name="path">The path to an image file of PNG, JPEG, GIF or BMP format whose size is to be found.</param>
        /// <returns>The width and height of the image, in pixels.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="path"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">The image was of an unrecognized format.</exception>
        public static Size GetSize(string path)
        {
            using (FileStream stream =
                new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 32, FileOptions.SequentialScan))
            using (BinaryReader reader = new BinaryReader(stream))
            {
                try
                {
                    return GetSize(reader);
                }
                catch (ArgumentException ex)
                {
                    if (ex.Message.StartsWith(ErrorMessage, StringComparison.Ordinal))
                        throw new ArgumentException(ErrorMessage, "path", ex);
                    else
                        throw;
                }
            }
        }

        /// <summary>
        /// Gets the width and height of an image, in pixels.
        /// </summary>
        /// <param name="reader">A <see cref="T:System.IO.BinaryReader"/> that is positioned to read an image stream of PNG, JPEG,
        /// GIF or BMP format.</param>
        /// <returns>The width and height of the image, in pixels.</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="reader"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException">The image was of an unrecognized format.</exception>    
        public static Size GetSize(BinaryReader reader)
        {
            Argument.EnsureNotNull(reader, "reader");
            
            byte[] magicBytes = new byte[MaxMagicBytesLength];

            for (int i = 0; i < magicBytes.Length; i++)
            {
                magicBytes[i] = reader.ReadByte();
                foreach (var imageFormatDecoder in ImageDecoders.Where(decoder => decoder.Key.Length == i + 1))
                    if (magicBytes.StartsWith(imageFormatDecoder.Key))
                        return imageFormatDecoder.Value(reader);
            }

            throw new ArgumentException(ErrorMessage, "reader");
        }

        /// <summary>
        /// Determines if this array starts with the same values as the given array.
        /// </summary>
        /// <param name="source">The source array which should be checked.</param>
        /// <param name="theseBytes">The array of bytes which should be checked against the start of the array.</param>
        /// <returns>Returns true if the source array is at least as long as the array to be checked, and all byte values in the array to
        /// be checked match those in the source array; otherwise false.</returns>
        private static bool StartsWith(this byte[] source, byte[] theseBytes)
        {
            if (source.Length < theseBytes.Length)
                return false;

            for (int i = 0; i < theseBytes.Length; i++)
                if (source[i] != theseBytes[i])
                    return false;

            return true;
        }

        /// <summary>
        /// Reads a little-endian 2-byte signed integer from the current stream and advances the current position of the stream by two
        /// bytes.
        /// </summary>
        /// <param name="binaryReader">The stream from which to read.</param>
        /// <returns>A 2-byte signed integer read from the current stream.</returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        private static short ReadLittleEndianInt16(this BinaryReader binaryReader)
        {
            byte[] bytes = new byte[sizeof(short)];
            for (int i = 0; i < bytes.Length; i++)
                bytes[bytes.Length - 1 - i] = binaryReader.ReadByte();
            return BitConverter.ToInt16(bytes, 0);
        }

        /// <summary>
        /// Reads a little-endian 4-byte signed integer from the current stream and advances the current position of the stream by four
        /// bytes.
        /// </summary>
        /// <param name="binaryReader">The stream from which to read.</param>
        /// <returns>A 4-byte signed integer read from the current stream.</returns>
        /// <exception cref="T:System.IO.EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The stream is closed.</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs.</exception>
        private static int ReadLittleEndianInt32(this BinaryReader binaryReader)
        {
            byte[] bytes = new byte[sizeof(int)];
            for (int i = 0; i < bytes.Length; i++)
                bytes[bytes.Length - 1 - i] = binaryReader.ReadByte();
            return BitConverter.ToInt32(bytes, 0);
        }

        /// <summary>
        /// Gets the image dimensions of a bitmap image from the given stream.
        /// </summary>
        /// <param name="binaryReader">The stream from which to read, which must already have advanced past the header.</param>
        /// <returns>The image dimensions for this bitmap image.</returns>
        private static Size DecodeBitmap(BinaryReader binaryReader)
        {
            binaryReader.ReadBytes(16);
            return new Size(binaryReader.ReadInt32(), binaryReader.ReadInt32());
        }

        /// <summary>
        /// Gets the image dimensions of a Graphics Interchange Format image from the given stream.
        /// </summary>
        /// <param name="binaryReader">The stream from which to read, which must already have advanced past the header.</param>
        /// <returns>The image dimensions for this GIF image.</returns>
        private static Size DecodeGif(BinaryReader binaryReader)
        {
            return new Size(binaryReader.ReadInt16(), binaryReader.ReadInt16());
        }

        /// <summary>
        /// Gets the image dimensions of a Portable Network Graphics image from the given stream.
        /// </summary>
        /// <param name="binaryReader">The stream from which to read, which must already have advanced past the header.</param>
        /// <returns>The image dimensions for this PNG image.</returns>
        private static Size DecodePng(BinaryReader binaryReader)
        {
            binaryReader.ReadBytes(8);
            return new Size(binaryReader.ReadLittleEndianInt32(), binaryReader.ReadLittleEndianInt32());
        }

        /// <summary>
        /// Gets the image dimensions of a JPEG File Interchange Format image from the given stream.
        /// </summary>
        /// <param name="binaryReader">The stream from which to read, which must already have advanced past the header.</param>
        /// <returns>The image dimensions for this JPEG image.</returns>
        /// <exception cref="T:System.ArgumentException">The image was of an unrecognized format.</exception>
        private static Size DecodeJfif(BinaryReader binaryReader)
        {
            while (binaryReader.ReadByte() == 0xFF)
            {
                byte marker = binaryReader.ReadByte();
                short chunkLength = binaryReader.ReadLittleEndianInt16();
                if (marker == 0xC0)
                {
                    binaryReader.ReadByte();
                    return new Size(binaryReader.ReadLittleEndianInt16(), binaryReader.ReadLittleEndianInt16());
                }

                if (chunkLength < 0)
                {
                    ushort uchunkLength = (ushort)chunkLength;
                    binaryReader.ReadBytes(uchunkLength - 2);
                }
                else
                {
                    binaryReader.ReadBytes(chunkLength - 2);
                }
            }

            throw new ArgumentException(ErrorMessage);
        }
    }
}
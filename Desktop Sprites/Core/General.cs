namespace DesktopSprites.Core
{
    using System;
    using System.Runtime;

    /// <summary>
    /// General utility methods.
    /// </summary>
    public static class General
    {
#if DEBUG
        /// <summary>
        /// Guards access to Buffer.
        /// </summary>
        private static readonly object BufferGuard = new object();
        /// <summary>
        /// Provides a reusable character buffer of sufficient size to write simply formatted signed 64-bit integers.
        /// </summary>
        private static readonly char[] Buffer = new char[(int)Math.Ceiling(Math.Log10(-(double)long.MinValue)) + 1];
#endif

        /// <summary>
        /// Performs a full cleanup of unused memory (full garbage collection, empty the finalization queue, another full garbage
        /// collection that compacts LOH).
        /// </summary>
        /// <remarks>
        /// <para>The first garbage collection identifies any objects which are no longer being referenced. For objects without a
        /// finalizer (i.e. most managed objects) their memory is reclaimed immediately. Objects implementing a finalizer on which
        /// finalization has not been suppressed must be finalized. Their memory therefore cannot be reclaimed immediately and these
        /// objects are kept alive and put into a queue for finalization. Any managed memory referenced by objects in the queue is
        /// therefore also kept alive.</para>
        /// <para>The finalization queue is now forcibly emptied by running all pending finalizers. These objects no longer need to be kept
        /// alive and their memory can be reclaimed. Therefore a second collection is run to reclaim the remaining unreferenced memory.
        /// </para>
        /// <para>This method should be called only when absolutely necessary or efficient GC operation will be disrupted as objects will
        /// be promoted into older generations earlier. If these are short-lived objects this means they will survive much longer than
        /// otherwise. It is best to call this method when you know a substantial amount of long-lived objects are no longer referenced and
        /// can be reclaimed and at a time when there are minimal short-lived objects with live references. When called in this manner it
        /// allows memory to be reclaimed earlier and more deterministically than waiting for the next natural GC cycle, but without
        /// promoting objects needlessly.</para>
        /// </remarks>
        public static void FullCollect()
        {
#if DEBUG
            // As we are outputting to console, we shall lock around the whole collection so as to minimize the interference from
            // multi-threaded calls, however other threads are still free to call for collections outside of this method, so this cannot be
            // guaranteed.
            lock (BufferGuard)
            {
                long beforeCollect = GC.GetTotalMemory(false);
                GC.Collect();
                long beforeFinalize = GC.GetTotalMemory(false);
                GC.WaitForPendingFinalizers();
                GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                GC.Collect();
                long afterCollect = GC.GetTotalMemory(false);

                int beforeCollectDigits = (int)Math.Ceiling(Math.Log10(beforeCollect));
                int beforeFinalizeDigits = (int)Math.Ceiling(Math.Log10(beforeFinalize));
                int afterCollectDigits = (int)Math.Ceiling(Math.Log10(afterCollect));
                int maxDigits = Math.Max(beforeCollectDigits, Math.Max(beforeFinalizeDigits, afterCollectDigits));

                Console.Write("FullCollect:  Before first collection: ");
                ConsoleWriteLineLong(beforeCollect, maxDigits);
                Console.Write("FullCollect:      Before finalization: ");
                ConsoleWriteLineLong(beforeFinalize, maxDigits);
                Console.Write("FullCollect: After finalize & collect: ");
                ConsoleWriteLineLong(afterCollect, maxDigits);
            }
#else
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
            GC.Collect();
#endif
        }

#if DEBUG
        /// <summary>
        /// Writes the text representation of the specified 64-bit signed integer value, followed by the current line terminator, to the
        /// standard output stream. No allocations are made.
        /// </summary>
        /// <param name="value">The value to write.</param>
        /// <param name="minChars">The minimum number of characters to output (padding will be used as needed).</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="minChars"/> is less than zero or greater than then
        /// maximum number of decimal digits required by a 64-bit signed integer, plus one (for the sign).</exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurred.</exception>
        private static void ConsoleWriteLineLong(long value, int minChars = 0)
        {
            Argument.EnsureInRangeInclusive(minChars, "minChars", 0, Buffer.Length);

            // Absolute value of value parameter.
            ulong v = value >= 0 ? (ulong)value : value != long.MinValue ? (ulong)-value : (ulong)long.MaxValue + 1;
            int bufferIndex = Buffer.Length - 1;
            do
            {
                ulong original = v;
                v /= 10;
                ulong digit = original - (v * 10);
                Buffer[bufferIndex--] = (char)('0' + digit);
            }
            while (v > 0);
            if (value < 0)
                Buffer[bufferIndex--] = '-';
            while (minChars > Buffer.Length - 1 - bufferIndex && bufferIndex >= 0)
                Buffer[bufferIndex--] = ' ';
            bufferIndex++;
            Console.WriteLine(Buffer, bufferIndex, Buffer.Length - bufferIndex);
        }
#endif

        /// <summary>
        /// Gets the file version of the calling assembly.
        /// </summary>
        /// <returns>The file version of the calling assembly.</returns>
        /// <exception cref="T:System.NotSupportedException">The current assembly is a dynamic assembly, or was loaded from a byte array.
        /// </exception>
        public static Version GetAssemblyVersion()
        {
            var location = System.Reflection.Assembly.GetCallingAssembly().Location;
            if (location == "")
                throw new NotSupportedException("The calling assembly was loaded from a byte array.");
            var fileVersionInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(location);
            return new Version(
                fileVersionInfo.FileMajorPart, fileVersionInfo.FileMinorPart,
                fileVersionInfo.FileBuildPart, fileVersionInfo.FilePrivatePart);
        }
    }
}

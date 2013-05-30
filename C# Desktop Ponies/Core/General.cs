namespace CSDesktopPonies.Core
{
    using System;

    /// <summary>
    /// General utility methods.
    /// </summary>
    public static class General
    {
#if DEBUG
        /// <summary>
        /// Provides a reusable character buffer of sufficient size to write simply formatted signed 64-bit integers.
        /// </summary>
        private static readonly char[] Buffer = new char[(int)Math.Ceiling(Math.Log10(-(double)long.MinValue)) + 1];
#endif

        /// <summary>
        /// Performs a full cleanup of unused memory (full garbage collection, empty the finalization queue, another full garbage
        /// collection).
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
        /// <para>Since this method ultimately performs two full garbage collections (i.e. collections across all generations) and
        /// processes finalizers (an expensive operation), it is very expensive to call and should be done only when absolutely necessary
        /// (e.g. when a long running process releases a substantial amount of memory that is likely to have survived into the oldest
        /// generation. If your process is not allocating much new memory it may be a substantial amount of time before the runtime decides
        /// to perform a full collection on its own accord.) Calling this method needlessly will disrupt the ability of the runtime to
        /// schedule garbage collections effectively.</para>
        /// </remarks>
        public static void FullCollect()
        {
#if DEBUG
            // As we are outputting to console, we shall lock around the whole collection so as to minimize the interference from
            // multithreaded calls, however other threads are still free to call for collections outside of this method, so this cannot be
            // guaranteed.
            lock (Buffer)
            {
                long beforeCollect = GC.GetTotalMemory(false);
                GC.Collect();
                long beforeFinalize = GC.GetTotalMemory(false);
                GC.WaitForPendingFinalizers();
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
            if (minChars < 0 || minChars > Buffer.Length)
                throw new ArgumentOutOfRangeException("minChars", minChars,
                    "minChars must be greater than or equal to zero and less than or equal to " + Buffer.Length);
            int bufferIndex = Buffer.Length - 1;
            long shift = 1;
            long doubleShiftedValue;
            do
            {
                long doubleShift = shift * 10;
                long shiftedValue = (value / shift) * shift;
                doubleShiftedValue = (value / doubleShift) * doubleShift;
                int digitOut = (int)((shiftedValue - doubleShiftedValue) / shift);
                if (value < 0)
                {
                    digitOut *= -1;
                    doubleShiftedValue *= -1;
                }
                Buffer[bufferIndex--] = (char)((int)'0' + digitOut);
                shift *= 10;
            }
            while (doubleShiftedValue > 0);
            if (value < 0)
                Buffer[bufferIndex--] = '-';
            while (minChars > Buffer.Length - 1 - bufferIndex && bufferIndex >= 0)
                Buffer[bufferIndex--] = ' ';
            bufferIndex++;
            Console.WriteLine(Buffer, bufferIndex, Buffer.Length - bufferIndex);
        }
#endif
    }
}

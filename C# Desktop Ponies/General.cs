using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSDesktopPonies
{
    /// <summary>
    /// General utility methods.
    /// </summary>
    public static class General
    {
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
            long beforeCollect;
            long beforeFinalize;
            long afterCollect;
            beforeCollect = GC.GetTotalMemory(false);
#endif
            GC.Collect();
#if DEBUG
            beforeFinalize = GC.GetTotalMemory(false);
#endif
            GC.WaitForPendingFinalizers();
            GC.Collect();
#if DEBUG
            afterCollect = GC.GetTotalMemory(false);
            int beforeCollectDigits = (int)Math.Ceiling(Math.Log10(beforeCollect));
            int beforeFinalizeDigits = (int)Math.Ceiling(Math.Log10(beforeFinalize));
            int afterCollectDigits = (int)Math.Ceiling(Math.Log10(afterCollect));

            Console.Write("FullCollect:  Before first collection: ");
            Console.WriteLine(beforeCollect);

            Console.Write("FullCollect:      Before finalization: ");
            for (int i = beforeFinalizeDigits; i < beforeCollectDigits; i++)
                Console.Write(" ");
            Console.WriteLine(beforeFinalize);

            Console.Write("FullCollect: After finalize & collect: ");
            for (int i = afterCollectDigits; i < beforeCollectDigits; i++)
                Console.Write(" ");
            Console.WriteLine(afterCollect);
#endif
        }
    }
}

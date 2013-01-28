namespace CSDesktopPonies.Collections
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines Sort extension methods for <see cref="T:System.Collections.Generic.LinkedList`1"/>.
    /// </summary>
    public static partial class LinkedListExtensions
    {
        /// <summary>
        /// Sorts the elements in the entire <see cref="T:System.Collections.Generic.LinkedList`1"/> using the default comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A list to sort.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">The default comparer
        /// <see cref="P:System.Collections.Generic.Comparer`1.Default"/> cannot find an implementation of the
        /// <see cref="T:System.IComparable`1"/> generic interface or the <see cref="T:System.IComparable"/> interface for type
        /// <typeparamref name="TSource"/>.</exception>
        public static void Sort<TSource>(this LinkedList<TSource> source)
        {
            Argument.EnsureNotNull(source, "source");
            if (Comparer<TSource>.Default == null)
                throw new InvalidOperationException();

            Sort(source, Comparer<TSource>.Default);
        }

        /// <summary>
        /// Sorts the elements in the entire <see cref="T:System.Collections.Generic.LinkedList`1"/> using the specified
        /// <see cref="T:System.Comparison`1"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A list to sort.</param>
        /// <param name="comparison">The <see cref="T:System.Comparison`1"/> to use when comparing elements.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="comparison"/> is null.
        /// </exception>
        public static void Sort<TSource>(this LinkedList<TSource> source, Comparison<TSource> comparison)
        {
            Argument.EnsureNotNull(source, "source");
            Argument.EnsureNotNull(comparison, "comparison");

            MergeSort(source, comparison);
        }

        /// <summary>
        /// Sorts the elements in the entire <see cref="T:System.Collections.Generic.LinkedList`1"/> using the specified comparer.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A list to sort.</param>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1"/> implementation to use when comparing
        /// elements, or null to use the default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default"/>.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.</exception>
        /// <exception cref="T:System.InvalidOperationException"><paramref name="comparer"/> is null, and the default comparer
        /// <see cref="P:System.Collections.Generic.Comparer`1.Default"/> cannot find implementation of the
        /// <see cref="T:System.IComparable`1"/> generic interface or the <see cref="T:System.IComparable"/> interface for type
        /// <typeparamref name="TSource"/>.</exception>
        public static void Sort<TSource>(this LinkedList<TSource> source, IComparer<TSource> comparer)
        {
            Argument.EnsureNotNull(source, "source");
            if (comparer == null && Comparer<TSource>.Default == null)
                throw new InvalidOperationException();

            Comparison<TSource> comparison;
            if (comparer == null)
                comparison = Comparer<TSource>.Default.Compare;
            else
                comparison = comparer.Compare;

            MergeSort(source, comparison);
        }

        /// <summary>
        /// Performs a merge sort on the elements in the entire <see cref="T:System.Collections.Generic.LinkedList`1"/> using the specified
        /// <see cref="T:System.Comparison`1"/>.
        /// </summary>
        /// <typeparam name="T">The type of the elements of list.</typeparam>
        /// <param name="list">A list to sort.</param>
        /// <param name="comparison">The <see cref="T:System.Comparison`1"/> to use when comparing elements.</param>
        /// <remarks>This method performs an in-place stable merge sort of the given list. The sort requires O(n log n) running time and
        /// O(1) extra memory.</remarks>
        private static void MergeSort<T>(LinkedList<T> list, Comparison<T> comparison)
        {
            // An empty list, or a list with one element is already sorted.
            if (list.Count <= 1)
                return;

            // Sort the list.
            int mergeSize = 1;
            int merges;
            LinkedListNode<T> leftHead;
            LinkedListNode<T> rightHead;
            LinkedListNode<T> preNextRightHead = list.First;
            // Perform a series of passes, in which all the pairs of lists are individually merged.
            do
            {
                // Keep a count of the merges done this pass, if none were done then the list is sorted.
                merges = 0;

                // Set the initial positions of the list sections to sort.
                leftHead = list.First;
                rightHead = preNextRightHead.Next;

                // Perform a series of merges considering mergeSize elements from each side, do this whilst a second list exists.
                while (rightHead != null)
                {
                    // Perform a merge on the next pair of lists.
                    merges++;
                    int leftSize = mergeSize;
                    int rightSize = mergeSize;

                    // Merge whilst there are elements remaining in either list.
                    while (leftSize > 0 && rightSize > 0 && rightHead != null)
                    {
                        if (comparison(leftHead.Value, rightHead.Value) <= 0)
                        {
                            // The elements were already in the correct order, so just advance on the left list.
                            // We also advance in the case of equality, in order to create a stable sort.
                            leftHead = leftHead.Next;
                            leftSize--;
                        }
                        else
                        {
                            // The elements are in the wrong order, we must swap them.
                            LinkedListNode<T> nextRightHead = rightHead.Next;
                            rightSize--;
                            list.Remove(rightHead);
                            list.AddBefore(leftHead, rightHead);
                            rightHead = nextRightHead;
                        }
                    }

                    // Advance the right head to the start of the next pair of lists.
                    while (rightSize > 0 && rightHead != null)
                    {
                        rightSize--;
                        rightHead = rightHead.Next;
                    }

                    // Set up the start positions for the next pair of lists.
                    leftHead = rightHead;
                    for (int i = 0; i < mergeSize && rightHead != null; i++)
                        rightHead = rightHead.Next;

                    // After the first merge, the left head will be positioned at the index where the right head for the next pass should
                    // begin. We'll note the previous element so when the next pass begins it can simply use the next element for the head 
                    // of the right list, as opposed to enumerating over mergeSize elements to get to the same location. We must note down
                    // the previous element, as the current marker the the left head may get swapped and thus would change index.
                    if (merges == 1)
                        if (leftHead != null)
                            preNextRightHead = leftHead.Previous;
                        else
                            preNextRightHead = list.Last;
                }
                // Double the size of the lists we are merging.
                mergeSize *= 2;
            }
            while (merges > 0);
        }
    }
}

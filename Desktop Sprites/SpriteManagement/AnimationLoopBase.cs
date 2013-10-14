namespace DesktopSprites.SpriteManagement
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Text;
    using System.Threading;
    using DesktopSprites.Collections;
    using DesktopSprites.Core;

    /// <summary>
    /// Defines a base class for animators. Provides a timing loop and monitors the frame rate.
    /// </summary>
    public abstract class AnimationLoopBase : Disposable, ISpriteCollectionController
    {
        #region ConcurrentReadOnlySpriteCollection struct
        /// <summary>
        /// Provides read-only access to the sprite collection. Multiple threads may enumerate the collection concurrently without external
        /// synchronization. When mutating of the collection is required, the animation thread block until all enumerators complete.
        /// </summary>
        public struct ConcurrentReadOnlySpriteCollection : ICollection<ISprite>
        {
            #region ConcurrentEnumerator struct
            /// <summary>
            /// Enumerates the sprite collection. Enumerator is blocked until all mutations of the sprite collection complete, but the
            /// collection may safely be enumerated from several threads at once whilst no mutations are occurring.
            /// </summary>
            public struct ConcurrentEnumerator : IEnumerator<ISprite>
            {
                private readonly ReaderWriterLockSlim guard;
                private LinkedList<ISprite>.Enumerator enumerator;
                internal ConcurrentEnumerator(AnimationLoopBase animationLoopBase)
                {
                    this.guard = animationLoopBase.spritesGuard;
                    guard.EnterReadLock();
                    enumerator = animationLoopBase.sprites.GetEnumerator();
                }
                /// <summary>
                /// Advances the enumerator to the next element of the collection.
                /// </summary>
                /// <returns>Returns true if the enumerator was successfully advanced to the next element; false if the enumerator has
                /// passed the end of the collection.</returns>
                public bool MoveNext()
                {
                    return enumerator.MoveNext();
                }
                /// <summary>
                /// Gets the element in the collection at the current position of the enumerator.
                /// </summary>
                public ISprite Current
                {
                    get { return enumerator.Current; }
                }
                object System.Collections.IEnumerator.Current
                {
                    get { return Current; }
                }
                /// <summary>
                /// Sets the enumerator to its initial position, which is before the first element in the collection.
                /// </summary>
                public void Reset()
                {
                    ((System.Collections.IEnumerator)enumerator).Reset();
                }
                /// <summary>
                /// Releases all resources used by the enumerator. Be sure to call this method or the animation thread will be blocked.
                /// </summary>
                public void Dispose()
                {
                    enumerator.Dispose();
                    guard.ExitReadLock();
                }
            }
            #endregion
            private AnimationLoopBase animationLoopBase;
            internal ConcurrentReadOnlySpriteCollection(AnimationLoopBase animationLoopBase)
            {
                this.animationLoopBase = animationLoopBase;
            }
            /// <summary>
            /// Gets the number of sprites in the collection.
            /// </summary>
            public int Count
            {
                get { return animationLoopBase.sprites.Count; }
            }
            /// <summary>
            /// Gets a thread-safe enumerator for the collection.
            /// </summary>
            /// <returns>A thread-safe enumerator for the collection.</returns>
            public ConcurrentEnumerator GetEnumerator()
            {
                return new ConcurrentEnumerator(animationLoopBase);
            }
            IEnumerator<ISprite> IEnumerable<ISprite>.GetEnumerator()
            {
                return GetEnumerator();
            }
            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
            /// <summary>
            /// Determines whether a sprite is in the collection.
            /// </summary>
            /// <param name="sprite">The sprite to locate in the collection.</param>
            /// <returns>Returns true if value is found in the collection; otherwise, false.</returns>
            public bool Contains(ISprite sprite)
            {
                using (animationLoopBase.spritesGuard.InReadMode())
                    return animationLoopBase.sprites.Contains(sprite);
            }
            /// <summary>
            /// Copies the entire collection to a compatible one-dimensional <see cref="T:System.Array"/>, starting at the specified index
            /// of the target array.
            /// </summary>
            /// <param name="array">The one-dimensional <see cref="T:System.Array"/> that is the destination of the elements copied from
            /// the collection. The <see cref="T:System.Array"/> must have zero-based indexing.</param>
            /// <param name="index">The zero-based index in array at which copying begins.</param>
            /// <exception cref="T:System.ArgumentNullException"><paramref name="array"/> is null.</exception>
            /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
            /// <exception cref="T:System.ArgumentException">The number of elements in the source collection is greater than the available
            /// space from <paramref name="index"/> to the end of the destination array.</exception>
            public void CopyTo(ISprite[] array, int index)
            {
                using (animationLoopBase.spritesGuard.InReadMode())
                    animationLoopBase.sprites.CopyTo(array, index);
            }
            void ICollection<ISprite>.Add(ISprite item)
            {
                throw ReadOnlyException();
            }
            void ICollection<ISprite>.Clear()
            {
                throw ReadOnlyException();
            }
            bool ICollection<ISprite>.IsReadOnly
            {
                get { return true; }
            }
            bool ICollection<ISprite>.Remove(ISprite item)
            {
                throw ReadOnlyException();
            }
            private InvalidOperationException ReadOnlyException()
            {
                return new InvalidOperationException("Collection is read-only.");
            }
        }
        #endregion

        #region FrameRecordCollector class
        /// <summary>
        /// Holds information about render times, frame rate and garbage collections. Provides methods to output and display this data.
        /// </summary>
        internal class FrameRecordCollector
        {
            #region FrameRecord struct
            /// <summary>
            /// Holds information for one frame about its render time, interval and garbage collections performed in this interval.
            /// </summary>
            private struct FrameRecord
            {
                /// <summary>
                /// The target frame time, in milliseconds.
                /// </summary>
                public readonly float Target;
                /// <summary>
                /// The time this frame took to update and draw, in milliseconds.
                /// </summary>
                public readonly float Time;
                /// <summary>
                /// The interval between the starting of this frame and the last frame, in milliseconds.
                /// </summary>
                public readonly float Interval;
                /// <summary>
                /// The cumulative number of generation zero garbage collections that had occurred by this frame.
                /// </summary>
                public readonly int Gen0Collections;
                /// <summary>
                /// The cumulative number of generation one garbage collections that had occurred by this frame.
                /// </summary>
                public readonly int Gen1Collections;
                /// <summary>
                /// The cumulative number of generation two garbage collections that had occurred by this frame.
                /// </summary>
                public readonly int Gen2Collections;

                /// <summary>
                /// Initializes a new instance of the
                /// <see cref="T:DesktopSprites.SpriteManagement.AnimationLoopBase.FrameRecordCollector.FrameRecord"/> struct with the
                /// given frame target, time and interval, and records the current garbage collection counts.
                /// </summary>
                /// <param name="target">The target frame time, in milliseconds.</param>
                /// <param name="time">The time the frame took to update and draw, in milliseconds.</param>
                /// <param name="interval">The interval between frames (inclusive of frame rendering time), in milliseconds.</param>
                public FrameRecord(float target, float time, float interval)
                {
                    Target = target;
                    Time = time;
                    Interval = interval;
                    Gen0Collections = GC.CollectionCount(0);
                    Gen1Collections = GC.CollectionCount(1);
                    Gen2Collections = GC.CollectionCount(2);
                }

                /// <summary>
                /// Determines the highest, if any, generation of garbage collection that was performed between two
                /// <see cref="T:DesktopSprites.SpriteManagement.AnimationLoopBase.FrameRecordCollector.FrameRecord"/> structures.
                /// </summary>
                /// <param name="a">First
                /// <see cref="T:DesktopSprites.SpriteManagement.AnimationLoopBase.FrameRecordCollector.FrameRecord"/> to compare for
                /// garbage collections.</param>
                /// <param name="b">Second
                /// <see cref="T:DesktopSprites.SpriteManagement.AnimationLoopBase.FrameRecordCollector.FrameRecord"/> to compare for
                /// garbage collections.</param>
                /// <returns>An integer indicating the highest garbage generation collected, or -1 if no collections occurred.</returns>
                public static int CollectionGenerationPerformed(FrameRecord a, FrameRecord b)
                {
                    if (a.Gen2Collections != b.Gen2Collections)
                        return 2;
                    else if (a.Gen1Collections != b.Gen1Collections)
                        return 1;
                    else if (a.Gen0Collections != b.Gen0Collections)
                        return 0;
                    else
                        return -1;
                }

                /// <summary>
                /// Gets a <see cref="T:System.Drawing.Brush"/> related to the highest, if any, generation of garbage collection that was
                /// performed between two FrameInfo structures.
                /// </summary>
                /// <param name="a">First
                /// <see cref="T:DesktopSprites.SpriteManagement.AnimationLoopBase.FrameRecordCollector.FrameRecord"/> to compare for
                /// garbage collections.</param>
                /// <param name="b">Second
                /// <see cref="T:DesktopSprites.SpriteManagement.AnimationLoopBase.FrameRecordCollector.FrameRecord"/> to compare for
                /// garbage collections.</param>
                /// <returns>A <see cref="T:System.Drawing.Brush"/> whose color is determined by the garbage collection performed, if any.
                /// The color matches those used in the CLR Profiler.</returns>
                public static Brush CollectionGenerationPerformedBrush(FrameRecord a, FrameRecord b)
                {
                    switch (CollectionGenerationPerformed(a, b))
                    {
                        case 2:
                            return Brushes.Blue;
                        case 1:
                            return Brushes.Green;
                        case 0:
                            return Brushes.Red;
                        default:
                            return Brushes.White;
                    }
                }
            }
            #endregion

            /// <summary>
            /// Information about previous frames, this array is reused circularly with the current start index given by marker.
            /// </summary>
            private FrameRecord[] frameRecords;
            /// <summary>
            /// The current location in the frameRecords array where the next record will be written.
            /// </summary>
            private int marker;

            /// <summary>
            /// The area the graph is to occupy when drawn.
            /// </summary>
            private Rectangle graphArea;
            /// <summary>
            /// The width of a bar for each frame time.
            /// </summary>
            private int barWidth;
            /// <summary>
            /// The scale factor for the height of bars, where a factor of 1 results in 1 pixel per millisecond.
            /// </summary>
            private float barHeightFactor;

            /// <summary>
            /// Gets the number of records this <see cref="T:DesktopSprites.SpriteManagement.AnimationLoopBase.FrameRecordCollector"/> can
            /// hold.
            /// </summary>
            public int Capacity
            {
                get { return frameRecords.Length; }
            }
            /// <summary>
            /// Gets the number of records in this <see cref="T:DesktopSprites.SpriteManagement.AnimationLoopBase.FrameRecordCollector"/>.
            /// </summary>
            public int Count { get; private set; }

            /// <summary>
            /// Gets the lowest frame time on record.
            /// </summary>
            public float MinTime
            {
                get
                {
                    float minTime = float.MaxValue;
                    for (int i = 0; i < Count; i++)
                        if (frameRecords[i].Time < minTime)
                            minTime = frameRecords[i].Time;
                    return Count != 0 ? minTime : 0;
                }
            }
            /// <summary>
            /// Gets the mean frame time of the whole record.
            /// </summary>
            public float MeanTime
            {
                get
                {
                    float sumTimes = 0;
                    for (int i = 0; i < Count; i++)
                        sumTimes += frameRecords[i].Time;
                    return Count != 0 ? sumTimes / (float)Count : 0;
                }
            }
            /// <summary>
            /// Gets the highest frame time on record.
            /// </summary>
            public float MaxTime
            {
                get
                {
                    float maxTime = float.MinValue;
                    for (int i = 0; i < Count; i++)
                        if (frameRecords[i].Time > maxTime)
                            maxTime = frameRecords[i].Time;
                    return Count != 0 ? maxTime : 0;
                }
            }

            /// <summary>
            /// Gets the lowest frame interval on record.
            /// </summary>
            public float MinInterval
            {
                get
                {
                    float minInterval = float.MaxValue;
                    for (int i = 0; i < Count; i++)
                        if (frameRecords[i].Interval < minInterval)
                            minInterval = frameRecords[i].Interval;
                    return Count != 0 ? minInterval : 0;
                }
            }
            /// <summary>
            /// Gets the mean frame interval of the whole record.
            /// </summary>
            public float MeanInterval
            {
                get
                {
                    float sumIntervals = 0;
                    for (int i = 0; i < Count; i++)
                        sumIntervals += frameRecords[i].Interval;
                    return Count != 0 ? sumIntervals / (float)Count : 0;
                }
            }
            /// <summary>
            /// Gets the highest frame interval on record.
            /// </summary>
            public float MaxInterval
            {
                get
                {
                    float maxInterval = float.MinValue;
                    for (int i = 0; i < Count; i++)
                        if (frameRecords[i].Interval > maxInterval)
                            maxInterval = frameRecords[i].Interval;
                    return Count != 0 ? maxInterval : 0;
                }
            }

            /// <summary>
            /// Gets the current frame rate averaged across all records.
            /// </summary>
            public float FramesPerSecond
            {
                get { return Count != 0 ? 1000f / MeanInterval : 0; }
            }
            /// <summary>
            /// Gets the achievable (i.e. if the frame rate was not limited) frame rate averaged across all records.
            /// </summary>
            public float AchievableFramesPerSecond
            {
                get { return Count != 0 ? 1000f / MeanTime : 0; }
            }

            /// <summary>
            /// Gets a printable string of the min, mean and max frame times.
            /// </summary>
            public string FrameTimings
            {
                get
                {
                    return "time: " +
                        MinTime.ToString("0.0", CultureInfo.CurrentCulture) + "ms/" +
                        MeanTime.ToString("0.0", CultureInfo.CurrentCulture) + "ms/" +
                        MaxTime.ToString("0.0", CultureInfo.CurrentCulture) + "ms";
                }
            }
            /// <summary>
            /// Gets a printable string of the min, mean and max frame intervals.
            /// </summary>
            public string FrameIntervals
            {
                get
                {
                    return "interval: " +
                        MinInterval.ToString("0.0", CultureInfo.CurrentCulture) + "ms/" +
                        MeanInterval.ToString("0.0", CultureInfo.CurrentCulture) + "ms/" +
                        MaxInterval.ToString("0.0", CultureInfo.CurrentCulture) + "ms";
                }
            }
            /// <summary>
            /// Gets a printable string of the current and achievable frames per second.
            /// </summary>
            public string FrameRates
            {
                get
                {
                    return "fps: " +
                        FramesPerSecond.ToString("0.0", CultureInfo.CurrentCulture) + "/" +
                        AchievableFramesPerSecond.ToString("0.0", CultureInfo.CurrentCulture);
                }
            }
            /// <summary>
            /// Gets a printable string of the garbage collections for generations 0, 1 and 2.
            /// </summary>
            public string CollectionCounts
            {
                get
                {
                    int index = marker - 1 >= 0 ? marker - 1 : Count - 1;
                    return "Collections: " +
                        frameRecords[index].Gen0Collections + "/" +
                        frameRecords[index].Gen1Collections + "/" +
                        frameRecords[index].Gen2Collections;
                }
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.AnimationLoopBase.FrameRecordCollector"/>
            /// class to hold data about the given number of frames.
            /// </summary>
            /// <param name="count">The number of frames to hold data on.</param>
            public FrameRecordCollector(int count)
            {
                frameRecords = new FrameRecord[count];
                SetGraphingAttributes(Point.Empty, 100, 1, 1f);
            }

            /// <summary>
            /// Records the rendering of a new frame with the specified times, and notes the current garbage collections made.
            /// </summary>
            /// <param name="target">The target frame time, in milliseconds.</param>
            /// <param name="time">The time the frame took to update and draw, in milliseconds.</param>
            /// <param name="interval">The interval between frames (inclusive of frame rendering time), in milliseconds.</param>
            public void Record(float target, float time, float interval)
            {
                frameRecords[marker] = new FrameRecord(target, time, interval);
                if (++marker >= frameRecords.Length)
                    marker = 0;
                if (Count < frameRecords.Length)
                    Count++;
            }

            /// <summary>
            /// Gets a short summary of current performance.
            /// </summary>
            public string GetSummary()
            {
                return string.Format(CultureInfo.CurrentCulture,
                    "fps: {0:0.0}/{1:0.0} time: {2:0.0}ms/{3:0.0}ms/{4:0.0}ms interval: {5:0.0}ms/{6:0.0}ms/{7:0.0}ms",
                    FramesPerSecond, AchievableFramesPerSecond,
                    MinTime, MeanTime, MaxTime,
                    MinInterval, MeanInterval, MaxInterval);
            }

            /// <summary>
            /// Sets up the desired position and bar sizes for the graph drawn by the
            /// <see cref="M:DesktopSprites.SpriteManagement.AnimationLoopBase.FrameRecordCollector.DrawGraph"/> method. Returns the
            /// resulting <see cref="T:System.Drawing.Rectangle"/> drawing this graph will occupy.
            /// </summary>
            /// <param name="location">The top-left corner the graph should be drawn from.</param>
            /// <param name="graphHeight">The desired height of the graph.</param>
            /// <param name="individualBarWidth">The width of each bar for each frame time.</param>
            /// <param name="barHeightScaleFactor">The scale factor for the height of bars.
            /// A factor of 1 results in 1 pixel per millisecond if this fits within the desired height.</param>
            /// <returns>The resulting <see cref="T:System.Drawing.Rectangle"/> drawing this graph will occupy.</returns>
            public Rectangle SetGraphingAttributes(
                Point location, int graphHeight, int individualBarWidth, float barHeightScaleFactor)
            {
                // Leave a 1 pixel padding on each side.
                graphArea.Width = individualBarWidth * frameRecords.Length + 2;
                graphArea.Height = graphHeight;
                graphArea.Location = location;
                barWidth = individualBarWidth;
                barHeightFactor = barHeightScaleFactor;

                return graphArea;
            }
            /// <summary>
            /// Draws a graph and related text output onto the given graphics surface.
            /// </summary>
            /// <param name="surface">The <see cref="T:System.Drawing.Graphics"/> surface on which to draw the graph.</param>
            public void DrawGraph(Graphics surface)
            {
                // Determine the bar height scale, this will adjust if the max time is too high to fit on the graph.
                float barHeightScale = barHeightFactor;
                float height = (float)(graphArea.Height - 2);
                if (MaxTime != 0 && MaxInterval != 0)
                    barHeightScale = Math.Min(barHeightFactor, Math.Min(height / MaxTime, height / MaxInterval));

                // Fill the graph area.
                // We are leaving a 1 pixel padding on all sides, so the component parts of the graph are often offset by 1.
                using (var graphBackgroundBrush = new SolidBrush(Color.FromArgb(1, 1, 1)))
                    surface.FillRectangle(graphBackgroundBrush, graphArea);

                // Start at the oldest record.
                int barOffset = frameRecords.Length - Count;
                int m = barOffset == 0 ? marker : 0;
                // Iterate through the records and draw each target, time and interval.
                for (int i = 0; i < Count; i++)
                {
                    // Determine bar color by comparing this record with the previous record.
                    // The first bar has no previous record, so just get the default color.
                    // The records array is circular, so we must join the first and last elements when required.
                    Brush timeBarBrush;
                    if (i == 0)
                        timeBarBrush = FrameRecord.CollectionGenerationPerformedBrush(
                            frameRecords[m], frameRecords[m]);
                    else
                        timeBarBrush = FrameRecord.CollectionGenerationPerformedBrush(
                            frameRecords[m], frameRecords[m != 0 ? m - 1 : Count - 1]);

                    // Determine bar sizes.
                    float targetHeight = frameRecords[m].Target * barHeightScale;
                    float timeHeight = frameRecords[m].Time * barHeightScale;
                    float intervalHeight = frameRecords[m].Interval * barHeightScale;
                    int barLeft = graphArea.Left + 1 + barOffset + i * barWidth;

                    // Draw the bars.
                    surface.FillRectangle(
                        Brushes.Gray, barLeft, graphArea.Bottom - 1 - intervalHeight, barWidth, intervalHeight - timeHeight);
                    surface.FillRectangle(
                        timeBarBrush, barLeft, graphArea.Bottom - 1 - timeHeight, barWidth, timeHeight);
                    surface.FillRectangle(
                        Brushes.DarkBlue, barLeft, graphArea.Bottom - 1 - targetHeight, barWidth, 1);

                    // Move to the next record.
                    if (++m >= Count)
                        m = 0;
                }

                // Draw the mean interval time.
                int meanIntervalHeight = graphArea.Bottom - 1 - (int)(MeanInterval * barHeightScale);
                surface.DrawLine(Pens.DarkGray, graphArea.Left + 1, meanIntervalHeight, graphArea.Right - 2, meanIntervalHeight);

                // Draw the mean frame time.
                int meanTimeHeight = graphArea.Bottom - 1 - (int)(MeanTime * barHeightScale);
                surface.DrawLine(Pens.LightGray, graphArea.Left + 1, meanTimeHeight, graphArea.Right - 2, meanTimeHeight);

                // Draw axis markers.
                int markerThickness = barHeightScale < .5f ? 1 : 2;
                for (int i = 0; i <= (graphArea.Height - 1) / barHeightScale; i += 10)
                {
                    int markerLineHeight = graphArea.Bottom - 1 - (int)(i * barHeightScale);
                    int markerWidth = i % 50 == 0 ? 12 : 4;
                    surface.FillRectangle(Brushes.Cyan, graphArea.Left + 1, markerLineHeight - 1, markerWidth, markerThickness);
                }
            }
            /// <summary>
            /// Draws a graph and related text output onto the given Cairo context.
            /// </summary>
            /// <param name="context">The <see cref="T:Cairo.Context"/> context on which to draw the graph.</param>
            public void DrawGraph(Cairo.Context context)
            {
                context.Save();

                // Determine the bar height scale, this will adjust if the max time is too high to fit on the graph.
                float barHeightScale = barHeightFactor;
                float height = (float)(graphArea.Height - 2);
                if (MaxTime != 0 && MaxInterval != 0)
                    barHeightScale = Math.Min(barHeightFactor, Math.Min(height / MaxTime, height / MaxInterval));

                // Fill the graph area.
                // We are leaving a 1 pixel padding on all sides, so the component parts of the graph are often offset by 1.
                context.Rectangle(new Cairo.Rectangle(graphArea.X, graphArea.Y, graphArea.Width, graphArea.Height));
                context.SetSourceRGB(0, 0, 0);
                context.Fill();

                // Start at the oldest record.
                int barOffset = frameRecords.Length - Count;
                int m = barOffset == 0 ? marker : 0;
                // Iterate through the records and draw each target, time and interval.
                for (int i = 0; i < Count; i++)
                {
                    // Determine bar color by comparing this record with the previous record.
                    // The first bar has no previous record, so just get the default color.
                    // The records array is circular, so we must join the first and last elements when required.
                    int timeBarNumber;
                    if (i == 0)
                        timeBarNumber = FrameRecord.CollectionGenerationPerformed(
                            frameRecords[m], frameRecords[m]);
                    else
                        timeBarNumber = FrameRecord.CollectionGenerationPerformed(
                            frameRecords[m], frameRecords[m != 0 ? m - 1 : Count - 1]);

                    // Determine bar sizes.
                    float targetHeight = frameRecords[m].Target * barHeightScale;
                    float timeHeight = frameRecords[m].Time * barHeightScale;
                    float intervalHeight = frameRecords[m].Interval * barHeightScale;
                    int barLeft = graphArea.Left + 1 + barOffset + i * barWidth;

                    // Draw the bars.
                    context.SetSourceRGB(0.5, 0.5, 0.5);
                    context.Rectangle(
                        new Cairo.Rectangle(
                        barLeft, graphArea.Bottom - 1 - intervalHeight, barWidth, intervalHeight - timeHeight));
                    context.Fill();
                        
                    if (timeBarNumber == 0)
                        context.SetSourceRGB(1, 0, 0);
                    else if (timeBarNumber == 1)
                        context.SetSourceRGB(0, 1, 0);
                    else if (timeBarNumber == 2)
                        context.SetSourceRGB(0, 0, 1);
                    else
                        context.SetSourceRGB(1, 1, 1);
                    
                    context.Rectangle(
                        new Cairo.Rectangle(barLeft, graphArea.Bottom - 1 - timeHeight, barWidth, timeHeight));
                    context.Fill();
                        
                    context.SetSourceRGB(0.2, 0.2, 0.5);
                    context.Rectangle(
                        new Cairo.Rectangle(barLeft, graphArea.Bottom - 1 - targetHeight, barWidth, 1));
                    context.Fill();

                    // Move to the next record.
                    if (++m >= Count)
                        m = 0;
                }
                
                context.LineWidth = 1;

                // Draw the mean interval time.
                int meanIntervalHeight = graphArea.Bottom - 1 - (int)(MeanInterval * barHeightScale);
                context.SetSourceRGB(0.3, 0.3, 0.3);
                context.MoveTo(graphArea.Left + 1, meanIntervalHeight);
                context.LineTo(graphArea.Right - 2, meanIntervalHeight);
                context.ClosePath();
                context.Stroke();
                context.Fill();

                // Draw the mean frame time.
                int meanTimeHeight = graphArea.Bottom - 1 - (int)(MeanTime * barHeightScale);
                context.SetSourceRGB(0.7, 0.7, 0.7);
                context.MoveTo(graphArea.Left + 1, meanTimeHeight);
                context.LineTo(graphArea.Right - 2, meanTimeHeight);
                context.ClosePath();
                context.Stroke();
                context.Fill();

                // Draw axis markers.
                int markerThickness = barHeightScale < .5f ? 1 : 2;
                context.SetSourceRGB(0.5, 1, 1);
                for (int i = 0; i <= (graphArea.Height - 1) / barHeightScale; i += 10)
                {
                    int markerLineHeight = graphArea.Bottom - 1 - (int)(i * barHeightScale);
                    int markerWidth = i % 50 == 0 ? 12 : 4;
                    context.Rectangle(
                    new Cairo.Rectangle(graphArea.Left + 1, markerLineHeight - 1, markerWidth, markerThickness));
                    context.Fill();
                }

                context.Restore();
            }
        }
        #endregion

        #region Fields and Properties
        /// <summary>
        /// Thread that runs the main animation loop.
        /// </summary>
        private Thread runner;
        /// <summary>
        /// Tracks the elapsed time for frame rendering, and frame intervals.
        /// </summary>
        private readonly System.Diagnostics.Stopwatch intervalWatch = new System.Diagnostics.Stopwatch();
        /// <summary>
        /// Tracks the total elapsed time of animation.
        /// </summary>
        private readonly System.Diagnostics.Stopwatch elapsedWatch = new System.Diagnostics.Stopwatch();
        /// <summary>
        /// Gets a value indicating whether animation has been started.
        /// </summary>
        public bool Started { get; private set; }
        /// <summary>
        /// Gets or sets a value indicating whether animation is currently paused.
        /// </summary>
        public bool Paused
        {
            get
            {
                return !elapsedWatch.IsRunning;
            }
            set
            {
                if (value)
                    Pause(false);
                else
                    Resume();
            }
        }
        /// <summary>
        /// Gets a value indicating whether animation has been stopped.
        /// </summary>
        public bool Stopped
        {
            get { return Disposed; }
        }
        /// <summary>
        /// Used to pause the thread running the main loop. Signals false whilst paused, otherwise signals true.
        /// </summary>
        private readonly ManualResetEvent running = new ManualResetEvent(true);
#if DEBUG
        /// <summary>
        /// Tracks lost time from long delays when debugging the application.
        /// </summary>
        private TimeSpan lostTime = TimeSpan.Zero;
#endif
        /// <summary>
        /// The total elapsed time that animation has been running.
        /// </summary>
        private TimeSpan elapsedTime;
        /// <summary>
        /// Gets the total elapsed time that animation has been running.
        /// </summary>
        public TimeSpan ElapsedTime
        {
            get 
            {
#if DEBUG
                lock (tickSync)
                    return elapsedTime - lostTime;
#else
                lock (tickSync)
                    return elapsedTime;
#endif
            }
        }
        /// <summary>
        /// Synchronization object for locking methods which may be accessed on this class from its owner, but also the animation thread.
        /// </summary>
        private readonly object tickSync = new object();

        /// <summary>
        /// Guards enumeration and mutation of the sprites collection.
        /// </summary>
        private readonly ReaderWriterLockSlim spritesGuard = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        /// <summary>
        /// The collection of active sprites.
        /// </summary>
        private readonly LinkedList<ISprite> sprites;
        /// <summary>
        /// Queue of actions to be performed on the active sprite collection during the next update cycle.
        /// </summary>
        private readonly Queue<Action> queuedSpriteActions = new Queue<Action>();
        /// <summary>
        /// Gets the collection of active sprites.
        /// </summary>
        protected ConcurrentReadOnlySpriteCollection Sprites { get; private set; }
        /// <summary>
        /// Gets the viewer for the sprite collection.
        /// </summary>
        protected ISpriteCollectionView Viewer { get; private set; }

        /// <summary>
        /// Holds information about the performance of the animator.
        /// </summary>
        private readonly FrameRecordCollector performanceRecorder = new FrameRecordCollector(300);
        /// <summary>
        /// Output of performance summary information.
        /// </summary>
        private readonly StringBuilder performanceSummary = new StringBuilder(100);
        /// <summary>
        /// The minimum value for the interval of the timer.
        /// </summary>
        private float minimumTickInterval = 1000f / 60f;
        /// <summary>
        /// Gets or sets the target frame rate of the animation. This value is greater than 0 and no more than 120.
        /// </summary>
        public float MaximumFramesPerSecond
        {
            get
            {
                return 1000f / minimumTickInterval;
            }
            set
            {
                if (value <= 0f)
                    throw new ArgumentOutOfRangeException("value", value, "Target frame rate must be positive.");
                if (value > 120f)
                    throw new ArgumentOutOfRangeException("value", value, "Target frame rate should not exceed 120.");
                minimumTickInterval = 1000f / value;
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when animation has started.
        /// </summary>
        public event EventHandler AnimationStarted;
        /// <summary>
        /// Occurs when animation has finished.
        /// </summary>
        public event EventHandler AnimationFinished;
        /// <summary>
        /// Occurs when a single sprite is added to the collection.
        /// </summary>
        protected event EventHandler<CollectionItemChangedEventArgs<ISprite>> SpriteAdded;
        /// <summary>
        /// Occurs when a single sprite is successfully removed from the collection.
        /// </summary>
        protected event EventHandler<CollectionItemChangedEventArgs<ISprite>> SpriteRemoved;
        /// <summary>
        /// Occurs when multiple sprites are added to the collection.
        /// </summary>
        protected event EventHandler<CollectionItemsChangedEventArgs<ISprite>> SpritesAdded;
        /// <summary>
        /// Occurs when multiple sprites are successfully removed from the collection.
        /// </summary>
        protected event EventHandler<CollectionItemsChangedEventArgs<ISprite>> SpritesRemoved;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.SpriteManagement.AnimationLoopBase"/> class handling the given
        /// collection of sprites.
        /// </summary>
        /// <param name="spriteViewer">The <see cref="T:DesktopSprites.SpriteManagement.ISpriteCollectionView"/> used to display the
        /// sprites.</param>
        /// <param name="spriteCollection">The initial collection of <see cref="T:DesktopSprites.SpriteManagement.ISprite"/> to be
        /// displayed by the animator, which may be null.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="spriteViewer"/> is null.</exception>
        protected AnimationLoopBase(ISpriteCollectionView spriteViewer, IEnumerable<ISprite> spriteCollection)
        {
            Argument.EnsureNotNull(spriteViewer, "spriteViewer");

            Viewer = spriteViewer;

            if (Viewer is WinFormSpriteInterface)
                ((WinFormSpriteInterface)Viewer).Collector = performanceRecorder;
            else if (Viewer is GtkSpriteInterface)
                ((GtkSpriteInterface)Viewer).Collector = performanceRecorder;

            // Create an asynchronous collection, so it can be safely exposed to derived classes.
            if (spriteCollection == null)
                sprites = new LinkedList<ISprite>();
            else
                sprites = new LinkedList<ISprite>(spriteCollection);
            Sprites = new ConcurrentReadOnlySpriteCollection(this);

            // Stop the animator when the viewer is closed.
            Viewer.InterfaceClosed += (sender, e) => Finish();

            string collectionType = "(empty collection)";
            if (spriteCollection != null)
                collectionType = spriteCollection.GetType().ToString();

            Console.WriteLine("Created AnimationLoopBase for:");
            Console.WriteLine("-ISpriteCollectionController: " + GetType());
            Console.WriteLine("-ISpriteCollectionView: " + Viewer.GetType());
            Console.WriteLine("-IEnumerable<ISprite>: " + collectionType);
        }

        /// <summary>
        /// Shows the animator and begins animating. Start is called on behalf of all sprites currently in the collection.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The animator has already been started.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The animator has been stopped.</exception>
        public virtual void Start()
        {
            if (Started)
                throw new InvalidOperationException("Cannot start an animator that has already been started.");
            EnsureNotDisposed();

            Console.WriteLine(GetType() + " is starting an animation loop...");
            Interlocked.Exchange(ref runner, new Thread(Run) { Name = "AnimationLoopBase.Run" });
            Viewer.Open();
            runner.Start();
        }

        /// <summary>
        /// Pauses animation, and optionally hides the animator while paused. If animation is already paused, this has no effect.
        /// </summary>
        /// <param name="hide">True to hide the animator, false to leave the animator visible in the paused state.</param>
        /// <exception cref="T:System.InvalidOperationException">The animator has not been started.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The animator has been stopped.</exception>
        public virtual void Pause(bool hide)
        {
            if (!Started)
                throw new InvalidOperationException("Cannot pause an animator that has not been started.");
            EnsureNotDisposed();

            if (!Paused)
            {
                running.Reset();

                Viewer.Pause();
                if (hide)
                    Viewer.Hide();

                intervalWatch.Stop();
                elapsedWatch.Stop();
            }
        }

        /// <summary>
        /// Resumes animation, and shows the animator if it was hidden while paused. If animation is not paused, this has no effect.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The animator has not been started.</exception>
        /// <exception cref="T:System.ObjectDisposedException">The animator has been stopped.</exception>
        public virtual void Resume()
        {
            if (!Started)
                throw new InvalidOperationException("Cannot resume an animator that has not been started.");
            EnsureNotDisposed();

            if (Paused)
            {
                elapsedWatch.Start();
                intervalWatch.Start();

                Viewer.Show();
                Viewer.Resume();

                running.Set();
            }
        }

        /// <summary>
        /// Queues a sprite to be added to the collection of active sprites at the start of the next update cycle.
        /// </summary>
        /// <param name="sprite">The sprite to add to the collection. Start will be called on this sprite when it is added.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sprite"/> is null.</exception>
        protected void QueueAddAndStart(ISprite sprite)
        {
            Argument.EnsureNotNull(sprite, "sprite");
            queuedSpriteActions.Enqueue(() =>
            {
                sprites.AddLast(sprite);
                sprite.Start(ElapsedTime);
                SpriteAdded.Raise(this, () => new CollectionItemChangedEventArgs<ISprite>(sprite));
            });
        }

        /// <summary>
        /// Queues a collection of sprites to be added to the active sprites at the start of the next update cycle.
        /// </summary>
        /// <param name="sprites">The collection of sprites to add. Start will be called on these sprite when they are added.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sprites"/> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="sprites"/> contained a sprite that was null.</exception>
        protected void QueueAddRangeAndStart(IEnumerable<ISprite> sprites)
        {
            Argument.EnsureNotNull(sprites, "sprites");
            var items = sprites.ToImmutableArray();
            foreach (ISprite sprite in items)
                if (sprite == null)
                    throw new ArgumentException("sprites contained a sprite that was null.", "sprites");
            queuedSpriteActions.Enqueue(() =>
            {
                foreach (ISprite sprite in items)
                {
                    this.sprites.AddLast(sprite);
                    sprite.Start(ElapsedTime);
                }
                SpritesAdded.Raise(this, () => new CollectionItemsChangedEventArgs<ISprite>(items));
            });
        }

        /// <summary>
        /// Queues a sprite to be removed from the collection of active sprites at the start of the next update cycle.
        /// </summary>
        /// <param name="sprite">The sprite to remove from the collection.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="sprite"/> is null.</exception>
        protected void QueueRemove(ISprite sprite)
        {
            Argument.EnsureNotNull(sprite, "sprite");
            queuedSpriteActions.Enqueue(() =>
            {
                if (sprites.Remove(sprite))
                    SpriteRemoved.Raise(this, () => new CollectionItemChangedEventArgs<ISprite>(sprite));
            });
        }

        /// <summary>
        /// Queues a removal of sprites according to the specified predicate from the collection of active sprites at the start of the next
        /// update cycle.
        /// </summary>
        /// <param name="predicate">A function that determines the sprites to remove from the collection.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="predicate"/> is null.</exception>
        protected void QueueRemove(Predicate<ISprite> predicate)
        {
            Argument.EnsureNotNull(predicate, "predicate");
            queuedSpriteActions.Enqueue(() =>
            {
                var spritesRemoved = SpritesRemoved;
                List<ISprite> items = spritesRemoved != null ? new List<ISprite>() : null;
                LinkedListNode<ISprite> node = sprites.First;
                while (node != null)
                {
                    LinkedListNode<ISprite> nextNode = node.Next;
                    if (predicate(node.Value))
                    {
                        sprites.Remove(node);
                        if (spritesRemoved != null)
                            items.Add(node.Value);
                    }
                    node = nextNode;
                }
                if (spritesRemoved != null && items.Count != 0)
                    spritesRemoved(this, new CollectionItemsChangedEventArgs<ISprite>(items));
            });
        }

        /// <summary>
        /// Queues a removal of all sprites from the collection of active sprites at the start of the next update cycle.
        /// </summary>
        protected void QueueClear()
        {
            queuedSpriteActions.Enqueue(() =>
            {
                var spritesRemoved = SpritesRemoved;
                var items = spritesRemoved != null ? sprites.ToImmutableArray() : null;
                sprites.Clear();
                if (spritesRemoved != null && items.Length != 0)
                    spritesRemoved(this, new CollectionItemsChangedEventArgs<ISprite>(items));
            });
        }

        /// <summary>
        /// Sorts the active sprites using the default comparer.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">The default comparer
        /// <see cref="P:System.Collections.Generic.Comparer`1.Default"/> cannot find an implementation of the
        /// <see cref="T:System.IComparable`1"/> generic interface or the <see cref="T:System.IComparable"/> interface for type
        /// ISprite.</exception>
        protected void Sort()
        {
            sprites.Sort();
        }

        /// <summary>
        /// Sorts the active sprites using the specified <see cref="T:System.Comparison`1"/>.
        /// </summary>
        /// <param name="comparison">The <see cref="T:System.Comparison`1"/> to use when comparing elements.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="comparison"/> is null.</exception>
        protected void Sort(Comparison<ISprite> comparison)
        {
            sprites.Sort(comparison);
        }

        /// <summary>
        /// Sorts the active sprites using the specified comparer.
        /// </summary>
        /// <param name="comparer">The <see cref="T:System.Collections.Generic.IComparer`1"/> implementation to use when comparing
        /// elements, or null to use the default comparer <see cref="P:System.Collections.Generic.Comparer`1.Default"/>.</param>
        /// <exception cref="T:System.InvalidOperationException"><paramref name="comparer"/> is null, and the default comparer
        /// <see cref="P:System.Collections.Generic.Comparer`1.Default"/> cannot find implementation of the
        /// <see cref="T:System.IComparable`1"/> generic interface or the <see cref="T:System.IComparable"/> interface for type
        /// ISprite.</exception>
        protected void Sort(IComparer<ISprite> comparer)
        {
            sprites.Sort(comparer);
        }

        /// <summary>
        /// Runs frame cycles continuously and tracks animation timings.
        /// </summary>
        private void Run()
        {
            // Keeps a count of frames since performance information was last displayed.
            int performanceDelayCount = 0;

            // Loop whilst not disposed, paused or stopped.
            while (!Disposed && running.WaitOne() && !Stopped)
            {
                // Run an update and draw cycle for one frame.
                Tick();

#if DEBUG
                // When debugging, assume long delays are due to hitting breakpoints.
                TimeSpan tickElapsed = elapsedWatch.Elapsed - elapsedTime;
                float frameTime;
                if (tickElapsed.TotalMilliseconds < 2000)
                {
                    frameTime = (float)tickElapsed.TotalMilliseconds;
                }
                else
                {
                    frameTime = minimumTickInterval;
                    lostTime += tickElapsed - TimeSpan.FromMilliseconds(frameTime);
                }
#else
                float frameTime = (float)(elapsedWatch.Elapsed - ElapsedTime).TotalMilliseconds;
#endif

                // Sleep until the next tick should start.
                float nextInterval = minimumTickInterval - frameTime;
                if (nextInterval > 0)
                    Thread.Sleep((int)Math.Ceiling(nextInterval));

                // Track interval timings.
                float intervalTime = (float)intervalWatch.Elapsed.TotalMilliseconds;
                intervalWatch.Restart();

                // Record the timings and display performance summary occasionally.
                performanceRecorder.Record(minimumTickInterval, frameTime, intervalTime);
                if (++performanceDelayCount >= MaximumFramesPerSecond * 10)
                {
                    performanceDelayCount = 0;
                    Console.WriteLine(performanceRecorder.GetSummary());
                }
            }
        }

        /// <summary>
        /// Runs an update and draw cycle.
        /// </summary>
        private void Tick()
        {
            if (!Started)
                StartInternal();
            else
                Update();
            if (!Disposed)
                Draw();
        }

        /// <summary>
        /// Starts timers and calls Start on all sprites in the collection.
        /// </summary>
        private void StartInternal()
        {
            // Start timers to track total elapsed time and interval time.
            elapsedWatch.Start();
            intervalWatch.Start();
            // Notify the outside world that animation has started.
            Started = true;
            AnimationStarted.Raise(this);
            // Start all the sprites.
            foreach (ISprite sprite in sprites)
                sprite.Start(ElapsedTime);
            // Force a collection now, to clear the heap of any memory from loading. Assuming the loop makes little to no allocations, this
            // should ensure cheap and quick generation zero collections, and will delay the first collection as long as possible.
            General.FullCollect();
        }

        /// <summary>
        /// Process any pending queued actions on the sprites collection now. This method is called during every update, buy may be called
        /// in derived classes if they need to force queued changes to be applied immediately. 
        /// </summary>
        protected void ProcessQueuedActions()
        {
            using (spritesGuard.InWriteMode())
                while (queuedSpriteActions.Count > 0)
                    queuedSpriteActions.Dequeue().Invoke();
        }

        /// <summary>
        /// Process queued actions and then updates the sprites.
        /// </summary>
        protected virtual void Update()
        {
            if (runner == null)
                EnsureNotDisposed();
            lock (tickSync)
            {
                elapsedTime = elapsedWatch.Elapsed;
                ProcessQueuedActions();
                foreach (ISprite sprite in sprites)
                    sprite.Update(ElapsedTime);
            }
        }

        /// <summary>
        /// Draws the sprites.
        /// </summary>
        protected virtual void Draw()
        {
            if (runner == null)
                EnsureNotDisposed();
            lock (tickSync)
                Viewer.Draw(Sprites);
        }

        /// <summary>
        /// Stops animation and closes the viewer.
        /// </summary>
        public virtual void Finish()
        {
            Dispose();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">Indicates if managed resources should be disposed in addition to unmanaged resources; otherwise, only
        /// unmanaged resources should be disposed.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (runner != null)
                {
                    // Un-pause so the main thread can gracefully exit now the signal to stop has been issued.
                    running.Set();
                    if (Thread.CurrentThread != runner)
                        runner.Join();
                    Interlocked.Exchange(ref runner, null);
                    AnimationFinished.Raise(this);
                }
                Viewer.Close();
                running.Dispose();
                spritesGuard.Dispose();
            }
        }
    }
}

namespace CsDesktopPonies.SpriteManagement
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using CsDesktopPonies.Collections;
using System.Text;

    /// <summary>
    /// Defines a base class for animators. Provides a timing loop and monitors the frame rate.
    /// </summary>
    public abstract class AnimationLoopBase : Disposable, ISpriteCollectionController
    {
        #region FrameRecordCollector class
        /// <summary>
        /// Holds information about render times, frame rate and garbage collections. Provides methods to output and display this data.
        /// </summary>
        internal class FrameRecordCollector : IDisposable
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
                /// <see cref="T:CsDesktopPonies.SpriteManagement.AnimationLoopBase.FrameRecordCollector.FrameRecord"/> struct with the
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
                /// <see cref="T:CsDesktopPonies.SpriteManagement.AnimationLoopBase.FrameRecordCollector.FrameRecord"/> structures.
                /// </summary>
                /// <param name="a">First
                /// <see cref="T:CsDesktopPonies.SpriteManagement.AnimationLoopBase.FrameRecordCollector.FrameRecord"/> to compare for
                /// garbage collections.</param>
                /// <param name="b">Second
                /// <see cref="T:CsDesktopPonies.SpriteManagement.AnimationLoopBase.FrameRecordCollector.FrameRecord"/> to compare for
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
                /// <see cref="T:CsDesktopPonies.SpriteManagement.AnimationLoopBase.FrameRecordCollector.FrameRecord"/> to compare for
                /// garbage collections.</param>
                /// <param name="b">Second
                /// <see cref="T:CsDesktopPonies.SpriteManagement.AnimationLoopBase.FrameRecordCollector.FrameRecord"/> to compare for
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
            /// The brush used to paint the background of the graph area.
            /// </summary>
            private Brush graphBackgroundBrush = new SolidBrush(Color.FromArgb(1, 1, 1));

            /// <summary>
            /// Gets the number of records this <see cref="T:CsDesktopPonies.SpriteManagement.AnimationLoopBase.FrameRecordCollector"/> can
            /// hold.
            /// </summary>
            public int Capacity
            {
                get { return frameRecords.Length; }
            }
            /// <summary>
            /// Gets the number of records in this <see cref="T:CsDesktopPonies.SpriteManagement.AnimationLoopBase.FrameRecordCollector"/>.
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
            /// Initializes a new instance of the <see cref="T:CsDesktopPonies.SpriteManagement.AnimationLoopBase.FrameRecordCollector"/>
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
            /// Writes performance summary to the specified <see cref="T:System.Text.StringBuilder"/>.
            /// </summary>
            /// <param name="sb">The <see cref="T:System.Text.StringBuilder"/> to be cleared and then where the performance summary will be
            /// output.</param>
            public void OutputSummaryTo(StringBuilder sb)
            {
                sb.Clear();
                sb.AppendFormat(CultureInfo.CurrentCulture,
                    "fps: {0:0.0}/{1:0.0} time: {2:0.0}ms/{3:0.0}ms/{4:0.0}ms interval: {5:0.0}ms/{6:0.0}ms/{7:0.0}ms",
                    FramesPerSecond, AchievableFramesPerSecond,
                    MinTime, MeanTime, MaxTime,
                    MinInterval, MeanInterval, MaxInterval);
            }

            /// <summary>
            /// Sets up the desired position and bar sizes for the graph drawn by the
            /// <see cref="M:CsDesktopPonies.SpriteManagement.AnimationLoopBase.FrameRecordCollector.DrawGraph"/> method. Returns the
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
            /// <summary>
            /// Releases all resources used by the <see cref="T:CsDesktopPonies.SpriteManagement.AnimationLoopBase.FrameRecordCollector"/>
            /// object.
            /// </summary>
            public void Dispose()
            {
                graphBackgroundBrush.Dispose();
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
                    Unpause();
            }
        }
        /// <summary>
        /// Gets a value indicating whether animation has been stopped.
        /// </summary>
        public bool Stopped { get; private set; }
        /// <summary>
        /// Used to pause the thread running the main loop. Signals false whilst paused, otherwise signals true.
        /// </summary>
        private ManualResetEvent running = new ManualResetEvent(true);
        /// <summary>
        /// Gets the total elapsed time that animation has been running.
        /// </summary>
        public TimeSpan ElapsedTime { get; private set; }

        /// <summary>
        /// Gets the collection of sprites.
        /// </summary>
        protected AsyncLinkedList<ISprite> Sprites { get; private set; }
        /// <summary>
        /// Gets the viewer for the sprite collection.
        /// </summary>
        protected ISpriteCollectionView Viewer { get; private set; }

        /// <summary>
        /// Holds information about the performance of the animator.
        /// </summary>
        private FrameRecordCollector performanceRecorder = new FrameRecordCollector(300);
        /// <summary>
        /// Output of performance summary information.
        /// </summary>
        private StringBuilder performanceSummary = new StringBuilder(100);
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

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.SpriteManagement.AnimationLoopBase"/> class handling the given
        /// collection of sprites.
        /// </summary>
        /// <param name="spriteViewer">The <see cref="T:CsDesktopPonies.SpriteManagement.ISpriteCollectionView"/> used to display the
        /// sprites.</param>
        /// <param name="spriteCollection">The initial collection of <see cref="T:CsDesktopPonies.SpriteManagement.ISprite"/> to be
        /// displayed by the animator, which may be null.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="spriteViewer"/> is null.</exception>
        protected AnimationLoopBase(ISpriteCollectionView spriteViewer, IEnumerable<ISprite> spriteCollection)
        {
            Argument.EnsureNotNull(spriteViewer, "spriteViewer");

            Viewer = spriteViewer;

#if DEBUG || true
            if (Viewer is WinFormSpriteInterface)
                ((WinFormSpriteInterface)Viewer).Collector = performanceRecorder;
            else if (Viewer is GtkSpriteInterface)
                ((GtkSpriteInterface)Viewer).Collector = performanceRecorder;
#endif

            // Create an asynchronous collection, so it can be safely exposed to derived classes.
            if (spriteCollection == null)
                Sprites = new AsyncLinkedList<ISprite>();
            else
                Sprites = new AsyncLinkedList<ISprite>(spriteCollection);

            // Whenever a new sprite is added, call Start on the sprite automatically.
            Sprites.ItemAdded += (sender, e) => e.Item.Start(ElapsedTime);
            Sprites.ItemsAdded += (sender, e) =>
            {
                foreach (var sprite in e.Items)
                    sprite.Start(ElapsedTime);
            };

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
        public virtual void Begin()
        {
            if (Started)
                throw new InvalidOperationException("Cannot start an animator that has already been started.");

            Console.WriteLine(GetType() + " is starting an animation loop...");
            Started = true;

            foreach (ISprite sprite in Sprites)
                sprite.Start(ElapsedTime);

            runner = new Thread(Run) { Name = "AnimationLoopBase.Run" };

            Viewer.Open();

            // Force a collection now, to clear the heap of any memory from loading. Assuming the loop makes little to no allocations, this
            // should ensure cheap and quick generation zero collections, and will delay the first collection as long as possible.
            GC.Collect();
            GC.WaitForPendingFinalizers();

            runner.Start();
        }

        /// <summary>
        /// Pauses animation, and optionally hides the animator while paused.
        /// </summary>
        /// <param name="hide">True to hide the animator, false to leave the animator visible in the paused state.</param>
        public virtual void Pause(bool hide)
        {
            if (!Started)
                throw new InvalidOperationException("Cannot pause an animator that has not been started.");

            if (Stopped)
                throw new InvalidOperationException("Cannot pause an animator that has been stopped.");

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
        /// Resumes animation, and shows the animator if it was hidden while paused.
        /// </summary>
        public virtual void Unpause()
        {
            if (!Started)
                throw new InvalidOperationException("Cannot resume an animator that has not been started.");

            if (Stopped)
                throw new InvalidOperationException("Cannot resume an animator that has been stopped.");

            if (Paused)
            {
                elapsedWatch.Start();
                intervalWatch.Start();

                Viewer.Show();
                Viewer.Unpause();

                running.Set();
            }
        }

        /// <summary>
        /// Runs frame cycles continuously and tracks animation timings.
        /// </summary>
        private void Run()
        {
            // Keeps a count of frames since performance information was last displayed.
            int performanceDelayCount = 0;
#if DEBUG
            // Tracks lost time from long delays when debugging the application.
            TimeSpan lostTime = TimeSpan.Zero;
#endif

            // Start timers to track total elapsed time and interval time.
            elapsedWatch.Start();
            intervalWatch.Start();

            // Loop whilst not disposed, paused or stopped.
            while (!Disposed && running.WaitOne() && !Stopped)
            {
                // Run an update and draw cycle for one frame.
                ElapsedTime = elapsedWatch.Elapsed;
                Tick();

#if DEBUG
                // When debugging, assume long delays are due to hitting breakpoints.
                TimeSpan tickElapsed = elapsedWatch.Elapsed - ElapsedTime;
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
                    performanceRecorder.OutputSummaryTo(performanceSummary);
                    Console.WriteLine(performanceSummary.ToString());
                    //Console.WriteLine(performanceRecorder.FrameRates + " " +
                    //                  performanceRecorder.FrameTimings + " " +
                    //                  performanceRecorder.FrameIntervals);
                }
            }
        }

        /// <summary>
        /// Runs an update and draw cycle.
        /// </summary>
        private void Tick()
        {
            Update();
            if (!Disposed)
                Draw();
        }

        /// <summary>
        /// Updates the sprites.
        /// </summary>
        protected virtual void Update()
        {
            foreach (ISprite sprite in Sprites)
                sprite.Update(ElapsedTime);
        }

        /// <summary>
        /// Draws the sprites.
        /// </summary>
        protected virtual void Draw()
        {
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
            if (disposing && !Stopped)
            {
                // Signal that animation has ended.
                Stopped = true;

                if (runner != null)
                {
                    // Un-pause so the main thread can gracefully exit now the signal to stop has been issued.
                    running.Set();
                    if (Thread.CurrentThread != runner)
                        runner.Join();
                    runner = null;
                }
                Viewer.Close();

                running.Dispose();
                performanceRecorder.Dispose();
            }
        }
    }
}

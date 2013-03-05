namespace CSDesktopPonies.DesktopPonies
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Globalization;
    using System.Windows.Forms;
    using CSDesktopPonies.SpriteManagement;

    /// <summary>
    /// Provides a control that allows the user to seek among the frames of an animated image.
    /// </summary>
    public partial class AnimatedImageIndexer : UserControl
    {
        /// <summary>
        /// Indicates whether the index is being changed.
        /// </summary>
        private bool updating;
        /// <summary>
        /// The durations of each frame.
        /// </summary>
        private int[] durations;
        /// <summary>
        /// A list of timings in the animation that mark the start and end of frames over the duration of the animation.
        /// </summary>
        private int[] sectionValues;
        /// <summary>
        /// Brushes used to draw sections for each frame.
        /// </summary>
        private static readonly Brush[] sectionBrushes = new Brush[] { Brushes.DarkGray, Brushes.LightGray };
        /// <summary>
        /// Brush used to draw the section of the currently selected frame.
        /// </summary>
        private Brush sectionHighlightBrush = Brushes.Red;

        /// <summary>
        /// Occurs when the frame or time index has been changed.
        /// </summary>
        [Description("Occurs when the frame or time index has been changed.")]
        public event EventHandler IndexChanged;

        /// <summary>
        /// Gets or sets the frame index.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FrameIndex
        {
            get
            {
                return durations == null ? -1 : FrameSelector.Value;
            }
            set
            {
                if (updating)
                    return;

                if (durations == null)
                    throw new InvalidOperationException("Cannot set the frame index until indexes have been defined.");

                if (value < FrameSelector.Minimum || value > FrameSelector.Maximum)
                    throw new ArgumentOutOfRangeException("value", value,
                        string.Format("value must be between {0} and {1} inclusive.", FrameSelector.Minimum,FrameSelector.Maximum));

                updating = true;
                FrameSelector.Value = value;

                int timeIndex = 0;
                for (int i = 0; i < value; i++)
                    timeIndex += durations[i];
                timeIndex += durations[value] / 2;
                TimeSelector.Value = timeIndex;

                OnIndexChanged();
                updating = false;
            }
        }

        /// <summary>
        /// Gets or sets the time index, in milliseconds.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TimeIndex
        {
            get
            {
                return durations == null ? -1 : TimeSelector.Value;
            }
            set
            {
                if (updating)
                    return;

                if (durations == null)
                    throw new InvalidOperationException("Cannot set the frame index until indexes have been defined.");

                if (value < TimeSelector.Minimum || value > TimeSelector.Maximum)
                    throw new ArgumentOutOfRangeException("value", value,
                        string.Format("value must be between {0} and {1} inclusive.", TimeSelector.Minimum, TimeSelector.Maximum));

                updating = true;
                TimeSelector.Value = value;

                int seekTime = durations[0];
                int frameIndex = 0;
                while (seekTime <= value && ++frameIndex < FrameSelector.Maximum)
                    seekTime += durations[frameIndex];
                FrameSelector.Value = frameIndex;

                OnIndexChanged();
                updating = false;
            }
        }

        /// <summary>
        /// Gets or sets the interval, in milliseconds, at which the time index is advanced when playback is active.
        /// </summary>
        [Description("The interval, in milliseconds, at which the time index is advanced when playback is active.")]
        [DefaultValue(50)]
        public int Step
        {
            get { return PlaybackTimer.Interval; }
            set { PlaybackTimer.Interval = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.DesktopPonies.AnimatedImageIndexer"/>
        /// </summary>
        public AnimatedImageIndexer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Uses durations from the specified image to set up the control for indexing.
        /// </summary>
        /// <typeparam name="T">The type of the frame images.</typeparam>
        /// <param name="image">The image whose durations should be used to specify frame and time indexes.</param>
        public void UseTimingsFrom<T>(GifImage<T> image)
        {
            PlaybackTimer.Stop();
            if (image == null)
            {
                durations = null;
                sectionValues = null;
                FrameSelector.Maximum = 0;
                TimeSelector.Maximum = 0;
                FrameLabel.Text = "";
                Enabled = false;
            }
            else
            {
                durations = new int[image.Frames.Count];
                sectionValues = new int[image.Frames.Count + 1];
                sectionValues[0] = 0;
                int accumulatedDuration = 0;
                for (int i = 0; i < image.Frames.Count; i++)
                {
                    int duration = image.Frames[i].Duration;
                    durations[i] = duration;
                    accumulatedDuration += duration;
                    sectionValues[i + 1] = accumulatedDuration;
                }
                FrameSelector.Maximum = image.Frames.Count - 1;
                TimeSelector.Maximum = image.Duration;
                UpdateLabel();
                Enabled = true;
            }
            TimeSelectorSections.Invalidate();
        }

        /// <summary>
        /// Updates the index summary label text.
        /// </summary>
        private void UpdateLabel()
        {
            FrameLabel.Text = string.Format(CultureInfo.CurrentCulture,
                "Frame: {0:00} of {1:00}  Time: {2:00.00} of {3:00.00} seconds",
                FrameIndex + 1, FrameSelector.Maximum + 1, TimeIndex / 1000f, TimeSelector.Maximum / 1000f);
        }

        /// <summary>
        /// Starts seeking through the indexes automatically.
        /// </summary>
        public void Play()
        {
            PlaybackTimer.Start();
        }

        /// <summary>
        /// Stops seeking through the indexes.
        /// </summary>
        public void Pause()
        {
            PlaybackTimer.Stop();
        }

        /// <summary>
        /// Raised when a frame is selected by the user.
        /// Updates the frame index.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Data about the event.</param>
        private void FrameSelector_ValueChanged(object sender, EventArgs e)
        {
            FrameIndex = FrameSelector.Value;
        }

        /// <summary>
        /// Raised when a time is selected by the user.
        /// Updates the time index.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">Data about the event.</param>
        private void TimeSelector_ValueChanged(object sender, EventArgs e)
        {
            TimeIndex = TimeSelector.Value;
        }

        /// <summary>
        /// Raised when PreviousCommand is clicked.
        /// Moves back one frame.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PreviousCommand_Click(object sender, EventArgs e)
        {
            int value = FrameIndex;
            if (--value < FrameSelector.Minimum)
                value = FrameSelector.Maximum;
            FrameIndex = value;
        }

        /// <summary>
        /// Raised when NextCommand is clicked.
        /// Moves forward one frame.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void NextCommand_Click(object sender, EventArgs e)
        {
            int value = FrameIndex;
            if (++value > FrameSelector.Maximum)
                value = FrameSelector.Minimum;
            FrameIndex = value;
        }

        /// <summary>
        /// Raised when PlayCommand is clicked.
        /// Toggles playback of the animation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PlayCommand_Click(object sender, EventArgs e)
        {
            PlaybackTimer.Enabled = !PlaybackTimer.Enabled;

            FrameSelector.Enabled = !PlaybackTimer.Enabled;
            TimeSelector.Enabled = !PlaybackTimer.Enabled;
            NextCommand.Enabled = !PlaybackTimer.Enabled;
            PreviousCommand.Enabled = !PlaybackTimer.Enabled;
            PlayCommand.Text = PlaybackTimer.Enabled ? "Pause" : "Play";
        }

        /// <summary>
        /// Raised when the playback timer ticks.
        /// Advances the current time index.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PlaybackTimer_Tick(object sender, EventArgs e)
        {
            int range = TimeSelector.Maximum - TimeSelector.Minimum;
            if (range != 0)
            {
                int value = TimeSelector.Value + PlaybackTimer.Interval;
                value %= range;
                TimeIndex = value;
            }
            else
            {
                TimeIndex = TimeSelector.Minimum;
            }
        }

        /// <summary>
        /// Raised when TimeSelectorSections is painted.
        /// Draws sections along the bar to mark the durations of each frame in sequence.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void TimeSelectorSections_Paint(object sender, PaintEventArgs e)
        {
            if (sectionValues == null)
                return;

            Graphics graphics = e.Graphics;

            int colorIndex = 0;
            float currentValue = GetRelativeTime(TimeSelector.Value);
            for (int section = 0; section < sectionValues.Length - 1; section++)
            {
                float min = GetRelativeTime(sectionValues[section]);
                float max = GetRelativeTime(sectionValues[section + 1]);

                Brush brush = sectionBrushes[colorIndex];
                if (currentValue >= min && (currentValue < max || (currentValue == 1 && currentValue == max)))
                    brush = sectionHighlightBrush;

                int width = TimeSelectorSections.Width;
                int height = TimeSelectorSections.Height;
                graphics.FillRectangle(brush, min * width, 0, (max - min) * width, height);

                if (++colorIndex >= sectionBrushes.Length)
                    colorIndex = 0;
            }
        }

        /// <summary>
        /// Gets the normalized value of the time into the animation.
        /// </summary>
        /// <param name="time">Absolute time into the animation, in milliseconds.</param>
        /// <returns>A value between 0 and 1 representing the time into the animation.</returns>
        private float GetRelativeTime(int time)
        {
            return (float)(time - TimeSelector.Minimum) / (TimeSelector.Maximum - TimeSelector.Minimum);
        }

        /// <summary>
        /// Raises the <see cref="E:CSDesktopPonies.DesktopPonies.AnimatedImageIndexer.IndexChanged"/> event.
        /// </summary>
        protected virtual void OnIndexChanged()
        {
            UpdateLabel();
            TimeSelectorSections.Invalidate();
            IndexChanged.Raise(this);
        }
    }
}

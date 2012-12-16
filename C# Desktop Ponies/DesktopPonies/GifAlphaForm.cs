namespace CsDesktopPonies.DesktopPonies
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Windows.Forms;
    using CsDesktopPonies.SpriteManagement;

    /// <summary>
    /// Allows an ARGB color table to be defined for gif files.
    /// </summary>
    public partial class GifAlphaForm : Form
    {
        /// <summary>
        /// The path to the gif image being modified.
        /// </summary>
        private string filePath;
        /// <summary>
        /// The gif image to display and modify.
        /// </summary>
        private GifImage<Bitmap> gifImage;
        /// <summary>
        /// Maintains the mapping table between source RGB colors and desired ARGB colors.
        /// </summary>
        private Dictionary<Color, Color> colorMappingTable = new Dictionary<Color, Color>();
        /// <summary>
        /// Color swatches that display the original palette.
        /// </summary>
        private List<Panel> sourceSwatches = new List<Panel>(0);
        /// <summary>
        /// Color swatches that display the modified palette.
        /// </summary>
        private List<Panel> desiredSwatches = new List<Panel>(0);
        /// <summary>
        /// The frames of <see cref="F:CsDesktopPonies.DesktopPonies.GifAlphaForm.gifImage"/>, altered to use the modified palette.
        /// </summary>
        private Bitmap[] desiredFrames;
        /// <summary>
        /// Indicates if an image is currently loaded and being displayed.
        /// </summary>
        private bool loaded;
        /// <summary>
        /// Indicates if the frame to be displayed is currently being changed. Controls need only set their values and not trigger further
        /// update events.
        /// </summary>
        private bool updating;
        /// <summary>
        /// Indicates if a change has been made, and thus saving is required.
        /// </summary>
        private bool changed;
        /// <summary>
        /// The index of the frame currently being displayed.
        /// </summary>
        private int frameIndex;
        /// <summary>
        /// The time index into the animation, in milliseconds.
        /// </summary>
        private int timeIndex;
        /// <summary>
        /// The current color in the source image that is currently being edited or otherwise modified.
        /// </summary>
        private Color sourceColor;
        /// <summary>
        /// A list of timings in the animation that mark the start and end of frames over the duration of the animation.
        /// </summary>
        private List<int> sectionValues = new List<int>();
        /// <summary>
        /// Brushes used to draw sections for each frame.
        /// </summary>
        private Brush[] sectionBrushes = new Brush[] { Brushes.DarkGray, Brushes.LightGray };
        /// <summary>
        /// Brush used to draw the section of the currently selected frame.
        /// </summary>
        private Brush sectionHighlightBrush = Brushes.Red;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.DesktopPonies.GifAlphaForm"/> class.
        /// </summary>
        public GifAlphaForm()
        {
            InitializeComponent();
            Icon = Properties.Resources.Twilight;

            Disposed += (sender, e) =>
            {
                if (gifImage != null)
                    foreach (GifFrame<Bitmap> frame in gifImage.Frames)
                        frame.Image.Dispose();
                if (desiredFrames != null)
                    foreach (Bitmap frame in desiredFrames)
                        frame.Dispose();
            };
        }

        /// <summary>
        /// Raised when the form has loaded.
        /// Gets a list of all gif files in <see cref="P:CsDesktopPonies.DesktopPonies.Program.PonyDirectory"/>, and sets up form controls.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GifAlphaForm_Load(object sender, EventArgs e)
        {
            ImageSelector.Items.AddRange(Directory.GetFiles(Program.PonyDirectory, "*.gif", SearchOption.AllDirectories));
            if (ImageSelector.Items.Count != 0)
                ImageSelector.SelectedIndex = 0;
            else
                MessageBox.Show(this,
                    string.Format(CultureInfo.CurrentCulture, "No .gif files found in {0} or its subdirectories.", Program.PonyDirectory),
                    "No Files", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //System.Threading.ThreadPool.QueueUserWorkItem(o =>
            //    ImageSelector.Invoke(new MethodInvoker(() =>
            //    {
            //        for (int i = 0; i < ImageSelector.Items.Count; i++)
            //            ImageSelector.SelectedIndex = i;
            //    })));
        }

        /// <summary>
        /// Raised when a new index is selected from ImageSelector.
        /// Loads the gif of that filename.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void ImageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!SavePrompt())
                return;

            loaded = false;
            changed = false;
            filePath = (string)ImageSelector.Items[ImageSelector.SelectedIndex];

            if (gifImage != null)
                foreach (GifFrame<Bitmap> frame in gifImage.Frames)
                    frame.Image.Dispose();

            if (desiredFrames != null)
                foreach (Bitmap frame in desiredFrames)
                    frame.Dispose();

            gifImage = null;
            desiredFrames = null;
            ImageComparison.Panel1.BackgroundImage = null;
            ImageComparison.Panel2.BackgroundImage = null;
            ImageSourceColor.BackColor = Color.Transparent;
            ImageDesiredColor.BackColor = Color.Transparent;
            ImageSourcePalette.BackColor = PaletteControls.BackColor;
            ImageDesiredPalette.BackColor = PaletteControls.BackColor;
            ImageSourcePalette.Controls.Clear();
            ImageDesiredPalette.Controls.Clear();
            foreach (Panel panel in sourceSwatches)
                panel.Dispose();
            foreach (Panel panel in desiredSwatches)
                panel.Dispose();
            sourceSwatches.Clear();
            desiredSwatches.Clear();
            SourceAlphaCode.ResetText();
            SourceColorCode.ResetText();
            DesiredAlphaCode.ResetText();
            DesiredColorCode.ResetText();
            FrameControls.Enabled = false;
            PaletteControls.Enabled = false;
            ColorControls.Enabled = false;
            FrameSelector.Maximum = 0;
            TimeSelector.Maximum = 0;
            TimeSelectorSections.Invalidate();
            FrameLabel.ResetText();
            ErrorLabel.Visible = false;
            sourceColor = Color.Empty;
            frameIndex = -1;
            timeIndex = 0;

            FileStream gifStream = null;
            try
            {
                gifStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                gifImage = GifImage.OfBitmap(gifStream);
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred attempting to load the file: " + filePath);
                ShowError("An error occurred attempting to load this file.");
                return;
            }
            finally
            {
                if (gifStream != null)
                    gifStream.Dispose();
            }

            sectionValues.Clear();
            int runningDuration = 0;
            sectionValues.Add(runningDuration);
            foreach (var frame in gifImage.Frames)
            {
                runningDuration += frame.Duration;
                sectionValues.Add(runningDuration);
            }

            FrameSelector.Maximum = gifImage.Frames.Count - 1;
            TimeSelector.Maximum = gifImage.Duration;

            AlphaRemappingTable map = new AlphaRemappingTable();
            string mapFile = Path.ChangeExtension(filePath, AlphaRemappingTable.FileExtension);
            if (File.Exists(mapFile))
                map.LoadMap(mapFile);

            colorMappingTable.Clear();

            foreach (var frame in gifImage.Frames)
                BuildColorMap(frame.GetColorTable(), map);

            sourceSwatches.Capacity = colorMappingTable.Count;
            desiredSwatches.Capacity = colorMappingTable.Count;

            int swatchSize = ImageSourcePalette.Height - 2;
            Size size = new Size(swatchSize, swatchSize);
            Point location = new Point(1, 1);

            ImageSourcePalette.SuspendLayout();
            ImageDesiredPalette.SuspendLayout();

            int mappingIndex = 0;
            foreach (var colorMapping in colorMappingTable)
            {
                Panel sourcePanel = new Panel() { Size = size, Location = location };
                Panel desiredPanel = new Panel() { Size = size, Location = location };
                sourcePanel.Tag = desiredPanel;
                desiredPanel.Tag = sourcePanel;
                sourceSwatches.Add(sourcePanel);
                desiredSwatches.Add(desiredPanel);
                ImageSourcePalette.Controls.Add(sourcePanel);
                ImageDesiredPalette.Controls.Add(desiredPanel);
                sourceSwatches[mappingIndex].Click += Swatch_Clicked;
                desiredSwatches[mappingIndex].Click += Swatch_Clicked;
                sourceSwatches[mappingIndex].BackColor = colorMapping.Key;
                desiredSwatches[mappingIndex].BackColor = colorMapping.Value;
                if (mappingIndex == 0)
                    sourceColor = colorMapping.Key;
                mappingIndex++;
                location.X += swatchSize + 1;
            }
            location.Y = 0;
            ImageSourcePalette.Controls.Add(
                new Panel() { Size = ImageSourcePalette.Size, Location = location, BackColor = SystemColors.Control });
            ImageDesiredPalette.Controls.Add(
                new Panel() { Size = ImageDesiredPalette.Size, Location = location, BackColor = SystemColors.Control });
            ImageSourcePalette.ResumeLayout();
            ImageDesiredPalette.ResumeLayout();
            ImageSourcePalette.BackColor = ImageComparison.BackColor;
            ImageDesiredPalette.BackColor = ImageComparison.BackColor;

            desiredFrames = new Bitmap[gifImage.Frames.Count];
            for (int i = 0; i < desiredFrames.Length; i++)
                desiredFrames[i] = (Bitmap)gifImage.Frames[i].Image.Clone();
            UpdateDesiredFrames();

            FrameControls.Enabled = true;
            PaletteControls.Enabled = true;
            ColorControls.Enabled = true;

            FrameSelector.Value = 0;
            TimeSelector.Value = 0;

            TimeSelectorSections.Invalidate();
            UpdateSelectedFrame(0);
            UpdateColorHex();
            UpdateColorDisplay();

            loaded = true;
        }

        /// <summary>
        /// Builds the color mapping by adding all colors in the given table, and also resolving lookups according to the given alpha map.
        /// </summary>
        /// <param name="colorTable">The colors that should be added to the current lookup mapping.</param>
        /// <param name="alphaMap">The alpha mapping that specifies new ARGB colors that should replace any given RGB colors in the color
        /// table.</param>
        private void BuildColorMap(ArgbColor[] colorTable, AlphaRemappingTable alphaMap)
        {
            foreach (ArgbColor sourceArgbColor in colorTable)
                if (sourceArgbColor.A == 255)
                {
                    RgbColor sourceRgbColor = (RgbColor)sourceArgbColor;
                    Color sourceColor = Color.FromArgb(sourceRgbColor.ToArgb());
                    if (!colorMappingTable.ContainsKey(sourceColor))
                    {
                        ArgbColor desiredArgbColor;
                        if (!alphaMap.TryGetMapping(sourceRgbColor, out desiredArgbColor))
                            desiredArgbColor = sourceArgbColor;
                        colorMappingTable.Add(sourceColor, Color.FromArgb(desiredArgbColor.ToArgb()));
                    }
                }
        }

        /// <summary>
        /// Displays an error message to the user about why the current image cannot be displayed.
        /// </summary>
        /// <param name="error">The message to display.</param>
        private void ShowError(string error)
        {
            ErrorLabel.Text = error;
            ErrorLabel.Visible = true;
        }

        /// <summary>
        /// Raised when the value of the FrameSelector is changed.
        /// Displays the frame of that index.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void FrameSelector_ValueChanged(object sender, EventArgs e)
        {
            if (!loaded || updating)
                return;

            updating = true;
            timeIndex = 0;
            int newFrameIndex = FrameSelector.Value;

            for (int i = 0; i < newFrameIndex; i++)
                timeIndex += gifImage.Frames[i].Duration;
            timeIndex += gifImage.Frames[newFrameIndex].Duration / 2;
            TimeSelector.Value = timeIndex;

            UpdateSelectedFrame(newFrameIndex);
            updating = false;
        }

        /// <summary>
        /// Raised when the value of TimeSelector is changed.
        /// Displays the frame for that time.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void TimeSelector_ValueChanged(object sender, EventArgs e)
        {
            TimeSelectorSections.Invalidate();

            if (!loaded || updating)
                return;

            updating = true;
            int newFrameIndex = 0;
            timeIndex = TimeSelector.Value;

            int seekTime = gifImage.Frames[newFrameIndex].Duration;
            while (seekTime <= timeIndex && ++newFrameIndex < gifImage.Frames.Count - 1)
                seekTime += gifImage.Frames[newFrameIndex].Duration;
            FrameSelector.Value = newFrameIndex;

            UpdateSelectedFrame(newFrameIndex);
            updating = false;
        }

        /// <summary>
        /// Raised when TimeSelectorSections is painted.
        /// Draws sections along the bar to mark the durations of each frame in sequence.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void TimeSelectorSections_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;

            int colorIndex = 0;
            float currentValue = GetRelativeTime(TimeSelector.Value);
            for (int section = 0; section < sectionValues.Count - 1; section++)
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
        /// Updates the display to a new frame.
        /// </summary>
        /// <param name="newFrameIndex">The index of the frame that should be displayed.</param>
        private void UpdateSelectedFrame(int newFrameIndex)
        {
            if (frameIndex != newFrameIndex)
            {
                frameIndex = newFrameIndex;
                ImageComparison.Panel1.BackgroundImage = gifImage.Frames[frameIndex].Image;
                ImageComparison.Panel2.BackgroundImage = desiredFrames[frameIndex];
                ImageComparison.Panel1.Invalidate();
                ImageComparison.Panel2.Invalidate();
            }

            FrameLabel.Text = string.Format(CultureInfo.CurrentCulture,
                "Frame: {0:00} of {1:00}  Time: {2:00.00} of {3:00.00} seconds",
                frameIndex + 1, gifImage.Frames.Count, timeIndex / 1000f, gifImage.Duration / 1000f);
        }

        /// <summary>
        /// Raised when PreviousCommand is clicked.
        /// Moves back one frame.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PreviousCommand_Click(object sender, EventArgs e)
        {
            int value = FrameSelector.Value;
            if (--value < FrameSelector.Minimum)
                value = FrameSelector.Maximum;
            FrameSelector.Value = value;
        }

        /// <summary>
        /// Raised when NextCommand is clicked.
        /// Moves forward one frame.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void NextCommand_Click(object sender, EventArgs e)
        {
            int value = FrameSelector.Value;
            if (++value > FrameSelector.Maximum)
                value = FrameSelector.Minimum;
            FrameSelector.Value = value;
        }

        /// <summary>
        /// Raised when PlayCommand is clicked.
        /// Toggles playback of the animation.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PlayCommand_Click(object sender, EventArgs e)
        {
            AnimationTimer.Enabled = !AnimationTimer.Enabled;

            FrameSelector.Enabled = !AnimationTimer.Enabled;
            TimeSelector.Enabled = !AnimationTimer.Enabled;
            NextCommand.Enabled = !AnimationTimer.Enabled;
            PreviousCommand.Enabled = !AnimationTimer.Enabled;
            PlayCommand.Text = AnimationTimer.Enabled ? "Pause" : "Play";
        }

        /// <summary>
        /// Raised when AnimationTimer ticks.
        /// Advances the current time index of the animation being played.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (!loaded)
                return;

            int range = TimeSelector.Maximum - TimeSelector.Minimum;
            if (range != 0)
            {
                int value = TimeSelector.Value + AnimationTimer.Interval;
                while (value >= TimeSelector.Maximum)
                    value -= range;
                TimeSelector.Value = value;
            }
            else
            {
                TimeSelector.Value = TimeSelector.Minimum;
            }
        }

        /// <summary>
        /// Raised when a color swatch is clicked.
        /// Displays hex values for color.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void Swatch_Clicked(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            if (panel.Parent == ImageSourcePalette)
                sourceColor = panel.BackColor;
            else
                sourceColor = ((Panel)panel.Tag).BackColor;
            UpdateColorHex();
            UpdateColorDisplay();
        }

        /// <summary>
        /// Raised when ImageDesiredColor is clicked.
        /// Allows the user to edit the desired color, for the color currently being edited.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void ImageDesiredColor_Click(object sender, EventArgs e)
        {
            changed = true;
            ColorDialog.Color = colorMappingTable[sourceColor];
            ColorDialog.ShowDialog(this);
            colorMappingTable[sourceColor] = Color.FromArgb(colorMappingTable[sourceColor].A, ColorDialog.Color);
            UpdateColorHex();
            UpdateColorDisplay();
        }

        /// <summary>
        /// Updates the controls that display the hexadecimal codes for the color being edited.
        /// </summary>
        private void UpdateColorHex()
        {
            SourceAlphaCode.Text = string.Format(CultureInfo.CurrentCulture, "{0:X2}", sourceColor.A);
            SourceColorCode.Text = string.Format(CultureInfo.CurrentCulture, "{0:X6}", sourceColor.ToArgb() & 0x00FFFFFF);
            DesiredAlphaCode.Text = string.Format(CultureInfo.CurrentCulture, "{0:X2}", colorMappingTable[sourceColor].A);
            DesiredColorCode.Text =
                string.Format(CultureInfo.CurrentCulture, "{0:X6}", colorMappingTable[sourceColor].ToArgb() & 0x00FFFFFF);
        }

        /// <summary>
        /// Updates the controls that display the color being edited.
        /// </summary>
        private void UpdateColorDisplay()
        {
            ImageSourceColor.BackColor = sourceColor;
            ImageDesiredColor.BackColor = colorMappingTable[sourceColor];
        }

        /// <summary>
        /// Raised when the text of DesiredAlphaCode is changed.
        /// Attempts to parse the new alpha code and modifies the image accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void DesiredAlphaCode_TextChanged(object sender, EventArgs e)
        {
            if (!loaded)
                return;

            byte value;
            if (byte.TryParse(DesiredAlphaCode.Text, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out value))
            {
                DesiredAlphaCode.ForeColor = Color.Black;
                Color newColor = Color.FromArgb(value, colorMappingTable[sourceColor]);
                if (colorMappingTable[sourceColor] != newColor)
                {
                    colorMappingTable[sourceColor] = newColor;
                    changed = true;
                }
                foreach (Panel sourcePanel in ImageSourcePalette.Controls)
                    if (sourcePanel.BackColor == sourceColor && sourcePanel.Tag != null)
                    {
                        ((Panel)sourcePanel.Tag).BackColor = colorMappingTable[sourceColor];
                        break;
                    }
                UpdateColorDisplay();
                UpdateDesiredFrames();
                ImageComparison.Panel2.Invalidate();
            }
            else
            {
                DesiredAlphaCode.ForeColor = Color.DarkRed;
            }
        }

        /// <summary>
        /// Raised when the text of DesiredColorCode is changed.
        /// Attempts to parse the new color code and modifies the image accordingly.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void DesiredColorCode_TextChanged(object sender, EventArgs e)
        {
            if (!loaded)
                return;

            int value;
            if (int.TryParse(DesiredColorCode.Text, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out value))
            {
                DesiredColorCode.ForeColor = Color.Black;
                Color newColor = Color.FromArgb(colorMappingTable[sourceColor].A, value >> 16, value >> 8 & 0xFF, value & 0xFF);
                if (colorMappingTable[sourceColor] != newColor)
                {
                    colorMappingTable[sourceColor] = newColor;
                    changed = true;
                }
                foreach (Panel sourcePanel in ImageSourcePalette.Controls)
                    if (sourcePanel.BackColor == sourceColor && sourcePanel.Tag != null)
                    {
                        ((Panel)sourcePanel.Tag).BackColor = colorMappingTable[sourceColor];
                        break;
                    }
                UpdateColorDisplay();
                UpdateDesiredFrames();
                ImageComparison.Panel2.Invalidate();
            }
            else
            {
                DesiredColorCode.ForeColor = Color.DarkRed;
            }
        }

        /// <summary>
        /// Updates all the modified images to use the desired palette.
        /// </summary>
        private void UpdateDesiredFrames()
        {
            for (int frame = 0; frame < gifImage.Frames.Count; frame++)
            {
                int tableSize = gifImage.Frames[frame].ColorTableSize;
                ColorPalette palette = gifImage.Frames[frame].Image.Palette;
                for (int i = 0; i < tableSize; i++)
                    if (palette.Entries[i].A == 255)
                        palette.Entries[i] = colorMappingTable[palette.Entries[i]];
                desiredFrames[frame].Palette = palette;
                desiredFrames[frame].PreMultiplyAlpha();
            }
        }

        /// <summary>
        /// Raised when ResetCommand_Click is clicked.
        /// Clears the current mapping.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void ResetCommand_Click(object sender, EventArgs e)
        {
            bool nonEmptyMap = false;
            foreach (var colorMapping in colorMappingTable)
                if (colorMapping.Key != colorMapping.Value)
                {
                    nonEmptyMap = true;
                    break;
                }
            if (nonEmptyMap &&
                MessageBox.Show(this,
                    "Are you sure you want to reset the mapping? You can still decline to save later.", "Confirm",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                Color[] keys = new Color[colorMappingTable.Keys.Count];
                colorMappingTable.Keys.CopyTo(keys, 0);
                foreach (Color color in keys)
                    colorMappingTable[color] = Color.FromArgb(255, color);
                changed = true;
            }
        }

        /// <summary>
        /// Raised when SaveCommand is clicked.
        /// Saves the mapping to file.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void SaveCommand_Click(object sender, EventArgs e)
        {
            SaveMapping();
        }

        /// <summary>
        /// Saves the mapping of the original RGB colors to modified ARGB colors to file.
        /// </summary>
        private void SaveMapping()
        {
            AlphaRemappingTable map = new AlphaRemappingTable();
            foreach (var colorMapping in colorMappingTable)
                if (colorMapping.Key != colorMapping.Value)
                    map.AddMapping(
                        new RgbColor(colorMapping.Key.R, colorMapping.Key.G, colorMapping.Key.B),
                        new ArgbColor(colorMapping.Value.A, colorMapping.Value.R, colorMapping.Value.G, colorMapping.Value.B));

            string mapFilePath = Path.ChangeExtension(filePath, AlphaRemappingTable.FileExtension);
            if (map.SaveMap(mapFilePath))
                MessageBox.Show(this, "Lookup mapping saved to '" + mapFilePath + "'",
                    "Mapping Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(this, "Source and destination colors match. Mapping file '" + mapFilePath + "' deleted.",
                    "Mapping Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
            changed = false;
        }

        /// <summary>
        /// Prompts the user to save any outstanding changes, if required, and returns a value indicating whether the caller can continue.
        /// </summary>
        /// <returns>A value indicating whether the caller should continue. Returns true to indicate it is ok to proceed, returns false to
        /// indicate the user wishes to review the current changes.</returns>
        private bool SavePrompt()
        {
            if (changed)
            {
                DialogResult result = MessageBox.Show(this, "You have unsaved changes. Save now?", "Unsaved Changes",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1);
                if (result == DialogResult.Yes)
                {
                    SaveMapping();
                    return true;
                }
                else if (result == DialogResult.No)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Raised when BackgroundColorCommand is clicked.
        /// Allows the user to change the background color that the images and swatches are displayed upon.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void BackgroundColorCommand_Click(object sender, EventArgs e)
        {
            ColorDialog.Color = ImageComparison.BackColor;
            ColorDialog.ShowDialog(this);
            ImageComparison.BackColor = ColorDialog.Color;
            ImageColors.BackColor = ColorDialog.Color;
            ImageSourcePalette.BackColor = ColorDialog.Color;
            ImageDesiredPalette.BackColor = ColorDialog.Color;
        }

        /// <summary>
        /// Raised when either panel on ImageComparison is clicked.
        /// Picks the color under the cursor for editing.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void ImageComparison_Panel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            Panel panel = (Panel)sender;

            int imageWidth = gifImage.Frames[frameIndex].Image.Width;
            int imageHeight = gifImage.Frames[frameIndex].Image.Height;
            Point location = e.Location;
            location -= new Size(panel.Width / 2, panel.Height / 2);
            location += new Size(imageWidth / 2, imageHeight / 2);

            if (location.X >= 0 && location.Y >= 0 && location.X < imageWidth && location.Y < imageHeight)
            {
                IEnumerable<Color> colors;
                if (sender == ImageComparison.Panel1)
                    colors = colorMappingTable.Keys;
                else
                    colors = colorMappingTable.Values;

                Color pixel = gifImage.Frames[frameIndex].Image.GetPixel(location.X, location.Y);
                foreach (Color color in colors)
                    // GetPixel always returns a color with binary alpha. This comparison relaxes the alpha comparison to work around this,
                    // but can lead to incorrect picks when two desired colors have the same RGB values but different alpha.
                    if (pixel.A == 255 && color.R == pixel.R && color.G == pixel.G && color.B == pixel.B)
                    {
                        sourceColor = pixel;
                        UpdateColorHex();
                        UpdateColorDisplay();
                        break;
                    }
            }
        }

        /// <summary>
        /// Raised when either image palette is resized.
        /// Resizes the last panel to cover unused area which would otherwise contain swatches.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void ImagePalette_Resize(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            if (panel.Controls.Count != 0)
                panel.Controls[panel.Controls.Count - 1].Width = panel.Width;
        }

        /// <summary>
        /// Raised when the form is closing.
        /// Checks if a save is required.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GifAlphaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = !SavePrompt();
        }
    }
}

namespace DesktopSprites.Forms
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using DesktopSprites.Collections;
    using DesktopSprites.Core;
    using DesktopSprites.SpriteManagement;

    /// <summary>
    /// Displays the individual frames and other information about gif files.
    /// </summary>
    public partial class GifFramesForm : Form
    {
        /// <summary>
        /// Location of directory from which to load GIF files.
        /// </summary>
        private readonly string filesPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.Forms.GifForm"/> class.
        /// </summary>
        /// <param name="path">The path to a directory from which GIF files should be loaded.</param>
        public GifFramesForm(string path)
        {
            InitializeComponent();
            filesPath = Argument.EnsureNotNull(path, "path");
        }

        /// <summary>
        /// Raised when the form has loaded.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GifForm_Load(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(LoadInternal));
        }

        /// <summary>
        /// Gets the collection of GIF files to be accessed.
        /// </summary>
        private void LoadInternal()
        {
            ImageSelector.Items.AddRange(
                Directory.GetFiles(filesPath, "*.gif", SearchOption.AllDirectories)
                .Select(path => path.Substring(filesPath.Length + 1))
                .ToArray());
            if (ImageSelector.Items.Count != 0)
                ImageSelector.SelectedIndex = 0;
            else
                MessageBox.Show(this,
                    string.Format(CultureInfo.CurrentCulture, "No .gif files found in {0} or its subdirectories.", filesPath),
                    "No Files", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Loads a gif file from the given path, and displays the resulting frames.
        /// </summary>
        /// <param name="path">The path to load the gif file from.</param>
        private void LoadGif(string path)
        {
            FramesDisplayPanel.SuspendLayout();

            // Remove current panels.
            foreach (GifControl gc in FramesDisplayPanel.Controls)
                gc.Dispose();
            FramesDisplayPanel.Controls.Clear();

            GifImage<BitmapFrame> gifImage = null;
            try
            {
                using (FileStream gifStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    gifImage = new GifImage<BitmapFrame>(gifStream, BitmapFrame.FromBuffer, BitmapFrame.AllowableBitDepths);
            }
            catch (Exception)
            {
                // Couldn't load gif, don't display.
                ImageInfo.Text = "Unable to load gif.";
                FramesDisplayPanel.ResumeLayout();
                return;
            }

            ImageInfo.Text = string.Format(CultureInfo.CurrentCulture,
                "Iterations: {0}  Size: {1}", gifImage.Iterations, gifImage.Size);

            ImmutableArray<GifFrame<BitmapFrame>> frames = gifImage.Frames;
            for (int i = 0; i < frames.Length; i++)
            {
                string info = string.Format(CultureInfo.CurrentCulture,
                    "{0}: {1}ms", i + 1, frames[i].Duration);
                GifControl gc = new GifControl(frames[i], info);
                FramesDisplayPanel.Controls.Add(gc);
            }

            FramesDisplayPanel.ResumeLayout();
        }

        /// <summary>
        /// Raised when a new index is selected from ImageSelector.
        /// Loads the gif of that filename.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void ImageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGif(Path.Combine(filesPath, (string)ImageSelector.Items[ImageSelector.SelectedIndex]));
        }

        /// <summary>
        /// Raised when the form is closed.
        /// Performs cleanup.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GifForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (GifControl gc in FramesDisplayPanel.Controls)
                gc.Dispose();
        }
    }
}
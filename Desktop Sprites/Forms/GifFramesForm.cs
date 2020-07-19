namespace DesktopSprites.Forms
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;
    using DesktopSprites.Core;
    using DesktopSprites.SpriteManagement;

    /// <summary>
    /// Displays the individual frames and other information about GIF files.
    /// </summary>
    public partial class GifFramesForm : Form
    {
        /// <summary>
        /// Location of directory from which to load GIF files.
        /// </summary>
        private readonly string filesPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:DesktopSprites.Forms.GifFramesForm"/> class.
        /// </summary>
        /// <param name="path">The path to a directory from which GIF files should be loaded.</param>
        public GifFramesForm(string path)
        {
            InitializeComponent();
            filesPath = Argument.EnsureNotNull(path, nameof(path));
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
                    "No .gif files found in {0} or its subdirectories.".FormatWith(filesPath),
                    "No Files", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Loads a GIF file from the given path, and displays the resulting frames.
        /// </summary>
        /// <param name="path">The path to load the GIF file from.</param>
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
                using (var gifStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                    gifImage = new GifImage<BitmapFrame>(gifStream, BitmapFrame.FromBuffer, BitmapFrame.AllowableBitDepths);
            }
            catch (Exception)
            {
                // Couldn't load GIF, don't display.
                ImageInfo.Text = "Unable to load gif.";
                FramesDisplayPanel.ResumeLayout();
                return;
            }

            ImageInfo.Text =
                "Duration: {0:0.00s} Iterations: {1}  Size: {2}".FormatWith(
                TimeSpan.FromMilliseconds(gifImage.Duration).TotalSeconds, gifImage.Iterations, gifImage.Size);

            var frames = gifImage.Frames;
            for (var i = 0; i < frames.Length; i++)
            {
                var info = "{0}: {1}ms".FormatWith(i + 1, frames[i].Duration);
                var gc = new GifControl(frames[i], info);
                FramesDisplayPanel.Controls.Add(gc);
            }

            FramesDisplayPanel.ResumeLayout();
        }

        /// <summary>
        /// Raised when a new index is selected from ImageSelector.
        /// Loads the GIF file of that filename.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void ImageSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadGif(Path.Combine(filesPath, (string)ImageSelector.Items[ImageSelector.SelectedIndex]));
        }

        /// <summary>
        /// Raised when the form is disposed.
        /// Performs cleanup.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void GifForm_Disposed(object sender, EventArgs e)
        {
            foreach (GifControl gc in FramesDisplayPanel.Controls)
                gc.Dispose();
        }
    }
}

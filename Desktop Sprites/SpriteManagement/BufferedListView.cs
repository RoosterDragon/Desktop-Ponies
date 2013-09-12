namespace DesktopSprites.SpriteManagement
{
    using System.Windows.Forms;

    /// <summary>
    /// Represents a Windows list view control, which displays a collection of items that can be displayed using one of four different
    /// views. Double buffering is enabled.
    /// </summary>
    public class BufferedListView : ListView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DesktopSprites.SpriteManagement.BufferedListView"/> class.
        /// </summary>
        public BufferedListView()
        {
            DoubleBuffered = true;
        }
    }
}

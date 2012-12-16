namespace CsDesktopPonies.DesktopPonies
{
    using System;
    using System.Windows.Forms;
    using CsDesktopPonies.SpriteManagement;

    /// <summary>
    /// Custom control that displays one pony template and allows the user to choose the number of them to have.
    /// </summary>
    public partial class PonySelectionControl : UserControl
    {
        /// <summary>
        /// Gets the name of the pony displayed to the user.
        /// </summary>
        public string PonyName { get; private set; }
        /// <summary>
        /// Gets the <see cref="T:CsDesktopPonies.SpriteManagement.AnimatedImage`1"/> that displays the pony selection image.
        /// </summary>
        public AnimatedImage<BitmapFrame> PonyImage { get; private set; }
        /// <summary>
        /// Gets a value indicating whether the <see cref="P:CsDesktopPonies.DesktopPonies.PonySelectionControl.PonyImage"/> should be
        /// flipped to display in the desired direction.
        /// </summary>
        public bool FlipImage { get; private set; }
        /// <summary>
        /// Gets the time index into the animated image which determines the frame to be displayed.
        /// </summary>
        public TimeSpan TimeIndex { get; private set; }
        /// <summary>
        /// The count of this template that has been requested.
        /// </summary>
        private int ponyCount = 0;
        /// <summary>
        /// Gets or sets the count of this template that has been requested.
        /// </summary>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The operation was set and <paramref name="value"/> was negative.
        /// </exception>
        public int PonyCount
        {
            get { return ponyCount; }
            set { SetCount(value, true); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.DesktopPonies.PonySelectionControl"/> class, displaying the pony
        /// to the user.
        /// </summary>
        /// <param name="name">The name of the pony, which will be displayed to the user.</param>
        /// <param name="image">The image of the pony, which will be displayed to the user.</param>
        /// <param name="flipImage">Indicates if image should be flipped to display correctly.</param>
        public PonySelectionControl(string name, AnimatedImage<BitmapFrame> image, bool flipImage)
        {
            // Load designer components.
            InitializeComponent();

            PonyName = name;
            NameInfo.Text = name;
            PonyImage = image;
            FlipImage = flipImage;
        }

        /// <summary>
        /// Advances the time index by the given amount, and causes the control to be redrawn as required.
        /// </summary>
        /// <param name="amount">The amount by which to advance the time.</param>
        public void AdvanceTimeIndex(TimeSpan amount)
        {
            if (amount <= TimeSpan.Zero)
                throw new ArgumentOutOfRangeException("amount", amount, "amount must be positive");

            BitmapFrame oldImage = PonyImage[TimeIndex];
            TimeIndex += amount;
            BitmapFrame newImage = PonyImage[TimeIndex];
            if (oldImage != newImage)
                Invalidate();
        }

        /// <summary>
        /// Raised when the control needs to be redrawn.
        /// Displays the pony image on the control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonySelectionControl_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            PonyImage[TimeIndex].Flip(FlipImage);
            e.Graphics.DrawImageUnscaled(PonyImage[TimeIndex].Image, Width - Margin.Right - PonyImage.Width, Margin.Top);
        }

        /// <summary>
        /// Raised when a selection button is clicked.
        /// Sets the count, based on the sender.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void SetCount_Click(object sender, EventArgs e)
        {
            if (sender == SetCount0Command)
                SetCount(0, true);
            else if (sender == SetCount1Command)
                SetCount(1, true);
        }

        /// <summary>
        /// Raised when the text of CountField is changed.
        /// The text is parsed into a numeric count if valid, or else the change is reverted.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void CountField_TextChanged(object sender, EventArgs e)
        {
            ushort newCount;
            if (string.IsNullOrEmpty(CountField.Text))
            {
                SetCount(0, false);
            }
            else if (ushort.TryParse(CountField.Text, out newCount))
            {
                const int MaxCount = 10000;
                if (newCount > MaxCount)
                    SetCount(MaxCount, true);
                else
                    SetCount(newCount, false);
            }
            else
            {
                SetCount(ponyCount, true);
            }
        }

        /// <summary>
        /// Occurs when the control loses focus.
        /// Replaces an empty text field with "0".
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event data.</param>
        private void PonySelectionControl_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CountField.Text))
                SetCount(0, true);
        }

        /// <summary>
        /// Sets the count of instances for this template.
        /// </summary>
        /// <param name="count">The number to be set.</param>
        /// <param name="updateText">Indicates if the text field should be updated.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="count"/> is negative.</exception>
        private void SetCount(int count, bool updateText)
        {
            Argument.EnsureNonnegative(count, "count");

            if (updateText)
                CountField.Text = count.ToString(System.Globalization.CultureInfo.CurrentCulture);

            if (count != ponyCount)
            {
                int oldCount = ponyCount;
                ponyCount = count;
                OnCountChanged(new CountChangedEventArgs(count, count - oldCount));
            }
        }

        /// <summary>
        /// Occurs when the count of pony instances is changed for this control.
        /// </summary>
        public event EventHandler<CountChangedEventArgs> CountChanged;

        /// <summary>
        /// Raises the <see cref="E:CsDesktopPonies.DesktopPonies.PonySelectionControl.CountChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:CsDesktopPonies.DesktopPonies.CountChangedEventArgs"/> that contains the event data.</param>
        protected virtual void OnCountChanged(CountChangedEventArgs e)
        {
            CountChanged.Raise(this, e);
        }
    }

    #region CountChangedEventArgs class
    /// <summary>
    /// Provides data for the <see cref="E:CsDesktopPonies.DesktopPonies.PonySelectionControl.CountChanged"/> event.
    /// </summary>
    public class CountChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new value for count.
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// Gets the change in the value for count.
        /// </summary>
        public int Change { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CsDesktopPonies.DesktopPonies.CountChangedEventArgs"/> class.
        /// </summary>
        /// <param name="count">The new count for the control.</param>
        /// <param name="change">The change in the count for the control.</param>
        public CountChangedEventArgs(int count, int change)
        {
            Count = count;
            Change = change;
        }
    }
    #endregion
}

namespace CSDesktopPonies.SpriteManagement
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;

    /// <summary>
    /// Provides a list of current sprite information.
    /// </summary>
    public partial class SpriteInfoForm : Form
    {
        private const int MaxColumns = 5;
        private const int MaxItems = 30;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:CSDesktopPonies.SpriteManagement.SpriteAnimationDetailsForm"/> class.
        /// </summary>
        public SpriteInfoForm()
        {
            InitializeComponent();
            for (int i = 0; i < MaxColumns; i++)
                SpriteListView.Columns.Add(new ColumnHeader());
            for (int i = 0; i < MaxItems; i++)
            {
                string[] columns = new string[MaxColumns];
                for (int c = 0; c < MaxColumns; c++)
                    columns[c] = "";
                SpriteListView.Items.Add(new ListViewItem(columns));
            }
        }

        /// <summary>
        /// Updates the sprite information.
        /// </summary>
        /// <param name="columns">The names of the columns.</param>
        /// <param name="details">The details for each item.</param>
        public void RefreshInfo(IList<string> columns, IList<IList<string>> details)
        {
            SpriteListView.Invoke(new MethodInvoker(() =>
            {
                SpriteListView.BeginUpdate();

                for (int i = 0; i < MaxColumns; i++)
                    SpriteListView.Columns[i].Text = i < columns.Count ? columns[i] : "";

                for (int i = 0; i < MaxItems; i++)
                {
                    ListViewItem.ListViewSubItemCollection subItems = SpriteListView.Items[i].SubItems;
                    for (int j = 0; j < MaxColumns; j++)
                        subItems[j].Text = i < details.Count && j < columns.Count ? details[i][j] : "";
                }

                SpriteListView.EndUpdate();
            }));
        }

        private void SpriteInfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
                e.Cancel = true;
        }
    }
}

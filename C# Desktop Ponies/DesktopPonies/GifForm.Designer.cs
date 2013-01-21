namespace CSDesktopPonies.DesktopPonies
{
    partial class GifForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FramesDisplayPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.ImageSelector = new System.Windows.Forms.ComboBox();
            this.ControlsContainer = new System.Windows.Forms.Panel();
            this.ImageInfo = new System.Windows.Forms.Label();
            this.ControlsContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // FramesDisplayPanel
            // 
            this.FramesDisplayPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FramesDisplayPanel.AutoScroll = true;
            this.FramesDisplayPanel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.FramesDisplayPanel.Location = new System.Drawing.Point(12, 55);
            this.FramesDisplayPanel.Name = "FramesDisplayPanel";
            this.FramesDisplayPanel.Size = new System.Drawing.Size(710, 445);
            this.FramesDisplayPanel.TabIndex = 1;
            // 
            // ImageSelector
            // 
            this.ImageSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ImageSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ImageSelector.FormattingEnabled = true;
            this.ImageSelector.Location = new System.Drawing.Point(15, 10);
            this.ImageSelector.Name = "ImageSelector";
            this.ImageSelector.Size = new System.Drawing.Size(707, 21);
            this.ImageSelector.TabIndex = 1;
            this.ImageSelector.SelectedIndexChanged += new System.EventHandler(this.ImageSelector_SelectedIndexChanged);
            // 
            // ControlsContainer
            // 
            this.ControlsContainer.Controls.Add(this.ImageInfo);
            this.ControlsContainer.Controls.Add(this.ImageSelector);
            this.ControlsContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.ControlsContainer.Location = new System.Drawing.Point(0, 0);
            this.ControlsContainer.Name = "ControlsContainer";
            this.ControlsContainer.Size = new System.Drawing.Size(734, 49);
            this.ControlsContainer.TabIndex = 0;
            // 
            // ImageInfo
            // 
            this.ImageInfo.AutoSize = true;
            this.ImageInfo.Location = new System.Drawing.Point(12, 32);
            this.ImageInfo.Name = "ImageInfo";
            this.ImageInfo.Size = new System.Drawing.Size(0, 13);
            this.ImageInfo.TabIndex = 2;
            // 
            // GifForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 512);
            this.Controls.Add(this.ControlsContainer);
            this.Controls.Add(this.FramesDisplayPanel);
            this.MinimumSize = new System.Drawing.Size(600, 400);
            this.Name = "GifForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gif Viewer - C# Desktop Ponies";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GifForm_FormClosed);
            this.Load += new System.EventHandler(this.GifForm_Load);
            this.ControlsContainer.ResumeLayout(false);
            this.ControlsContainer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel FramesDisplayPanel;
        private System.Windows.Forms.ComboBox ImageSelector;
        private System.Windows.Forms.Panel ControlsContainer;
        private System.Windows.Forms.Label ImageInfo;
    }
}
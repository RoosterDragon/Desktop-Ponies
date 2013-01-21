namespace CSDesktopPonies.DesktopPonies
{
    partial class GifControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FrameInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // FrameInfo
            // 
            this.FrameInfo.AutoSize = true;
            this.FrameInfo.BackColor = System.Drawing.SystemColors.Control;
            this.FrameInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.FrameInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.FrameInfo.Location = new System.Drawing.Point(0, 135);
            this.FrameInfo.Name = "FrameInfo";
            this.FrameInfo.Size = new System.Drawing.Size(2, 15);
            this.FrameInfo.TabIndex = 0;
            // 
            // GifControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.FrameInfo);
            this.Name = "GifControl";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.GifControl_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label FrameInfo;
    }
}

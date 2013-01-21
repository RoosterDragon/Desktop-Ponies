namespace CSDesktopPonies.SpriteManagement
{
    partial class SpriteInfoForm
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
            this.SpriteListView = new CSDesktopPonies.SpriteManagement.BufferedListView();
            this.GeneralLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SpriteListView
            // 
            this.SpriteListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SpriteListView.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpriteListView.Location = new System.Drawing.Point(15, 25);
            this.SpriteListView.Name = "SpriteListView";
            this.SpriteListView.Size = new System.Drawing.Size(710, 225);
            this.SpriteListView.TabIndex = 0;
            this.SpriteListView.UseCompatibleStateImageBehavior = false;
            this.SpriteListView.View = System.Windows.Forms.View.Details;
            // 
            // GeneralLabel
            // 
            this.GeneralLabel.AutoSize = true;
            this.GeneralLabel.Location = new System.Drawing.Point(12, 9);
            this.GeneralLabel.Name = "GeneralLabel";
            this.GeneralLabel.Size = new System.Drawing.Size(44, 13);
            this.GeneralLabel.TabIndex = 1;
            this.GeneralLabel.Text = "General";
            // 
            // SpriteInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 262);
            this.Controls.Add(this.GeneralLabel);
            this.Controls.Add(this.SpriteListView);
            this.DoubleBuffered = true;
            this.Name = "SpriteInfoForm";
            this.Text = "Sprite Details";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpriteInfoForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CSDesktopPonies.SpriteManagement.BufferedListView SpriteListView;
        private System.Windows.Forms.Label GeneralLabel;
    }
}
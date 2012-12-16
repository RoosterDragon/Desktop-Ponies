namespace CsDesktopPonies.DesktopPonies
{
    partial class PonySelectionControl
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
            this.NameInfo = new System.Windows.Forms.Label();
            this.SetCount0Command = new System.Windows.Forms.Button();
            this.SetCount1Command = new System.Windows.Forms.Button();
            this.CountField = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // NameInfo
            // 
            this.NameInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.NameInfo.AutoSize = true;
            this.NameInfo.Location = new System.Drawing.Point(3, 102);
            this.NameInfo.Name = "NameInfo";
            this.NameInfo.Size = new System.Drawing.Size(35, 13);
            this.NameInfo.TabIndex = 0;
            this.NameInfo.Text = "Name";
            // 
            // SetCount0Command
            // 
            this.SetCount0Command.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetCount0Command.AutoSize = true;
            this.SetCount0Command.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SetCount0Command.Location = new System.Drawing.Point(3, 118);
            this.SetCount0Command.Margin = new System.Windows.Forms.Padding(3, 3, 1, 3);
            this.SetCount0Command.Name = "SetCount0Command";
            this.SetCount0Command.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.SetCount0Command.Size = new System.Drawing.Size(29, 23);
            this.SetCount0Command.TabIndex = 1;
            this.SetCount0Command.Text = "0";
            this.SetCount0Command.UseVisualStyleBackColor = true;
            this.SetCount0Command.Click += new System.EventHandler(this.SetCount_Click);
            // 
            // SetCount1Command
            // 
            this.SetCount1Command.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SetCount1Command.AutoSize = true;
            this.SetCount1Command.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SetCount1Command.Location = new System.Drawing.Point(34, 118);
            this.SetCount1Command.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.SetCount1Command.Name = "SetCount1Command";
            this.SetCount1Command.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.SetCount1Command.Size = new System.Drawing.Size(29, 23);
            this.SetCount1Command.TabIndex = 2;
            this.SetCount1Command.Text = "1";
            this.SetCount1Command.UseVisualStyleBackColor = true;
            this.SetCount1Command.Click += new System.EventHandler(this.SetCount_Click);
            // 
            // CountField
            // 
            this.CountField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CountField.Location = new System.Drawing.Point(67, 120);
            this.CountField.MaxLength = 6;
            this.CountField.Name = "CountField";
            this.CountField.Size = new System.Drawing.Size(41, 20);
            this.CountField.TabIndex = 3;
            this.CountField.Text = "0";
            this.CountField.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.CountField.TextChanged += new System.EventHandler(this.CountField_TextChanged);
            // 
            // PonySelectionControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.CountField);
            this.Controls.Add(this.SetCount1Command);
            this.Controls.Add(this.SetCount0Command);
            this.Controls.Add(this.NameInfo);
            this.DoubleBuffered = true;
            this.MinimumSize = new System.Drawing.Size(100, 100);
            this.Name = "PonySelectionControl";
            this.Size = new System.Drawing.Size(112, 144);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PonySelectionControl_Paint);
            this.Leave += new System.EventHandler(this.PonySelectionControl_Leave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label NameInfo;
        private System.Windows.Forms.Button SetCount0Command;
        private System.Windows.Forms.Button SetCount1Command;
        private System.Windows.Forms.TextBox CountField;
    }
}

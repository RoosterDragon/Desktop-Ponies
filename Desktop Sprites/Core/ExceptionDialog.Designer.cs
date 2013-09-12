namespace DesktopSprites.Core
{
    partial class ExceptionDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.IconBox = new System.Windows.Forms.PictureBox();
            this.ExceptionText = new System.Windows.Forms.TextBox();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.CopyTextButton = new System.Windows.Forms.Button();
            this.CloseButton = new System.Windows.Forms.Button();
            this.LayoutTable = new System.Windows.Forms.TableLayoutPanel();
            this.MessageTable = new System.Windows.Forms.TableLayoutPanel();
            this.MessagePanel = new System.Windows.Forms.Panel();
            this.ButtonPanel = new System.Windows.Forms.Panel();
            this.TimeLabel = new System.Windows.Forms.Label();
            this.LayoutTable.SuspendLayout();
            this.MessageTable.SuspendLayout();
            this.MessagePanel.SuspendLayout();
            this.ButtonPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // IconBox
            // 
            this.IconBox.ErrorImage = null;
            this.IconBox.InitialImage = null;
            this.IconBox.Location = new System.Drawing.Point(3, 3);
            this.IconBox.Name = "IconBox";
            this.IconBox.Size = new System.Drawing.Size(32, 32);
            this.IconBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.IconBox.TabIndex = 0;
            this.IconBox.TabStop = false;
            // 
            // ExceptionText
            // 
            this.ExceptionText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ExceptionText.Location = new System.Drawing.Point(3, 47);
            this.ExceptionText.Multiline = true;
            this.ExceptionText.Name = "ExceptionText";
            this.ExceptionText.ReadOnly = true;
            this.ExceptionText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ExceptionText.Size = new System.Drawing.Size(254, 153);
            this.ExceptionText.TabIndex = 1;
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Location = new System.Drawing.Point(41, 3);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(0, 13);
            this.MessageLabel.TabIndex = 0;
            // 
            // CopyTextButton
            // 
            this.CopyTextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CopyTextButton.Location = new System.Drawing.Point(122, 6);
            this.CopyTextButton.Name = "CopyTextButton";
            this.CopyTextButton.Size = new System.Drawing.Size(75, 23);
            this.CopyTextButton.TabIndex = 1;
            this.CopyTextButton.Text = "Copy Text";
            this.CopyTextButton.UseVisualStyleBackColor = true;
            this.CopyTextButton.Click += new System.EventHandler(this.CopyTextButton_Click);
            // 
            // CloseButton
            // 
            this.CloseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseButton.Location = new System.Drawing.Point(203, 6);
            this.CloseButton.Name = "CloseButton";
            this.CloseButton.Size = new System.Drawing.Size(75, 23);
            this.CloseButton.TabIndex = 2;
            this.CloseButton.Text = "Close";
            this.CloseButton.UseVisualStyleBackColor = true;
            this.CloseButton.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // LayoutTable
            // 
            this.LayoutTable.AutoSize = true;
            this.LayoutTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.LayoutTable.ColumnCount = 1;
            this.LayoutTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutTable.Controls.Add(this.MessageTable, 0, 0);
            this.LayoutTable.Controls.Add(this.ButtonPanel, 0, 1);
            this.LayoutTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LayoutTable.Location = new System.Drawing.Point(0, 0);
            this.LayoutTable.Name = "LayoutTable";
            this.LayoutTable.RowCount = 2;
            this.LayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.LayoutTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.LayoutTable.Size = new System.Drawing.Size(284, 262);
            this.LayoutTable.TabIndex = 0;
            // 
            // MessageTable
            // 
            this.MessageTable.AutoSize = true;
            this.MessageTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MessageTable.ColumnCount = 1;
            this.MessageTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MessageTable.Controls.Add(this.ExceptionText, 0, 1);
            this.MessageTable.Controls.Add(this.MessagePanel, 0, 0);
            this.MessageTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessageTable.Location = new System.Drawing.Point(12, 12);
            this.MessageTable.Margin = new System.Windows.Forms.Padding(12);
            this.MessageTable.Name = "MessageTable";
            this.MessageTable.RowCount = 2;
            this.MessageTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MessageTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MessageTable.Size = new System.Drawing.Size(260, 203);
            this.MessageTable.TabIndex = 1;
            // 
            // MessagePanel
            // 
            this.MessagePanel.AutoSize = true;
            this.MessagePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MessagePanel.Controls.Add(this.IconBox);
            this.MessagePanel.Controls.Add(this.MessageLabel);
            this.MessagePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessagePanel.Location = new System.Drawing.Point(3, 3);
            this.MessagePanel.Name = "MessagePanel";
            this.MessagePanel.Size = new System.Drawing.Size(254, 38);
            this.MessagePanel.TabIndex = 0;
            // 
            // ButtonPanel
            // 
            this.ButtonPanel.AutoSize = true;
            this.ButtonPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ButtonPanel.BackColor = System.Drawing.SystemColors.Control;
            this.ButtonPanel.Controls.Add(this.TimeLabel);
            this.ButtonPanel.Controls.Add(this.CloseButton);
            this.ButtonPanel.Controls.Add(this.CopyTextButton);
            this.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonPanel.Location = new System.Drawing.Point(0, 227);
            this.ButtonPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonPanel.Name = "ButtonPanel";
            this.ButtonPanel.Padding = new System.Windows.Forms.Padding(3);
            this.ButtonPanel.Size = new System.Drawing.Size(284, 35);
            this.ButtonPanel.TabIndex = 0;
            // 
            // TimeLabel
            // 
            this.TimeLabel.AutoSize = true;
            this.TimeLabel.Location = new System.Drawing.Point(12, 11);
            this.TimeLabel.Name = "TimeLabel";
            this.TimeLabel.Size = new System.Drawing.Size(0, 13);
            this.TimeLabel.TabIndex = 0;
            // 
            // ExceptionDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.LayoutTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Unexpected Error";
            this.LayoutTable.ResumeLayout(false);
            this.LayoutTable.PerformLayout();
            this.MessageTable.ResumeLayout(false);
            this.MessageTable.PerformLayout();
            this.MessagePanel.ResumeLayout(false);
            this.MessagePanel.PerformLayout();
            this.ButtonPanel.ResumeLayout(false);
            this.ButtonPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox IconBox;
        private System.Windows.Forms.TextBox ExceptionText;
        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.Button CopyTextButton;
        private System.Windows.Forms.Button CloseButton;
        private System.Windows.Forms.TableLayoutPanel LayoutTable;
        private System.Windows.Forms.Panel MessagePanel;
        private System.Windows.Forms.Panel ButtonPanel;
        private System.Windows.Forms.TableLayoutPanel MessageTable;
        private System.Windows.Forms.Label TimeLabel;
    }
}
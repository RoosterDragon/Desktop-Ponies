namespace CSDesktopPonies.DesktopPonies
{
    partial class AnimatedImageIndexer
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
            this.components = new System.ComponentModel.Container();
            this.PreviousCommand = new System.Windows.Forms.Button();
            this.NextCommand = new System.Windows.Forms.Button();
            this.PlayCommand = new System.Windows.Forms.Button();
            this.TimeSelectorSections = new System.Windows.Forms.Panel();
            this.TimeSelector = new System.Windows.Forms.TrackBar();
            this.FrameLabel = new System.Windows.Forms.Label();
            this.FrameSelector = new System.Windows.Forms.TrackBar();
            this.PlaybackTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.TimeSelector)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FrameSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // PreviousCommand
            // 
            this.PreviousCommand.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PreviousCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PreviousCommand.Location = new System.Drawing.Point(51, 52);
            this.PreviousCommand.Name = "PreviousCommand";
            this.PreviousCommand.Size = new System.Drawing.Size(25, 16);
            this.PreviousCommand.TabIndex = 9;
            this.PreviousCommand.Text = "<";
            this.PreviousCommand.UseVisualStyleBackColor = true;
            this.PreviousCommand.Click += new System.EventHandler(this.PreviousCommand_Click);
            // 
            // NextCommand
            // 
            this.NextCommand.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.NextCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NextCommand.Location = new System.Drawing.Point(138, 52);
            this.NextCommand.Name = "NextCommand";
            this.NextCommand.Size = new System.Drawing.Size(25, 16);
            this.NextCommand.TabIndex = 11;
            this.NextCommand.Text = ">";
            this.NextCommand.UseVisualStyleBackColor = true;
            this.NextCommand.Click += new System.EventHandler(this.NextCommand_Click);
            // 
            // PlayCommand
            // 
            this.PlayCommand.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.PlayCommand.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PlayCommand.Location = new System.Drawing.Point(82, 52);
            this.PlayCommand.Name = "PlayCommand";
            this.PlayCommand.Size = new System.Drawing.Size(50, 16);
            this.PlayCommand.TabIndex = 10;
            this.PlayCommand.Text = "Play";
            this.PlayCommand.UseVisualStyleBackColor = true;
            this.PlayCommand.Click += new System.EventHandler(this.PlayCommand_Click);
            // 
            // TimeSelectorSections
            // 
            this.TimeSelectorSections.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeSelectorSections.Location = new System.Drawing.Point(17, 88);
            this.TimeSelectorSections.Name = "TimeSelectorSections";
            this.TimeSelectorSections.Size = new System.Drawing.Size(180, 4);
            this.TimeSelectorSections.TabIndex = 13;
            this.TimeSelectorSections.Paint += new System.Windows.Forms.PaintEventHandler(this.TimeSelectorSections_Paint);
            // 
            // TimeSelector
            // 
            this.TimeSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TimeSelector.LargeChange = 5000;
            this.TimeSelector.Location = new System.Drawing.Point(3, 66);
            this.TimeSelector.Maximum = 0;
            this.TimeSelector.Name = "TimeSelector";
            this.TimeSelector.Size = new System.Drawing.Size(208, 45);
            this.TimeSelector.SmallChange = 1000;
            this.TimeSelector.TabIndex = 12;
            this.TimeSelector.TickFrequency = 0;
            this.TimeSelector.ValueChanged += new System.EventHandler(this.TimeSelector_ValueChanged);
            // 
            // FrameLabel
            // 
            this.FrameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrameLabel.Location = new System.Drawing.Point(3, 33);
            this.FrameLabel.Name = "FrameLabel";
            this.FrameLabel.Size = new System.Drawing.Size(208, 16);
            this.FrameLabel.TabIndex = 8;
            this.FrameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FrameSelector
            // 
            this.FrameSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FrameSelector.Location = new System.Drawing.Point(3, 3);
            this.FrameSelector.Maximum = 0;
            this.FrameSelector.Name = "FrameSelector";
            this.FrameSelector.Size = new System.Drawing.Size(208, 45);
            this.FrameSelector.TabIndex = 7;
            this.FrameSelector.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.FrameSelector.ValueChanged += new System.EventHandler(this.FrameSelector_ValueChanged);
            // 
            // PlaybackTimer
            // 
            this.PlaybackTimer.Interval = 50;
            this.PlaybackTimer.Tick += new System.EventHandler(this.PlaybackTimer_Tick);
            // 
            // AnimatedImageIndexer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PreviousCommand);
            this.Controls.Add(this.NextCommand);
            this.Controls.Add(this.PlayCommand);
            this.Controls.Add(this.TimeSelectorSections);
            this.Controls.Add(this.TimeSelector);
            this.Controls.Add(this.FrameLabel);
            this.Controls.Add(this.FrameSelector);
            this.Enabled = false;
            this.Name = "AnimatedImageIndexer";
            this.Size = new System.Drawing.Size(214, 114);
            ((System.ComponentModel.ISupportInitialize)(this.TimeSelector)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FrameSelector)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button PreviousCommand;
        private System.Windows.Forms.Button NextCommand;
        private System.Windows.Forms.Button PlayCommand;
        private System.Windows.Forms.Panel TimeSelectorSections;
        private System.Windows.Forms.TrackBar TimeSelector;
        private System.Windows.Forms.Label FrameLabel;
        private System.Windows.Forms.TrackBar FrameSelector;
        private System.Windows.Forms.Timer PlaybackTimer;


    }
}

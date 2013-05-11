namespace CSDesktopPonies.Core
{
    partial class ExceptionDialog
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
            this.ErrorMessage = new System.Windows.Forms.Label();
            this.ExceptionInformation = new System.Windows.Forms.Label();
            this.EmailCommand = new System.Windows.Forms.Button();
            this.CloseCommand = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ErrorMessage
            // 
            this.ErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ErrorMessage.Location = new System.Drawing.Point(12, 9);
            this.ErrorMessage.Name = "ErrorMessage";
            this.ErrorMessage.Size = new System.Drawing.Size(350, 51);
            this.ErrorMessage.TabIndex = 0;
            this.ErrorMessage.Text = "An error has occured that the program could not handle and it needs to close.";
            // 
            // ExceptionInformation
            // 
            this.ExceptionInformation.Location = new System.Drawing.Point(12, 60);
            this.ExceptionInformation.Name = "ExceptionInformation";
            this.ExceptionInformation.Size = new System.Drawing.Size(350, 81);
            this.ExceptionInformation.TabIndex = 1;
            this.ExceptionInformation.Text = "Exception:\r\nMessage:\r\n";
            // 
            // EmailCommand
            // 
            this.EmailCommand.Location = new System.Drawing.Point(206, 144);
            this.EmailCommand.Name = "EmailCommand";
            this.EmailCommand.Size = new System.Drawing.Size(75, 23);
            this.EmailCommand.TabIndex = 2;
            this.EmailCommand.Text = "Send E-Mail";
            this.EmailCommand.UseVisualStyleBackColor = true;
            this.EmailCommand.Click += new System.EventHandler(this.EmailCommand_Click);
            // 
            // CloseCommand
            // 
            this.CloseCommand.Location = new System.Drawing.Point(287, 144);
            this.CloseCommand.Name = "CloseCommand";
            this.CloseCommand.Size = new System.Drawing.Size(75, 23);
            this.CloseCommand.TabIndex = 4;
            this.CloseCommand.Text = "Close";
            this.CloseCommand.UseVisualStyleBackColor = true;
            this.CloseCommand.Click += new System.EventHandler(this.CloseCommand_Click);
            // 
            // ExceptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 179);
            this.Controls.Add(this.CloseCommand);
            this.Controls.Add(this.EmailCommand);
            this.Controls.Add(this.ExceptionInformation);
            this.Controls.Add(this.ErrorMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExceptionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Critical Error";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label ErrorMessage;
        private System.Windows.Forms.Label ExceptionInformation;
        private System.Windows.Forms.Button EmailCommand;
        private System.Windows.Forms.Button CloseCommand;
    }
}
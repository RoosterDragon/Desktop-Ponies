<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ScreensaverBackgroundForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.ScreenSaver_Image = New System.Windows.Forms.PictureBox()
        Me.SuspendLayout()
        '
        'ScreenSaver_Image
        '
        Me.ScreenSaver_Image.Location = New System.Drawing.Point(0, 0)
        Me.ScreenSaver_Image.Name = "ScreenSaver_Image"
        Me.ScreenSaver_Image.Size = New System.Drawing.Size(0, 0)
        Me.ScreenSaver_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.ScreenSaver_Image.TabIndex = 0
        Me.ScreenSaver_Image.TabStop = False
        '
        'ScreensaverBackgroundForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoValidate = System.Windows.Forms.AutoValidate.Disable
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.ClientSize = New System.Drawing.Size(0, 0)
        Me.ControlBox = False
        Me.Controls.Add(Me.ScreenSaver_Image)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ScreensaverBackgroundForm"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.Manual
        Me.Text = "Effect_Form"
        Me.TransparencyKey = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ScreenSaver_Image As System.Windows.Forms.PictureBox
End Class

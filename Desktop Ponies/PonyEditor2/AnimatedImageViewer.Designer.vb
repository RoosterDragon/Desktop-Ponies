<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AnimatedImageViewer
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Me.ErrorLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'ErrorLabel
        '
        Me.ErrorLabel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ErrorLabel.Location = New System.Drawing.Point(3, 0)
        Me.ErrorLabel.Name = "ErrorLabel"
        Me.ErrorLabel.Padding = New System.Windows.Forms.Padding(15)
        Me.ErrorLabel.Size = New System.Drawing.Size(200, 116)
        Me.ErrorLabel.TabIndex = 0
        Me.ErrorLabel.Text = "There was an error attempting to display this image."
        Me.ErrorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.ErrorLabel.Visible = False
        '
        'AnimatedImageViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.BackColor = System.Drawing.Color.White
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.ErrorLabel)
        Me.DoubleBuffered = True
        Me.Name = "AnimatedImageViewer"
        Me.Size = New System.Drawing.Size(206, 116)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ErrorLabel As System.Windows.Forms.Label

End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnusedFilesForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.StatusLabel = New System.Windows.Forms.Label()
        Me.UnusedFilesTextBox = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'StatusLabel
        '
        Me.StatusLabel.AutoSize = True
        Me.StatusLabel.Location = New System.Drawing.Point(12, 9)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(61, 13)
        Me.StatusLabel.TabIndex = 0
        Me.StatusLabel.Text = "Scanning..."
        '
        'UnusedFilesTextBox
        '
        Me.UnusedFilesTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UnusedFilesTextBox.Location = New System.Drawing.Point(12, 25)
        Me.UnusedFilesTextBox.Multiline = True
        Me.UnusedFilesTextBox.Name = "UnusedFilesTextBox"
        Me.UnusedFilesTextBox.ReadOnly = True
        Me.UnusedFilesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.UnusedFilesTextBox.Size = New System.Drawing.Size(360, 225)
        Me.UnusedFilesTextBox.TabIndex = 1
        '
        'UnusedFilesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(384, 262)
        Me.Controls.Add(Me.UnusedFilesTextBox)
        Me.Controls.Add(Me.StatusLabel)
        Me.MinimumSize = New System.Drawing.Size(300, 200)
        Me.Name = "UnusedFilesForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Unused Files - Desktop Ponies"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents StatusLabel As System.Windows.Forms.Label
    Friend WithEvents UnusedFilesTextBox As System.Windows.Forms.TextBox
End Class

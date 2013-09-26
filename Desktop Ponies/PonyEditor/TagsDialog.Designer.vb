<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class TagsDialog
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
        Me.PonyFilterList = New System.Windows.Forms.CheckedListBox()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.InstructionsLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'PonyFilterList
        '
        Me.PonyFilterList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PonyFilterList.CheckOnClick = True
        Me.PonyFilterList.FormattingEnabled = True
        Me.PonyFilterList.Items.AddRange(New Object() {"Main Ponies", "Supporting Ponies", "Alternate Art", "Fillies", "Pets", "Stallions", "Mares", "Alicorns", "Unicorns", "Pegasi", "Earth Ponies", "Non-Ponies", "Not Tagged"})
        Me.PonyFilterList.Location = New System.Drawing.Point(12, 51)
        Me.PonyFilterList.Name = "PonyFilterList"
        Me.PonyFilterList.Size = New System.Drawing.Size(385, 199)
        Me.PonyFilterList.TabIndex = 13
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cancel_Button.Location = New System.Drawing.Point(322, 262)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(75, 23)
        Me.Cancel_Button.TabIndex = 14
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OK_Button.Location = New System.Drawing.Point(241, 262)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(75, 23)
        Me.OK_Button.TabIndex = 15
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = True
        '
        'InstructionsLabel
        '
        Me.InstructionsLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InstructionsLabel.Location = New System.Drawing.Point(12, 9)
        Me.InstructionsLabel.Name = "InstructionsLabel"
        Me.InstructionsLabel.Size = New System.Drawing.Size(385, 39)
        Me.InstructionsLabel.TabIndex = 16
        Me.InstructionsLabel.Text = "Select the tags you want your pony to have here." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "You can edit the allowed tags i" & _
    "n the options menu by selecting ""Custom Filters""."
        Me.InstructionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TagsDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(409, 297)
        Me.Controls.Add(Me.InstructionsLabel)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.PonyFilterList)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MinimumSize = New System.Drawing.Size(300, 200)
        Me.Name = "TagsDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Tags for..."
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PonyFilterList As System.Windows.Forms.CheckedListBox
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents InstructionsLabel As System.Windows.Forms.Label
End Class

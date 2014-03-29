<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewSpeechDialog
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
        Me.CommandTable = New System.Windows.Forms.TableLayoutPanel()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.NameTextBox = New System.Windows.Forms.TextBox()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.LineTextBox = New System.Windows.Forms.TextBox()
        Me.LineLabel = New System.Windows.Forms.Label()
        Me.SoundTextBox = New System.Windows.Forms.TextBox()
        Me.SoundLabel = New System.Windows.Forms.Label()
        Me.SoundSelectButton = New System.Windows.Forms.Button()
        Me.UseRandomlyCheckBox = New System.Windows.Forms.CheckBox()
        Me.GroupNumber = New System.Windows.Forms.NumericUpDown()
        Me.GroupLabel = New System.Windows.Forms.Label()
        Me.LayoutTable = New System.Windows.Forms.TableLayoutPanel()
        Me.SoundClearButton = New System.Windows.Forms.Button()
        Me.SoundPanel = New System.Windows.Forms.Panel()
        Me.CommandTable.SuspendLayout()
        Me.LayoutTable.SuspendLayout()
        Me.SoundPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'CommandTable
        '
        Me.CommandTable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CommandTable.ColumnCount = 2
        Me.CommandTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.CommandTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.CommandTable.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.CommandTable.Controls.Add(Me.OK_Button, 0, 0)
        Me.CommandTable.Location = New System.Drawing.Point(152, 246)
        Me.CommandTable.Name = "CommandTable"
        Me.CommandTable.RowCount = 1
        Me.CommandTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.CommandTable.Size = New System.Drawing.Size(320, 29)
        Me.CommandTable.TabIndex = 1
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.BackColor = System.Drawing.SystemColors.Control
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(206, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = False
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.BackColor = System.Drawing.SystemColors.Control
        Me.OK_Button.Location = New System.Drawing.Point(46, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = False
        '
        'NameTextBox
        '
        Me.NameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.NameTextBox.Location = New System.Drawing.Point(55, 16)
        Me.NameTextBox.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.NameTextBox.Name = "NameTextBox"
        Me.NameTextBox.Size = New System.Drawing.Size(120, 20)
        Me.NameTextBox.TabIndex = 1
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Location = New System.Drawing.Point(3, 0)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(78, 13)
        Me.NameLabel.TabIndex = 0
        Me.NameLabel.Text = "Speech Name:"
        '
        'LineTextBox
        '
        Me.LineTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LayoutTable.SetColumnSpan(Me.LineTextBox, 2)
        Me.LineTextBox.Location = New System.Drawing.Point(15, 67)
        Me.LineTextBox.Margin = New System.Windows.Forms.Padding(15, 3, 15, 15)
        Me.LineTextBox.MaxLength = 254
        Me.LineTextBox.Name = "LineTextBox"
        Me.LineTextBox.Size = New System.Drawing.Size(430, 20)
        Me.LineTextBox.TabIndex = 5
        '
        'LineLabel
        '
        Me.LineLabel.AutoSize = True
        Me.LineLabel.Location = New System.Drawing.Point(3, 51)
        Me.LineLabel.Name = "LineLabel"
        Me.LineLabel.Size = New System.Drawing.Size(78, 13)
        Me.LineLabel.TabIndex = 4
        Me.LineLabel.Text = "Text to display:"
        '
        'SoundTextBox
        '
        Me.SoundTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LayoutTable.SetColumnSpan(Me.SoundTextBox, 2)
        Me.SoundTextBox.Location = New System.Drawing.Point(15, 118)
        Me.SoundTextBox.Margin = New System.Windows.Forms.Padding(15, 3, 15, 3)
        Me.SoundTextBox.MaxLength = 254
        Me.SoundTextBox.Name = "SoundTextBox"
        Me.SoundTextBox.ReadOnly = True
        Me.SoundTextBox.Size = New System.Drawing.Size(430, 20)
        Me.SoundTextBox.TabIndex = 7
        '
        'SoundLabel
        '
        Me.SoundLabel.AutoSize = True
        Me.SoundLabel.Location = New System.Drawing.Point(3, 102)
        Me.SoundLabel.Name = "SoundLabel"
        Me.SoundLabel.Size = New System.Drawing.Size(103, 13)
        Me.SoundLabel.TabIndex = 6
        Me.SoundLabel.Text = "Sound file (optional):"
        '
        'SoundSelectButton
        '
        Me.SoundSelectButton.Location = New System.Drawing.Point(15, 3)
        Me.SoundSelectButton.Margin = New System.Windows.Forms.Padding(15, 3, 3, 15)
        Me.SoundSelectButton.Name = "SoundSelectButton"
        Me.SoundSelectButton.Size = New System.Drawing.Size(100, 23)
        Me.SoundSelectButton.TabIndex = 0
        Me.SoundSelectButton.Text = "Select..."
        Me.SoundSelectButton.UseVisualStyleBackColor = True
        '
        'UseRandomlyCheckBox
        '
        Me.UseRandomlyCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.UseRandomlyCheckBox.AutoSize = True
        Me.UseRandomlyCheckBox.Checked = True
        Me.UseRandomlyCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.LayoutTable.SetColumnSpan(Me.UseRandomlyCheckBox, 2)
        Me.UseRandomlyCheckBox.Location = New System.Drawing.Point(52, 185)
        Me.UseRandomlyCheckBox.Name = "UseRandomlyCheckBox"
        Me.UseRandomlyCheckBox.Size = New System.Drawing.Size(355, 30)
        Me.UseRandomlyCheckBox.TabIndex = 9
        Me.UseRandomlyCheckBox.Text = "Use Randomly? (uncheck if you want this to be used for one behavior" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "only, which " & _
    "you will specify on the behavior edit screen)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.UseRandomlyCheckBox.UseVisualStyleBackColor = True
        '
        'GroupNumber
        '
        Me.GroupNumber.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.GroupNumber.Location = New System.Drawing.Point(285, 16)
        Me.GroupNumber.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.GroupNumber.Name = "GroupNumber"
        Me.GroupNumber.Size = New System.Drawing.Size(120, 20)
        Me.GroupNumber.TabIndex = 3
        '
        'GroupLabel
        '
        Me.GroupLabel.AutoSize = True
        Me.GroupLabel.Location = New System.Drawing.Point(233, 0)
        Me.GroupLabel.Name = "GroupLabel"
        Me.GroupLabel.Size = New System.Drawing.Size(84, 13)
        Me.GroupLabel.TabIndex = 2
        Me.GroupLabel.Text = "Behavior Group:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'LayoutTable
        '
        Me.LayoutTable.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LayoutTable.ColumnCount = 2
        Me.LayoutTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.LayoutTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.LayoutTable.Controls.Add(Me.NameLabel, 0, 0)
        Me.LayoutTable.Controls.Add(Me.UseRandomlyCheckBox, 0, 7)
        Me.LayoutTable.Controls.Add(Me.GroupNumber, 1, 1)
        Me.LayoutTable.Controls.Add(Me.GroupLabel, 1, 0)
        Me.LayoutTable.Controls.Add(Me.SoundTextBox, 0, 5)
        Me.LayoutTable.Controls.Add(Me.SoundLabel, 0, 4)
        Me.LayoutTable.Controls.Add(Me.NameTextBox, 0, 1)
        Me.LayoutTable.Controls.Add(Me.LineLabel, 0, 2)
        Me.LayoutTable.Controls.Add(Me.LineTextBox, 0, 3)
        Me.LayoutTable.Controls.Add(Me.SoundPanel, 0, 6)
        Me.LayoutTable.Location = New System.Drawing.Point(12, 12)
        Me.LayoutTable.Name = "LayoutTable"
        Me.LayoutTable.RowCount = 8
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.Size = New System.Drawing.Size(460, 228)
        Me.LayoutTable.TabIndex = 0
        '
        'SoundClearButton
        '
        Me.SoundClearButton.Enabled = False
        Me.SoundClearButton.Location = New System.Drawing.Point(121, 3)
        Me.SoundClearButton.Name = "SoundClearButton"
        Me.SoundClearButton.Size = New System.Drawing.Size(100, 23)
        Me.SoundClearButton.TabIndex = 1
        Me.SoundClearButton.Text = "Clear"
        Me.SoundClearButton.UseVisualStyleBackColor = True
        '
        'SoundPanel
        '
        Me.SoundPanel.AutoSize = True
        Me.SoundPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.LayoutTable.SetColumnSpan(Me.SoundPanel, 2)
        Me.SoundPanel.Controls.Add(Me.SoundSelectButton)
        Me.SoundPanel.Controls.Add(Me.SoundClearButton)
        Me.SoundPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SoundPanel.Location = New System.Drawing.Point(0, 141)
        Me.SoundPanel.Margin = New System.Windows.Forms.Padding(0)
        Me.SoundPanel.Name = "SoundPanel"
        Me.SoundPanel.Size = New System.Drawing.Size(460, 41)
        Me.SoundPanel.TabIndex = 8
        '
        'NewSpeechDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(484, 287)
        Me.Controls.Add(Me.LayoutTable)
        Me.Controls.Add(Me.CommandTable)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewSpeechDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create New Speech..."
        Me.CommandTable.ResumeLayout(False)
        Me.LayoutTable.ResumeLayout(False)
        Me.LayoutTable.PerformLayout()
        Me.SoundPanel.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CommandTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents NameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents LineTextBox As System.Windows.Forms.TextBox
    Friend WithEvents LineLabel As System.Windows.Forms.Label
    Friend WithEvents SoundTextBox As System.Windows.Forms.TextBox
    Friend WithEvents SoundLabel As System.Windows.Forms.Label
    Friend WithEvents SoundSelectButton As System.Windows.Forms.Button
    Friend WithEvents UseRandomlyCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents GroupNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupLabel As System.Windows.Forms.Label
    Friend WithEvents LayoutTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SoundPanel As System.Windows.Forms.Panel
    Friend WithEvents SoundClearButton As System.Windows.Forms.Button

End Class

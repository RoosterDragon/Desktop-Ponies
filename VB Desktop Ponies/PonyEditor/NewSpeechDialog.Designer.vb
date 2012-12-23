<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewSpeechDialog
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Name_Textbox = New System.Windows.Forms.TextBox()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.Text_TextBox = New System.Windows.Forms.TextBox()
        Me.LineLabel = New System.Windows.Forms.Label()
        Me.Sound_Textbox = New System.Windows.Forms.TextBox()
        Me.SoundLabel = New System.Windows.Forms.Label()
        Me.SetSound_Button = New System.Windows.Forms.Button()
        Me.Random_Checkbox = New System.Windows.Forms.CheckBox()
        Me.OpenSoundDialog = New System.Windows.Forms.OpenFileDialog()
        Me.Group_NumberBox = New System.Windows.Forms.NumericUpDown()
        Me.GroupLabel = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(182, 329)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(320, 29)
        Me.TableLayoutPanel1.TabIndex = 11
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
        'Name_Textbox
        '
        Me.Name_Textbox.Location = New System.Drawing.Point(78, 60)
        Me.Name_Textbox.Name = "Name_Textbox"
        Me.Name_Textbox.Size = New System.Drawing.Size(100, 20)
        Me.Name_Textbox.TabIndex = 1
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Location = New System.Drawing.Point(58, 44)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(78, 13)
        Me.NameLabel.TabIndex = 0
        Me.NameLabel.Text = "Speech Name:"
        '
        'Text_TextBox
        '
        Me.Text_TextBox.Location = New System.Drawing.Point(78, 132)
        Me.Text_TextBox.MaxLength = 254
        Me.Text_TextBox.Name = "Text_TextBox"
        Me.Text_TextBox.Size = New System.Drawing.Size(333, 20)
        Me.Text_TextBox.TabIndex = 5
        '
        'LineLabel
        '
        Me.LineLabel.AutoSize = True
        Me.LineLabel.Location = New System.Drawing.Point(58, 116)
        Me.LineLabel.Name = "LineLabel"
        Me.LineLabel.Size = New System.Drawing.Size(78, 13)
        Me.LineLabel.TabIndex = 4
        Me.LineLabel.Text = "Text to display:"
        '
        'Sound_Textbox
        '
        Me.Sound_Textbox.Location = New System.Drawing.Point(78, 200)
        Me.Sound_Textbox.MaxLength = 254
        Me.Sound_Textbox.Name = "Sound_Textbox"
        Me.Sound_Textbox.ReadOnly = True
        Me.Sound_Textbox.Size = New System.Drawing.Size(333, 20)
        Me.Sound_Textbox.TabIndex = 7
        '
        'SoundLabel
        '
        Me.SoundLabel.AutoSize = True
        Me.SoundLabel.Location = New System.Drawing.Point(58, 184)
        Me.SoundLabel.Name = "SoundLabel"
        Me.SoundLabel.Size = New System.Drawing.Size(103, 13)
        Me.SoundLabel.TabIndex = 6
        Me.SoundLabel.Text = "Sound file (optional):"
        '
        'SetSound_Button
        '
        Me.SetSound_Button.Location = New System.Drawing.Point(103, 226)
        Me.SetSound_Button.Name = "SetSound_Button"
        Me.SetSound_Button.Size = New System.Drawing.Size(75, 23)
        Me.SetSound_Button.TabIndex = 8
        Me.SetSound_Button.Text = "Set Sound"
        Me.SetSound_Button.UseVisualStyleBackColor = True
        '
        'Random_Checkbox
        '
        Me.Random_Checkbox.AutoSize = True
        Me.Random_Checkbox.Checked = True
        Me.Random_Checkbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Random_Checkbox.Location = New System.Drawing.Point(78, 279)
        Me.Random_Checkbox.Name = "Random_Checkbox"
        Me.Random_Checkbox.Size = New System.Drawing.Size(355, 30)
        Me.Random_Checkbox.TabIndex = 9
        Me.Random_Checkbox.Text = "Use Randomly? (uncheck if you want this to be used for one behavior" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "only, which " & _
    "you will specify on the behavior edit screen)" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.Random_Checkbox.UseVisualStyleBackColor = True
        '
        'Group_NumberBox
        '
        Me.Group_NumberBox.Location = New System.Drawing.Point(268, 60)
        Me.Group_NumberBox.Name = "Group_NumberBox"
        Me.Group_NumberBox.Size = New System.Drawing.Size(120, 20)
        Me.Group_NumberBox.TabIndex = 3
        '
        'GroupLabel
        '
        Me.GroupLabel.AutoSize = True
        Me.GroupLabel.Location = New System.Drawing.Point(250, 44)
        Me.GroupLabel.Name = "GroupLabel"
        Me.GroupLabel.Size = New System.Drawing.Size(92, 13)
        Me.GroupLabel.TabIndex = 2
        Me.GroupLabel.Text = "Behavior group #:"
        '
        'NewSpeechDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(514, 370)
        Me.Controls.Add(Me.GroupLabel)
        Me.Controls.Add(Me.Group_NumberBox)
        Me.Controls.Add(Me.Random_Checkbox)
        Me.Controls.Add(Me.SetSound_Button)
        Me.Controls.Add(Me.SoundLabel)
        Me.Controls.Add(Me.Sound_Textbox)
        Me.Controls.Add(Me.LineLabel)
        Me.Controls.Add(Me.Text_TextBox)
        Me.Controls.Add(Me.NameLabel)
        Me.Controls.Add(Me.Name_Textbox)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewSpeechDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create New Speech..."
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Name_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents Text_TextBox As System.Windows.Forms.TextBox
    Friend WithEvents LineLabel As System.Windows.Forms.Label
    Friend WithEvents Sound_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents SoundLabel As System.Windows.Forms.Label
    Friend WithEvents SetSound_Button As System.Windows.Forms.Button
    Friend WithEvents Random_Checkbox As System.Windows.Forms.CheckBox
    Friend WithEvents OpenSoundDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Group_NumberBox As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupLabel As System.Windows.Forms.Label

End Class

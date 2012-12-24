<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewBehaviorDialog
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
        Me.Left_ImageBox = New System.Windows.Forms.PictureBox()
        Me.Right_ImageBox = New System.Windows.Forms.PictureBox()
        Me.NameTextbox = New System.Windows.Forms.TextBox()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.LeftImageLabel = New System.Windows.Forms.Label()
        Me.RightImageLabel = New System.Windows.Forms.Label()
        Me.Left_Image_Set_Button = New System.Windows.Forms.Button()
        Me.Right_Image_Set_Button = New System.Windows.Forms.Button()
        Me.Movement_Combobox = New System.Windows.Forms.ComboBox()
        Me.MovementLabel = New System.Windows.Forms.Label()
        Me.Chance_Box = New System.Windows.Forms.TextBox()
        Me.Max_Box = New System.Windows.Forms.TextBox()
        Me.Min_Box = New System.Windows.Forms.TextBox()
        Me.StartSpeech_Box = New System.Windows.Forms.ComboBox()
        Me.EndSpeech_Box = New System.Windows.Forms.ComboBox()
        Me.Follow_Box = New System.Windows.Forms.TextBox()
        Me.SetFollow_Button = New System.Windows.Forms.Button()
        Me.Link_Box = New System.Windows.Forms.ComboBox()
        Me.ChanceLabel = New System.Windows.Forms.Label()
        Me.MaxDurationLabel = New System.Windows.Forms.Label()
        Me.MinDurationLabel = New System.Windows.Forms.Label()
        Me.StartSpeechLabel = New System.Windows.Forms.Label()
        Me.EndSpeechLabel = New System.Windows.Forms.Label()
        Me.FollowLabel = New System.Windows.Forms.Label()
        Me.LinkLabel = New System.Windows.Forms.Label()
        Me.Speed_Box = New System.Windows.Forms.TextBox()
        Me.SpeedLabel = New System.Windows.Forms.Label()
        Me.DontRepeat_CheckBox = New System.Windows.Forms.CheckBox()
        Me.DontRunRandomly_CheckBox = New System.Windows.Forms.CheckBox()
        Me.Group_Numberbox = New System.Windows.Forms.NumericUpDown()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(323, 581)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(320, 29)
        Me.TableLayoutPanel1.TabIndex = 29
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
        'Left_ImageBox
        '
        Me.Left_ImageBox.Location = New System.Drawing.Point(108, 157)
        Me.Left_ImageBox.Name = "Left_ImageBox"
        Me.Left_ImageBox.Size = New System.Drawing.Size(150, 150)
        Me.Left_ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.Left_ImageBox.TabIndex = 1
        Me.Left_ImageBox.TabStop = False
        '
        'Right_ImageBox
        '
        Me.Right_ImageBox.Location = New System.Drawing.Point(388, 157)
        Me.Right_ImageBox.Name = "Right_ImageBox"
        Me.Right_ImageBox.Size = New System.Drawing.Size(150, 150)
        Me.Right_ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.Right_ImageBox.TabIndex = 2
        Me.Right_ImageBox.TabStop = False
        '
        'NameTextbox
        '
        Me.NameTextbox.Location = New System.Drawing.Point(87, 60)
        Me.NameTextbox.Name = "NameTextbox"
        Me.NameTextbox.Size = New System.Drawing.Size(100, 20)
        Me.NameTextbox.TabIndex = 1
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Location = New System.Drawing.Point(58, 44)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(83, 13)
        Me.NameLabel.TabIndex = 0
        Me.NameLabel.Text = "Behavior Name:"
        '
        'LeftImageLabel
        '
        Me.LeftImageLabel.AutoSize = True
        Me.LeftImageLabel.Location = New System.Drawing.Point(80, 139)
        Me.LeftImageLabel.Name = "LeftImageLabel"
        Me.LeftImageLabel.Size = New System.Drawing.Size(60, 13)
        Me.LeftImageLabel.TabIndex = 8
        Me.LeftImageLabel.Text = "Left Image:"
        '
        'RightImageLabel
        '
        Me.RightImageLabel.AutoSize = True
        Me.RightImageLabel.Location = New System.Drawing.Point(365, 139)
        Me.RightImageLabel.Name = "RightImageLabel"
        Me.RightImageLabel.Size = New System.Drawing.Size(67, 13)
        Me.RightImageLabel.TabIndex = 10
        Me.RightImageLabel.Text = "Right Image:"
        '
        'Left_Image_Set_Button
        '
        Me.Left_Image_Set_Button.Location = New System.Drawing.Point(145, 313)
        Me.Left_Image_Set_Button.Name = "Left_Image_Set_Button"
        Me.Left_Image_Set_Button.Size = New System.Drawing.Size(75, 23)
        Me.Left_Image_Set_Button.TabIndex = 9
        Me.Left_Image_Set_Button.Text = "Select..."
        Me.Left_Image_Set_Button.UseVisualStyleBackColor = True
        '
        'Right_Image_Set_Button
        '
        Me.Right_Image_Set_Button.Location = New System.Drawing.Point(426, 313)
        Me.Right_Image_Set_Button.Name = "Right_Image_Set_Button"
        Me.Right_Image_Set_Button.Size = New System.Drawing.Size(75, 23)
        Me.Right_Image_Set_Button.TabIndex = 11
        Me.Right_Image_Set_Button.Text = "Select..."
        Me.Right_Image_Set_Button.UseVisualStyleBackColor = True
        '
        'Movement_Combobox
        '
        Me.Movement_Combobox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Movement_Combobox.FormattingEnabled = True
        Me.Movement_Combobox.Location = New System.Drawing.Point(262, 60)
        Me.Movement_Combobox.Name = "Movement_Combobox"
        Me.Movement_Combobox.Size = New System.Drawing.Size(121, 21)
        Me.Movement_Combobox.TabIndex = 3
        '
        'MovementLabel
        '
        Me.MovementLabel.AutoSize = True
        Me.MovementLabel.Location = New System.Drawing.Point(243, 44)
        Me.MovementLabel.Name = "MovementLabel"
        Me.MovementLabel.Size = New System.Drawing.Size(60, 13)
        Me.MovementLabel.TabIndex = 2
        Me.MovementLabel.Text = "Movement:"
        '
        'Chance_Box
        '
        Me.Chance_Box.Location = New System.Drawing.Point(91, 382)
        Me.Chance_Box.Name = "Chance_Box"
        Me.Chance_Box.Size = New System.Drawing.Size(100, 20)
        Me.Chance_Box.TabIndex = 14
        '
        'Max_Box
        '
        Me.Max_Box.Location = New System.Drawing.Point(282, 382)
        Me.Max_Box.Name = "Max_Box"
        Me.Max_Box.Size = New System.Drawing.Size(100, 20)
        Me.Max_Box.TabIndex = 16
        '
        'Min_Box
        '
        Me.Min_Box.Location = New System.Drawing.Point(470, 382)
        Me.Min_Box.Name = "Min_Box"
        Me.Min_Box.Size = New System.Drawing.Size(100, 20)
        Me.Min_Box.TabIndex = 18
        '
        'StartSpeech_Box
        '
        Me.StartSpeech_Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.StartSpeech_Box.FormattingEnabled = True
        Me.StartSpeech_Box.Location = New System.Drawing.Point(83, 468)
        Me.StartSpeech_Box.Name = "StartSpeech_Box"
        Me.StartSpeech_Box.Size = New System.Drawing.Size(121, 21)
        Me.StartSpeech_Box.TabIndex = 20
        '
        'EndSpeech_Box
        '
        Me.EndSpeech_Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.EndSpeech_Box.FormattingEnabled = True
        Me.EndSpeech_Box.Location = New System.Drawing.Point(272, 468)
        Me.EndSpeech_Box.Name = "EndSpeech_Box"
        Me.EndSpeech_Box.Size = New System.Drawing.Size(121, 21)
        Me.EndSpeech_Box.TabIndex = 22
        '
        'Follow_Box
        '
        Me.Follow_Box.Location = New System.Drawing.Point(470, 469)
        Me.Follow_Box.Name = "Follow_Box"
        Me.Follow_Box.ReadOnly = True
        Me.Follow_Box.Size = New System.Drawing.Size(100, 20)
        Me.Follow_Box.TabIndex = 24
        '
        'SetFollow_Button
        '
        Me.SetFollow_Button.Location = New System.Drawing.Point(484, 495)
        Me.SetFollow_Button.Name = "SetFollow_Button"
        Me.SetFollow_Button.Size = New System.Drawing.Size(75, 23)
        Me.SetFollow_Button.TabIndex = 25
        Me.SetFollow_Button.Text = "Select..."
        Me.SetFollow_Button.UseVisualStyleBackColor = True
        '
        'Link_Box
        '
        Me.Link_Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Link_Box.FormattingEnabled = True
        Me.Link_Box.Location = New System.Drawing.Point(83, 547)
        Me.Link_Box.Name = "Link_Box"
        Me.Link_Box.Size = New System.Drawing.Size(121, 21)
        Me.Link_Box.TabIndex = 27
        '
        'ChanceLabel
        '
        Me.ChanceLabel.AutoSize = True
        Me.ChanceLabel.Location = New System.Drawing.Point(80, 366)
        Me.ChanceLabel.Name = "ChanceLabel"
        Me.ChanceLabel.Size = New System.Drawing.Size(106, 13)
        Me.ChanceLabel.TabIndex = 13
        Me.ChanceLabel.Text = "Chance to occur (%):"
        '
        'MaxDurationLabel
        '
        Me.MaxDurationLabel.AutoSize = True
        Me.MaxDurationLabel.Location = New System.Drawing.Point(242, 366)
        Me.MaxDurationLabel.Name = "MaxDurationLabel"
        Me.MaxDurationLabel.Size = New System.Drawing.Size(151, 13)
        Me.MaxDurationLabel.TabIndex = 15
        Me.MaxDurationLabel.Text = "Maximum Duration in seconds:"
        '
        'MinDurationLabel
        '
        Me.MinDurationLabel.AutoSize = True
        Me.MinDurationLabel.Location = New System.Drawing.Point(435, 366)
        Me.MinDurationLabel.Name = "MinDurationLabel"
        Me.MinDurationLabel.Size = New System.Drawing.Size(150, 13)
        Me.MinDurationLabel.TabIndex = 17
        Me.MinDurationLabel.Text = "Minimum Duration in Seconds:"
        '
        'StartSpeechLabel
        '
        Me.StartSpeechLabel.AutoSize = True
        Me.StartSpeechLabel.Location = New System.Drawing.Point(71, 452)
        Me.StartSpeechLabel.Name = "StartSpeechLabel"
        Me.StartSpeechLabel.Size = New System.Drawing.Size(86, 13)
        Me.StartSpeechLabel.TabIndex = 19
        Me.StartSpeechLabel.Text = "Starting Speech:"
        '
        'EndSpeechLabel
        '
        Me.EndSpeechLabel.AutoSize = True
        Me.EndSpeechLabel.Location = New System.Drawing.Point(258, 452)
        Me.EndSpeechLabel.Name = "EndSpeechLabel"
        Me.EndSpeechLabel.Size = New System.Drawing.Size(83, 13)
        Me.EndSpeechLabel.TabIndex = 21
        Me.EndSpeechLabel.Text = "Ending Speech:"
        '
        'FollowLabel
        '
        Me.FollowLabel.AutoSize = True
        Me.FollowLabel.Location = New System.Drawing.Point(457, 452)
        Me.FollowLabel.Name = "FollowLabel"
        Me.FollowLabel.Size = New System.Drawing.Size(97, 13)
        Me.FollowLabel.TabIndex = 23
        Me.FollowLabel.Text = "Follow/Goto mode:"
        '
        'LinkLabel
        '
        Me.LinkLabel.AutoSize = True
        Me.LinkLabel.Location = New System.Drawing.Point(71, 531)
        Me.LinkLabel.Name = "LinkLabel"
        Me.LinkLabel.Size = New System.Drawing.Size(47, 13)
        Me.LinkLabel.TabIndex = 26
        Me.LinkLabel.Text = "Links to:"
        '
        'Speed_Box
        '
        Me.Speed_Box.Location = New System.Drawing.Point(485, 60)
        Me.Speed_Box.MaxLength = 5
        Me.Speed_Box.Name = "Speed_Box"
        Me.Speed_Box.Size = New System.Drawing.Size(100, 20)
        Me.Speed_Box.TabIndex = 5
        '
        'SpeedLabel
        '
        Me.SpeedLabel.AutoSize = True
        Me.SpeedLabel.Location = New System.Drawing.Point(458, 44)
        Me.SpeedLabel.Name = "SpeedLabel"
        Me.SpeedLabel.Size = New System.Drawing.Size(94, 13)
        Me.SpeedLabel.TabIndex = 4
        Me.SpeedLabel.Text = "Movement Speed:"
        '
        'DontRepeat_CheckBox
        '
        Me.DontRepeat_CheckBox.AutoSize = True
        Me.DontRepeat_CheckBox.Location = New System.Drawing.Point(261, 331)
        Me.DontRepeat_CheckBox.Name = "DontRepeat_CheckBox"
        Me.DontRepeat_CheckBox.Size = New System.Drawing.Size(137, 17)
        Me.DontRepeat_CheckBox.TabIndex = 12
        Me.DontRepeat_CheckBox.Text = "Don't repeat animations"
        Me.DontRepeat_CheckBox.UseVisualStyleBackColor = True
        '
        'DontRunRandomly_CheckBox
        '
        Me.DontRunRandomly_CheckBox.AutoSize = True
        Me.DontRunRandomly_CheckBox.Location = New System.Drawing.Point(268, 547)
        Me.DontRunRandomly_CheckBox.Name = "DontRunRandomly_CheckBox"
        Me.DontRunRandomly_CheckBox.Size = New System.Drawing.Size(361, 17)
        Me.DontRunRandomly_CheckBox.TabIndex = 28
        Me.DontRunRandomly_CheckBox.Text = "Don't run randomly (use when intended for a scripted event/interaction)"
        Me.DontRunRandomly_CheckBox.UseVisualStyleBackColor = True
        '
        'Group_Numberbox
        '
        Me.Group_Numberbox.Location = New System.Drawing.Point(381, 103)
        Me.Group_Numberbox.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.Group_Numberbox.Name = "Group_Numberbox"
        Me.Group_Numberbox.Size = New System.Drawing.Size(120, 20)
        Me.Group_Numberbox.TabIndex = 7
        '
        'GroupLabel
        '
        Me.GroupLabel.AutoSize = True
        Me.GroupLabel.Location = New System.Drawing.Point(71, 105)
        Me.GroupLabel.Name = "GroupLabel"
        Me.GroupLabel.Size = New System.Drawing.Size(286, 13)
        Me.GroupLabel.TabIndex = 6
        Me.GroupLabel.Text = "Behavior Group (leave at 0 to be used regardless of group):"
        '
        'NewBehaviorDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(655, 622)
        Me.Controls.Add(Me.GroupLabel)
        Me.Controls.Add(Me.Group_Numberbox)
        Me.Controls.Add(Me.DontRunRandomly_CheckBox)
        Me.Controls.Add(Me.DontRepeat_CheckBox)
        Me.Controls.Add(Me.SpeedLabel)
        Me.Controls.Add(Me.Speed_Box)
        Me.Controls.Add(Me.LinkLabel)
        Me.Controls.Add(Me.FollowLabel)
        Me.Controls.Add(Me.EndSpeechLabel)
        Me.Controls.Add(Me.StartSpeechLabel)
        Me.Controls.Add(Me.MinDurationLabel)
        Me.Controls.Add(Me.MaxDurationLabel)
        Me.Controls.Add(Me.ChanceLabel)
        Me.Controls.Add(Me.Link_Box)
        Me.Controls.Add(Me.SetFollow_Button)
        Me.Controls.Add(Me.Follow_Box)
        Me.Controls.Add(Me.EndSpeech_Box)
        Me.Controls.Add(Me.StartSpeech_Box)
        Me.Controls.Add(Me.Min_Box)
        Me.Controls.Add(Me.Max_Box)
        Me.Controls.Add(Me.Chance_Box)
        Me.Controls.Add(Me.MovementLabel)
        Me.Controls.Add(Me.Movement_Combobox)
        Me.Controls.Add(Me.Right_Image_Set_Button)
        Me.Controls.Add(Me.Left_Image_Set_Button)
        Me.Controls.Add(Me.RightImageLabel)
        Me.Controls.Add(Me.LeftImageLabel)
        Me.Controls.Add(Me.NameLabel)
        Me.Controls.Add(Me.NameTextbox)
        Me.Controls.Add(Me.Right_ImageBox)
        Me.Controls.Add(Me.Left_ImageBox)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewBehaviorDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create New Behavior..."
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Left_ImageBox As System.Windows.Forms.PictureBox
    Friend WithEvents Right_ImageBox As System.Windows.Forms.PictureBox
    Friend WithEvents NameTextbox As System.Windows.Forms.TextBox
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents LeftImageLabel As System.Windows.Forms.Label
    Friend WithEvents RightImageLabel As System.Windows.Forms.Label
    Friend WithEvents Left_Image_Set_Button As System.Windows.Forms.Button
    Friend WithEvents Right_Image_Set_Button As System.Windows.Forms.Button
    Friend WithEvents Movement_Combobox As System.Windows.Forms.ComboBox
    Friend WithEvents MovementLabel As System.Windows.Forms.Label
    Friend WithEvents Chance_Box As System.Windows.Forms.TextBox
    Friend WithEvents Max_Box As System.Windows.Forms.TextBox
    Friend WithEvents Min_Box As System.Windows.Forms.TextBox
    Friend WithEvents StartSpeech_Box As System.Windows.Forms.ComboBox
    Friend WithEvents EndSpeech_Box As System.Windows.Forms.ComboBox
    Friend WithEvents Follow_Box As System.Windows.Forms.TextBox
    Friend WithEvents SetFollow_Button As System.Windows.Forms.Button
    Friend WithEvents Link_Box As System.Windows.Forms.ComboBox
    Friend WithEvents ChanceLabel As System.Windows.Forms.Label
    Friend WithEvents MaxDurationLabel As System.Windows.Forms.Label
    Friend WithEvents MinDurationLabel As System.Windows.Forms.Label
    Friend WithEvents StartSpeechLabel As System.Windows.Forms.Label
    Friend WithEvents EndSpeechLabel As System.Windows.Forms.Label
    Friend WithEvents FollowLabel As System.Windows.Forms.Label
    Friend WithEvents LinkLabel As System.Windows.Forms.Label
    Friend WithEvents Speed_Box As System.Windows.Forms.TextBox
    Friend WithEvents SpeedLabel As System.Windows.Forms.Label
    Friend WithEvents DontRepeat_CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents DontRunRandomly_CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Group_Numberbox As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupLabel As System.Windows.Forms.Label

End Class

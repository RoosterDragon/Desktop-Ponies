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
        Me.Name_Textbox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Left_Image_Set_Button = New System.Windows.Forms.Button()
        Me.Right_Image_Set_Button = New System.Windows.Forms.Button()
        Me.Movement_Combobox = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Chance_Box = New System.Windows.Forms.TextBox()
        Me.Max_Box = New System.Windows.Forms.TextBox()
        Me.Min_Box = New System.Windows.Forms.TextBox()
        Me.StartSpeech_Box = New System.Windows.Forms.ComboBox()
        Me.EndSpeech_Box = New System.Windows.Forms.ComboBox()
        Me.Follow_Box = New System.Windows.Forms.TextBox()
        Me.SetFollow_Button = New System.Windows.Forms.Button()
        Me.Link_Box = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Speed_Box = New System.Windows.Forms.TextBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.DontRepeat_CheckBox = New System.Windows.Forms.CheckBox()
        Me.DontRunRandomly_CheckBox = New System.Windows.Forms.CheckBox()
        Me.Group_Numberbox = New System.Windows.Forms.NumericUpDown()
        Me.Label13 = New System.Windows.Forms.Label()
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
        'Name_Textbox
        '
        Me.Name_Textbox.Location = New System.Drawing.Point(87, 60)
        Me.Name_Textbox.Name = "Name_Textbox"
        Me.Name_Textbox.Size = New System.Drawing.Size(100, 20)
        Me.Name_Textbox.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(58, 44)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(83, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Behavior Name:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(80, 139)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Left Image:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(365, 139)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Right Image:"
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
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(243, 44)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 13)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "Movement:"
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
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(80, 366)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(106, 13)
        Me.Label5.TabIndex = 13
        Me.Label5.Text = "Chance to occur (%):"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(242, 366)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(151, 13)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "Maximum Duration in seconds:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(435, 366)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(150, 13)
        Me.Label7.TabIndex = 17
        Me.Label7.Text = "Minimum Duration in Seconds:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(71, 452)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(86, 13)
        Me.Label8.TabIndex = 19
        Me.Label8.Text = "Starting Speech:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(258, 452)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(83, 13)
        Me.Label9.TabIndex = 21
        Me.Label9.Text = "Ending Speech:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(457, 452)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(97, 13)
        Me.Label10.TabIndex = 23
        Me.Label10.Text = "Follow/Goto mode:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(71, 531)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(47, 13)
        Me.Label11.TabIndex = 26
        Me.Label11.Text = "Links to:"
        '
        'Speed_Box
        '
        Me.Speed_Box.Location = New System.Drawing.Point(485, 60)
        Me.Speed_Box.MaxLength = 5
        Me.Speed_Box.Name = "Speed_Box"
        Me.Speed_Box.Size = New System.Drawing.Size(100, 20)
        Me.Speed_Box.TabIndex = 5
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(458, 44)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(94, 13)
        Me.Label12.TabIndex = 4
        Me.Label12.Text = "Movement Speed:"
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
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(71, 105)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(286, 13)
        Me.Label13.TabIndex = 6
        Me.Label13.Text = "Behavior Group (leave at 0 to be used regardless of group):"
        '
        'NewBehaviorDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(655, 622)
        Me.Controls.Add(Me.Label13)
        Me.Controls.Add(Me.Group_Numberbox)
        Me.Controls.Add(Me.DontRunRandomly_CheckBox)
        Me.Controls.Add(Me.DontRepeat_CheckBox)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.Speed_Box)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Link_Box)
        Me.Controls.Add(Me.SetFollow_Button)
        Me.Controls.Add(Me.Follow_Box)
        Me.Controls.Add(Me.EndSpeech_Box)
        Me.Controls.Add(Me.StartSpeech_Box)
        Me.Controls.Add(Me.Min_Box)
        Me.Controls.Add(Me.Max_Box)
        Me.Controls.Add(Me.Chance_Box)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Movement_Combobox)
        Me.Controls.Add(Me.Right_Image_Set_Button)
        Me.Controls.Add(Me.Left_Image_Set_Button)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Name_Textbox)
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
    Friend WithEvents Name_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Left_Image_Set_Button As System.Windows.Forms.Button
    Friend WithEvents Right_Image_Set_Button As System.Windows.Forms.Button
    Friend WithEvents Movement_Combobox As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Chance_Box As System.Windows.Forms.TextBox
    Friend WithEvents Max_Box As System.Windows.Forms.TextBox
    Friend WithEvents Min_Box As System.Windows.Forms.TextBox
    Friend WithEvents StartSpeech_Box As System.Windows.Forms.ComboBox
    Friend WithEvents EndSpeech_Box As System.Windows.Forms.ComboBox
    Friend WithEvents Follow_Box As System.Windows.Forms.TextBox
    Friend WithEvents SetFollow_Button As System.Windows.Forms.Button
    Friend WithEvents Link_Box As System.Windows.Forms.ComboBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Speed_Box As System.Windows.Forms.TextBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents DontRepeat_CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents DontRunRandomly_CheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents Group_Numberbox As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label13 As System.Windows.Forms.Label

End Class

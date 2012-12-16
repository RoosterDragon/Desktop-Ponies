<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewEffectDialog
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
        Me.Duration_Box = New System.Windows.Forms.TextBox()
        Me.Behavior_Box = New System.Windows.Forms.ComboBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.repeat_box = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.follow_checkbox = New System.Windows.Forms.CheckBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.R_Placement_Box = New System.Windows.Forms.ComboBox()
        Me.L_Placement_Box = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.R_Centering_Box = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.L_Centering_Box = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.DontRepeat_CheckBox = New System.Windows.Forms.CheckBox()
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
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(323, 579)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(320, 29)
        Me.TableLayoutPanel1.TabIndex = 24
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
        Me.Left_ImageBox.Location = New System.Drawing.Point(109, 127)
        Me.Left_ImageBox.Name = "Left_ImageBox"
        Me.Left_ImageBox.Size = New System.Drawing.Size(150, 150)
        Me.Left_ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.Left_ImageBox.TabIndex = 1
        Me.Left_ImageBox.TabStop = False
        '
        'Right_ImageBox
        '
        Me.Right_ImageBox.Location = New System.Drawing.Point(389, 127)
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
        Me.Label1.Size = New System.Drawing.Size(69, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Effect Name:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(81, 109)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Left Image:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(366, 109)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Right Image:"
        '
        'Left_Image_Set_Button
        '
        Me.Left_Image_Set_Button.Location = New System.Drawing.Point(146, 283)
        Me.Left_Image_Set_Button.Name = "Left_Image_Set_Button"
        Me.Left_Image_Set_Button.Size = New System.Drawing.Size(75, 23)
        Me.Left_Image_Set_Button.TabIndex = 4
        Me.Left_Image_Set_Button.Text = "Select..."
        Me.Left_Image_Set_Button.UseVisualStyleBackColor = True
        '
        'Right_Image_Set_Button
        '
        Me.Right_Image_Set_Button.Location = New System.Drawing.Point(427, 283)
        Me.Right_Image_Set_Button.Name = "Right_Image_Set_Button"
        Me.Right_Image_Set_Button.Size = New System.Drawing.Size(75, 23)
        Me.Right_Image_Set_Button.TabIndex = 6
        Me.Right_Image_Set_Button.Text = "Select..."
        Me.Right_Image_Set_Button.UseVisualStyleBackColor = True
        '
        'Duration_Box
        '
        Me.Duration_Box.Location = New System.Drawing.Point(439, 363)
        Me.Duration_Box.Name = "Duration_Box"
        Me.Duration_Box.Size = New System.Drawing.Size(100, 20)
        Me.Duration_Box.TabIndex = 18
        '
        'Behavior_Box
        '
        Me.Behavior_Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Behavior_Box.FormattingEnabled = True
        Me.Behavior_Box.Location = New System.Drawing.Point(100, 363)
        Me.Behavior_Box.Name = "Behavior_Box"
        Me.Behavior_Box.Size = New System.Drawing.Size(121, 21)
        Me.Behavior_Box.TabIndex = 8
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(375, 347)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(104, 13)
        Me.Label6.TabIndex = 17
        Me.Label6.Text = "Duration in seconds:"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(81, 347)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(110, 13)
        Me.Label11.TabIndex = 7
        Me.Label11.Text = "Behavior that triggers:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(375, 386)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(229, 13)
        Me.Label4.TabIndex = 19
        Me.Label4.Text = "(Set to 0 if it should last until the behavior ends)"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(375, 426)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(129, 13)
        Me.Label5.TabIndex = 20
        Me.Label5.Text = "Repeat Delay in seconds:"
        '
        'repeat_box
        '
        Me.repeat_box.Location = New System.Drawing.Point(439, 442)
        Me.repeat_box.Name = "repeat_box"
        Me.repeat_box.Size = New System.Drawing.Size(100, 20)
        Me.repeat_box.TabIndex = 21
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(375, 465)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(133, 13)
        Me.Label7.TabIndex = 22
        Me.Label7.Text = "(Set to 0 to play only once)"
        '
        'follow_checkbox
        '
        Me.follow_checkbox.AutoSize = True
        Me.follow_checkbox.Location = New System.Drawing.Point(389, 62)
        Me.follow_checkbox.Name = "follow_checkbox"
        Me.follow_checkbox.Size = New System.Drawing.Size(153, 17)
        Me.follow_checkbox.TabIndex = 2
        Me.follow_checkbox.Text = "Effect graphic follows pony"
        Me.follow_checkbox.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(31, 402)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(228, 13)
        Me.Label8.TabIndex = 9
        Me.Label8.Text = "Image placement, when pony is heading Right:"
        '
        'R_Placement_Box
        '
        Me.R_Placement_Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.R_Placement_Box.FormattingEnabled = True
        Me.R_Placement_Box.Location = New System.Drawing.Point(100, 418)
        Me.R_Placement_Box.Name = "R_Placement_Box"
        Me.R_Placement_Box.Size = New System.Drawing.Size(121, 21)
        Me.R_Placement_Box.TabIndex = 10
        '
        'L_Placement_Box
        '
        Me.L_Placement_Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.L_Placement_Box.FormattingEnabled = True
        Me.L_Placement_Box.Location = New System.Drawing.Point(100, 475)
        Me.L_Placement_Box.Name = "L_Placement_Box"
        Me.L_Placement_Box.Size = New System.Drawing.Size(121, 21)
        Me.L_Placement_Box.TabIndex = 12
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(12, 512)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(262, 13)
        Me.Label10.TabIndex = 13
        Me.Label10.Text = "Image centering relative to pony, when heading Right:"
        '
        'R_Centering_Box
        '
        Me.R_Centering_Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.R_Centering_Box.FormattingEnabled = True
        Me.R_Centering_Box.Location = New System.Drawing.Point(100, 528)
        Me.R_Centering_Box.Name = "R_Centering_Box"
        Me.R_Centering_Box.Size = New System.Drawing.Size(121, 21)
        Me.R_Centering_Box.TabIndex = 14
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(12, 563)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(255, 13)
        Me.Label12.TabIndex = 15
        Me.Label12.Text = "Image centering relative to pony, when heading Left:"
        '
        'L_Centering_Box
        '
        Me.L_Centering_Box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.L_Centering_Box.FormattingEnabled = True
        Me.L_Centering_Box.Location = New System.Drawing.Point(100, 579)
        Me.L_Centering_Box.Name = "L_Centering_Box"
        Me.L_Centering_Box.Size = New System.Drawing.Size(121, 21)
        Me.L_Centering_Box.TabIndex = 16
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(31, 459)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(221, 13)
        Me.Label9.TabIndex = 11
        Me.Label9.Text = "Image placement, when pony is heading Left:"
        '
        'DontRepeat_CheckBox
        '
        Me.DontRepeat_CheckBox.AutoSize = True
        Me.DontRepeat_CheckBox.Location = New System.Drawing.Point(389, 512)
        Me.DontRepeat_CheckBox.Name = "DontRepeat_CheckBox"
        Me.DontRepeat_CheckBox.Size = New System.Drawing.Size(137, 17)
        Me.DontRepeat_CheckBox.TabIndex = 23
        Me.DontRepeat_CheckBox.Text = "Don't repeat animations"
        Me.DontRepeat_CheckBox.UseVisualStyleBackColor = True
        '
        'NewEffectDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(655, 620)
        Me.Controls.Add(Me.DontRepeat_CheckBox)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label12)
        Me.Controls.Add(Me.L_Centering_Box)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.R_Centering_Box)
        Me.Controls.Add(Me.L_Placement_Box)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.R_Placement_Box)
        Me.Controls.Add(Me.follow_checkbox)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.repeat_box)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label11)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Behavior_Box)
        Me.Controls.Add(Me.Duration_Box)
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
        Me.Name = "NewEffectDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create New Effect..."
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
    Friend WithEvents Duration_Box As System.Windows.Forms.TextBox
    Friend WithEvents Behavior_Box As System.Windows.Forms.ComboBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents repeat_box As System.Windows.Forms.TextBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents follow_checkbox As System.Windows.Forms.CheckBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents R_Placement_Box As System.Windows.Forms.ComboBox
    Friend WithEvents L_Placement_Box As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents R_Centering_Box As System.Windows.Forms.ComboBox
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents L_Centering_Box As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents DontRepeat_CheckBox As System.Windows.Forms.CheckBox

End Class

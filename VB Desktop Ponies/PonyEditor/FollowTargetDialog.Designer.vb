<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FollowTargetDialog
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Pony_Radio = New System.Windows.Forms.RadioButton()
        Me.Point_Radio = New System.Windows.Forms.RadioButton()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.units_label = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Point_Loc_Y = New System.Windows.Forms.NumericUpDown()
        Me.Point_Loc_X = New System.Windows.Forms.NumericUpDown()
        Me.Point_Preview_Image = New System.Windows.Forms.PictureBox()
        Me.Follow_ComboBox = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.relativeto_label = New System.Windows.Forms.Label()
        Me.DisableRadio = New System.Windows.Forms.RadioButton()
        Me.Auto_Select_Images_Checkbox = New System.Windows.Forms.CheckBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.stopped_behavior_box = New System.Windows.Forms.ComboBox()
        Me.moving_behavior_box = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 466)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(476, 29)
        Me.TableLayoutPanel1.TabIndex = 20
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Cancel_Button.Location = New System.Drawing.Point(71, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(96, 23)
        Me.Cancel_Button.TabIndex = 0
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.OK_Button.Location = New System.Drawing.Point(309, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(96, 23)
        Me.OK_Button.TabIndex = 1
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = True
        '
        'Pony_Radio
        '
        Me.Pony_Radio.AutoSize = True
        Me.Pony_Radio.Checked = True
        Me.Pony_Radio.Location = New System.Drawing.Point(154, 25)
        Me.Pony_Radio.Name = "Pony_Radio"
        Me.Pony_Radio.Size = New System.Drawing.Size(165, 17)
        Me.Pony_Radio.TabIndex = 1
        Me.Pony_Radio.TabStop = True
        Me.Pony_Radio.Text = "Follow another pony or effect."
        Me.Pony_Radio.UseVisualStyleBackColor = True
        '
        'Point_Radio
        '
        Me.Point_Radio.AutoSize = True
        Me.Point_Radio.Location = New System.Drawing.Point(154, 48)
        Me.Point_Radio.Name = "Point_Radio"
        Me.Point_Radio.Size = New System.Drawing.Size(157, 17)
        Me.Point_Radio.TabIndex = 2
        Me.Point_Radio.Text = "Go to a point on the screen."
        Me.Point_Radio.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(130, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "The pony should..."
        '
        'units_label
        '
        Me.units_label.AutoSize = True
        Me.units_label.Location = New System.Drawing.Point(24, 368)
        Me.units_label.Name = "units_label"
        Me.units_label.Size = New System.Drawing.Size(74, 13)
        Me.units_label.TabIndex = 15
        Me.units_label.Text = "Location (X,Y)"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(39, 319)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(179, 13)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "Point Relative to pony/effect center:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(296, 302)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(119, 13)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "Point selection preview:"
        '
        'Point_Loc_Y
        '
        Me.Point_Loc_Y.Location = New System.Drawing.Point(134, 384)
        Me.Point_Loc_Y.Name = "Point_Loc_Y"
        Me.Point_Loc_Y.Size = New System.Drawing.Size(46, 20)
        Me.Point_Loc_Y.TabIndex = 17
        '
        'Point_Loc_X
        '
        Me.Point_Loc_X.Location = New System.Drawing.Point(61, 384)
        Me.Point_Loc_X.Name = "Point_Loc_X"
        Me.Point_Loc_X.Size = New System.Drawing.Size(46, 20)
        Me.Point_Loc_X.TabIndex = 16
        '
        'Point_Preview_Image
        '
        Me.Point_Preview_Image.Location = New System.Drawing.Point(251, 328)
        Me.Point_Preview_Image.Name = "Point_Preview_Image"
        Me.Point_Preview_Image.Size = New System.Drawing.Size(227, 123)
        Me.Point_Preview_Image.TabIndex = 28
        Me.Point_Preview_Image.TabStop = False
        '
        'Follow_ComboBox
        '
        Me.Follow_ComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Follow_ComboBox.FormattingEnabled = True
        Me.Follow_ComboBox.Location = New System.Drawing.Point(96, 128)
        Me.Follow_ComboBox.Name = "Follow_ComboBox"
        Me.Follow_ComboBox.Size = New System.Drawing.Size(290, 21)
        Me.Follow_ComboBox.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(109, 112)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(109, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Pony/Effect to follow:"
        '
        'relativeto_label
        '
        Me.relativeto_label.AutoSize = True
        Me.relativeto_label.Location = New System.Drawing.Point(48, 427)
        Me.relativeto_label.Name = "relativeto_label"
        Me.relativeto_label.Size = New System.Drawing.Size(155, 13)
        Me.relativeto_label.TabIndex = 18
        Me.relativeto_label.Text = "(Relative to pony/effect center)"
        '
        'DisableRadio
        '
        Me.DisableRadio.AutoSize = True
        Me.DisableRadio.Location = New System.Drawing.Point(154, 71)
        Me.DisableRadio.Name = "DisableRadio"
        Me.DisableRadio.Size = New System.Drawing.Size(163, 17)
        Me.DisableRadio.TabIndex = 3
        Me.DisableRadio.Text = "Do nothing (disable following)"
        Me.DisableRadio.UseVisualStyleBackColor = True
        '
        'Auto_Select_Images_Checkbox
        '
        Me.Auto_Select_Images_Checkbox.AutoSize = True
        Me.Auto_Select_Images_Checkbox.Checked = True
        Me.Auto_Select_Images_Checkbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Auto_Select_Images_Checkbox.Location = New System.Drawing.Point(177, 172)
        Me.Auto_Select_Images_Checkbox.Name = "Auto_Select_Images_Checkbox"
        Me.Auto_Select_Images_Checkbox.Size = New System.Drawing.Size(118, 17)
        Me.Auto_Select_Images_Checkbox.TabIndex = 6
        Me.Auto_Select_Images_Checkbox.Text = "Auto Select Images"
        Me.Auto_Select_Images_Checkbox.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(93, 192)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(304, 13)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "When checked, this option automatically selects images out of "
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(67, 205)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(386, 13)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "all behaviors for the pony when following, depending on the direction and speed."
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(80, 218)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(349, 13)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "Uncheck this to manually select what behaviors to use for images below."
        '
        'stopped_behavior_box
        '
        Me.stopped_behavior_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.stopped_behavior_box.FormattingEnabled = True
        Me.stopped_behavior_box.Location = New System.Drawing.Point(42, 264)
        Me.stopped_behavior_box.Name = "stopped_behavior_box"
        Me.stopped_behavior_box.Size = New System.Drawing.Size(161, 21)
        Me.stopped_behavior_box.TabIndex = 11
        '
        'moving_behavior_box
        '
        Me.moving_behavior_box.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.moving_behavior_box.FormattingEnabled = True
        Me.moving_behavior_box.Location = New System.Drawing.Point(279, 264)
        Me.moving_behavior_box.Name = "moving_behavior_box"
        Me.moving_behavior_box.Size = New System.Drawing.Size(161, 21)
        Me.moving_behavior_box.TabIndex = 13
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(24, 248)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(144, 13)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Behavior for stopped images:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(248, 248)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(140, 13)
        Me.Label9.TabIndex = 12
        Me.Label9.Text = "Behavior for moving images:"
        '
        'FollowTargetDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(500, 507)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.moving_behavior_box)
        Me.Controls.Add(Me.stopped_behavior_box)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Auto_Select_Images_Checkbox)
        Me.Controls.Add(Me.DisableRadio)
        Me.Controls.Add(Me.relativeto_label)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Follow_ComboBox)
        Me.Controls.Add(Me.units_label)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Point_Loc_Y)
        Me.Controls.Add(Me.Point_Loc_X)
        Me.Controls.Add(Me.Point_Preview_Image)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Point_Radio)
        Me.Controls.Add(Me.Pony_Radio)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "FollowTargetDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select following parameters..."
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Pony_Radio As System.Windows.Forms.RadioButton
    Friend WithEvents Point_Radio As System.Windows.Forms.RadioButton
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents units_label As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Point_Loc_Y As System.Windows.Forms.NumericUpDown
    Friend WithEvents Point_Loc_X As System.Windows.Forms.NumericUpDown
    Friend WithEvents Point_Preview_Image As System.Windows.Forms.PictureBox
    Friend WithEvents Follow_ComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents relativeto_label As System.Windows.Forms.Label
    Friend WithEvents DisableRadio As System.Windows.Forms.RadioButton
    Friend WithEvents Auto_Select_Images_Checkbox As System.Windows.Forms.CheckBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents stopped_behavior_box As System.Windows.Forms.ComboBox
    Friend WithEvents moving_behavior_box As System.Windows.Forms.ComboBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label

End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImageCentersForm
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
        Me.Right_Image_Set_Button = New System.Windows.Forms.Button()
        Me.Left_Image_Set_Button = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Right_ImageBox = New System.Windows.Forms.PictureBox()
        Me.Left_ImageBox = New System.Windows.Forms.PictureBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.behavior_name_label = New System.Windows.Forms.Label()
        Me.Prev_Button = New System.Windows.Forms.Button()
        Me.Next_Button = New System.Windows.Forms.Button()
        Me.left_center_label = New System.Windows.Forms.Label()
        Me.right_center_label = New System.Windows.Forms.Label()
        Me.frame_slider = New System.Windows.Forms.TrackBar()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.frame_label = New System.Windows.Forms.Label()
        Me.BG_Black_Radio = New System.Windows.Forms.RadioButton()
        Me.BG_White_Radio = New System.Windows.Forms.RadioButton()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Right_Image_Set_Button
        '
        Me.Right_Image_Set_Button.Location = New System.Drawing.Point(611, 438)
        Me.Right_Image_Set_Button.Name = "Right_Image_Set_Button"
        Me.Right_Image_Set_Button.Size = New System.Drawing.Size(75, 23)
        Me.Right_Image_Set_Button.TabIndex = 10
        Me.Right_Image_Set_Button.Text = "Reset"
        Me.Right_Image_Set_Button.UseVisualStyleBackColor = True
        '
        'Left_Image_Set_Button
        '
        Me.Left_Image_Set_Button.Location = New System.Drawing.Point(57, 438)
        Me.Left_Image_Set_Button.Name = "Left_Image_Set_Button"
        Me.Left_Image_Set_Button.Size = New System.Drawing.Size(75, 23)
        Me.Left_Image_Set_Button.TabIndex = 4
        Me.Left_Image_Set_Button.Text = "Reset"
        Me.Left_Image_Set_Button.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(396, 114)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(67, 13)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "Right Image:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(40, 114)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Left Image:"
        '
        'Right_ImageBox
        '
        Me.Right_ImageBox.BackColor = System.Drawing.Color.Black
        Me.Right_ImageBox.Location = New System.Drawing.Point(399, 132)
        Me.Right_ImageBox.Name = "Right_ImageBox"
        Me.Right_ImageBox.Size = New System.Drawing.Size(300, 300)
        Me.Right_ImageBox.TabIndex = 10
        Me.Right_ImageBox.TabStop = False
        '
        'Left_ImageBox
        '
        Me.Left_ImageBox.BackColor = System.Drawing.Color.Black
        Me.Left_ImageBox.Location = New System.Drawing.Point(43, 132)
        Me.Left_ImageBox.Name = "Left_ImageBox"
        Me.Left_ImageBox.Size = New System.Drawing.Size(300, 300)
        Me.Left_ImageBox.TabIndex = 9
        Me.Left_ImageBox.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(201, 67)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(307, 20)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Click the center of the pony in each image:"
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.BackColor = System.Drawing.SystemColors.Control
        Me.OK_Button.Location = New System.Drawing.Point(632, 496)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 17
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(231, 31)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(75, 20)
        Me.Label4.TabIndex = 0
        Me.Label4.Text = "Behavior:"
        '
        'behavior_name_label
        '
        Me.behavior_name_label.AutoSize = True
        Me.behavior_name_label.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.behavior_name_label.Location = New System.Drawing.Point(373, 31)
        Me.behavior_name_label.Name = "behavior_name_label"
        Me.behavior_name_label.Size = New System.Drawing.Size(118, 20)
        Me.behavior_name_label.TabIndex = 1
        Me.behavior_name_label.Text = "behavior_name"
        '
        'Prev_Button
        '
        Me.Prev_Button.Location = New System.Drawing.Point(268, 438)
        Me.Prev_Button.Name = "Prev_Button"
        Me.Prev_Button.Size = New System.Drawing.Size(75, 23)
        Me.Prev_Button.TabIndex = 6
        Me.Prev_Button.Text = "Previous"
        Me.Prev_Button.UseVisualStyleBackColor = True
        '
        'Next_Button
        '
        Me.Next_Button.Location = New System.Drawing.Point(399, 438)
        Me.Next_Button.Name = "Next_Button"
        Me.Next_Button.Size = New System.Drawing.Size(75, 23)
        Me.Next_Button.TabIndex = 8
        Me.Next_Button.Text = "Next"
        Me.Next_Button.UseVisualStyleBackColor = True
        '
        'left_center_label
        '
        Me.left_center_label.AutoSize = True
        Me.left_center_label.Location = New System.Drawing.Point(163, 443)
        Me.left_center_label.Name = "left_center_label"
        Me.left_center_label.Size = New System.Drawing.Size(67, 13)
        Me.left_center_label.TabIndex = 5
        Me.left_center_label.Text = "Right Image:"
        '
        'right_center_label
        '
        Me.right_center_label.AutoSize = True
        Me.right_center_label.Location = New System.Drawing.Point(508, 443)
        Me.right_center_label.Name = "right_center_label"
        Me.right_center_label.Size = New System.Drawing.Size(67, 13)
        Me.right_center_label.TabIndex = 9
        Me.right_center_label.Text = "Right Image:"
        '
        'frame_slider
        '
        Me.frame_slider.LargeChange = 15
        Me.frame_slider.Location = New System.Drawing.Point(257, 474)
        Me.frame_slider.Maximum = 500
        Me.frame_slider.Name = "frame_slider"
        Me.frame_slider.Size = New System.Drawing.Size(217, 45)
        Me.frame_slider.TabIndex = 14
        Me.frame_slider.TickFrequency = 25
        Me.frame_slider.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(265, 509)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(133, 13)
        Me.Label5.TabIndex = 15
        Me.Label5.Text = "Animation Frame selection:"
        '
        'frame_label
        '
        Me.frame_label.AutoSize = True
        Me.frame_label.Location = New System.Drawing.Point(417, 509)
        Me.frame_label.Name = "frame_label"
        Me.frame_label.Size = New System.Drawing.Size(17, 13)
        Me.frame_label.TabIndex = 16
        Me.frame_label.Text = "xx"
        '
        'BG_Black_Radio
        '
        Me.BG_Black_Radio.AutoSize = True
        Me.BG_Black_Radio.Checked = True
        Me.BG_Black_Radio.Location = New System.Drawing.Point(112, 474)
        Me.BG_Black_Radio.Name = "BG_Black_Radio"
        Me.BG_Black_Radio.Size = New System.Drawing.Size(52, 17)
        Me.BG_Black_Radio.TabIndex = 12
        Me.BG_Black_Radio.TabStop = True
        Me.BG_Black_Radio.Text = "Black"
        Me.BG_Black_Radio.UseVisualStyleBackColor = True
        '
        'BG_White_Radio
        '
        Me.BG_White_Radio.AutoSize = True
        Me.BG_White_Radio.Location = New System.Drawing.Point(112, 502)
        Me.BG_White_Radio.Name = "BG_White_Radio"
        Me.BG_White_Radio.Size = New System.Drawing.Size(53, 17)
        Me.BG_White_Radio.TabIndex = 13
        Me.BG_White_Radio.Text = "White"
        Me.BG_White_Radio.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(12, 487)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(94, 13)
        Me.Label6.TabIndex = 11
        Me.Label6.Text = "Background color:"
        '
        'ImageCentersForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(745, 531)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.BG_White_Radio)
        Me.Controls.Add(Me.BG_Black_Radio)
        Me.Controls.Add(Me.frame_label)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.frame_slider)
        Me.Controls.Add(Me.right_center_label)
        Me.Controls.Add(Me.left_center_label)
        Me.Controls.Add(Me.Next_Button)
        Me.Controls.Add(Me.Prev_Button)
        Me.Controls.Add(Me.behavior_name_label)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.OK_Button)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Right_Image_Set_Button)
        Me.Controls.Add(Me.Left_Image_Set_Button)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Right_ImageBox)
        Me.Controls.Add(Me.Left_ImageBox)
        Me.Name = "ImageCentersForm"
        Me.Text = "Image Centers - Desktop Ponies"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Right_Image_Set_Button As System.Windows.Forms.Button
    Friend WithEvents Left_Image_Set_Button As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Right_ImageBox As System.Windows.Forms.PictureBox
    Friend WithEvents Left_ImageBox As System.Windows.Forms.PictureBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents behavior_name_label As System.Windows.Forms.Label
    Friend WithEvents Prev_Button As System.Windows.Forms.Button
    Friend WithEvents Next_Button As System.Windows.Forms.Button
    Friend WithEvents left_center_label As System.Windows.Forms.Label
    Friend WithEvents right_center_label As System.Windows.Forms.Label
    Friend WithEvents frame_slider As System.Windows.Forms.TrackBar
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents frame_label As System.Windows.Forms.Label
    Friend WithEvents BG_Black_Radio As System.Windows.Forms.RadioButton
    Friend WithEvents BG_White_Radio As System.Windows.Forms.RadioButton
    Friend WithEvents Label6 As System.Windows.Forms.Label
End Class

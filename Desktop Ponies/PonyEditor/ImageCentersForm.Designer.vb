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
        Me.RightImageResetButton = New System.Windows.Forms.Button()
        Me.LeftImageResetButton = New System.Windows.Forms.Button()
        Me.RightImageLabel = New System.Windows.Forms.Label()
        Me.LeftImageLabel = New System.Windows.Forms.Label()
        Me.RightImageBox = New System.Windows.Forms.PictureBox()
        Me.LeftImageBox = New System.Windows.Forms.PictureBox()
        Me.InfoLabel = New System.Windows.Forms.Label()
        Me.OKButton = New System.Windows.Forms.Button()
        Me.BehaviorLabel = New System.Windows.Forms.Label()
        Me.BehaviorNameLabel = New System.Windows.Forms.Label()
        Me.PreviousButton = New System.Windows.Forms.Button()
        Me.NextButton = New System.Windows.Forms.Button()
        Me.LeftCenterLabel = New System.Windows.Forms.Label()
        Me.RightCenterLabel = New System.Windows.Forms.Label()
        Me.FrameSlider = New System.Windows.Forms.TrackBar()
        Me.FrameLabel = New System.Windows.Forms.Label()
        Me.FrameIndexLabel = New System.Windows.Forms.Label()
        Me.BackgroundOptionBlack = New System.Windows.Forms.RadioButton()
        Me.BackgroundOptionWhite = New System.Windows.Forms.RadioButton()
        Me.BackgroundColorLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'RightImageResetButton
        '
        Me.RightImageResetButton.Location = New System.Drawing.Point(611, 438)
        Me.RightImageResetButton.Name = "RightImageResetButton"
        Me.RightImageResetButton.Size = New System.Drawing.Size(75, 23)
        Me.RightImageResetButton.TabIndex = 10
        Me.RightImageResetButton.Text = "Reset"
        Me.RightImageResetButton.UseVisualStyleBackColor = True
        '
        'LeftImageResetButton
        '
        Me.LeftImageResetButton.Location = New System.Drawing.Point(57, 438)
        Me.LeftImageResetButton.Name = "LeftImageResetButton"
        Me.LeftImageResetButton.Size = New System.Drawing.Size(75, 23)
        Me.LeftImageResetButton.TabIndex = 4
        Me.LeftImageResetButton.Text = "Reset"
        Me.LeftImageResetButton.UseVisualStyleBackColor = True
        '
        'RightImageLabel
        '
        Me.RightImageLabel.AutoSize = True
        Me.RightImageLabel.Location = New System.Drawing.Point(396, 114)
        Me.RightImageLabel.Name = "RightImageLabel"
        Me.RightImageLabel.Size = New System.Drawing.Size(67, 13)
        Me.RightImageLabel.TabIndex = 7
        Me.RightImageLabel.Text = "Right Image:"
        '
        'LeftImageLabel
        '
        Me.LeftImageLabel.AutoSize = True
        Me.LeftImageLabel.Location = New System.Drawing.Point(40, 114)
        Me.LeftImageLabel.Name = "LeftImageLabel"
        Me.LeftImageLabel.Size = New System.Drawing.Size(60, 13)
        Me.LeftImageLabel.TabIndex = 3
        Me.LeftImageLabel.Text = "Left Image:"
        '
        'RightImageBox
        '
        Me.RightImageBox.BackColor = System.Drawing.Color.Black
        Me.RightImageBox.Location = New System.Drawing.Point(399, 132)
        Me.RightImageBox.Name = "RightImageBox"
        Me.RightImageBox.Size = New System.Drawing.Size(300, 300)
        Me.RightImageBox.TabIndex = 10
        Me.RightImageBox.TabStop = False
        '
        'LeftImageBox
        '
        Me.LeftImageBox.BackColor = System.Drawing.Color.Black
        Me.LeftImageBox.Location = New System.Drawing.Point(43, 132)
        Me.LeftImageBox.Name = "LeftImageBox"
        Me.LeftImageBox.Size = New System.Drawing.Size(300, 300)
        Me.LeftImageBox.TabIndex = 9
        Me.LeftImageBox.TabStop = False
        '
        'InfoLabel
        '
        Me.InfoLabel.AutoSize = True
        Me.InfoLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InfoLabel.Location = New System.Drawing.Point(201, 67)
        Me.InfoLabel.Name = "InfoLabel"
        Me.InfoLabel.Size = New System.Drawing.Size(307, 20)
        Me.InfoLabel.TabIndex = 2
        Me.InfoLabel.Text = "Click the center of the pony in each image:"
        '
        'OKButton
        '
        Me.OKButton.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OKButton.BackColor = System.Drawing.SystemColors.Control
        Me.OKButton.Location = New System.Drawing.Point(632, 496)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(67, 23)
        Me.OKButton.TabIndex = 17
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = False
        '
        'BehaviorLabel
        '
        Me.BehaviorLabel.AutoSize = True
        Me.BehaviorLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BehaviorLabel.Location = New System.Drawing.Point(231, 31)
        Me.BehaviorLabel.Name = "BehaviorLabel"
        Me.BehaviorLabel.Size = New System.Drawing.Size(75, 20)
        Me.BehaviorLabel.TabIndex = 0
        Me.BehaviorLabel.Text = "Behavior:"
        '
        'BehaviorNameLabel
        '
        Me.BehaviorNameLabel.AutoSize = True
        Me.BehaviorNameLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BehaviorNameLabel.Location = New System.Drawing.Point(373, 31)
        Me.BehaviorNameLabel.Name = "BehaviorNameLabel"
        Me.BehaviorNameLabel.Size = New System.Drawing.Size(118, 20)
        Me.BehaviorNameLabel.TabIndex = 1
        Me.BehaviorNameLabel.Text = "behavior_name"
        '
        'PreviousButton
        '
        Me.PreviousButton.Location = New System.Drawing.Point(268, 438)
        Me.PreviousButton.Name = "PreviousButton"
        Me.PreviousButton.Size = New System.Drawing.Size(75, 23)
        Me.PreviousButton.TabIndex = 6
        Me.PreviousButton.Text = "Previous"
        Me.PreviousButton.UseVisualStyleBackColor = True
        '
        'NextButton
        '
        Me.NextButton.Location = New System.Drawing.Point(399, 438)
        Me.NextButton.Name = "NextButton"
        Me.NextButton.Size = New System.Drawing.Size(75, 23)
        Me.NextButton.TabIndex = 8
        Me.NextButton.Text = "Next"
        Me.NextButton.UseVisualStyleBackColor = True
        '
        'LeftCenterLabel
        '
        Me.LeftCenterLabel.AutoSize = True
        Me.LeftCenterLabel.Location = New System.Drawing.Point(163, 443)
        Me.LeftCenterLabel.Name = "LeftCenterLabel"
        Me.LeftCenterLabel.Size = New System.Drawing.Size(67, 13)
        Me.LeftCenterLabel.TabIndex = 5
        Me.LeftCenterLabel.Text = "Right Image:"
        '
        'RightCenterLabel
        '
        Me.RightCenterLabel.AutoSize = True
        Me.RightCenterLabel.Location = New System.Drawing.Point(508, 443)
        Me.RightCenterLabel.Name = "RightCenterLabel"
        Me.RightCenterLabel.Size = New System.Drawing.Size(67, 13)
        Me.RightCenterLabel.TabIndex = 9
        Me.RightCenterLabel.Text = "Right Image:"
        '
        'FrameSlider
        '
        Me.FrameSlider.LargeChange = 15
        Me.FrameSlider.Location = New System.Drawing.Point(257, 474)
        Me.FrameSlider.Maximum = 500
        Me.FrameSlider.Name = "FrameSlider"
        Me.FrameSlider.Size = New System.Drawing.Size(217, 45)
        Me.FrameSlider.TabIndex = 14
        Me.FrameSlider.TickFrequency = 25
        Me.FrameSlider.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        '
        'FrameLabel
        '
        Me.FrameLabel.AutoSize = True
        Me.FrameLabel.Location = New System.Drawing.Point(265, 509)
        Me.FrameLabel.Name = "FrameLabel"
        Me.FrameLabel.Size = New System.Drawing.Size(133, 13)
        Me.FrameLabel.TabIndex = 15
        Me.FrameLabel.Text = "Animation Frame selection:"
        '
        'FrameIndexLabel
        '
        Me.FrameIndexLabel.AutoSize = True
        Me.FrameIndexLabel.Location = New System.Drawing.Point(417, 509)
        Me.FrameIndexLabel.Name = "FrameIndexLabel"
        Me.FrameIndexLabel.Size = New System.Drawing.Size(17, 13)
        Me.FrameIndexLabel.TabIndex = 16
        Me.FrameIndexLabel.Text = "xx"
        '
        'BackgroundOptionBlack
        '
        Me.BackgroundOptionBlack.AutoSize = True
        Me.BackgroundOptionBlack.Checked = True
        Me.BackgroundOptionBlack.Location = New System.Drawing.Point(112, 474)
        Me.BackgroundOptionBlack.Name = "BackgroundOptionBlack"
        Me.BackgroundOptionBlack.Size = New System.Drawing.Size(52, 17)
        Me.BackgroundOptionBlack.TabIndex = 12
        Me.BackgroundOptionBlack.TabStop = True
        Me.BackgroundOptionBlack.Text = "Black"
        Me.BackgroundOptionBlack.UseVisualStyleBackColor = True
        '
        'BackgroundOptionWhite
        '
        Me.BackgroundOptionWhite.AutoSize = True
        Me.BackgroundOptionWhite.Location = New System.Drawing.Point(112, 502)
        Me.BackgroundOptionWhite.Name = "BackgroundOptionWhite"
        Me.BackgroundOptionWhite.Size = New System.Drawing.Size(53, 17)
        Me.BackgroundOptionWhite.TabIndex = 13
        Me.BackgroundOptionWhite.Text = "White"
        Me.BackgroundOptionWhite.UseVisualStyleBackColor = True
        '
        'BackgroundColorLabel
        '
        Me.BackgroundColorLabel.AutoSize = True
        Me.BackgroundColorLabel.Location = New System.Drawing.Point(12, 487)
        Me.BackgroundColorLabel.Name = "BackgroundColorLabel"
        Me.BackgroundColorLabel.Size = New System.Drawing.Size(94, 13)
        Me.BackgroundColorLabel.TabIndex = 11
        Me.BackgroundColorLabel.Text = "Background color:"
        '
        'ImageCentersForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(745, 531)
        Me.Controls.Add(Me.BackgroundColorLabel)
        Me.Controls.Add(Me.BackgroundOptionWhite)
        Me.Controls.Add(Me.BackgroundOptionBlack)
        Me.Controls.Add(Me.FrameIndexLabel)
        Me.Controls.Add(Me.FrameLabel)
        Me.Controls.Add(Me.FrameSlider)
        Me.Controls.Add(Me.RightCenterLabel)
        Me.Controls.Add(Me.LeftCenterLabel)
        Me.Controls.Add(Me.NextButton)
        Me.Controls.Add(Me.PreviousButton)
        Me.Controls.Add(Me.BehaviorNameLabel)
        Me.Controls.Add(Me.BehaviorLabel)
        Me.Controls.Add(Me.OKButton)
        Me.Controls.Add(Me.InfoLabel)
        Me.Controls.Add(Me.RightImageResetButton)
        Me.Controls.Add(Me.LeftImageResetButton)
        Me.Controls.Add(Me.RightImageLabel)
        Me.Controls.Add(Me.LeftImageLabel)
        Me.Controls.Add(Me.RightImageBox)
        Me.Controls.Add(Me.LeftImageBox)
        Me.Name = "ImageCentersForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Image Centers - Desktop Ponies"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RightImageResetButton As System.Windows.Forms.Button
    Friend WithEvents LeftImageResetButton As System.Windows.Forms.Button
    Friend WithEvents RightImageLabel As System.Windows.Forms.Label
    Friend WithEvents LeftImageLabel As System.Windows.Forms.Label
    Friend WithEvents RightImageBox As System.Windows.Forms.PictureBox
    Friend WithEvents LeftImageBox As System.Windows.Forms.PictureBox
    Friend WithEvents InfoLabel As System.Windows.Forms.Label
    Friend WithEvents OKButton As System.Windows.Forms.Button
    Friend WithEvents BehaviorLabel As System.Windows.Forms.Label
    Friend WithEvents BehaviorNameLabel As System.Windows.Forms.Label
    Friend WithEvents PreviousButton As System.Windows.Forms.Button
    Friend WithEvents NextButton As System.Windows.Forms.Button
    Friend WithEvents LeftCenterLabel As System.Windows.Forms.Label
    Friend WithEvents RightCenterLabel As System.Windows.Forms.Label
    Friend WithEvents FrameSlider As System.Windows.Forms.TrackBar
    Friend WithEvents FrameLabel As System.Windows.Forms.Label
    Friend WithEvents FrameIndexLabel As System.Windows.Forms.Label
    Friend WithEvents BackgroundOptionBlack As System.Windows.Forms.RadioButton
    Friend WithEvents BackgroundOptionWhite As System.Windows.Forms.RadioButton
    Friend WithEvents BackgroundColorLabel As System.Windows.Forms.Label
End Class

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
        Me.FrameSlider = New System.Windows.Forms.TrackBar()
        Me.FrameLabel = New System.Windows.Forms.Label()
        Me.FrameIndexLabel = New System.Windows.Forms.Label()
        Me.BackgroundOptionBlack = New System.Windows.Forms.RadioButton()
        Me.BackgroundOptionWhite = New System.Windows.Forms.RadioButton()
        Me.BackgroundColorLabel = New System.Windows.Forms.Label()
        Me.LayoutTable = New System.Windows.Forms.TableLayoutPanel()
        Me.ControlsPanel = New System.Windows.Forms.Panel()
        Me.LeftImageResetButton = New System.Windows.Forms.Button()
        Me.LeftCenterLabel = New System.Windows.Forms.Label()
        Me.LeftCenterX = New System.Windows.Forms.NumericUpDown()
        Me.LeftCenterY = New System.Windows.Forms.NumericUpDown()
        Me.LeftCenterPanel = New System.Windows.Forms.Panel()
        Me.RightImageResetButton = New System.Windows.Forms.Button()
        Me.RightCenterLabel = New System.Windows.Forms.Label()
        Me.RightCenterX = New System.Windows.Forms.NumericUpDown()
        Me.RightCenterY = New System.Windows.Forms.NumericUpDown()
        Me.RightCenterPanel = New System.Windows.Forms.Panel()
        Me.LeftImageMirrorButton = New System.Windows.Forms.Button()
        Me.RightImageMirrorButton = New System.Windows.Forms.Button()
        Me.LayoutTable.SuspendLayout()
        Me.ControlsPanel.SuspendLayout()
        Me.LeftCenterPanel.SuspendLayout()
        Me.RightCenterPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'RightImageLabel
        '
        Me.RightImageLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.RightImageLabel.AutoSize = True
        Me.RightImageLabel.Location = New System.Drawing.Point(370, 20)
        Me.RightImageLabel.Name = "RightImageLabel"
        Me.RightImageLabel.Size = New System.Drawing.Size(67, 13)
        Me.RightImageLabel.TabIndex = 3
        Me.RightImageLabel.Text = "Right Image:"
        Me.RightImageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'LeftImageLabel
        '
        Me.LeftImageLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.LeftImageLabel.AutoSize = True
        Me.LeftImageLabel.Location = New System.Drawing.Point(3, 20)
        Me.LeftImageLabel.Name = "LeftImageLabel"
        Me.LeftImageLabel.Size = New System.Drawing.Size(60, 13)
        Me.LeftImageLabel.TabIndex = 1
        Me.LeftImageLabel.Text = "Left Image:"
        Me.LeftImageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'RightImageBox
        '
        Me.RightImageBox.BackColor = System.Drawing.Color.Black
        Me.RightImageBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RightImageBox.Location = New System.Drawing.Point(370, 36)
        Me.RightImageBox.Name = "RightImageBox"
        Me.RightImageBox.Size = New System.Drawing.Size(361, 303)
        Me.RightImageBox.TabIndex = 10
        Me.RightImageBox.TabStop = False
        '
        'LeftImageBox
        '
        Me.LeftImageBox.BackColor = System.Drawing.Color.Black
        Me.LeftImageBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LeftImageBox.Location = New System.Drawing.Point(3, 36)
        Me.LeftImageBox.Name = "LeftImageBox"
        Me.LeftImageBox.Size = New System.Drawing.Size(361, 303)
        Me.LeftImageBox.TabIndex = 9
        Me.LeftImageBox.TabStop = False
        '
        'InfoLabel
        '
        Me.InfoLabel.AutoSize = True
        Me.LayoutTable.SetColumnSpan(Me.InfoLabel, 2)
        Me.InfoLabel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InfoLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InfoLabel.Location = New System.Drawing.Point(3, 0)
        Me.InfoLabel.Name = "InfoLabel"
        Me.InfoLabel.Size = New System.Drawing.Size(728, 20)
        Me.InfoLabel.TabIndex = 0
        Me.InfoLabel.Text = "Click the center of the pony in each image" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'OKButton
        '
        Me.OKButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.OKButton.BackColor = System.Drawing.SystemColors.Control
        Me.OKButton.Location = New System.Drawing.Point(658, 11)
        Me.OKButton.Name = "OKButton"
        Me.OKButton.Size = New System.Drawing.Size(67, 23)
        Me.OKButton.TabIndex = 6
        Me.OKButton.Text = "OK"
        Me.OKButton.UseVisualStyleBackColor = False
        '
        'BehaviorLabel
        '
        Me.BehaviorLabel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.BehaviorLabel.AutoSize = True
        Me.BehaviorLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BehaviorLabel.Location = New System.Drawing.Point(289, 406)
        Me.BehaviorLabel.Name = "BehaviorLabel"
        Me.BehaviorLabel.Size = New System.Drawing.Size(75, 20)
        Me.BehaviorLabel.TabIndex = 5
        Me.BehaviorLabel.Text = "Behavior:"
        '
        'BehaviorNameLabel
        '
        Me.BehaviorNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.BehaviorNameLabel.AutoSize = True
        Me.BehaviorNameLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.BehaviorNameLabel.Location = New System.Drawing.Point(370, 406)
        Me.BehaviorNameLabel.Name = "BehaviorNameLabel"
        Me.BehaviorNameLabel.Size = New System.Drawing.Size(118, 20)
        Me.BehaviorNameLabel.TabIndex = 6
        Me.BehaviorNameLabel.Text = "behavior_name"
        '
        'PreviousButton
        '
        Me.PreviousButton.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.PreviousButton.Location = New System.Drawing.Point(289, 429)
        Me.PreviousButton.Name = "PreviousButton"
        Me.PreviousButton.Size = New System.Drawing.Size(75, 23)
        Me.PreviousButton.TabIndex = 7
        Me.PreviousButton.Text = "Previous"
        Me.PreviousButton.UseVisualStyleBackColor = True
        '
        'NextButton
        '
        Me.NextButton.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.NextButton.Location = New System.Drawing.Point(370, 429)
        Me.NextButton.Name = "NextButton"
        Me.NextButton.Size = New System.Drawing.Size(75, 23)
        Me.NextButton.TabIndex = 8
        Me.NextButton.Text = "Next"
        Me.NextButton.UseVisualStyleBackColor = True
        '
        'FrameSlider
        '
        Me.FrameSlider.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.FrameSlider.Location = New System.Drawing.Point(258, 3)
        Me.FrameSlider.Name = "FrameSlider"
        Me.FrameSlider.Size = New System.Drawing.Size(217, 45)
        Me.FrameSlider.TabIndex = 3
        Me.FrameSlider.TickFrequency = 5
        Me.FrameSlider.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        '
        'FrameLabel
        '
        Me.FrameLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.FrameLabel.AutoSize = True
        Me.FrameLabel.Location = New System.Drawing.Point(266, 38)
        Me.FrameLabel.Name = "FrameLabel"
        Me.FrameLabel.Size = New System.Drawing.Size(133, 13)
        Me.FrameLabel.TabIndex = 4
        Me.FrameLabel.Text = "Animation Frame selection:"
        '
        'FrameIndexLabel
        '
        Me.FrameIndexLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.FrameIndexLabel.AutoSize = True
        Me.FrameIndexLabel.Location = New System.Drawing.Point(418, 38)
        Me.FrameIndexLabel.Name = "FrameIndexLabel"
        Me.FrameIndexLabel.Size = New System.Drawing.Size(17, 13)
        Me.FrameIndexLabel.TabIndex = 5
        Me.FrameIndexLabel.Text = "xx"
        '
        'BackgroundOptionBlack
        '
        Me.BackgroundOptionBlack.AutoSize = True
        Me.BackgroundOptionBlack.Checked = True
        Me.BackgroundOptionBlack.Location = New System.Drawing.Point(103, 3)
        Me.BackgroundOptionBlack.Name = "BackgroundOptionBlack"
        Me.BackgroundOptionBlack.Size = New System.Drawing.Size(52, 17)
        Me.BackgroundOptionBlack.TabIndex = 1
        Me.BackgroundOptionBlack.TabStop = True
        Me.BackgroundOptionBlack.Text = "Black"
        Me.BackgroundOptionBlack.UseVisualStyleBackColor = True
        '
        'BackgroundOptionWhite
        '
        Me.BackgroundOptionWhite.AutoSize = True
        Me.BackgroundOptionWhite.Location = New System.Drawing.Point(103, 26)
        Me.BackgroundOptionWhite.Name = "BackgroundOptionWhite"
        Me.BackgroundOptionWhite.Size = New System.Drawing.Size(53, 17)
        Me.BackgroundOptionWhite.TabIndex = 2
        Me.BackgroundOptionWhite.Text = "White"
        Me.BackgroundOptionWhite.UseVisualStyleBackColor = True
        '
        'BackgroundColorLabel
        '
        Me.BackgroundColorLabel.AutoSize = True
        Me.BackgroundColorLabel.Location = New System.Drawing.Point(3, 16)
        Me.BackgroundColorLabel.Name = "BackgroundColorLabel"
        Me.BackgroundColorLabel.Size = New System.Drawing.Size(94, 13)
        Me.BackgroundColorLabel.TabIndex = 0
        Me.BackgroundColorLabel.Text = "Background color:"
        '
        'LayoutTable
        '
        Me.LayoutTable.ColumnCount = 2
        Me.LayoutTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.LayoutTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.LayoutTable.Controls.Add(Me.ControlsPanel, 0, 6)
        Me.LayoutTable.Controls.Add(Me.RightCenterPanel, 1, 3)
        Me.LayoutTable.Controls.Add(Me.LeftCenterPanel, 0, 3)
        Me.LayoutTable.Controls.Add(Me.InfoLabel, 0, 0)
        Me.LayoutTable.Controls.Add(Me.LeftImageLabel, 0, 1)
        Me.LayoutTable.Controls.Add(Me.RightImageLabel, 1, 1)
        Me.LayoutTable.Controls.Add(Me.RightImageBox, 1, 2)
        Me.LayoutTable.Controls.Add(Me.LeftImageBox, 0, 2)
        Me.LayoutTable.Controls.Add(Me.PreviousButton, 0, 5)
        Me.LayoutTable.Controls.Add(Me.BehaviorLabel, 0, 4)
        Me.LayoutTable.Controls.Add(Me.NextButton, 1, 5)
        Me.LayoutTable.Controls.Add(Me.BehaviorNameLabel, 1, 4)
        Me.LayoutTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutTable.Location = New System.Drawing.Point(0, 0)
        Me.LayoutTable.Name = "LayoutTable"
        Me.LayoutTable.RowCount = 7
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.LayoutTable.Size = New System.Drawing.Size(734, 512)
        Me.LayoutTable.TabIndex = 0
        '
        'ControlsPanel
        '
        Me.ControlsPanel.AutoSize = True
        Me.ControlsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.LayoutTable.SetColumnSpan(Me.ControlsPanel, 2)
        Me.ControlsPanel.Controls.Add(Me.BackgroundColorLabel)
        Me.ControlsPanel.Controls.Add(Me.BackgroundOptionBlack)
        Me.ControlsPanel.Controls.Add(Me.OKButton)
        Me.ControlsPanel.Controls.Add(Me.FrameIndexLabel)
        Me.ControlsPanel.Controls.Add(Me.BackgroundOptionWhite)
        Me.ControlsPanel.Controls.Add(Me.FrameLabel)
        Me.ControlsPanel.Controls.Add(Me.FrameSlider)
        Me.ControlsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ControlsPanel.Location = New System.Drawing.Point(3, 458)
        Me.ControlsPanel.Name = "ControlsPanel"
        Me.ControlsPanel.Size = New System.Drawing.Size(728, 51)
        Me.ControlsPanel.TabIndex = 9
        '
        'LeftImageResetButton
        '
        Me.LeftImageResetButton.Location = New System.Drawing.Point(3, 32)
        Me.LeftImageResetButton.Name = "LeftImageResetButton"
        Me.LeftImageResetButton.Size = New System.Drawing.Size(75, 23)
        Me.LeftImageResetButton.TabIndex = 3
        Me.LeftImageResetButton.Text = "Reset"
        Me.LeftImageResetButton.UseVisualStyleBackColor = True
        '
        'LeftCenterLabel
        '
        Me.LeftCenterLabel.AutoSize = True
        Me.LeftCenterLabel.Location = New System.Drawing.Point(3, 8)
        Me.LeftCenterLabel.Name = "LeftCenterLabel"
        Me.LeftCenterLabel.Size = New System.Drawing.Size(62, 13)
        Me.LeftCenterLabel.TabIndex = 0
        Me.LeftCenterLabel.Text = "Left Center:"
        '
        'LeftCenterX
        '
        Me.LeftCenterX.Location = New System.Drawing.Point(71, 6)
        Me.LeftCenterX.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.LeftCenterX.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.LeftCenterX.Name = "LeftCenterX"
        Me.LeftCenterX.Size = New System.Drawing.Size(60, 20)
        Me.LeftCenterX.TabIndex = 1
        '
        'LeftCenterY
        '
        Me.LeftCenterY.Location = New System.Drawing.Point(137, 6)
        Me.LeftCenterY.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.LeftCenterY.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.LeftCenterY.Name = "LeftCenterY"
        Me.LeftCenterY.Size = New System.Drawing.Size(60, 20)
        Me.LeftCenterY.TabIndex = 2
        '
        'LeftCenterPanel
        '
        Me.LeftCenterPanel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.LeftCenterPanel.AutoSize = True
        Me.LeftCenterPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.LeftCenterPanel.Controls.Add(Me.LeftImageMirrorButton)
        Me.LeftCenterPanel.Controls.Add(Me.LeftImageResetButton)
        Me.LeftCenterPanel.Controls.Add(Me.LeftCenterY)
        Me.LeftCenterPanel.Controls.Add(Me.LeftCenterX)
        Me.LeftCenterPanel.Controls.Add(Me.LeftCenterLabel)
        Me.LeftCenterPanel.Location = New System.Drawing.Point(83, 345)
        Me.LeftCenterPanel.Name = "LeftCenterPanel"
        Me.LeftCenterPanel.Size = New System.Drawing.Size(200, 58)
        Me.LeftCenterPanel.TabIndex = 2
        '
        'RightImageResetButton
        '
        Me.RightImageResetButton.Location = New System.Drawing.Point(3, 32)
        Me.RightImageResetButton.Name = "RightImageResetButton"
        Me.RightImageResetButton.Size = New System.Drawing.Size(75, 23)
        Me.RightImageResetButton.TabIndex = 3
        Me.RightImageResetButton.Text = "Reset"
        Me.RightImageResetButton.UseVisualStyleBackColor = True
        '
        'RightCenterLabel
        '
        Me.RightCenterLabel.AutoSize = True
        Me.RightCenterLabel.Location = New System.Drawing.Point(3, 8)
        Me.RightCenterLabel.Name = "RightCenterLabel"
        Me.RightCenterLabel.Size = New System.Drawing.Size(69, 13)
        Me.RightCenterLabel.TabIndex = 0
        Me.RightCenterLabel.Text = "Right Center:"
        '
        'RightCenterX
        '
        Me.RightCenterX.Location = New System.Drawing.Point(78, 6)
        Me.RightCenterX.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.RightCenterX.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.RightCenterX.Name = "RightCenterX"
        Me.RightCenterX.Size = New System.Drawing.Size(60, 20)
        Me.RightCenterX.TabIndex = 1
        '
        'RightCenterY
        '
        Me.RightCenterY.Location = New System.Drawing.Point(144, 6)
        Me.RightCenterY.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.RightCenterY.Minimum = New Decimal(New Integer() {10000, 0, 0, -2147483648})
        Me.RightCenterY.Name = "RightCenterY"
        Me.RightCenterY.Size = New System.Drawing.Size(60, 20)
        Me.RightCenterY.TabIndex = 2
        '
        'RightCenterPanel
        '
        Me.RightCenterPanel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.RightCenterPanel.AutoSize = True
        Me.RightCenterPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.RightCenterPanel.Controls.Add(Me.RightImageMirrorButton)
        Me.RightCenterPanel.Controls.Add(Me.RightCenterY)
        Me.RightCenterPanel.Controls.Add(Me.RightCenterX)
        Me.RightCenterPanel.Controls.Add(Me.RightCenterLabel)
        Me.RightCenterPanel.Controls.Add(Me.RightImageResetButton)
        Me.RightCenterPanel.Location = New System.Drawing.Point(447, 345)
        Me.RightCenterPanel.Name = "RightCenterPanel"
        Me.RightCenterPanel.Size = New System.Drawing.Size(207, 58)
        Me.RightCenterPanel.TabIndex = 4
        '
        'LeftImageMirrorButton
        '
        Me.LeftImageMirrorButton.Location = New System.Drawing.Point(97, 32)
        Me.LeftImageMirrorButton.Name = "LeftImageMirrorButton"
        Me.LeftImageMirrorButton.Size = New System.Drawing.Size(100, 23)
        Me.LeftImageMirrorButton.TabIndex = 4
        Me.LeftImageMirrorButton.Text = "Mirror to Right"
        Me.LeftImageMirrorButton.UseVisualStyleBackColor = True
        '
        'RightImageMirrorButton
        '
        Me.RightImageMirrorButton.Location = New System.Drawing.Point(104, 32)
        Me.RightImageMirrorButton.Name = "RightImageMirrorButton"
        Me.RightImageMirrorButton.Size = New System.Drawing.Size(100, 23)
        Me.RightImageMirrorButton.TabIndex = 4
        Me.RightImageMirrorButton.Text = "Mirror to Left"
        Me.RightImageMirrorButton.UseVisualStyleBackColor = True
        '
        'ImageCentersForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(734, 512)
        Me.Controls.Add(Me.LayoutTable)
        Me.MinimumSize = New System.Drawing.Size(550, 350)
        Me.Name = "ImageCentersForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Image Centers - Desktop Ponies"
        Me.LayoutTable.ResumeLayout(False)
        Me.LayoutTable.PerformLayout()
        Me.ControlsPanel.ResumeLayout(False)
        Me.ControlsPanel.PerformLayout()
        Me.LeftCenterPanel.ResumeLayout(False)
        Me.LeftCenterPanel.PerformLayout()
        Me.RightCenterPanel.ResumeLayout(False)
        Me.RightCenterPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
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
    Friend WithEvents FrameSlider As System.Windows.Forms.TrackBar
    Friend WithEvents FrameLabel As System.Windows.Forms.Label
    Friend WithEvents FrameIndexLabel As System.Windows.Forms.Label
    Friend WithEvents BackgroundOptionBlack As System.Windows.Forms.RadioButton
    Friend WithEvents BackgroundOptionWhite As System.Windows.Forms.RadioButton
    Friend WithEvents BackgroundColorLabel As System.Windows.Forms.Label
    Friend WithEvents LayoutTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ControlsPanel As System.Windows.Forms.Panel
    Friend WithEvents RightCenterPanel As System.Windows.Forms.Panel
    Friend WithEvents RightCenterY As System.Windows.Forms.NumericUpDown
    Friend WithEvents RightCenterX As System.Windows.Forms.NumericUpDown
    Friend WithEvents RightCenterLabel As System.Windows.Forms.Label
    Friend WithEvents RightImageResetButton As System.Windows.Forms.Button
    Friend WithEvents LeftCenterPanel As System.Windows.Forms.Panel
    Friend WithEvents LeftCenterY As System.Windows.Forms.NumericUpDown
    Friend WithEvents LeftCenterX As System.Windows.Forms.NumericUpDown
    Friend WithEvents LeftCenterLabel As System.Windows.Forms.Label
    Friend WithEvents LeftImageResetButton As System.Windows.Forms.Button
    Friend WithEvents RightImageMirrorButton As System.Windows.Forms.Button
    Friend WithEvents LeftImageMirrorButton As System.Windows.Forms.Button
End Class

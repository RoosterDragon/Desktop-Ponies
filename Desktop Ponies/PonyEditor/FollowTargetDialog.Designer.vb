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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FollowTargetDialog))
        Me.CommandsTable = New System.Windows.Forms.TableLayoutPanel()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.FollowOption = New System.Windows.Forms.RadioButton()
        Me.GoToPointOption = New System.Windows.Forms.RadioButton()
        Me.TargetModeLabel = New System.Windows.Forms.Label()
        Me.UnitsLabel = New System.Windows.Forms.Label()
        Me.PointLabel = New System.Windows.Forms.Label()
        Me.PointPreviewLabel = New System.Windows.Forms.Label()
        Me.PointY = New System.Windows.Forms.NumericUpDown()
        Me.PointX = New System.Windows.Forms.NumericUpDown()
        Me.PointPreviewArea = New System.Windows.Forms.PictureBox()
        Me.FollowComboBox = New System.Windows.Forms.ComboBox()
        Me.FollowLabel = New System.Windows.Forms.Label()
        Me.RelativeToLabel = New System.Windows.Forms.Label()
        Me.NoTargetOption = New System.Windows.Forms.RadioButton()
        Me.AutoSelectImageCheckbox = New System.Windows.Forms.CheckBox()
        Me.AutoSelectImagesLabel = New System.Windows.Forms.Label()
        Me.StoppedComboBox = New System.Windows.Forms.ComboBox()
        Me.MovingComboBox = New System.Windows.Forms.ComboBox()
        Me.StoppedLabel = New System.Windows.Forms.Label()
        Me.MovingLabel = New System.Windows.Forms.Label()
        Me.CommandsTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'CommandsTable
        '
        Me.CommandsTable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CommandsTable.ColumnCount = 2
        Me.CommandsTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.CommandsTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.CommandsTable.Controls.Add(Me.Cancel_Button, 0, 0)
        Me.CommandsTable.Controls.Add(Me.OK_Button, 0, 0)
        Me.CommandsTable.Location = New System.Drawing.Point(12, 466)
        Me.CommandsTable.Name = "CommandsTable"
        Me.CommandsTable.RowCount = 1
        Me.CommandsTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.CommandsTable.Size = New System.Drawing.Size(476, 29)
        Me.CommandsTable.TabIndex = 20
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Cancel_Button.Location = New System.Drawing.Point(309, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(96, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.OK_Button.Location = New System.Drawing.Point(71, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(96, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        Me.OK_Button.UseVisualStyleBackColor = True
        '
        'FollowOption
        '
        Me.FollowOption.AutoSize = True
        Me.FollowOption.Checked = True
        Me.FollowOption.Location = New System.Drawing.Point(154, 25)
        Me.FollowOption.Name = "FollowOption"
        Me.FollowOption.Size = New System.Drawing.Size(165, 17)
        Me.FollowOption.TabIndex = 1
        Me.FollowOption.TabStop = True
        Me.FollowOption.Text = "Follow another pony or effect."
        Me.FollowOption.UseVisualStyleBackColor = True
        '
        'GoToPointOption
        '
        Me.GoToPointOption.AutoSize = True
        Me.GoToPointOption.Location = New System.Drawing.Point(154, 48)
        Me.GoToPointOption.Name = "GoToPointOption"
        Me.GoToPointOption.Size = New System.Drawing.Size(157, 17)
        Me.GoToPointOption.TabIndex = 2
        Me.GoToPointOption.Text = "Go to a point on the screen."
        Me.GoToPointOption.UseVisualStyleBackColor = True
        '
        'TargetModeLabel
        '
        Me.TargetModeLabel.AutoSize = True
        Me.TargetModeLabel.Location = New System.Drawing.Point(130, 9)
        Me.TargetModeLabel.Name = "TargetModeLabel"
        Me.TargetModeLabel.Size = New System.Drawing.Size(95, 13)
        Me.TargetModeLabel.TabIndex = 0
        Me.TargetModeLabel.Text = "The pony should..."
        '
        'UnitsLabel
        '
        Me.UnitsLabel.AutoSize = True
        Me.UnitsLabel.Location = New System.Drawing.Point(24, 368)
        Me.UnitsLabel.Name = "UnitsLabel"
        Me.UnitsLabel.Size = New System.Drawing.Size(74, 13)
        Me.UnitsLabel.TabIndex = 15
        Me.UnitsLabel.Text = "Location (X,Y)"
        '
        'PointLabel
        '
        Me.PointLabel.AutoSize = True
        Me.PointLabel.Location = New System.Drawing.Point(39, 319)
        Me.PointLabel.Name = "PointLabel"
        Me.PointLabel.Size = New System.Drawing.Size(179, 13)
        Me.PointLabel.TabIndex = 14
        Me.PointLabel.Text = "Point Relative to pony/effect center:"
        '
        'PointPreviewLabel
        '
        Me.PointPreviewLabel.AutoSize = True
        Me.PointPreviewLabel.Location = New System.Drawing.Point(296, 302)
        Me.PointPreviewLabel.Name = "PointPreviewLabel"
        Me.PointPreviewLabel.Size = New System.Drawing.Size(119, 13)
        Me.PointPreviewLabel.TabIndex = 19
        Me.PointPreviewLabel.Text = "Point selection preview:"
        '
        'PointY
        '
        Me.PointY.Location = New System.Drawing.Point(134, 384)
        Me.PointY.Name = "PointY"
        Me.PointY.Size = New System.Drawing.Size(46, 20)
        Me.PointY.TabIndex = 17
        '
        'PointX
        '
        Me.PointX.Location = New System.Drawing.Point(61, 384)
        Me.PointX.Name = "PointX"
        Me.PointX.Size = New System.Drawing.Size(46, 20)
        Me.PointX.TabIndex = 16
        '
        'PointPreviewArea
        '
        Me.PointPreviewArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PointPreviewArea.Location = New System.Drawing.Point(251, 328)
        Me.PointPreviewArea.Name = "PointPreviewArea"
        Me.PointPreviewArea.Size = New System.Drawing.Size(227, 123)
        Me.PointPreviewArea.TabIndex = 28
        Me.PointPreviewArea.TabStop = False
        '
        'FollowComboBox
        '
        Me.FollowComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.FollowComboBox.FormattingEnabled = True
        Me.FollowComboBox.Location = New System.Drawing.Point(96, 128)
        Me.FollowComboBox.Name = "FollowComboBox"
        Me.FollowComboBox.Size = New System.Drawing.Size(290, 21)
        Me.FollowComboBox.TabIndex = 5
        '
        'FollowLabel
        '
        Me.FollowLabel.AutoSize = True
        Me.FollowLabel.Location = New System.Drawing.Point(109, 112)
        Me.FollowLabel.Name = "FollowLabel"
        Me.FollowLabel.Size = New System.Drawing.Size(109, 13)
        Me.FollowLabel.TabIndex = 4
        Me.FollowLabel.Text = "Pony/Effect to follow:"
        '
        'RelativeToLabel
        '
        Me.RelativeToLabel.AutoSize = True
        Me.RelativeToLabel.Location = New System.Drawing.Point(48, 427)
        Me.RelativeToLabel.Name = "RelativeToLabel"
        Me.RelativeToLabel.Size = New System.Drawing.Size(155, 13)
        Me.RelativeToLabel.TabIndex = 18
        Me.RelativeToLabel.Text = "(Relative to pony/effect center)"
        '
        'NoTargetOption
        '
        Me.NoTargetOption.AutoSize = True
        Me.NoTargetOption.Location = New System.Drawing.Point(154, 71)
        Me.NoTargetOption.Name = "NoTargetOption"
        Me.NoTargetOption.Size = New System.Drawing.Size(163, 17)
        Me.NoTargetOption.TabIndex = 3
        Me.NoTargetOption.Text = "Do nothing (disable following)"
        Me.NoTargetOption.UseVisualStyleBackColor = True
        '
        'AutoSelectImageCheckbox
        '
        Me.AutoSelectImageCheckbox.AutoSize = True
        Me.AutoSelectImageCheckbox.Checked = True
        Me.AutoSelectImageCheckbox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AutoSelectImageCheckbox.Location = New System.Drawing.Point(177, 172)
        Me.AutoSelectImageCheckbox.Name = "AutoSelectImageCheckbox"
        Me.AutoSelectImageCheckbox.Size = New System.Drawing.Size(118, 17)
        Me.AutoSelectImageCheckbox.TabIndex = 6
        Me.AutoSelectImageCheckbox.Text = "Auto Select Images"
        Me.AutoSelectImageCheckbox.UseVisualStyleBackColor = True
        '
        'AutoSelectImagesLabel
        '
        Me.AutoSelectImagesLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AutoSelectImagesLabel.Location = New System.Drawing.Point(51, 192)
        Me.AutoSelectImagesLabel.Name = "AutoSelectImagesLabel"
        Me.AutoSelectImagesLabel.Size = New System.Drawing.Size(389, 39)
        Me.AutoSelectImagesLabel.TabIndex = 7
        Me.AutoSelectImagesLabel.Text = resources.GetString("AutoSelectImagesLabel.Text")
        Me.AutoSelectImagesLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'StoppedComboBox
        '
        Me.StoppedComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.StoppedComboBox.FormattingEnabled = True
        Me.StoppedComboBox.Location = New System.Drawing.Point(42, 264)
        Me.StoppedComboBox.Name = "StoppedComboBox"
        Me.StoppedComboBox.Size = New System.Drawing.Size(161, 21)
        Me.StoppedComboBox.TabIndex = 11
        '
        'MovingComboBox
        '
        Me.MovingComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.MovingComboBox.FormattingEnabled = True
        Me.MovingComboBox.Location = New System.Drawing.Point(279, 264)
        Me.MovingComboBox.Name = "MovingComboBox"
        Me.MovingComboBox.Size = New System.Drawing.Size(161, 21)
        Me.MovingComboBox.TabIndex = 13
        '
        'StoppedLabel
        '
        Me.StoppedLabel.AutoSize = True
        Me.StoppedLabel.Location = New System.Drawing.Point(24, 248)
        Me.StoppedLabel.Name = "StoppedLabel"
        Me.StoppedLabel.Size = New System.Drawing.Size(144, 13)
        Me.StoppedLabel.TabIndex = 10
        Me.StoppedLabel.Text = "Behavior for stopped images:"
        '
        'MovingLabel
        '
        Me.MovingLabel.AutoSize = True
        Me.MovingLabel.Location = New System.Drawing.Point(248, 248)
        Me.MovingLabel.Name = "MovingLabel"
        Me.MovingLabel.Size = New System.Drawing.Size(140, 13)
        Me.MovingLabel.TabIndex = 12
        Me.MovingLabel.Text = "Behavior for moving images:"
        '
        'FollowTargetDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(500, 507)
        Me.Controls.Add(Me.MovingLabel)
        Me.Controls.Add(Me.StoppedLabel)
        Me.Controls.Add(Me.MovingComboBox)
        Me.Controls.Add(Me.StoppedComboBox)
        Me.Controls.Add(Me.AutoSelectImagesLabel)
        Me.Controls.Add(Me.AutoSelectImageCheckbox)
        Me.Controls.Add(Me.NoTargetOption)
        Me.Controls.Add(Me.RelativeToLabel)
        Me.Controls.Add(Me.FollowLabel)
        Me.Controls.Add(Me.FollowComboBox)
        Me.Controls.Add(Me.UnitsLabel)
        Me.Controls.Add(Me.PointLabel)
        Me.Controls.Add(Me.PointPreviewLabel)
        Me.Controls.Add(Me.PointY)
        Me.Controls.Add(Me.PointX)
        Me.Controls.Add(Me.PointPreviewArea)
        Me.Controls.Add(Me.TargetModeLabel)
        Me.Controls.Add(Me.GoToPointOption)
        Me.Controls.Add(Me.FollowOption)
        Me.Controls.Add(Me.CommandsTable)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(516, 545)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(516, 545)
        Me.Name = "FollowTargetDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Select following parameters..."
        Me.CommandsTable.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CommandsTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents FollowOption As System.Windows.Forms.RadioButton
    Friend WithEvents GoToPointOption As System.Windows.Forms.RadioButton
    Friend WithEvents TargetModeLabel As System.Windows.Forms.Label
    Friend WithEvents UnitsLabel As System.Windows.Forms.Label
    Friend WithEvents PointLabel As System.Windows.Forms.Label
    Friend WithEvents PointPreviewLabel As System.Windows.Forms.Label
    Friend WithEvents PointY As System.Windows.Forms.NumericUpDown
    Friend WithEvents PointX As System.Windows.Forms.NumericUpDown
    Friend WithEvents PointPreviewArea As System.Windows.Forms.PictureBox
    Friend WithEvents FollowComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents FollowLabel As System.Windows.Forms.Label
    Friend WithEvents RelativeToLabel As System.Windows.Forms.Label
    Friend WithEvents NoTargetOption As System.Windows.Forms.RadioButton
    Friend WithEvents AutoSelectImageCheckbox As System.Windows.Forms.CheckBox
    Friend WithEvents AutoSelectImagesLabel As System.Windows.Forms.Label
    Friend WithEvents StoppedComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents MovingComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents StoppedLabel As System.Windows.Forms.Label
    Friend WithEvents MovingLabel As System.Windows.Forms.Label

End Class

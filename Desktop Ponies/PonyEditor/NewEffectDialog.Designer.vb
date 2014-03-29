<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewEffectDialog
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
        Me.LeftImageBox = New System.Windows.Forms.PictureBox()
        Me.RightImageBox = New System.Windows.Forms.PictureBox()
        Me.NameTextBox = New System.Windows.Forms.TextBox()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.LeftImageLabel = New System.Windows.Forms.Label()
        Me.RightImageLabel = New System.Windows.Forms.Label()
        Me.LeftImageSelectButton = New System.Windows.Forms.Button()
        Me.RightImageSelectButton = New System.Windows.Forms.Button()
        Me.BehaviorCombo = New System.Windows.Forms.ComboBox()
        Me.DurationLabel = New System.Windows.Forms.Label()
        Me.BehaviorLabel = New System.Windows.Forms.Label()
        Me.DurationZeroLabel = New System.Windows.Forms.Label()
        Me.RepeatDelayLabel = New System.Windows.Forms.Label()
        Me.RepeatDelayZeroLabel = New System.Windows.Forms.Label()
        Me.FollowCheckBox = New System.Windows.Forms.CheckBox()
        Me.RightPlacementLabel = New System.Windows.Forms.Label()
        Me.RightPlacementCombo = New System.Windows.Forms.ComboBox()
        Me.LeftPlacementCombo = New System.Windows.Forms.ComboBox()
        Me.RightCenteringLabel = New System.Windows.Forms.Label()
        Me.RightCenteringCombo = New System.Windows.Forms.ComboBox()
        Me.LeftCenteringLabel = New System.Windows.Forms.Label()
        Me.LeftCenteringCombo = New System.Windows.Forms.ComboBox()
        Me.LeftPlacementLabel = New System.Windows.Forms.Label()
        Me.DoNotRepeatAnimationsCheckBox = New System.Windows.Forms.CheckBox()
        Me.LayoutTable = New System.Windows.Forms.TableLayoutPanel()
        Me.DurationNumber = New System.Windows.Forms.NumericUpDown()
        Me.RepeatDelayNumber = New System.Windows.Forms.NumericUpDown()
        Me.CommandTable.SuspendLayout()
        Me.LayoutTable.SuspendLayout()
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
        Me.CommandTable.Location = New System.Drawing.Point(227, 521)
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
        'LeftImageBox
        '
        Me.LeftImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LeftImageBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LeftImageBox.ErrorImage = Nothing
        Me.LeftImageBox.InitialImage = Nothing
        Me.LeftImageBox.Location = New System.Drawing.Point(3, 55)
        Me.LeftImageBox.Name = "LeftImageBox"
        Me.LeftImageBox.Size = New System.Drawing.Size(261, 109)
        Me.LeftImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.LeftImageBox.TabIndex = 1
        Me.LeftImageBox.TabStop = False
        '
        'RightImageBox
        '
        Me.RightImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RightImageBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RightImageBox.ErrorImage = Nothing
        Me.RightImageBox.InitialImage = Nothing
        Me.RightImageBox.Location = New System.Drawing.Point(270, 55)
        Me.RightImageBox.Name = "RightImageBox"
        Me.RightImageBox.Size = New System.Drawing.Size(262, 109)
        Me.RightImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.RightImageBox.TabIndex = 2
        Me.RightImageBox.TabStop = False
        '
        'NameTextBox
        '
        Me.NameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.NameTextBox.Location = New System.Drawing.Point(73, 16)
        Me.NameTextBox.Name = "NameTextBox"
        Me.NameTextBox.Size = New System.Drawing.Size(120, 20)
        Me.NameTextBox.TabIndex = 1
        '
        'NameLabel
        '
        Me.NameLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Location = New System.Drawing.Point(3, 0)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(69, 13)
        Me.NameLabel.TabIndex = 0
        Me.NameLabel.Text = "Effect Name:"
        '
        'LeftImageLabel
        '
        Me.LeftImageLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LeftImageLabel.AutoSize = True
        Me.LeftImageLabel.Location = New System.Drawing.Point(3, 39)
        Me.LeftImageLabel.Name = "LeftImageLabel"
        Me.LeftImageLabel.Size = New System.Drawing.Size(60, 13)
        Me.LeftImageLabel.TabIndex = 2
        Me.LeftImageLabel.Text = "Left Image:"
        '
        'RightImageLabel
        '
        Me.RightImageLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RightImageLabel.AutoSize = True
        Me.RightImageLabel.Location = New System.Drawing.Point(270, 39)
        Me.RightImageLabel.Name = "RightImageLabel"
        Me.RightImageLabel.Size = New System.Drawing.Size(67, 13)
        Me.RightImageLabel.TabIndex = 4
        Me.RightImageLabel.Text = "Right Image:"
        '
        'LeftImageSelectButton
        '
        Me.LeftImageSelectButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LeftImageSelectButton.Location = New System.Drawing.Point(83, 170)
        Me.LeftImageSelectButton.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.LeftImageSelectButton.Name = "LeftImageSelectButton"
        Me.LeftImageSelectButton.Size = New System.Drawing.Size(100, 23)
        Me.LeftImageSelectButton.TabIndex = 3
        Me.LeftImageSelectButton.Text = "Select..."
        Me.LeftImageSelectButton.UseVisualStyleBackColor = True
        '
        'RightImageSelectButton
        '
        Me.RightImageSelectButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.RightImageSelectButton.Location = New System.Drawing.Point(351, 170)
        Me.RightImageSelectButton.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.RightImageSelectButton.Name = "RightImageSelectButton"
        Me.RightImageSelectButton.Size = New System.Drawing.Size(100, 23)
        Me.RightImageSelectButton.TabIndex = 5
        Me.RightImageSelectButton.Text = "Select..."
        Me.RightImageSelectButton.UseVisualStyleBackColor = True
        '
        'BehaviorCombo
        '
        Me.BehaviorCombo.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.BehaviorCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.BehaviorCombo.FormattingEnabled = True
        Me.BehaviorCombo.Location = New System.Drawing.Point(73, 259)
        Me.BehaviorCombo.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.BehaviorCombo.Name = "BehaviorCombo"
        Me.BehaviorCombo.Size = New System.Drawing.Size(120, 21)
        Me.BehaviorCombo.TabIndex = 8
        '
        'DurationLabel
        '
        Me.DurationLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.DurationLabel.AutoSize = True
        Me.DurationLabel.Location = New System.Drawing.Point(270, 243)
        Me.DurationLabel.Name = "DurationLabel"
        Me.DurationLabel.Size = New System.Drawing.Size(104, 13)
        Me.DurationLabel.TabIndex = 17
        Me.DurationLabel.Text = "Duration in seconds:"
        '
        'BehaviorLabel
        '
        Me.BehaviorLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BehaviorLabel.AutoSize = True
        Me.BehaviorLabel.Location = New System.Drawing.Point(3, 243)
        Me.BehaviorLabel.Name = "BehaviorLabel"
        Me.BehaviorLabel.Size = New System.Drawing.Size(110, 13)
        Me.BehaviorLabel.TabIndex = 7
        Me.BehaviorLabel.Text = "Behavior that triggers:"
        '
        'DurationZeroLabel
        '
        Me.DurationZeroLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.DurationZeroLabel.AutoSize = True
        Me.DurationZeroLabel.Location = New System.Drawing.Point(286, 295)
        Me.DurationZeroLabel.Name = "DurationZeroLabel"
        Me.DurationZeroLabel.Size = New System.Drawing.Size(229, 13)
        Me.DurationZeroLabel.TabIndex = 19
        Me.DurationZeroLabel.Text = "(Set to 0 if it should last until the behavior ends)"
        '
        'RepeatDelayLabel
        '
        Me.RepeatDelayLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RepeatDelayLabel.AutoSize = True
        Me.RepeatDelayLabel.Location = New System.Drawing.Point(270, 347)
        Me.RepeatDelayLabel.Name = "RepeatDelayLabel"
        Me.RepeatDelayLabel.Size = New System.Drawing.Size(129, 13)
        Me.RepeatDelayLabel.TabIndex = 20
        Me.RepeatDelayLabel.Text = "Repeat Delay in seconds:"
        '
        'RepeatDelayZeroLabel
        '
        Me.RepeatDelayZeroLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.RepeatDelayZeroLabel.AutoSize = True
        Me.RepeatDelayZeroLabel.Location = New System.Drawing.Point(334, 399)
        Me.RepeatDelayZeroLabel.Name = "RepeatDelayZeroLabel"
        Me.RepeatDelayZeroLabel.Size = New System.Drawing.Size(133, 13)
        Me.RepeatDelayZeroLabel.TabIndex = 22
        Me.RepeatDelayZeroLabel.Text = "(Set to 0 to play only once)"
        '
        'FollowCheckBox
        '
        Me.FollowCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.FollowCheckBox.AutoSize = True
        Me.FollowCheckBox.Location = New System.Drawing.Point(324, 467)
        Me.FollowCheckBox.Name = "FollowCheckBox"
        Me.FollowCheckBox.Size = New System.Drawing.Size(153, 17)
        Me.FollowCheckBox.TabIndex = 23
        Me.FollowCheckBox.Text = "Effect graphic follows pony"
        Me.FollowCheckBox.UseVisualStyleBackColor = True
        '
        'RightPlacementLabel
        '
        Me.RightPlacementLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RightPlacementLabel.AutoSize = True
        Me.RightPlacementLabel.Location = New System.Drawing.Point(3, 295)
        Me.RightPlacementLabel.Name = "RightPlacementLabel"
        Me.RightPlacementLabel.Size = New System.Drawing.Size(223, 13)
        Me.RightPlacementLabel.TabIndex = 9
        Me.RightPlacementLabel.Text = "Image placement, when pony is heading right:"
        '
        'RightPlacementCombo
        '
        Me.RightPlacementCombo.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.RightPlacementCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.RightPlacementCombo.FormattingEnabled = True
        Me.RightPlacementCombo.Location = New System.Drawing.Point(73, 311)
        Me.RightPlacementCombo.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.RightPlacementCombo.Name = "RightPlacementCombo"
        Me.RightPlacementCombo.Size = New System.Drawing.Size(120, 21)
        Me.RightPlacementCombo.TabIndex = 10
        '
        'LeftPlacementCombo
        '
        Me.LeftPlacementCombo.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LeftPlacementCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.LeftPlacementCombo.FormattingEnabled = True
        Me.LeftPlacementCombo.Location = New System.Drawing.Point(73, 363)
        Me.LeftPlacementCombo.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.LeftPlacementCombo.Name = "LeftPlacementCombo"
        Me.LeftPlacementCombo.Size = New System.Drawing.Size(120, 21)
        Me.LeftPlacementCombo.TabIndex = 12
        '
        'RightCenteringLabel
        '
        Me.RightCenteringLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RightCenteringLabel.AutoSize = True
        Me.RightCenteringLabel.Location = New System.Drawing.Point(3, 399)
        Me.RightCenteringLabel.Name = "RightCenteringLabel"
        Me.RightCenteringLabel.Size = New System.Drawing.Size(257, 13)
        Me.RightCenteringLabel.TabIndex = 13
        Me.RightCenteringLabel.Text = "Image centering relative to pony, when heading right:"
        '
        'RightCenteringCombo
        '
        Me.RightCenteringCombo.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.RightCenteringCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.RightCenteringCombo.FormattingEnabled = True
        Me.RightCenteringCombo.Location = New System.Drawing.Point(73, 415)
        Me.RightCenteringCombo.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.RightCenteringCombo.Name = "RightCenteringCombo"
        Me.RightCenteringCombo.Size = New System.Drawing.Size(120, 21)
        Me.RightCenteringCombo.TabIndex = 14
        '
        'LeftCenteringLabel
        '
        Me.LeftCenteringLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LeftCenteringLabel.AutoSize = True
        Me.LeftCenteringLabel.Location = New System.Drawing.Point(3, 451)
        Me.LeftCenteringLabel.Name = "LeftCenteringLabel"
        Me.LeftCenteringLabel.Size = New System.Drawing.Size(251, 13)
        Me.LeftCenteringLabel.TabIndex = 15
        Me.LeftCenteringLabel.Text = "Image centering relative to pony, when heading left:"
        '
        'LeftCenteringCombo
        '
        Me.LeftCenteringCombo.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LeftCenteringCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.LeftCenteringCombo.FormattingEnabled = True
        Me.LeftCenteringCombo.Location = New System.Drawing.Point(73, 467)
        Me.LeftCenteringCombo.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.LeftCenteringCombo.Name = "LeftCenteringCombo"
        Me.LeftCenteringCombo.Size = New System.Drawing.Size(120, 21)
        Me.LeftCenteringCombo.TabIndex = 16
        '
        'LeftPlacementLabel
        '
        Me.LeftPlacementLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LeftPlacementLabel.AutoSize = True
        Me.LeftPlacementLabel.Location = New System.Drawing.Point(3, 347)
        Me.LeftPlacementLabel.Name = "LeftPlacementLabel"
        Me.LeftPlacementLabel.Size = New System.Drawing.Size(217, 13)
        Me.LeftPlacementLabel.TabIndex = 11
        Me.LeftPlacementLabel.Text = "Image placement, when pony is heading left:"
        '
        'DoNotRepeatAnimationsCheckBox
        '
        Me.DoNotRepeatAnimationsCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.DoNotRepeatAnimationsCheckBox.AutoSize = True
        Me.LayoutTable.SetColumnSpan(Me.DoNotRepeatAnimationsCheckBox, 2)
        Me.DoNotRepeatAnimationsCheckBox.Location = New System.Drawing.Point(199, 211)
        Me.DoNotRepeatAnimationsCheckBox.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.DoNotRepeatAnimationsCheckBox.Name = "DoNotRepeatAnimationsCheckBox"
        Me.DoNotRepeatAnimationsCheckBox.Size = New System.Drawing.Size(137, 17)
        Me.DoNotRepeatAnimationsCheckBox.TabIndex = 6
        Me.DoNotRepeatAnimationsCheckBox.Text = "Don't repeat animations"
        Me.DoNotRepeatAnimationsCheckBox.UseVisualStyleBackColor = True
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
        Me.LayoutTable.Controls.Add(Me.LeftCenteringCombo, 0, 15)
        Me.LayoutTable.Controls.Add(Me.LeftCenteringLabel, 0, 14)
        Me.LayoutTable.Controls.Add(Me.LeftPlacementLabel, 0, 10)
        Me.LayoutTable.Controls.Add(Me.RightCenteringCombo, 0, 13)
        Me.LayoutTable.Controls.Add(Me.RightCenteringLabel, 0, 12)
        Me.LayoutTable.Controls.Add(Me.NameTextBox, 0, 1)
        Me.LayoutTable.Controls.Add(Me.RepeatDelayZeroLabel, 1, 12)
        Me.LayoutTable.Controls.Add(Me.LeftPlacementCombo, 0, 11)
        Me.LayoutTable.Controls.Add(Me.RepeatDelayLabel, 1, 10)
        Me.LayoutTable.Controls.Add(Me.DurationZeroLabel, 1, 8)
        Me.LayoutTable.Controls.Add(Me.LeftImageLabel, 0, 2)
        Me.LayoutTable.Controls.Add(Me.RightImageLabel, 1, 2)
        Me.LayoutTable.Controls.Add(Me.LeftImageBox, 0, 3)
        Me.LayoutTable.Controls.Add(Me.RightPlacementCombo, 0, 9)
        Me.LayoutTable.Controls.Add(Me.RightPlacementLabel, 0, 8)
        Me.LayoutTable.Controls.Add(Me.RightImageBox, 1, 3)
        Me.LayoutTable.Controls.Add(Me.LeftImageSelectButton, 0, 4)
        Me.LayoutTable.Controls.Add(Me.RightImageSelectButton, 1, 4)
        Me.LayoutTable.Controls.Add(Me.BehaviorLabel, 0, 6)
        Me.LayoutTable.Controls.Add(Me.BehaviorCombo, 0, 7)
        Me.LayoutTable.Controls.Add(Me.DurationLabel, 1, 6)
        Me.LayoutTable.Controls.Add(Me.DurationNumber, 1, 7)
        Me.LayoutTable.Controls.Add(Me.RepeatDelayNumber, 1, 11)
        Me.LayoutTable.Controls.Add(Me.FollowCheckBox, 1, 15)
        Me.LayoutTable.Controls.Add(Me.DoNotRepeatAnimationsCheckBox, 0, 5)
        Me.LayoutTable.Location = New System.Drawing.Point(12, 12)
        Me.LayoutTable.Name = "LayoutTable"
        Me.LayoutTable.RowCount = 16
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.Size = New System.Drawing.Size(535, 503)
        Me.LayoutTable.TabIndex = 0
        '
        'DurationNumber
        '
        Me.DurationNumber.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.DurationNumber.DecimalPlaces = 2
        Me.DurationNumber.Location = New System.Drawing.Point(341, 259)
        Me.DurationNumber.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.DurationNumber.Name = "DurationNumber"
        Me.DurationNumber.Size = New System.Drawing.Size(120, 20)
        Me.DurationNumber.TabIndex = 18
        '
        'RepeatDelayNumber
        '
        Me.RepeatDelayNumber.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.RepeatDelayNumber.DecimalPlaces = 2
        Me.RepeatDelayNumber.Location = New System.Drawing.Point(341, 363)
        Me.RepeatDelayNumber.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.RepeatDelayNumber.Name = "RepeatDelayNumber"
        Me.RepeatDelayNumber.Size = New System.Drawing.Size(120, 20)
        Me.RepeatDelayNumber.TabIndex = 21
        '
        'NewEffectDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(559, 562)
        Me.Controls.Add(Me.CommandTable)
        Me.Controls.Add(Me.LayoutTable)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(400, 550)
        Me.Name = "NewEffectDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create New Effect..."
        Me.CommandTable.ResumeLayout(False)
        Me.LayoutTable.ResumeLayout(False)
        Me.LayoutTable.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CommandTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents LeftImageBox As System.Windows.Forms.PictureBox
    Friend WithEvents RightImageBox As System.Windows.Forms.PictureBox
    Friend WithEvents NameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents LeftImageLabel As System.Windows.Forms.Label
    Friend WithEvents RightImageLabel As System.Windows.Forms.Label
    Friend WithEvents LeftImageSelectButton As System.Windows.Forms.Button
    Friend WithEvents RightImageSelectButton As System.Windows.Forms.Button
    Friend WithEvents BehaviorCombo As System.Windows.Forms.ComboBox
    Friend WithEvents DurationLabel As System.Windows.Forms.Label
    Friend WithEvents BehaviorLabel As System.Windows.Forms.Label
    Friend WithEvents DurationZeroLabel As System.Windows.Forms.Label
    Friend WithEvents RepeatDelayLabel As System.Windows.Forms.Label
    Friend WithEvents RepeatDelayZeroLabel As System.Windows.Forms.Label
    Friend WithEvents FollowCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents RightPlacementLabel As System.Windows.Forms.Label
    Friend WithEvents RightPlacementCombo As System.Windows.Forms.ComboBox
    Friend WithEvents LeftPlacementCombo As System.Windows.Forms.ComboBox
    Friend WithEvents RightCenteringLabel As System.Windows.Forms.Label
    Friend WithEvents RightCenteringCombo As System.Windows.Forms.ComboBox
    Friend WithEvents LeftCenteringLabel As System.Windows.Forms.Label
    Friend WithEvents LeftCenteringCombo As System.Windows.Forms.ComboBox
    Friend WithEvents LeftPlacementLabel As System.Windows.Forms.Label
    Friend WithEvents DoNotRepeatAnimationsCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents LayoutTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents DurationNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents RepeatDelayNumber As System.Windows.Forms.NumericUpDown

End Class

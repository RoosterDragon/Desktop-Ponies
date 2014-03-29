<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewBehaviorDialog
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
        Me.MovementComboBox = New System.Windows.Forms.ComboBox()
        Me.MovementLabel = New System.Windows.Forms.Label()
        Me.StartSpeechComboBox = New System.Windows.Forms.ComboBox()
        Me.EndSpeechComboBox = New System.Windows.Forms.ComboBox()
        Me.FollowTextBox = New System.Windows.Forms.TextBox()
        Me.FollowSelectButton = New System.Windows.Forms.Button()
        Me.LinkComboBox = New System.Windows.Forms.ComboBox()
        Me.ChanceLabel = New System.Windows.Forms.Label()
        Me.MaxDurationLabel = New System.Windows.Forms.Label()
        Me.MinDurationLabel = New System.Windows.Forms.Label()
        Me.StartSpeechLabel = New System.Windows.Forms.Label()
        Me.EndSpeechLabel = New System.Windows.Forms.Label()
        Me.FollowLabel = New System.Windows.Forms.Label()
        Me.SpeedLabel = New System.Windows.Forms.Label()
        Me.DoNotRepeatAnimationsCheckBox = New System.Windows.Forms.CheckBox()
        Me.DoNotRunRandomlyCheckBox = New System.Windows.Forms.CheckBox()
        Me.GroupNumber = New System.Windows.Forms.NumericUpDown()
        Me.GroupLabel = New System.Windows.Forms.Label()
        Me.MinDurationNumber = New System.Windows.Forms.NumericUpDown()
        Me.MaxDurationNumber = New System.Windows.Forms.NumericUpDown()
        Me.ChanceNumber = New System.Windows.Forms.NumericUpDown()
        Me.SpeedNumber = New System.Windows.Forms.NumericUpDown()
        Me.LayoutTable = New System.Windows.Forms.TableLayoutPanel()
        Me.ImagesTable = New System.Windows.Forms.TableLayoutPanel()
        Me.FollowPanel = New System.Windows.Forms.Panel()
        Me.LinkLabel = New System.Windows.Forms.Label()
        Me.CommandTable.SuspendLayout()
        Me.LayoutTable.SuspendLayout()
        Me.ImagesTable.SuspendLayout()
        Me.FollowPanel.SuspendLayout()
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
        Me.CommandTable.Location = New System.Drawing.Point(167, 521)
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
        Me.LeftImageBox.Location = New System.Drawing.Point(3, 16)
        Me.LeftImageBox.Name = "LeftImageBox"
        Me.LeftImageBox.Size = New System.Drawing.Size(228, 168)
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
        Me.RightImageBox.Location = New System.Drawing.Point(237, 16)
        Me.RightImageBox.Name = "RightImageBox"
        Me.RightImageBox.Size = New System.Drawing.Size(229, 168)
        Me.RightImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.RightImageBox.TabIndex = 2
        Me.RightImageBox.TabStop = False
        '
        'NameTextBox
        '
        Me.NameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.NameTextBox.Location = New System.Drawing.Point(19, 16)
        Me.NameTextBox.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
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
        Me.NameLabel.Size = New System.Drawing.Size(83, 13)
        Me.NameLabel.TabIndex = 0
        Me.NameLabel.Text = "Behavior Name:"
        '
        'LeftImageLabel
        '
        Me.LeftImageLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LeftImageLabel.AutoSize = True
        Me.LeftImageLabel.Location = New System.Drawing.Point(3, 0)
        Me.LeftImageLabel.Name = "LeftImageLabel"
        Me.LeftImageLabel.Size = New System.Drawing.Size(60, 13)
        Me.LeftImageLabel.TabIndex = 0
        Me.LeftImageLabel.Text = "Left Image:"
        '
        'RightImageLabel
        '
        Me.RightImageLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.RightImageLabel.AutoSize = True
        Me.RightImageLabel.Location = New System.Drawing.Point(237, 0)
        Me.RightImageLabel.Name = "RightImageLabel"
        Me.RightImageLabel.Size = New System.Drawing.Size(67, 13)
        Me.RightImageLabel.TabIndex = 2
        Me.RightImageLabel.Text = "Right Image:"
        '
        'LeftImageSelectButton
        '
        Me.LeftImageSelectButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LeftImageSelectButton.Location = New System.Drawing.Point(67, 190)
        Me.LeftImageSelectButton.Name = "LeftImageSelectButton"
        Me.LeftImageSelectButton.Size = New System.Drawing.Size(100, 23)
        Me.LeftImageSelectButton.TabIndex = 1
        Me.LeftImageSelectButton.Text = "Select..."
        Me.LeftImageSelectButton.UseVisualStyleBackColor = True
        '
        'RightImageSelectButton
        '
        Me.RightImageSelectButton.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.RightImageSelectButton.Location = New System.Drawing.Point(301, 190)
        Me.RightImageSelectButton.Name = "RightImageSelectButton"
        Me.RightImageSelectButton.Size = New System.Drawing.Size(100, 23)
        Me.RightImageSelectButton.TabIndex = 3
        Me.RightImageSelectButton.Text = "Select..."
        Me.RightImageSelectButton.UseVisualStyleBackColor = True
        '
        'MovementComboBox
        '
        Me.MovementComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.MovementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.MovementComboBox.FormattingEnabled = True
        Me.MovementComboBox.Location = New System.Drawing.Point(177, 16)
        Me.MovementComboBox.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.MovementComboBox.Name = "MovementComboBox"
        Me.MovementComboBox.Size = New System.Drawing.Size(120, 21)
        Me.MovementComboBox.TabIndex = 3
        '
        'MovementLabel
        '
        Me.MovementLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.MovementLabel.AutoSize = True
        Me.MovementLabel.Location = New System.Drawing.Point(161, 0)
        Me.MovementLabel.Name = "MovementLabel"
        Me.MovementLabel.Size = New System.Drawing.Size(60, 13)
        Me.MovementLabel.TabIndex = 2
        Me.MovementLabel.Text = "Movement:"
        '
        'StartSpeechComboBox
        '
        Me.StartSpeechComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.StartSpeechComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.StartSpeechComboBox.FormattingEnabled = True
        Me.StartSpeechComboBox.Location = New System.Drawing.Point(19, 426)
        Me.StartSpeechComboBox.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.StartSpeechComboBox.Name = "StartSpeechComboBox"
        Me.StartSpeechComboBox.Size = New System.Drawing.Size(120, 21)
        Me.StartSpeechComboBox.TabIndex = 17
        '
        'EndSpeechComboBox
        '
        Me.EndSpeechComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.EndSpeechComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.EndSpeechComboBox.FormattingEnabled = True
        Me.EndSpeechComboBox.Location = New System.Drawing.Point(177, 426)
        Me.EndSpeechComboBox.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.EndSpeechComboBox.Name = "EndSpeechComboBox"
        Me.EndSpeechComboBox.Size = New System.Drawing.Size(120, 21)
        Me.EndSpeechComboBox.TabIndex = 19
        '
        'FollowTextBox
        '
        Me.FollowTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FollowTextBox.Location = New System.Drawing.Point(3, 5)
        Me.FollowTextBox.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.FollowTextBox.Name = "FollowTextBox"
        Me.FollowTextBox.ReadOnly = True
        Me.FollowTextBox.Size = New System.Drawing.Size(72, 20)
        Me.FollowTextBox.TabIndex = 0
        '
        'FollowSelectButton
        '
        Me.FollowSelectButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FollowSelectButton.Location = New System.Drawing.Point(81, 3)
        Me.FollowSelectButton.Name = "FollowSelectButton"
        Me.FollowSelectButton.Size = New System.Drawing.Size(75, 23)
        Me.FollowSelectButton.TabIndex = 1
        Me.FollowSelectButton.Text = "Select..."
        Me.FollowSelectButton.UseVisualStyleBackColor = True
        '
        'LinkComboBox
        '
        Me.LinkComboBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LinkComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.LinkComboBox.FormattingEnabled = True
        Me.LinkComboBox.Location = New System.Drawing.Point(19, 479)
        Me.LinkComboBox.Name = "LinkComboBox"
        Me.LinkComboBox.Size = New System.Drawing.Size(120, 21)
        Me.LinkComboBox.TabIndex = 23
        '
        'ChanceLabel
        '
        Me.ChanceLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChanceLabel.AutoSize = True
        Me.ChanceLabel.Location = New System.Drawing.Point(3, 359)
        Me.ChanceLabel.Name = "ChanceLabel"
        Me.ChanceLabel.Size = New System.Drawing.Size(106, 13)
        Me.ChanceLabel.TabIndex = 10
        Me.ChanceLabel.Text = "Chance to occur (%):"
        '
        'MaxDurationLabel
        '
        Me.MaxDurationLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.MaxDurationLabel.AutoSize = True
        Me.MaxDurationLabel.Location = New System.Drawing.Point(161, 359)
        Me.MaxDurationLabel.Name = "MaxDurationLabel"
        Me.MaxDurationLabel.Size = New System.Drawing.Size(151, 13)
        Me.MaxDurationLabel.TabIndex = 12
        Me.MaxDurationLabel.Text = "Maximum Duration in seconds:"
        '
        'MinDurationLabel
        '
        Me.MinDurationLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.MinDurationLabel.AutoSize = True
        Me.MinDurationLabel.Location = New System.Drawing.Point(319, 359)
        Me.MinDurationLabel.Name = "MinDurationLabel"
        Me.MinDurationLabel.Size = New System.Drawing.Size(148, 13)
        Me.MinDurationLabel.TabIndex = 14
        Me.MinDurationLabel.Text = "Minimum Duration in seconds:"
        '
        'StartSpeechLabel
        '
        Me.StartSpeechLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.StartSpeechLabel.AutoSize = True
        Me.StartSpeechLabel.Location = New System.Drawing.Point(3, 410)
        Me.StartSpeechLabel.Name = "StartSpeechLabel"
        Me.StartSpeechLabel.Size = New System.Drawing.Size(86, 13)
        Me.StartSpeechLabel.TabIndex = 16
        Me.StartSpeechLabel.Text = "Starting Speech:"
        '
        'EndSpeechLabel
        '
        Me.EndSpeechLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.EndSpeechLabel.AutoSize = True
        Me.EndSpeechLabel.Location = New System.Drawing.Point(161, 410)
        Me.EndSpeechLabel.Name = "EndSpeechLabel"
        Me.EndSpeechLabel.Size = New System.Drawing.Size(83, 13)
        Me.EndSpeechLabel.TabIndex = 18
        Me.EndSpeechLabel.Text = "Ending Speech:"
        '
        'FollowLabel
        '
        Me.FollowLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FollowLabel.AutoSize = True
        Me.FollowLabel.Location = New System.Drawing.Point(319, 410)
        Me.FollowLabel.Name = "FollowLabel"
        Me.FollowLabel.Size = New System.Drawing.Size(97, 13)
        Me.FollowLabel.TabIndex = 20
        Me.FollowLabel.Text = "Follow/Goto mode:"
        '
        'SpeedLabel
        '
        Me.SpeedLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SpeedLabel.AutoSize = True
        Me.SpeedLabel.Location = New System.Drawing.Point(319, 0)
        Me.SpeedLabel.Name = "SpeedLabel"
        Me.SpeedLabel.Size = New System.Drawing.Size(94, 13)
        Me.SpeedLabel.TabIndex = 4
        Me.SpeedLabel.Text = "Movement Speed:"
        '
        'DoNotRepeatAnimationsCheckBox
        '
        Me.DoNotRepeatAnimationsCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.DoNotRepeatAnimationsCheckBox.AutoSize = True
        Me.DoNotRepeatAnimationsCheckBox.Location = New System.Drawing.Point(168, 327)
        Me.DoNotRepeatAnimationsCheckBox.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.DoNotRepeatAnimationsCheckBox.Name = "DoNotRepeatAnimationsCheckBox"
        Me.DoNotRepeatAnimationsCheckBox.Size = New System.Drawing.Size(137, 17)
        Me.DoNotRepeatAnimationsCheckBox.TabIndex = 9
        Me.DoNotRepeatAnimationsCheckBox.Text = "Don't repeat animations"
        Me.DoNotRepeatAnimationsCheckBox.UseVisualStyleBackColor = True
        '
        'DoNotRunRandomlyCheckBox
        '
        Me.DoNotRunRandomlyCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.DoNotRunRandomlyCheckBox.AutoSize = True
        Me.LayoutTable.SetColumnSpan(Me.DoNotRunRandomlyCheckBox, 2)
        Me.DoNotRunRandomlyCheckBox.Location = New System.Drawing.Point(174, 479)
        Me.DoNotRunRandomlyCheckBox.Name = "DoNotRunRandomlyCheckBox"
        Me.DoNotRunRandomlyCheckBox.Size = New System.Drawing.Size(285, 17)
        Me.DoNotRunRandomlyCheckBox.TabIndex = 24
        Me.DoNotRunRandomlyCheckBox.Text = "Don't run randomly (use when intended for interactions)"
        Me.DoNotRunRandomlyCheckBox.UseVisualStyleBackColor = True
        '
        'GroupNumber
        '
        Me.GroupNumber.Location = New System.Drawing.Point(319, 55)
        Me.GroupNumber.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.GroupNumber.Name = "GroupNumber"
        Me.GroupNumber.Size = New System.Drawing.Size(120, 20)
        Me.GroupNumber.TabIndex = 7
        '
        'GroupLabel
        '
        Me.GroupLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupLabel.AutoSize = True
        Me.LayoutTable.SetColumnSpan(Me.GroupLabel, 2)
        Me.GroupLabel.Location = New System.Drawing.Point(27, 58)
        Me.GroupLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 0)
        Me.GroupLabel.Name = "GroupLabel"
        Me.GroupLabel.Size = New System.Drawing.Size(286, 13)
        Me.GroupLabel.TabIndex = 6
        Me.GroupLabel.Text = "Behavior Group (leave at 0 to be used regardless of group):"
        '
        'MinDurationNumber
        '
        Me.MinDurationNumber.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.MinDurationNumber.DecimalPlaces = 2
        Me.MinDurationNumber.Location = New System.Drawing.Point(335, 375)
        Me.MinDurationNumber.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.MinDurationNumber.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.MinDurationNumber.Name = "MinDurationNumber"
        Me.MinDurationNumber.Size = New System.Drawing.Size(120, 20)
        Me.MinDurationNumber.TabIndex = 15
        '
        'MaxDurationNumber
        '
        Me.MaxDurationNumber.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.MaxDurationNumber.DecimalPlaces = 2
        Me.MaxDurationNumber.Location = New System.Drawing.Point(177, 375)
        Me.MaxDurationNumber.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.MaxDurationNumber.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.MaxDurationNumber.Name = "MaxDurationNumber"
        Me.MaxDurationNumber.Size = New System.Drawing.Size(120, 20)
        Me.MaxDurationNumber.TabIndex = 13
        '
        'ChanceNumber
        '
        Me.ChanceNumber.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ChanceNumber.DecimalPlaces = 2
        Me.ChanceNumber.Location = New System.Drawing.Point(19, 375)
        Me.ChanceNumber.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.ChanceNumber.Name = "ChanceNumber"
        Me.ChanceNumber.Size = New System.Drawing.Size(120, 20)
        Me.ChanceNumber.TabIndex = 11
        '
        'SpeedNumber
        '
        Me.SpeedNumber.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.SpeedNumber.DecimalPlaces = 2
        Me.SpeedNumber.Location = New System.Drawing.Point(335, 16)
        Me.SpeedNumber.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.SpeedNumber.Maximum = New Decimal(New Integer() {25, 0, 0, 0})
        Me.SpeedNumber.Name = "SpeedNumber"
        Me.SpeedNumber.Size = New System.Drawing.Size(120, 20)
        Me.SpeedNumber.TabIndex = 5
        '
        'LayoutTable
        '
        Me.LayoutTable.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LayoutTable.ColumnCount = 3
        Me.LayoutTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.LayoutTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
        Me.LayoutTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
        Me.LayoutTable.Controls.Add(Me.FollowPanel, 2, 8)
        Me.LayoutTable.Controls.Add(Me.ImagesTable, 0, 3)
        Me.LayoutTable.Controls.Add(Me.NameLabel, 0, 0)
        Me.LayoutTable.Controls.Add(Me.DoNotRunRandomlyCheckBox, 1, 10)
        Me.LayoutTable.Controls.Add(Me.ChanceNumber, 0, 6)
        Me.LayoutTable.Controls.Add(Me.LinkComboBox, 0, 10)
        Me.LayoutTable.Controls.Add(Me.SpeedNumber, 2, 1)
        Me.LayoutTable.Controls.Add(Me.StartSpeechLabel, 0, 7)
        Me.LayoutTable.Controls.Add(Me.EndSpeechLabel, 1, 7)
        Me.LayoutTable.Controls.Add(Me.StartSpeechComboBox, 0, 8)
        Me.LayoutTable.Controls.Add(Me.EndSpeechComboBox, 1, 8)
        Me.LayoutTable.Controls.Add(Me.FollowLabel, 2, 7)
        Me.LayoutTable.Controls.Add(Me.MaxDurationNumber, 1, 6)
        Me.LayoutTable.Controls.Add(Me.MovementLabel, 1, 0)
        Me.LayoutTable.Controls.Add(Me.MinDurationNumber, 2, 6)
        Me.LayoutTable.Controls.Add(Me.SpeedLabel, 2, 0)
        Me.LayoutTable.Controls.Add(Me.GroupLabel, 0, 2)
        Me.LayoutTable.Controls.Add(Me.DoNotRepeatAnimationsCheckBox, 1, 4)
        Me.LayoutTable.Controls.Add(Me.MovementComboBox, 1, 1)
        Me.LayoutTable.Controls.Add(Me.GroupNumber, 2, 2)
        Me.LayoutTable.Controls.Add(Me.NameTextBox, 0, 1)
        Me.LayoutTable.Controls.Add(Me.MinDurationLabel, 2, 5)
        Me.LayoutTable.Controls.Add(Me.ChanceLabel, 0, 5)
        Me.LayoutTable.Controls.Add(Me.MaxDurationLabel, 1, 5)
        Me.LayoutTable.Controls.Add(Me.LinkLabel, 0, 9)
        Me.LayoutTable.Location = New System.Drawing.Point(12, 12)
        Me.LayoutTable.Name = "LayoutTable"
        Me.LayoutTable.RowCount = 11
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
        Me.LayoutTable.Size = New System.Drawing.Size(475, 503)
        Me.LayoutTable.TabIndex = 0
        '
        'ImagesTable
        '
        Me.ImagesTable.AutoSize = True
        Me.ImagesTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ImagesTable.ColumnCount = 2
        Me.LayoutTable.SetColumnSpan(Me.ImagesTable, 3)
        Me.ImagesTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.ImagesTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.ImagesTable.Controls.Add(Me.LeftImageLabel, 0, 0)
        Me.ImagesTable.Controls.Add(Me.RightImageLabel, 1, 0)
        Me.ImagesTable.Controls.Add(Me.RightImageSelectButton, 1, 2)
        Me.ImagesTable.Controls.Add(Me.LeftImageBox, 0, 1)
        Me.ImagesTable.Controls.Add(Me.LeftImageSelectButton, 0, 2)
        Me.ImagesTable.Controls.Add(Me.RightImageBox, 1, 1)
        Me.ImagesTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ImagesTable.Location = New System.Drawing.Point(3, 93)
        Me.ImagesTable.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.ImagesTable.Name = "ImagesTable"
        Me.ImagesTable.RowCount = 3
        Me.ImagesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.ImagesTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.ImagesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.ImagesTable.Size = New System.Drawing.Size(469, 216)
        Me.ImagesTable.TabIndex = 8
        '
        'FollowPanel
        '
        Me.FollowPanel.AutoSize = True
        Me.FollowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FollowPanel.Controls.Add(Me.FollowTextBox)
        Me.FollowPanel.Controls.Add(Me.FollowSelectButton)
        Me.FollowPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FollowPanel.Location = New System.Drawing.Point(316, 423)
        Me.FollowPanel.Margin = New System.Windows.Forms.Padding(0)
        Me.FollowPanel.Name = "FollowPanel"
        Me.FollowPanel.Size = New System.Drawing.Size(159, 40)
        Me.FollowPanel.TabIndex = 21
        '
        'LinkLabel
        '
        Me.LinkLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.LinkLabel.AutoSize = True
        Me.LinkLabel.Location = New System.Drawing.Point(3, 463)
        Me.LinkLabel.Name = "LinkLabel"
        Me.LinkLabel.Size = New System.Drawing.Size(51, 13)
        Me.LinkLabel.TabIndex = 22
        Me.LinkLabel.Text = "Links To:"
        '
        'NewBehaviorDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(499, 562)
        Me.Controls.Add(Me.LayoutTable)
        Me.Controls.Add(Me.CommandTable)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(515, 500)
        Me.Name = "NewBehaviorDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create New Behavior..."
        Me.CommandTable.ResumeLayout(False)
        Me.LayoutTable.ResumeLayout(False)
        Me.LayoutTable.PerformLayout()
        Me.ImagesTable.ResumeLayout(False)
        Me.ImagesTable.PerformLayout()
        Me.FollowPanel.ResumeLayout(False)
        Me.FollowPanel.PerformLayout()
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
    Friend WithEvents MovementComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents MovementLabel As System.Windows.Forms.Label
    Friend WithEvents StartSpeechComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents EndSpeechComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents FollowTextBox As System.Windows.Forms.TextBox
    Friend WithEvents FollowSelectButton As System.Windows.Forms.Button
    Friend WithEvents LinkComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents ChanceLabel As System.Windows.Forms.Label
    Friend WithEvents MaxDurationLabel As System.Windows.Forms.Label
    Friend WithEvents MinDurationLabel As System.Windows.Forms.Label
    Friend WithEvents StartSpeechLabel As System.Windows.Forms.Label
    Friend WithEvents EndSpeechLabel As System.Windows.Forms.Label
    Friend WithEvents FollowLabel As System.Windows.Forms.Label
    Friend WithEvents SpeedLabel As System.Windows.Forms.Label
    Friend WithEvents DoNotRepeatAnimationsCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents DoNotRunRandomlyCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents GroupNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupLabel As System.Windows.Forms.Label
    Friend WithEvents MinDurationNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents MaxDurationNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents ChanceNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents SpeedNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents LayoutTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ImagesTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents FollowPanel As System.Windows.Forms.Panel
    Friend WithEvents LinkLabel As System.Windows.Forms.Label

End Class

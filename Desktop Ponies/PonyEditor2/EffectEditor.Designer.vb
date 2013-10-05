<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EffectEditor
    Inherits ItemEditorBase

    'UserControl overrides dispose to clean up the component list.
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
        Me.PropertiesTable = New System.Windows.Forms.TableLayoutPanel()
        Me.BehaviorComboBox = New System.Windows.Forms.ComboBox()
        Me.PreventLoopCheckBox = New System.Windows.Forms.CheckBox()
        Me.PreventLoopLabel = New System.Windows.Forms.Label()
        Me.FollowLabel = New System.Windows.Forms.Label()
        Me.BehaviorLabel = New System.Windows.Forms.Label()
        Me.RightCenterComboBox = New System.Windows.Forms.ComboBox()
        Me.RightPlacementComboBox = New System.Windows.Forms.ComboBox()
        Me.RightCenterLabel = New System.Windows.Forms.Label()
        Me.RightPlacementLabel = New System.Windows.Forms.Label()
        Me.LeftCenterComboBox = New System.Windows.Forms.ComboBox()
        Me.LeftCenterLabel = New System.Windows.Forms.Label()
        Me.LeftPlacementLabel = New System.Windows.Forms.Label()
        Me.RepeatDelayNumber = New System.Windows.Forms.NumericUpDown()
        Me.RepeatDelayLabel = New System.Windows.Forms.Label()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.RightImageFileSelector = New DesktopPonies.FileSelector()
        Me.LeftImageFileSelector = New DesktopPonies.FileSelector()
        Me.DurationLabel = New System.Windows.Forms.Label()
        Me.NameTextBox = New System.Windows.Forms.TextBox()
        Me.LeftImageLabel = New System.Windows.Forms.Label()
        Me.RightImageLabel = New System.Windows.Forms.Label()
        Me.LeftImageViewer = New DesktopPonies.EffectImageViewer()
        Me.RightImageViewer = New DesktopPonies.EffectImageViewer()
        Me.DurationNumber = New System.Windows.Forms.NumericUpDown()
        Me.LeftPlacementComboBox = New System.Windows.Forms.ComboBox()
        Me.FollowCheckBox = New System.Windows.Forms.CheckBox()
        Me.PropertiesPanel.SuspendLayout()
        Me.PropertiesTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'PropertiesPanel
        '
        Me.PropertiesPanel.Controls.Add(Me.PropertiesTable)
        '
        'PropertiesTable
        '
        Me.PropertiesTable.AutoSize = True
        Me.PropertiesTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.PropertiesTable.ColumnCount = 4
        Me.PropertiesTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.PropertiesTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.PropertiesTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.PropertiesTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.PropertiesTable.Controls.Add(Me.BehaviorComboBox, 3, 0)
        Me.PropertiesTable.Controls.Add(Me.PreventLoopCheckBox, 3, 2)
        Me.PropertiesTable.Controls.Add(Me.PreventLoopLabel, 2, 2)
        Me.PropertiesTable.Controls.Add(Me.FollowLabel, 2, 1)
        Me.PropertiesTable.Controls.Add(Me.BehaviorLabel, 2, 0)
        Me.PropertiesTable.Controls.Add(Me.RightCenterComboBox, 3, 5)
        Me.PropertiesTable.Controls.Add(Me.RightPlacementComboBox, 3, 4)
        Me.PropertiesTable.Controls.Add(Me.RightCenterLabel, 2, 5)
        Me.PropertiesTable.Controls.Add(Me.RightPlacementLabel, 2, 4)
        Me.PropertiesTable.Controls.Add(Me.LeftCenterComboBox, 1, 5)
        Me.PropertiesTable.Controls.Add(Me.LeftCenterLabel, 0, 5)
        Me.PropertiesTable.Controls.Add(Me.LeftPlacementLabel, 0, 4)
        Me.PropertiesTable.Controls.Add(Me.RepeatDelayNumber, 1, 2)
        Me.PropertiesTable.Controls.Add(Me.RepeatDelayLabel, 0, 2)
        Me.PropertiesTable.Controls.Add(Me.NameLabel, 0, 0)
        Me.PropertiesTable.Controls.Add(Me.RightImageFileSelector, 3, 6)
        Me.PropertiesTable.Controls.Add(Me.LeftImageFileSelector, 1, 6)
        Me.PropertiesTable.Controls.Add(Me.DurationLabel, 0, 1)
        Me.PropertiesTable.Controls.Add(Me.NameTextBox, 1, 0)
        Me.PropertiesTable.Controls.Add(Me.LeftImageLabel, 0, 6)
        Me.PropertiesTable.Controls.Add(Me.RightImageLabel, 2, 6)
        Me.PropertiesTable.Controls.Add(Me.LeftImageViewer, 0, 7)
        Me.PropertiesTable.Controls.Add(Me.RightImageViewer, 2, 7)
        Me.PropertiesTable.Controls.Add(Me.DurationNumber, 1, 1)
        Me.PropertiesTable.Controls.Add(Me.LeftPlacementComboBox, 1, 4)
        Me.PropertiesTable.Controls.Add(Me.FollowCheckBox, 3, 1)
        Me.PropertiesTable.Location = New System.Drawing.Point(0, 0)
        Me.PropertiesTable.Name = "PropertiesTable"
        Me.PropertiesTable.RowCount = 8
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.PropertiesTable.Size = New System.Drawing.Size(778, 188)
        Me.PropertiesTable.TabIndex = 0
        '
        'BehaviorComboBox
        '
        Me.BehaviorComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BehaviorComboBox.FormattingEnabled = True
        Me.BehaviorComboBox.Location = New System.Drawing.Point(516, 3)
        Me.BehaviorComboBox.Name = "BehaviorComboBox"
        Me.BehaviorComboBox.Size = New System.Drawing.Size(259, 21)
        Me.BehaviorComboBox.TabIndex = 7
        '
        'PreventLoopCheckBox
        '
        Me.PreventLoopCheckBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PreventLoopCheckBox.AutoSize = True
        Me.PreventLoopCheckBox.Location = New System.Drawing.Point(516, 56)
        Me.PreventLoopCheckBox.Name = "PreventLoopCheckBox"
        Me.PreventLoopCheckBox.Size = New System.Drawing.Size(15, 20)
        Me.PreventLoopCheckBox.TabIndex = 11
        Me.PreventLoopCheckBox.UseVisualStyleBackColor = True
        '
        'PreventLoopLabel
        '
        Me.PreventLoopLabel.AutoSize = True
        Me.PreventLoopLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.PreventLoopLabel.Location = New System.Drawing.Point(387, 59)
        Me.PreventLoopLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.PreventLoopLabel.Name = "PreventLoopLabel"
        Me.PreventLoopLabel.Size = New System.Drawing.Size(123, 13)
        Me.PreventLoopLabel.TabIndex = 10
        Me.PreventLoopLabel.Text = "Prevent Animation Loop:"
        Me.PreventLoopLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'FollowLabel
        '
        Me.FollowLabel.AutoSize = True
        Me.FollowLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.FollowLabel.Location = New System.Drawing.Point(387, 33)
        Me.FollowLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.FollowLabel.Name = "FollowLabel"
        Me.FollowLabel.Size = New System.Drawing.Size(123, 13)
        Me.FollowLabel.TabIndex = 8
        Me.FollowLabel.Text = "Effect Follows Pony:"
        Me.FollowLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'BehaviorLabel
        '
        Me.BehaviorLabel.AutoSize = True
        Me.BehaviorLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.BehaviorLabel.Location = New System.Drawing.Point(387, 6)
        Me.BehaviorLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.BehaviorLabel.Name = "BehaviorLabel"
        Me.BehaviorLabel.Size = New System.Drawing.Size(123, 13)
        Me.BehaviorLabel.TabIndex = 6
        Me.BehaviorLabel.Text = "Behavior:"
        Me.BehaviorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RightCenterComboBox
        '
        Me.RightCenterComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RightCenterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.RightCenterComboBox.FormattingEnabled = True
        Me.RightCenterComboBox.Location = New System.Drawing.Point(516, 129)
        Me.RightCenterComboBox.Name = "RightCenterComboBox"
        Me.RightCenterComboBox.Size = New System.Drawing.Size(259, 21)
        Me.RightCenterComboBox.TabIndex = 22
        '
        'RightPlacementComboBox
        '
        Me.RightPlacementComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RightPlacementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.RightPlacementComboBox.FormattingEnabled = True
        Me.RightPlacementComboBox.Location = New System.Drawing.Point(516, 102)
        Me.RightPlacementComboBox.Name = "RightPlacementComboBox"
        Me.RightPlacementComboBox.Size = New System.Drawing.Size(259, 21)
        Me.RightPlacementComboBox.TabIndex = 20
        '
        'RightCenterLabel
        '
        Me.RightCenterLabel.AutoSize = True
        Me.RightCenterLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.RightCenterLabel.Location = New System.Drawing.Point(387, 132)
        Me.RightCenterLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.RightCenterLabel.Name = "RightCenterLabel"
        Me.RightCenterLabel.Size = New System.Drawing.Size(123, 13)
        Me.RightCenterLabel.TabIndex = 21
        Me.RightCenterLabel.Text = "Right Image Center:"
        Me.RightCenterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RightPlacementLabel
        '
        Me.RightPlacementLabel.AutoSize = True
        Me.RightPlacementLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.RightPlacementLabel.Location = New System.Drawing.Point(387, 105)
        Me.RightPlacementLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.RightPlacementLabel.Name = "RightPlacementLabel"
        Me.RightPlacementLabel.Size = New System.Drawing.Size(123, 13)
        Me.RightPlacementLabel.TabIndex = 19
        Me.RightPlacementLabel.Text = "Right Image Placement:"
        Me.RightPlacementLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LeftCenterComboBox
        '
        Me.LeftCenterComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LeftCenterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.LeftCenterComboBox.FormattingEnabled = True
        Me.LeftCenterComboBox.Location = New System.Drawing.Point(122, 129)
        Me.LeftCenterComboBox.Name = "LeftCenterComboBox"
        Me.LeftCenterComboBox.Size = New System.Drawing.Size(259, 21)
        Me.LeftCenterComboBox.TabIndex = 15
        '
        'LeftCenterLabel
        '
        Me.LeftCenterLabel.AutoSize = True
        Me.LeftCenterLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.LeftCenterLabel.Location = New System.Drawing.Point(3, 132)
        Me.LeftCenterLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.LeftCenterLabel.Name = "LeftCenterLabel"
        Me.LeftCenterLabel.Size = New System.Drawing.Size(113, 13)
        Me.LeftCenterLabel.TabIndex = 14
        Me.LeftCenterLabel.Text = "Left Image Center:"
        Me.LeftCenterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LeftPlacementLabel
        '
        Me.LeftPlacementLabel.AutoSize = True
        Me.LeftPlacementLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.LeftPlacementLabel.Location = New System.Drawing.Point(3, 105)
        Me.LeftPlacementLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.LeftPlacementLabel.Name = "LeftPlacementLabel"
        Me.LeftPlacementLabel.Size = New System.Drawing.Size(113, 13)
        Me.LeftPlacementLabel.TabIndex = 12
        Me.LeftPlacementLabel.Text = "Left Image Placement:"
        Me.LeftPlacementLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RepeatDelayNumber
        '
        Me.RepeatDelayNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RepeatDelayNumber.DecimalPlaces = 2
        Me.RepeatDelayNumber.Location = New System.Drawing.Point(122, 56)
        Me.RepeatDelayNumber.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.RepeatDelayNumber.Name = "RepeatDelayNumber"
        Me.RepeatDelayNumber.Size = New System.Drawing.Size(259, 20)
        Me.RepeatDelayNumber.TabIndex = 5
        '
        'RepeatDelayLabel
        '
        Me.RepeatDelayLabel.AutoSize = True
        Me.RepeatDelayLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.RepeatDelayLabel.Location = New System.Drawing.Point(3, 59)
        Me.RepeatDelayLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.RepeatDelayLabel.Name = "RepeatDelayLabel"
        Me.RepeatDelayLabel.Size = New System.Drawing.Size(113, 13)
        Me.RepeatDelayLabel.TabIndex = 4
        Me.RepeatDelayLabel.Text = "Repeat Delay (sec):"
        Me.RepeatDelayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.NameLabel.Location = New System.Drawing.Point(3, 6)
        Me.NameLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(113, 13)
        Me.NameLabel.TabIndex = 0
        Me.NameLabel.Text = "Name:"
        Me.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RightImageFileSelector
        '
        Me.RightImageFileSelector.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RightImageFileSelector.Location = New System.Drawing.Point(516, 156)
        Me.RightImageFileSelector.Name = "RightImageFileSelector"
        Me.RightImageFileSelector.Size = New System.Drawing.Size(259, 23)
        Me.RightImageFileSelector.TabIndex = 24
        '
        'LeftImageFileSelector
        '
        Me.LeftImageFileSelector.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LeftImageFileSelector.Location = New System.Drawing.Point(122, 156)
        Me.LeftImageFileSelector.Name = "LeftImageFileSelector"
        Me.LeftImageFileSelector.Size = New System.Drawing.Size(259, 23)
        Me.LeftImageFileSelector.TabIndex = 17
        '
        'DurationLabel
        '
        Me.DurationLabel.AutoSize = True
        Me.DurationLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.DurationLabel.Location = New System.Drawing.Point(3, 33)
        Me.DurationLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.DurationLabel.Name = "DurationLabel"
        Me.DurationLabel.Size = New System.Drawing.Size(113, 13)
        Me.DurationLabel.TabIndex = 2
        Me.DurationLabel.Text = "Duration (sec):"
        Me.DurationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'NameTextBox
        '
        Me.NameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NameTextBox.Location = New System.Drawing.Point(122, 3)
        Me.NameTextBox.MaxLength = 50
        Me.NameTextBox.Name = "NameTextBox"
        Me.NameTextBox.Size = New System.Drawing.Size(259, 20)
        Me.NameTextBox.TabIndex = 1
        '
        'LeftImageLabel
        '
        Me.LeftImageLabel.AutoSize = True
        Me.LeftImageLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.LeftImageLabel.Location = New System.Drawing.Point(3, 159)
        Me.LeftImageLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.LeftImageLabel.Name = "LeftImageLabel"
        Me.LeftImageLabel.Size = New System.Drawing.Size(113, 13)
        Me.LeftImageLabel.TabIndex = 16
        Me.LeftImageLabel.Text = "Left Image:"
        Me.LeftImageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RightImageLabel
        '
        Me.RightImageLabel.AutoSize = True
        Me.RightImageLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.RightImageLabel.Location = New System.Drawing.Point(387, 159)
        Me.RightImageLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.RightImageLabel.Name = "RightImageLabel"
        Me.RightImageLabel.Size = New System.Drawing.Size(123, 13)
        Me.RightImageLabel.TabIndex = 23
        Me.RightImageLabel.Text = "Right Image:"
        Me.RightImageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LeftImageViewer
        '
        Me.LeftImageViewer.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LeftImageViewer.Animate = False
        Me.LeftImageViewer.AutoSize = True
        Me.LeftImageViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.LeftImageViewer.BackColor = System.Drawing.Color.White
        Me.LeftImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LeftImageViewer.Centering = DesktopPonies.Direction.TopLeft
        Me.PropertiesTable.SetColumnSpan(Me.LeftImageViewer, 2)
        Me.LeftImageViewer.EffectImage = Nothing
        Me.LeftImageViewer.Location = New System.Drawing.Point(192, 185)
        Me.LeftImageViewer.Name = "LeftImageViewer"
        Me.LeftImageViewer.Placement = DesktopPonies.Direction.TopLeft
        Me.LeftImageViewer.Size = New System.Drawing.Size(0, 0)
        Me.LeftImageViewer.TabIndex = 18
        '
        'RightImageViewer
        '
        Me.RightImageViewer.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.RightImageViewer.Animate = False
        Me.RightImageViewer.AutoSize = True
        Me.RightImageViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.RightImageViewer.BackColor = System.Drawing.Color.White
        Me.RightImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RightImageViewer.Centering = DesktopPonies.Direction.TopLeft
        Me.PropertiesTable.SetColumnSpan(Me.RightImageViewer, 2)
        Me.RightImageViewer.EffectImage = Nothing
        Me.RightImageViewer.Location = New System.Drawing.Point(581, 185)
        Me.RightImageViewer.Name = "RightImageViewer"
        Me.RightImageViewer.Placement = DesktopPonies.Direction.TopLeft
        Me.RightImageViewer.Size = New System.Drawing.Size(0, 0)
        Me.RightImageViewer.TabIndex = 25
        '
        'DurationNumber
        '
        Me.DurationNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DurationNumber.DecimalPlaces = 2
        Me.DurationNumber.Location = New System.Drawing.Point(122, 30)
        Me.DurationNumber.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.DurationNumber.Name = "DurationNumber"
        Me.DurationNumber.Size = New System.Drawing.Size(259, 20)
        Me.DurationNumber.TabIndex = 3
        '
        'LeftPlacementComboBox
        '
        Me.LeftPlacementComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LeftPlacementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.LeftPlacementComboBox.FormattingEnabled = True
        Me.LeftPlacementComboBox.Location = New System.Drawing.Point(122, 102)
        Me.LeftPlacementComboBox.Name = "LeftPlacementComboBox"
        Me.LeftPlacementComboBox.Size = New System.Drawing.Size(259, 21)
        Me.LeftPlacementComboBox.TabIndex = 13
        '
        'FollowCheckBox
        '
        Me.FollowCheckBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.FollowCheckBox.AutoSize = True
        Me.FollowCheckBox.Location = New System.Drawing.Point(516, 30)
        Me.FollowCheckBox.Name = "FollowCheckBox"
        Me.FollowCheckBox.Size = New System.Drawing.Size(15, 20)
        Me.FollowCheckBox.TabIndex = 9
        Me.FollowCheckBox.UseVisualStyleBackColor = True
        '
        'EffectEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Name = "EffectEditor"
        Me.PropertiesPanel.ResumeLayout(False)
        Me.PropertiesPanel.PerformLayout()
        Me.PropertiesTable.ResumeLayout(False)
        Me.PropertiesTable.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PropertiesTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents LeftImageLabel As System.Windows.Forms.Label
    Friend WithEvents RightImageLabel As System.Windows.Forms.Label
    Friend WithEvents DurationLabel As System.Windows.Forms.Label
    Friend WithEvents NameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents LeftImageFileSelector As DesktopPonies.FileSelector
    Friend WithEvents RightImageFileSelector As DesktopPonies.FileSelector
    Friend WithEvents DurationNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents LeftImageViewer As DesktopPonies.EffectImageViewer
    Friend WithEvents RightImageViewer As DesktopPonies.EffectImageViewer
    Friend WithEvents RepeatDelayNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents RepeatDelayLabel As System.Windows.Forms.Label
    Friend WithEvents LeftCenterComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents LeftCenterLabel As System.Windows.Forms.Label
    Friend WithEvents LeftPlacementLabel As System.Windows.Forms.Label
    Friend WithEvents LeftPlacementComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents RightCenterComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents RightPlacementComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents RightCenterLabel As System.Windows.Forms.Label
    Friend WithEvents RightPlacementLabel As System.Windows.Forms.Label
    Friend WithEvents PreventLoopLabel As System.Windows.Forms.Label
    Friend WithEvents FollowLabel As System.Windows.Forms.Label
    Friend WithEvents BehaviorLabel As System.Windows.Forms.Label
    Friend WithEvents FollowCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents PreventLoopCheckBox As System.Windows.Forms.CheckBox
    Friend WithEvents BehaviorComboBox As System.Windows.Forms.ComboBox

End Class

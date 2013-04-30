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
        Me.RightImageFileSelector = New VBDesktopPonies.FileSelector()
        Me.LeftImageFileSelector = New VBDesktopPonies.FileSelector()
        Me.DurationLabel = New System.Windows.Forms.Label()
        Me.NameTextBox = New System.Windows.Forms.TextBox()
        Me.LeftImageLabel = New System.Windows.Forms.Label()
        Me.RightImageLabel = New System.Windows.Forms.Label()
        Me.LeftImageViewer = New VBDesktopPonies.EffectImageViewer()
        Me.RightImageViewer = New VBDesktopPonies.EffectImageViewer()
        Me.DurationNumber = New System.Windows.Forms.NumericUpDown()
        Me.LeftPlacementComboBox = New System.Windows.Forms.ComboBox()
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
        Me.PropertiesTable.Size = New System.Drawing.Size(775, 187)
        Me.PropertiesTable.TabIndex = 0
        '
        'RightCenterComboBox
        '
        Me.RightCenterComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RightCenterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.RightCenterComboBox.FormattingEnabled = True
        Me.RightCenterComboBox.Location = New System.Drawing.Point(513, 128)
        Me.RightCenterComboBox.Name = "RightCenterComboBox"
        Me.RightCenterComboBox.Size = New System.Drawing.Size(259, 21)
        Me.RightCenterComboBox.TabIndex = 16
        '
        'RightPlacementComboBox
        '
        Me.RightPlacementComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RightPlacementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.RightPlacementComboBox.FormattingEnabled = True
        Me.RightPlacementComboBox.Location = New System.Drawing.Point(513, 101)
        Me.RightPlacementComboBox.Name = "RightPlacementComboBox"
        Me.RightPlacementComboBox.Size = New System.Drawing.Size(259, 21)
        Me.RightPlacementComboBox.TabIndex = 14
        '
        'RightCenterLabel
        '
        Me.RightCenterLabel.AutoSize = True
        Me.RightCenterLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.RightCenterLabel.Location = New System.Drawing.Point(387, 131)
        Me.RightCenterLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.RightCenterLabel.Name = "RightCenterLabel"
        Me.RightCenterLabel.Size = New System.Drawing.Size(120, 13)
        Me.RightCenterLabel.TabIndex = 15
        Me.RightCenterLabel.Text = "Right Image Center:"
        Me.RightCenterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RightPlacementLabel
        '
        Me.RightPlacementLabel.AutoSize = True
        Me.RightPlacementLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.RightPlacementLabel.Location = New System.Drawing.Point(387, 104)
        Me.RightPlacementLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.RightPlacementLabel.Name = "RightPlacementLabel"
        Me.RightPlacementLabel.Size = New System.Drawing.Size(120, 13)
        Me.RightPlacementLabel.TabIndex = 13
        Me.RightPlacementLabel.Text = "Right Image Placement:"
        Me.RightPlacementLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LeftCenterComboBox
        '
        Me.LeftCenterComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LeftCenterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.LeftCenterComboBox.FormattingEnabled = True
        Me.LeftCenterComboBox.Location = New System.Drawing.Point(122, 128)
        Me.LeftCenterComboBox.Name = "LeftCenterComboBox"
        Me.LeftCenterComboBox.Size = New System.Drawing.Size(259, 21)
        Me.LeftCenterComboBox.TabIndex = 9
        '
        'LeftCenterLabel
        '
        Me.LeftCenterLabel.AutoSize = True
        Me.LeftCenterLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.LeftCenterLabel.Location = New System.Drawing.Point(3, 131)
        Me.LeftCenterLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.LeftCenterLabel.Name = "LeftCenterLabel"
        Me.LeftCenterLabel.Size = New System.Drawing.Size(113, 13)
        Me.LeftCenterLabel.TabIndex = 8
        Me.LeftCenterLabel.Text = "Left Image Center:"
        Me.LeftCenterLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LeftPlacementLabel
        '
        Me.LeftPlacementLabel.AutoSize = True
        Me.LeftPlacementLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.LeftPlacementLabel.Location = New System.Drawing.Point(3, 104)
        Me.LeftPlacementLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.LeftPlacementLabel.Name = "LeftPlacementLabel"
        Me.LeftPlacementLabel.Size = New System.Drawing.Size(113, 13)
        Me.LeftPlacementLabel.TabIndex = 6
        Me.LeftPlacementLabel.Text = "Left Image Placement:"
        Me.LeftPlacementLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RepeatDelayNumber
        '
        Me.RepeatDelayNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RepeatDelayNumber.DecimalPlaces = 2
        Me.RepeatDelayNumber.Location = New System.Drawing.Point(122, 55)
        Me.RepeatDelayNumber.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.RepeatDelayNumber.Name = "RepeatDelayNumber"
        Me.RepeatDelayNumber.Size = New System.Drawing.Size(259, 20)
        Me.RepeatDelayNumber.TabIndex = 5
        '
        'RepeatDelayLabel
        '
        Me.RepeatDelayLabel.AutoSize = True
        Me.RepeatDelayLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.RepeatDelayLabel.Location = New System.Drawing.Point(3, 58)
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
        Me.RightImageFileSelector.Location = New System.Drawing.Point(513, 155)
        Me.RightImageFileSelector.Name = "RightImageFileSelector"
        Me.RightImageFileSelector.Size = New System.Drawing.Size(259, 23)
        Me.RightImageFileSelector.TabIndex = 18
        '
        'LeftImageFileSelector
        '
        Me.LeftImageFileSelector.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LeftImageFileSelector.Location = New System.Drawing.Point(122, 155)
        Me.LeftImageFileSelector.Name = "LeftImageFileSelector"
        Me.LeftImageFileSelector.Size = New System.Drawing.Size(259, 23)
        Me.LeftImageFileSelector.TabIndex = 11
        '
        'DurationLabel
        '
        Me.DurationLabel.AutoSize = True
        Me.DurationLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.DurationLabel.Location = New System.Drawing.Point(3, 32)
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
        Me.LeftImageLabel.Location = New System.Drawing.Point(3, 158)
        Me.LeftImageLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.LeftImageLabel.Name = "LeftImageLabel"
        Me.LeftImageLabel.Size = New System.Drawing.Size(113, 13)
        Me.LeftImageLabel.TabIndex = 10
        Me.LeftImageLabel.Text = "Left Image:"
        Me.LeftImageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RightImageLabel
        '
        Me.RightImageLabel.AutoSize = True
        Me.RightImageLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.RightImageLabel.Location = New System.Drawing.Point(387, 158)
        Me.RightImageLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.RightImageLabel.Name = "RightImageLabel"
        Me.RightImageLabel.Size = New System.Drawing.Size(120, 13)
        Me.RightImageLabel.TabIndex = 17
        Me.RightImageLabel.Text = "Right Image:"
        Me.RightImageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LeftImageViewer
        '
        Me.LeftImageViewer.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.LeftImageViewer.AutoSize = True
        Me.LeftImageViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.LeftImageViewer.BackColor = System.Drawing.Color.White
        Me.LeftImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LeftImageViewer.Centering = VBDesktopPonies.Direction.TopLeft
        Me.PropertiesTable.SetColumnSpan(Me.LeftImageViewer, 2)
        Me.LeftImageViewer.EffectImage = Nothing
        Me.LeftImageViewer.Location = New System.Drawing.Point(192, 184)
        Me.LeftImageViewer.Name = "LeftImageViewer"
        Me.LeftImageViewer.Placement = VBDesktopPonies.Direction.TopLeft
        Me.LeftImageViewer.Size = New System.Drawing.Size(0, 0)
        Me.LeftImageViewer.TabIndex = 12
        '
        'RightImageViewer
        '
        Me.RightImageViewer.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.RightImageViewer.AutoSize = True
        Me.RightImageViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.RightImageViewer.BackColor = System.Drawing.Color.White
        Me.RightImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.RightImageViewer.Centering = VBDesktopPonies.Direction.TopLeft
        Me.PropertiesTable.SetColumnSpan(Me.RightImageViewer, 2)
        Me.RightImageViewer.EffectImage = Nothing
        Me.RightImageViewer.Location = New System.Drawing.Point(579, 184)
        Me.RightImageViewer.Name = "RightImageViewer"
        Me.RightImageViewer.Placement = VBDesktopPonies.Direction.TopLeft
        Me.RightImageViewer.Size = New System.Drawing.Size(0, 0)
        Me.RightImageViewer.TabIndex = 19
        '
        'DurationNumber
        '
        Me.DurationNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DurationNumber.DecimalPlaces = 2
        Me.DurationNumber.Location = New System.Drawing.Point(122, 29)
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
        Me.LeftPlacementComboBox.Location = New System.Drawing.Point(122, 101)
        Me.LeftPlacementComboBox.Name = "LeftPlacementComboBox"
        Me.LeftPlacementComboBox.Size = New System.Drawing.Size(259, 21)
        Me.LeftPlacementComboBox.TabIndex = 7
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
    Friend WithEvents LeftImageFileSelector As VBDesktopPonies.FileSelector
    Friend WithEvents RightImageFileSelector As VBDesktopPonies.FileSelector
    Friend WithEvents DurationNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents LeftImageViewer As VBDesktopPonies.EffectImageViewer
    Friend WithEvents RightImageViewer As VBDesktopPonies.EffectImageViewer
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

End Class

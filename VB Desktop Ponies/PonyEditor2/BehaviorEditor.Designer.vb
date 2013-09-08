<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BehaviorEditor
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
        Me.TargetLabel = New System.Windows.Forms.Label()
        Me.LinkedBehaviorComboBox = New System.Windows.Forms.ComboBox()
        Me.LinkedBehaviorLabel = New System.Windows.Forms.Label()
        Me.EndSpeechComboBox = New VBDesktopPonies.ComboBoxCaseSensitive()
        Me.StartSpeechComboBox = New VBDesktopPonies.ComboBoxCaseSensitive()
        Me.EndSpeechLabel = New System.Windows.Forms.Label()
        Me.StartSpeechLabel = New System.Windows.Forms.Label()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.GroupLabel = New System.Windows.Forms.Label()
        Me.ChanceLabel = New System.Windows.Forms.Label()
        Me.RightImageFileSelector = New VBDesktopPonies.FileSelector()
        Me.LeftImageFileSelector = New VBDesktopPonies.FileSelector()
        Me.SpeedLabel = New System.Windows.Forms.Label()
        Me.MovementLabel = New System.Windows.Forms.Label()
        Me.MinDurationLabel = New System.Windows.Forms.Label()
        Me.MaxDurationLabel = New System.Windows.Forms.Label()
        Me.NameTextBox = New System.Windows.Forms.TextBox()
        Me.MovementComboBox = New System.Windows.Forms.ComboBox()
        Me.MinDurationNumber = New System.Windows.Forms.NumericUpDown()
        Me.MaxDurationNumber = New System.Windows.Forms.NumericUpDown()
        Me.ChanceNumber = New System.Windows.Forms.NumericUpDown()
        Me.SpeedNumber = New System.Windows.Forms.NumericUpDown()
        Me.GroupNumber = New System.Windows.Forms.NumericUpDown()
        Me.LeftImageLabel = New System.Windows.Forms.Label()
        Me.RightImageLabel = New System.Windows.Forms.Label()
        Me.LeftImageViewer = New VBDesktopPonies.AnimatedImageViewer()
        Me.RightImageViewer = New VBDesktopPonies.AnimatedImageViewer()
        Me.TargetButton = New System.Windows.Forms.Button()
        Me.PropertiesPanel.SuspendLayout()
        Me.PropertiesTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'PropertiesPanel
        '
        Me.PropertiesPanel.Controls.Add(Me.PropertiesTable)
        Me.PropertiesPanel.TabIndex = 2
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
        Me.PropertiesTable.Controls.Add(Me.TargetLabel, 0, 5)
        Me.PropertiesTable.Controls.Add(Me.LinkedBehaviorComboBox, 3, 4)
        Me.PropertiesTable.Controls.Add(Me.LinkedBehaviorLabel, 2, 4)
        Me.PropertiesTable.Controls.Add(Me.EndSpeechComboBox, 3, 3)
        Me.PropertiesTable.Controls.Add(Me.StartSpeechComboBox, 3, 2)
        Me.PropertiesTable.Controls.Add(Me.EndSpeechLabel, 2, 3)
        Me.PropertiesTable.Controls.Add(Me.StartSpeechLabel, 2, 2)
        Me.PropertiesTable.Controls.Add(Me.NameLabel, 0, 0)
        Me.PropertiesTable.Controls.Add(Me.GroupLabel, 0, 1)
        Me.PropertiesTable.Controls.Add(Me.ChanceLabel, 0, 2)
        Me.PropertiesTable.Controls.Add(Me.RightImageFileSelector, 3, 6)
        Me.PropertiesTable.Controls.Add(Me.LeftImageFileSelector, 1, 6)
        Me.PropertiesTable.Controls.Add(Me.SpeedLabel, 0, 3)
        Me.PropertiesTable.Controls.Add(Me.MovementLabel, 0, 4)
        Me.PropertiesTable.Controls.Add(Me.MinDurationLabel, 2, 0)
        Me.PropertiesTable.Controls.Add(Me.MaxDurationLabel, 2, 1)
        Me.PropertiesTable.Controls.Add(Me.NameTextBox, 1, 0)
        Me.PropertiesTable.Controls.Add(Me.MovementComboBox, 1, 4)
        Me.PropertiesTable.Controls.Add(Me.MinDurationNumber, 3, 0)
        Me.PropertiesTable.Controls.Add(Me.MaxDurationNumber, 3, 1)
        Me.PropertiesTable.Controls.Add(Me.ChanceNumber, 1, 2)
        Me.PropertiesTable.Controls.Add(Me.SpeedNumber, 1, 3)
        Me.PropertiesTable.Controls.Add(Me.GroupNumber, 1, 1)
        Me.PropertiesTable.Controls.Add(Me.LeftImageLabel, 0, 6)
        Me.PropertiesTable.Controls.Add(Me.RightImageLabel, 2, 6)
        Me.PropertiesTable.Controls.Add(Me.LeftImageViewer, 0, 7)
        Me.PropertiesTable.Controls.Add(Me.RightImageViewer, 2, 7)
        Me.PropertiesTable.Controls.Add(Me.TargetButton, 1, 5)
        Me.PropertiesTable.Location = New System.Drawing.Point(0, 0)
        Me.PropertiesTable.Name = "PropertiesTable"
        Me.PropertiesTable.RowCount = 8
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.Size = New System.Drawing.Size(701, 197)
        Me.PropertiesTable.TabIndex = 0
        '
        'TargetLabel
        '
        Me.TargetLabel.AutoSize = True
        Me.TargetLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TargetLabel.Location = New System.Drawing.Point(3, 139)
        Me.TargetLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.TargetLabel.Name = "TargetLabel"
        Me.TargetLabel.Size = New System.Drawing.Size(60, 13)
        Me.TargetLabel.TabIndex = 20
        Me.TargetLabel.Text = "Target:"
        Me.TargetLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'LinkedBehaviorComboBox
        '
        Me.LinkedBehaviorComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LinkedBehaviorComboBox.DisplayMember = "Name"
        Me.LinkedBehaviorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.LinkedBehaviorComboBox.FormattingEnabled = True
        Me.LinkedBehaviorComboBox.Location = New System.Drawing.Point(439, 109)
        Me.LinkedBehaviorComboBox.Name = "LinkedBehaviorComboBox"
        Me.LinkedBehaviorComboBox.Size = New System.Drawing.Size(259, 21)
        Me.LinkedBehaviorComboBox.TabIndex = 19
        '
        'LinkedBehaviorLabel
        '
        Me.LinkedBehaviorLabel.AutoSize = True
        Me.LinkedBehaviorLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.LinkedBehaviorLabel.Location = New System.Drawing.Point(334, 112)
        Me.LinkedBehaviorLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.LinkedBehaviorLabel.Name = "LinkedBehaviorLabel"
        Me.LinkedBehaviorLabel.Size = New System.Drawing.Size(99, 13)
        Me.LinkedBehaviorLabel.TabIndex = 18
        Me.LinkedBehaviorLabel.Text = "Linked Behavior:"
        Me.LinkedBehaviorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'EndSpeechComboBox
        '
        Me.EndSpeechComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EndSpeechComboBox.Location = New System.Drawing.Point(439, 82)
        Me.EndSpeechComboBox.Name = "EndSpeechComboBox"
        Me.EndSpeechComboBox.Size = New System.Drawing.Size(259, 21)
        Me.EndSpeechComboBox.TabIndex = 17
        '
        'StartSpeechComboBox
        '
        Me.StartSpeechComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.StartSpeechComboBox.Location = New System.Drawing.Point(439, 55)
        Me.StartSpeechComboBox.Name = "StartSpeechComboBox"
        Me.StartSpeechComboBox.Size = New System.Drawing.Size(259, 21)
        Me.StartSpeechComboBox.TabIndex = 15
        '
        'EndSpeechLabel
        '
        Me.EndSpeechLabel.AutoSize = True
        Me.EndSpeechLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.EndSpeechLabel.Location = New System.Drawing.Point(334, 85)
        Me.EndSpeechLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.EndSpeechLabel.Name = "EndSpeechLabel"
        Me.EndSpeechLabel.Size = New System.Drawing.Size(99, 13)
        Me.EndSpeechLabel.TabIndex = 16
        Me.EndSpeechLabel.Text = "End Speech:"
        Me.EndSpeechLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'StartSpeechLabel
        '
        Me.StartSpeechLabel.AutoSize = True
        Me.StartSpeechLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.StartSpeechLabel.Location = New System.Drawing.Point(334, 58)
        Me.StartSpeechLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.StartSpeechLabel.Name = "StartSpeechLabel"
        Me.StartSpeechLabel.Size = New System.Drawing.Size(99, 13)
        Me.StartSpeechLabel.TabIndex = 14
        Me.StartSpeechLabel.Text = "Start Speech:"
        Me.StartSpeechLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.NameLabel.Location = New System.Drawing.Point(3, 6)
        Me.NameLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(60, 13)
        Me.NameLabel.TabIndex = 0
        Me.NameLabel.Text = "Name:"
        Me.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'GroupLabel
        '
        Me.GroupLabel.AutoSize = True
        Me.GroupLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.GroupLabel.Location = New System.Drawing.Point(3, 32)
        Me.GroupLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.GroupLabel.Name = "GroupLabel"
        Me.GroupLabel.Size = New System.Drawing.Size(60, 13)
        Me.GroupLabel.TabIndex = 2
        Me.GroupLabel.Text = "Group:"
        Me.GroupLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ChanceLabel
        '
        Me.ChanceLabel.AutoSize = True
        Me.ChanceLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.ChanceLabel.Location = New System.Drawing.Point(3, 58)
        Me.ChanceLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.ChanceLabel.Name = "ChanceLabel"
        Me.ChanceLabel.Size = New System.Drawing.Size(60, 13)
        Me.ChanceLabel.TabIndex = 4
        Me.ChanceLabel.Text = "Chance:"
        Me.ChanceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RightImageFileSelector
        '
        Me.RightImageFileSelector.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RightImageFileSelector.Location = New System.Drawing.Point(439, 165)
        Me.RightImageFileSelector.Name = "RightImageFileSelector"
        Me.RightImageFileSelector.Size = New System.Drawing.Size(259, 23)
        Me.RightImageFileSelector.TabIndex = 26
        '
        'LeftImageFileSelector
        '
        Me.LeftImageFileSelector.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LeftImageFileSelector.Location = New System.Drawing.Point(69, 165)
        Me.LeftImageFileSelector.Name = "LeftImageFileSelector"
        Me.LeftImageFileSelector.Size = New System.Drawing.Size(259, 23)
        Me.LeftImageFileSelector.TabIndex = 23
        '
        'SpeedLabel
        '
        Me.SpeedLabel.AutoSize = True
        Me.SpeedLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.SpeedLabel.Location = New System.Drawing.Point(3, 85)
        Me.SpeedLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.SpeedLabel.Name = "SpeedLabel"
        Me.SpeedLabel.Size = New System.Drawing.Size(60, 13)
        Me.SpeedLabel.TabIndex = 6
        Me.SpeedLabel.Text = "Speed:"
        Me.SpeedLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'MovementLabel
        '
        Me.MovementLabel.AutoSize = True
        Me.MovementLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.MovementLabel.Location = New System.Drawing.Point(3, 112)
        Me.MovementLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.MovementLabel.Name = "MovementLabel"
        Me.MovementLabel.Size = New System.Drawing.Size(60, 13)
        Me.MovementLabel.TabIndex = 8
        Me.MovementLabel.Text = "Movement:"
        Me.MovementLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'MinDurationLabel
        '
        Me.MinDurationLabel.AutoSize = True
        Me.MinDurationLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.MinDurationLabel.Location = New System.Drawing.Point(334, 6)
        Me.MinDurationLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.MinDurationLabel.Name = "MinDurationLabel"
        Me.MinDurationLabel.Size = New System.Drawing.Size(99, 13)
        Me.MinDurationLabel.TabIndex = 10
        Me.MinDurationLabel.Text = "Min Duration (sec):"
        Me.MinDurationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'MaxDurationLabel
        '
        Me.MaxDurationLabel.AutoSize = True
        Me.MaxDurationLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.MaxDurationLabel.Location = New System.Drawing.Point(334, 32)
        Me.MaxDurationLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.MaxDurationLabel.Name = "MaxDurationLabel"
        Me.MaxDurationLabel.Size = New System.Drawing.Size(99, 13)
        Me.MaxDurationLabel.TabIndex = 12
        Me.MaxDurationLabel.Text = "Max Duration (sec):"
        Me.MaxDurationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'NameTextBox
        '
        Me.NameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NameTextBox.Location = New System.Drawing.Point(69, 3)
        Me.NameTextBox.MaxLength = 50
        Me.NameTextBox.Name = "NameTextBox"
        Me.NameTextBox.Size = New System.Drawing.Size(259, 20)
        Me.NameTextBox.TabIndex = 1
        '
        'MovementComboBox
        '
        Me.MovementComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MovementComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.MovementComboBox.FormattingEnabled = True
        Me.MovementComboBox.Location = New System.Drawing.Point(69, 109)
        Me.MovementComboBox.Name = "MovementComboBox"
        Me.MovementComboBox.Size = New System.Drawing.Size(259, 21)
        Me.MovementComboBox.TabIndex = 9
        '
        'MinDurationNumber
        '
        Me.MinDurationNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MinDurationNumber.DecimalPlaces = 2
        Me.MinDurationNumber.Location = New System.Drawing.Point(439, 3)
        Me.MinDurationNumber.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.MinDurationNumber.Name = "MinDurationNumber"
        Me.MinDurationNumber.Size = New System.Drawing.Size(259, 20)
        Me.MinDurationNumber.TabIndex = 11
        '
        'MaxDurationNumber
        '
        Me.MaxDurationNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MaxDurationNumber.DecimalPlaces = 2
        Me.MaxDurationNumber.Location = New System.Drawing.Point(439, 29)
        Me.MaxDurationNumber.Maximum = New Decimal(New Integer() {300, 0, 0, 0})
        Me.MaxDurationNumber.Name = "MaxDurationNumber"
        Me.MaxDurationNumber.Size = New System.Drawing.Size(259, 20)
        Me.MaxDurationNumber.TabIndex = 13
        '
        'ChanceNumber
        '
        Me.ChanceNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ChanceNumber.Location = New System.Drawing.Point(69, 55)
        Me.ChanceNumber.Name = "ChanceNumber"
        Me.ChanceNumber.Size = New System.Drawing.Size(259, 20)
        Me.ChanceNumber.TabIndex = 5
        '
        'SpeedNumber
        '
        Me.SpeedNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SpeedNumber.DecimalPlaces = 2
        Me.SpeedNumber.Location = New System.Drawing.Point(69, 82)
        Me.SpeedNumber.Maximum = New Decimal(New Integer() {25, 0, 0, 0})
        Me.SpeedNumber.Name = "SpeedNumber"
        Me.SpeedNumber.Size = New System.Drawing.Size(259, 20)
        Me.SpeedNumber.TabIndex = 7
        '
        'GroupNumber
        '
        Me.GroupNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupNumber.Location = New System.Drawing.Point(69, 29)
        Me.GroupNumber.Name = "GroupNumber"
        Me.GroupNumber.Size = New System.Drawing.Size(259, 20)
        Me.GroupNumber.TabIndex = 3
        '
        'LeftImageLabel
        '
        Me.LeftImageLabel.AutoSize = True
        Me.LeftImageLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.LeftImageLabel.Location = New System.Drawing.Point(3, 168)
        Me.LeftImageLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.LeftImageLabel.Name = "LeftImageLabel"
        Me.LeftImageLabel.Size = New System.Drawing.Size(60, 13)
        Me.LeftImageLabel.TabIndex = 22
        Me.LeftImageLabel.Text = "Left Image:"
        Me.LeftImageLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'RightImageLabel
        '
        Me.RightImageLabel.AutoSize = True
        Me.RightImageLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.RightImageLabel.Location = New System.Drawing.Point(334, 168)
        Me.RightImageLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.RightImageLabel.Name = "RightImageLabel"
        Me.RightImageLabel.Size = New System.Drawing.Size(99, 13)
        Me.RightImageLabel.TabIndex = 25
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
        Me.PropertiesTable.SetColumnSpan(Me.LeftImageViewer, 2)
        Me.LeftImageViewer.Location = New System.Drawing.Point(165, 194)
        Me.LeftImageViewer.Name = "LeftImageViewer"
        Me.LeftImageViewer.Size = New System.Drawing.Size(0, 0)
        Me.LeftImageViewer.TabIndex = 24
        '
        'RightImageViewer
        '
        Me.RightImageViewer.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.RightImageViewer.Animate = False
        Me.RightImageViewer.AutoSize = True
        Me.RightImageViewer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.RightImageViewer.BackColor = System.Drawing.Color.White
        Me.RightImageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PropertiesTable.SetColumnSpan(Me.RightImageViewer, 2)
        Me.RightImageViewer.Location = New System.Drawing.Point(516, 194)
        Me.RightImageViewer.Name = "RightImageViewer"
        Me.RightImageViewer.Size = New System.Drawing.Size(0, 0)
        Me.RightImageViewer.TabIndex = 27
        '
        'TargetButton
        '
        Me.TargetButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TargetButton.Enabled = False
        Me.TargetButton.Location = New System.Drawing.Point(69, 136)
        Me.TargetButton.Name = "TargetButton"
        Me.TargetButton.Size = New System.Drawing.Size(259, 23)
        Me.TargetButton.TabIndex = 21
        Me.TargetButton.Text = "Setup Target..."
        Me.TargetButton.UseVisualStyleBackColor = True
        '
        'BehaviorEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Name = "BehaviorEditor"
        Me.PropertiesPanel.ResumeLayout(False)
        Me.PropertiesPanel.PerformLayout()
        Me.PropertiesTable.ResumeLayout(False)
        Me.PropertiesTable.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PropertiesTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents GroupLabel As System.Windows.Forms.Label
    Friend WithEvents ChanceLabel As System.Windows.Forms.Label
    Friend WithEvents SpeedLabel As System.Windows.Forms.Label
    Friend WithEvents MovementLabel As System.Windows.Forms.Label
    Friend WithEvents LeftImageLabel As System.Windows.Forms.Label
    Friend WithEvents RightImageLabel As System.Windows.Forms.Label
    Friend WithEvents MinDurationLabel As System.Windows.Forms.Label
    Friend WithEvents MaxDurationLabel As System.Windows.Forms.Label
    Friend WithEvents NameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents LeftImageFileSelector As VBDesktopPonies.FileSelector
    Friend WithEvents RightImageFileSelector As VBDesktopPonies.FileSelector
    Friend WithEvents MovementComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents MinDurationNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents MaxDurationNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents ChanceNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents SpeedNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents GroupNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents LeftImageViewer As VBDesktopPonies.AnimatedImageViewer
    Friend WithEvents RightImageViewer As VBDesktopPonies.AnimatedImageViewer
    Friend WithEvents EndSpeechComboBox As VBDesktopPonies.ComboBoxCaseSensitive
    Friend WithEvents StartSpeechComboBox As VBDesktopPonies.ComboBoxCaseSensitive
    Friend WithEvents EndSpeechLabel As System.Windows.Forms.Label
    Friend WithEvents StartSpeechLabel As System.Windows.Forms.Label
    Friend WithEvents LinkedBehaviorComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents LinkedBehaviorLabel As System.Windows.Forms.Label
    Friend WithEvents TargetLabel As System.Windows.Forms.Label
    Friend WithEvents TargetButton As System.Windows.Forms.Button

End Class

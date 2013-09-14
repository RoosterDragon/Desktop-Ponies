<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InteractionEditor
    Inherits DesktopPonies.ItemEditorBase

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PropertiesTable = New System.Windows.Forms.TableLayoutPanel()
        Me.BehaviorsLabel = New System.Windows.Forms.Label()
        Me.TargetsLabel = New System.Windows.Forms.Label()
        Me.TypeComboBox = New System.Windows.Forms.ComboBox()
        Me.TypeLabel = New System.Windows.Forms.Label()
        Me.DelayNumber = New System.Windows.Forms.NumericUpDown()
        Me.DelayLabel = New System.Windows.Forms.Label()
        Me.ProximityNumber = New System.Windows.Forms.NumericUpDown()
        Me.ProximityLabel = New System.Windows.Forms.Label()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.ChanceLabel = New System.Windows.Forms.Label()
        Me.NameTextBox = New System.Windows.Forms.TextBox()
        Me.ChanceNumber = New System.Windows.Forms.NumericUpDown()
        Me.TargetsList = New System.Windows.Forms.CheckedListBox()
        Me.BehaviorsList = New System.Windows.Forms.CheckedListBox()
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
        Me.PropertiesTable.Controls.Add(Me.BehaviorsLabel, 2, 3)
        Me.PropertiesTable.Controls.Add(Me.TargetsLabel, 0, 3)
        Me.PropertiesTable.Controls.Add(Me.TypeComboBox, 1, 2)
        Me.PropertiesTable.Controls.Add(Me.TypeLabel, 0, 2)
        Me.PropertiesTable.Controls.Add(Me.DelayNumber, 3, 1)
        Me.PropertiesTable.Controls.Add(Me.DelayLabel, 2, 1)
        Me.PropertiesTable.Controls.Add(Me.ProximityNumber, 3, 0)
        Me.PropertiesTable.Controls.Add(Me.ProximityLabel, 2, 0)
        Me.PropertiesTable.Controls.Add(Me.NameLabel, 0, 0)
        Me.PropertiesTable.Controls.Add(Me.ChanceLabel, 0, 1)
        Me.PropertiesTable.Controls.Add(Me.NameTextBox, 1, 0)
        Me.PropertiesTable.Controls.Add(Me.ChanceNumber, 1, 1)
        Me.PropertiesTable.Controls.Add(Me.TargetsList, 1, 3)
        Me.PropertiesTable.Controls.Add(Me.BehaviorsList, 3, 3)
        Me.PropertiesTable.Location = New System.Drawing.Point(0, 0)
        Me.PropertiesTable.Name = "PropertiesTable"
        Me.PropertiesTable.RowCount = 4
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.PropertiesTable.Size = New System.Drawing.Size(754, 224)
        Me.PropertiesTable.TabIndex = 1
        '
        'BehaviorsLabel
        '
        Me.BehaviorsLabel.AutoSize = True
        Me.BehaviorsLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.BehaviorsLabel.Location = New System.Drawing.Point(350, 85)
        Me.BehaviorsLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.BehaviorsLabel.Name = "BehaviorsLabel"
        Me.BehaviorsLabel.Size = New System.Drawing.Size(136, 13)
        Me.BehaviorsLabel.TabIndex = 35
        Me.BehaviorsLabel.Text = "Behaviors:"
        Me.BehaviorsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TargetsLabel
        '
        Me.TargetsLabel.AutoSize = True
        Me.TargetsLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TargetsLabel.Location = New System.Drawing.Point(3, 85)
        Me.TargetsLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.TargetsLabel.Name = "TargetsLabel"
        Me.TargetsLabel.Size = New System.Drawing.Size(76, 13)
        Me.TargetsLabel.TabIndex = 34
        Me.TargetsLabel.Text = "Targets:"
        Me.TargetsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'TypeComboBox
        '
        Me.TypeComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.TypeComboBox.FormattingEnabled = True
        Me.TypeComboBox.Location = New System.Drawing.Point(85, 55)
        Me.TypeComboBox.Name = "TypeComboBox"
        Me.TypeComboBox.Size = New System.Drawing.Size(259, 21)
        Me.TypeComboBox.TabIndex = 33
        '
        'TypeLabel
        '
        Me.TypeLabel.AutoSize = True
        Me.TypeLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TypeLabel.Location = New System.Drawing.Point(3, 58)
        Me.TypeLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.TypeLabel.Name = "TypeLabel"
        Me.TypeLabel.Size = New System.Drawing.Size(76, 13)
        Me.TypeLabel.TabIndex = 32
        Me.TypeLabel.Text = "Interacts With:"
        Me.TypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'DelayNumber
        '
        Me.DelayNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DelayNumber.DecimalPlaces = 2
        Me.DelayNumber.Location = New System.Drawing.Point(492, 29)
        Me.DelayNumber.Maximum = New Decimal(New Integer() {3600, 0, 0, 0})
        Me.DelayNumber.Name = "DelayNumber"
        Me.DelayNumber.Size = New System.Drawing.Size(259, 20)
        Me.DelayNumber.TabIndex = 31
        '
        'DelayLabel
        '
        Me.DelayLabel.AutoSize = True
        Me.DelayLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.DelayLabel.Location = New System.Drawing.Point(350, 32)
        Me.DelayLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.DelayLabel.Name = "DelayLabel"
        Me.DelayLabel.Size = New System.Drawing.Size(136, 13)
        Me.DelayLabel.TabIndex = 30
        Me.DelayLabel.Text = "Reactivation Delay (sec):"
        Me.DelayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ProximityNumber
        '
        Me.ProximityNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ProximityNumber.Location = New System.Drawing.Point(492, 3)
        Me.ProximityNumber.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.ProximityNumber.Name = "ProximityNumber"
        Me.ProximityNumber.Size = New System.Drawing.Size(259, 20)
        Me.ProximityNumber.TabIndex = 29
        '
        'ProximityLabel
        '
        Me.ProximityLabel.AutoSize = True
        Me.ProximityLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.ProximityLabel.Location = New System.Drawing.Point(350, 6)
        Me.ProximityLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.ProximityLabel.Name = "ProximityLabel"
        Me.ProximityLabel.Size = New System.Drawing.Size(136, 13)
        Me.ProximityLabel.TabIndex = 28
        Me.ProximityLabel.Text = "Activation Proximity (pixels):"
        Me.ProximityLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.NameLabel.Location = New System.Drawing.Point(3, 6)
        Me.NameLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(76, 13)
        Me.NameLabel.TabIndex = 0
        Me.NameLabel.Text = "Name:"
        Me.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'ChanceLabel
        '
        Me.ChanceLabel.AutoSize = True
        Me.ChanceLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.ChanceLabel.Location = New System.Drawing.Point(3, 32)
        Me.ChanceLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.ChanceLabel.Name = "ChanceLabel"
        Me.ChanceLabel.Size = New System.Drawing.Size(76, 13)
        Me.ChanceLabel.TabIndex = 4
        Me.ChanceLabel.Text = "Chance:"
        Me.ChanceLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'NameTextBox
        '
        Me.NameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NameTextBox.Location = New System.Drawing.Point(85, 3)
        Me.NameTextBox.MaxLength = 50
        Me.NameTextBox.Name = "NameTextBox"
        Me.NameTextBox.Size = New System.Drawing.Size(259, 20)
        Me.NameTextBox.TabIndex = 1
        '
        'ChanceNumber
        '
        Me.ChanceNumber.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ChanceNumber.DecimalPlaces = 2
        Me.ChanceNumber.Location = New System.Drawing.Point(85, 29)
        Me.ChanceNumber.Name = "ChanceNumber"
        Me.ChanceNumber.Size = New System.Drawing.Size(259, 20)
        Me.ChanceNumber.TabIndex = 5
        '
        'TargetsList
        '
        Me.TargetsList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TargetsList.CheckOnClick = True
        Me.TargetsList.FormattingEnabled = True
        Me.TargetsList.Location = New System.Drawing.Point(85, 82)
        Me.TargetsList.Name = "TargetsList"
        Me.TargetsList.Size = New System.Drawing.Size(259, 139)
        Me.TargetsList.TabIndex = 36
        '
        'BehaviorsList
        '
        Me.BehaviorsList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BehaviorsList.CheckOnClick = True
        Me.BehaviorsList.FormattingEnabled = True
        Me.BehaviorsList.Location = New System.Drawing.Point(492, 82)
        Me.BehaviorsList.Name = "BehaviorsList"
        Me.BehaviorsList.Size = New System.Drawing.Size(259, 139)
        Me.BehaviorsList.TabIndex = 37
        '
        'InteractionEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Name = "InteractionEditor"
        Me.PropertiesPanel.ResumeLayout(False)
        Me.PropertiesPanel.PerformLayout()
        Me.PropertiesTable.ResumeLayout(False)
        Me.PropertiesTable.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PropertiesTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents ChanceLabel As System.Windows.Forms.Label
    Friend WithEvents NameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents ChanceNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents ProximityLabel As System.Windows.Forms.Label
    Friend WithEvents ProximityNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents DelayLabel As System.Windows.Forms.Label
    Friend WithEvents DelayNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents TypeLabel As System.Windows.Forms.Label
    Friend WithEvents TypeComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents BehaviorsLabel As System.Windows.Forms.Label
    Friend WithEvents TargetsLabel As System.Windows.Forms.Label
    Friend WithEvents TargetsList As System.Windows.Forms.CheckedListBox
    Friend WithEvents BehaviorsList As System.Windows.Forms.CheckedListBox

End Class

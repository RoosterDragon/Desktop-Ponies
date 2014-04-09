<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class NewInteractionDialog
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
        Me.NameTextBox = New System.Windows.Forms.TextBox()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.ChanceLabel = New System.Windows.Forms.Label()
        Me.ProximityLabel = New System.Windows.Forms.Label()
        Me.TargetsList = New System.Windows.Forms.CheckedListBox()
        Me.TargetsLabel = New System.Windows.Forms.Label()
        Me.BehaviorsLabel = New System.Windows.Forms.Label()
        Me.BehaviorsList = New System.Windows.Forms.CheckedListBox()
        Me.ReactivationDelayLabel = New System.Windows.Forms.Label()
        Me.InteractsWithGroupBox = New System.Windows.Forms.GroupBox()
        Me.AllRadioButton = New System.Windows.Forms.RadioButton()
        Me.AnyRadioButton = New System.Windows.Forms.RadioButton()
        Me.OneRadioButton = New System.Windows.Forms.RadioButton()
        Me.LayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.DelayNumber = New System.Windows.Forms.NumericUpDown()
        Me.ProximityNumber = New System.Windows.Forms.NumericUpDown()
        Me.ChanceNumber = New System.Windows.Forms.NumericUpDown()
        Me.CommandTable.SuspendLayout()
        Me.InteractsWithGroupBox.SuspendLayout()
        Me.LayoutPanel.SuspendLayout()
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
        Me.CommandTable.Location = New System.Drawing.Point(352, 321)
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
        'NameTextBox
        '
        Me.NameTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.NameTextBox.Location = New System.Drawing.Point(38, 16)
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
        Me.NameLabel.Size = New System.Drawing.Size(91, 13)
        Me.NameLabel.TabIndex = 0
        Me.NameLabel.Text = "Interaction Name:"
        '
        'ChanceLabel
        '
        Me.ChanceLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ChanceLabel.AutoSize = True
        Me.ChanceLabel.Location = New System.Drawing.Point(3, 51)
        Me.ChanceLabel.Name = "ChanceLabel"
        Me.ChanceLabel.Size = New System.Drawing.Size(106, 13)
        Me.ChanceLabel.TabIndex = 2
        Me.ChanceLabel.Text = "Chance to occur (%):"
        '
        'ProximityLabel
        '
        Me.ProximityLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ProximityLabel.AutoSize = True
        Me.ProximityLabel.Location = New System.Drawing.Point(3, 102)
        Me.ProximityLabel.Name = "ProximityLabel"
        Me.ProximityLabel.Size = New System.Drawing.Size(189, 13)
        Me.ProximityLabel.TabIndex = 4
        Me.ProximityLabel.Text = "Activation Proximity (in pixels to target):"
        '
        'TargetsList
        '
        Me.TargetsList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TargetsList.FormattingEnabled = True
        Me.TargetsList.Location = New System.Drawing.Point(199, 16)
        Me.TargetsList.Name = "TargetsList"
        Me.LayoutPanel.SetRowSpan(Me.TargetsList, 8)
        Me.TargetsList.Size = New System.Drawing.Size(225, 284)
        Me.TargetsList.TabIndex = 10
        '
        'TargetsLabel
        '
        Me.TargetsLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.TargetsLabel.AutoSize = True
        Me.TargetsLabel.Location = New System.Drawing.Point(199, 0)
        Me.TargetsLabel.Name = "TargetsLabel"
        Me.TargetsLabel.Size = New System.Drawing.Size(114, 13)
        Me.TargetsLabel.TabIndex = 9
        Me.TargetsLabel.Text = "Ponies to interact with:"
        '
        'BehaviorsLabel
        '
        Me.BehaviorsLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.BehaviorsLabel.AutoSize = True
        Me.BehaviorsLabel.Location = New System.Drawing.Point(430, 0)
        Me.BehaviorsLabel.Name = "BehaviorsLabel"
        Me.BehaviorsLabel.Size = New System.Drawing.Size(221, 13)
        Me.BehaviorsLabel.TabIndex = 11
        Me.BehaviorsLabel.Text = "Behaviors to trigger (one selected at random):"
        '
        'BehaviorsList
        '
        Me.BehaviorsList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BehaviorsList.FormattingEnabled = True
        Me.BehaviorsList.Location = New System.Drawing.Point(430, 16)
        Me.BehaviorsList.Name = "BehaviorsList"
        Me.LayoutPanel.SetRowSpan(Me.BehaviorsList, 8)
        Me.BehaviorsList.Size = New System.Drawing.Size(227, 284)
        Me.BehaviorsList.TabIndex = 12
        '
        'ReactivationDelayLabel
        '
        Me.ReactivationDelayLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ReactivationDelayLabel.AutoSize = True
        Me.ReactivationDelayLabel.Location = New System.Drawing.Point(3, 153)
        Me.ReactivationDelayLabel.Name = "ReactivationDelayLabel"
        Me.ReactivationDelayLabel.Size = New System.Drawing.Size(149, 13)
        Me.ReactivationDelayLabel.TabIndex = 6
        Me.ReactivationDelayLabel.Text = "Reactivation Delay (seconds):"
        '
        'InteractsWithGroupBox
        '
        Me.InteractsWithGroupBox.Controls.Add(Me.AllRadioButton)
        Me.InteractsWithGroupBox.Controls.Add(Me.AnyRadioButton)
        Me.InteractsWithGroupBox.Controls.Add(Me.OneRadioButton)
        Me.InteractsWithGroupBox.Location = New System.Drawing.Point(3, 207)
        Me.InteractsWithGroupBox.Name = "InteractsWithGroupBox"
        Me.InteractsWithGroupBox.Size = New System.Drawing.Size(190, 90)
        Me.InteractsWithGroupBox.TabIndex = 8
        Me.InteractsWithGroupBox.TabStop = False
        Me.InteractsWithGroupBox.Text = "Interacts With"
        '
        'AllRadioButton
        '
        Me.AllRadioButton.AutoSize = True
        Me.AllRadioButton.Location = New System.Drawing.Point(6, 65)
        Me.AllRadioButton.Name = "AllRadioButton"
        Me.AllRadioButton.Size = New System.Drawing.Size(179, 17)
        Me.AllRadioButton.TabIndex = 2
        Me.AllRadioButton.Text = "All - All targets must be available."
        Me.AllRadioButton.UseVisualStyleBackColor = True
        '
        'AnyRadioButton
        '
        Me.AnyRadioButton.AutoSize = True
        Me.AnyRadioButton.Location = New System.Drawing.Point(6, 42)
        Me.AnyRadioButton.Name = "AnyRadioButton"
        Me.AnyRadioButton.Size = New System.Drawing.Size(152, 17)
        Me.AnyRadioButton.TabIndex = 1
        Me.AnyRadioButton.Text = "Any - Any available ponies."
        Me.AnyRadioButton.UseVisualStyleBackColor = True
        '
        'OneRadioButton
        '
        Me.OneRadioButton.AutoSize = True
        Me.OneRadioButton.Checked = True
        Me.OneRadioButton.Location = New System.Drawing.Point(6, 19)
        Me.OneRadioButton.Name = "OneRadioButton"
        Me.OneRadioButton.Size = New System.Drawing.Size(160, 17)
        Me.OneRadioButton.TabIndex = 0
        Me.OneRadioButton.TabStop = True
        Me.OneRadioButton.Text = "One - Only the nearest pony."
        Me.OneRadioButton.UseVisualStyleBackColor = True
        '
        'LayoutPanel
        '
        Me.LayoutPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LayoutPanel.ColumnCount = 3
        Me.LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 49.99999!))
        Me.LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.LayoutPanel.Controls.Add(Me.DelayNumber, 0, 7)
        Me.LayoutPanel.Controls.Add(Me.ProximityNumber, 0, 5)
        Me.LayoutPanel.Controls.Add(Me.ChanceNumber, 0, 3)
        Me.LayoutPanel.Controls.Add(Me.NameLabel, 0, 0)
        Me.LayoutPanel.Controls.Add(Me.NameTextBox, 0, 1)
        Me.LayoutPanel.Controls.Add(Me.ReactivationDelayLabel, 0, 6)
        Me.LayoutPanel.Controls.Add(Me.ChanceLabel, 0, 2)
        Me.LayoutPanel.Controls.Add(Me.ProximityLabel, 0, 4)
        Me.LayoutPanel.Controls.Add(Me.InteractsWithGroupBox, 0, 8)
        Me.LayoutPanel.Controls.Add(Me.BehaviorsList, 2, 1)
        Me.LayoutPanel.Controls.Add(Me.BehaviorsLabel, 2, 0)
        Me.LayoutPanel.Controls.Add(Me.TargetsLabel, 1, 0)
        Me.LayoutPanel.Controls.Add(Me.TargetsList, 1, 1)
        Me.LayoutPanel.Location = New System.Drawing.Point(12, 12)
        Me.LayoutPanel.Name = "LayoutPanel"
        Me.LayoutPanel.RowCount = 9
        Me.LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.LayoutPanel.Size = New System.Drawing.Size(660, 303)
        Me.LayoutPanel.TabIndex = 0
        '
        'DelayNumber
        '
        Me.DelayNumber.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.DelayNumber.DecimalPlaces = 2
        Me.DelayNumber.Location = New System.Drawing.Point(38, 169)
        Me.DelayNumber.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.DelayNumber.Maximum = New Decimal(New Integer() {3600, 0, 0, 0})
        Me.DelayNumber.Name = "DelayNumber"
        Me.DelayNumber.Size = New System.Drawing.Size(120, 20)
        Me.DelayNumber.TabIndex = 7
        Me.DelayNumber.Value = New Decimal(New Integer() {60, 0, 0, 0})
        '
        'ProximityNumber
        '
        Me.ProximityNumber.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ProximityNumber.Location = New System.Drawing.Point(38, 118)
        Me.ProximityNumber.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.ProximityNumber.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.ProximityNumber.Name = "ProximityNumber"
        Me.ProximityNumber.Size = New System.Drawing.Size(120, 20)
        Me.ProximityNumber.TabIndex = 5
        Me.ProximityNumber.Value = New Decimal(New Integer() {300, 0, 0, 0})
        '
        'ChanceNumber
        '
        Me.ChanceNumber.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.ChanceNumber.DecimalPlaces = 2
        Me.ChanceNumber.Location = New System.Drawing.Point(38, 67)
        Me.ChanceNumber.Margin = New System.Windows.Forms.Padding(3, 3, 3, 15)
        Me.ChanceNumber.Name = "ChanceNumber"
        Me.ChanceNumber.Size = New System.Drawing.Size(120, 20)
        Me.ChanceNumber.TabIndex = 3
        Me.ChanceNumber.Value = New Decimal(New Integer() {25, 0, 0, 0})
        '
        'NewInteractionDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(684, 362)
        Me.Controls.Add(Me.LayoutPanel)
        Me.Controls.Add(Me.CommandTable)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(700, 400)
        Me.Name = "NewInteractionDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create New Interaction..."
        Me.CommandTable.ResumeLayout(False)
        Me.InteractsWithGroupBox.ResumeLayout(False)
        Me.InteractsWithGroupBox.PerformLayout()
        Me.LayoutPanel.ResumeLayout(False)
        Me.LayoutPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CommandTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents NameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents ChanceLabel As System.Windows.Forms.Label
    Friend WithEvents ProximityLabel As System.Windows.Forms.Label
    Friend WithEvents TargetsList As System.Windows.Forms.CheckedListBox
    Friend WithEvents TargetsLabel As System.Windows.Forms.Label
    Friend WithEvents BehaviorsLabel As System.Windows.Forms.Label
    Friend WithEvents BehaviorsList As System.Windows.Forms.CheckedListBox
    Friend WithEvents ReactivationDelayLabel As System.Windows.Forms.Label
    Friend WithEvents InteractsWithGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents AllRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents AnyRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents OneRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents LayoutPanel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ChanceNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents ProximityNumber As System.Windows.Forms.NumericUpDown
    Friend WithEvents DelayNumber As System.Windows.Forms.NumericUpDown

End Class

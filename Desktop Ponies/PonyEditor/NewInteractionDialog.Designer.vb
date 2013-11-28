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
        Me.CommandsTable = New System.Windows.Forms.TableLayoutPanel()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.NameTextbox = New System.Windows.Forms.TextBox()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.ChanceTextbox = New System.Windows.Forms.TextBox()
        Me.ProximityTextbox = New System.Windows.Forms.TextBox()
        Me.ChanceLabel = New System.Windows.Forms.Label()
        Me.ProximityLabel = New System.Windows.Forms.Label()
        Me.TargetsList = New System.Windows.Forms.CheckedListBox()
        Me.TargetsLabel = New System.Windows.Forms.Label()
        Me.BehaviorsLabel = New System.Windows.Forms.Label()
        Me.BehaviorsList = New System.Windows.Forms.CheckedListBox()
        Me.ReactivationDelayLabel = New System.Windows.Forms.Label()
        Me.ReactivationDelayTextbox = New System.Windows.Forms.TextBox()
        Me.InteractsWithGroupBox = New System.Windows.Forms.GroupBox()
        Me.AllRadioButton = New System.Windows.Forms.RadioButton()
        Me.AnyRadioButton = New System.Windows.Forms.RadioButton()
        Me.OneRadioButton = New System.Windows.Forms.RadioButton()
        Me.CommandsTable.SuspendLayout()
        Me.InteractsWithGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'CommandsTable
        '
        Me.CommandsTable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CommandsTable.ColumnCount = 2
        Me.CommandsTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.CommandsTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.CommandsTable.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.CommandsTable.Controls.Add(Me.OK_Button, 0, 0)
        Me.CommandsTable.Location = New System.Drawing.Point(323, 471)
        Me.CommandsTable.Name = "CommandsTable"
        Me.CommandsTable.RowCount = 1
        Me.CommandsTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.CommandsTable.Size = New System.Drawing.Size(320, 29)
        Me.CommandsTable.TabIndex = 13
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
        'NameTextbox
        '
        Me.NameTextbox.Location = New System.Drawing.Point(92, 73)
        Me.NameTextbox.Name = "NameTextbox"
        Me.NameTextbox.Size = New System.Drawing.Size(100, 20)
        Me.NameTextbox.TabIndex = 1
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Location = New System.Drawing.Point(60, 57)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(91, 13)
        Me.NameLabel.TabIndex = 0
        Me.NameLabel.Text = "Interaction Name:"
        '
        'ChanceTextbox
        '
        Me.ChanceTextbox.Location = New System.Drawing.Point(418, 48)
        Me.ChanceTextbox.Name = "ChanceTextbox"
        Me.ChanceTextbox.Size = New System.Drawing.Size(100, 20)
        Me.ChanceTextbox.TabIndex = 3
        '
        'ProximityTextbox
        '
        Me.ProximityTextbox.Location = New System.Drawing.Point(418, 108)
        Me.ProximityTextbox.Name = "ProximityTextbox"
        Me.ProximityTextbox.Size = New System.Drawing.Size(100, 20)
        Me.ProximityTextbox.TabIndex = 5
        Me.ProximityTextbox.Text = "300"
        '
        'ChanceLabel
        '
        Me.ChanceLabel.AutoSize = True
        Me.ChanceLabel.Location = New System.Drawing.Point(388, 32)
        Me.ChanceLabel.Name = "ChanceLabel"
        Me.ChanceLabel.Size = New System.Drawing.Size(106, 13)
        Me.ChanceLabel.TabIndex = 2
        Me.ChanceLabel.Text = "Chance to occur (%):"
        '
        'ProximityLabel
        '
        Me.ProximityLabel.AutoSize = True
        Me.ProximityLabel.Location = New System.Drawing.Point(378, 92)
        Me.ProximityLabel.Name = "ProximityLabel"
        Me.ProximityLabel.Size = New System.Drawing.Size(189, 13)
        Me.ProximityLabel.TabIndex = 4
        Me.ProximityLabel.Text = "Activation Proximity (in pixels to target):"
        '
        'TargetsList
        '
        Me.TargetsList.FormattingEnabled = True
        Me.TargetsList.Location = New System.Drawing.Point(54, 194)
        Me.TargetsList.Name = "TargetsList"
        Me.TargetsList.Size = New System.Drawing.Size(207, 94)
        Me.TargetsList.TabIndex = 9
        '
        'TargetsLabel
        '
        Me.TargetsLabel.AutoSize = True
        Me.TargetsLabel.Location = New System.Drawing.Point(37, 178)
        Me.TargetsLabel.Name = "TargetsLabel"
        Me.TargetsLabel.Size = New System.Drawing.Size(114, 13)
        Me.TargetsLabel.TabIndex = 8
        Me.TargetsLabel.Text = "Ponies to interact with:"
        '
        'BehaviorsLabel
        '
        Me.BehaviorsLabel.AutoSize = True
        Me.BehaviorsLabel.Location = New System.Drawing.Point(147, 315)
        Me.BehaviorsLabel.Name = "BehaviorsLabel"
        Me.BehaviorsLabel.Size = New System.Drawing.Size(221, 13)
        Me.BehaviorsLabel.TabIndex = 11
        Me.BehaviorsLabel.Text = "Behaviors to trigger (one selected at random):"
        '
        'BehaviorsList
        '
        Me.BehaviorsList.FormattingEnabled = True
        Me.BehaviorsList.Location = New System.Drawing.Point(208, 331)
        Me.BehaviorsList.Name = "BehaviorsList"
        Me.BehaviorsList.Size = New System.Drawing.Size(228, 109)
        Me.BehaviorsList.TabIndex = 12
        '
        'ReactivationDelayLabel
        '
        Me.ReactivationDelayLabel.AutoSize = True
        Me.ReactivationDelayLabel.Location = New System.Drawing.Point(378, 155)
        Me.ReactivationDelayLabel.Name = "ReactivationDelayLabel"
        Me.ReactivationDelayLabel.Size = New System.Drawing.Size(149, 13)
        Me.ReactivationDelayLabel.TabIndex = 6
        Me.ReactivationDelayLabel.Text = "Reactivation Delay (seconds):"
        '
        'ReactivationDelayTextbox
        '
        Me.ReactivationDelayTextbox.Location = New System.Drawing.Point(418, 171)
        Me.ReactivationDelayTextbox.Name = "ReactivationDelayTextbox"
        Me.ReactivationDelayTextbox.Size = New System.Drawing.Size(100, 20)
        Me.ReactivationDelayTextbox.TabIndex = 7
        Me.ReactivationDelayTextbox.Text = "60"
        '
        'InteractsWithGroupBox
        '
        Me.InteractsWithGroupBox.Controls.Add(Me.AllRadioButton)
        Me.InteractsWithGroupBox.Controls.Add(Me.AnyRadioButton)
        Me.InteractsWithGroupBox.Controls.Add(Me.OneRadioButton)
        Me.InteractsWithGroupBox.Location = New System.Drawing.Point(381, 198)
        Me.InteractsWithGroupBox.Name = "InteractsWithGroupBox"
        Me.InteractsWithGroupBox.Size = New System.Drawing.Size(190, 90)
        Me.InteractsWithGroupBox.TabIndex = 10
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
        'NewInteractionDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(655, 512)
        Me.Controls.Add(Me.InteractsWithGroupBox)
        Me.Controls.Add(Me.ReactivationDelayLabel)
        Me.Controls.Add(Me.ReactivationDelayTextbox)
        Me.Controls.Add(Me.BehaviorsLabel)
        Me.Controls.Add(Me.BehaviorsList)
        Me.Controls.Add(Me.TargetsLabel)
        Me.Controls.Add(Me.TargetsList)
        Me.Controls.Add(Me.ProximityLabel)
        Me.Controls.Add(Me.ChanceLabel)
        Me.Controls.Add(Me.ProximityTextbox)
        Me.Controls.Add(Me.ChanceTextbox)
        Me.Controls.Add(Me.NameLabel)
        Me.Controls.Add(Me.NameTextbox)
        Me.Controls.Add(Me.CommandsTable)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewInteractionDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create New Interaction..."
        Me.CommandsTable.ResumeLayout(False)
        Me.InteractsWithGroupBox.ResumeLayout(False)
        Me.InteractsWithGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CommandsTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents NameTextbox As System.Windows.Forms.TextBox
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents ChanceTextbox As System.Windows.Forms.TextBox
    Friend WithEvents ProximityTextbox As System.Windows.Forms.TextBox
    Friend WithEvents ChanceLabel As System.Windows.Forms.Label
    Friend WithEvents ProximityLabel As System.Windows.Forms.Label
    Friend WithEvents TargetsList As System.Windows.Forms.CheckedListBox
    Friend WithEvents TargetsLabel As System.Windows.Forms.Label
    Friend WithEvents BehaviorsLabel As System.Windows.Forms.Label
    Friend WithEvents BehaviorsList As System.Windows.Forms.CheckedListBox
    Friend WithEvents ReactivationDelayLabel As System.Windows.Forms.Label
    Friend WithEvents ReactivationDelayTextbox As System.Windows.Forms.TextBox
    Friend WithEvents InteractsWithGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents AllRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents AnyRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents OneRadioButton As System.Windows.Forms.RadioButton

End Class

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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Name_Textbox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Chance_Box = New System.Windows.Forms.TextBox()
        Me.Proximity_Box = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Targets_Box = New System.Windows.Forms.CheckedListBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Behaviors_Box = New System.Windows.Forms.CheckedListBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Reactivation_Delay_Textbox = New System.Windows.Forms.TextBox()
        Me.InteractsWithGroupBox = New System.Windows.Forms.GroupBox()
        Me.AllRadioButton = New System.Windows.Forms.RadioButton()
        Me.AnyRadioButton = New System.Windows.Forms.RadioButton()
        Me.OneRadioButton = New System.Windows.Forms.RadioButton()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.InteractsWithGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.OK_Button, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(323, 471)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(320, 29)
        Me.TableLayoutPanel1.TabIndex = 13
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
        'Name_Textbox
        '
        Me.Name_Textbox.Location = New System.Drawing.Point(92, 73)
        Me.Name_Textbox.Name = "Name_Textbox"
        Me.Name_Textbox.Size = New System.Drawing.Size(100, 20)
        Me.Name_Textbox.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(60, 57)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(91, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Interaction Name:"
        '
        'Chance_Box
        '
        Me.Chance_Box.Location = New System.Drawing.Point(418, 48)
        Me.Chance_Box.Name = "Chance_Box"
        Me.Chance_Box.Size = New System.Drawing.Size(100, 20)
        Me.Chance_Box.TabIndex = 3
        '
        'Proximity_Box
        '
        Me.Proximity_Box.Location = New System.Drawing.Point(418, 108)
        Me.Proximity_Box.Name = "Proximity_Box"
        Me.Proximity_Box.Size = New System.Drawing.Size(100, 20)
        Me.Proximity_Box.TabIndex = 5
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(388, 32)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(106, 13)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Chance to occur (%):"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(378, 92)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(189, 13)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Activation Proximity (in pixels to target):"
        '
        'Targets_Box
        '
        Me.Targets_Box.FormattingEnabled = True
        Me.Targets_Box.Location = New System.Drawing.Point(54, 194)
        Me.Targets_Box.Name = "Targets_Box"
        Me.Targets_Box.Size = New System.Drawing.Size(207, 94)
        Me.Targets_Box.TabIndex = 9
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(37, 178)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(114, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Ponies to interact with:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(147, 315)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(221, 13)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Behaviors to trigger (one selected at random):"
        '
        'Behaviors_Box
        '
        Me.Behaviors_Box.FormattingEnabled = True
        Me.Behaviors_Box.Location = New System.Drawing.Point(208, 331)
        Me.Behaviors_Box.Name = "Behaviors_Box"
        Me.Behaviors_Box.Size = New System.Drawing.Size(228, 109)
        Me.Behaviors_Box.TabIndex = 12
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(378, 155)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(149, 13)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Reactivation Delay (seconds):"
        '
        'Reactivation_Delay_Textbox
        '
        Me.Reactivation_Delay_Textbox.Location = New System.Drawing.Point(418, 171)
        Me.Reactivation_Delay_Textbox.Name = "Reactivation_Delay_Textbox"
        Me.Reactivation_Delay_Textbox.Size = New System.Drawing.Size(100, 20)
        Me.Reactivation_Delay_Textbox.TabIndex = 7
        Me.Reactivation_Delay_Textbox.Text = "60"
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
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Reactivation_Delay_Textbox)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Behaviors_Box)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Targets_Box)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Proximity_Box)
        Me.Controls.Add(Me.Chance_Box)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Name_Textbox)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "NewInteractionDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Create New Interaction..."
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.InteractsWithGroupBox.ResumeLayout(False)
        Me.InteractsWithGroupBox.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Name_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Chance_Box As System.Windows.Forms.TextBox
    Friend WithEvents Proximity_Box As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Targets_Box As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Behaviors_Box As System.Windows.Forms.CheckedListBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Reactivation_Delay_Textbox As System.Windows.Forms.TextBox
    Friend WithEvents InteractsWithGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents AllRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents AnyRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents OneRadioButton As System.Windows.Forms.RadioButton

End Class

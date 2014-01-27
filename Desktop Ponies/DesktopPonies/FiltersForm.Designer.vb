<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FiltersForm
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
        Me.FiltersTextBox = New System.Windows.Forms.TextBox()
        Me.InfoLabel = New System.Windows.Forms.Label()
        Me.Save_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.SaveLabel = New System.Windows.Forms.Label()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'FiltersTextBox
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.FiltersTextBox, 2)
        Me.FiltersTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FiltersTextBox.Location = New System.Drawing.Point(3, 22)
        Me.FiltersTextBox.Multiline = True
        Me.FiltersTextBox.Name = "FiltersTextBox"
        Me.FiltersTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.FiltersTextBox.Size = New System.Drawing.Size(378, 76)
        Me.FiltersTextBox.TabIndex = 1
        '
        'InfoLabel
        '
        Me.InfoLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.InfoLabel.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.InfoLabel, 2)
        Me.InfoLabel.Location = New System.Drawing.Point(35, 3)
        Me.InfoLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.InfoLabel.Name = "InfoLabel"
        Me.InfoLabel.Size = New System.Drawing.Size(314, 13)
        Me.InfoLabel.TabIndex = 0
        Me.InfoLabel.Text = "Edit the possible pony categories/tags/filters below (one per line)."
        Me.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Save_Button
        '
        Me.Save_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Save_Button.Location = New System.Drawing.Point(58, 136)
        Me.Save_Button.Name = "Save_Button"
        Me.Save_Button.Size = New System.Drawing.Size(75, 23)
        Me.Save_Button.TabIndex = 3
        Me.Save_Button.Text = "Apply"
        Me.Save_Button.UseVisualStyleBackColor = True
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(250, 136)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(75, 23)
        Me.Cancel_Button.TabIndex = 4
        Me.Cancel_Button.Text = "Cancel"
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'SaveLabel
        '
        Me.SaveLabel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.SaveLabel.AutoSize = True
        Me.TableLayoutPanel1.SetColumnSpan(Me.SaveLabel, 2)
        Me.SaveLabel.Location = New System.Drawing.Point(5, 104)
        Me.SaveLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.SaveLabel.Name = "SaveLabel"
        Me.SaveLabel.Size = New System.Drawing.Size(373, 26)
        Me.SaveLabel.TabIndex = 2
        Me.SaveLabel.Text = "Note that you still need to save your settings on the options menu to make the ch" & _
    "ange permanent!"
        Me.SaveLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.InfoLabel, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Cancel_Button, 1, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.SaveLabel, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.Save_Button, 0, 3)
        Me.TableLayoutPanel1.Controls.Add(Me.FiltersTextBox, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(384, 162)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'FiltersForm
        '
        Me.AcceptButton = Me.Save_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(384, 162)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.MinimumSize = New System.Drawing.Size(300, 175)
        Me.Name = "FiltersForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Edit Filters/Tags..."
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents FiltersTextBox As System.Windows.Forms.TextBox
    Friend WithEvents InfoLabel As System.Windows.Forms.Label
    Friend WithEvents Save_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents SaveLabel As System.Windows.Forms.Label
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
End Class

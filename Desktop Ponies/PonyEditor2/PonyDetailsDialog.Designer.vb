<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PonyDetailsDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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
        Me.DialogTable = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.PropertiesTable = New System.Windows.Forms.TableLayoutPanel()
        Me.TagsLabel = New System.Windows.Forms.Label()
        Me.DisplayNameLabel = New System.Windows.Forms.Label()
        Me.NameLabel = New System.Windows.Forms.Label()
        Me.NameTextBox = New System.Windows.Forms.TextBox()
        Me.DisplayNameTextBox = New System.Windows.Forms.TextBox()
        Me.TagsList = New System.Windows.Forms.CheckedListBox()
        Me.DialogTable.SuspendLayout()
        Me.PropertiesTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'DialogTable
        '
        Me.DialogTable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DialogTable.ColumnCount = 2
        Me.DialogTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.DialogTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.DialogTable.Controls.Add(Me.OK_Button, 0, 0)
        Me.DialogTable.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.DialogTable.Location = New System.Drawing.Point(226, 246)
        Me.DialogTable.Name = "DialogTable"
        Me.DialogTable.RowCount = 1
        Me.DialogTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.DialogTable.Size = New System.Drawing.Size(146, 29)
        Me.DialogTable.TabIndex = 1
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "Cancel"
        '
        'PropertiesTable
        '
        Me.PropertiesTable.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PropertiesTable.AutoSize = True
        Me.PropertiesTable.ColumnCount = 2
        Me.PropertiesTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.PropertiesTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.PropertiesTable.Controls.Add(Me.TagsLabel, 0, 2)
        Me.PropertiesTable.Controls.Add(Me.DisplayNameLabel, 0, 1)
        Me.PropertiesTable.Controls.Add(Me.NameLabel, 0, 0)
        Me.PropertiesTable.Controls.Add(Me.NameTextBox, 1, 0)
        Me.PropertiesTable.Controls.Add(Me.DisplayNameTextBox, 1, 1)
        Me.PropertiesTable.Controls.Add(Me.TagsList, 1, 2)
        Me.PropertiesTable.Location = New System.Drawing.Point(12, 12)
        Me.PropertiesTable.Name = "PropertiesTable"
        Me.PropertiesTable.RowCount = 3
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.PropertiesTable.Size = New System.Drawing.Size(360, 228)
        Me.PropertiesTable.TabIndex = 0
        '
        'TagsLabel
        '
        Me.TagsLabel.AutoSize = True
        Me.TagsLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.TagsLabel.Location = New System.Drawing.Point(3, 58)
        Me.TagsLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.TagsLabel.Name = "TagsLabel"
        Me.TagsLabel.Size = New System.Drawing.Size(75, 13)
        Me.TagsLabel.TabIndex = 4
        Me.TagsLabel.Text = "Tags:"
        Me.TagsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'DisplayNameLabel
        '
        Me.DisplayNameLabel.AutoSize = True
        Me.DisplayNameLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.DisplayNameLabel.Location = New System.Drawing.Point(3, 32)
        Me.DisplayNameLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.DisplayNameLabel.Name = "DisplayNameLabel"
        Me.DisplayNameLabel.Size = New System.Drawing.Size(75, 13)
        Me.DisplayNameLabel.TabIndex = 2
        Me.DisplayNameLabel.Text = "Display Name:"
        Me.DisplayNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'NameLabel
        '
        Me.NameLabel.AutoSize = True
        Me.NameLabel.Dock = System.Windows.Forms.DockStyle.Top
        Me.NameLabel.Location = New System.Drawing.Point(3, 6)
        Me.NameLabel.Margin = New System.Windows.Forms.Padding(3, 6, 3, 6)
        Me.NameLabel.Name = "NameLabel"
        Me.NameLabel.Size = New System.Drawing.Size(75, 13)
        Me.NameLabel.TabIndex = 0
        Me.NameLabel.Text = "Name:"
        Me.NameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'NameTextBox
        '
        Me.NameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NameTextBox.Location = New System.Drawing.Point(84, 3)
        Me.NameTextBox.MaxLength = 50
        Me.NameTextBox.Name = "NameTextBox"
        Me.NameTextBox.Size = New System.Drawing.Size(273, 20)
        Me.NameTextBox.TabIndex = 1
        '
        'DisplayNameTextBox
        '
        Me.DisplayNameTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DisplayNameTextBox.Location = New System.Drawing.Point(84, 29)
        Me.DisplayNameTextBox.MaxLength = 50
        Me.DisplayNameTextBox.Name = "DisplayNameTextBox"
        Me.DisplayNameTextBox.Size = New System.Drawing.Size(273, 20)
        Me.DisplayNameTextBox.TabIndex = 3
        '
        'TagsList
        '
        Me.TagsList.CheckOnClick = True
        Me.TagsList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TagsList.FormattingEnabled = True
        Me.TagsList.Location = New System.Drawing.Point(84, 55)
        Me.TagsList.Name = "TagsList"
        Me.TagsList.Size = New System.Drawing.Size(273, 170)
        Me.TagsList.TabIndex = 5
        '
        'PonyDetailsDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(384, 287)
        Me.Controls.Add(Me.PropertiesTable)
        Me.Controls.Add(Me.DialogTable)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "PonyDetailsDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Pony Details - Desktop Ponies"
        Me.DialogTable.ResumeLayout(False)
        Me.PropertiesTable.ResumeLayout(False)
        Me.PropertiesTable.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DialogTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents PropertiesTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents NameLabel As System.Windows.Forms.Label
    Friend WithEvents NameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents TagsLabel As System.Windows.Forms.Label
    Friend WithEvents DisplayNameLabel As System.Windows.Forms.Label
    Friend WithEvents DisplayNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents TagsList As System.Windows.Forms.CheckedListBox

End Class

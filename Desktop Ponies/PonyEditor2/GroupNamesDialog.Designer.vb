<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GroupNamesDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GroupNamesDialog))
        Me.ButtonsTable = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.NamesGrid = New System.Windows.Forms.DataGridView()
        Me.InfoLabel = New System.Windows.Forms.Label()
        Me.colNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.ButtonsTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonsTable
        '
        Me.ButtonsTable.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonsTable.ColumnCount = 2
        Me.ButtonsTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.ButtonsTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.ButtonsTable.Controls.Add(Me.OK_Button, 0, 0)
        Me.ButtonsTable.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.ButtonsTable.Location = New System.Drawing.Point(176, 271)
        Me.ButtonsTable.Name = "ButtonsTable"
        Me.ButtonsTable.RowCount = 1
        Me.ButtonsTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.ButtonsTable.Size = New System.Drawing.Size(146, 29)
        Me.ButtonsTable.TabIndex = 2
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
        'NamesGrid
        '
        Me.NamesGrid.AllowUserToAddRows = False
        Me.NamesGrid.AllowUserToDeleteRows = False
        Me.NamesGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NamesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.NamesGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colNumber, Me.colName})
        Me.NamesGrid.Location = New System.Drawing.Point(12, 82)
        Me.NamesGrid.Name = "NamesGrid"
        Me.NamesGrid.Size = New System.Drawing.Size(310, 183)
        Me.NamesGrid.TabIndex = 1
        '
        'InfoLabel
        '
        Me.InfoLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InfoLabel.Location = New System.Drawing.Point(12, 9)
        Me.InfoLabel.Name = "InfoLabel"
        Me.InfoLabel.Size = New System.Drawing.Size(310, 70)
        Me.InfoLabel.TabIndex = 0
        Me.InfoLabel.Text = resources.GetString("InfoLabel.Text")
        Me.InfoLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'colNumber
        '
        Me.colNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader
        Me.colNumber.HeaderText = "Number"
        Me.colNumber.Name = "colNumber"
        Me.colNumber.ReadOnly = True
        Me.colNumber.Width = 69
        '
        'colName
        '
        Me.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colName.HeaderText = "Name"
        Me.colName.MaxInputLength = 100
        Me.colName.Name = "colName"
        '
        'GroupNamesDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(334, 312)
        Me.Controls.Add(Me.InfoLabel)
        Me.Controls.Add(Me.NamesGrid)
        Me.Controls.Add(Me.ButtonsTable)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(275, 250)
        Me.Name = "GroupNamesDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Group Names - Desktop Ponies"
        Me.ButtonsTable.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonsTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents NamesGrid As System.Windows.Forms.DataGridView
    Friend WithEvents InfoLabel As System.Windows.Forms.Label
    Friend WithEvents colNumber As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn

End Class

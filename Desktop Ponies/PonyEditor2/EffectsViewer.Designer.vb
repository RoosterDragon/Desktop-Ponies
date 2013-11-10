<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class EffectsViewer
    Inherits DesktopPonies.ItemsViewerBase

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
        Me.EffectsGrid = New System.Windows.Forms.DataGridView()
        Me.colPreview = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colEdit = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SuspendLayout()
        '
        'EffectsGrid
        '
        Me.EffectsGrid.AllowUserToAddRows = False
        Me.EffectsGrid.AllowUserToDeleteRows = False
        Me.EffectsGrid.AllowUserToOrderColumns = True
        Me.EffectsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.EffectsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPreview, Me.colEdit, Me.colName})
        Me.EffectsGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EffectsGrid.Location = New System.Drawing.Point(0, 0)
        Me.EffectsGrid.Name = "EffectsGrid"
        Me.EffectsGrid.ReadOnly = True
        Me.EffectsGrid.Size = New System.Drawing.Size(500, 300)
        Me.EffectsGrid.TabIndex = 0
        '
        'colPreview
        '
        Me.colPreview.Frozen = True
        Me.colPreview.HeaderText = "Preview"
        Me.colPreview.Name = "colPreview"
        Me.colPreview.ReadOnly = True
        Me.colPreview.Text = "Preview"
        Me.colPreview.UseColumnTextForButtonValue = True
        Me.colPreview.Width = 50
        '
        'colEdit
        '
        Me.colEdit.Frozen = True
        Me.colEdit.HeaderText = "Edit"
        Me.colEdit.Name = "colEdit"
        Me.colEdit.ReadOnly = True
        Me.colEdit.Text = "Edit"
        Me.colEdit.UseColumnTextForButtonValue = True
        Me.colEdit.Width = 50
        '
        'colName
        '
        Me.colName.Frozen = True
        Me.colName.HeaderText = "Name"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        Me.colName.Width = 175
        '
        'EffectsViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.EffectsGrid)
        Me.Name = "EffectsViewer"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents EffectsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents colPreview As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colEdit As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn

End Class

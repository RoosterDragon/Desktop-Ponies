<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class InteractionsViewer
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
        Me.InteractionsGrid = New System.Windows.Forms.DataGridView()
        Me.colEdit = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.InteractionsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'InteractionsGrid
        '
        Me.InteractionsGrid.AllowUserToAddRows = False
        Me.InteractionsGrid.AllowUserToDeleteRows = False
        Me.InteractionsGrid.AllowUserToOrderColumns = True
        Me.InteractionsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.InteractionsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colEdit, Me.colName})
        Me.InteractionsGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InteractionsGrid.Location = New System.Drawing.Point(0, 0)
        Me.InteractionsGrid.Name = "InteractionsGrid"
        Me.InteractionsGrid.ReadOnly = True
        Me.InteractionsGrid.Size = New System.Drawing.Size(500, 300)
        Me.InteractionsGrid.TabIndex = 0
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
        'InteractionsViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.InteractionsGrid)
        Me.Name = "InteractionsViewer"
        CType(Me.InteractionsGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents InteractionsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents colEdit As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn

End Class

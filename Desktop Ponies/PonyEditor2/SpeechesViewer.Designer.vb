<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SpeechesViewer
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
        Me.SpeechesGrid = New System.Windows.Forms.DataGridView()
        Me.colEdit = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        CType(Me.SpeechesGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'SpeechesGrid
        '
        Me.SpeechesGrid.AllowUserToAddRows = False
        Me.SpeechesGrid.AllowUserToDeleteRows = False
        Me.SpeechesGrid.AllowUserToOrderColumns = True
        Me.SpeechesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.SpeechesGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colEdit, Me.colName})
        Me.SpeechesGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SpeechesGrid.Location = New System.Drawing.Point(0, 0)
        Me.SpeechesGrid.Name = "SpeechesGrid"
        Me.SpeechesGrid.ReadOnly = True
        Me.SpeechesGrid.Size = New System.Drawing.Size(500, 300)
        Me.SpeechesGrid.TabIndex = 0
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
        'SpeechesViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.SpeechesGrid)
        Me.Name = "SpeechesViewer"
        CType(Me.SpeechesGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SpeechesGrid As System.Windows.Forms.DataGridView
    Friend WithEvents colEdit As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn

End Class

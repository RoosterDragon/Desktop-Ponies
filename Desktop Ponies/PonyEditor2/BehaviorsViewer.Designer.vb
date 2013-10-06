<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class BehaviorsViewer
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
        Me.BehaviorsGrid = New System.Windows.Forms.DataGridView()
        Me.colPreview = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colEdit = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colGroup = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colChance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupNamesButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'BehaviorsGrid
        '
        Me.BehaviorsGrid.AllowUserToAddRows = False
        Me.BehaviorsGrid.AllowUserToDeleteRows = False
        Me.BehaviorsGrid.AllowUserToOrderColumns = True
        Me.BehaviorsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BehaviorsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.BehaviorsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPreview, Me.colEdit, Me.colName, Me.colGroup, Me.colChance})
        Me.BehaviorsGrid.Location = New System.Drawing.Point(3, 32)
        Me.BehaviorsGrid.Name = "BehaviorsGrid"
        Me.BehaviorsGrid.ReadOnly = True
        Me.BehaviorsGrid.Size = New System.Drawing.Size(494, 265)
        Me.BehaviorsGrid.TabIndex = 1
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
        Me.colName.Width = 150
        '
        'colGroup
        '
        Me.colGroup.HeaderText = "Group"
        Me.colGroup.Name = "colGroup"
        Me.colGroup.ReadOnly = True
        Me.colGroup.Width = 75
        '
        'colChance
        '
        Me.colChance.HeaderText = "Chance"
        Me.colChance.Name = "colChance"
        Me.colChance.ReadOnly = True
        Me.colChance.Width = 75
        '
        'GroupNamesButton
        '
        Me.GroupNamesButton.Location = New System.Drawing.Point(3, 3)
        Me.GroupNamesButton.Name = "GroupNamesButton"
        Me.GroupNamesButton.Size = New System.Drawing.Size(125, 23)
        Me.GroupNamesButton.TabIndex = 0
        Me.GroupNamesButton.Text = "Edit Group Names"
        Me.GroupNamesButton.UseVisualStyleBackColor = True
        '
        'BehaviorsViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.GroupNamesButton)
        Me.Controls.Add(Me.BehaviorsGrid)
        Me.Name = "BehaviorsViewer"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BehaviorsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents GroupNamesButton As System.Windows.Forms.Button
    Friend WithEvents colPreview As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colEdit As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colGroup As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colChance As System.Windows.Forms.DataGridViewTextBoxColumn

End Class

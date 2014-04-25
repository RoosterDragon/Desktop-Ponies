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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.InteractionsGrid = New System.Windows.Forms.DataGridView()
        Me.colEdit = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colChance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colProximity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colReactivationDelay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colInteractsWith = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTargets = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehaviors = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SuspendLayout()
        '
        'InteractionsGrid
        '
        Me.InteractionsGrid.AllowUserToAddRows = False
        Me.InteractionsGrid.AllowUserToDeleteRows = False
        Me.InteractionsGrid.AllowUserToOrderColumns = True
        Me.InteractionsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InteractionsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.InteractionsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colEdit, Me.colName, Me.colChance, Me.colProximity, Me.colReactivationDelay, Me.colInteractsWith, Me.colTargets, Me.colBehaviors})
        Me.InteractionsGrid.Location = New System.Drawing.Point(3, 32)
        Me.InteractionsGrid.Name = "InteractionsGrid"
        Me.InteractionsGrid.ReadOnly = True
        Me.InteractionsGrid.Size = New System.Drawing.Size(494, 265)
        Me.InteractionsGrid.TabIndex = 1
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
        'colChance
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colChance.DefaultCellStyle = DataGridViewCellStyle1
        Me.colChance.HeaderText = "Chance"
        Me.colChance.Name = "colChance"
        Me.colChance.ReadOnly = True
        Me.colChance.Width = 55
        '
        'colProximity
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colProximity.DefaultCellStyle = DataGridViewCellStyle2
        Me.colProximity.HeaderText = "Proximity"
        Me.colProximity.Name = "colProximity"
        Me.colProximity.ReadOnly = True
        Me.colProximity.Width = 55
        '
        'colReactivationDelay
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colReactivationDelay.DefaultCellStyle = DataGridViewCellStyle3
        Me.colReactivationDelay.HeaderText = "Reactiviation Delay"
        Me.colReactivationDelay.Name = "colReactivationDelay"
        Me.colReactivationDelay.ReadOnly = True
        Me.colReactivationDelay.Width = 125
        '
        'colInteractsWith
        '
        Me.colInteractsWith.HeaderText = "Interacts With"
        Me.colInteractsWith.Name = "colInteractsWith"
        Me.colInteractsWith.ReadOnly = True
        '
        'colTargets
        '
        Me.colTargets.HeaderText = "Targets"
        Me.colTargets.Name = "colTargets"
        Me.colTargets.ReadOnly = True
        Me.colTargets.Width = 300
        '
        'colBehaviors
        '
        Me.colBehaviors.HeaderText = "Behaviors"
        Me.colBehaviors.Name = "colBehaviors"
        Me.colBehaviors.ReadOnly = True
        Me.colBehaviors.Width = 150
        '
        'InteractionsViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.InteractionsGrid)
        Me.Name = "InteractionsViewer"
        Me.Controls.SetChildIndex(Me.InteractionsGrid, 0)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents InteractionsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents colEdit As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colChance As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colProximity As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colReactivationDelay As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colInteractsWith As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTargets As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehaviors As System.Windows.Forms.DataGridViewTextBoxColumn

End Class

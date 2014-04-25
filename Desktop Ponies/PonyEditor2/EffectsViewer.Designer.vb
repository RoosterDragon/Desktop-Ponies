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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.EffectsGrid = New System.Windows.Forms.DataGridView()
        Me.colPreview = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colEdit = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colDuration = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRepeatDelay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehavior = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFollowPony = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.colPreventLoop = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.colLeftImage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRightImage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLeftPlacement = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLeftCenter = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRightPlacement = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRightCenter = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.SuspendLayout()
        '
        'EffectsGrid
        '
        Me.EffectsGrid.AllowUserToAddRows = False
        Me.EffectsGrid.AllowUserToDeleteRows = False
        Me.EffectsGrid.AllowUserToOrderColumns = True
        Me.EffectsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EffectsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.EffectsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPreview, Me.colEdit, Me.colName, Me.colDuration, Me.colRepeatDelay, Me.colBehavior, Me.colFollowPony, Me.colPreventLoop, Me.colLeftImage, Me.colRightImage, Me.colLeftPlacement, Me.colLeftCenter, Me.colRightPlacement, Me.colRightCenter})
        Me.EffectsGrid.Location = New System.Drawing.Point(3, 32)
        Me.EffectsGrid.Name = "EffectsGrid"
        Me.EffectsGrid.ReadOnly = True
        Me.EffectsGrid.Size = New System.Drawing.Size(494, 265)
        Me.EffectsGrid.TabIndex = 1
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
        'colDuration
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colDuration.DefaultCellStyle = DataGridViewCellStyle1
        Me.colDuration.HeaderText = "Duration"
        Me.colDuration.Name = "colDuration"
        Me.colDuration.ReadOnly = True
        Me.colDuration.Width = 60
        '
        'colRepeatDelay
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colRepeatDelay.DefaultCellStyle = DataGridViewCellStyle2
        Me.colRepeatDelay.HeaderText = "Repeat Delay"
        Me.colRepeatDelay.Name = "colRepeatDelay"
        Me.colRepeatDelay.ReadOnly = True
        '
        'colBehavior
        '
        Me.colBehavior.HeaderText = "Behavior"
        Me.colBehavior.Name = "colBehavior"
        Me.colBehavior.ReadOnly = True
        Me.colBehavior.Width = 150
        '
        'colFollowPony
        '
        Me.colFollowPony.HeaderText = "Follow Pony"
        Me.colFollowPony.Name = "colFollowPony"
        Me.colFollowPony.ReadOnly = True
        Me.colFollowPony.Width = 70
        '
        'colPreventLoop
        '
        Me.colPreventLoop.HeaderText = "Prevent Loop"
        Me.colPreventLoop.Name = "colPreventLoop"
        Me.colPreventLoop.ReadOnly = True
        Me.colPreventLoop.Width = 80
        '
        'colLeftImage
        '
        Me.colLeftImage.HeaderText = "Left Image"
        Me.colLeftImage.Name = "colLeftImage"
        Me.colLeftImage.ReadOnly = True
        Me.colLeftImage.Width = 150
        '
        'colRightImage
        '
        Me.colRightImage.HeaderText = "Right Image"
        Me.colRightImage.Name = "colRightImage"
        Me.colRightImage.ReadOnly = True
        Me.colRightImage.Width = 150
        '
        'colLeftPlacement
        '
        Me.colLeftPlacement.HeaderText = "Left Placement"
        Me.colLeftPlacement.Name = "colLeftPlacement"
        Me.colLeftPlacement.ReadOnly = True
        Me.colLeftPlacement.Width = 110
        '
        'colLeftCenter
        '
        Me.colLeftCenter.HeaderText = "Left Centering"
        Me.colLeftCenter.Name = "colLeftCenter"
        Me.colLeftCenter.ReadOnly = True
        Me.colLeftCenter.Width = 110
        '
        'colRightPlacement
        '
        Me.colRightPlacement.HeaderText = "Right Placement"
        Me.colRightPlacement.Name = "colRightPlacement"
        Me.colRightPlacement.ReadOnly = True
        Me.colRightPlacement.Width = 110
        '
        'colRightCenter
        '
        Me.colRightCenter.HeaderText = "Right Centering"
        Me.colRightCenter.Name = "colRightCenter"
        Me.colRightCenter.ReadOnly = True
        Me.colRightCenter.Width = 110
        '
        'EffectsViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.EffectsGrid)
        Me.Name = "EffectsViewer"
        Me.Controls.SetChildIndex(Me.EffectsGrid, 0)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents EffectsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents colPreview As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colEdit As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDuration As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRepeatDelay As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehavior As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFollowPony As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colPreventLoop As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colLeftImage As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRightImage As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colLeftPlacement As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colLeftCenter As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRightPlacement As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRightCenter As System.Windows.Forms.DataGridViewTextBoxColumn

End Class

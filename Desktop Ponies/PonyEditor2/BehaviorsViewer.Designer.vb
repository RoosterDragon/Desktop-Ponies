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
        Me.colSpeed = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colMovement = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colMinDuration = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colMaxDuration = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLeftImage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRightImage = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colStartSpeech = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEndSpeech = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLinkedBehavior = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTarget = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFollowStoppedBehavior = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFollowMovingBehavior = New System.Windows.Forms.DataGridViewTextBoxColumn()
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
        Me.BehaviorsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPreview, Me.colEdit, Me.colName, Me.colGroup, Me.colChance, Me.colSpeed, Me.colMovement, Me.colMinDuration, Me.colMaxDuration, Me.colLeftImage, Me.colRightImage, Me.colStartSpeech, Me.colEndSpeech, Me.colLinkedBehavior, Me.colTarget, Me.colFollowStoppedBehavior, Me.colFollowMovingBehavior})
        Me.BehaviorsGrid.Location = New System.Drawing.Point(3, 32)
        Me.BehaviorsGrid.Name = "BehaviorsGrid"
        Me.BehaviorsGrid.ReadOnly = True
        Me.BehaviorsGrid.Size = New System.Drawing.Size(494, 265)
        Me.BehaviorsGrid.TabIndex = 2
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
        Me.colChance.Width = 55
        '
        'colSpeed
        '
        Me.colSpeed.HeaderText = "Speed"
        Me.colSpeed.Name = "colSpeed"
        Me.colSpeed.ReadOnly = True
        Me.colSpeed.Width = 50
        '
        'colMovement
        '
        Me.colMovement.HeaderText = "Movement"
        Me.colMovement.Name = "colMovement"
        Me.colMovement.ReadOnly = True
        '
        'colMinDuration
        '
        Me.colMinDuration.HeaderText = "Min Duration"
        Me.colMinDuration.Name = "colMinDuration"
        Me.colMinDuration.ReadOnly = True
        Me.colMinDuration.Width = 95
        '
        'colMaxDuration
        '
        Me.colMaxDuration.HeaderText = "Max Duration"
        Me.colMaxDuration.Name = "colMaxDuration"
        Me.colMaxDuration.ReadOnly = True
        Me.colMaxDuration.Width = 95
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
        'colStartSpeech
        '
        Me.colStartSpeech.HeaderText = "Start Speech"
        Me.colStartSpeech.Name = "colStartSpeech"
        Me.colStartSpeech.ReadOnly = True
        Me.colStartSpeech.Width = 150
        '
        'colEndSpeech
        '
        Me.colEndSpeech.HeaderText = "End Speech"
        Me.colEndSpeech.Name = "colEndSpeech"
        Me.colEndSpeech.ReadOnly = True
        Me.colEndSpeech.Width = 150
        '
        'colLinkedBehavior
        '
        Me.colLinkedBehavior.HeaderText = "Linked Behavior"
        Me.colLinkedBehavior.Name = "colLinkedBehavior"
        Me.colLinkedBehavior.ReadOnly = True
        Me.colLinkedBehavior.Width = 150
        '
        'colTarget
        '
        Me.colTarget.HeaderText = "Target"
        Me.colTarget.Name = "colTarget"
        Me.colTarget.ReadOnly = True
        '
        'colFollowStoppedBehavior
        '
        Me.colFollowStoppedBehavior.HeaderText = "Follow Stopped Behavior"
        Me.colFollowStoppedBehavior.Name = "colFollowStoppedBehavior"
        Me.colFollowStoppedBehavior.ReadOnly = True
        Me.colFollowStoppedBehavior.Width = 150
        '
        'colFollowMovingBehavior
        '
        Me.colFollowMovingBehavior.HeaderText = "Follow Moving Behavior"
        Me.colFollowMovingBehavior.Name = "colFollowMovingBehavior"
        Me.colFollowMovingBehavior.ReadOnly = True
        Me.colFollowMovingBehavior.Width = 150
        '
        'GroupNamesButton
        '
        Me.GroupNamesButton.Location = New System.Drawing.Point(84, 3)
        Me.GroupNamesButton.Name = "GroupNamesButton"
        Me.GroupNamesButton.Size = New System.Drawing.Size(125, 23)
        Me.GroupNamesButton.TabIndex = 1
        Me.GroupNamesButton.Text = "Edit Group Names"
        Me.GroupNamesButton.UseVisualStyleBackColor = True
        '
        'BehaviorsViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.GroupNamesButton)
        Me.Controls.Add(Me.BehaviorsGrid)
        Me.Name = "BehaviorsViewer"
        Me.Controls.SetChildIndex(Me.BehaviorsGrid, 0)
        Me.Controls.SetChildIndex(Me.GroupNamesButton, 0)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BehaviorsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents GroupNamesButton As System.Windows.Forms.Button
    Friend WithEvents colPreview As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colEdit As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colGroup As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colChance As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSpeed As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colMovement As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colMinDuration As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colMaxDuration As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colLeftImage As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRightImage As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colStartSpeech As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colEndSpeech As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colLinkedBehavior As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTarget As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFollowStoppedBehavior As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFollowMovingBehavior As System.Windows.Forms.DataGridViewTextBoxColumn

End Class

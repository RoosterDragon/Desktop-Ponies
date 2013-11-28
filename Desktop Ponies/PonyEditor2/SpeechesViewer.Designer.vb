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
        Me.colGroup = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colRandom = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.colLine = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSoundFile = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupNamesButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'SpeechesGrid
        '
        Me.SpeechesGrid.AllowUserToAddRows = False
        Me.SpeechesGrid.AllowUserToDeleteRows = False
        Me.SpeechesGrid.AllowUserToOrderColumns = True
        Me.SpeechesGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SpeechesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.SpeechesGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colEdit, Me.colName, Me.colGroup, Me.colRandom, Me.colLine, Me.colSoundFile})
        Me.SpeechesGrid.Location = New System.Drawing.Point(3, 32)
        Me.SpeechesGrid.Name = "SpeechesGrid"
        Me.SpeechesGrid.ReadOnly = True
        Me.SpeechesGrid.Size = New System.Drawing.Size(494, 265)
        Me.SpeechesGrid.TabIndex = 2
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
        'colRandom
        '
        Me.colRandom.HeaderText = "Use Randomly"
        Me.colRandom.Name = "colRandom"
        Me.colRandom.ReadOnly = True
        Me.colRandom.Width = 90
        '
        'colLine
        '
        Me.colLine.HeaderText = "Line"
        Me.colLine.Name = "colLine"
        Me.colLine.ReadOnly = True
        Me.colLine.Width = 325
        '
        'colSoundFile
        '
        Me.colSoundFile.HeaderText = "Sound File"
        Me.colSoundFile.Name = "colSoundFile"
        Me.colSoundFile.ReadOnly = True
        Me.colSoundFile.Width = 150
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
        'SpeechesViewer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.Controls.Add(Me.GroupNamesButton)
        Me.Controls.Add(Me.SpeechesGrid)
        Me.Name = "SpeechesViewer"
        Me.Controls.SetChildIndex(Me.SpeechesGrid, 0)
        Me.Controls.SetChildIndex(Me.GroupNamesButton, 0)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents SpeechesGrid As System.Windows.Forms.DataGridView
    Friend WithEvents colEdit As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colGroup As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colRandom As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colLine As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSoundFile As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents GroupNamesButton As System.Windows.Forms.Button

End Class

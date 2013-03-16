<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PonySelectionControl
    Inherits System.Windows.Forms.UserControl

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.PonyCountLabel = New System.Windows.Forms.Label()
        Me.NoDuplicates = New System.Windows.Forms.CheckBox()
        Me.PonyCount = New System.Windows.Forms.TextBox()
        Me.PonyName = New System.Windows.Forms.Label()
        Me.DetailPanel = New System.Windows.Forms.Panel()
        Me.PlusButton = New System.Windows.Forms.Button()
        Me.MinusButton = New System.Windows.Forms.Button()
        Me.DetailPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'PonyCountLabel
        '
        Me.PonyCountLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PonyCountLabel.AutoSize = True
        Me.PonyCountLabel.BackColor = System.Drawing.Color.Transparent
        Me.PonyCountLabel.Location = New System.Drawing.Point(4, 35)
        Me.PonyCountLabel.Name = "PonyCountLabel"
        Me.PonyCountLabel.Size = New System.Drawing.Size(64, 13)
        Me.PonyCountLabel.TabIndex = 2
        Me.PonyCountLabel.Text = "How Many?"
        '
        'NoDuplicates
        '
        Me.NoDuplicates.AutoSize = True
        Me.NoDuplicates.BackColor = System.Drawing.Color.Transparent
        Me.NoDuplicates.Checked = True
        Me.NoDuplicates.CheckState = System.Windows.Forms.CheckState.Checked
        Me.NoDuplicates.Location = New System.Drawing.Point(3, 16)
        Me.NoDuplicates.Name = "NoDuplicates"
        Me.NoDuplicates.Size = New System.Drawing.Size(93, 17)
        Me.NoDuplicates.TabIndex = 1
        Me.NoDuplicates.TabStop = False
        Me.NoDuplicates.Text = "No Duplicates"
        Me.NoDuplicates.UseVisualStyleBackColor = False
        Me.NoDuplicates.Visible = False
        '
        'PonyCount
        '
        Me.PonyCount.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PonyCount.Location = New System.Drawing.Point(32, 51)
        Me.PonyCount.MaxLength = 5
        Me.PonyCount.Name = "PonyCount"
        Me.PonyCount.Size = New System.Drawing.Size(36, 20)
        Me.PonyCount.TabIndex = 4
        Me.PonyCount.Text = "0"
        Me.PonyCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
        '
        'PonyName
        '
        Me.PonyName.AutoSize = True
        Me.PonyName.BackColor = System.Drawing.Color.Transparent
        Me.PonyName.Location = New System.Drawing.Point(3, 0)
        Me.PonyName.Name = "PonyName"
        Me.PonyName.Size = New System.Drawing.Size(47, 13)
        Me.PonyName.TabIndex = 0
        Me.PonyName.Text = "<Name>"
        '
        'DetailPanel
        '
        Me.DetailPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DetailPanel.BackColor = System.Drawing.Color.Transparent
        Me.DetailPanel.Controls.Add(Me.PlusButton)
        Me.DetailPanel.Controls.Add(Me.MinusButton)
        Me.DetailPanel.Controls.Add(Me.PonyName)
        Me.DetailPanel.Controls.Add(Me.PonyCountLabel)
        Me.DetailPanel.Controls.Add(Me.PonyCount)
        Me.DetailPanel.Controls.Add(Me.NoDuplicates)
        Me.DetailPanel.Location = New System.Drawing.Point(100, 0)
        Me.DetailPanel.Margin = New System.Windows.Forms.Padding(0)
        Me.DetailPanel.MinimumSize = New System.Drawing.Size(100, 75)
        Me.DetailPanel.Name = "DetailPanel"
        Me.DetailPanel.Size = New System.Drawing.Size(100, 75)
        Me.DetailPanel.TabIndex = 0
        '
        'PlusButton
        '
        Me.PlusButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PlusButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.PlusButton.Location = New System.Drawing.Point(74, 49)
        Me.PlusButton.Name = "PlusButton"
        Me.PlusButton.Size = New System.Drawing.Size(23, 23)
        Me.PlusButton.TabIndex = 5
        Me.PlusButton.TabStop = False
        Me.PlusButton.Text = "+"
        Me.PlusButton.UseVisualStyleBackColor = True
        '
        'MinusButton
        '
        Me.MinusButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.MinusButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinusButton.Location = New System.Drawing.Point(3, 49)
        Me.MinusButton.Name = "MinusButton"
        Me.MinusButton.Size = New System.Drawing.Size(23, 23)
        Me.MinusButton.TabIndex = 3
        Me.MinusButton.TabStop = False
        Me.MinusButton.Text = "-"
        Me.MinusButton.UseVisualStyleBackColor = True
        '
        'PonySelectionControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.ControlDark
        Me.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Controls.Add(Me.DetailPanel)
        Me.DoubleBuffered = True
        Me.Name = "PonySelectionControl"
        Me.Size = New System.Drawing.Size(200, 75)
        Me.DetailPanel.ResumeLayout(False)
        Me.DetailPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PonyCountLabel As System.Windows.Forms.Label
    Friend WithEvents NoDuplicates As System.Windows.Forms.CheckBox
    Friend WithEvents PonyCount As System.Windows.Forms.TextBox
    Friend WithEvents PonyName As System.Windows.Forms.Label
    Friend WithEvents DetailPanel As System.Windows.Forms.Panel
    Friend WithEvents PlusButton As System.Windows.Forms.Button
    Friend WithEvents MinusButton As System.Windows.Forms.Button

End Class

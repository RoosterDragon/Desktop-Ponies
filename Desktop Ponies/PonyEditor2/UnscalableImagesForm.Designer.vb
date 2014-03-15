<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UnscalableImagesForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(UnscalableImagesForm))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.InfoLabel = New System.Windows.Forms.Label()
        Me.ImagesGrid = New System.Windows.Forms.DataGridView()
        Me.colImagePath = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colFilesize = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.IgnoreListLabel = New System.Windows.Forms.Label()
        Me.IgnoreListTextBox = New System.Windows.Forms.TextBox()
        Me.FilterButton = New System.Windows.Forms.Button()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.CountLabel = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'InfoLabel
        '
        Me.InfoLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InfoLabel.Location = New System.Drawing.Point(12, 9)
        Me.InfoLabel.Name = "InfoLabel"
        Me.InfoLabel.Size = New System.Drawing.Size(460, 52)
        Me.InfoLabel.TabIndex = 0
        Me.InfoLabel.Text = resources.GetString("InfoLabel.Text")
        '
        'ImagesGrid
        '
        Me.ImagesGrid.AllowUserToAddRows = False
        Me.ImagesGrid.AllowUserToDeleteRows = False
        Me.ImagesGrid.AllowUserToOrderColumns = True
        Me.ImagesGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ImagesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ImagesGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colImagePath, Me.colFilesize})
        Me.ImagesGrid.Location = New System.Drawing.Point(12, 77)
        Me.ImagesGrid.Name = "ImagesGrid"
        Me.ImagesGrid.ReadOnly = True
        Me.ImagesGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.ImagesGrid.Size = New System.Drawing.Size(460, 250)
        Me.ImagesGrid.TabIndex = 2
        '
        'colImagePath
        '
        Me.colImagePath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colImagePath.HeaderText = "Image Path"
        Me.colImagePath.Name = "colImagePath"
        Me.colImagePath.ReadOnly = True
        '
        'colFilesize
        '
        Me.colFilesize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        Me.colFilesize.DefaultCellStyle = DataGridViewCellStyle1
        Me.colFilesize.HeaderText = "File Size"
        Me.colFilesize.Name = "colFilesize"
        Me.colFilesize.ReadOnly = True
        Me.colFilesize.Width = 71
        '
        'IgnoreListLabel
        '
        Me.IgnoreListLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.IgnoreListLabel.Location = New System.Drawing.Point(12, 330)
        Me.IgnoreListLabel.Name = "IgnoreListLabel"
        Me.IgnoreListLabel.Size = New System.Drawing.Size(460, 52)
        Me.IgnoreListLabel.TabIndex = 3
        Me.IgnoreListLabel.Text = resources.GetString("IgnoreListLabel.Text")
        '
        'IgnoreListTextBox
        '
        Me.IgnoreListTextBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.IgnoreListTextBox.Enabled = False
        Me.IgnoreListTextBox.Location = New System.Drawing.Point(12, 385)
        Me.IgnoreListTextBox.Multiline = True
        Me.IgnoreListTextBox.Name = "IgnoreListTextBox"
        Me.IgnoreListTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.IgnoreListTextBox.Size = New System.Drawing.Size(460, 86)
        Me.IgnoreListTextBox.TabIndex = 4
        Me.IgnoreListTextBox.Text = resources.GetString("IgnoreListTextBox.Text")
        '
        'FilterButton
        '
        Me.FilterButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FilterButton.Enabled = False
        Me.FilterButton.Location = New System.Drawing.Point(316, 477)
        Me.FilterButton.Name = "FilterButton"
        Me.FilterButton.Size = New System.Drawing.Size(75, 23)
        Me.FilterButton.TabIndex = 5
        Me.FilterButton.Text = "Update Filter"
        Me.FilterButton.UseVisualStyleBackColor = True
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.Location = New System.Drawing.Point(397, 477)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 6
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'CountLabel
        '
        Me.CountLabel.AutoSize = True
        Me.CountLabel.Location = New System.Drawing.Point(12, 61)
        Me.CountLabel.Name = "CountLabel"
        Me.CountLabel.Size = New System.Drawing.Size(56, 13)
        Me.CountLabel.TabIndex = 1
        Me.CountLabel.Text = "Working..."
        '
        'UnscalableImagesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 512)
        Me.Controls.Add(Me.CountLabel)
        Me.Controls.Add(Me.CloseButton)
        Me.Controls.Add(Me.FilterButton)
        Me.Controls.Add(Me.IgnoreListTextBox)
        Me.Controls.Add(Me.IgnoreListLabel)
        Me.Controls.Add(Me.ImagesGrid)
        Me.Controls.Add(Me.InfoLabel)
        Me.MinimumSize = New System.Drawing.Size(400, 350)
        Me.Name = "UnscalableImagesForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Unscalable Images - Desktop Ponies"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents InfoLabel As System.Windows.Forms.Label
    Friend WithEvents ImagesGrid As System.Windows.Forms.DataGridView
    Friend WithEvents IgnoreListLabel As System.Windows.Forms.Label
    Friend WithEvents IgnoreListTextBox As System.Windows.Forms.TextBox
    Friend WithEvents FilterButton As System.Windows.Forms.Button
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents CountLabel As System.Windows.Forms.Label
    Friend WithEvents colImagePath As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colFilesize As System.Windows.Forms.DataGridViewTextBoxColumn
End Class

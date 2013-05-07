<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PonyEditorForm2
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
        Me.EditingArea = New System.Windows.Forms.SplitContainer()
        Me.Documents = New System.Windows.Forms.TabControl()
        Me.DocumentsView = New System.Windows.Forms.TreeView()
        Me.EditorToolStrip = New System.Windows.Forms.ToolStrip()
        Me.EditorStatusStrip = New System.Windows.Forms.StatusStrip()
        Me.EditorStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.EditorProgressBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.SaveButton = New System.Windows.Forms.ToolStripButton()
        Me.Output = New System.Windows.Forms.TabControl()
        Me.PreviewPage = New System.Windows.Forms.TabPage()
        Me.ErrorsPage = New System.Windows.Forms.TabPage()
        Me.EditingArea.Panel1.SuspendLayout()
        Me.EditingArea.Panel2.SuspendLayout()
        Me.EditingArea.SuspendLayout()
        Me.EditorToolStrip.SuspendLayout()
        Me.EditorStatusStrip.SuspendLayout()
        Me.Output.SuspendLayout()
        Me.SuspendLayout()
        '
        'EditingArea
        '
        Me.EditingArea.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EditingArea.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
        Me.EditingArea.Location = New System.Drawing.Point(0, 28)
        Me.EditingArea.Name = "EditingArea"
        '
        'EditingArea.Panel1
        '
        Me.EditingArea.Panel1.Controls.Add(Me.Documents)
        Me.EditingArea.Panel1.UseWaitCursor = True
        Me.EditingArea.Panel1MinSize = 400
        '
        'EditingArea.Panel2
        '
        Me.EditingArea.Panel2.Controls.Add(Me.DocumentsView)
        Me.EditingArea.Panel2.UseWaitCursor = True
        Me.EditingArea.Panel2MinSize = 100
        Me.EditingArea.Size = New System.Drawing.Size(784, 253)
        Me.EditingArea.SplitterDistance = 550
        Me.EditingArea.TabIndex = 0
        Me.EditingArea.UseWaitCursor = True
        '
        'Documents
        '
        Me.Documents.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Documents.Location = New System.Drawing.Point(0, 0)
        Me.Documents.Name = "Documents"
        Me.Documents.SelectedIndex = 0
        Me.Documents.Size = New System.Drawing.Size(550, 253)
        Me.Documents.TabIndex = 0
        Me.Documents.UseWaitCursor = True
        '
        'DocumentsView
        '
        Me.DocumentsView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DocumentsView.FullRowSelect = True
        Me.DocumentsView.HotTracking = True
        Me.DocumentsView.Location = New System.Drawing.Point(0, 0)
        Me.DocumentsView.Name = "DocumentsView"
        Me.DocumentsView.Size = New System.Drawing.Size(230, 253)
        Me.DocumentsView.TabIndex = 0
        Me.DocumentsView.UseWaitCursor = True
        '
        'EditorToolStrip
        '
        Me.EditorToolStrip.ImageScalingSize = New System.Drawing.Size(32, 32)
        Me.EditorToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveButton})
        Me.EditorToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.EditorToolStrip.Name = "EditorToolStrip"
        Me.EditorToolStrip.Size = New System.Drawing.Size(784, 25)
        Me.EditorToolStrip.TabIndex = 0
        Me.EditorToolStrip.Text = "Controls"
        Me.EditorToolStrip.UseWaitCursor = True
        '
        'EditorStatusStrip
        '
        Me.EditorStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditorStatus, Me.EditorProgressBar})
        Me.EditorStatusStrip.Location = New System.Drawing.Point(0, 490)
        Me.EditorStatusStrip.Name = "EditorStatusStrip"
        Me.EditorStatusStrip.Size = New System.Drawing.Size(784, 22)
        Me.EditorStatusStrip.TabIndex = 2
        Me.EditorStatusStrip.Text = "StatusStrip1"
        Me.EditorStatusStrip.UseWaitCursor = True
        '
        'EditorStatus
        '
        Me.EditorStatus.AutoSize = False
        Me.EditorStatus.Name = "EditorStatus"
        Me.EditorStatus.Size = New System.Drawing.Size(80, 17)
        Me.EditorStatus.Text = "Loading..."
        Me.EditorStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.EditorStatus.TextDirection = System.Windows.Forms.ToolStripTextDirection.Horizontal
        '
        'EditorProgressBar
        '
        Me.EditorProgressBar.Name = "EditorProgressBar"
        Me.EditorProgressBar.Size = New System.Drawing.Size(100, 16)
        Me.EditorProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'SaveButton
        '
        Me.SaveButton.Enabled = False
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(35, 22)
        Me.SaveButton.Text = "Save"
        Me.SaveButton.ToolTipText = "Save This Item"
        '
        'Output
        '
        Me.Output.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Output.Controls.Add(Me.PreviewPage)
        Me.Output.Controls.Add(Me.ErrorsPage)
        Me.Output.Location = New System.Drawing.Point(0, 287)
        Me.Output.Multiline = True
        Me.Output.Name = "Output"
        Me.Output.SelectedIndex = 0
        Me.Output.Size = New System.Drawing.Size(784, 200)
        Me.Output.TabIndex = 3
        Me.Output.UseWaitCursor = True
        '
        'PreviewPage
        '
        Me.PreviewPage.Location = New System.Drawing.Point(4, 22)
        Me.PreviewPage.Name = "PreviewPage"
        Me.PreviewPage.Padding = New System.Windows.Forms.Padding(3)
        Me.PreviewPage.Size = New System.Drawing.Size(776, 174)
        Me.PreviewPage.TabIndex = 0
        Me.PreviewPage.Text = "Preview"
        Me.PreviewPage.UseVisualStyleBackColor = True
        Me.PreviewPage.UseWaitCursor = True
        '
        'ErrorsPage
        '
        Me.ErrorsPage.Location = New System.Drawing.Point(4, 22)
        Me.ErrorsPage.Name = "ErrorsPage"
        Me.ErrorsPage.Padding = New System.Windows.Forms.Padding(3)
        Me.ErrorsPage.Size = New System.Drawing.Size(776, 174)
        Me.ErrorsPage.TabIndex = 1
        Me.ErrorsPage.Text = "Errors"
        Me.ErrorsPage.UseVisualStyleBackColor = True
        Me.ErrorsPage.UseWaitCursor = True
        '
        'PonyEditorForm2
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 512)
        Me.Controls.Add(Me.Output)
        Me.Controls.Add(Me.EditorStatusStrip)
        Me.Controls.Add(Me.EditingArea)
        Me.Controls.Add(Me.EditorToolStrip)
        Me.Name = "PonyEditorForm2"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pony Editor - Desktop Ponies"
        Me.UseWaitCursor = True
        Me.EditingArea.Panel1.ResumeLayout(False)
        Me.EditingArea.Panel2.ResumeLayout(False)
        Me.EditingArea.ResumeLayout(False)
        Me.EditorToolStrip.ResumeLayout(False)
        Me.EditorToolStrip.PerformLayout()
        Me.EditorStatusStrip.ResumeLayout(False)
        Me.EditorStatusStrip.PerformLayout()
        Me.Output.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents EditingArea As System.Windows.Forms.SplitContainer
    Friend WithEvents Documents As System.Windows.Forms.TabControl
    Friend WithEvents DocumentsView As System.Windows.Forms.TreeView
    Friend WithEvents EditorToolStrip As System.Windows.Forms.ToolStrip
    Friend WithEvents EditorStatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents EditorStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents EditorProgressBar As System.Windows.Forms.ToolStripProgressBar
    Private WithEvents SaveButton As System.Windows.Forms.ToolStripButton
    Friend WithEvents Output As System.Windows.Forms.TabControl
    Friend WithEvents PreviewPage As System.Windows.Forms.TabPage
    Friend WithEvents ErrorsPage As System.Windows.Forms.TabPage
End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PonyPreview
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.PreviewPanel = New System.Windows.Forms.Panel()
        Me.InfoTable = New System.Windows.Forms.TableLayoutPanel()
        Me.PonyNameLabel = New System.Windows.Forms.Label()
        Me.PonyNameValueLabel = New System.Windows.Forms.Label()
        Me.BehaviorNameLabel = New System.Windows.Forms.Label()
        Me.BehaviorNameValueLabel = New System.Windows.Forms.Label()
        Me.TimeLeftLabel = New System.Windows.Forms.Label()
        Me.TimeLeftValueLabel = New System.Windows.Forms.Label()
        Me.InfoTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'PreviewPanel
        '
        Me.PreviewPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PreviewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PreviewPanel.Location = New System.Drawing.Point(3, 3)
        Me.PreviewPanel.Name = "PreviewPanel"
        Me.PreviewPanel.Size = New System.Drawing.Size(494, 262)
        Me.PreviewPanel.TabIndex = 0
        '
        'InfoTable
        '
        Me.InfoTable.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InfoTable.AutoSize = True
        Me.InfoTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.InfoTable.ColumnCount = 4
        Me.InfoTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.InfoTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.InfoTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.InfoTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.InfoTable.Controls.Add(Me.PonyNameLabel, 0, 0)
        Me.InfoTable.Controls.Add(Me.PonyNameValueLabel, 1, 0)
        Me.InfoTable.Controls.Add(Me.BehaviorNameLabel, 2, 0)
        Me.InfoTable.Controls.Add(Me.BehaviorNameValueLabel, 3, 0)
        Me.InfoTable.Controls.Add(Me.TimeLeftLabel, 2, 1)
        Me.InfoTable.Controls.Add(Me.TimeLeftValueLabel, 3, 1)
        Me.InfoTable.Location = New System.Drawing.Point(3, 271)
        Me.InfoTable.Name = "InfoTable"
        Me.InfoTable.RowCount = 2
        Me.InfoTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.InfoTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.InfoTable.Size = New System.Drawing.Size(494, 26)
        Me.InfoTable.TabIndex = 1
        '
        'PonyNameLabel
        '
        Me.PonyNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.PonyNameLabel.AutoSize = True
        Me.PonyNameLabel.Location = New System.Drawing.Point(3, 0)
        Me.PonyNameLabel.Name = "PonyNameLabel"
        Me.PonyNameLabel.Size = New System.Drawing.Size(34, 13)
        Me.PonyNameLabel.TabIndex = 0
        Me.PonyNameLabel.Text = "Pony:"
        '
        'PonyNameValueLabel
        '
        Me.PonyNameValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.PonyNameValueLabel.AutoSize = True
        Me.PonyNameValueLabel.Location = New System.Drawing.Point(43, 0)
        Me.PonyNameValueLabel.Name = "PonyNameValueLabel"
        Me.PonyNameValueLabel.Size = New System.Drawing.Size(0, 13)
        Me.PonyNameValueLabel.TabIndex = 1
        '
        'BehaviorNameLabel
        '
        Me.BehaviorNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.BehaviorNameLabel.AutoSize = True
        Me.BehaviorNameLabel.Location = New System.Drawing.Point(242, 0)
        Me.BehaviorNameLabel.Name = "BehaviorNameLabel"
        Me.BehaviorNameLabel.Size = New System.Drawing.Size(52, 13)
        Me.BehaviorNameLabel.TabIndex = 2
        Me.BehaviorNameLabel.Text = "Behavior:"
        '
        'BehaviorNameValueLabel
        '
        Me.BehaviorNameValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.BehaviorNameValueLabel.AutoSize = True
        Me.BehaviorNameValueLabel.Location = New System.Drawing.Point(300, 0)
        Me.BehaviorNameValueLabel.Name = "BehaviorNameValueLabel"
        Me.BehaviorNameValueLabel.Size = New System.Drawing.Size(0, 13)
        Me.BehaviorNameValueLabel.TabIndex = 3
        '
        'TimeLeftLabel
        '
        Me.TimeLeftLabel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.TimeLeftLabel.AutoSize = True
        Me.TimeLeftLabel.Location = New System.Drawing.Point(240, 13)
        Me.TimeLeftLabel.Name = "TimeLeftLabel"
        Me.TimeLeftLabel.Size = New System.Drawing.Size(54, 13)
        Me.TimeLeftLabel.TabIndex = 4
        Me.TimeLeftLabel.Text = "Time Left:"
        '
        'TimeLeftValueLabel
        '
        Me.TimeLeftValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.TimeLeftValueLabel.AutoSize = True
        Me.TimeLeftValueLabel.Location = New System.Drawing.Point(300, 13)
        Me.TimeLeftValueLabel.Name = "TimeLeftValueLabel"
        Me.TimeLeftValueLabel.Size = New System.Drawing.Size(0, 13)
        Me.TimeLeftValueLabel.TabIndex = 5
        '
        'PonyPreview
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.InfoTable)
        Me.Controls.Add(Me.PreviewPanel)
        Me.Name = "PonyPreview"
        Me.Size = New System.Drawing.Size(500, 300)
        Me.InfoTable.ResumeLayout(False)
        Me.InfoTable.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PreviewPanel As System.Windows.Forms.Panel
    Friend WithEvents InfoTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents PonyNameLabel As System.Windows.Forms.Label
    Friend WithEvents PonyNameValueLabel As System.Windows.Forms.Label
    Friend WithEvents BehaviorNameLabel As System.Windows.Forms.Label
    Friend WithEvents BehaviorNameValueLabel As System.Windows.Forms.Label
    Friend WithEvents TimeLeftLabel As System.Windows.Forms.Label
    Friend WithEvents TimeLeftValueLabel As System.Windows.Forms.Label

End Class

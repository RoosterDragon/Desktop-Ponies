<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PonyLogForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        Me.components = New System.ComponentModel.Container()
        Me.lblPony = New System.Windows.Forms.Label()
        Me.LogView = New System.Windows.Forms.TreeView()
        Me.RefreshTimer = New System.Windows.Forms.Timer(Me.components)
        Me.chkActiveRefresh = New System.Windows.Forms.CheckBox()
        Me.SuspendLayout()
        '
        'lblPony
        '
        Me.lblPony.AutoSize = True
        Me.lblPony.Location = New System.Drawing.Point(12, 13)
        Me.lblPony.Name = "lblPony"
        Me.lblPony.Size = New System.Drawing.Size(31, 13)
        Me.lblPony.TabIndex = 0
        Me.lblPony.Text = "Pony"
        '
        'LogView
        '
        Me.LogView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LogView.Location = New System.Drawing.Point(12, 35)
        Me.LogView.Name = "LogView"
        Me.LogView.Size = New System.Drawing.Size(526, 436)
        Me.LogView.TabIndex = 2
        '
        'RefreshTimer
        '
        Me.RefreshTimer.Enabled = True
        Me.RefreshTimer.Interval = 330
        '
        'chkActiveRefresh
        '
        Me.chkActiveRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkActiveRefresh.AutoSize = True
        Me.chkActiveRefresh.Checked = True
        Me.chkActiveRefresh.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkActiveRefresh.Location = New System.Drawing.Point(442, 12)
        Me.chkActiveRefresh.Name = "chkActiveRefresh"
        Me.chkActiveRefresh.Size = New System.Drawing.Size(96, 17)
        Me.chkActiveRefresh.TabIndex = 1
        Me.chkActiveRefresh.Text = "Active Refresh"
        Me.chkActiveRefresh.UseVisualStyleBackColor = True
        '
        'PonyLogForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(550, 483)
        Me.Controls.Add(Me.chkActiveRefresh)
        Me.Controls.Add(Me.LogView)
        Me.Controls.Add(Me.lblPony)
        Me.Name = "PonyLogForm"
        Me.Text = "Pony Logs - Desktop Ponies"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents lblPony As System.Windows.Forms.Label
    Friend WithEvents LogView As System.Windows.Forms.TreeView
    Friend WithEvents RefreshTimer As System.Windows.Forms.Timer
    Friend WithEvents chkActiveRefresh As System.Windows.Forms.CheckBox
End Class

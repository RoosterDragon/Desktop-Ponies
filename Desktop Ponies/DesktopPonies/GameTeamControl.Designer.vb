<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GameTeamControl
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
        Me.LayoutTable = New System.Windows.Forms.TableLayoutPanel()
        Me.TeamPanel = New System.Windows.Forms.Panel()
        Me.TeamNameLabel = New System.Windows.Forms.Label()
        Me.SpacesTable = New System.Windows.Forms.TableLayoutPanel()
        Me.RequiredSpacesLeftCountLabel = New System.Windows.Forms.Label()
        Me.SpacesLeftLabel = New System.Windows.Forms.Label()
        Me.RequiredSpacesLeftLabel = New System.Windows.Forms.Label()
        Me.SpacesLeftCountLabel = New System.Windows.Forms.Label()
        Me.LayoutTable.SuspendLayout()
        Me.SpacesTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'LayoutTable
        '
        Me.LayoutTable.AutoSize = True
        Me.LayoutTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.LayoutTable.ColumnCount = 1
        Me.LayoutTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.LayoutTable.Controls.Add(Me.TeamPanel, 0, 1)
        Me.LayoutTable.Controls.Add(Me.TeamNameLabel, 0, 0)
        Me.LayoutTable.Controls.Add(Me.SpacesTable, 0, 2)
        Me.LayoutTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutTable.Location = New System.Drawing.Point(0, 0)
        Me.LayoutTable.Name = "LayoutTable"
        Me.LayoutTable.RowCount = 4
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.LayoutTable.Size = New System.Drawing.Size(144, 73)
        Me.LayoutTable.TabIndex = 0
        '
        'TeamPanel
        '
        Me.TeamPanel.AutoScroll = True
        Me.TeamPanel.AutoSize = True
        Me.TeamPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TeamPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.TeamPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TeamPanel.Location = New System.Drawing.Point(3, 16)
        Me.TeamPanel.Name = "TeamPanel"
        Me.TeamPanel.Size = New System.Drawing.Size(138, 2)
        Me.TeamPanel.TabIndex = 1
        '
        'TeamNameLabel
        '
        Me.TeamNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.TeamNameLabel.AutoSize = True
        Me.TeamNameLabel.Location = New System.Drawing.Point(39, 0)
        Me.TeamNameLabel.Name = "TeamNameLabel"
        Me.TeamNameLabel.Size = New System.Drawing.Size(65, 13)
        Me.TeamNameLabel.TabIndex = 0
        Me.TeamNameLabel.Text = "Team Name"
        '
        'SpacesTable
        '
        Me.SpacesTable.AutoSize = True
        Me.SpacesTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.SpacesTable.ColumnCount = 2
        Me.SpacesTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.SpacesTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.SpacesTable.Controls.Add(Me.RequiredSpacesLeftCountLabel, 1, 1)
        Me.SpacesTable.Controls.Add(Me.SpacesLeftLabel, 0, 0)
        Me.SpacesTable.Controls.Add(Me.RequiredSpacesLeftLabel, 0, 1)
        Me.SpacesTable.Controls.Add(Me.SpacesLeftCountLabel, 1, 0)
        Me.SpacesTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SpacesTable.Location = New System.Drawing.Point(3, 24)
        Me.SpacesTable.Name = "SpacesTable"
        Me.SpacesTable.RowCount = 2
        Me.SpacesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.SpacesTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.SpacesTable.Size = New System.Drawing.Size(138, 26)
        Me.SpacesTable.TabIndex = 2
        '
        'RequiredSpacesLeftCountLabel
        '
        Me.RequiredSpacesLeftCountLabel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.RequiredSpacesLeftCountLabel.AutoSize = True
        Me.RequiredSpacesLeftCountLabel.Location = New System.Drawing.Point(122, 13)
        Me.RequiredSpacesLeftCountLabel.Name = "RequiredSpacesLeftCountLabel"
        Me.RequiredSpacesLeftCountLabel.Size = New System.Drawing.Size(13, 13)
        Me.RequiredSpacesLeftCountLabel.TabIndex = 3
        Me.RequiredSpacesLeftCountLabel.Text = "0"
        '
        'SpacesLeftLabel
        '
        Me.SpacesLeftLabel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.SpacesLeftLabel.AutoSize = True
        Me.SpacesLeftLabel.Location = New System.Drawing.Point(49, 0)
        Me.SpacesLeftLabel.Name = "SpacesLeftLabel"
        Me.SpacesLeftLabel.Size = New System.Drawing.Size(67, 13)
        Me.SpacesLeftLabel.TabIndex = 0
        Me.SpacesLeftLabel.Text = "Spaces Left:"
        '
        'RequiredSpacesLeftLabel
        '
        Me.RequiredSpacesLeftLabel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.RequiredSpacesLeftLabel.AutoSize = True
        Me.RequiredSpacesLeftLabel.Location = New System.Drawing.Point(3, 13)
        Me.RequiredSpacesLeftLabel.Name = "RequiredSpacesLeftLabel"
        Me.RequiredSpacesLeftLabel.Size = New System.Drawing.Size(113, 13)
        Me.RequiredSpacesLeftLabel.TabIndex = 2
        Me.RequiredSpacesLeftLabel.Text = "Required Spaces Left:"
        '
        'SpacesLeftCountLabel
        '
        Me.SpacesLeftCountLabel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.SpacesLeftCountLabel.AutoSize = True
        Me.SpacesLeftCountLabel.Location = New System.Drawing.Point(122, 0)
        Me.SpacesLeftCountLabel.Name = "SpacesLeftCountLabel"
        Me.SpacesLeftCountLabel.Size = New System.Drawing.Size(13, 13)
        Me.SpacesLeftCountLabel.TabIndex = 1
        Me.SpacesLeftCountLabel.Text = "0"
        '
        'GameTeamControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Controls.Add(Me.LayoutTable)
        Me.Name = "GameTeamControl"
        Me.Size = New System.Drawing.Size(144, 73)
        Me.LayoutTable.ResumeLayout(False)
        Me.LayoutTable.PerformLayout()
        Me.SpacesTable.ResumeLayout(False)
        Me.SpacesTable.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents LayoutTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TeamPanel As System.Windows.Forms.Panel
    Friend WithEvents TeamNameLabel As System.Windows.Forms.Label
    Friend WithEvents SpacesTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents RequiredSpacesLeftCountLabel As System.Windows.Forms.Label
    Friend WithEvents SpacesLeftLabel As System.Windows.Forms.Label
    Friend WithEvents RequiredSpacesLeftLabel As System.Windows.Forms.Label
    Friend WithEvents SpacesLeftCountLabel As System.Windows.Forms.Label

End Class

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GameSelectionForm
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
        Me.PonyList = New System.Windows.Forms.ListView()
        Me.GameList = New System.Windows.Forms.ListView()
        Me.Team1AddButton = New System.Windows.Forms.Button()
        Me.Team2AddButton = New System.Windows.Forms.Button()
        Me.LayoutTable = New System.Windows.Forms.TableLayoutPanel()
        Me.TeamTable = New System.Windows.Forms.TableLayoutPanel()
        Me.GameGroup = New System.Windows.Forms.GroupBox()
        Me.CommandsPanel = New System.Windows.Forms.Panel()
        Me.Transition3Label = New System.Windows.Forms.Label()
        Me.Transition2Label = New System.Windows.Forms.Label()
        Me.Transition1Label = New System.Windows.Forms.Label()
        Me.StartLabel = New System.Windows.Forms.Label()
        Me.SelectPlayersLabel = New System.Windows.Forms.Label()
        Me.SelectGameLabel = New System.Windows.Forms.Label()
        Me.MonitorLabel = New System.Windows.Forms.Label()
        Me.PlayButton = New System.Windows.Forms.Button()
        Me.MonitorComboBox = New System.Windows.Forms.ComboBox()
        Me.GameTeam2 = New DesktopPonies.GameTeamControl()
        Me.GameTeam1 = New DesktopPonies.GameTeamControl()
        Me.GameDescriptionLabel = New System.Windows.Forms.Label()
        Me.LayoutTable.SuspendLayout()
        Me.TeamTable.SuspendLayout()
        Me.GameGroup.SuspendLayout()
        Me.CommandsPanel.SuspendLayout()
        Me.SuspendLayout()
        '
        'PonyList
        '
        Me.PonyList.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.PonyList.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.PonyList.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PonyList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.PonyList.HideSelection = False
        Me.PonyList.Location = New System.Drawing.Point(189, 3)
        Me.PonyList.MultiSelect = False
        Me.PonyList.Name = "PonyList"
        Me.PonyList.ShowGroups = False
        Me.PonyList.Size = New System.Drawing.Size(564, 443)
        Me.PonyList.TabIndex = 2
        Me.PonyList.UseCompatibleStateImageBehavior = False
        '
        'GameList
        '
        Me.GameList.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.GameList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GameList.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.GameList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.GameList.HideSelection = False
        Me.GameList.Location = New System.Drawing.Point(6, 19)
        Me.GameList.MultiSelect = False
        Me.GameList.Name = "GameList"
        Me.GameList.ShowGroups = False
        Me.GameList.Size = New System.Drawing.Size(927, 101)
        Me.GameList.TabIndex = 0
        Me.GameList.UseCompatibleStateImageBehavior = False
        '
        'Team1AddButton
        '
        Me.Team1AddButton.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Team1AddButton.Location = New System.Drawing.Point(153, 30)
        Me.Team1AddButton.Margin = New System.Windows.Forms.Padding(3, 30, 3, 70)
        Me.Team1AddButton.Name = "Team1AddButton"
        Me.Team1AddButton.Size = New System.Drawing.Size(30, 349)
        Me.Team1AddButton.TabIndex = 1
        Me.Team1AddButton.Text = "<"
        Me.Team1AddButton.UseVisualStyleBackColor = True
        '
        'Team2AddButton
        '
        Me.Team2AddButton.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Team2AddButton.Location = New System.Drawing.Point(759, 30)
        Me.Team2AddButton.Margin = New System.Windows.Forms.Padding(3, 30, 3, 70)
        Me.Team2AddButton.Name = "Team2AddButton"
        Me.Team2AddButton.Size = New System.Drawing.Size(30, 349)
        Me.Team2AddButton.TabIndex = 3
        Me.Team2AddButton.Text = ">"
        Me.Team2AddButton.UseVisualStyleBackColor = True
        '
        'LayoutTable
        '
        Me.LayoutTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.LayoutTable.ColumnCount = 1
        Me.LayoutTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.LayoutTable.Controls.Add(Me.TeamTable, 0, 2)
        Me.LayoutTable.Controls.Add(Me.GameGroup, 0, 1)
        Me.LayoutTable.Controls.Add(Me.CommandsPanel, 0, 0)
        Me.LayoutTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutTable.Location = New System.Drawing.Point(0, 0)
        Me.LayoutTable.Name = "LayoutTable"
        Me.LayoutTable.RowCount = 3
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.LayoutTable.Size = New System.Drawing.Size(948, 690)
        Me.LayoutTable.TabIndex = 0
        '
        'TeamTable
        '
        Me.TeamTable.AutoSize = True
        Me.TeamTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TeamTable.ColumnCount = 5
        Me.TeamTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TeamTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TeamTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TeamTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TeamTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TeamTable.Controls.Add(Me.GameTeam2, 4, 0)
        Me.TeamTable.Controls.Add(Me.PonyList, 2, 0)
        Me.TeamTable.Controls.Add(Me.Team1AddButton, 1, 0)
        Me.TeamTable.Controls.Add(Me.Team2AddButton, 3, 0)
        Me.TeamTable.Controls.Add(Me.GameTeam1, 0, 0)
        Me.TeamTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TeamTable.Location = New System.Drawing.Point(3, 238)
        Me.TeamTable.Name = "TeamTable"
        Me.TeamTable.RowCount = 1
        Me.TeamTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TeamTable.Size = New System.Drawing.Size(942, 449)
        Me.TeamTable.TabIndex = 2
        '
        'GameGroup
        '
        Me.GameGroup.AutoSize = True
        Me.GameGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GameGroup.Controls.Add(Me.GameDescriptionLabel)
        Me.GameGroup.Controls.Add(Me.GameList)
        Me.GameGroup.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GameGroup.Location = New System.Drawing.Point(3, 45)
        Me.GameGroup.Name = "GameGroup"
        Me.GameGroup.Size = New System.Drawing.Size(942, 187)
        Me.GameGroup.TabIndex = 1
        Me.GameGroup.TabStop = False
        Me.GameGroup.Text = "Select Game"
        '
        'CommandsPanel
        '
        Me.CommandsPanel.AutoSize = True
        Me.CommandsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CommandsPanel.Controls.Add(Me.MonitorComboBox)
        Me.CommandsPanel.Controls.Add(Me.Transition3Label)
        Me.CommandsPanel.Controls.Add(Me.Transition2Label)
        Me.CommandsPanel.Controls.Add(Me.Transition1Label)
        Me.CommandsPanel.Controls.Add(Me.StartLabel)
        Me.CommandsPanel.Controls.Add(Me.SelectPlayersLabel)
        Me.CommandsPanel.Controls.Add(Me.SelectGameLabel)
        Me.CommandsPanel.Controls.Add(Me.MonitorLabel)
        Me.CommandsPanel.Controls.Add(Me.PlayButton)
        Me.CommandsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CommandsPanel.Location = New System.Drawing.Point(3, 3)
        Me.CommandsPanel.Name = "CommandsPanel"
        Me.CommandsPanel.Size = New System.Drawing.Size(942, 36)
        Me.CommandsPanel.TabIndex = 0
        '
        'Transition3Label
        '
        Me.Transition3Label.AutoSize = True
        Me.Transition3Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Transition3Label.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Transition3Label.Location = New System.Drawing.Point(467, 6)
        Me.Transition3Label.Name = "Transition3Label"
        Me.Transition3Label.Size = New System.Drawing.Size(22, 24)
        Me.Transition3Label.TabIndex = 5
        Me.Transition3Label.Text = ">"
        '
        'Transition2Label
        '
        Me.Transition2Label.AutoSize = True
        Me.Transition2Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Transition2Label.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Transition2Label.Location = New System.Drawing.Point(359, 6)
        Me.Transition2Label.Name = "Transition2Label"
        Me.Transition2Label.Size = New System.Drawing.Size(22, 24)
        Me.Transition2Label.TabIndex = 3
        Me.Transition2Label.Text = ">"
        '
        'Transition1Label
        '
        Me.Transition1Label.AutoSize = True
        Me.Transition1Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Transition1Label.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Transition1Label.Location = New System.Drawing.Point(160, 6)
        Me.Transition1Label.Name = "Transition1Label"
        Me.Transition1Label.Size = New System.Drawing.Size(22, 24)
        Me.Transition1Label.TabIndex = 1
        Me.Transition1Label.Text = ">"
        '
        'StartLabel
        '
        Me.StartLabel.AutoSize = True
        Me.StartLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StartLabel.ForeColor = System.Drawing.Color.RoyalBlue
        Me.StartLabel.Location = New System.Drawing.Point(387, 6)
        Me.StartLabel.Name = "StartLabel"
        Me.StartLabel.Size = New System.Drawing.Size(74, 24)
        Me.StartLabel.TabIndex = 4
        Me.StartLabel.Text = "3. Start"
        '
        'SelectPlayersLabel
        '
        Me.SelectPlayersLabel.AutoSize = True
        Me.SelectPlayersLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SelectPlayersLabel.ForeColor = System.Drawing.Color.RoyalBlue
        Me.SelectPlayersLabel.Location = New System.Drawing.Point(188, 6)
        Me.SelectPlayersLabel.Name = "SelectPlayersLabel"
        Me.SelectPlayersLabel.Size = New System.Drawing.Size(165, 24)
        Me.SelectPlayersLabel.TabIndex = 2
        Me.SelectPlayersLabel.Text = "2. Select Players"
        '
        'SelectGameLabel
        '
        Me.SelectGameLabel.AutoSize = True
        Me.SelectGameLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SelectGameLabel.ForeColor = System.Drawing.Color.RoyalBlue
        Me.SelectGameLabel.Location = New System.Drawing.Point(2, 6)
        Me.SelectGameLabel.Name = "SelectGameLabel"
        Me.SelectGameLabel.Size = New System.Drawing.Size(152, 24)
        Me.SelectGameLabel.TabIndex = 0
        Me.SelectGameLabel.Text = "1. Select Game"
        '
        'MonitorLabel
        '
        Me.MonitorLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MonitorLabel.AutoSize = True
        Me.MonitorLabel.Location = New System.Drawing.Point(735, 12)
        Me.MonitorLabel.Name = "MonitorLabel"
        Me.MonitorLabel.Size = New System.Drawing.Size(77, 13)
        Me.MonitorLabel.TabIndex = 8
        Me.MonitorLabel.Text = "Monitor to use:"
        '
        'PlayButton
        '
        Me.PlayButton.Location = New System.Drawing.Point(495, 3)
        Me.PlayButton.Name = "PlayButton"
        Me.PlayButton.Size = New System.Drawing.Size(100, 30)
        Me.PlayButton.TabIndex = 6
        Me.PlayButton.Text = "PLAY"
        Me.PlayButton.UseVisualStyleBackColor = True
        '
        'MonitorComboBox
        '
        Me.MonitorComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MonitorComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.MonitorComboBox.FormattingEnabled = True
        Me.MonitorComboBox.Location = New System.Drawing.Point(818, 9)
        Me.MonitorComboBox.Name = "MonitorComboBox"
        Me.MonitorComboBox.Size = New System.Drawing.Size(121, 21)
        Me.MonitorComboBox.TabIndex = 10
        '
        'GameTeam2
        '
        Me.GameTeam2.AutoSize = True
        Me.GameTeam2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GameTeam2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GameTeam2.Location = New System.Drawing.Point(795, 3)
        Me.GameTeam2.Name = "GameTeam2"
        Me.GameTeam2.Size = New System.Drawing.Size(144, 443)
        Me.GameTeam2.TabIndex = 4
        '
        'GameTeam1
        '
        Me.GameTeam1.AutoSize = True
        Me.GameTeam1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GameTeam1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GameTeam1.Location = New System.Drawing.Point(3, 3)
        Me.GameTeam1.Name = "GameTeam1"
        Me.GameTeam1.Size = New System.Drawing.Size(144, 443)
        Me.GameTeam1.TabIndex = 0
        '
        'GameDescriptionLabel
        '
        Me.GameDescriptionLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GameDescriptionLabel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GameDescriptionLabel.Location = New System.Drawing.Point(6, 123)
        Me.GameDescriptionLabel.Name = "GameDescriptionLabel"
        Me.GameDescriptionLabel.Size = New System.Drawing.Size(927, 48)
        Me.GameDescriptionLabel.TabIndex = 1
        '
        'GameSelectionForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(948, 690)
        Me.Controls.Add(Me.LayoutTable)
        Me.MinimumSize = New System.Drawing.Size(841, 400)
        Me.Name = "GameSelectionForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Game Setup - Desktop Ponies"
        Me.LayoutTable.ResumeLayout(False)
        Me.LayoutTable.PerformLayout()
        Me.TeamTable.ResumeLayout(False)
        Me.TeamTable.PerformLayout()
        Me.GameGroup.ResumeLayout(False)
        Me.CommandsPanel.ResumeLayout(False)
        Me.CommandsPanel.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PonyList As System.Windows.Forms.ListView
    Friend WithEvents GameList As System.Windows.Forms.ListView
    Friend WithEvents Team1AddButton As System.Windows.Forms.Button
    Friend WithEvents Team2AddButton As System.Windows.Forms.Button
    Friend WithEvents LayoutTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents TeamTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents GameGroup As System.Windows.Forms.GroupBox
    Friend WithEvents GameTeam2 As DesktopPonies.GameTeamControl
    Friend WithEvents GameTeam1 As DesktopPonies.GameTeamControl
    Friend WithEvents CommandsPanel As System.Windows.Forms.Panel
    Friend WithEvents MonitorLabel As System.Windows.Forms.Label
    Friend WithEvents PlayButton As System.Windows.Forms.Button
    Friend WithEvents Transition3Label As System.Windows.Forms.Label
    Friend WithEvents Transition2Label As System.Windows.Forms.Label
    Friend WithEvents Transition1Label As System.Windows.Forms.Label
    Friend WithEvents StartLabel As System.Windows.Forms.Label
    Friend WithEvents SelectPlayersLabel As System.Windows.Forms.Label
    Friend WithEvents SelectGameLabel As System.Windows.Forms.Label
    Friend WithEvents MonitorComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents GameDescriptionLabel As System.Windows.Forms.Label
End Class

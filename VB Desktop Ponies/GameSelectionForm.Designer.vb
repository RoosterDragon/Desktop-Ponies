<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GameSelectionForm
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
        Me.Pony_Selection_View = New System.Windows.Forms.ListView()
        Me.Game_Selection_View = New System.Windows.Forms.ListView()
        Me.Team1_Panel = New System.Windows.Forms.Panel()
        Me.Team2_Panel = New System.Windows.Forms.Panel()
        Me.Team1_Label = New System.Windows.Forms.Label()
        Me.Team2_Label = New System.Windows.Forms.Label()
        Me.Play_Button = New System.Windows.Forms.Button()
        Me.Add_Team1_Button = New System.Windows.Forms.Button()
        Me.Add_Team2_Button = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.team1_spotsleft_label = New System.Windows.Forms.Label()
        Me.team1_requiredleft_label = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.team2_requiredleft_label = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.team2_spotsleft_label = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Prompt_Label = New System.Windows.Forms.Label()
        Me.Info_Button = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.ScreenSelection_Box = New System.Windows.Forms.ListBox()
        Me.SuspendLayout()
        '
        'Pony_Selection_View
        '
        Me.Pony_Selection_View.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.Pony_Selection_View.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Pony_Selection_View.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.Pony_Selection_View.HideSelection = False
        Me.Pony_Selection_View.LabelWrap = False
        Me.Pony_Selection_View.Location = New System.Drawing.Point(540, 49)
        Me.Pony_Selection_View.MultiSelect = False
        Me.Pony_Selection_View.Name = "Pony_Selection_View"
        Me.Pony_Selection_View.ShowGroups = False
        Me.Pony_Selection_View.Size = New System.Drawing.Size(150, 426)
        Me.Pony_Selection_View.TabIndex = 3
        Me.Pony_Selection_View.UseCompatibleStateImageBehavior = False
        '
        'Game_Selection_View
        '
        Me.Game_Selection_View.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.Game_Selection_View.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.Game_Selection_View.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.Game_Selection_View.HideSelection = False
        Me.Game_Selection_View.LabelWrap = False
        Me.Game_Selection_View.Location = New System.Drawing.Point(44, 49)
        Me.Game_Selection_View.MultiSelect = False
        Me.Game_Selection_View.Name = "Game_Selection_View"
        Me.Game_Selection_View.ShowGroups = False
        Me.Game_Selection_View.Size = New System.Drawing.Size(192, 321)
        Me.Game_Selection_View.TabIndex = 4
        Me.Game_Selection_View.UseCompatibleStateImageBehavior = False
        '
        'Team1_Panel
        '
        Me.Team1_Panel.AutoScroll = True
        Me.Team1_Panel.Location = New System.Drawing.Point(291, 69)
        Me.Team1_Panel.Name = "Team1_Panel"
        Me.Team1_Panel.Size = New System.Drawing.Size(163, 374)
        Me.Team1_Panel.TabIndex = 5
        '
        'Team2_Panel
        '
        Me.Team2_Panel.AutoScroll = True
        Me.Team2_Panel.Location = New System.Drawing.Point(764, 69)
        Me.Team2_Panel.Name = "Team2_Panel"
        Me.Team2_Panel.Size = New System.Drawing.Size(164, 374)
        Me.Team2_Panel.TabIndex = 6
        '
        'Team1_Label
        '
        Me.Team1_Label.AutoSize = True
        Me.Team1_Label.Location = New System.Drawing.Point(294, 53)
        Me.Team1_Label.Name = "Team1_Label"
        Me.Team1_Label.Size = New System.Drawing.Size(43, 13)
        Me.Team1_Label.TabIndex = 7
        Me.Team1_Label.Text = "Team 1"
        '
        'Team2_Label
        '
        Me.Team2_Label.AutoSize = True
        Me.Team2_Label.Location = New System.Drawing.Point(773, 53)
        Me.Team2_Label.Name = "Team2_Label"
        Me.Team2_Label.Size = New System.Drawing.Size(43, 13)
        Me.Team2_Label.TabIndex = 8
        Me.Team2_Label.Text = "Team 2"
        '
        'Play_Button
        '
        Me.Play_Button.Location = New System.Drawing.Point(103, 391)
        Me.Play_Button.Name = "Play_Button"
        Me.Play_Button.Size = New System.Drawing.Size(75, 23)
        Me.Play_Button.TabIndex = 9
        Me.Play_Button.Text = "PLAY"
        Me.Play_Button.UseVisualStyleBackColor = True
        '
        'Add_Team1_Button
        '
        Me.Add_Team1_Button.Location = New System.Drawing.Point(481, 162)
        Me.Add_Team1_Button.Name = "Add_Team1_Button"
        Me.Add_Team1_Button.Size = New System.Drawing.Size(31, 208)
        Me.Add_Team1_Button.TabIndex = 10
        Me.Add_Team1_Button.Text = "<"
        Me.Add_Team1_Button.UseVisualStyleBackColor = True
        '
        'Add_Team2_Button
        '
        Me.Add_Team2_Button.Location = New System.Drawing.Point(712, 162)
        Me.Add_Team2_Button.Name = "Add_Team2_Button"
        Me.Add_Team2_Button.Size = New System.Drawing.Size(31, 208)
        Me.Add_Team2_Button.TabIndex = 11
        Me.Add_Team2_Button.Text = ">"
        Me.Add_Team2_Button.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(335, 462)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(54, 13)
        Me.Label1.TabIndex = 12
        Me.Label1.Text = "Spots left:"
        '
        'team1_spotsleft_label
        '
        Me.team1_spotsleft_label.AutoSize = True
        Me.team1_spotsleft_label.Location = New System.Drawing.Point(401, 462)
        Me.team1_spotsleft_label.Name = "team1_spotsleft_label"
        Me.team1_spotsleft_label.Size = New System.Drawing.Size(13, 13)
        Me.team1_spotsleft_label.TabIndex = 14
        Me.team1_spotsleft_label.Text = "0"
        '
        'team1_requiredleft_label
        '
        Me.team1_requiredleft_label.AutoSize = True
        Me.team1_requiredleft_label.Location = New System.Drawing.Point(401, 488)
        Me.team1_requiredleft_label.Name = "team1_requiredleft_label"
        Me.team1_requiredleft_label.Size = New System.Drawing.Size(13, 13)
        Me.team1_requiredleft_label.TabIndex = 18
        Me.team1_requiredleft_label.Text = "0"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(289, 488)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 13)
        Me.Label4.TabIndex = 17
        Me.Label4.Text = "Required Spots left:"
        '
        'team2_requiredleft_label
        '
        Me.team2_requiredleft_label.AutoSize = True
        Me.team2_requiredleft_label.Location = New System.Drawing.Point(885, 488)
        Me.team2_requiredleft_label.Name = "team2_requiredleft_label"
        Me.team2_requiredleft_label.Size = New System.Drawing.Size(13, 13)
        Me.team2_requiredleft_label.TabIndex = 22
        Me.team2_requiredleft_label.Text = "0"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(773, 488)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(100, 13)
        Me.Label5.TabIndex = 21
        Me.Label5.Text = "Required Spots left:"
        '
        'team2_spotsleft_label
        '
        Me.team2_spotsleft_label.AutoSize = True
        Me.team2_spotsleft_label.Location = New System.Drawing.Point(885, 462)
        Me.team2_spotsleft_label.Name = "team2_spotsleft_label"
        Me.team2_spotsleft_label.Size = New System.Drawing.Size(13, 13)
        Me.team2_spotsleft_label.TabIndex = 20
        Me.team2_spotsleft_label.Text = "0"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(819, 462)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(54, 13)
        Me.Label7.TabIndex = 19
        Me.Label7.Text = "Spots left:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(30, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(75, 13)
        Me.Label2.TabIndex = 23
        Me.Label2.Text = "Select game..."
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(314, 510)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(121, 13)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "(Click a pony to remove)"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(789, 510)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(121, 13)
        Me.Label6.TabIndex = 25
        Me.Label6.Text = "(Click a pony to remove)"
        '
        'Prompt_Label
        '
        Me.Prompt_Label.AutoSize = True
        Me.Prompt_Label.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Prompt_Label.ForeColor = System.Drawing.Color.RoyalBlue
        Me.Prompt_Label.Location = New System.Drawing.Point(115, 547)
        Me.Prompt_Label.Name = "Prompt_Label"
        Me.Prompt_Label.Size = New System.Drawing.Size(509, 37)
        Me.Prompt_Label.TabIndex = 26
        Me.Prompt_Label.Text = "Select the game you wish to play..."
        '
        'Info_Button
        '
        Me.Info_Button.Location = New System.Drawing.Point(103, 420)
        Me.Info_Button.Name = "Info_Button"
        Me.Info_Button.Size = New System.Drawing.Size(75, 23)
        Me.Info_Button.TabIndex = 27
        Me.Info_Button.Text = "INFO"
        Me.Info_Button.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(30, 446)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(77, 13)
        Me.Label8.TabIndex = 29
        Me.Label8.Text = "Monitor to use:"
        '
        'ScreenSelection_Box
        '
        Me.ScreenSelection_Box.FormattingEnabled = True
        Me.ScreenSelection_Box.Location = New System.Drawing.Point(74, 462)
        Me.ScreenSelection_Box.Name = "ScreenSelection_Box"
        Me.ScreenSelection_Box.Size = New System.Drawing.Size(135, 69)
        Me.ScreenSelection_Box.TabIndex = 28
        '
        'Game_Selection_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(954, 591)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.ScreenSelection_Box)
        Me.Controls.Add(Me.Info_Button)
        Me.Controls.Add(Me.Prompt_Label)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.team2_requiredleft_label)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.team2_spotsleft_label)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.team1_requiredleft_label)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.team1_spotsleft_label)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Add_Team2_Button)
        Me.Controls.Add(Me.Add_Team1_Button)
        Me.Controls.Add(Me.Play_Button)
        Me.Controls.Add(Me.Team2_Label)
        Me.Controls.Add(Me.Team1_Label)
        Me.Controls.Add(Me.Team2_Panel)
        Me.Controls.Add(Me.Team1_Panel)
        Me.Controls.Add(Me.Game_Selection_View)
        Me.Controls.Add(Me.Pony_Selection_View)
        Me.Name = "Game_Selection_Form"
        Me.Text = "Select game and players..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Pony_Selection_View As System.Windows.Forms.ListView
    Friend WithEvents Game_Selection_View As System.Windows.Forms.ListView
    Friend WithEvents Team1_Panel As System.Windows.Forms.Panel
    Friend WithEvents Team2_Panel As System.Windows.Forms.Panel
    Friend WithEvents Team1_Label As System.Windows.Forms.Label
    Friend WithEvents Team2_Label As System.Windows.Forms.Label
    Friend WithEvents Play_Button As System.Windows.Forms.Button
    Friend WithEvents Add_Team1_Button As System.Windows.Forms.Button
    Friend WithEvents Add_Team2_Button As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents team1_spotsleft_label As System.Windows.Forms.Label
    Friend WithEvents team1_requiredleft_label As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents team2_requiredleft_label As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents team2_spotsleft_label As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Prompt_Label As System.Windows.Forms.Label
    Friend WithEvents Info_Button As System.Windows.Forms.Button
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ScreenSelection_Box As System.Windows.Forms.ListBox
End Class

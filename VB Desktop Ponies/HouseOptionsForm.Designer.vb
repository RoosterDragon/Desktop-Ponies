<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class HouseOptionsForm
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
        Me.Visitors_CheckedListBox = New VbDesktopPonies.BufferedCheckedListBox()
        Me.Save_Button = New System.Windows.Forms.Button()
        Me.lvlVisitors = New System.Windows.Forms.Label()
        Me.Cycle_Counter = New System.Windows.Forms.NumericUpDown()
        Me.lblDoorLocation = New System.Windows.Forms.Label()
        Me.House_ImageBox = New System.Windows.Forms.PictureBox()
        Me.lblCycle = New System.Windows.Forms.Label()
        Me.DoorLocation_Label = New System.Windows.Forms.Label()
        Me.ClearVisitors_Button = New System.Windows.Forms.Button()
        Me.lblMinimumPonies = New System.Windows.Forms.Label()
        Me.MinSpawn_Counter = New System.Windows.Forms.NumericUpDown()
        Me.lblMaximumPonies = New System.Windows.Forms.Label()
        Me.MaxSpawn_Counter = New System.Windows.Forms.NumericUpDown()
        Me.Bias_TrackBar = New System.Windows.Forms.TrackBar()
        Me.lblBiasGuide = New System.Windows.Forms.Label()
        Me.lblBias = New System.Windows.Forms.Label()
        Me.Close_Button = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Visitors_CheckedListBox
        '
        Me.Visitors_CheckedListBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Visitors_CheckedListBox.FormattingEnabled = True
        Me.Visitors_CheckedListBox.Location = New System.Drawing.Point(12, 185)
        Me.Visitors_CheckedListBox.Name = "Visitors_CheckedListBox"
        Me.Visitors_CheckedListBox.Size = New System.Drawing.Size(194, 169)
        Me.Visitors_CheckedListBox.TabIndex = 10
        '
        'Save_Button
        '
        Me.Save_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Save_Button.Location = New System.Drawing.Point(356, 361)
        Me.Save_Button.Name = "Save_Button"
        Me.Save_Button.Size = New System.Drawing.Size(75, 23)
        Me.Save_Button.TabIndex = 14
        Me.Save_Button.Text = "Save"
        '
        'lvlVisitors
        '
        Me.lvlVisitors.AutoSize = True
        Me.lvlVisitors.Location = New System.Drawing.Point(12, 169)
        Me.lvlVisitors.Name = "lvlVisitors"
        Me.lvlVisitors.Size = New System.Drawing.Size(119, 13)
        Me.lvlVisitors.TabIndex = 9
        Me.lvlVisitors.Text = "Occupants and Visitors:"
        '
        'Cycle_Counter
        '
        Me.Cycle_Counter.Location = New System.Drawing.Point(156, 12)
        Me.Cycle_Counter.Maximum = New Decimal(New Integer() {3600, 0, 0, 0})
        Me.Cycle_Counter.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.Cycle_Counter.Name = "Cycle_Counter"
        Me.Cycle_Counter.Size = New System.Drawing.Size(50, 20)
        Me.Cycle_Counter.TabIndex = 1
        Me.Cycle_Counter.Value = New Decimal(New Integer() {300, 0, 0, 0})
        '
        'lblDoorLocation
        '
        Me.lblDoorLocation.AutoSize = True
        Me.lblDoorLocation.Location = New System.Drawing.Point(212, 14)
        Me.lblDoorLocation.Name = "lblDoorLocation"
        Me.lblDoorLocation.Size = New System.Drawing.Size(140, 13)
        Me.lblDoorLocation.TabIndex = 12
        Me.lblDoorLocation.Text = "Door Location (Click to Set):"
        '
        'House_ImageBox
        '
        Me.House_ImageBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.House_ImageBox.BackColor = System.Drawing.Color.Black
        Me.House_ImageBox.Location = New System.Drawing.Point(212, 30)
        Me.House_ImageBox.Name = "House_ImageBox"
        Me.House_ImageBox.Size = New System.Drawing.Size(300, 300)
        Me.House_ImageBox.TabIndex = 17
        Me.House_ImageBox.TabStop = False
        '
        'lblCycle
        '
        Me.lblCycle.AutoSize = True
        Me.lblCycle.Location = New System.Drawing.Point(39, 14)
        Me.lblCycle.Name = "lblCycle"
        Me.lblCycle.Size = New System.Drawing.Size(111, 13)
        Me.lblCycle.TabIndex = 0
        Me.lblCycle.Text = "Cycle Time (seconds):"
        '
        'DoorLocation_Label
        '
        Me.DoorLocation_Label.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DoorLocation_Label.Location = New System.Drawing.Point(212, 333)
        Me.DoorLocation_Label.Name = "DoorLocation_Label"
        Me.DoorLocation_Label.Size = New System.Drawing.Size(300, 13)
        Me.DoorLocation_Label.TabIndex = 13
        Me.DoorLocation_Label.Text = "{X=x,Y=y}"
        Me.DoorLocation_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'ClearVisitors_Button
        '
        Me.ClearVisitors_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.ClearVisitors_Button.Location = New System.Drawing.Point(11, 360)
        Me.ClearVisitors_Button.Name = "ClearVisitors_Button"
        Me.ClearVisitors_Button.Size = New System.Drawing.Size(194, 23)
        Me.ClearVisitors_Button.TabIndex = 11
        Me.ClearVisitors_Button.TabStop = False
        Me.ClearVisitors_Button.Text = "Clear/Select All"
        '
        'lblMinimumPonies
        '
        Me.lblMinimumPonies.AutoSize = True
        Me.lblMinimumPonies.Location = New System.Drawing.Point(12, 40)
        Me.lblMinimumPonies.Name = "lblMinimumPonies"
        Me.lblMinimumPonies.Size = New System.Drawing.Size(138, 13)
        Me.lblMinimumPonies.TabIndex = 2
        Me.lblMinimumPonies.Text = "Minimum Ponies To Spawn:"
        '
        'MinSpawn_Counter
        '
        Me.MinSpawn_Counter.Location = New System.Drawing.Point(156, 38)
        Me.MinSpawn_Counter.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.MinSpawn_Counter.Name = "MinSpawn_Counter"
        Me.MinSpawn_Counter.Size = New System.Drawing.Size(50, 20)
        Me.MinSpawn_Counter.TabIndex = 3
        Me.MinSpawn_Counter.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'lblMaximumPonies
        '
        Me.lblMaximumPonies.AutoSize = True
        Me.lblMaximumPonies.Location = New System.Drawing.Point(9, 66)
        Me.lblMaximumPonies.Name = "lblMaximumPonies"
        Me.lblMaximumPonies.Size = New System.Drawing.Size(141, 13)
        Me.lblMaximumPonies.TabIndex = 4
        Me.lblMaximumPonies.Text = "Maximum Ponies To Spawn:"
        '
        'MaxSpawn_Counter
        '
        Me.MaxSpawn_Counter.Location = New System.Drawing.Point(156, 64)
        Me.MaxSpawn_Counter.Maximum = New Decimal(New Integer() {9999, 0, 0, 0})
        Me.MaxSpawn_Counter.Minimum = New Decimal(New Integer() {5, 0, 0, 0})
        Me.MaxSpawn_Counter.Name = "MaxSpawn_Counter"
        Me.MaxSpawn_Counter.Size = New System.Drawing.Size(50, 20)
        Me.MaxSpawn_Counter.TabIndex = 5
        Me.MaxSpawn_Counter.Value = New Decimal(New Integer() {50, 0, 0, 0})
        '
        'Bias_TrackBar
        '
        Me.Bias_TrackBar.Location = New System.Drawing.Point(12, 108)
        Me.Bias_TrackBar.Maximum = 9
        Me.Bias_TrackBar.Minimum = 1
        Me.Bias_TrackBar.Name = "Bias_TrackBar"
        Me.Bias_TrackBar.Size = New System.Drawing.Size(194, 45)
        Me.Bias_TrackBar.TabIndex = 7
        Me.Bias_TrackBar.Value = 5
        '
        'lblBiasGuide
        '
        Me.lblBiasGuide.Location = New System.Drawing.Point(12, 140)
        Me.lblBiasGuide.Name = "lblBiasGuide"
        Me.lblBiasGuide.Size = New System.Drawing.Size(194, 13)
        Me.lblBiasGuide.TabIndex = 8
        Me.lblBiasGuide.Text = "<--- Less Ponies - More Ponies --->" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        Me.lblBiasGuide.TextAlign = System.Drawing.ContentAlignment.BottomCenter
        '
        'lblBias
        '
        Me.lblBias.AutoSize = True
        Me.lblBias.Location = New System.Drawing.Point(12, 92)
        Me.lblBias.Name = "lblBias"
        Me.lblBias.Size = New System.Drawing.Size(84, 13)
        Me.lblBias.TabIndex = 6
        Me.lblBias.Text = "Bias adjustment:"
        '
        'Close_Button
        '
        Me.Close_Button.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Close_Button.Location = New System.Drawing.Point(437, 361)
        Me.Close_Button.Name = "Close_Button"
        Me.Close_Button.Size = New System.Drawing.Size(75, 23)
        Me.Close_Button.TabIndex = 15
        Me.Close_Button.Text = "Close"
        '
        'HouseOptionsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(524, 396)
        Me.Controls.Add(Me.Close_Button)
        Me.Controls.Add(Me.lblBias)
        Me.Controls.Add(Me.lblBiasGuide)
        Me.Controls.Add(Me.Bias_TrackBar)
        Me.Controls.Add(Me.lblMaximumPonies)
        Me.Controls.Add(Me.MaxSpawn_Counter)
        Me.Controls.Add(Me.lblMinimumPonies)
        Me.Controls.Add(Me.MinSpawn_Counter)
        Me.Controls.Add(Me.ClearVisitors_Button)
        Me.Controls.Add(Me.DoorLocation_Label)
        Me.Controls.Add(Me.lblCycle)
        Me.Controls.Add(Me.lblDoorLocation)
        Me.Controls.Add(Me.House_ImageBox)
        Me.Controls.Add(Me.Cycle_Counter)
        Me.Controls.Add(Me.lvlVisitors)
        Me.Controls.Add(Me.Save_Button)
        Me.Controls.Add(Me.Visitors_CheckedListBox)
        Me.MinimumSize = New System.Drawing.Size(540, 434)
        Me.Name = "HouseOptionsForm"
        Me.Text = "House Options - Desktop Ponies"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Visitors_CheckedListBox As VbDesktopPonies.BufferedCheckedListBox
    Friend WithEvents Save_Button As System.Windows.Forms.Button
    Friend WithEvents lvlVisitors As System.Windows.Forms.Label
    Friend WithEvents Cycle_Counter As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblDoorLocation As System.Windows.Forms.Label
    Friend WithEvents House_ImageBox As System.Windows.Forms.PictureBox
    Friend WithEvents lblCycle As System.Windows.Forms.Label
    Friend WithEvents DoorLocation_Label As System.Windows.Forms.Label
    Friend WithEvents ClearVisitors_Button As System.Windows.Forms.Button
    Friend WithEvents lblMinimumPonies As System.Windows.Forms.Label
    Friend WithEvents MinSpawn_Counter As System.Windows.Forms.NumericUpDown
    Friend WithEvents lblMaximumPonies As System.Windows.Forms.Label
    Friend WithEvents MaxSpawn_Counter As System.Windows.Forms.NumericUpDown
    Friend WithEvents Bias_TrackBar As System.Windows.Forms.TrackBar
    Friend WithEvents lblBiasGuide As System.Windows.Forms.Label
    Friend WithEvents lblBias As System.Windows.Forms.Label
    Friend WithEvents Close_Button As System.Windows.Forms.Button
End Class

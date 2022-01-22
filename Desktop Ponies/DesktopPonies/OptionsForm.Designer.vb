<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsForm
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
        Me.MonitorsSelection = New System.Windows.Forms.ListBox()
        Me.CursorAwareness = New System.Windows.Forms.CheckBox()
        Me.CursorAvoidanceRadius = New System.Windows.Forms.NumericUpDown()
        Me.CursorAvoidanceRadiusLabel = New System.Windows.Forms.Label()
        Me.PonyDragging = New System.Windows.Forms.CheckBox()
        Me.PonySpeechChance = New System.Windows.Forms.NumericUpDown()
        Me.PonySpeechChanceLabel = New System.Windows.Forms.Label()
        Me.MaxPonies = New System.Windows.Forms.NumericUpDown()
        Me.MaxPoniesLabel = New System.Windows.Forms.Label()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.LoadButton = New System.Windows.Forms.Button()
        Me.ResetButton = New System.Windows.Forms.Button()
        Me.Effects = New System.Windows.Forms.CheckBox()
        Me.Sound = New System.Windows.Forms.CheckBox()
        Me.Interactions = New System.Windows.Forms.CheckBox()
        Me.WindowAvoidance = New System.Windows.Forms.CheckBox()
        Me.Speech = New System.Windows.Forms.CheckBox()
        Me.PoniesAvoidPonies = New System.Windows.Forms.CheckBox()
        Me.WindowContainment = New System.Windows.Forms.CheckBox()
        Me.SizeScale = New System.Windows.Forms.TrackBar()
        Me.SizeScaleLabel = New System.Windows.Forms.Label()
        Me.SizeScaleValueLabel = New System.Windows.Forms.Label()
        Me.CustomFiltersButton = New System.Windows.Forms.Button()
        Me.ScreensaverSounds = New System.Windows.Forms.CheckBox()
        Me.SoundLimitOneGlobally = New System.Windows.Forms.RadioButton()
        Me.SoundLimitOnePerPony = New System.Windows.Forms.RadioButton()
        Me.ScreensaverGroup = New System.Windows.Forms.GroupBox()
        Me.ScreensaverBackgroundTable = New System.Windows.Forms.TableLayoutPanel()
        Me.ScreensaverColorButton = New System.Windows.Forms.Button()
        Me.ScreensaverImageNeededLabel = New System.Windows.Forms.Label()
        Me.ScreensaverImageButton = New System.Windows.Forms.Button()
        Me.ScreensaverTransparent = New System.Windows.Forms.RadioButton()
        Me.ScreensaverColorNeededLabel = New System.Windows.Forms.Label()
        Me.ScreensaverColor = New System.Windows.Forms.RadioButton()
        Me.ScreensaverImage = New System.Windows.Forms.RadioButton()
        Me.ScreensaverBackgroundLabel = New System.Windows.Forms.Label()
        Me.AlwaysOnTop = New System.Windows.Forms.CheckBox()
        Me.Volume = New System.Windows.Forms.TrackBar()
        Me.VolumeLabel = New System.Windows.Forms.Label()
        Me.VolumeValueLabel = New System.Windows.Forms.Label()
        Me.TimeScaleValueLabel = New System.Windows.Forms.Label()
        Me.TimeScaleLabel = New System.Windows.Forms.Label()
        Me.TimeScale = New System.Windows.Forms.TrackBar()
        Me.WindowsGroup = New System.Windows.Forms.GroupBox()
        Me.ShowViewerInTaskbar = New System.Windows.Forms.CheckBox()
        Me.ScreenGroup = New System.Windows.Forms.GroupBox()
        Me.BackgroundColorButton = New System.Windows.Forms.Button()
        Me.OutOfBoundsLabel = New System.Windows.Forms.Label()
        Me.OutOfBoundsWalk = New System.Windows.Forms.RadioButton()
        Me.OutOfBoundsTeleport = New System.Windows.Forms.RadioButton()
        Me.ScreenCoveragePreviewLabel = New System.Windows.Forms.Label()
        Me.ScreenCoveragePreview = New System.Windows.Forms.Panel()
        Me.ScreenCoveragePanel = New System.Windows.Forms.Panel()
        Me.AvoidanceAreaTable = New System.Windows.Forms.TableLayoutPanel()
        Me.AvoidanceAreaHeightLabel = New System.Windows.Forms.Label()
        Me.ExclusionAreaHeight = New System.Windows.Forms.NumericUpDown()
        Me.AvoidanceAreaWidthLabel = New System.Windows.Forms.Label()
        Me.ExclusionAreaWidth = New System.Windows.Forms.NumericUpDown()
        Me.AvoidanceAreaYLabel = New System.Windows.Forms.Label()
        Me.AvoidanceAreaXLabel = New System.Windows.Forms.Label()
        Me.ExculsionAreaX = New System.Windows.Forms.NumericUpDown()
        Me.ExclusionAreaY = New System.Windows.Forms.NumericUpDown()
        Me.ScreenAreaTable = New System.Windows.Forms.TableLayoutPanel()
        Me.ScreenAreaHeightLabel = New System.Windows.Forms.Label()
        Me.ScreenAreaHeight = New System.Windows.Forms.NumericUpDown()
        Me.ScreenAreaWidthLabel = New System.Windows.Forms.Label()
        Me.ScreenAreaWidth = New System.Windows.Forms.NumericUpDown()
        Me.ScreenAreaYLabel = New System.Windows.Forms.Label()
        Me.ScreenAreaXLabel = New System.Windows.Forms.Label()
        Me.ScreenAreaX = New System.Windows.Forms.NumericUpDown()
        Me.ScreenAreaY = New System.Windows.Forms.NumericUpDown()
        Me.ScreenExclusion = New System.Windows.Forms.CheckBox()
        Me.ScreenCoverageArea = New System.Windows.Forms.RadioButton()
        Me.ScreenCoverageMonitors = New System.Windows.Forms.RadioButton()
        Me.ScreenLabel = New System.Windows.Forms.Label()
        Me.SoundGroup = New System.Windows.Forms.GroupBox()
        Me.MonitoringGroup = New System.Windows.Forms.GroupBox()
        Me.ShowPerformanceGraph = New System.Windows.Forms.CheckBox()
        Me.EnablePonyLogs = New System.Windows.Forms.CheckBox()
        Me.PoniesGroup = New System.Windows.Forms.GroupBox()
        Me.TransparentBackground = New System.Windows.Forms.CheckBox()
        Me.ScreensaverGroup.SuspendLayout()
        Me.ScreensaverBackgroundTable.SuspendLayout()
        Me.WindowsGroup.SuspendLayout()
        Me.ScreenGroup.SuspendLayout()
        Me.ScreenCoveragePanel.SuspendLayout()
        Me.AvoidanceAreaTable.SuspendLayout()
        Me.ScreenAreaTable.SuspendLayout()
        Me.SoundGroup.SuspendLayout()
        Me.MonitoringGroup.SuspendLayout()
        Me.PoniesGroup.SuspendLayout()
        Me.SuspendLayout()
        '
        'MonitorsSelection
        '
        Me.MonitorsSelection.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MonitorsSelection.DisplayMember = "DeviceName"
        Me.MonitorsSelection.Enabled = False
        Me.MonitorsSelection.FormattingEnabled = True
        Me.MonitorsSelection.Location = New System.Drawing.Point(3, 26)
        Me.MonitorsSelection.Name = "MonitorsSelection"
        Me.MonitorsSelection.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.MonitorsSelection.Size = New System.Drawing.Size(236, 43)
        Me.MonitorsSelection.TabIndex = 1
        '
        'CursorAwareness
        '
        Me.CursorAwareness.AutoSize = True
        Me.CursorAwareness.Location = New System.Drawing.Point(6, 130)
        Me.CursorAwareness.Name = "CursorAwareness"
        Me.CursorAwareness.Size = New System.Drawing.Size(245, 17)
        Me.CursorAwareness.TabIndex = 6
        Me.CursorAwareness.Text = "Ponies avoid cursor / stop when hovered over"
        Me.CursorAwareness.UseVisualStyleBackColor = True
        '
        'CursorAvoidanceRadius
        '
        Me.CursorAvoidanceRadius.Enabled = False
        Me.CursorAvoidanceRadius.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        Me.CursorAvoidanceRadius.Location = New System.Drawing.Point(184, 151)
        Me.CursorAvoidanceRadius.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.CursorAvoidanceRadius.Name = "CursorAvoidanceRadius"
        Me.CursorAvoidanceRadius.Size = New System.Drawing.Size(66, 20)
        Me.CursorAvoidanceRadius.TabIndex = 8
        '
        'CursorAvoidanceRadiusLabel
        '
        Me.CursorAvoidanceRadiusLabel.AutoSize = True
        Me.CursorAvoidanceRadiusLabel.Enabled = False
        Me.CursorAvoidanceRadiusLabel.Location = New System.Drawing.Point(6, 153)
        Me.CursorAvoidanceRadiusLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.CursorAvoidanceRadiusLabel.Name = "CursorAvoidanceRadiusLabel"
        Me.CursorAvoidanceRadiusLabel.Size = New System.Drawing.Size(172, 13)
        Me.CursorAvoidanceRadiusLabel.TabIndex = 7
        Me.CursorAvoidanceRadiusLabel.Text = "Radius around cursor to avoid (px):"
        '
        'PonyDragging
        '
        Me.PonyDragging.AutoSize = True
        Me.PonyDragging.Location = New System.Drawing.Point(6, 107)
        Me.PonyDragging.Name = "PonyDragging"
        Me.PonyDragging.Size = New System.Drawing.Size(105, 17)
        Me.PonyDragging.TabIndex = 5
        Me.PonyDragging.Text = "Enable Dragging"
        Me.PonyDragging.UseVisualStyleBackColor = True
        '
        'PonySpeechChance
        '
        Me.PonySpeechChance.Enabled = False
        Me.PonySpeechChance.Location = New System.Drawing.Point(159, 63)
        Me.PonySpeechChance.Name = "PonySpeechChance"
        Me.PonySpeechChance.Size = New System.Drawing.Size(50, 20)
        Me.PonySpeechChance.TabIndex = 3
        '
        'PonySpeechChanceLabel
        '
        Me.PonySpeechChanceLabel.AutoSize = True
        Me.PonySpeechChanceLabel.Enabled = False
        Me.PonySpeechChanceLabel.Location = New System.Drawing.Point(6, 65)
        Me.PonySpeechChanceLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.PonySpeechChanceLabel.Name = "PonySpeechChanceLabel"
        Me.PonySpeechChanceLabel.Size = New System.Drawing.Size(147, 13)
        Me.PonySpeechChanceLabel.TabIndex = 2
        Me.PonySpeechChanceLabel.Text = "Random Speech Chance (%):"
        '
        'MaxPonies
        '
        Me.MaxPonies.Increment = New Decimal(New Integer() {10, 0, 0, 0})
        Me.MaxPonies.Location = New System.Drawing.Point(129, 295)
        Me.MaxPonies.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.MaxPonies.Minimum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.MaxPonies.Name = "MaxPonies"
        Me.MaxPonies.Size = New System.Drawing.Size(60, 20)
        Me.MaxPonies.TabIndex = 17
        Me.MaxPonies.Value = New Decimal(New Integer() {10, 0, 0, 0})
        '
        'MaxPoniesLabel
        '
        Me.MaxPoniesLabel.AutoSize = True
        Me.MaxPoniesLabel.Location = New System.Drawing.Point(6, 297)
        Me.MaxPoniesLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.MaxPoniesLabel.Name = "MaxPoniesLabel"
        Me.MaxPoniesLabel.Size = New System.Drawing.Size(117, 13)
        Me.MaxPoniesLabel.TabIndex = 16
        Me.MaxPoniesLabel.Text = "Max Number of Ponies:"
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(655, 467)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(103, 23)
        Me.SaveButton.TabIndex = 9
        Me.SaveButton.Text = "SAVE"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'LoadButton
        '
        Me.LoadButton.Location = New System.Drawing.Point(656, 438)
        Me.LoadButton.Name = "LoadButton"
        Me.LoadButton.Size = New System.Drawing.Size(103, 23)
        Me.LoadButton.TabIndex = 8
        Me.LoadButton.Text = "LOAD"
        Me.LoadButton.UseVisualStyleBackColor = True
        '
        'ResetButton
        '
        Me.ResetButton.Location = New System.Drawing.Point(547, 438)
        Me.ResetButton.Name = "ResetButton"
        Me.ResetButton.Size = New System.Drawing.Size(103, 23)
        Me.ResetButton.TabIndex = 6
        Me.ResetButton.Text = "RESET"
        Me.ResetButton.UseVisualStyleBackColor = True
        '
        'Effects
        '
        Me.Effects.AutoSize = True
        Me.Effects.Location = New System.Drawing.Point(6, 19)
        Me.Effects.Name = "Effects"
        Me.Effects.Size = New System.Drawing.Size(95, 17)
        Me.Effects.TabIndex = 0
        Me.Effects.Text = "Enable Effects"
        Me.Effects.UseVisualStyleBackColor = True
        '
        'Sound
        '
        Me.Sound.AutoSize = True
        Me.Sound.Location = New System.Drawing.Point(6, 19)
        Me.Sound.Name = "Sound"
        Me.Sound.Size = New System.Drawing.Size(98, 17)
        Me.Sound.TabIndex = 0
        Me.Sound.Text = "Enable Sounds"
        Me.Sound.UseVisualStyleBackColor = True
        '
        'Interactions
        '
        Me.Interactions.AutoSize = True
        Me.Interactions.Location = New System.Drawing.Point(6, 84)
        Me.Interactions.Name = "Interactions"
        Me.Interactions.Size = New System.Drawing.Size(117, 17)
        Me.Interactions.TabIndex = 4
        Me.Interactions.Text = "Enable Interactions"
        Me.Interactions.UseVisualStyleBackColor = True
        '
        'WindowAvoidance
        '
        Me.WindowAvoidance.AutoSize = True
        Me.WindowAvoidance.Location = New System.Drawing.Point(6, 65)
        Me.WindowAvoidance.Name = "WindowAvoidance"
        Me.WindowAvoidance.Size = New System.Drawing.Size(184, 17)
        Me.WindowAvoidance.TabIndex = 2
        Me.WindowAvoidance.Text = "Ponies try to avoid other windows"
        Me.WindowAvoidance.UseVisualStyleBackColor = True
        '
        'Speech
        '
        Me.Speech.AutoSize = True
        Me.Speech.Location = New System.Drawing.Point(6, 42)
        Me.Speech.Name = "Speech"
        Me.Speech.Size = New System.Drawing.Size(99, 17)
        Me.Speech.TabIndex = 1
        Me.Speech.Text = "Enable Speech"
        Me.Speech.UseVisualStyleBackColor = True
        '
        'PoniesAvoidPonies
        '
        Me.PoniesAvoidPonies.AutoSize = True
        Me.PoniesAvoidPonies.Location = New System.Drawing.Point(6, 172)
        Me.PoniesAvoidPonies.Name = "PoniesAvoidPonies"
        Me.PoniesAvoidPonies.Size = New System.Drawing.Size(174, 17)
        Me.PoniesAvoidPonies.TabIndex = 9
        Me.PoniesAvoidPonies.Text = "Ponies try to avoid other ponies"
        Me.PoniesAvoidPonies.UseVisualStyleBackColor = True
        '
        'WindowContainment
        '
        Me.WindowContainment.AutoSize = True
        Me.WindowContainment.Location = New System.Drawing.Point(6, 88)
        Me.WindowContainment.Name = "WindowContainment"
        Me.WindowContainment.Size = New System.Drawing.Size(228, 17)
        Me.WindowContainment.TabIndex = 3
        Me.WindowContainment.Text = "Ponies don't leave windows they are inside"
        Me.WindowContainment.UseVisualStyleBackColor = True
        '
        'SizeScale
        '
        Me.SizeScale.LargeChange = 25
        Me.SizeScale.Location = New System.Drawing.Point(6, 214)
        Me.SizeScale.Maximum = 80
        Me.SizeScale.Minimum = 5
        Me.SizeScale.Name = "SizeScale"
        Me.SizeScale.Size = New System.Drawing.Size(244, 45)
        Me.SizeScale.TabIndex = 12
        Me.SizeScale.TickFrequency = 5
        Me.SizeScale.Value = 5
        '
        'SizeScaleLabel
        '
        Me.SizeScaleLabel.AutoSize = True
        Me.SizeScaleLabel.Location = New System.Drawing.Point(6, 195)
        Me.SizeScaleLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.SizeScaleLabel.Name = "SizeScaleLabel"
        Me.SizeScaleLabel.Size = New System.Drawing.Size(62, 13)
        Me.SizeScaleLabel.TabIndex = 10
        Me.SizeScaleLabel.Text = "Pony Sizes:"
        '
        'SizeScaleValueLabel
        '
        Me.SizeScaleValueLabel.AutoSize = True
        Me.SizeScaleValueLabel.Location = New System.Drawing.Point(74, 195)
        Me.SizeScaleValueLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.SizeScaleValueLabel.Name = "SizeScaleValueLabel"
        Me.SizeScaleValueLabel.Size = New System.Drawing.Size(18, 13)
        Me.SizeScaleValueLabel.TabIndex = 11
        Me.SizeScaleValueLabel.Text = "0x"
        '
        'CustomFiltersButton
        '
        Me.CustomFiltersButton.Location = New System.Drawing.Point(547, 466)
        Me.CustomFiltersButton.Name = "CustomFiltersButton"
        Me.CustomFiltersButton.Size = New System.Drawing.Size(103, 23)
        Me.CustomFiltersButton.TabIndex = 7
        Me.CustomFiltersButton.Text = "Custom Filters"
        Me.CustomFiltersButton.UseVisualStyleBackColor = True
        '
        'ScreensaverSounds
        '
        Me.ScreensaverSounds.AutoSize = True
        Me.ScreensaverSounds.Location = New System.Drawing.Point(6, 19)
        Me.ScreensaverSounds.Name = "ScreensaverSounds"
        Me.ScreensaverSounds.Size = New System.Drawing.Size(201, 17)
        Me.ScreensaverSounds.TabIndex = 0
        Me.ScreensaverSounds.Text = "Enable Sounds in Screensaver mode"
        Me.ScreensaverSounds.UseVisualStyleBackColor = True
        '
        'SoundLimitOneGlobally
        '
        Me.SoundLimitOneGlobally.AutoSize = True
        Me.SoundLimitOneGlobally.Enabled = False
        Me.SoundLimitOneGlobally.Location = New System.Drawing.Point(6, 65)
        Me.SoundLimitOneGlobally.Name = "SoundLimitOneGlobally"
        Me.SoundLimitOneGlobally.Size = New System.Drawing.Size(159, 17)
        Me.SoundLimitOneGlobally.TabIndex = 2
        Me.SoundLimitOneGlobally.Text = "Limit sounds to one at a time"
        Me.SoundLimitOneGlobally.UseVisualStyleBackColor = True
        '
        'SoundLimitOnePerPony
        '
        Me.SoundLimitOnePerPony.AutoSize = True
        Me.SoundLimitOnePerPony.Checked = True
        Me.SoundLimitOnePerPony.Enabled = False
        Me.SoundLimitOnePerPony.Location = New System.Drawing.Point(6, 42)
        Me.SoundLimitOnePerPony.Name = "SoundLimitOnePerPony"
        Me.SoundLimitOnePerPony.Size = New System.Drawing.Size(160, 17)
        Me.SoundLimitOnePerPony.TabIndex = 1
        Me.SoundLimitOnePerPony.TabStop = True
        Me.SoundLimitOnePerPony.Text = "Limit sounds to one per pony"
        Me.SoundLimitOnePerPony.UseVisualStyleBackColor = True
        '
        'ScreensaverGroup
        '
        Me.ScreensaverGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ScreensaverGroup.Controls.Add(Me.ScreensaverBackgroundTable)
        Me.ScreensaverGroup.Controls.Add(Me.ScreensaverBackgroundLabel)
        Me.ScreensaverGroup.Controls.Add(Me.ScreensaverSounds)
        Me.ScreensaverGroup.Location = New System.Drawing.Point(534, 263)
        Me.ScreensaverGroup.Name = "ScreensaverGroup"
        Me.ScreensaverGroup.Size = New System.Drawing.Size(236, 150)
        Me.ScreensaverGroup.TabIndex = 5
        Me.ScreensaverGroup.TabStop = False
        Me.ScreensaverGroup.Text = "Screensaver"
        '
        'ScreensaverBackgroundTable
        '
        Me.ScreensaverBackgroundTable.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScreensaverBackgroundTable.AutoSize = True
        Me.ScreensaverBackgroundTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ScreensaverBackgroundTable.ColumnCount = 2
        Me.ScreensaverBackgroundTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.ScreensaverBackgroundTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.ScreensaverBackgroundTable.Controls.Add(Me.ScreensaverColorButton, 1, 1)
        Me.ScreensaverBackgroundTable.Controls.Add(Me.ScreensaverImageNeededLabel, 0, 4)
        Me.ScreensaverBackgroundTable.Controls.Add(Me.ScreensaverImageButton, 1, 3)
        Me.ScreensaverBackgroundTable.Controls.Add(Me.ScreensaverTransparent, 0, 0)
        Me.ScreensaverBackgroundTable.Controls.Add(Me.ScreensaverColorNeededLabel, 0, 2)
        Me.ScreensaverBackgroundTable.Controls.Add(Me.ScreensaverColor, 0, 1)
        Me.ScreensaverBackgroundTable.Controls.Add(Me.ScreensaverImage, 0, 3)
        Me.ScreensaverBackgroundTable.Location = New System.Drawing.Point(6, 55)
        Me.ScreensaverBackgroundTable.Name = "ScreensaverBackgroundTable"
        Me.ScreensaverBackgroundTable.RowCount = 5
        Me.ScreensaverBackgroundTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.ScreensaverBackgroundTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.ScreensaverBackgroundTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.ScreensaverBackgroundTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.ScreensaverBackgroundTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.ScreensaverBackgroundTable.Size = New System.Drawing.Size(217, 107)
        Me.ScreensaverBackgroundTable.TabIndex = 2
        '
        'ScreensaverColorButton
        '
        Me.ScreensaverColorButton.Location = New System.Drawing.Point(126, 26)
        Me.ScreensaverColorButton.Name = "ScreensaverColorButton"
        Me.ScreensaverColorButton.Size = New System.Drawing.Size(88, 23)
        Me.ScreensaverColorButton.TabIndex = 2
        Me.ScreensaverColorButton.Text = "Select Color"
        Me.ScreensaverColorButton.UseVisualStyleBackColor = True
        '
        'ScreensaverImageNeededLabel
        '
        Me.ScreensaverImageNeededLabel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScreensaverImageNeededLabel.AutoSize = True
        Me.ScreensaverBackgroundTable.SetColumnSpan(Me.ScreensaverImageNeededLabel, 2)
        Me.ScreensaverImageNeededLabel.ForeColor = System.Drawing.Color.DarkRed
        Me.ScreensaverImageNeededLabel.Location = New System.Drawing.Point(3, 94)
        Me.ScreensaverImageNeededLabel.Name = "ScreensaverImageNeededLabel"
        Me.ScreensaverImageNeededLabel.Size = New System.Drawing.Size(211, 13)
        Me.ScreensaverImageNeededLabel.TabIndex = 6
        Me.ScreensaverImageNeededLabel.Text = "*No image selected yet*"
        Me.ScreensaverImageNeededLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ScreensaverImageNeededLabel.Visible = False
        '
        'ScreensaverImageButton
        '
        Me.ScreensaverImageButton.Location = New System.Drawing.Point(126, 68)
        Me.ScreensaverImageButton.Name = "ScreensaverImageButton"
        Me.ScreensaverImageButton.Size = New System.Drawing.Size(88, 23)
        Me.ScreensaverImageButton.TabIndex = 5
        Me.ScreensaverImageButton.Text = "Select Image"
        Me.ScreensaverImageButton.UseVisualStyleBackColor = True
        '
        'ScreensaverTransparent
        '
        Me.ScreensaverTransparent.AutoSize = True
        Me.ScreensaverTransparent.Location = New System.Drawing.Point(3, 3)
        Me.ScreensaverTransparent.Name = "ScreensaverTransparent"
        Me.ScreensaverTransparent.Size = New System.Drawing.Size(117, 17)
        Me.ScreensaverTransparent.TabIndex = 0
        Me.ScreensaverTransparent.Text = "None (Transparent)"
        Me.ScreensaverTransparent.UseVisualStyleBackColor = True
        '
        'ScreensaverColorNeededLabel
        '
        Me.ScreensaverColorNeededLabel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScreensaverColorNeededLabel.AutoSize = True
        Me.ScreensaverBackgroundTable.SetColumnSpan(Me.ScreensaverColorNeededLabel, 2)
        Me.ScreensaverColorNeededLabel.ForeColor = System.Drawing.Color.DarkRed
        Me.ScreensaverColorNeededLabel.Location = New System.Drawing.Point(3, 52)
        Me.ScreensaverColorNeededLabel.Name = "ScreensaverColorNeededLabel"
        Me.ScreensaverColorNeededLabel.Size = New System.Drawing.Size(211, 13)
        Me.ScreensaverColorNeededLabel.TabIndex = 3
        Me.ScreensaverColorNeededLabel.Text = "*No color selected yet*"
        Me.ScreensaverColorNeededLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        Me.ScreensaverColorNeededLabel.Visible = False
        '
        'ScreensaverColor
        '
        Me.ScreensaverColor.AutoSize = True
        Me.ScreensaverColor.Location = New System.Drawing.Point(3, 26)
        Me.ScreensaverColor.Name = "ScreensaverColor"
        Me.ScreensaverColor.Size = New System.Drawing.Size(75, 17)
        Me.ScreensaverColor.TabIndex = 1
        Me.ScreensaverColor.Text = "Solid Color"
        Me.ScreensaverColor.UseVisualStyleBackColor = True
        '
        'ScreensaverImage
        '
        Me.ScreensaverImage.AutoSize = True
        Me.ScreensaverImage.Location = New System.Drawing.Point(3, 68)
        Me.ScreensaverImage.Name = "ScreensaverImage"
        Me.ScreensaverImage.Size = New System.Drawing.Size(91, 17)
        Me.ScreensaverImage.TabIndex = 4
        Me.ScreensaverImage.Text = "Custom image"
        Me.ScreensaverImage.UseVisualStyleBackColor = True
        '
        'ScreensaverBackgroundLabel
        '
        Me.ScreensaverBackgroundLabel.AutoSize = True
        Me.ScreensaverBackgroundLabel.Location = New System.Drawing.Point(6, 39)
        Me.ScreensaverBackgroundLabel.Name = "ScreensaverBackgroundLabel"
        Me.ScreensaverBackgroundLabel.Size = New System.Drawing.Size(131, 13)
        Me.ScreensaverBackgroundLabel.TabIndex = 1
        Me.ScreensaverBackgroundLabel.Text = "Screensaver Background:"
        '
        'AlwaysOnTop
        '
        Me.AlwaysOnTop.AutoSize = True
        Me.AlwaysOnTop.Location = New System.Drawing.Point(6, 42)
        Me.AlwaysOnTop.Name = "AlwaysOnTop"
        Me.AlwaysOnTop.Size = New System.Drawing.Size(227, 17)
        Me.AlwaysOnTop.TabIndex = 1
        Me.AlwaysOnTop.Text = "Ponies are always on top of other windows"
        Me.AlwaysOnTop.UseVisualStyleBackColor = True
        '
        'Volume
        '
        Me.Volume.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Volume.Enabled = False
        Me.Volume.LargeChange = 10
        Me.Volume.Location = New System.Drawing.Point(6, 101)
        Me.Volume.Maximum = 100
        Me.Volume.Minimum = 10
        Me.Volume.Name = "Volume"
        Me.Volume.Size = New System.Drawing.Size(238, 45)
        Me.Volume.TabIndex = 5
        Me.Volume.TickFrequency = 10
        Me.Volume.Value = 10
        '
        'VolumeLabel
        '
        Me.VolumeLabel.AutoSize = True
        Me.VolumeLabel.Enabled = False
        Me.VolumeLabel.Location = New System.Drawing.Point(6, 85)
        Me.VolumeLabel.Name = "VolumeLabel"
        Me.VolumeLabel.Size = New System.Drawing.Size(79, 13)
        Me.VolumeLabel.TabIndex = 3
        Me.VolumeLabel.Text = "Sound Volume:"
        '
        'VolumeValueLabel
        '
        Me.VolumeValueLabel.AutoSize = True
        Me.VolumeValueLabel.Enabled = False
        Me.VolumeValueLabel.Location = New System.Drawing.Point(91, 85)
        Me.VolumeValueLabel.Name = "VolumeValueLabel"
        Me.VolumeValueLabel.Size = New System.Drawing.Size(22, 13)
        Me.VolumeValueLabel.TabIndex = 4
        Me.VolumeValueLabel.Text = "0.0"
        '
        'TimeScaleValueLabel
        '
        Me.TimeScaleValueLabel.AutoSize = True
        Me.TimeScaleValueLabel.Location = New System.Drawing.Point(83, 246)
        Me.TimeScaleValueLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.TimeScaleValueLabel.Name = "TimeScaleValueLabel"
        Me.TimeScaleValueLabel.Size = New System.Drawing.Size(18, 13)
        Me.TimeScaleValueLabel.TabIndex = 14
        Me.TimeScaleValueLabel.Text = "0x"
        '
        'TimeScaleLabel
        '
        Me.TimeScaleLabel.AutoSize = True
        Me.TimeScaleLabel.Location = New System.Drawing.Point(6, 246)
        Me.TimeScaleLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.TimeScaleLabel.Name = "TimeScaleLabel"
        Me.TimeScaleLabel.Size = New System.Drawing.Size(71, 13)
        Me.TimeScaleLabel.TabIndex = 13
        Me.TimeScaleLabel.Text = "Time Dilation:"
        '
        'TimeScale
        '
        Me.TimeScale.LargeChange = 10
        Me.TimeScale.Location = New System.Drawing.Point(6, 265)
        Me.TimeScale.Maximum = 40
        Me.TimeScale.Minimum = 1
        Me.TimeScale.Name = "TimeScale"
        Me.TimeScale.Size = New System.Drawing.Size(244, 45)
        Me.TimeScale.TabIndex = 15
        Me.TimeScale.TickFrequency = 10
        Me.TimeScale.Value = 1
        '
        'WindowsGroup
        '
        Me.WindowsGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.WindowsGroup.Controls.Add(Me.ShowViewerInTaskbar)
        Me.WindowsGroup.Controls.Add(Me.WindowAvoidance)
        Me.WindowsGroup.Controls.Add(Me.WindowContainment)
        Me.WindowsGroup.Controls.Add(Me.AlwaysOnTop)
        Me.WindowsGroup.Location = New System.Drawing.Point(12, 347)
        Me.WindowsGroup.Name = "WindowsGroup"
        Me.WindowsGroup.Size = New System.Drawing.Size(260, 142)
        Me.WindowsGroup.TabIndex = 1
        Me.WindowsGroup.TabStop = False
        Me.WindowsGroup.Text = "Windows"
        '
        'ShowViewerInTaskbar
        '
        Me.ShowViewerInTaskbar.AutoSize = True
        Me.ShowViewerInTaskbar.Location = New System.Drawing.Point(6, 19)
        Me.ShowViewerInTaskbar.Name = "ShowViewerInTaskbar"
        Me.ShowViewerInTaskbar.Size = New System.Drawing.Size(136, 17)
        Me.ShowViewerInTaskbar.TabIndex = 0
        Me.ShowViewerInTaskbar.Text = "Show ponies in taskbar"
        Me.ShowViewerInTaskbar.UseVisualStyleBackColor = True
        '
        'ScreenGroup
        '
        Me.ScreenGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ScreenGroup.Controls.Add(Me.TransparentBackground)
        Me.ScreenGroup.Controls.Add(Me.BackgroundColorButton)
        Me.ScreenGroup.Controls.Add(Me.OutOfBoundsLabel)
        Me.ScreenGroup.Controls.Add(Me.OutOfBoundsWalk)
        Me.ScreenGroup.Controls.Add(Me.OutOfBoundsTeleport)
        Me.ScreenGroup.Controls.Add(Me.ScreenCoveragePreviewLabel)
        Me.ScreenGroup.Controls.Add(Me.ScreenCoveragePreview)
        Me.ScreenGroup.Controls.Add(Me.ScreenCoveragePanel)
        Me.ScreenGroup.Controls.Add(Me.ScreenLabel)
        Me.ScreenGroup.Location = New System.Drawing.Point(278, 12)
        Me.ScreenGroup.Name = "ScreenGroup"
        Me.ScreenGroup.Size = New System.Drawing.Size(490, 245)
        Me.ScreenGroup.TabIndex = 2
        Me.ScreenGroup.TabStop = False
        Me.ScreenGroup.Text = "Screen"
        '
        'BackgroundColorButton
        '
        Me.BackgroundColorButton.Location = New System.Drawing.Point(406, 216)
        Me.BackgroundColorButton.Name = "BackgroundColorButton"
        Me.BackgroundColorButton.Size = New System.Drawing.Size(78, 23)
        Me.BackgroundColorButton.TabIndex = 8
        Me.BackgroundColorButton.Text = "Color"
        Me.BackgroundColorButton.UseVisualStyleBackColor = True
        '
        'OutOfBoundsLabel
        '
        Me.OutOfBoundsLabel.AutoSize = True
        Me.OutOfBoundsLabel.Location = New System.Drawing.Point(253, 16)
        Me.OutOfBoundsLabel.Name = "OutOfBoundsLabel"
        Me.OutOfBoundsLabel.Size = New System.Drawing.Size(211, 13)
        Me.OutOfBoundsLabel.TabIndex = 2
        Me.OutOfBoundsLabel.Text = "What happens if ponies are out of bounds?"
        '
        'OutOfBoundsWalk
        '
        Me.OutOfBoundsWalk.AutoSize = True
        Me.OutOfBoundsWalk.Location = New System.Drawing.Point(256, 55)
        Me.OutOfBoundsWalk.Name = "OutOfBoundsWalk"
        Me.OutOfBoundsWalk.Size = New System.Drawing.Size(177, 17)
        Me.OutOfBoundsWalk.TabIndex = 4
        Me.OutOfBoundsWalk.TabStop = True
        Me.OutOfBoundsWalk.Text = "Ponies walk back within bounds"
        Me.OutOfBoundsWalk.UseVisualStyleBackColor = True
        '
        'OutOfBoundsTeleport
        '
        Me.OutOfBoundsTeleport.AutoSize = True
        Me.OutOfBoundsTeleport.Location = New System.Drawing.Point(256, 32)
        Me.OutOfBoundsTeleport.Name = "OutOfBoundsTeleport"
        Me.OutOfBoundsTeleport.Size = New System.Drawing.Size(171, 17)
        Me.OutOfBoundsTeleport.TabIndex = 3
        Me.OutOfBoundsTeleport.TabStop = True
        Me.OutOfBoundsTeleport.Text = "Ponies teleport back in bounds"
        Me.OutOfBoundsTeleport.UseVisualStyleBackColor = True
        '
        'ScreenCoveragePreviewLabel
        '
        Me.ScreenCoveragePreviewLabel.AutoSize = True
        Me.ScreenCoveragePreviewLabel.Location = New System.Drawing.Point(330, 195)
        Me.ScreenCoveragePreviewLabel.Name = "ScreenCoveragePreviewLabel"
        Me.ScreenCoveragePreviewLabel.Size = New System.Drawing.Size(70, 13)
        Me.ScreenCoveragePreviewLabel.TabIndex = 6
        Me.ScreenCoveragePreviewLabel.Text = "Area Preview"
        '
        'ScreenCoveragePreview
        '
        Me.ScreenCoveragePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ScreenCoveragePreview.Location = New System.Drawing.Point(256, 78)
        Me.ScreenCoveragePreview.Name = "ScreenCoveragePreview"
        Me.ScreenCoveragePreview.Size = New System.Drawing.Size(225, 114)
        Me.ScreenCoveragePreview.TabIndex = 5
        '
        'ScreenCoveragePanel
        '
        Me.ScreenCoveragePanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScreenCoveragePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.ScreenCoveragePanel.Controls.Add(Me.AvoidanceAreaTable)
        Me.ScreenCoveragePanel.Controls.Add(Me.ScreenAreaTable)
        Me.ScreenCoveragePanel.Controls.Add(Me.ScreenExclusion)
        Me.ScreenCoveragePanel.Controls.Add(Me.ScreenCoverageArea)
        Me.ScreenCoveragePanel.Controls.Add(Me.ScreenCoverageMonitors)
        Me.ScreenCoveragePanel.Controls.Add(Me.MonitorsSelection)
        Me.ScreenCoveragePanel.Location = New System.Drawing.Point(6, 32)
        Me.ScreenCoveragePanel.Name = "ScreenCoveragePanel"
        Me.ScreenCoveragePanel.Size = New System.Drawing.Size(244, 207)
        Me.ScreenCoveragePanel.TabIndex = 1
        '
        'AvoidanceAreaTable
        '
        Me.AvoidanceAreaTable.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AvoidanceAreaTable.ColumnCount = 4
        Me.AvoidanceAreaTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.AvoidanceAreaTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.AvoidanceAreaTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.AvoidanceAreaTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.AvoidanceAreaTable.Controls.Add(Me.AvoidanceAreaHeightLabel, 3, 0)
        Me.AvoidanceAreaTable.Controls.Add(Me.ExclusionAreaHeight, 3, 1)
        Me.AvoidanceAreaTable.Controls.Add(Me.AvoidanceAreaWidthLabel, 2, 0)
        Me.AvoidanceAreaTable.Controls.Add(Me.ExclusionAreaWidth, 2, 1)
        Me.AvoidanceAreaTable.Controls.Add(Me.AvoidanceAreaYLabel, 1, 0)
        Me.AvoidanceAreaTable.Controls.Add(Me.AvoidanceAreaXLabel, 0, 0)
        Me.AvoidanceAreaTable.Controls.Add(Me.ExculsionAreaX, 0, 1)
        Me.AvoidanceAreaTable.Controls.Add(Me.ExclusionAreaY, 1, 1)
        Me.AvoidanceAreaTable.Enabled = False
        Me.AvoidanceAreaTable.Location = New System.Drawing.Point(3, 162)
        Me.AvoidanceAreaTable.Name = "AvoidanceAreaTable"
        Me.AvoidanceAreaTable.RowCount = 2
        Me.AvoidanceAreaTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.AvoidanceAreaTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.AvoidanceAreaTable.Size = New System.Drawing.Size(236, 39)
        Me.AvoidanceAreaTable.TabIndex = 4
        '
        'AvoidanceAreaHeightLabel
        '
        Me.AvoidanceAreaHeightLabel.AutoSize = True
        Me.AvoidanceAreaHeightLabel.Location = New System.Drawing.Point(180, 0)
        Me.AvoidanceAreaHeightLabel.Name = "AvoidanceAreaHeightLabel"
        Me.AvoidanceAreaHeightLabel.Size = New System.Drawing.Size(41, 13)
        Me.AvoidanceAreaHeightLabel.TabIndex = 6
        Me.AvoidanceAreaHeightLabel.Text = "Height:"
        '
        'ExclusionAreaHeight
        '
        Me.ExclusionAreaHeight.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExclusionAreaHeight.DecimalPlaces = 1
        Me.ExclusionAreaHeight.Location = New System.Drawing.Point(180, 16)
        Me.ExclusionAreaHeight.Name = "ExclusionAreaHeight"
        Me.ExclusionAreaHeight.Size = New System.Drawing.Size(53, 20)
        Me.ExclusionAreaHeight.TabIndex = 7
        '
        'AvoidanceAreaWidthLabel
        '
        Me.AvoidanceAreaWidthLabel.AutoSize = True
        Me.AvoidanceAreaWidthLabel.Location = New System.Drawing.Point(121, 0)
        Me.AvoidanceAreaWidthLabel.Name = "AvoidanceAreaWidthLabel"
        Me.AvoidanceAreaWidthLabel.Size = New System.Drawing.Size(38, 13)
        Me.AvoidanceAreaWidthLabel.TabIndex = 4
        Me.AvoidanceAreaWidthLabel.Text = "Width:"
        '
        'ExclusionAreaWidth
        '
        Me.ExclusionAreaWidth.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExclusionAreaWidth.DecimalPlaces = 1
        Me.ExclusionAreaWidth.Location = New System.Drawing.Point(121, 16)
        Me.ExclusionAreaWidth.Name = "ExclusionAreaWidth"
        Me.ExclusionAreaWidth.Size = New System.Drawing.Size(53, 20)
        Me.ExclusionAreaWidth.TabIndex = 5
        '
        'AvoidanceAreaYLabel
        '
        Me.AvoidanceAreaYLabel.AutoSize = True
        Me.AvoidanceAreaYLabel.Location = New System.Drawing.Point(62, 0)
        Me.AvoidanceAreaYLabel.Name = "AvoidanceAreaYLabel"
        Me.AvoidanceAreaYLabel.Size = New System.Drawing.Size(17, 13)
        Me.AvoidanceAreaYLabel.TabIndex = 2
        Me.AvoidanceAreaYLabel.Text = "Y:"
        '
        'AvoidanceAreaXLabel
        '
        Me.AvoidanceAreaXLabel.AutoSize = True
        Me.AvoidanceAreaXLabel.Location = New System.Drawing.Point(3, 0)
        Me.AvoidanceAreaXLabel.Name = "AvoidanceAreaXLabel"
        Me.AvoidanceAreaXLabel.Size = New System.Drawing.Size(17, 13)
        Me.AvoidanceAreaXLabel.TabIndex = 0
        Me.AvoidanceAreaXLabel.Text = "X:"
        '
        'ExculsionAreaX
        '
        Me.ExculsionAreaX.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExculsionAreaX.DecimalPlaces = 1
        Me.ExculsionAreaX.Location = New System.Drawing.Point(3, 16)
        Me.ExculsionAreaX.Name = "ExculsionAreaX"
        Me.ExculsionAreaX.Size = New System.Drawing.Size(53, 20)
        Me.ExculsionAreaX.TabIndex = 1
        '
        'ExclusionAreaY
        '
        Me.ExclusionAreaY.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ExclusionAreaY.DecimalPlaces = 1
        Me.ExclusionAreaY.Location = New System.Drawing.Point(62, 16)
        Me.ExclusionAreaY.Name = "ExclusionAreaY"
        Me.ExclusionAreaY.Size = New System.Drawing.Size(53, 20)
        Me.ExclusionAreaY.TabIndex = 3
        '
        'ScreenAreaTable
        '
        Me.ScreenAreaTable.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScreenAreaTable.ColumnCount = 4
        Me.ScreenAreaTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.ScreenAreaTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.ScreenAreaTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.ScreenAreaTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
        Me.ScreenAreaTable.Controls.Add(Me.ScreenAreaHeightLabel, 3, 0)
        Me.ScreenAreaTable.Controls.Add(Me.ScreenAreaHeight, 3, 1)
        Me.ScreenAreaTable.Controls.Add(Me.ScreenAreaWidthLabel, 2, 0)
        Me.ScreenAreaTable.Controls.Add(Me.ScreenAreaWidth, 2, 1)
        Me.ScreenAreaTable.Controls.Add(Me.ScreenAreaYLabel, 1, 0)
        Me.ScreenAreaTable.Controls.Add(Me.ScreenAreaXLabel, 0, 0)
        Me.ScreenAreaTable.Controls.Add(Me.ScreenAreaX, 0, 1)
        Me.ScreenAreaTable.Controls.Add(Me.ScreenAreaY, 1, 1)
        Me.ScreenAreaTable.Enabled = False
        Me.ScreenAreaTable.Location = New System.Drawing.Point(3, 94)
        Me.ScreenAreaTable.Name = "ScreenAreaTable"
        Me.ScreenAreaTable.RowCount = 2
        Me.ScreenAreaTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.ScreenAreaTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.ScreenAreaTable.Size = New System.Drawing.Size(236, 39)
        Me.ScreenAreaTable.TabIndex = 2
        '
        'ScreenAreaHeightLabel
        '
        Me.ScreenAreaHeightLabel.AutoSize = True
        Me.ScreenAreaHeightLabel.Location = New System.Drawing.Point(180, 0)
        Me.ScreenAreaHeightLabel.Name = "ScreenAreaHeightLabel"
        Me.ScreenAreaHeightLabel.Size = New System.Drawing.Size(41, 13)
        Me.ScreenAreaHeightLabel.TabIndex = 6
        Me.ScreenAreaHeightLabel.Text = "Height:"
        '
        'ScreenAreaHeight
        '
        Me.ScreenAreaHeight.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScreenAreaHeight.Location = New System.Drawing.Point(180, 16)
        Me.ScreenAreaHeight.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.ScreenAreaHeight.Name = "ScreenAreaHeight"
        Me.ScreenAreaHeight.Size = New System.Drawing.Size(53, 20)
        Me.ScreenAreaHeight.TabIndex = 7
        Me.ScreenAreaHeight.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'ScreenAreaWidthLabel
        '
        Me.ScreenAreaWidthLabel.AutoSize = True
        Me.ScreenAreaWidthLabel.Location = New System.Drawing.Point(121, 0)
        Me.ScreenAreaWidthLabel.Name = "ScreenAreaWidthLabel"
        Me.ScreenAreaWidthLabel.Size = New System.Drawing.Size(38, 13)
        Me.ScreenAreaWidthLabel.TabIndex = 4
        Me.ScreenAreaWidthLabel.Text = "Width:"
        '
        'ScreenAreaWidth
        '
        Me.ScreenAreaWidth.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScreenAreaWidth.Location = New System.Drawing.Point(121, 16)
        Me.ScreenAreaWidth.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.ScreenAreaWidth.Name = "ScreenAreaWidth"
        Me.ScreenAreaWidth.Size = New System.Drawing.Size(53, 20)
        Me.ScreenAreaWidth.TabIndex = 5
        Me.ScreenAreaWidth.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'ScreenAreaYLabel
        '
        Me.ScreenAreaYLabel.AutoSize = True
        Me.ScreenAreaYLabel.Location = New System.Drawing.Point(62, 0)
        Me.ScreenAreaYLabel.Name = "ScreenAreaYLabel"
        Me.ScreenAreaYLabel.Size = New System.Drawing.Size(17, 13)
        Me.ScreenAreaYLabel.TabIndex = 2
        Me.ScreenAreaYLabel.Text = "Y:"
        '
        'ScreenAreaXLabel
        '
        Me.ScreenAreaXLabel.AutoSize = True
        Me.ScreenAreaXLabel.Location = New System.Drawing.Point(3, 0)
        Me.ScreenAreaXLabel.Name = "ScreenAreaXLabel"
        Me.ScreenAreaXLabel.Size = New System.Drawing.Size(17, 13)
        Me.ScreenAreaXLabel.TabIndex = 0
        Me.ScreenAreaXLabel.Text = "X:"
        '
        'ScreenAreaX
        '
        Me.ScreenAreaX.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScreenAreaX.Location = New System.Drawing.Point(3, 16)
        Me.ScreenAreaX.Name = "ScreenAreaX"
        Me.ScreenAreaX.Size = New System.Drawing.Size(53, 20)
        Me.ScreenAreaX.TabIndex = 1
        '
        'ScreenAreaY
        '
        Me.ScreenAreaY.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ScreenAreaY.Location = New System.Drawing.Point(62, 16)
        Me.ScreenAreaY.Name = "ScreenAreaY"
        Me.ScreenAreaY.Size = New System.Drawing.Size(53, 20)
        Me.ScreenAreaY.TabIndex = 3
        '
        'ScreenExclusion
        '
        Me.ScreenExclusion.AutoSize = True
        Me.ScreenExclusion.Location = New System.Drawing.Point(3, 139)
        Me.ScreenExclusion.Name = "ScreenExclusion"
        Me.ScreenExclusion.Size = New System.Drawing.Size(155, 17)
        Me.ScreenExclusion.TabIndex = 3
        Me.ScreenExclusion.Text = "Avoid a portion of this area:"
        Me.ScreenExclusion.UseVisualStyleBackColor = True
        '
        'ScreenCoverageArea
        '
        Me.ScreenCoverageArea.AutoSize = True
        Me.ScreenCoverageArea.Location = New System.Drawing.Point(3, 75)
        Me.ScreenCoverageArea.Name = "ScreenCoverageArea"
        Me.ScreenCoverageArea.Size = New System.Drawing.Size(235, 17)
        Me.ScreenCoverageArea.TabIndex = 1
        Me.ScreenCoverageArea.Text = "Select area within which ponies may appear:"
        Me.ScreenCoverageArea.UseVisualStyleBackColor = True
        '
        'ScreenCoverageMonitors
        '
        Me.ScreenCoverageMonitors.AutoSize = True
        Me.ScreenCoverageMonitors.Location = New System.Drawing.Point(3, 3)
        Me.ScreenCoverageMonitors.Name = "ScreenCoverageMonitors"
        Me.ScreenCoverageMonitors.Size = New System.Drawing.Size(238, 17)
        Me.ScreenCoverageMonitors.TabIndex = 0
        Me.ScreenCoverageMonitors.Text = "Select monitors on which ponies may appear:"
        Me.ScreenCoverageMonitors.UseVisualStyleBackColor = True
        '
        'ScreenLabel
        '
        Me.ScreenLabel.AutoSize = True
        Me.ScreenLabel.Location = New System.Drawing.Point(6, 16)
        Me.ScreenLabel.Name = "ScreenLabel"
        Me.ScreenLabel.Size = New System.Drawing.Size(224, 13)
        Me.ScreenLabel.TabIndex = 0
        Me.ScreenLabel.Text = "Choose the area in which ponies are allowed: "
        '
        'SoundGroup
        '
        Me.SoundGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.SoundGroup.Controls.Add(Me.VolumeLabel)
        Me.SoundGroup.Controls.Add(Me.VolumeValueLabel)
        Me.SoundGroup.Controls.Add(Me.Sound)
        Me.SoundGroup.Controls.Add(Me.SoundLimitOneGlobally)
        Me.SoundGroup.Controls.Add(Me.SoundLimitOnePerPony)
        Me.SoundGroup.Controls.Add(Me.Volume)
        Me.SoundGroup.Location = New System.Drawing.Point(278, 263)
        Me.SoundGroup.Name = "SoundGroup"
        Me.SoundGroup.Size = New System.Drawing.Size(250, 150)
        Me.SoundGroup.TabIndex = 3
        Me.SoundGroup.TabStop = False
        Me.SoundGroup.Text = "Sound"
        '
        'MonitoringGroup
        '
        Me.MonitoringGroup.Controls.Add(Me.ShowPerformanceGraph)
        Me.MonitoringGroup.Controls.Add(Me.EnablePonyLogs)
        Me.MonitoringGroup.Location = New System.Drawing.Point(278, 419)
        Me.MonitoringGroup.Name = "MonitoringGroup"
        Me.MonitoringGroup.Size = New System.Drawing.Size(250, 70)
        Me.MonitoringGroup.TabIndex = 4
        Me.MonitoringGroup.TabStop = False
        Me.MonitoringGroup.Text = "Monitoring (applies for this session only)"
        '
        'ShowPerformanceGraph
        '
        Me.ShowPerformanceGraph.AutoSize = True
        Me.ShowPerformanceGraph.Location = New System.Drawing.Point(6, 42)
        Me.ShowPerformanceGraph.Name = "ShowPerformanceGraph"
        Me.ShowPerformanceGraph.Size = New System.Drawing.Size(145, 17)
        Me.ShowPerformanceGraph.TabIndex = 1
        Me.ShowPerformanceGraph.Text = "Show performance graph"
        Me.ShowPerformanceGraph.UseVisualStyleBackColor = True
        '
        'EnablePonyLogs
        '
        Me.EnablePonyLogs.AutoSize = True
        Me.EnablePonyLogs.Location = New System.Drawing.Point(6, 19)
        Me.EnablePonyLogs.Name = "EnablePonyLogs"
        Me.EnablePonyLogs.Size = New System.Drawing.Size(174, 17)
        Me.EnablePonyLogs.TabIndex = 0
        Me.EnablePonyLogs.Text = "Enable pony logs when running"
        Me.EnablePonyLogs.UseVisualStyleBackColor = True
        '
        'PoniesGroup
        '
        Me.PoniesGroup.Controls.Add(Me.MaxPonies)
        Me.PoniesGroup.Controls.Add(Me.MaxPoniesLabel)
        Me.PoniesGroup.Controls.Add(Me.SizeScaleValueLabel)
        Me.PoniesGroup.Controls.Add(Me.PonyDragging)
        Me.PoniesGroup.Controls.Add(Me.CursorAvoidanceRadius)
        Me.PoniesGroup.Controls.Add(Me.PoniesAvoidPonies)
        Me.PoniesGroup.Controls.Add(Me.TimeScaleValueLabel)
        Me.PoniesGroup.Controls.Add(Me.CursorAvoidanceRadiusLabel)
        Me.PoniesGroup.Controls.Add(Me.TimeScaleLabel)
        Me.PoniesGroup.Controls.Add(Me.TimeScale)
        Me.PoniesGroup.Controls.Add(Me.CursorAwareness)
        Me.PoniesGroup.Controls.Add(Me.Interactions)
        Me.PoniesGroup.Controls.Add(Me.PonySpeechChance)
        Me.PoniesGroup.Controls.Add(Me.Speech)
        Me.PoniesGroup.Controls.Add(Me.SizeScaleLabel)
        Me.PoniesGroup.Controls.Add(Me.SizeScale)
        Me.PoniesGroup.Controls.Add(Me.PonySpeechChanceLabel)
        Me.PoniesGroup.Controls.Add(Me.Effects)
        Me.PoniesGroup.Location = New System.Drawing.Point(12, 12)
        Me.PoniesGroup.Name = "PoniesGroup"
        Me.PoniesGroup.Size = New System.Drawing.Size(260, 329)
        Me.PoniesGroup.TabIndex = 0
        Me.PoniesGroup.TabStop = False
        Me.PoniesGroup.Text = "Ponies"
        '
        'TransparentBackground
        '
        Me.TransparentBackground.AutoSize = True
        Me.TransparentBackground.Location = New System.Drawing.Point(256, 220)
        Me.TransparentBackground.Name = "TransparentBackground"
        Me.TransparentBackground.Size = New System.Drawing.Size(144, 17)
        Me.TransparentBackground.TabIndex = 7
        Me.TransparentBackground.Text = "Transparent Background"
        Me.TransparentBackground.UseVisualStyleBackColor = True
        '
        'OptionsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(784, 512)
        Me.Controls.Add(Me.PoniesGroup)
        Me.Controls.Add(Me.MonitoringGroup)
        Me.Controls.Add(Me.SoundGroup)
        Me.Controls.Add(Me.ScreenGroup)
        Me.Controls.Add(Me.WindowsGroup)
        Me.Controls.Add(Me.ScreensaverGroup)
        Me.Controls.Add(Me.CustomFiltersButton)
        Me.Controls.Add(Me.ResetButton)
        Me.Controls.Add(Me.LoadButton)
        Me.Controls.Add(Me.SaveButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "OptionsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Options - Desktop Ponies"
        Me.ScreensaverGroup.ResumeLayout(False)
        Me.ScreensaverGroup.PerformLayout()
        Me.ScreensaverBackgroundTable.ResumeLayout(False)
        Me.ScreensaverBackgroundTable.PerformLayout()
        Me.WindowsGroup.ResumeLayout(False)
        Me.WindowsGroup.PerformLayout()
        Me.ScreenGroup.ResumeLayout(False)
        Me.ScreenGroup.PerformLayout()
        Me.ScreenCoveragePanel.ResumeLayout(False)
        Me.ScreenCoveragePanel.PerformLayout()
        Me.AvoidanceAreaTable.ResumeLayout(False)
        Me.AvoidanceAreaTable.PerformLayout()
        Me.ScreenAreaTable.ResumeLayout(False)
        Me.ScreenAreaTable.PerformLayout()
        Me.SoundGroup.ResumeLayout(False)
        Me.SoundGroup.PerformLayout()
        Me.MonitoringGroup.ResumeLayout(False)
        Me.MonitoringGroup.PerformLayout()
        Me.PoniesGroup.ResumeLayout(False)
        Me.PoniesGroup.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents MonitorsSelection As System.Windows.Forms.ListBox
    Friend WithEvents CursorAwareness As System.Windows.Forms.CheckBox
    Friend WithEvents CursorAvoidanceRadius As System.Windows.Forms.NumericUpDown
    Friend WithEvents CursorAvoidanceRadiusLabel As System.Windows.Forms.Label
    Friend WithEvents PonyDragging As System.Windows.Forms.CheckBox
    Friend WithEvents PonySpeechChance As System.Windows.Forms.NumericUpDown
    Friend WithEvents PonySpeechChanceLabel As System.Windows.Forms.Label
    Friend WithEvents MaxPonies As System.Windows.Forms.NumericUpDown
    Friend WithEvents MaxPoniesLabel As System.Windows.Forms.Label
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents LoadButton As System.Windows.Forms.Button
    Friend WithEvents ResetButton As System.Windows.Forms.Button
    Friend WithEvents Effects As System.Windows.Forms.CheckBox
    Friend WithEvents Sound As System.Windows.Forms.CheckBox
    Friend WithEvents Interactions As System.Windows.Forms.CheckBox
    Friend WithEvents WindowAvoidance As System.Windows.Forms.CheckBox
    Friend WithEvents Speech As System.Windows.Forms.CheckBox
    Friend WithEvents PoniesAvoidPonies As System.Windows.Forms.CheckBox
    Friend WithEvents WindowContainment As System.Windows.Forms.CheckBox
    Friend WithEvents SizeScale As System.Windows.Forms.TrackBar
    Friend WithEvents SizeScaleLabel As System.Windows.Forms.Label
    Friend WithEvents SizeScaleValueLabel As System.Windows.Forms.Label
    Friend WithEvents CustomFiltersButton As System.Windows.Forms.Button
    Friend WithEvents ScreensaverSounds As System.Windows.Forms.CheckBox
    Friend WithEvents SoundLimitOneGlobally As System.Windows.Forms.RadioButton
    Friend WithEvents SoundLimitOnePerPony As System.Windows.Forms.RadioButton
    Friend WithEvents ScreensaverGroup As System.Windows.Forms.GroupBox
    Friend WithEvents ScreensaverImageButton As System.Windows.Forms.Button
    Friend WithEvents ScreensaverImage As System.Windows.Forms.RadioButton
    Friend WithEvents ScreensaverColor As System.Windows.Forms.RadioButton
    Friend WithEvents ScreensaverBackgroundLabel As System.Windows.Forms.Label
    Friend WithEvents ScreensaverTransparent As System.Windows.Forms.RadioButton
    Friend WithEvents ScreensaverColorButton As System.Windows.Forms.Button
    Friend WithEvents ScreensaverImageNeededLabel As System.Windows.Forms.Label
    Friend WithEvents ScreensaverColorNeededLabel As System.Windows.Forms.Label
    Friend WithEvents AlwaysOnTop As System.Windows.Forms.CheckBox
    Friend WithEvents Volume As System.Windows.Forms.TrackBar
    Friend WithEvents VolumeLabel As System.Windows.Forms.Label
    Friend WithEvents VolumeValueLabel As System.Windows.Forms.Label
    Friend WithEvents TimeScaleValueLabel As System.Windows.Forms.Label
    Friend WithEvents TimeScaleLabel As System.Windows.Forms.Label
    Friend WithEvents TimeScale As System.Windows.Forms.TrackBar
    Friend WithEvents WindowsGroup As System.Windows.Forms.GroupBox
    Friend WithEvents ScreensaverBackgroundTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ScreenGroup As System.Windows.Forms.GroupBox
    Friend WithEvents SoundGroup As System.Windows.Forms.GroupBox
    Friend WithEvents ShowViewerInTaskbar As System.Windows.Forms.CheckBox
    Friend WithEvents MonitoringGroup As System.Windows.Forms.GroupBox
    Friend WithEvents EnablePonyLogs As System.Windows.Forms.CheckBox
    Friend WithEvents ShowPerformanceGraph As System.Windows.Forms.CheckBox
    Friend WithEvents PoniesGroup As System.Windows.Forms.GroupBox
    Friend WithEvents ScreenCoveragePanel As System.Windows.Forms.Panel
    Friend WithEvents ScreenAreaTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ScreenAreaHeightLabel As System.Windows.Forms.Label
    Friend WithEvents ScreenAreaHeight As System.Windows.Forms.NumericUpDown
    Friend WithEvents ScreenAreaWidthLabel As System.Windows.Forms.Label
    Friend WithEvents ScreenAreaWidth As System.Windows.Forms.NumericUpDown
    Friend WithEvents ScreenAreaYLabel As System.Windows.Forms.Label
    Friend WithEvents ScreenAreaXLabel As System.Windows.Forms.Label
    Friend WithEvents ScreenAreaX As System.Windows.Forms.NumericUpDown
    Friend WithEvents ScreenAreaY As System.Windows.Forms.NumericUpDown
    Friend WithEvents ScreenExclusion As System.Windows.Forms.CheckBox
    Friend WithEvents ScreenCoverageArea As System.Windows.Forms.RadioButton
    Friend WithEvents ScreenCoverageMonitors As System.Windows.Forms.RadioButton
    Friend WithEvents ScreenLabel As System.Windows.Forms.Label
    Friend WithEvents AvoidanceAreaTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents AvoidanceAreaHeightLabel As System.Windows.Forms.Label
    Friend WithEvents ExclusionAreaHeight As System.Windows.Forms.NumericUpDown
    Friend WithEvents AvoidanceAreaWidthLabel As System.Windows.Forms.Label
    Friend WithEvents ExclusionAreaWidth As System.Windows.Forms.NumericUpDown
    Friend WithEvents AvoidanceAreaYLabel As System.Windows.Forms.Label
    Friend WithEvents AvoidanceAreaXLabel As System.Windows.Forms.Label
    Friend WithEvents ExculsionAreaX As System.Windows.Forms.NumericUpDown
    Friend WithEvents ExclusionAreaY As System.Windows.Forms.NumericUpDown
    Friend WithEvents ScreenCoveragePreview As System.Windows.Forms.Panel
    Friend WithEvents ScreenCoveragePreviewLabel As System.Windows.Forms.Label
    Friend WithEvents OutOfBoundsWalk As System.Windows.Forms.RadioButton
    Friend WithEvents OutOfBoundsTeleport As System.Windows.Forms.RadioButton
    Friend WithEvents OutOfBoundsLabel As System.Windows.Forms.Label
    Friend WithEvents BackgroundColorButton As Button
    Friend WithEvents TransparentBackground As CheckBox
End Class

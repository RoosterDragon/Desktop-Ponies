<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class OptionsForm
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
        Me.AvoidanceZoneLocationLabel = New System.Windows.Forms.Label()
        Me.AvoidanceZoneSizeLabel = New System.Windows.Forms.Label()
        Me.AvoidanceZonePreviewLabel = New System.Windows.Forms.Label()
        Me.AvoidanceZoneY = New System.Windows.Forms.NumericUpDown()
        Me.AvoidanceZoneHeight = New System.Windows.Forms.NumericUpDown()
        Me.AvoidanceZoneX = New System.Windows.Forms.NumericUpDown()
        Me.AvoidanceZoneWidth = New System.Windows.Forms.NumericUpDown()
        Me.AvoidanceZonePreviewImage = New System.Windows.Forms.PictureBox()
        Me.MonitorsLabel = New System.Windows.Forms.Label()
        Me.MonitorsSelection = New System.Windows.Forms.ListBox()
        Me.MonitorsMinimumLabel = New System.Windows.Forms.Label()
        Me.CursorAvoidance = New System.Windows.Forms.CheckBox()
        Me.CursorAvoidanceRadius = New System.Windows.Forms.NumericUpDown()
        Me.CursorAvoidanceRadiusLabel = New System.Windows.Forms.Label()
        Me.PonyDragging = New System.Windows.Forms.CheckBox()
        Me.PonySpeechChance = New System.Windows.Forms.NumericUpDown()
        Me.PonySpeechChanceLabel = New System.Windows.Forms.Label()
        Me.MaxPonies = New System.Windows.Forms.NumericUpDown()
        Me.MaxPoniesLabel = New System.Windows.Forms.Label()
        Me.MaxPoniesWarningLabel = New System.Windows.Forms.Label()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.LoadButton = New System.Windows.Forms.Button()
        Me.ResetButton = New System.Windows.Forms.Button()
        Me.Effects = New System.Windows.Forms.CheckBox()
        Me.Sound = New System.Windows.Forms.CheckBox()
        Me.Interactions = New System.Windows.Forms.CheckBox()
        Me.InteractionsMissingLabel = New System.Windows.Forms.Label()
        Me.InteractionsErrorLabel = New System.Windows.Forms.Label()
        Me.WindowAvoidance = New System.Windows.Forms.CheckBox()
        Me.SpeechDisabled = New System.Windows.Forms.CheckBox()
        Me.InteractionErrorsDisplayed = New System.Windows.Forms.CheckBox()
        Me.InteractionErrorsDisplayedLabel = New System.Windows.Forms.Label()
        Me.PoniesAvoidPonies = New System.Windows.Forms.CheckBox()
        Me.PoniesStayInBoxes = New System.Windows.Forms.CheckBox()
        Me.AvoidanceZoneGroup = New System.Windows.Forms.GroupBox()
        Me.AvoidanceZoneAreaTable = New System.Windows.Forms.TableLayoutPanel()
        Me.SizeScale = New System.Windows.Forms.TrackBar()
        Me.SizeScaleLabel = New System.Windows.Forms.Label()
        Me.SizeScaleValueLabel = New System.Windows.Forms.Label()
        Me.CustomFiltersButton = New System.Windows.Forms.Button()
        Me.SoundDisabledLabel = New System.Windows.Forms.Label()
        Me.ScreensaverSounds = New System.Windows.Forms.CheckBox()
        Me.SoundLimitOneGlobally = New System.Windows.Forms.RadioButton()
        Me.SoundLimitOnePerPony = New System.Windows.Forms.RadioButton()
        Me.Teleport = New System.Windows.Forms.CheckBox()
        Me.TeleportLabel = New System.Windows.Forms.Label()
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
        Me.AlwaysOnTopLabel = New System.Windows.Forms.Label()
        Me.SuspendForFullscreenApp = New System.Windows.Forms.CheckBox()
        Me.Volume = New System.Windows.Forms.TrackBar()
        Me.VolumeLabel = New System.Windows.Forms.Label()
        Me.VolumeValueLabel = New System.Windows.Forms.Label()
        Me.TimeScaleValueLabel = New System.Windows.Forms.Label()
        Me.TimeScaleLabel = New System.Windows.Forms.Label()
        Me.TimeScale = New System.Windows.Forms.TrackBar()
        Me.AlphaBlending = New System.Windows.Forms.CheckBox()
        Me.SpeechGroup = New System.Windows.Forms.GroupBox()
        Me.SpeechDisabledLabel = New System.Windows.Forms.Label()
        Me.CursorGroup = New System.Windows.Forms.GroupBox()
        Me.InteractionsGroup = New System.Windows.Forms.GroupBox()
        Me.InteractionsTable = New System.Windows.Forms.TableLayoutPanel()
        Me.WindowsGroup = New System.Windows.Forms.GroupBox()
        Me.ScreenGroup = New System.Windows.Forms.GroupBox()
        Me.SoundGroup = New System.Windows.Forms.GroupBox()
        Me.GeneralGroup = New System.Windows.Forms.GroupBox()
        Me.AvoidanceZoneGroup.SuspendLayout()
        Me.AvoidanceZoneAreaTable.SuspendLayout()
        Me.ScreensaverGroup.SuspendLayout()
        Me.ScreensaverBackgroundTable.SuspendLayout()
        Me.SpeechGroup.SuspendLayout()
        Me.CursorGroup.SuspendLayout()
        Me.InteractionsGroup.SuspendLayout()
        Me.InteractionsTable.SuspendLayout()
        Me.WindowsGroup.SuspendLayout()
        Me.ScreenGroup.SuspendLayout()
        Me.SoundGroup.SuspendLayout()
        Me.GeneralGroup.SuspendLayout()
        Me.SuspendLayout()
        '
        'AvoidanceZoneLocationLabel
        '
        Me.AvoidanceZoneLocationLabel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AvoidanceZoneLocationLabel.Location = New System.Drawing.Point(3, 0)
        Me.AvoidanceZoneLocationLabel.Name = "AvoidanceZoneLocationLabel"
        Me.AvoidanceZoneLocationLabel.Size = New System.Drawing.Size(74, 26)
        Me.AvoidanceZoneLocationLabel.TabIndex = 0
        Me.AvoidanceZoneLocationLabel.Text = "Location (X,Y)"
        Me.AvoidanceZoneLocationLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AvoidanceZoneSizeLabel
        '
        Me.AvoidanceZoneSizeLabel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AvoidanceZoneSizeLabel.Location = New System.Drawing.Point(3, 26)
        Me.AvoidanceZoneSizeLabel.Name = "AvoidanceZoneSizeLabel"
        Me.AvoidanceZoneSizeLabel.Size = New System.Drawing.Size(74, 26)
        Me.AvoidanceZoneSizeLabel.TabIndex = 3
        Me.AvoidanceZoneSizeLabel.Text = "Size (X,Y)"
        Me.AvoidanceZoneSizeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'AvoidanceZonePreviewLabel
        '
        Me.AvoidanceZonePreviewLabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AvoidanceZonePreviewLabel.Location = New System.Drawing.Point(6, 189)
        Me.AvoidanceZonePreviewLabel.Name = "AvoidanceZonePreviewLabel"
        Me.AvoidanceZonePreviewLabel.Size = New System.Drawing.Size(257, 13)
        Me.AvoidanceZonePreviewLabel.TabIndex = 1
        Me.AvoidanceZonePreviewLabel.Text = "Pony Avoidance Zone Preview"
        Me.AvoidanceZonePreviewLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'AvoidanceZoneY
        '
        Me.AvoidanceZoneY.Location = New System.Drawing.Point(135, 3)
        Me.AvoidanceZoneY.Name = "AvoidanceZoneY"
        Me.AvoidanceZoneY.Size = New System.Drawing.Size(46, 20)
        Me.AvoidanceZoneY.TabIndex = 2
        '
        'AvoidanceZoneHeight
        '
        Me.AvoidanceZoneHeight.Location = New System.Drawing.Point(135, 29)
        Me.AvoidanceZoneHeight.Name = "AvoidanceZoneHeight"
        Me.AvoidanceZoneHeight.Size = New System.Drawing.Size(46, 20)
        Me.AvoidanceZoneHeight.TabIndex = 5
        '
        'AvoidanceZoneX
        '
        Me.AvoidanceZoneX.Location = New System.Drawing.Point(83, 3)
        Me.AvoidanceZoneX.Name = "AvoidanceZoneX"
        Me.AvoidanceZoneX.Size = New System.Drawing.Size(46, 20)
        Me.AvoidanceZoneX.TabIndex = 1
        '
        'AvoidanceZoneWidth
        '
        Me.AvoidanceZoneWidth.Location = New System.Drawing.Point(83, 29)
        Me.AvoidanceZoneWidth.Name = "AvoidanceZoneWidth"
        Me.AvoidanceZoneWidth.Size = New System.Drawing.Size(46, 20)
        Me.AvoidanceZoneWidth.TabIndex = 4
        '
        'AvoidanceZonePreviewImage
        '
        Me.AvoidanceZonePreviewImage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AvoidanceZonePreviewImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.AvoidanceZonePreviewImage.Location = New System.Drawing.Point(6, 77)
        Me.AvoidanceZonePreviewImage.Name = "AvoidanceZonePreviewImage"
        Me.AvoidanceZonePreviewImage.Size = New System.Drawing.Size(257, 109)
        Me.AvoidanceZonePreviewImage.TabIndex = 15
        Me.AvoidanceZonePreviewImage.TabStop = False
        '
        'MonitorsLabel
        '
        Me.MonitorsLabel.AutoSize = True
        Me.MonitorsLabel.Location = New System.Drawing.Point(42, 19)
        Me.MonitorsLabel.Name = "MonitorsLabel"
        Me.MonitorsLabel.Size = New System.Drawing.Size(82, 13)
        Me.MonitorsLabel.TabIndex = 0
        Me.MonitorsLabel.Text = "Monitors to use:"
        '
        'MonitorsSelection
        '
        Me.MonitorsSelection.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MonitorsSelection.FormattingEnabled = True
        Me.MonitorsSelection.Location = New System.Drawing.Point(130, 19)
        Me.MonitorsSelection.Name = "MonitorsSelection"
        Me.MonitorsSelection.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.MonitorsSelection.Size = New System.Drawing.Size(222, 56)
        Me.MonitorsSelection.TabIndex = 2
        '
        'MonitorsMinimumLabel
        '
        Me.MonitorsMinimumLabel.ForeColor = System.Drawing.Color.Maroon
        Me.MonitorsMinimumLabel.Location = New System.Drawing.Point(16, 32)
        Me.MonitorsMinimumLabel.Name = "MonitorsMinimumLabel"
        Me.MonitorsMinimumLabel.Size = New System.Drawing.Size(108, 42)
        Me.MonitorsMinimumLabel.TabIndex = 1
        Me.MonitorsMinimumLabel.Text = "(You need at least one monitor selected)"
        Me.MonitorsMinimumLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.MonitorsMinimumLabel.Visible = False
        '
        'CursorAvoidance
        '
        Me.CursorAvoidance.AutoSize = True
        Me.CursorAvoidance.Checked = True
        Me.CursorAvoidance.CheckState = System.Windows.Forms.CheckState.Checked
        Me.CursorAvoidance.Location = New System.Drawing.Point(6, 19)
        Me.CursorAvoidance.Name = "CursorAvoidance"
        Me.CursorAvoidance.Size = New System.Drawing.Size(245, 17)
        Me.CursorAvoidance.TabIndex = 0
        Me.CursorAvoidance.Text = "Ponies avoid cursor / stop when hovered over"
        Me.CursorAvoidance.UseVisualStyleBackColor = True
        '
        'CursorAvoidanceRadius
        '
        Me.CursorAvoidanceRadius.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        Me.CursorAvoidanceRadius.Location = New System.Drawing.Point(187, 42)
        Me.CursorAvoidanceRadius.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
        Me.CursorAvoidanceRadius.Name = "CursorAvoidanceRadius"
        Me.CursorAvoidanceRadius.Size = New System.Drawing.Size(76, 20)
        Me.CursorAvoidanceRadius.TabIndex = 2
        Me.CursorAvoidanceRadius.Value = New Decimal(New Integer() {100, 0, 0, 0})
        '
        'CursorAvoidanceRadiusLabel
        '
        Me.CursorAvoidanceRadiusLabel.AutoSize = True
        Me.CursorAvoidanceRadiusLabel.Location = New System.Drawing.Point(6, 46)
        Me.CursorAvoidanceRadiusLabel.Name = "CursorAvoidanceRadiusLabel"
        Me.CursorAvoidanceRadiusLabel.Size = New System.Drawing.Size(175, 13)
        Me.CursorAvoidanceRadiusLabel.TabIndex = 1
        Me.CursorAvoidanceRadiusLabel.Text = "Size of area around cursor to avoid:"
        '
        'PonyDragging
        '
        Me.PonyDragging.AutoSize = True
        Me.PonyDragging.Checked = True
        Me.PonyDragging.CheckState = System.Windows.Forms.CheckState.Checked
        Me.PonyDragging.Location = New System.Drawing.Point(6, 68)
        Me.PonyDragging.Name = "PonyDragging"
        Me.PonyDragging.Size = New System.Drawing.Size(246, 17)
        Me.PonyDragging.TabIndex = 3
        Me.PonyDragging.Text = "Ponies can be dragged around with the mouse"
        Me.PonyDragging.UseVisualStyleBackColor = True
        '
        'PonySpeechChance
        '
        Me.PonySpeechChance.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        Me.PonySpeechChance.Location = New System.Drawing.Point(159, 39)
        Me.PonySpeechChance.Name = "PonySpeechChance"
        Me.PonySpeechChance.Size = New System.Drawing.Size(50, 20)
        Me.PonySpeechChance.TabIndex = 2
        Me.PonySpeechChance.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'PonySpeechChanceLabel
        '
        Me.PonySpeechChanceLabel.AutoSize = True
        Me.PonySpeechChanceLabel.Location = New System.Drawing.Point(6, 41)
        Me.PonySpeechChanceLabel.Name = "PonySpeechChanceLabel"
        Me.PonySpeechChanceLabel.Size = New System.Drawing.Size(147, 13)
        Me.PonySpeechChanceLabel.TabIndex = 1
        Me.PonySpeechChanceLabel.Text = "Random Speech Chance (%):"
        '
        'MaxPonies
        '
        Me.MaxPonies.Location = New System.Drawing.Point(129, 42)
        Me.MaxPonies.Maximum = New Decimal(New Integer() {1000, 0, 0, 0})
        Me.MaxPonies.Name = "MaxPonies"
        Me.MaxPonies.Size = New System.Drawing.Size(50, 20)
        Me.MaxPonies.TabIndex = 2
        Me.MaxPonies.Value = New Decimal(New Integer() {300, 0, 0, 0})
        '
        'MaxPoniesLabel
        '
        Me.MaxPoniesLabel.AutoSize = True
        Me.MaxPoniesLabel.Location = New System.Drawing.Point(6, 44)
        Me.MaxPoniesLabel.Name = "MaxPoniesLabel"
        Me.MaxPoniesLabel.Size = New System.Drawing.Size(117, 13)
        Me.MaxPoniesLabel.TabIndex = 1
        Me.MaxPoniesLabel.Text = "Max Number of Ponies:"
        '
        'MaxPoniesWarningLabel
        '
        Me.MaxPoniesWarningLabel.AutoSize = True
        Me.MaxPoniesWarningLabel.ForeColor = System.Drawing.Color.DarkRed
        Me.MaxPoniesWarningLabel.Location = New System.Drawing.Point(6, 65)
        Me.MaxPoniesWarningLabel.Name = "MaxPoniesWarningLabel"
        Me.MaxPoniesWarningLabel.Size = New System.Drawing.Size(210, 26)
        Me.MaxPoniesWarningLabel.TabIndex = 3
        Me.MaxPoniesWarningLabel.Text = "**** WARNING: Too many ponies may  ****" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "**** make your computer run slowly ****"
        Me.MaxPoniesWarningLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'CloseButton
        '
        Me.CloseButton.Location = New System.Drawing.Point(777, 501)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(103, 23)
        Me.CloseButton.TabIndex = 13
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.Location = New System.Drawing.Point(777, 394)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(103, 23)
        Me.SaveButton.TabIndex = 10
        Me.SaveButton.Text = "SAVE"
        Me.SaveButton.UseVisualStyleBackColor = True
        '
        'LoadButton
        '
        Me.LoadButton.Location = New System.Drawing.Point(777, 365)
        Me.LoadButton.Name = "LoadButton"
        Me.LoadButton.Size = New System.Drawing.Size(103, 23)
        Me.LoadButton.TabIndex = 9
        Me.LoadButton.Text = "LOAD"
        Me.LoadButton.UseVisualStyleBackColor = True
        '
        'ResetButton
        '
        Me.ResetButton.Location = New System.Drawing.Point(777, 423)
        Me.ResetButton.Name = "ResetButton"
        Me.ResetButton.Size = New System.Drawing.Size(103, 23)
        Me.ResetButton.TabIndex = 11
        Me.ResetButton.Text = "RESET"
        Me.ResetButton.UseVisualStyleBackColor = True
        '
        'Effects
        '
        Me.Effects.AutoSize = True
        Me.Effects.Checked = True
        Me.Effects.CheckState = System.Windows.Forms.CheckState.Checked
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
        Me.Interactions.Checked = True
        Me.Interactions.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Interactions.Location = New System.Drawing.Point(3, 3)
        Me.Interactions.Name = "Interactions"
        Me.Interactions.Size = New System.Drawing.Size(117, 17)
        Me.Interactions.TabIndex = 0
        Me.Interactions.Text = "Enable Interactions"
        Me.Interactions.UseVisualStyleBackColor = True
        '
        'InteractionsMissingLabel
        '
        Me.InteractionsMissingLabel.AutoSize = True
        Me.InteractionsMissingLabel.ForeColor = System.Drawing.Color.DarkRed
        Me.InteractionsMissingLabel.Location = New System.Drawing.Point(3, 23)
        Me.InteractionsMissingLabel.Name = "InteractionsMissingLabel"
        Me.InteractionsMissingLabel.Size = New System.Drawing.Size(190, 13)
        Me.InteractionsMissingLabel.TabIndex = 1
        Me.InteractionsMissingLabel.Text = "*Disabled due to no interactions.ini file*"
        Me.InteractionsMissingLabel.Visible = False
        '
        'InteractionsErrorLabel
        '
        Me.InteractionsErrorLabel.AutoSize = True
        Me.InteractionsErrorLabel.ForeColor = System.Drawing.Color.DarkRed
        Me.InteractionsErrorLabel.Location = New System.Drawing.Point(3, 36)
        Me.InteractionsErrorLabel.Name = "InteractionsErrorLabel"
        Me.InteractionsErrorLabel.Size = New System.Drawing.Size(113, 13)
        Me.InteractionsErrorLabel.TabIndex = 2
        Me.InteractionsErrorLabel.Text = "*Disabled due to error*"
        Me.InteractionsErrorLabel.Visible = False
        '
        'WindowAvoidance
        '
        Me.WindowAvoidance.AutoSize = True
        Me.WindowAvoidance.Location = New System.Drawing.Point(6, 55)
        Me.WindowAvoidance.Name = "WindowAvoidance"
        Me.WindowAvoidance.Size = New System.Drawing.Size(184, 17)
        Me.WindowAvoidance.TabIndex = 2
        Me.WindowAvoidance.Text = "Ponies try to avoid other windows"
        Me.WindowAvoidance.UseVisualStyleBackColor = True
        '
        'SpeechDisabled
        '
        Me.SpeechDisabled.AutoSize = True
        Me.SpeechDisabled.Location = New System.Drawing.Point(6, 19)
        Me.SpeechDisabled.Name = "SpeechDisabled"
        Me.SpeechDisabled.Size = New System.Drawing.Size(132, 17)
        Me.SpeechDisabled.TabIndex = 0
        Me.SpeechDisabled.Text = "Disable all speech text"
        Me.SpeechDisabled.UseVisualStyleBackColor = True
        '
        'InteractionErrorsDisplayed
        '
        Me.InteractionErrorsDisplayed.AutoSize = True
        Me.InteractionErrorsDisplayed.Location = New System.Drawing.Point(3, 52)
        Me.InteractionErrorsDisplayed.Name = "InteractionErrorsDisplayed"
        Me.InteractionErrorsDisplayed.Size = New System.Drawing.Size(135, 17)
        Me.InteractionErrorsDisplayed.TabIndex = 3
        Me.InteractionErrorsDisplayed.Text = "Show Interaction errors"
        Me.InteractionErrorsDisplayed.UseVisualStyleBackColor = True
        '
        'InteractionErrorsDisplayedLabel
        '
        Me.InteractionErrorsDisplayedLabel.AutoSize = True
        Me.InteractionErrorsDisplayedLabel.Location = New System.Drawing.Point(3, 72)
        Me.InteractionErrorsDisplayedLabel.Name = "InteractionErrorsDisplayedLabel"
        Me.InteractionErrorsDisplayedLabel.Size = New System.Drawing.Size(183, 13)
        Me.InteractionErrorsDisplayedLabel.TabIndex = 4
        Me.InteractionErrorsDisplayedLabel.Text = "(Unchecked: Silently disable on error)"
        '
        'PoniesAvoidPonies
        '
        Me.PoniesAvoidPonies.AutoSize = True
        Me.PoniesAvoidPonies.Location = New System.Drawing.Point(27, 78)
        Me.PoniesAvoidPonies.Name = "PoniesAvoidPonies"
        Me.PoniesAvoidPonies.Size = New System.Drawing.Size(174, 17)
        Me.PoniesAvoidPonies.TabIndex = 3
        Me.PoniesAvoidPonies.Text = "Ponies try to avoid other ponies"
        Me.PoniesAvoidPonies.UseVisualStyleBackColor = True
        '
        'PoniesStayInBoxes
        '
        Me.PoniesStayInBoxes.AutoSize = True
        Me.PoniesStayInBoxes.Location = New System.Drawing.Point(27, 101)
        Me.PoniesStayInBoxes.Name = "PoniesStayInBoxes"
        Me.PoniesStayInBoxes.Size = New System.Drawing.Size(228, 17)
        Me.PoniesStayInBoxes.TabIndex = 4
        Me.PoniesStayInBoxes.Text = "Ponies don't leave windows they are inside"
        Me.PoniesStayInBoxes.UseVisualStyleBackColor = True
        '
        'AvoidanceZoneGroup
        '
        Me.AvoidanceZoneGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.AvoidanceZoneGroup.Controls.Add(Me.AvoidanceZoneAreaTable)
        Me.AvoidanceZoneGroup.Controls.Add(Me.AvoidanceZonePreviewLabel)
        Me.AvoidanceZoneGroup.Controls.Add(Me.AvoidanceZonePreviewImage)
        Me.AvoidanceZoneGroup.Location = New System.Drawing.Point(247, 122)
        Me.AvoidanceZoneGroup.Name = "AvoidanceZoneGroup"
        Me.AvoidanceZoneGroup.Size = New System.Drawing.Size(269, 218)
        Me.AvoidanceZoneGroup.TabIndex = 4
        Me.AvoidanceZoneGroup.TabStop = False
        Me.AvoidanceZoneGroup.Text = "Avoidance Zone (""Everfree Forest"")"
        '
        'AvoidanceZoneAreaTable
        '
        Me.AvoidanceZoneAreaTable.AutoSize = True
        Me.AvoidanceZoneAreaTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.AvoidanceZoneAreaTable.ColumnCount = 3
        Me.AvoidanceZoneAreaTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.AvoidanceZoneAreaTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.AvoidanceZoneAreaTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.AvoidanceZoneAreaTable.Controls.Add(Me.AvoidanceZoneLocationLabel, 0, 0)
        Me.AvoidanceZoneAreaTable.Controls.Add(Me.AvoidanceZoneSizeLabel, 0, 1)
        Me.AvoidanceZoneAreaTable.Controls.Add(Me.AvoidanceZoneY, 2, 0)
        Me.AvoidanceZoneAreaTable.Controls.Add(Me.AvoidanceZoneWidth, 1, 1)
        Me.AvoidanceZoneAreaTable.Controls.Add(Me.AvoidanceZoneX, 1, 0)
        Me.AvoidanceZoneAreaTable.Controls.Add(Me.AvoidanceZoneHeight, 2, 1)
        Me.AvoidanceZoneAreaTable.Location = New System.Drawing.Point(6, 19)
        Me.AvoidanceZoneAreaTable.Name = "AvoidanceZoneAreaTable"
        Me.AvoidanceZoneAreaTable.RowCount = 2
        Me.AvoidanceZoneAreaTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.AvoidanceZoneAreaTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.AvoidanceZoneAreaTable.Size = New System.Drawing.Size(184, 52)
        Me.AvoidanceZoneAreaTable.TabIndex = 0
        '
        'SizeScale
        '
        Me.SizeScale.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SizeScale.LargeChange = 25
        Me.SizeScale.Location = New System.Drawing.Point(6, 113)
        Me.SizeScale.Maximum = 500
        Me.SizeScale.Minimum = 25
        Me.SizeScale.Name = "SizeScale"
        Me.SizeScale.Size = New System.Drawing.Size(217, 45)
        Me.SizeScale.TabIndex = 6
        Me.SizeScale.TickFrequency = 25
        Me.SizeScale.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.SizeScale.Value = 100
        '
        'SizeScaleLabel
        '
        Me.SizeScaleLabel.AutoSize = True
        Me.SizeScaleLabel.Location = New System.Drawing.Point(6, 97)
        Me.SizeScaleLabel.Name = "SizeScaleLabel"
        Me.SizeScaleLabel.Size = New System.Drawing.Size(62, 13)
        Me.SizeScaleLabel.TabIndex = 4
        Me.SizeScaleLabel.Text = "Pony Sizes:"
        '
        'SizeScaleValueLabel
        '
        Me.SizeScaleValueLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SizeScaleValueLabel.AutoSize = True
        Me.SizeScaleValueLabel.Location = New System.Drawing.Point(191, 97)
        Me.SizeScaleValueLabel.Name = "SizeScaleValueLabel"
        Me.SizeScaleValueLabel.Size = New System.Drawing.Size(18, 13)
        Me.SizeScaleValueLabel.TabIndex = 5
        Me.SizeScaleValueLabel.Text = "1x"
        '
        'CustomFiltersButton
        '
        Me.CustomFiltersButton.Location = New System.Drawing.Point(777, 472)
        Me.CustomFiltersButton.Name = "CustomFiltersButton"
        Me.CustomFiltersButton.Size = New System.Drawing.Size(103, 23)
        Me.CustomFiltersButton.TabIndex = 12
        Me.CustomFiltersButton.Text = "Custom Filters"
        Me.CustomFiltersButton.UseVisualStyleBackColor = True
        '
        'SoundDisabledLabel
        '
        Me.SoundDisabledLabel.AutoSize = True
        Me.SoundDisabledLabel.ForeColor = System.Drawing.Color.DarkRed
        Me.SoundDisabledLabel.Location = New System.Drawing.Point(6, 39)
        Me.SoundDisabledLabel.Name = "SoundDisabledLabel"
        Me.SoundDisabledLabel.Size = New System.Drawing.Size(257, 13)
        Me.SoundDisabledLabel.TabIndex = 1
        Me.SoundDisabledLabel.Text = "*Disabled due to missing or incorrect DirectX version*"
        Me.SoundDisabledLabel.Visible = False
        '
        'ScreensaverSounds
        '
        Me.ScreensaverSounds.AutoSize = True
        Me.ScreensaverSounds.Checked = True
        Me.ScreensaverSounds.CheckState = System.Windows.Forms.CheckState.Checked
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
        Me.SoundLimitOneGlobally.Location = New System.Drawing.Point(6, 78)
        Me.SoundLimitOneGlobally.Name = "SoundLimitOneGlobally"
        Me.SoundLimitOneGlobally.Size = New System.Drawing.Size(159, 17)
        Me.SoundLimitOneGlobally.TabIndex = 3
        Me.SoundLimitOneGlobally.Text = "Limit sounds to one at a time"
        Me.SoundLimitOneGlobally.UseVisualStyleBackColor = True
        '
        'SoundLimitOnePerPony
        '
        Me.SoundLimitOnePerPony.AutoSize = True
        Me.SoundLimitOnePerPony.Checked = True
        Me.SoundLimitOnePerPony.Location = New System.Drawing.Point(6, 55)
        Me.SoundLimitOnePerPony.Name = "SoundLimitOnePerPony"
        Me.SoundLimitOnePerPony.Size = New System.Drawing.Size(160, 17)
        Me.SoundLimitOnePerPony.TabIndex = 2
        Me.SoundLimitOnePerPony.TabStop = True
        Me.SoundLimitOnePerPony.Text = "Limit sounds to one per pony"
        Me.SoundLimitOnePerPony.UseVisualStyleBackColor = True
        '
        'Teleport
        '
        Me.Teleport.AutoSize = True
        Me.Teleport.Location = New System.Drawing.Point(6, 104)
        Me.Teleport.Name = "Teleport"
        Me.Teleport.Size = New System.Drawing.Size(306, 17)
        Me.Teleport.TabIndex = 4
        Me.Teleport.Text = "Ponies teleport out of disallowed areas/screens immediately"
        Me.Teleport.UseVisualStyleBackColor = True
        '
        'TeleportLabel
        '
        Me.TeleportLabel.AutoSize = True
        Me.TeleportLabel.Location = New System.Drawing.Point(6, 124)
        Me.TeleportLabel.Name = "TeleportLabel"
        Me.TeleportLabel.Size = New System.Drawing.Size(274, 13)
        Me.TeleportLabel.TabIndex = 5
        Me.TeleportLabel.Text = "(Unchecked: Ponies slowly walk out of disallowed areas)"
        '
        'ScreensaverGroup
        '
        Me.ScreensaverGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ScreensaverGroup.Controls.Add(Me.ScreensaverBackgroundTable)
        Me.ScreensaverGroup.Controls.Add(Me.ScreensaverBackgroundLabel)
        Me.ScreensaverGroup.Controls.Add(Me.ScreensaverSounds)
        Me.ScreensaverGroup.Location = New System.Drawing.Point(12, 259)
        Me.ScreensaverGroup.Name = "ScreensaverGroup"
        Me.ScreensaverGroup.Size = New System.Drawing.Size(229, 181)
        Me.ScreensaverGroup.TabIndex = 1
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
        Me.ScreensaverTransparent.Checked = True
        Me.ScreensaverTransparent.Location = New System.Drawing.Point(3, 3)
        Me.ScreensaverTransparent.Name = "ScreensaverTransparent"
        Me.ScreensaverTransparent.Size = New System.Drawing.Size(117, 17)
        Me.ScreensaverTransparent.TabIndex = 0
        Me.ScreensaverTransparent.TabStop = True
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
        Me.AlwaysOnTop.Checked = True
        Me.AlwaysOnTop.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AlwaysOnTop.Location = New System.Drawing.Point(6, 19)
        Me.AlwaysOnTop.Name = "AlwaysOnTop"
        Me.AlwaysOnTop.Size = New System.Drawing.Size(227, 17)
        Me.AlwaysOnTop.TabIndex = 0
        Me.AlwaysOnTop.Text = "Ponies are always on top of other windows"
        Me.AlwaysOnTop.UseVisualStyleBackColor = True
        '
        'AlwaysOnTopLabel
        '
        Me.AlwaysOnTopLabel.AutoSize = True
        Me.AlwaysOnTopLabel.Location = New System.Drawing.Point(6, 39)
        Me.AlwaysOnTopLabel.Name = "AlwaysOnTopLabel"
        Me.AlwaysOnTopLabel.Size = New System.Drawing.Size(304, 13)
        Me.AlwaysOnTopLabel.TabIndex = 1
        Me.AlwaysOnTopLabel.Text = "(Unchecked: Ponies walk behind windows you are working on)"
        '
        'SuspendForFullscreenApp
        '
        Me.SuspendForFullscreenApp.AutoSize = True
        Me.SuspendForFullscreenApp.Location = New System.Drawing.Point(6, 81)
        Me.SuspendForFullscreenApp.Name = "SuspendForFullscreenApp"
        Me.SuspendForFullscreenApp.Size = New System.Drawing.Size(337, 17)
        Me.SuspendForFullscreenApp.TabIndex = 3
        Me.SuspendForFullscreenApp.Text = "Suspend and hide ponies when a full screen application is running"
        Me.SuspendForFullscreenApp.UseVisualStyleBackColor = True
        '
        'Volume
        '
        Me.Volume.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Volume.LargeChange = 100
        Me.Volume.Location = New System.Drawing.Point(6, 114)
        Me.Volume.Maximum = 1000
        Me.Volume.Minimum = 100
        Me.Volume.Name = "Volume"
        Me.Volume.Size = New System.Drawing.Size(257, 45)
        Me.Volume.SmallChange = 25
        Me.Volume.TabIndex = 6
        Me.Volume.TickFrequency = 100
        Me.Volume.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.Volume.Value = 650
        '
        'VolumeLabel
        '
        Me.VolumeLabel.AutoSize = True
        Me.VolumeLabel.Location = New System.Drawing.Point(6, 98)
        Me.VolumeLabel.Name = "VolumeLabel"
        Me.VolumeLabel.Size = New System.Drawing.Size(79, 13)
        Me.VolumeLabel.TabIndex = 4
        Me.VolumeLabel.Text = "Sound Volume:"
        '
        'VolumeValueLabel
        '
        Me.VolumeValueLabel.AutoSize = True
        Me.VolumeValueLabel.Location = New System.Drawing.Point(230, 98)
        Me.VolumeValueLabel.Name = "VolumeValueLabel"
        Me.VolumeValueLabel.Size = New System.Drawing.Size(22, 13)
        Me.VolumeValueLabel.TabIndex = 5
        Me.VolumeValueLabel.Text = "6.5"
        '
        'TimeScaleValueLabel
        '
        Me.TimeScaleValueLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TimeScaleValueLabel.AutoSize = True
        Me.TimeScaleValueLabel.Location = New System.Drawing.Point(191, 161)
        Me.TimeScaleValueLabel.Name = "TimeScaleValueLabel"
        Me.TimeScaleValueLabel.Size = New System.Drawing.Size(18, 13)
        Me.TimeScaleValueLabel.TabIndex = 8
        Me.TimeScaleValueLabel.Text = "1x"
        '
        'TimeScaleLabel
        '
        Me.TimeScaleLabel.AutoSize = True
        Me.TimeScaleLabel.Location = New System.Drawing.Point(6, 161)
        Me.TimeScaleLabel.Name = "TimeScaleLabel"
        Me.TimeScaleLabel.Size = New System.Drawing.Size(77, 13)
        Me.TimeScaleLabel.TabIndex = 7
        Me.TimeScaleLabel.Text = "Time Dialation:"
        '
        'TimeScale
        '
        Me.TimeScale.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TimeScale.LargeChange = 25
        Me.TimeScale.Location = New System.Drawing.Point(6, 177)
        Me.TimeScale.Maximum = 40
        Me.TimeScale.Minimum = 1
        Me.TimeScale.Name = "TimeScale"
        Me.TimeScale.Size = New System.Drawing.Size(217, 45)
        Me.TimeScale.TabIndex = 9
        Me.TimeScale.TickFrequency = 10
        Me.TimeScale.TickStyle = System.Windows.Forms.TickStyle.TopLeft
        Me.TimeScale.Value = 10
        '
        'AlphaBlending
        '
        Me.AlphaBlending.AutoSize = True
        Me.AlphaBlending.Checked = True
        Me.AlphaBlending.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AlphaBlending.Location = New System.Drawing.Point(6, 140)
        Me.AlphaBlending.Name = "AlphaBlending"
        Me.AlphaBlending.Size = New System.Drawing.Size(133, 17)
        Me.AlphaBlending.TabIndex = 6
        Me.AlphaBlending.Text = "Enable Alpha Blending"
        Me.AlphaBlending.UseVisualStyleBackColor = True
        '
        'SpeechGroup
        '
        Me.SpeechGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.SpeechGroup.Controls.Add(Me.SpeechDisabledLabel)
        Me.SpeechGroup.Controls.Add(Me.SpeechDisabled)
        Me.SpeechGroup.Controls.Add(Me.PonySpeechChance)
        Me.SpeechGroup.Controls.Add(Me.PonySpeechChanceLabel)
        Me.SpeechGroup.Location = New System.Drawing.Point(12, 446)
        Me.SpeechGroup.Name = "SpeechGroup"
        Me.SpeechGroup.Size = New System.Drawing.Size(229, 78)
        Me.SpeechGroup.TabIndex = 2
        Me.SpeechGroup.TabStop = False
        Me.SpeechGroup.Text = "Speech"
        '
        'SpeechDisabledLabel
        '
        Me.SpeechDisabledLabel.AutoSize = True
        Me.SpeechDisabledLabel.ForeColor = System.Drawing.Color.DarkRed
        Me.SpeechDisabledLabel.Location = New System.Drawing.Point(6, 60)
        Me.SpeechDisabledLabel.Name = "SpeechDisabledLabel"
        Me.SpeechDisabledLabel.Size = New System.Drawing.Size(135, 13)
        Me.SpeechDisabledLabel.TabIndex = 3
        Me.SpeechDisabledLabel.Text = "*Unavailable on Mac OSX*"
        Me.SpeechDisabledLabel.Visible = False
        '
        'CursorGroup
        '
        Me.CursorGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CursorGroup.Controls.Add(Me.CursorAvoidance)
        Me.CursorGroup.Controls.Add(Me.CursorAvoidanceRadiusLabel)
        Me.CursorGroup.Controls.Add(Me.CursorAvoidanceRadius)
        Me.CursorGroup.Controls.Add(Me.PonyDragging)
        Me.CursorGroup.Location = New System.Drawing.Point(247, 12)
        Me.CursorGroup.Name = "CursorGroup"
        Me.CursorGroup.Size = New System.Drawing.Size(269, 104)
        Me.CursorGroup.TabIndex = 3
        Me.CursorGroup.TabStop = False
        Me.CursorGroup.Text = "Cursor"
        '
        'InteractionsGroup
        '
        Me.InteractionsGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.InteractionsGroup.Controls.Add(Me.InteractionsTable)
        Me.InteractionsGroup.Location = New System.Drawing.Point(522, 346)
        Me.InteractionsGroup.Name = "InteractionsGroup"
        Me.InteractionsGroup.Size = New System.Drawing.Size(249, 178)
        Me.InteractionsGroup.TabIndex = 8
        Me.InteractionsGroup.TabStop = False
        Me.InteractionsGroup.Text = "Interactions"
        '
        'InteractionsTable
        '
        Me.InteractionsTable.AutoSize = True
        Me.InteractionsTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.InteractionsTable.ColumnCount = 1
        Me.InteractionsTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.InteractionsTable.Controls.Add(Me.Interactions, 0, 0)
        Me.InteractionsTable.Controls.Add(Me.InteractionErrorsDisplayedLabel, 0, 4)
        Me.InteractionsTable.Controls.Add(Me.InteractionErrorsDisplayed, 0, 3)
        Me.InteractionsTable.Controls.Add(Me.InteractionsErrorLabel, 0, 2)
        Me.InteractionsTable.Controls.Add(Me.InteractionsMissingLabel, 0, 1)
        Me.InteractionsTable.Location = New System.Drawing.Point(6, 19)
        Me.InteractionsTable.Name = "InteractionsTable"
        Me.InteractionsTable.RowCount = 5
        Me.InteractionsTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.InteractionsTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.InteractionsTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.InteractionsTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.InteractionsTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.InteractionsTable.Size = New System.Drawing.Size(196, 85)
        Me.InteractionsTable.TabIndex = 0
        '
        'WindowsGroup
        '
        Me.WindowsGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.WindowsGroup.Controls.Add(Me.WindowAvoidance)
        Me.WindowsGroup.Controls.Add(Me.PoniesAvoidPonies)
        Me.WindowsGroup.Controls.Add(Me.PoniesStayInBoxes)
        Me.WindowsGroup.Controls.Add(Me.AlwaysOnTop)
        Me.WindowsGroup.Controls.Add(Me.AlwaysOnTopLabel)
        Me.WindowsGroup.Location = New System.Drawing.Point(522, 194)
        Me.WindowsGroup.Name = "WindowsGroup"
        Me.WindowsGroup.Size = New System.Drawing.Size(358, 146)
        Me.WindowsGroup.TabIndex = 7
        Me.WindowsGroup.TabStop = False
        Me.WindowsGroup.Text = "Windows"
        '
        'ScreenGroup
        '
        Me.ScreenGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ScreenGroup.Controls.Add(Me.SuspendForFullscreenApp)
        Me.ScreenGroup.Controls.Add(Me.Teleport)
        Me.ScreenGroup.Controls.Add(Me.TeleportLabel)
        Me.ScreenGroup.Controls.Add(Me.MonitorsLabel)
        Me.ScreenGroup.Controls.Add(Me.MonitorsSelection)
        Me.ScreenGroup.Controls.Add(Me.AlphaBlending)
        Me.ScreenGroup.Controls.Add(Me.MonitorsMinimumLabel)
        Me.ScreenGroup.Location = New System.Drawing.Point(522, 12)
        Me.ScreenGroup.Name = "ScreenGroup"
        Me.ScreenGroup.Size = New System.Drawing.Size(358, 176)
        Me.ScreenGroup.TabIndex = 6
        Me.ScreenGroup.TabStop = False
        Me.ScreenGroup.Text = "Screen"
        '
        'SoundGroup
        '
        Me.SoundGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.SoundGroup.Controls.Add(Me.Sound)
        Me.SoundGroup.Controls.Add(Me.SoundDisabledLabel)
        Me.SoundGroup.Controls.Add(Me.SoundLimitOneGlobally)
        Me.SoundGroup.Controls.Add(Me.SoundLimitOnePerPony)
        Me.SoundGroup.Controls.Add(Me.VolumeLabel)
        Me.SoundGroup.Controls.Add(Me.Volume)
        Me.SoundGroup.Controls.Add(Me.VolumeValueLabel)
        Me.SoundGroup.Location = New System.Drawing.Point(247, 346)
        Me.SoundGroup.Name = "SoundGroup"
        Me.SoundGroup.Size = New System.Drawing.Size(269, 178)
        Me.SoundGroup.TabIndex = 5
        Me.SoundGroup.TabStop = False
        Me.SoundGroup.Text = "Sound"
        '
        'GeneralGroup
        '
        Me.GeneralGroup.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GeneralGroup.Controls.Add(Me.Effects)
        Me.GeneralGroup.Controls.Add(Me.MaxPoniesLabel)
        Me.GeneralGroup.Controls.Add(Me.MaxPonies)
        Me.GeneralGroup.Controls.Add(Me.MaxPoniesWarningLabel)
        Me.GeneralGroup.Controls.Add(Me.SizeScaleLabel)
        Me.GeneralGroup.Controls.Add(Me.SizeScale)
        Me.GeneralGroup.Controls.Add(Me.TimeScaleValueLabel)
        Me.GeneralGroup.Controls.Add(Me.SizeScaleValueLabel)
        Me.GeneralGroup.Controls.Add(Me.TimeScaleLabel)
        Me.GeneralGroup.Controls.Add(Me.TimeScale)
        Me.GeneralGroup.Location = New System.Drawing.Point(12, 12)
        Me.GeneralGroup.Name = "GeneralGroup"
        Me.GeneralGroup.Size = New System.Drawing.Size(229, 241)
        Me.GeneralGroup.TabIndex = 0
        Me.GeneralGroup.TabStop = False
        Me.GeneralGroup.Text = "General"
        '
        'OptionsForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(892, 536)
        Me.ControlBox = False
        Me.Controls.Add(Me.GeneralGroup)
        Me.Controls.Add(Me.SoundGroup)
        Me.Controls.Add(Me.ScreenGroup)
        Me.Controls.Add(Me.WindowsGroup)
        Me.Controls.Add(Me.InteractionsGroup)
        Me.Controls.Add(Me.CursorGroup)
        Me.Controls.Add(Me.SpeechGroup)
        Me.Controls.Add(Me.AvoidanceZoneGroup)
        Me.Controls.Add(Me.ScreensaverGroup)
        Me.Controls.Add(Me.CustomFiltersButton)
        Me.Controls.Add(Me.ResetButton)
        Me.Controls.Add(Me.LoadButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.CloseButton)
        Me.MinimumSize = New System.Drawing.Size(400, 300)
        Me.Name = "OptionsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Options - Desktop Ponies"
        Me.AvoidanceZoneGroup.ResumeLayout(False)
        Me.AvoidanceZoneGroup.PerformLayout()
        Me.AvoidanceZoneAreaTable.ResumeLayout(False)
        Me.ScreensaverGroup.ResumeLayout(False)
        Me.ScreensaverGroup.PerformLayout()
        Me.ScreensaverBackgroundTable.ResumeLayout(False)
        Me.ScreensaverBackgroundTable.PerformLayout()
        Me.SpeechGroup.ResumeLayout(False)
        Me.SpeechGroup.PerformLayout()
        Me.CursorGroup.ResumeLayout(False)
        Me.CursorGroup.PerformLayout()
        Me.InteractionsGroup.ResumeLayout(False)
        Me.InteractionsGroup.PerformLayout()
        Me.InteractionsTable.ResumeLayout(False)
        Me.InteractionsTable.PerformLayout()
        Me.WindowsGroup.ResumeLayout(False)
        Me.WindowsGroup.PerformLayout()
        Me.ScreenGroup.ResumeLayout(False)
        Me.ScreenGroup.PerformLayout()
        Me.SoundGroup.ResumeLayout(False)
        Me.SoundGroup.PerformLayout()
        Me.GeneralGroup.ResumeLayout(False)
        Me.GeneralGroup.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents AvoidanceZoneLocationLabel As System.Windows.Forms.Label
    Friend WithEvents AvoidanceZoneSizeLabel As System.Windows.Forms.Label
    Friend WithEvents AvoidanceZonePreviewLabel As System.Windows.Forms.Label
    Friend WithEvents AvoidanceZoneY As System.Windows.Forms.NumericUpDown
    Friend WithEvents AvoidanceZoneHeight As System.Windows.Forms.NumericUpDown
    Friend WithEvents AvoidanceZoneX As System.Windows.Forms.NumericUpDown
    Friend WithEvents AvoidanceZoneWidth As System.Windows.Forms.NumericUpDown
    Friend WithEvents AvoidanceZonePreviewImage As System.Windows.Forms.PictureBox
    Friend WithEvents MonitorsLabel As System.Windows.Forms.Label
    Friend WithEvents MonitorsSelection As System.Windows.Forms.ListBox
    Friend WithEvents MonitorsMinimumLabel As System.Windows.Forms.Label
    Friend WithEvents CursorAvoidance As System.Windows.Forms.CheckBox
    Friend WithEvents CursorAvoidanceRadius As System.Windows.Forms.NumericUpDown
    Friend WithEvents CursorAvoidanceRadiusLabel As System.Windows.Forms.Label
    Friend WithEvents PonyDragging As System.Windows.Forms.CheckBox
    Friend WithEvents PonySpeechChance As System.Windows.Forms.NumericUpDown
    Friend WithEvents PonySpeechChanceLabel As System.Windows.Forms.Label
    Friend WithEvents MaxPonies As System.Windows.Forms.NumericUpDown
    Friend WithEvents MaxPoniesLabel As System.Windows.Forms.Label
    Friend WithEvents MaxPoniesWarningLabel As System.Windows.Forms.Label
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents LoadButton As System.Windows.Forms.Button
    Friend WithEvents ResetButton As System.Windows.Forms.Button
    Friend WithEvents Effects As System.Windows.Forms.CheckBox
    Friend WithEvents Sound As System.Windows.Forms.CheckBox
    Friend WithEvents Interactions As System.Windows.Forms.CheckBox
    Friend WithEvents InteractionsMissingLabel As System.Windows.Forms.Label
    Friend WithEvents InteractionsErrorLabel As System.Windows.Forms.Label
    Friend WithEvents WindowAvoidance As System.Windows.Forms.CheckBox
    Friend WithEvents SpeechDisabled As System.Windows.Forms.CheckBox
    Friend WithEvents InteractionErrorsDisplayed As System.Windows.Forms.CheckBox
    Friend WithEvents InteractionErrorsDisplayedLabel As System.Windows.Forms.Label
    Friend WithEvents PoniesAvoidPonies As System.Windows.Forms.CheckBox
    Friend WithEvents PoniesStayInBoxes As System.Windows.Forms.CheckBox
    Friend WithEvents AvoidanceZoneGroup As System.Windows.Forms.GroupBox
    Friend WithEvents SizeScale As System.Windows.Forms.TrackBar
    Friend WithEvents SizeScaleLabel As System.Windows.Forms.Label
    Friend WithEvents SizeScaleValueLabel As System.Windows.Forms.Label
    Friend WithEvents CustomFiltersButton As System.Windows.Forms.Button
    Friend WithEvents SoundDisabledLabel As System.Windows.Forms.Label
    Friend WithEvents ScreensaverSounds As System.Windows.Forms.CheckBox
    Friend WithEvents SoundLimitOneGlobally As System.Windows.Forms.RadioButton
    Friend WithEvents SoundLimitOnePerPony As System.Windows.Forms.RadioButton
    Friend WithEvents Teleport As System.Windows.Forms.CheckBox
    Friend WithEvents TeleportLabel As System.Windows.Forms.Label
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
    Friend WithEvents AlwaysOnTopLabel As System.Windows.Forms.Label
    Friend WithEvents SuspendForFullscreenApp As System.Windows.Forms.CheckBox
    Friend WithEvents Volume As System.Windows.Forms.TrackBar
    Friend WithEvents VolumeLabel As System.Windows.Forms.Label
    Friend WithEvents VolumeValueLabel As System.Windows.Forms.Label
    Friend WithEvents TimeScaleValueLabel As System.Windows.Forms.Label
    Friend WithEvents TimeScaleLabel As System.Windows.Forms.Label
    Friend WithEvents TimeScale As System.Windows.Forms.TrackBar
    Friend WithEvents AlphaBlending As System.Windows.Forms.CheckBox
    Friend WithEvents SpeechGroup As System.Windows.Forms.GroupBox
    Friend WithEvents CursorGroup As System.Windows.Forms.GroupBox
    Friend WithEvents InteractionsGroup As System.Windows.Forms.GroupBox
    Friend WithEvents WindowsGroup As System.Windows.Forms.GroupBox
    Friend WithEvents AvoidanceZoneAreaTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ScreensaverBackgroundTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents ScreenGroup As System.Windows.Forms.GroupBox
    Friend WithEvents SoundGroup As System.Windows.Forms.GroupBox
    Friend WithEvents GeneralGroup As System.Windows.Forms.GroupBox
    Friend WithEvents SpeechDisabledLabel As System.Windows.Forms.Label
    Friend WithEvents InteractionsTable As System.Windows.Forms.TableLayoutPanel
End Class

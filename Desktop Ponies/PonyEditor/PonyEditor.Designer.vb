<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PonyEditor
    Inherits System.Windows.Forms.Form

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.PonyPreviewPanel = New System.Windows.Forms.Panel()
        Me.PonyList = New System.Windows.Forms.ListView()
        Me.colPony = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.NewPonyButton = New System.Windows.Forms.Button()
        Me.PonyName = New System.Windows.Forms.TextBox()
        Me.PonyNameLabel = New System.Windows.Forms.Label()
        Me.BehaviorsGrid = New System.Windows.Forms.DataGridView()
        Me.colBehaviorActivate = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colBehaviorName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehaviorOriginalName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehaviorGroup = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehaviorGroupName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehaviorChance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehaviorMaxDuration = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehaviorMinDuration = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehaviorSpeed = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehaviorRightImage = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colBehaviorLeftImage = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colBehaviorMovement = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colBehaviorStartSpeech = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colBehaviorEndSpeech = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colBehaviorFollow = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colBehaviorLinked = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colBehaviorLinkOrder = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colBehaviorDoNotRunRandomly = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.colBehaviorDoNotRepeatAnimations = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.SpeechesGrid = New System.Windows.Forms.DataGridView()
        Me.colSpeechName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSpeechOriginalName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSpeechGroup = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSpeechGroupName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSpeechText = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSpeechSoundFile = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colSpeechUseRandomly = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.InteractionsGrid = New System.Windows.Forms.DataGridView()
        Me.colInteractionName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colInteractionOriginalName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colInteractionChance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colInteractionProximity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colInteractionTargets = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colInteractionInteractWith = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colInteractionBehaviors = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colInteractionReactivationDelay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.BehaviorsLabel = New System.Windows.Forms.Label()
        Me.SpeechesLabel = New System.Windows.Forms.Label()
        Me.InteractionsLabel = New System.Windows.Forms.Label()
        Me.EffectsGrid = New System.Windows.Forms.DataGridView()
        Me.colEffectName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEffectOriginalName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEffectBehavior = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colEffectRightImage = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colEffectLeftImage = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colEffectDuration = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEffectRepeatDelay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colEffectLocationRight = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colEffectCenteringRight = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colEffectLocationLeft = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colEffectCenteringLeft = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colEffectFollowPony = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.colEffectDoNotRepeatAnimations = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.CurrentBehaviorLabel = New System.Windows.Forms.Label()
        Me.CurrentBehaviorValueLabel = New System.Windows.Forms.Label()
        Me.TimeLeftLabel = New System.Windows.Forms.Label()
        Me.TimeLeftValueLabel = New System.Windows.Forms.Label()
        Me.EffectsLabel = New System.Windows.Forms.Label()
        Me.NewBehaviorButton = New System.Windows.Forms.Button()
        Me.NewSpeechButton = New System.Windows.Forms.Button()
        Me.NewEffectButton = New System.Windows.Forms.Button()
        Me.NewInteractionButton = New System.Windows.Forms.Button()
        Me.SpeechesSwapButton = New System.Windows.Forms.Button()
        Me.EffectsSwapButton = New System.Windows.Forms.Button()
        Me.InteractionsSwapButton = New System.Windows.Forms.Button()
        Me.BehaviorsSwapButton = New System.Windows.Forms.Button()
        Me.PausePonyButton = New System.Windows.Forms.Button()
        Me.OpenPictureDialog = New System.Windows.Forms.OpenFileDialog()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.EditTagsButton = New System.Windows.Forms.Button()
        Me.RefreshButton = New System.Windows.Forms.Button()
        Me.GroupValueLabel = New System.Windows.Forms.Label()
        Me.GroupLabel = New System.Windows.Forms.Label()
        Me.SelectPonyGroup = New System.Windows.Forms.GroupBox()
        Me.PonyPreviewGroup = New System.Windows.Forms.GroupBox()
        Me.GridTable = New System.Windows.Forms.TableLayoutPanel()
        Me.InteractionsPanel = New System.Windows.Forms.Panel()
        Me.EffectsPanel = New System.Windows.Forms.Panel()
        Me.SpeechesPanel = New System.Windows.Forms.Panel()
        Me.BehaviorsPanel = New System.Windows.Forms.Panel()
        Me.LayoutTable = New System.Windows.Forms.TableLayoutPanel()
        Me.PonyPanel = New System.Windows.Forms.Panel()
        Me.ImagesButton = New System.Windows.Forms.Button()
        Me.ImagesContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ImageCentersMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GifAlphaMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GifViewerMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelectPonyGroup.SuspendLayout()
        Me.PonyPreviewGroup.SuspendLayout()
        Me.GridTable.SuspendLayout()
        Me.InteractionsPanel.SuspendLayout()
        Me.EffectsPanel.SuspendLayout()
        Me.SpeechesPanel.SuspendLayout()
        Me.BehaviorsPanel.SuspendLayout()
        Me.LayoutTable.SuspendLayout()
        Me.PonyPanel.SuspendLayout()
        Me.ImagesContextMenu.SuspendLayout()
        Me.SuspendLayout()
        '
        'PonyPreviewPanel
        '
        Me.PonyPreviewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PonyPreviewPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PonyPreviewPanel.Location = New System.Drawing.Point(3, 16)
        Me.PonyPreviewPanel.Name = "PonyPreviewPanel"
        Me.PonyPreviewPanel.Size = New System.Drawing.Size(855, 215)
        Me.PonyPreviewPanel.TabIndex = 0
        '
        'PonyList
        '
        Me.PonyList.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.PonyList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PonyList.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.PonyList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colPony})
        Me.PonyList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.PonyList.HideSelection = False
        Me.PonyList.Location = New System.Drawing.Point(6, 19)
        Me.PonyList.MultiSelect = False
        Me.PonyList.Name = "PonyList"
        Me.PonyList.ShowGroups = False
        Me.PonyList.Size = New System.Drawing.Size(117, 232)
        Me.PonyList.TabIndex = 0
        Me.PonyList.UseCompatibleStateImageBehavior = False
        '
        'colPony
        '
        Me.colPony.Text = "Pony"
        '
        'NewPonyButton
        '
        Me.NewPonyButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NewPonyButton.Location = New System.Drawing.Point(6, 257)
        Me.NewPonyButton.Name = "NewPonyButton"
        Me.NewPonyButton.Size = New System.Drawing.Size(117, 21)
        Me.NewPonyButton.TabIndex = 1
        Me.NewPonyButton.Text = "Create NEW Pony"
        Me.NewPonyButton.UseVisualStyleBackColor = True
        '
        'PonyName
        '
        Me.PonyName.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.PonyName.Location = New System.Drawing.Point(289, 244)
        Me.PonyName.Name = "PonyName"
        Me.PonyName.Size = New System.Drawing.Size(125, 20)
        Me.PonyName.TabIndex = 3
        '
        'PonyNameLabel
        '
        Me.PonyNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.PonyNameLabel.AutoSize = True
        Me.PonyNameLabel.Location = New System.Drawing.Point(245, 247)
        Me.PonyNameLabel.Name = "PonyNameLabel"
        Me.PonyNameLabel.Size = New System.Drawing.Size(38, 13)
        Me.PonyNameLabel.TabIndex = 2
        Me.PonyNameLabel.Text = "Name:"
        '
        'BehaviorsGrid
        '
        Me.BehaviorsGrid.AllowUserToAddRows = False
        Me.BehaviorsGrid.AllowUserToResizeRows = False
        Me.BehaviorsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BehaviorsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.BehaviorsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colBehaviorActivate, Me.colBehaviorName, Me.colBehaviorOriginalName, Me.colBehaviorGroup, Me.colBehaviorGroupName, Me.colBehaviorChance, Me.colBehaviorMaxDuration, Me.colBehaviorMinDuration, Me.colBehaviorSpeed, Me.colBehaviorRightImage, Me.colBehaviorLeftImage, Me.colBehaviorMovement, Me.colBehaviorStartSpeech, Me.colBehaviorEndSpeech, Me.colBehaviorFollow, Me.colBehaviorLinked, Me.colBehaviorLinkOrder, Me.colBehaviorDoNotRunRandomly, Me.colBehaviorDoNotRepeatAnimations})
        Me.BehaviorsGrid.Location = New System.Drawing.Point(3, 32)
        Me.BehaviorsGrid.MultiSelect = False
        Me.BehaviorsGrid.Name = "BehaviorsGrid"
        Me.BehaviorsGrid.Size = New System.Drawing.Size(990, 170)
        Me.BehaviorsGrid.TabIndex = 1
        '
        'colBehaviorActivate
        '
        Me.colBehaviorActivate.HeaderText = "Activate"
        Me.colBehaviorActivate.Name = "colBehaviorActivate"
        Me.colBehaviorActivate.Width = 52
        '
        'colBehaviorName
        '
        Me.colBehaviorName.HeaderText = "Name"
        Me.colBehaviorName.MaxInputLength = 255
        Me.colBehaviorName.Name = "colBehaviorName"
        Me.colBehaviorName.Width = 60
        '
        'colBehaviorOriginalName
        '
        Me.colBehaviorOriginalName.HeaderText = "Original Name"
        Me.colBehaviorOriginalName.Name = "colBehaviorOriginalName"
        Me.colBehaviorOriginalName.Visible = False
        Me.colBehaviorOriginalName.Width = 98
        '
        'colBehaviorGroup
        '
        Me.colBehaviorGroup.HeaderText = "Group"
        Me.colBehaviorGroup.Name = "colBehaviorGroup"
        Me.colBehaviorGroup.Width = 61
        '
        'colBehaviorGroupName
        '
        Me.colBehaviorGroupName.HeaderText = "Group Name"
        Me.colBehaviorGroupName.Name = "colBehaviorGroupName"
        Me.colBehaviorGroupName.Width = 92
        '
        'colBehaviorChance
        '
        Me.colBehaviorChance.HeaderText = "Chance"
        Me.colBehaviorChance.Name = "colBehaviorChance"
        Me.colBehaviorChance.Width = 69
        '
        'colBehaviorMaxDuration
        '
        Me.colBehaviorMaxDuration.HeaderText = "Max Duration"
        Me.colBehaviorMaxDuration.Name = "colBehaviorMaxDuration"
        Me.colBehaviorMaxDuration.Width = 95
        '
        'colBehaviorMinDuration
        '
        Me.colBehaviorMinDuration.HeaderText = "Min Duration"
        Me.colBehaviorMinDuration.Name = "colBehaviorMinDuration"
        Me.colBehaviorMinDuration.Width = 92
        '
        'colBehaviorSpeed
        '
        Me.colBehaviorSpeed.HeaderText = "Speed"
        Me.colBehaviorSpeed.Name = "colBehaviorSpeed"
        Me.colBehaviorSpeed.Width = 63
        '
        'colBehaviorRightImage
        '
        Me.colBehaviorRightImage.HeaderText = "Right Image"
        Me.colBehaviorRightImage.Name = "colBehaviorRightImage"
        Me.colBehaviorRightImage.Width = 70
        '
        'colBehaviorLeftImage
        '
        Me.colBehaviorLeftImage.HeaderText = "Left Image"
        Me.colBehaviorLeftImage.Name = "colBehaviorLeftImage"
        Me.colBehaviorLeftImage.Width = 63
        '
        'colBehaviorMovement
        '
        Me.colBehaviorMovement.HeaderText = "Movement Allowed"
        Me.colBehaviorMovement.Items.AddRange(New Object() {"None", "Horizontal Only", "Vertical Only", "Horizontal/Vertical", "Diagonal Only", "Diagonal/horizontal", "Diagonal/Vertical", "All", "MouseOver", "Sleep", "Dragged"})
        Me.colBehaviorMovement.Name = "colBehaviorMovement"
        Me.colBehaviorMovement.Width = 103
        '
        'colBehaviorStartSpeech
        '
        Me.colBehaviorStartSpeech.HeaderText = "Starting Speech"
        Me.colBehaviorStartSpeech.Name = "colBehaviorStartSpeech"
        Me.colBehaviorStartSpeech.Width = 89
        '
        'colBehaviorEndSpeech
        '
        Me.colBehaviorEndSpeech.HeaderText = "Ending Speech"
        Me.colBehaviorEndSpeech.Name = "colBehaviorEndSpeech"
        Me.colBehaviorEndSpeech.Width = 86
        '
        'colBehaviorFollow
        '
        Me.colBehaviorFollow.HeaderText = "Follows/Goto:"
        Me.colBehaviorFollow.Name = "colBehaviorFollow"
        Me.colBehaviorFollow.Width = 79
        '
        'colBehaviorLinked
        '
        Me.colBehaviorLinked.HeaderText = "Link to:"
        Me.colBehaviorLinked.Name = "colBehaviorLinked"
        Me.colBehaviorLinked.Width = 48
        '
        'colBehaviorLinkOrder
        '
        Me.colBehaviorLinkOrder.HeaderText = "Link Order"
        Me.colBehaviorLinkOrder.Name = "colBehaviorLinkOrder"
        Me.colBehaviorLinkOrder.ReadOnly = True
        Me.colBehaviorLinkOrder.Width = 81
        '
        'colBehaviorDoNotRunRandomly
        '
        Me.colBehaviorDoNotRunRandomly.HeaderText = "Don't run randomly"
        Me.colBehaviorDoNotRunRandomly.Name = "colBehaviorDoNotRunRandomly"
        Me.colBehaviorDoNotRunRandomly.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colBehaviorDoNotRunRandomly.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.colBehaviorDoNotRunRandomly.Width = 120
        '
        'colBehaviorDoNotRepeatAnimations
        '
        Me.colBehaviorDoNotRepeatAnimations.HeaderText = "Don't repeat animations"
        Me.colBehaviorDoNotRepeatAnimations.Name = "colBehaviorDoNotRepeatAnimations"
        Me.colBehaviorDoNotRepeatAnimations.Width = 124
        '
        'SpeechesGrid
        '
        Me.SpeechesGrid.AllowUserToAddRows = False
        Me.SpeechesGrid.AllowUserToResizeRows = False
        Me.SpeechesGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SpeechesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.SpeechesGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colSpeechName, Me.colSpeechOriginalName, Me.colSpeechGroup, Me.colSpeechGroupName, Me.colSpeechText, Me.colSpeechSoundFile, Me.colSpeechUseRandomly})
        Me.SpeechesGrid.Location = New System.Drawing.Point(3, 32)
        Me.SpeechesGrid.MultiSelect = False
        Me.SpeechesGrid.Name = "SpeechesGrid"
        Me.SpeechesGrid.Size = New System.Drawing.Size(322, 132)
        Me.SpeechesGrid.TabIndex = 1
        '
        'colSpeechName
        '
        Me.colSpeechName.HeaderText = "Name"
        Me.colSpeechName.Name = "colSpeechName"
        Me.colSpeechName.Width = 60
        '
        'colSpeechOriginalName
        '
        Me.colSpeechOriginalName.HeaderText = "Original Name"
        Me.colSpeechOriginalName.Name = "colSpeechOriginalName"
        Me.colSpeechOriginalName.Visible = False
        Me.colSpeechOriginalName.Width = 98
        '
        'colSpeechGroup
        '
        Me.colSpeechGroup.HeaderText = "Group"
        Me.colSpeechGroup.Name = "colSpeechGroup"
        Me.colSpeechGroup.Width = 61
        '
        'colSpeechGroupName
        '
        Me.colSpeechGroupName.HeaderText = "Group Name"
        Me.colSpeechGroupName.Name = "colSpeechGroupName"
        Me.colSpeechGroupName.ReadOnly = True
        Me.colSpeechGroupName.Width = 92
        '
        'colSpeechText
        '
        Me.colSpeechText.HeaderText = "Text"
        Me.colSpeechText.Name = "colSpeechText"
        Me.colSpeechText.Width = 53
        '
        'colSpeechSoundFile
        '
        Me.colSpeechSoundFile.HeaderText = "Sound File"
        Me.colSpeechSoundFile.Name = "colSpeechSoundFile"
        Me.colSpeechSoundFile.Width = 63
        '
        'colSpeechUseRandomly
        '
        Me.colSpeechUseRandomly.HeaderText = "Use Randomly"
        Me.colSpeechUseRandomly.Name = "colSpeechUseRandomly"
        Me.colSpeechUseRandomly.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colSpeechUseRandomly.Width = 82
        '
        'InteractionsGrid
        '
        Me.InteractionsGrid.AllowUserToAddRows = False
        Me.InteractionsGrid.AllowUserToResizeRows = False
        Me.InteractionsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InteractionsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.InteractionsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colInteractionName, Me.colInteractionOriginalName, Me.colInteractionChance, Me.colInteractionProximity, Me.colInteractionTargets, Me.colInteractionInteractWith, Me.colInteractionBehaviors, Me.colInteractionReactivationDelay})
        Me.InteractionsGrid.Location = New System.Drawing.Point(3, 32)
        Me.InteractionsGrid.MultiSelect = False
        Me.InteractionsGrid.Name = "InteractionsGrid"
        Me.InteractionsGrid.Size = New System.Drawing.Size(322, 132)
        Me.InteractionsGrid.TabIndex = 1
        '
        'colInteractionName
        '
        Me.colInteractionName.HeaderText = "Name"
        Me.colInteractionName.Name = "colInteractionName"
        Me.colInteractionName.Width = 60
        '
        'colInteractionOriginalName
        '
        Me.colInteractionOriginalName.HeaderText = "Original Name"
        Me.colInteractionOriginalName.Name = "colInteractionOriginalName"
        Me.colInteractionOriginalName.Visible = False
        Me.colInteractionOriginalName.Width = 98
        '
        'colInteractionChance
        '
        Me.colInteractionChance.HeaderText = "Chance"
        Me.colInteractionChance.Name = "colInteractionChance"
        Me.colInteractionChance.Width = 69
        '
        'colInteractionProximity
        '
        Me.colInteractionProximity.HeaderText = "Activation Proximity"
        Me.colInteractionProximity.Name = "colInteractionProximity"
        Me.colInteractionProximity.Width = 123
        '
        'colInteractionTargets
        '
        Me.colInteractionTargets.HeaderText = "Targets"
        Me.colInteractionTargets.Name = "colInteractionTargets"
        Me.colInteractionTargets.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colInteractionTargets.Width = 49
        '
        'colInteractionInteractWith
        '
        Me.colInteractionInteractWith.HeaderText = "Interact with:"
        Me.colInteractionInteractWith.Name = "colInteractionInteractWith"
        Me.colInteractionInteractWith.Width = 74
        '
        'colInteractionBehaviors
        '
        Me.colInteractionBehaviors.HeaderText = "Behaviors"
        Me.colInteractionBehaviors.Name = "colInteractionBehaviors"
        Me.colInteractionBehaviors.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colInteractionBehaviors.Width = 60
        '
        'colInteractionReactivationDelay
        '
        Me.colInteractionReactivationDelay.HeaderText = "Reactivation Delay"
        Me.colInteractionReactivationDelay.Name = "colInteractionReactivationDelay"
        Me.colInteractionReactivationDelay.Width = 122
        '
        'BehaviorsLabel
        '
        Me.BehaviorsLabel.AutoSize = True
        Me.BehaviorsLabel.Location = New System.Drawing.Point(3, 8)
        Me.BehaviorsLabel.Name = "BehaviorsLabel"
        Me.BehaviorsLabel.Size = New System.Drawing.Size(57, 13)
        Me.BehaviorsLabel.TabIndex = 0
        Me.BehaviorsLabel.Text = "Behaviors:"
        '
        'SpeechesLabel
        '
        Me.SpeechesLabel.AutoSize = True
        Me.SpeechesLabel.Location = New System.Drawing.Point(5, 8)
        Me.SpeechesLabel.Name = "SpeechesLabel"
        Me.SpeechesLabel.Size = New System.Drawing.Size(58, 13)
        Me.SpeechesLabel.TabIndex = 0
        Me.SpeechesLabel.Text = "Speeches:"
        '
        'InteractionsLabel
        '
        Me.InteractionsLabel.AutoSize = True
        Me.InteractionsLabel.Location = New System.Drawing.Point(3, 8)
        Me.InteractionsLabel.Name = "InteractionsLabel"
        Me.InteractionsLabel.Size = New System.Drawing.Size(65, 13)
        Me.InteractionsLabel.TabIndex = 0
        Me.InteractionsLabel.Text = "Interactions:"
        '
        'EffectsGrid
        '
        Me.EffectsGrid.AllowUserToAddRows = False
        Me.EffectsGrid.AllowUserToResizeRows = False
        Me.EffectsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EffectsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.EffectsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colEffectName, Me.colEffectOriginalName, Me.colEffectBehavior, Me.colEffectRightImage, Me.colEffectLeftImage, Me.colEffectDuration, Me.colEffectRepeatDelay, Me.colEffectLocationRight, Me.colEffectCenteringRight, Me.colEffectLocationLeft, Me.colEffectCenteringLeft, Me.colEffectFollowPony, Me.colEffectDoNotRepeatAnimations})
        Me.EffectsGrid.Location = New System.Drawing.Point(3, 32)
        Me.EffectsGrid.MultiSelect = False
        Me.EffectsGrid.Name = "EffectsGrid"
        Me.EffectsGrid.Size = New System.Drawing.Size(322, 132)
        Me.EffectsGrid.TabIndex = 1
        '
        'colEffectName
        '
        Me.colEffectName.HeaderText = "Name"
        Me.colEffectName.Name = "colEffectName"
        Me.colEffectName.Width = 60
        '
        'colEffectOriginalName
        '
        Me.colEffectOriginalName.HeaderText = "Original_Name"
        Me.colEffectOriginalName.Name = "colEffectOriginalName"
        Me.colEffectOriginalName.Visible = False
        Me.colEffectOriginalName.Width = 101
        '
        'colEffectBehavior
        '
        Me.colEffectBehavior.HeaderText = "Behavior"
        Me.colEffectBehavior.Name = "colEffectBehavior"
        Me.colEffectBehavior.Width = 55
        '
        'colEffectRightImage
        '
        Me.colEffectRightImage.HeaderText = "Right Image"
        Me.colEffectRightImage.Name = "colEffectRightImage"
        Me.colEffectRightImage.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colEffectRightImage.Width = 70
        '
        'colEffectLeftImage
        '
        Me.colEffectLeftImage.HeaderText = "Left Image"
        Me.colEffectLeftImage.Name = "colEffectLeftImage"
        Me.colEffectLeftImage.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colEffectLeftImage.Width = 63
        '
        'colEffectDuration
        '
        Me.colEffectDuration.HeaderText = "Duration"
        Me.colEffectDuration.Name = "colEffectDuration"
        Me.colEffectDuration.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colEffectDuration.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colEffectDuration.Width = 53
        '
        'colEffectRepeatDelay
        '
        Me.colEffectRepeatDelay.HeaderText = "Repeat Delay"
        Me.colEffectRepeatDelay.Name = "colEffectRepeatDelay"
        Me.colEffectRepeatDelay.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colEffectRepeatDelay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.colEffectRepeatDelay.Width = 78
        '
        'colEffectLocationRight
        '
        Me.colEffectLocationRight.HeaderText = "Location Heading Right"
        Me.colEffectLocationRight.Items.AddRange(New Object() {"Top", "Bottom", "Left", "Right", "Bottom Right", "Bottom Left", "Top Right", "Top Left", "Center", "Any", "Any-Not Center"})
        Me.colEffectLocationRight.Name = "colEffectLocationRight"
        Me.colEffectLocationRight.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colEffectLocationRight.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.colEffectLocationRight.Width = 144
        '
        'colEffectCenteringRight
        '
        Me.colEffectCenteringRight.HeaderText = "Centering Right"
        Me.colEffectCenteringRight.Items.AddRange(New Object() {"Top", "Bottom", "Left", "Right", "Bottom Right", "Bottom Left", "Top Right", "Top Left", "Center", "Any", "Any-Not Center"})
        Me.colEffectCenteringRight.Name = "colEffectCenteringRight"
        Me.colEffectCenteringRight.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colEffectCenteringRight.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.colEffectCenteringRight.Width = 105
        '
        'colEffectLocationLeft
        '
        Me.colEffectLocationLeft.HeaderText = "Location Heading Left"
        Me.colEffectLocationLeft.Items.AddRange(New Object() {"Top", "Bottom", "Left", "Right", "Bottom Right", "Bottom Left", "Top Right", "Top Left", "Center", "Any", "Any-Not Center"})
        Me.colEffectLocationLeft.Name = "colEffectLocationLeft"
        Me.colEffectLocationLeft.Width = 118
        '
        'colEffectCenteringLeft
        '
        Me.colEffectCenteringLeft.HeaderText = "Centering Left"
        Me.colEffectCenteringLeft.Items.AddRange(New Object() {"Top", "Bottom", "Left", "Right", "Bottom Right", "Bottom Left", "Top Right", "Top Left", "Center", "Any", "Any-Not Center"})
        Me.colEffectCenteringLeft.Name = "colEffectCenteringLeft"
        Me.colEffectCenteringLeft.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colEffectCenteringLeft.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.colEffectCenteringLeft.Width = 98
        '
        'colEffectFollowPony
        '
        Me.colEffectFollowPony.HeaderText = "Follow Pony?"
        Me.colEffectFollowPony.Name = "colEffectFollowPony"
        Me.colEffectFollowPony.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.colEffectFollowPony.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic
        Me.colEffectFollowPony.Width = 95
        '
        'colEffectDoNotRepeatAnimations
        '
        Me.colEffectDoNotRepeatAnimations.HeaderText = "Don't Repeat Animations"
        Me.colEffectDoNotRepeatAnimations.Name = "colEffectDoNotRepeatAnimations"
        Me.colEffectDoNotRepeatAnimations.Width = 130
        '
        'CurrentBehaviorLabel
        '
        Me.CurrentBehaviorLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.CurrentBehaviorLabel.AutoSize = True
        Me.CurrentBehaviorLabel.Location = New System.Drawing.Point(741, 247)
        Me.CurrentBehaviorLabel.Name = "CurrentBehaviorLabel"
        Me.CurrentBehaviorLabel.Size = New System.Drawing.Size(89, 13)
        Me.CurrentBehaviorLabel.TabIndex = 9
        Me.CurrentBehaviorLabel.Text = "Current Behavior:"
        '
        'CurrentBehaviorValueLabel
        '
        Me.CurrentBehaviorValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.CurrentBehaviorValueLabel.AutoSize = True
        Me.CurrentBehaviorValueLabel.Location = New System.Drawing.Point(836, 247)
        Me.CurrentBehaviorValueLabel.Name = "CurrentBehaviorValueLabel"
        Me.CurrentBehaviorValueLabel.Size = New System.Drawing.Size(27, 13)
        Me.CurrentBehaviorValueLabel.TabIndex = 10
        Me.CurrentBehaviorValueLabel.Text = "N/A"
        '
        'TimeLeftLabel
        '
        Me.TimeLeftLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.TimeLeftLabel.AutoSize = True
        Me.TimeLeftLabel.Location = New System.Drawing.Point(716, 274)
        Me.TimeLeftLabel.Name = "TimeLeftLabel"
        Me.TimeLeftLabel.Size = New System.Drawing.Size(54, 13)
        Me.TimeLeftLabel.TabIndex = 11
        Me.TimeLeftLabel.Text = "Time Left:"
        '
        'TimeLeftValueLabel
        '
        Me.TimeLeftValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.TimeLeftValueLabel.AutoSize = True
        Me.TimeLeftValueLabel.Location = New System.Drawing.Point(776, 274)
        Me.TimeLeftValueLabel.Name = "TimeLeftValueLabel"
        Me.TimeLeftValueLabel.Size = New System.Drawing.Size(27, 13)
        Me.TimeLeftValueLabel.TabIndex = 12
        Me.TimeLeftValueLabel.Text = "N/A"
        '
        'EffectsLabel
        '
        Me.EffectsLabel.AutoSize = True
        Me.EffectsLabel.Location = New System.Drawing.Point(3, 8)
        Me.EffectsLabel.Name = "EffectsLabel"
        Me.EffectsLabel.Size = New System.Drawing.Size(43, 13)
        Me.EffectsLabel.TabIndex = 0
        Me.EffectsLabel.Text = "Effects:"
        '
        'NewBehaviorButton
        '
        Me.NewBehaviorButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NewBehaviorButton.Location = New System.Drawing.Point(903, 3)
        Me.NewBehaviorButton.Name = "NewBehaviorButton"
        Me.NewBehaviorButton.Size = New System.Drawing.Size(90, 23)
        Me.NewBehaviorButton.TabIndex = 3
        Me.NewBehaviorButton.Text = "New Behavior"
        Me.NewBehaviorButton.UseVisualStyleBackColor = True
        '
        'NewSpeechButton
        '
        Me.NewSpeechButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NewSpeechButton.Location = New System.Drawing.Point(235, 3)
        Me.NewSpeechButton.Name = "NewSpeechButton"
        Me.NewSpeechButton.Size = New System.Drawing.Size(90, 23)
        Me.NewSpeechButton.TabIndex = 3
        Me.NewSpeechButton.Text = "New Speech"
        Me.NewSpeechButton.UseVisualStyleBackColor = True
        '
        'NewEffectButton
        '
        Me.NewEffectButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NewEffectButton.Location = New System.Drawing.Point(235, 3)
        Me.NewEffectButton.Name = "NewEffectButton"
        Me.NewEffectButton.Size = New System.Drawing.Size(90, 23)
        Me.NewEffectButton.TabIndex = 3
        Me.NewEffectButton.Text = "New Effect"
        Me.NewEffectButton.UseVisualStyleBackColor = True
        '
        'NewInteractionButton
        '
        Me.NewInteractionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NewInteractionButton.Location = New System.Drawing.Point(235, 3)
        Me.NewInteractionButton.Name = "NewInteractionButton"
        Me.NewInteractionButton.Size = New System.Drawing.Size(90, 23)
        Me.NewInteractionButton.TabIndex = 3
        Me.NewInteractionButton.Text = "New Interaction"
        Me.NewInteractionButton.UseVisualStyleBackColor = True
        '
        'SpeechesSwapButton
        '
        Me.SpeechesSwapButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SpeechesSwapButton.BackColor = System.Drawing.Color.GreenYellow
        Me.SpeechesSwapButton.Location = New System.Drawing.Point(154, 3)
        Me.SpeechesSwapButton.Name = "SpeechesSwapButton"
        Me.SpeechesSwapButton.Size = New System.Drawing.Size(75, 23)
        Me.SpeechesSwapButton.TabIndex = 2
        Me.SpeechesSwapButton.Text = "Swap"
        Me.SpeechesSwapButton.UseVisualStyleBackColor = False
        '
        'EffectsSwapButton
        '
        Me.EffectsSwapButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.EffectsSwapButton.BackColor = System.Drawing.Color.GreenYellow
        Me.EffectsSwapButton.Location = New System.Drawing.Point(154, 3)
        Me.EffectsSwapButton.Name = "EffectsSwapButton"
        Me.EffectsSwapButton.Size = New System.Drawing.Size(75, 23)
        Me.EffectsSwapButton.TabIndex = 2
        Me.EffectsSwapButton.Text = "Swap"
        Me.EffectsSwapButton.UseVisualStyleBackColor = False
        '
        'InteractionsSwapButton
        '
        Me.InteractionsSwapButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.InteractionsSwapButton.BackColor = System.Drawing.Color.GreenYellow
        Me.InteractionsSwapButton.Location = New System.Drawing.Point(154, 3)
        Me.InteractionsSwapButton.Name = "InteractionsSwapButton"
        Me.InteractionsSwapButton.Size = New System.Drawing.Size(75, 23)
        Me.InteractionsSwapButton.TabIndex = 2
        Me.InteractionsSwapButton.Text = "Swap"
        Me.InteractionsSwapButton.UseVisualStyleBackColor = False
        '
        'BehaviorsSwapButton
        '
        Me.BehaviorsSwapButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.BehaviorsSwapButton.BackColor = System.Drawing.Color.GreenYellow
        Me.BehaviorsSwapButton.Location = New System.Drawing.Point(822, 3)
        Me.BehaviorsSwapButton.Name = "BehaviorsSwapButton"
        Me.BehaviorsSwapButton.Size = New System.Drawing.Size(75, 23)
        Me.BehaviorsSwapButton.TabIndex = 2
        Me.BehaviorsSwapButton.Text = "Swap"
        Me.BehaviorsSwapButton.UseVisualStyleBackColor = False
        '
        'PausePonyButton
        '
        Me.PausePonyButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.PausePonyButton.Enabled = False
        Me.PausePonyButton.Location = New System.Drawing.Point(617, 243)
        Me.PausePonyButton.Name = "PausePonyButton"
        Me.PausePonyButton.Size = New System.Drawing.Size(90, 21)
        Me.PausePonyButton.TabIndex = 6
        Me.PausePonyButton.Text = "Pause Pony"
        Me.PausePonyButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.SaveButton.BackColor = System.Drawing.Color.IndianRed
        Me.SaveButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SaveButton.Location = New System.Drawing.Point(420, 270)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(100, 21)
        Me.SaveButton.TabIndex = 7
        Me.SaveButton.Text = "SAVE"
        Me.SaveButton.UseVisualStyleBackColor = False
        '
        'EditTagsButton
        '
        Me.EditTagsButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.EditTagsButton.Location = New System.Drawing.Point(420, 243)
        Me.EditTagsButton.Name = "EditTagsButton"
        Me.EditTagsButton.Size = New System.Drawing.Size(90, 21)
        Me.EditTagsButton.TabIndex = 4
        Me.EditTagsButton.Text = "Edit Tags"
        Me.EditTagsButton.UseVisualStyleBackColor = True
        '
        'RefreshButton
        '
        Me.RefreshButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.RefreshButton.BackColor = System.Drawing.Color.SpringGreen
        Me.RefreshButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RefreshButton.Location = New System.Drawing.Point(532, 270)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.Size = New System.Drawing.Size(175, 21)
        Me.RefreshButton.TabIndex = 8
        Me.RefreshButton.Text = "Apply/Refresh/Validate"
        Me.RefreshButton.UseVisualStyleBackColor = False
        '
        'GroupValueLabel
        '
        Me.GroupValueLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.GroupValueLabel.AutoSize = True
        Me.GroupValueLabel.Location = New System.Drawing.Point(860, 274)
        Me.GroupValueLabel.Name = "GroupValueLabel"
        Me.GroupValueLabel.Size = New System.Drawing.Size(27, 13)
        Me.GroupValueLabel.TabIndex = 14
        Me.GroupValueLabel.Text = "N/A"
        '
        'GroupLabel
        '
        Me.GroupLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.GroupLabel.AutoSize = True
        Me.GroupLabel.Location = New System.Drawing.Point(815, 274)
        Me.GroupLabel.Name = "GroupLabel"
        Me.GroupLabel.Size = New System.Drawing.Size(39, 13)
        Me.GroupLabel.TabIndex = 13
        Me.GroupLabel.Text = "Group:"
        '
        'SelectPonyGroup
        '
        Me.SelectPonyGroup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SelectPonyGroup.Controls.Add(Me.NewPonyButton)
        Me.SelectPonyGroup.Controls.Add(Me.PonyList)
        Me.SelectPonyGroup.Location = New System.Drawing.Point(3, 3)
        Me.SelectPonyGroup.Name = "SelectPonyGroup"
        Me.SelectPonyGroup.Size = New System.Drawing.Size(129, 288)
        Me.SelectPonyGroup.TabIndex = 0
        Me.SelectPonyGroup.TabStop = False
        Me.SelectPonyGroup.Text = "Select Pony"
        '
        'PonyPreviewGroup
        '
        Me.PonyPreviewGroup.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PonyPreviewGroup.Controls.Add(Me.PonyPreviewPanel)
        Me.PonyPreviewGroup.Location = New System.Drawing.Point(138, 3)
        Me.PonyPreviewGroup.Name = "PonyPreviewGroup"
        Me.PonyPreviewGroup.Size = New System.Drawing.Size(861, 234)
        Me.PonyPreviewGroup.TabIndex = 1
        Me.PonyPreviewGroup.TabStop = False
        Me.PonyPreviewGroup.Text = "Pony Preview"
        '
        'GridTable
        '
        Me.GridTable.ColumnCount = 3
        Me.GridTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.GridTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.GridTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.GridTable.Controls.Add(Me.InteractionsPanel, 2, 1)
        Me.GridTable.Controls.Add(Me.EffectsPanel, 1, 1)
        Me.GridTable.Controls.Add(Me.SpeechesPanel, 0, 1)
        Me.GridTable.Controls.Add(Me.BehaviorsPanel, 0, 0)
        Me.GridTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GridTable.Location = New System.Drawing.Point(3, 303)
        Me.GridTable.Name = "GridTable"
        Me.GridTable.RowCount = 2
        Me.GridTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55.0!))
        Me.GridTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.0!))
        Me.GridTable.Size = New System.Drawing.Size(1002, 384)
        Me.GridTable.TabIndex = 0
        '
        'InteractionsPanel
        '
        Me.InteractionsPanel.Controls.Add(Me.InteractionsLabel)
        Me.InteractionsPanel.Controls.Add(Me.InteractionsSwapButton)
        Me.InteractionsPanel.Controls.Add(Me.InteractionsGrid)
        Me.InteractionsPanel.Controls.Add(Me.NewInteractionButton)
        Me.InteractionsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.InteractionsPanel.Location = New System.Drawing.Point(671, 214)
        Me.InteractionsPanel.Name = "InteractionsPanel"
        Me.InteractionsPanel.Size = New System.Drawing.Size(328, 167)
        Me.InteractionsPanel.TabIndex = 3
        '
        'EffectsPanel
        '
        Me.EffectsPanel.Controls.Add(Me.EffectsLabel)
        Me.EffectsPanel.Controls.Add(Me.EffectsGrid)
        Me.EffectsPanel.Controls.Add(Me.EffectsSwapButton)
        Me.EffectsPanel.Controls.Add(Me.NewEffectButton)
        Me.EffectsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EffectsPanel.Location = New System.Drawing.Point(337, 214)
        Me.EffectsPanel.Name = "EffectsPanel"
        Me.EffectsPanel.Size = New System.Drawing.Size(328, 167)
        Me.EffectsPanel.TabIndex = 2
        '
        'SpeechesPanel
        '
        Me.SpeechesPanel.Controls.Add(Me.NewSpeechButton)
        Me.SpeechesPanel.Controls.Add(Me.SpeechesSwapButton)
        Me.SpeechesPanel.Controls.Add(Me.SpeechesLabel)
        Me.SpeechesPanel.Controls.Add(Me.SpeechesGrid)
        Me.SpeechesPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SpeechesPanel.Location = New System.Drawing.Point(3, 214)
        Me.SpeechesPanel.Name = "SpeechesPanel"
        Me.SpeechesPanel.Size = New System.Drawing.Size(328, 167)
        Me.SpeechesPanel.TabIndex = 1
        '
        'BehaviorsPanel
        '
        Me.GridTable.SetColumnSpan(Me.BehaviorsPanel, 3)
        Me.BehaviorsPanel.Controls.Add(Me.NewBehaviorButton)
        Me.BehaviorsPanel.Controls.Add(Me.BehaviorsSwapButton)
        Me.BehaviorsPanel.Controls.Add(Me.BehaviorsLabel)
        Me.BehaviorsPanel.Controls.Add(Me.BehaviorsGrid)
        Me.BehaviorsPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.BehaviorsPanel.Location = New System.Drawing.Point(3, 3)
        Me.BehaviorsPanel.Name = "BehaviorsPanel"
        Me.BehaviorsPanel.Size = New System.Drawing.Size(996, 205)
        Me.BehaviorsPanel.TabIndex = 0
        '
        'LayoutTable
        '
        Me.LayoutTable.ColumnCount = 1
        Me.LayoutTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.LayoutTable.Controls.Add(Me.GridTable, 0, 1)
        Me.LayoutTable.Controls.Add(Me.PonyPanel, 0, 0)
        Me.LayoutTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutTable.Location = New System.Drawing.Point(0, 0)
        Me.LayoutTable.Name = "LayoutTable"
        Me.LayoutTable.RowCount = 2
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300.0!))
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.LayoutTable.Size = New System.Drawing.Size(1008, 690)
        Me.LayoutTable.TabIndex = 1
        '
        'PonyPanel
        '
        Me.PonyPanel.Controls.Add(Me.ImagesButton)
        Me.PonyPanel.Controls.Add(Me.SelectPonyGroup)
        Me.PonyPanel.Controls.Add(Me.GroupValueLabel)
        Me.PonyPanel.Controls.Add(Me.TimeLeftValueLabel)
        Me.PonyPanel.Controls.Add(Me.PonyPreviewGroup)
        Me.PonyPanel.Controls.Add(Me.PausePonyButton)
        Me.PonyPanel.Controls.Add(Me.GroupLabel)
        Me.PonyPanel.Controls.Add(Me.TimeLeftLabel)
        Me.PonyPanel.Controls.Add(Me.PonyNameLabel)
        Me.PonyPanel.Controls.Add(Me.SaveButton)
        Me.PonyPanel.Controls.Add(Me.RefreshButton)
        Me.PonyPanel.Controls.Add(Me.CurrentBehaviorValueLabel)
        Me.PonyPanel.Controls.Add(Me.PonyName)
        Me.PonyPanel.Controls.Add(Me.EditTagsButton)
        Me.PonyPanel.Controls.Add(Me.CurrentBehaviorLabel)
        Me.PonyPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PonyPanel.Location = New System.Drawing.Point(3, 3)
        Me.PonyPanel.Name = "PonyPanel"
        Me.PonyPanel.Size = New System.Drawing.Size(1002, 294)
        Me.PonyPanel.TabIndex = 1
        '
        'ImagesButton
        '
        Me.ImagesButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.ImagesButton.Location = New System.Drawing.Point(520, 243)
        Me.ImagesButton.Name = "ImagesButton"
        Me.ImagesButton.Size = New System.Drawing.Size(90, 21)
        Me.ImagesButton.TabIndex = 5
        Me.ImagesButton.Text = "Images..."
        Me.ImagesButton.UseVisualStyleBackColor = True
        '
        'ImagesContextMenu
        '
        Me.ImagesContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ImageCentersMenuItem, Me.GifAlphaMenuItem, Me.GifViewerMenuItem})
        Me.ImagesContextMenu.Name = "ImagesContextMenu"
        Me.ImagesContextMenu.Size = New System.Drawing.Size(199, 70)
        '
        'ImageCentersMenuItem
        '
        Me.ImageCentersMenuItem.Name = "ImageCentersMenuItem"
        Me.ImageCentersMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.ImageCentersMenuItem.Text = "Set Image Centers"
        '
        'GifAlphaMenuItem
        '
        Me.GifAlphaMenuItem.Name = "GifAlphaMenuItem"
        Me.GifAlphaMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.GifAlphaMenuItem.Text = "Setup GIF Transparency"
        '
        'GifViewerMenuItem
        '
        Me.GifViewerMenuItem.Name = "GifViewerMenuItem"
        Me.GifViewerMenuItem.Size = New System.Drawing.Size(198, 22)
        Me.GifViewerMenuItem.Text = "View GIF Frames"
        '
        'PonyEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(1008, 690)
        Me.Controls.Add(Me.LayoutTable)
        Me.MinimumSize = New System.Drawing.Size(800, 500)
        Me.Name = "PonyEditor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pony Editor - Desktop Ponies"
        Me.SelectPonyGroup.ResumeLayout(False)
        Me.PonyPreviewGroup.ResumeLayout(False)
        Me.GridTable.ResumeLayout(False)
        Me.InteractionsPanel.ResumeLayout(False)
        Me.InteractionsPanel.PerformLayout()
        Me.EffectsPanel.ResumeLayout(False)
        Me.EffectsPanel.PerformLayout()
        Me.SpeechesPanel.ResumeLayout(False)
        Me.SpeechesPanel.PerformLayout()
        Me.BehaviorsPanel.ResumeLayout(False)
        Me.BehaviorsPanel.PerformLayout()
        Me.LayoutTable.ResumeLayout(False)
        Me.PonyPanel.ResumeLayout(False)
        Me.PonyPanel.PerformLayout()
        Me.ImagesContextMenu.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PonyPreviewPanel As System.Windows.Forms.Panel
    Friend WithEvents PonyList As System.Windows.Forms.ListView
    Friend WithEvents NewPonyButton As System.Windows.Forms.Button
    Friend WithEvents PonyName As System.Windows.Forms.TextBox
    Friend WithEvents PonyNameLabel As System.Windows.Forms.Label
    Friend WithEvents BehaviorsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents SpeechesGrid As System.Windows.Forms.DataGridView
    Friend WithEvents InteractionsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents BehaviorsLabel As System.Windows.Forms.Label
    Friend WithEvents SpeechesLabel As System.Windows.Forms.Label
    Friend WithEvents InteractionsLabel As System.Windows.Forms.Label
    Friend WithEvents EffectsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents CurrentBehaviorLabel As System.Windows.Forms.Label
    Friend WithEvents CurrentBehaviorValueLabel As System.Windows.Forms.Label
    Friend WithEvents TimeLeftLabel As System.Windows.Forms.Label
    Friend WithEvents TimeLeftValueLabel As System.Windows.Forms.Label
    Friend WithEvents EffectsLabel As System.Windows.Forms.Label
    Friend WithEvents NewBehaviorButton As System.Windows.Forms.Button
    Friend WithEvents NewSpeechButton As System.Windows.Forms.Button
    Friend WithEvents NewEffectButton As System.Windows.Forms.Button
    Friend WithEvents NewInteractionButton As System.Windows.Forms.Button
    Friend WithEvents SpeechesSwapButton As System.Windows.Forms.Button
    Friend WithEvents EffectsSwapButton As System.Windows.Forms.Button
    Friend WithEvents InteractionsSwapButton As System.Windows.Forms.Button
    Friend WithEvents BehaviorsSwapButton As System.Windows.Forms.Button
    Friend WithEvents PausePonyButton As System.Windows.Forms.Button
    Friend WithEvents OpenPictureDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents EditTagsButton As System.Windows.Forms.Button
    Friend WithEvents RefreshButton As System.Windows.Forms.Button
    Friend WithEvents GroupValueLabel As System.Windows.Forms.Label
    Friend WithEvents GroupLabel As System.Windows.Forms.Label
    Friend WithEvents SelectPonyGroup As System.Windows.Forms.GroupBox
    Friend WithEvents PonyPreviewGroup As System.Windows.Forms.GroupBox
    Friend WithEvents colSpeechName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSpeechOriginalName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSpeechGroup As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSpeechGroupName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSpeechText As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSpeechSoundFile As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colSpeechUseRandomly As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colInteractionName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colInteractionOriginalName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colInteractionChance As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colInteractionProximity As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colInteractionTargets As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colInteractionInteractWith As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colInteractionBehaviors As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colInteractionReactivationDelay As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colEffectName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colEffectOriginalName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colEffectBehavior As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colEffectRightImage As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colEffectLeftImage As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colEffectDuration As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colEffectRepeatDelay As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colEffectLocationRight As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colEffectCenteringRight As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colEffectLocationLeft As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colEffectCenteringLeft As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colEffectFollowPony As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colEffectDoNotRepeatAnimations As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colPony As System.Windows.Forms.ColumnHeader
    Friend WithEvents GridTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SpeechesPanel As System.Windows.Forms.Panel
    Friend WithEvents BehaviorsPanel As System.Windows.Forms.Panel
    Friend WithEvents EffectsPanel As System.Windows.Forms.Panel
    Friend WithEvents InteractionsPanel As System.Windows.Forms.Panel
    Friend WithEvents LayoutTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents PonyPanel As System.Windows.Forms.Panel
    Friend WithEvents colBehaviorActivate As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colBehaviorName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehaviorOriginalName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehaviorGroup As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehaviorGroupName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehaviorChance As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehaviorMaxDuration As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehaviorMinDuration As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehaviorSpeed As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehaviorRightImage As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colBehaviorLeftImage As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colBehaviorMovement As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colBehaviorStartSpeech As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colBehaviorEndSpeech As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colBehaviorFollow As System.Windows.Forms.DataGridViewButtonColumn
    Friend WithEvents colBehaviorLinked As System.Windows.Forms.DataGridViewComboBoxColumn
    Friend WithEvents colBehaviorLinkOrder As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colBehaviorDoNotRunRandomly As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colBehaviorDoNotRepeatAnimations As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents ImagesButton As System.Windows.Forms.Button
    Friend WithEvents ImagesContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents ImageCentersMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GifAlphaMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents GifViewerMenuItem As System.Windows.Forms.ToolStripMenuItem
End Class

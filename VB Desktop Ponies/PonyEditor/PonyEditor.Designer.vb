<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PonyEditor
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
        Me.PonyPreviewPanel = New System.Windows.Forms.Panel()
        Me.PonySelectionView = New System.Windows.Forms.ListView()
        Me.NewPonyButton = New System.Windows.Forms.Button()
        Me.PonyName = New System.Windows.Forms.TextBox()
        Me.PonyNameLabel = New System.Windows.Forms.Label()
        Me.PonyBehaviorsGrid = New System.Windows.Forms.DataGridView()
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
        Me.PonySpeechesGrid = New System.Windows.Forms.DataGridView()
        Me.colSpeechName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSpeechOriginalName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSpeechGroup = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSpeechGroupName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSpeechText = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colSpeechSoundFile = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colSpeechUseRandomly = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.PonyInteractionsGrid = New System.Windows.Forms.DataGridView()
        Me.colInteractionName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colInteractionOriginalName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colInteractionChance = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colInteractionProximity = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colInteractionTargets = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colInteractionInteractWith = New System.Windows.Forms.DataGridViewComboBoxColumn()
        Me.colInteractionBehaviors = New System.Windows.Forms.DataGridViewButtonColumn()
        Me.colInteractionReactivationDelay = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Slot0Label = New System.Windows.Forms.Label()
        Me.Slot1Label = New System.Windows.Forms.Label()
        Me.Slot3Label = New System.Windows.Forms.Label()
        Me.PonyEffectsGrid = New System.Windows.Forms.DataGridView()
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
        Me.Slot2Label = New System.Windows.Forms.Label()
        Me.NewBehaviorButton = New System.Windows.Forms.Button()
        Me.NewSpeechButton = New System.Windows.Forms.Button()
        Me.NewEffectButton = New System.Windows.Forms.Button()
        Me.NewInteractionButton = New System.Windows.Forms.Button()
        Me.Swap1_0 = New System.Windows.Forms.Button()
        Me.Swap2_0 = New System.Windows.Forms.Button()
        Me.Swap3_0 = New System.Windows.Forms.Button()
        Me.Swap0_1 = New System.Windows.Forms.Button()
        Me.PausePonyButton = New System.Windows.Forms.Button()
        Me.OpenPictureDialog = New System.Windows.Forms.OpenFileDialog()
        Me.SaveButton = New System.Windows.Forms.Button()
        Me.EditTagsButton = New System.Windows.Forms.Button()
        Me.SetImageCentersButton = New System.Windows.Forms.Button()
        Me.RefreshButton = New System.Windows.Forms.Button()
        Me.GroupValueLabel = New System.Windows.Forms.Label()
        Me.GroupLabel = New System.Windows.Forms.Label()
        Me.SelectPonyGroup = New System.Windows.Forms.GroupBox()
        Me.PonyPreviewGroup = New System.Windows.Forms.GroupBox()
        Me.SelectPonyGroup.SuspendLayout()
        Me.PonyPreviewGroup.SuspendLayout()
        Me.SuspendLayout()
        '
        'PonyPreviewPanel
        '
        Me.PonyPreviewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.PonyPreviewPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PonyPreviewPanel.Location = New System.Drawing.Point(3, 16)
        Me.PonyPreviewPanel.Name = "PonyPreviewPanel"
        Me.PonyPreviewPanel.Size = New System.Drawing.Size(783, 208)
        Me.PonyPreviewPanel.TabIndex = 0
        '
        'PonySelectionView
        '
        Me.PonySelectionView.Activation = System.Windows.Forms.ItemActivation.OneClick
        Me.PonySelectionView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PonySelectionView.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.PonySelectionView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None
        Me.PonySelectionView.HideSelection = False
        Me.PonySelectionView.LabelWrap = False
        Me.PonySelectionView.Location = New System.Drawing.Point(6, 19)
        Me.PonySelectionView.MultiSelect = False
        Me.PonySelectionView.Name = "PonySelectionView"
        Me.PonySelectionView.ShowGroups = False
        Me.PonySelectionView.Size = New System.Drawing.Size(117, 175)
        Me.PonySelectionView.TabIndex = 0
        Me.PonySelectionView.UseCompatibleStateImageBehavior = False
        '
        'NewPonyButton
        '
        Me.NewPonyButton.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NewPonyButton.Location = New System.Drawing.Point(6, 200)
        Me.NewPonyButton.Name = "NewPonyButton"
        Me.NewPonyButton.Size = New System.Drawing.Size(117, 21)
        Me.NewPonyButton.TabIndex = 1
        Me.NewPonyButton.Text = "Create NEW Pony"
        Me.NewPonyButton.UseVisualStyleBackColor = True
        '
        'PonyName
        '
        Me.PonyName.Location = New System.Drawing.Point(234, 254)
        Me.PonyName.Name = "PonyName"
        Me.PonyName.Size = New System.Drawing.Size(125, 20)
        Me.PonyName.TabIndex = 3
        '
        'PonyNameLabel
        '
        Me.PonyNameLabel.AutoSize = True
        Me.PonyNameLabel.Location = New System.Drawing.Point(190, 257)
        Me.PonyNameLabel.Name = "PonyNameLabel"
        Me.PonyNameLabel.Size = New System.Drawing.Size(38, 13)
        Me.PonyNameLabel.TabIndex = 2
        Me.PonyNameLabel.Text = "Name:"
        '
        'PonyBehaviorsGrid
        '
        Me.PonyBehaviorsGrid.AllowUserToAddRows = False
        Me.PonyBehaviorsGrid.AllowUserToResizeRows = False
        Me.PonyBehaviorsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PonyBehaviorsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.PonyBehaviorsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colBehaviorActivate, Me.colBehaviorName, Me.colBehaviorOriginalName, Me.colBehaviorGroup, Me.colBehaviorGroupName, Me.colBehaviorChance, Me.colBehaviorMaxDuration, Me.colBehaviorMinDuration, Me.colBehaviorSpeed, Me.colBehaviorRightImage, Me.colBehaviorLeftImage, Me.colBehaviorMovement, Me.colBehaviorStartSpeech, Me.colBehaviorEndSpeech, Me.colBehaviorFollow, Me.colBehaviorLinked, Me.colBehaviorLinkOrder, Me.colBehaviorDoNotRunRandomly, Me.colBehaviorDoNotRepeatAnimations})
        Me.PonyBehaviorsGrid.Location = New System.Drawing.Point(12, 315)
        Me.PonyBehaviorsGrid.MultiSelect = False
        Me.PonyBehaviorsGrid.Name = "PonyBehaviorsGrid"
        Me.PonyBehaviorsGrid.Size = New System.Drawing.Size(915, 172)
        Me.PonyBehaviorsGrid.TabIndex = 16
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
        Me.colBehaviorMovement.Items.AddRange(New Object() {"None", "Horizontal Only", "Vertical Only", "Horizontal Vertical", "Diagonal Only", "Diagonal/horizontal", "Diagonal/Vertical", "All", "MouseOver", "Sleep", "Dragged"})
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
        'PonySpeechesGrid
        '
        Me.PonySpeechesGrid.AllowUserToAddRows = False
        Me.PonySpeechesGrid.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PonySpeechesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.PonySpeechesGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colSpeechName, Me.colSpeechOriginalName, Me.colSpeechGroup, Me.colSpeechGroupName, Me.colSpeechText, Me.colSpeechSoundFile, Me.colSpeechUseRandomly})
        Me.PonySpeechesGrid.Location = New System.Drawing.Point(7, 528)
        Me.PonySpeechesGrid.MultiSelect = False
        Me.PonySpeechesGrid.Name = "PonySpeechesGrid"
        Me.PonySpeechesGrid.Size = New System.Drawing.Size(307, 150)
        Me.PonySpeechesGrid.TabIndex = 20
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
        'PonyInteractionsGrid
        '
        Me.PonyInteractionsGrid.AllowUserToAddRows = False
        Me.PonyInteractionsGrid.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PonyInteractionsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.PonyInteractionsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colInteractionName, Me.colInteractionOriginalName, Me.colInteractionChance, Me.colInteractionProximity, Me.colInteractionTargets, Me.colInteractionInteractWith, Me.colInteractionBehaviors, Me.colInteractionReactivationDelay})
        Me.PonyInteractionsGrid.Location = New System.Drawing.Point(634, 528)
        Me.PonyInteractionsGrid.MultiSelect = False
        Me.PonyInteractionsGrid.Name = "PonyInteractionsGrid"
        Me.PonyInteractionsGrid.Size = New System.Drawing.Size(302, 150)
        Me.PonyInteractionsGrid.TabIndex = 28
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
        'Slot0Label
        '
        Me.Slot0Label.AutoSize = True
        Me.Slot0Label.Location = New System.Drawing.Point(160, 299)
        Me.Slot0Label.Name = "Slot0Label"
        Me.Slot0Label.Size = New System.Drawing.Size(57, 13)
        Me.Slot0Label.TabIndex = 15
        Me.Slot0Label.Text = "Behaviors:"
        '
        'Slot1Label
        '
        Me.Slot1Label.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Slot1Label.AutoSize = True
        Me.Slot1Label.Location = New System.Drawing.Point(24, 512)
        Me.Slot1Label.Name = "Slot1Label"
        Me.Slot1Label.Size = New System.Drawing.Size(58, 13)
        Me.Slot1Label.TabIndex = 19
        Me.Slot1Label.Text = "Speeches:"
        '
        'Slot3Label
        '
        Me.Slot3Label.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Slot3Label.AutoSize = True
        Me.Slot3Label.Location = New System.Drawing.Point(647, 512)
        Me.Slot3Label.Name = "Slot3Label"
        Me.Slot3Label.Size = New System.Drawing.Size(65, 13)
        Me.Slot3Label.TabIndex = 27
        Me.Slot3Label.Text = "Interactions:"
        '
        'PonyEffectsGrid
        '
        Me.PonyEffectsGrid.AllowUserToAddRows = False
        Me.PonyEffectsGrid.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PonyEffectsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.PonyEffectsGrid.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colEffectName, Me.colEffectOriginalName, Me.colEffectBehavior, Me.colEffectRightImage, Me.colEffectLeftImage, Me.colEffectDuration, Me.colEffectRepeatDelay, Me.colEffectLocationRight, Me.colEffectCenteringRight, Me.colEffectLocationLeft, Me.colEffectCenteringLeft, Me.colEffectFollowPony, Me.colEffectDoNotRepeatAnimations})
        Me.PonyEffectsGrid.Location = New System.Drawing.Point(329, 528)
        Me.PonyEffectsGrid.MultiSelect = False
        Me.PonyEffectsGrid.Name = "PonyEffectsGrid"
        Me.PonyEffectsGrid.Size = New System.Drawing.Size(291, 150)
        Me.PonyEffectsGrid.TabIndex = 24
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
        Me.CurrentBehaviorLabel.AutoSize = True
        Me.CurrentBehaviorLabel.Location = New System.Drawing.Point(748, 245)
        Me.CurrentBehaviorLabel.Name = "CurrentBehaviorLabel"
        Me.CurrentBehaviorLabel.Size = New System.Drawing.Size(89, 13)
        Me.CurrentBehaviorLabel.TabIndex = 9
        Me.CurrentBehaviorLabel.Text = "Current Behavior:"
        '
        'CurrentBehaviorValueLabel
        '
        Me.CurrentBehaviorValueLabel.AutoSize = True
        Me.CurrentBehaviorValueLabel.Location = New System.Drawing.Point(843, 245)
        Me.CurrentBehaviorValueLabel.Name = "CurrentBehaviorValueLabel"
        Me.CurrentBehaviorValueLabel.Size = New System.Drawing.Size(27, 13)
        Me.CurrentBehaviorValueLabel.TabIndex = 10
        Me.CurrentBehaviorValueLabel.Text = "N/A"
        '
        'TimeLeftLabel
        '
        Me.TimeLeftLabel.AutoSize = True
        Me.TimeLeftLabel.Location = New System.Drawing.Point(728, 261)
        Me.TimeLeftLabel.Name = "TimeLeftLabel"
        Me.TimeLeftLabel.Size = New System.Drawing.Size(54, 13)
        Me.TimeLeftLabel.TabIndex = 11
        Me.TimeLeftLabel.Text = "Time Left:"
        '
        'TimeLeftValueLabel
        '
        Me.TimeLeftValueLabel.AutoSize = True
        Me.TimeLeftValueLabel.Location = New System.Drawing.Point(788, 261)
        Me.TimeLeftValueLabel.Name = "TimeLeftValueLabel"
        Me.TimeLeftValueLabel.Size = New System.Drawing.Size(27, 13)
        Me.TimeLeftValueLabel.TabIndex = 12
        Me.TimeLeftValueLabel.Text = "N/A"
        '
        'Slot2Label
        '
        Me.Slot2Label.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Slot2Label.AutoSize = True
        Me.Slot2Label.Location = New System.Drawing.Point(356, 512)
        Me.Slot2Label.Name = "Slot2Label"
        Me.Slot2Label.Size = New System.Drawing.Size(43, 13)
        Me.Slot2Label.TabIndex = 23
        Me.Slot2Label.Text = "Effects:"
        '
        'NewBehaviorButton
        '
        Me.NewBehaviorButton.Location = New System.Drawing.Point(830, 286)
        Me.NewBehaviorButton.Name = "NewBehaviorButton"
        Me.NewBehaviorButton.Size = New System.Drawing.Size(93, 23)
        Me.NewBehaviorButton.TabIndex = 18
        Me.NewBehaviorButton.Text = "New Behavior"
        Me.NewBehaviorButton.UseVisualStyleBackColor = True
        '
        'NewSpeechButton
        '
        Me.NewSpeechButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.NewSpeechButton.Location = New System.Drawing.Point(196, 502)
        Me.NewSpeechButton.Name = "NewSpeechButton"
        Me.NewSpeechButton.Size = New System.Drawing.Size(90, 23)
        Me.NewSpeechButton.TabIndex = 22
        Me.NewSpeechButton.Text = "New Speech"
        Me.NewSpeechButton.UseVisualStyleBackColor = True
        '
        'NewEffectButton
        '
        Me.NewEffectButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.NewEffectButton.Location = New System.Drawing.Point(508, 502)
        Me.NewEffectButton.Name = "NewEffectButton"
        Me.NewEffectButton.Size = New System.Drawing.Size(96, 23)
        Me.NewEffectButton.TabIndex = 26
        Me.NewEffectButton.Text = "New Effect"
        Me.NewEffectButton.UseVisualStyleBackColor = True
        '
        'NewInteractionButton
        '
        Me.NewInteractionButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.NewInteractionButton.Location = New System.Drawing.Point(817, 502)
        Me.NewInteractionButton.Name = "NewInteractionButton"
        Me.NewInteractionButton.Size = New System.Drawing.Size(92, 23)
        Me.NewInteractionButton.TabIndex = 30
        Me.NewInteractionButton.Text = "New Interaction"
        Me.NewInteractionButton.UseVisualStyleBackColor = True
        '
        'Swap1_0
        '
        Me.Swap1_0.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Swap1_0.BackColor = System.Drawing.Color.GreenYellow
        Me.Swap1_0.Location = New System.Drawing.Point(106, 502)
        Me.Swap1_0.Name = "Swap1_0"
        Me.Swap1_0.Size = New System.Drawing.Size(73, 23)
        Me.Swap1_0.TabIndex = 21
        Me.Swap1_0.Text = "Swap"
        Me.Swap1_0.UseVisualStyleBackColor = False
        '
        'Swap2_0
        '
        Me.Swap2_0.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Swap2_0.BackColor = System.Drawing.Color.GreenYellow
        Me.Swap2_0.Location = New System.Drawing.Point(417, 502)
        Me.Swap2_0.Name = "Swap2_0"
        Me.Swap2_0.Size = New System.Drawing.Size(75, 23)
        Me.Swap2_0.TabIndex = 25
        Me.Swap2_0.Text = "Swap"
        Me.Swap2_0.UseVisualStyleBackColor = False
        '
        'Swap3_0
        '
        Me.Swap3_0.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Swap3_0.BackColor = System.Drawing.Color.GreenYellow
        Me.Swap3_0.Location = New System.Drawing.Point(731, 502)
        Me.Swap3_0.Name = "Swap3_0"
        Me.Swap3_0.Size = New System.Drawing.Size(68, 23)
        Me.Swap3_0.TabIndex = 29
        Me.Swap3_0.Text = "Swap"
        Me.Swap3_0.UseVisualStyleBackColor = False
        '
        'Swap0_1
        '
        Me.Swap0_1.BackColor = System.Drawing.Color.GreenYellow
        Me.Swap0_1.Location = New System.Drawing.Point(731, 286)
        Me.Swap0_1.Name = "Swap0_1"
        Me.Swap0_1.Size = New System.Drawing.Size(93, 23)
        Me.Swap0_1.TabIndex = 17
        Me.Swap0_1.Text = "Swap"
        Me.Swap0_1.UseVisualStyleBackColor = False
        '
        'PausePonyButton
        '
        Me.PausePonyButton.Location = New System.Drawing.Point(577, 253)
        Me.PausePonyButton.Name = "PausePonyButton"
        Me.PausePonyButton.Size = New System.Drawing.Size(75, 21)
        Me.PausePonyButton.TabIndex = 6
        Me.PausePonyButton.Text = "Pause Pony"
        Me.PausePonyButton.UseVisualStyleBackColor = True
        '
        'SaveButton
        '
        Me.SaveButton.BackColor = System.Drawing.Color.IndianRed
        Me.SaveButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SaveButton.Location = New System.Drawing.Point(365, 280)
        Me.SaveButton.Name = "SaveButton"
        Me.SaveButton.Size = New System.Drawing.Size(100, 21)
        Me.SaveButton.TabIndex = 7
        Me.SaveButton.Text = "SAVE"
        Me.SaveButton.UseVisualStyleBackColor = False
        '
        'EditTagsButton
        '
        Me.EditTagsButton.Location = New System.Drawing.Point(365, 253)
        Me.EditTagsButton.Name = "EditTagsButton"
        Me.EditTagsButton.Size = New System.Drawing.Size(75, 21)
        Me.EditTagsButton.TabIndex = 4
        Me.EditTagsButton.Text = "Edit Tags"
        Me.EditTagsButton.UseVisualStyleBackColor = True
        '
        'SetImageCentersButton
        '
        Me.SetImageCentersButton.Location = New System.Drawing.Point(446, 253)
        Me.SetImageCentersButton.Name = "SetImageCentersButton"
        Me.SetImageCentersButton.Size = New System.Drawing.Size(125, 21)
        Me.SetImageCentersButton.TabIndex = 5
        Me.SetImageCentersButton.Text = "Set Image Centers"
        Me.SetImageCentersButton.UseVisualStyleBackColor = True
        '
        'RefreshButton
        '
        Me.RefreshButton.BackColor = System.Drawing.Color.SpringGreen
        Me.RefreshButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.RefreshButton.Location = New System.Drawing.Point(477, 280)
        Me.RefreshButton.Name = "RefreshButton"
        Me.RefreshButton.Size = New System.Drawing.Size(175, 21)
        Me.RefreshButton.TabIndex = 8
        Me.RefreshButton.Text = "Apply/Refresh/Validate"
        Me.RefreshButton.UseVisualStyleBackColor = False
        '
        'GroupValueLabel
        '
        Me.GroupValueLabel.AutoSize = True
        Me.GroupValueLabel.Location = New System.Drawing.Point(872, 261)
        Me.GroupValueLabel.Name = "GroupValueLabel"
        Me.GroupValueLabel.Size = New System.Drawing.Size(27, 13)
        Me.GroupValueLabel.TabIndex = 14
        Me.GroupValueLabel.Text = "N/A"
        '
        'GroupLabel
        '
        Me.GroupLabel.AutoSize = True
        Me.GroupLabel.Location = New System.Drawing.Point(827, 261)
        Me.GroupLabel.Name = "GroupLabel"
        Me.GroupLabel.Size = New System.Drawing.Size(39, 13)
        Me.GroupLabel.TabIndex = 13
        Me.GroupLabel.Text = "Group:"
        '
        'SelectPonyGroup
        '
        Me.SelectPonyGroup.Controls.Add(Me.NewPonyButton)
        Me.SelectPonyGroup.Controls.Add(Me.PonySelectionView)
        Me.SelectPonyGroup.Location = New System.Drawing.Point(12, 12)
        Me.SelectPonyGroup.Name = "SelectPonyGroup"
        Me.SelectPonyGroup.Size = New System.Drawing.Size(129, 227)
        Me.SelectPonyGroup.TabIndex = 0
        Me.SelectPonyGroup.TabStop = False
        Me.SelectPonyGroup.Text = "Select Pony"
        '
        'PonyPreviewGroup
        '
        Me.PonyPreviewGroup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PonyPreviewGroup.Controls.Add(Me.PonyPreviewPanel)
        Me.PonyPreviewGroup.Location = New System.Drawing.Point(147, 12)
        Me.PonyPreviewGroup.Name = "PonyPreviewGroup"
        Me.PonyPreviewGroup.Size = New System.Drawing.Size(789, 227)
        Me.PonyPreviewGroup.TabIndex = 1
        Me.PonyPreviewGroup.TabStop = False
        Me.PonyPreviewGroup.Text = "Pony Preview"
        '
        'PonyEditor
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(948, 690)
        Me.Controls.Add(Me.PonyPreviewGroup)
        Me.Controls.Add(Me.SelectPonyGroup)
        Me.Controls.Add(Me.GroupValueLabel)
        Me.Controls.Add(Me.GroupLabel)
        Me.Controls.Add(Me.RefreshButton)
        Me.Controls.Add(Me.SetImageCentersButton)
        Me.Controls.Add(Me.EditTagsButton)
        Me.Controls.Add(Me.SaveButton)
        Me.Controls.Add(Me.PausePonyButton)
        Me.Controls.Add(Me.Swap0_1)
        Me.Controls.Add(Me.Swap3_0)
        Me.Controls.Add(Me.Swap2_0)
        Me.Controls.Add(Me.Swap1_0)
        Me.Controls.Add(Me.NewInteractionButton)
        Me.Controls.Add(Me.NewEffectButton)
        Me.Controls.Add(Me.NewSpeechButton)
        Me.Controls.Add(Me.NewBehaviorButton)
        Me.Controls.Add(Me.Slot2Label)
        Me.Controls.Add(Me.TimeLeftValueLabel)
        Me.Controls.Add(Me.TimeLeftLabel)
        Me.Controls.Add(Me.CurrentBehaviorValueLabel)
        Me.Controls.Add(Me.CurrentBehaviorLabel)
        Me.Controls.Add(Me.PonyEffectsGrid)
        Me.Controls.Add(Me.Slot3Label)
        Me.Controls.Add(Me.Slot1Label)
        Me.Controls.Add(Me.Slot0Label)
        Me.Controls.Add(Me.PonyInteractionsGrid)
        Me.Controls.Add(Me.PonySpeechesGrid)
        Me.Controls.Add(Me.PonyBehaviorsGrid)
        Me.Controls.Add(Me.PonyNameLabel)
        Me.Controls.Add(Me.PonyName)
        Me.MinimumSize = New System.Drawing.Size(964, 726)
        Me.Name = "PonyEditor"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pony Editor - Desktop Ponies"
        Me.SelectPonyGroup.ResumeLayout(False)
        Me.PonyPreviewGroup.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PonyPreviewPanel As System.Windows.Forms.Panel
    Friend WithEvents PonySelectionView As System.Windows.Forms.ListView
    Friend WithEvents NewPonyButton As System.Windows.Forms.Button
    Friend WithEvents PonyName As System.Windows.Forms.TextBox
    Friend WithEvents PonyNameLabel As System.Windows.Forms.Label
    Friend WithEvents PonyBehaviorsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents PonySpeechesGrid As System.Windows.Forms.DataGridView
    Friend WithEvents PonyInteractionsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents Slot0Label As System.Windows.Forms.Label
    Friend WithEvents Slot1Label As System.Windows.Forms.Label
    Friend WithEvents Slot3Label As System.Windows.Forms.Label
    Friend WithEvents PonyEffectsGrid As System.Windows.Forms.DataGridView
    Friend WithEvents CurrentBehaviorLabel As System.Windows.Forms.Label
    Friend WithEvents CurrentBehaviorValueLabel As System.Windows.Forms.Label
    Friend WithEvents TimeLeftLabel As System.Windows.Forms.Label
    Friend WithEvents TimeLeftValueLabel As System.Windows.Forms.Label
    Friend WithEvents Slot2Label As System.Windows.Forms.Label
    Friend WithEvents NewBehaviorButton As System.Windows.Forms.Button
    Friend WithEvents NewSpeechButton As System.Windows.Forms.Button
    Friend WithEvents NewEffectButton As System.Windows.Forms.Button
    Friend WithEvents NewInteractionButton As System.Windows.Forms.Button
    Friend WithEvents Swap1_0 As System.Windows.Forms.Button
    Friend WithEvents Swap2_0 As System.Windows.Forms.Button
    Friend WithEvents Swap3_0 As System.Windows.Forms.Button
    Friend WithEvents Swap0_1 As System.Windows.Forms.Button
    Friend WithEvents PausePonyButton As System.Windows.Forms.Button
    Friend WithEvents OpenPictureDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents SaveButton As System.Windows.Forms.Button
    Friend WithEvents EditTagsButton As System.Windows.Forms.Button
    Friend WithEvents SetImageCentersButton As System.Windows.Forms.Button
    Friend WithEvents RefreshButton As System.Windows.Forms.Button
    Friend WithEvents GroupValueLabel As System.Windows.Forms.Label
    Friend WithEvents GroupLabel As System.Windows.Forms.Label
    Friend WithEvents SelectPonyGroup As System.Windows.Forms.GroupBox
    Friend WithEvents PonyPreviewGroup As System.Windows.Forms.GroupBox
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
End Class

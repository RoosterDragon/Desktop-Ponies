Imports System.Globalization
Imports System.IO
Imports CSDesktopPonies.SpriteManagement

Public Class PonyEditor
    Private pe_animator As PonyEditorAnimator
    Private pe_interface As ISpriteCollectionView

    Private ponyBases As PonyBase()
    Private ponyImageList As ImageList
    Private infoGrids As PonyInfoGrid()
    Private loaded As Boolean

    ''' <summary>
    ''' Keep track of when the grids are being updated when loading a pony, otherwise we incorrectly think the user is making changes.
    ''' </summary>
    Private alreadyUpdating As Boolean
    ''' <summary>
    ''' Keep track of if changes have been made since the last save.
    ''' </summary>
    Private hasSaved As Boolean = True
    Private _changesMade As Boolean
    ''' <summary>
    ''' Keep track of any saves made at all in the editor. If so, we'll need to reload files when we quit.
    ''' </summary>
    Public ReadOnly Property ChangesMade As Boolean
        Get
            Return _changesMade
        End Get
    End Property
    Private _isClosing As Boolean
    Public ReadOnly Property IsClosing As Boolean
        Get
            Return _isClosing
        End Get
    End Property
    Private _previewPony As Pony
    ''' <summary>
    ''' The pony displayed in the preview window and where settings are changed (live).
    ''' </summary>
    Public ReadOnly Property PreviewPony As Pony
        Get
            Return _previewPony
        End Get
    End Property
    Public ReadOnly Property PreviewPonyBase As MutablePonyBase
        Get
            If PreviewPony IsNot Nothing Then
                Return DirectCast(PreviewPony.Base, MutablePonyBase)
            Else
                Return Nothing
            End If
        End Get
    End Property

    ''' <summary>
    ''' Used so we can swap grid positions and keep track of how everything is sorted when we refresh.
    ''' </summary>
    Private Class PonyInfoGrid
        Public Property Grid As DataGridView
        Public Property SortColumn As DataGridViewColumn
        Public Property SortOrder As SortOrder
        Public Sub New(_grid As DataGridView)
            Grid = _grid
        End Sub
    End Class

    Public Sub New(ponyBaseCollection As IEnumerable(Of PonyBase))
        Argument.EnsureNotNull(ponyBaseCollection, "ponyBaseCollection")
        InitializeComponent()
        Icon = My.Resources.Twilight
        ponyBases = ponyBaseCollection.ToArray()
    End Sub

    Private Sub PonyEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BeginInvoke(New MethodInvoker(AddressOf LoadInternal))
    End Sub

    Private Sub LoadInternal()
        Enabled = False
        Update()
        Try
            For Each value In DirectCast([Enum].GetValues(GetType(Interaction.TargetActivation)), Interaction.TargetActivation())
                colInteractionInteractWith.Items.Add(value.ToString())
            Next

            infoGrids = {New PonyInfoGrid(BehaviorsGrid), New PonyInfoGrid(SpeechesGrid),
                         New PonyInfoGrid(EffectsGrid), New PonyInfoGrid(InteractionsGrid)}

            'add all possible ponies to the selection window.
            Const size = 50
            ponyImageList = New ImageList() With {.ImageSize = New Size(size, size)}
            For Each ponyBase In ponyBases
                Dim imagePath = ponyBase.Behaviors(0).LeftImagePath

                Dim dstImage = New Bitmap(size, size)
                Using srcImage = Bitmap.FromFile(imagePath), g = Graphics.FromImage(dstImage)
                    g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                    g.Clear(Me.PonyList.BackColor)
                    g.DrawImage(srcImage, 0, 0, size, size)
                End Using

                ponyImageList.Images.Add(dstImage)
            Next
            PonyList.LargeImageList = ponyImageList
            PonyList.SmallImageList = ponyImageList

            PonyList.SuspendLayout()
            For i = 0 To ponyBases.Length - 1
                PonyList.Items.Add(New ListViewItem(ponyBases(i).Directory, i) With {.Tag = ponyBases(i)})
            Next
            PonyList.ResumeLayout()

            loaded = True
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error attempting to load the editor. It will now close.")
            Me.Close()
        End Try

        ' TEMP: Convert filenames to lowercase.
        'For i = 0 To Pony_Selection_View.Items.Count - 1
        '    Pony_Selection_View.SelectedIndices.Clear()
        '    Pony_Selection_View.SelectedIndices.Add(i)
        '    For Each behavior As Pony.Behavior In Preview_Pony.Behaviors
        '        behavior.left_image_path = behavior.left_image_path.ToLowerInvariant().Replace(" ", "_").Replace("-", "_")
        '        behavior.right_image_path = behavior.right_image_path.ToLowerInvariant().Replace(" ", "_").Replace("-", "_")
        '        For Each effect As Effect In behavior.Effects
        '            effect.left_image_path = effect.left_image_path.ToLowerInvariant().Replace(" ", "_").Replace("-", "_")
        '            effect.right_image_path = effect.right_image_path.ToLowerInvariant().Replace(" ", "_").Replace("-", "_")
        '        Next
        '    Next
        '    Save_Button_Click(sender, e)
        'Next

        pe_interface = Options.GetInterface()
        pe_interface.Topmost = True
        pe_animator = New PonyEditorAnimator(Me, pe_interface, Nothing)
        AddHandler pe_animator.AnimationFinished, AddressOf PonyEditorAnimator_AnimationFinished
        Enabled = True
    End Sub

    Private Sub PonyList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PonyList.SelectedIndexChanged
        Try
            If PonyList.SelectedItems.Count = 0 Then Return

            If PreventStateChange("Save changes before loading a different pony?") Then Return

            LoadPony()

            hasSaved = True
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error attempting to select pony.")
        End Try
    End Sub

    Private Sub LoadPony()
        Dim worker = IdleWorker.CurrentThreadWorker
        worker.QueueTask(Sub()
                             Pony.CurrentViewer = pe_interface
                             Pony.CurrentAnimator = pe_animator
                             If Not pe_animator.Started Then
                                 pe_animator.Start()
                                 PausePonyButton.Enabled = True
                             Else
                                 pe_animator.Clear()
                                 pe_animator.Pause(True)
                             End If

                             Enabled = False
                         End Sub)
        worker.QueueTask(Sub()
                             SaveSortOrder()
                             RestoreSortOrder()
                         End Sub)

        Dim directory = DirectCast(PonyList.SelectedItems(0).Tag, PonyBase).Directory
        Dim ponyLoadTask =
            New Threading.Tasks.Task(
            Sub()
                _previewPony = New Pony(New MutablePonyBase(directory))
                If PreviewPony.Behaviors.Count = 0 Then
                    PreviewPonyBase.AddBehavior("Default", 1, 60, 60, 0, AllowedMoves.None, "", "", "")
                End If
            End Sub)
        ponyLoadTask.ContinueWith(
            Sub()
                worker.QueueTask(Sub()
                                     pe_animator.AddPony(PreviewPony)
                                     LoadParameters(PreviewPony)

                                     PausePonyButton.Text = "Pause Pony"
                                     Enabled = True
                                     pe_animator.Resume()
                                 End Sub)
            End Sub)
        ponyLoadTask.Start()
    End Sub

    Private Sub PonyEditor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If PreventStateChange("Save changes before closing?") Then
            e.Cancel = True
            Return
        End If
        _isClosing = True
        If pe_animator IsNot Nothing AndAlso Not pe_animator.Disposed Then
            If pe_animator.Started Then pe_animator.Pause(True)
            RemoveHandler pe_animator.AnimationFinished, AddressOf PonyEditorAnimator_AnimationFinished
        End If
    End Sub

    Friend Function GetPreviewWindowScreenRectangle() As Rectangle
        ' Traverses the parent controls to find the total offset.
        ' Equivalent to the below, but does not requiring invoking:
        'Return PonyPreviewPanel.RectangleToScreen(PonyPreviewPanel.ClientRectangle)
        Dim location = PonyPreviewPanel.Location
        Dim container = PonyPreviewPanel.Parent
        While container IsNot Nothing
            location += New Size(container.Location)
            If Object.ReferenceEquals(Me, container) Then
                location.X += SystemInformation.Border3DSize.Width
                location.Y += SystemInformation.CaptionHeight + SystemInformation.Border3DSize.Height
            Else
                Dim borderSize = container.Size - container.ClientSize
                location += New Size(CInt(borderSize.Width / 2), CInt(borderSize.Height / 2))
            End If
            container = container.Parent
        End While
        Return New Rectangle(location, PonyPreviewPanel.Size)
    End Function

    'This is used to keep track of the links in each behavior chain.
    Private Structure ChainLink
        Public Property Behavior As Behavior
        Public Property Order As Integer
        Public Property Series As Integer
        Public Sub New(_behavior As Behavior, _order As Integer, _series As Integer)
            Behavior = _behavior
            Order = _order
            Series = _series
        End Sub
    End Structure

    Private Sub LoadParameters(pony As Pony)
        Try
            If alreadyUpdating Then Exit Sub

            alreadyUpdating = True

            BehaviorsGrid.SuspendLayout()
            EffectsGrid.SuspendLayout()
            InteractionsGrid.SuspendLayout()
            SpeechesGrid.SuspendLayout()

            BehaviorsGrid.Rows.Clear()
            EffectsGrid.Rows.Clear()
            InteractionsGrid.Rows.Clear()
            SpeechesGrid.Rows.Clear()

            PonyName.Text = pony.Name

            CurrentBehaviorValueLabel.Text = "N/A"
            TimeLeftValueLabel.Text = "N/A"

            colEffectBehavior.Items.Clear()

            colBehaviorLinked.Items.Clear()
            colBehaviorLinked.Items.Add("None")

            colBehaviorStartSpeech.Items.Clear()
            colBehaviorStartSpeech.Items.Add("None")
            colBehaviorEndSpeech.Items.Clear()
            colBehaviorEndSpeech.Items.Add("None")

            Dim unnamedCounter = 1
            For Each speech As Behavior.SpeakingLine In pony.Base.SpeakingLines
                If speech.Name = "Unnamed" Then
                    speech.Name = "Unnamed #" & unnamedCounter
                    unnamedCounter += 1
                End If

                SpeechesGrid.Rows.Add(
                    speech.Name, speech.Name, speech.Group, pony.GetBehaviorGroupName(speech.Group), speech.Text,
                    GetFilename(speech.SoundFile), (Not speech.Skip).ToString())
                colBehaviorStartSpeech.Items.Add(speech.Name)
                colBehaviorEndSpeech.Items.Add(speech.Name)
            Next

            For Each behavior In pony.Behaviors
                colBehaviorLinked.Items.Add(behavior.Name)
                colEffectBehavior.Items.Add(behavior.Name)
            Next

            'Go through each behavior to see which ones are part of a chain, and if so, which order they go in.
            Dim allChains As New List(Of ChainLink)()

            Dim link_series = 0
            For Each behavior In pony.Behaviors
                If (behavior.LinkedBehaviorName) <> "" AndAlso behavior.LinkedBehaviorName <> "None" Then
                    'ignore behaviors that are not the first ones in a chain
                    '(chains that loop forever are ignored)

                    Dim startOfChain = Not pony.Behaviors.Any(
                        Function(b) String.Equals(b.LinkedBehaviorName, behavior.Name, StringComparison.OrdinalIgnoreCase))
                    If Not startOfChain Then Continue For

                    link_series += 1
                    Dim newChainLink As New ChainLink(behavior, 1, link_series)

                    Dim linkOrder As New List(Of ChainLink)()
                    linkOrder.Add(newChainLink)

                    Dim next_link = behavior.LinkedBehaviorName

                    Dim depth = 1

                    Dim no_more = False
                    Do Until no_more OrElse next_link = "None"
                        Append_Next_Link(next_link, depth, link_series, pony, linkOrder)
                        depth = depth + 1
                        If (linkOrder(linkOrder.Count - 1).Behavior.LinkedBehaviorName) = "" OrElse linkOrder.Count <> depth Then
                            no_more = True
                        Else
                            next_link = linkOrder(linkOrder.Count - 1).Behavior.LinkedBehaviorName
                        End If
                    Loop

                    allChains.AddRange(linkOrder)
                End If
            Next

            For Each behavior In pony.Behaviors
                Dim followName As String = ""
                If behavior.OriginalFollowObjectName <> "" Then
                    followName = behavior.OriginalFollowObjectName
                Else
                    If behavior.OriginalDestinationXCoord <> 0 AndAlso behavior.OriginalDestinationYCoord <> 0 Then
                        followName = behavior.OriginalDestinationXCoord & " , " & behavior.OriginalDestinationYCoord
                    Else
                        followName = "Select..."
                    End If
                End If

                Dim link_depth = ""
                For Each link In allChains
                    If String.Equals(link.Behavior.Name, behavior.Name, StringComparison.OrdinalIgnoreCase) Then
                        If link_depth <> "" Then
                            link_depth += ", " & link.Series & "-" & link.Order
                        Else
                            link_depth = link.Series & "-" & link.Order
                        End If
                    End If
                Next

                Dim chance = "N/A"
                If Not behavior.Skip Then
                    chance = behavior.ChanceOfOccurence.ToString("P", CultureInfo.CurrentCulture)
                End If

                BehaviorsGrid.Rows.Add(
                    "Run",
                    behavior.Name,
                    behavior.Name,
                    behavior.Group,
                    pony.GetBehaviorGroupName(behavior.Group),
                    chance,
                    behavior.MaxDuration,
                    behavior.MinDuration,
                    behavior.Speed,
                    GetFilename(behavior.RightImagePath),
                    GetFilename(behavior.LeftImagePath),
                    AllowedMovesToString(behavior.AllowedMovement),
                    behavior.StartLineName,
                    behavior.EndLineName,
                    followName,
                    behavior.LinkedBehaviorName,
                    link_depth,
                    behavior.Skip,
                    behavior.DoNotRepeatImageAnimations)
            Next

            For Each effect In PreviewPonyEffects()
                EffectsGrid.Rows.Add(
                    effect.Name,
                    effect.Name,
                    effect.BehaviorName,
                    GetFilename(effect.RightImagePath),
                    GetFilename(effect.LeftImagePath),
                    effect.Duration,
                    effect.RepeatDelay,
                    Location_ToString(effect.PlacementDirectionRight),
                    Location_ToString(effect.CenteringRight),
                    Location_ToString(effect.PlacementDirectionLeft),
                    Location_ToString(effect.CenteringLeft),
                    effect.Follow,
                    effect.DoNotRepeatImageAnimations)
            Next

            For Each interaction In PreviewPony.Interactions
                InteractionsGrid.Rows.Add(
                    interaction.Name,
                    interaction.Name,
                    interaction.Probability.ToString("P", CultureInfo.CurrentCulture),
                    interaction.Proximity_Activation_Distance,
                    "Select...",
                    interaction.Targets_Activated.ToString(),
                    "Select...",
                    interaction.ReactivationDelay)
            Next

            'to make sure that speech match behaviors
            PreviewPonyBase.LinkBehaviors()

            alreadyUpdating = False

            Dim conflicts As New HashSet(Of String)()

            For Each behavior In pony.Behaviors
                For Each otherbehavior In pony.Behaviors
                    If ReferenceEquals(behavior, otherbehavior) Then Continue For

                    For Each effect In behavior.Effects
                        For Each othereffect In otherbehavior.Effects
                            If ReferenceEquals(effect, othereffect) Then Continue For

                            If String.Equals(effect.Name, othereffect.Name, StringComparison.OrdinalIgnoreCase) Then
                                conflicts.Add("Effect: " & effect.Name)
                            End If
                        Next
                    Next

                    If String.Equals(behavior.Name, otherbehavior.Name, StringComparison.OrdinalIgnoreCase) Then
                        conflicts.Add("Behavior: " & behavior.Name)
                    End If
                Next
            Next

            For Each speech In pony.Base.SpeakingLines
                For Each otherspeech In pony.Base.SpeakingLines
                    If ReferenceEquals(speech, otherspeech) Then Continue For

                    If String.Equals(speech.Name, otherspeech.Name, StringComparison.OrdinalIgnoreCase) Then
                        conflicts.Add("Speech: " & speech.Name)
                    End If
                Next
            Next

            For Each interaction In pony.Interactions
                For Each otherinteraction In pony.Interactions
                    If ReferenceEquals(interaction, otherinteraction) Then Continue For

                    If String.Equals(interaction.Name, otherinteraction.Name, StringComparison.OrdinalIgnoreCase) Then
                        conflicts.Add("Interaction: " & interaction.Name)
                    End If
                Next

            Next

            If conflicts.Count > 0 Then
                MessageBox.Show(
                    Me, "Warning: Two or more behaviors, interactions, effects or speeches have duplicate names. " &
                    "Please give them unique names or the pony may act in undefined ways." & Environment.NewLine & Environment.NewLine &
                    String.Join(Environment.NewLine, conflicts),
                    "Duplicate Names", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            BehaviorsGrid.ResumeLayout()
            EffectsGrid.ResumeLayout()
            InteractionsGrid.ResumeLayout()
            SpeechesGrid.ResumeLayout()
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error loading pony parameters.")
            Me.Close()
        End Try
    End Sub

    Private Shared Sub Append_Next_Link(link_name As String, depth As Integer, series As Integer,
                                           pony As Pony, chain_list As List(Of ChainLink))

        For Each behavior In pony.Behaviors
            If behavior.Name = link_name Then

                depth += 1

                Dim new_link As New ChainLink()
                new_link.behavior = behavior
                new_link.order = depth
                new_link.series = series

                chain_list.Add(new_link)
                Exit For
            End If
        Next

    End Sub

    ''' <summary>
    ''' If we want to run a behavior that has a follow object, we can add it with this.
    ''' </summary>
    ''' <param name="ponyname"></param>
    ''' <returns></returns>
    Private Function AddPony(ponyname As String) As Pony

        Try
            For Each pony In pe_animator.Ponies()
                If String.Equals(Trim(pony.Directory), Trim(ponyname), StringComparison.OrdinalIgnoreCase) Then
                    Return pony
                End If
            Next

            For Each ponyBase In ponyBases
                If String.Equals(Trim(ponyBase.Directory), Trim(ponyname), StringComparison.OrdinalIgnoreCase) Then
                    Dim new_pony = New Pony(ponyBase)
                    new_pony.Teleport()
                    pe_animator.AddPony(new_pony)
                    Return new_pony
                End If
            Next

            Return Nothing

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error adding pony to the editor.")
            Me.Close()
            Return Nothing
        End Try

        Return Nothing
    End Function

    Private Function AddEffect(name As String) As Effect
        Try
            Dim effect As Effect = Nothing
            For Each listing In GetAllEffects()
                If String.Equals(Trim(listing.Name), Trim(name), StringComparison.OrdinalIgnoreCase) Then
                    effect = New Effect(listing, Not PreviewPony.facingRight)
                    effect.OwningPony = PreviewPony
                End If
            Next

            If IsNothing(effect) Then Return Nothing

            If effect.Base.Duration <> 0 Then
                effect.DesiredDuration = effect.Base.Duration
                effect.CloseOnNewBehavior = False
            End If

            Dim rect = GetPreviewWindowScreenRectangle()
            effect.Location = New Point(CInt(rect.X + rect.Width * Rng.NextDouble), CInt(rect.Y + rect.Height * Rng.NextDouble))
            pe_animator.AddEffect(effect)
            PreviewPony.ActiveEffects.Add(effect)

            Return effect

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error adding effect to the editor.")
            Me.Close()
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' Usually thrown when a value entered isn't in the list of allowed values of a dropdown box in a grid.
    ''' </summary>
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The event data.</param>
    Private Sub GridError(sender As Object, e As DataGridViewDataErrorEventArgs)
        Try

            Dim grid As DataGridView = CType(sender, DataGridView)

            Dim replacement As String = ""

            Select Case grid.Name
                Case "Pony_Effects_Grid"
                    grid = EffectsGrid
                    replacement = ""
                Case "Pony_Behaviors_Grid"
                    grid = BehaviorsGrid
                    replacement = "None"
                Case Else
                    Throw New Exception("Unhandled error for grid: " & grid.Name)
            End Select

            Dim invalid_value As String = CStr(grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)
            grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = replacement

            MsgBox(PonyBase.ConfigFilename & " file appears to have an invalid line: '" & invalid_value & "' is not valid for column '" & grid.Columns(e.ColumnIndex).HeaderText _
                   & "'" & ControlChars.NewLine & "Details: Column: " & e.ColumnIndex & " Row: " & e.RowIndex & " - " & e.Exception.Message & ControlChars.NewLine & _
                   ControlChars.NewLine & "Value will be reset.")

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error trying to handle a data error! The editor will now close.")
            Me.Close()
        End Try

    End Sub

    Private Sub PonyBehaviorsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles BehaviorsGrid.CellClick

        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim changed_behavior_name As String = CStr(BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorOriginalName.Index).Value)
            Dim changed_behavior As Behavior = Nothing

            For Each behavior In PreviewPony.Behaviors
                If behavior.Name = changed_behavior_name Then
                    changed_behavior = behavior
                    Exit For
                End If
            Next

            If IsNothing(changed_behavior) Then
                LoadParameters(PreviewPony)
                RestoreSortOrder()
                Exit Sub
            End If

            Select Case e.ColumnIndex

                Case colBehaviorActivate.Index

                    Dim ponies_to_remove As New List(Of Pony)

                    For Each pony In pe_animator.Ponies()
                        If pony.Directory <> PreviewPony.Directory Then
                            ponies_to_remove.Add(pony)
                        End If
                    Next

                    For Each Pony In ponies_to_remove
                        pe_animator.RemovePony(Pony)
                    Next

                    PreviewPony.ActiveEffects.Clear()

                    Dim follow_sprite As ISprite = Nothing
                    If changed_behavior.OriginalFollowObjectName <> "" Then
                        follow_sprite = AddPony(changed_behavior.OriginalFollowObjectName)

                        If IsNothing(follow_sprite) Then
                            follow_sprite = AddEffect(changed_behavior.OriginalFollowObjectName)
                        End If

                        If IsNothing(follow_sprite) Then
                            MessageBox.Show("The specified pony or effect to follow (" & changed_behavior.OriginalFollowObjectName &
                                            ") for this behavior (" & changed_behavior_name &
                                            ") does not exist. Please review this setting.",
                                            "Cannot Run Behavior", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Return
                        End If
                    End If

                    PreviewPony.SelectBehavior(changed_behavior)
                    PreviewPony.followObject = follow_sprite

                Case colBehaviorRightImage.Index
                    HidePony()
                    Dim new_image_path = Add_Picture(PreviewPony.Directory & " Right Image...")
                    If Not IsNothing(new_image_path) Then
                        changed_behavior.SetRightImagePath(new_image_path)
                        BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorRightImage.Index).Value = GetFilename(new_image_path)
                        ImageSizeCheck(changed_behavior.RightImageSize)
                    End If
                    ShowPony()
                Case colBehaviorLeftImage.Index
                    HidePony()
                    Dim new_image_path = Add_Picture(PreviewPony.Directory & " Left Image...")
                    If Not IsNothing(new_image_path) Then
                        changed_behavior.SetLeftImagePath(new_image_path)
                        BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorLeftImage.Index).Value = GetFilename(new_image_path)
                        ImageSizeCheck(changed_behavior.LeftImageSize)
                    End If
                    ShowPony()
                Case colBehaviorFollow.Index
                    HidePony()
                    Set_Behavior_Follow_Parameters(changed_behavior)
                    ShowPony()

                Case Else
                    Exit Sub

            End Select

            'If changes_made_now Then
            '    Load_Parameters(Preview_Pony)
            '    RestoreSortOrder()
            'End If

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Private Sub PonySpeechesGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles SpeechesGrid.CellClick
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim changed_speech_name As String = CStr(SpeechesGrid.Rows(e.RowIndex).Cells(colSpeechOriginalName.Index).Value)
            Dim changed_speech As Behavior.SpeakingLine = Nothing

            For Each speech As Behavior.SpeakingLine In PreviewPonyBase.SpeakingLines
                If speech.Name = changed_speech_name Then
                    changed_speech = speech
                    Exit For
                End If
            Next

            If IsNothing(changed_speech) Then
                LoadParameters(PreviewPony)
                RestoreSortOrder()
                Exit Sub
            End If

            Dim changes_made_now = False

            Select Case e.ColumnIndex

                Case colSpeechSoundFile.Index
                    HidePony()
                    changed_speech.SoundFile = SetSound()
                    SpeechesGrid.Rows(e.RowIndex).Cells(colSpeechSoundFile.Index).Value = changed_speech.SoundFile
                    changes_made_now = True
                    ShowPony()
                Case Else
                    Exit Sub

            End Select

            If changes_made_now Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                hasSaved = False
            End If

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try
    End Sub

    Private Sub PonyEffectsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles EffectsGrid.CellClick
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim changed_effect_name As String = CStr(EffectsGrid.Rows(e.RowIndex).Cells(colEffectOriginalName.Index).Value)
            Dim changed_effect As EffectBase = Nothing

            For Each effect In PreviewPonyEffects()
                If effect.Name = changed_effect_name Then
                    changed_effect = effect
                    Exit For
                End If
            Next

            If IsNothing(changed_effect) Then
                LoadParameters(PreviewPony)
                RestoreSortOrder()
                Exit Sub
            End If

            Dim changes_made_now = False

            Select Case e.ColumnIndex

                Case colEffectRightImage.Index
                    HidePony()
                    Dim new_image_path As String = Add_Picture(changed_effect_name & " Right Image...")
                    If Not IsNothing(new_image_path) Then
                        changed_effect.SetRightImagePath(new_image_path)
                        EffectsGrid.Rows(e.RowIndex).Cells(colEffectRightImage.Index).Value = GetFilename(new_image_path)
                        changes_made_now = True
                    End If
                    ShowPony()
                Case colEffectLeftImage.Index
                    HidePony()
                    Dim new_image_path = Add_Picture(changed_effect_name & " Left Image...")
                    If Not IsNothing(new_image_path) Then
                        changed_effect.SetLeftImagePath(new_image_path)
                        EffectsGrid.Rows(e.RowIndex).Cells(colEffectLeftImage.Index).Value = GetFilename(new_image_path)
                        changes_made_now = True
                    End If
                    ShowPony()
                Case Else
                    Exit Sub

            End Select

            If changes_made_now Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                hasSaved = False
            End If


        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try
    End Sub

    Private Sub PonyInteractionsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles InteractionsGrid.CellClick
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim changed_interaction_name As String = CStr(InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionOriginalName.Index).Value)
            Dim changed_interaction As Interaction = Nothing

            For Each interaction As Interaction In PreviewPony.Interactions
                If interaction.Name = changed_interaction_name Then
                    changed_interaction = interaction
                    Exit For
                End If
            Next

            If IsNothing(changed_interaction) Then
                LoadParameters(PreviewPony)
                RestoreSortOrder()
                Exit Sub
            End If

            Dim changes_made_now = False


            Select Case e.ColumnIndex
                Case colInteractionTargets.Index, colInteractionBehaviors.Index
                    HidePony()
                    Using form = New NewInteractionDialog(Me)
                        form.ChangeInteraction(changed_interaction)
                        form.ShowDialog(Me)
                    End Using
                    changes_made_now = True
                    ShowPony()
                Case Else
                    Exit Sub
            End Select

            If changes_made_now Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                hasSaved = False
            End If


        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try
    End Sub

    Private Sub PonyBehaviorsGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles BehaviorsGrid.CellValueChanged
        If alreadyUpdating Then Return
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim new_value As String = CStr(BehaviorsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Dim changed_behavior_name As String = CStr(BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorOriginalName.Index).Value)
            Dim changed_behavior As Behavior = Nothing

            For Each behavior In PreviewPony.Behaviors
                If behavior.Name = changed_behavior_name Then
                    changed_behavior = behavior
                    Exit For
                End If
            Next

            If IsNothing(changed_behavior) Then
                LoadParameters(PreviewPony)
                Exit Sub
            End If

            Try

                Select Case e.ColumnIndex

                    Case colBehaviorName.Index
                        If new_value = "" Then
                            MsgBox("You must give a behavior a name.  It can't be blank.")
                            BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorName.Index).Value = changed_behavior_name
                            Exit Sub
                        End If

                        If new_value = changed_behavior_name Then
                            Exit Sub
                        End If

                        For Each behavior In PreviewPony.Behaviors
                            If String.Equals(behavior.Name, new_value, StringComparison.OrdinalIgnoreCase) Then
                                MsgBox("Behavior names must be unique.  Behavior '" & new_value & "' already exists.")
                                BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorName.Index).Value = changed_behavior_name
                                Exit Sub
                            End If
                        Next
                        changed_behavior.Name = new_value
                        BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorOriginalName.Index).Value = new_value
                    Case colBehaviorChance.Index
                        changed_behavior.ChanceOfOccurence = Double.Parse(Trim(Replace(new_value, "%", "")), CultureInfo.CurrentCulture) / 100
                    Case colBehaviorMaxDuration.Index
                        Dim maxDuration = Double.Parse(new_value, CultureInfo.CurrentCulture)
                        If maxDuration > 0 Then
                            changed_behavior.MaxDuration = maxDuration
                        Else
                            Throw New InvalidDataException("Maximum Duration must be greater than 0")
                        End If
                    Case colBehaviorMinDuration.Index
                        Dim minDuration = Double.Parse(new_value, CultureInfo.CurrentCulture)
                        If minDuration >= 0 Then
                            changed_behavior.MinDuration = minDuration
                        Else
                            Throw New InvalidDataException("Minimum Duration must be greater than or equal to 0")
                        End If
                    Case colBehaviorSpeed.Index
                        changed_behavior.SetSpeed(Double.Parse(new_value, CultureInfo.CurrentCulture))
                    Case colBehaviorMovement.Index
                        changed_behavior.AllowedMovement = AllowedMovesFromString(new_value)
                    Case colBehaviorStartSpeech.Index
                        If new_value = "None" Then
                            changed_behavior.StartLineName = ""
                        Else
                            changed_behavior.StartLineName = new_value
                        End If
                    Case colBehaviorEndSpeech.Index
                        If new_value = "None" Then
                            changed_behavior.EndLineName = ""
                        Else
                            changed_behavior.EndLineName = new_value
                        End If
                        PreviewPonyBase.LinkBehaviors()
                    Case colBehaviorLinked.Index
                        changed_behavior.LinkedBehaviorName = new_value
                        PreviewPonyBase.LinkBehaviors()
                    Case colBehaviorDoNotRunRandomly.Index
                        changed_behavior.Skip = Boolean.Parse(new_value)
                    Case colBehaviorDoNotRepeatAnimations.Index
                        changed_behavior.DoNotRepeatImageAnimations = Boolean.Parse(new_value)
                    Case colBehaviorGroup.Index
                        Dim new_group_value = Integer.Parse(new_value, CultureInfo.CurrentCulture)
                        If new_group_value < 0 Then
                            MsgBox("You can't have a group ID less than 0.")
                            Exit Sub
                        End If
                        changed_behavior.Group = new_group_value
                    Case colBehaviorGroupName.Index
                        If changed_behavior.Group = Behavior.AnyGroup Then
                            MsgBox("You can't change the name of the 'Any' group. This is reserved. " &
                                   "It means the behavior can run at any time, regardless of the current group that is running.")
                            Exit Sub
                        End If

                        If PreviewPony.GetBehaviorGroupName(changed_behavior.Group) = "Unnamed" Then
                            PreviewPony.BehaviorGroups.Add(New BehaviorGroup(new_value, changed_behavior.Group))
                        Else
                            For Each behaviorgroup In PreviewPony.BehaviorGroups

                                If behaviorgroup.Name = new_value Then
                                    MsgBox("Error:  That group name already exists under a different ID.")
                                    Exit Sub
                                End If
                            Next

                            For Each behaviorgroup In PreviewPony.BehaviorGroups
                                If behaviorgroup.Number = changed_behavior.Group Then
                                    behaviorgroup.Name = new_value
                                End If
                            Next

                        End If

                End Select

            Catch ex As Exception
                My.Application.NotifyUserOfNonFatalException(ex, "You entered an invalid value for column '" &
                                                             BehaviorsGrid.Columns(e.ColumnIndex).HeaderText & "': " &
                                                             CStr(BehaviorsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value))
            End Try

            PreviewPonyBase.LinkBehaviors()

            If alreadyUpdating = False Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                hasSaved = False
            End If

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Private Sub PonyEffectsGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles EffectsGrid.CellValueChanged
        If alreadyUpdating Then Return
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim new_value As String = CStr(EffectsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Dim changed_effect_name As String = CStr(EffectsGrid.Rows(e.RowIndex).Cells(colEffectOriginalName.Index).Value)
            Dim changed_effect As EffectBase = Nothing

            For Each effect In PreviewPonyEffects()
                If effect.Name = changed_effect_name Then
                    changed_effect = effect
                    Exit For
                End If
            Next

            If IsNothing(changed_effect) Then
                LoadParameters(PreviewPony)
                Exit Sub
            End If

            Try

                Select Case e.ColumnIndex
                    Case colEffectName.Index
                        If new_value = "" Then
                            MsgBox("You must give an effect a name.  It can't be blank.")
                            EffectsGrid.Rows(e.RowIndex).Cells(colEffectName.Index).Value = changed_effect_name
                            Exit Sub
                        End If

                        If new_value = changed_effect_name Then
                            Exit Sub
                        End If

                        For Each effect In GetAllEffects()
                            If String.Equals(effect.Name, new_value, StringComparison.OrdinalIgnoreCase) Then
                                MsgBox("Effect names must be unique.  Effect '" & new_value & "' already exists.")
                                EffectsGrid.Rows(e.RowIndex).Cells(colEffectName.Index).Value = changed_effect_name
                                Exit Sub
                            End If
                        Next

                        changed_effect.Name = new_value
                        EffectsGrid.Rows(e.RowIndex).Cells(colEffectOriginalName.Index).Value = new_value
                    Case colEffectBehavior.Index
                        For Each behavior In PreviewPony.Behaviors
                            If behavior.Name = changed_effect.BehaviorName Then
                                behavior.RemoveEffect(changed_effect)
                                Exit For
                            End If
                        Next
                        changed_effect.BehaviorName = new_value
                        For Each behavior In PreviewPony.Behaviors
                            If behavior.Name = changed_effect.BehaviorName Then
                                behavior.AddEffect(changed_effect, PreviewPonyBase)
                                Exit For
                            End If
                        Next
                    Case colEffectDuration.Index
                        changed_effect.Duration = Double.Parse(new_value, CultureInfo.InvariantCulture)
                    Case colEffectRepeatDelay.Index
                        changed_effect.RepeatDelay = Double.Parse(new_value, CultureInfo.InvariantCulture)
                    Case colEffectLocationRight.Index
                        changed_effect.PlacementDirectionRight = DirectionFromString(new_value)
                    Case colEffectLocationLeft.Index
                        changed_effect.PlacementDirectionLeft = DirectionFromString(new_value)
                    Case colEffectCenteringRight.Index
                        changed_effect.CenteringRight = DirectionFromString(new_value)
                    Case colEffectCenteringLeft.Index
                        changed_effect.CenteringLeft = DirectionFromString(new_value)
                    Case colEffectFollowPony.Index
                        changed_effect.follow = Boolean.Parse(new_value)
                    Case colEffectDoNotRepeatAnimations.Index
                        changed_effect.DoNotRepeatImageAnimations = Boolean.Parse(new_value)
                End Select

            Catch ex As Exception
                My.Application.NotifyUserOfNonFatalException(ex, "You entered an invalid value for column '" &
                                                             EffectsGrid.Columns(e.ColumnIndex).HeaderText & "': " &
                                                             CStr(EffectsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value))
            End Try

            PreviewPonyBase.LinkBehaviors()

            If alreadyUpdating = False Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                hasSaved = False
            End If


        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Private Sub PonySpeechesGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles SpeechesGrid.CellValueChanged
        If alreadyUpdating Then Return
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim new_value As String = CStr(SpeechesGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Dim changed_speech_name As String = CStr(SpeechesGrid.Rows(e.RowIndex).Cells(colSpeechOriginalName.Index).Value)
            Dim changed_speech As Behavior.SpeakingLine = Nothing

            For Each speech In PreviewPonyBase.SpeakingLines
                If speech.Name = changed_speech_name Then
                    changed_speech = speech
                    Exit For
                End If
            Next

            If IsNothing(changed_speech) Then
                LoadParameters(PreviewPony)
                Exit Sub
            End If

            Try

                Select Case e.ColumnIndex
                    Case colSpeechName.Index
                        If new_value = "" Then
                            MsgBox("You must give a speech a name, it can't be blank")
                            SpeechesGrid.Rows(e.RowIndex).Cells(colSpeechName.Index).Value = changed_speech_name
                            Exit Sub
                        End If

                        If new_value = changed_speech_name Then
                            Exit Sub
                        End If

                        For Each speechname In PreviewPonyBase.SpeakingLines
                            If String.Equals(speechname.Name, new_value, StringComparison.OrdinalIgnoreCase) Then
                                MsgBox("Speech names must be unique.  Speech '" & new_value & "' already exists.")
                                SpeechesGrid.Rows(e.RowIndex).Cells(colSpeechName.Index).Value = changed_speech_name
                                Exit Sub
                            End If
                        Next
                        changed_speech.Name = new_value
                        SpeechesGrid.Rows(e.RowIndex).Cells(colSpeechOriginalName.Index).Value = new_value
                    Case colSpeechText.Index
                        changed_speech.Text = new_value
                    Case colSpeechUseRandomly.Index
                        changed_speech.Skip = Not (Boolean.Parse(new_value))
                    Case colSpeechGroup.Index
                        changed_speech.Group = Integer.Parse(new_value, CultureInfo.InvariantCulture)
                End Select

            Catch ex As Exception
                My.Application.NotifyUserOfNonFatalException(ex, "You entered an invalid value for column '" &
                                                             SpeechesGrid.Columns(e.ColumnIndex).HeaderText & "': " &
                                                             CStr(SpeechesGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value))
                Exit Sub
            End Try

            PreviewPonyBase.LinkBehaviors()

            If alreadyUpdating = False Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                hasSaved = False
            End If


        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Private Sub PonyInteractionsGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles InteractionsGrid.CellValueChanged
        If alreadyUpdating Then Return
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim new_value As String = CStr(InteractionsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Dim changed_interaction_name As String = CStr(InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionOriginalName.Index).Value)
            Dim changed_interaction As Interaction = Nothing

            For Each interaction In PreviewPony.Interactions
                If interaction.Name = changed_interaction_name Then
                    changed_interaction = interaction
                    Exit For
                End If
            Next

            If IsNothing(changed_interaction) Then
                LoadParameters(PreviewPony)
                Exit Sub
            End If

            Try

                Select Case e.ColumnIndex
                    Case colInteractionName.Index
                        If new_value = "" Then
                            MsgBox("You must give an interaction a name.  It can't be blank.")
                            InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionName.Index).Value = changed_interaction_name
                            Exit Sub
                        End If

                        If new_value = changed_interaction_name Then
                            Exit Sub
                        End If

                        For Each Interaction In PreviewPony.Interactions
                            If String.Equals(Interaction.Name, new_value, StringComparison.OrdinalIgnoreCase) Then
                                MsgBox("Interaction with name '" & Interaction.Name & "' already exists for this pony.  Please select another name.")
                                InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionName.Index).Value = changed_interaction_name
                                Exit Sub
                            End If
                        Next

                        changed_interaction.Name = new_value
                        InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionOriginalName.Index).Value = new_value
                    Case colInteractionChance.Index
                        changed_interaction.Probability = Double.Parse(Trim(Replace(new_value, "%", "")), CultureInfo.InvariantCulture) / 100
                    Case colInteractionProximity.Index
                        changed_interaction.Proximity_Activation_Distance = Double.Parse(new_value, CultureInfo.InvariantCulture)
                    Case colInteractionInteractWith.Index
                        changed_interaction.Targets_Activated =
                            CType([Enum].Parse(GetType(Interaction.TargetActivation), new_value), Interaction.TargetActivation)
                    Case colInteractionReactivationDelay.Index
                        changed_interaction.ReactivationDelay = Integer.Parse(new_value, CultureInfo.InvariantCulture)
                End Select

            Catch ex As Exception
                My.Application.NotifyUserOfNonFatalException(ex, "You entered an invalid value for column '" &
                                                             InteractionsGrid.Columns(e.ColumnIndex).HeaderText & "': " &
                                                             CStr(InteractionsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value))
            End Try

            If alreadyUpdating = False Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                hasSaved = False
            End If

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Friend Function GetAllEffects() As EffectBase()
        Return ponyBases.SelectMany(Function(pb) pb.Behaviors).SelectMany(Function(b) b.Effects).ToArray()
    End Function

    Private Function PreviewPonyEffects() As IEnumerable(Of EffectBase)
        Return PreviewPony.Behaviors.SelectMany(Function(behavior) (behavior.Effects))
    End Function

    Private Sub SaveSortOrder()
        For Each infoGrid In infoGrids
            infoGrid.SortColumn = infoGrid.Grid.SortedColumn
            infoGrid.SortOrder = infoGrid.Grid.SortOrder
        Next
    End Sub

    Private Sub RestoreSortOrder()
        Try
            For Each infoGrid In infoGrids
                If infoGrid.SortColumn Is Nothing Then Continue For
                infoGrid.Grid.Sort(infoGrid.SortColumn, ConvertSortOrder(infoGrid.SortOrder))
            Next
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error restoring sort order for grid.")
        End Try
    End Sub

    ''' <summary>
    ''' The DataGridView returns a SortOrder object when you ask it how it is sorted.
    ''' But, when telling it to sort, you need to specify a ListSortDirection instead, which is different...
    ''' </summary>
    ''' <param name="sort"></param>
    ''' <returns></returns>
    Private Shared Function ConvertSortOrder(sort As SortOrder) As System.ComponentModel.ListSortDirection
        Select Case sort
            Case SortOrder.Ascending
                Return System.ComponentModel.ListSortDirection.Ascending
            Case SortOrder.Descending
                Return System.ComponentModel.ListSortDirection.Descending
            Case Else
                Return System.ComponentModel.ListSortDirection.Ascending
        End Select
    End Function

    Private Sub PausePonyButton_Click(sender As Object, e As EventArgs) Handles PausePonyButton.Click

        Try
            If Not pe_animator.Paused Then
                pe_animator.Pause(False)
                PausePonyButton.Text = "Resume Pony"
            Else
                pe_animator.Resume()
                PausePonyButton.Text = "Pause Pony"
            End If
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error attempting to pause or resume animation.")
        End Try

    End Sub

    Private Sub HidePony()
        PausePonyButton.Text = "Resume Pony"
        If pe_animator.Started Then pe_animator.Pause(False)
        If PreviewPony IsNot Nothing Then pe_interface.Hide()
    End Sub

    Private Sub ShowPony()
        PausePonyButton.Text = "Pause Pony"
        If pe_animator.Started Then pe_animator.Resume()
        If PreviewPony IsNot Nothing Then pe_interface.Show()
    End Sub

    Private Sub NewBehaviorButton_Click(sender As Object, e As EventArgs) Handles NewBehaviorButton.Click
        Try

            If IsNothing(PreviewPony) Then
                MsgBox("Select a pony or create a new one first,")
                Exit Sub
            End If

            HidePony()
            Using form = New NewBehaviorDialog(Me)
                form.ShowDialog()
            End Using

            ShowPony()
            PreviewPony.SelectBehavior(PreviewPony.Behaviors(0))

            LoadParameters(PreviewPony)
            hasSaved = False

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error creating new behavior. The editor will now close.")
            Me.Close()
        End Try

    End Sub

    Private Sub NewSpeechButton_Click(sender As Object, e As EventArgs) Handles NewSpeechButton.Click
        Try
            If IsNothing(PreviewPony) Then
                MsgBox("Select a pony or create a new one first,")
                Exit Sub
            End If

            HidePony()

            Using form = New NewSpeechDialog(Me)
                form.ShowDialog()
            End Using

            ShowPony()

            LoadParameters(PreviewPony)
            hasSaved = False
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error creating new speech. The editor will now close.")
            Me.Close()
        End Try
    End Sub

    Private Sub NewEffectButton_Click(sender As Object, e As EventArgs) Handles NewEffectButton.Click
        Try

            If IsNothing(PreviewPony) Then
                MsgBox("Select a pony or create a new one first,")
                Exit Sub
            End If

            HidePony()

            Using form = New NewEffectDialog(Me)
                form.ShowDialog()
            End Using

            ShowPony()

            LoadParameters(PreviewPony)
            hasSaved = False
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error creating new effect, the editor will now close.")
            Me.Close()
        End Try
    End Sub

    Private Sub NewInteractionButton_Click(sender As Object, e As EventArgs) Handles NewInteractionButton.Click
        Try

            If IsNothing(PreviewPony) Then
                MsgBox("Select a pony or create a new one first,")
                Exit Sub
            End If

            HidePony()
            Using form = New NewInteractionDialog(Me)
                form.ChangeInteraction(Nothing)
                form.ShowDialog()
            End Using
            ShowPony()

            LoadParameters(PreviewPony)
            hasSaved = False

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error creating new interaction. The editor will now close.")
            Me.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Note that we try to rename the file and save the path as all lowercase.
    ''' This is for compatibility with other ports that run on case-sensitive operating systems.
    ''' (Otherwise they go crazy renaming everything with each update.)
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns></returns>
    Friend Function Add_Picture(Optional text As String = "") As String

        Try

            OpenPictureDialog.Filter = "GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png|JPG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg|All Files (*.*)|*.*"
            OpenPictureDialog.FilterIndex = 4
            OpenPictureDialog.InitialDirectory = Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, PreviewPony.Directory)

            If text = "" Then
                OpenPictureDialog.Title = "Select picture..."
            Else
                OpenPictureDialog.Title = "Select picture for: " & text
            End If

            Dim picture_path As String = Nothing

            If OpenPictureDialog.ShowDialog() = DialogResult.OK Then
                picture_path = OpenPictureDialog.FileName
            Else
                Return Nothing
            End If

            ' Try to load this image.
            Image.FromFile(picture_path)

            Dim new_path = IO.Path.Combine(Options.InstallLocation, PonyBase.RootDirectory,
                                           PreviewPony.Directory, GetFilename(picture_path))

            If new_path <> picture_path Then
                Try
                    If Not File.Exists(new_path) Then
                        File.Create(new_path).Close()
                    End If
                    My.Computer.FileSystem.CopyFile(picture_path, new_path, True)
                Catch ex As Exception
                    My.Application.NotifyUserOfNonFatalException(
                        ex, "Couldn't copy the image file to the pony directory." &
                        " If you were trying to use the same image for left and right, you can safely ignore this message.")
                End Try
            End If

            Return new_path

            hasSaved = False

        Catch ex As Exception

            My.Application.NotifyUserOfNonFatalException(ex, "Error loading image.")
            Return Nothing

        End Try

    End Function

    Private Function SetSound() As String
        Dim sound_path As String = Nothing

        Using dialog = New NewSpeechDialog(Me)
            dialog.OpenSoundDialog.Filter = "MP3 Files (*.mp3)|*.mp3"
            dialog.OpenSoundDialog.InitialDirectory = Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, PreviewPony.Directory)
            If dialog.OpenSoundDialog.ShowDialog() = DialogResult.OK Then
                sound_path = dialog.OpenSoundDialog.FileName
            End If
        End Using

        If IsNothing(sound_path) Then
            Return ""
        End If

        Dim new_path = IO.Path.Combine(Options.InstallLocation, PonyBase.RootDirectory,
                                       PreviewPony.Directory, GetFilename(sound_path))

        If new_path <> sound_path Then
            If Not File.Exists(new_path) Then
                File.Create(new_path).Close()
            End If
            My.Computer.FileSystem.CopyFile(sound_path, new_path, True)
        End If

        Return GetFilename(sound_path)

    End Function

    Private Sub Grid_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles BehaviorsGrid.UserDeletingRow, EffectsGrid.UserDeletingRow,
                                                                                                            InteractionsGrid.UserDeletingRow, SpeechesGrid.UserDeletingRow
        Try
            SaveSortOrder()
            Dim grid As DataGridView = DirectCast(sender, DataGridView)

            If Object.ReferenceEquals(grid, EffectsGrid) Then
                Dim todelete As EffectBase = Nothing
                For Each behavior In PreviewPony.Behaviors
                    For Each effect In behavior.Effects
                        If effect.Name = CStr(e.Row.Cells(colEffectName.Index).Value) Then
                            todelete = effect
                            Exit For
                        End If
                    Next
                    If Not IsNothing(todelete) Then
                        behavior.RemoveEffect(todelete)
                    End If
                Next
            ElseIf Object.ReferenceEquals(grid, BehaviorsGrid) Then
                If grid.RowCount = 1 Then
                    e.Cancel = True
                    MsgBox("A pony must have at least 1 behavior.  You can't delete the last one.")
                End If
                Dim todelete As Behavior = Nothing
                For Each behavior In PreviewPony.Behaviors
                    If CStr(e.Row.Cells(colBehaviorName.Index).Value) = behavior.Name Then
                        todelete = behavior
                        Exit For
                    End If
                Next
                If Not IsNothing(todelete) Then
                    PreviewPony.Behaviors.Remove(todelete)
                End If
            ElseIf Object.ReferenceEquals(grid, InteractionsGrid) Then
                Dim todelete As Interaction = Nothing
                For Each interaction In PreviewPony.Interactions
                    If CStr(e.Row.Cells(colInteractionName.Index).Value) = interaction.Name Then
                        todelete = interaction
                        Exit For
                    End If
                Next
                If Not IsNothing(todelete) Then
                    PreviewPony.Interactions.Remove(todelete)
                End If
            ElseIf Object.ReferenceEquals(grid, SpeechesGrid) Then
                Dim todelete As Behavior.SpeakingLine = Nothing
                For Each speech In PreviewPonyBase.SpeakingLines
                    If CStr(e.Row.Cells(colSpeechName.Index).Value) = speech.Name Then
                        todelete = speech
                        Exit For
                    End If
                Next
                If Not IsNothing(todelete) Then
                    PreviewPonyBase.SpeakingLines.Remove(todelete)
                    PreviewPonyBase.SetLines(PreviewPonyBase.SpeakingLines)
                End If
            Else
                Throw New Exception("Unknown grid when deleting row: " & grid.Name)
            End If

            hasSaved = False

            PreviewPonyBase.LinkBehaviors()

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error handling row deletion.")
        End Try

    End Sub

    Private Sub Grid_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles BehaviorsGrid.UserDeletedRow, EffectsGrid.UserDeletedRow,
                                                                                                        InteractionsGrid.UserDeletedRow, SpeechesGrid.UserDeletedRow
        'Load_Parameters(Preview_Pony)
        'RestoreSortOrder()
    End Sub

    Private Sub Grid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles SpeechesGrid.DataError, InteractionsGrid.DataError, EffectsGrid.DataError, BehaviorsGrid.DataError
        My.Application.NotifyUserOfNonFatalException(e.Exception, "Error interpreting the data entered into the editor.")
    End Sub

    Private Sub Set_Behavior_Follow_Parameters(behavior As Behavior)
        Try

            HidePony()

            Using form = New FollowTargetDialog(Me)
                form.Change_Behavior(behavior)
                form.ShowDialog()
            End Using
            LoadParameters(PreviewPony)

            ShowPony()

            hasSaved = False
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error setting follow parameters. The editor will now close.")
            Me.Close()
        End Try
    End Sub

    Friend Function GetFilename(path As String) As String
        Return IO.Path.GetFileName(path)
    End Function

    Private Sub PonyName_TextChanged(sender As Object, e As EventArgs) Handles PonyName.TextChanged
        If Not alreadyUpdating Then
            PreviewPonyBase.Name = PonyName.Text
            hasSaved = False
        End If
    End Sub

    Private Sub NewPonyButton_Click(sender As Object, e As EventArgs) Handles NewPonyButton.Click

        Try
            If PreventStateChange("Save changes before creating a new pony?") Then Return

            HidePony()

            Dim previousPony As Pony = Nothing
            If Not IsNothing(PreviewPony) Then
                previousPony = PreviewPony
            End If

            Dim ponyBase = New MutablePonyBase()
            ponyBase.Name = "New Pony"
            _previewPony = New Pony(ponyBase)

            Using form = New NewPonyDialog(Me)
                If form.ShowDialog() = DialogResult.Cancel Then
                    If Not IsNothing(previousPony) Then
                        _previewPony = previousPony
                        ShowPony()
                    End If
                    Exit Sub
                End If
            End Using

            MsgBox("All ponies must now be reloaded. Once this operation is complete, you can reopen the editor and select your pony for editing.")
            _changesMade = True
            Me.Close()

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error creating new pony. The editor will now close.")
            Me.Close()
        End Try

    End Sub

    Friend Sub SavePony(path As String)
        Try
            PreviewPonyBase.Save()
        Catch ex As ArgumentException When ex.ParamName = "text"
            MessageBox.Show(Me, "Some invalid characters were detected. Please remove them." & Environment.NewLine &
                            ex.Message.Remove(ex.Message.LastIndexOf(Environment.NewLine)),
                            "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "There was an unexpected error trying to save the pony.")
            Return
        End Try
        hasSaved = True
        MessageBox.Show(Me, "Save completed!", "Save Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ''' <summary>
    ''' Prompts the user about saving outstanding changes, if required.
    ''' </summary>
    ''' <returns>A value indicating whether the caller should abort any destructive state changes, as the user does not wish them this to
    ''' happen at this time.</returns>
    Private Function PreventStateChange(message As String) As Boolean
        If Not hasSaved Then
            Dim result = MessageBox.Show(Me, message, "Unsaved Changes",
                                         MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3)
            If result = DialogResult.Yes Then
                Try
                    SaveButton_Click(SaveButton, EventArgs.Empty)
                Catch ex As Exception
                    Return True
                End Try
            ElseIf result = DialogResult.Cancel Then
                Return True
            End If
        End If
        Return False
    End Function

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        PausePonyButton.Enabled = False
        pe_animator.Pause(False)

        _changesMade = True
        SavePony(PreviewPony.Directory)

        RefreshButton_Click(Nothing, Nothing)

        pe_animator.Resume()
        PausePonyButton.Enabled = True

    End Sub

    Friend Sub ImageSizeCheck(imageSize As Size)
        If imageSize.Height > PonyPreviewPanel.Size.Height OrElse
            imageSize.Width > PonyPreviewPanel.Size.Width Then
            MessageBox.Show(Me, "The selected image is too large for the preview area. " &
                            "The preview may be inaccurate as a result, but the pony will still function correctly when used normally.",
                            "Large Preview", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub EditTagsButton_Click(sender As Object, e As EventArgs) Handles EditTagsButton.Click

        If IsNothing(PreviewPony) Then
            MsgBox("Select a pony first!")
            Exit Sub
        End If

        HidePony()
        Using form = New TagsForm(Me)
            form.ShowDialog()
        End Using
        hasSaved = False
        ShowPony()

    End Sub

    Private Sub SetImageCentersButton_Click(sender As Object, e As EventArgs) Handles SetImageCentersButton.Click

        If IsNothing(PreviewPony) Then
            MsgBox("Select a pony first.")
            Exit Sub
        End If
        HidePony()
        Using form = New ImageCentersForm(Me)
            form.ShowDialog()
        End Using
        hasSaved = False
        ShowPony()

    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        LoadParameters(PreviewPony)
        RestoreSortOrder()
    End Sub

    Private Sub PonyEditorAnimator_AnimationFinished(sender As Object, e As EventArgs)
        SmartInvoke(AddressOf Close)
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing Then
                If components IsNot Nothing Then components.Dispose()
                If pe_animator IsNot Nothing AndAlso Not pe_animator.Disposed Then
                    pe_animator.Finish()
                    pe_animator.Dispose()
                    If Object.ReferenceEquals(pe_animator, Pony.CurrentAnimator) Then
                        Pony.CurrentAnimator = Nothing
                    End If
                End If
                If ponyImageList IsNot Nothing Then ponyImageList.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
        General.FullCollect()
    End Sub

    Private Sub SwapButton_Click(sender As Object, e As EventArgs) Handles BehaviorsSwapButton.Click,
        SpeechesSwapButton.Click, EffectsSwapButton.Click, InteractionsSwapButton.Click
        Dim swapButton = DirectCast(sender, Button)
        Dim swapPanel = DirectCast(swapButton.Parent, Panel)
        Dim topPanel = GridTable.GetControlFromPosition(0, 0)
        If ReferenceEquals(topPanel, swapPanel) Then
            swapPanel = DirectCast(GridTable.GetControlFromPosition(0, 1), Panel)
        End If
        GridTable.SuspendLayout()
        GridTable.SetColumnSpan(topPanel, 1)
        GridTable.SetCellPosition(topPanel, GridTable.GetCellPosition(swapPanel))
        GridTable.SetCellPosition(swapPanel, New TableLayoutPanelCellPosition(0, 0))
        GridTable.SetColumnSpan(swapPanel, 3)
        GridTable.ResumeLayout()
    End Sub
End Class
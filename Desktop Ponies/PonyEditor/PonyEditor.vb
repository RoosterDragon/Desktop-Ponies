Imports System.Globalization
Imports System.IO
Imports DesktopSprites.SpriteManagement

Public Class PonyEditor
    Private editorAnimator As EditorPonyAnimator
    Private editorInterface As ISpriteCollectionView

    Private ReadOnly worker As New IdleWorker(Me)
    Friend Ponies As PonyCollection
    Private ponyImageList As ImageList
    Private infoGrids As ImmutableArray(Of PonyInfoGrid)

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

    Public Sub New(collection As PonyCollection)
        InitializeComponent()
        Icon = My.Resources.Twilight
        Ponies = Argument.EnsureNotNull(collection, "collection")
    End Sub

    Private Sub PonyEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BeginInvoke(New MethodInvoker(AddressOf LoadInternal))
    End Sub

    Private Sub LoadInternal()
        EnableWaitCursor(True, True)

        Try
            For Each value In DirectCast([Enum].GetValues(GetType(TargetActivation)), TargetActivation())
                colInteractionInteractWith.Items.Add(value.ToString())
            Next

            infoGrids = {New PonyInfoGrid(BehaviorsGrid), New PonyInfoGrid(SpeechesGrid),
                         New PonyInfoGrid(EffectsGrid), New PonyInfoGrid(InteractionsGrid)}.ToImmutableArray()

            'add all possible ponies to the selection window.
            ponyImageList = GenerateImageList(Ponies.AllBases, 50, PonyList.BackColor, Function(b) b.LeftImage.Path)
            PonyList.LargeImageList = ponyImageList
            PonyList.SmallImageList = ponyImageList

            PonyList.SuspendLayout()
            For i = 0 To Ponies.AllBases.Length - 1
                PonyList.Items.Add(New ListViewItem(Ponies.AllBases(i).Directory, i) With {.Tag = Ponies.AllBases(i)})
            Next
            PonyList.ResumeLayout()
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error attempting to load the editor. It will now close.")
            Me.Close()
        End Try

        editorInterface = Options.GetInterface()
        editorInterface.Topmost = True
        editorAnimator = New EditorPonyAnimator(editorInterface, Ponies, Me)
        AddHandler editorAnimator.AnimationFinished, AddressOf PonyEditorAnimator_AnimationFinished
        Enabled = True
        UseWaitCursor = False
    End Sub

    Public Shared Function GenerateImageList(ponyBases As IEnumerable(Of PonyBase), size As Integer, backColor As Color,
                                             pathSelect As Func(Of Behavior, String)) As ImageList
        Argument.EnsureNotNull(ponyBases, "ponyBases")
        Argument.EnsureNotNull(pathSelect, "pathSelect")
        Dim imageList = New ImageList() With {.ImageSize = New Size(size, size)}
        For Each ponyBase In ponyBases
            Dim image = GetListImage(ponyBase, size, backColor, pathSelect)
            imageList.Images.Add(image)
        Next
        Return imageList
    End Function

    Private Shared Function GetListImage(ponyBase As PonyBase, size As Integer, backColor As Color,
                                   pathSelect As Func(Of Behavior, String)) As Bitmap
        Dim dstImage = New Bitmap(size, size)
        Using g = Graphics.FromImage(dstImage)
            g.Clear(backColor)
            Dim srcImage As Image = Nothing
            Try
                srcImage = If(ponyBase.Behaviors.Any(),
                              Bitmap.FromFile(pathSelect(ponyBase.Behaviors(0))),
                              My.Resources.RandomPony)
                g.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
                g.DrawImage(srcImage, 0, 0, size, size)
            Catch ex As Exception
                ' Ignore errors trying to get the image to load, we'll just leave the area blank.
            Finally
                If srcImage IsNot Nothing Then srcImage.Dispose()
            End Try
        End Using
        Return dstImage
    End Function

    Private Sub UpdatePreviewListImage()
        Dim newImage = GetListImage(PreviewPony.Base, 50, PonyList.BackColor, Function(b) b.LeftImage.Path)
        Dim oldImage = ponyImageList.Images(PonyList.SelectedIndices(0))
        ponyImageList.Images(PonyList.SelectedIndices(0)) = newImage
        PonyList.Refresh()
        oldImage.Dispose()
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
        worker.QueueTask(Sub()
                             EnableWaitCursor(True, False)

                             Pony.CurrentViewer = editorInterface
                             Pony.CurrentAnimator = editorAnimator
                             If Not editorAnimator.Started Then
                                 editorAnimator.Start()
                                 PausePonyButton.Enabled = True
                             Else
                                 editorAnimator.Clear()
                                 editorAnimator.Pause(True)
                             End If
                         End Sub)
        worker.QueueTask(Sub()
                             EnableWaitCursor(False, False)

                             SaveSortOrder()
                             RestoreSortOrder()
                         End Sub)

        Dim base = DirectCast(PonyList.SelectedItems(0).Tag, PonyBase)
        Threading.ThreadPool.QueueUserWorkItem(
            Sub()
                _previewPony = New Pony(base)
                worker.QueueTask(Sub()
                                     EnableWaitCursor(False, True)

                                     If PreviewPony.Base.Behaviors.Any() Then editorAnimator.AddPony(PreviewPony)
                                     LoadPonyInfo()

                                     PausePonyButton.Text = "Pause Pony"
                                     Enabled = True
                                     editorAnimator.Resume()
                                     UseWaitCursor = False
                                 End Sub)
            End Sub)
    End Sub

    'This is used to keep track of the links in each behavior chain.
    Private Structure ChainLink
        Public ReadOnly LinkedBehaviorName As CaseInsensitiveString
        Public ReadOnly Series As Integer
        Public ReadOnly Order As Integer
        Public Sub New(linkedBehaviorName As CaseInsensitiveString, series As Integer, order As Integer)
            Me.LinkedBehaviorName = linkedBehaviorName
            Me.Series = series
            Me.Order = order
        End Sub
    End Structure

    Private Sub LoadPonyInfo()
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

            PonyName.Text = PreviewPony.DisplayName

            CurrentBehaviorValueLabel.Text = "N/A"
            TimeLeftValueLabel.Text = "N/A"

            Dim none = New CaseInsensitiveString("None")

            colEffectBehavior.Items.Clear()

            colBehaviorLinked.Items.Clear()
            colBehaviorLinked.Items.Add(none)

            colBehaviorStartSpeech.Items.Clear()
            colBehaviorStartSpeech.Items.Add(none)
            colBehaviorEndSpeech.Items.Clear()
            colBehaviorEndSpeech.Items.Add(none)

            Dim unnamedCounter = 1
            For Each speech In PreviewPony.Base.Speeches
                If speech.Name = "Unnamed" Then
                    speech.Name = "Unnamed #" & unnamedCounter
                    unnamedCounter += 1
                End If

                SpeechesGrid.Rows.Add(
                    speech.Name, speech.Name, speech.Group, PreviewPony.GetBehaviorGroupName(speech.Group), speech.Text,
                    GetFilename(speech.SoundFile), (Not speech.Skip).ToString())
                colBehaviorStartSpeech.Items.Add(speech.Name)
                colBehaviorEndSpeech.Items.Add(speech.Name)
            Next

            For Each behavior In PreviewPony.Behaviors
                colBehaviorLinked.Items.Add(behavior.Name)
                colEffectBehavior.Items.Add(behavior.Name)
            Next

            Dim behaviorSequencesByName = DetermineBehaviorSequences()
            For Each behavior In PreviewPony.Behaviors
                Dim followName As String = ""
                If behavior.OriginalFollowTargetName <> "" Then
                    followName = behavior.OriginalFollowTargetName
                Else
                    If behavior.OriginalDestinationXCoord <> 0 AndAlso behavior.OriginalDestinationYCoord <> 0 Then
                        followName = behavior.OriginalDestinationXCoord & " , " & behavior.OriginalDestinationYCoord
                    Else
                        followName = "Select..."
                    End If
                End If

                Dim chance = "N/A"
                If Not behavior.Skip Then
                    chance = behavior.Chance.ToString("P", CultureInfo.CurrentCulture)
                End If

                BehaviorsGrid.Rows.Add(
                    "Run",
                    behavior.Name,
                    behavior.Name,
                    behavior.Group,
                    PreviewPony.GetBehaviorGroupName(behavior.Group),
                    chance,
                    behavior.MaxDuration,
                    behavior.MinDuration,
                    behavior.Speed,
                    GetFilename(behavior.RightImage.Path),
                    GetFilename(behavior.LeftImage.Path),
                    AllowedMovesToString(behavior.AllowedMovement),
                    If(behavior.StartLineName <> "", behavior.StartLineName, none),
                    If(behavior.EndLineName <> "", behavior.EndLineName, none),
                    followName,
                    If(behavior.LinkedBehaviorName <> "", behavior.LinkedBehaviorName, none),
                    behaviorSequencesByName(behavior.Name),
                    behavior.Skip,
                    behavior.DoNotRepeatImageAnimations)
            Next

            For Each effect In PreviewPony.Base.Effects
                EffectsGrid.Rows.Add(
                    effect.Name,
                    effect.Name,
                    effect.BehaviorName,
                    GetFilename(effect.RightImage.Path),
                    GetFilename(effect.LeftImage.Path),
                    effect.Duration,
                    effect.RepeatDelay,
                    Location_ToString(effect.PlacementDirectionRight),
                    Location_ToString(effect.CenteringRight),
                    Location_ToString(effect.PlacementDirectionLeft),
                    Location_ToString(effect.CenteringLeft),
                    effect.Follow,
                    effect.DoNotRepeatImageAnimations)
            Next

            For Each interaction In PreviewPony.InteractionBases
                InteractionsGrid.Rows.Add(
                    interaction.Name,
                    interaction.Name,
                    interaction.Chance.ToString("P", CultureInfo.CurrentCulture),
                    interaction.Proximity,
                    "Select...",
                    interaction.Activation.ToString(),
                    "Select...",
                    interaction.ReactivationDelay.TotalSeconds)
            Next

            alreadyUpdating = False

            Dim conflicts As New List(Of String)()

            Dim behaviorNames As New HashSet(Of CaseInsensitiveString)()
            Dim effectNames As New HashSet(Of CaseInsensitiveString)()
            For Each behavior In PreviewPony.Behaviors
                If Not behaviorNames.Add(behavior.Name) Then
                    conflicts.Add("Behavior: " & behavior.Name)
                End If
                effectNames.Clear()
                For Each effect In behavior.Effects
                    If Not effectNames.Add(effect.Name) Then
                        conflicts.Add("Effect: " & effect.Name & " of behavior " & behavior.Name)
                    End If
                Next
            Next

            Dim speechNames As New HashSet(Of CaseInsensitiveString)()
            For Each speech In PreviewPony.Base.Speeches
                If Not speechNames.Add(speech.Name) Then
                    conflicts.Add("Speech: " & speech.Name)
                End If
            Next

            Dim interactionNames As New HashSet(Of CaseInsensitiveString)()
            For Each interaction In PreviewPony.InteractionBases
                If Not interactionNames.Add(interaction.Name) Then
                    conflicts.Add("Interaction: " & interaction.Name)
                End If
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

            editorAnimator.CreateEditorMenu(PreviewPony.Base)
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error loading pony parameters. The editor will now close.")
            Me.Close()
        End Try
    End Sub

    Private Function DetermineBehaviorSequences() As Dictionary(Of CaseInsensitiveString, String)
        'Go through each behavior to see which ones are part of a chain, and if so, which order they go in.
        Dim chainsByBehaviorName As New Dictionary(Of CaseInsensitiveString, List(Of ChainLink))()
        Dim linkSeries = 1
        Dim behaviorsByName = PreviewPony.Behaviors.ToDictionary(Function(b) b.Name)
        Dim behaviorNamesInChain = New HashSet(Of CaseInsensitiveString)()
        Dim behaviorNamesLinkedTo = New HashSet(Of CaseInsensitiveString)(PreviewPony.Behaviors.Select(Function(b) b.LinkedBehaviorName))
        For Each behavior In PreviewPony.Behaviors
            ' Don't bother building a chain for behaviors that aren't linked.
            If behavior.LinkedBehaviorName = "" Then Continue For
            ' We will only start chains from the head elements in any chain.
            If behaviorNamesLinkedTo.Contains(behavior.Name) Then Continue For
            Dim behaviorChain As New List(Of ChainLink)()
            Dim nextBehaviorName = behavior.Name
            Dim depth = 1
            Do
                Dim links As List(Of ChainLink) = Nothing
                If Not chainsByBehaviorName.TryGetValue(nextBehaviorName, links) Then
                    links = New List(Of ChainLink)()
                    chainsByBehaviorName(nextBehaviorName) = links
                End If
                links.Add(New ChainLink(nextBehaviorName, linkSeries, depth))
                Dim nextBehavior As Behavior = Nothing
                If Not behaviorsByName.TryGetValue(nextBehaviorName, nextBehavior) Then Exit Do
                nextBehaviorName = nextBehavior.LinkedBehaviorName
                depth += 1
                ' Loop until end of chain, or a circular chain is encountered.
            Loop Until nextBehaviorName = "" OrElse Not behaviorNamesInChain.Add(nextBehaviorName)
            linkSeries += 1
            behaviorNamesInChain.Clear()
        Next

        Dim rowIndexesByName = New Dictionary(Of CaseInsensitiveString, Integer)()
        For i = 0 To BehaviorsGrid.RowCount - 1
            rowIndexesByName.Add(DirectCast(BehaviorsGrid.Rows(i).Cells(colBehaviorOriginalName.Index).Value, CaseInsensitiveString), i)
        Next
        Dim behaviorSequencesByName As New Dictionary(Of CaseInsensitiveString, String)()
        Dim sequence As New System.Text.StringBuilder()
        For Each behavior In PreviewPony.Behaviors
            Dim links As List(Of ChainLink) = Nothing
            If chainsByBehaviorName.TryGetValue(behavior.Name, links) Then
                For Each link In links
                    If sequence.Length > 0 Then
                        sequence.Append(", ")
                    End If
                    sequence.Append(link.Series)
                    sequence.Append("-")
                    sequence.Append(link.Order)
                Next
            End If
            behaviorSequencesByName.Add(behavior.Name, sequence.ToString())
            sequence.Clear()
        Next
        Return behaviorSequencesByName
    End Function

    ''' <summary>
    ''' If we want to run a behavior that has a follow object, we can add it with this.
    ''' </summary>
    ''' <param name="name">The name of the pony to find, or add to the editor, so that it may be interacted with.</param>
    ''' <returns>An existing pony in the editor that can be interacted with, if one exists. Otherwise a new pony started from the template
    ''' with the specified name. If no such pony of template exists, returns null.</returns>
    Private Function AddPony(name As String) As Pony

        Try
            For Each pony In editorAnimator.Ponies()
                If Not Object.ReferenceEquals(PreviewPony, pony) AndAlso pony.Directory = name Then
                    Return pony
                End If
            Next

            For Each ponyBase In Ponies.Bases
                If ponyBase.Directory = name Then
                    Dim newPony = New Pony(ponyBase)
                    newPony.Teleport()
                    editorAnimator.AddPony(newPony)
                    Return newPony
                End If
            Next

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error adding pony to the editor. The editor will now close.")
            Me.Close()
        End Try

        Return Nothing
    End Function

    Public Sub RunBehavior(behavior As Behavior)
        Argument.EnsureNotNull(behavior, "behavior")
        Dim poniesToRemove = editorAnimator.Ponies().Where(Function(p) Not Object.ReferenceEquals(p, PreviewPony)).ToArray()
            For Each pony In poniesToRemove
                editorAnimator.RemovePonyAndReinitializeInteractions(pony)
            Next

            PreviewPony.ActiveEffects.Clear()

            Dim followTarget As Pony = Nothing
            If behavior.OriginalFollowTargetName <> "" Then
                followTarget = AddPony(behavior.OriginalFollowTargetName)

                If followTarget Is Nothing Then
                    MessageBox.Show("The specified pony to follow (" & behavior.OriginalFollowTargetName &
                                    ") for this behavior (" & behavior.Name &
                                    ") does not exist, or has no behaviors. Please review this setting.",
                                    "Cannot Run Behavior", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            End If

            PreviewPony.SelectBehavior(behavior)
            PreviewPony.followTarget = followTarget
    End Sub

    Private Function GetGridItem(Of TPonyIniSerializable As IPonyIniSerializable)(sender As Object, e As DataGridViewCellEventArgs,
                                                                              column As DataGridViewTextBoxColumn,
                                                                              items As IEnumerable(Of TPonyIniSerializable)
                                                                              ) As TPonyIniSerializable
        Dim grid = DirectCast(sender, DataGridView)
        If Not Object.ReferenceEquals(column.DataGridView, grid) Then
            Throw New ArgumentException("column must be a child of the sender grid.")
        End If
        Dim name = DirectCast(grid.Rows(e.RowIndex).Cells(column.Index).Value, CaseInsensitiveString)
        Dim item = items.OnlyOrDefault(Function(i) i.Name = name)

        SaveSortOrder()
        If item Is Nothing Then
            LoadPonyInfo()
            RestoreSortOrder()
        End If
        Return item
    End Function

    Private Sub BehaviorsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles BehaviorsGrid.CellClick
        If e.RowIndex < 0 Then Return
        Try
            Dim behavior = GetGridItem(sender, e, colBehaviorOriginalName, PreviewPony.Base.Behaviors)
            If behavior Is Nothing Then Return

            Select Case e.ColumnIndex
                Case colBehaviorActivate.Index
                    RunBehavior(behavior)

                Case colBehaviorRightImage.Index
                    HidePony()
                    Dim newImagePath = AddPicture(PreviewPony.Directory & " Right Image...")
                    If newImagePath IsNot Nothing Then
                        behavior.RightImage.Path = newImagePath
                        BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorRightImage.Index).Value = GetFilename(newImagePath)
                        ImageSizeCheck(behavior.RightImage.Size)
                    End If
                    ShowPony()

                Case colBehaviorLeftImage.Index
                    HidePony()
                    Dim newImagePath = AddPicture(PreviewPony.Directory & " Left Image...")
                    If newImagePath IsNot Nothing Then
                        behavior.LeftImage.Path = newImagePath
                        BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorLeftImage.Index).Value = GetFilename(newImagePath)
                        ImageSizeCheck(behavior.LeftImage.Size)
                        If Object.ReferenceEquals(behavior, PreviewPony.Behaviors(0)) Then
                            UpdatePreviewListImage()
                        End If
                    End If
                    ShowPony()

                Case colBehaviorFollow.Index
                    HidePony()
                    SetBehaviorFollowParameters(behavior)
                    ShowPony()

            End Select

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Private Sub EffectsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles EffectsGrid.CellClick
        If e.RowIndex < 0 Then Return
        Try
            Dim effect = GetGridItem(sender, e, colEffectOriginalName, PreviewPony.Base.Effects)
            If effect Is Nothing Then Return

            Select Case e.ColumnIndex
                Case colEffectRightImage.Index
                    HidePony()
                    Dim newImagePath As String = AddPicture(effect.Name & " Right Image...")
                    If newImagePath IsNot Nothing Then
                        effect.RightImage.Path = newImagePath
                        EffectsGrid.Rows(e.RowIndex).Cells(colEffectRightImage.Index).Value = GetFilename(newImagePath)
                        hasSaved = False
                    End If
                    ShowPony()

                Case colEffectLeftImage.Index
                    HidePony()
                    Dim newImagePath = AddPicture(effect.Name & " Left Image...")
                    If newImagePath IsNot Nothing Then
                        effect.LeftImage.Path = newImagePath
                        EffectsGrid.Rows(e.RowIndex).Cells(colEffectLeftImage.Index).Value = GetFilename(newImagePath)
                        hasSaved = False
                    End If
                    ShowPony()

            End Select

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try
    End Sub

    Private Sub SpeechesGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles SpeechesGrid.CellClick
        If e.RowIndex < 0 Then Return
        Try
            Dim speech = GetGridItem(sender, e, colSpeechOriginalName, PreviewPony.Base.Speeches)
            If speech Is Nothing Then Return

            Select Case e.ColumnIndex
                Case colSpeechSoundFile.Index
                    HidePony()
                    speech.SoundFile = SetSound()
                    SpeechesGrid.Rows(e.RowIndex).Cells(colSpeechSoundFile.Index).Value = speech.SoundFile
                    hasSaved = False
                    ShowPony()

                Case Else
                    Exit Sub

            End Select

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try
    End Sub

    Private Sub InteractionsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles InteractionsGrid.CellClick
        If e.RowIndex < 0 Then Return
        Try
            Dim interaction = GetGridItem(sender, e, colInteractionOriginalName, PreviewPony.Base.Interactions)
            If interaction Is Nothing Then Return

            Dim changeMade = False
            Select Case e.ColumnIndex
                Case colInteractionTargets.Index, colInteractionBehaviors.Index
                    HidePony()
                    Using form = New NewInteractionDialog(Me)
                        form.ChangeInteraction(interaction)
                        form.ShowDialog(Me)
                        changeMade = form.DialogResult = Windows.Forms.DialogResult.OK
                    End Using
                    ShowPony()

            End Select

            If changeMade Then
                InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionChance.Index).Value =
                    interaction.Chance.ToString("P", CultureInfo.CurrentCulture)
                InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionProximity.Index).Value =
                    interaction.Proximity.ToString(CultureInfo.CurrentCulture)
                InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionInteractWith.Index).Value =
                    interaction.Activation.ToString()
                InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionReactivationDelay.Index).Value =
                    interaction.ReactivationDelay.TotalSeconds.ToString(CultureInfo.CurrentCulture)
                hasSaved = False
            End If

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try
    End Sub

    Private Sub BehaviorsGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles BehaviorsGrid.CellValueChanged
        If alreadyUpdating OrElse e.RowIndex < 0 Then Return
        Try
            Dim behavior = GetGridItem(sender, e, colBehaviorOriginalName, PreviewPony.Base.Behaviors)
            If behavior Is Nothing Then Return

            Dim newValue = BehaviorsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()
            Try
                Select Case e.ColumnIndex
                    Case colBehaviorName.Index
                        If newValue = "" Then
                            MsgBox("You must give a behavior a name. It can't be blank.")
                            BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorName.Index).Value = behavior.Name
                            Exit Sub
                        End If

                        If newValue = behavior.Name Then
                            Exit Sub
                        End If

                        For Each behavior In PreviewPony.Behaviors
                            If behavior.Name = newValue Then
                                MsgBox("Behavior names must be unique. Behavior '" & newValue & "' already exists.")
                                BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorName.Index).Value = behavior.Name
                                Exit Sub
                            End If
                        Next
                        behavior.Name = newValue
                        BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorOriginalName.Index).Value = newValue

                    Case colBehaviorChance.Index
                        behavior.Chance = Double.Parse(
                            Trim(Replace(newValue, CultureInfo.CurrentCulture.NumberFormat.PercentSymbol, "")),
                            CultureInfo.CurrentCulture) / 100

                    Case colBehaviorMaxDuration.Index
                        Dim maxDuration = Double.Parse(newValue, CultureInfo.CurrentCulture)
                        If maxDuration > 0 Then
                            behavior.MaxDuration = maxDuration
                        Else
                            Throw New InvalidDataException("Maximum Duration must be greater than 0")
                        End If

                    Case colBehaviorMinDuration.Index
                        Dim minDuration = Double.Parse(newValue, CultureInfo.CurrentCulture)
                        If minDuration >= 0 Then
                            behavior.MinDuration = minDuration
                        Else
                            Throw New InvalidDataException("Minimum Duration must be greater than or equal to 0")
                        End If

                    Case colBehaviorSpeed.Index
                        behavior.Speed = Double.Parse(newValue, CultureInfo.CurrentCulture)

                    Case colBehaviorMovement.Index
                        If newValue = "" Then
                            behavior.AllowedMovement = AllowedMoves.None
                        Else
                            behavior.AllowedMovement = AllowedMovesFromString(newValue)
                        End If

                    Case colBehaviorStartSpeech.Index
                        If newValue = "None" Then
                            behavior.StartLineName = ""
                        Else
                            behavior.StartLineName = newValue
                        End If

                    Case colBehaviorEndSpeech.Index
                        If newValue = "None" Then
                            behavior.EndLineName = ""
                        Else
                            behavior.EndLineName = newValue
                        End If

                    Case colBehaviorLinked.Index
                        If newValue = "None" Then
                            behavior.LinkedBehaviorName = ""
                        Else
                            behavior.LinkedBehaviorName = newValue
                        End If
                        Dim rowIndexesByName = New Dictionary(Of CaseInsensitiveString, Integer)()
                        For i = 0 To BehaviorsGrid.RowCount - 1
                            rowIndexesByName.Add(
                                DirectCast(BehaviorsGrid.Rows(i).Cells(colBehaviorOriginalName.Index).Value, CaseInsensitiveString), i)
                        Next
                        For Each kvp In DetermineBehaviorSequences()
                            BehaviorsGrid.Rows(rowIndexesByName(kvp.Key)).Cells(colBehaviorLinkOrder.Index).Value = kvp.Value
                        Next

                    Case colBehaviorDoNotRunRandomly.Index
                        behavior.Skip = Boolean.Parse(newValue)

                    Case colBehaviorDoNotRepeatAnimations.Index
                        behavior.DoNotRepeatImageAnimations = Boolean.Parse(newValue)

                    Case colBehaviorGroup.Index
                        Dim newGroupValue = Integer.Parse(newValue, CultureInfo.CurrentCulture)
                        If newGroupValue < 0 Then
                            MsgBox("You can't have a group ID less than 0.")
                            Exit Sub
                        End If
                        behavior.Group = newGroupValue

                    Case colBehaviorGroupName.Index
                        If behavior.Group = behavior.AnyGroup Then
                            MsgBox("You can't change the name of the 'Any' group. This is reserved. " &
                                   "It means the behavior can run at any time, regardless of the current group that is running.")
                            Exit Sub
                        End If

                        If PreviewPony.GetBehaviorGroupName(behavior.Group) = "Unnamed" Then
                            PreviewPony.Base.BehaviorGroups.Add(New BehaviorGroup(newValue, behavior.Group))
                        Else
                            For Each behaviorgroup In PreviewPony.Base.BehaviorGroups
                                If behaviorgroup.Name = newValue Then
                                    MsgBox("Error:  That group name already exists under a different ID.")
                                    Exit Sub
                                End If
                            Next

                            For Each behaviorgroup In PreviewPony.Base.BehaviorGroups
                                If behaviorgroup.Number = behavior.Group Then
                                    behaviorgroup.Name = newValue
                                End If
                            Next
                        End If

                End Select

            Catch ex As Exception
                My.Application.NotifyUserOfNonFatalException(ex, "You entered an invalid value for column '" &
                                                             BehaviorsGrid.Columns(e.ColumnIndex).HeaderText & "': " & newValue)
            End Try
            hasSaved = False

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Private Sub EffectsGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles EffectsGrid.CellValueChanged
        If alreadyUpdating OrElse e.RowIndex < 0 Then Return
        Try
            Dim effect = GetGridItem(sender, e, colEffectOriginalName, PreviewPony.Base.Effects)
            If effect Is Nothing Then Return

            Dim newValue = EffectsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()
            Try

                Select Case e.ColumnIndex
                    Case colEffectName.Index
                        If newValue = "" Then
                            MsgBox("You must give an effect a name. It can't be blank.")
                            EffectsGrid.Rows(e.RowIndex).Cells(colEffectName.Index).Value = effect.Name
                            Exit Sub
                        End If

                        If newValue = effect.Name Then
                            Exit Sub
                        End If

                        For Each effect In GetAllEffects()
                            If effect.Name = newValue Then
                                MsgBox("Effect names must be unique. Effect '" & newValue & "' already exists.")
                                EffectsGrid.Rows(e.RowIndex).Cells(colEffectName.Index).Value = effect.Name
                                Exit Sub
                            End If
                        Next

                        effect.Name = newValue
                        EffectsGrid.Rows(e.RowIndex).Cells(colEffectOriginalName.Index).Value = newValue

                    Case colEffectBehavior.Index
                        effect.BehaviorName = newValue

                    Case colEffectDuration.Index
                        effect.Duration = Double.Parse(newValue, CultureInfo.CurrentCulture)

                    Case colEffectRepeatDelay.Index
                        effect.RepeatDelay = Double.Parse(newValue, CultureInfo.CurrentCulture)

                    Case colEffectLocationRight.Index
                        effect.PlacementDirectionRight = DirectionFromString(newValue)

                    Case colEffectLocationLeft.Index
                        effect.PlacementDirectionLeft = DirectionFromString(newValue)

                    Case colEffectCenteringRight.Index
                        effect.CenteringRight = DirectionFromString(newValue)

                    Case colEffectCenteringLeft.Index
                        effect.CenteringLeft = DirectionFromString(newValue)

                    Case colEffectFollowPony.Index
                        effect.Follow = Boolean.Parse(newValue)

                    Case colEffectDoNotRepeatAnimations.Index
                        effect.DoNotRepeatImageAnimations = Boolean.Parse(newValue)

                End Select

            Catch ex As Exception
                My.Application.NotifyUserOfNonFatalException(ex, "You entered an invalid value for column '" &
                                                             EffectsGrid.Columns(e.ColumnIndex).HeaderText & "': " & newValue)
            End Try
            hasSaved = False

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Private Sub SpeechesGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles SpeechesGrid.CellValueChanged
        If alreadyUpdating OrElse e.RowIndex < 0 Then Return
        Try
            Dim speech = GetGridItem(sender, e, colSpeechOriginalName, PreviewPony.Base.Speeches)
            If speech Is Nothing Then Return

            Dim newValue = SpeechesGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()
            Try

                Select Case e.ColumnIndex
                    Case colSpeechName.Index
                        If newValue = "" Then
                            MsgBox("You must give a speech a name, it can't be blank")
                            SpeechesGrid.Rows(e.RowIndex).Cells(colSpeechName.Index).Value = speech.Name
                            Exit Sub
                        End If

                        If newValue = speech.Name Then
                            Exit Sub
                        End If

                        For Each otherSpeech In PreviewPony.Base.Speeches
                            If otherSpeech.Name = newValue Then
                                MsgBox("Speech names must be unique. Speech '" & newValue & "' already exists.")
                                SpeechesGrid.Rows(e.RowIndex).Cells(colSpeechName.Index).Value = speech.Name
                                Exit Sub
                            End If
                        Next
                        speech.Name = newValue
                        SpeechesGrid.Rows(e.RowIndex).Cells(colSpeechOriginalName.Index).Value = newValue

                    Case colSpeechText.Index
                        speech.Text = newValue

                    Case colSpeechUseRandomly.Index
                        speech.Skip = Not Boolean.Parse(newValue)

                    Case colSpeechGroup.Index
                        speech.Group = Integer.Parse(newValue, CultureInfo.CurrentCulture)

                End Select

            Catch ex As Exception
                My.Application.NotifyUserOfNonFatalException(ex, "You entered an invalid value for column '" &
                                                             SpeechesGrid.Columns(e.ColumnIndex).HeaderText & "': " & newValue)
            End Try
            hasSaved = False

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Private Sub InteractionsGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles InteractionsGrid.CellValueChanged
        If alreadyUpdating OrElse e.RowIndex < 0 Then Return
        Try
            Dim interaction = GetGridItem(sender, e, colInteractionOriginalName, PreviewPony.Base.Interactions)
            If interaction Is Nothing Then Return

            Dim newValue = InteractionsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()
            Try

                Select Case e.ColumnIndex
                    Case colInteractionName.Index
                        If newValue = "" Then
                            MsgBox("You must give an interaction a name. It can't be blank.")
                            InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionName.Index).Value = interaction.Name
                            Exit Sub
                        End If

                        If newValue = interaction.Name Then
                            Exit Sub
                        End If

                        For Each Interaction In PreviewPony.InteractionBases
                            If interaction.Name = newValue Then
                                MsgBox("Interaction with name '" & interaction.Name &
                                       "' already exists for this pony. Please select another name.")
                                InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionName.Index).Value = interaction.Name
                                Exit Sub
                            End If
                        Next

                        interaction.Name = newValue
                        InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionOriginalName.Index).Value = newValue

                    Case colInteractionChance.Index
                        interaction.Chance = Double.Parse(
                            Trim(Replace(newValue, CultureInfo.CurrentCulture.NumberFormat.PercentSymbol, "")),
                            CultureInfo.CurrentCulture) / 100

                    Case colInteractionProximity.Index
                        interaction.Proximity = Double.Parse(newValue, CultureInfo.CurrentCulture)

                    Case colInteractionInteractWith.Index
                        interaction.Activation =
                            CType([Enum].Parse(GetType(TargetActivation), newValue), TargetActivation)

                    Case colInteractionReactivationDelay.Index
                        interaction.ReactivationDelay = TimeSpan.FromSeconds(Double.Parse(newValue, CultureInfo.CurrentCulture))

                End Select

            Catch ex As Exception
                My.Application.NotifyUserOfNonFatalException(ex, "You entered an invalid value for column '" &
                                                             InteractionsGrid.Columns(e.ColumnIndex).HeaderText & "': " & newValue)
            End Try
            hasSaved = False

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Friend Function GetAllEffects() As EffectBase()
        Return Ponies.AllBases.SelectMany(Function(pb) pb.Effects).ToArray()
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
            If Not editorAnimator.Paused Then
                editorAnimator.Pause(False)
                PausePonyButton.Text = "Resume Pony"
            Else
                editorAnimator.Resume()
                PausePonyButton.Text = "Pause Pony"
            End If
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error attempting to pause or resume animation.")
        End Try

    End Sub

    Private Sub HidePony()
        PausePonyButton.Text = "Resume Pony"
        If editorAnimator.Started Then editorAnimator.Pause(False)
        If PreviewPony IsNot Nothing Then editorInterface.Hide()
    End Sub

    Private Sub ShowPony()
        PausePonyButton.Text = "Pause Pony"
        If editorAnimator.Started Then editorAnimator.Resume()
        If PreviewPony IsNot Nothing Then editorInterface.Show()
    End Sub

    Private Sub NewBehaviorButton_Click(sender As Object, e As EventArgs) Handles NewBehaviorButton.Click
        Try

            If IsNothing(PreviewPony) Then
                MessageBox.Show(Me, "Select a pony or create a new one first.",
                                "No Pony Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            HidePony()
            Dim addedNew As Boolean
            Using dialog = New NewBehaviorDialog(Me)
                addedNew = (dialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK)
            End Using
            ShowPony()

            If addedNew Then
                If PreviewPony.Behaviors.Count = 1 Then
                    editorAnimator.AddPony(PreviewPony)
                    UpdatePreviewListImage()
                End If
                PreviewPony.SelectBehavior(PreviewPony.Behaviors(0))
                LoadPonyInfo()
                hasSaved = False
            End If

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error creating new behavior. The editor will now close.")
            Me.Close()
        End Try

    End Sub

    Private Sub NewSpeechButton_Click(sender As Object, e As EventArgs) Handles NewSpeechButton.Click
        Try
            If IsNothing(PreviewPony) Then
                MessageBox.Show(Me, "Select a pony or create a new one first.",
                                "No Pony Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            HidePony()

            Using dialog = New NewSpeechDialog(Me)
                dialog.ShowDialog(Me)
            End Using

            ShowPony()

            LoadPonyInfo()
            hasSaved = False
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error creating new speech. The editor will now close.")
            Me.Close()
        End Try
    End Sub

    Private Sub NewEffectButton_Click(sender As Object, e As EventArgs) Handles NewEffectButton.Click
        Try

            If IsNothing(PreviewPony) Then
                MessageBox.Show(Me, "Select a pony or create a new one first.",
                                "No Pony Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            HidePony()

            Using dialog = New NewEffectDialog(Me)
                dialog.ShowDialog(Me)
            End Using

            ShowPony()

            LoadPonyInfo()
            hasSaved = False
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error creating new effect, the editor will now close.")
            Me.Close()
        End Try
    End Sub

    Private Sub NewInteractionButton_Click(sender As Object, e As EventArgs) Handles NewInteractionButton.Click
        Try

            If IsNothing(PreviewPony) Then
                MessageBox.Show(Me, "Select a pony or create a new one first.",
                                "No Pony Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If

            HidePony()
            Using dialog = New NewInteractionDialog(Me)
                dialog.ChangeInteraction(Nothing)
                dialog.ShowDialog(Me)
            End Using
            ShowPony()

            LoadPonyInfo()
            hasSaved = False

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error creating new interaction. The editor will now close.")
            Me.Close()
        End Try
    End Sub

    Friend Function AddPicture(Optional text As String = Nothing) As String
        Try
            OpenPictureDialog.Filter = "GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png|JPG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg|All Files (*.*)|*.*"
            OpenPictureDialog.FilterIndex = 4
            OpenPictureDialog.InitialDirectory = Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, PreviewPony.Directory)

            If text Is Nothing Then
                OpenPictureDialog.Title = "Select picture..."
            Else
                OpenPictureDialog.Title = "Select picture for: " & text
            End If

            Dim imagePath As String = Nothing

            If OpenPictureDialog.ShowDialog() = DialogResult.OK Then
                imagePath = OpenPictureDialog.FileName
            Else
                Return Nothing
            End If

            ' Try to load this image.
            Image.FromFile(imagePath)

            Dim desiredPath = IO.Path.Combine(Options.InstallLocation, PonyBase.RootDirectory,
                                           PreviewPony.Directory, GetFilename(imagePath))

            If desiredPath <> imagePath Then
                Try
                    If Not File.Exists(desiredPath) Then
                        File.Create(desiredPath).Dispose()
                    End If
                    My.Computer.FileSystem.CopyFile(imagePath, desiredPath, True)
                Catch ex As Exception
                    My.Application.NotifyUserOfNonFatalException(
                        ex, "Couldn't copy the image file to the pony directory." &
                        " If you were trying to use the same image for left and right, you can safely ignore this message.")
                End Try
            End If

            Return desiredPath

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
                Dim effectToRemove = PreviewPony.Base.Effects.Single(
                    Function(effect) effect.Name = DirectCast(e.Row.Cells(colEffectName.Index).Value, CaseInsensitiveString))
                PreviewPony.Base.Effects.Remove(effectToRemove)
            ElseIf Object.ReferenceEquals(grid, BehaviorsGrid) Then
                Dim todelete As Behavior = Nothing
                For Each behavior In PreviewPony.Behaviors
                    If DirectCast(e.Row.Cells(colBehaviorName.Index).Value, CaseInsensitiveString) = behavior.Name Then
                        todelete = behavior
                        Exit For
                    End If
                Next
                If Not IsNothing(todelete) Then
                    PreviewPony.Behaviors.Remove(todelete)
                End If
            ElseIf Object.ReferenceEquals(grid, InteractionsGrid) Then
                Dim todelete As InteractionBase = Nothing
                For Each interaction In PreviewPony.InteractionBases
                    If DirectCast(e.Row.Cells(colInteractionName.Index).Value, CaseInsensitiveString) = interaction.Name Then
                        todelete = interaction
                        Exit For
                    End If
                Next
                If Not IsNothing(todelete) Then
                    PreviewPony.Base.Interactions.Remove(todelete)
                End If
            ElseIf Object.ReferenceEquals(grid, SpeechesGrid) Then
                Dim todelete As Speech = Nothing
                For Each speech In PreviewPony.Base.Speeches
                    If DirectCast(e.Row.Cells(colSpeechName.Index).Value, CaseInsensitiveString) = speech.Name Then
                        todelete = speech
                        Exit For
                    End If
                Next
                If Not IsNothing(todelete) Then
                    PreviewPony.Base.Speeches.Remove(todelete)
                End If
            Else
                Throw New Exception("Unknown grid when deleting row: " & grid.Name)
            End If

            hasSaved = False

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error handling row deletion.")
        End Try

    End Sub

    Private Sub Grid_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles BehaviorsGrid.UserDeletedRow, EffectsGrid.UserDeletedRow,
                                                                                                        InteractionsGrid.UserDeletedRow, SpeechesGrid.UserDeletedRow
        'LoadParameters(Preview_Pony)
        'RestoreSortOrder()
    End Sub

    Private Sub Grid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles SpeechesGrid.DataError, InteractionsGrid.DataError, EffectsGrid.DataError, BehaviorsGrid.DataError
        Try
            Dim grid As DataGridView = DirectCast(sender, DataGridView)

            Dim replacement As Object = ""
            If TypeOf grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value Is CaseInsensitiveString Then
                Dim invalidValue = DirectCast(grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, CaseInsensitiveString)
                If grid.Columns(e.ColumnIndex).CellType = GetType(DataGridViewComboBoxCell) Then
                    ' Combo box cells require a case sensitive match, but we use case-insensitive matching.
                    ' See if a case-insensitive match exists and transparently replace the value.
                    Dim first = True
                    For Each item As CaseInsensitiveString In
                        DirectCast(grid.Rows(e.RowIndex).Cells(e.ColumnIndex), DataGridViewComboBoxCell).Items
                        If item = invalidValue Then
                            grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = item
                            Return
                        End If
                        If first Then
                            replacement = item
                            first = False
                        End If
                    Next
                End If
            End If

            Dim invalidString = grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value.ToString()
            grid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value = replacement

            MessageBox.Show(Me, PonyBase.ConfigFilename & " file appears to have an invalid line: '" & invalidString &
                            "' is not valid for column '" & grid.Columns(e.ColumnIndex).HeaderText & "'" & ControlChars.NewLine &
                            "Details: Column: " & e.ColumnIndex &
                            " Row: " & e.RowIndex & " - " & e.Exception.Message & ControlChars.NewLine & ControlChars.NewLine &
                            "Value will be reset.",
                            "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error trying to handle a data error! The editor will now close.")
            Me.Close()
        End Try
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

    Private Sub SetBehaviorFollowParameters(behavior As Behavior)
        Try

            HidePony()

            Using dialog = New FollowTargetDialog(Me, behavior)
                dialog.ShowDialog(Me)
            End Using
            LoadPonyInfo()

            ShowPony()

            hasSaved = False
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error setting follow parameters. The editor will now close.")
            Me.Close()
        End Try
    End Sub

    Friend Shared Function GetFilename(path As String) As String
        Return IO.Path.GetFileName(path)
    End Function

    Private Sub PonyName_TextChanged(sender As Object, e As EventArgs) Handles PonyName.TextChanged
        If Not alreadyUpdating Then
            PreviewPony.Base.DisplayName = PonyName.Text
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

            Dim base = PonyBase.CreateInMemory(Ponies)
            base.DisplayName = "New Pony"
            _previewPony = New Pony(base)

            Using dialog = New NewPonyDialog(Me)
                If dialog.ShowDialog(Me) = DialogResult.Cancel Then
                    If Not IsNothing(previousPony) Then
                        _previewPony = previousPony
                        ShowPony()
                    End If
                    Exit Sub
                End If
            End Using

            MessageBox.Show(
                Me, "All ponies must now be reloaded. " &
                "Once this operation is complete, you can reopen the editor and select your pony for editing.",
                "Ponies Must Be Reloaded", MessageBoxButtons.OK, MessageBoxIcon.Information)
            _changesMade = True
            Me.Close()

        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "Error creating new pony. The editor will now close.")
            Me.Close()
        End Try

    End Sub

    Friend Function SavePony() As Boolean
        Try
            PausePonyButton.Enabled = False
            If editorAnimator.Started Then editorAnimator.Pause(False)
            _changesMade = True
            PreviewPony.Base.Save()
            RefreshButton_Click(RefreshButton, EventArgs.Empty)
            If editorAnimator.Started Then editorAnimator.Resume()
            PausePonyButton.Enabled = True
        Catch ex As ArgumentException When ex.ParamName = "text"
            MessageBox.Show(Me, "Some invalid characters were detected. Please remove them." & Environment.NewLine &
                            ex.Message.Remove(ex.Message.LastIndexOf(Environment.NewLine, StringComparison.CurrentCulture)),
                            "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        Catch ex As Exception
            My.Application.NotifyUserOfNonFatalException(ex, "There was an unexpected error trying to save the pony.")
            Return False
        End Try
        hasSaved = True
        MessageBox.Show(Me, "Save completed!", "Save Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Return True
    End Function

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
                Return Not SavePony()
            ElseIf result = DialogResult.Cancel Then
                Return True
            End If
        End If
        Return False
    End Function

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        SavePony()
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
            MessageBox.Show(Me, "Select a pony or create a new one first.",
                                "No Pony Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        HidePony()
        Using dialog = New TagsDialog(Me)
            If dialog.ShowDialog(Me) = DialogResult.OK Then hasSaved = False
        End Using
        ShowPony()

    End Sub

    Private Sub ImagesButton_Click(sender As Object, e As EventArgs) Handles ImagesButton.Click
        If PreviewPony Is Nothing Then
            MessageBox.Show(Me, "Select a pony or create a new one first.",
                                "No Pony Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Else
            ImagesContextMenu.Show(ImagesButton, Point.Empty)
        End If
    End Sub

    Private Sub ImagesContextMenu_ItemClicked(sender As Object, e As ToolStripItemClickedEventArgs) Handles ImagesContextMenu.ItemClicked
        HidePony()
        ImagesContextMenu.Hide()
        If Object.ReferenceEquals(e.ClickedItem, ImageCentersMenuItem) Then
            Using form = New ImageCentersForm(Me)
                form.ShowDialog(Me)
            End Using
            hasSaved = False
        ElseIf Object.ReferenceEquals(e.ClickedItem, GifAlphaMenuItem) Then
            Using form = New DesktopSprites.Forms.GifAlphaForm(
                         Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, PreviewPony.Directory))
                form.Icon = Icon
                form.Text &= " - Desktop Ponies"
                form.ShowDialog(Me)
            End Using
        ElseIf Object.ReferenceEquals(e.ClickedItem, GifViewerMenuItem) Then
            Using form = New DesktopSprites.Forms.GifFramesForm(
                         Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, PreviewPony.Directory))
                form.Icon = Icon
                form.Text &= " - Desktop Ponies"
                form.ShowDialog(Me)
            End Using
        End If
        ShowPony()
    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        LoadPonyInfo()
        RestoreSortOrder()
    End Sub

    Private Sub PonyEditorAnimator_AnimationFinished(sender As Object, e As EventArgs)
        SmartInvoke(AddressOf Close)
    End Sub

    Private Sub PonyEditor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If PreventStateChange("Save changes before closing?") Then
            e.Cancel = True
            Return
        End If
        _isClosing = True
        If editorAnimator IsNot Nothing AndAlso Not editorAnimator.Disposed Then
            RemoveHandler editorAnimator.AnimationFinished, AddressOf PonyEditorAnimator_AnimationFinished
            If editorAnimator.Started Then editorAnimator.Finish()
        End If
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing Then
                If components IsNot Nothing Then components.Dispose()
                If editorAnimator IsNot Nothing AndAlso Not editorAnimator.Disposed Then
                    editorAnimator.Finish()
                    editorAnimator.Dispose()
                    If Object.ReferenceEquals(editorAnimator, Pony.CurrentAnimator) Then
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
End Class
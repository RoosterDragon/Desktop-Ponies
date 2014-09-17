Imports System.Globalization
Imports System.IO
Imports DesktopSprites.SpriteManagement

Public Class PonyEditor
    Public Const NoneText = "[None]"

    Private editorAnimator As EditorPonyAnimator
    Private editorInterface As ISpriteCollectionView

    Private worker As IdleWorker
    Private ReadOnly context As New PonyContext()
    Private ponies As PonyCollection
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
    Private _currentBase As PonyBase
    Public ReadOnly Property CurrentBase As PonyBase
        Get
            Return _currentBase
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

    ''' <summary>
    ''' This is used to keep track of the links in each behavior chain.
    ''' </summary>
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

    Public Sub New()
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Private Sub PonyEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BeginInvoke(New MethodInvoker(AddressOf LoadInternal))
    End Sub

    Private Sub LoadInternal()
        EnableWaitCursor(True)

        Try
            For Each value In DirectCast([Enum].GetValues(GetType(Direction)), Direction())
                colEffectLocationLeft.Items.Add(value.ToDisplayString())
                colEffectLocationRight.Items.Add(value.ToDisplayString())
                colEffectCenteringLeft.Items.Add(value.ToDisplayString())
                colEffectCenteringRight.Items.Add(value.ToDisplayString())
            Next

            For Each value In DirectCast([Enum].GetValues(GetType(AllowedMoves)), AllowedMoves())
                colBehaviorMovement.Items.Add(value.ToDisplayString())
            Next

            For Each value In DirectCast([Enum].GetValues(GetType(TargetActivation)), TargetActivation())
                colInteractionInteractWith.Items.Add(value.ToString())
            Next

            infoGrids = {New PonyInfoGrid(BehaviorsGrid), New PonyInfoGrid(SpeechesGrid),
                         New PonyInfoGrid(EffectsGrid), New PonyInfoGrid(InteractionsGrid)}.ToImmutableArray()

            LoadPonies(Nothing)
        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error attempting to load the editor. It will now close.")
            Me.Close()
        End Try

        editorInterface = Options.GetInterface()
        editorInterface.Topmost = True
        editorAnimator = New EditorPonyAnimator(editorInterface, ponies, context, Me)
        AddHandler editorAnimator.AnimationFinished, AddressOf PonyEditorAnimator_AnimationFinished

        worker = New IdleWorker(Me)

        Enabled = True
        UseWaitCursor = False
    End Sub

    Private Sub LoadPonies(initialPonyToShow As String)
        ' Load all ponies, even those which are incomplete.
        ponies = New PonyCollection(False)
        ' Add ponies to the selection window.
        If ponyImageList IsNot Nothing Then ponyImageList.Dispose()
        ponyImageList = GenerateImageList(ponies.Bases, 50, PonyList.BackColor, Function(b) b.LeftImage.Path)
        PonyList.SuspendLayout()
        PonyList.Items.Clear()
        PonyList.LargeImageList = ponyImageList
        PonyList.SmallImageList = ponyImageList
        Dim initialIndex = -1
        For i = 0 To ponies.Bases.Length - 1
            Dim base = ponies.Bases(i)
            Dim directory = base.Directory
            PonyList.Items.Add(New ListViewItem(directory, i) With {.Tag = base})
            If directory = initialPonyToShow Then initialIndex = i
        Next
        PonyList.ResumeLayout()
        If initialIndex <> -1 Then PonyList.SelectedIndices.Add(initialIndex)
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
        Dim newImage = GetListImage(CurrentBase, 50, PonyList.BackColor, Function(b) b.LeftImage.Path)
        Dim oldImage = ponyImageList.Images(PonyList.SelectedIndices(0))
        ponyImageList.Images(PonyList.SelectedIndices(0)) = newImage
        PonyList.Refresh()
        If Not Object.ReferenceEquals(oldImage, My.Resources.RandomPony) Then oldImage.Dispose()
    End Sub

    Private Sub PonyList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PonyList.SelectedIndexChanged
        Try
            If PonyList.SelectedItems.Count = 0 Then Return
            If PreventStateChange("Save changes before loading a different pony?") Then Return
            LoadPony()
            hasSaved = True
        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error attempting to select pony.")
        End Try
    End Sub

    Private Sub LoadPony()
        worker.QueueTask(Sub()
                             EnableWaitCursor(True)

                             If Not editorAnimator.Started Then
                                 editorAnimator.Start()
                                 PausePonyButton.Enabled = True
                             Else
                                 editorAnimator.Clear()
                                 editorAnimator.Pause(True)
                             End If
                         End Sub)
        worker.QueueTask(Sub()
                             EnableWaitCursor(False)

                             SaveSortOrder()
                             RestoreSortOrder()
                         End Sub)
        worker.QueueTask(Sub()
                             EnableWaitCursor(False)

                             _currentBase = DirectCast(PonyList.SelectedItems(0).Tag, PonyBase)
                             LoadPonyInfo()

                             PausePonyButton.Text = "Pause Pony"
                             PonyName.Enabled = True
                             Enabled = True
                             editorAnimator.Resume()
                             UseWaitCursor = False
                         End Sub)
    End Sub

    Private Sub LoadPonyInfo()
        Try
            If alreadyUpdating Then Exit Sub

            If PreviewPony IsNot Nothing Then editorAnimator.RemoveSprite(PreviewPony)
            _previewPony = Nothing
            If CurrentBase.Behaviors.Any() Then
                _previewPony = New Pony(context, CurrentBase)
                context.PendingSprites.Add(PreviewPony)
            End If

            alreadyUpdating = True

            BehaviorsGrid.SuspendLayout()
            EffectsGrid.SuspendLayout()
            InteractionsGrid.SuspendLayout()
            SpeechesGrid.SuspendLayout()

            BehaviorsGrid.Rows.Clear()
            EffectsGrid.Rows.Clear()
            InteractionsGrid.Rows.Clear()
            SpeechesGrid.Rows.Clear()

            PonyName.Text = CurrentBase.DisplayName

            CurrentBehaviorValueLabel.Text = "N/A"
            TimeLeftValueLabel.Text = "N/A"

            Dim none = New CaseInsensitiveString(NoneText)

            colEffectBehavior.Items.Clear()

            colBehaviorLinked.Items.Clear()
            colBehaviorLinked.Items.Add(none)

            colBehaviorStartSpeech.Items.Clear()
            colBehaviorStartSpeech.Items.Add(none)
            colBehaviorEndSpeech.Items.Clear()
            colBehaviorEndSpeech.Items.Add(none)

            Dim unnamedCounter = 1
            For Each speech In CurrentBase.Speeches
                If speech.Name = speech.Unnamed Then
                    speech.Name = speech.Unnamed & " #" & unnamedCounter
                    unnamedCounter += 1
                End If

                SpeechesGrid.Rows.Add(
                    speech.Name, speech.Name, speech.Group, CurrentBase.GetBehaviorGroupName(speech.Group), speech.Text,
                    Path.GetFileName(speech.SoundFile), (Not speech.Skip).ToString())
                colBehaviorStartSpeech.Items.Add(speech.Name)
                colBehaviorEndSpeech.Items.Add(speech.Name)
            Next

            For Each behavior In CurrentBase.Behaviors
                colBehaviorLinked.Items.Add(behavior.Name)
                colEffectBehavior.Items.Add(behavior.Name)
            Next

            Dim behaviorSequencesByName = DetermineBehaviorSequences()
            For Each behavior In CurrentBase.Behaviors
                Dim followName As String = ""
                Select Case behavior.TargetMode
                    Case TargetMode.Pony
                        followName = behavior.FollowTargetName
                    Case TargetMode.Point
                        followName = behavior.TargetVector.X & " , " & behavior.TargetVector.Y
                    Case TargetMode.None
                        followName = "Select..."
                End Select

                Dim chance = "N/A"
                If Not behavior.Skip Then
                    chance = behavior.Chance.ToString("P", CultureInfo.CurrentCulture)
                End If

                BehaviorsGrid.Rows.Add(
                    "Run",
                    behavior.Name,
                    behavior.Name,
                    behavior.Group,
                    CurrentBase.GetBehaviorGroupName(behavior.Group),
                    chance,
                    behavior.MaxDuration.TotalSeconds,
                    behavior.MinDuration.TotalSeconds,
                    behavior.Speed,
                    Path.GetFileName(behavior.RightImage.Path),
                    Path.GetFileName(behavior.LeftImage.Path),
                    behavior.AllowedMovement.ToDisplayString(),
                    If(behavior.StartLineName <> "", behavior.StartLineName, none),
                    If(behavior.EndLineName <> "", behavior.EndLineName, none),
                    followName,
                    If(behavior.LinkedBehaviorName <> "", behavior.LinkedBehaviorName, none),
                    behaviorSequencesByName(behavior.Name),
                    behavior.Skip,
                    behavior.DoNotRepeatImageAnimations)
            Next

            For Each effect In CurrentBase.Effects
                EffectsGrid.Rows.Add(
                    effect.Name,
                    effect.Name,
                    effect.BehaviorName,
                    Path.GetFileName(effect.RightImage.Path),
                    Path.GetFileName(effect.LeftImage.Path),
                    effect.Duration.TotalSeconds,
                    effect.RepeatDelay.TotalSeconds,
                    effect.PlacementDirectionRight.ToDisplayString(),
                    effect.CenteringRight.ToDisplayString(),
                    effect.PlacementDirectionLeft.ToDisplayString(),
                    effect.CenteringLeft.ToDisplayString(),
                    effect.Follow,
                    effect.DoNotRepeatImageAnimations)
            Next

            For Each interaction In CurrentBase.Interactions
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
            For Each behavior In CurrentBase.Behaviors
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
            For Each speech In CurrentBase.Speeches
                If Not speechNames.Add(speech.Name) Then
                    conflicts.Add("Speech: " & speech.Name)
                End If
            Next

            Dim interactionNames As New HashSet(Of CaseInsensitiveString)()
            For Each interaction In CurrentBase.Interactions
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

            editorAnimator.CreateEditorMenu(CurrentBase)
        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error loading pony parameters. The editor will now close.")
            Me.Close()
        End Try
    End Sub

    Private Function DetermineBehaviorSequences() As Dictionary(Of CaseInsensitiveString, String)
        'Go through each behavior to see which ones are part of a chain, and if so, which order they go in.
        Dim chainsByBehaviorName As New Dictionary(Of CaseInsensitiveString, List(Of ChainLink))()
        Dim linkSeries = 1
        Dim behaviorsByName = CurrentBase.Behaviors.ToDictionary(Function(b) b.Name)
        Dim behaviorNamesInChain = New HashSet(Of CaseInsensitiveString)()
        Dim behaviorNamesLinkedTo = New HashSet(Of CaseInsensitiveString)(
                                    CurrentBase.Behaviors.Select(Function(b) b.LinkedBehaviorName))
        For Each behavior In CurrentBase.Behaviors
            ' Don't bother building a chain for behaviors that aren't linked.
            If behavior.LinkedBehaviorName = "" Then Continue For
            ' We will only start chains from the head elements in any chain.
            If behaviorNamesLinkedTo.Contains(behavior.Name) Then Continue For
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
        For Each behavior In CurrentBase.Behaviors
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

    Public Sub RunBehavior(behavior As Behavior)
        Argument.EnsureNotNull(behavior, "behavior")

        Dim poniesToRemove = context.OtherPonies(PreviewPony)
        For Each pony In poniesToRemove
            pony.Expire()
        Next

        PreviewPony.SetBehavior(behavior)
        If Object.ReferenceEquals(PreviewPony.CurrentBehavior, behavior) Then
            If behavior.FollowTargetName <> "" Then
                Dim followTarget As Pony = Nothing
                Dim targetBase = ponies.Bases.OnlyOrDefault(Function(ponyBase) ponyBase.Directory = behavior.FollowTargetName)
                If targetBase IsNot Nothing Then
                    followTarget = New Pony(context, targetBase)
                    context.PendingSprites.Add(followTarget)
                End If

                If followTarget IsNot Nothing Then
                    PreviewPony.FollowTarget = followTarget
                Else
                    MessageBox.Show(Me, "The specified pony to follow (" & behavior.FollowTargetName &
                                    ") for this behavior (" & behavior.Name &
                                    ") does not exist, or has no behaviors. Please review this setting.",
                                    "Follow Target Invalid", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If
        Else
            MessageBox.Show(Me, "This behavior could not be run. Please check it is valid, and that the images exist.",
                            "Run Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
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
            Dim behavior = GetGridItem(sender, e, colBehaviorOriginalName, CurrentBase.Behaviors)
            If behavior Is Nothing Then Return

            Select Case e.ColumnIndex
                Case colBehaviorActivate.Index
                    RunBehavior(behavior)

                Case colBehaviorRightImage.Index
                    UpdateImagePath(BehaviorsGrid, e.RowIndex, colBehaviorRightImage.Index,
                                    Sub(newPath) behavior.RightImage.Path = newPath, CurrentBase.Directory & " Right Image...")

                Case colBehaviorLeftImage.Index
                    UpdateImagePath(BehaviorsGrid, e.RowIndex, colBehaviorLeftImage.Index,
                                    Sub(newPath)
                                        behavior.LeftImage.Path = newPath
                                        If Object.ReferenceEquals(behavior, CurrentBase.Behaviors(0)) Then
                                            UpdatePreviewListImage()
                                        End If
                                    End Sub, CurrentBase.Directory & " Left Image...")

                Case colBehaviorFollow.Index
                    RunMutatingDialog(Function() New FollowTargetDialog(behavior))

            End Select

        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Private Sub EffectsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles EffectsGrid.CellClick
        If e.RowIndex < 0 Then Return
        Try
            Dim effect = GetGridItem(sender, e, colEffectOriginalName, CurrentBase.Effects)
            If effect Is Nothing Then Return

            Select Case e.ColumnIndex
                Case colEffectRightImage.Index
                    UpdateImagePath(EffectsGrid, e.RowIndex, colEffectRightImage.Index,
                                    Sub(newPath) effect.RightImage.Path = newPath, effect.Name & " Right Image...")

                Case colEffectLeftImage.Index
                    UpdateImagePath(EffectsGrid, e.RowIndex, colEffectLeftImage.Index,
                                    Sub(newPath) effect.LeftImage.Path = newPath, effect.Name & " Left Image...")

            End Select

        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try
    End Sub

    Private Sub SpeechesGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles SpeechesGrid.CellClick
        If e.RowIndex < 0 Then Return
        Try
            Dim speech = GetGridItem(sender, e, colSpeechOriginalName, CurrentBase.Speeches)
            If speech Is Nothing Then Return

            Select Case e.ColumnIndex
                Case colSpeechSoundFile.Index
                    HidePony()
                    Dim newSoundPath = EditorCommon.PromptUserForSoundPath(Me, CurrentBase)
                    If newSoundPath IsNot Nothing Then
                        speech.SoundFile = newSoundPath
                        SpeechesGrid.Rows(e.RowIndex).Cells(colSpeechSoundFile.Index).Value = Path.GetFileName(newSoundPath)
                        hasSaved = False
                    End If
                    ShowPony()

            End Select

        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try
    End Sub

    Private Sub InteractionsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles InteractionsGrid.CellClick
        If e.RowIndex < 0 Then Return
        Try
            Dim interaction = GetGridItem(sender, e, colInteractionOriginalName, CurrentBase.Interactions)
            If interaction Is Nothing Then Return

            Dim changeMade = False
            Select Case e.ColumnIndex
                Case colInteractionTargets.Index, colInteractionBehaviors.Index
                    HidePony()
                    Using form = New NewInteractionDialog(interaction, CurrentBase)
                        form.ShowDialog(Me)
                        changeMade = form.DialogResult = DialogResult.OK
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
            Program.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try
    End Sub

    Private Sub UpdateImagePath(grid As DataGridView, rowIndex As Integer, columnIndex As Integer,
                                setNewPath As Action(Of String), promptText As String)
        HidePony()
        Dim newImagePath = EditorCommon.PromptUserForImagePath(Me, CurrentBase, promptText)
        If newImagePath IsNot Nothing Then
            setNewPath(newImagePath)
            grid.Rows(rowIndex).Cells(columnIndex).Value = Path.GetFileName(newImagePath)
            hasSaved = False
        End If
        ShowPony()
    End Sub

    Private Sub BehaviorsGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles BehaviorsGrid.CellValueChanged
        If alreadyUpdating OrElse e.RowIndex < 0 Then Return
        Try
            Dim behavior = GetGridItem(sender, e, colBehaviorOriginalName, CurrentBase.Behaviors)
            If behavior Is Nothing Then Return

            Dim newValue = If(BehaviorsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, "").ToString()
            Try
                Select Case e.ColumnIndex
                    Case colBehaviorName.Index
                        If Not EditorCommon.ValidateName(Me, "behavior", newValue, CurrentBase.Behaviors, behavior.Name) Then Return
                        Dim oldValue = behavior.Name
                        behavior.Name = newValue
                        BehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorOriginalName.Index).Value = behavior.Name

                        For Each b In CurrentBase.Behaviors
                            If b.LinkedBehaviorName = oldValue Then b.LinkedBehaviorName = newValue
                            If b.FollowMovingBehaviorName = oldValue Then b.FollowMovingBehaviorName = newValue
                            If b.FollowStoppedBehaviorName = oldValue Then b.FollowStoppedBehaviorName = newValue
                        Next
                        For Each effect In CurrentBase.Effects
                            If effect.BehaviorName = oldValue Then effect.BehaviorName = newValue
                        Next

                        LoadPonyInfo()

                    Case colBehaviorChance.Index
                        behavior.Chance = Double.Parse(
                            Trim(Replace(newValue, CultureInfo.CurrentCulture.NumberFormat.PercentSymbol, "")),
                            CultureInfo.CurrentCulture) / 100

                    Case colBehaviorMaxDuration.Index
                        Dim maxDuration = Double.Parse(newValue, CultureInfo.CurrentCulture)
                        If maxDuration > 0 Then
                            behavior.MaxDuration = TimeSpan.FromSeconds(maxDuration)
                        Else
                            Throw New InvalidDataException("Maximum Duration must be greater than 0")
                        End If

                    Case colBehaviorMinDuration.Index
                        Dim minDuration = Double.Parse(newValue, CultureInfo.CurrentCulture)
                        If minDuration >= 0 Then
                            behavior.MinDuration = TimeSpan.FromSeconds(minDuration)
                        Else
                            Throw New InvalidDataException("Minimum Duration must be greater than or equal to 0")
                        End If

                    Case colBehaviorSpeed.Index
                        behavior.Speed = Double.Parse(newValue, CultureInfo.CurrentCulture)

                    Case colBehaviorMovement.Index
                        behavior.AllowedMovement = AllowedMovesFromDisplayString(newValue)

                    Case colBehaviorStartSpeech.Index
                        If newValue = NoneText Then
                            behavior.StartLineName = ""
                        Else
                            behavior.StartLineName = newValue
                        End If

                    Case colBehaviorEndSpeech.Index
                        If newValue = NoneText Then
                            behavior.EndLineName = ""
                        Else
                            behavior.EndLineName = newValue
                        End If

                    Case colBehaviorLinked.Index
                        If newValue = NoneText Then
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
                        Dim group = Integer.Parse(newValue, CultureInfo.CurrentCulture)
                        If group >= 0 AndAlso group <= 100 Then
                            behavior.Group = group
                        Else
                            Throw New InvalidDataException("Group must be between 0 and 100 inclusive.")
                        End If

                    Case colBehaviorGroupName.Index
                        If behavior.Group = behavior.AnyGroup Then
                            MessageBox.Show(Me, "You can't change the name of the 'Any' group. This is reserved. " &
                                            "It means the behavior can run at any time, regardless of the current group that is running.",
                                            "Editing Denied", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Return
                        End If

                        If CurrentBase.GetBehaviorGroupName(behavior.Group) Is Nothing Then
                            CurrentBase.BehaviorGroups.Add(New BehaviorGroup(newValue, behavior.Group))
                        Else
                            For Each behaviorgroup In CurrentBase.BehaviorGroups
                                If behaviorgroup.Name = newValue Then
                                    MessageBox.Show(Me, "Another group with this name already exists. " &
                                                    "Try renaming that group first if you want to change the name.",
                                                    "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Return
                                End If
                            Next

                            For Each behaviorgroup In CurrentBase.BehaviorGroups
                                If behaviorgroup.Number = behavior.Group Then
                                    behaviorgroup.Name = newValue
                                End If
                            Next
                        End If

                End Select

            Catch ex As Exception
                Program.NotifyUserOfNonFatalException(ex, "You entered an invalid value for column '" &
                                                             BehaviorsGrid.Columns(e.ColumnIndex).HeaderText & "': " & newValue)
            Finally
                LoadPonyInfo()
            End Try
            hasSaved = False

        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Private Sub EffectsGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles EffectsGrid.CellValueChanged
        If alreadyUpdating OrElse e.RowIndex < 0 Then Return
        Try
            Dim effect = GetGridItem(sender, e, colEffectOriginalName, CurrentBase.Effects)
            If effect Is Nothing Then Return

            Dim newValue = If(EffectsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, "").ToString()
            Try
                Select Case e.ColumnIndex
                    Case colEffectName.Index
                        If Not EditorCommon.ValidateName(Me, "effect", newValue, CurrentBase.Effects, effect.Name) Then Return
                        effect.Name = newValue
                        EffectsGrid.Rows(e.RowIndex).Cells(colEffectOriginalName.Index).Value = effect.Name

                    Case colEffectBehavior.Index
                        effect.BehaviorName = newValue

                    Case colEffectDuration.Index
                        effect.Duration = TimeSpan.FromSeconds(Double.Parse(newValue, CultureInfo.CurrentCulture))

                    Case colEffectRepeatDelay.Index
                        effect.RepeatDelay = TimeSpan.FromSeconds(Double.Parse(newValue, CultureInfo.CurrentCulture))

                    Case colEffectLocationRight.Index
                        effect.PlacementDirectionRight = DirectionFromDisplayString(newValue)

                    Case colEffectLocationLeft.Index
                        effect.PlacementDirectionLeft = DirectionFromDisplayString(newValue)

                    Case colEffectCenteringRight.Index
                        effect.CenteringRight = DirectionFromDisplayString(newValue)

                    Case colEffectCenteringLeft.Index
                        effect.CenteringLeft = DirectionFromDisplayString(newValue)

                    Case colEffectFollowPony.Index
                        effect.Follow = Boolean.Parse(newValue)

                    Case colEffectDoNotRepeatAnimations.Index
                        effect.DoNotRepeatImageAnimations = Boolean.Parse(newValue)

                End Select

            Catch ex As Exception
                Program.NotifyUserOfNonFatalException(ex, "You entered an invalid value for column '" &
                                                             EffectsGrid.Columns(e.ColumnIndex).HeaderText & "': " & newValue)
            Finally
                LoadPonyInfo()
            End Try
            hasSaved = False

        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Private Sub SpeechesGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles SpeechesGrid.CellValueChanged
        If alreadyUpdating OrElse e.RowIndex < 0 Then Return
        Try
            Dim speech = GetGridItem(sender, e, colSpeechOriginalName, CurrentBase.Speeches)
            If speech Is Nothing Then Return

            Dim newValue = If(SpeechesGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, "").ToString()
            Try
                Select Case e.ColumnIndex
                    Case colSpeechName.Index
                        If Not EditorCommon.ValidateName(Me, "speech", newValue, CurrentBase.Speeches, speech.Name) Then Return
                        Dim oldValue = speech.Name
                        speech.Name = newValue
                        SpeechesGrid.Rows(e.RowIndex).Cells(colSpeechOriginalName.Index).Value = speech.Name

                        For Each b In CurrentBase.Behaviors
                            If b.StartLineName = oldValue Then b.StartLineName = newValue
                            If b.EndLineName = oldValue Then b.EndLineName = newValue
                        Next

                        LoadPonyInfo()

                    Case colSpeechText.Index
                        speech.Text = newValue

                    Case colSpeechUseRandomly.Index
                        speech.Skip = Not Boolean.Parse(newValue)

                    Case colSpeechGroup.Index
                        speech.Group = Integer.Parse(newValue, CultureInfo.CurrentCulture)

                End Select

            Catch ex As Exception
                Program.NotifyUserOfNonFatalException(ex, "You entered an invalid value for column '" &
                                                             SpeechesGrid.Columns(e.ColumnIndex).HeaderText & "': " & newValue)
            Finally
                LoadPonyInfo()
            End Try
            hasSaved = False

        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

    Private Sub InteractionsGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles InteractionsGrid.CellValueChanged
        If alreadyUpdating OrElse e.RowIndex < 0 Then Return
        Try
            Dim interaction = GetGridItem(sender, e, colInteractionOriginalName, CurrentBase.Interactions)
            If interaction Is Nothing Then Return

            Dim newValue = If(InteractionsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, "").ToString()
            Try
                Select Case e.ColumnIndex
                    Case colInteractionName.Index
                        If Not EditorCommon.ValidateName(Me, "interaction", newValue, CurrentBase.Interactions, interaction.Name) Then Return
                        interaction.Name = newValue
                        InteractionsGrid.Rows(e.RowIndex).Cells(colInteractionOriginalName.Index).Value = interaction.Name

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
                Program.NotifyUserOfNonFatalException(ex, "You entered an invalid value for column '" &
                                                             InteractionsGrid.Columns(e.ColumnIndex).HeaderText & "': " & newValue)
            Finally
                LoadPonyInfo()
            End Try
            hasSaved = False

        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error altering pony parameters.")
        End Try

    End Sub

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
            Program.NotifyUserOfNonFatalException(ex, "Error restoring sort order for grid.")
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
            Program.NotifyUserOfNonFatalException(ex, "Error attempting to pause or resume animation.")
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
        If RunMutatingDialog(Function() New NewBehaviorDialog(Me)) Then
            If CurrentBase.Behaviors.Count = 1 Then
                _previewPony = New Pony(context, CurrentBase)
                context.PendingSprites.Add(PreviewPony)
                UpdatePreviewListImage()
            End If
            PreviewPony.SetBehavior(CurrentBase.Behaviors(0))
        End If
    End Sub

    Private Sub NewSpeechButton_Click(sender As Object, e As EventArgs) Handles NewSpeechButton.Click
        RunMutatingDialog(Function() New NewSpeechDialog(Me))
    End Sub

    Private Sub NewEffectButton_Click(sender As Object, e As EventArgs) Handles NewEffectButton.Click
        RunMutatingDialog(Function() New NewEffectDialog(Me))
    End Sub

    Private Sub NewInteractionButton_Click(sender As Object, e As EventArgs) Handles NewInteractionButton.Click
        RunMutatingDialog(Function() New NewInteractionDialog(Nothing, CurrentBase))
    End Sub

    Private Function RunMutatingDialog(createDialog As Func(Of Form)) As Boolean
        Try
            If CurrentBase Is Nothing Then
                MessageBox.Show(Me, "Select a pony or create a new one first.",
                                "No Pony Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If

            HidePony()
            Dim changesMade As Boolean
            Using dialog = createDialog()
                changesMade = dialog.ShowDialog(Me) = DialogResult.OK
            End Using
            If changesMade Then
                LoadPonyInfo()
                hasSaved = False
            End If
            ShowPony()

            Return changesMade
        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error attempting to perform this action. The editor will now close.")
            Close()
            Return False
        End Try
    End Function

    Private Sub Grid_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles BehaviorsGrid.UserDeletingRow,
        EffectsGrid.UserDeletingRow, InteractionsGrid.UserDeletingRow, SpeechesGrid.UserDeletingRow
        SaveSortOrder()
        Try
            Dim grid As DataGridView = DirectCast(sender, DataGridView)
            If Object.ReferenceEquals(grid, EffectsGrid) Then
                Dim effectToRemove = CurrentBase.Effects.Single(
                    Function(effect) effect.Name = e.Row.Cells(colEffectName.Index).Value.ToString())
                CurrentBase.Effects.Remove(effectToRemove)
            ElseIf Object.ReferenceEquals(grid, BehaviorsGrid) Then
                Dim behaviorToRemove = CurrentBase.Behaviors.Single(
                    Function(behavior) behavior.Name = e.Row.Cells(colBehaviorName.Index).Value.ToString())
                CurrentBase.Behaviors.Remove(behaviorToRemove)
                UpdatePreviewListImage()
            ElseIf Object.ReferenceEquals(grid, InteractionsGrid) Then
                Dim interactionToRemove = CurrentBase.Interactions.Single(
                    Function(interaction) interaction.Name = e.Row.Cells(colInteractionName.Index).Value.ToString())
                CurrentBase.Interactions.Remove(interactionToRemove)
            ElseIf Object.ReferenceEquals(grid, SpeechesGrid) Then
                Dim speechToRemove = CurrentBase.Speeches.Single(
                    Function(speech) speech.Name = e.Row.Cells(colSpeechName.Index).Value.ToString())
                CurrentBase.Speeches.Remove(speechToRemove)
            Else
                Throw New Exception("Unknown grid when deleting row: " & grid.Name)
            End If

            hasSaved = False
        Catch ex As Exception
            e.Cancel = True
            Program.NotifyUserOfNonFatalException(ex, "Error handling row deletion.")
        End Try
    End Sub

    Private Sub Grid_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles BehaviorsGrid.UserDeletedRow,
        EffectsGrid.UserDeletedRow, InteractionsGrid.UserDeletedRow, SpeechesGrid.UserDeletedRow
        LoadPonyInfo()
        RestoreSortOrder()
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
            Program.NotifyUserOfNonFatalException(ex, "Error trying to handle a data error! The editor will now close.")
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

    Private Sub PonyName_TextChanged(sender As Object, e As EventArgs) Handles PonyName.TextChanged
        If Not alreadyUpdating Then
            CurrentBase.DisplayName = PonyName.Text
            hasSaved = False
        End If
    End Sub

    Private Sub NewPonyButton_Click(sender As Object, e As EventArgs) Handles NewPonyButton.Click
        Try
            If PreventStateChange("Save changes before creating a new pony?") Then Return

            HidePony()
            Dim newBase As PonyBase
            Using dialog = New NewPonyDialog(ponies)
                If dialog.ShowDialog(Me) = DialogResult.Cancel Then
                    ShowPony()
                    Exit Sub
                End If
                newBase = dialog.Base
                _changesMade = True
            End Using

            EnableWaitCursor(True)
            LoadPonies(newBase.Directory)
            Enabled = True
            UseWaitCursor = False

        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "Error creating new pony. The editor will now close.")
            Me.Close()
        End Try
    End Sub

    Private Function SavePony() As Boolean
        Try
            PausePonyButton.Enabled = False
            If editorAnimator.Started Then editorAnimator.Pause(False)
            _changesMade = True
            CurrentBase.Save()
            LoadPonyInfo()
            RestoreSortOrder()
            If editorAnimator.Started Then editorAnimator.Resume()
            PausePonyButton.Enabled = True
        Catch ex As ArgumentException When ex.ParamName = "text"
            MessageBox.Show(Me, "Some invalid characters were detected. Please remove them." & Environment.NewLine &
                            ex.Message.Remove(ex.Message.LastIndexOf(Environment.NewLine, StringComparison.CurrentCulture)),
                            "Invalid Characters", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return False
        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "There was an unexpected error trying to save the pony.")
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

    Private Sub EditTagsButton_Click(sender As Object, e As EventArgs) Handles EditTagsButton.Click
        If CurrentBase Is Nothing Then
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
        If CurrentBase Is Nothing Then
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
            If CurrentBase.Behaviors.Count = 0 Then
                MessageBox.Show(Me, "You need to create some behaviors before you can set up image centers.",
                                "No Behaviors", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                Using form = New ImageCentersForm(CurrentBase)
                    form.ShowDialog(Me)
                    If form.ChangesMade Then hasSaved = False
                End Using
            End If
        ElseIf Object.ReferenceEquals(e.ClickedItem, GifAlphaMenuItem) Then
            Using form = New DesktopSprites.Forms.GifAlphaForm(Path.Combine(PonyBase.RootDirectory, CurrentBase.Directory))
                form.Icon = Icon
                form.Text &= " - Desktop Ponies"
                form.ShowDialog(Me)
            End Using
        ElseIf Object.ReferenceEquals(e.ClickedItem, GifViewerMenuItem) Then
            Using form = New DesktopSprites.Forms.GifFramesForm(Path.Combine(PonyBase.RootDirectory, CurrentBase.Directory))
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
        TryInvoke(AddressOf Close)
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
                End If
                If ponyImageList IsNot Nothing Then ponyImageList.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
        General.FullCollect()
    End Sub
End Class

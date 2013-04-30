Imports System.Globalization
Imports System.IO
Imports CSDesktopPonies.SpriteManagement

Public Class PonyEditor
    ''' <summary>
    ''' The pony displayed in the preview window and where settings are changed (live).
    ''' </summary>
    Friend PreviewPony As Pony

    Friend ReadOnly Property PreviewPonyBase As MutablePonyBase
        Get
            If PreviewPony IsNot Nothing Then
                Return DirectCast(PreviewPony.Base, MutablePonyBase)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Friend IsClosing As Boolean

    Dim grids As New List(Of PonyInfoGrid)
    Dim loaded As Boolean = False

    Dim ponyNameList As New List(Of String)

    ''' <summary>
    ''' Keep track of when the grids are being updated when loading a pony, otherwise we incorrectly think the user is making changes.
    ''' </summary>
    Dim already_updating As Boolean = False

    ''' <summary>
    ''' Keep track of if changes have been made since the last save.
    ''' </summary>
    Dim has_saved As Boolean = True
    ''' <summary>
    ''' Keep track of any saves made at all in the editor. If so, we'll need to reload files when we quit.
    ''' </summary>
    Friend changes_made As Boolean = False

    ''' <summary>
    ''' Used so we can swap grid positions and keep track of how everything is sorted when we refresh.
    ''' </summary>
    Private Class PonyInfoGrid

        Public Grid As DataGridView
        Public Slot As Integer
        Public SwapButton As Button
        Public SortColumn As DataGridViewColumn = Nothing
        Public SortOrder As SortOrder = Nothing

        Sub New(_grid As DataGridView, _slot As Integer, _swapButton As Button)
            Grid = _grid
            Slot = _slot
            SwapButton = _swapButton
        End Sub

    End Class

    Private pe_animator As PonyEditorAnimator
    Private pe_interface As SpriteManagement.ISpriteCollectionView

    Public Sub New()
        InitializeComponent()
        CreateHandle()

        For Each value As Interaction.TargetActivation In [Enum].GetValues(GetType(Interaction.TargetActivation))
            colInteractionInteractWith.Items.Add(value.ToString())
        Next

        Icon = My.Resources.Twilight
    End Sub

    Private Sub PonyEditor_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            has_saved = True
            changes_made = False

            PreviewPony = Nothing

            loaded = False

            PonyBehaviorsGrid.Rows.Clear()
            PonyEffectsGrid.Rows.Clear()
            PonyInteractionsGrid.Rows.Clear()
            PonySpeechesGrid.Rows.Clear()

            grids.Clear()
            grids.Add(New PonyInfoGrid(PonyBehaviorsGrid, 0, NewBehaviorButton))
            grids.Add(New PonyInfoGrid(PonySpeechesGrid, 1, NewSpeechButton))
            grids.Add(New PonyInfoGrid(PonyEffectsGrid, 2, NewEffectButton))
            grids.Add(New PonyInfoGrid(PonyInteractionsGrid, 3, NewInteractionButton))

            PonySelectionView.Items.Clear()

            Dim pony_image_list As New ImageList()
            pony_image_list.ImageSize = New Size(50, 50)

            'add all possible ponies to the selection window.
            ponyNameList.Clear()
            ponyNameList.Capacity = Main.Instance.PonySelectionPanel.Controls.Count
            For Each ponyPanel As PonySelectionControl In Main.Instance.PonySelectionPanel.Controls
                Dim ponyName = ponyPanel.PonyName.Text
                ponyNameList.Add(ponyName)
                pony_image_list.Images.Add(Bitmap.FromFile(Main.Instance.SelectablePonies.Find(Function(base As PonyBase)
                                                                                                   Return ponyName = base.Directory
                                                                                               End Function).Behaviors(0).LeftImagePath))
            Next

            PonySelectionView.LargeImageList = pony_image_list
            PonySelectionView.SmallImageList = pony_image_list

            Dim pony_menu_order As Integer = 0
            For Each PonyName As String In ponyNameList
                PonySelectionView.Items.Add(New ListViewItem(PonyName, pony_menu_order))
                pony_menu_order += 1
            Next

            PonySelectionView.Columns.Add("Pony")

            loaded = True

        Catch ex As Exception
            MsgBox("Error loading the editor..." & ex.Message & ControlChars.NewLine & ex.StackTrace)
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

        pe_interface = Main.Instance.GetInterface()
        pe_interface.Topmost = True
        pe_animator = New PonyEditorAnimator(Me, pe_interface, Nothing)
        AddHandler pe_animator.AnimationFinished, AddressOf PonyEditorAnimator_AnimationFinished
    End Sub

    ''' <summary>
    ''' get the index of the pony by name in the SelectablePonies list.
    ''' This is needed because the order they appear in on the menu is different that in the list of all selectable ponies, due to filters.
    ''' </summary>
    ''' <param name="ponyname"></param>
    ''' <returns></returns>
    Private Shared Function GetPonyOrder(ponyname As String) As Integer

        Dim index As Integer = 0

        For Each ponyBase In Main.Instance.SelectablePonies
            If ponyBase.Directory = ponyname Then
                Return index
            Else
                index += 1
            End If
        Next

        Throw New ArgumentException("Pony not found in selectable pony list.", "ponyname")

    End Function

    Private Sub PonySelectionView_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PonySelectionView.SelectedIndexChanged

        Try
            If PonySelectionView.SelectedItems.Count = 0 Then Exit Sub

            If has_saved = False Then
                If SaveDialog.ShowDialog() = DialogResult.Cancel Then
                    Exit Sub
                End If
            End If

            If PonySelectionView.SelectedIndices.Count = 0 Then Exit Sub

            LoadPony(PonySelectionView.SelectedIndices(0))

            has_saved = True
        Catch ex As Exception
            MsgBox("Error selecting pony..." & ex.Message & ControlChars.NewLine & ex.StackTrace)
            Exit Sub
        End Try
    End Sub

    Private Sub LoadPony(menu_index As Integer)

        Enabled = False

        Dim index As Integer = GetPonyOrder(ponyNameList(menu_index))

        'as everywhere else, we make a copy from the master copy in selectable_ponies
        PreviewPony = New Pony(New MutablePonyBase(Main.Instance.SelectablePonies(index).Directory))

        If PreviewPony.Behaviors.Count = 0 Then
            PreviewPonyBase.AddBehavior("Default", 1, 60, 60, 0, AllowedMoves.None, "", "", "")
        End If

        SaveSortOrder()
        RestoreSortOrder()

        Pony.CurrentViewer = pe_interface
        Pony.CurrentAnimator = pe_animator
        If Not pe_animator.Started Then
            pe_animator.Start()
            PausePonyButton.Enabled = True
        Else
            pe_animator.Pause(True)
            pe_animator.Clear()
        End If

        pe_animator.AddPony(PreviewPony)
        Load_Parameters(PreviewPony)

        PausePonyButton.Text = "Pause Pony"
        pe_animator.Resume()

        Enabled = True

    End Sub

    Private Sub PonyEditor_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If has_saved = False Then
            If SaveDialog.ShowDialog() = DialogResult.Cancel Then
                e.Cancel = True
                Exit Sub
            End If
        End If
        IsClosing = True
        If pe_animator IsNot Nothing AndAlso Not pe_animator.Disposed AndAlso pe_animator.Started Then pe_animator.Pause(True)
        RemoveHandler pe_animator.AnimationFinished, AddressOf PonyEditorAnimator_AnimationFinished
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
        Dim behavior As Behavior
        Dim order As Integer
        Dim series As Integer
    End Structure

    Friend Sub Load_Parameters(pony As Pony)
        Try
            If already_updating Then Exit Sub

            already_updating = True

            PonyBehaviorsGrid.Rows.Clear()
            PonyEffectsGrid.Rows.Clear()
            PonyInteractionsGrid.Rows.Clear()
            PonySpeechesGrid.Rows.Clear()

            PonyName.Text = pony.Name

            CurrentBehaviorValueLabel.Text = "N/A"
            TimeLeftValueLabel.Text = "N/A"

            Dim effect_behavior_list = colEffectBehavior
            effect_behavior_list.Items.Clear()

            Dim linked_behavior_list = colBehaviorLinked
            linked_behavior_list.Items.Clear()
            linked_behavior_list.Items.Add("None")

            Dim start_speech_list = colBehaviorStartSpeech
            Dim end_speech_list = colBehaviorEndSpeech

            start_speech_list.Items.Clear()
            start_speech_list.Items.Add("None")
            end_speech_list.Items.Clear()
            end_speech_list.Items.Add("None")

            Dim unnamed_counter = 1
            For Each speech As Behavior.SpeakingLine In pony.Base.SpeakingLines
                If speech.Name = "Unnamed" Then
                    speech.Name = "Unnamed #" & unnamed_counter
                    unnamed_counter += 1
                End If

                PonySpeechesGrid.Rows.Add(speech.Name, speech.Name, speech.Group, pony.GetBehaviorGroupName(speech.Group), speech.Text, Get_Filename(speech.SoundFile), (Not speech.Skip).ToString())
                start_speech_list.Items.Add(LCase(speech.Name))
                end_speech_list.Items.Add(LCase(speech.Name))
            Next

            For Each behavior In pony.Behaviors
                linked_behavior_list.Items.Add(behavior.Name)
                effect_behavior_list.Items.Add(behavior.Name)
            Next

            'Go through each behavior to see which ones are part of a chain, and if so, which order they go in.
            Dim all_chains As New List(Of ChainLink)

            Dim link_series = 0
            For Each behavior In pony.Behaviors

                If (behavior.LinkedBehaviorName) <> "" AndAlso behavior.LinkedBehaviorName <> "None" Then

                    Dim not_start_of_chain = False
                    For Each other_behavior In pony.Behaviors
                        If other_behavior.LinkedBehaviorName = behavior.Name Then
                            not_start_of_chain = True
                            Exit For
                        End If
                    Next

                    'ignore behaviors that are not the first ones in a chain
                    '(chains that loop forever are ignored)
                    If not_start_of_chain Then Continue For

                    Dim new_link As New ChainLink()
                    new_link.behavior = behavior
                    new_link.order = 1
                    link_series += 1
                    new_link.series = link_series

                    Dim link_order As New List(Of ChainLink)
                    link_order.Add(new_link)

                    Dim next_link = behavior.LinkedBehaviorName

                    Dim depth = 1

                    Dim no_more = False
                    Do Until no_more = True OrElse next_link = "None"
                        Append_Next_Link(next_link, depth, link_series, pony, link_order)
                        depth = depth + 1
                        If (link_order(link_order.Count - 1).behavior.LinkedBehaviorName) = "" OrElse link_order.Count <> depth Then
                            no_more = True
                        Else
                            next_link = link_order(link_order.Count - 1).behavior.LinkedBehaviorName
                        End If
                    Loop

                    For Each link In link_order
                        all_chains.Add(link)
                    Next

                End If
            Next

            For Each behavior In pony.Behaviors

                Dim follow_name As String = ""
                If behavior.OriginalFollowObjectName <> "" Then
                    follow_name = behavior.OriginalFollowObjectName
                Else
                    If behavior.OriginalDestinationXCoord <> 0 AndAlso behavior.OriginalDestinationYCoord <> 0 Then
                        follow_name = behavior.OriginalDestinationXCoord & " , " & behavior.OriginalDestinationYCoord
                    Else
                        follow_name = "Select..."
                    End If
                End If

                Dim link_depth = ""
                For Each link In all_chains
                    If link.behavior.Name = behavior.Name Then
                        If link_depth <> "" Then
                            link_depth += ", " & link.series & "-" & link.order
                        Else
                            link_depth = link.series & "-" & link.order
                        End If

                    End If
                Next

                Dim chance = "N/A"
                If behavior.Skip = False Then
                    chance = behavior.ChanceOfOccurence.ToString("P", CultureInfo.CurrentCulture)
                End If

                With behavior
                    PonyBehaviorsGrid.Rows.Add("Run", .Name, .Name, .Group, pony.GetBehaviorGroupName(.Group), _
                                        chance, _
                                        .MaxDuration, _
                                        .MinDuration, _
                                        .Speed, _
                                        Get_Filename(.RightImagePath), _
                                        Get_Filename(.LeftImagePath), _
                                        Movement_ToString(.AllowedMovement), _
                                        LCase(.StartLineName), _
                                        LCase(.EndLineName), _
                                        follow_name, _
                                        .LinkedBehaviorName, _
                                        link_depth,
                                        .Skip, .DoNotRepeatImageAnimations)

                End With
            Next

            For Each effect In PreviewPonyEffects()
                With effect
                    PonyEffectsGrid.Rows.Add(.Name, _
                                                 .Name, _
                                                 .BehaviorName, _
                                                 Get_Filename(.RightImagePath), _
                                                 Get_Filename(.LeftImagePath), _
                                                 .Duration, _
                                                 .RepeatDelay, _
                                                 Location_ToString(.PlacementDirectionRight), _
                                                 Location_ToString(.CenteringRight), _
                                                 Location_ToString(.PlacementDirectionLeft), _
                                                 Location_ToString(.CenteringLeft), _
                                                 .Follow, .DoNotRepeatImageAnimations)
                End With
            Next

            For Each Interaction As Interaction In PreviewPony.Interactions

                With Interaction
                    PonyInteractionsGrid.Rows.Add(.Name, _
                                                   .Name, _
                                                   .Probability.ToString("P", CultureInfo.CurrentCulture), _
                                                   .Proximity_Activation_Distance, _
                                                   "Select...",
                                                   Interaction.Targets_Activated.ToString(),
                                                   "Select...",
                                                   .ReactivationDelay)

                End With


            Next

            'to make sure that speech match behaviors
            PreviewPonyBase.LinkBehaviors()

            already_updating = False


            Dim Conflicting_names As Boolean = False
            Dim conflicts As New List(Of String)

            For Each behavior In pony.Behaviors
                For Each otherbehavior In pony.Behaviors

                    If ReferenceEquals(behavior, otherbehavior) Then Continue For

                    For Each effect In behavior.Effects
                        For Each othereffect In otherbehavior.Effects

                            If ReferenceEquals(effect, othereffect) Then Continue For

                            If String.Equals(effect.Name, othereffect.Name, StringComparison.OrdinalIgnoreCase) Then
                                Conflicting_names = True
                                If Not conflicts.Contains("Effect: " & effect.Name) Then conflicts.Add("Effect: " & effect.Name)
                            End If
                        Next
                    Next

                    If String.Equals(behavior.Name, otherbehavior.Name, StringComparison.OrdinalIgnoreCase) Then
                        Conflicting_names = True
                        If Not conflicts.Contains("Behavior: " & behavior.Name) Then conflicts.Add("Behavior: " & behavior.Name)
                    End If
                Next

            Next

            For Each speech In pony.Base.SpeakingLines
                For Each otherspeech In pony.Base.SpeakingLines
                    If ReferenceEquals(speech, otherspeech) Then Continue For
                    If String.Equals(speech.Name, otherspeech.Name, StringComparison.OrdinalIgnoreCase) Then
                        Conflicting_names = True
                        If Not conflicts.Contains("Speech: " & speech.Name) Then conflicts.Add("Speech: " & speech.Name)
                    End If
                Next

            Next

            For Each interaction In pony.Interactions
                For Each otherinteraction In pony.Interactions
                    If ReferenceEquals(interaction, otherinteraction) Then Continue For
                    If String.Equals(interaction.Name, otherinteraction.Name, StringComparison.OrdinalIgnoreCase) Then
                        Conflicting_names = True
                        If Not conflicts.Contains("Interaction: " & interaction.Name) Then conflicts.Add("Interaction: " & interaction.Name)
                    End If
                Next

            Next

            If Conflicting_names Then

                Dim conflicts_list As String = ""
                For Each conflict In conflicts
                    conflicts_list += conflict & ControlChars.NewLine
                Next

                MsgBox("Warning: Two or more behaviors, interactions, effects, of speeches have duplicate names.  Please correct these, or the results may be unexpected." & ControlChars.NewLine & conflicts_list)
            End If

            ' Force layouts so scrollbars are correct, otherwise they will be the wrong sizes.
            PonyBehaviorsGrid.PerformLayout()
            PonyEffectsGrid.PerformLayout()
            PonyInteractionsGrid.PerformLayout()
            PonySpeechesGrid.PerformLayout()

        Catch ex As Exception
            MsgBox("Error loading pony parameters! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
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

    Friend Shared Function String_ToMovement(movement As String) As AllowedMoves
        Select Case movement
            Case "None"
                Return AllowedMoves.None
            Case "Horizontal Only"
                Return AllowedMoves.HorizontalOnly
            Case "Vertical Only"
                Return AllowedMoves.VerticalOnly
            Case "Horizontal Vertical"
                Return AllowedMoves.HorizontalVertical
            Case "Diagonal Only"
                Return AllowedMoves.DiagonalOnly
            Case "Diagonal/horizontal"
                Return AllowedMoves.DiagonalHorizontal
            Case "Diagonal/Vertical"
                Return AllowedMoves.DiagonalVertical
            Case "All"
                Return AllowedMoves.All
            Case "MouseOver"
                Return AllowedMoves.MouseOver
            Case "Sleep"
                Return AllowedMoves.Sleep
            Case "Dragged"
                Return AllowedMoves.Dragged
            Case Else
                Throw New ArgumentException("Invalid movement string:" & movement, "movement")
        End Select
    End Function

    Friend Shared Function String_ToLocation(location As String) As Direction
        Select Case location
            Case "Top"
                Return Direction.TopCenter
            Case "Bottom"
                Return Direction.BottomCenter
            Case "Left"
                Return Direction.MiddleLeft
            Case "Right"
                Return Direction.MiddleRight
            Case "Bottom Right"
                Return Direction.BottomRight
            Case "Bottom Left"
                Return Direction.BottomLeft
            Case "Top Right"
                Return Direction.TopRight
            Case "Top Left"
                Return Direction.TopLeft
            Case "Center"
                Return Direction.MiddleCenter
            Case "Any"
                Return Direction.Random
            Case "Any-Not Center"
                Return Direction.RandomNotCenter
            Case Else
                Throw New ArgumentException("Invalid Location/Direction option: " & location, "location")
        End Select
    End Function

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

            For Each ponyBase In Main.Instance.SelectablePonies
                If String.Equals(Trim(ponyBase.Directory), Trim(ponyname), StringComparison.OrdinalIgnoreCase) Then
                    Dim new_pony = New Pony(ponyBase)
                    new_pony.Teleport()
                    pe_animator.AddPony(new_pony)
                    Return new_pony
                End If
            Next

            Return Nothing

        Catch ex As Exception
            MsgBox("Error adding pony to editor! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
            Me.Close()
            Return Nothing
        End Try

        Return Nothing
    End Function

    Private Function AddEffect(name As String) As Effect

        Try

            Dim effects_list = get_effect_list()

            Dim effect As Effect = Nothing

            For Each listing In effects_list
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
            MsgBox("Error adding effect to editor! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
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
                    grid = PonyEffectsGrid
                    replacement = ""
                Case "Pony_Behaviors_Grid"
                    grid = PonyBehaviorsGrid
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
            MsgBox("Error while trying to handle a dataerror! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
            Me.Close()
        End Try

    End Sub

    Private Sub PonyEditor_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        If Me.loaded = False Then Exit Sub

        Dim slot1 As DataGridView = Nothing
        Dim slot2 As DataGridView = Nothing
        Dim slot3 As DataGridView = Nothing

        For Each entry In grids
            Select Case entry.Slot
                Case 1
                    slot1 = entry.Grid
                Case 2
                    slot2 = entry.Grid
                Case 3
                    slot3 = entry.Grid
            End Select
        Next

        slot1.Size = New Size(CInt((Me.Size.Width / 3) - 10), slot1.Size.Height)

        slot2.Location = New Point(CInt((Me.Size.Width / 3) + 5), slot2.Location.Y)
        slot2.Size = New Size(CInt((Me.Size.Width / 3) - 12), slot2.Size.Height)

        slot3.Location = New Point(CInt(2 * (Me.Size.Width / 3)), slot3.Location.Y)
        slot3.Size = New Size(CInt((Me.Size.Width / 3) - 25), slot3.Size.Height)

    End Sub

    Private Sub PonyBehaviorsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles PonyBehaviorsGrid.CellClick

        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim changed_behavior_name As String = CStr(PonyBehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorOriginalName.Index).Value)
            Dim changed_behavior As Behavior = Nothing

            For Each behavior In PreviewPony.Behaviors
                If behavior.Name = changed_behavior_name Then
                    changed_behavior = behavior
                    Exit For
                End If
            Next

            If IsNothing(changed_behavior) Then
                Load_Parameters(PreviewPony)
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
                        PonyBehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorRightImage.Index).Value = Get_Filename(new_image_path)
                        ImageSizeCheck(changed_behavior.RightImageSize)
                    End If
                    ShowPony()
                Case colBehaviorLeftImage.Index
                    HidePony()
                    Dim new_image_path = Add_Picture(PreviewPony.Directory & " Left Image...")
                    If Not IsNothing(new_image_path) Then
                        changed_behavior.SetLeftImagePath(new_image_path)
                        PonyBehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorLeftImage.Index).Value = Get_Filename(new_image_path)
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
            MsgBox("Error altering pony parameters.  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
        End Try

    End Sub

    Private Sub PonySpeechesGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles PonySpeechesGrid.CellClick
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim changed_speech_name As String = CStr(PonySpeechesGrid.Rows(e.RowIndex).Cells(colSpeechOriginalName.Index).Value)
            Dim changed_speech As Behavior.SpeakingLine = Nothing

            For Each speech As Behavior.SpeakingLine In PreviewPonyBase.SpeakingLines
                If speech.Name = changed_speech_name Then
                    changed_speech = speech
                    Exit For
                End If
            Next

            If IsNothing(changed_speech) Then
                Load_Parameters(PreviewPony)
                RestoreSortOrder()
                Exit Sub
            End If

            Dim changes_made_now = False

            Select Case e.ColumnIndex

                Case colSpeechSoundFile.Index
                    HidePony()
                    changed_speech.SoundFile = SetSound()
                    PonySpeechesGrid.Rows(e.RowIndex).Cells(colSpeechSoundFile.Index).Value = changed_speech.SoundFile
                    changes_made_now = True
                    ShowPony()
                Case Else
                    Exit Sub

            End Select

            If changes_made_now Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                has_saved = False
            End If


        Catch ex As Exception
            MsgBox("Error altering pony parameters.  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub PonyEffectsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles PonyEffectsGrid.CellClick
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim changed_effect_name As String = CStr(PonyEffectsGrid.Rows(e.RowIndex).Cells(colEffectOriginalName.Index).Value)
            Dim changed_effect As EffectBase = Nothing

            For Each effect In PreviewPonyEffects()
                If effect.Name = changed_effect_name Then
                    changed_effect = effect
                    Exit For
                End If
            Next

            If IsNothing(changed_effect) Then
                Load_Parameters(PreviewPony)
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
                        PonyEffectsGrid.Rows(e.RowIndex).Cells(colEffectRightImage.Index).Value = Get_Filename(new_image_path)
                        changes_made_now = True
                    End If
                    ShowPony()
                Case colEffectLeftImage.Index
                    HidePony()
                    Dim new_image_path = Add_Picture(changed_effect_name & " Left Image...")
                    If Not IsNothing(new_image_path) Then
                        changed_effect.SetLeftImagePath(new_image_path)
                        PonyEffectsGrid.Rows(e.RowIndex).Cells(colEffectLeftImage.Index).Value = Get_Filename(new_image_path)
                        changes_made_now = True
                    End If
                    ShowPony()
                Case Else
                    Exit Sub

            End Select

            If changes_made_now Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                has_saved = False
            End If


        Catch ex As Exception
            MsgBox("Error altering pony parameters.  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub PonyInteractionsGrid_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles PonyInteractionsGrid.CellClick
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim changed_interaction_name As String = CStr(PonyInteractionsGrid.Rows(e.RowIndex).Cells(colInteractionOriginalName.Index).Value)
            Dim changed_interaction As Interaction = Nothing

            For Each interaction As Interaction In PreviewPony.Interactions
                If interaction.Name = changed_interaction_name Then
                    changed_interaction = interaction
                    Exit For
                End If
            Next

            If IsNothing(changed_interaction) Then
                Load_Parameters(PreviewPony)
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
                has_saved = False
            End If


        Catch ex As Exception
            MsgBox("Error altering pony parameters.  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
        End Try
    End Sub

    Private Sub PonyBehaviorsGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles PonyBehaviorsGrid.CellValueChanged
        If already_updating Then Return
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim new_value As String = CStr(PonyBehaviorsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Dim changed_behavior_name As String = CStr(PonyBehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorOriginalName.Index).Value)
            Dim changed_behavior As Behavior = Nothing

            For Each behavior In PreviewPony.Behaviors
                If behavior.Name = changed_behavior_name Then
                    changed_behavior = behavior
                    Exit For
                End If
            Next

            If IsNothing(changed_behavior) Then
                Load_Parameters(PreviewPony)
                Exit Sub
            End If

            Try

                Select Case e.ColumnIndex

                    Case colBehaviorName.Index
                        If new_value = "" Then
                            MsgBox("You must give a behavior a name.  It can't be blank.")
                            PonyBehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorName.Index).Value = changed_behavior_name
                            Exit Sub
                        End If

                        If new_value = changed_behavior_name Then
                            Exit Sub
                        End If

                        For Each behavior In PreviewPony.Behaviors
                            If String.Equals(behavior.Name, new_value, StringComparison.OrdinalIgnoreCase) Then
                                MsgBox("Behavior names must be unique.  Behavior '" & new_value & "' already exists.")
                                PonyBehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorName.Index).Value = changed_behavior_name
                                Exit Sub
                            End If
                        Next
                        changed_behavior.Name = new_value
                        PonyBehaviorsGrid.Rows(e.RowIndex).Cells(colBehaviorOriginalName.Index).Value = new_value
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
                        changed_behavior.AllowedMovement = String_ToMovement(new_value)
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
                MsgBox("You entered an invalid value for column '" & PonyBehaviorsGrid.Columns(e.ColumnIndex).HeaderText & "': " &
                       CStr(PonyBehaviorsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) & _
                       ControlChars.NewLine & "Details: " & ex.Message)
            End Try

            PreviewPonyBase.LinkBehaviors()

            If already_updating = False Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                has_saved = False
            End If

        Catch ex As Exception
            MsgBox("Error altering pony parameters.  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
        End Try

    End Sub

    Private Sub PonyEffectsGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles PonyEffectsGrid.CellValueChanged
        If already_updating Then Return
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim new_value As String = CStr(PonyEffectsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Dim changed_effect_name As String = CStr(PonyEffectsGrid.Rows(e.RowIndex).Cells(colEffectOriginalName.Index).Value)
            Dim changed_effect As EffectBase = Nothing

            For Each effect In PreviewPonyEffects()
                If effect.Name = changed_effect_name Then
                    changed_effect = effect
                    Exit For
                End If
            Next

            If IsNothing(changed_effect) Then
                Load_Parameters(PreviewPony)
                Exit Sub
            End If

            Try

                Select Case e.ColumnIndex
                    Case colEffectName.Index
                        If new_value = "" Then
                            MsgBox("You must give an effect a name.  It can't be blank.")
                            PonyEffectsGrid.Rows(e.RowIndex).Cells(colEffectName.Index).Value = changed_effect_name
                            Exit Sub
                        End If

                        If new_value = changed_effect_name Then
                            Exit Sub
                        End If

                        For Each effect In get_effect_list()
                            If String.Equals(effect.Name, new_value, StringComparison.OrdinalIgnoreCase) Then
                                MsgBox("Effect names must be unique.  Effect '" & new_value & "' already exists.")
                                PonyEffectsGrid.Rows(e.RowIndex).Cells(colEffectName.Index).Value = changed_effect_name
                                Exit Sub
                            End If
                        Next

                        changed_effect.Name = new_value
                        PonyEffectsGrid.Rows(e.RowIndex).Cells(colEffectOriginalName.Index).Value = new_value
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
                        changed_effect.PlacementDirectionRight = String_ToLocation(new_value)
                    Case colEffectLocationLeft.Index
                        changed_effect.PlacementDirectionLeft = String_ToLocation(new_value)
                    Case colEffectCenteringRight.Index
                        changed_effect.CenteringRight = String_ToLocation(new_value)
                    Case colEffectCenteringLeft.Index
                        changed_effect.CenteringLeft = String_ToLocation(new_value)
                    Case colEffectFollowPony.Index
                        changed_effect.follow = Boolean.Parse(new_value)
                    Case colEffectDoNotRepeatAnimations.Index
                        changed_effect.DoNotRepeatImageAnimations = Boolean.Parse(new_value)
                End Select

            Catch ex As Exception
                MsgBox("You entered an invalid value for column '" & PonyEffectsGrid.Columns(e.ColumnIndex).HeaderText & "': " & CStr(PonyEffectsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) & _
                       ControlChars.NewLine & "Details: " & ex.Message)
            End Try

            PreviewPonyBase.LinkBehaviors()

            If already_updating = False Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                has_saved = False
            End If


        Catch ex As Exception
            MsgBox("Error altering pony parameters.  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
        End Try

    End Sub

    Private Sub PonySpeechesGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles PonySpeechesGrid.CellValueChanged
        If already_updating Then Return
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim new_value As String = CStr(PonySpeechesGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Dim changed_speech_name As String = CStr(PonySpeechesGrid.Rows(e.RowIndex).Cells(colSpeechOriginalName.Index).Value)
            Dim changed_speech As Behavior.SpeakingLine = Nothing

            For Each speech In PreviewPonyBase.SpeakingLines
                If speech.Name = changed_speech_name Then
                    changed_speech = speech
                    Exit For
                End If
            Next

            If IsNothing(changed_speech) Then
                Load_Parameters(PreviewPony)
                Exit Sub
            End If

            Try

                Select Case e.ColumnIndex
                    Case colSpeechName.Index
                        If new_value = "" Then
                            MsgBox("You must give a speech a name, it can't be blank")
                            PonySpeechesGrid.Rows(e.RowIndex).Cells(colSpeechName.Index).Value = changed_speech_name
                            Exit Sub
                        End If

                        If new_value = changed_speech_name Then
                            Exit Sub
                        End If

                        For Each speechname In PreviewPonyBase.SpeakingLines
                            If String.Equals(speechname.Name, new_value, StringComparison.OrdinalIgnoreCase) Then
                                MsgBox("Speech names must be unique.  Speech '" & new_value & "' already exists.")
                                PonySpeechesGrid.Rows(e.RowIndex).Cells(colSpeechName.Index).Value = changed_speech_name
                                Exit Sub
                            End If
                        Next
                        changed_speech.Name = new_value
                        PonySpeechesGrid.Rows(e.RowIndex).Cells(colSpeechOriginalName.Index).Value = new_value
                    Case colSpeechText.Index
                        changed_speech.Text = new_value
                    Case colSpeechUseRandomly.Index
                        changed_speech.Skip = Not (Boolean.Parse(new_value))
                    Case colSpeechGroup.Index
                        changed_speech.Group = Integer.Parse(new_value, CultureInfo.InvariantCulture)
                End Select

            Catch ex As Exception
                MsgBox("You entered an invalid value for column '" & PonySpeechesGrid.Columns(e.ColumnIndex).HeaderText & "': " &
                       CStr(PonySpeechesGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) &
                       ControlChars.NewLine & "Details: " & ex.Message)
                Exit Sub
            End Try

            PreviewPonyBase.LinkBehaviors()

            If already_updating = False Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                has_saved = False
            End If


        Catch ex As Exception
            MsgBox("Error altering pony parameters.  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
        End Try

    End Sub

    Private Sub PonyInteractionsGrid_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles PonyInteractionsGrid.CellValueChanged
        If already_updating Then Return
        Try

            If e.RowIndex < 0 Then Exit Sub

            SaveSortOrder()

            Dim new_value As String = CStr(PonyInteractionsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value)

            Dim changed_interaction_name As String = CStr(PonyInteractionsGrid.Rows(e.RowIndex).Cells(colInteractionOriginalName.Index).Value)
            Dim changed_interaction As Interaction = Nothing

            For Each interaction In PreviewPony.Interactions
                If interaction.Name = changed_interaction_name Then
                    changed_interaction = interaction
                    Exit For
                End If
            Next

            If IsNothing(changed_interaction) Then
                Load_Parameters(PreviewPony)
                Exit Sub
            End If

            Try

                Select Case e.ColumnIndex
                    Case colInteractionName.Index
                        If new_value = "" Then
                            MsgBox("You must give an interaction a name.  It can't be blank.")
                            PonyInteractionsGrid.Rows(e.RowIndex).Cells(colInteractionName.Index).Value = changed_interaction_name
                            Exit Sub
                        End If

                        If new_value = changed_interaction_name Then
                            Exit Sub
                        End If

                        For Each Interaction In PreviewPony.Interactions
                            If String.Equals(Interaction.Name, new_value, StringComparison.OrdinalIgnoreCase) Then
                                MsgBox("Interaction with name '" & Interaction.Name & "' already exists for this pony.  Please select another name.")
                                PonyInteractionsGrid.Rows(e.RowIndex).Cells(colInteractionName.Index).Value = changed_interaction_name
                                Exit Sub
                            End If
                        Next

                        changed_interaction.Name = new_value
                        PonyInteractionsGrid.Rows(e.RowIndex).Cells(colInteractionOriginalName.Index).Value = new_value
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
                MsgBox("You entered an invalid value for column '" & PonyInteractionsGrid.Columns(e.ColumnIndex).HeaderText & "': " &
                       CStr(PonyInteractionsGrid.Rows(e.RowIndex).Cells(e.ColumnIndex).Value) & _
                       ControlChars.NewLine & "Details: " & ex.Message)
            End Try

            If already_updating = False Then
                'Load_Parameters(Preview_Pony)
                'RestoreSortOrder()
                has_saved = False
            End If

        Catch ex As Exception
            MsgBox("Error altering pony parameters.  Details: " & ex.Message & ControlChars.NewLine & ex.StackTrace)
        End Try

    End Sub

    Friend Shared Function get_effect_list() As List(Of EffectBase)

        Dim effect_list As New List(Of EffectBase)

        For Each ponyBase In Main.Instance.SelectablePonies
            For Each behavior As Behavior In ponyBase.Behaviors
                For Each effect In behavior.Effects
                    effect_list.Add(effect)
                Next
            Next
        Next

        Return effect_list
    End Function

    Private Function PreviewPonyEffects() As IEnumerable(Of EffectBase)
        Return PreviewPony.Behaviors.SelectMany(Function(behavior) (behavior.Effects))
    End Function

    'Swap positions of the grids.
    Private Sub Swap0_1_Click(sender As Object, e As EventArgs) Handles Swap0_1.Click, Swap1_0.Click
        SwapGrids(0, 1, Slot0Label, Slot1Label)
    End Sub
    Private Sub Swap2_0_Click(sender As Object, e As EventArgs) Handles Swap2_0.Click
        SwapGrids(2, 0, Slot2Label, Slot0Label)
    End Sub
    Private Sub Swap3_0_Click(sender As Object, e As EventArgs) Handles Swap3_0.Click
        SwapGrids(3, 0, Slot3Label, Slot0Label)
    End Sub

    Private Sub SwapGrids(slot0_number As Integer, slot1_number As Integer, label0 As Label, label1 As Label)

        Dim slot0 As DataGridView = Nothing
        Dim slot1 As DataGridView = Nothing
        Dim button0 As Button = Nothing
        Dim button1 As Button = Nothing

        For Each entry In grids
            Select Case entry.Slot
                Case slot0_number
                    slot0 = entry.Grid
                    button0 = entry.SwapButton
                    entry.Slot = slot1_number
                Case slot1_number
                    slot1 = entry.Grid
                    entry.Slot = slot0_number
                    button1 = entry.SwapButton
            End Select
        Next

        If IsNothing(slot0) OrElse IsNothing(slot1) OrElse IsNothing(button0) OrElse IsNothing(button1) Then
            Throw New Exception("Error in swap_grids: did not find right control.")
        End If

        Dim tempsize As Size = slot0.Size
        Dim templocation As Point = slot0.Location
        Dim temp_button_size As Size = button0.Size
        Dim temp_button_loc As Point = button0.Location
        Dim temp_button_anchor = button0.Anchor
        Dim templabel As String = label0.Text
        Dim tempanchor = slot0.Anchor

        slot0.Location = slot1.Location
        slot0.Size = slot1.Size
        slot0.Anchor = slot1.Anchor
        label0.Text = label1.Text
        button0.Size = button1.Size
        button0.Location = button1.Location
        button0.Anchor = button1.Anchor

        slot1.Location = templocation
        slot1.Size = tempsize
        slot1.Anchor = tempanchor
        label1.Text = templabel
        button1.Size = temp_button_size
        button1.Location = temp_button_loc
        button1.Anchor = temp_button_anchor

    End Sub

    Private Sub SaveSortOrder()
        For Each control As PonyInfoGrid In grids
            control.SortColumn = control.Grid.SortedColumn
            control.SortOrder = control.Grid.SortOrder
        Next
    End Sub

    Private Sub RestoreSortOrder()
        Try

            For Each control As PonyInfoGrid In grids
                If IsNothing(control.SortColumn) Then Continue For
                control.Grid.Sort(control.SortColumn, ConvertSortOrder(control.SortOrder))
            Next

        Catch ex As Exception
            MsgBox("Error restore sort order for grid. " & ex.Message & ex.StackTrace)
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
            MsgBox("Error on pause/resume! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
        End Try

    End Sub

    Private Sub HidePony()

        PausePonyButton.Text = "Resume Pony"

        If pe_animator.Started Then
            pe_animator.Pause(False)
        End If
        If Not IsNothing(PreviewPony) Then
            pe_interface.Hide()

            'Preview_Pony.Form.Visible = False
        End If

    End Sub

    Private Sub ShowPony()
        PausePonyButton.Text = "Pause Pony"

        If pe_animator.Started Then
            pe_animator.Resume()
        End If

        If Not IsNothing(PreviewPony) Then

            pe_interface.Show()

            'Preview_Pony.Form.Visible = True
        End If
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

            Load_Parameters(PreviewPony)
            has_saved = False

        Catch ex As Exception
            MsgBox("Error creating new behavior! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
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

            Load_Parameters(PreviewPony)
            has_saved = False
        Catch ex As Exception
            MsgBox("Error creating new speech! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
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

            Load_Parameters(PreviewPony)
            has_saved = False
        Catch ex As Exception
            MsgBox("Error creating new effect! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
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

            Load_Parameters(PreviewPony)
            has_saved = False

        Catch ex As Exception
            MsgBox("Error creating new interaction! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
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
                                           PreviewPony.Directory, Get_Filename(picture_path))

            If new_path <> picture_path Then
                Try
                    If Not My.Computer.FileSystem.FileExists(new_path) Then
                        File.Create(new_path).Close()
                    End If
                    My.Computer.FileSystem.CopyFile(picture_path, new_path, True)
                Catch ex As Exception
                    MsgBox("Warning: Couldn't copy the image file to the pony directory. If you were trying to use the same image for left and right, you can safely ignore this message. " _
                           & ControlChars.NewLine & "Details: " & ex.Message)
                End Try
            End If

            Return new_path

            has_saved = False

        Catch ex As Exception

            MsgBox("Error loading image.  Details: " & ex.Message)
            Return Nothing

        End Try

    End Function

    Function SetSound() As String
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
                                       PreviewPony.Directory, Get_Filename(sound_path))

        If new_path <> sound_path Then
            If Not My.Computer.FileSystem.FileExists(new_path) Then
                File.Create(new_path).Close()
            End If
            My.Computer.FileSystem.CopyFile(sound_path, new_path, True)
        End If

        Return Get_Filename(sound_path)

    End Function

    Private Sub Grid_UserDeletingRow(sender As Object, e As DataGridViewRowCancelEventArgs) Handles PonyBehaviorsGrid.UserDeletingRow, PonyEffectsGrid.UserDeletingRow,
                                                                                                            PonyInteractionsGrid.UserDeletingRow, PonySpeechesGrid.UserDeletingRow
        Try
            SaveSortOrder()
            Dim grid As DataGridView = DirectCast(sender, DataGridView)

            If Object.ReferenceEquals(grid, PonyEffectsGrid) Then
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
            ElseIf Object.ReferenceEquals(grid, PonyBehaviorsGrid) Then
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
            ElseIf Object.ReferenceEquals(grid, PonyInteractionsGrid) Then
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
            ElseIf Object.ReferenceEquals(grid, PonySpeechesGrid) Then
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

            has_saved = False

            PreviewPonyBase.LinkBehaviors()

        Catch ex As Exception
            MsgBox("Error handling row deletion! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
        End Try

    End Sub

    Private Sub Grid_UserDeletedRow(sender As Object, e As DataGridViewRowEventArgs) Handles PonyBehaviorsGrid.UserDeletedRow, PonyEffectsGrid.UserDeletedRow,
                                                                                                        PonyInteractionsGrid.UserDeletedRow, PonySpeechesGrid.UserDeletedRow
        'Load_Parameters(Preview_Pony)
        'RestoreSortOrder()
    End Sub

    Private Sub Grid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles PonySpeechesGrid.DataError, PonyInteractionsGrid.DataError, PonyEffectsGrid.DataError, PonyBehaviorsGrid.DataError
        MessageBox.Show(Me, e.Exception.ToString(), "Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
    End Sub

    Private Sub Set_Behavior_Follow_Parameters(behavior As Behavior)
        Try

            HidePony()

            Using form = New FollowTargetDialog(Me)
                form.Change_Behavior(behavior)
                form.ShowDialog()
            End Using
            Load_Parameters(PreviewPony)

            ShowPony()

            has_saved = False
        Catch ex As Exception
            MsgBox("Error setting follow parameters! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
            Me.Close()
        End Try
    End Sub

    Friend Function Get_Filename(path As String) As String
        Try
            Dim path_components = Split(path, IO.Path.DirectorySeparatorChar)

            'force lowercase for compatibility with other ports (like Browser ponies)
            Return LCase(path_components(UBound(path_components)))
        Catch ex As Exception
            MsgBox("Error getting filename from path! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
            Me.Close()
            Return Nothing
        End Try
    End Function

    Private Sub PonyName_TextChanged(sender As Object, e As EventArgs) Handles PonyName.TextChanged

        If already_updating = False Then
            PreviewPonyBase.Name = PonyName.Text
            has_saved = False
        End If

    End Sub

    Private Sub NewPonyButton_Click(sender As Object, e As EventArgs) Handles NewPonyButton.Click

        Try

            If has_saved = False Then
                If SaveDialog.ShowDialog() = DialogResult.Cancel Then
                    Exit Sub
                End If
            End If

            HidePony()

            Dim previous_pony As Pony = Nothing
            If Not IsNothing(PreviewPony) Then
                previous_pony = PreviewPony
            End If

            Dim ponyBase = New MutablePonyBase()
            ponyBase.Name = "New Pony"
            PreviewPony = New Pony(ponyBase)

            'Preview_Pony.Form.Visible = False

            Using form = New NewPonyDialog(Me)
                If form.ShowDialog() = DialogResult.Cancel Then
                    If Not IsNothing(previous_pony) Then
                        PreviewPony = previous_pony
                        ShowPony()
                    End If
                    Exit Sub
                End If
            End Using

            MsgBox("All ponies must now be reloaded. Once this operation is complete, you can reopen the editor and select your pony for editing.")
            changes_made = True
            Me.Close()

        Catch ex As Exception
            MsgBox("Error creating new pony! " & ex.Message & ControlChars.NewLine & ex.StackTrace)
            Me.Close()
        End Try

    End Sub

    Friend Sub SavePony(path As String)
        Try
            'rebuild the list of ponies, in the original order
            Dim temp_list As New List(Of PonyBase)
            For Each Pony In Main.Instance.SelectablePonies
                If Pony.Directory <> PreviewPony.Directory Then
                    temp_list.Add(Pony)
                Else
                    temp_list.Add(PreviewPonyBase)
                End If
            Next

            Main.Instance.SelectablePonies = temp_list

            Dim comments As New List(Of String)
            Dim ponyIniFilePath = IO.Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, path, PonyBase.ConfigFilename)
            If My.Computer.FileSystem.FileExists(ponyIniFilePath) Then
                Using existing_ini As New StreamReader(ponyIniFilePath)
                    Do Until existing_ini.EndOfStream
                        Dim line = existing_ini.ReadLine()
                        If line.Length > 0 AndAlso line(0) = "'" Then
                            comments.Add(line)
                        End If
                    Loop
                End Using
            End If

            Using newPonyIniFile As New StreamWriter(ponyIniFilePath, False, System.Text.Encoding.UTF8)
                For Each line In comments
                    newPonyIniFile.WriteLine(line)
                Next

                newPonyIniFile.WriteLine(String.Join(",", "Name", PreviewPony.Name))
                newPonyIniFile.WriteLine(String.Join(",", "Categories", String.Join(",", PreviewPony.Tags.Select(Function(tag As String)
                                                                                                                     Return Quoted(tag)
                                                                                                                 End Function))))

                For Each behaviorGroup In PreviewPony.BehaviorGroups
                    newPonyIniFile.WriteLine(behaviorGroup.GetPonyIni())
                Next

                For Each behavior In PreviewPony.Behaviors
                    newPonyIniFile.WriteLine(behavior.GetPonyIni())
                Next

                For Each effect In PreviewPonyEffects()
                    newPonyIniFile.WriteLine(effect.GetPonyIni())
                Next

                For Each speech As Behavior.SpeakingLine In PreviewPonyBase.SpeakingLines
                    newPonyIniFile.WriteLine(speech.GetPonyIni())
                Next
            End Using

            Try

                Dim interactions_lines As New List(Of String)

                Using reader = New StreamReader(IO.Path.Combine(
                                                Options.InstallLocation, PonyBase.RootDirectory, Interaction.ConfigFilename))
                    Do Until reader.EndOfStream
                        Dim line = reader.ReadLine()
                        Dim name_check = CommaSplitQuoteQualified(line)

                        If UBound(name_check) > 2 Then
                            If name_check(1) = PreviewPony.Directory Then
                                Continue Do
                            End If
                        End If
                        interactions_lines.Add(line)
                    Loop
                End Using

                Using writer = New StreamWriter(IO.Path.Combine(
                                                Options.InstallLocation, PonyBase.RootDirectory, Interaction.ConfigFilename),
                                            False, System.Text.Encoding.UTF8)
                    If IsNothing(writer) Then Throw New Exception("Unable to write back to interactions.ini file...")

                    For Each line In interactions_lines
                        writer.WriteLine(line)
                    Next

                    For Each interaction In PreviewPony.Interactions
                        writer.WriteLine(interaction.GetPonyIni())
                    Next
                End Using

            Catch ex As Exception
                Throw New Exception("Failed while updating interactions: " & ex.Message, ex)
            End Try
        Catch ex As Exception
            MsgBox("Unable to save pony! Details: " & ControlChars.NewLine & ex.Message)
            Exit Sub
        End Try

        has_saved = True
        MsgBox("Save completed!")

    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        PausePonyButton.Enabled = False
        pe_animator.Pause(False)

        changes_made = True
        SavePony(PreviewPony.Directory)

        RefreshButton_Click(Nothing, Nothing)

        pe_animator.Resume()
        PausePonyButton.Enabled = True

    End Sub

    Sub ImageSizeCheck(imageSize As Size)

        If imageSize.Height > PonyPreviewPanel.Size.Height OrElse
            imageSize.Width > PonyPreviewPanel.Size.Width Then
            MsgBox("Note:  The selected image is too large for the Pony Editor's preview window.  The results shown will not be accurate, but the pony will still work fine.")
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
        has_saved = False
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
        has_saved = False
        ShowPony()

    End Sub

    Private Sub RefreshButton_Click(sender As Object, e As EventArgs) Handles RefreshButton.Click
        Load_Parameters(PreviewPony)
        RestoreSortOrder()
    End Sub

    Private Sub PonyEditorAnimator_AnimationFinished(sender As Object, e As EventArgs)
        Invoke(Sub() Close())
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
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
End Class
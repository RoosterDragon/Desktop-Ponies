Friend Class BehaviorEditor
    Private Shared allowedMovesValues As Object() =
        [Enum].GetValues(GetType(AllowedMoves)).Cast(Of Object)().ToArray()

    Private Shadows Property Edited As Behavior
        Get
            Return DirectCast(MyBase.Edited, Behavior)
        End Get
        Set(value As Behavior)
            MyBase.Edited = value
        End Set
    End Property
    Protected Overrides ReadOnly Property Collection As System.Collections.IList
        Get
            Return CType(Base.Behaviors, Collections.IList)
        End Get
    End Property
    Protected Overrides ReadOnly Property ItemTypeName As String
        Get
            Return "behavior"
        End Get
    End Property

    Private imageListCrossRefreshNeeded As Boolean

    Protected Overrides Sub CreateBindings()
        MovementComboBox.Items.AddRange(allowedMovesValues)
        Bind(Function() Edited.Name, NameTextBox)
        Bind(Function() Edited.Group, GroupNumber, Function(int) CDec(int), Function(dec) CInt(dec))
        Bind(Function() Edited.Chance, ChanceNumber, Function(dbl) CDec(dbl) * 100, Function(dec) dec / 100)
        Bind(Function() Edited.Speed, SpeedNumber, Function(dbl) CDec(dbl), Function(dec) CDbl(dec))
        Bind(Function() Edited.AllowedMovement, MovementComboBox)
        Bind(Function() Edited.MinDuration, MinDurationNumber, Function(dbl) CDec(dbl), Function(dec) CDbl(dec))
        Bind(Function() Edited.MaxDuration, MaxDurationNumber, Function(dbl) CDec(dbl), Function(dec) CDbl(dec))
        Bind(Function() Edited.StartLineName, StartSpeechComboBox)
        Bind(Function() Edited.EndLineName, EndSpeechComboBox)
        Bind(Function() Edited.LinkedBehaviorName, LinkedBehaviorComboBox)
        Bind(Function() Edited.LeftImage.Path, LeftImageFileSelector, LeftImageViewer)
        Bind(Function() Edited.RightImage.Path, RightImageFileSelector, RightImageViewer)
    End Sub

    Protected Overrides Sub ChangeItem()
        RemoveHandler LeftImageFileSelector.ListRefreshed, AddressOf LeftImageFileSelector_ListRefreshed
        RemoveHandler RightImageFileSelector.ListRefreshed, AddressOf RightImageFileSelector_ListRefreshed
        LeftImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        RightImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        AddHandler LeftImageFileSelector.ListRefreshed, AddressOf LeftImageFileSelector_ListRefreshed
        AddHandler RightImageFileSelector.ListRefreshed, AddressOf RightImageFileSelector_ListRefreshed

        Dim speeches = Base.Speeches.Select(Function(s) s.Name).ToArray()
        ReplaceItemsInComboBox(StartSpeechComboBox, speeches)
        ReplaceItemsInComboBox(EndSpeechComboBox, speeches)

        Dim behaviors = Base.Behaviors.Where(Function(b) Not Object.ReferenceEquals(b, Edited)).Select(Function(s) s.Name).ToArray()
        ReplaceItemsInComboBox(LinkedBehaviorComboBox, behaviors)
    End Sub

    Protected Overrides Sub ReparseSource(ByRef parseIssues As ImmutableArray(Of ParseIssue))
        Dim b As Behavior = Nothing
        Behavior.TryLoad(Source.Text, PonyBasePath, Base, b, parseIssues)
        Edited = b

        Dim duration As TimeSpan? = TimeSpan.FromSeconds(Math.Max(Edited.MinDuration, Edited.MaxDuration))
        LeftImageViewer.FixedAnimationDuration = duration
        RightImageViewer.FixedAnimationDuration = duration
    End Sub

    Public Overrides Sub AnimateImages(animate As Boolean)
        LeftImageViewer.Animate = animate
        RightImageViewer.Animate = animate
    End Sub

    Private Sub LeftImageFileSelector_ListRefreshed(sender As Object, e As EventArgs)
        imageListCrossRefreshNeeded = Not imageListCrossRefreshNeeded
        If imageListCrossRefreshNeeded Then RightImageFileSelector.ReinitializeFromCurrentDirectory()
    End Sub

    Private Sub RightImageFileSelector_ListRefreshed(sender As Object, e As EventArgs)
        imageListCrossRefreshNeeded = Not imageListCrossRefreshNeeded
        If imageListCrossRefreshNeeded Then LeftImageFileSelector.ReinitializeFromCurrentDirectory()
    End Sub

    Private Sub ImageViewer_Resize(sender As Object, e As EventArgs) Handles LeftImageViewer.Resize, RightImageViewer.Resize
        DirectCast(sender, Control).Anchor = AnchorStyles.Top
    End Sub

    Private Sub TargetButton_Click(sender As Object, e As EventArgs) Handles TargetButton.Click
        Using dialog As New FollowTargetDialog(Edited)
            If dialog.ShowDialog(Me) = DialogResult.OK Then
                OnItemPropertyChanged()
            End If
        End Using
    End Sub
End Class

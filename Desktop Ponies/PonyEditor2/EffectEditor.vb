Friend Class EffectEditor
    Private Shared directionValues As Object() =
        [Enum].GetValues(GetType(Direction)).Cast(Of Object)().ToArray()

    Private Shadows Property Edited As EffectBase
        Get
            Return DirectCast(MyBase.Edited, EffectBase)
        End Get
        Set(value As EffectBase)
            MyBase.Edited = value
        End Set
    End Property
    Protected Overrides ReadOnly Property Collection As System.Collections.IList
        Get
            Return CType(Base.Effects, Collections.IList)
        End Get
    End Property
    Protected Overrides ReadOnly Property ItemTypeName As String
        Get
            Return "effect"
        End Get
    End Property

    Private imageListCrossRefreshNeeded As Boolean

    Protected Overrides Sub CreateBindings()
        LeftPlacementComboBox.Items.AddRange(directionValues)
        LeftCenterComboBox.Items.AddRange(directionValues)
        RightPlacementComboBox.Items.AddRange(directionValues)
        RightCenterComboBox.Items.AddRange(directionValues)
        Bind(Function() Edited.Name, NameTextBox)
        Bind(Function() Edited.Duration, DurationNumber, Function(ts) CDec(ts.TotalSeconds), Function(dec) TimeSpan.FromSeconds(dec))
        Bind(Function() Edited.RepeatDelay, RepeatDelayNumber, Function(ts) CDec(ts.TotalSeconds), Function(dec) TimeSpan.FromSeconds(dec))
        Bind(Function() Edited.BehaviorName, BehaviorComboBox)
        Bind(Function() Edited.Follow, FollowCheckBox)
        Bind(Function() Edited.DoNotRepeatImageAnimations, PreventLoopCheckBox)
        Bind(Function() Edited.PlacementDirectionLeft, LeftPlacementComboBox)
        Bind(Function() Edited.PlacementDirectionRight, RightPlacementComboBox)
        Bind(Function() Edited.CenteringLeft, LeftCenterComboBox)
        Bind(Function() Edited.CenteringRight, RightCenterComboBox)
        Bind(Function() Edited.LeftImage.Path, LeftImageFileSelector, LeftImageViewer,
             BehaviorComboBox, AddressOf Edited.GetBehavior, True)
        Bind(Function() Edited.RightImage.Path, RightImageFileSelector, RightImageViewer,
             BehaviorComboBox, AddressOf Edited.GetBehavior, False)
    End Sub

    Protected Overrides Sub ChangeItem()
        RemoveHandler LeftImageFileSelector.ListRefreshed, AddressOf LeftImageFileSelector_ListRefreshed
        RemoveHandler RightImageFileSelector.ListRefreshed, AddressOf RightImageFileSelector_ListRefreshed
        LeftImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        RightImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        AddHandler LeftImageFileSelector.ListRefreshed, AddressOf LeftImageFileSelector_ListRefreshed
        AddHandler RightImageFileSelector.ListRefreshed, AddressOf RightImageFileSelector_ListRefreshed

        Dim behaviors = Base.Behaviors.Select(Function(b) b.Name).ToArray()
        ReplaceItemsInComboBox(BehaviorComboBox, behaviors)
    End Sub

    Protected Overrides Sub ReparseSource(ByRef parseIssues As ImmutableArray(Of ParseIssue))
        Dim e As EffectBase = Nothing
        EffectBase.TryLoad(Source.Text, PonyBasePath, Base, e, parseIssues)
        Edited = e

        Dim duration As TimeSpan?
        If Edited.Duration <> TimeSpan.Zero Then
            duration = Edited.Duration
        Else
            Dim behavior = Edited.GetBehavior()
            If behavior IsNot Nothing Then duration = behavior.MinDuration
        End If
        LeftImageViewer.Placement = Edited.PlacementDirectionLeft
        LeftImageViewer.Centering = Edited.CenteringLeft
        LeftImageViewer.FixedAnimationDuration = duration
        RightImageViewer.Placement = Edited.PlacementDirectionRight
        RightImageViewer.Centering = Edited.CenteringRight
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
End Class

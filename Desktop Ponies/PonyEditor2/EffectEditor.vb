Imports System.IO

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
    Private leftBehaviorImagePath As String
    Private leftEffectImagePath As String
    Private rightBehaviorImagePath As String
    Private rightEffectImagePath As String

    Public Sub New()
        InitializeComponent()
        LeftPlacementComboBox.Items.AddRange(directionValues)
        LeftCenterComboBox.Items.AddRange(directionValues)
        RightPlacementComboBox.Items.AddRange(directionValues)
        RightCenterComboBox.Items.AddRange(directionValues)
        AddHandler NameTextBox.KeyPress, AddressOf IgnoreQuoteCharacter
        AddHandler NameTextBox.TextChanged, Sub() UpdateProperty(Sub() Edited.Name = NameTextBox.Text)
        AddHandler DurationNumber.ValueChanged, Sub() UpdateProperty(Sub() Edited.Duration = DurationNumber.Value)
        AddHandler RepeatDelayNumber.ValueChanged, Sub() UpdateProperty(Sub() Edited.RepeatDelay = RepeatDelayNumber.Value)
        AddHandler BehaviorComboBox.KeyPress, AddressOf IgnoreQuoteCharacter
        AddHandler BehaviorComboBox.SelectedIndexChanged, Sub() UpdateProperty(Sub() Edited.BehaviorName = BehaviorComboBox.Text)
        AddHandler BehaviorComboBox.TextChanged, Sub() UpdateProperty(Sub() Edited.BehaviorName = BehaviorComboBox.Text)
        AddHandler FollowCheckBox.CheckedChanged, Sub() UpdateProperty(Sub() Edited.Follow = FollowCheckBox.Checked)
        AddHandler PreventLoopCheckBox.CheckedChanged,
            Sub() UpdateProperty(Sub() Edited.DoNotRepeatImageAnimations = PreventLoopCheckBox.Checked)
        AddHandler LeftPlacementComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub() Edited.PlacementDirectionLeft = DirectCast(LeftPlacementComboBox.SelectedItem, Direction))
        AddHandler RightPlacementComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub() Edited.PlacementDirectionRight = DirectCast(RightPlacementComboBox.SelectedItem, Direction))
        AddHandler LeftCenterComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub() Edited.CenteringLeft = DirectCast(LeftCenterComboBox.SelectedItem, Direction))
        AddHandler RightCenterComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub() Edited.CenteringRight = DirectCast(RightCenterComboBox.SelectedItem, Direction))

        AddHandler LeftImageFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim filePath = If(LeftImageFileSelector.FilePath = Nothing,
                                                       Nothing, Path.Combine(PonyBasePath, LeftImageFileSelector.FilePath))
                                     Edited.LeftImage.Path = filePath
                                 End Sub)
        AddHandler RightImageFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim filePath = If(RightImageFileSelector.FilePath = Nothing,
                                                       Nothing, Path.Combine(PonyBasePath, RightImageFileSelector.FilePath))
                                     Edited.RightImage.Path = filePath
                                 End Sub)
    End Sub

    Public Overrides Sub NewItem(name As String)
        ' TODO.
    End Sub

    Public Overrides Sub LoadItem()
        LeftImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        RightImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        AddHandler LeftImageFileSelector.ListRefreshed, AddressOf LeftImageFileSelector_ListRefreshed
        AddHandler RightImageFileSelector.ListRefreshed, AddressOf RightImageFileSelector_ListRefreshed

        Dim behaviors = Base.Behaviors.Select(Function(b) b.Name).ToArray()
        ReplaceItemsInComboBox(BehaviorComboBox, behaviors, True)
    End Sub

    Public Overrides Sub AnimateImages(animate As Boolean)
        LeftImageViewer.Animate = animate
        RightImageViewer.Animate = animate
    End Sub

    Private lastTypedLeftFileName As String
    Private lastTypedRightFileName As String
    Private lastTypedLeftFileNameMissing As Boolean
    Private lastTypedRightFileNameMissing As Boolean

    Protected Overrides Sub SourceTextChanged()
        Dim e As EffectBase = Nothing
        EffectBase.TryLoad(Source.Text, PonyBasePath, Base, e, ParseIssues)
        ReferentialIssues = e.GetReferentialIssues(Base.Collection)
        OnIssuesChanged(EventArgs.Empty)
        Edited = e

        NameTextBox.Text = Edited.Name
        DurationNumber.Value = CDec(Edited.Duration)
        RepeatDelayNumber.Value = CDec(Edited.RepeatDelay)
        FollowCheckBox.Checked = Edited.Follow
        PreventLoopCheckBox.Checked = Edited.DoNotRepeatImageAnimations

        SelectItemElseNoneOption(LeftPlacementComboBox, Edited.PlacementDirectionLeft)
        SelectItemElseNoneOption(LeftCenterComboBox, Edited.CenteringLeft)
        SelectItemElseNoneOption(RightPlacementComboBox, Edited.PlacementDirectionRight)
        SelectItemElseNoneOption(RightCenterComboBox, Edited.CenteringRight)
        SelectOrOvertypeItem(BehaviorComboBox, Edited.BehaviorName)

        SyncTypedImagePath(LeftImageFileSelector, Edited.LeftImage.Path,
                           Sub(filePath) Edited.LeftImage.Path = Path.Combine(PonyBasePath, filePath),
                           lastTypedLeftFileName, lastTypedLeftFileNameMissing)
        SyncTypedImagePath(RightImageFileSelector, Edited.RightImage.Path,
                           Sub(filePath) Edited.RightImage.Path = Path.Combine(PonyBasePath, filePath),
                           lastTypedRightFileName, lastTypedRightFileNameMissing)
        LeftImageViewer.Placement = Edited.PlacementDirectionLeft
        LeftImageViewer.Centering = Edited.CenteringLeft
        RightImageViewer.Placement = Edited.PlacementDirectionRight
        RightImageViewer.Centering = Edited.CenteringRight

        Dim behavior = Base.Behaviors.OnlyOrDefault(Function(b) b.Name = Edited.BehaviorName)

        Dim newLeftBehaviorImagePath As String = Nothing
        If behavior IsNot Nothing Then newLeftBehaviorImagePath = behavior.LeftImage.Path
        If leftBehaviorImagePath <> newLeftBehaviorImagePath OrElse leftEffectImagePath <> LeftImageFileSelector.FilePath Then
            leftBehaviorImagePath = newLeftBehaviorImagePath
            leftEffectImagePath = LeftImageFileSelector.FilePath
            UpdateImageDisplay(LeftImageFileSelector, LeftImageViewer, leftBehaviorImagePath)
        End If

        Dim newRightBehaviorImagePath As String = Nothing
        If behavior IsNot Nothing Then newRightBehaviorImagePath = behavior.RightImage.Path
        If rightBehaviorImagePath <> newRightBehaviorImagePath OrElse rightEffectImagePath <> RightImageFileSelector.FilePath Then
            rightBehaviorImagePath = newRightBehaviorImagePath
            rightEffectImagePath = RightImageFileSelector.FilePath
            UpdateImageDisplay(RightImageFileSelector, RightImageViewer, rightBehaviorImagePath)
        End If
    End Sub

    Private Sub UpdateImageDisplay(selector As FileSelector, viewer As EffectImageViewer, behaviorImagePath As String)
        LoadNewImageForViewer(selector, viewer, BehaviorComboBox, behaviorImagePath)
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

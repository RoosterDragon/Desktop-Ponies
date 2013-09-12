Imports System.IO
Imports DesktopSprites.SpriteManagement

Friend Class EffectEditor
    Private Shared directionValues As Object() =
        [Enum].GetValues(GetType(Direction)).Cast(Of Object)().ToArray()

    Private Shadows Property Original As EffectBase
        Get
            Return DirectCast(MyBase.Original, EffectBase)
        End Get
        Set(value As EffectBase)
            MyBase.Original = value
        End Set
    End Property
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

    Public Sub New()
        InitializeComponent()
        LeftPlacementComboBox.Items.AddRange(directionValues)
        LeftCenterComboBox.Items.AddRange(directionValues)
        RightPlacementComboBox.Items.AddRange(directionValues)
        RightCenterComboBox.Items.AddRange(directionValues)
        AddHandler NameTextBox.TextChanged, Sub() UpdateProperty(Sub() Edited.Name = NameTextBox.Text)
        AddHandler DurationNumber.ValueChanged, Sub() UpdateProperty(Sub() Edited.Duration = DurationNumber.Value)
        AddHandler RepeatDelayNumber.ValueChanged, Sub() UpdateProperty(Sub() Edited.RepeatDelay = RepeatDelayNumber.Value)
        AddHandler BehaviorComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub()
                                     Dim behavior = TryCast(BehaviorComboBox.SelectedItem, Behavior)
                                     Edited.BehaviorName = If(behavior Is Nothing, Nothing, behavior.Name)
                                 End Sub)
        AddHandler FollowCheckBox.CheckedChanged, Sub() UpdateProperty(Sub() Edited.Follow = FollowCheckBox.Checked)
        AddHandler PreventLoopCheckBox.CheckedChanged,
            Sub() UpdateProperty(Sub() Edited.DoNotRepeatImageAnimations = PreventLoopCheckBox.Checked)
        AddHandler LeftPlacementComboBox.SelectedIndexChanged,
            Sub()
                Dim direction = DirectCast(LeftPlacementComboBox.SelectedItem, Direction)
                LeftImageViewer.Placement = direction
                Edited.PlacementDirectionLeft = direction
            End Sub
        AddHandler RightPlacementComboBox.SelectedIndexChanged,
            Sub()
                Dim direction = DirectCast(RightPlacementComboBox.SelectedItem, Direction)
                RightImageViewer.Placement = direction
                Edited.PlacementDirectionRight = direction
            End Sub
        AddHandler LeftCenterComboBox.SelectedIndexChanged,
            Sub()
                Dim direction = DirectCast(LeftCenterComboBox.SelectedItem, Direction)
                LeftImageViewer.Centering = direction
                Edited.CenteringLeft = direction
            End Sub
        AddHandler RightCenterComboBox.SelectedIndexChanged,
            Sub()
                Dim direction = DirectCast(RightCenterComboBox.SelectedItem, Direction)
                RightImageViewer.Centering = direction
                Edited.CenteringRight = direction
            End Sub
        AddHandler LeftImageFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim filePath = If(LeftImageFileSelector.FilePath = Nothing,
                                                       Nothing, Path.Combine(PonyBasePath, LeftImageFileSelector.FilePath))
                                     Edited.LeftImage.Path = filePath
                                 End Sub)

        Dim updateLeftImage = Sub(sender As Object, e As EventArgs)
                                  Dim behavior = Base.Behaviors.SingleOrDefault(Function(b) b.Name = Edited.BehaviorName)
                                  Dim behaviorFilePath As String = Nothing
                                  If behavior IsNot Nothing Then behaviorFilePath = behavior.LeftImage.Path
                                  LoadNewImageForViewer(LeftImageFileSelector, LeftImageViewer, BehaviorComboBox, behaviorFilePath)
                              End Sub
        AddHandler LeftImageFileSelector.FilePathSelected, updateLeftImage
        AddHandler BehaviorComboBox.SelectedIndexChanged, updateLeftImage

        AddHandler RightImageFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim filePath = If(RightImageFileSelector.FilePath = Nothing,
                                                       Nothing, Path.Combine(PonyBasePath, RightImageFileSelector.FilePath))
                                     Edited.RightImage.Path = filePath
                                 End Sub)

        Dim updateRightImage = Sub(sender As Object, e As EventArgs)
                                   Dim behavior = Base.Behaviors.SingleOrDefault(Function(b) b.Name = Edited.BehaviorName)
                                   Dim behaviorFilePath As String = Nothing
                                   If behavior IsNot Nothing Then behaviorFilePath = behavior.RightImage.Path
                                   LoadNewImageForViewer(RightImageFileSelector, RightImageViewer, BehaviorComboBox, behaviorFilePath)
                               End Sub
        AddHandler RightImageFileSelector.FilePathSelected, updateRightImage
        AddHandler BehaviorComboBox.SelectedIndexChanged, updateRightImage
    End Sub

    Public Overrides Sub NewItem(name As String)
        ' TODO.
    End Sub

    Public Overrides Sub LoadItem()
        LeftImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        RightImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        AddHandler LeftImageFileSelector.ListRefreshed, AddressOf LeftImageFileSelector_ListRefreshed
        AddHandler RightImageFileSelector.ListRefreshed, AddressOf RightImageFileSelector_ListRefreshed
        'SelectItemElseAddItem(LeftImageFileSelector.FilePathComboBox, Path.GetFileName(Edited.LeftImagePath))
        'SelectItemElseAddItem(RightImageFileSelector.FilePathComboBox, Path.GetFileName(Edited.RightImagePath))

        Dim behaviors = Base.Behaviors.Select(Function(b) b.Name).ToArray()
        ReplaceItemsInComboBox(BehaviorComboBox, behaviors, True)
    End Sub

    Public Overrides Sub AnimateImages(animate As Boolean)
        LeftImageViewer.Animate = animate
        RightImageViewer.Animate = animate
    End Sub

    Private Sub NameTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles NameTextBox.KeyPress
        e.Handled = (e.KeyChar = """"c)
    End Sub

    Private lastTypedLeftFileName As String
    Private lastTypedRightFileName As String
    Private lastTypedLeftFileNameMissing As Boolean
    Private lastTypedRightFileNameMissing As Boolean

    Protected Overrides Sub SourceTextChanged()
        Dim e As EffectBase = Nothing
        EffectBase.TryLoad(Source.Text, PonyBasePath, Base, e, ParseIssues)
        ReferentialIssues = e.GetReferentialIssues()
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

        SyncTypedImagePath(LeftImageFileSelector, Edited.LeftImage.Path, Sub(path) Edited.LeftImage.Path = path,
                           lastTypedLeftFileName, lastTypedLeftFileNameMissing)
        SyncTypedImagePath(RightImageFileSelector, Edited.RightImage.Path, Sub(path) Edited.RightImage.Path = path,
                           lastTypedRightFileName, lastTypedRightFileNameMissing)
    End Sub

    Private Sub SyncTypedImagePath(selector As FileSelector, behaviorImagePath As String, setPath As Action(Of String),
                                   ByRef typedPath As String, ByRef typedPathMissing As Boolean)
        If typedPathMissing Then
            selector.FilePathComboBox.Items.Remove(typedPath)
        End If
        typedPath = Path.GetFileName(behaviorImagePath)
        If typedPath = Base.Directory OrElse typedPath = "" Then typedPath = Nothing
        selector.FilePath = typedPath
        typedPathMissing = Not String.Equals(selector.FilePath, typedPath, PathComparison.Current)
        If typedPathMissing Then
            selector.FilePathComboBox.Items.Add(typedPath)
            selector.FilePathComboBox.SelectedIndex = selector.FilePathComboBox.Items.Count - 1
        End If
        setPath(selector.FilePath)
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

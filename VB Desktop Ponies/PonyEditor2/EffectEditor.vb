Imports System.IO
Imports CSDesktopPonies.SpriteManagement

Friend Class EffectEditor
    Private Shared directionValues As Object() =
        [Enum].GetValues(GetType(Direction)).Cast(Of Object)().ToArray()

    Private originalEffect As EffectBase
    Private newEffect As EffectBase

    Private imageListCrossRefreshNeeded As Boolean

    Public Sub New()
        InitializeComponent()
        LeftPlacementComboBox.Items.AddRange(directionValues)
        LeftCenterComboBox.Items.AddRange(directionValues)
        RightPlacementComboBox.Items.AddRange(directionValues)
        RightCenterComboBox.Items.AddRange(directionValues)
        AddHandler NameTextBox.TextChanged, Sub() UpdateProperty(Sub() newEffect.Name = NameTextBox.Text)
        AddHandler DurationNumber.ValueChanged, Sub() UpdateProperty(Sub() newEffect.Duration = DurationNumber.Value)
        AddHandler RepeatDelayNumber.ValueChanged, Sub() UpdateProperty(Sub() newEffect.RepeatDelay = RepeatDelayNumber.Value)
        AddHandler BehaviorComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub()
                                     Dim behavior = TryCast(BehaviorComboBox.SelectedItem, Behavior)
                                     newEffect.BehaviorName = If(behavior Is Nothing, Nothing, behavior.Name)
                                 End Sub)
        AddHandler FollowCheckBox.CheckedChanged, Sub() UpdateProperty(Sub() newEffect.Follow = FollowCheckBox.Checked)
        AddHandler PreventLoopCheckBox.CheckedChanged,
            Sub() UpdateProperty(Sub() newEffect.DoNotRepeatImageAnimations = PreventLoopCheckBox.Checked)
        AddHandler LeftPlacementComboBox.SelectedIndexChanged,
            Sub()
                Dim direction = DirectCast(LeftPlacementComboBox.SelectedItem, Direction)
                LeftImageViewer.Placement = direction
                newEffect.PlacementDirectionLeft = direction
            End Sub
        AddHandler RightPlacementComboBox.SelectedIndexChanged,
            Sub()
                Dim direction = DirectCast(RightPlacementComboBox.SelectedItem, Direction)
                RightImageViewer.Placement = direction
                newEffect.PlacementDirectionRight = direction
            End Sub
        AddHandler LeftCenterComboBox.SelectedIndexChanged,
            Sub()
                Dim direction = DirectCast(LeftCenterComboBox.SelectedItem, Direction)
                LeftImageViewer.Centering = direction
                newEffect.CenteringLeft = direction
            End Sub
        AddHandler RightCenterComboBox.SelectedIndexChanged,
            Sub()
                Dim direction = DirectCast(RightCenterComboBox.SelectedItem, Direction)
                RightImageViewer.Centering = direction
                newEffect.CenteringRight = direction
            End Sub
        AddHandler LeftImageFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim filePath = If(LeftImageFileSelector.FilePath = Nothing,
                                                       Nothing, Path.Combine(PonyBasePath, LeftImageFileSelector.FilePath))
                                     newEffect.SetLeftImagePath(filePath)
                                 End Sub)

        Dim updateLeftImage = Sub(sender As Object, e As EventArgs)
                                  Dim behavior = Base.Behaviors.SingleOrDefault(Function(b) b.Name = newEffect.BehaviorName)
                                  Dim behaviorFilePath As String = Nothing
                                  If behavior IsNot Nothing Then behaviorFilePath = behavior.LeftImagePath
                                  LoadNewImageForViewer(LeftImageFileSelector, LeftImageViewer, BehaviorComboBox, behaviorFilePath)
                              End Sub
        AddHandler LeftImageFileSelector.FilePathSelected, updateLeftImage
        AddHandler BehaviorComboBox.SelectedIndexChanged, updateLeftImage

        AddHandler RightImageFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim filePath = If(RightImageFileSelector.FilePath = Nothing,
                                                       Nothing, Path.Combine(PonyBasePath, RightImageFileSelector.FilePath))
                                     newEffect.SetRightImagePath(filePath)
                                 End Sub)

        Dim updateRightImage = Sub(sender As Object, e As EventArgs)
                                   Dim behavior = Base.Behaviors.SingleOrDefault(Function(b) b.Name = newEffect.BehaviorName)
                                   Dim behaviorFilePath As String = Nothing
                                   If behavior IsNot Nothing Then behaviorFilePath = behavior.RightImagePath
                                   LoadNewImageForViewer(RightImageFileSelector, RightImageViewer, BehaviorComboBox, behaviorFilePath)
                               End Sub
        AddHandler RightImageFileSelector.FilePathSelected, updateRightImage
        AddHandler BehaviorComboBox.SelectedIndexChanged, updateRightImage
    End Sub

    Public Overrides ReadOnly Property ItemName As String
        Get
            Return originalEffect.Name
        End Get
    End Property

    Public Overrides Sub NewItem(name As String)
        ' TODO.
    End Sub

    Public Overrides Sub LoadItem(effectName As String)
        originalEffect = Base.Effects.Single(Function(e) e.Name = effectName)
        newEffect = originalEffect.MemberwiseClone()

        LeftImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        RightImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        AddHandler LeftImageFileSelector.ListRefreshed, AddressOf LeftImageFileSelector_ListRefreshed
        AddHandler RightImageFileSelector.ListRefreshed, AddressOf RightImageFileSelector_ListRefreshed
        SelectItemElseAddItem(LeftImageFileSelector.FilePathComboBox, Path.GetFileName(newEffect.LeftImagePath))
        SelectItemElseAddItem(RightImageFileSelector.FilePathComboBox, Path.GetFileName(newEffect.RightImagePath))

        SelectItemElseNoneOption(LeftPlacementComboBox, newEffect.PlacementDirectionLeft)
        SelectItemElseNoneOption(LeftCenterComboBox, newEffect.CenteringLeft)
        SelectItemElseNoneOption(RightPlacementComboBox, newEffect.PlacementDirectionRight)
        SelectItemElseNoneOption(RightCenterComboBox, newEffect.CenteringRight)

        Dim behaviors = Base.Behaviors.ToArray()
        ReplaceItemsInComboBox(BehaviorComboBox, behaviors, True)
        SelectItemElseNoneOption(BehaviorComboBox, behaviors.FirstOrDefault(Function(b) b.Name = newEffect.BehaviorName))

        Source.Text = newEffect.SourceIni
    End Sub

    Private Sub LoadItemCommon()
        NameTextBox.Text = newEffect.Name
        DurationNumber.Value = CDec(newEffect.Duration)
        RepeatDelayNumber.Value = CDec(newEffect.RepeatDelay)
        FollowCheckBox.Checked = newEffect.Follow
        PreventLoopCheckBox.Checked = newEffect.DoNotRepeatImageAnimations
    End Sub

    Public Overrides Sub SaveItem()
        If Base.Effects.Any(Function(e) e.Name = newEffect.Name AndAlso Not Object.ReferenceEquals(e, originalEffect)) Then
            MessageBox.Show(Me, "An effect with the name '" & newEffect.Name &
                            "' already exists for this pony. Please choose another name.",
                            "Name Not Unique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Base.Effects.Remove(originalEffect)
        Base.Effects.Add(newEffect)

        MyBase.SaveItem()

        originalEffect = newEffect
        originalEffect.UpdateSourceIniTo(Source.Text)
        newEffect = originalEffect.MemberwiseClone()
    End Sub

    Protected Overrides Sub OnItemPropertyChanged()
        MyBase.OnItemPropertyChanged()
        Source.Text = newEffect.GetPonyIni()
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
        OnIssuesChanged(Me, EventArgs.Empty)
        newEffect = e
        LoadItemCommon()

        SyncTypedImagePath(LeftImageFileSelector, newEffect.LeftImagePath, AddressOf newEffect.SetLeftImagePath,
                           lastTypedLeftFileName, lastTypedLeftFileNameMissing)
        SyncTypedImagePath(RightImageFileSelector, newEffect.RightImagePath, AddressOf newEffect.SetRightImagePath,
                           lastTypedRightFileName, lastTypedRightFileNameMissing)

        'Dim behaviors = PonyBase.Behaviors.Where(Function(b) Not Object.ReferenceEquals(b, newBehavior)).ToArray()
        'ReplaceItemsInComboBox(LinkedBehaviorComboBox, behaviors, True)
        'SelectItemElseNoneOption(LinkedBehaviorComboBox, newBehavior.LinkedBehavior)
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

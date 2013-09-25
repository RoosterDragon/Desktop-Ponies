Imports System.IO
Imports DesktopSprites.SpriteManagement

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

    Public Sub New()
        InitializeComponent()
        MovementComboBox.Items.AddRange(allowedMovesValues)
        AddHandler NameTextBox.TextChanged, Sub() UpdateProperty(Sub() Edited.Name = NameTextBox.Text)
        AddHandler GroupNumber.ValueChanged, Sub() UpdateProperty(Sub() Edited.Group = CInt(GroupNumber.Value))
        AddHandler ChanceNumber.TextChanged, Sub() UpdateProperty(Sub() Edited.Chance = ChanceNumber.Value / 100)
        AddHandler SpeedNumber.TextChanged, Sub() UpdateProperty(Sub() Edited.Speed = SpeedNumber.Value)
        AddHandler MovementComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub() Edited.AllowedMovement = DirectCast(MovementComboBox.SelectedItem, AllowedMoves))
        AddHandler MinDurationNumber.ValueChanged, Sub() UpdateProperty(Sub() Edited.MinDuration = MinDurationNumber.Value)
        AddHandler MaxDurationNumber.ValueChanged, Sub() UpdateProperty(Sub() Edited.MaxDuration = MaxDurationNumber.Value)
        AddHandler StartSpeechComboBox.ValueChanged,
            Sub() UpdateProperty(Sub()
                                     Edited.StartLineName =
                                         If(StartSpeechComboBox.SelectedIndex <> 0, StartSpeechComboBox.Text, "")
                                 End Sub)
        AddHandler EndSpeechComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub()
                                     Edited.EndLineName =
                                         If(EndSpeechComboBox.SelectedIndex <> 0, EndSpeechComboBox.Text, "")
                                 End Sub)
        AddHandler LinkedBehaviorComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub()
                                     Dim behavior = TryCast(LinkedBehaviorComboBox.SelectedItem, Behavior)
                                     Edited.LinkedBehaviorName = If(behavior Is Nothing, CaseInsensitiveString.Empty, behavior.Name)
                                 End Sub)
        AddHandler LeftImageFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim leftPath = If(LeftImageFileSelector.FilePath = Nothing,
                                                       Nothing, Path.Combine(PonyBasePath, LeftImageFileSelector.FilePath))
                                     Edited.LeftImage.Path = leftPath
                                 End Sub)
        AddHandler LeftImageFileSelector.FilePathSelected, Sub() LoadNewImageForViewer(LeftImageFileSelector, LeftImageViewer)
        AddHandler RightImageFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim rightPath = If(RightImageFileSelector.FilePath = Nothing,
                                                        Nothing, Path.Combine(PonyBasePath, RightImageFileSelector.FilePath))
                                     Edited.RightImage.Path = rightPath
                                 End Sub)
        AddHandler RightImageFileSelector.FilePathSelected, Sub() LoadNewImageForViewer(RightImageFileSelector, RightImageViewer)
    End Sub

    Public Overrides Sub NewItem(name As String)
        ' TODO.
    End Sub

    Public Overrides Sub LoadItem()
        LeftImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        RightImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        AddHandler LeftImageFileSelector.ListRefreshed, AddressOf LeftImageFileSelector_ListRefreshed
        AddHandler RightImageFileSelector.ListRefreshed, AddressOf RightImageFileSelector_ListRefreshed

        Dim speeches = Base.Speeches.Select(Function(s) s.Name).Cast(Of Object).ToArray()
        ReplaceItemsInComboBox(StartSpeechComboBox, speeches, True)
        ReplaceItemsInComboBox(EndSpeechComboBox, speeches, True)

        Dim behaviors = Base.Behaviors.Where(Function(b) Not Object.ReferenceEquals(b, Edited)).ToArray()
        ReplaceItemsInComboBox(LinkedBehaviorComboBox, behaviors, True)
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
        Dim b As Behavior = Nothing
        Behavior.TryLoad(Source.Text, PonyBasePath, Base, b, ParseIssues)
        ReferentialIssues = b.GetReferentialIssues(Base.Collection)
        OnIssuesChanged(EventArgs.Empty)
        Edited = b

        NameTextBox.Text = Edited.Name
        GroupNumber.Value = Edited.Group
        ChanceNumber.Value = CDec(Edited.Chance) * 100
        SpeedNumber.Value = CDec(Edited.Speed)
        MovementComboBox.SelectedItem = Edited.AllowedMovement
        MinDurationNumber.Value = CDec(Edited.MinDuration)
        MaxDurationNumber.Value = CDec(Edited.MaxDuration)

        SelectOrOvertypeItem(StartSpeechComboBox, Edited.StartLineName)
        SelectOrOvertypeItem(EndSpeechComboBox, Edited.EndLineName)
        SelectItemElseNoneOption(LinkedBehaviorComboBox, Edited.LinkedBehavior)

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
        typedPathMissing = Not String.Equals(selector.FilePath, typedPath, PathEquality.Comparison)
        If typedPathMissing Then
            selector.FilePathComboBox.SelectedIndex = selector.FilePathComboBox.Items.Add(typedPath)
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

    Private Sub TargetButton_Click(sender As Object, e As EventArgs) Handles TargetButton.Click
        ' TODO: Make FollowTargetDialog usable from here.
    End Sub
End Class

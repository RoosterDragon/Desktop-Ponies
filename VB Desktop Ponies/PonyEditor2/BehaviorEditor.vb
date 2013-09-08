Imports System.IO
Imports CSDesktopPonies.SpriteManagement

Friend Class BehaviorEditor
    Private Shared allowedMovesValues As Object() =
        [Enum].GetValues(GetType(AllowedMoves)).Cast(Of Object)().ToArray()

    Private originalBehavior As Behavior
    Private newBehavior As Behavior

    Private imageListCrossRefreshNeeded As Boolean

    Public Sub New()
        InitializeComponent()
        MovementComboBox.Items.AddRange(allowedMovesValues)
        AddHandler NameTextBox.TextChanged, Sub() UpdateProperty(Sub() newBehavior.Name = NameTextBox.Text)
        AddHandler GroupNumber.ValueChanged, Sub() UpdateProperty(Sub() newBehavior.Group = CInt(GroupNumber.Value))
        AddHandler ChanceNumber.TextChanged, Sub() UpdateProperty(Sub() newBehavior.ChanceOfOccurence = ChanceNumber.Value / 100)
        AddHandler SpeedNumber.TextChanged, Sub() UpdateProperty(Sub() newBehavior.Speed = SpeedNumber.Value)
        AddHandler MovementComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub() newBehavior.AllowedMovement = DirectCast(MovementComboBox.SelectedItem, AllowedMoves))
        AddHandler MinDurationNumber.ValueChanged, Sub() UpdateProperty(Sub() newBehavior.MinDuration = MinDurationNumber.Value)
        AddHandler MaxDurationNumber.ValueChanged, Sub() UpdateProperty(Sub() newBehavior.MaxDuration = MaxDurationNumber.Value)
        AddHandler StartSpeechComboBox.ValueChanged,
            Sub() UpdateProperty(Sub()
                                     newBehavior.StartLineName =
                                         If(StartSpeechComboBox.SelectedIndex <> 0, StartSpeechComboBox.Text, Nothing)
                                 End Sub)
        AddHandler EndSpeechComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub()
                                     newBehavior.EndLineName =
                                         If(EndSpeechComboBox.SelectedIndex <> 0, EndSpeechComboBox.Text, "")
                                 End Sub)
        AddHandler LinkedBehaviorComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub()
                                     Dim behavior = TryCast(LinkedBehaviorComboBox.SelectedItem, Behavior)
                                     newBehavior.LinkedBehaviorName = If(behavior Is Nothing, "", behavior.Name)
                                 End Sub)
        AddHandler LeftImageFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim leftPath = If(LeftImageFileSelector.FilePath = Nothing,
                                                       Nothing, Path.Combine(PonyBasePath, LeftImageFileSelector.FilePath))
                                     newBehavior.SetLeftImagePath(leftPath)
                                 End Sub)
        AddHandler LeftImageFileSelector.FilePathSelected, Sub() LoadNewImageForViewer(LeftImageFileSelector, LeftImageViewer)
        AddHandler RightImageFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim rightPath = If(RightImageFileSelector.FilePath = Nothing,
                                                        Nothing, Path.Combine(PonyBasePath, RightImageFileSelector.FilePath))
                                     newBehavior.SetRightImagePath(rightPath)
                                 End Sub)
        AddHandler RightImageFileSelector.FilePathSelected, Sub() LoadNewImageForViewer(RightImageFileSelector, RightImageViewer)
    End Sub

    Public Overrides ReadOnly Property ItemName As String
        Get
            Return originalBehavior.Name
        End Get
    End Property

    Public Overrides Sub NewItem(name As String)
        ' TODO.
    End Sub

    Public Overrides Sub LoadItem(behaviorName As String)
        originalBehavior = Base.Behaviors.Single(Function(b) b.Name = behaviorName)
        newBehavior = originalBehavior.MemberwiseClone()

        LeftImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        RightImageFileSelector.InitializeFromDirectory(PonyBasePath, "*.gif", "*.png")
        AddHandler LeftImageFileSelector.ListRefreshed, AddressOf LeftImageFileSelector_ListRefreshed
        AddHandler RightImageFileSelector.ListRefreshed, AddressOf RightImageFileSelector_ListRefreshed
        SelectItemElseAddItem(LeftImageFileSelector.FilePathComboBox, Path.GetFileName(newBehavior.LeftImagePath))
        SelectItemElseAddItem(RightImageFileSelector.FilePathComboBox, Path.GetFileName(newBehavior.RightImagePath))

        Dim speeches = Base.SpeakingLines.Select(Function(s) s.Name).ToArray()
        ReplaceItemsInComboBox(StartSpeechComboBox, speeches, True)
        ReplaceItemsInComboBox(EndSpeechComboBox, speeches, True)

        Dim behaviors = Base.Behaviors.Where(Function(b) Not Object.ReferenceEquals(b, newBehavior)).ToArray()
        ReplaceItemsInComboBox(LinkedBehaviorComboBox, behaviors, True)

        LoadItemCommon()

        Source.Text = newBehavior.GetPonyIni()
    End Sub

    Private Sub LoadItemCommon()
        NameTextBox.Text = newBehavior.Name
        GroupNumber.Value = newBehavior.Group
        ChanceNumber.Value = CDec(newBehavior.ChanceOfOccurence) * 100
        SpeedNumber.Value = CDec(newBehavior.Speed)
        MovementComboBox.SelectedItem = newBehavior.AllowedMovement
        MinDurationNumber.Value = CDec(newBehavior.MinDuration)
        MaxDurationNumber.Value = CDec(newBehavior.MaxDuration)

        SelectOrOvertypeItem(StartSpeechComboBox, newBehavior.StartLineName)
        SelectOrOvertypeItem(EndSpeechComboBox, newBehavior.EndLineName)
        SelectItemElseNoneOption(LinkedBehaviorComboBox, newBehavior.LinkedBehavior)
    End Sub

    Public Overrides Sub SaveItem()
        If Base.Behaviors.Any(Function(b) b.Name = newBehavior.Name AndAlso Not Object.ReferenceEquals(b, originalBehavior)) Then
            MessageBox.Show(Me, "A behavior with the name '" & newBehavior.Name &
                            "' already exists for this pony. Please choose another name.",
                            "Name Not Unique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        If originalBehavior.Name <> newBehavior.Name Then
            If MessageBox.Show(Me, "Changing the name of this behavior will break other references. Continue with save?",
                               "Rename Behavior?", MessageBoxButtons.YesNo,
                               MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                Return
            End If
        End If

        Dim index = Base.Behaviors.IndexOf(originalBehavior)
        Base.Behaviors(index) = newBehavior

        MyBase.SaveItem()

        originalBehavior = newBehavior
        newBehavior = originalBehavior.MemberwiseClone()
    End Sub

    Protected Overrides Sub OnItemPropertyChanged()
        MyBase.OnItemPropertyChanged()
        Source.Text = newBehavior.GetPonyIni()
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
        OnIssuesChanged(Me, EventArgs.Empty)
        newBehavior = b
        LoadItemCommon()

        SyncTypedImagePath(LeftImageFileSelector, newBehavior.LeftImagePath, AddressOf newBehavior.SetLeftImagePath,
                           lastTypedLeftFileName, lastTypedLeftFileNameMissing)
        SyncTypedImagePath(RightImageFileSelector, newBehavior.RightImagePath, AddressOf newBehavior.SetRightImagePath,
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

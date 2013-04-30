Imports System.IO
Imports CSDesktopPonies.SpriteManagement

Friend Class BehaviorEditor
    Private Shared allowedMovesValues As Object() =
        [Enum].GetValues(GetType(AllowedMoves)).Cast(Of Object)().ToArray()

    Private originalBehavior As Behavior
    Private newBehavior As Behavior

    Public Sub New()
        InitializeComponent()
        MovementComboBox.Items.AddRange(allowedMovesValues)
        AddHandler NameTextBox.TextChanged, Sub() UpdateProperty(Sub() newBehavior.Name = NameTextBox.Text)
        AddHandler GroupNumber.ValueChanged, Sub() UpdateProperty(Sub() newBehavior.Group = CInt(GroupNumber.Value))
        AddHandler ChanceNumber.TextChanged, Sub() UpdateProperty(Sub() newBehavior.ChanceOfOccurence = ChanceNumber.Value)
        AddHandler SpeedNumber.TextChanged, Sub() UpdateProperty(Sub() newBehavior.SetSpeed(SpeedNumber.Value))
        AddHandler MovementComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub() newBehavior.AllowedMovement = DirectCast(MovementComboBox.SelectedItem, AllowedMoves))
        AddHandler MinDurationNumber.ValueChanged, Sub() UpdateProperty(Sub() newBehavior.MinDuration = MinDurationNumber.Value)
        AddHandler MaxDurationNumber.ValueChanged, Sub() UpdateProperty(Sub() newBehavior.MaxDuration = MaxDurationNumber.Value)
        AddHandler StartSpeechComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub()
                                     Dim startLine = TryCast(StartSpeechComboBox.SelectedItem, Behavior.SpeakingLine)
                                     newBehavior.StartLine = startLine
                                 End Sub)
        AddHandler EndSpeechComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub()
                                     Dim endLine = TryCast(EndSpeechComboBox.SelectedItem, Behavior.SpeakingLine)
                                     newBehavior.EndLine = endLine
                                 End Sub)
        AddHandler LinkedBehaviorComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub()
                                     Dim linkedBehavior = TryCast(LinkedBehaviorComboBox.SelectedItem, Behavior)
                                     newBehavior.LinkedBehavior = linkedBehavior
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

    Public Overrides Sub LoadItem(ponyBase As PonyBase, behaviorName As String)
        LoadingItem = True
        MyBase.LoadItem(ponyBase, behaviorName)

        originalBehavior = ponyBase.Behaviors.Single(Function(b) b.Name = behaviorName)
        newBehavior = originalBehavior.MemberwiseClone()
        NameTextBox.Text = newBehavior.Name
        GroupNumber.Value = newBehavior.Group
        ChanceNumber.Value = CDec(newBehavior.ChanceOfOccurence) * 100
        SpeedNumber.Value = CDec(newBehavior.Speed)
        MovementComboBox.SelectedItem = newBehavior.AllowedMovement
        MinDurationNumber.Value = CDec(newBehavior.MinDuration)
        MaxDurationNumber.Value = CDec(newBehavior.MaxDuration)

        Dim images =
            Directory.GetFiles(PonyBasePath, "*.gif").Concat(Directory.GetFiles(PonyBasePath, "*.png")).
            Select(Function(filePath) Path.GetFileName(filePath)).ToArray()
        ReplaceItemsInComboBox(LeftImageFileSelector.FilePathComboBox, images, True)
        ReplaceItemsInComboBox(RightImageFileSelector.FilePathComboBox, images, True)
        SelectItemElseAddItem(LeftImageFileSelector.FilePathComboBox, Path.GetFileName(newBehavior.LeftImagePath))
        SelectItemElseAddItem(RightImageFileSelector.FilePathComboBox, Path.GetFileName(newBehavior.RightImagePath))

        Dim speeches = ponyBase.SpeakingLines.ToArray()
        ReplaceItemsInComboBox(StartSpeechComboBox, speeches, True)
        ReplaceItemsInComboBox(EndSpeechComboBox, speeches, True)
        SelectItemElseNoneOption(StartSpeechComboBox, newBehavior.StartLine)
        SelectItemElseNoneOption(EndSpeechComboBox, newBehavior.EndLine)

        Dim behaviors = ponyBase.Behaviors.Where(Function(b) Not Object.ReferenceEquals(b, newBehavior)).ToArray()
        ReplaceItemsInComboBox(LinkedBehaviorComboBox, behaviors, True)
        SelectItemElseNoneOption(LinkedBehaviorComboBox, newBehavior.LinkedBehavior)

        Source.Text = newBehavior.GetPonyIni()

        LoadingItem = False
    End Sub

    Public Overrides Sub SaveItem()
        If IsNewItem AndAlso Base.Behaviors.Any(Function(b) b.Name = newBehavior.Name) Then
            MessageBox.Show(Me, "A behavior with the name '" & newBehavior.Name &
                            "' already exists for this pony. Please choose another name.",
                            "Name Not Unique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Base.Behaviors.Remove(originalBehavior)
        Base.Behaviors.Add(newBehavior)

        MyBase.SaveItem()
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
End Class

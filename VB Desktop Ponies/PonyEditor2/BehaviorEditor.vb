Imports System.IO
Imports CSDesktopPonies.SpriteManagement

Friend Class BehaviorEditor
    Private Shared allowedMovesValues As Object() =
        [Enum].GetValues(GetType(AllowedMoves)).Cast(Of Object)().ToArray()

    Private loadingItem As Boolean
    Private behavior As Behavior

    Public Sub New()
        InitializeComponent()
        MovementComboBox.Items.AddRange(allowedMovesValues)
    End Sub

    Public Overrides Sub LoadItem(ponyBase As PonyBase, behaviorName As String)
        MyBase.LoadItem(ponyBase, behaviorName)

        loadingItem = True
        behavior = ponyBase.Behaviors.Single(Function(b) b.Name = behaviorName)
        NameTextBox.Text = behavior.Name
        GroupNumber.Value = behavior.Group
        ChanceNumber.Value = CDec(behavior.ChanceOfOccurence) * 100
        SpeedNumber.Value = CDec(behavior.Speed)
        MovementComboBox.SelectedItem = behavior.AllowedMovement
        MinDurationNumber.Value = CDec(behavior.MinDuration)
        MaxDurationNumber.Value = CDec(behavior.MaxDuration)

        Dim images =
            Directory.GetFiles(PonyBasePath, "*.gif").Concat(Directory.GetFiles(PonyBasePath, "*.png")).
            Select(Function(filePath) Path.GetFileName(filePath)).ToArray()
        ReplaceItemsInComboBox(LeftImageFileSelector.FilePathComboBox, images, True)
        ReplaceItemsInComboBox(RightImageFileSelector.FilePathComboBox, images, True)
        SelectItemElseNoneOption(LeftImageFileSelector.FilePathComboBox, Path.GetFileName(behavior.LeftImagePath))
        SelectItemElseNoneOption(RightImageFileSelector.FilePathComboBox, Path.GetFileName(behavior.RightImagePath))

        Dim speeches = ponyBase.SpeakingLines.ToArray()
        ReplaceItemsInComboBox(StartSpeechComboBox, speeches, True)
        ReplaceItemsInComboBox(EndSpeechComboBox, speeches, True)
        SelectItemElseNoneOption(StartSpeechComboBox, behavior.StartLine)
        SelectItemElseNoneOption(EndSpeechComboBox, behavior.EndLine)

        Dim behaviors = ponyBase.Behaviors.Where(Function(b) Not Object.ReferenceEquals(b, behavior)).ToArray()
        ReplaceItemsInComboBox(LinkedBehaviorComboBox, behaviors, True)
        SelectItemElseNoneOption(LinkedBehaviorComboBox, behavior.LinkedBehavior)

        Source.Text = behavior.GetPonyIni()

        loadingItem = False
    End Sub

    Public Overrides Sub SaveItem()
        If IsNewItem Then
            If Linq.Enumerable.Count(Base.Behaviors, Function(b) b.Name = behavior.Name) > 0 Then
                MessageBox.Show(Me, "A behavior with the name '" & behavior.Name &
                                "' already exists for this pony. Please choose another name.",
                                "Name Not Unique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return
            End If
        End If

        behavior.Name = NameTextBox.Text
        behavior.Group = CInt(GroupNumber.Value)
        behavior.ChanceOfOccurence = ChanceNumber.Value
        behavior.SetSpeed(SpeedNumber.Value)
        behavior.AllowedMovement = DirectCast(MovementComboBox.SelectedItem, AllowedMoves)
        behavior.MinDuration = MinDurationNumber.Value
        behavior.MaxDuration = MaxDurationNumber.Value

        Dim startLine = TryCast(StartSpeechComboBox.SelectedItem, Behavior.SpeakingLine)
        behavior.StartLine = startLine
        Dim endLine = TryCast(EndSpeechComboBox.SelectedItem, Behavior.SpeakingLine)
        behavior.EndLine = endLine
        Dim linkedBehavior = TryCast(LinkedBehaviorComboBox.SelectedItem, Behavior)
        behavior.LinkedBehavior = linkedBehavior
        Dim leftPath = If(LeftImageFileSelector.FilePath = Nothing, Nothing, Path.Combine(PonyBasePath, LeftImageFileSelector.FilePath))
        behavior.SetLeftImagePath(leftPath)
        Dim rightPath = If(RightImageFileSelector.FilePath = Nothing, Nothing, Path.Combine(PonyBasePath, RightImageFileSelector.FilePath))
        behavior.SetRightImagePath(rightPath)

        MyBase.SaveItem()
    End Sub

    Protected Overrides Sub Property_ValueChanged(sender As Object, e As EventArgs)
        If loadingItem Then Return
        MyBase.Property_ValueChanged(sender, e)
        Source.Text = behavior.GetPonyIni()
    End Sub

    Private Sub LeftImageFileSelector_FilePathSelected(sender As Object, e As EventArgs) Handles LeftImageFileSelector.FilePathSelected
        LoadNewImageForViewer(LeftImageFileSelector, LeftImageViewer)
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub RightImageFileSelector_FilePathSelected(sender As Object, e As EventArgs) Handles RightImageFileSelector.FilePathSelected
        LoadNewImageForViewer(RightImageFileSelector, RightImageViewer)
        Property_ValueChanged(sender, e)
    End Sub
End Class

Imports System.IO
Imports CSDesktopPonies.SpriteManagement

Friend Class BehaviorEditor
    Private Shared allowedMovesValues As Object() =
        [Enum].GetValues(GetType(AllowedMoves)).Cast(Of Object)().ToArray()

    Private behavior As Behavior
    Private loadingBehavior As Boolean

    Public Sub New()
        InitializeComponent()
        MovementComboBox.Items.AddRange(allowedMovesValues)
    End Sub

    Public Overrides Sub LoadItem(ponyBase As PonyBase, behaviorName As String)
        MyBase.LoadItem(ponyBase, behaviorName)
        loadingBehavior = True

        behavior = ponyBase.Behaviors.Single(Function(b) b.Name = behaviorName)
        NameTextBox.Text = behavior.Name
        GroupNumber.Value = behavior.Group
        ChanceNumber.Value = CDec(behavior.ChanceOfOccurance) * 100
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

        loadingBehavior = False
    End Sub

    Protected Overrides Sub Property_ValueChanged(sender As Object, e As EventArgs)
        MyBase.Property_ValueChanged(sender, e)
        Source.Text = behavior.GetPonyIni()
    End Sub

    Private Sub NameTextBox_TextChanged(sender As Object, e As EventArgs) Handles NameTextBox.TextChanged
        If loadingBehavior Then Return
        ' TODO: Handle name changes.
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub GroupNumber_ValueChanged(sender As Object, e As EventArgs) Handles GroupNumber.ValueChanged
        If loadingBehavior Then Return
        behavior.Group = CInt(GroupNumber.Value)
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub ChanceNumber_ValueChanged(sender As Object, e As EventArgs) Handles ChanceNumber.ValueChanged
        If loadingBehavior Then Return
        behavior.ChanceOfOccurance = ChanceNumber.Value
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub SpeedNumber_ValueChanged(sender As Object, e As EventArgs) Handles SpeedNumber.ValueChanged
        If loadingBehavior Then Return
        behavior.SetSpeed(SpeedNumber.Value)
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub MovementComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles MovementComboBox.SelectedIndexChanged
        If loadingBehavior Then Return
        behavior.AllowedMovement = DirectCast(MovementComboBox.SelectedItem, AllowedMoves)
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub MinDurationNumber_ValueChanged(sender As Object, e As EventArgs) Handles MinDurationNumber.ValueChanged
        If loadingBehavior Then Return
        behavior.MinDuration = MinDurationNumber.Value
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub MaxDurationNumber_ValueChanged(sender As Object, e As EventArgs) Handles MaxDurationNumber.ValueChanged
        If loadingBehavior Then Return
        behavior.MaxDuration = MaxDurationNumber.Value
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub StartSpeechComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles StartSpeechComboBox.SelectedIndexChanged
        If loadingBehavior Then Return
        Dim line = TryCast(StartSpeechComboBox.SelectedItem, Behavior.SpeakingLine)
        behavior.StartLine = line
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub EndSpeechComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles EndSpeechComboBox.SelectedIndexChanged
        If loadingBehavior Then Return
        Dim line = TryCast(EndSpeechComboBox.SelectedItem, Behavior.SpeakingLine)
        behavior.EndLine = line
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub LinkedBehaviorComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LinkedBehaviorComboBox.SelectedIndexChanged
        If loadingBehavior Then Return
        Dim linkedBehavior = TryCast(LinkedBehaviorComboBox.SelectedItem, Behavior)
        behavior.LinkedBehavior = linkedBehavior
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub LeftImageFileSelector_FilePathSelected(sender As Object, e As EventArgs) Handles LeftImageFileSelector.FilePathSelected
        LoadNewImageForViewer(LeftImageFileSelector, LeftImageViewer)
        If loadingBehavior Then Return
        Dim filePath = If(LeftImageFileSelector.FilePath = Nothing, Nothing, Path.Combine(ponyBasePath, LeftImageFileSelector.FilePath))
        behavior.SetLeftImagePath(filePath)
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub RightImageFileSelector_FilePathSelected(sender As Object, e As EventArgs) Handles RightImageFileSelector.FilePathSelected
        LoadNewImageForViewer(RightImageFileSelector, RightImageViewer)
        If loadingBehavior Then Return
        Dim filePath = If(RightImageFileSelector.FilePath = Nothing, Nothing, Path.Combine(ponyBasePath, RightImageFileSelector.FilePath))
        behavior.SetRightImagePath(filePath)
        Property_ValueChanged(sender, e)
    End Sub
End Class

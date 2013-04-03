Imports System.IO
Imports CSDesktopPonies.SpriteManagement

Friend Class EffectEditor
    Private Shared directionValues As Object() =
        [Enum].GetValues(GetType(Direction)).Cast(Of Object)().ToArray()

    Private effect As EffectBase
    Private loadingEffect As Boolean

    Private ponyBaseRef As PonyBase

    Public Sub New()
        InitializeComponent()
        LeftPlacementComboBox.Items.AddRange(directionValues)
        LeftCenterComboBox.Items.AddRange(directionValues)
        RightPlacementComboBox.Items.AddRange(directionValues)
        RightCenterComboBox.Items.AddRange(directionValues)
    End Sub

    Public Overrides Sub LoadItem(ponyBase As PonyBase, effectName As String)
        MyBase.LoadItem(ponyBase, effectName)
        loadingEffect = True

        ponyBaseRef = ponyBase

        effect = ponyBase.Effects.Single(Function(e) e.Name = effectName)
        NameTextBox.Text = effect.Name
        DurationNumber.Value = CDec(effect.Duration)

        Dim images =
            Directory.GetFiles(PonyBasePath, "*.gif").Concat(Directory.GetFiles(PonyBasePath, "*.png")).
            Select(Function(filePath) Path.GetFileName(filePath)).ToArray()
        ReplaceItemsInComboBox(LeftImageFileSelector.FilePathComboBox, images, True)
        ReplaceItemsInComboBox(RightImageFileSelector.FilePathComboBox, images, True)
        SelectItemElseNoneOption(LeftImageFileSelector.FilePathComboBox, Path.GetFileName(effect.LeftImagePath))
        SelectItemElseNoneOption(RightImageFileSelector.FilePathComboBox, Path.GetFileName(effect.RightImagePath))

        SelectItemElseNoneOption(LeftPlacementComboBox, effect.PlacementDirectionLeft)
        SelectItemElseNoneOption(LeftCenterComboBox, effect.CenteringLeft)
        SelectItemElseNoneOption(RightPlacementComboBox, effect.PlacementDirectionRight)
        SelectItemElseNoneOption(RightCenterComboBox, effect.CenteringRight)

        Source.Text = effect.GetPonyIni()

        loadingEffect = False
    End Sub

    Protected Overrides Sub Property_ValueChanged(sender As Object, e As EventArgs)
        MyBase.Property_ValueChanged(sender, e)
        Source.Text = effect.GetPonyIni()
    End Sub

    Private Sub NameTextBox_TextChanged(sender As Object, e As EventArgs) Handles NameTextBox.TextChanged
        If loadingEffect Then Return
        ' TODO: Handle name changes.
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub DurationNumber_ValueChanged(sender As Object, e As EventArgs) Handles DurationNumber.ValueChanged
        If loadingEffect Then Return
        effect.Duration = DurationNumber.Value
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub RepeatDelayNumber_ValueChanged(sender As Object, e As EventArgs) Handles RepeatDelayNumber.ValueChanged
        If loadingEffect Then Return
        effect.Repeat_Delay = RepeatDelayNumber.Value
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub LeftPlacementComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LeftPlacementComboBox.SelectedIndexChanged
        Dim direction = CType(LeftPlacementComboBox.SelectedItem, Direction)
        LeftImageViewer.Placement = direction
        If loadingEffect Then Return
        effect.PlacementDirectionLeft = direction
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub LeftCenterComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LeftCenterComboBox.SelectedIndexChanged
        Dim direction = CType(LeftCenterComboBox.SelectedItem, Direction)
        LeftImageViewer.Centering = direction
        If loadingEffect Then Return
        effect.CenteringLeft = direction
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub RightPlacementComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RightPlacementComboBox.SelectedIndexChanged
        Dim direction = CType(RightPlacementComboBox.SelectedItem, Direction)
        RightImageViewer.Placement = direction
        If loadingEffect Then Return
        effect.PlacementDirectionRight = direction
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub RightCenterComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RightCenterComboBox.SelectedIndexChanged
        Dim direction = CType(RightCenterComboBox.SelectedItem, Direction)
        RightImageViewer.Centering = direction
        If loadingEffect Then Return
        effect.CenteringRight = direction
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub LeftImageFileSelector_FilePathSelected(sender As Object, e As EventArgs) Handles LeftImageFileSelector.FilePathSelected
        Dim behavior = ponyBaseRef.Behaviors.SingleOrDefault(Function(b) b.Name = effect.BehaviorName)
        Dim behaviorFilePath As String = Nothing
        If behavior IsNot Nothing Then
            behaviorFilePath = behavior.LeftImagePath
        End If
        LoadNewImageForViewer(LeftImageFileSelector, LeftImageViewer, behaviorFilePath)
        If loadingEffect Then Return
        Dim filePath = If(LeftImageFileSelector.FilePath = Nothing, Nothing, Path.Combine(ponyBasePath, LeftImageFileSelector.FilePath))
        effect.SetLeftImagePath(filePath)
        Property_ValueChanged(sender, e)
    End Sub

    Private Sub RightImageFileSelector_FilePathSelected(sender As Object, e As EventArgs) Handles RightImageFileSelector.FilePathSelected
        Dim behavior = ponyBaseRef.Behaviors.SingleOrDefault(Function(b) b.Name = effect.BehaviorName)
        Dim behaviorFilePath As String = Nothing
        If behavior IsNot Nothing Then
            behaviorFilePath = behavior.RightImagePath
        End If
        LoadNewImageForViewer(RightImageFileSelector, RightImageViewer, behaviorFilePath)
        If loadingEffect Then Return
        Dim filePath = If(RightImageFileSelector.FilePath = Nothing, Nothing, Path.Combine(ponyBasePath, RightImageFileSelector.FilePath))
        effect.SetRightImagePath(filePath)
        Property_ValueChanged(sender, e)
    End Sub
End Class

Imports System.IO
Imports CSDesktopPonies.SpriteManagement

Friend Class EffectEditor
    Private Shared directionValues As Object() =
        [Enum].GetValues(GetType(Direction)).Cast(Of Object)().ToArray()

    Private originalEffect As EffectBase
    Private newEffect As EffectBase

    Public Sub New()
        InitializeComponent()
        LeftPlacementComboBox.Items.AddRange(directionValues)
        LeftCenterComboBox.Items.AddRange(directionValues)
        RightPlacementComboBox.Items.AddRange(directionValues)
        RightCenterComboBox.Items.AddRange(directionValues)
        AddHandler NameTextBox.TextChanged, Sub() UpdateProperty(Sub() newEffect.Name = NameTextBox.Text)
        AddHandler DurationNumber.ValueChanged, Sub() UpdateProperty(Sub() newEffect.Duration = DurationNumber.Value)
        AddHandler RepeatDelayNumber.ValueChanged, Sub() UpdateProperty(Sub() newEffect.RepeatDelay = RepeatDelayNumber.Value)
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
        AddHandler LeftImageFileSelector.FilePathSelected,
            Sub()
                Dim behavior = Base.Behaviors.SingleOrDefault(Function(b) b.Name = newEffect.BehaviorName)
                Dim behaviorFilePath As String = Nothing
                If behavior IsNot Nothing Then behaviorFilePath = behavior.LeftImagePath
                LoadNewImageForViewer(LeftImageFileSelector, LeftImageViewer, behaviorFilePath)
            End Sub
        AddHandler RightImageFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim filePath = If(RightImageFileSelector.FilePath = Nothing,
                                                       Nothing, Path.Combine(PonyBasePath, RightImageFileSelector.FilePath))
                                     newEffect.SetRightImagePath(filePath)
                                 End Sub)
        AddHandler RightImageFileSelector.FilePathSelected,
            Sub()
                Dim behavior = Base.Behaviors.SingleOrDefault(Function(b) b.Name = newEffect.BehaviorName)
                Dim behaviorFilePath As String = Nothing
                If behavior IsNot Nothing Then behaviorFilePath = behavior.RightImagePath
                LoadNewImageForViewer(RightImageFileSelector, RightImageViewer, behaviorFilePath)
            End Sub
    End Sub

    Public Overrides Sub LoadItem(ponyBase As PonyBase, effectName As String)
        LoadingItem = True
        MyBase.LoadItem(ponyBase, effectName)

        originalEffect = ponyBase.Effects.Single(Function(e) e.Name = effectName)
        newEffect = originalEffect.MemberwiseClone()
        NameTextBox.Text = newEffect.Name
        DurationNumber.Value = CDec(newEffect.Duration)
        RepeatDelayNumber.Value = CDec(newEffect.RepeatDelay)

        Dim images =
            Directory.GetFiles(PonyBasePath, "*.gif").Concat(Directory.GetFiles(PonyBasePath, "*.png")).
            Select(Function(filePath) Path.GetFileName(filePath)).ToArray()
        ReplaceItemsInComboBox(LeftImageFileSelector.FilePathComboBox, images, True)
        ReplaceItemsInComboBox(RightImageFileSelector.FilePathComboBox, images, True)
        SelectItemElseAddItem(LeftImageFileSelector.FilePathComboBox, Path.GetFileName(newEffect.LeftImagePath))
        SelectItemElseAddItem(RightImageFileSelector.FilePathComboBox, Path.GetFileName(newEffect.RightImagePath))

        SelectItemElseNoneOption(LeftPlacementComboBox, newEffect.PlacementDirectionLeft)
        SelectItemElseNoneOption(LeftCenterComboBox, newEffect.CenteringLeft)
        SelectItemElseNoneOption(RightPlacementComboBox, newEffect.PlacementDirectionRight)
        SelectItemElseNoneOption(RightCenterComboBox, newEffect.CenteringRight)

        Source.Text = newEffect.GetPonyIni()

        LoadingItem = False
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
End Class

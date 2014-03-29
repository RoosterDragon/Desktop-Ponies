Public Class NewBehaviorDialog

    Private Const FollowModeNoneText = "[None]"
    Private ReadOnly editor As PonyEditor
    Private ReadOnly newBehavior As Behavior
    Private leftImagePath As String
    Private rightImagePath As String

    Public Sub New(editor As PonyEditor)
        Me.editor = Argument.EnsureNotNull(editor, "editor")
        InitializeComponent()
        Icon = My.Resources.Twilight

        For Each item In editor.colBehaviorStartSpeech.Items
            StartSpeechComboBox.Items.Add(item)
            EndSpeechComboBox.Items.Add(item)
        Next
        StartSpeechComboBox.SelectedIndex = 0
        EndSpeechComboBox.SelectedIndex = 0
        For Each item In editor.colBehaviorMovement.Items
            MovementComboBox.Items.Add(item)
        Next
        MovementComboBox.SelectedIndex = 0
        For Each item In editor.colBehaviorLinked.Items
            LinkComboBox.Items.Add(item)
        Next
        LinkComboBox.SelectedIndex = 0
        FollowTextBox.Text = FollowModeNoneText

        newBehavior = New Behavior(editor.CurrentBase)
    End Sub

    Private Sub LeftImageSelectButton_Click(sender As Object, e As EventArgs) Handles LeftImageSelectButton.Click
        Dim newPath = EditorCommon.SetImage(editor, Me, LeftImageBox, AddressOf UseRuntimeDuration)
        If newPath IsNot Nothing Then leftImagePath = newPath
    End Sub

    Private Sub RightImageSelectButton_Click(sender As Object, e As EventArgs) Handles RightImageSelectButton.Click
        Dim newPath = EditorCommon.SetImage(editor, Me, RightImageBox, AddressOf UseRuntimeDuration)
        If newPath IsNot Nothing Then rightImagePath = newPath
    End Sub

    Private Sub UseRuntimeDuration(duration As Decimal)
        If MessageBox.Show(Me, "Use the duration of this image as the duration for the behavior?",
                           "Duration", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                           MessageBoxDefaultButton.Button1) <> DialogResult.Yes Then Return
        If duration <= MinDurationNumber.Maximum Then MinDurationNumber.Value = duration
        If duration <= MaxDurationNumber.Maximum Then MaxDurationNumber.Value = duration
    End Sub

    Private Sub FollowSelectButton_Click(sender As Object, e As EventArgs) Handles FollowSelectButton.Click
        Using dialog = New FollowTargetDialog(newBehavior)
            If dialog.ShowDialog(Me) = DialogResult.OK Then
                Dim text As String = Nothing
                Select Case newBehavior.TargetMode
                    Case TargetMode.Pony
                        text = newBehavior.OriginalFollowTargetName
                    Case TargetMode.Point
                        text = New Vector2(newBehavior.OriginalDestinationXCoord, newBehavior.OriginalDestinationYCoord).ToString()
                    Case TargetMode.None
                        text = FollowModeNoneText
                End Select
                FollowTextBox.Text = text
            End If
        End Using
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Dim newName = NameTextBox.Text.Trim()
        If Not EditorCommon.ValidateName(Me, "behavior", newName, editor.CurrentBase.Behaviors) Then Return

        If String.IsNullOrEmpty(leftImagePath) OrElse String.IsNullOrEmpty(rightImagePath) Then
            MessageBox.Show(Me,
                            "You still need to select the two images to use for this behavior, for both the left and right directions.",
                            "Images Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim minDuration = MinDurationNumber.Value
        Dim maxDuration = MaxDurationNumber.Value
        If minDuration > maxDuration Then
            MessageBox.Show(Me, "The maximum duration needs to be the same as, or larger than, the minimum duration.",
                            "Invalid Durations Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim startLine = CaseInsensitiveString.Empty
        If StartSpeechComboBox.SelectedIndex > 0 Then
            startLine = DirectCast(StartSpeechComboBox.SelectedItem, CaseInsensitiveString)
        End If

        Dim endLine = CaseInsensitiveString.Empty
        If EndSpeechComboBox.SelectedIndex > 0 Then
            endLine = DirectCast(EndSpeechComboBox.SelectedItem, CaseInsensitiveString)
        End If

        Dim linkedBehavior = CaseInsensitiveString.Empty
        If LinkComboBox.SelectedIndex > 0 Then
            linkedBehavior = DirectCast(LinkComboBox.SelectedItem, CaseInsensitiveString)
        End If

        newBehavior.Name = newName
        newBehavior.Chance = ChanceNumber.Value / 100
        newBehavior.MinDuration = minDuration
        newBehavior.MaxDuration = maxDuration
        newBehavior.Speed = SpeedNumber.Value
        newBehavior.LeftImage.Path = leftImagePath
        newBehavior.RightImage.Path = rightImagePath
        newBehavior.AllowedMovement = AllowedMovesFromDisplayString(DirectCast(MovementComboBox.SelectedItem, String))
        newBehavior.LinkedBehaviorName = linkedBehavior
        newBehavior.StartLineName = startLine
        newBehavior.EndLineName = endLine
        newBehavior.Skip = DoNotRunRandomlyCheckBox.Checked
        newBehavior.DoNotRepeatImageAnimations = DoNotRepeatAnimationsCheckBox.Checked
        newBehavior.Group = CInt(GroupNumber.Value)

        editor.CurrentBase.Behaviors.Add(newBehavior)

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class

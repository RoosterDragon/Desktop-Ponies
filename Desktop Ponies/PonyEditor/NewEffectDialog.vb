Public Class NewEffectDialog

    Private ReadOnly editor As PonyEditor
    Private leftImagePath As String
    Private rightImagePath As String

    Public Sub New(editor As PonyEditor)
        Me.editor = Argument.EnsureNotNull(editor, "editor")
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Private Sub NewEffectDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        For Each item In editor.colBehaviorLinked.Items
            BehaviorCombo.Items.Add(item)
        Next

        BehaviorCombo.Items.Remove(New CaseInsensitiveString(PonyEditor.NoneText))

        For Each value In DirectCast([Enum].GetValues(GetType(Direction)), Direction())
            LeftPlacementCombo.Items.Add(value.ToDisplayString())
            RightPlacementCombo.Items.Add(value.ToDisplayString())
            LeftCenteringCombo.Items.Add(value.ToDisplayString())
            RightCenteringCombo.Items.Add(value.ToDisplayString())
        Next

        Dim middleCenterDisplayString = Direction.MiddleCenter.ToDisplayString()
        RightPlacementCombo.SelectedItem = middleCenterDisplayString
        LeftPlacementCombo.SelectedItem = middleCenterDisplayString
        RightCenteringCombo.SelectedItem = middleCenterDisplayString
        LeftCenteringCombo.SelectedItem = middleCenterDisplayString
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
        If MessageBox.Show(Me, "Use the duration of this image as the duration for the effect?",
                           "Duration", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                           MessageBoxDefaultButton.Button1) <> DialogResult.Yes Then Return
        DurationNumber.Value = duration
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        If BehaviorCombo.SelectedIndex = -1 Then
            MessageBox.Show(Me, "You need to select a behavior to trigger this effect.",
                            "No Behavior Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim parentBehavior = editor.CurrentBase.Behaviors.First(
            Function(b) b.Name = DirectCast(BehaviorCombo.SelectedItem, CaseInsensitiveString))

        Dim newName = NameTextBox.Text.Trim()
        If Not EditorCommon.ValidateName(Me, "effect", newName, parentBehavior.Effects) Then Return

        If String.IsNullOrEmpty(leftImagePath) OrElse String.IsNullOrEmpty(rightImagePath) Then
            MessageBox.Show(Me, "You need to select two pictures - one left, one right.",
                            "Pictures Missing", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim newEffect As New EffectBase(newName, leftImagePath, rightImagePath) With {
            .BehaviorName = parentBehavior.Name,
            .Duration = TimeSpan.FromSeconds(DurationNumber.Value),
            .RepeatDelay = TimeSpan.FromSeconds(RepeatDelayNumber.Value),
            .PlacementDirectionLeft = DirectionFromDisplayString(DirectCast(LeftPlacementCombo.SelectedItem, String)),
            .CenteringLeft = DirectionFromDisplayString(DirectCast(LeftCenteringCombo.SelectedItem, String)),
            .PlacementDirectionRight = DirectionFromDisplayString(DirectCast(RightPlacementCombo.SelectedItem, String)),
            .CenteringRight = DirectionFromDisplayString(DirectCast(RightCenteringCombo.SelectedItem, String)),
            .Follow = FollowCheckBox.Checked,
            .DoNotRepeatImageAnimations = DoNotRepeatAnimationsCheckBox.Checked,
            .ParentPonyBase = parentBehavior.Base
        }
        parentBehavior.Base.Effects.Add(newEffect)

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class

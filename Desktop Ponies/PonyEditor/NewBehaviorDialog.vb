Imports System.Globalization

Public Class NewBehaviorDialog

    Dim follow_name As String = ""
    Dim follow_x As Integer = 0
    Dim follow_y As Integer = 0
    Dim left_image_path As String = ""
    Dim right_image_path As String = ""

    Private _editor As PonyEditor
    Private newBehavior As Behavior
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        _editor = editor
        newBehavior = New Behavior(_editor.CurrentBase)
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click

        If String.IsNullOrWhiteSpace(NameTextbox.Text) Then
            MessageBox.Show(Me, "You must enter a name for the new behavior.",
                            "No Name Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        For Each behavior In _editor.CurrentBase.Behaviors
            If behavior.Name = NameTextbox.Text Then
                MessageBox.Show(Me, "Behavior '" & behavior.Name & "' already exists for this pony. Please select another name.",
                                "Duplicate Name Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        Next

        If NameTextbox.Text.IndexOfAny({","c, "{"c, "}"c}) <> -1 Then
            MessageBox.Show(Me, "The behavior name cannot contain a comma (,) or curly braces ({}). Please select another name.",
                            "Invalid Name Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If left_image_path = "" OrElse right_image_path = "" Then
            MessageBox.Show(Me,
                            "You still need to select the two images to use for this behavior, for both the left and right directions.",
                            "Images Not Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If Movement_Combobox.SelectedIndex = -1 Then
            MessageBox.Show(Me, "You need to select a movement type.",
                            "No Movement Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim chance As Double

        If Not Double.TryParse(Trim(Replace(Chance_Box.Text, "%", "")), NumberStyles.Float, CultureInfo.InvariantCulture, chance) Then
            MessageBox.Show(Me, "You have not entered the chance the behavior has to occur (or the value you entered was invalid).",
                            "No Chance Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim minDuration As Double
        Dim maxDuration As Double

        If Not Double.TryParse(Trim(Replace(Min_Box.Text, "%", "")), NumberStyles.Float, CultureInfo.InvariantCulture, minDuration) OrElse
           Not Double.TryParse(Trim(Replace(Max_Box.Text, "%", "")), NumberStyles.Float, CultureInfo.InvariantCulture, maxDuration) Then
            MessageBox.Show(Me, "You need to enter minimum and maximum durations of the behavior in seconds.",
                            "Durations Not Set", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If minDuration > maxDuration Then
            MessageBox.Show(Me, "The maximum duration needs to be the same as, or larger than, the minimum duration.",
                            "Invalid Durations Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If chance < 0 OrElse minDuration < 0 OrElse maxDuration < 0 Then
            MessageBox.Show(Me, "You entered a negative value for a duration or chance. This is not allowed.",
                            "Invalid Value Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim speed As Double

        If Not Double.TryParse(Trim(Replace(Speed_Box.Text, "%", "")), NumberStyles.Float, CultureInfo.InvariantCulture, speed) Then
            MessageBox.Show(Me, "You have not entered the movement speed (or the value you entered was invalid).",
                            "No Speed Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim linked_behavior = ""

        linked_behavior = Link_Box.Text

        Dim skip = DontRunRandomly_CheckBox.Checked

        Dim start_line = ""
        If StartSpeech_Box.SelectedIndex <> -1 Then
            start_line = DirectCast(StartSpeech_Box.SelectedItem, CaseInsensitiveString)
        End If
        If start_line = "None" Then start_line = ""

        Dim end_line = ""
        If EndSpeech_Box.SelectedIndex <> -1 Then
            end_line = DirectCast(EndSpeech_Box.SelectedItem, CaseInsensitiveString)
        End If

        If end_line = "None" Then end_line = ""

        newBehavior.Name = NameTextbox.Text
        newBehavior.Chance = chance / 100
        newBehavior.MinDuration = minDuration
        newBehavior.MaxDuration = maxDuration
        newBehavior.Speed = speed
        newBehavior.LeftImage.Path = left_image_path
        newBehavior.RightImage.Path = right_image_path
        newBehavior.AllowedMovement = AllowedMovesFromString(DirectCast(Movement_Combobox.SelectedItem, String))
        newBehavior.LinkedBehaviorName = linked_behavior
        newBehavior.StartLineName = start_line
        newBehavior.EndLineName = end_line
        newBehavior.Skip = skip
        newBehavior.DoNotRepeatImageAnimations = DontRepeat_CheckBox.Checked
        newBehavior.Group = CInt(Group_Numberbox.Value)

        _editor.CurrentBase.Behaviors.Add(newBehavior)

        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub New_Behavior_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Link_Box.Items.Clear()
        EndSpeech_Box.Items.Clear()
        StartSpeech_Box.Items.Clear()
        Movement_Combobox.Items.Clear()

        Chance_Box.Text = ""
        Min_Box.Text = ""
        Max_Box.Text = ""
        Speed_Box.Text = ""
        follow_name = ""
        follow_x = 0
        follow_y = 0
        NameTextbox.Text = ""

        right_image_path = ""
        left_image_path = ""

        Right_ImageBox.Image = Nothing
        Left_ImageBox.Image = Nothing

        With _editor

            Dim linked_behavior_list = .colBehaviorLinked
            For Each item In linked_behavior_list.Items
                Link_Box.Items.Add(item)
            Next

            Dim speech_list = .colBehaviorStartSpeech
            For Each item In speech_list.Items
                StartSpeech_Box.Items.Add(item)
                EndSpeech_Box.Items.Add(item)
            Next

            Dim movement_list = .colBehaviorMovement
            For Each item In movement_list.Items
                Movement_Combobox.Items.Add(item)
            Next

        End With

        Follow_Box.Text = "[None]"

    End Sub

    Private Sub Left_Image_Set_Button_Click(sender As Object, e As EventArgs) Handles Left_Image_Set_Button.Click

        If Not IsNothing(Left_ImageBox.Image) Then
            Left_ImageBox.Image.Dispose()
            Left_ImageBox.Image = Nothing
        End If

        Dim path = _editor.AddPicture()
        If IsNothing(path) Then Exit Sub
        Left_ImageBox.Image = Image.FromFile(path)

        left_image_path = path

        _editor.ImageSizeCheck(Left_ImageBox.Image.Size)

        Dim runtime = GetGifTotalRuntime(Left_ImageBox.Image)

        If runtime <> 0 Then
            Using dialog As New GifRuntimeDialog()
                If dialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    Min_Box.Text = runtime.ToString(CultureInfo.CurrentCulture)
                    Max_Box.Text = runtime.ToString(CultureInfo.CurrentCulture)
                End If
            End Using
        End If

    End Sub

    Private Sub Right_Image_Set_Button_Click(sender As Object, e As EventArgs) Handles Right_Image_Set_Button.Click

        If Not IsNothing(Right_ImageBox.Image) Then
            Right_ImageBox.Image.Dispose()
            Right_ImageBox.Image = Nothing
        End If

        Dim path = _editor.AddPicture()
        If IsNothing(path) Then Exit Sub
        Right_ImageBox.Image = Image.FromFile(path)

        right_image_path = path

        _editor.ImageSizeCheck(Right_ImageBox.Image.Size)

        Dim runtime = GetGifTotalRuntime(Right_ImageBox.Image)

        If runtime <> 0 Then
            Using dialog As New GifRuntimeDialog()
                If dialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                    Min_Box.Text = runtime.ToString(CultureInfo.CurrentCulture)
                    Max_Box.Text = runtime.ToString(CultureInfo.CurrentCulture)
                End If
            End Using
        End If

    End Sub

    Private Sub SetFollow_Button_Click(sender As Object, e As EventArgs) Handles SetFollow_Button.Click
        Using dialog = New FollowTargetDialog(newBehavior)
            dialog.ShowDialog(Me)
        End Using

        Follow_Box.Text = If(newBehavior.TargetMode = TargetMode.Pony, newBehavior.OriginalFollowTargetName,
                             If(newBehavior.TargetMode = TargetMode.Point, New Vector2(newBehavior.OriginalDestinationXCoord,
                                                                                       newBehavior.OriginalDestinationYCoord).ToString(),
                                                                               "[None]"))

    End Sub

    Private Shared Function GetGifTotalRuntime(image As Image) As Double
        Try
            Dim dimension = New System.Drawing.Imaging.FrameDimension(image.FrameDimensionsList(0))
            Dim frameCount = image.GetFrameCount(dimension)

            Const PropertyTagFrameDelay As Integer = &H5100 ' From gdiplugimaging.h
            Dim frameDelaysItem = image.GetPropertyItem(PropertyTagFrameDelay)
            Dim frameDelaysItemValue = frameDelaysItem.Value

            Dim delays(frameCount) As Integer
            For frame = 0 To frameCount - 1
                delays(frame) = BitConverter.ToInt32(frameDelaysItemValue, frame * 4)
            Next

            Dim totalDelay = 0
            For Each delay In delays
                totalDelay += delay
            Next

            Return totalDelay / 100
        Catch ex As Exception
            ' Could not get timing info.
            Return 0
        End Try
    End Function

End Class

Imports System.Globalization

Public Class NewBehaviorDialog

    Dim follow_name As String = ""
    Dim follow_x As Integer = 0
    Dim follow_y As Integer = 0
    Dim left_image_path As String = ""
    Dim right_image_path As String = ""

    Private m_editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        m_editor = editor
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click

        If Trim(NameTextbox.Text) = "" Then
            MsgBox("You must enter a name for the new behavior.")
            Exit Sub
        End If

        For Each behavior In m_editor.PreviewPony.Behaviors
            If String.Equals(behavior.Name, NameTextbox.Text, StringComparison.OrdinalIgnoreCase) Then
                MsgBox("Behavior '" & behavior.Name & "' already exists for this pony.  Please select another name.")
                Exit Sub
            End If
        Next

        If InStr(NameTextbox.Text, ",") <> 0 Then
            MsgBox("The behavior name can't have a comma in it.")
            Exit Sub
        End If

        If InStr(NameTextbox.Text, "{") <> 0 Then
            MsgBox("The behavior name can't have a { in it.")
            Exit Sub
        End If

        If InStr(NameTextbox.Text, "}") <> 0 Then
            MsgBox("The behavior name can't have a } in it.")
            Exit Sub
        End If

        If left_image_path = "" OrElse right_image_path = "" Then
            MsgBox("You need to select two pictures - one left, one right.")
            Exit Sub
        End If

        If Movement_Combobox.SelectedIndex = -1 Then
            MsgBox("You need to select a movement type.")
            Exit Sub
        End If

        Dim chance As Double

        If Not Double.TryParse(Trim(Replace(Chance_Box.Text, "%", "")), NumberStyles.Float, CultureInfo.InvariantCulture, chance) Then
            MsgBox("You need to enter a % chance that the behavior has to occur (or you may have entered an invalid one).")
            Exit Sub
        End If

        Dim minDuration As Double
        Dim maxDuration As Double

        If Not Double.TryParse(Trim(Replace(Min_Box.Text, "%", "")), NumberStyles.Float, CultureInfo.InvariantCulture, minDuration) OrElse
           Not Double.TryParse(Trim(Replace(Max_Box.Text, "%", "")), NumberStyles.Float, CultureInfo.InvariantCulture, maxDuration) Then
            MsgBox("You need to enter minimum and maximum durations of the behavior in seconds.")
            Exit Sub
        End If

        If minDuration > maxDuration Then
            MsgBox("The maximum duration needs to be larger than the minimum duration.")
            Exit Sub
        End If

        If chance < 0 OrElse minDuration < 0 OrElse maxDuration < 0 Then
            MsgBox("You entered a negative value for % chance, Min duration, or Max duration.  Please correct this.")
            Exit Sub
        End If

        Dim speed As Double

        If Not Double.TryParse(Trim(Replace(Speed_Box.Text, "%", "")), NumberStyles.Float, CultureInfo.InvariantCulture, speed) Then
            MsgBox("You need to enter a movement speed (or entered an invalid one)")
            Exit Sub

        End If

        Dim linked_behavior = ""

        linked_behavior = Link_Box.Text

        Dim skip = DontRunRandomly_CheckBox.Checked

        Dim start_line = ""
        If StartSpeech_Box.SelectedIndex <> -1 Then
            start_line = CStr(StartSpeech_Box.SelectedItem)
        End If
        If start_line = "None" Then start_line = ""

        Dim end_line = ""
        If EndSpeech_Box.SelectedIndex <> -1 Then
            end_line = CStr(EndSpeech_Box.SelectedItem)
        End If

        If end_line = "None" Then end_line = ""

        m_editor.PreviewPonyBase.AddBehavior(NameTextbox.Text, _
                                                       chance / 100, _
                                                       maxDuration, _
                                                       minDuration, _
                                                       speed, _
                                                       right_image_path, _
                                                       left_image_path, _
                                                       PonyEditor.String_ToMovement(CStr(Movement_Combobox.SelectedItem)), _
                                                       linked_behavior, _
                                                       start_line, _
                                                       end_line, _
                                                       skip, _
                                                       follow_x, _
                                                       follow_y, _
                                                       follow_name,
                                                    False, "", "", Nothing, Nothing, DontRepeat_CheckBox.Checked, CInt(Group_Numberbox.Value))


        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click

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

        With m_editor

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

        Follow_Box.Text = "None"

    End Sub

    Private Sub Left_Image_Set_Button_Click(sender As Object, e As EventArgs) Handles Left_Image_Set_Button.Click

        If Not IsNothing(Left_ImageBox.Image) Then
            Left_ImageBox.Image.Dispose()
            Left_ImageBox.Image = Nothing
        End If

        Dim path = m_editor.Add_Picture()
        If IsNothing(path) Then Exit Sub
        Left_ImageBox.Image = Image.FromFile(path)

        left_image_path = path

        m_editor.ImageSizeCheck(Left_ImageBox.Image.Size)

        Dim runtime = GetGifTotalRuntime(Left_ImageBox.Image)

        If runtime <> 0 AndAlso My.Forms.GifRuntimeDialog.ShowDialog = DialogResult.OK Then
            Min_Box.Text = CStr(runtime)
            Max_Box.Text = CStr(runtime)
        End If

    End Sub

    Private Sub Right_Image_Set_Button_Click(sender As Object, e As EventArgs) Handles Right_Image_Set_Button.Click

        If Not IsNothing(Right_ImageBox.Image) Then
            Right_ImageBox.Image.Dispose()
            Right_ImageBox.Image = Nothing
        End If

        Dim path = m_editor.Add_Picture()
        If IsNothing(path) Then Exit Sub
        Right_ImageBox.Image = Image.FromFile(path)

        right_image_path = path

        m_editor.ImageSizeCheck(Right_ImageBox.Image.Size)

        Dim runtime = GetGifTotalRuntime(Right_ImageBox.Image)

        If runtime <> 0 AndAlso My.Forms.GifRuntimeDialog.ShowDialog = DialogResult.OK Then
            Min_Box.Text = CStr(runtime)
            Max_Box.Text = CStr(runtime)
        End If

    End Sub

    Private Sub SetFollow_Button_Click(sender As Object, e As EventArgs) Handles SetFollow_Button.Click

        Dim new_behavior As New Behavior("", "")

        Using form = New FollowTargetDialog(m_editor)
            form.Change_Behavior(new_behavior)
            form.ShowDialog()
        End Using

        If new_behavior.OriginalFollowObjectName <> "" Then
            follow_name = new_behavior.OriginalFollowObjectName
            'MsgBox("Note:  If you wish to have this behavior follow another pony, you should change the movement type to 'All'."
        Else
            If new_behavior.OriginalDestinationXCoord <> 0 AndAlso new_behavior.OriginalDestinationYCoord <> 0 Then
                follow_name = new_behavior.OriginalDestinationXCoord & " , " & new_behavior.OriginalDestinationYCoord
            End If
        End If

        Follow_Box.Text = follow_name
        follow_name = new_behavior.OriginalFollowObjectName
        follow_x = new_behavior.OriginalDestinationXCoord
        follow_y = new_behavior.OriginalDestinationYCoord

    End Sub

    Friend Shared Function GetGifTotalRuntime(image As Image) As Double

        Try
            Dim gif_dimensions = New System.Drawing.Imaging.FrameDimension(image.FrameDimensionsList(0))
            Dim gif_framecount = image.GetFrameCount(gif_dimensions)

            Dim PropertyTagFrameDelay As Integer = &H5100 '"from gdiplugimaging.h"

            Dim propertyitem = image.GetPropertyItem(PropertyTagFrameDelay)

            Dim bytes() = propertyitem.Value

            Dim delays(gif_framecount) As Integer

            For frame = 0 To gif_framecount - 1
                delays(frame) = BitConverter.ToInt32(bytes, frame * 4)
            Next

            Dim total_delay As Integer = 0
            For Each delay In delays
                total_delay += delay
            Next

            Return total_delay / 100

        Catch ex As Exception

            'could not get timing info.
            Return 0

        End Try
    End Function

End Class

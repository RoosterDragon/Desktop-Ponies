Imports System.Globalization

Public Class NewEffectDialog

    Dim left_image_path As String = ""
    Dim right_image_path As String = ""

    Private m_editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        m_editor = editor
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click

        If Trim(Name_Textbox.Text) = "" Then
            MsgBox("You must enter a name for the new effect.")
            Exit Sub
        End If

        For Each effect In m_editor.get_effect_list()
            If String.Equals(effect.Name, Name_Textbox.Text, StringComparison.OrdinalIgnoreCase) Then
                MsgBox("Effect names must be unique.  Effect '" & Name_Textbox.Text & "' already exists.  Please select a different name.")
                Exit Sub
            End If
        Next

        If left_image_path = "" OrElse right_image_path = "" Then
            MsgBox("You need to select two pictures - one left, one right.")
            Exit Sub
        End If

        Dim duration As Double

        If Not Double.TryParse(Duration_Box.Text, NumberStyles.Float, CultureInfo.CurrentCulture, duration) Then
            MsgBox("You need to enter a duration for the effect in seconds.")
            Exit Sub
        End If

        If duration < 0 Then
            MsgBox("You entered a negative value for duration.  Please correct this.")
            Exit Sub
        End If

        Dim repeatDelay As Double

        If Not Double.TryParse(repeat_box.Text, NumberStyles.Float, CultureInfo.CurrentCulture, repeatDelay) Then
            MsgBox("You need to enter a repeat delay for the effect in seconds.")
            Exit Sub
        End If

        If repeatDelay < 0.03 AndAlso repeatDelay <> 0 Then
            MsgBox("You entered a value for repeat delay that is too small.  It needs to be greater than 0.03 or 0.")
            Exit Sub
        End If

        Dim linked_behavior = ""
        If Behavior_Box.SelectedIndex <> -1 Then
            linked_behavior = CStr(Behavior_Box.SelectedItem)
        End If

        If linked_behavior = "" Then
            MsgBox("You need to select a behavior to trigger this effect")
            Exit Sub
        End If

        Dim behavior As PonyBase.Behavior = Nothing

        For Each behavior In m_editor.PreviewPony.Behaviors
            If String.Equals(behavior.Name, CStr(Behavior_Box.SelectedItem), StringComparison.OrdinalIgnoreCase) Then
                behavior = behavior
                Exit For
            End If
        Next

        If IsNothing(behavior) Then
            Throw New Exception("Couldn't find behavior to link effect to!")
        End If


        behavior.AddEffect(Name_Textbox.Text, _
                           right_image_path, _
                           left_image_path, _
                           duration, _
                           repeatDelay, _
                           m_editor.String_ToLocation(CStr(R_Placement_Box.SelectedItem)), _
                           m_editor.String_ToLocation(CStr(R_Centering_Box.SelectedItem)), _
                           m_editor.String_ToLocation(CStr(L_Placement_Box.SelectedItem)), _
                           m_editor.String_ToLocation(CStr(L_Centering_Box.SelectedItem)), _
                           follow_checkbox.Checked, DontRepeat_CheckBox.Checked, True)

        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

    Private Sub New_Behavior_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Behavior_Box.Items.Clear()
        R_Placement_Box.Items.Clear()
        L_Placement_Box.Items.Clear()
        R_Centering_Box.Items.Clear()
        L_Centering_Box.Items.Clear()

        Duration_Box.Text = ""
        Name_Textbox.Text = ""

        right_image_path = ""
        left_image_path = ""

        Right_ImageBox.Image = Nothing
        Left_ImageBox.Image = Nothing

        With m_editor

            Dim linked_behavior_list = .colBehaviorLinked
            For Each item In linked_behavior_list.Items
                Behavior_Box.Items.Add(item)
            Next

            Behavior_Box.Items.Remove("None")

            Dim movement_list = CType(.PonyEffectsGrid.Columns(.colEffectLocationRight.Index), DataGridViewComboBoxColumn)
            For Each item In movement_list.Items
                R_Placement_Box.Items.Add(item)
                L_Placement_Box.Items.Add(item)
                R_Centering_Box.Items.Add(item)
                L_Centering_Box.Items.Add(item)
            Next


            R_Placement_Box.SelectedIndex = 8
            L_Placement_Box.SelectedIndex = 8
            R_Centering_Box.SelectedIndex = 8
            L_Centering_Box.SelectedIndex = 8
        End With


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

        Dim runtime = GetGifTotalRuntime(Left_ImageBox.Image)

        If runtime <> 0 AndAlso My.Forms.GifRuntimeDialog.ShowDialog = DialogResult.OK Then
            Duration_Box.Text = CStr(runtime)
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

        Dim runtime = GetGifTotalRuntime(Left_ImageBox.Image)

        If runtime <> 0 AndAlso My.Forms.GifRuntimeDialog.ShowDialog = DialogResult.OK Then
            Duration_Box.Text = CStr(runtime)
        End If

    End Sub

    Friend Function GetGifTotalRuntime(image As Image) As Double

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

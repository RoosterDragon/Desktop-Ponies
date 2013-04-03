Imports System.Globalization

Public Class NewInteractionDialog

    Dim change_existing_interaction As Boolean = False
    Dim existing_interaction As Interaction = Nothing

    Private m_editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        m_editor = editor
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        If Trim(Name_Textbox.Text) = "" Then
            MsgBox("You must enter a name for the new behavior.")
            Exit Sub
        End If

        If InStr(Name_Textbox.Text, ",") <> 0 Then
            MsgBox("The interaction name can't have a comma in it.")
            Exit Sub
        End If

        For Each Interaction In m_editor.PreviewPony.Interactions
            If Not change_existing_interaction AndAlso
                String.Equals(Interaction.Name, Trim(Name_Textbox.Text), StringComparison.OrdinalIgnoreCase) Then
                MsgBox("Interaction with name '" & Interaction.Name & "' already exists.  Please select a different name.")
                Exit Sub
            End If
        Next

        Dim chance As Double

        If Not Double.TryParse(Trim(Replace(Chance_Box.Text, "%", "")), NumberStyles.Float, CultureInfo.CurrentCulture, chance) Then
            MsgBox("You need to enter a % chance that the behavior has to occur (or you may have entered an invalid one).")
            Exit Sub
        End If

        If chance < 0 Then
            MsgBox("You entered a negative value for % chance.  Please correct this.")
            Exit Sub
        End If

        Dim proximity As Double

        If Not Double.TryParse(Proximity_Box.Text, NumberStyles.Float, CultureInfo.CurrentCulture, proximity) Then
            MsgBox("You need to enter a number for proximity.")
            Exit Sub
        End If

        If proximity < 0 Then
            MsgBox("You entered a negative value for activation distance.  Please correct this.")
            Exit Sub
        End If

        Dim reactivationDelay As Double

        If Not Double.TryParse(Reactivation_Delay_Textbox.Text, NumberStyles.Float, CultureInfo.CurrentCulture, reactivationDelay) Then
            MsgBox("You need to enter a number for the reactivation delay.")
            Exit Sub
        End If

        If reactivationDelay < 0 Then
            MsgBox("You entered a negative value for reactivation delay.  Please correct this.")
            Exit Sub
        End If

        If Targets_Box.CheckedItems.Count = 0 Then
            MsgBox("You need to select at least one pony to interact with.")
            Exit Sub
        End If

        If Behaviors_Box.CheckedItems.Count = 0 Then
            MsgBox("You need to select at least one behavior to trigger.")
            Exit Sub
        End If

        Dim targetlist As String = ""

        For Each Pony As String In Targets_Box.CheckedItems
            targetlist += Quoted(Pony) & ","
        Next

        targetlist = Mid(targetlist, 1, targetlist.Length - 1)

        Dim behaviorlist As String = ""
        For Each behavior As String In Behaviors_Box.CheckedItems
            behaviorlist += behavior & ","
        Next

        behaviorlist = Mid(behaviorlist, 1, behaviorlist.Length - 1)

        If change_existing_interaction Then
            Dim toRemove = m_editor.PreviewPonyBase.Interactions.Where(
                Function(interaction) interaction.Name = Name_Textbox.Text).ToArray()
            For Each interaction In toRemove
                m_editor.PreviewPonyBase.Interactions.Remove(interaction)
            Next
        End If

        Dim targetsActivated As Interaction.TargetActivation
        If OneRadioButton.Checked Then targetsActivated = Interaction.TargetActivation.One
        If AnyRadioButton.Checked Then targetsActivated = Interaction.TargetActivation.Any
        If AllRadioButton.Checked Then targetsActivated = Interaction.TargetActivation.All

        m_editor.PreviewPonyBase.AddInteraction(Name_Textbox.Text,
                                                      m_editor.PreviewPony.Directory, _
                                                      chance / 100,
                                                      Proximity_Box.Text, _
                                                      targetlist, _
                                                      targetsActivated, _
                                                      behaviorlist, _
                                                      CInt(reactivationDelay), _
                                                      False)

        MessageBox.Show(Me, "Important note:" & vbCrLf &
                        "You need to make sure all the targets ponies have all the behaviors you selected, or the interaction won't work.",
                        "Desktop Ponies", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        Me.Close()
    End Sub

    Friend Sub ChangeInteraction(interaction As Interaction)
        Targets_Box.Items.Clear()
        Behaviors_Box.Items.Clear()

        With m_editor

            Dim linked_behavior_list = .colBehaviorLinked
            For Each item In linked_behavior_list.Items
                Behaviors_Box.Items.Add(item)
            Next

            For Each Pony In Main.Instance.SelectablePonies
                Targets_Box.Items.Add(Pony.Directory)
            Next

        End With

        Behaviors_Box.Items.Remove("None")

        Me.Text = "Create new interaction..."

        Chance_Box.Text = ""
        Proximity_Box.Text = "300"
        Name_Textbox.Text = ""

        If IsNothing(interaction) Then
            change_existing_interaction = False
            Name_Textbox.Enabled = True
            existing_interaction = Nothing
            Exit Sub
        End If

        Name_Textbox.Enabled = False
        change_existing_interaction = True
        existing_interaction = interaction
        Select Case interaction.Targets_Activated
            Case Interaction.TargetActivation.One
                OneRadioButton.Checked = True
            Case Interaction.TargetActivation.Any
                AnyRadioButton.Checked = True
            Case Interaction.TargetActivation.All
                AllRadioButton.Checked = True
        End Select

        Chance_Box.Text = CStr(interaction.Probability * 100)
        Name_Textbox.Text = interaction.Name
        Proximity_Box.Text = CStr(interaction.Proximity_Activation_Distance)
        Me.Text = "Edit interaction..."

        Dim targets = CommaSplitQuoteQualified(interaction.Targets_String)

        Dim target_index_list As New List(Of Integer)

        For Each target In targets
            For Each item As String In Targets_Box.Items
                If String.Equals(Trim(target), Trim(item), StringComparison.OrdinalIgnoreCase) Then
                    target_index_list.Add(Targets_Box.Items.IndexOf(item))
                End If
            Next
        Next

        Dim behaviors_index_list As New List(Of Integer)

        For Each behavior In interaction.BehaviorList
            For Each item As String In Behaviors_Box.Items
                If String.Equals(Trim(behavior.Name), Trim(item), StringComparison.OrdinalIgnoreCase) Then
                    behaviors_index_list.Add(Behaviors_Box.Items.IndexOf(item))
                End If
            Next
        Next

        For Each index In target_index_list
            Targets_Box.SetItemChecked(index, True)
        Next

        For Each index In behaviors_index_list
            Behaviors_Box.SetItemChecked(index, True)
        Next
    End Sub
End Class

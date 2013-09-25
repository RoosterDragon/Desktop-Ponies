Imports System.Globalization

Public Class NewInteractionDialog

    Dim change_existing_interaction As Boolean = False

    Private _editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        _editor = editor
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

        For Each Interaction In _editor.PreviewPony.InteractionBases
            If Not change_existing_interaction AndAlso Interaction.Name = Trim(Name_Textbox.Text) Then
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

        If change_existing_interaction Then
            Dim toRemove = _editor.PreviewPony.Base.Interactions.Where(
                Function(interaction) interaction.Name = Name_Textbox.Text).ToArray()
            For Each interaction In toRemove
                _editor.PreviewPony.Base.Interactions.Remove(interaction)
            Next
        End If

        Dim targetsActivated As TargetActivation
        If OneRadioButton.Checked Then targetsActivated = TargetActivation.One
        If AnyRadioButton.Checked Then targetsActivated = TargetActivation.Any
        If AllRadioButton.Checked Then targetsActivated = TargetActivation.All

        Dim newInteraction = New InteractionBase() With
                             {.Name = Name_Textbox.Text,
                              .InitiatorName = _editor.PreviewPony.Directory,
                              .Chance = chance / 100,
                              .Proximity = proximity,
                              .Activation = targetsActivated,
                              .ReactivationDelay = TimeSpan.FromSeconds(reactivationDelay)}
        newInteraction.TargetNames.UnionWith(Targets_Box.CheckedItems.Cast(Of String))
        newInteraction.BehaviorNames.UnionWith(Behaviors_Box.CheckedItems.Cast(Of String).Select(Function(s) New CaseInsensitiveString(s)))
        _editor.PreviewPony.Base.Interactions.Add(newInteraction)

        MessageBox.Show(Me, "Important note:" & Environment.NewLine &
                        "You need to make sure all the targets ponies have all the behaviors you selected, or the interaction won't work.",
                        "Desktop Ponies", MessageBoxButtons.OK, MessageBoxIcon.Information)

        DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Friend Sub ChangeInteraction(interaction As InteractionBase)
        Targets_Box.Items.Clear()
        Behaviors_Box.Items.Clear()

        With _editor

            Dim linked_behavior_list = .colBehaviorLinked
            For Each item In linked_behavior_list.Items
                Behaviors_Box.Items.Add(item)
            Next

            For Each Pony In _editor.Ponies.AllBases
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
            Exit Sub
        End If

        Name_Textbox.Enabled = False
        change_existing_interaction = True
        Select Case interaction.Activation
            Case TargetActivation.One
                OneRadioButton.Checked = True
            Case TargetActivation.Any
                AnyRadioButton.Checked = True
            Case TargetActivation.All
                AllRadioButton.Checked = True
        End Select

        Chance_Box.Text = (interaction.Chance * 100).ToString(CultureInfo.CurrentCulture)
        Name_Textbox.Text = interaction.Name
        Proximity_Box.Text = interaction.Proximity.ToString(CultureInfo.CurrentCulture)
        Reactivation_Delay_Textbox.Text = interaction.ReactivationDelay.TotalSeconds.ToString(CultureInfo.CurrentCulture)
        Me.Text = "Edit interaction..."

        Dim target_index_list As New List(Of Integer)

        For Each target In interaction.TargetNames
            For Each item As String In Targets_Box.Items
                If Trim(target) = Trim(item) Then
                    target_index_list.Add(Targets_Box.Items.IndexOf(item))
                End If
            Next
        Next

        Dim behaviors_index_list As New List(Of Integer)

        For Each behaviorName In interaction.BehaviorNames
            For Each item As String In Behaviors_Box.Items
                If behaviorName = item Then
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

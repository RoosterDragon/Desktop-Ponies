Imports System.Globalization

Public Class NewInteractionDialog

    Private base As PonyBase
    Private interactionToEdit As InteractionBase

    Public Sub New(interaction As InteractionBase, ponyBase As PonyBase)
        base = Argument.EnsureNotNull(ponyBase, "ponyBase")
        interactionToEdit = interaction
        InitializeComponent()
        Icon = My.Resources.Twilight

        BehaviorsList.SuspendLayout()
        For Each behavior In ponyBase.Behaviors
            BehaviorsList.Items.Add(behavior.Name)
        Next
        If interaction IsNot Nothing Then
            For i = 0 To BehaviorsList.Items.Count - 1
                If interaction.BehaviorNames.Contains(DirectCast(BehaviorsList.Items(i), CaseInsensitiveString)) Then
                    BehaviorsList.SetItemChecked(i, True)
                End If
            Next
        End If
        BehaviorsList.ResumeLayout()

        TargetsList.SuspendLayout()
        For Each target In ponyBase.Collection.Bases
            TargetsList.Items.Add(target.Directory)
        Next
        If interaction IsNot Nothing Then
            For i = 0 To TargetsList.Items.Count - 1
                If interaction.TargetNames.Contains(DirectCast(TargetsList.Items(i), String)) Then
                    TargetsList.SetItemChecked(i, True)
                End If
            Next
        End If
        TargetsList.ResumeLayout()

        If interaction Is Nothing Then Return

        Text = "Edit Interaction..."
        NameTextBox.Text = interaction.Name
        ChanceNumber.Value = CDec(interaction.Chance) * 100
        ProximityNumber.Value = CDec(interaction.Proximity)
        DelayNumber.Value = CDec(interaction.ReactivationDelay.TotalSeconds)
        Select Case interaction.Activation
            Case TargetActivation.One
                OneRadioButton.Checked = True
            Case TargetActivation.Any
                AnyRadioButton.Checked = True
            Case TargetActivation.All
                AllRadioButton.Checked = True
        End Select
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        Dim newName = NameTextBox.Text.Trim()
        If Not EditorCommon.ValidateName(Me, "interaction", newName, base.Interactions,
                                         If(interactionToEdit Is Nothing, Nothing, interactionToEdit.Name)) Then Return

        If TargetsList.CheckedItems.Count = 0 Then
            MessageBox.Show(Me, "You need to select at least one target to interact with.",
                            "No Targets Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If BehaviorsList.CheckedItems.Count = 0 Then
            MessageBox.Show(Me, "You need to select at least one behavior to activate.",
                            "No Behaviors Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim targetsActivated As TargetActivation
        If OneRadioButton.Checked Then targetsActivated = TargetActivation.One
        If AnyRadioButton.Checked Then targetsActivated = TargetActivation.Any
        If AllRadioButton.Checked Then targetsActivated = TargetActivation.All

        If interactionToEdit Is Nothing Then
            interactionToEdit = New InteractionBase()
            base.Interactions.Add(interactionToEdit)
        End If

        interactionToEdit.Name = newName
        interactionToEdit.InitiatorName = base.Directory
        interactionToEdit.Chance = ChanceNumber.Value / 100
        interactionToEdit.Proximity = ProximityNumber.Value
        interactionToEdit.Activation = targetsActivated
        interactionToEdit.ReactivationDelay = TimeSpan.FromSeconds(DelayNumber.Value)
        interactionToEdit.TargetNames.Clear()
        interactionToEdit.TargetNames.UnionWith(TargetsList.CheckedItems.Cast(Of String))
        interactionToEdit.BehaviorNames.Clear()
        interactionToEdit.BehaviorNames.UnionWith(BehaviorsList.CheckedItems.Cast(Of CaseInsensitiveString))

        MessageBox.Show(Me, "Important note:" & Environment.NewLine &
                        "You need to make sure each target pony has one or more of the behaviors you selected, or the interaction won't work.",
                        "Desktop Ponies", MessageBoxButtons.OK, MessageBoxIcon.Information)

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class

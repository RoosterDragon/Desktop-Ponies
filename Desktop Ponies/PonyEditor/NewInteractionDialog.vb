Imports System.Globalization

Public Class NewInteractionDialog

    Private base As PonyBase
    Private interactionToEdit As InteractionBase

    Public Sub New(interaction As InteractionBase, ponyBase As PonyBase)
        base = Argument.EnsureNotNull(ponyBase, "ponyBase")
        interactionToEdit = interaction
        InitializeComponent()
        Icon = My.Resources.Twilight

        TargetsList.SuspendLayout()
        For Each target In ponyBase.Collection.Bases
            TargetsList.Items.Add(target.Directory)
        Next
        If interaction IsNot Nothing Then
            For i = 0 To TargetsList.Items.Count - 1
                TargetsList.SetItemChecked(i, interaction.TargetNames.Contains(DirectCast(TargetsList.Items(i), String)))
            Next
        End If
        TargetsList.ResumeLayout()
        AddHandler TargetsList.ItemCheck, AddressOf TargetsList_ItemCheck

        If interaction IsNot Nothing Then
            RebuildBehaviorsList(interaction.TargetNames, interaction.BehaviorNames)
        Else
            RebuildBehaviorsList({}, New HashSet(Of CaseInsensitiveString)())
        End If

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

    Private Sub RebuildBehaviorsList(targetNames As IEnumerable(Of String), behaviorNames As ISet(Of CaseInsensitiveString))
        Dim behaviors = New SortedSet(Of CaseInsensitiveString)(behaviorNames)
        behaviors.UnionWith(base.Behaviors.Select(Function(b) b.Name))
        For Each targetName In targetNames
            Dim targetBase = base.Collection.Bases.FirstOrDefault(Function(b) b.Directory = targetName)
            If targetBase Is Nothing Then Continue For
            behaviors.UnionWith(targetBase.Behaviors.Select(Function(b) b.Name))
        Next

        BehaviorsList.SuspendLayout()
        BehaviorsList.Items.Clear()
        BehaviorsList.Items.AddRange(behaviors.ToArray())
        For i = 0 To BehaviorsList.Items.Count - 1
            BehaviorsList.SetItemChecked(i, behaviorNames.Contains(DirectCast(BehaviorsList.Items(i), CaseInsensitiveString)))
        Next
        BehaviorsList.ResumeLayout()
    End Sub

    Private Sub TargetsList_ItemCheck(sender As Object, e As ItemCheckEventArgs)
        Dim targets = New HashSet(Of String)(TargetsList.CheckedItems.Cast(Of String)())
        Dim item = DirectCast(TargetsList.Items(e.Index), String)
        If e.NewValue <> CheckState.Unchecked Then
            targets.Add(item)
        Else
            targets.Remove(item)
        End If
        RebuildBehaviorsList(targets, New HashSet(Of CaseInsensitiveString)(BehaviorsList.CheckedItems.Cast(Of CaseInsensitiveString)()))
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

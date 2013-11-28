Imports System.Globalization

Public Class NewInteractionDialog

    Private base As PonyBase
    Private interactionToEdit As InteractionBase

    Public Sub New(interaction As InteractionBase, ponyBase As PonyBase)
        InitializeComponent()

        base = Argument.EnsureNotNull(ponyBase, "ponyBase")
        interactionToEdit = interaction

        For Each behavior In ponyBase.Behaviors
            BehaviorsList.Items.Add(behavior.Name)
        Next

        For Each target In ponyBase.Collection.Bases
            TargetsList.Items.Add(target.Directory)
        Next

        If interaction Is Nothing Then Return

        Text = "Edit Interaction..."
        NameTextbox.Text = interaction.Name
        ChanceTextbox.Text = (interaction.Chance * 100).ToString(CultureInfo.CurrentCulture)
        ProximityTextbox.Text = interaction.Proximity.ToString(CultureInfo.CurrentCulture)
        ReactivationDelayTextbox.Text = interaction.ReactivationDelay.TotalSeconds.ToString(CultureInfo.CurrentCulture)
        Select Case interaction.Activation
            Case TargetActivation.One
                OneRadioButton.Checked = True
            Case TargetActivation.Any
                AnyRadioButton.Checked = True
            Case TargetActivation.All
                AllRadioButton.Checked = True
        End Select

        For i = 0 To BehaviorsList.Items.Count - 1
            If interaction.BehaviorNames.Contains(DirectCast(BehaviorsList.Items(i), CaseInsensitiveString)) Then
                BehaviorsList.SetItemChecked(i, True)
            End If
        Next

        For i = 0 To TargetsList.Items.Count - 1
            If interaction.TargetNames.Contains(DirectCast(TargetsList.Items(i), String)) Then
                TargetsList.SetItemChecked(i, True)
            End If
        Next
    End Sub

    Private Sub OK_Button_Click(sender As Object, e As EventArgs) Handles OK_Button.Click
        If String.IsNullOrWhiteSpace(NameTextbox.Text) Then
            MessageBox.Show(Me, "You must enter a name for the new interaction.",
                            "No Name Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        For Each interaction In base.Interactions
            If interaction.Name = NameTextbox.Text AndAlso Not Object.ReferenceEquals(interactionToEdit, interaction) Then
                MessageBox.Show(Me, "Interaction '" & interaction.Name & "' already exists for this pony. Please select another name.",
                                "Duplicate Name Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
        Next

        If NameTextbox.Text.IndexOfAny({","c, "{"c, "}"c}) <> -1 Then
            MessageBox.Show(Me, "The interaction name cannot contain a comma (,) or curly braces ({}). Please select another name.",
                            "Invalid Name Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim chance As Double

        If Not Double.TryParse(Trim(Replace(ChanceTextbox.Text, "%", "")), NumberStyles.Float, CultureInfo.InvariantCulture, chance) Then
            MessageBox.Show(Me, "You have not entered the chance the interaction has to occur (or the value you entered was invalid).",
                            "No Chance Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If chance < 0 Then
            MessageBox.Show(Me, "You entered a negative value for chance. This is not allowed.",
                            "Invalid Value Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim proximity As Double

        If Not Double.TryParse(ProximityTextbox.Text, NumberStyles.Float, CultureInfo.CurrentCulture, proximity) Then
            MessageBox.Show(Me, "You have not entered the proximity (or the value you entered was invalid).",
                            "No Speed Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If proximity < 0 Then
            MessageBox.Show(Me, "You entered a negative value for proximity. This is not allowed.",
                            "Invalid Value Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        Dim reactivationDelay As Double

        If Not Double.TryParse(ReactivationDelayTextbox.Text, NumberStyles.Float, CultureInfo.CurrentCulture, reactivationDelay) Then
            MessageBox.Show(Me, "You have not entered the reactivation delay (or the value you entered was invalid).",
                            "No Speed Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

        If reactivationDelay < 0 Then
            MessageBox.Show(Me, "You entered a negative value for reactivation delay. This is not allowed.",
                            "Invalid Value Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

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

        interactionToEdit.Name = NameTextbox.Text
        interactionToEdit.InitiatorName = base.Directory
        interactionToEdit.Chance = chance / 100
        interactionToEdit.Proximity = proximity
        interactionToEdit.Activation = targetsActivated
        interactionToEdit.ReactivationDelay = TimeSpan.FromSeconds(reactivationDelay)
        interactionToEdit.TargetNames.Clear()
        interactionToEdit.TargetNames.UnionWith(TargetsList.CheckedItems.Cast(Of String))
        interactionToEdit.BehaviorNames.Clear()
        interactionToEdit.BehaviorNames.UnionWith(BehaviorsList.CheckedItems.Cast(Of CaseInsensitiveString))

        MessageBox.Show(Me, "Important note:" & Environment.NewLine &
                        "You need to make sure all the targets ponies have all the behaviors you selected, or the interaction won't work.",
                        "Desktop Ponies", MessageBoxButtons.OK, MessageBoxIcon.Information)

        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub Cancel_Button_Click(sender As Object, e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class

Public Class InteractionEditor
    Private Shared typeValues As Object() =
        [Enum].GetValues(GetType(TargetActivation)).Cast(Of Object)().ToArray()

    Private Shadows Property Edited As InteractionBase
        Get
            Return DirectCast(MyBase.Edited, InteractionBase)
        End Get
        Set(value As InteractionBase)
            MyBase.Edited = value
        End Set
    End Property
    Protected Overrides ReadOnly Property Collection As System.Collections.IList
        Get
            Return CType(Base.Interactions, Collections.IList)
        End Get
    End Property
    Protected Overrides ReadOnly Property ItemTypeName As String
        Get
            Return "interaction"
        End Get
    End Property

    Public Sub New()
        InitializeComponent()
        TypeComboBox.Items.AddRange(typeValues)
        AddHandler NameTextBox.TextChanged, Sub() UpdateProperty(Sub() Edited.Name = NameTextBox.Text)
        AddHandler ChanceNumber.ValueChanged, Sub() UpdateProperty(Sub() Edited.Chance = ChanceNumber.Value / 100)
        AddHandler TypeComboBox.SelectedIndexChanged,
            Sub() UpdateProperty(Sub() Edited.Activation = DirectCast(TypeComboBox.SelectedItem, TargetActivation))
        AddHandler ProximityNumber.ValueChanged, Sub() UpdateProperty(Sub() Edited.Proximity = ProximityNumber.Value)
        AddHandler DelayNumber.ValueChanged, Sub() UpdateProperty(Sub() Edited.ReactivationDelay = TimeSpan.FromSeconds(DelayNumber.Value))
        AddHandler TargetsList.ItemCheck,
            Sub(sender, e) UpdateProperty(Sub()
                                              Edited.TargetNames.Clear()
                                              Edited.TargetNames.UnionWith(TargetsList.CheckedItems.Cast(Of String))
                                              Dim item = DirectCast(TargetsList.Items(e.Index), String)
                                              If e.NewValue <> CheckState.Unchecked Then
                                                  Edited.TargetNames.Add(item)
                                              Else
                                                  Edited.TargetNames.Remove(item)
                                              End If
                                          End Sub)
        AddHandler BehaviorsList.ItemCheck,
            Sub(sender, e) UpdateProperty(Sub()
                                              Edited.BehaviorNames.Clear()
                                              Edited.BehaviorNames.UnionWith(BehaviorsList.CheckedItems.Cast(Of String))
                                              Dim item = DirectCast(BehaviorsList.Items(e.Index), String)
                                              If e.NewValue <> CheckState.Unchecked Then
                                                  Edited.BehaviorNames.Add(item)
                                              Else
                                                  Edited.BehaviorNames.Remove(item)
                                              End If
                                          End Sub)
    End Sub

    Public Overrides Sub NewItem(name As String)
        ' TODO.
    End Sub

    Public Overrides Sub LoadItem()
        TargetsList.Items.Clear()
        TargetsList.Items.AddRange(Base.Collection.AllBases.Select(Function(pb) pb.Directory).ToArray())
        BehaviorsList.Items.Clear()
        BehaviorsList.Items.AddRange(Base.Behaviors.Select(Function(b) b.Name).ToArray())
    End Sub

    Private Sub NameTextBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles NameTextBox.KeyPress
        e.Handled = (e.KeyChar = """"c)
    End Sub

    Protected Overrides Sub SourceTextChanged()
        Dim ib As InteractionBase = Nothing
        InteractionBase.TryLoad(Source.Text, ib, ParseIssues)
        ReferentialIssues = ib.GetReferentialIssues(Base.Collection)
        OnIssuesChanged(EventArgs.Empty)
        Edited = ib

        NameTextBox.Text = Edited.Name
        ChanceNumber.Value = CDec(Edited.Chance * 100)
        ProximityNumber.Value = CDec(Edited.Proximity)
        DelayNumber.Value = CDec(Edited.ReactivationDelay.TotalSeconds)

        SelectItemElseNoneOption(TypeComboBox, Edited.Activation)
        UpdateList(TargetsList, Edited.TargetNames)
        UpdateList(BehaviorsList, Edited.BehaviorNames)
    End Sub

    Private Sub UpdateList(list As CheckedListBox, values As HashSet(Of String))
        list.SuspendLayout()
        For i = 0 To list.Items.Count - 1
            list.SetItemChecked(i, values.Contains(DirectCast(list.Items(i), String)))
        Next
        list.ResumeLayout()
    End Sub
End Class

Friend Class InteractionEditor
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

    Protected Overrides Sub CreateBindings()
        TypeComboBox.Items.AddRange(typeValues)
        Bind(Function() Edited.Name, NameTextBox)
        Bind(Function() Edited.Chance, ChanceNumber, Function(dbl) CDec(dbl) * 100, Function(dec) dec / 100)
        Bind(Function() Edited.Activation, TypeComboBox)
        Bind(Function() Edited.Proximity, ProximityNumber, Function(dbl) CDec(dbl), Function(dec) CDbl(dec))
        Bind(Function() Edited.ReactivationDelay, DelayNumber, Function(ts) CDec(ts.TotalSeconds), Function(dec) TimeSpan.FromSeconds(dec))
        Bind(Function() Edited.TargetNames, TargetsList)
        Bind(Function() Edited.BehaviorNames, BehaviorsList)
    End Sub

    Protected Overrides Sub ChangeItem()
        TargetsList.Items.Clear()
        TargetsList.Items.AddRange(Base.Collection.Bases.Select(Function(pb) pb.Directory).ToArray())
        BehaviorsList.Items.Clear()
        BehaviorsList.Items.AddRange(Base.Behaviors.Select(Function(b) b.Name).ToArray())
    End Sub

    Protected Overrides Sub ReparseSource(ByRef parseIssues As ImmutableArray(Of ParseIssue))
        Dim ib As InteractionBase = Nothing
        InteractionBase.TryLoad(Source.Text, ib, parseIssues)
        Edited = ib
    End Sub
End Class

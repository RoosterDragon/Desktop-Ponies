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
        AddHandler TargetsList.ItemCheck, AddressOf TargetsList_ItemCheck
    End Sub

    Protected Overrides Sub ChangeItem()
        TargetsList.SuspendLayout()
        TargetsList.Items.Clear()
        TargetsList.Items.AddRange(Base.Collection.Bases.Select(Function(pb) pb.Directory).ToArray())
        TargetsList.ResumeLayout()

        RebuildBehaviorsList()
    End Sub

    Protected Overrides Sub ReparseSource(ByRef parseIssues As ImmutableArray(Of ParseIssue))
        Dim ib As InteractionBase = Nothing
        InteractionBase.TryLoad(Source.Text, ib, parseIssues)
        Edited = ib
    End Sub

    Private Sub TargetsList_ItemCheck(sender As Object, e As ItemCheckEventArgs)
        RebuildBehaviorsList()

        Dim behaviorNames = New HashSet(Of CaseInsensitiveString)(Edited.BehaviorNames)
        BehaviorsList.SuspendLayout()
        For i = 0 To BehaviorsList.Items.Count - 1
            BehaviorsList.SetItemChecked(i, behaviorNames.Contains(DirectCast(BehaviorsList.Items(i), CaseInsensitiveString)))
        Next
        BehaviorsList.ResumeLayout()
    End Sub

    Private Sub RebuildBehaviorsList()
        Dim behaviors = New SortedSet(Of CaseInsensitiveString)(Edited.BehaviorNames)
        behaviors.UnionWith(Base.Behaviors.Select(Function(b) b.Name))
        For Each targetName In Edited.TargetNames
            Dim targetBase = Base.Collection.Bases.FirstOrDefault(Function(b) b.Directory = targetName)
            If targetBase Is Nothing Then Continue For
            behaviors.UnionWith(targetBase.Behaviors.Select(Function(b) b.Name))
        Next

        BehaviorsList.SuspendLayout()
        BehaviorsList.Items.Clear()
        BehaviorsList.Items.AddRange(behaviors.ToArray())
        BehaviorsList.ResumeLayout()
    End Sub
End Class

Public Class GroupNamesDialog

    Private base As PonyBase

    Public Sub New(base As PonyBase)
        Me.base = Argument.EnsureNotNull(base, "base")
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Private Sub GroupNamesDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BeginInvoke(New MethodInvoker(AddressOf LoadInternal))
    End Sub

    Private Sub LoadInternal()
        Enabled = False
        NamesGrid.SuspendLayout()
        For loopI = 0 To 100
            Dim i = loopI
            Dim name = ""
            If i = Behavior.AnyGroup Then
                name = "Any"
            Else
                Dim group = base.BehaviorGroups.FirstOrDefault(Function(bg) bg.Number = i)
                If group IsNot Nothing Then name = group.Name
            End If
            NamesGrid.Rows.Add(i, name)
            If i = Behavior.AnyGroup Then
                NamesGrid.Rows(i).ReadOnly = True
            End If
        Next
        NamesGrid.ResumeLayout()
        Enabled = True
    End Sub

    Private Sub OK_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles OK_Button.Click
        Dim originalGroups = base.BehaviorGroups.ToImmutableArray()
        base.BehaviorGroups.Clear()
        For Each row As DataGridViewRow In NamesGrid.Rows
            Dim number = DirectCast(row.Cells(colNumber.Index).Value, Integer)
            Dim name = DirectCast(row.Cells(colName.Index).Value, String)
            If number = Behavior.AnyGroup Then Continue For
            If Not String.IsNullOrWhiteSpace(name) Then base.BehaviorGroups.Add(New BehaviorGroup(name, number))
        Next
        Try
            base.Save()
        Catch ex As Exception
            base.BehaviorGroups.Clear()
            base.BehaviorGroups.AddRange(originalGroups)
            MessageBox.Show(Me, "Failed to save. Please try again.", "Save Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancel_Button.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class

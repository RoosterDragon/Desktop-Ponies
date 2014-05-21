Public Class ItemsViewerBase
    Public Class ViewerItemEventArgs
        Inherits EventArgs
        Private ReadOnly _item As IPonyIniSourceable
        Public ReadOnly Property Item As IPonyIniSourceable
            Get
                Return _item
            End Get
        End Property
        Public Sub New(item As IPonyIniSourceable)
            _item = Argument.EnsureNotNull(item, "item")
        End Sub
    End Class

    Public Class ViewerNewItemEventArgs
        Inherits EventArgs
        Private ReadOnly _content As PageContent
        Public ReadOnly Property Content As PageContent
            Get
                Return _content
            End Get
        End Property
        Public Sub New(content As PageContent)
            _content = Argument.EnsureNotNull(content, "content")
        End Sub
    End Class

    Public Event NewRequested As EventHandler(Of ViewerNewItemEventArgs)
    Public Event PreviewRequested As EventHandler(Of ViewerItemEventArgs)
    Protected Overridable Sub OnPreviewRequested(e As ViewerItemEventArgs)
        RaiseEvent PreviewRequested(Me, e)
    End Sub
    Public Event EditRequested As EventHandler(Of ViewerItemEventArgs)
    Protected Overridable Sub OnEditRequested(e As ViewerItemEventArgs)
        RaiseEvent EditRequested(Me, e)
    End Sub

    Protected Function GetItemForRow(rowIndex As Integer) As IPonyIniSourceable
        Return DirectCast(Grid.Rows(rowIndex).Tag, IPonyIniSourceable)
    End Function

    Public Overridable Sub LoadFor(ponyBase As PonyBase)
        Grid.SuspendLayout()
        Grid.Rows.Clear()
        For Each row In GetGridRows(ponyBase)
            Dim index = Grid.Rows.Add(row.Item2)
            Grid.Rows(index).Tag = row.Item1
        Next
        If Grid.SortedColumn IsNot Nothing AndAlso Grid.SortOrder <> SortOrder.None Then
            Grid.Sort(Grid.SortedColumn, If(Grid.SortOrder = SortOrder.Ascending,
                                            System.ComponentModel.ListSortDirection.Ascending,
                                            System.ComponentModel.ListSortDirection.Descending))
        End If
        Grid.ResumeLayout()
    End Sub

    Protected Overridable ReadOnly Property Content As PageContent
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Protected Overridable ReadOnly Property Grid As DataGridView
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Protected Overridable Function GetGridRows(ponyBase As PonyBase) As IEnumerable(Of Tuple(Of IPonyIniSourceable, Object()))
        Throw New NotImplementedException()
    End Function

    Protected Shared Function GetGroupName(ponyBase As PonyBase, groupNumber As Integer) As String
        Argument.EnsureNotNull(ponyBase, "ponyBase")
        If groupNumber = Behavior.AnyGroup Then Return "Any"
        Dim group = ponyBase.BehaviorGroups.FirstOrDefault(Function(bg) bg.Number = groupNumber)
        Return If(group Is Nothing, groupNumber.ToString(Globalization.CultureInfo.CurrentCulture), group.Name.ToString())
    End Function

    Protected Shared Function GetFileNameRelaxed(filePath As String) As String
        If filePath Is Nothing Then
            Return Nothing
        Else
            Try
                Return IO.Path.GetFileName(filePath)
            Catch ex As Exception
                Return filePath
            End Try
        End If
    End Function

    Private Sub NewButton_Click(sender As Object, e As EventArgs) Handles NewButton.Click
        RaiseEvent NewRequested(Me, New ViewerNewItemEventArgs(Content))
    End Sub
End Class

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

    Protected Overridable ReadOnly Property Grid As DataGridView
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Protected Overridable Function GetGridRows(ponyBase As PonyBase) As IEnumerable(Of Tuple(Of IPonyIniSourceable, Object()))
        Throw New NotImplementedException()
    End Function
End Class

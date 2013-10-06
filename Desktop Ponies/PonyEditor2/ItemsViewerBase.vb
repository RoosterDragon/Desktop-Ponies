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

    Public Overridable Sub LoadFor(ponyBase As PonyBase)
        Throw New NotImplementedException()
    End Sub
End Class

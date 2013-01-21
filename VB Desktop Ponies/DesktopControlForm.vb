Imports System.Collections.ObjectModel
Imports CSDesktopPonies.SpriteManagement

Public Class DesktopControlForm
    Private allowClose As Boolean

    Private animator As DesktopPonyAnimator
    Private menuStrips As New List(Of MenuStripAsContextMenu)

#Region "ToolStripItemAsContextMenuItem class"
    Private Class ToolStripItemAsContextMenuItem
        Implements ISimpleContextMenuItem, IDisposable

        Private _menuItem As ToolStripMenuItem
        Private _separatorItem As ToolStripSeparator

        Public Sub New(separator As ToolStripSeparator)
            _separatorItem = separator
        End Sub

        Public Sub New(menuItem As ToolStripMenuItem, activated As EventHandler)
            _menuItem = menuItem
            Me.Activated = activated
        End Sub

        Public Sub New(menuItem As ToolStripMenuItem, subItems As IEnumerable(Of ISimpleContextMenuItem))
            Argument.EnsureNotNull(menuItem, "menuItem")
            Argument.EnsureNotNull(subItems, "subItems")
            If Not menuItem.HasDropDownItems Then
                Throw New ArgumentException("menuItem must have drop down items.", "menuItem")
            End If

            Dim subItemsList As New List(Of ISimpleContextMenuItem)(subItems)

            If menuItem.DropDownItems.Count <> subItemsList.Count Then
                Throw New ArgumentException(
                    "The number of sub-items in menuItem is not the same as the number in the subItems collection.")
            End If

            Dim winFormSubItemsList As New List(Of ISimpleContextMenuItem)(subItemsList.Count)
            Dim index = 0
            For Each _toolStripItem As ToolStripItem In menuItem.DropDownItems
                If subItemsList(index).IsSeparator Then
                    winFormSubItemsList.Add(
                        New ToolStripItemAsContextMenuItem(CType(_toolStripItem, ToolStripSeparator)))
                ElseIf subItemsList(index).SubItems Is Nothing Then
                    winFormSubItemsList.Add(
                        New ToolStripItemAsContextMenuItem(CType(_toolStripItem, ToolStripMenuItem), subItemsList(index).Activated))
                Else
                    winFormSubItemsList.Add(
                        New ToolStripItemAsContextMenuItem(CType(_toolStripItem, ToolStripMenuItem), subItemsList(index).SubItems))
                End If
                index += 1
            Next

            subItems = New ReadOnlyCollection(Of ISimpleContextMenuItem)(winFormSubItemsList)

            _menuItem = menuItem
        End Sub

        Private _handler As EventHandler
        Public Property Activated As System.EventHandler Implements CSDesktopPonies.SpriteManagement.ISimpleContextMenuItem.Activated
            Get
                Return _handler
            End Get
            Set(value As System.EventHandler)
                If _handler IsNot Nothing Then RemoveHandler _menuItem.Click, _handler
                _handler = value
                If _handler IsNot Nothing Then AddHandler _menuItem.Click, _handler
            End Set
        End Property

        Public Property IsSeparator As Boolean Implements CSDesktopPonies.SpriteManagement.ISimpleContextMenuItem.IsSeparator
            Get
                Return _separatorItem IsNot Nothing
            End Get
            Set(value As Boolean)
                If value Then
                    If _separatorItem IsNot Nothing Then _separatorItem.Dispose()
                    _menuItem = New ToolStripMenuItem()
                Else
                    If _menuItem IsNot Nothing Then _menuItem.Dispose()
                    _separatorItem = New ToolStripSeparator()
                End If
            End Set
        End Property

        Public _subItems As System.Collections.ObjectModel.ReadOnlyCollection(Of CSDesktopPonies.SpriteManagement.ISimpleContextMenuItem)
        Public ReadOnly Property SubItems As System.Collections.ObjectModel.ReadOnlyCollection(Of CSDesktopPonies.SpriteManagement.ISimpleContextMenuItem) Implements CSDesktopPonies.SpriteManagement.ISimpleContextMenuItem.SubItems
            Get
                Return _subItems
            End Get
        End Property

        Public Property ISimpleContextMenuItemText As String Implements CSDesktopPonies.SpriteManagement.ISimpleContextMenuItem.Text
            Get
                Return _menuItem.Text
            End Get
            Set(value As String)
                _menuItem.Text = value
            End Set
        End Property

#Region "IDisposable Support"
        Private disposedValue As Boolean
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    If _menuItem IsNot Nothing Then _menuItem.Dispose()
                    If _separatorItem IsNot Nothing Then _separatorItem.Dispose()
                End If
            End If
            Me.disposedValue = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
#End Region

#Region "MenuStripAsContextMenu class"
    Private Class MenuStripAsContextMenu
        Inherits MenuStrip
        Implements ISimpleContextMenu

        Private owner As DesktopControlForm
        Private _items As New List(Of ISimpleContextMenuItem)
        Private _readOnlyItems As New ReadOnlyCollection(Of ISimpleContextMenuItem)(_items)

        Public Sub New(parent As DesktopControlForm, menuItems As IEnumerable(Of ISimpleContextMenuItem))
            Argument.EnsureNotNull(parent, "parent")
            Argument.EnsureNotNull(menuItems, "menuItems")

            LayoutStyle = ToolStripLayoutStyle.VerticalStackWithOverflow
            Dock = DockStyle.Fill

            owner = parent

            For Each _menuItem In menuItems
                Dim winFormMenuItem As ToolStripItem
                Dim toolStripItem As ToolStripItemAsContextMenuItem

                If _menuItem.IsSeparator Then
                    winFormMenuItem = New ToolStripSeparator()
                    toolStripItem = New ToolStripItemAsContextMenuItem(CType(winFormMenuItem, ToolStripSeparator))
                ElseIf _menuItem.SubItems Is Nothing Then
                    winFormMenuItem = New ToolStripMenuItem(_menuItem.Text)
                    toolStripItem = New ToolStripItemAsContextMenuItem(CType(winFormMenuItem, ToolStripMenuItem), _menuItem.Activated)
                Else
                    winFormMenuItem = CreateItemWithSubItems(_menuItem)
                    toolStripItem = New ToolStripItemAsContextMenuItem(CType(winFormMenuItem, ToolStripMenuItem), _menuItem.SubItems)
                End If
                Items.Add(winFormMenuItem)
                _items.Add(toolStripItem)
            Next

            AddHandler Disposed, Sub(sender, e)
                                     For Each item As ToolStripItemAsContextMenuItem In _items
                                         item.Dispose()
                                     Next
                                 End Sub
        End Sub

        Private Function CreateItemWithSubItems(_menuItem As ISimpleContextMenuItem) As ToolStripMenuItem
            Argument.EnsureNotNull(_menuItem, "_menuItem")
            If _menuItem.SubItems Is Nothing OrElse _menuItem.SubItems.Count = 0 Then
                Throw New ArgumentException("_menuItem.SubItems must not be null or empty.")
            End If

            Dim _subItems(_menuItem.SubItems.Count - 1) As ToolStripItem
            For i = 0 To _subItems.Length - 1
                Dim _subItem = _menuItem.SubItems(i)

                Dim winFormMenuItem As ToolStripItem
                If _subItem.IsSeparator Then
                    winFormMenuItem = New ToolStripSeparator()
                ElseIf _subItem.SubItems Is Nothing Then
                    winFormMenuItem = New ToolStripMenuItem(_subItem.Text)
                Else
                    winFormMenuItem = CreateItemWithSubItems(_subItem)
                End If
                _subItems(i) = winFormMenuItem
            Next
            Return New ToolStripMenuItem(_menuItem.Text, Nothing, _subItems)
        End Function

        Public ReadOnly Property ISimpleContextMenuItems As ReadOnlyCollection(Of ISimpleContextMenuItem) Implements ISimpleContextMenu.Items
            Get
                Return _readOnlyItems
            End Get
        End Property

        Public Sub ISimpleContextMenuShow(x As Integer, y As Integer) Implements ISimpleContextMenu.Show
            owner.SmartInvoke(Sub()
                                  owner.MenuStripPanel.SuspendLayout()
                                  owner.MenuStripPanel.Controls.Clear()
                                  owner.MenuStripPanel.Controls.Add(Me)
                                  Show()
                                  owner.MenuStripPanel.ResumeLayout()
                              End Sub)
        End Sub
    End Class
#End Region

    Public Sub New(ponyAnimator As DesktopPonyAnimator)
        InitializeComponent()
        Icon = My.Resources.Twilight
        animator = ponyAnimator
        CreateHandle()
    End Sub

    Public Sub SmartInvoke(method As MethodInvoker)
        Argument.EnsureNotNull(method, "method")
        If InvokeRequired Then
            Invoke(method)
        Else
            method()
        End If
    End Sub

    Public Function CreateContextMenu(menuItems As IEnumerable(Of ISimpleContextMenuItem)) As ISimpleContextMenu
        Dim contextMenu As MenuStripAsContextMenu
        SmartInvoke(Sub() contextMenu = New MenuStripAsContextMenu(Me, menuItems))
        menuStrips.Add(contextMenu)
        Return contextMenu
    End Function

    Private Sub ReturnButton_Click(sender As Object, e As EventArgs) Handles ReturnButton.Click
        Pony.CurrentAnimator.Finish()
        Main.Instance.Invoke(Sub()
                                 Main.Instance.Pony_Shutdown()
                                 Main.Instance.Opacity = 100 'for when autostarted
                                 Main.Instance.Show()
                             End Sub)
        allowClose = True
        Close()
    End Sub

    Public Sub ForceClose()
        allowClose = True
        Close()
    End Sub

    Private Sub DesktopControlForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = Not allowClose
    End Sub

    Private Sub PonyComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles PonyComboBox.SelectedIndexChanged
        Dim selected As ISprite = TryCast(PonyComboBox.SelectedItem, ISprite)
        If selected IsNot Nothing Then
            Dim center = Point.Round(selected.Region.Center())
            animator.Viewer_MouseDown(sender, New SimpleMouseEventArgs(SimpleMouseButtons.Right, center.X, center.Y))
        End If
    End Sub

    Public Sub NotifyRemovedPonyItems()
        If Disposing OrElse IsDisposed Then Return
        If PonyComboBox.SelectedItem Is Nothing Then
            MenuStripPanel.Controls.Clear()
        End If
    End Sub

    Private Sub DesktopControlForm_Disposed(sender As Object, e As EventArgs) Handles MyBase.Disposed
        For Each _menuStrip In menuStrips
            _menuStrip.Dispose()
        Next
    End Sub
End Class
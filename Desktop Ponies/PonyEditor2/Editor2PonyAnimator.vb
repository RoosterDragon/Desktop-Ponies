Imports System.Globalization
Imports DesktopSprites.SpriteManagement

Public Class Editor2PonyAnimator
    Inherits PonyAnimator

    Private ReadOnly pendingAddedNotifications As New Dictionary(Of Pony, Action(Of Pony))()
    Private preview As PonyPreview
    Private editorMenu As ISimpleContextMenu

    Public Sub New(spriteViewer As ISpriteCollectionView, ponyCollection As PonyCollection, ponyContext As PonyContext,
                   preview As PonyPreview)
        MyBase.New(spriteViewer, Nothing, ponyCollection, ponyContext)
        ExitWhenNoSprites = False
        Me.preview = preview
        AddHandler Viewer.InterfaceClosed, Sub() Finish()
        AddHandler Viewer.MouseClick, AddressOf Viewer_MouseClick
        AddHandler SpriteAdded, AddressOf NotifySpriteAdded
    End Sub

    Public Sub AddPonyNotify(pony As Pony, callback As Action(Of Pony))
        pendingAddedNotifications.Add(pony, callback)
        AddPony(pony)
    End Sub

    Private Sub NotifySpriteAdded(sender As Object, e As CollectionItemChangedEventArgs(Of ISprite))
        Dim callback As Action(Of Pony) = Nothing
        Dim pony = TryCast(e.Item, Pony)
        If pony IsNot Nothing AndAlso pendingAddedNotifications.TryGetValue(pony, callback) Then
            pendingAddedNotifications.Remove(pony)
            callback(pony)
        End If
    End Sub

    Public Sub ChangeEditorMenu(base As PonyBase)
        Dim behaviorItems = base.Behaviors.Select(
            Function(b) New SimpleContextMenuItem(b.Name, Sub() preview.SmartInvoke(Sub() preview.RunBehavior(b))))
        If behaviorItems.Any() Then
            Dim menuItems = New LinkedList(Of SimpleContextMenuItem)()
            menuItems.AddLast(New SimpleContextMenuItem("Run Behavior", behaviorItems))
            editorMenu = Viewer.CreateContextMenu(menuItems)
        Else
            editorMenu = Nothing
        End If
    End Sub

    Private Sub Viewer_MouseClick(sender As Object, e As SimpleMouseEventArgs)
        If (e.Buttons And SimpleMouseButtons.Right) = SimpleMouseButtons.Right Then
            If editorMenu IsNot Nothing Then editorMenu.Show(e.X, e.Y)
        End If
    End Sub

    Public Overrides Sub Start()
        preview.AnimatorStart()
        MyBase.Start()
    End Sub

    Public Overrides Sub Pause(hide As Boolean)
        MyBase.Pause(hide)
    End Sub

    Protected Overrides Sub Update()
        MyBase.Update()
        preview.AnimatorUpdate()
    End Sub

    Protected Overrides Sub SynchronizeContext()
        PonyContext.SynchronizeWithGlobalOptionsWithAvoidanceOverrides()
    End Sub
End Class

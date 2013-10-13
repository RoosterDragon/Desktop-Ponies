Imports System.Globalization
Imports DesktopSprites.SpriteManagement

Public Class EditorPonyAnimator
    Inherits PonyAnimator

    Private editor As PonyEditor
    Private editorMenu As ISimpleContextMenu

    Public Sub New(spriteViewer As ISpriteCollectionView, ponyCollection As PonyCollection, editor As PonyEditor)
        MyBase.New(spriteViewer, Nothing, ponyCollection)
        ExitWhenNoSprites = False
        Me.editor = editor
        AddHandler Viewer.InterfaceClosed, Sub() Finish()
        AddHandler Viewer.MouseDown, AddressOf Viewer_MouseDown
        CreateEditorMenu(Nothing)
    End Sub

    Public Sub CreateEditorMenu(base As PonyBase)
        If base Is Nothing OrElse OperatingSystemInfo.IsMacOSX Then
            editorMenu = Nothing
        Else
            Dim menuItems = New LinkedList(Of SimpleContextMenuItem)()
            Dim behaviorItems = base.Behaviors.Select(Function(b) New SimpleContextMenuItem(b.Name, Sub() editor.RunBehavior(b)))
            menuItems.AddLast(New SimpleContextMenuItem("Run Behavior", behaviorItems))
            editorMenu = Viewer.CreateContextMenu(menuItems)
        End If
    End Sub

    Public Overrides Sub Start()
        editor.SmartInvoke(Sub() EvilGlobals.PreviewWindowRectangle =
                               editor.PonyPreviewPanel.RectangleToScreen(editor.PonyPreviewPanel.ClientRectangle))
        MyBase.Start()
    End Sub

    Public Overrides Sub Pause(hide As Boolean)
        If hide Then Draw()
        MyBase.Pause(hide)
    End Sub

    Protected Overrides Sub Update()
        If Not editor.IsClosing Then
            editor.BeginInvoke(New MethodInvoker(
                                Sub()
                                    If editor.IsClosing Then Exit Sub
                                    EvilGlobals.PreviewWindowRectangle =
                                        editor.PonyPreviewPanel.RectangleToScreen(editor.PonyPreviewPanel.ClientRectangle)
                                End Sub))
        End If
        MyBase.Update()
        QueueRemove(AddressOf SpriteIsOldInteractionTarget)
        ProcessQueuedActions()
        If Not editor.IsClosing Then
            editor.BeginInvoke(New MethodInvoker(
                                 Sub()
                                     If editor.IsClosing Then Exit Sub
                                     If editor.PreviewPony Is Nothing Then
                                         editor.CurrentBehaviorValueLabel.Text = ""
                                         editor.GroupValueLabel.Text = ""
                                         editor.TimeLeftValueLabel.Text = ""
                                     Else
                                         If editor.PreviewPony.CurrentBehavior IsNot Nothing Then
                                             editor.CurrentBehaviorValueLabel.Text = editor.PreviewPony.CurrentBehavior.Name
                                         End If
                                         editor.GroupValueLabel.Text =
                                             editor.PreviewPony.CurrentBehaviorGroup & " - " &
                                             editor.PreviewPony.GetBehaviorGroupName(editor.PreviewPony.CurrentBehaviorGroup)
                                         editor.TimeLeftValueLabel.Text =
                                             (editor.PreviewPony.BehaviorDesiredDuration - editor.PreviewPony.CurrentTime).
                                             TotalSeconds.ToString("0.0", CultureInfo.CurrentCulture)
                                     End If
                                 End Sub))
        End If
    End Sub

    Private Function SpriteIsOldInteractionTarget(sprite As ISprite) As Boolean
        Dim pony = TryCast(sprite, Pony)
        Dim result = pony IsNot Nothing AndAlso
            Not Object.ReferenceEquals(pony, editor.PreviewPony) AndAlso
            Not Object.ReferenceEquals(pony, editor.PreviewPony.followTarget)
        If result Then Stop
        Return result
    End Function

    Private Sub Viewer_MouseDown(sender As Object, e As SimpleMouseEventArgs)
        If e.Buttons.HasFlag(SimpleMouseButtons.Right) Then
            If editorMenu IsNot Nothing Then editorMenu.Show(e.X, e.Y)
        End If
    End Sub
End Class

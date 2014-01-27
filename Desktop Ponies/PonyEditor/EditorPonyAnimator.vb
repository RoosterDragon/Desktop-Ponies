Imports System.Globalization
Imports DesktopSprites.SpriteManagement

Public Class EditorPonyAnimator
    Inherits PonyAnimator

    Private editor As PonyEditor
    Private editorMenu As ISimpleContextMenu

    Public Sub New(spriteViewer As ISpriteCollectionView, ponyCollection As PonyCollection, ponyContext As PonyContext,
                   editor As PonyEditor)
        MyBase.New(spriteViewer, Nothing, ponyCollection, ponyContext)
        ExitWhenNoSprites = False
        Me.editor = editor
        AddHandler Viewer.InterfaceClosed, Sub() Finish()
        AddHandler Viewer.MouseClick, AddressOf Viewer_MouseClick
        CreateEditorMenu(Nothing)
    End Sub

    Public Sub CreateEditorMenu(base As PonyBase)
        If base Is Nothing Then
            editorMenu = Nothing
        Else
            Dim behaviorItems = base.Behaviors.Select(Function(b) New SimpleContextMenuItem(b.Name, Sub() editor.RunBehavior(b)))
            If behaviorItems.Any() Then
                Dim menuItems = New LinkedList(Of SimpleContextMenuItem)()
                menuItems.AddLast(New SimpleContextMenuItem("Run Behavior", behaviorItems))
                editorMenu = Viewer.CreateContextMenu(menuItems)
            Else
                editorMenu = Nothing
            End If
        End If
    End Sub

    Public Overrides Sub Start()
        PonyContext.Region = Rectangle.Empty
        editor.SmartInvoke(AddressOf SetBounds)
        MyBase.Start()
    End Sub

    Public Overrides Sub Pause(hide As Boolean)
        MyBase.Pause(hide)
    End Sub

    Protected Overrides Sub Update()
        If Not editor.IsClosing Then
            editor.BeginInvoke(New MethodInvoker(
                                Sub()
                                    If editor.IsClosing Then Exit Sub
                                    SetBounds()
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
                                             editor.PreviewPony.Base.GetBehaviorGroupName(editor.PreviewPony.CurrentBehaviorGroup)
                                         editor.TimeLeftValueLabel.Text =
                                             editor.PreviewPony.BehaviorRemainingDuration.
                                             TotalSeconds.ToString("0.0", CultureInfo.CurrentCulture)
                                     End If
                                 End Sub))
        End If
    End Sub

    Private Sub SetBounds()
        Dim bounds = editor.PonyPreviewPanel.RectangleToScreen(editor.PonyPreviewPanel.ClientRectangle)
        PonyContext.Region = bounds
    End Sub

    Private Function SpriteIsOldInteractionTarget(sprite As ISprite) As Boolean
        Dim pony = TryCast(sprite, Pony)
        Return pony IsNot Nothing AndAlso
            Not Object.ReferenceEquals(pony, editor.PreviewPony) AndAlso
            Not Object.ReferenceEquals(pony, editor.PreviewPony.FollowTarget)
    End Function

    Protected Overrides Sub SynchronizeContext()
        PonyContext.SynchronizeWithGlobalOptionsWithAvoidanceOverrides()
    End Sub

    Private Sub Viewer_MouseClick(sender As Object, e As SimpleMouseEventArgs)
        If (e.Buttons And SimpleMouseButtons.Right) = SimpleMouseButtons.Right Then
            If editorMenu IsNot Nothing Then editorMenu.Show(e.X, e.Y)
        End If
    End Sub
End Class

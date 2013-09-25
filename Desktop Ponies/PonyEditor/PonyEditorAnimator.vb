Imports System.Globalization
Imports DesktopSprites.SpriteManagement

Public Class PonyEditorAnimator
    Inherits DesktopPonyAnimator

    Private _editor As PonyEditor

    Public Sub New(editor As PonyEditor, spriteViewer As ISpriteCollectionView, spriteCollection As IEnumerable(Of ISprite),
                   ponyBaseCollection As IEnumerable(Of PonyBase))
        MyBase.New(spriteViewer, spriteCollection, ponyBaseCollection, Nothing, False)
        ExitWhenNoSprites = False
        _editor = editor
        AddHandler Viewer.InterfaceClosed, Sub(sender As Object, e As EventArgs) Finish()
    End Sub

    Public Overrides Sub Pause(hide As Boolean)
        If hide Then Draw()
        MyBase.Pause(hide)
    End Sub

    Protected Overrides Sub Update()
        MyBase.Update()
        If Not _editor.IsClosing Then
            _editor.BeginInvoke(New MethodInvoker(
                                 Sub()
                                     If _editor.IsClosing Then Exit Sub
                                     If _editor.PreviewPony Is Nothing Then
                                         _editor.CurrentBehaviorValueLabel.Text = ""
                                         _editor.GroupValueLabel.Text = ""
                                         _editor.TimeLeftValueLabel.Text = ""
                                     Else
                                         If _editor.PreviewPony.CurrentBehavior IsNot Nothing Then
                                             _editor.CurrentBehaviorValueLabel.Text = _editor.PreviewPony.CurrentBehavior.Name
                                         End If
                                         _editor.GroupValueLabel.Text =
                                             _editor.PreviewPony.CurrentBehaviorGroup & " - " &
                                             _editor.PreviewPony.GetBehaviorGroupName(_editor.PreviewPony.CurrentBehaviorGroup)
                                         _editor.TimeLeftValueLabel.Text =
                                             (_editor.PreviewPony.BehaviorDesiredDuration - _editor.PreviewPony.CurrentTime).
                                             TotalSeconds.ToString("0.0", CultureInfo.CurrentCulture)
                                     End If
                                 End Sub))
        End If
    End Sub
End Class

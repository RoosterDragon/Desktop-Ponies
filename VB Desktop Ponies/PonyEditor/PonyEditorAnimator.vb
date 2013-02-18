Imports System.Globalization

Public Class PonyEditorAnimator
    Inherits DesktopPonyAnimator

    Private m_editor As PonyEditor

    Public Sub New(editor As PonyEditor, spriteViewer As SpriteManagement.ISpriteCollectionView, spriteCollection As IEnumerable(Of SpriteManagement.ISprite))
        MyBase.New(spriteViewer, spriteCollection, False)
        m_editor = editor

    End Sub

    Protected Overrides Sub Update()
        MyBase.Update()
        If Not m_editor.IsClosing Then
            m_editor.BeginInvoke(Sub()
                                     If m_editor.IsClosing Then Exit Sub
                                     If m_editor.PreviewPony.CurrentBehavior IsNot Nothing Then
                                         m_editor.CurrentBehaviorValueLabel.Text = m_editor.PreviewPony.CurrentBehavior.Name
                                     End If
                                     m_editor.GroupValueLabel.Text =
                                         m_editor.PreviewPony.CurrentBehaviorGroup & " - " &
                                         m_editor.PreviewPony.GetBehaviorGroupName(m_editor.PreviewPony.CurrentBehaviorGroup)
                                     m_editor.TimeLeftValueLabel.Text =
                                         (m_editor.PreviewPony.BehaviorDesiredDuration - m_editor.PreviewPony.CurrentTime).
                                         TotalSeconds.ToString("0.0", CultureInfo.CurrentCulture)
                                 End Sub)
        End If
    End Sub
End Class

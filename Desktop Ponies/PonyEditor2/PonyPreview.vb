Imports DesktopSprites.SpriteManagement
Imports System.Globalization

Public Class PonyPreview
    Private loaded As Boolean
    Private ponies As PonyCollection
    Private editorAnimator As Editor2PonyAnimator
    Private editorInterface As ISpriteCollectionView
    Private previewPony As Pony
    Private previewPonyReady As Boolean
    Private ReadOnly previewPonyGuard As New Object()

    Public Sub New(ponies As PonyCollection)
        Me.ponies = Argument.EnsureNotNull(ponies, "ponies")
        InitializeComponent()
        AddHandler Disposed, Sub()
                                 If editorAnimator IsNot Nothing Then editorAnimator.Finish()
                                 Pony.CurrentAnimator = Nothing
                             End Sub
    End Sub

    Private Sub PonyPreview_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If loaded Then Return
        editorInterface = Options.GetInterface()
        editorInterface.Topmost = True
        editorAnimator = New Editor2PonyAnimator(editorInterface, ponies, Me)
        Pony.CurrentAnimator = editorAnimator
        editorAnimator.Start()
        loaded = True
    End Sub

    Public Sub RestartForPony(base As PonyBase)
        If Not Created Then CreateControl()
        editorAnimator.Pause(True)
        editorAnimator.Clear()
        SyncLock previewPonyGuard
            previewPonyReady = False
            previewPony = New Pony(base)
        End SyncLock
        editorAnimator.AddPonyNotify(previewPony, AddressOf HandleAddedNotification)
        PonyNameValueLabel.Text = base.Directory
        BehaviorNameValueLabel.Text = ""
        TimeLeftValueLabel.Text = ""
        editorAnimator.Resume()
    End Sub

    Private Sub HandleAddedNotification(addedPony As Pony)
        SyncLock previewPonyGuard
            If Object.ReferenceEquals(addedPony, previewPony) Then previewPonyReady = True
        End SyncLock
    End Sub

    Public Sub HidePreview()
        If editorAnimator IsNot Nothing Then editorAnimator.Pause(True)
    End Sub

    Public Sub ShowPreview()
        If editorAnimator IsNot Nothing Then editorAnimator.Resume()
    End Sub

    Public Sub AnimatorStart()
        BeginInvoke(New MethodInvoker(Sub() Pony.PreviewWindowRectangle = RectangleToScreen(PreviewPanel.ClientRectangle)))
    End Sub

    Public Sub AnimatorUpdate()
        BeginInvoke(New MethodInvoker(
            Sub()
                Pony.PreviewWindowRectangle = RectangleToScreen(PreviewPanel.ClientRectangle)
                SyncLock previewPonyGuard
                    If previewPony Is Nothing OrElse Not previewPonyReady Then Return
                    BehaviorNameValueLabel.Text = previewPony.CurrentBehavior.Name
                    TimeLeftValueLabel.Text =
                        (previewPony.BehaviorDesiredDuration - previewPony.CurrentTime).
                        TotalSeconds.ToString("0.0", CultureInfo.CurrentCulture)
                End SyncLock
            End Sub))
    End Sub
End Class

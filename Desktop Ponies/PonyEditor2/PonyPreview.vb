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
    Private ReadOnly parents As New List(Of Control)()
    Private determineLocationOnPaint As Boolean

    Public Sub New(ponies As PonyCollection)
        Me.ponies = Argument.EnsureNotNull(ponies, "ponies")
        InitializeComponent()
        AddHandler Disposed, Sub()
                                 If editorAnimator IsNot Nothing Then editorAnimator.Finish()
                                 EvilGlobals.CurrentAnimator = Nothing
                             End Sub
    End Sub

    Private Sub DetermineParentsAndScreenLocation(sender As Object, e As EventArgs)
        For Each oldParent In parents
            RemoveHandler oldParent.LocationChanged, AddressOf DetermineScreenLocation
            RemoveHandler oldParent.SizeChanged, AddressOf DetermineScreenLocation
            RemoveHandler oldParent.ParentChanged, AddressOf DetermineParentsAndScreenLocation
        Next
        parents.Clear()
        Dim newParent As Control = PreviewPanel
        While newParent IsNot Nothing
            parents.Add(newParent)
            AddHandler newParent.LocationChanged, AddressOf DetermineScreenLocation
            AddHandler newParent.SizeChanged, AddressOf DetermineScreenLocation
            AddHandler newParent.ParentChanged, AddressOf DetermineParentsAndScreenLocation
            newParent = newParent.Parent
        End While
        If TypeOf parents(parents.Count - 1) Is Form Then
            DetermineScreenLocation(Me, EventArgs.Empty)
        Else
            determineLocationOnPaint = True
        End If
    End Sub

    Private Sub DetermineScreenLocation(sender As Object, e As EventArgs)
        Dim bounds = PreviewPanel.RectangleToScreen(PreviewPanel.ClientRectangle)
        EvilGlobals.PreviewWindowRectangle = bounds
        If TypeOf editorInterface Is WinFormSpriteInterface Then
            DirectCast(editorInterface, WinFormSpriteInterface).DisplayBounds = bounds
        End If
    End Sub

    Private Sub PreviewPanel_Paint(sender As Object, e As PaintEventArgs) Handles PreviewPanel.Paint
        If determineLocationOnPaint Then
            DetermineScreenLocation(Me, EventArgs.Empty)
            determineLocationOnPaint = False
        End If
    End Sub

    Private Sub PonyPreview_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If loaded Then Return
        editorInterface = Options.GetInterface()
        editorInterface.Topmost = True
        DetermineParentsAndScreenLocation(Me, EventArgs.Empty)
        editorAnimator = New Editor2PonyAnimator(editorInterface, ponies, Me)
        EvilGlobals.CurrentAnimator = editorAnimator
        editorAnimator.Start()
        loaded = True
    End Sub

    Public Sub RestartForPony(base As PonyBase, Optional startBehavior As Behavior = Nothing)
        If Not Created Then CreateControl()
        editorAnimator.Pause(True)
        editorAnimator.Clear()
        SyncLock previewPonyGuard
            previewPonyReady = False
            previewPony = New Pony(base)
        End SyncLock
        editorAnimator.AddPonyNotify(previewPony, Sub(pony) HandleAddedNotification(pony, startBehavior))
        PonyNameValueLabel.Text = base.Directory
        BehaviorNameValueLabel.Text = ""
        TimeLeftValueLabel.Text = ""
        editorAnimator.Resume()
    End Sub

    Private Sub HandleAddedNotification(addedPony As Pony, startBehavior As Behavior)
        SyncLock previewPonyGuard
            If Object.ReferenceEquals(addedPony, previewPony) Then
                previewPonyReady = True
                editorAnimator.ChangeEditorMenu(previewPony.Base)
                If startBehavior IsNot Nothing Then
                    previewPony.SelectBehavior(startBehavior)
                End If
            End If
        End SyncLock
    End Sub

    Public Sub RunBehavior(behavior As Behavior)
        previewPony.SelectBehavior(behavior)
    End Sub

    Public Sub HidePreview()
        If editorAnimator IsNot Nothing Then editorAnimator.Pause(True)
    End Sub

    Public Sub ShowPreview()
        If editorAnimator IsNot Nothing Then editorAnimator.Resume()
    End Sub

    Public Sub AnimatorStart()
        BeginInvoke(New EventHandler(AddressOf DetermineScreenLocation))
    End Sub

    Public Sub AnimatorUpdate()
        BeginInvoke(New MethodInvoker(
            Sub()
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

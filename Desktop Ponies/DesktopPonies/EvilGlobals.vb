Imports System.IO
Imports DesktopSprites.SpriteManagement

''' <summary>
''' Application global properties.
''' </summary>
''' <remarks>
''' TODO: Remove or relocate these globals as it becomes possible.
''' Also evil: Most everything in Options.
''' </remarks>
Public NotInheritable Class EvilGlobals
    Private Sub New()
    End Sub
    Friend Shared Property Main As MainForm

    Public Shared Property CurrentAnimator As PonyAnimator
    Public Shared Property CurrentViewer As ISpriteCollectionView
    Public Shared Property CurrentGame As Game

    ''' <summary>
    ''' Are ponies currently walking around the desktop?
    ''' </summary>
    Public Shared Property PoniesHaveLaunched As Boolean
    Public Shared Property InScreensaverMode As Boolean

    Private Shared ReadOnly directXSoundAvailableSync As New Object()
    Private Shared _directXSoundAvailable As Boolean?
    Public Shared ReadOnly Property DirectXSoundAvailable As Boolean
        Get
            SyncLock directXSoundAvailableSync
                If Not _directXSoundAvailable.HasValue Then
                    _directXSoundAvailable = IsDirectXSoundAvailable()
                End If
                Return _directXSoundAvailable.Value
            End SyncLock
        End Get
    End Property

    Private Shared Function IsDirectXSoundAvailable() As Boolean
        ' Check to see if the right version of DirectX is installed for sounds.
        Try
            ' You may get a LoaderLock exception here when debugging. It does not occur normally - only under a debugger. Ignoring it
            ' appears to be harmless and the load will not be affected.
            System.Reflection.Assembly.Load(
                "Microsoft.DirectX.AudioVideoPlayback, Version=1.0.2902.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35")
            Return True
        Catch ex As Exception
            ' If we can't load the assembly, just don't enable sound.
        End Try
        Return False
    End Function

    Public Shared Function IsScreensaverExecutable() As Boolean
        Return Environment.GetCommandLineArgs()(0).EndsWith(".scr", PathEquality.Comparison)
    End Function
End Class

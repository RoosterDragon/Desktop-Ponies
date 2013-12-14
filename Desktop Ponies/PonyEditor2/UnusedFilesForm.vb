Imports System.IO

Public Class UnusedFilesForm
    Private ponies As PonyCollection

    Public Sub New(ponyCollection As PonyCollection)
        ponies = Argument.EnsureNotNull(ponyCollection, "ponyCollection")
        InitializeComponent()
        Icon = My.Resources.Twilight
    End Sub

    Private Sub UnusedFilesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Threading.ThreadPool.QueueUserWorkItem(AddressOf ScanForUnusedFiles, New IdleWorker(Me))
    End Sub

    Private Sub ScanForUnusedFiles(workerObject As Object)
        Dim worker As IdleWorker = DirectCast(workerObject, IdleWorker)
        Dim files As New HashSet(Of String)(PathEquality.Comparer)
        Dim unusedFiles As New List(Of String)()
        Dim baseRootDirectory = Path.Combine(EvilGlobals.InstallLocation, PonyBase.RootDirectory)
        For Each base In ponies.Bases
            unusedFiles.Clear()
            Dim basePath = Path.Combine(baseRootDirectory, base.Directory)
            files.UnionWith(Directory.GetFiles(basePath, "*.gif"))
            files.UnionWith(Directory.GetFiles(basePath, "*.png"))
            files.UnionWith(Directory.GetFiles(basePath, "*.mp3"))
            For Each behavior In base.Behaviors
                files.Remove(behavior.LeftImage.Path)
                files.Remove(behavior.RightImage.Path)
            Next
            For Each effect In base.Effects
                files.Remove(effect.LeftImage.Path)
                files.Remove(effect.RightImage.Path)
            Next
            For Each speech In base.Speeches
                If speech.SoundFile IsNot Nothing Then files.Remove(speech.SoundFile)
            Next
            unusedFiles.AddRange(files)
        Next
        unusedFiles.Sort(PathEquality.Comparer)
        worker.QueueTask(Sub()
                             UnusedFilesTextBox.SuspendLayout()
                             For Each unusedFile In unusedFiles
                                 UnusedFilesTextBox.AppendText(unusedFile.Replace(baseRootDirectory, "") & vbNewLine)
                             Next
                             UnusedFilesTextBox.ResumeLayout()
                             StatusLabel.Text = "Found " & unusedFiles.Count & " unused files."
                         End Sub)
    End Sub
End Class
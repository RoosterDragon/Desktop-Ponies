Imports System.IO
Imports System.Threading
Imports DesktopSprites.SpriteManagement

Public Class UnscalableImagesForm
    Private ReadOnly ponies As PonyCollection
    Private ReadOnly unscalableImages As New List(Of Tuple(Of String, Long))()

    Public Sub New(ponyCollection As PonyCollection)
        ponies = Argument.EnsureNotNull(ponyCollection, "ponyCollection")
        InitializeComponent()
        Icon = My.Resources.Twilight
        IgnoreListTextBox.Text = IgnoreListTextBox.Text.Replace("\"c, Path.DirectorySeparatorChar)
    End Sub

    Private Sub UnscalableImagesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        BeginInvoke(New MethodInvoker(AddressOf LoadInternal))
    End Sub

    Private Sub LoadInternal()
        ThreadPool.QueueUserWorkItem(
            Sub()
                Dim basePath = PonyBase.RootDirectory & Path.DirectorySeparatorChar
                Dim imagePaths = New HashSet(Of String)(
                        ponies.Bases.SelectMany(
                            Function(pb) pb.Behaviors).SelectMany(
                            Function(b) New String() {b.LeftImage.Path, b.RightImage.Path}).Concat(
                            ponies.Bases.SelectMany(
                                Function(pb) pb.Effects).SelectMany(
                                Function(e) New String() {e.LeftImage.Path, e.RightImage.Path})).Where(
                            Function(imagePath) PathEquality.Equals(Path.GetExtension(imagePath), ".gif")),
                        PathEquality.Comparer)
                Dim count = 0
                For Each imagePath In imagePaths
                    Dim downscales = True
                    Dim bufferConverter As New BufferToImage(Of Bitmap)(
                        Function(buffer() As Byte, palette() As RgbColor, transparentIndex As Byte?,
                                 stride As Integer, width As Integer, height As Integer, depth As Byte)
                            Dim bitmap As Bitmap = Nothing
                            If downscales Then
                                Dim nativeSize = New Size(width, height)
                                If nativeSize <> New Size(1, 1) Then
                                    bitmap = GifImage.BufferToImageOfBitmap()(
                                        buffer, palette, transparentIndex, stride, width, height, depth)
                                    GifProcessing.LosslessDownscale(buffer, palette, transparentIndex, stride, width, height, depth)
                                    downscales = nativeSize <> New Size(width, height)
                                End If
                            End If
                            Return bitmap
                        End Function)
                    Using fileStream = File.OpenRead(imagePath)
                        Try
                            Dim image = New GifImage(Of Bitmap)(fileStream, bufferConverter, GifImage.AllowableDepthsForBitmap)
                            If Not downscales Then
                                unscalableImages.Add(Tuple.Create(
                                                     imagePath.Replace(basePath, ""),
                                                     New FileInfo(imagePath).Length))
                            End If
                        Catch ex As Exception
                            ' Ignore exceptions from trying to generate this list.
                        End Try
                    End Using
                    count += 1
                    If Disposing OrElse IsDisposed Then Return
                    If count Mod 10 = 0 Then SmartInvoke(Sub() CountLabel.Text = "Working... " & count & "/" & imagePaths.Count)
                Next
                unscalableImages.TrimExcess()
                If Disposing OrElse IsDisposed Then Return
                SmartInvoke(Sub()
                                UpdateFilter()
                                IgnoreListTextBox.Enabled = True
                                FilterButton.Enabled = True
                            End Sub)
            End Sub)
    End Sub

    Private Sub UpdateFilter()
        ImagesGrid.SuspendLayout()
        ImagesGrid.Rows.Clear()
        Dim ignoreLines = IgnoreListTextBox.Text.Split(New String() {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries)
        Dim unscalableImagesToDisplay = unscalableImages.Where(
            Function(ui) Not ignoreLines.Any(Function(line) ui.Item1.StartsWith(line, PathEquality.Comparison))).ToArray()
        For Each unscalableImage In unscalableImagesToDisplay
            ImagesGrid.Rows.Add(unscalableImage.Item1, unscalableImage.Item2)
        Next
        ImagesGrid.Sort(colImagePath, System.ComponentModel.ListSortDirection.Ascending)
        ImagesGrid.ResumeLayout()
        CountLabel.Text = "Showing " & unscalableImagesToDisplay.Length & " of " & unscalableImages.Count & " unscalable images."
    End Sub

    Private Sub FilterButton_Click(sender As Object, e As EventArgs) Handles FilterButton.Click
        UpdateFilter()
    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub
End Class

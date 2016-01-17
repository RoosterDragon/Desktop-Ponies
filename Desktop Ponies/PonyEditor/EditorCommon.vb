Imports System.IO

Friend Class EditorCommon
    Private Sub New()
    End Sub
    Public Shared Function ValidateName(parent As IWin32Window, itemName As String, newName As String) As Boolean
        If newName = "" Then
            MessageBox.Show(parent, "You must enter a name for the " & itemName & ".",
                            "No Name Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        If newName.IndexOfAny({""""c, ","c, "{"c, "}"c}) <> -1 Then
            MessageBox.Show(parent, "The name for the " & itemName & " cannot contain a quote(""), comma (,) or curly braces ({}). " &
                            "Please select another name.",
                            "Invalid Name Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return False
        End If

        Return True
    End Function
    Public Shared Function ValidateName(parent As IWin32Window, itemName As String, newName As String,
                                        items As IEnumerable(Of IPonyIniSerializable)) As Boolean
        Return ValidateName(parent, itemName, newName, items, Nothing)
    End Function
    Public Shared Function ValidateName(parent As IWin32Window, itemName As String, newName As String,
                                        items As IEnumerable(Of IPonyIniSerializable), oldName As CaseInsensitiveString) As Boolean
        If Not ValidateName(parent, itemName, newName) Then Return False

        For Each item In items
            If item.Name = newName AndAlso item.Name <> oldName Then
                MessageBox.Show(parent, "Another " & itemName & " with this name already exists! " &
                                "Please select another name or rename the other " & itemName & " first.",
                                "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return False
            End If
        Next

        Return True
    End Function
    Public Shared Function SetImage(editor As PonyEditor, parent As IWin32Window,
                                    box As PictureBox, durationCallback As Action(Of Decimal)) As String
        Dim path = PromptUserForImagePath(parent, editor.CurrentBase)
        If path Is Nothing Then Return Nothing

        If box.Image IsNot Nothing Then box.Image.Dispose()
        box.Image = Image.FromFile(path)

        If PathEquality.Equals(IO.Path.GetExtension(path), ".gif") Then
            Dim runtime = GetGifTotalRuntime(box.Image)
            If runtime <> 0 Then durationCallback(runtime)
        End If

        Return path
    End Function
    Private Shared Function GetGifTotalRuntime(image As Image) As Decimal
        Try
            Dim dimension = New Imaging.FrameDimension(image.FrameDimensionsList(0))
            Dim frameCount = image.GetFrameCount(dimension)

            Const PropertyTagFrameDelay As Integer = &H5100 ' From gdiplugimaging.h
            Dim frameDelaysItem = image.GetPropertyItem(PropertyTagFrameDelay)
            Dim frameDelaysItemValue = frameDelaysItem.Value

            Dim delays(frameCount) As Integer
            For frame = 0 To frameCount - 1
                delays(frame) = BitConverter.ToInt32(frameDelaysItemValue, frame * 4)
            Next

            Dim totalDelay = 0
            For Each delay In delays
                totalDelay += delay
            Next

            Return totalDelay / 100D
        Catch ex As Exception
            ' Could not get timing info.
            Return 0
        End Try
    End Function
    Public Shared Function PromptUserForImagePath(owner As IWin32Window, base As PonyBase, Optional text As String = Nothing) As String
        Dim imagePath As String
        Using dialog = New OpenFileDialog()
            dialog.Filter =
                "GIF Files (*.gif)|*.gif|PNG Files (*.png)|*.png|JPG Files (*.jpg;*.jpeg)|*.jpg;*.jpeg" &
                "|All Image Files|*.gif;*.png;*.jpg;*.jpeg"
            dialog.FilterIndex = 4
            dialog.InitialDirectory = Path.GetFullPath(Path.Combine(PonyBase.RootDirectory, base.Directory))

            If text Is Nothing Then
                dialog.Title = "Select picture..."
            Else
                dialog.Title = "Select picture for: " & text
            End If


            If dialog.ShowDialog(owner) = DialogResult.OK Then
                imagePath = dialog.FileName
            Else
                Return Nothing
            End If
        End Using

        Try
            ' Try to load this image to ensure it is valid.
            Image.FromFile(imagePath).Dispose()
        Catch ex As Exception
            Program.NotifyUserOfNonFatalException(ex, "This file does not appear to be a valid image. Please choose another file.")
            Return Nothing
        End Try

        Return CopyFileLocallyAndGetPath(owner, base, imagePath)
    End Function
    Public Shared Function PromptUserForSoundPath(owner As IWin32Window, base As PonyBase) As String
        Dim soundPath As String
        Using dialog = New OpenFileDialog()
            dialog.Filter = "MP3 Files (*.mp3)|*.mp3"
            dialog.InitialDirectory = Path.GetFullPath(Path.Combine(PonyBase.RootDirectory, base.Directory))
            If dialog.ShowDialog(owner) = DialogResult.OK Then
                soundPath = dialog.FileName
            Else
                Return Nothing
            End If
        End Using

        Return CopyFileLocallyAndGetPath(owner, base, soundPath)
    End Function
    Private Shared Function CopyFileLocallyAndGetPath(owner As IWin32Window, base As PonyBase, absoluteSourcePath As String) As String
        ' If the file is from elsewhere on the file system, we will need to make a local copy.
        Dim desiredPath = Path.Combine(PonyBase.RootDirectory, base.Directory, Path.GetFileName(absoluteSourcePath))
        If Not PathEquality.Equals(absoluteSourcePath, Path.GetFullPath(desiredPath)) Then
            If File.Exists(desiredPath) Then
                MessageBox.Show(owner, "The pony already has a file with this name. The file you want to use cannot be " &
                                "copied to the pony directory because of this. Try renaming one of the files.",
                                "Duplicate Name", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return Nothing
            End If
            Try
                My.Computer.FileSystem.CopyFile(absoluteSourcePath, desiredPath, True)
            Catch ex As Exception
                Program.NotifyUserOfNonFatalException(ex, "Couldn't copy the file to the pony directory. Please try again.")
                Return Nothing
            End Try
        End If
        Return desiredPath
    End Function
End Class

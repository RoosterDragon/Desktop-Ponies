Imports System.ComponentModel

Public Class FileSelector
    <Description("Occurs when the displayed list of files is refreshed.")>
    Public Event ListRefreshed As EventHandler
    <Description("Occurs when the value of FilePath changes.")>
    Public Event FilePathSelected As EventHandler

    Private _baseDirectory As String
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property BaseDirectory As String
        Get
            Return _baseDirectory
        End Get
    End Property
    Private _searchPatterns As String()
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public ReadOnly Property SearchPatterns As String()
        Get
            Return _searchPatterns
        End Get
    End Property
    Private previousFilePath As String

    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Public Property FilePath As String
        Get
            Return If(FilePathComboBox.SelectedIndex = 0, Nothing, FilePathComboBox.Text)
        End Get
        Set(value As String)
            If value Is Nothing Then
                FilePathComboBox.SelectedIndex = 0
            Else
                Dim i = 0
                For Each item As String In FilePathComboBox.Items
                    If String.Equals(value, item, PathEquality.Comparison) Then
                        FilePathComboBox.SelectedIndex = i
                        Exit For
                    End If
                    i += 1
                Next
            End If
        End Set
    End Property

    Public Sub New()
        InitializeComponent()
        FilePathComboBox.SelectedIndex = 0
    End Sub

    Public Sub InitializeFromDirectory(baseDirectory As String, ParamArray searchPatterns As String())
        previousFilePath = Nothing
        _baseDirectory = Argument.EnsureNotNull(baseDirectory, "baseDirectory")
        _searchPatterns = Argument.EnsureNotNull(searchPatterns, "searchPatterns")
        FilePathComboBox.BeginUpdate()
        FilePathComboBox.Items.Clear()
        FilePathComboBox.Items.Add("[None]")
        Dim fileNames As New List(Of String)()
        For Each fileExtension In _searchPatterns
            fileNames.AddRange(IO.Directory.GetFiles(_baseDirectory, fileExtension).Select(Function(s) IO.Path.GetFileName(s)))
        Next
        fileNames.Sort(StringComparer.OrdinalIgnoreCase)
        FilePathComboBox.Items.AddRange(fileNames.ToArray())
        FilePathComboBox.EndUpdate()
        RaiseEvent ListRefreshed(Me, EventArgs.Empty)
    End Sub

    Public Sub ReinitializeFromCurrentDirectory()
        If _baseDirectory IsNot Nothing Then InitializeFromDirectory(_baseDirectory, _searchPatterns)
    End Sub

    Private Sub FilePathComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles FilePathComboBox.SelectedIndexChanged
        If previousFilePath <> FilePath Then
            previousFilePath = FilePath
            FilePathDeleteButton.Enabled = FilePathComboBox.SelectedIndex > 0
            RaiseEvent FilePathSelected(Me, EventArgs.Empty)
        End If
    End Sub

    Private Sub FilePathChooseButton_Click(sender As Object, e As EventArgs) Handles FilePathChooseButton.Click
        Using dialog As New OpenFileDialog()
            dialog.InitialDirectory = _baseDirectory
            If dialog.ShowDialog(Me.ParentForm) = DialogResult.OK Then
                Dim destinationFileName = IO.Path.GetFileName(dialog.FileName)
                If IO.Path.GetDirectoryName(dialog.FileName) = BaseDirectory Then
                    FilePathComboBox.SelectedItem = destinationFileName
                    Return
                End If
                Dim tryCopy = False
                Dim overwrite = False
                If items.Contains(destinationFileName, StringComparer.OrdinalIgnoreCase) Then
                    If MessageBox.Show(Me, "The file you have chosen has the same name as a file in the target directory. Overwrite it?",
                                    "Replace File?", MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.OK Then
                        tryCopy = True
                        overwrite = True
                    End If
                Else
                    tryCopy = True
                End If
                If tryCopy Then
                    Dim copied = False
                    Dim destinationFilePath = IO.Path.Combine(BaseDirectory, destinationFileName)
                    Try
                        IO.File.Copy(dialog.FileName, destinationFilePath, overwrite)
                        copied = True
                    Catch ex As Exception
                        My.Application.NotifyUserOfNonFatalException(
                            ex, "Could not copy file from '" & dialog.FileName & "' to '" & destinationFilePath & "'")
                    End Try
                    If copied Then
                        ReinitializeFromCurrentDirectory()
                        FilePathComboBox.SelectedItem = destinationFileName
                    End If
                End If
            End If
        End Using
    End Sub

    Private Sub FilePathDeleteButton_Click(sender As Object, e As EventArgs) Handles FilePathDeleteButton.Click
        If MessageBox.Show(Me, "Are you sure you want to delete '" & FilePath & "'?",
                           "Delete File?", MessageBoxButtons.YesNo,
                           MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = DialogResult.Yes Then
            Dim deleted = False
            Dim fullPath = IO.Path.Combine(BaseDirectory, FilePath)
            Try
                IO.File.Delete(fullPath)
                deleted = True
            Catch ex As Exception
                My.Application.NotifyUserOfNonFatalException(ex, "Could not delete file '" & fullPath & "'")
            End Try
            If deleted Then
                ReinitializeFromCurrentDirectory()
                FilePathComboBox.SelectedIndex = 0
            End If
        End If
    End Sub

    Private ReadOnly Property items As IEnumerable(Of String)
        Get
            Return FilePathComboBox.Items.Cast(Of String)().Skip(1)
        End Get
    End Property
End Class

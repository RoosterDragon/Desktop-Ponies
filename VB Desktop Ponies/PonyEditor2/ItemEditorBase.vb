Imports CSDesktopPonies.SpriteManagement
Imports System.IO

Public Class ItemEditorBase

    Private ReadOnly idleFocusControl As New Control(Me, Nothing)
    Private ReadOnly idleWorker As IdleWorker = idleWorker.CurrentThreadWorker

    Private _base As PonyBase
    Protected ReadOnly Property Base As PonyBase
        Get
            Return _base
        End Get
    End Property
    Private _ponyBasePath As String
    Protected ReadOnly Property PonyBasePath As String
        Get
            Return _ponyBasePath
        End Get
    End Property
    Private _isNewItem As Boolean = True
    Protected ReadOnly Property IsNewItem As Boolean
        Get
            Return _isNewItem
        End Get
    End Property
    Private _itemChanged As Boolean
    Protected ReadOnly Property ItemChanged As Boolean
        Get
            Return _itemChanged
        End Get
    End Property

    Public Overridable Sub NewItem(ponyBase As PonyBase)
        Argument.EnsureNotNull(ponyBase, "ponyBase")
        _ponyBasePath = Path.Combine(Options.InstallLocation, ponyBase.RootDirectory, ponyBase.Directory)
        _base = ponyBase
    End Sub

    Public Overridable Sub LoadItem(ponyBase As PonyBase, name As String)
        NewItem(ponyBase)
        _isNewItem = False
    End Sub

    Public Overridable Sub SaveItem()
        _isNewItem = False
        _itemChanged = False
        UpdateDirtyFlag(_itemChanged)
    End Sub

    Protected Overridable Sub Property_ValueChanged(sender As Object, e As EventArgs)
        _itemChanged = True
        UpdateDirtyFlag(_itemChanged)
    End Sub

    Private Sub UpdateDirtyFlag(dirty As Boolean)
        DirectCast(Parent, ItemTabPage).IsDirty = dirty
    End Sub

    Protected Sub ReplaceItemsInComboBox(comboBox As ComboBox, items As Object(), includeNoneOption As Boolean)
        Argument.EnsureNotNull(comboBox, "comboBox")
        comboBox.BeginUpdate()
        comboBox.Items.Clear()
        If includeNoneOption Then comboBox.Items.Add("[None]")
        comboBox.Items.AddRange(items)
        comboBox.EndUpdate()
    End Sub

    Protected Sub SelectItemElseNoneOption(comboBox As ComboBox, item As Object)
        Argument.EnsureNotNull(comboBox, "comboBox")
        comboBox.SelectedItem = item
        If comboBox.SelectedIndex = -1 Then comboBox.SelectedIndex = 0
    End Sub

    Protected Sub LoadNewImageForViewer(selector As FileSelector, viewer As AnimatedImageViewer)
        Argument.EnsureNotNull(selector, "selector")
        Argument.EnsureNotNull(viewer, "viewer")
        selector.Enabled = False
        selector.UseWaitCursor = True
        viewer.UseWaitCursor = True
        idleFocusControl.Focus()
        Dim filePath = selector.FilePath
        Dim newImage As AnimatedImage(Of BitmapFrame) = Nothing
        Threading.ThreadPool.QueueUserWorkItem(
            Sub()
                Dim ex As Exception = Nothing
                If filePath IsNot Nothing Then
                    Try
                        newImage = BitmapFrame.AnimationFromFile(Path.Combine(PonyBasePath, filePath))
                    Catch e As Exception
                        ex = e
                    End Try
                End If
                idleWorker.QueueTask(
                    Sub()
                        If filePath <> selector.FilePath Then
                            ' The file path was changed whilst we were generating the image.
                            If newImage IsNot Nothing Then newImage.Dispose()
                        Else
                            If viewer.Image IsNot Nothing Then
                                viewer.Image.Dispose()
                            End If
                            viewer.Image = newImage

                            If ex IsNot Nothing Then
                                viewer.ShowError("There was an error attempting to display this image." & vbNewLine & vbNewLine &
                                                 ex.GetType().ToString() & vbNewLine & ex.Message)
                            Else
                                viewer.ClearError()
                            End If

                            viewer.UseWaitCursor = False
                            selector.UseWaitCursor = False
                            selector.Enabled = True
                            If Object.ReferenceEquals(ActiveControl, idleFocusControl) Then
                                selector.Focus()
                            End If
                        End If
                    End Sub)
            End Sub)
    End Sub

    Protected Sub LoadNewImageForViewer(selector As FileSelector, viewer As EffectImageViewer, behaviorImagePath As String)
        Argument.EnsureNotNull(selector, "selector")
        Argument.EnsureNotNull(viewer, "viewer")
        selector.Enabled = False
        selector.UseWaitCursor = True
        viewer.UseWaitCursor = True
        idleFocusControl.Focus()
        Dim filePath = selector.FilePath
        Dim newImage As AnimatedImage(Of BitmapFrame) = Nothing
        Dim behaviorImage As AnimatedImage(Of BitmapFrame) = Nothing
        Threading.ThreadPool.QueueUserWorkItem(
            Sub()
                Dim ex As Exception = Nothing
                Try
                    If filePath IsNot Nothing Then
                        newImage = BitmapFrame.AnimationFromFile(Path.Combine(PonyBasePath, filePath))
                    End If
                    If behaviorImagePath IsNot Nothing Then
                        behaviorImage = BitmapFrame.AnimationFromFile(Path.Combine(PonyBasePath, behaviorImagePath))
                    End If
                Catch e As Exception
                    ex = e
                End Try
                idleWorker.QueueTask(
                    Sub()
                        If filePath <> selector.FilePath Then
                            ' The file path was changed whilst we were generating the image.
                            If newImage IsNot Nothing Then newImage.Dispose()
                            If behaviorImage IsNot Nothing Then behaviorImage.Dispose()
                        Else
                            If viewer.Image IsNot Nothing Then
                                viewer.Image.Dispose()
                            End If
                            viewer.Image = behaviorImage
                            If viewer.EffectImage IsNot Nothing Then
                                viewer.EffectImage.Dispose()
                            End If
                            viewer.EffectImage = newImage

                            If ex IsNot Nothing Then
                                viewer.ShowError("There was an error attempting to display this image." & vbNewLine & vbNewLine &
                                                 ex.GetType().ToString() & vbNewLine & ex.Message)
                            Else
                                viewer.ClearError()
                            End If

                            viewer.UseWaitCursor = False
                            selector.UseWaitCursor = False
                            selector.Enabled = True
                            If Object.ReferenceEquals(ActiveControl, idleFocusControl) Then
                                selector.Focus()
                            End If
                        End If
                    End Sub)
            End Sub)
    End Sub
End Class

Imports CSDesktopPonies.SpriteManagement
Imports System.IO

Public Class ItemEditorBase

    Private ReadOnly idleFocusControl As New Control(Me, Nothing)
    Private ReadOnly worker As IdleWorker = IdleWorker.CurrentThreadWorker

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
    Public ReadOnly Property IsNewItem As Boolean
        Get
            Return _isNewItem
        End Get
    End Property
    Private _isItemDirty As Boolean
    Public ReadOnly Property IsItemDirty As Boolean
        Get
            Return _isItemDirty
        End Get
    End Property
    Private _loadingItem As Boolean
    ''' <summary>
    ''' Gets or sets a value indicating whether an item is being loaded, in which case property updates are ignored.
    ''' </summary>
    Protected Property LoadingItem As Boolean
        Get
            Return _loadingItem
        End Get
        Set(value As Boolean)
            _loadingItem = value
            UseWaitCursor = _loadingItem
        End Set
    End Property

    Public Event DirtinessChanged As EventHandler

    Public Overridable Sub NewItem(ponyBase As PonyBase)
        Argument.EnsureNotNull(ponyBase, "ponyBase")
        _ponyBasePath = Path.Combine(Options.InstallLocation, ponyBase.RootDirectory, ponyBase.Directory)
        _base = ponyBase
        Enabled = True
    End Sub

    Public Overridable Sub LoadItem(ponyBase As PonyBase, name As String)
        NewItem(ponyBase)
        _isNewItem = False
    End Sub

    Public Overridable Sub SaveItem()
        Dim result = DialogResult.None
        Do
            Try
                Base.Save()
                _isNewItem = False
                UpdateDirtyFlag(False)
            Catch ex As IOException
                result = MessageBox.Show(Me, "There was an error attempting to save the pony." & vbNewLine &
                                         vbNewLine & ex.Message & vbNewLine & vbNewLine &
                                         "Retry?", "Save Error", MessageBoxButtons.RetryCancel,
                                         MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            End Try
        Loop While result = DialogResult.Retry
    End Sub

    Protected Overridable Sub OnItemPropertyChanged()
        UpdateDirtyFlag(True)
    End Sub

    Public Overridable Sub AnimateImages(animate As Boolean)
    End Sub

    Private Sub UpdateDirtyFlag(newState As Boolean)
        If _isItemDirty = newState Then Return
        _isItemDirty = newState
        RaiseEvent DirtinessChanged(Me, EventArgs.Empty)
        DirectCast(Parent, ItemTabPage).IsDirty = newState
    End Sub

    Protected Sub UpdateProperty(handler As Action)
        If LoadingItem Then Return
        handler()
        OnItemPropertyChanged()
    End Sub

    Protected Shared Sub ReplaceItemsInComboBox(comboBox As ComboBox, items As Object(), includeNoneOption As Boolean)
        Argument.EnsureNotNull(comboBox, "comboBox")
        comboBox.BeginUpdate()
        comboBox.Items.Clear()
        If includeNoneOption Then comboBox.Items.Add("[None]")
        comboBox.Items.AddRange(items)
        comboBox.EndUpdate()
    End Sub

    Protected Shared Sub SelectItemElseNoneOption(comboBox As ComboBox, item As Object)
        Argument.EnsureNotNull(comboBox, "comboBox")
        comboBox.SelectedItem = item
        If comboBox.SelectedIndex = -1 Then comboBox.SelectedIndex = 0
    End Sub

    Protected Shared Sub SelectItemElseAddItem(comboBox As ComboBox, item As Object)
        Argument.EnsureNotNull(comboBox, "comboBox")
        comboBox.SelectedItem = item
        If comboBox.SelectedIndex = -1 Then
            comboBox.Items.Add(item)
            comboBox.SelectedItem = item
        End If
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
                worker.QueueTask(
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
                worker.QueueTask(
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

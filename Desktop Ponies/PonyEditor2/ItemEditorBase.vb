Imports DesktopSprites.SpriteManagement
Imports System.IO

Public Class ItemEditorBase
    Private ReadOnly worker As IdleWorker = IdleWorker.CurrentThreadWorker
    Private ReadOnly idleFocusControl As New Control(Me, Nothing) With {.TabStop = False}
    Private lastFocusedControl As Control

    Protected Property Original As IPonyIniSourceable
    Protected Property Edited As IPonyIniSourceable
    Public ReadOnly Property Item As IPonyIniSourceable
        Get
            Return Original
        End Get
    End Property
    Protected Overridable ReadOnly Property Collection As System.Collections.IList
        Get
            Throw New NotImplementedException()
        End Get
    End Property
    Protected Overridable ReadOnly Property ItemTypeName As String
        Get
            Throw New NotImplementedException()
        End Get
    End Property

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

    Protected Property ParseIssues As ImmutableArray(Of ParseIssue)
    Protected Property ReferentialIssues As ImmutableArray(Of ParseIssue)
    Public Overridable ReadOnly Property Issues As IEnumerable(Of ParseIssue)
        Get
            Return If(ParseIssues, Linq.Enumerable.Empty(Of ParseIssue)()).Concat(
                If(ReferentialIssues, Linq.Enumerable.Empty(Of ParseIssue)()))
        End Get
    End Property
    Public Event IssuesChanged As EventHandler
    Protected Overridable Sub OnIssuesChanged(e As EventArgs)
        RaiseEvent IssuesChanged(Me, e)
    End Sub

    Public Event DirtinessChanged As EventHandler

    Private Sub SetupItem(ponyBase As PonyBase)
        Argument.EnsureNotNull(ponyBase, "ponyBase")
        _ponyBasePath = Path.Combine(Options.InstallLocation, ponyBase.RootDirectory, ponyBase.Directory)
        _base = ponyBase
        Enabled = True
    End Sub

    Public Sub NewItem(ponyBase As PonyBase, name As String)
        SetupItem(ponyBase)
        LoadingItem = True
        NewItem(name)
        LoadingItem = False
    End Sub

    Public Overridable Sub NewItem(name As String)
        Throw New NotImplementedException()
    End Sub

    Public Sub LoadItem(ponyBase As PonyBase, item As IPonyIniSourceable)
        Argument.EnsureNotNull(item, "item")
        _isNewItem = False
        SetupItem(ponyBase)
        LoadingItem = True
        Original = item
        Edited = DirectCast(item.MemberwiseClone(), IPonyIniSourceable)
        LoadItem()
        Source.Text = Edited.SourceIni
        LoadingItem = False
    End Sub

    Public Overridable Sub LoadItem()
        Throw New NotImplementedException()
    End Sub

    Public Sub SaveItem()
        If Collection.Cast(Of IPonyIniSourceable).Any(Function(item) item.Name = Edited.Name AndAlso Not Object.ReferenceEquals(item, Original)) Then
            MessageBox.Show(Me, "A " & ItemTypeName & " with the name '" & Edited.Name &
                            "' already exists for this pony. Please choose another name.",
                            "Name Not Unique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        If Original.Name <> Edited.Name Then
            If MessageBox.Show(Me, "Changing the name of this " & ItemTypeName & " will break other references. Continue with save?",
                               "Rename?", MessageBoxButtons.YesNo,
                               MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                Return
            End If
        End If

        Dim index = Collection.IndexOf(Original)
        Collection(index) = Edited

        Dim result = DialogResult.None
        Do
            Try
                Edited.SourceIni = Source.Text
                Base.Save()
                _isNewItem = False
                UpdateDirtyFlag(False)
            Catch ex As IOException
                result = MessageBox.Show(Me, "There was an error attempting to save the pony." & Environment.NewLine &
                                         Environment.NewLine & ex.Message & Environment.NewLine & Environment.NewLine &
                                         "Retry?", "Save Error", MessageBoxButtons.RetryCancel,
                                         MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            End Try
        Loop While result = DialogResult.Retry

        Original = Edited
        Edited = DirectCast(Original.MemberwiseClone(), IPonyIniSourceable)
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
        Argument.EnsureNotNull(handler, "handler")
        If LoadingItem Then Return
        handler()
        OnItemPropertyChanged()
    End Sub

    Protected Overridable Sub OnItemPropertyChanged()
        UpdateDirtyFlag(True)
        Source.Text = Edited.GetPonyIni()
    End Sub

    Private Sub Source_TextChanged(sender As Object, e As EventArgs) Handles Source.TextChanged
        SourceTextTimer.Stop()
        SourceTextTimer.Start()
    End Sub

    Private Sub SourceTextTimer_Tick(sender As Object, e As EventArgs) Handles SourceTextTimer.Tick
        SourceTextTimer.Stop()
        LoadingItem = True
        SourceTextChanged()
        UpdateDirtyFlag(True)
        LoadingItem = False
    End Sub

    Protected Overridable Sub SourceTextChanged()
        Throw New NotImplementedException()
    End Sub

    Protected Shared Sub ReplaceItemsInComboBox(comboBox As ComboBox, items As Object(), includeNoneOption As Boolean)
        Argument.EnsureNotNull(comboBox, "comboBox")
        comboBox.BeginUpdate()
        comboBox.Items.Clear()
        If includeNoneOption Then comboBox.Items.Add("[None]")
        comboBox.Items.AddRange(items)
        comboBox.EndUpdate()
    End Sub

    Protected Shared Sub SelectOrOvertypeItem(comboBox As ComboBox, item As Object)
        Argument.EnsureNotNull(comboBox, "comboBox")
        If TypeOf item Is String AndAlso String.IsNullOrEmpty(DirectCast(item, String)) Then
            comboBox.SelectedIndex = 0
        Else
            comboBox.Text = If(item IsNot Nothing, item.ToString(), Nothing)
        End If
    End Sub

    Protected Shared Sub SelectItemElseNoneOption(comboBox As ComboBox, item As Object)
        Argument.EnsureNotNull(comboBox, "comboBox")
        comboBox.SelectedItem = item
        If comboBox.SelectedIndex = -1 Then comboBox.SelectedIndex = 0
    End Sub

    Protected Shared Sub SelectItemElseAddItem(comboBox As ComboBox, item As Object)
        Argument.EnsureNotNull(comboBox, "comboBox")
        Dim index = comboBox.Items.IndexOf(item)
        If index = -1 Then index = comboBox.Items.Add(item)
        comboBox.SelectedIndex = index
    End Sub

    Protected Sub LoadNewImageForViewer(selector As FileSelector, viewer As AnimatedImageViewer)
        Argument.EnsureNotNull(selector, "selector")
        Argument.EnsureNotNull(viewer, "viewer")
        Dim hadFocus = selector.ContainsFocus OrElse idleFocusControl.ContainsFocus
        selector.Enabled = False
        selector.UseWaitCursor = True
        viewer.UseWaitCursor = True
        If hadFocus Then
            idleFocusControl.Focus()
            lastFocusedControl = selector.FilePathComboBox
        End If
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
                                viewer.ShowError("There was an error attempting to display this image." & Environment.NewLine & Environment.NewLine &
                                                 ex.GetType().ToString() & Environment.NewLine & ex.Message)
                            Else
                                viewer.ClearError()
                            End If

                            viewer.UseWaitCursor = False
                            selector.UseWaitCursor = False
                            selector.Enabled = True
                            If hadFocus AndAlso lastFocusedControl IsNot Nothing AndAlso
                                Object.ReferenceEquals(ActiveControl, idleFocusControl) Then
                                lastFocusedControl.Focus()
                                lastFocusedControl = Nothing
                            End If
                        End If
                    End Sub)
            End Sub)
    End Sub

    Protected Sub LoadNewImageForViewer(selector As FileSelector, viewer As EffectImageViewer, behaviorCombo As ComboBox, behaviorImagePath As String)
        Argument.EnsureNotNull(selector, "selector")
        Argument.EnsureNotNull(viewer, "viewer")
        Argument.EnsureNotNull(behaviorCombo, "behaviorCombo")
        Dim behaviorComboHasFocus = behaviorCombo.Focused
        Dim hadFocus = selector.ContainsFocus OrElse behaviorComboHasFocus
        selector.Enabled = False
        behaviorCombo.Enabled = False
        selector.UseWaitCursor = True
        behaviorCombo.UseWaitCursor = True
        viewer.UseWaitCursor = True
        If hadFocus Then
            idleFocusControl.Focus()
            lastFocusedControl = If(behaviorComboHasFocus, behaviorCombo, selector.FilePathComboBox)
        End If
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
                                viewer.ShowError("There was an error attempting to display this image." & Environment.NewLine & Environment.NewLine &
                                                 ex.GetType().ToString() & Environment.NewLine & ex.Message)
                            Else
                                viewer.ClearError()
                            End If

                            viewer.UseWaitCursor = False
                            selector.UseWaitCursor = False
                            behaviorCombo.UseWaitCursor = False
                            selector.Enabled = True
                            behaviorCombo.Enabled = True
                            If hadFocus AndAlso lastFocusedControl IsNot Nothing AndAlso
                                Object.ReferenceEquals(ActiveControl, idleFocusControl) Then
                                lastFocusedControl.Focus()
                                lastFocusedControl = Nothing
                            End If
                        End If
                    End Sub)
            End Sub)
    End Sub
End Class

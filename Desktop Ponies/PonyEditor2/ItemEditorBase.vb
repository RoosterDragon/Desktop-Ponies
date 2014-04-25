Imports DesktopSprites.SpriteManagement
Imports System.IO

Public Class ItemEditorBase
    Private ReadOnly worker As New IdleWorker(Me)
    Private ReadOnly idleFocusControl As New Control(Me, Nothing) With {.TabStop = False}
    Private lastFocusedControl As Control

    Private original As IPonyIniSourceable
    Protected Property Edited As IPonyIniSourceable
    Public ReadOnly Property Item As IPonyIniSourceable
        Get
            Return original
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

    Private bindingsCreated As Boolean
    Private _loadingItem As Boolean
    ''' <summary>
    ''' Gets or sets a value indicating whether an item is being loaded, in which case property updates are ignored.
    ''' </summary>
    Private Property loadingItem As Boolean
        Get
            Return _loadingItem
        End Get
        Set(value As Boolean)
            _loadingItem = value
            If _loadingItem Then
                EnableWaitCursor(False)
            Else
                UseWaitCursor = False
            End If
        End Set
    End Property
    Private propertyUpdating As Boolean
    Private sourceUpdating As Boolean
    Private Event SourceTextChanged As Action

    Private parseIssues As ImmutableArray(Of ParseIssue)
    Private referentialIssues As ImmutableArray(Of ParseIssue)
    Public ReadOnly Property Issues As IEnumerable(Of ParseIssue)
        Get
            Return If(parseIssues, System.Linq.Enumerable.Empty(Of ParseIssue)()).Concat(
                If(referentialIssues, System.Linq.Enumerable.Empty(Of ParseIssue)()))
        End Get
    End Property
    Public Event IssuesChanged As EventHandler
    Protected Overridable Sub OnIssuesChanged(e As EventArgs)
        RaiseEvent IssuesChanged(Me, e)
    End Sub

    Private _isItemDirty As Boolean
    Public ReadOnly Property IsItemDirty As Boolean
        Get
            Return _isItemDirty
        End Get
    End Property
    Public Event DirtinessChanged As EventHandler
    Public Event AssetFileIOPerformed As EventHandler

    Public Sub NewItem(ponyBase As PonyBase, item As IPonyIniSourceable)
        SetupItem(ponyBase, item)
    End Sub

    Public Sub LoadItem(ponyBase As PonyBase, item As IPonyIniSourceable)
        _isNewItem = False
        SetupItem(ponyBase, item)
    End Sub

    Private Sub SetupItem(ponyBase As PonyBase, item As IPonyIniSourceable)
        Argument.EnsureNotNull(ponyBase, "ponyBase")
        Argument.EnsureNotNull(item, "item")
        loadingItem = True
        _ponyBasePath = Path.Combine(ponyBase.RootDirectory, ponyBase.Directory)
        _base = ponyBase
        original = item
        Edited = item.Clone()
        If Not bindingsCreated Then
            CreateBindings()
            bindingsCreated = True
        End If
        ChangeItem()
        Source.Text = Edited.SourceIni
        Enabled = True
        loadingItem = False
    End Sub

    Protected Overridable Sub CreateBindings()
        Throw New NotImplementedException()
    End Sub

    Protected Overridable Sub ChangeItem()
        Throw New NotImplementedException()
    End Sub

    Protected Shared Sub ReplaceItemsInComboBox(comboBox As ComboBox, items As Object())
        Argument.EnsureNotNull(comboBox, "comboBox")
        comboBox.BeginUpdate()
        comboBox.Items.Clear()
        comboBox.Items.Add(CaseInsensitiveString.Empty)
        comboBox.Items.AddRange(items)
        comboBox.EndUpdate()
    End Sub

    Protected Overridable Sub ReparseSource(ByRef parseIssues As ImmutableArray(Of ParseIssue))
        Throw New NotImplementedException()
    End Sub

    Public Overridable Sub AnimateImages(animate As Boolean)
    End Sub

#Region "Binding"
    Private Structure BindingProperty(Of T)
        Public ReadOnly Getter As Func(Of T)
        Public ReadOnly Setter As Action(Of T)
        Public Sub New(getter As Func(Of T), setter As Action(Of T))
            Me.Getter = getter
            Me.Setter = setter
        End Sub
    End Structure

    Private Function VerifyAndGetBindingProperty(Of T)(memberAccessExpression As Expressions.Expression(Of Func(Of T)),
                                                       Optional writable As Boolean = True) As BindingProperty(Of T)
        Dim body = TryCast(Argument.EnsureNotNull(memberAccessExpression, "memberAccessExpression").Body, Expressions.MemberExpression)
        If body Is Nothing Then Throw New ArgumentException(
            "The body of the expression must access a field or property.", "memberAccessExpression")
        Dim bodyDescription = body.ToString()

        Dim setter As Action(Of T) = Nothing
        If writable Then
            Dim param = Expressions.Expression.Parameter(body.Type, "value")
            Dim assign As Expressions.BinaryExpression
            Try
                assign = Expressions.Expression.Assign(body, param)
            Catch ex As Exception
                Throw New ArgumentException("The field or property in the expression is not writable.", ex)
            End Try
            Dim setExpression = Expressions.Expression.Lambda(Of Action(Of T))(assign, "set_" & bodyDescription, {param})
            setter = setExpression.Compile()
        End If
        Dim getExpression = Expressions.Expression.Lambda(Of Func(Of T))(
            memberAccessExpression.Body, "get_" & bodyDescription, memberAccessExpression.TailCall, memberAccessExpression.Parameters)
        Dim getter = getExpression.Compile()
        Return New BindingProperty(Of T)(getter, setter)
    End Function

    Protected Sub Bind(memberAccessExpression As Expressions.Expression(Of Func(Of Boolean)), checkBox As CheckBox,
                       Optional inverse As Boolean = False)
        Argument.EnsureNotNull(checkBox, "checkBox")
        Dim bindProp = VerifyAndGetBindingProperty(memberAccessExpression)
        If inverse Then
            AddHandler checkBox.CheckedChanged, Sub() UpdateProperty(Sub() bindProp.Setter(Not checkBox.Checked))
            AddHandler SourceTextChanged, Sub() UpdateSource(Sub() checkBox.Checked = Not bindProp.Getter())
        Else
            AddHandler checkBox.CheckedChanged, Sub() UpdateProperty(Sub() bindProp.Setter(checkBox.Checked))
            AddHandler SourceTextChanged, Sub() UpdateSource(Sub() checkBox.Checked = bindProp.Getter())
        End If
    End Sub

    Protected Sub Bind(memberAccessExpression As Expressions.Expression(Of Func(Of String)), textBox As TextBox)
        Argument.EnsureNotNull(textBox, "textBox")
        Dim bindProp = VerifyAndGetBindingProperty(memberAccessExpression)
        AddHandler textBox.KeyPress, AddressOf IgnoreQuoteCharacter
        AddHandler textBox.TextChanged, Sub() UpdateProperty(Sub() bindProp.Setter(textBox.Text))
        AddHandler SourceTextChanged, Sub() UpdateSource(Sub() textBox.Text = bindProp.Getter())
    End Sub

    Protected Sub Bind(memberAccessExpression As Expressions.Expression(Of Func(Of CaseInsensitiveString)), textBox As TextBox)
        Argument.EnsureNotNull(textBox, "textBox")
        Dim bindProp = VerifyAndGetBindingProperty(memberAccessExpression)
        AddHandler textBox.KeyPress, AddressOf IgnoreQuoteCharacter
        AddHandler textBox.TextChanged, Sub() UpdateProperty(Sub() bindProp.Setter(textBox.Text))
        AddHandler SourceTextChanged, Sub() UpdateSource(Sub() textBox.Text = bindProp.Getter())
    End Sub

    Protected Sub Bind(Of T)(memberAccessExpression As Expressions.Expression(Of Func(Of T)), numericUpDown As NumericUpDown,
                             valueToDecimal As Func(Of T, Decimal), decimalToValue As Func(Of Decimal, T))
        Argument.EnsureNotNull(numericUpDown, "numericUpDown")
        Dim bindProp = VerifyAndGetBindingProperty(memberAccessExpression)
        AddHandler numericUpDown.ValueChanged, Sub() UpdateProperty(Sub() bindProp.Setter(decimalToValue(numericUpDown.Value)))
        AddHandler SourceTextChanged, Sub() UpdateSource(Sub() numericUpDown.Value = valueToDecimal(bindProp.Getter()))
    End Sub

    Protected Sub Bind(memberAccessExpression As Expressions.Expression(Of Func(Of CaseInsensitiveString)), comboBox As ComboBox)
        Argument.EnsureNotNull(comboBox, "comboBox")
        Dim bindProp = VerifyAndGetBindingProperty(memberAccessExpression)
        AddHandler comboBox.KeyPress, AddressOf IgnoreQuoteCharacter
        AddHandler comboBox.TextChanged, Sub() UpdateProperty(Sub() bindProp.Setter(comboBox.Text))
        AddHandler comboBox.SelectedIndexChanged, Sub() UpdateProperty(Sub() bindProp.Setter(comboBox.Text))
        AddHandler SourceTextChanged, Sub() UpdateSource(Sub() SelectOrOvertypeItem(comboBox, bindProp.Getter()))
    End Sub

    Protected Sub Bind(Of T)(memberAccessExpression As Expressions.Expression(Of Func(Of T)), comboBox As ComboBox)
        Argument.EnsureNotNull(comboBox, "comboBox")
        Dim bindProp = VerifyAndGetBindingProperty(memberAccessExpression)
        AddHandler comboBox.SelectedIndexChanged, Sub() UpdateProperty(Sub() bindProp.Setter(DirectCast(comboBox.SelectedItem, T)))
        AddHandler SourceTextChanged, Sub() UpdateSource(Sub() SelectItemElseNoneOption(comboBox, bindProp.Getter()))
    End Sub

    Protected Sub Bind(memberAccessExpression As Expressions.Expression(Of Func(Of String)), fileSelector As FileSelector)
        Argument.EnsureNotNull(fileSelector, "fileSelector")
        Dim bindProp = VerifyAndGetBindingProperty(memberAccessExpression)
        AddHandler fileSelector.ListRefreshed, Sub()
                                                   If loadingItem Then Return
                                                   RaiseEvent AssetFileIOPerformed(Me, EventArgs.Empty)
                                               End Sub
        AddHandler fileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim fullPath = If(fileSelector.FilePath = Nothing,
                                                       Nothing, Path.Combine(PonyBasePath, fileSelector.FilePath))
                                     bindProp.Setter(fullPath)
                                 End Sub)
        Dim lastTypedFileName As String = Nothing
        Dim lastTypedFileNameMissing As Boolean
        AddHandler SourceTextChanged,
            Sub() UpdateSource(
                Sub()
                    SyncTypedImagePath(
                        fileSelector, bindProp.Getter(),
                        Sub(filePath) bindProp.Setter(If(filePath Is Nothing, Nothing, Path.Combine(PonyBasePath, filePath))),
                        lastTypedFileName, lastTypedFileNameMissing)
                End Sub)
    End Sub

    Protected Sub Bind(memberAccessExpression As Expressions.Expression(Of Func(Of String)), fileSelector As FileSelector,
                       animatedImageViewer As AnimatedImageViewer)
        Argument.EnsureNotNull(animatedImageViewer, "animatedImageViewer")
        Bind(memberAccessExpression, fileSelector)
        AddHandler fileSelector.FilePathSelected, Sub() LoadNewImageForViewer(fileSelector, animatedImageViewer)
    End Sub

    Protected Sub Bind(memberAccessExpression As Expressions.Expression(Of Func(Of String)), fileSelector As FileSelector,
                   effectImageViewer As EffectImageViewer, behaviorComboBox As ComboBox,
                   getBehavior As Func(Of Behavior), useLeftImage As Boolean)
        Argument.EnsureNotNull(effectImageViewer, "effectImageViewer")
        Argument.EnsureNotNull(behaviorComboBox, "behaviorComboBox")
        Argument.EnsureNotNull(getBehavior, "getBehavior")
        Bind(memberAccessExpression, fileSelector)

        Dim behaviorImagePath As String = Nothing
        Dim effectImagePath As String = Nothing
        Dim refreshImageViewer =
            Sub()
                Dim behavior = getBehavior()
                Dim newBehaviorImagePath As String = Nothing
                If behavior IsNot Nothing Then newBehaviorImagePath = If(useLeftImage, behavior.LeftImage, behavior.RightImage).Path
                If Not PathEquality.Equals(behaviorImagePath, newBehaviorImagePath) OrElse
                    Not PathEquality.Equals(effectImagePath, fileSelector.FilePath) Then
                    behaviorImagePath = newBehaviorImagePath
                    effectImagePath = fileSelector.FilePath
                    LoadNewImageForViewer(fileSelector, effectImageViewer, behaviorComboBox, behaviorImagePath)
                End If
            End Sub

        AddHandler SourceTextChanged, Sub() refreshImageViewer()
    End Sub

    Protected Sub Bind(Of T)(memberAccessExpression As Expressions.Expression(Of Func(Of HashSet(Of T))), checkedListBox As CheckedListBox)
        Argument.EnsureNotNull(checkedListBox, "checkedListBox")
        Dim bindProp = VerifyAndGetBindingProperty(memberAccessExpression, False)
        AddHandler checkedListBox.ItemCheck, Sub(sender, e) UpdateCheckedListBox(sender, e, bindProp.Getter())
        AddHandler SourceTextChanged, Sub() UpdateSource(Sub() UpdateList(checkedListBox, bindProp.Getter()))
    End Sub

    Private Shared Sub IgnoreQuoteCharacter(sender As Object, e As KeyPressEventArgs)
        e.Handled = e.KeyChar = """"c
    End Sub

    Private Shared Sub SelectOrOvertypeItem(comboBox As ComboBox, item As Object)
        Argument.EnsureNotNull(comboBox, "comboBox")
        Dim itemText = If(item IsNot Nothing, item.ToString(), "")
        If comboBox.Text.Trim() <> itemText.Trim() Then comboBox.Text = itemText
    End Sub

    Private Shared Sub SelectItemElseNoneOption(comboBox As ComboBox, item As Object)
        Argument.EnsureNotNull(comboBox, "comboBox")
        comboBox.SelectedItem = item
        If comboBox.SelectedIndex = -1 Then comboBox.SelectedIndex = 0
    End Sub

    Private Sub UpdateCheckedListBox(Of T)(sender As Object, e As ItemCheckEventArgs, names As HashSet(Of T))
        UpdateProperty(Sub()
                           Dim listBox = DirectCast(sender, CheckedListBox)
                           names.Clear()
                           names.UnionWith(listBox.CheckedItems.Cast(Of T))
                           Dim item = DirectCast(listBox.Items(e.Index), T)
                           If e.NewValue <> CheckState.Unchecked Then
                               names.Add(item)
                           Else
                               names.Remove(item)
                           End If
                       End Sub)
    End Sub

    Private Shared Sub UpdateList(Of T)(list As CheckedListBox, values As HashSet(Of T))
        list.SuspendLayout()
        For i = 0 To list.Items.Count - 1
            list.SetItemChecked(i, values.Contains(DirectCast(list.Items(i), T)))
        Next
        list.ResumeLayout()
    End Sub

    Private Sub LoadNewImageForViewer(selector As FileSelector, viewer As AnimatedImageViewer)
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
                                viewer.ShowError("There was an error attempting to display this image." &
                                                 Environment.NewLine & Environment.NewLine &
                                                 ex.GetType().ToString() &
                                                 Environment.NewLine &
                                                 ex.Message)
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

    Private Sub LoadNewImageForViewer(selector As FileSelector, viewer As EffectImageViewer,
                                      behaviorCombo As ComboBox, behaviorImagePath As String)
        Argument.EnsureNotNull(selector, "selector")
        Argument.EnsureNotNull(viewer, "viewer")
        Argument.EnsureNotNull(behaviorCombo, "behaviorCombo")
        Dim behaviorComboHasFocus = behaviorCombo.Focused
        Dim selectionStart As Integer
        Dim selectionLength As Integer
        If behaviorComboHasFocus Then
            selectionStart = behaviorCombo.SelectionStart
            selectionLength = behaviorCombo.SelectionLength
        End If
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
                        behaviorImage = BitmapFrame.AnimationFromFile(behaviorImagePath)
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
                                viewer.ShowError("There was an error attempting to display this image." &
                                                 Environment.NewLine & Environment.NewLine &
                                                 ex.GetType().ToString() &
                                                 Environment.NewLine &
                                                 ex.Message)
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
                                If behaviorComboHasFocus Then
                                    Dim lastFocusedComboBox = DirectCast(lastFocusedControl, ComboBox)
                                    lastFocusedComboBox.SelectionStart = selectionStart
                                    lastFocusedComboBox.SelectionLength = selectionLength
                                End If
                                lastFocusedControl = Nothing
                            End If
                        End If
                    End Sub)
            End Sub)
    End Sub

    Private Sub SyncTypedImagePath(selector As FileSelector, fullPath As String, setPath As Action(Of String),
                                   ByRef typedPath As String, ByRef typedPathMissing As Boolean)
        If typedPathMissing Then
            selector.FilePathComboBox.Items.Remove(typedPath)
        End If
        typedPath = Path.GetFileName(fullPath)
        If typedPath = Base.Directory OrElse typedPath = "" Then typedPath = Nothing
        selector.FilePath = typedPath
        typedPathMissing = Not PathEquality.Equals(selector.FilePath, typedPath)
        If typedPathMissing Then
            selector.FilePathComboBox.SelectedIndex = selector.FilePathComboBox.Items.Add(typedPath)
        End If
        setPath(selector.FilePath)
    End Sub
#End Region

    Private Sub UpdateSource(updateUIFromProperty As Action)
        If propertyUpdating Then Return
        sourceUpdating = True
        updateUIFromProperty()
        sourceUpdating = False
    End Sub

    Private Sub UpdateProperty(updatePropertyFromUI As Action)
        If loadingItem OrElse sourceUpdating Then Return
        propertyUpdating = True
        updatePropertyFromUI()
        OnItemPropertyChanged()
        propertyUpdating = False
    End Sub

    Protected Sub OnItemPropertyChanged()
        propertyUpdating = True
        UpdateDirtyFlag(True)
        Source.Text = Edited.GetPonyIni()
        propertyUpdating = False
    End Sub

    Private Sub Source_TextChanged(sender As Object, e As EventArgs) Handles Source.TextChanged
        If loadingItem OrElse propertyUpdating Then
            SourceChangesMade()
            Return
        End If
        SourceTextTimer.Stop()
        SourceTextTimer.Start()
    End Sub

    Private Sub SourceTextTimer_Tick(sender As Object, e As EventArgs) Handles SourceTextTimer.Tick
        SourceTextTimer.Stop()
        SourceChangesMade()
        UpdateDirtyFlag(True)
    End Sub

    Private Sub SourceChangesMade()
        ReparseSource(parseIssues)
        UpdateReferentialIssues()
        OnIssuesChanged(EventArgs.Empty)
        RaiseEvent SourceTextChanged()
    End Sub

    Private Function UpdateReferentialIssues() As Boolean
        Dim referential = TryCast(Edited, IReferential)
        If referential IsNot Nothing Then
            referentialIssues = referential.GetReferentialIssues(Base.Collection)
        End If
        Return referential IsNot Nothing
    End Function

    Public Sub RefreshReferentialIssues()
        If UpdateReferentialIssues() Then OnIssuesChanged(EventArgs.Empty)
    End Sub

    Public Function SaveItem() As Boolean
        If String.IsNullOrWhiteSpace(Edited.Name) Then
            MessageBox.Show(Me, "Please enter a name for this " & ItemTypeName & ".",
                            "Name Missing", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If

        If Collection.Cast(Of IPonyIniSourceable).Any(
            Function(item) item.Name = Edited.Name AndAlso Not Object.ReferenceEquals(item, original)) Then
            MessageBox.Show(Me, "A " & ItemTypeName & " with the name '" & Edited.Name &
                            "' already exists for this pony. Please choose another name.",
                            "Name Not Unique", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return False
        End If

        If Not IsNewItem AndAlso original.Name <> Edited.Name Then
            If MessageBox.Show(Me, "Renaming this " & ItemTypeName & " will break other references. Continue with save?",
                               "Rename?", MessageBoxButtons.YesNo,
                               MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) = DialogResult.No Then
                Return False
            End If
        End If

        Dim index = Collection.IndexOf(original)
        Dim added = index = -1
        If added Then
            index = Collection.Add(Edited)
        Else
            Collection(index) = Edited
        End If

        Dim result = DialogResult.None
        Do
            Try
                Edited.SourceIni = Source.Text
                Base.Save()
                _isNewItem = False
                UpdateDirtyFlag(False)
                result = DialogResult.OK
            Catch ex As IOException
                result = MessageBox.Show(Me, "There was an error attempting to save the pony." & Environment.NewLine &
                                         Environment.NewLine & ex.Message & Environment.NewLine & Environment.NewLine &
                                         "Retry?", "Save Error", MessageBoxButtons.RetryCancel,
                                         MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
            End Try
        Loop While result = DialogResult.Retry

        If result <> DialogResult.OK Then
            If added Then
                Collection.Remove(Edited)
            Else
                Collection(index) = original
            End If
            Return False
        Else
            original = Edited
            Edited = original.Clone()
            Return True
        End If
    End Function

    Protected Sub UpdateDirtyFlag(newState As Boolean)
        If _isItemDirty = newState Then Return
        _isItemDirty = newState
        RaiseEvent DirtinessChanged(Me, EventArgs.Empty)
        DirectCast(Parent, ItemTabPage).IsDirty = newState
    End Sub
End Class

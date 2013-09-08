Imports System.IO

Public Class PonyEditorForm2
    Private ReadOnly worker As IdleWorker = IdleWorker.CurrentThreadWorker
    Private ReadOnly bases As New Dictionary(Of String, PonyBase)

    Private workingCount As Integer

    Private Class PageRef
        Private ReadOnly _ponyBase As MutablePonyBase
        Public ReadOnly Property PonyBase As MutablePonyBase
            Get
                Return _ponyBase
            End Get
        End Property
        Private ReadOnly _pageContent As PageContent
        Public ReadOnly Property PageContent As PageContent
            Get
                Return _pageContent
            End Get
        End Property
        Public Property ItemName As String
        Public Sub New(ponyBase As MutablePonyBase, pageContent As PageContent, itemName As String)
            _ponyBase = ponyBase
            _pageContent = pageContent
            Me.ItemName = itemName
        End Sub
        Public Overrides Function ToString() As String
            Return String.Join(Path.DirectorySeparatorChar, If(PonyBase IsNot Nothing, PonyBase.Directory, ""), PageContent, ItemName)
        End Function
    End Class

    Private ReadOnly Property ActiveItemEditor As ItemEditorBase
        Get
            Return If(Documents.SelectedTab Is Nothing, Nothing, DirectCast(Documents.SelectedTab.Controls(0), ItemEditorBase))
        End Get
    End Property

    Public Sub New(ponyBaseCollection As IEnumerable(Of PonyBase))
        InitializeComponent()
        Icon = My.Resources.Twilight
        DocumentsView.PathSeparator = Path.DirectorySeparatorChar
    End Sub

    Private Sub PonyEditorForm2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Enabled = False
        Dim screenArea = Screen.FromHandle(Handle).WorkingArea.Size
        Size = New Size(CInt(screenArea.Width * 0.8), screenArea.Height)
        CenterToScreen()
        Threading.ThreadPool.QueueUserWorkItem(Sub() LoadBases())
    End Sub

    Private Sub LoadBases()
        Dim poniesNode As TreeNode = Nothing
        worker.QueueTask(Sub()
                             poniesNode = New TreeNode("Ponies") With
                                          {.Tag = New PageRef(Nothing, PageContent.Ponies, Nothing)}
                             DocumentsView.Nodes.Add(poniesNode)
                             poniesNode.Expand()
                         End Sub)

        Dim ponyCollection = New PonyCollection()
        ponyCollection.LoadAll(
            Sub(count) worker.QueueTask(Sub() EditorProgressBar.Maximum = count),
            Sub(pony) worker.QueueTask(
                Sub()
                    bases.Add(pony.Directory, pony)
                    Dim ponyBaseRef = New PageRef(pony, PageContent.Pony, Nothing)
                    Dim ponyBaseNode = New TreeNode(pony.Directory) With
                                       {.Tag = ponyBaseRef, .Name = ponyBaseRef.ToString()}
                    poniesNode.Nodes.Add(ponyBaseNode)

                    Dim behaviorsRef = New PageRef(pony, PageContent.Behaviors, Nothing)
                    Dim behaviorsNode = New TreeNode("Behaviors") With
                                       {.Tag = behaviorsRef, .Name = behaviorsRef.ToString()}
                    ponyBaseNode.Nodes.Add(behaviorsNode)
                    Dim effectsRef = New PageRef(pony, PageContent.Effects, Nothing)
                    Dim effectsNode = New TreeNode("Effects") With
                                          {.Tag = effectsRef, .Name = effectsRef.ToString()}
                    ponyBaseNode.Nodes.Add(effectsNode)
                    Dim speechesRef = New PageRef(pony, PageContent.Speeches, Nothing)
                    Dim speechesNode = New TreeNode("Speeches") With
                                       {.Tag = speechesRef, .Name = speechesRef.ToString()}
                    ponyBaseNode.Nodes.Add(speechesNode)

                    For Each behavior In pony.Behaviors
                        Dim ref = New PageRef(pony, PageContent.Behavior, behavior.Name)
                        behaviorsNode.Nodes.Add(New TreeNode(behavior.Name) With {.Tag = ref, .Name = ref.ToString()})
                    Next

                    For Each effect In pony.Effects
                        Dim ref = New PageRef(pony, PageContent.Effect, effect.Name)
                        effectsNode.Nodes.Add(New TreeNode(effect.Name) With {.Tag = ref, .Name = ref.ToString()})
                    Next

                    For Each speech In pony.SpeakingLines
                        Dim ref = New PageRef(pony, PageContent.Speech, speech.Name)
                        speechesNode.Nodes.Add(New TreeNode(speech.Name) With {.Tag = ref, .Name = ref.ToString()})
                    Next

                    EditorProgressBar.Value += 1
                End Sub))
        worker.QueueTask(Sub()
                             poniesNode.TreeView.Sort()
                         End Sub)

        worker.QueueTask(Sub()
                             Dim basesCopy = bases.Values.ToArray()
                             For Each base In bases.Values
                                 base.LoadInteractions(basesCopy)
                             Next
                         End Sub)
        worker.QueueTask(Sub()
                             EditorStatus.Text = "Ready"
                             EditorProgressBar.Value = 1
                             EditorProgressBar.Maximum = 1
                             EditorProgressBar.Style = ProgressBarStyle.Marquee
                             EditorProgressBar.Visible = False
                             DocumentsView.TopNode.Expand()
                             UseWaitCursor = False
                             Enabled = True
                             DocumentsView.Focus()
                         End Sub)
    End Sub

    Private Shared Function GetTabText(pageRef As PageRef) As String
        Select Case pageRef.PageContent
            Case PageContent.Ponies
                Return PageContent.Ponies.ToString()
            Case PageContent.Pony
                Return pageRef.PonyBase.Directory
            Case PageContent.Behaviors, PageContent.Effects, PageContent.Speeches
                Return pageRef.PonyBase.Directory & " - " & pageRef.PageContent.ToString()
            Case PageContent.Behavior, PageContent.Effect, PageContent.Speech
                Return pageRef.PonyBase.Directory & ": " & pageRef.ItemName
            Case Else
                Throw New System.ComponentModel.InvalidEnumArgumentException("Unknown Content in pageRef")
        End Select
    End Function

    Private Shadows Function GetPageRef(tab As TabPage) As PageRef
        Return DirectCast(tab.Tag, PageRef)
    End Function

    Private Shadows Function GetPageRef(node As TreeNode) As PageRef
        Return DirectCast(node.Tag, PageRef)
    End Function

    Private Sub DocumentsView_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles DocumentsView.NodeMouseDoubleClick
        OpenTab(GetPageRef(e.Node))
    End Sub

    Private Sub DocumentsView_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DocumentsView.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = OpenTab(GetPageRef(DocumentsView.SelectedNode))
        End If
    End Sub

    Private Function OpenTab(pageRef As PageRef) As Boolean
        Dim pageRefKey = pageRef.ToString()
        Dim tab = Documents.TabPages.Item(pageRefKey)

        If tab Is Nothing Then
            Dim editor As ItemEditorBase = Nothing
            Select Case pageRef.PageContent
                Case PageContent.Behavior
                    editor = New BehaviorEditor()
                Case PageContent.Effect
                    editor = New EffectEditor()
                Case PageContent.Speech
                    editor = New SpeechEditor()
            End Select
            If editor IsNot Nothing Then
                QueueWorkItem(Sub() editor.LoadItem(pageRef.PonyBase, pageRef.ItemName))
                editor.Dock = DockStyle.Fill
                tab = New ItemTabPage() With {.Name = pageRefKey, .Text = GetTabText(pageRef), .Tag = pageRef}
                tab.Controls.Add(editor)
                Documents.TabPages.Add(tab)
                CloseTabButton.Enabled = True
                CloseAllTabsButton.Enabled = True
            End If
        End If

        If tab IsNot Nothing Then
            Documents.SelectedTab = tab
            SwitchTab(tab)
            DocumentsView.Select()
            DocumentsView.SelectedNode = DocumentsView.Nodes.Find(pageRefKey, True)(0)
            Return True
        End If

        Return False
    End Function

    Private Sub Documents_Selected(sender As Object, e As TabControlEventArgs) Handles Documents.Selected
        SwitchTab(e.TabPage)
    End Sub

    Private Sub SwitchTab(newTab As TabPage)
        If Documents.SelectedTab IsNot Nothing Then
            RemoveHandler ActiveItemEditor.IssuesChanged, AddressOf ActiveItemEditor_IssuesChanged
            RemoveHandler ActiveItemEditor.DirtinessChanged, AddressOf ActiveItemEditor_DirtinessChanged
            ActiveItemEditor.AnimateImages(False)
        End If

        Documents.SelectedTab = newTab

        If Documents.SelectedTab IsNot Nothing Then
            ActiveItemEditor.AnimateImages(True)
            AddHandler ActiveItemEditor.DirtinessChanged, AddressOf ActiveItemEditor_DirtinessChanged
            AddHandler ActiveItemEditor.IssuesChanged, AddressOf ActiveItemEditor_IssuesChanged
        End If
        ActiveItemEditor_DirtinessChanged(Me, EventArgs.Empty)
        ActiveItemEditor_IssuesChanged(Me, EventArgs.Empty)
    End Sub

    Private Sub ActiveItemEditor_DirtinessChanged(sender As Object, e As EventArgs)
        Dim dirty = If(ActiveItemEditor Is Nothing, False, ActiveItemEditor.IsItemDirty)
        SaveItemButton.Enabled = dirty
        SaveItemButton.ToolTipText = If(dirty,
                                        "Save the changes made to the item in the visible tab.",
                                        "No changes have been made to the item in the visible tab.")
    End Sub

    Private Sub ActiveItemEditor_IssuesChanged(sender As Object, e As EventArgs)
        IssuesGrid.SuspendLayout()
        IssuesGrid.Rows.Clear()
        If ActiveItemEditor IsNot Nothing AndAlso ActiveItemEditor.Issues IsNot Nothing Then
            For Each issue In ActiveItemEditor.Issues
                IssuesGrid.Rows.Add(If(issue.Fatal, SystemIcons.Error, SystemIcons.Warning),
                                    If(issue.PropertyName, "Element " & issue.Index + 1),
                                    issue.Reason,
                                    issue.FallbackValue,
                                    issue.Source)
            Next
        End If
        IssuesGrid.ResumeLayout()
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveItemButton.Click
        ActiveItemEditor.SaveItem()
        Dim ref = GetPageRef(Documents.SelectedTab)
        Dim node = DocumentsView.Nodes.Find(ref.ToString(), True)(0)

        ref.ItemName = ActiveItemEditor.ItemName
        Documents.SelectedTab.Text = GetTabText(ref)
        node.Name = ref.ToString()
        node.Text = ref.ItemName

        EditorStatus.Text = "Saved"
    End Sub

    Private Sub CloseTabButton_Click(sender As Object, e As EventArgs) Handles CloseTabButton.Click
        RemoveTab(Documents.SelectedTab)
        SwitchTab(Documents.SelectedTab)
    End Sub

    Private Sub CloseAllTabsButton_Click(sender As Object, e As EventArgs) Handles CloseAllTabsButton.Click
        For Each t In Documents.TabPages.Cast(Of TabPage)().ToArray()
            RemoveTab(t)
        Next
        SwitchTab(Documents.SelectedTab)
    End Sub

    Private Sub RemoveTab(tab As TabPage)
        Documents.TabPages.Remove(tab)
        tab.Dispose()
        CloseTabButton.Enabled = Documents.TabPages.Count > 0
        CloseAllTabsButton.Enabled = Documents.TabPages.Count > 0
    End Sub

    Private Sub QueueWorkItem(item As MethodInvoker)
        workingCount += 1
        EditorProgressBar.Visible = True
        EditorStatus.Text = "Working..."
        worker.QueueTask(Sub()
                             Try
                                 item()
                             Finally
                                 workingCount -= 1
                                 If workingCount = 0 Then
                                     EditorProgressBar.Visible = False
                                     EditorStatus.Text = "Ready"
                                 End If
                             End Try
                         End Sub)
    End Sub

    Private Sub PonyEditorForm2_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' TODO: Change IdleWorker to allow it be depend on a specified control, and drop remaining tasks if the control is destroyed.
        ' Until then, we'll make sure we process any tasks before closing to prevent errors.
        worker.WaitOnAllTasks()
    End Sub
End Class
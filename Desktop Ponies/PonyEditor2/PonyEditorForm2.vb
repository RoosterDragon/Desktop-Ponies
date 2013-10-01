Imports System.IO

Public Class PonyEditorForm2
    Private Shared ReadOnly ErrorBitmap As Bitmap = SystemIcons.Error.ToBitmap()
    Private Shared ReadOnly WarningBitmap As Bitmap = SystemIcons.Warning.ToBitmap()

    Private ReadOnly worker As New IdleWorker(Me)
    Private ponies As PonyCollection
    Private ReadOnly bases As New Dictionary(Of String, PonyBase)()
    Private ReadOnly nodeLookup As New Dictionary(Of String, TreeNode)()

    Private preview As PonyPreview

    Private workingCount As Integer

    Private Class PageRef
        Private ReadOnly _ponyBase As PonyBase
        Public ReadOnly Property PonyBase As PonyBase
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
        Public Property Item As IPonyIniSourceable
        Public Sub New(ponyBase As PonyBase, pageContent As PageContent, item As IPonyIniSourceable)
            _ponyBase = ponyBase
            _pageContent = pageContent
            Me.Item = item
        End Sub
        Public Overrides Function ToString() As String
            Return String.Join(Path.DirectorySeparatorChar,
                               If(PonyBase IsNot Nothing, PonyBase.Directory, ""),
                               PageContent,
                               If(Item IsNot Nothing, Item.Name.ToString(), ""))
        End Function
    End Class

    Private ReadOnly Property ActiveItemEditor As ItemEditorBase
        Get
            If Documents.SelectedTab Is Nothing OrElse Documents.SelectedTab.Controls.Count = 0 Then
                Return Nothing
            Else
                Return TryCast(Documents.SelectedTab.Controls(0), ItemEditorBase)
            End If
        End Get
    End Property

    Public Sub New()
        InitializeComponent()
        Icon = My.Resources.Twilight
        DocumentsView.PathSeparator = Path.DirectorySeparatorChar
    End Sub

    Private Sub PonyEditorForm2_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim screenArea = Screen.FromHandle(Handle).WorkingArea.Size
        Size = New Size(CInt(screenArea.Width * 0.8), screenArea.Height)
        CenterToScreen()
        worker.QueueTask(Sub()
                             EnableWaitCursor(True)
                             Dim images = New ImageList()
                             images.Images.Add(SystemIcons.Warning)
                             images.Images.Add(SystemIcons.WinLogo)
                             images.Images.Add(SystemIcons.Error)
                             DocumentsView.ImageList = images
                         End Sub)
        Threading.ThreadPool.QueueUserWorkItem(Sub() LoadBases())
    End Sub

    Private Sub LoadBases()
        Dim poniesNode As TreeNode = Nothing
        worker.QueueTask(Sub()
                             poniesNode = New TreeNode("Ponies") With
                                          {.Tag = New PageRef(Nothing, PageContent.Ponies, Nothing)}
                             DocumentsView.Nodes.Add(poniesNode)
                             nodeLookup(poniesNode.Name) = poniesNode
                             poniesNode.Expand()
                         End Sub)

        ponies = New PonyCollection(
            Sub(count) worker.QueueTask(Sub() EditorProgressBar.Maximum = count),
            Sub(pony) worker.QueueTask(
                Sub()
                    bases.Add(pony.Directory, pony)
                    Dim ponyBaseRef = New PageRef(pony, PageContent.Pony, Nothing)
                    Dim ponyBaseNode = New TreeNode(pony.Directory) With
                                       {.Tag = ponyBaseRef, .Name = ponyBaseRef.ToString()}
                    poniesNode.Nodes.Add(ponyBaseNode)
                    nodeLookup(ponyBaseNode.Name) = ponyBaseNode

                    Dim behaviorsRef = New PageRef(pony, PageContent.Behaviors, Nothing)
                    Dim behaviorsNode = New TreeNode("Behaviors") With
                                       {.Tag = behaviorsRef, .Name = behaviorsRef.ToString()}
                    ponyBaseNode.Nodes.Add(behaviorsNode)
                    nodeLookup(behaviorsNode.Name) = behaviorsNode
                    Dim effectsRef = New PageRef(pony, PageContent.Effects, Nothing)
                    Dim effectsNode = New TreeNode("Effects") With
                                          {.Tag = effectsRef, .Name = effectsRef.ToString()}
                    ponyBaseNode.Nodes.Add(effectsNode)
                    nodeLookup(effectsNode.Name) = effectsNode
                    Dim speechesRef = New PageRef(pony, PageContent.Speeches, Nothing)
                    Dim speechesNode = New TreeNode("Speeches") With
                                       {.Tag = speechesRef, .Name = speechesRef.ToString()}
                    ponyBaseNode.Nodes.Add(speechesNode)
                    nodeLookup(speechesNode.Name) = speechesNode
                    Dim interactionsRef = New PageRef(pony, PageContent.Interactions, Nothing)
                    Dim interactionsNode = New TreeNode("Interactions") With
                                       {.Tag = interactionsRef, .Name = interactionsRef.ToString()}
                    ponyBaseNode.Nodes.Add(interactionsNode)
                    nodeLookup(interactionsNode.Name) = interactionsNode

                    For Each behavior In pony.Behaviors
                        Dim ref = New PageRef(pony, PageContent.Behavior, behavior)
                        Dim node = New TreeNode(behavior.Name) With {.Tag = ref, .Name = ref.ToString()}
                        behaviorsNode.Nodes.Add(node)
                        nodeLookup(node.Name) = node
                    Next

                    For Each effect In pony.Effects
                        Dim ref = New PageRef(pony, PageContent.Effect, effect)
                        Dim node = New TreeNode(effect.Name) With {.Tag = ref, .Name = ref.ToString()}
                        effectsNode.Nodes.Add(node)
                        nodeLookup(node.Name) = node
                    Next

                    For Each speech In pony.Speeches
                        Dim ref = New PageRef(pony, PageContent.Speech, speech)
                        Dim node = New TreeNode(speech.Name) With {.Tag = ref, .Name = ref.ToString()}
                        speechesNode.Nodes.Add(node)
                        nodeLookup(node.Name) = node
                    Next

                    For Each interaction In pony.Interactions
                        Dim ref = New PageRef(pony, PageContent.Interaction, interaction)
                        Dim node = New TreeNode(interaction.Name) With {.Tag = ref, .Name = ref.ToString()}
                        interactionsNode.Nodes.Add(node)
                        nodeLookup(node.Name) = node
                    Next

                    EditorProgressBar.Value += 1
                End Sub))
        worker.QueueTask(Sub() poniesNode.TreeView.Sort())
        worker.QueueTask(Sub()
                             preview = New PonyPreview(ponies)
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
        worker.WaitOnAllTasks()
        ValidateBases()
    End Sub

    Private Sub ValidateBases()
        For Each base In bases.Values.OrderBy(Function(pb) pb.Directory)
            ValidateBase(base)
        Next
    End Sub

    Private Sub ValidateBase(base As PonyBase)
        Dim validateBehavior = Function(behavior As Behavior)
                                   Dim b As Behavior = Nothing
                                   Return behavior.TryLoad(
                                       behavior.SourceIni,
                                       Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, base.Directory),
                                       base, b, Nothing) AndAlso b.GetReferentialIssues(ponies).Length = 0
                               End Function
        Dim behaviorsValid = ValidateItems(base, base.Behaviors, validateBehavior, PageContent.Behaviors, PageContent.Behavior)
        Dim validateEffect = Function(effect As EffectBase)
                                 Dim e As EffectBase = Nothing
                                 Return EffectBase.TryLoad(
                                     effect.SourceIni,
                                     Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, base.Directory),
                                     base, e, Nothing) AndAlso e.GetReferentialIssues(ponies).Length = 0
                             End Function
        Dim effectsValid = ValidateItems(base, base.Effects, validateEffect, PageContent.Effects, PageContent.Effect)
        Dim validateSpeech = Function(speech As Speech)
                                 Dim ref = New PageRef(base, PageContent.Speech, speech)
                                 Return speech.TryLoad(
                                     speech.SourceIni,
                                     Path.Combine(Options.InstallLocation, PonyBase.RootDirectory, ref.PonyBase.Directory),
                                     Nothing, Nothing)
                             End Function
        Dim speechesValid = ValidateItems(base, base.Speeches, validateSpeech, PageContent.Speeches, PageContent.Speech)
        Dim validateInteraction = Function(interaction As InteractionBase)
                                      Dim ref = New PageRef(base, PageContent.Interaction, interaction)
                                      Dim i As InteractionBase = Nothing
                                      Return InteractionBase.TryLoad(
                                          interaction.SourceIni,
                                          i, Nothing) AndAlso i.GetReferentialIssues(ponies).Length = 0
                                  End Function
        Dim interactionsValid = ValidateItems(base, base.Interactions, validateInteraction,
                                              PageContent.Interactions, PageContent.Interaction)

        worker.QueueTask(Sub()
                             Dim ref = New PageRef(base, PageContent.Pony, Nothing)
                             Dim node = FindNode(ref.ToString())
                             node.ImageIndex = If(behaviorsValid AndAlso effectsValid AndAlso speechesValid AndAlso interactionsValid,
                                                  1, 2)
                         End Sub)
    End Sub

    Private Function ValidateItems(Of T As IPonyIniSourceable)(base As PonyBase, items As IEnumerable(Of T),
                                                               validateItem As Func(Of T, Boolean),
                                                               content As PageContent, childContent As PageContent) As Boolean
        Dim itemsValid = True
        For Each item In items
            Dim ref = New PageRef(base, childContent, item)
            Dim parsedItem As T = Nothing
            Dim itemValid = validateItem(item)
            If Not itemValid Then itemsValid = False
            worker.QueueTask(Sub()
                                 Dim node = FindNode(ref.ToString())
                                 node.ImageIndex = If(itemValid, 1, 2)
                             End Sub)
        Next
        worker.QueueTask(Sub()
                             Dim ref = New PageRef(base, content, Nothing)
                             Dim node = FindNode(ref.ToString())
                             node.ImageIndex = If(itemsValid, 1, 2)
                         End Sub)
        Return itemsValid
    End Function

    Private Function FindNode(name As String) As TreeNode
        Dim node As TreeNode = Nothing
        If nodeLookup.TryGetValue(name, node) Then
            Return node
        Else
            Return DocumentsView.Nodes.Find(name, True).Single()
        End If
    End Function

    Private Shared Function GetTabText(pageRef As PageRef) As String
        Select Case pageRef.PageContent
            Case PageContent.Ponies
                Return PageContent.Ponies.ToString()
            Case PageContent.Pony
                Return pageRef.PonyBase.Directory
            Case PageContent.Behaviors, PageContent.Effects, PageContent.Speeches, PageContent.Interactions
                Return pageRef.PonyBase.Directory & " - " & pageRef.PageContent.ToString()
            Case PageContent.Behavior, PageContent.Effect, PageContent.Speech, PageContent.Interaction
                Return pageRef.PonyBase.Directory & ": " & pageRef.Item.Name
            Case Else
                Throw New System.ComponentModel.InvalidEnumArgumentException("Unknown Content in pageRef")
        End Select
    End Function

    Private Shared Shadows Function GetPageRef(tab As TabPage) As PageRef
        Return DirectCast(tab.Tag, PageRef)
    End Function

    Private Shared Shadows Function GetPageRef(node As TreeNode) As PageRef
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
            Dim childControl As Control = Nothing
            Select Case pageRef.PageContent
                Case PageContent.Pony
                    childControl = preview
                Case PageContent.Behavior, PageContent.Effect, PageContent.Speech, PageContent.Interaction
                    Dim editor As ItemEditorBase = Nothing
                    Select Case pageRef.PageContent
                        Case PageContent.Behavior
                            editor = New BehaviorEditor(ponies)
                        Case PageContent.Effect
                            editor = New EffectEditor()
                        Case PageContent.Speech
                            editor = New SpeechEditor()
                        Case PageContent.Interaction
                            editor = New InteractionEditor()
                    End Select
                    QueueWorkItem(Sub() editor.LoadItem(pageRef.PonyBase, pageRef.Item))
                    childControl = editor
            End Select
            If childControl IsNot Nothing Then
                childControl.Dock = DockStyle.Fill
                tab = New ItemTabPage() With {.Name = pageRefKey, .Text = GetTabText(pageRef), .Tag = pageRef}
                tab.Controls.Add(childControl)
                Documents.TabPages.Add(tab)
                CloseTabButton.Enabled = True
                CloseAllTabsButton.Enabled = True
            End If
        End If

        If tab IsNot Nothing Then
            Documents.SelectedTab = tab
            SwitchTab(tab)
            DocumentsView.Select()
            DocumentsView.SelectedNode = FindNode(pageRefKey)
            Return True
        End If

        Return False
    End Function

    Private Sub Documents_Selected(sender As Object, e As TabControlEventArgs) Handles Documents.Selected
        SwitchTab(e.TabPage)
    End Sub

    Private Sub SwitchTab(newTab As TabPage)
        If ActiveItemEditor IsNot Nothing Then
            RemoveHandler ActiveItemEditor.IssuesChanged, AddressOf ActiveItemEditor_IssuesChanged
            RemoveHandler ActiveItemEditor.DirtinessChanged, AddressOf ActiveItemEditor_DirtinessChanged
            ActiveItemEditor.AnimateImages(False)
        End If

        Documents.SelectedTab = newTab

        If ActiveItemEditor IsNot Nothing Then
            ActiveItemEditor.AnimateImages(True)
            AddHandler ActiveItemEditor.DirtinessChanged, AddressOf ActiveItemEditor_DirtinessChanged
            AddHandler ActiveItemEditor.IssuesChanged, AddressOf ActiveItemEditor_IssuesChanged
        End If
        ActiveItemEditor_DirtinessChanged(Me, EventArgs.Empty)
        ActiveItemEditor_IssuesChanged(Me, EventArgs.Empty)

        Dim pageRef As PageRef = Nothing
        If newTab IsNot Nothing Then
            pageRef = GetPageRef(newTab)
            If pageRef.PageContent = PageContent.Pony Then
                newTab.Controls.Add(preview)
                preview.RestartForPony(pageRef.PonyBase)
                preview.ShowPreview()
            Else
                pageRef = Nothing
            End If
        End If
        If pageRef Is Nothing Then preview.HidePreview()
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
        If ActiveItemEditor IsNot Nothing Then
            For Each issue In ActiveItemEditor.Issues
                IssuesGrid.Rows.Add(If(issue.Fatal, ErrorBitmap, WarningBitmap),
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
        Dim node = FindNode(ref.ToString())

        ref.Item = ActiveItemEditor.Item
        Documents.SelectedTab.Text = GetTabText(ref)
        node.Name = ref.ToString()
        node.Text = ref.Item.Name

        EditorStatus.Text = "Saved"

        Threading.ThreadPool.QueueUserWorkItem(Sub() ValidateBases())
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
        tab.Controls.Remove(preview)
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
        If preview.Parent IsNot Nothing Then preview.Parent.Controls.Remove(preview)
        preview.Dispose()
    End Sub
End Class
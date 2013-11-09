Imports System.Globalization
Imports System.IO

Public Class PonyEditorForm2
    Private Const ValidationPendingIndex = 0
    Private Const ValidationErrorIndex = 1
    Private Const ValidationWarningIndex = 2
    Private Const ValidationOkIndex = 3

    Private Shared ReadOnly ErrorBitmap As Bitmap = SystemIcons.Error.ToBitmap()
    Private Shared ReadOnly WarningBitmap As Bitmap = SystemIcons.Warning.ToBitmap()

    Private ReadOnly worker As New IdleWorker(Me)
    Private ponies As PonyCollection
    Private ReadOnly nodeLookup As New Dictionary(Of String, TreeNode)()

    Private preview As PonyPreview
    Private previewStartBehavior As Behavior
    Private contextRef As PageRef
    Private previousItemEditor As ItemEditorBase

    Private workingCount As Integer
    Private validationIndex As Integer
    Private ReadOnly validationGuard As New Object()

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
        Private _item As IPonyIniSourceable
        Public Property Item As IPonyIniSourceable
            Get
                Return _item
            End Get
            Set(value As IPonyIniSourceable)
                If value IsNot Nothing AndAlso Not _pageContent.IsItem() Then
                    Throw New InvalidOperationException("This PageRef may not refer to an Item.")
                End If
                _item = Argument.EnsureNotNull(value, "value")
            End Set
        End Property
        Public Sub New()
            _pageContent = DesktopPonies.PageContent.Ponies
        End Sub
        Public Sub New(ponyBase As PonyBase)
            _ponyBase = Argument.EnsureNotNull(ponyBase, "ponyBase")
            _pageContent = DesktopPonies.PageContent.Pony
        End Sub
        Public Sub New(ponyBase As PonyBase, pageContent As PageContent)
            _ponyBase = Argument.EnsureNotNull(ponyBase, "ponyBase")
            If Not pageContent.IsItem() AndAlso Not pageContent.IsItemCollection() Then
                Throw New ArgumentException("pageContent must refer to an item or item collection.", "pageContent")
            End If
            _pageContent = pageContent
        End Sub
        Public Sub New(ponyBase As PonyBase, pageContent As PageContent, item As IPonyIniSourceable)
            _ponyBase = Argument.EnsureNotNull(ponyBase, "ponyBase")
            If Not pageContent.IsItem() Then Throw New ArgumentException("pageContent must refer to an item.", "pageContent")
            _pageContent = pageContent
            _item = Argument.EnsureNotNull(item, "item")
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
            Return GetItemEditor(Documents.SelectedTab)
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
                             DocumentsView.TreeViewNodeSorter = Comparer.Create(Of TreeNode)(
                                 Function(x, y)
                                     ' Get page ref & content - we know both nodes should have same content.
                                     Dim refX = GetPageRef(x)
                                     If refX.PageContent = PageContent.Pony Then
                                         ' Sort ponies by directory name.
                                         Return String.Compare(
                                             refX.PonyBase.Directory, GetPageRef(y).PonyBase.Directory, StringComparison.OrdinalIgnoreCase)
                                     Else
                                         ' Leave other nodes in their insertion order.
                                         Return x.Index.CompareTo(y.Index)
                                     End If
                                 End Function)

                             EnableWaitCursor(True)
                             Dim images = New ImageList()
                             images.Images.Add(SystemIcons.Question)
                             images.Images.Add(SystemIcons.Error)
                             images.Images.Add(SystemIcons.Warning)
                             images.Images.Add(SystemIcons.Application)
                             DocumentsView.ImageList = images
                         End Sub)
        Threading.ThreadPool.QueueUserWorkItem(Sub() LoadBases())
    End Sub

    Private Sub ClearBases()
        If Documents.TabCount > 0 Then Throw New InvalidOperationException("Cannot clear bases with documents open.")
        nodeLookup.Clear()
        DocumentsView.Nodes.Clear()
    End Sub

    Private Sub LoadBases(Optional initialPonyToFocus As String = Nothing)
        worker.QueueTask(Sub()
                             EditorStatus.Text = "Loading..."
                             EditorProgressBar.Value = 0
                             EditorProgressBar.Style = ProgressBarStyle.Continuous
                             EditorProgressBar.Visible = True
                         End Sub)

        Dim poniesNode As TreeNode = Nothing
        worker.QueueTask(Sub()
                             poniesNode = New TreeNode("Ponies") With
                                          {.Tag = New PageRef()}
                             DocumentsView.Nodes.Add(poniesNode)
                             nodeLookup(poniesNode.Name) = poniesNode
                         End Sub)

        ponies = New PonyCollection(
            False,
            Sub(count) worker.QueueTask(Sub() EditorProgressBar.Maximum = count),
            Sub(base) worker.QueueTask(
                Sub()
                    AddPonyBaseToDocumentsView(base)
                    EditorProgressBar.Value += 1
                End Sub))
        worker.QueueTask(AddressOf DocumentsView.Sort)
        worker.QueueTask(Sub()
                             poniesNode.Expand()
                             If preview IsNot Nothing Then preview.Dispose()
                             preview = New PonyPreview(ponies)
                             EditorStatus.Text = "Ready"
                             EditorProgressBar.Value = 1
                             EditorProgressBar.Maximum = 1
                             EditorProgressBar.Style = ProgressBarStyle.Marquee
                             EditorProgressBar.Visible = False
                             UseWaitCursor = False
                             Enabled = True
                             Dim focusPending = True
                             If initialPonyToFocus IsNot Nothing Then
                                 Dim pony = ponies.Bases.FirstOrDefault(Function(pb) pb.Directory = initialPonyToFocus)
                                 If pony IsNot Nothing Then
                                     Dim node = FindNode(New PageRef(pony).ToString())
                                     If node IsNot Nothing Then
                                         DocumentsView.SelectedNode = node
                                         focusPending = False
                                     End If
                                 End If
                             End If
                             If focusPending Then DocumentsView.Focus()
                         End Sub)
        worker.WaitOnAllTasks()
        ValidateBases()
    End Sub

    Private Sub AddPonyBaseToDocumentsView(base As PonyBase)
        If base.Directory = PonyBase.RandomDirectory Then Return

        Dim ponyBaseRef = New PageRef(base)
        Dim ponyBaseNode = New TreeNode(base.Directory) With
                           {.Tag = ponyBaseRef, .Name = ponyBaseRef.ToString()}
        DocumentsView.Nodes(0).Nodes.Add(ponyBaseNode)
        nodeLookup(ponyBaseNode.Name) = ponyBaseNode

        Dim behaviorsRef = New PageRef(base, PageContent.Behaviors)
        Dim behaviorsNode = New TreeNode("Behaviors") With
                           {.Tag = behaviorsRef, .Name = behaviorsRef.ToString()}
        ponyBaseNode.Nodes.Add(behaviorsNode)
        nodeLookup(behaviorsNode.Name) = behaviorsNode
        Dim effectsRef = New PageRef(base, PageContent.Effects)
        Dim effectsNode = New TreeNode("Effects") With
                              {.Tag = effectsRef, .Name = effectsRef.ToString()}
        ponyBaseNode.Nodes.Add(effectsNode)
        nodeLookup(effectsNode.Name) = effectsNode
        Dim speechesRef = New PageRef(base, PageContent.Speeches)
        Dim speechesNode = New TreeNode("Speeches") With
                           {.Tag = speechesRef, .Name = speechesRef.ToString()}
        ponyBaseNode.Nodes.Add(speechesNode)
        nodeLookup(speechesNode.Name) = speechesNode
        Dim interactionsRef = New PageRef(base, PageContent.Interactions)
        Dim interactionsNode = New TreeNode("Interactions") With
                           {.Tag = interactionsRef, .Name = interactionsRef.ToString()}
        ponyBaseNode.Nodes.Add(interactionsNode)
        nodeLookup(interactionsNode.Name) = interactionsNode

        For Each behavior In base.Behaviors
            AddItemNode(base, behavior, behaviorsNode)
        Next

        For Each effect In base.Effects
            AddItemNode(base, effect, effectsNode)
        Next

        For Each interaction In base.Interactions
            AddItemNode(base, interaction, interactionsNode)
        Next

        For Each speech In base.Speeches
            AddItemNode(base, speech, speechesNode)
        Next
    End Sub

    Private Function AddItemNode(base As PonyBase, item As IPonyIniSourceable, parentCollectionNode As TreeNode) As TreeNode
        Dim ref = New PageRef(base, PageContentExtensions.FromSource(item), item)
        Dim node = New TreeNode(item.Name) With {.Tag = ref, .Name = ref.ToString()}
        parentCollectionNode.Nodes.Add(node)
        nodeLookup(node.Name) = node
        Return node
    End Function

    Private Sub ValidateBases()
        Dim initialValidationIndex = Threading.Interlocked.Increment(validationIndex)
        SyncLock validationGuard
            Dim resetNodeIndices As Action(Of TreeNodeCollection) =
                Sub(nodes As TreeNodeCollection)
                For Each node As TreeNode In nodes
                    node.ImageIndex = ValidationPendingIndex
                    resetNodeIndices(node.Nodes)
                Next
            End Sub
            worker.QueueTask(Sub() resetNodeIndices(DocumentsView.Nodes))
            For Each base In ponies.Bases
                If initialValidationIndex <> validationIndex Then Return
                ValidateBase(base)
            Next
        End SyncLock
    End Sub

    Private Sub ValidateBase(base As PonyBase)
        Dim validateBehavior = Function(behavior As Behavior)
                                   Dim b As Behavior = Nothing
                                   Return behavior.TryLoad(
                                       behavior.SourceIni,
                                       Path.Combine(EvilGlobals.InstallLocation, PonyBase.RootDirectory, base.Directory),
                                       base, b, Nothing).Combine(
                                       If(b.GetReferentialIssues(ponies).Length = 0, ParseResult.Success, ParseResult.Fallback))
                               End Function
        Dim behaviorsValid = ValidateItems(base, base.Behaviors, validateBehavior, PageContent.Behaviors, PageContent.Behavior)
        Dim validateEffect = Function(effect As EffectBase)
                                 Dim e As EffectBase = Nothing
                                 Return EffectBase.TryLoad(
                                     effect.SourceIni,
                                     Path.Combine(EvilGlobals.InstallLocation, PonyBase.RootDirectory, base.Directory),
                                     base, e, Nothing).Combine(
                                     If(e.GetReferentialIssues(ponies).Length = 0, ParseResult.Success, ParseResult.Fallback))
                             End Function
        Dim effectsValid = ValidateItems(base, base.Effects, validateEffect, PageContent.Effects, PageContent.Effect)
        Dim validateSpeech = Function(speech As Speech)
                                 Return speech.TryLoad(
                                     speech.SourceIni,
                                     Path.Combine(EvilGlobals.InstallLocation, PonyBase.RootDirectory, base.Directory),
                                     Nothing, Nothing)
                             End Function
        Dim speechesValid = ValidateItems(base, base.Speeches, validateSpeech, PageContent.Speeches, PageContent.Speech)
        Dim validateInteraction = Function(interaction As InteractionBase)
                                      Dim i As InteractionBase = Nothing
                                      Return InteractionBase.TryLoad(
                                          interaction.SourceIni,
                                          i, Nothing).Combine(
                                          If(i.GetReferentialIssues(ponies).Length = 0, ParseResult.Success, ParseResult.Fallback))
                                  End Function
        Dim interactionsValid = ValidateItems(base, base.Interactions, validateInteraction,
                                              PageContent.Interactions, PageContent.Interaction)

        worker.QueueTask(Sub()
                             Dim ref = New PageRef(base)
                             Dim node = FindNode(ref.ToString())
                             Dim result = behaviorsValid.Combine(effectsValid).Combine(speechesValid).Combine(interactionsValid)
                             Dim index = If(result = ParseResult.Success, ValidationOkIndex,
                                            If(result = ParseResult.Fallback, ValidationWarningIndex, ValidationErrorIndex))
                             node.ImageIndex = index
                             node.SelectedImageIndex = index
                         End Sub)
    End Sub

    Private Function ValidateItems(Of T As IPonyIniSourceable)(base As PonyBase, items As IEnumerable(Of T),
                                                               validateItem As Func(Of T, ParseResult),
                                                               content As PageContent, childContent As PageContent) As ParseResult
        Dim itemsValid = ParseResult.Success
        For Each item In items
            Dim ref = New PageRef(base, childContent, item)
            Dim itemValid = validateItem(item)
            itemsValid = itemsValid.Combine(itemValid)
            worker.QueueTask(Sub()
                                 Dim node = FindNode(ref.ToString())
                                 Dim index = If(itemValid = ParseResult.Success, ValidationOkIndex,
                                                If(itemValid = ParseResult.Fallback, ValidationWarningIndex, ValidationErrorIndex))
                                 node.ImageIndex = index
                                 node.SelectedImageIndex = index
                             End Sub)
        Next
        worker.QueueTask(Sub()
                             Dim ref = New PageRef(base, content)
                             Dim node = FindNode(ref.ToString())
                             Dim index = If(itemsValid = ParseResult.Success, ValidationOkIndex,
                                            If(itemsValid = ParseResult.Fallback, ValidationWarningIndex, ValidationErrorIndex))
                             node.ImageIndex = index
                             node.SelectedImageIndex = index
                         End Sub)
        Return itemsValid
    End Function

    Private Function FindNode(name As String) As TreeNode
        Dim node As TreeNode = Nothing
        If nodeLookup.TryGetValue(name, node) Then
            Return node
        Else
            Return DocumentsView.Nodes.Find(name, True).SingleOrDefault()
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
                Return pageRef.PonyBase.Directory & ": " & If(pageRef.Item Is Nothing, "[New]", pageRef.Item.Name.ToString())
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

    Private Shared Function GetItemEditor(tab As TabPage) As ItemEditorBase
        If tab Is Nothing OrElse tab.Controls.Count = 0 Then
            Return Nothing
        Else
            Return TryCast(tab.Controls(0), ItemEditorBase)
        End If
    End Function

    Private Function ShowDialogOverPreview(show As Func(Of DialogResult)) As DialogResult
        Dim wasVisible = preview.PreviewVisible
        If wasVisible Then preview.HidePreview()
        Dim result = show()
        If wasVisible Then preview.ShowPreview()
        Return result
    End Function

    Private Sub DocumentsView_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DocumentsView.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = True
            OpenTabFromNode(DocumentsView.SelectedNode)
        End If
    End Sub

    Private Sub DocumentsView_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles DocumentsView.NodeMouseDoubleClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            OpenTabFromNode(e.Node)
        End If
    End Sub

    Private Sub OpenTabFromNode(node As TreeNode)
        contextRef = GetPageRef(node)
        If contextRef.PageContent.IsItem() Then OpenTab(contextRef)
    End Sub

    Private Sub DocumentsView_NodeMouseClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles DocumentsView.NodeMouseClick
        If e.Button = Windows.Forms.MouseButtons.Right Then
            contextRef = GetPageRef(e.Node)
            Select Case contextRef.PageContent
                Case PageContent.Pony
                    PonyNodeContextMenu.Show(DocumentsView, e.Location)
            End Select
        End If
    End Sub

    Private Sub DocumentsView_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles DocumentsView.AfterSelect
        contextRef = GetPageRef(e.Node)
        EnableEditorToolStripButtons(contextRef.PageContent <> PageContent.Ponies, Documents.SelectedTab IsNot Nothing)
    End Sub

    Private Sub NewPonyButton_Click(sender As Object, e As EventArgs) Handles NewPonyButton.Click
        If Documents.TabCount > 0 Then
            If ShowDialogOverPreview(
                Function() MessageBox.Show(
                    Me, "All documents must be closed before a new pony can be created. Close them now?",
                    "Close Documents?", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)) <> DialogResult.OK Then Return
            For Each t In Documents.TabPages.Cast(Of TabPage)().ToArray()
                RemoveTab(t)
            Next
        End If
        Using dialog = New NewPonyDialog2()
            If dialog.ShowDialog(Me) = Windows.Forms.DialogResult.OK Then
                ClearBases()
                Threading.ThreadPool.QueueUserWorkItem(Sub() LoadBases(dialog.NewDirectory))
            End If
        End Using
    End Sub

    Private Sub Preview_Click(sender As Object, e As EventArgs) Handles PreviewContextMenuItem.Click, PreviewButton.Click
        If contextRef.PageContent = PageContent.Behavior Then
            previewStartBehavior = DirectCast(contextRef.Item, Behavior)
        End If
        OpenTab(New PageRef(contextRef.PonyBase))
    End Sub

    Private Sub DetailsMenuItem_Click(sender As Object, e As EventArgs) Handles DetailsContextMenuItem.Click, DetailsMenuItem.Click
        Dim contextBase = contextRef.PonyBase
        Dim contextBaseHasOpenDocuments = Documents.TabPages.Cast(Of TabPage).Any(
            Function(tab) Object.ReferenceEquals(GetPageRef(tab).PonyBase, contextBase))
        Using dialog As New PonyDetailsDialog(contextBase, Not contextBaseHasOpenDocuments)
            Dim ref = New PageRef(contextBase)
            Dim refOriginalName = ref.ToString()
            If ShowDialogOverPreview(Function() dialog.ShowDialog(Me)) = DialogResult.OK Then
                Dim refNewName = ref.ToString()
                If refOriginalName <> refNewName Then
                    Dim node = FindNode(refOriginalName)
                    node.Name = refNewName
                    node.Text = contextBase.Directory
                    RenameNodes(DocumentsView.Nodes)
                    DocumentsView.Sort()
                    For Each t As TabPage In Documents.TabPages
                        Dim editor = GetItemEditor(t)
                        If editor IsNot Nothing Then editor.RefreshReferentialIssues()
                    Next
                    EditorStatus.Text = "Saved"
                    Threading.ThreadPool.QueueUserWorkItem(Sub() ValidateBases())
                End If
            End If
        End Using
    End Sub

    Private Sub RenameNodes(nodeCollection As TreeNodeCollection)
        For Each node As TreeNode In nodeCollection
            RenameNodes(node.Nodes)
            nodeLookup.Remove(node.Name)
            node.Name = DirectCast(node.Tag, PageRef).ToString()
            nodeLookup.Add(node.Name, node)
        Next
    End Sub

    Private Sub NewBehaviorMenuItem_Click(sender As Object, e As EventArgs) Handles NewBehaviorMenuItem.Click
        OpenTab(New PageRef(contextRef.PonyBase, PageContent.Behavior))
    End Sub

    Private Sub NewEffectMenuItem_Click(sender As Object, e As EventArgs) Handles NewEffectMenuItem.Click
        OpenTab(New PageRef(contextRef.PonyBase, PageContent.Effect))
    End Sub

    Private Sub NewInteractionMenuItem_Click(sender As Object, e As EventArgs) Handles NewInteractionMenuItem.Click
        OpenTab(New PageRef(contextRef.PonyBase, PageContent.Interaction))
    End Sub

    Private Sub NewSpeechMenuItem_Click(sender As Object, e As EventArgs) Handles NewSpeechMenuItem.Click
        OpenTab(New PageRef(contextRef.PonyBase, PageContent.Speech))
    End Sub

    Private Sub BehaviorsMenuItem_Click(sender As Object, e As EventArgs) Handles BehaviorsContextMenuItem.Click, BehaviorsMenuItem.Click
        OpenTab(New PageRef(contextRef.PonyBase, PageContent.Behaviors))
    End Sub

    Private Sub EffectsMenuItem_Click(sender As Object, e As EventArgs) Handles EffectsContextMenuItem.Click, EffectsMenuItem.Click
        OpenTab(New PageRef(contextRef.PonyBase, PageContent.Effects))
    End Sub

    Private Sub InteractionsMenuItem_Click(sender As Object, e As EventArgs) Handles InteractionsContextMenuItem.Click, InteractionsMenuItem.Click
        OpenTab(New PageRef(contextRef.PonyBase, PageContent.Interactions))
    End Sub

    Private Sub SpeechesMenuItem_Click(sender As Object, e As EventArgs) Handles SpeechesContextMenuItem.Click, SpeechesMenuItem.Click
        OpenTab(New PageRef(contextRef.PonyBase, PageContent.Speeches))
    End Sub

    Private Sub OpenContextMenuItem_Click(sender As Object, e As EventArgs) Handles OpenContextMenuItem.Click
        OpenTab(contextRef)
    End Sub

    Private Sub OpenTab(pageRef As PageRef)
        Const MaxTabs = 50
        If Documents.TabPages.Count >= MaxTabs Then
            ShowDialogOverPreview(
                Function() MessageBox.Show(Me, "You already have " & MaxTabs & " documents opens. Please close some before opening more.",
                                           "Document Limit Reached", MessageBoxButtons.OK, MessageBoxIcon.Warning))
            Return
        End If

        Dim pageRefKey = pageRef.ToString()
        Dim tab = Documents.TabPages.Item(pageRefKey)

        Dim isFirstTab = False
        If tab Is Nothing Then
            Dim childControl As Control = Nothing
            If pageRef.PageContent = PageContent.Pony Then
                childControl = preview
            ElseIf pageRef.PageContent.IsItemCollection() Then
                ' TODO.
                Dim viewer As ItemsViewerBase = Nothing
                Select Case pageRef.PageContent
                    Case PageContent.Behaviors
                        viewer = New BehaviorsViewer()
                    Case PageContent.Effects
                        viewer = New BehaviorsViewer()
                    Case PageContent.Interactions
                        viewer = New BehaviorsViewer()
                    Case PageContent.Speeches
                        viewer = New BehaviorsViewer()
                End Select
                QueueWorkItem(Sub() viewer.LoadFor(pageRef.PonyBase))
                AddHandler viewer.PreviewRequested, AddressOf Viewer_PreviewRequested
                AddHandler viewer.EditRequested, AddressOf Viewer_EditRequested
                childControl = viewer
            ElseIf pageRef.PageContent.IsItem() Then
                Dim editor As ItemEditorBase = Nothing
                Select Case pageRef.PageContent
                    Case PageContent.Behavior
                        editor = New BehaviorEditor()
                    Case PageContent.Effect
                        editor = New EffectEditor()
                    Case PageContent.Interaction
                        editor = New InteractionEditor()
                    Case PageContent.Speech
                        editor = New SpeechEditor()
                End Select
                If pageRef.Item Is Nothing Then
                    Dim newItem As IPonyIniSourceable = Nothing
                    If pageRef.PageContent = PageContent.Behavior Then
                        newItem = New Behavior(pageRef.PonyBase)
                    ElseIf pageRef.PageContent = PageContent.Effect Then
                        newItem = New EffectBase(pageRef.PonyBase)
                    ElseIf pageRef.PageContent = PageContent.Interaction Then
                        newItem = New InteractionBase() With {.InitiatorName = pageRef.PonyBase.Directory}
                    ElseIf pageRef.PageContent = PageContent.Speech Then
                        newItem = New Speech()
                    End If
                    QueueWorkItem(Sub() editor.NewItem(pageRef.PonyBase, newItem))
                Else
                    QueueWorkItem(Sub() editor.LoadItem(pageRef.PonyBase, pageRef.Item))
                End If
                childControl = editor
            End If
            If childControl IsNot Nothing Then
                childControl.Tag = pageRef
                childControl.Dock = DockStyle.Fill
                tab = New ItemTabPage() With {.Name = pageRefKey, .Text = GetTabText(pageRef), .Tag = pageRef}
                tab.Controls.Add(childControl)
                isFirstTab = Documents.TabPages.Count = 0
                Documents.TabPages.Add(tab)
            End If
        End If

        If tab IsNot Nothing Then
            Documents.SelectedTab = tab
            If isFirstTab Then SwitchTab(tab)
            DocumentsView.Select()
            Dim node = FindNode(pageRefKey)
            If node IsNot Nothing Then DocumentsView.SelectedNode = node
        End If
    End Sub

    Private Sub Documents_Selected(sender As Object, e As TabControlEventArgs) Handles Documents.Selected
        SwitchTab(e.TabPage)
    End Sub

    Private Sub SwitchTab(newTab As TabPage)
        If previousItemEditor IsNot Nothing Then
            RemoveHandler previousItemEditor.IssuesChanged, AddressOf ActiveItemEditor_IssuesChanged
            RemoveHandler previousItemEditor.DirtinessChanged, AddressOf ActiveItemEditor_DirtinessChanged
            previousItemEditor.AnimateImages(False)
        End If

        If ActiveItemEditor IsNot Nothing Then
            previousItemEditor = ActiveItemEditor
            ActiveItemEditor.AnimateImages(True)
            AddHandler ActiveItemEditor.DirtinessChanged, AddressOf ActiveItemEditor_DirtinessChanged
            AddHandler ActiveItemEditor.IssuesChanged, AddressOf ActiveItemEditor_IssuesChanged
        End If
        ActiveItemEditor_DirtinessChanged(Me, EventArgs.Empty)
        ActiveItemEditor_IssuesChanged(Me, EventArgs.Empty)

        If newTab IsNot Nothing Then
            contextRef = GetPageRef(newTab)
            If contextRef.PageContent = PageContent.Pony Then
                newTab.Controls.Add(preview)
                preview.RestartForPony(contextRef.PonyBase, previewStartBehavior)
                previewStartBehavior = Nothing
                preview.ShowPreview()
                PreviewRestartButton.Visible = True
                Focus()
            End If
        Else
            contextRef = GetPageRef(DocumentsView.SelectedNode)
        End If
        If newTab Is Nothing OrElse contextRef.PageContent <> PageContent.Pony Then
            preview.HidePreview()
            PreviewRestartButton.Visible = False
        End If
        EnableEditorToolStripButtons(contextRef.PageContent <> PageContent.Ponies, Documents.TabPages.Count > 0)
    End Sub

    Private Sub Viewer_PreviewRequested(sender As Object, e As ItemsViewerBase.ViewerItemEventArgs)
        Dim ref = DirectCast(DirectCast(sender, Control).Tag, PageRef)
        previewStartBehavior = DirectCast(e.Item, Behavior)
        OpenTab(New PageRef(ref.PonyBase))
    End Sub

    Private Sub Viewer_EditRequested(sender As Object, e As ItemsViewerBase.ViewerItemEventArgs)
        Dim ref = DirectCast(DirectCast(sender, Control).Tag, PageRef)
        OpenTab(New PageRef(ref.PonyBase, ref.PageContent.ItemCollectionToItem(), e.Item))
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

    Private Sub EnableEditorToolStripButtons(ponyEnable As Boolean, itemEnable As Boolean)
        PreviewButton.Enabled = ponyEnable
        NewItemButton.Enabled = ponyEnable
        ItemsButton.Enabled = ponyEnable
        CloseTabButton.Enabled = itemEnable
        CloseAllTabsButton.Enabled = itemEnable
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveItemButton.Click
        If Not ActiveItemEditor.SaveItem() Then Return
        Dim ref = GetPageRef(Documents.SelectedTab)
        Dim node = FindNode(ref.ToString())

        If node Is Nothing Then
            Dim parentNode = FindNode(New PageRef(ref.PonyBase, ref.PageContent.ItemToItemCollection()).ToString())
            node = AddItemNode(ref.PonyBase, ActiveItemEditor.Item, parentNode)
        End If

        nodeLookup.Remove(node.Name)
        ref.Item = ActiveItemEditor.Item
        Documents.SelectedTab.Text = GetTabText(ref)
        node.Name = ref.ToString()
        node.Text = ref.Item.Name
        nodeLookup.Add(node.Name, node)

        EditorStatus.Text = "Saved"

        Threading.ThreadPool.QueueUserWorkItem(Sub() ValidateBases())
    End Sub

    Private Sub CloseTabButton_Click(sender As Object, e As EventArgs) Handles CloseTabButton.Click
        RemoveTab(Documents.SelectedTab)
    End Sub

    Private Sub CloseAllTabsButton_Click(sender As Object, e As EventArgs) Handles CloseAllTabsButton.Click
        For Each t In Documents.TabPages.Cast(Of TabPage)().ToArray()
            QueueWorkItem(Sub() RemoveTab(t))
        Next
    End Sub

    Private Sub PreviewRestartButton_Click(sender As Object, e As EventArgs) Handles PreviewRestartButton.Click
        PreviewRestartButton.Enabled = False

        Dim shouldRestorePreview = False
        If Documents.SelectedTab IsNot Nothing AndAlso Documents.SelectedTab.Controls.Contains(preview) Then
            Documents.SelectedTab.Controls.Remove(preview)
            shouldRestorePreview = True
        End If
        If preview IsNot Nothing Then preview.Dispose()
        preview = New PonyPreview(ponies)
        preview.Dock = DockStyle.Fill
        If shouldRestorePreview Then Documents.SelectedTab.Controls.Add(preview)
        preview.RestartForPony(contextRef.PonyBase)
        preview.ShowPreview()

        PreviewRestartButton.Enabled = True
        Focus()
    End Sub

    Private Sub RemoveTab(tab As TabPage)
        Dim index = Documents.TabPages.IndexOf(Documents.SelectedTab)
        tab.Controls.Remove(preview)
        If index > 0 Then Documents.SelectedIndex = index - 1
        Documents.TabPages.Remove(tab)
        tab.Dispose()
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

    Private Sub PonyEditorForm2_Disposed(sender As Object, e As EventArgs) Handles MyBase.Disposed
        If preview.Parent IsNot Nothing Then preview.Parent.Controls.Remove(preview)
        preview.Dispose()
    End Sub
End Class
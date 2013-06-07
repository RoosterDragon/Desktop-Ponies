Imports System.IO

Public Class PonyEditorForm2
    Private ReadOnly idleWorker As IdleWorker = idleWorker.CurrentThreadWorker
    Private ReadOnly bases As New Dictionary(Of String, PonyBase)
    Private activeTab As TabPage

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
        Public Sub New(ponyBase As MutablePonyBase, pageContent As PageContent)
            _ponyBase = ponyBase
            _pageContent = pageContent
        End Sub
    End Class

    Private ReadOnly Property ActiveItemEditor As ItemEditorBase
        Get
            Return If(activeTab Is Nothing, Nothing, DirectCast(activeTab.Controls(0), ItemEditorBase))
        End Get
    End Property

    Public Sub New()
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
        Dim ponyBaseDirectories = Directory.GetDirectories(Path.Combine(Options.InstallLocation, PonyBase.RootDirectory))
        Array.Sort(ponyBaseDirectories, StringComparer.CurrentCultureIgnoreCase)

        idleWorker.QueueTask(Sub() EditorProgressBar.Maximum = ponyBaseDirectories.Length)
        Dim poniesNode As TreeNode = Nothing
        idleWorker.QueueTask(Sub()
                                 poniesNode = New TreeNode("Ponies") With
                                              {.Tag = New PageRef(Nothing, PageContent.Ponies)}
                                 DocumentsView.Nodes.Add(poniesNode)
                                 poniesNode.Expand()
                             End Sub)
        For Each directory In ponyBaseDirectories
            Dim ponyBase As New MutablePonyBase(directory)
            bases.Add(ponyBase.Directory, ponyBase)
            idleWorker.QueueTask(Sub()
                                     Dim ponyBaseNode = New TreeNode(ponyBase.Directory) With
                                                        {.Tag = New PageRef(ponyBase, PageContent.Pony)}
                                     poniesNode.Nodes.Add(ponyBaseNode)

                                     Dim behaviorsNode = New TreeNode("Behaviors") With
                                                        {.Tag = New PageRef(ponyBase, PageContent.Behaviors)}
                                     ponyBaseNode.Nodes.Add(behaviorsNode)
                                     Dim effectsNode = New TreeNode("Effects") With
                                                           {.Tag = New PageRef(ponyBase, PageContent.Effects)}
                                     ponyBaseNode.Nodes.Add(effectsNode)
                                     Dim speechesNode = New TreeNode("Speeches") With
                                                        {.Tag = New PageRef(ponyBase, PageContent.Speeches)}
                                     ponyBaseNode.Nodes.Add(speechesNode)

                                     For Each behavior In ponyBase.Behaviors
                                         behaviorsNode.Nodes.Add(
                                             New TreeNode(behavior.Name) With {.Tag = New PageRef(ponyBase, PageContent.Behavior)})
                                     Next

                                     For Each effect In ponyBase.Effects
                                         effectsNode.Nodes.Add(
                                             New TreeNode(effect.Name) With {.Tag = New PageRef(ponyBase, PageContent.Effect)})
                                     Next

                                     For Each speech In ponyBase.SpeakingLines
                                         speechesNode.Nodes.Add(
                                             New TreeNode(speech.Name) With {.Tag = New PageRef(ponyBase, PageContent.Speech)})
                                     Next

                                     EditorProgressBar.Value += 1
                                 End Sub)
        Next
        idleWorker.QueueTask(Sub()
                                 EditorStatus.Text = "Ready"
                                 EditorProgressBar.Value = 0
                                 EditorProgressBar.Maximum = 0
                                 DocumentsView.TopNode.Expand()
                                 UseWaitCursor = False
                                 Enabled = True
                                 DocumentsView.Focus()
                             End Sub)
    End Sub

    Private Shared Function GetTabText(pageRef As PageRef, itemName As String) As String
        Select Case pageRef.PageContent
            Case PageContent.Ponies
                Return itemName
            Case PageContent.Pony
                Return pageRef.PonyBase.Directory
            Case PageContent.Behaviors, PageContent.Effects, PageContent.Speeches
                Return pageRef.PonyBase.Directory & " - " & itemName
            Case PageContent.Behavior, PageContent.Effect, PageContent.Speech
                Return pageRef.PonyBase.Directory & ": " & itemName
            Case Else
                Throw New System.ComponentModel.InvalidEnumArgumentException("Unknown Content in pageRef")
        End Select
    End Function

    Private Sub DocumentsView_NodeMouseDoubleClick(sender As Object, e As TreeNodeMouseClickEventArgs) Handles DocumentsView.NodeMouseDoubleClick
        OpenTab(e.Node)
    End Sub

    Private Sub DocumentsView_KeyPress(sender As Object, e As KeyPressEventArgs) Handles DocumentsView.KeyPress
        If e.KeyChar = ChrW(Keys.Enter) Then
            e.Handled = OpenTab(DocumentsView.SelectedNode)
        End If
    End Sub

    Private Function OpenTab(node As TreeNode) As Boolean
        If node Is Nothing Then Return False

        Dim tab = Documents.TabPages.Item(node.FullPath)
        Dim itemName = node.Text

        If tab Is Nothing Then
            Dim pageRef = DirectCast(node.Tag, PageRef)
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
                idleWorker.QueueTask(Sub() editor.LoadItem(pageRef.PonyBase, itemName))
                editor.Dock = DockStyle.Fill
                tab = New ItemTabPage() With {.Name = node.FullPath, .Text = GetTabText(pageRef, itemName)}
                tab.Controls.Add(editor)
                Documents.TabPages.Add(tab)
            End If
        End If

        If tab IsNot Nothing Then
            Documents.SelectedTab = tab
            If activeTab Is Nothing Then SwitchTab(tab)
            DocumentsView.Select()
            DocumentsView.SelectedNode = node
            Return True
        End If

        Return False
    End Function

    Private Sub Documents_Selected(sender As Object, e As TabControlEventArgs) Handles Documents.Selected
        SwitchTab(e.TabPage)
    End Sub

    Private Sub SwitchTab(newTab As TabPage)
        If activeTab IsNot Nothing Then
            RemoveHandler ActiveItemEditor.DirtinessChanged, AddressOf ActiveItemEditor_DirtinessChanged
            ActiveItemEditor.AnimateImages(False)
        End If

        activeTab = newTab

        ActiveItemEditor.AnimateImages(True)
        AddHandler ActiveItemEditor.DirtinessChanged, AddressOf ActiveItemEditor_DirtinessChanged
        ActiveItemEditor_DirtinessChanged(Me, EventArgs.Empty)
    End Sub

    Private Sub ActiveItemEditor_DirtinessChanged(sender As Object, e As EventArgs)
        SaveButton.Enabled = ActiveItemEditor.IsItemDirty
        SaveButton.ToolTipText = If(ActiveItemEditor.IsItemDirty, "Save This Item", "No Save Needed")
    End Sub

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        ActiveItemEditor.SaveItem()
        EditorStatus.Text = "Saved"
    End Sub
End Class
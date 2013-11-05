<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CommunityDialog
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(CommunityDialog))
        Me.DownloadLink = New System.Windows.Forms.LinkLabel()
        Me.CloseButton = New System.Windows.Forms.Button()
        Me.LinkContextMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenLinkMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyLinkMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LinkToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.PatchLink = New System.Windows.Forms.LinkLabel()
        Me.LayoutTable = New System.Windows.Forms.TableLayoutPanel()
        Me.CertificateTextBox = New System.Windows.Forms.TextBox()
        Me.LinksTable = New System.Windows.Forms.TableLayoutPanel()
        Me.InfoLabel = New System.Windows.Forms.Label()
        Me.PatchTextBox = New System.Windows.Forms.TextBox()
        Me.LinkContextMenu.SuspendLayout()
        Me.LayoutTable.SuspendLayout()
        Me.LinksTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'DownloadLink
        '
        Me.DownloadLink.AutoSize = True
        Me.DownloadLink.Location = New System.Drawing.Point(3, 3)
        Me.DownloadLink.Margin = New System.Windows.Forms.Padding(3)
        Me.DownloadLink.Name = "DownloadLink"
        Me.DownloadLink.Size = New System.Drawing.Size(55, 13)
        Me.DownloadLink.TabIndex = 0
        Me.DownloadLink.TabStop = True
        Me.DownloadLink.Text = "Download"
        Me.LinkToolTip.SetToolTip(Me.DownloadLink, "Download the latest version of Desktop Ponies.")
        '
        'CloseButton
        '
        Me.CloseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CloseButton.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.CloseButton.Location = New System.Drawing.Point(444, 227)
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.Size = New System.Drawing.Size(75, 23)
        Me.CloseButton.TabIndex = 1
        Me.CloseButton.Text = "Close"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'LinkContextMenu
        '
        Me.LinkContextMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenLinkMenuItem, Me.CopyLinkMenuItem})
        Me.LinkContextMenu.Name = "LinkContextMenu"
        Me.LinkContextMenu.Size = New System.Drawing.Size(131, 48)
        '
        'OpenLinkMenuItem
        '
        Me.OpenLinkMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OpenLinkMenuItem.Name = "OpenLinkMenuItem"
        Me.OpenLinkMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.OpenLinkMenuItem.Text = "Open URL"
        '
        'CopyLinkMenuItem
        '
        Me.CopyLinkMenuItem.Name = "CopyLinkMenuItem"
        Me.CopyLinkMenuItem.Size = New System.Drawing.Size(130, 22)
        Me.CopyLinkMenuItem.Text = "Copy URL"
        '
        'LinkToolTip
        '
        Me.LinkToolTip.AutoPopDelay = 5000
        Me.LinkToolTip.InitialDelay = 0
        Me.LinkToolTip.ReshowDelay = 100
        '
        'PatchLink
        '
        Me.PatchLink.AutoSize = True
        Me.PatchLink.Location = New System.Drawing.Point(3, 22)
        Me.PatchLink.Margin = New System.Windows.Forms.Padding(3)
        Me.PatchLink.Name = "PatchLink"
        Me.PatchLink.Size = New System.Drawing.Size(85, 13)
        Me.PatchLink.TabIndex = 1
        Me.PatchLink.TabStop = True
        Me.PatchLink.Text = "Download patch"
        Me.LinkToolTip.SetToolTip(Me.PatchLink, "Download a patch to update you to the latest version of desktop ponies.")
        '
        'LayoutTable
        '
        Me.LayoutTable.ColumnCount = 1
        Me.LayoutTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.LayoutTable.Controls.Add(Me.CertificateTextBox, 0, 4)
        Me.LayoutTable.Controls.Add(Me.DownloadLink, 0, 0)
        Me.LayoutTable.Controls.Add(Me.LinksTable, 0, 3)
        Me.LayoutTable.Controls.Add(Me.PatchLink, 0, 1)
        Me.LayoutTable.Controls.Add(Me.PatchTextBox, 0, 2)
        Me.LayoutTable.Location = New System.Drawing.Point(12, 12)
        Me.LayoutTable.Name = "LayoutTable"
        Me.LayoutTable.RowCount = 5
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.Size = New System.Drawing.Size(507, 209)
        Me.LayoutTable.TabIndex = 0
        '
        'CertificateTextBox
        '
        Me.CertificateTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CertificateTextBox.Location = New System.Drawing.Point(3, 148)
        Me.CertificateTextBox.Multiline = True
        Me.CertificateTextBox.Name = "CertificateTextBox"
        Me.CertificateTextBox.ReadOnly = True
        Me.CertificateTextBox.Size = New System.Drawing.Size(501, 58)
        Me.CertificateTextBox.TabIndex = 4
        Me.CertificateTextBox.Text = resources.GetString("CertificateTextBox.Text")
        '
        'LinksTable
        '
        Me.LinksTable.AutoScroll = True
        Me.LinksTable.BackColor = System.Drawing.SystemColors.ControlDark
        Me.LinksTable.ColumnCount = 1
        Me.LinksTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.LinksTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.LinksTable.Controls.Add(Me.InfoLabel, 0, 0)
        Me.LinksTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LinksTable.Location = New System.Drawing.Point(3, 118)
        Me.LinksTable.Name = "LinksTable"
        Me.LinksTable.RowCount = 1
        Me.LinksTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LinksTable.Size = New System.Drawing.Size(501, 24)
        Me.LinksTable.TabIndex = 3
        '
        'InfoLabel
        '
        Me.InfoLabel.AutoSize = True
        Me.InfoLabel.Location = New System.Drawing.Point(3, 3)
        Me.InfoLabel.Margin = New System.Windows.Forms.Padding(3)
        Me.InfoLabel.Name = "InfoLabel"
        Me.InfoLabel.Size = New System.Drawing.Size(270, 13)
        Me.InfoLabel.TabIndex = 0
        Me.InfoLabel.Text = "Here are some community websites you may like to visit:"
        '
        'PatchTextBox
        '
        Me.PatchTextBox.Dock = System.Windows.Forms.DockStyle.Fill
        Me.PatchTextBox.Location = New System.Drawing.Point(3, 41)
        Me.PatchTextBox.Multiline = True
        Me.PatchTextBox.Name = "PatchTextBox"
        Me.PatchTextBox.ReadOnly = True
        Me.PatchTextBox.Size = New System.Drawing.Size(501, 71)
        Me.PatchTextBox.TabIndex = 2
        Me.PatchTextBox.Text = resources.GetString("PatchTextBox.Text")
        '
        'CommunityDialog
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CancelButton = Me.CloseButton
        Me.ClientSize = New System.Drawing.Size(531, 262)
        Me.Controls.Add(Me.LayoutTable)
        Me.Controls.Add(Me.CloseButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.Name = "CommunityDialog"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Community - Desktop Ponies"
        Me.LinkContextMenu.ResumeLayout(False)
        Me.LayoutTable.ResumeLayout(False)
        Me.LayoutTable.PerformLayout()
        Me.LinksTable.ResumeLayout(False)
        Me.LinksTable.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DownloadLink As System.Windows.Forms.LinkLabel
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents LinkContextMenu As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents OpenLinkMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyLinkMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LinkToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents PatchLink As System.Windows.Forms.LinkLabel
    Friend WithEvents LayoutTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents LinksTable As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents InfoLabel As System.Windows.Forms.Label
    Friend WithEvents PatchTextBox As System.Windows.Forms.TextBox
    Friend WithEvents CertificateTextBox As System.Windows.Forms.TextBox
End Class

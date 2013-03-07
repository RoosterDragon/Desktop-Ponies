<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class DesktopControlForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(disposing As Boolean)
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
        Me.ReturnButton = New System.Windows.Forms.Button()
        Me.PonyComboBox = New System.Windows.Forms.ComboBox()
        Me.MenuStripPanel = New System.Windows.Forms.Panel()
        Me.LayoutTable = New System.Windows.Forms.TableLayoutPanel()
        Me.LayoutTable.SuspendLayout()
        Me.SuspendLayout()
        '
        'ReturnButton
        '
        Me.ReturnButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ReturnButton.Location = New System.Drawing.Point(3, 3)
        Me.ReturnButton.Name = "ReturnButton"
        Me.ReturnButton.Size = New System.Drawing.Size(178, 23)
        Me.ReturnButton.TabIndex = 0
        Me.ReturnButton.Text = "Return to Menu"
        Me.ReturnButton.UseVisualStyleBackColor = True
        '
        'PonyComboBox
        '
        Me.PonyComboBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PonyComboBox.DisplayMember = "Name"
        Me.PonyComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.PonyComboBox.Location = New System.Drawing.Point(3, 32)
        Me.PonyComboBox.Name = "PonyComboBox"
        Me.PonyComboBox.Size = New System.Drawing.Size(178, 21)
        Me.PonyComboBox.TabIndex = 1
        '
        'MenuStripPanel
        '
        Me.MenuStripPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.MenuStripPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.MenuStripPanel.Location = New System.Drawing.Point(3, 59)
        Me.MenuStripPanel.Name = "MenuStripPanel"
        Me.MenuStripPanel.Size = New System.Drawing.Size(178, 200)
        Me.MenuStripPanel.TabIndex = 2
        '
        'LayoutTable
        '
        Me.LayoutTable.AutoSize = True
        Me.LayoutTable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.LayoutTable.ColumnCount = 1
        Me.LayoutTable.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.LayoutTable.Controls.Add(Me.ReturnButton, 0, 0)
        Me.LayoutTable.Controls.Add(Me.PonyComboBox, 0, 1)
        Me.LayoutTable.Controls.Add(Me.MenuStripPanel, 0, 2)
        Me.LayoutTable.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LayoutTable.Location = New System.Drawing.Point(0, 0)
        Me.LayoutTable.Name = "LayoutTable"
        Me.LayoutTable.RowCount = 3
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.LayoutTable.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.LayoutTable.Size = New System.Drawing.Size(184, 262)
        Me.LayoutTable.TabIndex = 0
        '
        'DesktopControlForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(184, 262)
        Me.ControlBox = False
        Me.Controls.Add(Me.LayoutTable)
        Me.MinimumSize = New System.Drawing.Size(200, 95)
        Me.Name = "DesktopControlForm"
        Me.Text = "Pony Control - Desktop Ponies"
        Me.LayoutTable.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ReturnButton As System.Windows.Forms.Button
    Friend WithEvents PonyComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents MenuStripPanel As System.Windows.Forms.Panel
    Friend WithEvents LayoutTable As System.Windows.Forms.TableLayoutPanel
End Class

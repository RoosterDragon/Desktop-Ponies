<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ItemEditorBase
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.Source = New System.Windows.Forms.TextBox()
        Me.PropertiesPanel = New System.Windows.Forms.Panel()
        Me.SourceTextTimer = New System.Windows.Forms.Timer(Me.components)
        Me.SuspendLayout()
        '
        'Source
        '
        Me.Source.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Source.Location = New System.Drawing.Point(3, 277)
        Me.Source.Name = "Source"
        Me.Source.Size = New System.Drawing.Size(494, 20)
        Me.Source.TabIndex = 1
        '
        'PropertiesPanel
        '
        Me.PropertiesPanel.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PropertiesPanel.AutoScroll = True
        Me.PropertiesPanel.Location = New System.Drawing.Point(3, 3)
        Me.PropertiesPanel.Name = "PropertiesPanel"
        Me.PropertiesPanel.Size = New System.Drawing.Size(494, 268)
        Me.PropertiesPanel.TabIndex = 0
        '
        'SourceTextTimer
        '
        '
        'ItemEditorBase
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.PropertiesPanel)
        Me.Controls.Add(Me.Source)
        Me.Enabled = False
        Me.Name = "ItemEditorBase"
        Me.Size = New System.Drawing.Size(500, 300)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Protected WithEvents Source As System.Windows.Forms.TextBox
    Protected WithEvents PropertiesPanel As System.Windows.Forms.Panel
    Friend WithEvents SourceTextTimer As System.Windows.Forms.Timer

End Class

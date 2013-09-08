<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FileSelector
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
        Me.FilePathComboBox = New System.Windows.Forms.ComboBox()
        Me.FilePathDeleteButton = New System.Windows.Forms.Button()
        Me.FilePathChooseButton = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'FilePathComboBox
        '
        Me.FilePathComboBox.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FilePathComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.FilePathComboBox.Items.AddRange(New Object() {"[None]"})
        Me.FilePathComboBox.Location = New System.Drawing.Point(0, 1)
        Me.FilePathComboBox.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.FilePathComboBox.Name = "FilePathComboBox"
        Me.FilePathComboBox.Size = New System.Drawing.Size(328, 21)
        Me.FilePathComboBox.TabIndex = 0
        '
        'FilePathDeleteButton
        '
        Me.FilePathDeleteButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FilePathDeleteButton.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FilePathDeleteButton.ForeColor = System.Drawing.Color.Firebrick
        Me.FilePathDeleteButton.Location = New System.Drawing.Point(370, 0)
        Me.FilePathDeleteButton.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.FilePathDeleteButton.Name = "FilePathDeleteButton"
        Me.FilePathDeleteButton.Size = New System.Drawing.Size(30, 23)
        Me.FilePathDeleteButton.TabIndex = 2
        Me.FilePathDeleteButton.Text = "X"
        Me.FilePathDeleteButton.UseVisualStyleBackColor = True
        '
        'FilePathChooseButton
        '
        Me.FilePathChooseButton.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FilePathChooseButton.Location = New System.Drawing.Point(334, 0)
        Me.FilePathChooseButton.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
        Me.FilePathChooseButton.Name = "FilePathChooseButton"
        Me.FilePathChooseButton.Size = New System.Drawing.Size(30, 23)
        Me.FilePathChooseButton.TabIndex = 1
        Me.FilePathChooseButton.Text = "..."
        Me.FilePathChooseButton.UseVisualStyleBackColor = True
        '
        'FileSelector
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.FilePathDeleteButton)
        Me.Controls.Add(Me.FilePathComboBox)
        Me.Controls.Add(Me.FilePathChooseButton)
        Me.Name = "FileSelector"
        Me.Size = New System.Drawing.Size(400, 23)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents FilePathComboBox As System.Windows.Forms.ComboBox
    Public WithEvents FilePathDeleteButton As System.Windows.Forms.Button
    Public WithEvents FilePathChooseButton As System.Windows.Forms.Button

End Class

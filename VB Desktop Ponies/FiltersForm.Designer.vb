<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FiltersForm
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
        Me.Filters_Box = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Save_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Filters_Box
        '
        Me.Filters_Box.Location = New System.Drawing.Point(39, 69)
        Me.Filters_Box.Multiline = True
        Me.Filters_Box.Name = "Filters_Box"
        Me.Filters_Box.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.Filters_Box.Size = New System.Drawing.Size(399, 235)
        Me.Filters_Box.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(78, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(314, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Edit the possible pony categories/tags/filters below (one per line)."
        '
        'Save_Button
        '
        Me.Save_Button.Location = New System.Drawing.Point(311, 355)
        Me.Save_Button.Name = "Save_Button"
        Me.Save_Button.Size = New System.Drawing.Size(75, 23)
        Me.Save_Button.TabIndex = 2
        Me.Save_Button.Text = "Change"
        Me.Save_Button.UseVisualStyleBackColor = True
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Location = New System.Drawing.Point(81, 355)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(75, 23)
        Me.Cancel_Button.TabIndex = 3
        Me.Cancel_Button.Text = "CANCEL"
        Me.Cancel_Button.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(3, 325)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(468, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Note that you still need to save your settings on the options menu to make the ch" & _
            "ange permanent!"
        '
        'Filters_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.AppWorkspace
        Me.ClientSize = New System.Drawing.Size(469, 400)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Cancel_Button)
        Me.Controls.Add(Me.Save_Button)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Filters_Box)
        Me.Name = "Filters_Form"
        Me.Text = "Edit Filters/Tags..."
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Filters_Box As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Save_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class

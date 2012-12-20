﻿Imports System.Windows.Forms

Public Class NewPonyDialog

    Dim new_pony_path As String = ""

    Private m_editor As PonyEditor
    Public Sub New(editor As PonyEditor)
        InitializeComponent()
        m_editor = editor
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click

        If Trim(Name_Textbox.Text) = "" Then
            MsgBox("You must enter a name for the new pony.")
            Exit Sub
        End If

        If InStr(Name_Textbox.Text, ",") <> 0 Then
            MsgBox("The pony's name can't have a comma in it.")
            Exit Sub
        End If

        For Each Pony In Main.Instance.SelectablePonies
            If LCase(Pony.Directory) = LCase(Trim(Name_Textbox.Text)) Then
                MsgBox("A pony with this name already exists!  Please select another name or rename the other pony.")
                Exit Sub
            End If
        Next

        If InStr(Name_Textbox.Text, "{") <> 0 Then
            MsgBox("The pony's name can't have a { in it.")
            Exit Sub
        End If

        If InStr(Name_Textbox.Text, "}") <> 0 Then
            MsgBox("The pony's name can't have a } in it.")
            Exit Sub
        End If

        If m_editor.PreviewPony.Behaviors.Count = 0 Then
            MsgBox("You need to create at least one new behavior before the pony can be saved.")
            Exit Sub
        End If

        Try
            m_editor.SavePony(new_pony_path)
        Catch ex As Exception
            MsgBox("Unable to save pony! Details: " & ex.Message)
        End Try
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub New_Pony_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Name_Textbox.Text = ""
        Name_Textbox.Enabled = True
        Right_ImageBox.Image = Nothing
    End Sub

    Private Sub First_Behavior_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles First_Behavior_Button.Click

        If Trim(Name_Textbox.Text) = "" Then
            MsgBox("You must enter a name for the new pony first.")
            Exit Sub
        End If

        Name_Textbox.Enabled = False

        Try
            new_pony_path = Options.InstallLocation & System.IO.Path.DirectorySeparatorChar & Name_Textbox.Text & System.IO.Path.DirectorySeparatorChar
            If My.Computer.FileSystem.DirectoryExists(new_pony_path) Then
                Throw New Exception("Path already exists! Won't overwrite whatever is there: " & new_pony_path)
            End If
            My.Computer.FileSystem.CreateDirectory(new_pony_path)
            m_editor.PreviewPony.Base.Directory = new_pony_path
        Catch ex As Exception
            MsgBox("Unable to create directory for new pony:  " & ex.Message)
            Name_Textbox.Enabled = True
            Exit Sub
        End Try

        m_editor.PreviewPony.Base.Name = Name_Textbox.Text

        Using form = New NewBehaviorDialog(m_editor)
            form.ShowDialog()
        End Using

        For Each behavior In m_editor.PreviewPony.Behaviors
            If System.IO.File.Exists(behavior.RightImagePath) Then
                Right_ImageBox.Image = Image.FromFile(behavior.RightImagePath)
                Exit For
            End If
        Next

    End Sub

End Class
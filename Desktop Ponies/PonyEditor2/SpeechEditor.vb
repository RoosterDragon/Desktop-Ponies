Imports System.IO

Friend Class SpeechEditor
    Private Shadows Property Edited As Speech
        Get
            Return DirectCast(MyBase.Edited, Speech)
        End Get
        Set(value As Speech)
            MyBase.Edited = value
        End Set
    End Property
    Protected Overrides ReadOnly Property Collection As System.Collections.IList
        Get
            Return CType(Base.Speeches, Collections.IList)
        End Get
    End Property
    Protected Overrides ReadOnly Property ItemTypeName As String
        Get
            Return "speech"
        End Get
    End Property

    Public Sub New()
        InitializeComponent()
        AddHandler NameTextBox.KeyPress, AddressOf IgnoreQuoteCharacter
        AddHandler NameTextBox.TextChanged, Sub() UpdateProperty(Sub() Edited.Name = NameTextBox.Text)
        AddHandler LineTextBox.TextChanged, Sub() UpdateProperty(Sub() Edited.Text = LineTextBox.Text)
        AddHandler SoundFileSelector.FilePathSelected,
            Sub() UpdateProperty(Sub()
                                     Dim filePath = If(SoundFileSelector.FilePath = Nothing,
                                                       Nothing, Path.Combine(PonyBasePath, SoundFileSelector.FilePath))
                                     Edited.SoundFile = filePath
                                 End Sub)
        AddHandler RandomCheckBox.CheckedChanged, Sub() UpdateProperty(Sub() Edited.Skip = Not RandomCheckBox.Checked)
        AddHandler GroupNumber.ValueChanged, Sub() UpdateProperty(Sub() Edited.Group = CInt(GroupNumber.Value))
    End Sub

    Public Overrides Sub NewItem(name As String)
        ' TODO.
    End Sub

    Public Overrides Sub LoadItem()
        Dim sounds =
            Directory.GetFiles(PonyBasePath, "*.mp3").Concat(Directory.GetFiles(PonyBasePath, "*.ogg")).
            Select(Function(filePath) Path.GetFileName(filePath)).ToArray()
        ReplaceItemsInComboBox(SoundFileSelector.FilePathComboBox, sounds, True)
        If Edited.SoundFile IsNot Nothing Then
            SelectItemElseAddItem(SoundFileSelector.FilePathComboBox, Path.GetFileName(Edited.SoundFile))
        End If
    End Sub

    Protected Overrides Sub SourceTextChanged()
        Dim s As Speech = Nothing
        Speech.TryLoad(Source.Text, PonyBasePath, s, ParseIssues)
        OnIssuesChanged(EventArgs.Empty)
        Edited = s

        NameTextBox.Text = Edited.Name
        LineTextBox.Text = Edited.Text
        RandomCheckBox.Checked = Not Edited.Skip
        GroupNumber.Value = Edited.Group
    End Sub
End Class

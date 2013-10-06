Imports System.Runtime.CompilerServices

Public Enum PageContent
    Ponies
    Pony
    Behaviors
    Behavior
    Effects
    Effect
    Interactions
    Interaction
    Speeches
    Speech
End Enum

Public Module PageContentExtensions
    <Extension>
    Public Function IsItemCollection(content As PageContent) As Boolean
        Return content = PageContent.Behaviors OrElse
            content = PageContent.Effects OrElse
            content = PageContent.Interactions OrElse
            content = PageContent.Speeches
    End Function

    <Extension>
    Public Function IsItem(content As PageContent) As Boolean
        Return content = PageContent.Behavior OrElse
            content = PageContent.Effect OrElse
            content = PageContent.Interaction OrElse
            content = PageContent.Speech
    End Function

    <Extension>
    Public Function ItemCollectionToItem(content As PageContent) As PageContent
        If Not content.IsItemCollection() Then Throw New ArgumentException("content must be an item collection.", "content")
        Return DirectCast(content + 1, PageContent)
    End Function

    <Extension>
    Public Function ItemToItemCollection(content As PageContent) As PageContent
        If Not content.IsItem() Then Throw New ArgumentException("content must be an item.", "content")
        Return DirectCast(content - 1, PageContent)
    End Function
End Module
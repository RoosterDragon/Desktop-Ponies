Imports System.Runtime.CompilerServices

Public Module LinqSpecialized
    ''' <summary>
    ''' Returns the only element of a list that satisfies a specified condition or a default value if no such element exists or more than
    ''' one element satisfies the condition.
    ''' </summary>
    ''' <typeparam name="TSource">The type of the elements of source.</typeparam>
    ''' <param name="source">A <see cref="T:System.Collections.Generic.List`1"/> to return the only element of.</param>
    ''' <param name="predicate">A function to test an element for a condition.</param>
    ''' <returns>The single element of the list that satisfies the condition, or default(TSource) if no such element is found or more than
    ''' one such element is found.</returns>
    ''' <exception cref="T:System.ArgumentNullException"><paramref name="source"/> is null.-or-<paramref name="predicate"/> is null.
    ''' </exception>
    <Extension>
    Public Function OnlyOrDefault(Of TSource)(source As List(Of TSource), predicate As Func(Of TSource, Boolean)) As TSource
        Argument.EnsureNotNull(source, "source")
        Argument.EnsureNotNull(predicate, "predicate")
        Dim count = 0
        Dim result As TSource
        For Each element In source
            If predicate(element) Then
                count += 1
                If count >= 2 Then Return Nothing
                result = element
            End If
        Next
        Return result
    End Function
End Module

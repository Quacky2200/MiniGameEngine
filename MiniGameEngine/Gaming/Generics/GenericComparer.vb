Public Class GenericComparer(Of T)
    Implements IComparer(Of T)

    Public Delegate Function CompareDelegate(x As T, y As T) As Integer
    Private Property Comparer As CompareDelegate

    Public Sub New(Comparer As CompareDelegate)
        Me.Comparer = Comparer
    End Sub

    Public Function Compare(x As T, y As T) As Integer Implements IComparer(Of T).Compare
        If (Not IsNothing(Comparer)) Then
            Return Comparer(x, y)
        Else
            Throw New NullReferenceException()
        End If
    End Function
End Class

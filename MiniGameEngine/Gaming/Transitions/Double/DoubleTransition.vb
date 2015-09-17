Namespace Transitions
    Public Class DoubleTransition
        Inherits Transition
        Public Sub New(A As Double, B As Double)
            MyBase.New(A, B)
        End Sub
        Public Sub New(StartPosition As Double, A As Double, B As Double, Optional Duration As TimeSpan = Nothing, Optional Enabled As Boolean = False)
            MyBase.New(StartPosition, A, B, Duration, Enabled)
        End Sub
        Public Overrides Function ConvertFromRaw(rawValues() As Double) As Object
            Return rawValues(0)
        End Function

        Public Overrides Function ConvertToRaw(obj As Object) As Double()
            Return {obj}
        End Function
    End Class
End Namespace
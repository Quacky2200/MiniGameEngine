Namespace Transitions
    Public Class DoubleTransition
        Inherits Transition
        Public Sub New(A As Double, B As Double)
            MyBase.New(A, B)
        End Sub
        Public Sub New(StartValue As Double, EndValue As Double, Optional Duration As TimeSpan = Nothing, Optional Enabled As Boolean = True)
            MyBase.New(StartValue, EndValue, Duration, Enabled)
        End Sub

        Protected Overrides Function GetValue(StartValue As Object, EndValue As Object, T As Double) As Object
            Return EasingFunction(CDbl(StartValue), CDbl(EndValue), T)
        End Function
    End Class
End Namespace
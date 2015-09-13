Namespace Transitions
    Public Class RandomDoubleTransition
        Inherits DoubleTransition
        Public Property minDouble As Double
        Public Property maxDouble As Double
        Private random As New Random
        Public Sub New(minDouble As Double, maxDouble As Double)
            MyBase.New(minDouble, maxDouble)
            Me.minDouble = minDouble
            Me.maxDouble = maxDouble
        End Sub
        Public Sub New(currentDouble As Double, minDouble As Double, maxDouble As Double)
            MyBase.New(currentDouble, minDouble, maxDouble)
            Me.minDouble = minDouble
            Me.maxDouble = maxDouble
        End Sub
        Private Sub randomise(sender As Object) Handles Me.OnRepeat
            Me.A = Me.B
            Me.B = (random.NextDouble() * maxDouble) + minDouble
        End Sub
        Private Sub Reversed(oldDirection As TransitionDirection, newDirection As TransitionDirection) Handles Me.OnReverse
            If oldDirection = TransitionDirection.Forward Then
                Me.B = (random.NextDouble() * maxDouble) + minDouble
            Else
                Me.A = (random.NextDouble() * maxDouble) + minDouble
            End If
        End Sub
    End Class
End Namespace
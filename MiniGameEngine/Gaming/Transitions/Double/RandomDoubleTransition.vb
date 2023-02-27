Namespace Transitions
    Public Class RandomDoubleTransition
        Inherits DoubleTransition
        Public Property MinDouble As Double
        Public Property MaxDouble As Double

        Private ReadOnly Random As New Random()

        Public Sub New(MinDouble As Double, MaxDouble As Double)
            MyBase.New(MinDouble, MaxDouble)
            Me.MinDouble = MinDouble
            Me.MaxDouble = MaxDouble

            Randomise()
        End Sub

        Public Sub New(CurrentDouble As Double, MinDouble As Double, MaxDouble As Double)
            MyBase.New(CurrentDouble, CurrentDouble)
            Me.Value = CurrentDouble

            Me.MinDouble = MinDouble
            Me.MaxDouble = MaxDouble

            Randomise()
        End Sub

        Private Sub Randomise()
            Me.StartValue = If(Me.Reverse, Me.StartValue, Me.EndValue)
            Me.EndValue = (Random.NextDouble() * MaxDouble) + MinDouble
        End Sub

        Private Sub RandomDoubleTransition_OnRepeat(Sender As Object) Handles Me.OnRepeat
            Randomise()
        End Sub

        Private Sub RandomDoubleTransition_OnReverse(OldDirection As TransitionDirection, NewDirection As TransitionDirection) Handles Me.OnReverse
            If (NewDirection = TransitionDirection.Forward) Then Randomise()
        End Sub
    End Class
End Namespace
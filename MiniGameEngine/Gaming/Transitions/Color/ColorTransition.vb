Imports System.Drawing
Namespace Transitions
    Public Class ColorTransition
        Inherits Transition
        Public Sub New(StartValue As Color, EndValue As Color)
            MyBase.New(StartValue, EndValue)
        End Sub
        Public Sub New(StartValue As Color, EndValue As Color, Optional Duration As TimeSpan = Nothing, Optional Enabled As Boolean = True)
            MyBase.New(StartValue, EndValue, Duration, Enabled)
        End Sub

        Public Sub New()
        End Sub

        Private Function Limit(I As Double) As Integer
            Return Math.Min(255, Math.Max(0, Math.Round(I)))
        End Function

        Protected Overrides Function GetValue(StartValue As Object, EndValue As Object, T As Double) As Object
            Dim StartColor As Color = StartValue
            Dim EndColor As Color = EndValue

            Return Color.FromArgb(
                Limit(EasingFunction(StartColor.A, EndColor.A, T)),
                Limit(EasingFunction(StartColor.R, EndColor.R, T)),
                Limit(EasingFunction(StartColor.G, EndColor.G, T)),
                Limit(EasingFunction(StartColor.B, EndColor.B, T))
            )
        End Function
    End Class

End Namespace
Imports System.Drawing
Namespace Transitions
    Public Class PointTransition
        Inherits Transition

        Public Sub New(StartValue As Point, EndValue As Point)
            MyBase.New(StartValue, EndValue)
        End Sub

        Public Sub New(StartValue As Point, EndValue As Point, Optional Duration As TimeSpan = Nothing, Optional Enabled As Boolean = True)
            MyBase.New(StartValue, EndValue, Duration, Enabled)
        End Sub

        Protected Overrides Function GetValue(_StartValue As Object, _EndValue As Object, T As Double) As Object
            Dim StartValue As Point = _StartValue
            Dim EndValue As Point = _EndValue

            Return New Point() With {
                .X = Math.Round(EasingFunction(StartValue.X, EndValue.X, T)),
                .Y = Math.Round(EasingFunction(StartValue.Y, EndValue.Y, T))
            }
        End Function
    End Class
End Namespace
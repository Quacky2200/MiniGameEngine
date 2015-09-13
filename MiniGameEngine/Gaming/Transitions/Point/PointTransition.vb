Imports System.Drawing
Namespace Transitions
    Public Class PointTransition
        Inherits Transition(Of Point)
        Public Sub New(A As Point, B As Point)
            MyBase.New(A, B)
        End Sub
        Public Sub New(StartPosition As Point, A As Point, B As Point, Optional Duration As TimeSpan = Nothing, Optional Enabled As Boolean = False)
            MyBase.New(StartPosition, A, B, Duration, Enabled)
        End Sub
        Public Overrides Function ConvertFromRaw(rawValues() As Double) As Point
            Return New Point(CInt(rawValues(0)), CInt(rawValues(1)))
        End Function

        Public Overrides Function ConvertToRaw(obj As Point) As Double()
            Return {obj.X, obj.Y}
        End Function
    End Class
End Namespace
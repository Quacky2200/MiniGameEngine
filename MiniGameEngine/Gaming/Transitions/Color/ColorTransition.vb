Imports System.Drawing
Namespace Transitions
    Public Class ColorTransition
        Inherits Transition
        Public Sub New(A As Color, B As Color)
            MyBase.New(A, B)
        End Sub
        Public Sub New(StartPosition As Color, A As Color, B As Color, Optional Duration As TimeSpan = Nothing, Optional Enabled As Boolean = False)
            MyBase.New(StartPosition, A, B, Duration, Enabled)
        End Sub
        Public Overrides Function ConvertFromRaw(rawValues() As Double) As Object
            Dim a As Byte = CByte(Math.Min(255, Math.Max(0, rawValues(0))))
            Dim r As Byte = CByte(Math.Min(255, Math.Max(0, rawValues(1))))
            Dim g As Byte = CByte(Math.Min(255, Math.Max(0, rawValues(2))))
            Dim b As Byte = CByte(Math.Min(255, Math.Max(0, rawValues(3))))
            Return System.Drawing.Color.FromArgb(a, r, g, b)
        End Function

        Public Overrides Function ConvertToRaw(obj As Object) As Double()
            Return {obj.A, obj.R, obj.G, obj.B}
        End Function
    End Class

End Namespace
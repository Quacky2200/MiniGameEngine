Imports System.Drawing
Namespace Examples.Shapes.SineCircles
    Partial Public MustInherit Class SineCircleType
        Friend parent As SineCircle
        Public Sub New(parent As SineCircle)
            Me.parent = parent
        End Sub
        Public MustOverride Function getPath(Degree As Integer) As Point
    End Class
End Namespace
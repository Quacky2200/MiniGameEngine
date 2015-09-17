Imports System.Drawing
Namespace Examples.Shapes.SineCircles
    Public Class NormalSineCirclePathType
        Inherits SineCircleType
        Public Sub New(parent As SineCircle)
            MyBase.New(parent)
        End Sub
        Public Overrides Function getPath(Degree As Integer) As Point
            Dim radians As Double = Circle.getRadians(Degree)
            Dim x = (Me.parent.radius + Me.parent.depth * Math.Sin(Me.parent.frequency * (radians + Circle.getRadians(Me.parent.Rotation)))) * Math.Cos(radians) + Me.parent.Position.X
            Dim y = (Me.parent.radius + Me.parent.depth * Math.Sin(Me.parent.frequency * (radians + Circle.getRadians(Me.parent.Rotation)))) * Math.Sin(radians) + Me.parent.Position.Y
            Return New Point(CInt(x), CInt(y))
        End Function
    End Class
End Namespace
Imports System.Drawing
Namespace Examples.Shapes.SineCircles
    Public Class PiSineType
        Inherits SineCircleType
        Public Sub New(parent As SineCircle)
            MyBase.New(parent)
        End Sub
        Public Overrides Function getPath(Degree As Integer) As Point
            Dim tempradius = Me.parent.Radius + Me.parent.frequency * Math.Cos(Math.PI * Circle.GetRadians(Degree)) * Math.Cos(Math.PI + Me.parent.depth * Circle.GetRadians(Degree))
            Dim position = Circle.GetPoint(tempradius, Circle.GetRadians(Degree) + Circle.GetRadians(Me.parent.Rotation), Me.parent.Position)
            Return position
        End Function
    End Class
End Namespace
﻿Imports System.Drawing
Namespace Examples.Shapes.SineCircles
    Public Class DoubleSineCirclePathType
        Inherits SineCircleType
        Public Sub New(parent As SineCircle)
            MyBase.New(parent)
        End Sub
        Public Overrides Function getPath(Degree As Integer) As Point
            Dim tempradius = Me.parent.radius + Me.parent.frequency * Math.Sin(Me.parent.depth * Degree + Degree) * Math.Sin(Me.parent.depth * Degree - Degree)
            Dim position = Circle.getPoint(tempradius, Circle.getRadians(Degree) + Circle.getRadians(Me.parent.Rotation), Me.parent.Position)
            Return position
        End Function
    End Class
End Namespace
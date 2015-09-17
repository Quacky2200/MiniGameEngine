Imports MiniGameEngine.Transitions
Namespace Examples.Droplets
    Public Class SimpleEchoDroplets
        Inherits Droplets
        Public Sub New(Scene As Scene)
            MyBase.New(Scene)
        End Sub
        Dim last As Shapes.Circle = Nothing
        Friend Overloads Overrides Sub Spawn(ByVal i As Integer, ByRef currentCircle As Shapes.Circle)
            Dim currentMovement As New DoubleTransition(0, 0, 100, MovementDuration, True)
            Dim circle As Shapes.Circle = currentCircle
            Scene.add(circle.RadiusProperty, currentMovement, True, True)
            If currentCircle.Equals(last) Then
                Debug.Print("Same! ")
            End If
            last = currentCircle
            Threading.Thread.Sleep(100)
        End Sub
    End Class
End Namespace
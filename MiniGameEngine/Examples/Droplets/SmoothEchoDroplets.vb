Imports System.Drawing
Imports MiniGameEngine.Transitions
Namespace Examples.Droplets
    Public Class SmoothEchoDroplets

        Inherits Droplets
        Public Sub New(Scene As Scene)
            MyBase.New(Scene)
        End Sub

        Friend Overloads Overrides Sub Spawn(ByVal i As Integer, ByRef currentCircle As Shapes.Circle)
            Scene.add(currentCircle.RadiusProperty, New Transitions.DoubleTransition(1, dropletRadius) With {.Duration = MovementDuration, .Enabled = True}, True)
            Scene.add(currentCircle.ColorProperty, New Transitions.ColorTransition(dropletColor, Color.Transparent) With {.Duration = MovementDuration, .Enabled = True}, True, True)

            Threading.Thread.Sleep(20)
        End Sub
    End Class
End Namespace
Imports System.Drawing
Imports MiniGameEngine.Transitions
Namespace Examples.Droplets
    Public Class Waterdrops
        Inherits Droplets
        Public Property dropletFadeColor As Color = Color.Transparent
        Public Sub New(Scene As Scene)
            MyBase.New(Scene)
        End Sub
        Friend Overloads Overrides Sub Spawn(ByVal i As Integer, ByRef currentCircle As Shapes.Circle)
            Dim circleDissipate As New Transitions.ColorTransition(dropletColor, dropletFadeColor) With {.Enabled = True}
            Dim circleMovement As New DoubleTransition(0, 0, dropletRadius, MovementDuration, True)
            Dim circle As Shapes.Circle = currentCircle
            AddHandler circleDissipate.OnFinish, Sub(sender As Object)
                                                     Scene.remove(circle.RadiusProperty, circleMovement)
                                                     Scene.remove(circle.ColorProperty, circleDissipate)
                                                     Scene.remove(circle)
                                                 End Sub
            Scene.add(circle.RadiusProperty, circleMovement)
            Scene.add(circle.ColorProperty, circleDissipate)
            Threading.Thread.Sleep(0 + (50 * (dropletCount - i)))
        End Sub
    End Class
End Namespace
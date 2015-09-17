Imports MiniGameEngine.Transitions
Namespace Examples.Droplets
    Public Class Raindrops
        Inherits Waterdrops
        Public Sub New(Scene As Scene)
            MyBase.New(Scene)
        End Sub
        Friend Overloads Overrides Sub Spawn(ByVal i As Integer, ByRef currentCircle As Shapes.Circle)
            Dim circleDissipate As New Transitions.ColorTransition(dropletColor, dropletFadeColor) With {.Enabled = True, .Duration = TimeSpan.FromTicks(MovementDuration.Ticks + TimeSpan.FromMilliseconds(150 * i).Ticks)}
            Dim circleMovement As New DoubleTransition(0, 0, dropletRadius, TimeSpan.FromTicks(MovementDuration.Ticks + TimeSpan.FromMilliseconds(150 * i).Ticks), True)
            Dim circle As Shapes.Circle = currentCircle
            Scene.add(circle.RadiusProperty, circleMovement, True, False)
            Scene.add(circle.ColorProperty, circleDissipate, True, True)
            Threading.Thread.Sleep(1)
        End Sub
    End Class
End Namespace
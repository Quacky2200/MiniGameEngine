Imports MiniGameEngine.Transitions
Namespace Examples.Droplets
    Public Class Raindrops
        Inherits Waterdrops
        Public Sub New(Scene As Scene)
            MyBase.New(Scene)
        End Sub

        Friend Overloads Overrides Sub Spawn(ByVal i As Integer, ByRef CurrentCircle As Shapes.Circle)
            Dim DissipationRate As Double = 150 * (Math.Max(DropletRadius, 100) / 100)
            Dim CircleDissipate As New Transitions.ColorTransition(DropletColor, DropletFadeColor) With {
                .Enabled = True, .Duration = TimeSpan.FromTicks(MovementDuration.Ticks + TimeSpan.FromMilliseconds(DissipationRate * i).Ticks)
            }
            Dim CircleMovement As New DoubleTransition(0, 0, DropletRadius, TimeSpan.FromTicks(MovementDuration.Ticks + TimeSpan.FromMilliseconds(DissipationRate * i).Ticks), True)
            Dim Circle As Shapes.Circle = CurrentCircle
            Circle.AddTransition(Circle.RadiusProperty, CircleMovement, True, False)
            Circle.AddTransition(Circle.ColorProperty, CircleDissipate, True, True)
        End Sub
    End Class
End Namespace
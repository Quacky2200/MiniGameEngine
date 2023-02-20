Imports System.Drawing
Imports MiniGameEngine.Transitions
Namespace Examples.Droplets
    Public Class Waterdrops
        Inherits Droplets
        Public Property DropletFadeColor As Color = Color.Transparent
        Public Sub New(Scene As Scene)
            MyBase.New(Scene)
        End Sub
        Friend Overloads Overrides Sub Spawn(ByVal i As Integer, ByRef CurrentCircle As Shapes.Circle)
            Dim CircleDissapation As New Transitions.ColorTransition(DropletColor, DropletFadeColor) With {.Enabled = True}
            Dim CircleMovement As New DoubleTransition(0, 0, DropletRadius, MovementDuration, True)
            Dim Circle As Shapes.Circle = CurrentCircle
            Circle.AddTransition(Circle.RadiusProperty, CircleMovement, True)
            Circle.AddTransition(Circle.ColorProperty, CircleDissapation, True, True)
            Threading.Thread.Sleep(0 + (50 * (DropletCount - i)))
        End Sub
    End Class
End Namespace
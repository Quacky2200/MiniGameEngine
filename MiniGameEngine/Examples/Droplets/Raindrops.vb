Imports MiniGameEngine.Transitions
Imports MiniGameEngine.General.Color
Imports System.Drawing

Namespace Examples.Droplets
    Public Class Raindrops
        Inherits Waterdrops
        Public Sub New(Scene As Scene)
            MyBase.New(Scene)
        End Sub

        Friend Overloads Overrides Sub Spawn(ByVal i As Integer, ByRef Circle As Shapes.Circle)
            Dim DissipationRate As Double = 120 * (Math.Max(DropletRadius, 100) / 100)
            Dim DissipationDuration As TimeSpan = MovementDuration + TimeSpan.FromMilliseconds(DissipationRate * i)
            Dim DissipateColor = RGBHSL.ModifyBrightness(DropletColor, (DropletCount - i) / DropletCount)

            Dim DissipationDisperseTransition As New DoubleTransition(0, DropletRadius, DissipationDuration, True)
            Dim DissipateFadeTransition = New ColorTransition(DissipateColor, ColorTools.ModifyAlpha(DissipateColor, 0), DissipationDuration)

            Circle.ZIndex = i
            Circle.LineColor = Color.Transparent

            Circle.AddTransition(Circle.RadiusProperty, DissipationDisperseTransition, True, False)
            Circle.AddTransition(Circle.FillProperty, DissipateFadeTransition, True, True)
        End Sub
    End Class
End Namespace
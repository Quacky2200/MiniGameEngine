Imports MiniGameEngine.Transitions
Namespace Examples.Droplets
    Public Class SimpleEchoDroplets
        Inherits Droplets
        Public Sub New(Scene As Scene)
            MyBase.New(Scene)
        End Sub

        Friend Overloads Overrides Sub Spawn(ByVal i As Integer, ByRef CurrentCircle As Shapes.Circle)
            Dim CircleMovement As New DoubleTransition(0, 0, 100, MovementDuration, True)
            Dim Circle As Shapes.Circle = CurrentCircle
            Circle.AddTransition(Circle.RadiusProperty, CircleMovement, True, True)
            Threading.Thread.Sleep(100)
        End Sub
    End Class
End Namespace
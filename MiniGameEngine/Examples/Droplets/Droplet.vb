Imports System.Drawing
Imports MiniGameEngine.General.Threading
Imports MiniGameEngine.Transitions
Namespace Examples.Droplets
    Partial Public MustInherit Class Droplets
        Friend Property Scene As Scene
        Public Sub New(Scene As Scene)
            Me.Scene = Scene
        End Sub
        Public Property dropletCount As Integer = 5
        Public Property dropletRadius As Integer = 25
        Public Property MovementDuration As TimeSpan = TimeSpan.FromMilliseconds(800)
        Public Property dropletColor As Color = Color.White

        Public Sub Spawn(Center As Point)
            Dim RainbowColors() As Color = {
                Color.Red,
                Color.Green,
                Color.Blue,
                Color.Yellow,
                Color.Orange,
                Color.Purple
            }

            dropletRadius = Math.Floor((New Random()).NextDouble() * (100 - 15) + 15)
            dropletColor = Color.LightSkyBlue

            'Colors(Math.Floor((New Random()).NextDouble() * (Colors.Length)))

            For i = 1 To dropletCount
                Dim circle As New Shapes.Circle(Center, 1) With {.lineColor = dropletColor, .lineWidth = 1}
                Scene.add(circle)

                Spawn(i, circle)
            Next
        End Sub

        Friend MustOverride Sub Spawn(ByVal i As Integer, ByRef currentCircle As Shapes.Circle)
    End Class
End Namespace
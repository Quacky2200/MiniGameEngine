Imports System.Drawing
Imports MiniGameEngine.General.Color.RGBHSL
Imports MiniGameEngine.Transitions
Namespace Examples.Droplets
    Partial Public MustInherit Class Droplets
        Friend Property Scene As Scene
        Public Sub New(Scene As Scene)
            Me.Scene = Scene
        End Sub
        Public Property DropletCount As Integer = 5
        Public Property DropletRadius As Integer = 100
        Public Property DropletColor As Color = Color.AliceBlue

        Public Property MovementDuration As TimeSpan = TimeSpan.FromMilliseconds(800)
        Public Sub Spawn(Center As Point)
            For i = 1 To DropletCount
                Dim Circle As New Shapes.Circle(Center, 1) With {.LineColor = DropletColor}
                Spawn(i, Circle)
                Scene.AddGameObject(Circle)
            Next
        End Sub
        Friend MustOverride Sub Spawn(ByVal i As Integer, ByRef currentCircle As Shapes.Circle)
    End Class
End Namespace
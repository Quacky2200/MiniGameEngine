Imports System.Drawing
Imports MiniGameEngine.Transitions
Namespace Examples.Droplets
    Partial Public MustInherit Class Droplets
        Friend Property Scene As Scene
        Public Sub New(Scene As Scene)
            Me.Scene = Scene
        End Sub
        Public Property dropletCount As Integer = 5
        Public Property dropletRadius As Integer = 100
        Public Property MovementDuration As TimeSpan = TimeSpan.FromMilliseconds(800)
        Public Property dropletColor As Color = Color.White
        Public Sub Spawn(Center As Point)
            For i = 1 To dropletCount
                Dim circle As New Shapes.Circle(Center, 1) With {.lineColor = dropletColor}
                Scene.add(circle)
                Spawn(i, circle)
            Next
        End Sub
        Friend MustOverride Sub Spawn(ByVal i As Integer, ByRef currentCircle As Shapes.Circle)
    End Class
End Namespace
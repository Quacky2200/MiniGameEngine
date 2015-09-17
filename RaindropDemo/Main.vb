Imports MiniGameEngine
Imports MiniGameEngine.Examples.Droplets
Imports MiniGameEngine.Transitions
Imports MiniGameEngine.UI

Public Class Main

    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub

    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Game As New GameContainer(Me) With {.AutomaticallyPause = False}
        Dim Scene As New Examples.Scenes.RaindropDemo(Game)
        Game.add(Scene)
        Game.switchScenes(Of Examples.Scenes.RaindropDemo)()
    End Sub
End Class

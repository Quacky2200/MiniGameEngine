Imports MiniGameEngine
Imports MiniGameEngine.Examples.Droplets
Imports MiniGameEngine.Transitions
Imports MiniGameEngine.UI

Public Class Main

    Private Sub Main_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Game As New GameContainer(Me) With {.Clip = True}
        Dim EchoScene As New Examples.Scenes.EchoLocationDemo(Game)
        Game.add(EchoScene)
        Game.switchScenes(Of Examples.Scenes.EchoLocationDemo)()
    End Sub
End Class
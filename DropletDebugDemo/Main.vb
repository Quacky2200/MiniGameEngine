Imports MiniGameEngine

Public Class Main
    Private Sub Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Game As New GameContainer(Me) With {.Clip = False}
        Dim EchoScene As New Examples.Scenes.DropletDebugTest(Game)
        Game.AddScene(EchoScene)
        Game.SwitchScenes(Of Examples.Scenes.DropletDebugTest)()
    End Sub
End Class
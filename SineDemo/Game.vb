Option Strict Off
Imports MiniGameEngine
Public Class Game

    Private Sub Game_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim Game As New GameContainer(Me)
        Me.Icon = Icon.FromHandle(GameContainer.GetDefaultIcon().GetHicon())
        Game.Enabled = True
        Game.AddScene(New Examples.Scenes.SineDemo(Game))
        Game.SwitchScenes(Of Examples.Scenes.SineDemo)()
        Game.AutomaticallyPause = False
    End Sub
End Class


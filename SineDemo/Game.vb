Option Strict Off
Imports MiniGameEngine
Public Class Game

    Private Sub Game_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        End
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim b As New Bitmap(32, 32)
        Me.Icon = Icon.FromHandle(b.GetHicon())
        Dim Game As New GameContainer(Me)
        Game.add(New Examples.Scenes.SineDemo(Game))
        Game.switchScenes(Of Examples.Scenes.SineDemo)()
    End Sub
End Class


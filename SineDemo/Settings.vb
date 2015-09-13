Option Strict Off
Imports MiniGameEngine
Public Class Settings
    Private WithEvents Game As New Game("", 1024, 768) With {.FormBorderStyle = Windows.Forms.FormBorderStyle.FixedToolWindow, .StartPosition = FormStartPosition.CenterScreen}
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Visible = False
        Dim b As New Bitmap(32, 32)
        Game.Icon = Icon.FromHandle(b.GetHicon())
        Game.Show()
    End Sub
    Private Sub GameLoaded() Handles Game.Load
        Game.addScene(New MainGameScene(Game))
        Game.addScene(New SineDemo(Game))
        Game.switchScenes(Of MainGameScene)()
    End Sub
    Private Sub GameCloses() Handles Game.FormClosed
        End
    End Sub
End Class


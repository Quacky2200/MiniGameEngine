Imports MiniGameEngine.General.Threading
Namespace Examples.Scenes
    Public Class EchoLocationDemo
        Inherits Scene
        Public Sub New(Game As GameContainer)
            MyBase.New(Game)
            Game.Clip = False

        End Sub
        Dim Echo As New Examples.Droplets.SimpleEchoDroplets(Me)
        Public Overrides Sub MouseClick(MouseButton As Windows.Forms.MouseButtons)
            ThreadWork.Start(Sub() Echo.Spawn(lastknown))
        End Sub
        Dim lastknown As System.Drawing.Point
        Public Overrides Sub MouseMove(Location As Drawing.Point)
            lastknown = Location
        End Sub
        Public Overrides Sub Init()

        End Sub

        Public Overrides Sub Render(g As Drawing.Graphics)

        End Sub

        Public Overrides Sub Update(delta As Double)

        End Sub
    End Class
End Namespace

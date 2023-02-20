Imports MiniGameEngine.General.Threading
Namespace Examples.Scenes
    Public Class EchoLocationDemo
        Inherits Scene
        Public Sub New(Game As GameContainer)
            MyBase.New(Game)
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
    End Class
End Namespace

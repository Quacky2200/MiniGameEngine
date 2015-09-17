Imports System.Windows.Forms
Public Class Input
    Private WithEvents Listener As GameContainer
    Private WithEvents Window As Form
    Public Sub New(ByRef Listener As GameContainer, Optional Attached As Boolean = True)
        Me.Listener = Listener
        Me.Window = Listener.Window
        Me._Attached = Attached
    End Sub
    Private Property _Attached As Boolean
    Public Sub Attach()
        _Attached = True
    End Sub
    Public Sub Detach()
        _Attached = False
    End Sub
    Private Sub Me_KeyDown(sender As Object, e As KeyEventArgs) Handles Window.KeyDown
        If _Attached Then Listener.currentScene.KeyDown(e.KeyCode)
    End Sub
    Private Sub Me_KeyUp(sender As Object, e As KeyEventArgs) Handles Window.KeyUp
        If _Attached Then Listener.currentScene.KeyUp(e.KeyCode)
    End Sub
    Private Sub Me_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Window.KeyPress
        'Listener.currentScene.KeyPress(e.)
        'Throw New NotImplementedException()
    End Sub
    Dim dbl As Boolean = False
    Private Sub Me_MouseDown(sender As Object, e As MouseEventArgs) Handles Window.MouseDown
        If _Attached Then Listener.currentScene.MouseDown(e.Button)
    End Sub
    Private Sub Me_MouseUp(sender As Object, e As MouseEventArgs) Handles Window.MouseUp
        If _Attached Then Listener.currentScene.MouseUp(e.Button)
    End Sub
    Private Sub Me_MouseClick(sender As Object, e As MouseEventArgs) Handles Window.MouseClick
        If _Attached Then Listener.currentScene.MouseClick(e.Button)
    End Sub
    Private Sub Me_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Window.MouseDoubleClick
        dbl = True
        If _Attached Then Listener.currentScene.MouseDoubleClick(e.Button)
    End Sub
    Private Sub Me_MouseMove(sender As Object, e As MouseEventArgs) Handles Window.MouseMove
        If _Attached Then Listener.currentScene.MouseMove(e.Location)
    End Sub
End Class

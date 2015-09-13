Imports System.Windows.Forms
Public Class Input
    Private WithEvents Listener As Game
    Public Sub New(Listener As Game, Optional Attached As Boolean = True)
        Me.Listener = Listener
        Me._Attached = Attached
    End Sub
    Private Property _Attached As Boolean
    Public Sub Attach()
        _Attached = True
    End Sub
    Public Sub Detach()
        _Attached = False
    End Sub
    Private Sub Me_KeyDown(sender As Object, e As KeyEventArgs) Handles Listener.KeyDown
        If _Attached Then Listener.currentScene.KeyDown(e.KeyCode)
    End Sub
    Private Sub Me_KeyUp(sender As Object, e As KeyEventArgs) Handles Listener.KeyUp
        If _Attached Then Listener.currentScene.KeyUp(e.KeyCode)
    End Sub
    Private Sub Me_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Listener.KeyPress
        'Listener.currentScene.KeyPress(e.)
        Throw New NotImplementedException()
    End Sub
    Private Sub Me_MouseDown(sender As Object, e As MouseEventArgs) Handles Listener.MouseDown
        If _Attached Then Listener.currentScene.MouseDown(e.Button)
    End Sub
    Private Sub Me_MouseUp(sender As Object, e As MouseEventArgs) Handles Listener.MouseUp
        If _Attached Then Listener.currentScene.MouseUp(e.Button)
    End Sub
    Private Sub Me_MouseClick(sender As Object, e As MouseEventArgs) Handles Listener.MouseClick
        If _Attached Then Listener.currentScene.MouseClick(e.Button)
    End Sub
    Private Sub Me_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Listener.MouseDoubleClick
        If _Attached Then Listener.currentScene.MouseDoubleClick(e.Button)
    End Sub
    Private Sub Me_MouseMove(sender As Object, e As MouseEventArgs) Handles Listener.MouseMove
        If _Attached Then Listener.currentScene.MouseMove(e.Location)
    End Sub
End Class

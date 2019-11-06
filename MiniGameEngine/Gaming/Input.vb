Imports System.Windows.Forms
Public Class Input

    Private WithEvents Listener As GameContainer
    Private WithEvents Window As Form
    Private Property _Attached As Boolean
    Private CachedKey(2) As Object
    Const KEY_PRESS_MILLISECOND_REGISTER = 150

    Public Sub New(ByRef Listener As GameContainer, Optional Attached As Boolean = True)
        Me.Listener = Listener
        Me.Window = Listener.Window
        Me._Attached = Attached
    End Sub

    Public Sub Attach()
        _Attached = True
    End Sub

    Public Sub Detach()
        _Attached = False
    End Sub

    Private Sub Me_KeyDown(sender As Object, e As KeyEventArgs) Handles Window.KeyDown
        If _Attached Then
            CachedKey(0) = DateTime.Now.Ticks
            CachedKey(1) = e.KeyCode
            Listener.currentScene.KeyDown(e.KeyCode)
        End If
    End Sub

    Private Sub Me_KeyUp(sender As Object, e As KeyEventArgs) Handles Window.KeyUp
        If _Attached Then
            Dim diff As TimeSpan = DateTime.Now - New Date(CachedKey(0))
            If (diff.Milliseconds < KEY_PRESS_MILLISECOND_REGISTER) AndAlso CachedKey(1) = e.KeyCode Then
                CachedKey(0) = DateTime.Now.Ticks
                CachedKey(1) = Nothing
                Listener.currentScene.KeyPress(e.KeyCode)
            End If
            Listener.currentScene.KeyUp(e.KeyCode)
        End If
    End Sub

    Private Sub Me_KeyPress(sender As Object, e As KeyPressEventArgs) Handles Window.KeyPress
        ' Ignore this, I have implemented this manually myself!
    End Sub

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
        If _Attached Then Listener.currentScene.MouseDoubleClick(e.Button)
    End Sub

    Private Sub Me_MouseMove(sender As Object, e As MouseEventArgs) Handles Window.MouseMove
        If _Attached Then Listener.currentScene.MouseMove(e.Location)
    End Sub

    Private Sub Me_SizeChanged(sender As Object, e As Object) Handles Window.SizeChanged
        If _Attached Then Listener.currentScene.WindowSizeChange()
    End Sub
End Class

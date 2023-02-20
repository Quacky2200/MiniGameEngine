Imports System.Windows.Forms
Public Class Input
    Private WithEvents Listener As GameContainer
    Private WithEvents Window As Form
    Private Attached As Boolean

    Public Sub New(ByRef Listener As GameContainer, Optional Attached As Boolean = True)
        Me.Listener = Listener
        Me.Window = Listener.Window
        Me.Attached = Attached
    End Sub

    Public Sub Attach()
        Attached = True
    End Sub

    Public Sub Detach()
        Attached = False
    End Sub

    Private Sub Me_KeyDown(sender As Object, e As KeyEventArgs) Handles Window.KeyDown
        If Attached Then Listener.CurrentScene.KeyDown(e.KeyCode)
    End Sub

    Private Sub Me_KeyUp(sender As Object, e As KeyEventArgs) Handles Window.KeyUp
        If Attached Then Listener.CurrentScene.KeyUp(e.KeyCode)
    End Sub

    Private Sub Me_KeyPress(sender As Object, e As KeyPressEventArgs) ' Handles Window.KeyPress
        ' Not supported since KeyPress is used to filter KeyUp from functioning by modifying the KeyPressEventArgs.Handled property
        ' See https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.control.keypress?view=windowsdesktop-7.0 for more information
        ' If _Attached Then Listener.CurrentScene.KeyPress(???)
    End Sub

    Private Sub Me_MouseDown(sender As Object, e As MouseEventArgs) Handles Window.MouseDown
        If Attached Then Listener.CurrentScene.MouseDown(e.Button)
    End Sub

    Private Sub Me_MouseUp(sender As Object, e As MouseEventArgs) Handles Window.MouseUp
        If Attached Then Listener.CurrentScene.MouseUp(e.Button)
    End Sub

    Private Sub Me_MouseClick(sender As Object, e As MouseEventArgs) Handles Window.MouseClick
        If Attached Then Listener.CurrentScene.MouseClick(e.Button)
    End Sub
    Private Sub Me_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles Window.MouseDoubleClick
        If Attached Then Listener.CurrentScene.MouseDoubleClick(e.Button)
    End Sub
    Private Sub Me_MouseMove(sender As Object, e As MouseEventArgs) Handles Window.MouseMove
        If Attached Then Listener.CurrentScene.MouseMove(e.Location)
    End Sub
End Class

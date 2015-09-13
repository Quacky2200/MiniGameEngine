Imports System.Drawing
Imports System.Windows.Forms
''' <summary>
''' A Scene class that must be inherited and use these functions when inherited. (See below)
''' </summary>
''' <remarks></remarks>
Partial Public MustInherit Class Scene
    Public Property BackgroundColor As Color = Color.Black
    Public Property Game As Game
    Public Sub New(Game As Game)
        Me.Game = Game
    End Sub
    ''' <summary>
    ''' What happens when the scene is loaded
    ''' </summary>
    ''' <remarks></remarks>
    Public MustOverride Sub Init()
    ''' <summary>
    ''' The method that draws all the objects to the screen
    ''' </summary>
    ''' <param name="g">The current game graphics object</param>
    ''' <remarks></remarks>
    Public MustOverride Sub Render(g As Graphics)
    ''' <summary>
    ''' The scenes logic allowing the games state to change
    ''' </summary>
    ''' <param name="delta">The time before the last frame occured</param>
    ''' <remarks></remarks>
    Public MustOverride Sub Update(delta As Double)
    ''' <summary>
    ''' What happens when the scene appears on the screen
    ''' </summary>
    ''' <param name="lastScene">The previous scene shown before this one</param>
    ''' <remarks></remarks>
    Public Overridable Sub Enter(lastScene As Scene)

    End Sub
    ''' <summary>
    ''' What happens when the scene is to disappear off the screen
    ''' </summary>
    ''' <param name="nextScene">The next scene to be shown</param>
    ''' <remarks></remarks>
    Public Overridable Sub Leave(nextScene As Scene)

    End Sub
    ''' <summary>
    ''' What happens when the game is quitting.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub ExitGame()

    End Sub
    Public Overridable Sub KeyDown(KeyCode As Keys)

    End Sub
    Public Overridable Sub KeyPress(KeyCode As Keys)

    End Sub
    Public Overridable Sub KeyUp(KeyCode As Keys)

    End Sub
    Public Overridable Sub MouseDown(MouseButton As MouseButtons)

    End Sub
    Public Overridable Sub MouseUp(MouseButton As MouseButtons)

    End Sub
    Public Overridable Sub MouseClick(MouseButton As MouseButtons)

    End Sub
    Public Overridable Sub MouseDoubleClick(MouseButton As MouseButtons)

    End Sub
    Public Overridable Sub MouseMove(Location As Point)

    End Sub
End Class
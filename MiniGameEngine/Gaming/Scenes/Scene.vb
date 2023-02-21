Imports System.Drawing
Imports System.Windows.Forms

''' <summary>
''' A Scene class that must be inherited and use these functions when inherited. (See below)
''' </summary>
''' <remarks></remarks>
Partial Public MustInherit Class Scene
    Public Property BackgroundColor As Color = Color.Black
    Public Property Game As GameContainer
    Private _active As Boolean = False

    Public ReadOnly Property Active As Boolean
        Get
            Return _active
        End Get
    End Property

    Public ReadOnly GameObjects As New ModifiableList(Of GameObject) ' ModifiableList(Of GameObject)

    Public Sub RefreshZIndexes()
        SyncLock GameObjects
            GameObjects.Sort(Function(T1, T2) T1.zIndex - T2.zIndex)
        End SyncLock
    End Sub

    Public Sub AddGameObject(GameObject As GameObject)
        If IsNothing(GameObject) Then Return

        Dim Refresh As Boolean = False

        SyncLock GameObjects
            'If Not GameObjects.Contains(GameObject) Then
            GameObjects.Add(GameObject)
            GameObject.ReferTo(Me)
            Refresh = True
            'End If
        End SyncLock

        If Refresh Then RefreshZIndexes()
    End Sub

    Public Sub RemoveGameObject(GameObject As GameObject)
        If IsNothing(GameObject) Then Return

        SyncLock GameObjects
            GameObjects.Remove(GameObject)
            GameObject.ReferTo(Nothing)
        End SyncLock

        RefreshZIndexes()
    End Sub

    Public Sub Pause()
        GameObjects.Access(Sub(Collection)
                               For Each O In Collection
                                   O.Pause()
                               Next
                           End Sub)
    End Sub

    Public Sub [Resume]()
        GameObjects.Access(Sub(Collection)
                               For Each O In Collection
                                   O.Resume()
                               Next
                           End Sub)
    End Sub

    Public Sub New(Game As GameContainer)
        Me.Game = Game
    End Sub

    ''' <summary>
    ''' What happens when the scene is loaded
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub Init()
        ' Do nothing.
    End Sub

    ''' <summary>
    ''' The method that draws all the objects to the screen
    ''' </summary>
    ''' <param name="g">The current game graphics object</param>
    ''' <remarks></remarks>
    Public Overridable Sub Render(g As Graphics)
        'GameObjects.Each(Sub(O)
        '                     If IsNothing(O) Then Return
        '                     If (O.Visible) Then O.Render(g)
        '                 End Sub)
        GameObjects.Access(Sub() _RenderGameObjects(g))
    End Sub

    Private Sub _RenderGameObjects(g As Graphics)
        For Each Obj As GameObject In GameObjects
            If Not (Not IsNothing(Obj) AndAlso Obj.Visible) Then Continue For
            Obj.UpdateTransitions()
            Obj.Render(g)
        Next
    End Sub

    ''' <summary>
    ''' The scenes logic allowing the games state to change
    ''' </summary>
    ''' <param name="delta">The time before the last frame occured</param>
    ''' <remarks></remarks>
    Public Overridable Sub Update(delta As Double)
        'GameObjects.Each(Sub(O)
        '                     If IsNothing(O) Then Return
        '                     If (O.Visible) Then O.Update(delta)
        '                 End Sub)
        GameObjects.Access(Sub() _UpdateGameObjects(delta))
    End Sub

    ''' <summary>
    ''' Go through all Game Objects and update them
    ''' </summary>
    Private Sub _UpdateGameObjects(delta As Double)
        For Each Obj As GameObject In GameObjects
            If Not (Not IsNothing(Obj) AndAlso Obj.Visible) Then Continue For
            Obj.Update(delta)
        Next
    End Sub

    ''' <summary>
    ''' What happens when the scene appears on the screen
    ''' </summary>
    ''' <param name="lastScene">The previous scene shown before this one</param>
    ''' <remarks></remarks>
    Public Overridable Sub Enter(lastScene As Scene)
        _active = True
    End Sub

    ''' <summary>
    ''' What happens when the game pauses (pausing occurs when the window loses focus)
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub onPause()
    End Sub

    ''' <summary>
    ''' What happens when the scene is to disappear off the screen
    ''' </summary>
    ''' <param name="nextScene">The next scene to be shown</param>
    ''' <remarks></remarks>
    Public Overridable Sub Leave(nextScene As Scene)
        _active = False
    End Sub

    ''' <summary>
    ''' What happens when the game is quitting.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub ExitGame()
        _active = False
    End Sub

    Public Overridable Sub KeyDown(KeyCode As Keys)

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
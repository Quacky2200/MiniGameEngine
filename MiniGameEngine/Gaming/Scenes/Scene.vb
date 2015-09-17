Imports System.Drawing
Imports System.Windows.Forms
Imports MiniGameEngine.Transitions
Imports Newtonsoft.Json.Serialization
Imports System.Reflection
Imports Newtonsoft.Json

''' <summary>
''' A Scene class that must be inherited and use these functions when inherited. (See below)
''' </summary>
''' <remarks></remarks>
Partial Public MustInherit Class Scene
    Public Property BackgroundColor As Color = Color.Black
    Public Property Game As GameContainer
    Private GameObjects As New List(Of GameObject)
    Private Transitions As New List(Of TransitionPropertyItem)
    Private _active As Boolean = False
    Public ReadOnly Property Active As Boolean
        Get
            Return _active
        End Get
    End Property
    Private TransitionThread As New General.Threading.ThreadLoop(AddressOf Me.TransitionMainThread) With {.Enabled = True}
    Private Sub TransitionMainThread()
        If Game.Enabled And Not Game.Paused AndAlso _active Then
            Dim t As TransitionPropertyItem() = AllTransitions
            For Each item In t
                item.TransitionProperty.Work(item.Transition)
                If item.Transition.isFinished AndAlso item.AutomaticallyRemoveObject Then
                    Dim obj = AllGameObjects.Where(Function(x) Not IsNothing(x) AndAlso x.ID = item.TransitionProperty.ObjectID)
                    If obj.Count >= 1 Then
                        ' Debug.Print("automatically removed object [name:{0}, ID:{1}] and transition [ID:{2}]", obj.GetType.Name, item.TransitionProperty.ObjectID, item.TransitionProperty.ID)
                        remove(obj.Single)
                    End If
                End If
                If item.Transition.isFinished AndAlso item.AutomaticallyRemoveTransition Then
                    ' Debug.Print("automatically removed [TransitionProperty:{0}, Transition:{1}]", item.TransitionProperty.ID, item.Transition.ID)
                    remove(item.TransitionProperty, item.Transition)
                End If
            Next
        End If
    End Sub
    Public ReadOnly Property AllGameObjects As GameObject()
        Get
            SyncLock GameObjects
                Return If(GameObjects.Count = 0, {}, GameObjects.ToArray)
            End SyncLock
        End Get
    End Property
    Public ReadOnly Property AllTransitions As TransitionPropertyItem()
        Get
            SyncLock Transitions
                Return If(Transitions.Count = 0, {}, Transitions.ToArray)
            End SyncLock
        End Get
    End Property

    Public Sub add(GameObject As GameObject)
        SyncLock GameObjects
            If GameObjects.IndexOf(GameObject) < 0 Then
                GameObjects.Add(GameObject)
            End If
        End SyncLock
    End Sub
    Public Sub add(ByRef TransitionProperty As TransitionProperty, Transition As Transition, Optional AutomaticallyRemoveTransition As Boolean = False, Optional AutomaticallyRemoveObject As Boolean = False)
        SyncLock Transitions
            Dim item As New TransitionPropertyItem(TransitionProperty, Transition, AutomaticallyRemoveTransition, AutomaticallyRemoveObject)
            If Transitions.IndexOf(item) < 0 Then
                Transitions.Add(item)
            End If
        End SyncLock
    End Sub
    Public Sub remove(GameObject As GameObject)
        SyncLock GameObjects
            GameObjects.Remove(GameObject)
        End SyncLock
    End Sub
    Public Sub remove(ByRef TransitionProperty As TransitionProperty, Transition As Transition)
        SyncLock Transitions
            Transitions.Remove(New TransitionPropertyItem(TransitionProperty, Transition))
        End SyncLock
    End Sub
    Public Sub New(Game As GameContainer)
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
    ''' NOTE: Transitions will not run unless MyBase.Enter(lastScene) is used to set the scene active.
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
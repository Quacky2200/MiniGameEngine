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

    Private TransitionThread As New General.Threading.ThreadLoop(AddressOf Me.TransitionMainThread)

    Private Sub TransitionMainThread()
        If Game.Enabled AndAlso Not (Game.Paused AndAlso Game.PauseThreads) AndAlso _active AndAlso AllTransitions.Count > 0 Then
            SyncLock AllTransitions
                For i = AllTransitions.Count - 1 To 0 Step -1
                    Dim Item As TransitionPropertyItem = AllTransitions(i)
                    Item.TransitionProperty.Work(Item.Transition)
                    If Item.Transition.isFinished AndAlso Item.AutomaticallyRemoveObject Then
                        Dim obj = Item.TransitionProperty.Reference
                        'Dim formattedStr = String.Format("Automatically removed object [name:{0}, ID:{1}] and transition [ID:{2}]", obj.GetType().Name, obj.ID, Item.TransitionProperty.ID)
                        remove(Item.TransitionProperty.Reference)
                        'Debug.Print(formattedStr)
                    End If
                    If Item.Transition.isFinished AndAlso Item.AutomaticallyRemoveTransition Then
                        'Debug.Print("Automatically removed [TransitionProperty:{0}, Transition:{1}]", Item.TransitionProperty.ID, Item.Transition.ID)
                        remove(Item.Transition)
                    End If
                Next
            End SyncLock
        End If
    End Sub

    Public ReadOnly Property AllGameObjects As List(Of GameObject)
        Get
            SyncLock GameObjects
                Return GameObjects
            End SyncLock
        End Get
    End Property
    Public ReadOnly Property AllTransitions As List(Of TransitionPropertyItem)
        Get
            SyncLock Transitions
                Return Transitions
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
    Public Sub remove(ByRef TransitionProperty As TransitionProperty)
        SyncLock Transitions
            Dim Remove As TransitionPropertyItem = Nothing
            For Each Prop In Transitions
                If (Prop.TransitionProperty.ID = TransitionProperty.ID) Then
                    Remove = Prop
                    Exit For
                End If
            Next
            If (Not IsNothing(Remove)) Then
                Transitions.Remove(Remove)
            End If
        End SyncLock
    End Sub
    Public Sub remove(ByRef Transition As Transition)
        SyncLock Transitions
            Dim Remove As TransitionPropertyItem = Nothing
            For Each Prop In Transitions
                If (Prop.Transition.ID = Transition.ID) Then
                    Remove = Prop
                    Exit For
                End If
            Next
            If (Not IsNothing(Remove)) Then
                Transitions.Remove(Remove)
            End If
        End SyncLock
    End Sub
    Public Sub New(Game As GameContainer)
        Me.Game = Game
        TransitionThread.Enabled = True
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
        TransitionThread.Enabled = True
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
        TransitionThread.Enabled = False
    End Sub
    ''' <summary>
    ''' What happens when the game is quitting.
    ''' </summary>
    ''' <remarks></remarks>
    Public Overridable Sub ExitGame()
        _active = False
        TransitionThread.Enabled = False
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

    Public Overridable Sub WindowSizeChange()

    End Sub
End Class
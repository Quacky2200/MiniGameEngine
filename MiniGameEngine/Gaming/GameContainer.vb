Imports MiniGameEngine.General.Time
Imports MiniGameEngine.General.Threading
Imports MiniGameEngine.Transitions
Imports System.Drawing
Imports System.Windows.Forms
Imports Newtonsoft.Json

Public Class GameContainer

#Region "Threading"
    Private GraphicsThread As New ThreadLoop(AddressOf Me.GraphicsMainThread)
    Private GameThread As New ThreadLoop(AddressOf Me.GameMainThread)

    Public Property Enabled As Boolean = False

    ''' <summary>
    ''' Automatically pauses the game once the focus is lost and will resume it when it regains focus
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutomaticallyPause As Boolean = True
    Public Property PauseThreads As Boolean = False

    Private Property _Paused As Boolean = False
    Public Property Paused As Boolean
        Get
            Return _Paused
        End Get
        Set(value As Boolean)
            If Not value And _Paused Then
                lastFrame = Now.Ticks
            End If
            _Paused = value
            While Not IsNothing(currentScene)
                'For Each t In currentScene.AllTransitions
                '    t.Transition.Paused = value
                'Next
                currentScene.onPause()
                Exit While
            End While
        End Set
    End Property

    Public Sub Start()
        Enabled = True
        GraphicsThread.Start()
        GameThread.Start()
    End Sub
    Public Sub [Stop]()
        Enabled = False
        GraphicsThread.Stop()
        GameThread.Stop()
    End Sub
    ''' <summary>
    ''' Force the screen to refresh
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GraphicsMainThread()
        If Enabled AndAlso Not (Paused AndAlso PauseThreads) Then Me._Window.Invalidate()
    End Sub

    ''' <summary>
    ''' Force the game update
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GameMainThread()
        If Not IsNothing(currentScene) AndAlso Enabled AndAlso Not (Paused AndAlso PauseThreads) Then
            Dim delta As Double = getDelta()
            currentScene.Update(delta)
            While Not IsNothing(currentScene)
                SyncLock currentScene.AllGameObjects
                    For Each Obj As GameObject In currentScene.AllGameObjects '.ToList.Where(Function(x) Not IsNothing(x) AndAlso x.Visible).ToList().OrderBy(Function(x) x.zIndex)
                        Obj.Update(delta)
                    Next
                    Exit While
                End SyncLock

            End While
        End If
    End Sub
#End Region

#Region "Constructor"
    Public WithEvents Window As Form
    Private Shared _Instance As GameContainer = Nothing
    Public Sub New(Window As Form)
        Me.Window = Window
        _Input = New Input(Me)
        lastFormBorderStyle = Me.Window.FormBorderStyle
        lastSize = Me.Window.Size
        lastPosition = Me.Window.Location

        Me.cursorPosition = Me.Window.RectangleToScreen(New Rectangle(Me.MIDDLE_POS, Me.Window.Size)).Location
        _currentScene = New EmptyScene(Me)
        add(currentScene)
        switchScenes(Of EmptyScene)()
    End Sub

    Public Shared ReadOnly Property Instance
        Get
            If (IsNothing(_Instance)) Then
                Throw New Exception("Instance has not yet been initialised")
            End If
            Return _Instance
        End Get
    End Property
#End Region

#Region "Container Events"
    Private pausedBeforeUnfocus As Boolean = False

    ''' <summary>
    ''' Start the game when the window loads
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Me_Shown(sender As Object, e As EventArgs) Handles Window.Shown
        Start()
    End Sub
    ''' <summary>
    ''' Paint the screen (render the current scene)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Me_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Window.Paint
        If Clip Then
            Cursor.Clip = Window.RectangleToScreen(Window.ClientRectangle)
        Else
            Cursor.Clip = Nothing
        End If
        If Not IsNothing(_currentScene) Then
            e.Graphics.Clear(currentScene.BackgroundColor)
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            currentScene.Render(e.Graphics)
            SyncLock currentScene.AllGameObjects
                For Each Obj As GameObject In currentScene.AllGameObjects
                    Obj.Render(e.Graphics)
                Next
            End SyncLock
            updateFPS()
            Dim fpsText = String.Format("FPS: {0} (Avg: {1}, {2} GameObjects, {3} Transitions)", fps, fpsAvg, currentScene.AllGameObjects.Count, currentScene.AllTransitions.Count)
            If showFPS Then e.Graphics.DrawString(fpsText, Me._Window.Font, Brushes.Red, New Point(0, 0))
            lastFrame = Now.Ticks()
        End If
    End Sub

    Private Sub Me_Unfocus(sender As Object, e As EventArgs) Handles Window.LostFocus
        If Paused Then
            pausedBeforeUnfocus = True
        Else
            pausedBeforeUnfocus = False
        End If
        If AutomaticallyPause And Not Paused Then
            Cursor.Clip = Nothing
            Paused = True
        End If
    End Sub
    Private Sub Me_Focus(sender As Object, e As EventArgs) Handles Window.GotFocus
        If AutomaticallyPause And Not pausedBeforeUnfocus Then Paused = False
        pausedBeforeUnfocus = False
    End Sub
    ''' <summary>
    ''' When we close, stop everything gracefully
    ''' </summary>
    ''' <param name="sender">Me - the game object</param>
    ''' <param name="e">The event arguments - form closing</param>
    ''' <remarks>We can use e.closing = false to prevent the window from closing</remarks>
    Private Sub Me_Closing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Window.FormClosing
        [Stop]()
        Input.Detach()
        currentScene.ExitGame()
        Scenes.Clear()
        _Input = Nothing
        _currentScene = Nothing
    End Sub

    Public Sub Me_SizeChanged(sender As Object, e As EventArgs) Handles Window.SizeChanged
        If _Window.Width < My.Computer.Screen.WorkingArea.Width Then
            lastSize.Width = _Window.Width
        End If
        If _Window.Height < My.Computer.Screen.WorkingArea.Height Then
            lastSize.Height = _Window.Height
        End If
    End Sub
    Public Sub Me_LocationChanged(sender As Object, e As EventArgs) Handles Window.LocationChanged
        If _Window.Width < My.Computer.Screen.WorkingArea.Width AndAlso _Window.Height < My.Computer.Screen.WorkingArea.Height Then
            lastPosition = Me.Window.Location
        End If
    End Sub

#End Region

#Region "Input"
    Private _Input As Input

    Public ReadOnly Property Input As Input
        Get
            Return _Input
        End Get
    End Property
    Private _Cursor As New Cursor(Cursor.Current.Handle)
    ''' <summary>
    ''' Clip the cursor to the screen?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Clip As Boolean = False
    ''' <summary>
    ''' Return or set the cursor position
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property cursorPosition As Point
        Get
            Return Cursor.Position
        End Get
        Set(value As Point)
            Cursor.Position = value
        End Set
    End Property
    Public Property cursorSpawnPosition As New Point(0, 0)
#End Region

#Region "Scene Properties"
    Private _currentScene As Scene
    Private Scenes As New List(Of Scene)
#End Region

#Region "Scene Functions"
    Public ReadOnly Property currentScene As Scene
        Get
            Return _currentScene
        End Get
    End Property
    ''' <summary>
    ''' Add a scene to the list
    ''' </summary>
    ''' <param name="Scene"></param>
    ''' <remarks></remarks>
    Public Sub add(Scene As Scene)
        Scenes.Add(Scene)
    End Sub
    ''' <summary>
    ''' Add a list of scenes to the list
    ''' </summary>
    ''' <param name="Scenes"></param>
    ''' <remarks></remarks>
    Public Sub add(Scenes() As Scene)
        For Each Scene As Scene In Scenes
            Me.add(Scene)
        Next
    End Sub
    ''' <summary>
    ''' Switch the scene you want
    ''' </summary>
    ''' <param name="Scene"></param>
    ''' <remarks></remarks>
    Public Sub switchScenes(Scene As Scene)
        Dim index As Integer = Scenes.ToList.IndexOf(Scene)
        If index > -1 Then
            Dim PreviousScene As Scene = currentScene
            If Not IsNothing(PreviousScene) Then
                PreviousScene.Leave(Scene)
            End If
            _currentScene = Scenes(index)
            currentScene.Enter(PreviousScene)
            currentScene.Init()
            lastFrame = Now.Ticks()
        Else
            Throw New Exception("Scene not in index.")
        End If
    End Sub
    ''' <summary>
    ''' Switch to a scene from the list
    ''' </summary>
    ''' <param name="index">The index of the scene (you want to swap to)</param>
    ''' <remarks></remarks>
    Public Sub switchScenes(index As Integer)
        If index < 0 AndAlso index < Scenes.Count Then
            switchScenes(Scenes(index))
        Else
            Throw New Exception("Out of index!")
        End If
    End Sub
    ''' <summary>
    ''' Switch to a scene using a class type
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub switchScenes(Of T As {Scene})()
        For Each Scene In Me.Scenes
            If TypeOf Scene Is T Then
                switchScenes(Scene)
                Exit Sub
            End If
        Next
        Throw New Exception("Scene not in index.")
    End Sub
    ''' <summary>
    ''' Remove a scene from the list
    ''' </summary>
    ''' <param name="Scene">The scene you want to remove</param>
    ''' <remarks></remarks>
    Public Sub removeScene(Scene As Scene)
        If Scenes.IndexOf(Scene) > 0 Then
            Scenes.Remove(Scene)
        Else
            Throw New Exception("Scene not in index.")
        End If
    End Sub
    ''' <summary>
    ''' Remove a scene from the list
    ''' </summary>
    ''' <param name="index">The index of the scene in the list (you want to remove)</param>
    ''' <remarks></remarks>
    Public Sub removeScene(index As Integer)
        If index < 0 AndAlso index < Scenes.Count Then
            removeScene(Scenes(index))
        Else
            Throw New Exception("Out of index!")
        End If
    End Sub
#End Region

#Region "Timing"
    Private Property lastFPS As Long = getTime()
    Private Property FPSCounts As List(Of Integer) = New List(Of Integer)
    Private Property fpsCounter As Integer = 0
    Public Property fps As Integer = 0
    Public Property fpsAvg As Integer = 0
    Private Property lastFrame As Long = getTime()
    ''' <summary>
    ''' Get the time in milliseconds
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getTime() As Long
        Return Watch.getLiveMilliseconds
    End Function

    ''' <summary>
    ''' Get the number of milliseconds that have past since the last frame
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function getDelta() As Double
        Dim time As Long = getTime()
        Dim delta As Double = (time - lastFrame) / TimeSpan.TicksPerMillisecond
        lastFrame = time

        Return If(delta < 0, 0, delta)
    End Function

    Private Sub updateFPS()
        ' FPS Counting...
        If (getTime() - lastFPS) > 1000 Then
            fps = fpsCounter
            If (FPSCounts.Count > 4) Then
                FPSCounts.Remove(0)
            End If

            FPSCounts.Add(fps)
            fpsCounter = 0
            lastFPS += 1000
        End If

        fpsAvg = If(FPSCounts.Count > 0, FPSCounts.Average(), fps)
        fpsCounter += 1
    End Sub
    Public Property showFPS As Boolean = True
#End Region

#Region "Window Properties"
    Private lastSize As Size
    Private lastPosition As Point
    Private lastFormBorderStyle As System.Windows.Forms.FormBorderStyle
    Public Overloads Property Location As Point
        Get
            Return Me.Window.Location
        End Get
        Set(value As Point)
            Me.Window.Location = value
        End Set
    End Property
    Public Overloads Property Width As Integer
        Get
            Return Me._Window.Width
        End Get
        Set(value As Integer)
            Me._Window.Width = value
            If value < My.Computer.Screen.WorkingArea.Width Then
                Me.lastSize.Width = value
            End If
        End Set
    End Property

    Public Overloads Property Height As Integer
        Get
            Return Me._Window.Height
        End Get
        Set(value As Integer)
            Me._Window.Height = value
            If value < My.Computer.Screen.WorkingArea.Height Then
                Me.lastSize.Height = value
            End If
        End Set
    End Property

    Public Overloads Property FormBorderStyle As Windows.Forms.FormBorderStyle
        Get
            Return Me._Window.FormBorderStyle
        End Get
        Set(value As Windows.Forms.FormBorderStyle)
            Me._Window.FormBorderStyle = value
            If value <> Windows.Forms.FormBorderStyle.None Then
                Me.lastFormBorderStyle = value
            End If
        End Set
    End Property

    Public ReadOnly Property aspectRatio As Double
        Get
            Return Width / Height
        End Get
    End Property

    Public Property Fullscreen As Boolean
        Get
            Return Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None AndAlso Width = Screen.PrimaryScreen.Bounds.Width AndAlso Height = Screen.PrimaryScreen.Bounds.Height
        End Get
        Set(value As Boolean)
            If value Then
                Me._Window.Left = Screen.PrimaryScreen.Bounds.Left
                Me._Window.Top = Screen.PrimaryScreen.Bounds.Top
                FormBorderStyle = Windows.Forms.FormBorderStyle.None
                Width = Screen.PrimaryScreen.Bounds.Width
                Height = Screen.PrimaryScreen.Bounds.Height
            Else
                Width = lastSize.Width
                Height = lastSize.Height
                FormBorderStyle = lastFormBorderStyle
            End If
        End Set
    End Property

#End Region

#Region "Generic Window Positions"

    Public ReadOnly Property MIDDLE_POS As Point
        Get
            Return New Point(CInt(Me.Width / 2), CInt(Me.Height / 2))
        End Get
    End Property

    Public ReadOnly Property MIDDLE_LEFT_POS As Point
        Get
            Return New Point(0, CInt(Me.Height / 2))
        End Get
    End Property

    Public ReadOnly Property MIDDLE_RIGHT_POS As Point
        Get
            Return New Point(Me.Width, CInt(Me.Height / 2))
        End Get
    End Property

    Public ReadOnly Property TOP_LEFT_POS As Point
        Get
            Return New Point(0, 0)
        End Get
    End Property

    Public ReadOnly Property TOP_MIDDLE_POS As Point
        Get
            Return New Point(CInt(Me.Width / 2), 0)
        End Get
    End Property

    Public ReadOnly Property TOP_RIGHT_POS As Point
        Get
            Return New Point(Me.Width, 0)
        End Get
    End Property

    Public ReadOnly Property BOTTOM_LEFT_POS As Point
        Get
            Return New Point(0, Me.Height)
        End Get
    End Property

    Public ReadOnly Property BOTTOM_MIDDLE_POS As Point
        Get
            Return New Point(CInt(Me.Width / 2), Me.Height)
        End Get
    End Property

    Public ReadOnly Property BOTTOM_RIGHT_POS As Point
        Get
            Return New Point(Me.Width, Me.Height)
        End Get
    End Property

#End Region

End Class
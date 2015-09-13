Option Strict Off
Imports System.Drawing
Imports System.Windows.Forms
Public Class Game
    Inherits Form
#Region "Singleton Instance"
    Private Shared _instance As Game = Nothing
    Public Shared Function getInstance() As Game
        Return _instance
    End Function
#End Region
#Region "Transition Property"
    Private Property Transitions As New Dictionary(Of Object, Object)
    Public Sub addTransition(Of T1 As TransitionProperty(Of Transitions.Transition(Of T2), T2), T2)(TransitionProperty As T1, Transition As Transitions.Transition(Of T2))
        Transitions.Add(TransitionProperty, Transition)
    End Sub
    Public Function getTransition(Of T1 As TransitionProperty(Of Transitions.Transition(Of T2), T2), T2)(TransitionProperty As T1) As Transitions.Transition(Of T2)
        Return Transitions.Item(TransitionProperty)
    End Function
    Public Sub removeTransition(Of T1 As TransitionProperty(Of Transitions.Transition(Of T2), T2), T2)(TransitionProperty As T1)
        Transitions.Remove(TransitionProperty)
    End Sub

    Public Class TransitionProperty(Of T1 As Transitions.Transition(Of T2), T2)
        Public Delegate Sub Worker(ByVal Transition As T1)
        Private _Work As Worker
        Public Sub New(ByVal Work As Worker)
            Me._Work = Work
        End Sub
        Public Sub Work(ByVal Transition As T1)
            Me._Work(Transition)
        End Sub
    End Class

#End Region
#Region "Threading"
    Private GraphicsThread As New ThreadLoop(AddressOf Me.GraphicsMainThread)
    Private GameThread As New ThreadLoop(AddressOf Me.GameMainThread)
    Private TransitionThread As New ThreadLoop(AddressOf Me.TransitionMainThread)
    Private Sub TransitionMainThread()
        For Each TransitionProperty In Me.Transitions
            TransitionProperty.Key.Work(TransitionProperty.Value)
           
        Next
    End Sub
    ''' <summary>
    ''' Force the screen to refresh
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GraphicsMainThread()
        Me.Invalidate()
    End Sub
    ''' <summary>
    ''' Force the game update
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GameMainThread()
        If Not IsNothing(currentScene) Then
            Dim newdelta As Double = Now.Ticks() - lastFrame '(Now.Ticks - lastFrame) / 10000 
            currentScene.Update(newdelta)
            Try
                For Each Obj As GameObject In Scenes(currentScene).ToList.Where(Function(x) x.Visible).ToList().OrderBy(Function(x) x.zIndex)
                    Obj.Update(newdelta)
                Next
            Catch ex As AccessViolationException
                Debug.WriteLine(DateTime.Now.ToLongTimeString & " an error occured. " & ex.Message)
            End Try
        End If
    End Sub
#End Region

#Region "Constructor"

    Public Sub New(Title As String, Width As Integer, Height As Integer)
        MyBase.New()
        Me.DoubleBuffered = True
        Me.Width = Width
        Me.Height = Height
        MyBase.Text = Title
        MyBase.OnLoad(Nothing)
        _instance = Me
        _currentScene = New EmptyScene(Me)
        addScene(currentScene)
        switchScenes(Of EmptyScene)()
    End Sub
    Private Sub InitializeComponent()
        Me.SuspendLayout()
        Me.ClientSize = New Size(Me.Width, Height)
        Me.Name = Me.Text
        Me.ResumeLayout(False)
    End Sub
#End Region

#Region "Form Events"
    ''' <summary>
    ''' Start the game when the window loads
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Me_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        GraphicsThread.Start()
        GameThread.Start()
        TransitionThread.Start()
    End Sub
    ''' <summary>
    ''' Paint the screen (render the current scene)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Me_Paint(sender As Object, e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        If Not IsNothing(_currentScene) Then
            e.Graphics.Clear(currentScene.BackgroundColor)
            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias
            e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
            currentScene.Render(e.Graphics)
            Try
                For Each Obj As GameObject In Scenes(currentScene).ToList.Where(Function(x) x.Visible).ToList().OrderBy(Function(x) x.zIndex)
                    Obj.Render(e.Graphics)
                Next
            Catch ex As AccessViolationException
                Debug.WriteLine(DateTime.Now.ToLongTimeString & " an error occured. " & ex.Message)
            End Try
            updateFPS()
            If showFPS Then e.Graphics.DrawString("FPS: " & fps, Me.Font, Brushes.Red, New Point(0, 0))
            lastFrame = Now.Ticks()
        End If
    End Sub
    ''' <summary>
    ''' When we close, stop everything gracefully
    ''' </summary>
    ''' <param name="sender">Me - the game object</param>
    ''' <param name="e">The event arguments - form closing</param>
    ''' <remarks>We can use e.closing = false to prevent the window from closing</remarks>
    Private Sub Me_Closing(sender As Object, e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        Input.Detach()
        _Input = Nothing
        currentScene.ExitGame()
        TransitionThread.Stop()
        Transitions.Clear()
        While (GraphicsThread.Enabled = True Or GameThread.Enabled)
            GraphicsThread.Stop()
            GameThread.Stop()
        End While
        _currentScene = Nothing
        Scenes.Clear()
    End Sub
#End Region

#Region "Input"
    Private _Input As New Input(Me)

    Public ReadOnly Property Input As Input
        Get
            Return _Input
        End Get
    End Property

#End Region

#Region "Scene Properties"
    Private _currentScene As Scene
    Private Scenes As New Dictionary(Of Scene, List(Of GameObject))
#End Region

#Region "Game Object Functions"
    Public Sub addGameObject(Obj As GameObject)
        Scenes(currentScene).Add(Obj)
        Scenes(currentScene).Item(Scenes(currentScene).IndexOf(Obj)).zIndex = Scenes(currentScene).Count
    End Sub
    Public Function allGameObjects() As List(Of GameObject)
        Try
            Return Scenes(currentScene)
        Catch ex As Exception
            Return New List(Of GameObject)
        End Try
    End Function
    Public Sub removeGameObject(Obj As GameObject)
        Scenes(currentScene).Remove(Obj)
    End Sub
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
    Public Sub addScene(Scene As Scene)

        Scenes.Add(Scene, New List(Of GameObject))
    End Sub
    ''' <summary>
    ''' Add a list of scenes to the list
    ''' </summary>
    ''' <param name="Scenes"></param>
    ''' <remarks></remarks>
    Public Sub addScenes(Scenes() As Scene)
        For Each Scene As Scene In Scenes
            Me.addScene(Scene)
        Next
    End Sub
    ''' <summary>
    ''' Switch the scene you want
    ''' </summary>
    ''' <param name="Scene"></param>
    ''' <remarks></remarks>
    Public Sub switchScenes(Scene As Scene)
        Dim index As Integer = Scenes.Keys.ToList.IndexOf(Scene)
        If index > -1 Then
            Dim PreviousScene As Scene = currentScene
            If Not IsNothing(PreviousScene) Then
                PreviousScene.Leave(Scene)
            End If
            _currentScene = Scenes.Keys.ToArray(index)
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
            switchScenes(Scenes.Keys(index))
        Else
            Throw New Exception("Out of index!")
        End If
    End Sub
    ''' <summary>
    ''' Switch to a scene using a class type
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub switchScenes(Of T As {Scene})()
        For Each Scene In Me.Scenes.Keys
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
        If Scenes.Keys.ToList().IndexOf(Scene) > 0 Then
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
            removeScene(Scenes.Keys.ToArray(index))
        Else
            Throw New Exception("Out of index!")
        End If
    End Sub
#End Region

#Region "Timing"
    Private Property lastFPS As Long = getTime()
    Private Property fpsCounter As Integer = 0
    Public Property fps As Integer = 0
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
    Public Function getDelta() As Double
        Dim time As Long = getTime()
        Dim delta As Double = (time - lastFrame) / TimeSpan.TicksPerMillisecond
        lastFrame = time
        Return delta
    End Function
    Public Sub updateFPS()
        If (getTime() - lastFPS) > 1000 Then
            fps = fpsCounter
            fpsCounter = 0
            lastFPS += 1000
        End If
        fpsCounter += 1
    End Sub
#End Region

#Region "FPS & Delta Functions"

    'Private Sub updateFPS()
    '    Dim milliseconds = Watch.getLiveMilliseconds
    '    If milliseconds - lastFPS > 1000 Then
    '        lastFPS = milliseconds
    '        recordedFPS = FPS
    '        FPS = 0
    '    End If
    '    FPS += 1
    'End Sub

    'Private Function getFPS() As Integer
    '    Return recordedFPS
    'End Function

    Public Property showFPS As Boolean = True
#End Region

#Region "Window Properties"
    Private lastSize As Size

    Private lastFormBorderSize As System.Windows.Forms.FormBorderStyle

    Public Overloads Property Width As Integer
        Get
            Return MyBase.Width
        End Get
        Set(value As Integer)
            MyBase.Width = value
            If value < My.Computer.Screen.WorkingArea.Width Then
                Me.lastSize.Width = value
            End If
        End Set
    End Property

    Public Overloads Property Height As Integer
        Get
            Return MyBase.Height
        End Get
        Set(value As Integer)
            MyBase.Height = value
            If value < My.Computer.Screen.WorkingArea.Height Then
                Me.lastSize.Height = value
            End If
        End Set
    End Property

    Public Overloads Property FormBorderStyle As Windows.Forms.FormBorderStyle
        Get
            Return MyBase.FormBorderStyle
        End Get
        Set(value As Windows.Forms.FormBorderStyle)
            MyBase.FormBorderStyle = value
            If value <> Windows.Forms.FormBorderStyle.None Then
                Me.lastFormBorderSize = value
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
                Me.Left = Screen.PrimaryScreen.Bounds.Left
                Me.Top = Screen.PrimaryScreen.Bounds.Top
                FormBorderStyle = Windows.Forms.FormBorderStyle.None
                Width = Screen.PrimaryScreen.Bounds.Width
                Height = Screen.PrimaryScreen.Bounds.Height
            Else
                Width = lastSize.Width
                Height = lastSize.Height
                FormBorderStyle = lastFormBorderSize
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
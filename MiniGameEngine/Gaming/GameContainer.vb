Imports MiniGameEngine.General.Time
Imports MiniGameEngine.General.Threading
Imports System.Drawing
Imports System.Windows.Forms

Public Class GameContainer
#Region "Threading"
    Private ReadOnly GraphicsThread As New ThreadLoop(AddressOf GraphicsMainThread)
    Private ReadOnly GameThread As New ThreadLoop(AddressOf GameMainThread)

    Public Property Enabled As Boolean = False
    ''' <summary>
    ''' Automatically pauses the game once the focus is lost and will resume it when it regains focus
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutomaticallyPause As Boolean = True
    Private _Paused As Boolean = False
    Public Property Paused As Boolean
        Get
            Return _Paused
        End Get
        Set(value As Boolean)
            If value = _Paused Then Return

            If Not value Then
                ' Prevent jump when resuming
                LastFrame = Now.Ticks
            End If

            _Paused = value

            If Not IsNothing(CurrentScene) Then
                CurrentScene.onPause()
            End If
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
        If Enabled And Not Paused Then _Window.Invalidate()
    End Sub

    ''' <summary>
    ''' Force the game update
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GameMainThread()
        If IsNothing(CurrentScene) Or (Not Enabled) Or Paused Then Return
        LastMs = GetTime()
        CurrentScene.Update(GetDelta())
        UpdateThreadMs()
    End Sub
#End Region

#Region "Constructor"
    Public WithEvents Window As Form
    Public Sub New(Window As Form)
        Me.Window = Window
        _Input = New Input(Me)

        Me.cursorPosition = Me.Window.RectangleToScreen(New Rectangle(Me.MIDDLE_POS, Me.Window.Size)).Location

        _CurrentScene = New EmptyScene(Me)
        AddScene(CurrentScene)
        SwitchScenes(Of EmptyScene)()
    End Sub
#End Region

#Region "Container Events"
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
        Cursor.Clip = If(Clip, Window.RectangleToScreen(Window.ClientRectangle), Nothing)

        If IsNothing(_CurrentScene) Then Return

        e.Graphics.Clear(CurrentScene.BackgroundColor)
        e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias
        e.Graphics.InterpolationMode = Drawing2D.InterpolationMode.HighQualityBicubic
        CurrentScene.Render(e.Graphics)
        UpdateFPS()
        If ShowFPS Then e.Graphics.DrawString(String.Format("FPS: {0}, LoopMs {1}ms ({2} GameObjects)", FPS, LoopMs, CurrentScene.GameObjects.Count), _Window.Font, Brushes.Red, New Point(0, 0))
        LastFrame = Now.Ticks()
    End Sub

    Private pausedBeforeUnfocus As Boolean = False
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
        CurrentScene.ExitGame()
        Scenes.Clear()
        _Input = Nothing
        _CurrentScene = Nothing
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
    Private _CurrentScene As Scene
    Private ReadOnly Scenes As New HashSet(Of Scene)
#End Region

#Region "Scene Functions"
    Public ReadOnly Property CurrentScene As Scene
        Get
            Return _CurrentScene
        End Get
    End Property
    ''' <summary>
    ''' Add a scene to the list
    ''' </summary>
    ''' <param name="Scene"></param>
    ''' <remarks></remarks>
    Public Sub AddScene(Scene As Scene)
        Scenes.Add(Scene)
    End Sub
    ''' <summary>
    ''' Add a list of scenes to the list
    ''' </summary>
    ''' <param name="Scenes"></param>
    ''' <remarks></remarks>
    Public Sub AddScenes(Scenes() As Scene)
        For Each Scene As Scene In Scenes
            AddScene(Scene)
        Next
    End Sub
    ''' <summary>
    ''' Switch the scene you want
    ''' </summary>
    ''' <param name="Scene"></param>
    ''' <remarks></remarks>
    Public Sub SwitchScenes(Scene As Scene)
        Dim index As Integer = Scenes.ToList.IndexOf(Scene)
        If index > -1 Then
            Dim PreviousScene As Scene = CurrentScene
            If Not IsNothing(PreviousScene) Then
                PreviousScene.Leave(Scene)
            End If
            _CurrentScene = Scene 'Scenes(index)
            CurrentScene.Enter(PreviousScene)
            CurrentScene.Init()
            LastFrame = Now.Ticks()
        Else
            Throw New Exception("Scene not in index.")
        End If
    End Sub
    ''' <summary>
    ''' Switch to a scene from the list
    ''' </summary>
    ''' <param name="index">The index of the scene (you want to swap to)</param>
    ''' <remarks></remarks>
    Public Sub SwitchScenes(index As Integer)
        If index < 0 AndAlso index < Scenes.Count Then
            SwitchScenes(Scenes(index))
        Else
            Throw New Exception("Out of index!")
        End If
    End Sub
    ''' <summary>
    ''' Switch to a scene using a class type
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SwitchScenes(Of T As {Scene})()
        For Each Scene In Me.Scenes
            If TypeOf Scene Is T Then
                SwitchScenes(Scene)
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
    Public Sub RemoveScene(Scene As Scene)
        If Scenes.Contains(Scene) Then
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
    Public Sub RemoveScene(index As Integer)
        If index < 0 AndAlso index < Scenes.Count Then
            RemoveScene(Scenes(index))
        Else
            Throw New Exception("Out of index!")
        End If
    End Sub
#End Region

#Region "Timing"
    Private Property LastFrame As Long = GetTime()
    Private Property LastFPS As Long = GetTime()
    Private Property LastMs As Long = GetTime()
    Private Property FPSCounter As Integer = 0
    Public Property FPS As Integer = 0
    Public Property LoopMs As Long = 0

    ''' <summary>
    ''' Get the time in milliseconds
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTime() As Long
        Return Watch.getLiveMilliseconds
    End Function

    ''' <summary>
    ''' Get the number of milliseconds that have past since the last frame
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDelta() As Double
        Dim Time As Long = GetTime()
        Dim Delta As Double = (Time - LastFrame) / TimeSpan.TicksPerMillisecond
        LastFrame = Time
        Return Math.Max(0, Delta)
    End Function

    Private Sub UpdateFPS()
        Dim Diff As Long = GetTime() - LastFPS
        If Diff >= 1000 Then
            FPS = FPSCounter
            FPSCounter = 0
            LastFPS += 1000 ' Diff
        End If
        FPSCounter += 1
    End Sub

    Private Sub UpdateThreadMs()
        LoopMs = GetTime() - LastMs
        LastMs = GetTime()
    End Sub

    Public Property ShowFPS As Boolean = True
#End Region

#Region "Window Properties"
    Private LastFormBorderStyle As System.Windows.Forms.FormBorderStyle
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
            If Window.WindowState = FormWindowState.Minimized Then
                Return Window.RestoreBounds.Width
            Else
                Return Window.Width
            End If
        End Get
        Set(value As Integer)
            Window.Width = value
        End Set
    End Property

    Public Overloads Property Height As Integer
        Get
            If Window.WindowState = FormWindowState.Minimized Then
                Return Window.RestoreBounds.Height
            Else
                Return Window.Height
            End If
        End Get
        Set(value As Integer)
            Window.Height = value
        End Set
    End Property

    Public Overloads Property FormBorderStyle As Windows.Forms.FormBorderStyle
        Get
            Return Window.FormBorderStyle
        End Get
        Set(value As Windows.Forms.FormBorderStyle)
            Window.FormBorderStyle = value
        End Set
    End Property

    Public ReadOnly Property aspectRatio As Double
        Get
            Return Width / Height
        End Get
    End Property

    Public CachedNonFullscreenPosition As Point = Nothing
    Public CachedNonFullscreenSize As Size = Nothing

    Public Property Fullscreen As Boolean
        Get
            Return (
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.None AndAlso
                Not IsNothing(CachedNonFullscreenPosition)
            )
        End Get
        Set(value As Boolean)
            If value Then
                Dim Screens = Screen.AllScreens
                For Each Screen In Screens
                    Dim WinRect = New Rectangle(Window.Left, Window.Top, Window.Width, Window.Height)
                    If Screen.Bounds.IntersectsWith(WinRect) Then
                        CachedNonFullscreenSize = New Size(Width, Height)
                        CachedNonFullscreenPosition = New Point(Window.Left, Window.Top)
                        LastFormBorderStyle = FormBorderStyle
                        FormBorderStyle = Windows.Forms.FormBorderStyle.None
                        Window.Left = Screen.Bounds.Left
                        Window.Top = Screen.Bounds.Top
                        Width = Screen.Bounds.Width
                        Height = Screen.Bounds.Height
                        Return
                    End If
                Next
            Else
                'FormBorderStyle be first as it changes the size of the window (form size consists of the toolbar too)
                FormBorderStyle = LastFormBorderStyle
                Width = CachedNonFullscreenSize.Width
                Height = CachedNonFullscreenSize.Height
                Window.Left = CachedNonFullscreenPosition.X
                Window.Top = CachedNonFullscreenPosition.Y
                CachedNonFullscreenSize = Nothing
                CachedNonFullscreenPosition = Nothing
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

#Region "Generic"
    Public Shared Function GetDefaultIcon() As Bitmap
        Dim Ico As New Bitmap(512, 512)
        Dim Font As New Font("Arial", 200, FontStyle.Bold)
        Dim Letters() As LazyText = {
            New LazyText("M", Font, New SolidBrush(Color.Red), New Point(8, 16)),
            New LazyText("G", Font, New SolidBrush(Color.LimeGreen), New Point(128, 128)),
            New LazyText("E", Font, New SolidBrush(Color.Blue), New Point(256, 200))
        }

        Using Graphics As Graphics = Graphics.FromImage(Ico)
            Graphics.Clear(Color.White)
            For I = 0 To Letters.Length - 1
                Dim Letter As LazyText = Letters(I)
                Letter.DrawWith(Graphics)
            Next
        End Using

        Return Ico
    End Function
#End Region

End Class
Imports MiniGameEngine.Transitions
Imports System.Reflection
Imports System.Drawing
Imports System.Windows.Forms
Imports MiniGameEngine.Examples.Shapes
Imports MiniGameEngine.Examples.Shapes.SineCircles
Imports MiniGameEngine.UI

Namespace Examples.Scenes
    Public Class SineDemo
        Inherits Scene

#Region "General Properties"

        'Music
        Private MP3Path As String = IO.Path.Combine(My.Application.Info.DirectoryPath, "temp.mp3")
        Private WMPObject As New WMPLib.WindowsMediaPlayer()

        'GameObjects
        Public Sine As New SineCircle(Game.MIDDLE_POS, Game.Height / 3, 40, 40)
        Private DebuggingInfomation As New TextElement("") With {
            .Position = New Point(0, 15),
            .Font = New Font("consolas", 10)
        }

        Private FullScreenInformation As New TextElement(
            $"Left-Click to cycle through sine circles. Right-Click to toggle background color{Environment.NewLine}{"".PadLeft(18)}[Space] to pause. [F11] to toggle fullscreen"
        ) With {
            .Font = New Font("consolas", 14),
            .Position = New Point(Game.BOTTOM_MIDDLE_POS.X, Game.BOTTOM_MIDDLE_POS.Y - 100),
            .HorizontalAlignment = UI.HorizontalAlignment.Center,
            .VerticalAlignment = UI.VerticalAlignment.Center
        }

        Private PausedInformation As New TextElement("[paused]") With {
            .Font = New Font("consolas", 20),
            .Position = Game.MIDDLE_POS,
            .HorizontalAlignment = UI.HorizontalAlignment.Center,
            .VerticalAlignment = UI.VerticalAlignment.Center,
            .Visible = False,
            .ZIndex = 99
        }

        'Transitions
        Private PausedTextColorTransition As New ColorTransition(Color.Black, Color.White) With {
            .Repeat = True,
            .Reverse = True,
            .Enabled = True,
            .Duration = TimeSpan.FromMilliseconds(100)
        }

        Public Radius As New RandomDoubleTransition(50, 20, MaxTransitionalRadius()) With {.Repeat = True, .Reverse = True, .Enabled = True}

        Public Frequency As New RandomDoubleTransition(24, 0, 85) With {.Repeat = True, .Reverse = True, .Enabled = True}

        Public Depth As New RandomDoubleTransition(25, 0, 80) With {.Repeat = True, .Reverse = True, .Enabled = True}

        Public Position As New RandomPointTransition(Game.MIDDLE_POS, MinTransitionalPosition(), MaxTransitionalPosition()) With {.Repeat = True, .Enabled = True}

        Public Rotation As New RandomDoubleTransition(0, 360) With {.Repeat = True, .Reverse = True, .ReverseUsesDuration = True, .Enabled = True}

        Public ColorTransition As New RandomColorTransition(0, COLOR_TRANSITION_HSL_SATURATION_LIGHT, COLOR_TRANSITION_HSL_LIGHTNESS_LIGHT) With
                                                           {.Duration = TimeSpan.FromSeconds(5), .Repeat = True, .Reverse = True, .Enabled = True}

        Public Const COLOR_TRANSITION_HSL_SATURATION_LIGHT = 1
        Public Const COLOR_TRANSITION_HSL_SATURATION_DARK = 1.0
        Public Const COLOR_TRANSITION_HSL_LIGHTNESS_LIGHT = 0.3
        Public Const COLOR_TRANSITION_HSL_LIGHTNESS_DARK = 0.5
#End Region

#Region "Sine Circle Types"
        Private CurrentCircle As Integer = -1
        Private Circles As New Dictionary(Of SineCircleType, Action) From {
            {New PiSineType(Sine), AddressOf FastSineCirclesTransition},
            {New NormalSineCirclePathType(Sine), AddressOf FastSineCirclesTransition},
            {New DoubleSineCirclePathType(Sine), AddressOf SlowSineCirclesTransition},
            {New DoubleCosCirclePathType(Sine), AddressOf SlowSineCirclesTransition}
        }

        Private Sub SlowSineCirclesTransition()
            Radius.Duration = TimeSpan.FromSeconds(10)
            Frequency.Duration = TimeSpan.FromSeconds(10)
            Depth.Duration = TimeSpan.FromSeconds(10)
            Rotation.Duration = TimeSpan.FromSeconds(6)
            Position.Duration = TimeSpan.FromSeconds(4)
        End Sub

        Private Sub FastSineCirclesTransition()
            Radius.Duration = TimeSpan.FromSeconds(1)
            Frequency.Duration = TimeSpan.FromSeconds(1)
            Depth.Duration = TimeSpan.FromSeconds(2)
            Position.Duration = TimeSpan.FromMilliseconds(800)
            Rotation.Duration = TimeSpan.FromSeconds(3)
        End Sub

        Public Sub AdvanceSineType()
            CurrentCircle = (CurrentCircle + 1) Mod Circles.Count
            Dim key = Circles.Keys.ToArray(CurrentCircle)
            Sine.type = key
            Circles(key)()
        End Sub
#End Region

#Region "Min/Max Properties"
        Public ReadOnly Property MaxTransitionalPosition() As Point
            Get
                Return New Point(CInt((Game.Width / 4) * 3), CInt((Game.Height / 4) * 3))
            End Get
        End Property
        Public ReadOnly Property MinTransitionalPosition() As Point
            Get
                Return New Point(CInt(Game.Width / 4), CInt(Game.Height / 4))
            End Get
        End Property
        Public ReadOnly Property MaxTransitionalRadius() As Double
            Get
                Return Game.Height / 3
            End Get
        End Property
#End Region

#Region "Construction"

        Public Sub New(Game As GameContainer)
            MyBase.New(Game)
            BackgroundColor = Color.White
        End Sub

        Public Overrides Sub Init()
            AddHandler Game.Window.SizeChanged, AddressOf Resized
            Game.FPSRefreshRate = TimeSpan.FromSeconds(0)

            'Add the sine circle
            AddGameObject(Sine)
            'Get the first Sine Type and change it
            AdvanceSineType()

            Rotation.EasingFunction = AddressOf EasingFunctions.EaseIn
            Position.EasingFunction = AddressOf EasingFunctions.EaseIn
            ColorTransition.EasingFunction = AddressOf EasingFunctions.EaseInOut
            Frequency.EasingFunction = AddressOf EasingFunctions.EaseIn
            Depth.EasingFunction = AddressOf EasingFunctions.EaseIn
            Radius.EasingFunction = AddressOf EasingFunctions.EaseIn


            'Add all Sine Circle Transitions
            Sine.AddTransition(Sine.RotationProperty, Rotation)
            Sine.AddTransition(Sine.PositionProperty, Position)
            Sine.AddTransition(Sine.ColorProperty, ColorTransition)
            Sine.AddTransition(Sine.FrequencyProperty, Frequency)
            Sine.AddTransition(Sine.DepthProperty, Depth)
            Sine.AddTransition(Sine.RadiusProperty, Radius)

            'Add debugging information text
            AddGameObject(DebuggingInfomation)
            'Add debugging information transition
            Sine.AddTransition(DebuggingInfomation.ColorProperty, ColorTransition) ' DebugTextColorTransition)
            'Add fullscreen information text
            AddGameObject(FullScreenInformation)
            'Add the same debugging transition to fullscreen text
            FullScreenInformation.AddTransition(FullScreenInformation.ColorProperty, ColorTransition) ' DebugTextColorTransition)
            'Add paused text
            AddGameObject(PausedInformation)

            'Add paused transition
            PausedInformation.AddTransition(PausedInformation.ColorProperty, PausedTextColorTransition)

            ' Make an audio stream to open our music
            Dim AudioStream As IO.Stream
            'Find the executing assembly and store it
            Dim ExecutingAssembly As [Assembly] = [Assembly].GetExecutingAssembly()
            'Get the song from the assembly
            AudioStream = ExecutingAssembly.GetManifestResourceStream("MiniGameEngine.dancin.mp3")
            'Create a temporary stream for the music
            Dim AudioWriteStream As New IO.FileStream(MP3Path, IO.FileMode.Create)
            'Store the music somewhere
            Dim AudioBuffer(CInt(AudioStream.Length)) As Byte
            'Read the music from the stream into the data byte array variable
            AudioStream.Read(AudioBuffer, 0, CInt(AudioStream.Length))
            'Write the music to the temporary file using the stream
            AudioWriteStream.Write(AudioBuffer, 0, CInt(AudioStream.Length))
            'Close the stream, now ready to be played
            AudioWriteStream.Close()
            'Play the music from temporary file 
            WMPObject.settings.autoStart = False
            WMPObject.URL = MP3Path
            WMPObject.settings.setMode("loop", True)
            'WMPObject.controls.play()
            Paused = True
        End Sub
#End Region

#Region "Mouse & Keyboard Input"
        Public Overrides Sub MouseClick(MouseButton As MouseButtons)
            If MouseButton = MouseButtons.Right Then
                'Toggle the background color on right mouse button click
                BackgroundColor = If(BackgroundColor = Color.White, Color.Black, Color.White)
                ColorTransition.HSL.L = If(BackgroundColor = Color.White, COLOR_TRANSITION_HSL_LIGHTNESS_LIGHT, COLOR_TRANSITION_HSL_LIGHTNESS_DARK)
                ColorTransition.HSL.S = If(BackgroundColor = Color.White, COLOR_TRANSITION_HSL_SATURATION_LIGHT, COLOR_TRANSITION_HSL_SATURATION_DARK)
            ElseIf MouseButton = MouseButtons.Left Then
                'Advance to the next sine type
                AdvanceSineType()
            End If
        End Sub

        Public Sub Resized()
            Position.MaxPosition = MaxTransitionalPosition
            Radius.MaxDouble = MaxTransitionalRadius
            PausedInformation.Position = Game.MIDDLE_POS
            FullScreenInformation.Position = New Point(Game.BOTTOM_MIDDLE_POS.X, Game.BOTTOM_MIDDLE_POS.Y - 100)
        End Sub

        Public Overrides Sub KeyDown(KeyCode As Keys)
            'Change window fullscreen mode for double click on left mouse button
            If KeyCode = Keys.F11 Then
                Game.Fullscreen = If(Game.Fullscreen, False, True)
            End If

            If KeyCode = Keys.F5 Then
                Sine.ShowMidPoint = Not Sine.ShowMidPoint
            End If

            If KeyCode = Keys.Space Then
                Paused = Not Paused
            End If
        End Sub
#End Region

#Region "Game Update & Render"
        Private ReadOnly DebugText As New StatisticVariable(Of String)(AddressOf UpdateDebuggingText, TimeSpan.FromMilliseconds(50))
        Private Function UpdateDebuggingText() As String
            Return $"Sine Generator [{Sine.GetStatistics()}]{Environment.NewLine}TextColor:{ColorTransition.Value}"
        End Function

        Public Overrides Sub Update(delta As Double)
            MyBase.Update(delta)
            DebuggingInfomation.Text = DebugText.Value
        End Sub

        Public Overrides Sub Render(g As Graphics)
            MyBase.Render(g)
        End Sub
#End Region

        Private Property Paused As Boolean
            Get
                Return PausedInformation.Visible
            End Get
            Set(value As Boolean)
                If (value) Then
                    WMPObject.controls.pause()
                    Sine.Pause()
                    DebuggingInfomation.Pause()
                Else
                    WMPObject.controls.play()
                    Sine.Resume()
                    DebuggingInfomation.Resume()
                End If

                PausedInformation.Visible = value
            End Set
        End Property

        Public Overrides Sub ExitGame()
            MyBase.ExitGame()
            WMPObject.controls.stop()
            WMPObject.close()
            IO.File.Delete(MP3Path)
        End Sub
    End Class
End Namespace

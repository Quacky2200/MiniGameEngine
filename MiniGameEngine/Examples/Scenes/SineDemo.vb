Imports MiniGameEngine
Imports MiniGameEngine.Transitions
Imports System.Reflection
Imports WMPLib
Imports System.Drawing
Imports System.Windows.Forms
Imports MiniGameEngine.Examples.Shapes
Imports MiniGameEngine.Examples.Shapes.SineCircles
Imports MiniGameEngine.UI

Namespace Examples.Scenes
    Public Class SineDemo
        Inherits Scene
        Public Sine As New SineCircle(Game.MIDDLE_POS, Game.Height / 3, 40, 40)

#Region "General Properties"
        Private Property Paused As Boolean
            Get
                Return PausedInformation.Visible
            End Get
            Set(value As Boolean)
                If (value) Then
                    WMPObject.controls.pause()
                Else
                    WMPObject.controls.play()
                End If

                Radius.Paused = value
                Frequency.Paused = value
                Depth.Paused = value
                Position.Paused = value
                Rotation.Paused = value
                SineColorTransition.Paused = value
                PausedInformation.Visible = value
                TextColorTransition.Paused = value
            End Set
        End Property

        'Music
        Private MP3Path As String = IO.Path.Combine(My.Application.Info.DirectoryPath, "temp.mp3")
        Private WMPObject As New WMPLib.WindowsMediaPlayer()

        'Information
        Private DebuggingInfomation As New TextElement("") With {
            .Position = New Point(0, 15),
            .Font = New Font("consolas", 10)
        }

        Private FullScreenInformation As New TextElement("Double-click to toggle fullscreen") With {
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
            .zIndex = 99
        }

        'Text Transition
        Private TextColorTransition As New ColorTransition(Color.Red, Color.Blue) With {
            .Duration = TimeSpan.FromSeconds(5),
            .Repeat = True,
            .Reverse = True,
            .ReverseUsesDuration = True,
            .Enabled = True
        }

        Private PausedTextColorTransition As New ColorTransition(Color.Black, Color.White) With {
            .Repeat = True,
            .Reverse = True,
            .Enabled = True,
            .Duration = TimeSpan.FromMilliseconds(100)
        }

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
            Rotation.Duration = TimeSpan.FromSeconds(3)
            Position.Duration = TimeSpan.FromSeconds(4)
        End Sub

        Private Sub FastSineCirclesTransition()
            Radius.Duration = TimeSpan.FromSeconds(1)
            Frequency.Duration = TimeSpan.FromSeconds(1)
            Depth.Duration = TimeSpan.FromSeconds(2)
            Position.Duration = TimeSpan.FromMilliseconds(800)
            Rotation.Duration = TimeSpan.FromSeconds(1)
        End Sub

        Public Sub AdvanceSineType()
            CurrentCircle = (CurrentCircle + 1) Mod Circles.Count
            'Dim dip As Integer = currentSineType
            'While dip = currentSineType
            '    dip = random.Next(0, circletypes.Count)
            'End While
            'currentSineType = dip
            Dim key = Circles.Keys.ToArray(CurrentCircle)
            Sine.type = key
            Circles(key)()
        End Sub
#End Region

#Region "Transitions"

        Public Radius As New RandomDoubleTransition(50, 20, MaxTransitionalRadius()) With {.Repeat = True, .Reverse = True, .Enabled = True}

        Public Frequency As New RandomDoubleTransition(24, 0, 85) With {.Repeat = True, .Reverse = True, .Enabled = True}

        Public Depth As New RandomDoubleTransition(25, 0, 80) With {.Repeat = True, .Reverse = True, .Enabled = True}

        Public Position As New RandomPointTransition(Game.MIDDLE_POS, MinTransitionalPosition(), MaxTransitionalPosition()) With {.Repeat = True, .Reverse = True, .Enabled = True}

        Public Rotation As New RandomDoubleTransition(0, 360) With {.Repeat = True, .Reverse = True, .ReverseUsesDuration = True, .Enabled = True}

        Public SineColorTransition As New ColorTransition(Color.Red, Color.LimeGreen) With {.Duration = TimeSpan.FromSeconds(5), .Repeat = True, .Reverse = True, .Enabled = True}

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
            'Add the sine circle
            AddGameObject(Sine)
            'Get the first Sine Type and change it
            AdvanceSineType()

            'add all of the Sine Circle Transitions
            Sine.AddTransition(Sine.RotationProperty, Rotation)
            Sine.AddTransition(Sine.PositionProperty, Position)
            Sine.AddTransition(Sine.ColorProperty, SineColorTransition)
            Sine.AddTransition(Sine.FrequencyProperty, Frequency)
            Sine.AddTransition(Sine.DepthProperty, Depth)
            Sine.AddTransition(Sine.RadiusProperty, Radius)

            'Add the debugging information text
            AddGameObject(DebuggingInfomation)
            'Add the debugging information transition
            Sine.AddTransition(DebuggingInfomation.ColorProperty, TextColorTransition)
            'Add the fullscreen information text
            AddGameObject(FullScreenInformation)
            'Add the same debugging transition to fullscreen text
            FullScreenInformation.AddTransition(FullScreenInformation.ColorProperty, TextColorTransition)
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
            WMPObject.URL = MP3Path
            WMPObject.settings.setMode("loop", True)
            WMPObject.controls.play()
        End Sub
#End Region

#Region "Mouse Positions"
        Public Overrides Sub MouseClick(MouseButton As MouseButtons)
            If MouseButton = MouseButtons.Right Then
                'Toggle the background color on right mouse button click
                BackgroundColor = If(BackgroundColor = Color.White, Color.Black, Color.White)
            ElseIf MouseButton = MouseButtons.Left Then
                'Advance to the next sine type
                AdvanceSineType()
            End If
        End Sub

        Public Overrides Sub KeyDown(KeyCode As Keys)
            'Change window fullscreen mode for double click on left mouse button
            If KeyCode = Keys.F11 Then
                Game.Fullscreen = If(Game.Fullscreen, False, True)
                Position.maxPosition = MaxTransitionalPosition
                Radius.maxDouble = MaxTransitionalRadius
                PausedInformation.Position = Game.MIDDLE_POS
                FullScreenInformation.Position = New Point(Game.BOTTOM_MIDDLE_POS.X, Game.BOTTOM_MIDDLE_POS.Y - 100)
            End If

            If KeyCode = Keys.F5 Then
                Sine.ShowMidPoint = Not Sine.ShowMidPoint
            End If

            If KeyCode = Keys.Space Then
                'Toggle pause on the left mouse button click
                Paused = Not Paused
            End If
        End Sub
#End Region

#Region "Game Update & Render"
        Public Overrides Sub Update(delta As Double)
            DebuggingInfomation.Text = String.Format("Sine Generator [{0}]{1}TextColor:{2}", Sine.GetStatistics(), Environment.NewLine, TextColorTransition.lastValue)
            MyBase.Update(delta)
        End Sub

        Public Overrides Sub Render(g As Graphics)
            MyBase.Render(g)
        End Sub
#End Region

        Public Overrides Sub ExitGame()
            MyBase.ExitGame()
            WMPObject.controls.stop()
            IO.File.Delete(MP3Path)
        End Sub
    End Class
End Namespace

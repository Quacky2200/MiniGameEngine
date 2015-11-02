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
        'Music
        Private tempMP3 As String = My.Computer.FileSystem.SpecialDirectories.Temp & "\temp.mp3"
        Private player As New WMPLib.WindowsMediaPlayer()

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

        Private PausedInformation As New TextElement("Paused!") With {
            .Font = New Font("consolas", 20),
            .Position = Game.MIDDLE_POS,
            .HorizontalAlignment = UI.HorizontalAlignment.Center,
            .VerticalAlignment = UI.VerticalAlignment.Center,
            .Visible = False
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
            .ReverseUsesDuration = True
        }

#End Region
#Region "Sine Circle Types"
        Private currentSineType As Integer = -1
        Private circletypes As New Dictionary(Of SineCircleType, Action) From {
            {New PiSineType(Sine), AddressOf fastSineCirclesTransition}, _
            {New NormalSineCirclePathType(Sine), AddressOf fastSineCirclesTransition}, _
            {New DoubleSineCirclePathType(Sine), AddressOf slowSineCirclesTransition}, _
            {New DoubleCosCirclePathType(Sine), AddressOf slowSineCirclesTransition} _
        }

        Private Sub slowSineCirclesTransition()
            Radius.Duration = TimeSpan.FromSeconds(10)
            Frequency.Duration = TimeSpan.FromSeconds(10)
            Depth.Duration = TimeSpan.FromSeconds(10)
            Rotation.Duration = TimeSpan.FromSeconds(3)
            Position.Duration = TimeSpan.FromSeconds(4)
        End Sub
        Private Sub fastSineCirclesTransition()
            Radius.Duration = TimeSpan.FromSeconds(1)
            Frequency.Duration = TimeSpan.FromSeconds(1)
            Depth.Duration = TimeSpan.FromSeconds(2)
            Position.Duration = TimeSpan.FromMilliseconds(800)
            Rotation.Duration = TimeSpan.FromSeconds(1)
        End Sub

        Public Sub advanceSineType()
            currentSineType = (currentSineType + 1) Mod circletypes.Count
            'Dim dip As Integer = currentSineType
            'While dip = currentSineType
            '    dip = random.Next(0, circletypes.Count)
            'End While
            'currentSineType = dip
            Dim key = circletypes.Keys.ToArray(currentSineType)
            Sine.type = key
            circletypes(key)()
        End Sub
#End Region

#Region "Transitions"

        Public Radius As New RandomDoubleTransition(50, 20, maxTransitionalRadius()) With {.Repeat = True, .Reverse = True, .Enabled = True}

        Public Frequency As New RandomDoubleTransition(24, 0, 85) With {.Repeat = True, .Reverse = True, .Enabled = True}

        Public Depth As New RandomDoubleTransition(25, 0, 80) With {.Repeat = True, .Reverse = True, .Enabled = True}

        Public Position As New RandomPointTransition(Game.MIDDLE_POS, minTransitionalPosition(), maxTransitionalPosition()) With {.Repeat = True, .Reverse = True, .Enabled = True}

        Public Rotation As New RandomDoubleTransition(0, 360) With {.Repeat = True, .Reverse = True, .ReverseUsesDuration = True, .Enabled = True}

        Public SineColorTransition As New ColorTransition(Color.Red, Color.LimeGreen) With {.Duration = TimeSpan.FromSeconds(5), .Repeat = True, .Reverse = True, .Enabled = True}

#End Region

#Region "max Properties"
        Public ReadOnly Property maxTransitionalPosition() As Point
            Get
                Return New Point(CInt((Game.Width / 4) * 3), CInt((Game.Height / 4) * 3))
            End Get
        End Property
        Public ReadOnly Property minTransitionalPosition() As Point
            Get
                Return New Point(CInt(Game.Width / 4), CInt(Game.Height / 4))
            End Get
        End Property
        Public ReadOnly Property maxTransitionalRadius() As Double
            Get
                Return Game.Height / 3
            End Get
        End Property
#End Region

#Region "Construction"

        Public Sub New(game As GameContainer)
            MyBase.New(game)
            BackgroundColor = Color.White
        End Sub

        Public Overrides Sub Init()
            'Add the sine circle
            add(Sine)
            'Get the first Sine Type and change it
            advanceSineType()
            'add all of the Sine Circle Transitions
            add(Sine.RotationProperty, Rotation)
            add(Sine.PositionProperty, Position)
            add(Sine.ColorProperty, SineColorTransition)
            add(Sine.FrequencyProperty, Frequency)
            add(Sine.DepthProperty, Depth)
            add(Sine.RadiusProperty, Radius)

            
            'Add the debugging information text
            add(DebuggingInfomation)
            'Add the debugging information transition
            add(DebuggingInfomation.ColorProperty, TextColorTransition)
            'Add the fullscreen information text
            add(FullScreenInformation)
            'Add the same debugging transition to fullscreen text
            add(FullScreenInformation.ColorProperty, TextColorTransition)
            'Add paused text
            add(PausedInformation)
            'Add paused transition
            add(PausedInformation.ColorProperty, PausedTextColorTransition)
            ' Make an audio stream to open our music
            Dim audioStream As IO.Stream
            'Find the executing assembly and store it
            Dim _assembly As [Assembly] = [Assembly].GetExecutingAssembly()
            'Get the song from the assembly
            audioStream = _assembly.GetManifestResourceStream("MiniGameEngine.dancin.mp3")
            'Create a temporary stream for the music
            Dim _tempaudioStream As New IO.FileStream(tempMP3, IO.FileMode.Create)
            'Store the music somewhere
            Dim data(CInt(audioStream.Length)) As Byte
            'Read the music from the stream into the data byte array variable
            audioStream.Read(data, 0, CInt(audioStream.Length))
            'Write the music to the temporary file using the stream
            _tempaudioStream.Write(data, 0, CInt(audioStream.Length))
            'Close the stream, now ready to be played
            _tempaudioStream.Close()
            'Play the music from temporary file 
            player.URL = tempMP3
            player.settings.setMode("loop", True)
            player.controls.play()
        End Sub
#End Region

#Region "Mouse Positions"
        Public Overrides Sub MouseClick(MouseButton As MouseButtons)
            If MouseButton = MouseButtons.Right Then
                'Toggle the background color on right mouse button click
                BackgroundColor = If(BackgroundColor = Color.White, Color.Black, Color.White)
            ElseIf MouseButton = MouseButtons.Left Then
                'Toggle pause on the left mouse button click
                Game.Paused = If(Game.Paused, False, True)
            ElseIf MouseButton = MouseButtons.Middle Then
                'Advance to the next sine type
                advanceSineType()
            End If
        End Sub
        Public Overrides Sub onPause()
            If Game.Paused Then
                player.controls.pause()
            Else
                player.controls.play()
            End If
        End Sub
        Public Overrides Sub MouseDoubleClick(MouseButton As MouseButtons)
            'Change window fullscreen mode for double click on left mouse button
            If MouseButton = MouseButtons.Left Then
                Game.Fullscreen = If(Game.Fullscreen, False, True)
                Position.maxPosition = maxTransitionalPosition
                Radius.maxDouble = maxTransitionalRadius
                PausedInformation.Position = Game.MIDDLE_POS
                FullScreenInformation.Position = New Point(Game.BOTTOM_MIDDLE_POS.X, Game.BOTTOM_MIDDLE_POS.Y - 100)
            End If
        End Sub
#End Region

#Region "Game Update & Render"
        Public Overrides Sub Update(delta As Double)
            DebuggingInfomation.Text = String.Format("Sine Generator [{0}]{1}TextColor:{2}", Sine.toString(), Environment.NewLine, TextColorTransition.lastValue)
        End Sub

        Public Overrides Sub Render(g As Graphics)

        End Sub

#End Region

        Public Overrides Sub ExitGame()
            player.controls.stop()
        End Sub
    End Class
End Namespace

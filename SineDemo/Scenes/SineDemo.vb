Imports MiniGameEngine

Imports MiniGameEngine.Transitions
Imports System.Reflection
Imports WMPLib
Public Class SineDemo
    Inherits Scene


#Region "General Properties"
    Private Property Paused As Boolean = False
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
        .HorizontalAlignment = HorizontalAlignment.Center,
        .VerticalAlignment = VerticalAlignment.Center
    }

    Private PausedInformation As New TextElement("Paused!") With {
        .Font = New Font("consolas", 20),
        .Position = Game.MIDDLE_POS,
        .HorizontalAlignment = HorizontalAlignment.Center,
        .VerticalAlignment = VerticalAlignment.Center,
        .Visible = False
    }

    'Text Transition
    Private TextColorTransition As New ColorTransition(Color.Red, Color.Blue) With {
        .Duration = TimeSpan.FromSeconds(5),
        .Repeat = True,
        .Reverse = True,
        .Enabled = True
    }

    Private PausedTextColorTransition As New ColorTransition(Color.Black, Color.White) With {
        .Repeat = True,
        .Reverse = True
    }

#End Region

#Region "Construction"

    Public Sub New(game As Game)
        MyBase.New(game)
        BackgroundColor = Color.White
    End Sub

    Public Overrides Sub Init()
        'add all of the Sine Circle Transitions
        SineDemoCircleExample.init(Game)
        'Add the sine circle
        Game.addGameObject(SineDemoCircleExample.Sine)
        'Get the first Sine Type and change it
        SineDemoCircleExample.advanceSineType()
        'Add the debugging information text
        Game.addGameObject(DebuggingInfomation)
        'Add the debugging information transition
        Game.addTransition(DebuggingInfomation.ColorProperty, TextColorTransition)
        'Add the fullscreen information text
        Game.addGameObject(FullScreenInformation)
        'Add the same debugging transition to fullscreen text
        Game.addTransition(FullScreenInformation.ColorProperty, TextColorTransition)
        'Add paused text
        Game.addGameObject(PausedInformation)
        'Add paused transition
        Game.addTransition(PausedInformation.ColorProperty, PausedTextColorTransition)
        'Make an audio stream to open our music
        Dim audioStream As IO.Stream
        'Find the executing assembly and store it
        Dim _assembly As [Assembly] = [Assembly].GetExecutingAssembly()
        'Get the song from the assembly
        audioStream = _assembly.GetManifestResourceStream("SineWaveOverTime.Aaron_Smith_-_Dancin_(KRONO_Remix).mp3")
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
        'player.URL = tempMP3
        'player.settings.setMode("loop", True)
        ' player.controls.play()
    End Sub

    Public Overrides Sub Enter(lastScene As Scene)
        Game.Text = "Sine Generator"
    End Sub
#End Region

#Region "Mouse Positions"
    Public Overrides Sub MouseClick(MouseButton As MouseButtons)
        If MouseButton = MouseButtons.Right Then
            'Toggle the background color on right mouse button click
            BackgroundColor = If(BackgroundColor = Color.White, Color.Black, Color.White)
        ElseIf MouseButton = MouseButtons.Left Then
            'Toggle pause on the left mouse button click
            Paused = If(Paused, False, True)
            'toggle all transitions
            SineDemoCircleExample.pause(Paused)
            'toggle the text color transition
            TextColorTransition.Paused = Paused
            'Show/Hide the paused information (since when paused is true, visible will be true).
            PausedInformation.Visible = Paused
            'Enable/Disable the color transition
            PausedTextColorTransition.Enabled = Paused
        ElseIf MouseButton = MouseButtons.Middle Then
            'Advance to the next sine type
            SineDemoCircleExample.advanceSineType()
        End If
    End Sub

    Public Overrides Sub MouseDoubleClick(MouseButton As MouseButtons)
        'Change window fullscreen mode for double click on left mouse button
        If MouseButton = MouseButtons.Left Then
            Game.Fullscreen = If(Game.Fullscreen, False, True)
            SineDemoCircleExample.Position.maxPosition = SineDemoCircleExample.maxTransitionalPosition
            SineDemoCircleExample.Radius.maxDouble = SineDemoCircleExample.maxTransitionalRadius
            PausedInformation.Position = Game.MIDDLE_POS
            FullScreenInformation.Position = New Point(Game.BOTTOM_MIDDLE_POS.X, Game.BOTTOM_MIDDLE_POS.Y - 100)
        End If
    End Sub
#End Region

#Region "Game Update & Render"
    Public Overrides Sub Update(delta As Double)
        DebuggingInfomation.Text = String.Format("Sine Generator [{0}]{1}TextColor:{2}", SineDemoCircleExample.Sine.toString(), Environment.NewLine, TextColorTransition.lastValue)
    End Sub

    Public Overrides Sub Render(g As Graphics)

    End Sub

#End Region

    Public Overrides Sub ExitGame()
        ' player.controls.stop()
    End Sub

End Class
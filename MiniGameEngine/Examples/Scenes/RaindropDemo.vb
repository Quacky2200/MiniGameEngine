Imports MiniGameEngine.Examples.Droplets
Imports System.Drawing
Imports MiniGameEngine.Transitions
Imports MiniGameEngine.UI
Imports MiniGameEngine.General.Color
Imports System.Reflection
Imports WMPLib

Namespace Examples.Scenes
    Public Class RaindropDemo
        Inherits Scene

        Public Property MaxRadius As Integer = 125
        Public Property MinRadius As Integer = 20

        Private WeatherInformation As New TextElement("Weather") With {
            .Font = New Font("consolas", 20),
            .Position = Game.MIDDLE_POS,
            .HorizontalAlignment = UI.HorizontalAlignment.Center,
            .VerticalAlignment = UI.VerticalAlignment.Center,
            .Color = Color.White,
            .Visible = True,
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

        Enum WeatherChance As UShort
            Drizzle = 7
            Rain = 3
            Stormy = 0
        End Enum
        Public Property Weather As WeatherChance = WeatherChance.Stormy

        Private LastUpdate As Long = Now.Ticks
        Private Random As New Random
        Private Cloud As New Raindrops(Me)
        Private WMPObject As New WindowsMediaPlayer()

        Public Sub New(Game As GameContainer)
            MyBase.New(Game)
            BackgroundColor = Color.Black
        End Sub

        Private MP3Path = IO.Path.Combine(My.Application.Info.DirectoryPath, "rains-falling-on-my-head.mp3")
        Public Overrides Sub Init()
            Game.MustSmooth = True
            Game.MustInterpolate = True
            Game.MustAntiAliasText = True

            AddHandler Game.Window.SizeChanged, AddressOf Resize
            AddGameObject(WeatherInformation)
            UpdateText()

            ' Make an audio stream to open our music
            Dim AudioStream As IO.Stream
            'Find the executing assembly and store it
            Dim ExecutingAssembly As [Assembly] = [Assembly].GetExecutingAssembly()
            'Get the song from the assembly
            AudioStream = ExecutingAssembly.GetManifestResourceStream("MiniGameEngine.rains-falling-on-my-head-loop.mp3")
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

        Public Overrides Sub ExitGame()
            MyBase.ExitGame()
            WMPObject.controls.stop()
            WMPObject.close()
            IO.File.Delete(MP3Path)
        End Sub

        Private LastSpawn As New Point(0, 0)

        Public Overrides Sub Update(delta As Double)
            MyBase.Update(delta)

            ' Perform rain check every 25ms
            Dim Diff As Long = Now.Ticks - LastUpdate
            Dim AsTimespan = TimeSpan.FromTicks(Diff)
            If AsTimespan.Milliseconds < 25 Then Return

            Dim Chance = Random.Next(0, 256)

            If (Chance And Weather) = Weather Then
                Select Case Random.Next(1, 11)
                    Case 10
                        Cloud.DropletRadius = Random.Next(100, 200)
                        Cloud.DropletCount = 8
                        Cloud.MovementDuration = TimeSpan.FromMilliseconds(Cloud.MovementDuration.TotalMilliseconds * 2)
                        Cloud.DropletColor = RGBHSL.ModifyBrightness(Color.FromArgb(255, 50, 95, 140), 0.25)
                    Case Else
                        Cloud.DropletColor = Color.FromArgb(255, 50, 95, 140)
                        Cloud.DropletRadius = Random.Next(20, 100)
                        Cloud.DropletCount = 8
                        Cloud.MovementDuration = TimeSpan.FromMilliseconds(800)
                        LastSpawn = New Point(Random.Next(MinRadius, Game.Width - MaxRadius), Random.Next(MinRadius, Game.Height - MaxRadius))
                End Select
                Cloud.Spawn(LastSpawn)
            End If
            LastUpdate = Now.Ticks
        End Sub

        Public Overrides Sub MouseClick(MouseButton As Windows.Forms.MouseButtons)
            MyBase.MouseClick(MouseButton)

            ToggleWeather()
        End Sub

        Private Sub Resize()
            WeatherInformation.Position = Game.MIDDLE_POS
        End Sub

        Public Overrides Sub KeyDown(KeyCode As Windows.Forms.Keys)
            If (KeyCode = Windows.Forms.Keys.F11) Then
                Game.Fullscreen = Not Game.Fullscreen
            End If
        End Sub

        Private Sub ToggleWeather()
            Select Case Weather
                Case WeatherChance.Drizzle
                    Weather = WeatherChance.Rain
                Case WeatherChance.Rain
                    Weather = WeatherChance.Stormy
                Case WeatherChance.Stormy
                    Weather = WeatherChance.Drizzle
            End Select

            UpdateText()
        End Sub

        Private Sub UpdateText()
            WeatherInformation.Text = "Weather: "

            Select Case Weather
                Case WeatherChance.Drizzle
                    WeatherInformation.Text += "Drizzle"
                Case WeatherChance.Rain
                    WeatherInformation.Text += "Rain"
                Case WeatherChance.Stormy
                    WeatherInformation.Text += "Stormy"
            End Select
        End Sub
    End Class
End Namespace
Imports MiniGameEngine.Examples.Droplets
Imports MiniGameEngine.General.Time
Imports System.Drawing
Imports MiniGameEngine.Transitions
Imports System.Windows.Forms
Imports MiniGameEngine.General.Threading

Namespace Examples.Scenes
    Public Enum WeatherType As Integer
        Drizzle = 2
        LightRain = 4
        Rain = 6
        Stormy = 8
        HailFreakinStorm = 10
    End Enum
    Public Class RaindropDemo
        Inherits Scene

        Public Property MaxRaindrops As Integer = 100
        Public Property MaxRadius As Integer = 125
        Public Property MinRadius As Integer = 20
        Public Property Type As WeatherType = WeatherType.Stormy

        Public Sub New(Game As GameContainer)
            MyBase.New(Game)
            BackgroundColor = Color.Black
        End Sub

        Private Random As New Random
        Private Cloud As New SmoothEchoDroplets(Me)
        Private Information As New MiniGameEngine.UI.TextElement("Current Weather: Unknown") With {
            .Font = New Font("Arial", 15),
            .Position = Game.MIDDLE_POS,
            .HorizontalAlignment = UI.HorizontalAlignment.Center,
            .Color = Color.Blue
        }
        Private Help As New MiniGameEngine.UI.TextElement("
Controls:
Spacebar - Toggle Pause,
       N - Change Weather,
       M - Rain Button
") With {
            .Font = New Font("Arial", 10),
            .Position = Game.TOP_RIGHT_POS,
            .HorizontalAlignment = UI.HorizontalAlignment.Right,
            .Color = Color.Gray
        }

        Private Pause As New MiniGameEngine.UI.TextElement("Rain Generation Paused") With {
            .Font = New Font("Arial", 15),
            .HorizontalAlignment = UI.HorizontalAlignment.Center,
            .Color = Color.Gray
        }

        Public Overrides Sub WindowSizeChange()
            Information.Position = Game.MIDDLE_POS

            Dim Pos = Game.MIDDLE_POS
            Pos.Offset(0, -25)
            Pause.Position = Pos

            Pos = Game.TOP_RIGHT_POS
            Pos.Offset(-20, 0)
            Help.Position = Pos
        End Sub

        Private WeatherChange As New Timer
        Private DropSpawn As New Timer
        Private Status As Boolean = True

        Public Sub ChangeWeather()
            Dim Choice As Integer = Math.Floor((New Random()).NextDouble() * _Amounts.Count)
            Weather = _Amounts.ElementAt(Choice)
            MinimumAmount = Weather.Value(0)
            MaximumAmount = Weather.Value(1)
            Information.Text = "Current Weather: " + Weather.Key
            Spawned = 0

            WeatherChange.Interval = Math.Floor((New Random()).NextDouble() * (30000 - 5000)) + 5000
        End Sub

        Public Sub Spawn()
            Dim RaindropAmount As Integer = Math.Floor(Random.NextDouble() * (MaximumAmount - MinimumAmount)) + MinimumAmount

            Dim CachedAmount As Integer = MaximumAmount
            For i = 0 To RaindropAmount
                Dim spawnPoint As New System.Drawing.Point(Random.Next(0, Game.Window.Size.Width + 150), Random.Next(0, Game.Window.Size.Height + 150))
                Spawned = (Spawned + 1) Mod MaximumAmount + 1
                ThreadWork.Start(Sub()
                                     Cloud.Spawn(spawnPoint)
                                 End Sub)
            Next
        End Sub

        Public Overrides Sub Init()
            AddHandler WeatherChange.Tick, AddressOf ChangeWeather
            WeatherChange.Interval = 1
            WeatherChange.Start()

            AddHandler DropSpawn.Tick, AddressOf Spawn
            DropSpawn.Interval = 50
            DropSpawn.Start()

            WindowSizeChange()
            Game.Window.FormBorderStyle = FormBorderStyle.Sizable
            add(Information)
            add(Help)
        End Sub

        Public Sub TogglePause()
            Status = Not Status
            If (Status) Then
                DropSpawn.Start()
                WeatherChange.Start()
                remove(Pause)
            Else
                add(Pause)
                DropSpawn.Stop()
                WeatherChange.Stop()
            End If
        End Sub

        Public Overrides Sub MouseDoubleClick(MouseButton As MouseButtons)
            Game.Fullscreen = Not Game.Fullscreen
        End Sub

        Public Overrides Sub MouseClick(MouseButton As MouseButtons)
            'If MouseButton = MouseButtons.Left Then
            '    TogglePause()
            'Else
            '    ChangeWeather()
            'End If

        End Sub

        Public Overrides Sub KeyPress(KeyCode As Keys)
            Select Case KeyCode
                Case Keys.N
                    ChangeWeather()
                    Return
                Case Keys.M
                    Spawn()
                    Return
                Case Keys.F
                    Game.Fullscreen = Not Game.Fullscreen
                    Return
                Case Keys.Space
                    TogglePause()
                    Return
            End Select
        End Sub

        Public Overrides Sub Render(g As Drawing.Graphics)

        End Sub

        Private Weather As KeyValuePair(Of String, Integer())
        Private _Amounts As New Dictionary(Of String, Integer()) From {
            {"Drizzle", {0, 5}},
            {"Light Rain", {0, 20}},
            {"Rain Showers", {0, 50}},
            {"Heavy Rain", {50, 150}},
            {"Da-Rude Rain-Storm", {100, 200}},
            {" The """"We're going to Die!"""" Hurricane", {250, 300}}
        }
        Private MaximumAmount As Double = 50
        Private MinimumAmount As Double = 0
        Private Spawned As Integer = 0

        Public Overrides Sub Update(delta As Double)
            Information.Text = String.Format("Current Weather: {0} (spawned {1} of {2})", Weather.Key, Spawned, MaximumAmount)
        End Sub
    End Class
End Namespace
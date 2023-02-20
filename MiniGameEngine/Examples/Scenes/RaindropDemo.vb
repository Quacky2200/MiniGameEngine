Imports MiniGameEngine.Examples.Droplets
Imports MiniGameEngine.General.Time
Imports System.Drawing
Imports MiniGameEngine.Transitions
Imports MiniGameEngine.UI

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

        Public Sub New(Game As GameContainer)
            MyBase.New(Game)
            BackgroundColor = Color.Black
        End Sub

        Public Overrides Sub Init()
            AddGameObject(WeatherInformation)
            UpdateText()
        End Sub

        Public Overrides Sub Update(delta As Double)
            MyBase.Update(delta)

            ' Perform rain check every 100ms
            Dim Diff As Long = Now.Ticks - LastUpdate
            Dim AsTimespan = TimeSpan.FromTicks(Diff)
            If AsTimespan.Milliseconds < 50 Then Return

            Dim Chance = Random.Next(0, 256)

            If (Chance And Weather) = Weather Then
                Cloud.DropletRadius = Random.Next(20, 100)
                Cloud.DropletCount = Random.Next(1, 5)

                Select Case Random.Next(1, 6)
                    Case 1
                        Cloud.DropletColor = Color.MediumPurple
                    Case 2
                        Cloud.DropletColor = Color.LightBlue
                    Case 3
                        Cloud.DropletColor = Color.LightCoral
                    Case 4
                        Cloud.DropletColor = Color.PeachPuff
                    Case 5
                        Cloud.DropletColor = Color.PaleGreen
                    Case 6
                        Cloud.DropletColor = Color.LightSalmon
                End Select

                Cloud.Spawn(New Point(Random.Next(MinRadius, Game.Window.Size.Width - MaxRadius), Random.Next(MinRadius, Game.Window.Size.Height - MaxRadius)))
            End If
            LastUpdate = Now.Ticks
        End Sub

        Public Overrides Sub MouseClick(MouseButton As Windows.Forms.MouseButtons)
            MyBase.MouseClick(MouseButton)

            ToggleWeather()
        End Sub

        Public Overrides Sub KeyDown(KeyCode As Windows.Forms.Keys)
            If (KeyCode = Windows.Forms.Keys.F11) Then
                Game.Fullscreen = Not Game.Fullscreen
                WeatherInformation.Position = Game.MIDDLE_POS
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
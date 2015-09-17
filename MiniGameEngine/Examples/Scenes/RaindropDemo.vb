Imports MiniGameEngine.Examples.Droplets
Imports MiniGameEngine.General.Time
Imports System.Drawing
Imports MiniGameEngine.Transitions

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

        Public Property MaxRaindrops As Integer = 10
        Public Property MaxRadius As Integer = 125
        Public Property MinRadius As Integer = 20
        Public Property Type As WeatherType = WeatherType.Stormy



        Public Sub New(Game As GameContainer)
            MyBase.New(Game)
            BackgroundColor = Color.Black
        End Sub

        Private Property multiplier As Integer = multiplierMaximumAmmount / Type
        Public Property multiplierAmmount As Double = 5
        Private Const multiplierMinimumAmmount As Double = 1
        Private Const multiplierMaximumAmmount As Double = 10
        Private Property isMouseDown As Boolean = False
        Private Property liveTicks As Long = Now.Ticks
        Private Random As New Random
        Private Cloud As New Raindrops(Me)
        Public RainDifferential As Double = 1
        Private RainArea As New List(Of Shapes.Circle)
        'TODO: Mouse down to make it rain (brew a rain cloud, lol)
        'TODO: Rain to die out once mouse button is up, settling the cloud.
        'TODO: Make list full of rain
        Private makeItRainInfo As New UI.TextElement("Left click to make it rain") With {.Font = New Font(Game.Window.Font.FontFamily, 20), .Color = Color.Transparent, .HorizontalAlignment = UI.HorizontalAlignment.Center, .VerticalAlignment = UI.VerticalAlignment.Bottom}
        Private showMakeItRainTransition As New ColorTransition(Color.Transparent, Color.Transparent, Color.White, TimeSpan.FromSeconds(1.5))
        Private hideMakeItRainTransition As New ColorTransition(Color.White, Color.White, Color.Transparent, TimeSpan.FromSeconds(1.5))
        Private debugInfo As New UI.TextElement("") With {.Font = Game.Window.Font, .Color = Color.Red, .HorizontalAlignment = UI.HorizontalAlignment.Center}
        Public Overrides Sub Init()
            add(makeItRainInfo)
            add(debugInfo)
            debugInfo.Position = Game.MIDDLE_POS
        End Sub
        Public Overrides Sub MouseDown(MouseButton As Windows.Forms.MouseButtons)
            isMouseDown = (MouseButton = Windows.Forms.MouseButtons.Left)
        End Sub
        Public Overrides Sub MouseUp(MouseButton As Windows.Forms.MouseButtons)
            isMouseDown = Not (MouseButton = Windows.Forms.MouseButtons.Left)
        End Sub
        Public Overrides Sub Render(g As Drawing.Graphics)

        End Sub

        Public Overrides Sub Update(delta As Double)
            If Active Then
                'for each second the mouse is down, decrease the multiplier, thereby increasing the chance of rain
                Dim timer = New Watch(New Date(liveTicks), Watch.getNow).Milliseconds
                If isMouseDown And timer >= (1000 / Type) Then
                    multiplier -= If(multiplier - multiplierAmmount <= multiplierMinimumAmmount, 0, multiplierAmmount)
                    liveTicks = Now.Ticks
                    If (hideMakeItRainTransition.Enabled = False AndAlso Not hideMakeItRainTransition.isFinished) Or showMakeItRainTransition.isFinished Then
                        hideMakeItRainTransition.Reset()
                        hideMakeItRainTransition.Enabled = True
                    End If

                    add(makeItRainInfo.ColorProperty, hideMakeItRainTransition, True)
                ElseIf Not isMouseDown And timer >= (1000 / Type) Then
                    'Otherwise, for each second the mouse is not down, increase the multiplier, thereby decreasing the chance of rain
                    multiplier += If(multiplier + multiplierAmmount >= (multiplierMaximumAmmount / Type), 0, multiplierAmmount)
                    liveTicks = Now.Ticks
                    makeItRainInfo.Position = Game.MIDDLE_POS
                    If (showMakeItRainTransition.Enabled = False AndAlso Not showMakeItRainTransition.isFinished) Or hideMakeItRainTransition.isFinished Then
                        showMakeItRainTransition.Reset()
                        showMakeItRainTransition.Enabled = True
                    End If

                    add(makeItRainInfo.ColorProperty, showMakeItRainTransition, True)
                End If
                Dim update As Boolean = False
                Dim lastKnownChanceOfRaindrop As Integer
                If timer >= (1000 / Type) Then
                    update = True
                    Dim ChanceOfRaindrop As Integer = Math.Floor(Random.NextDouble() * multiplier) + multiplierMinimumAmmount
                    Dim PuddleAmmount As Integer = AllGameObjects.Where(Function(x) x.GetType() = GetType(Raindrops)).Count
                    lastKnownChanceOfRaindrop = ChanceOfRaindrop
                    If ChanceOfRaindrop = 1 And PuddleAmmount < MaxRaindrops + Type Then
                        'If we have hit the jackpot, and we need more rain
                        Dim spawnPoint As New System.Drawing.Point(Random.Next(MinRadius, Game.Window.Size.Width - MaxRadius), Random.Next(MinRadius, Game.Window.Size.Height - MaxRadius))
                        While AllGameObjects.Where(Function(x As Object) x.GetType() = GetType(Raindrops) AndAlso x.inBoundries(spawnPoint)).Count > 0
                            spawnPoint = New System.Drawing.Point(Random.Next(MinRadius, Game.Window.Size.Width - MaxRadius), Random.Next(MinRadius, Game.Window.Size.Height - MaxRadius))
                        End While
                        ' RainArea.Add(New Shapes.Circle(spawnPoint, MaxRadius))
                        Cloud.Spawn(spawnPoint)

                    End If
                    debugInfo.Text = String.Format("chance:{2}, isMouseDown:{0}, Update rain:{1}", If(isMouseDown, "Y", "X"), If(update, "Y", "X"), ChanceOfRaindrop)
                End If
            End If
        End Sub
    End Class
End Namespace
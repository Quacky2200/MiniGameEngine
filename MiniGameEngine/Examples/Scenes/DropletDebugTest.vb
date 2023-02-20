Imports MiniGameEngine.General.Threading
Imports MiniGameEngine.Transitions
Imports MiniGameEngine.Examples.Droplets
Imports System.Drawing
Imports System.Windows.Forms

Namespace Examples.Scenes
    Public Class DropletDebugTest
        Inherits Scene

        Private Drops As Droplets.Droplets

        Private DropTypes As New UI.TextElement("") With {
            .Font = New Font("consolas", 25),
            .HorizontalAlignment = UI.HorizontalAlignment.Center,
            .VerticalAlignment = UI.VerticalAlignment.Center
        }

        Public Sub New(Game As GameContainer)
            MyBase.New(Game)
            Me.BackgroundColor = Color.Black
        End Sub
        Private FadeInOut As New ColorTransition(Color.Transparent, Color.White) With {.Reverse = True, .ReverseUsesDuration = True, .Duration = TimeSpan.FromMilliseconds(500)}
        Public Overrides Sub MouseClick(MouseButton As MouseButtons)
            Select Case MouseButton
                Case MouseButtons.Left
                    'Dim t As New Examples.Shapes.Circle(lastKnownLocation, 100) With {.LineColor = Color.AliceBlue}

                    'AddGameObject(t)

                    Drops = New SimpleEchoDroplets(Me)
                Case MouseButtons.Right
                    Drops = New Raindrops(Me) With {.DropletFadeColor = Color.FromArgb(0, 0, 68, 255), .DropletColor = Color.FromArgb(100, 138, 226, 255)}
                Case Else
                    Drops = New Waterdrops(Me) With {.DropletFadeColor = Color.FromArgb(0, 0, 68, 255), .DropletColor = Color.FromArgb(100, 138, 226, 255)}
            End Select

            DropTypes.Position = Me.Game.MIDDLE_POS
            DropTypes.Text = "Test" 'Drops.GetType.Name.ToString
            If FadeInOut.Enabled = False Then
                FadeInOut.Reset()
                FadeInOut.Enabled = True
            End If

            DropTypes.AddTransition(DropTypes.ColorProperty, FadeInOut)
            ThreadWork.Start(Sub() Drops.Spawn(lastKnownLocation))
        End Sub
        Private lastKnownLocation As New Point(0, 0)
        Public Overrides Sub MouseMove(Location As Point)
            lastKnownLocation = Location
        End Sub

        Public Overrides Sub Init()
            AddGameObject(DropTypes)
        End Sub

        'Public Overrides Sub Render(g As Graphics)
        '    MyBase.Render(g)
        'End Sub

        'Public Overrides Sub Update(delta As Double)
        '    MyBase.Update(delta)
        'End Sub
    End Class
End Namespace

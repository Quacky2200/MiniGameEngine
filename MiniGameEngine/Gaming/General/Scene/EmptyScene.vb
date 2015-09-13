Imports System.Drawing
Public Class EmptyScene
    Inherits Scene
    Private WelcomeMessage As New TextElement("Welcome!") With {
        .Position = New Point(Game.MIDDLE_POS.X, Game.MIDDLE_POS.Y - 50),
        .Font = New Font(Game.Font.FontFamily, 45),
        .Color = Color.White,
        .HorizontalAlignment = HorizontalAlignment.Center
    }

    Private Instructions As New TextElement("To get started, add a scene to your project using the addScene sub procedure.") With {
        .Position = New Point(Game.MIDDLE_POS.X, Game.MIDDLE_POS.Y + 25),
        .Font = New Font(Game.Font.FontFamily, 14),
        .Color = Color.White,
        .HorizontalAlignment = HorizontalAlignment.Center
    }
    Public Sub New(game As Game)
        MyBase.New(game)
    End Sub
    Public Overrides Sub Init()
        Game.addGameObject(WelcomeMessage)
        Game.addGameObject(Instructions)
    End Sub

    Public Overrides Sub Render(g As Graphics)

    End Sub

    Public Overrides Sub Update(delta As Double)

    End Sub
End Class
Imports MiniGameEngine
Imports MiniGameEngine.Transitions
Imports MiniGameEngine.Examples.Shapes
Imports MiniGameEngine.UI

Public Class MainGameScene
    Inherits Scene

#Region ""

    Private ClickToBeginMsg As New TextElement("Click to Begin") With {
    .Color = Color.White,
    .HorizontalAlignment = HorizontalAlignment.Center,
    .VerticalAlignment = VerticalAlignment.Center,
    .Position = Game.MIDDLE_POS
}
    Private ClickToBeginMsgColorTransition As New ColorTransition(Color.Transparent, Color.White) With {
        .Enabled = True,
        .Repeat = True,
        .Reverse = True,
        .Duration = TimeSpan.FromMilliseconds(500)
    }

#End Region







    Const PAUSE_MESSAGE As String = "[PAUSED] - Press Escape again to Quit, or Click to Continue."
    ' Const BEGIN_MESSAGE As String = "Click to Begin"

    Dim CharacterPosition As Point
    ' Private lastDelta As Double = 0
    ' Private lastMousePosition As Point
    'Private MyFont As New System.Drawing.Font("consolas", 15) '("KOMIKA AXIS"
    'Private MyMessage As String = BEGIN_MESSAGE

    Dim Circle As New SineCircle(CharacterPosition, Game.Width / 10, 10, 20)
    'Private Color As New ColorStepTransition(System.Drawing.Color.FromArgb(0, 250, 250, 250), Color.White) With {.repeat = True, .speed = 550}
    Private Ready As Boolean = False
    Private Begin As Boolean = False
    Private goToDemo As Boolean = False
    'Private Sines As New List(Of SineCircleProperty)

    Private Chunks As New Dictionary(Of SineCircle, Object())

    Public Sub New(game As GameContainer)
        MyBase.New(game)
        Me.BackgroundColor = System.Drawing.Color.FromArgb(240, 40, 40, 40)
    End Sub

    Public Overrides Sub Init()
        'colorFade1.Start()
        CharacterPosition = Game.MIDDLE_POS
        ' lastMousePosition = Game.MIDDLE_POS
        'For i = 0 To (New Random).Next(0, 4)
        '    Sines.Add(New SineCircleProperty( _
        '        New Point((New Random).Next(-Game.Width * 3, Game.Width * 3), (New Random).Next(-Game.Height * 3, Game.Height * 3)), _
        '        (New Random).Next(Game.Width / 20, Game.Width / 10), _
        '        (New Random).Next(1, 30),
        '        (New Random).Next(1, 40)
        '    ))
        'Next
        'Add 
        AddGameObject(ClickToBeginMsg)
        'Game.addTransition(ClickToBeginMsg.ColorProperty, ClickToBeginMsgColorTransition)
    End Sub
    Public Overrides Sub Render(g As Graphics)
        MyBase.Render(g)
        ' Dim Path As PointF() = Circle.getSinePath(10, 20).PathPoints
        'g.DrawClosedCurve(New Pen(New SolidBrush(BadBacteriaTransition.Outline.value), 5), Path)
        ' g.FillClosedCurve(New SolidBrush(BadBacteriaTransition.Fill.value), Path)

        'Dim Circle2 As New SineCircle(CharacterPosition, Game.Width / 10)
        'Dim Path2 As PointF() = Circle.getSinePath(30, 40).PathPoints
        'g.DrawClosedCurve(New Pen(New SolidBrush(Color.LimeGreen), 5), Path2)
        'g.FillClosedCurve(New SolidBrush(Color.LawnGreen), Path2)
    End Sub
    Public Overrides Sub KeyDown(KeyCode As Keys)
        'Select Case KeyCode
        '    Case Keys.Escape
        '        If MyMessage = PAUSE_MESSAGE And Begin And Not Ready Then
        '            End
        '        ElseIf MyMessage <> PAUSE_MESSAGE And MyMessage = "" And Begin Then
        '            Ready = False
        '            ' colorFade1.Reverse = False
        '            ' colorFade1.ReverseDirection()
        '            ' colorFade1.Start()
        '            MyMessage = PAUSE_MESSAGE
        '        End If
        '    Case Keys.Tab
        '        If MyMessage = PAUSE_MESSAGE And Begin And Not Ready Then
        '            goToDemo = True
        '        End If
        '    Case Else
        '        Debug.WriteLine(KeyCode.ToString)
        'End Select
    End Sub
    Public Overrides Sub MouseMove(Location As Point)
        'lastMousePosition = Location
    End Sub
    Public Overrides Sub MouseClick(MouseButton As MouseButtons)
        ClickToBeginMsgColorTransition.Paused = If(ClickToBeginMsgColorTransition.Paused, False, True)
        If MouseButton = MouseButtons.Left And Not Begin And Not Ready Then
            'colorFade1.ReverseDirection(True)
            ' colorFade1.Start()
            'AddHandler colorFade1.OnFinish, AddressOf RemoveMessage
        End If
    End Sub
    Sub RemoveMessage(sender As Object)
        'MyMessage = ""
        'RemoveHandler colorFade1.OnFinish, AddressOf RemoveMessage
        Begin = True
        Ready = True
    End Sub
    Public Overloads Sub Update(delta As Double)
        MyBase.Update(delta)
        'BadBacteriaTransition.updateAll(delta)
        If goToDemo Then
            Game.SwitchScenes(Of Examples.Scenes.SineDemo)()
        End If
        ' lastDelta = delta

        If Not Ready Then
            Return
        End If
        Dim ScreenCenter = Game.MIDDLE_POS

        'CharacterPosition.X += delta * (ScreenCenter.X - lastMousePosition.X)
        'CharacterPosition.Y += delta * (ScreenCenter.Y - lastMousePosition.Y)
    End Sub
End Class

Option Strict Off
Imports MiniGameEngine
Imports MiniGameEngine.Transitions

Public Class SineDemoCircleExample
    'Public Shared Property Enabled As Boolean = True
#Region "Transitions"
    Public Shared Radius As New RandomDoubleTransition(50, 20, maxTransitionalRadius()) With {.Repeat = True, .Reverse = True, .Enabled = True}

    Public Shared Frequency As New RandomDoubleTransition(24, 0, 85) With {.Repeat = True, .Reverse = True, .Enabled = True}

    Public Shared Depth As New RandomDoubleTransition(25, 0, 80) With {.Repeat = True, .Reverse = True, .Enabled = True}

    Public Shared Position As New RandomPointTransition(Game.getInstance().MIDDLE_POS, minTransitionalPosition(), maxTransitionalPosition()) With {.Repeat = True, .Reverse = True, .Enabled = True}

    Public Shared Rotation As New RandomDoubleTransition(0, 360) With {.Repeat = True, .Reverse = True, .Enabled = True}

    Public Shared SineColorTransition As New ColorTransition(Color.Red, Color.LimeGreen) With {.Duration = TimeSpan.FromSeconds(5), .Repeat = True, .Reverse = True, .Enabled = True}


    Public Shared Sine As New SineCircle(Game.getInstance().MIDDLE_POS, Game.getInstance().Height / 3, 40, 40)
#End Region
#Region "max Properties"
    Public Shared ReadOnly Property maxTransitionalPosition() As Point
        Get
            Return New Point(CInt((Game.getInstance().Width / 4) * 3), CInt((Game.getInstance().Height / 4) * 3))
        End Get
    End Property
    Public Shared ReadOnly Property minTransitionalPosition() As Point
        Get
            Return New Point(CInt(Game.getInstance().Width / 4), CInt(Game.getInstance().Height / 4))
        End Get
    End Property
    Public Shared ReadOnly Property maxTransitionalRadius() As Double
        Get
            Return Game.getInstance().Height / 3
        End Get
    End Property
#End Region
#Region "Sine Circle Types"
    Private Shared currentSineType As Integer = -1
    Private Shared random As New Random
    Private Shared circletypes As New Dictionary(Of SineCircleType, Action) From {
        {New PiSineType(Sine), AddressOf fastSineCirclesTransition}, _
        {New NormalSineCirclePathType(Sine), AddressOf fastSineCirclesTransition}, _
        {New DoubleSineCirclePathType(Sine), AddressOf slowSineCirclesTransition}, _
        {New DoubleCosCirclePathType(Sine), AddressOf slowSineCirclesTransition} _
    }

    Private Shared Sub slowSineCirclesTransition()
        Radius.Duration = TimeSpan.FromSeconds(10)
        Frequency.Duration = TimeSpan.FromSeconds(10)
        Depth.Duration = TimeSpan.FromSeconds(10)
        Rotation.Duration = TimeSpan.FromSeconds(3)
        Position.Duration = TimeSpan.FromSeconds(4)
    End Sub
    Private Shared Sub fastSineCirclesTransition()
        Radius.Duration = TimeSpan.FromSeconds(1)
        Frequency.Duration = TimeSpan.FromSeconds(1)
        Depth.Duration = TimeSpan.FromSeconds(2)
        Position.Duration = TimeSpan.FromMilliseconds(800)
        Rotation.Duration = TimeSpan.FromSeconds(1)
    End Sub

    Public Shared Sub advanceSineType()
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
    Public Shared Sub init(game As Game)
        game.addTransition(Sine.RotationProperty, Rotation)
        game.addTransition(Sine.PositionProperty, Position)
        game.addTransition(Sine.ColorProperty, SineColorTransition)
        game.addTransition(Sine.FrequencyProperty, Frequency)
        game.addTransition(Sine.DepthProperty, Depth)
        game.addTransition(Sine.RadiusProperty, Radius)
    End Sub
    Public Shared Sub pause(paused As Boolean)
        Rotation.Paused = paused
        Position.Paused = paused
        SineColorTransition.Paused = paused
        Frequency.Paused = paused
        Depth.Paused = paused
        Radius.Paused = paused
    End Sub
#End Region
End Class

Imports MiniGameEngine
Public Class Circle
    Inherits GameObject
    ''' <summary>
    ''' Make a new Circle object with origin and radius
    ''' </summary>
    ''' <param name="position">The center point of the circle</param>
    ''' <param name="radius">The size of the circle (remember:- it's half of the diameter)</param>
    ''' <remarks></remarks>
    Public Sub New(position As Point, radius As Double)
        Me.Position = position
        Me.radius = radius
    End Sub

#Region "Painting"
    Public Property fillColor As Color = Color.Transparent
    Public Property lineColor As Color = Color.Black
    Public Property lineWidth As Integer = 1
#End Region

#Region "Transition Properties"
    Public ReadOnly Property ColorProperty As Game.TransitionProperty(Of Transitions.Transition(Of Color), Color)
        Get
            Return New Game.TransitionProperty(Of Transitions.Transition(Of Color), Color)(AddressOf ColorPropertyWorker)
        End Get
    End Property
    Private Sub ColorPropertyWorker(ByVal Transition As Transitions.Transition(Of Color))
        Me.lineColor = Transition.Value
    End Sub

    Public ReadOnly Property FillProperty As Game.TransitionProperty(Of Transitions.Transition(Of Color), Color)
        Get
            Return New Game.TransitionProperty(Of Transitions.Transition(Of Color), Color)(AddressOf FillPropertyWorker)
        End Get
    End Property
    Private Sub FillPropertyWorker(ByVal Transition As Transitions.Transition(Of Color))
        Me.fillColor = Transition.Value
    End Sub

    Public ReadOnly Property lineWidthProperty As Game.TransitionProperty(Of Transitions.Transition(Of Double), Double)
        Get
            Return New Game.TransitionProperty(Of Transitions.Transition(Of Double), Double)(AddressOf lineWidthPropertyWorker)
        End Get
    End Property
    Private Sub lineWidthPropertyWorker(ByVal Transition As Transitions.Transition(Of Double))
        Me.lineWidth = CInt(Transition.Value)
    End Sub

    Public ReadOnly Property RadiusProperty As Game.TransitionProperty(Of Transitions.Transition(Of Double), Double)
        Get
            Return New Game.TransitionProperty(Of Transitions.Transition(Of Double), Double)(AddressOf RadiusPropertyWorker)
        End Get
    End Property
    Private Sub RadiusPropertyWorker(ByVal Transition As Transitions.Transition(Of Double))
        Me.radius = Transition.Value
    End Sub

#End Region

    ''' <summary>
    ''' Get the circumference of the circle
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property circumference() As Double
        Get
            Return Math.PI * diameter
        End Get
    End Property
    ''' <summary>
    ''' Get the area of the circle
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property area As Double
        Get
            Return Math.PI * (radius * radius)
        End Get
    End Property
    Public Property radius As Double
    ''' <summary>
    ''' Get of set the diameter of the circle
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property diameter As Double
        Get
            Return radius * 2
        End Get
        Set(diameter As Double)
            Me.radius = diameter / 2
        End Set
    End Property
    ''' <summary>
    ''' Return a circle path from the current circle object
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getPath() As Drawing2D.GraphicsPath
        Dim positions As New List(Of PointF)
        For i = 0 To 360
            positions.Add(getPoint(radius, getRadians(i), Me.Position))
        Next
        Dim Path As New Drawing2D.GraphicsPath
        Path.AddCurve(positions.ToArray)
        Return Path
    End Function
    ''' <summary>
    ''' Returns the position (point) of which the point should be determined by the radius, radians (not degrees, you must convert) and the origin.
    ''' </summary>
    ''' <param name="radius"></param>
    ''' <param name="radians"></param>
    ''' <param name="origin"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getPoint(radius As Double, radians As Double, origin As Point) As Point
        Dim x As Double = (radius * Math.Cos(radians)) + origin.X
        Dim y As Double = (radius * Math.Sin(radians)) + origin.Y
        Return New Point(CInt(x), CInt(y))
    End Function
    ''' <summary>
    ''' Converts degrees to radians.
    ''' </summary>
    ''' <param name="degrees"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getRadians(degrees As Double) As Double
        Return degrees * (Math.PI / 180)
    End Function
    ''' <summary>
    ''' Converts radians to degrees
    ''' </summary>
    ''' <param name="radians"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getDegrees(radians As Double) As Double
        Return radians * (180 / Math.PI)
    End Function

    Public Overrides Sub Render(Graphics As Graphics)
        Graphics.DrawClosedCurve(New Pen(lineColor), Me.getPath().PathPoints)
        Graphics.FillClosedCurve(New SolidBrush(fillColor), Me.getPath().PathPoints)
    End Sub

    Public Overrides Sub Update(delta As Double)

    End Sub
End Class
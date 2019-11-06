Imports MiniGameEngine
Imports MiniGameEngine.Transitions
Imports System.Drawing
Namespace Examples.Shapes
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
        Private _ColorProperty As New TransitionProperty(Me, Sub(ByRef Transition As Transitions.Transition)
                                                                 Me.lineColor = CType(Transition.Value, Color)
                                                             End Sub)

        Public ReadOnly Property ColorProperty As TransitionProperty
            Get
                Return _ColorProperty
            End Get
        End Property

        Private _FillProperty As New TransitionProperty(Me, Sub(ByRef Transition As Transitions.Transition)
                                                                Me.fillColor = CType(Transition.Value, Color)
                                                            End Sub)
        Public ReadOnly Property FillProperty As TransitionProperty
            Get
                Return _FillProperty
            End Get
        End Property

        Private _RadiusProperty As New TransitionProperty(Me, Sub(ByRef Transition As Transitions.Transition)
                                                                  Me.radius = CDbl(Transition.Value)
                                                              End Sub)
        Public ReadOnly Property RadiusProperty As TransitionProperty
            Get
                Return _RadiusProperty
            End Get
        End Property

        Private _lineWidthProperty As New TransitionProperty(Me, Sub(ByRef Transition As Transitions.Transition)
                                                                     Me.lineWidth = CInt(Transition.Value)
                                                                 End Sub)
        Public ReadOnly Property lineWidthProperty As TransitionProperty
            Get
                Return _lineWidthProperty
            End Get
        End Property

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
            Graphics.DrawEllipse(New Pen(lineColor, lineWidth), CInt(Position.X - radius), CInt(Position.Y - radius), CInt(diameter), CInt(diameter))
            Graphics.FillEllipse(New SolidBrush(fillColor), CInt(Position.X - radius), CInt(Position.Y - radius), CInt(diameter), CInt(diameter))
            ' Graphics.DrawClosedCurve(New Pen(lineColor), Me.getPath().PathPoints)
            ' Graphics.FillClosedCurve(New SolidBrush(fillColor), Me.getPath().PathPoints)
        End Sub
        Public Function inBoundries(Point As Point) As Boolean
            Dim SquareDistance As Double = (Position.X - Point.X) ^ 2 + (Position.Y - Point.Y) ^ 2
            Return SquareDistance <= (radius ^ 2)
        End Function
        Public Overrides Sub Update(delta As Double)

        End Sub
    End Class
    Public Class Raindrop2D
        Inherits Circle

        Public Sub New(position As Point, radius As Double)
            MyBase.New(position, radius)
        End Sub

        Public Property refractAmount As Integer

        'Public Overrides Sub Render(Graphics As Graphics)
        '    MyBase.Render(Graphics)
        '    For i = 1 To refractAmount
        '        Dim currentDiameter As Double = Math.Max((diameter / refractAmount) * i, 0)
        '        Graphics.DrawEllipse(New Pen(lineColor), CInt(Position.X - radius), CInt(Position.Y - radius), CInt(currentDiameter), CInt(currentDiameter))
        '        Graphics.FillEllipse(New SolidBrush(fillColor), CInt(Position.X - radius), CInt(Position.Y - radius), CInt(currentDiameter), CInt(currentDiameter))
        '    Next
        'End Sub
    End Class
End Namespace
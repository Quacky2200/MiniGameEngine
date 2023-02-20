Imports MiniGameEngine
Imports MiniGameEngine.Transitions
Imports System.Drawing
Namespace Examples.Shapes
    Public Class Circle
        Inherits GameObject
        ''' <summary>
        ''' Make a new Circle object with origin and radius
        ''' </summary>
        ''' <param name="Position">The center point of the circle</param>
        ''' <param name="Radius">The size of the circle (remember:- it's half of the diameter)</param>
        ''' <remarks></remarks>
        Public Sub New(Position As Point, Radius As Double)
            Me.Position = Position
            Me.Radius = Radius
        End Sub

#Region "Painting"
        Public Property FillColor As Color = Color.Transparent
        Public Property LineColor As Color = Color.Black
        Public Property LineWidth As Integer = 1
#End Region

#Region "Transition Properties"
        Private _ColorProperty As New TransitionProperty(Me.ID, Sub(ByRef Transition As Transition)
                                                                    LineColor = CType(Transition.Value, Color)
                                                                End Sub)

        Public ReadOnly Property ColorProperty As TransitionProperty
            Get
                Return _ColorProperty
            End Get
        End Property

        Private _FillProperty As New TransitionProperty(Me.ID, Sub(ByRef Transition As Transition)
                                                                   FillColor = CType(Transition.Value, Color)
                                                               End Sub)
        Public ReadOnly Property FillProperty As TransitionProperty
            Get
                Return _FillProperty
            End Get
        End Property

        Private _RadiusProperty As New TransitionProperty(Me.ID, Sub(ByRef Transition As Transition)
                                                                     Radius = CDbl(Transition.Value)
                                                                 End Sub)
        Public ReadOnly Property RadiusProperty As TransitionProperty
            Get
                Return _RadiusProperty
            End Get
        End Property

        Private _lineWidthProperty As New TransitionProperty(Me.ID, Sub(ByRef Transition As Transition)
                                                                        LineWidth = CInt(Transition.Value)
                                                                    End Sub)
        Public ReadOnly Property LineWidthProperty As TransitionProperty
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
        Public ReadOnly Property Circumference() As Double
            Get
                Return Math.PI * Diameter
            End Get
        End Property
        ''' <summary>
        ''' Get the area of the circle
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Area As Double
            Get
                Return Math.PI * (Radius * Radius)
            End Get
        End Property
        Public Property Radius As Double
        ''' <summary>
        ''' Get of set the diameter of the circle
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Diameter As Double
            Get
                Return Radius * 2
            End Get
            Set(diameter As Double)
                Me.Radius = diameter / 2
            End Set
        End Property
        ''' <summary>
        ''' Returns the position (point) of which the point should be determined by the radius, radians (not degrees, you must convert) and the origin.
        ''' </summary>
        ''' <param name="Radius"></param>
        ''' <param name="Radians"></param>
        ''' <param name="Origin"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetPoint(Radius As Double, Radians As Double, Origin As Point) As Point
            Dim x As Double = (Radius * Math.Cos(Radians)) + Origin.X
            Dim y As Double = (Radius * Math.Sin(Radians)) + Origin.Y
            Return New Point(CInt(x), CInt(y))
        End Function
        ''' <summary>
        ''' Converts degrees to radians.
        ''' </summary>
        ''' <param name="Degrees"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetRadians(Degrees As Double) As Double
            Return Degrees * (Math.PI / 180)
        End Function
        ''' <summary>
        ''' Converts radians to degrees
        ''' </summary>
        ''' <param name="Radians"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDegrees(Radians As Double) As Double
            Return Radians * (180 / Math.PI)
        End Function

        Public Overrides Sub Render(Graphics As Graphics)
            MyBase.Render(Graphics)
            Graphics.DrawEllipse(New Pen(LineColor), CInt(Position.X - Radius), CInt(Position.Y - Radius), CInt(Diameter), CInt(Diameter))
            Graphics.FillEllipse(New SolidBrush(FillColor), CInt(Position.X - Radius), CInt(Position.Y - Radius), CInt(Diameter), CInt(Diameter))
            ' Graphics.DrawClosedCurve(New Pen(lineColor), Me.getPath().PathPoints)
            ' Graphics.FillClosedCurve(New SolidBrush(fillColor), Me.getPath().PathPoints)
        End Sub

        Public Function InBoundries(Point As Point) As Boolean
            Dim SquareDistance As Double = (Position.X - Point.X) ^ 2 + (Position.Y - Point.Y) ^ 2
            Return SquareDistance <= (Radius ^ 2)
        End Function
    End Class
End Namespace
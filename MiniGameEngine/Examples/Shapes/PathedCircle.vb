Imports MiniGameEngine
Imports MiniGameEngine.Transitions
Imports System.Drawing
Namespace Examples.Shapes
    Public Class PathedCircle
        Inherits GameObject
        ''' <summary>
        ''' Make a new Circle object with origin and radius
        ''' </summary>
        ''' <param name="position">The center point of the circle</param>
        ''' <param name="radius">The size of the circle (remember:- it's half of the diameter)</param>
        ''' <remarks></remarks>
        Public Sub New(position As Point, radius As Double)
            Me.Position = position
            Me.Radius = radius
        End Sub

#Region "Painting"
        Public Property FillColor As Color = Color.Transparent
        Public Property LineColor As Color = Color.Black
        Public Property LineWidth As Integer = 1
#End Region

#Region "Transition Properties"
        Public ReadOnly ColorProperty As New TransitionProperty(AddressOf OnColorPropertyUpdated)
        Private Sub OnColorPropertyUpdated(ByRef Sender As Object, ByRef Value As Object)
            Me.LineColor = CType(Value, Color)
        End Sub

        Public ReadOnly FillProperty As New TransitionProperty(AddressOf OnFillPropertyUpdated)
        Private Sub OnFillPropertyUpdated(ByRef Sender As Object, ByRef Value As Object)
            Me.FillColor = CType(Value, Color)
        End Sub

        Public ReadOnly RadiusProperty As New TransitionProperty(AddressOf OnRadiusPropertyUpdated)
        Private Sub OnRadiusPropertyUpdated(ByRef Sender As Object, ByRef Value As Object)
            Me.Radius = CDbl(Value)
        End Sub

        Public ReadOnly LineWidthProperty As New TransitionProperty(AddressOf OnlineWidthPropertyUpdated)
        Private Sub OnlineWidthPropertyUpdated(ByRef Sender As Object, ByRef Value As Object)
            Me.LineWidth = CInt(Value)
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
        ''' Return a circle path from the current circle object
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPath() As Drawing2D.GraphicsPath
            Dim positions As New List(Of PointF)
            For i = 0 To 360 Step 360
                positions.Add(GetPoint(Radius, GetRadians(i), Me.Position))
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
        Public Shared Function GetPoint(radius As Double, radians As Double, origin As Point) As Point
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
        Public Shared Function GetRadians(degrees As Double) As Double
            Return degrees * (Math.PI / 180)
        End Function
        ''' <summary>
        ''' Converts radians to degrees
        ''' </summary>
        ''' <param name="radians"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDegrees(radians As Double) As Double
            Return radians * (180 / Math.PI)
        End Function

        Public Overrides Sub Render(Graphics As Graphics)
            Graphics.DrawClosedCurve(New Pen(LineColor), Me.GetPath().PathPoints)
            Graphics.FillClosedCurve(New SolidBrush(FillColor), Me.GetPath().PathPoints)
        End Sub
        Public Function WithinBoundries(Point As Point) As Boolean
            Dim SquareDistance As Double = (Position.X - Point.X) ^ 2 + (Position.Y - Point.Y) ^ 2
            Return SquareDistance <= (Radius ^ 2)
        End Function
        'Public Overrides Sub Update(delta As Double)
        '    MyBase.Update(delta)
        'End Sub
    End Class
End Namespace
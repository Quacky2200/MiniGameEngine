﻿Imports MiniGameEngine
Imports MiniGameEngine.Transitions
Imports System.Drawing
Imports MiniGameEngine.Examples.Shapes.SineCircles

Namespace Examples.Shapes
    Public Class SineCircle
        Inherits Circle

        Public Property ShowMidPoint As Boolean = False

        Public Sub New(Position As Point, Radius As Double, Frequency As Integer, Depth As Integer)
            MyBase.New(Position, Radius)
            Me.radius = CInt(Radius)
            Me.Position = Position
            _frequency = Frequency
            _depth = Depth
        End Sub

#Region "Transition Properties"
        Public ReadOnly FrequencyProperty As New TransitionProperty(AddressOf OnFrequencyPropertyUpdated)
        Private Sub OnFrequencyPropertyUpdated(ByRef Sender As Object, ByRef Value As Object)
            Me.frequency = CInt(Value)
        End Sub

        Public ReadOnly DepthProperty As New TransitionProperty(AddressOf OnDepthPropertyUpdated)
        Private Sub OnDepthPropertyUpdated(ByRef Sender As Object, ByRef Value As Object)
            Me.depth = CInt(Value)
        End Sub

#End Region

#Region "General Properties"

        Public Overloads Function ToString() As String
            Return MyBase.ToString() + " " + GetStatistics()
        End Function

        Public Function GetStatistics() As String
            Return String.Format(
                "Position:{0},Rotation:{1},Radius:{2},Frequency:{3},Depth:{4},Type:{5}",
                Position, Rotation.ToString("000.00"), Radius.ToString("000.00"), frequency, depth, type.GetType.Name
            )
        End Function
#End Region

#Region "Frequency & Depth"
        Private Property _frequency As Integer

        Public Property frequency As Integer
            Get
                Return _frequency
            End Get
            Set(value As Integer)
                If _frequency <> value Then
                    _frequency = value
                End If
            End Set
        End Property

        Private Property _depth As Integer

        Public Property depth As Integer
            Get
                Return _depth
            End Get
            Set(value As Integer)
                If _depth <> value Then
                    _depth = value
                End If
            End Set
        End Property
#End Region

#Region "Type Generation"

        Public Property maxDegrees As Integer = 360

        Private Property _type As SineCircleType = New NormalSineCirclePathType(Me)

        Public Property type As SineCircleType
            Get
                Return _type
            End Get
            Set(value As SineCircleType)
                _type = value
            End Set
        End Property

        Public Property Closed As Boolean = True
        Public Overrides Sub Render(Graphics As Graphics)
            If ShowMidPoint Then
                MyBase.Render(Graphics)
            End If

            If Closed Then
                Graphics.DrawClosedCurve(New Pen(LineColor, LineWidth), Me.getPath())
                Graphics.FillClosedCurve(New SolidBrush(FillColor), Me.getPath())
            Else
                Graphics.DrawCurve(New Pen(LineColor, LineWidth), Me.getPath())
            End If
        End Sub
        Public Overloads Function getPath() As PointF()
            Dim positions As New List(Of Point)
            For i = 0 To maxDegrees
                positions.Add(type.getPath(i))
            Next
            Dim gpath As New Drawing2D.GraphicsPath
            gpath.AddCurve(positions.ToArray)
            Return gpath.PathPoints
        End Function
#End Region

    End Class
End Namespace
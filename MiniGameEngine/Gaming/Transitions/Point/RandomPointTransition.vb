Imports System.Drawing
Namespace Transitions
    Public Class RandomPointTransition
        Inherits PointTransition
        Public Property minPosition As Point
        Public Property maxPosition As Point
        Private random As New Random((New Random).Next(0, Integer.MaxValue))
        Public Sub New(minPosition As Point, maxPosition As Point)
            MyBase.New(minPosition, maxPosition)
            Me.minPosition = minPosition
            Me.maxPosition = maxPosition
        End Sub
        Public Sub New(startPosition As Point, minPosition As Point, maxPosition As Point)
            MyBase.New(startPosition, minPosition, maxPosition)
            Me.minPosition = minPosition
            Me.maxPosition = maxPosition
        End Sub
        Private Sub randomise(sender As Object) Handles Me.OnRepeat
            Me.A = Me.B
            Me.B = New Point(random.Next(minPosition.X, maxPosition.X), random.Next(minPosition.Y, maxPosition.Y))
        End Sub
        Private Sub Reversed(oldDirection As TransitionDirection, newDirection As TransitionDirection) Handles Me.OnReverse
            If oldDirection = TransitionDirection.Forward Then
                Me.B = New Point(random.Next(minPosition.X, maxPosition.X), random.Next(minPosition.Y, maxPosition.Y))
            Else
                Me.A = New Point(random.Next(minPosition.X, maxPosition.X), random.Next(minPosition.Y, maxPosition.Y))
            End If
        End Sub

    End Class
End Namespace
'Public Class Validate
'    Public Shared Function Number(Of T As {IConvertible, IComparable, IComparable(Of T), 
'                             IFormattable, IEquatable(Of T)})(minValue As T, maxValue As T, currentValue As T, Optional Modulus As Boolean = False) As Decimal
'        If currentValue.ToDecimal(Nothing) < minValue.ToDecimal(Nothing) Then
'            Return If(Modulus, (Math.Abs(currentValue.ToDecimal(Nothing)) Mod maxValue.ToDecimal(Nothing)) + minValue.ToDecimal(Nothing), minValue.ToDecimal(Nothing))
'        ElseIf currentValue.ToDecimal(Nothing) > maxValue.ToDecimal(Nothing) Then
'            Return If(Modulus, (Math.Abs(currentValue.ToDecimal(Nothing)) Mod maxValue.ToDecimal(Nothing)) + minValue.ToDecimal(Nothing), maxValue.ToDecimal(Nothing))
'        Else
'            Return currentValue.ToDecimal(Nothing)
'        End If
'    End Function
'End Class
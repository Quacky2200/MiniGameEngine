Imports System.Drawing
Namespace Transitions
    Public Class RandomPointTransition
        Inherits PointTransition
        Public Property MinPosition As Point
        Public Property MaxPosition As Point

        Private ReadOnly Random As New Random((New Random).Next(0, Integer.MaxValue))

        Public Sub New(MinPosition As Point, MaxPosition As Point)
            MyBase.New(MinPosition, MaxPosition)
            Me.MinPosition = MinPosition
            Me.MaxPosition = MaxPosition
        End Sub

        Public Sub New(StartPosition As Point, MinPosition As Point, MaxPosition As Point)
            MyBase.New(StartPosition, StartPosition)
            Me.Value = StartPosition
            Me.MinPosition = MinPosition
            Me.MaxPosition = MaxPosition
            Randomise()
        End Sub

        Private Sub Randomise()
            Me.StartValue = If(Me.Reverse, Me.StartValue, Me.EndValue)
            Me.EndValue = New Point(Random.Next(MinPosition.X, MaxPosition.X), Random.Next(MinPosition.Y, MaxPosition.Y))
        End Sub

        Private Sub RandomPointTransition_OnRepeat(Sender As Object) Handles Me.OnRepeat
            Randomise()
        End Sub

        Private Sub RandomPointTransition_OnReverse(OldDirection As TransitionDirection, NewDirection As TransitionDirection) Handles Me.OnReverse
            If (NewDirection = TransitionDirection.Forward) Then Randomise()
        End Sub

    End Class
End Namespace
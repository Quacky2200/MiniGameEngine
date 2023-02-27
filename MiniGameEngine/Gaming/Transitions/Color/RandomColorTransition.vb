Imports System.Drawing
Imports MiniGameEngine.General.Color.RGBHSL

Namespace Transitions
    Public Class RandomColorTransition
        Inherits ColorTransition

        Public ReadOnly HSL As New HSL()
        Private ReadOnly Random As New Random()

        Public Sub New(Optional StartHue As Double = 0, Optional Saturation As Double = 1.0, Optional Lightness As Double = 0.5)
            MyBase.New()

            HSL.H = Clamp(StartHue, 0, 1)
            HSL.S = Clamp(Saturation, 0, 1)
            HSL.L = Clamp(Lightness, 0, 1)

            Randomise()

            Me.StartValue = HSL_to_Color(HSL)
        End Sub

        Private Sub Randomise()
            Me.StartValue = If(Me.Reverse, Me.StartValue, Me.EndValue)

            HSL.H = Random.NextDouble()

            Me.EndValue = HSL_to_Color(HSL)
        End Sub

        Private Sub RandomColorTransition_OnRepeat(sender As Object) Handles Me.OnRepeat
            Randomise()
        End Sub

        Private Sub RandomColorTransition_OnReverse(OldDirection As TransitionDirection, NewDirection As TransitionDirection) Handles Me.OnReverse
            If (NewDirection = TransitionDirection.Forward) Then Randomise()
        End Sub
    End Class
End Namespace
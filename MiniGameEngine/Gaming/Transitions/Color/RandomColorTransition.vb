Imports System.Drawing
Namespace Transitions
    Public Class RandomColorTransition
        Inherits ColorTransition
        Public Sub New()
            MyBase.New(Color.Red, Color.Blue)
        End Sub
        Private Sub Cycled(sender As Object) Handles MyBase.OnRepeat
            Me.A = Me.B
            Me.B = Color.AliceBlue
        End Sub
        Private Sub Reversed(oldDirection As TransitionDirection, newDirection As TransitionDirection) Handles Me.OnReverse
            If Direction = TransitionDirection.Forward Then
                Me.A = Color.AliceBlue
            Else
                Me.B = Color.AliceBlue
            End If

        End Sub
        'Private Sub Reversed(sender As Object, d As TransitionDirection) Handles MyBase.OnReverse
        '    If Direction = TransitionDirection.Forward Then
        '        Me.A = Color.AliceBlue
        '    Else
        '        Me.B = Color.AliceBlue
        '    End If
        'End Sub
    End Class
End Namespace
'Public Class HSLColorRange
'    Public Property hueBasePosition As Double = 0
'    Public Property hueRange As Double = 360

'    Public Property saturationBasePosition As Double = 0
'    Public Property saturationRange As Double = 100

'    Public Property lightnessBasePosition As Double = 0
'    Public Property lightnessRange As Integer = 100

'    Public random As New Random

'    Public Sub New()
'    End Sub
'    Public Sub New(hueBasePosition As Double, hueRange As Double, saturationBasePosition As Double, saturationRange As Double, lightnessBasePosition As Double, lightnessRange As Double)

'    End Sub
'    Public Function generate() As Color
'        Dim hue As Double = Math.Abs(hueBasePosition + If(hueRange < 0, random.Next(CInt(hueRange), 0), random.Next(0, CInt(Math.Abs(hueRange))))) Mod 360
'        Dim saturation As Double = random.Next()
'    End Function
'End Class
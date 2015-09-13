﻿Imports System.Drawing
Public Class ColorTools
    Public Shared Function RGBEquals(Color1 As Color, Color2 As Color) As Boolean
        Return Color1.A = Color2.A AndAlso Color1.R = Color2.R AndAlso Color1.G = Color2.G AndAlso Color1.B = Color2.B
    End Function
    'Thanks to: 
    Public Shared Function ColorToArray(color As Color) As Double()
        Return {color.A, color.R, color.G, color.B}
    End Function
    Public Shared Function ColorFromArray(color As Double()) As Color
        Dim colors(3) As Byte
        For i = 0 To colors.Length - 1
            colors(i) = CByte(Math.Min(255, Math.Max(0, color(i))))
        Next
        Return System.Drawing.Color.FromArgb(colors(0), colors(1), colors(2), colors(3))
    End Function
End Class

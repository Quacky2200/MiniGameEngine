Namespace General.Color
    Public Class ColorTools
        Public Shared Function RGBEquals(Color1 As System.Drawing.Color, Color2 As System.Drawing.Color) As Boolean
            Return Color1.A = Color2.A AndAlso Color1.R = Color2.R AndAlso Color1.G = Color2.G AndAlso Color1.B = Color2.B
        End Function
        Public Shared Function ColorToArray(color As System.Drawing.Color) As Double()
            Return {color.A, color.R, color.G, color.B}
        End Function
        Public Shared Function ColorFromArray(color As Double()) As System.Drawing.Color
            Dim colors(3) As Byte
            For i = 0 To colors.Length - 1
                colors(i) = CByte(Math.Min(255, Math.Max(0, color(i))))
            Next
            Return System.Drawing.Color.FromArgb(colors(0), colors(1), colors(2), colors(3))
        End Function

        Public Shared Function ModifyAlpha(C As System.Drawing.Color, Alpha As Byte)
            Return System.Drawing.Color.FromArgb(Alpha, C)
        End Function

        Public Shared Function ModifyRed(C As System.Drawing.Color, Red As Byte)
            Return System.Drawing.Color.FromArgb(C.A, Red, C.G, C.B)
        End Function

        Public Shared Function ModifyGreen(C As System.Drawing.Color, Green As Byte)
            Return System.Drawing.Color.FromArgb(C.A, C.R, Green, C.B)
        End Function

        Public Shared Function ModifyBlue(C As System.Drawing.Color, Blue As Byte)
            Return System.Drawing.Color.FromArgb(C.A, C.R, C.G, Blue)
        End Function
    End Class
End Namespace

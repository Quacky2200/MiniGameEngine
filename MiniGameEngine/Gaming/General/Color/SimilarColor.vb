Imports System.Drawing
Namespace General.Color
    Public Class SimilarColor
        Private color As RandomColor
        ''' <summary>
        ''' Make a random color based on the color provided and the depth involved
        ''' </summary>
        ''' <param name="similarColor">The color to be based of</param>
        ''' <remarks></remarks>
        Public Sub New(similarColor As Drawing.Color, Depth As Double)
            Dim minHue As Double = Math.Abs(similarColor.GetHue - Math.Abs(Depth)) Mod 360
            Dim maxHue As Double = Math.Abs(similarColor.GetHue + Math.Abs(Depth)) Mod 360
            'Debug.WriteLine("Min: {0} Max:{1} 0-1:{2:0.000},{3:0.000}", minHue, maxHue, minHue / 360, maxHue / 360)
            Dim HSLColorMinimum As Drawing.Color = RGBHSL.ModifyHue(similarColor, minHue / 360)
            Dim HSLColorMaximum As Drawing.Color = RGBHSL.ModifyHue(similarColor, maxHue / 360)
            color = New RandomColor(HSLColorMinimum.R, HSLColorMaximum.R, HSLColorMinimum.G, HSLColorMaximum.G, HSLColorMinimum.B, HSLColorMaximum.B, HSLColorMinimum.A, HSLColorMaximum.A)
        End Sub
        Public Function [Next]() As Drawing.Color
            Return color.Next()
        End Function
    End Class
End Namespace
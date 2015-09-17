Imports System.Drawing
Namespace General.Color
    Public Class RandomColor
        Private random As New Random()
        Public Property minR As Byte
        Public Property maxR As Byte
        Public Property minG As Byte
        Public Property maxG As Byte
        Public Property minB As Byte
        Public Property maxB As Byte
        Public Property minA As Byte
        Public Property maxA As Byte
        ''' <summary>
        ''' Make a RandomColor object using minimum and maximum values. 
        ''' </summary>
        ''' <param name="minR">The minimum the R component will be</param>
        ''' <param name="maxR">The maximum the R component will be</param>
        ''' <param name="minG">The minimum the G component will be</param>
        ''' <param name="maxG">The maximum the G component will be</param>
        ''' <param name="minB">The minimum the B component will be</param>
        ''' <param name="maxB">The maximum the B component will be</param>
        ''' <remarks></remarks>
        Public Sub New(minR As Byte, maxR As Byte, minG As Byte, maxG As Byte, minB As Byte, maxB As Byte, minA As Byte, maxA As Byte)
            Me.minR = minR
            Me.maxR = maxR
            Me.minG = minG
            Me.maxG = maxG
            Me.minB = minB
            Me.maxB = maxB
            Me.minA = minA
            Me.maxA = maxA
        End Sub

        Public Function [Next]() As System.Drawing.Color
            Return System.Drawing.Color.FromArgb( _
                If(minA > maxA, Me.random.Next(Me.maxA, Me.minA), Me.random.Next(Me.minA, Me.maxA)), _
                If(minR > maxR, Me.random.Next(Me.maxR, Me.minR), Me.random.Next(Me.minR, Me.maxR)), _
                If(minG > maxG, Me.random.Next(Me.maxG, Me.minG), Me.random.Next(Me.minG, Me.maxG)), _
                If(minB > maxB, Me.random.Next(Me.maxB, Me.minB), Me.random.Next(Me.minB, Me.maxB)) _
            )
        End Function
    End Class
End Namespace
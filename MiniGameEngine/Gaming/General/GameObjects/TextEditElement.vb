'Public Class TextEditElement
'    Inherits TextElement
'    Public Const CURSOR As String = "|"

'    Public Sub New(Text As String)
'        MyBase.New(Text)
'        Me.Text = Text
'    End Sub
'    Private Property cursorTransition As New Transitions.ColorTransition(Color.Transparent, Color.White) With {.Duration}
'    Public Overrides Sub Render(Graphics As Graphics)
'        'Get all text into an array seperated by newlines
'        Dim allText As String() = Me.Text.Split(CChar(Environment.NewLine))
'        'Sum of the texts' height in this variable
'        Dim height As Integer = 0
'        'Get whom has the widest width
'        Dim width As Integer = 0
'        'Get all the sizes and sum the height whilst getting the biggest width
'        For Each line In allText
'            Dim tempLineSize As Size = Graphics.MeasureString(line, Me.Font).ToSize
'            height += tempLineSize.Height
'            If tempLineSize.Width > width Then width = tempLineSize.Width
'        Next
'        'Store the size in Demensions
'        Dim Dimensions As New Size(width, height)
'        'This is the offset we use for our text
'        Dim horizontalOffset = {0, -0.5, 1}
'        Dim verticalOffset = {0, -0.5, 1}
'        'Let's make a new position with the offset
'        Dim TextPosition As Point = Me.Position
'        TextPosition.Offset(CInt(Dimensions.Width * horizontalOffset(Me.HorizontalAlignment)), CInt(Dimensions.Height * verticalOffset(Me.VerticalAlignment)))

'        Graphics.DrawRectangle(Border, New Rectangle(TextPosition, Dimensions))
'        Graphics.FillRectangle(New SolidBrush(Background), New Rectangle(TextPosition, Dimensions))

'        For Each line In allText
'            Graphics.DrawString(line, Me.Font, New SolidBrush(Me.Color), TextPosition)
'            ' Dim test As Integer = CInt(Graphics.MeasureString(line, Me.Font).Height)
'            'TextPosition.Offset(0, test)
'        Next
'        Graphics.DrawString(CURSOR, Me.Font, New SolidBrush(Me.Color), TextPosition)
'    End Sub
'    Public Overrides Sub Update(delta As Double)

'    End Sub
'End Class

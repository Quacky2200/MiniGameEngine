Imports System.Drawing
Public Class TextElement
    Inherits GameObject
    Public Property Text As String = ""
    Public Property HorizontalAlignment As HorizontalAlignment = HorizontalAlignment.Left
    Public Property VerticalAlignment As VerticalAlignment = VerticalAlignment.Top
    Public Property Font As Font = New System.Drawing.Font("consolas", 15)
    Public Property Color As Color = Color.Black
    Public Property Background As Color = Color.Transparent
    Public Property Border As New Pen(New SolidBrush(Color.Transparent), 1)

#Region "Transition Properties"
    Public ReadOnly Property ColorProperty As Game.TransitionProperty(Of Transitions.Transition(Of Color), Color)
        Get
            Return New Game.TransitionProperty(Of Transitions.Transition(Of Color), Color)(AddressOf ColorPropertyWorker)
        End Get
    End Property
    Private Sub ColorPropertyWorker(ByVal Transition As Transitions.Transition(Of Color))
        Me.Color = Transition.Value
    End Sub

    Public ReadOnly Property BackgroundProperty As Game.TransitionProperty(Of Transitions.Transition(Of Color), Color)
        Get
            Return New Game.TransitionProperty(Of Transitions.Transition(Of Color), Color)(AddressOf BackgroundPropertyWorker)
        End Get
    End Property
    Private Sub BackgroundPropertyWorker(ByVal Transition As Transitions.Transition(Of Color))
        Me.Background = Transition.Value
    End Sub

    Public ReadOnly Property BorderColorProperty As Game.TransitionProperty(Of Transitions.Transition(Of Color), Color)
        Get
            Return New Game.TransitionProperty(Of Transitions.Transition(Of Color), Color)(AddressOf BorderColorPropertyWorker)
        End Get
    End Property
    Private Sub BorderColorPropertyWorker(ByVal Transition As Transitions.Transition(Of Color))
        Me.Border.Color = Transition.Value
    End Sub

#End Region

    Public Sub New(Text As String)
        Me.Text = Text
    End Sub
    Public Overrides Sub Render(Graphics As Graphics)
        'Get all text into an array seperated by newlines
        Dim allText As String() = Me.Text.Split(CChar(Environment.NewLine))
        'Sum of the texts' height in this variable
        Dim height As Integer = 0
        'Get whom has the widest width
        Dim width As Integer = 0
        'Get all the sizes and sum the height whilst getting the biggest width
        For Each line In allText
            Dim tempLineSize As Size = Graphics.MeasureString(line, Me.Font).ToSize
            height += tempLineSize.Height
            If tempLineSize.Width > width Then width = tempLineSize.Width
        Next
        'Store the size in Demensions
        Dim Dimensions As New Size(width, height)
        'This is the offset we use for our text
        Dim horizontalOffset = {0, -0.5, 1}
        Dim verticalOffset = {0, -0.5, 1}
        'Let's make a new position with the offset
        Dim TextPosition As Point = Me.Position
        TextPosition.Offset(CInt(Dimensions.Width * horizontalOffset(Me.HorizontalAlignment)), CInt(Dimensions.Height * verticalOffset(Me.VerticalAlignment)))

        Graphics.DrawRectangle(Border, New Rectangle(TextPosition, Dimensions))
        Graphics.FillRectangle(New SolidBrush(Background), New Rectangle(TextPosition, Dimensions))

        For Each line In allText
            Graphics.DrawString(line, Me.Font, New SolidBrush(Me.Color), TextPosition)
            ' Dim test As Integer = CInt(Graphics.MeasureString(line, Me.Font).Height)
            'TextPosition.Offset(0, test)
        Next
    End Sub

    Public Overrides Sub Update(delta As Double)

    End Sub
End Class
Public Enum HorizontalAlignment
    Left = 0
    Center = 1
    Right = 2
End Enum
Public Enum VerticalAlignment
    Top = 0
    Center = 1
    Bottom = 2
End Enum

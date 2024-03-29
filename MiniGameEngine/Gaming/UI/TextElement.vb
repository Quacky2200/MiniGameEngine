﻿Imports System.Drawing
Imports MiniGameEngine.Transitions
Namespace UI
    Public Class TextElement
        Inherits GameObject
        Public Property Text As String = ""
        Public Property HorizontalAlignment As HorizontalAlignment = HorizontalAlignment.Left
        Public Property VerticalAlignment As VerticalAlignment = VerticalAlignment.Top
        Public Property Font As Font = New System.Drawing.Font("consolas", 15)
        Public Property Color As Color = Color.Black
        Public Property Background As Color = Color.Transparent
        Public Property BorderColor As Color = Color.Transparent
        Public Property BorderWidth As Integer = 0

#Region "Transition Properties"
        Public ReadOnly ColorProperty As New TransitionProperty(AddressOf OnColorPropertyUpdated)
        Private Sub OnColorPropertyUpdated(ByRef Sender As Object, ByRef Value As Color)
            Me.Color = Value
        End Sub

        Public ReadOnly BackgroundProperty As New TransitionProperty(AddressOf OnBackgroundPropertyUpdated)
        Private Sub OnBackgroundPropertyUpdated(ByRef Sender As Object, ByRef Value As Color)
            Me.Background = Value
        End Sub

        Public ReadOnly BorderColorProperty As New TransitionProperty(AddressOf OnBorderColorPropertyUpdated)
        Private Sub OnBorderColorPropertyUpdated(ByRef Sender As Object, ByRef Value As Color)
            Me.BorderColor = Value
        End Sub

        Public ReadOnly BorderWidthProperty As New TransitionProperty(AddressOf OnBorderWidthPropertyUpdated)
        Private Sub OnBorderWidthPropertyUpdated(ByRef Sender As Object, ByRef Value As Integer)
            Me.BorderWidth = Value
        End Sub

#End Region

        Public Sub New(Optional Text As String = "")
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
            Dim horizontalOffset = {0, -0.5, -1}
            Dim verticalOffset = {0, -0.5, -1}
            'Let's make a new position with the offset
            Dim TextPosition As Point = Me.Position
            TextPosition.Offset(CInt(Dimensions.Width * horizontalOffset(Me.HorizontalAlignment)), CInt(Dimensions.Height * verticalOffset(Me.VerticalAlignment)))

            Graphics.DrawRectangle(New Pen(BorderColor, BorderWidth), New Rectangle(TextPosition, Dimensions))
            Graphics.FillRectangle(New SolidBrush(Background), New Rectangle(TextPosition, Dimensions))

            For Each line In allText
                Graphics.DrawString(line, Me.Font, New SolidBrush(Me.Color), TextPosition)
                ' Dim test As Integer = CInt(Graphics.MeasureString(line, Me.Font).Height)
                'TextPosition.Offset(0, test)
            Next
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
End Namespace

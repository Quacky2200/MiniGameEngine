Imports System.Drawing

Public Class LazyText
    Public Property Text As String
    Public Property Font As Font
    Public Property Size As Int16
    Public Property Brush As Brush
    Public Property Position As Point

    Public Sub New(Text As String, Font As Font, Brush As Brush, Position As Point)
        Me.Text = Text
        Me.Font = Font
        Me.Size = Size
        Me.Brush = Brush
        Me.Position = Position
    End Sub

    Public Sub DrawWith(Graphics As Graphics)
        Graphics.DrawString(Text, Font, Brush, Position)
    End Sub
End Class
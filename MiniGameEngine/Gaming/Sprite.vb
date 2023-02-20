Public Class Sprite
    Inherits GameObject

    'Public Property Image As Bitmap
    'Public Property Position As Point
    'Public Property Size As Size
    '    Get
    '        Return Image.Size
    '    End Get
    '    Set(value As Size)
    '        Image = New Bitmap(value.Width, value.Height)
    '        ' Render(Graphics.FromImage(Image))
    '    End Set
    'End Property

    'Public Sub New(position As Point, size As Size)
    '    Me.Position = position
    '    Me.Size = size
    'End Sub

    'Public ReadOnly Iterator Property getBoundsInner() As IEnumerable(Of Point)
    '    Get
    '        For x = 0 To _Image.Size.Width - 1
    '            For y = 0 To _Image.Size.Height - 1
    '                If _Image.GetPixel(x, y) <> Color.Transparent Then
    '                    Yield New Point(x, y)
    '                End If
    '            Next
    '        Next
    '    End Get
    'End Property

    'Public ReadOnly Iterator Property getBoundsOuter() As IEnumerable(Of Point)
    '    Get
    '        For x = 0 To _Image.Size.Width - 1
    '            For y = 0 To _Image.Size.Height - 1
    '                If _Image.GetPixel(x, y) = Color.Transparent Then
    '                    Yield New Point(x, y)
    '                End If
    '            Next
    '        Next
    '    End Get
    'End Property

    'Public ReadOnly Property isPointIntersecting(Point As Point) As Boolean
    '    Get
    '        Return Me.getBoundsInner().ToList().IndexOf(Point) >= 0
    '    End Get
    'End Property

End Class

Imports System.Drawing
Imports MiniGameEngine.Transitions

Public MustInherit Class GameObject
    'Private Property Buffer As BufferedGraphics
    'Private Property Game As Game
    Public ReadOnly ID As String = Guid.NewGuid.ToString
    Public Property Position As New Point(0, 0)
    Private Property _Rotation As Integer = 0

    Public Property Rotation As Integer
        Get
            Return _Rotation
        End Get
        Set(value As Integer)
            _Rotation = Math.Min(360, Math.Max(0, value))
        End Set
    End Property
    Private _PositionProperty As New TransitionProperty(Me.ID, Sub(ByRef Transition As Transition)
                                                                   Me.Position = Transition.Value
                                                               End Sub)
    Public ReadOnly Property PositionProperty As TransitionProperty
        Get
            Return _PositionProperty
        End Get
    End Property

    Private _RotationProperty As New TransitionProperty(Me.ID, Sub(ByRef Transition As Transition)
                                                                   Me.Rotation = Transition.Value
                                                               End Sub)
    Public ReadOnly Property RotationProperty As TransitionProperty
        Get
            Return _RotationProperty
        End Get
    End Property
   
    Public Property zIndex As Integer = 0
    Public Property Visible As Boolean = True
    Public Sub Show()
        Visible = True
    End Sub
    Public Sub Hide()
        Visible = False
    End Sub
    Public Sub New()

    End Sub

    Public Sub sendToTop()
        'TODO:
        'Push all Gameobject zIndexes down and set this one to the last
        Throw New NotImplementedException()
        'zIndex As Integer = Game.getInstance().allGameObjects.Length
    End Sub
    Public Sub sendToBottom()
        'TODO: 
        'Push all gameObject zindexes up and set this to 0
        Throw New NotImplementedException()
    End Sub


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
    Public MustOverride Sub Render(Graphics As Graphics)
    Public MustOverride Sub Update(delta As Double)

End Class

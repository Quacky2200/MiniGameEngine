Public Class IDObject
    Private _ID As String = Guid.NewGuid().ToString()

    Public Property ID As String
        Get
            Return _ID
        End Get
        Protected Set(value As String)
            _ID = value
        End Set
    End Property
End Class

Public Class Component
    Inherits IDObject

    ' This is a weak imitation of Unity's MonoBehaviour component system.

    Private _Enabled As Boolean = True
    Public Property Enabled As Boolean
        Get
            Return _Enabled
        End Get
        Set(value As Boolean)
            If (_Enabled = value) Then Return

            _Enabled = value
            If value Then
                OnEnable()
            Else
                OnDisable()
            End If
        End Set
    End Property

    Public Overridable Sub Update(Delta As Double)

    End Sub

    Public Overridable Sub Begin()

    End Sub

    Public Overridable Sub OnDisable()

    End Sub
    Public Overridable Sub OnEnable()

    End Sub
End Class

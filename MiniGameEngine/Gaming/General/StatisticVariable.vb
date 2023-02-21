Public Class StatisticVariable(Of T)
    Public Refresher As Func(Of T)
    Public Timeout As TimeSpan

    Private LastAccess As Long
    Private LastValue As T

    Public Sub New(Refresher As Func(Of T), Timeout As TimeSpan)
        Me.Refresher = Refresher
        Me.Timeout = Timeout
        LastAccess = 0
    End Sub

    Public ReadOnly Property Value As T
        Get
            If TimeSpan.FromTicks(Now.Ticks - LastAccess) > Timeout Then
                LastValue = Refresher()
                LastAccess = Now.Ticks
            End If

            Return LastValue
        End Get
    End Property
End Class

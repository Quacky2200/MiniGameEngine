Public Class StatisticVariableAvg
    Public Refresher As Func(Of Double)
    Private Statistic As Double = 0
    Private Idx As Integer = 0
    Private Count As Integer = 0
    Private LastValue As Double = 0

    Public Sub New(Refresher As Func(Of Double), Count As Integer)
        Me.Refresher = Refresher
        Me.Count = Count
        Me.Idx = 0
    End Sub

    Public ReadOnly Property Value As Double
        Get
            Statistic += Refresher()
            Idx += 1

            If (Idx >= Count) Then
                LastValue = Statistic / Count
                Statistic = 0
                Idx = 0
            End If

            Return LastValue
        End Get
    End Property
End Class

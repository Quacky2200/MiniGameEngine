Public Class Watch
    Private dateOne, dateTwo As DateTime
    Public Sub New(dateOne As DateTime, dateTwo As DateTime)
        Me.dateOne = dateOne
        Me.dateTwo = dateTwo
    End Sub
    Public ReadOnly Property Milliseconds() As Double
        Get
            Return Math.Abs(Me.dateOne.Ticks - Me.dateTwo.Ticks) / 10000
        End Get
    End Property
    Public ReadOnly Property Seconds() As Double
        Get
            Return Milliseconds / 1000
        End Get
    End Property
    Public ReadOnly Property Minutes() As Double
        Get
            Return Seconds / 60
        End Get
    End Property
    Public ReadOnly Property Hours() As Double
        Get
            Return Minutes / 60
        End Get
    End Property
    Public Shared Function getNow() As DateTime
        Return New DateTime(Now.Ticks)
    End Function
    Public Shared Function getLiveTicks() As Long
        Return Now.Ticks
    End Function
    Public Shared Function getLiveMilliseconds() As Long
        Return CLng(Now.Ticks / TimeSpan.TicksPerMillisecond)
    End Function
End Class

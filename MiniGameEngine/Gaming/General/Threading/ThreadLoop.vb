Imports System.Threading
Namespace General.Threading
    Public Class ThreadLoop
        Private [Loop] As Thread

        Private Property Routine As Work
        Public Delegate Sub Work()
        Private Property _Enabled As Boolean = False

        Public Property Enabled As Boolean
            Get
                Return _Enabled
            End Get
            Set(value As Boolean)
                _Enabled = value
                If _Enabled Then
                    [Loop] = New Thread(AddressOf Me.ThreadLoop)
                    [Loop].Start()
                End If
            End Set
        End Property

        Public Sub Start()
            Enabled = True
        End Sub

        Public Sub [Stop]()
            Enabled = False
        End Sub

        Public Sub New(Routine As Work)
            Me.Routine = Routine
        End Sub

        Public Const MAX_FPS = 60
        Public Sub ThreadLoop()
            Dim s As SpinWait
            While Enabled
                Me.Routine.Invoke()
                'Thread.Sleep(1)
                s.SpinOnce()
            End While
        End Sub
    End Class
End Namespace

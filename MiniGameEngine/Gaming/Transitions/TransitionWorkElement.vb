Namespace Transitions
    Public Class TransitionWorkElement
        Public Property StartPosition As Double()
        Public Property Work As Double()
        Public Property Destination As Double()
        Public Sub New(StartPosition As Double(), Work As Double())
            Me.StartPosition = StartPosition
            Me.Work = Work
        End Sub
    End Class
End Namespace
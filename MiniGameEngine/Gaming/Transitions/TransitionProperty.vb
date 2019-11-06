Namespace Transitions
    Public Class TransitionProperty

        Public ReadOnly ID As String = Guid.NewGuid().ToString
        Public Delegate Sub Worker(ByRef Transition As Transition)
        Private _Work As Worker
        Public Property Reference As GameObject

        Public Sub New(ByRef Reference As GameObject, ByVal Work As Worker)
            Me.Reference = Reference
            Me._Work = Work
        End Sub

        Public Sub Work(ByVal Transition As Transition)
            Me._Work(Transition)
        End Sub
    End Class
End Namespace
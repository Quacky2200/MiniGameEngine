Imports MiniGameEngine.Observers

Namespace Transitions

    Public Class TransitionProperty
        Inherits Subscriber(Of Object)

        Public Delegate Sub PropertyUpdateDelegate(ByRef Sender As Observable(Of Object), ByRef Value As Object)
        Private ReadOnly [Delegate] As PropertyUpdateDelegate

        Public Sub New(ByVal [Delegate] As PropertyUpdateDelegate)
            Me.Delegate = [Delegate]
        End Sub

        Public Overrides Sub Notify(Value As Object)
            [Delegate](Observer, Value)
        End Sub
    End Class
End Namespace
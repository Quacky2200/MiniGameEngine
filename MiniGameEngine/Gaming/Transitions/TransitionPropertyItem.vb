Namespace Transitions
    Public Class TransitionPropertyItem
        Public Property TransitionProperty As TransitionProperty
        Public Property Transition As Transition
        Public Property AutomaticallyRemoveTransition As Boolean
        Public Property AutomaticallyRemoveObject As Boolean
        Public Sub New(TransitionProperty As TransitionProperty, Transition As Transition, Optional AutomaticallyRemoveTransition As Boolean = False, Optional AutomaticallyRemoveObject As Boolean = False)
            Me.TransitionProperty = TransitionProperty
            Me.Transition = Transition
            Me.AutomaticallyRemoveTransition = AutomaticallyRemoveTransition
            Me.AutomaticallyRemoveObject = AutomaticallyRemoveObject
        End Sub
    End Class
End Namespace
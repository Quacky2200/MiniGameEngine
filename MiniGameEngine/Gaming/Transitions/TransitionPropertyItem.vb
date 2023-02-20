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

        Public Sub Work()
            TransitionProperty.Work(Transition)
        End Sub

        Public ReadOnly Property RemoveObjectNow As Boolean
            Get
                Return Transition.isFinished AndAlso AutomaticallyRemoveObject
            End Get
        End Property

        Public ReadOnly Property RemoveTransitionNow As Boolean
            Get
                Return Transition.isFinished AndAlso AutomaticallyRemoveTransition
            End Get
        End Property

        Public Function GetId() As String
            Return String.Format("{0}-{1}", TransitionProperty.ID, Transition.ID)
        End Function

        Public Shared Function GetId(TransitionProperty As TransitionProperty, Transition As Transition) As String
            Return String.Format("{0}-{1}", TransitionProperty.ID, Transition.ID)
        End Function
    End Class
End Namespace
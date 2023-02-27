Namespace Observers
    Public Class Observable(Of T)
        Public ReadOnly Subscribers As New List(Of Subscriber(Of T))

        Public Sub Subscribe(Subscriber As Subscriber(Of T))
            If Subscribers.Contains(Subscriber) Then Return
            Subscriber.Observer = Me
            Subscribers.Add(Subscriber)
        End Sub

        Public Sub Unsubscribe(Subscriber As Subscriber(Of T))
            If Not Subscribers.Contains(Subscriber) Then Return
            Subscriber.Observer = Nothing
            Subscribers.Remove(Subscriber)
        End Sub

        Private _Value As T
        Public Property Value As T
            Get
                Return _Value
            End Get
            Set(value As T)
                _Value = value
                Dim Count = Subscribers.Count
                For Idx = 0 To Count - 1
                    Subscribers(Idx).Notify(value)
                Next
            End Set
        End Property
    End Class

End Namespace

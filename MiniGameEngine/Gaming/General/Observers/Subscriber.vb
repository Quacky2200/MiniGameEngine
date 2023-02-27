Namespace Observers
    Partial Public MustInherit Class Subscriber(Of T)
        Inherits IDObject

        Public MustOverride Sub Notify(Value As T)

        Public Property Observer As Observable(Of T)
    End Class
End Namespace
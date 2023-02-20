Imports System.Reflection

Public Class CachedCall
    Public Property Method As System.Reflection.MethodInfo
    Public Property Params As Object()
    Public Property Context As Object = Nothing

    Public Sub New(Method As MethodInfo, Optional Params As Object() = Nothing, Optional Context As Object = Nothing)
        Me.Method = Method
        Me.Params = Params
        Me.Context = Context
    End Sub

    Public Sub New(Method As Action, Optional Params As Object() = Nothing)
        Me.Method = Method.GetType().GetMethod("Invoke")
        Me.Params = Params
        Me.Context = Method
    End Sub

    Public Function [Call]() As Object
        Return Method.Invoke(Context, Params)
    End Function
End Class

Imports System.Reflection

Public Class CachedCallSpawner(Of T)
    Public Property Context As T
    Public ReadOnly Reflect As New CachedReflector(Of T)

    Public Sub New(Context As Object)
        Me.Context = Context
    End Sub

    Public Function ByMethodInfo(Method As MethodInfo, Optional Params As Object() = Nothing)
        Return New CachedCall(Method, Params, Context)
    End Function

    Public Function ByMethodName(Method As String, Optional Params As Object() = Nothing)
        Return New CachedCall(Reflect.MethodCalled(Method), Params, Context)
    End Function

    Public Function ByPropertySetter(Setter As String, Optional Params As Object() = Nothing)
        Return New CachedCall(Reflect.SetPropertyCalled(Setter), Params, Context)
    End Function
End Class
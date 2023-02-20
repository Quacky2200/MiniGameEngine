Imports System.Reflection

Public Class CachedReflector(Of T)

    Public Function SetPropertyCalled(PropertyName As String) As MethodInfo
        Dim PropertyInfo = GetType(T).GetProperty(PropertyName)
        Return PropertyInfo.GetSetMethod()
    End Function

    Public Function MethodCalled(MethodName As String) As MethodInfo
        Return GetType(T).GetMethod(MethodName)
    End Function

    Public Function SetPropertyCalled(PropertyName As String, ReturnType As Type, Types As Type()) As MethodInfo
        Dim PropertyInfo = GetType(T).GetProperty(PropertyName, ReturnType, Types)
        Return PropertyInfo.GetSetMethod()
    End Function

    Public Function MethodCalled(MethodName As String, Types As Type()) As MethodInfo
        Return GetType(T).GetMethod(MethodName, Types)
    End Function
End Class
﻿Imports System.Reflection

''' <summary>
''' Modifiable list pools insertions and removables until it's safe to do so
''' </summary>
Public Class ModifiableList(Of T)
    Implements IList(Of T)

    Private ReadOnly Collection As New List(Of T)

    Private ReadOnly CallSpawner As New CachedCallSpawner(Of List(Of T))(Collection)

    Private ReadOnly Pool As New HashSet(Of CachedCall)

    Private Pooling As Boolean = False

    Public Sub Access(Action As Action(Of ModifiableList(Of T)))
        SyncLock Collection
            Pooling = True

            Action(Me)

            While Pool.Count > 0
                Dim Caller As CachedCall
                SyncLock Pool
                    Caller = Pool(0)
                    Pool.Remove(Caller)
                End SyncLock
                ' Sync all modifications after access has finished
                If (Not IsNothing(Caller)) Then Caller.Call()
            End While
        End SyncLock
    End Sub

    Public Sub Sort(F As GenericComparer(Of T).CompareDelegate)
        ' FIXME: Replace ByMethodName with CachedCall and the correct method which accepts an icomparer, or be lazy and keep the Action method below...
        ' Dim Operation = CallSpawner.ByMethodName(MethodBase.GetCurrentMethod().Name, {New GenericComparer(Of T)(F)})
        Dim Operation = New CachedCall(Sub()
                                           Collection.Sort(New GenericComparer(Of T)(F))
                                       End Sub)
        If Pooling Then Pool.Add(Operation) Else Operation.Call()
    End Sub


    Default Public Property Item(index As Integer) As T Implements IList(Of T).Item
        Get
            Return Collection(index)
        End Get
        Set(value As T)
            Dim Operation = New CachedCall(CallSpawner.Reflect.SetPropertyCalled("Item", value.GetType(), New Type() {GetType(Integer)}), {index, value})
            If Pooling Then Pool.Add(Operation) Else Operation.Call()
            Collection(index) = value
        End Set
    End Property

    Public ReadOnly Property Count As Integer Implements ICollection(Of T).Count
        Get
            Return Collection.Count
        End Get
    End Property

    Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of T).IsReadOnly
        Get
            Return False
        End Get
    End Property

    Public Sub Insert(index As Integer, item As T) Implements IList(Of T).Insert
        Dim Operation = CallSpawner.ByMethodName(MethodBase.GetCurrentMethod().Name, {index, item})
        If Pooling Then Pool.Add(Operation) Else Operation.Call()
    End Sub

    Public Sub RemoveAt(index As Integer) Implements IList(Of T).RemoveAt
        Dim Operation = CallSpawner.ByMethodName(MethodBase.GetCurrentMethod().Name, {index})
        If Pooling Then Pool.Add(Operation) Else Operation.Call()
    End Sub

    Public Sub Add(item As T) Implements ICollection(Of T).Add
        Dim Operation = CallSpawner.ByMethodName(MethodBase.GetCurrentMethod().Name, {item})
        If Pooling Then Pool.Add(Operation) Else Operation.Call()
    End Sub

    Public Sub Clear() Implements ICollection(Of T).Clear
        Dim Operation = CallSpawner.ByMethodName(MethodBase.GetCurrentMethod().Name)
        If Pooling Then Pool.Add(Operation) Else Operation.Call()
    End Sub

    Public Sub CopyTo(array() As T, arrayIndex As Integer) Implements ICollection(Of T).CopyTo
        Collection.CopyTo(array, arrayIndex)
    End Sub

    Public Function IndexOf(item As T) As Integer Implements IList(Of T).IndexOf
        Return Collection.IndexOf(item)
    End Function

    Public Function Contains(item As T) As Boolean Implements ICollection(Of T).Contains
        Return Collection.Contains(item)
    End Function

    Public Function Remove(item As T) As Boolean Implements ICollection(Of T).Remove
        Dim Operation = CallSpawner.ByMethodName(MethodBase.GetCurrentMethod().Name, {item})
        If Pooling Then Pool.Add(Operation) Else Return Operation.Call()
        Return True ' Call without pooling if you wish to guarantee removal.
    End Function

    Public Function GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
        Return Collection.GetEnumerator()
    End Function

    Private Function IEnumerable_GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
        Return Collection.GetEnumerator()
    End Function
End Class
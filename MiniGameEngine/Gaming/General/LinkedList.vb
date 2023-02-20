Public Class Node(Of T)
    Public Property Value As T = Nothing
    Public Property Left As Node(Of T) = Nothing
    Public Property Right As Node(Of T) = Nothing
    Public Sub New(ByVal Value As T, ByRef Left As Node(Of T), ByRef Right As Node(Of T))
        Me.Value = Value
        Me.Left = Left
        Me.Right = Right
    End Sub
End Class

Public Class LinkedList(Of T)
    Private ReadOnly AddPool As New HashSet(Of T)
    Private ReadOnly RemovePool As New HashSet(Of T)
    Private Begin As Node(Of T) = Nothing

    Public Sub Add(Value As T)
        SyncLock AddPool
            AddPool.Add(Value)
        End SyncLock
    End Sub

    Public Sub Remove(Value As T)
        SyncLock RemovePool
            RemovePool.Add(Value)
        End SyncLock
    End Sub

    Public ReadOnly Property Count As Integer
        Get
            Dim _Count = 0
            Dim Current = Begin
            While (Not IsNothing(Current))
                _Count += 1
                Current = Current.Right
            End While
            Return _Count
        End Get
    End Property

    Private Function ScheduledForRemoval(Node As Node(Of T)) As Boolean
        If (Not RemovePool.Contains(Node.Value)) Then Return False

        Dim RightNode = Node.Right
        Dim LeftNode = Node.Left

        If Not IsNothing(RightNode) Then RightNode.Left = LeftNode
        If Not IsNothing(LeftNode) Then LeftNode.Right = RightNode

        If (Node.Equals(Begin)) Then
            Begin = RightNode
        End If

        Node.Value = Nothing
        Node.Left = Nothing
        Node.Right = Nothing

        SyncLock RemovePool
            'Debug.WriteLine("RemovePool Count:{0}", RemovePool.Count)
            RemovePool.Remove(Node.Value)
        End SyncLock
        Node = Nothing

        Return True
    End Function

    Private Sub PerformAdditions(Node As Node(Of T))
        ' Debug.WriteLine("AddPool Count:{0}", AddPool.Count)
        ' Add all new nodes from pool
        Dim PoolCount = AddPool.Count - 1
        For i = 0 To PoolCount
            Dim Value As T
            SyncLock AddPool
                Value = AddPool(0)
                AddPool.Remove(Value)
            End SyncLock
            If IsNothing(Node) AndAlso IsNothing(Begin) Then
                Node = New Node(Of T)(Value, Nothing, Nothing)
                Begin = Node
            Else
                Node.Right = New Node(Of T)(Value, Node, Nothing)
                Node = Node.Right
            End If
        Next
    End Sub

    Public Sub [Each](Action As Action(Of T))
        Dim CurrentNode As Node(Of T) = Begin
        Dim LastNode As Node(Of T) = Nothing

        While Not IsNothing(CurrentNode)
            Dim [Next] = CurrentNode.Right
            If Not ScheduledForRemoval(CurrentNode) Then
                LastNode = CurrentNode
                If Not IsNothing(CurrentNode.Value) Then Action(CurrentNode.Value)
            End If

            CurrentNode = [Next]
        End While

        PerformAdditions(LastNode)
    End Sub
End Class
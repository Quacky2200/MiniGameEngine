Imports System.Drawing
Imports MiniGameEngine.Transitions

Public MustInherit Class GameObject
    Private Transitions As New Dictionary(Of String, TransitionPropertyItem)
    Private Scene As Scene = Nothing

    Public ReadOnly ID As String = Guid.NewGuid.ToString
    Public Property Position As New Point(0, 0)

    Public Property Size As New Size

    Private _Rotation As Integer = 0
    Public Property Rotation As Integer
        Get
            Return _Rotation
        End Get
        Set(value As Integer)
            _Rotation = Math.Min(360, Math.Max(0, value))
        End Set
    End Property

    Private ReadOnly _PositionProperty As New TransitionProperty(Me.ID, Sub(ByRef Transition As Transition)
                                                                            Me.Position = Transition.Value
                                                                        End Sub)
    Public ReadOnly Property PositionProperty As TransitionProperty
        Get
            Return _PositionProperty
        End Get
    End Property

    Private ReadOnly _RotationProperty As New TransitionProperty(Me.ID, Sub(ByRef Transition As Transition)
                                                                            Me.Rotation = Transition.Value
                                                                        End Sub)
    Public ReadOnly Property RotationProperty As TransitionProperty
        Get
            Return _RotationProperty
        End Get
    End Property

    Public Property zIndex As Integer = 0
    Public Property Visible As Boolean = True

    Public Sub New()

    End Sub

    Public Sub Show()
        Visible = True
    End Sub

    Public Sub Hide()
        Visible = False
    End Sub

    Public Sub Pause()
        SyncLock Transitions
            For Each KVP In Transitions
                KVP.Value.Transition.Paused = True
            Next
        End SyncLock
    End Sub

    Public Sub [Resume]()
        SyncLock Transitions
            For Each KVP In Transitions
                KVP.Value.Transition.Paused = False
            Next
        End SyncLock
    End Sub

    Public Sub ReferTo(ByRef Scene As Scene)
        Me.Scene = Scene
    End Sub

    Public Sub AddTransition(ByRef TransitionProperty As TransitionProperty, Transition As Transition, Optional AutomaticallyRemoveTransition As Boolean = False, Optional AutomaticallyRemoveObject As Boolean = False)
        SyncLock Transitions
            Dim item As New TransitionPropertyItem(TransitionProperty, Transition, AutomaticallyRemoveTransition, AutomaticallyRemoveObject)
            If Not Transitions.ContainsKey(item.GetId()) Then
                Transitions.Add(item.GetId(), item)
            End If
        End SyncLock
    End Sub

    Public Sub RemoveTransition(ByRef TransitionProperty As TransitionProperty, Transition As Transition)
        SyncLock Transitions
            Dim Key = TransitionPropertyItem.GetId(TransitionProperty, Transition)
            If (Transitions.ContainsKey(Key)) Then Transitions.Remove(Key)
        End SyncLock
    End Sub

    Public Sub SendToTop()
        Dim TopMostGameObject = Scene.GameObjects.OrderByDescending(Function(G) G.zIndex).First()
        If Not IsNothing(TopMostGameObject) Then
            zIndex = TopMostGameObject.zIndex + 1
        End If
        Scene.RefreshZIndexes()
    End Sub

    Public Sub SendToBack()
        Dim BottomGameObject = Scene.GameObjects.OrderByDescending(Function(G) G.zIndex).Last()
        If Not IsNothing(BottomGameObject) Then
            zIndex = BottomGameObject.zIndex - 1
        End If
        ' zIndex = -1
        Scene.RefreshZIndexes()
    End Sub

    Public Overridable Sub Render(Graphics As Graphics)
        ' Derived objects must call Mybase.Render(Graphics) for transitions to update
    End Sub

    Public Sub UpdateTransitions()
        ' UpdateTransitions()
        ' Transitions.Access(AddressOf UpdateTransitions)
        Dim Bin As New List(Of String)
        For Each KVP In Transitions
            Dim Item = KVP.Value

            Item.Work()

            If Item.RemoveObjectNow Then Scene.RemoveGameObject(Me)
            If Item.RemoveTransitionNow Then Bin.Add(KVP.Key)
        Next

        For Each Item In Bin
            Transitions.Remove(Item)
        Next
    End Sub

    Public Overridable Sub Update(delta As Double)
        ' Do nothing for now.
    End Sub

End Class

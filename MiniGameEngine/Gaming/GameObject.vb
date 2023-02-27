Imports System.Drawing
Imports MiniGameEngine.Transitions

Public MustInherit Class GameObject
    Inherits IDObject

    Private ReadOnly Transitions As New LinkedList(Of Transition)
    Private ReadOnly Components As New Dictionary(Of String, Component)

    Private _Scene As Scene = Nothing
    Public Property Scene As Scene
        Get
            Return _Scene
        End Get
        Private Set(value As Scene)
            _Scene = value
        End Set
    End Property

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

    Public ReadOnly PositionProperty As New TransitionProperty(AddressOf OnPositionUpdateProperty)
    Private Sub OnPositionUpdateProperty(ByRef Sender As Object, ByRef Value As Point)
        Me.Position = Value
    End Sub

    Public ReadOnly RotationProperty As New TransitionProperty(AddressOf OnRotationUpdateProperty)
    Private Sub OnRotationUpdateProperty(ByRef Sender As Object, ByRef Value As Integer)
        Me.Rotation = Value
    End Sub

    Public Property ZIndex As Integer = 0
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
            Transitions.Each(Sub(Transition As Transition)
                                 Transition.Paused = True
                             End Sub)
        End SyncLock
    End Sub

    Public Sub [Resume]()
        SyncLock Transitions
            Transitions.Each(Sub(Transition As Transition)
                                 Transition.Paused = False
                             End Sub)
        End SyncLock
    End Sub

    Public Sub ReferTo(ByRef Scene As Scene)
        Me.Scene = Scene
    End Sub

    Private Sub Transition_OnFinish_RemoveTransition(sender As Object)
        Transitions.Remove(sender)
    End Sub

    Private Sub Transition_OnFinish_RemoveGameObject(sender As Object)
        Scene.RemoveGameObject(Me)
    End Sub

    Public Sub AddTransition(ByRef TransitionProperty As TransitionProperty, Transition As Transition, Optional AutomaticallyRemoveTransition As Boolean = False, Optional AutomaticallyRemoveObject As Boolean = False)
        SyncLock Transitions
            If Transitions.Contains(Transition) Then Return

            Transition.Subscribe(TransitionProperty)

            If AutomaticallyRemoveTransition Then
                AddHandler Transition.OnFinish, AddressOf Transition_OnFinish_RemoveTransition
            End If

            If AutomaticallyRemoveObject Then
                AddHandler Transition.OnFinish, AddressOf Transition_OnFinish_RemoveGameObject
            End If

            Transitions.Add(Transition)

            Transition.Activate()
        End SyncLock
    End Sub

    Public Sub AddTransition(Transition As Transition)
        SyncLock Transitions
            If Transitions.Contains(Transition) Then Return

            Transitions.Add(Transition)
            Transition.Activate()
        End SyncLock
    End Sub

    Public Sub RemoveTransition(ByRef TransitionProperty As TransitionProperty, Transition As Transition)
        SyncLock Transitions
            Transition.Unsubscribe(TransitionProperty)
            RemoveTransition(Transition)
        End SyncLock
    End Sub

    Public Sub RemoveTransition(Transition As Transition)
        If Transitions.Contains(Transition) Then Return
        Transitions.Remove(Transition)
    End Sub

    Public Sub AddComponent(Component As Component)
        If IsNothing(Component) Then Return

        SyncLock Components
            If Not Components.ContainsKey(Component.ID) Then
                Components.Add(Component.ID, Component)
                Component.Begin()
            End If
        End SyncLock
    End Sub

    Public Sub RemoveTransition(Component As Component)
        If IsNothing(Component) Then Return

        SyncLock Components
            If (Components.ContainsKey(Component.ID)) Then
                Components.Remove(Component.ID)
            End If
        End SyncLock
    End Sub

    Public Sub SendToTop()
        Dim TopMostGameObject = Scene.GameObjects.OrderByDescending(Function(G) G.ZIndex).First()
        If Not IsNothing(TopMostGameObject) Then
            ZIndex = TopMostGameObject.ZIndex + 1
        End If
        Scene.RefreshZIndexes()
    End Sub

    Public Sub SendToBack()
        Dim BottomGameObject = Scene.GameObjects.OrderByDescending(Function(G) G.ZIndex).Last()
        If Not IsNothing(BottomGameObject) Then
            ZIndex = BottomGameObject.ZIndex - 1
        End If
        Scene.RefreshZIndexes()
    End Sub

    Public Overridable Sub Render(Graphics As Graphics)
    End Sub

    Public Sub UpdateTransitions()
        Transitions.Each(Sub(Transition As Transition)
                             Transition.Update()
                         End Sub)
    End Sub

    Public Sub UpdateComponents(Delta As Double)
        Dim Bin As New List(Of String)

        For Each KVP In Components
            Dim Item = KVP.Value
            If (Item.Enabled) Then Item.Update(Delta)
        Next

        For Each Item In Bin
            Components.Remove(Item)
        Next
    End Sub

    Public Overridable Sub Update(Delta As Double)
        ' Do nothing for now.
    End Sub
End Class

Namespace Transitions
    Partial Public MustInherit Class Transition
        Inherits Observers.Observable(Of Object)

        Public EasingFunction As EasingFunctions.Delegate = AddressOf EasingFunctions.Linear

#Region "Events & General Properties"
        Public Event OnFinish(Sender As Object)

        Public Event OnRepeat(Sender As Object)

        Public Event OnReverse(OldDirection As TransitionDirection, NewDirection As TransitionDirection)

        Public Sub Reset()
            If IsFinished Then
                _IsFinished = False
                Direction = TransitionDirection.Forward
                Value = GetValue(0)
            End If
        End Sub

        Private Property Speed As Double = 1
        ''' <summary>
        ''' On true, it uses half the duration to create the reverse in the other half, thereby creating a 'whole' transition
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ReverseUsesDuration As Boolean
            Get
                Return Speed = 0.5
            End Get
            Set(value As Boolean)
                If value Then
                    Speed = 0.5
                Else
                    Speed = 1
                End If
            End Set
        End Property

        ''' <summary>
        ''' One end of the transition spectrum
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property StartValue As Object

        ''' <summary>
        ''' One end of the transition spectrum
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EndValue As Object

        ''' <summary>
        ''' Should the transition reverse?
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Reverse As Boolean = False

        ''' <summary>
        ''' Should the transition repeat?
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Repeat As Boolean = False

#End Region

#Region "Constructor"

        Public Sub New()
        End Sub

        ''' <summary>
        ''' Make a new basic transition
        ''' </summary>
        ''' <param name="StartValue">The place to start at</param>
        ''' <param name="EndValue">The place to end at</param>
        ''' <remarks></remarks>
        Public Sub New(StartValue As Object, EndValue As Object)
            Me.StartValue = StartValue
            Me.EndValue = EndValue
            Me.Value = GetValue(0)
        End Sub

        ''' <summary>
        ''' Make a transition from point A to B starting at Start Position
        ''' </summary>
        ''' <param name="StartValue">One end of the transition spectrum</param>
        ''' <param name="EndValue">One end of the transition spectrum</param>
        ''' <param name="Duration">How long you want the transition to last</param>
        ''' <param name="Enabled">Should we start it?</param>
        ''' <remarks></remarks>
        Public Sub New(StartValue As Object, EndValue As Object, Optional Duration As TimeSpan = Nothing, Optional Enabled As Boolean = False)
            Me.StartValue = StartValue
            Me.EndValue = EndValue
            Me.Duration = TimeSpan.FromTicks(Math.Max(Duration.Ticks, TimeSpan.TicksPerMillisecond)) ' 1ms minimum
            Me.Enabled = Enabled
            Me.Value = GetValue(0)
        End Sub

        Public Sub New(CurrentValue As Object, StartValue As Object, EndValue As Object, Optional Duration As TimeSpan = Nothing, Optional Enabled As Boolean = False)
            Me.StartValue = StartValue
            Me.EndValue = EndValue
            Me.Duration = TimeSpan.FromTicks(Math.Max(Duration.Ticks, TimeSpan.TicksPerMillisecond)) ' 1ms minimum
            Me.Enabled = Enabled
            Me.Value = GetValue(0)
        End Sub
#End Region

#Region "On/Off Switch"

        Private _PausedProgress As Double = 0
        Private _Paused As Boolean = False
        Public Property Enabled As Boolean

        Public Sub Start()
            Enabled = True
        End Sub

        Public Sub [Stop]()
            Enabled = False
        End Sub

        Public Property Paused As Boolean
            Get
                Return _Paused
            End Get
            Set(value As Boolean)
                If value Then
                    _PausedProgress = Progress()
                ElseIf Not value AndAlso _Paused Then
                    ' Set time to artificial time based on current progress
                    StartTick = Now.Ticks - ((Duration.Ticks * Speed) * _PausedProgress)
                End If
                _Paused = value
            End Set
        End Property
#End Region

#Region "Timing"

        Public Property Duration As TimeSpan = TimeSpan.FromSeconds(1)

        Private StartTick As Long = 0

        Public ReadOnly Property Progress As Double
            Get
                If StartTick = 0 Then Return 0
                Return Clamp((Now.Ticks() - StartTick) / (Duration.Ticks * Speed), 0, 1)
            End Get
        End Property

#End Region

#Region "Transition Function"

        Public Property Direction As TransitionDirection = TransitionDirection.Forward
        ''' <summary>
        ''' Easy way to swap the transition
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SwapDirections()
            Direction = If(Direction = TransitionDirection.Forward, TransitionDirection.Backward, TransitionDirection.Forward)
        End Sub

        Protected MustOverride Function GetValue(StartValue As Object, EndValue As Object, T As Double) As Object

        Protected Function GetValue(T As Double) As Object
            Dim StartValue As Object = Me.StartValue
            Dim EndValue As Object = Me.EndValue

            If IsNothing(StartValue) Or IsNothing(EndValue) Then Return Nothing

            Return GetValue(StartValue, EndValue, If(Direction = TransitionDirection.Forward, T, 1.0 - T))
        End Function

        Public Sub Update()
            If (Not Enabled) Or Paused Then Return

            Dim T = Progress

            Value = GetValue(T)

            If T >= 1 Then
                'If we're running a normal transition (A->B) OR finished the reverse process without repetition, stop everything
                If Not Reverse And Not Repeat Or (Reverse AndAlso Not Repeat AndAlso _Direction = TransitionDirection.Backward) Then
                    _Enabled = False
                    RaiseEvent OnFinish(Me)
                    _IsFinished = True
                ElseIf Not Reverse And Repeat Then
                    ' If we're not reversing but repeating, start again from the first value
                    RaiseEvent OnRepeat(Me)
                Else
                    Dim OldDirection = Direction
                    SwapDirections()
                    RaiseEvent OnReverse(OldDirection, Direction)
                End If
                StartTick = Now.Ticks
            End If
        End Sub

        Public Sub Activate()
            If StartTick = 0 Then StartTick = Now.Ticks
            Value = GetValue(0)
        End Sub

        Private _IsFinished As Boolean = False
        Public ReadOnly Property IsFinished As Boolean
            Get
                Return _IsFinished
            End Get
        End Property
#End Region
    End Class
End Namespace
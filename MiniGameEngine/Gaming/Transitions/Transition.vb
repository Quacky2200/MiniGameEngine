Namespace Transitions
    Partial Public MustInherit Class Transition
#Region "Events & General Properties"
        Public ReadOnly ID As String = Guid.NewGuid.ToString()
        Public Event OnFinish(sender As Object)

        Public Event OnRepeat(sender As Object)

        Public Event OnReverse(oldDirection As TransitionDirection, newDirection As TransitionDirection)

        Public Sub Reset()
            If isFinished Then
                _isFinished = False
                Direction = TransitionDirection.Forward
                C = A
            End If
        End Sub
        ''' <summary>
        ''' On true, it uses half the duration to create the reverse in the other half, thereby creating a 'whole' transition
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ReverseUsesDuration As Boolean
        ''' <summary>
        ''' One end of the transition spectrum
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property A As Object
        ''' <summary>
        ''' One end of the transition spectrum
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property B As Object
        ''' <summary>
        ''' The current value in the transition
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Property C As Object
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
        ''' <summary>
        ''' Make a new basic transition
        ''' </summary>
        ''' <param name="A">The place to start at</param>
        ''' <param name="B">The place to end at</param>
        ''' <remarks></remarks>
        Public Sub New(A As Object, B As Object)
            Me.A = A
            Me.B = B
            Me.C = A
            Evaluate()
        End Sub
        ''' <summary>
        ''' Make a transition from point A to B starting at Start Position
        ''' </summary>
        ''' <param name="StartPosition">The point at which to start at</param>
        ''' <param name="A">One end of the transition spectrum</param>
        ''' <param name="B">One end of the transition spectrum</param>
        ''' <param name="Duration">How long you want the transition to last</param>
        ''' <param name="Enabled">Should we start it?</param>
        ''' <remarks></remarks>
        Public Sub New(StartPosition As Object, A As Object, B As Object, Optional Duration As TimeSpan = Nothing, Optional Enabled As Boolean = False)
            Me.A = A
            Me.B = B
            Me.C = StartPosition
            Evaluate()
            Me.Duration = If(Duration.Ticks = 0, Me.Duration, Duration)
            Me.Enabled = Enabled
        End Sub
#End Region

#Region "On/Off Switch"
        Private Property _Enabled As Boolean = False

        Public Property Enabled As Boolean
            Get
                Return _Enabled
            End Get
            Set(value As Boolean)
                If value Then
                    startTick = Now.Ticks()
                    Evaluate()
                End If
                _Enabled = value
            End Set
        End Property

        Public Sub Start()
            Enabled = True
        End Sub

        Public Sub [Stop]()
            Enabled = False
        End Sub
        Private Property _Paused As Boolean = False
        Private Property PausedTick As Long
        Public Property Paused As Boolean
            Get
                Return _Paused
            End Get
            Set(value As Boolean)
                If value Then
                    PausedTick = Now.Ticks
                ElseIf Not value AndAlso _Paused Then
                    Dim TimeTaken As Long = PausedTick - startTick
                    startTick = Now.Ticks - PausedTick
                End If
                _Paused = value
            End Set
        End Property
#End Region

#Region "Timing"

        Public Property Duration As TimeSpan = TimeSpan.FromSeconds(1)

        Private startTick As Long

        Private Function GetDifference() As Long
            Dim CurrentTick As Long = Now.Ticks()
            Return CurrentTick - startTick
        End Function

#End Region

#Region "Transition Function"
        Private transitionTime As Long = Now.Ticks
        Public Property Direction As TransitionDirection = TransitionDirection.Forward
        ''' <summary>
        ''' Easy way to swap the transition
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub swapDirections()
            Direction = If(Direction = TransitionDirection.Forward, TransitionDirection.Backward, TransitionDirection.Forward)
        End Sub
        ''' <summary>
        ''' The converter function to convert the object we have changed back into a readable format
        ''' </summary>
        ''' <param name="rawValues"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function ConvertFromRaw(ByVal rawValues As Double()) As Object
        ''' <summary>
        ''' The converter function to convert an object to a understandable transition type (double)
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public MustOverride Function ConvertToRaw(ByVal obj As Object) As Double()

        ''' <summary>
        ''' The work we have to complete
        ''' </summary>
        ''' <remarks></remarks>
        Private _W As TransitionWorkElement
        ''' <summary>
        ''' Before each time a new transition occurs, the evaluation finds the amount of work to do and where to go, finally storing it in a TransitionWorkElement for later.
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub Evaluate()
            Dim _C As Double() = ConvertToRaw(C)
            Dim _D As Double() = ConvertToRaw(If(Direction = TransitionDirection.Forward, B, A))
            Dim _W(_C.Length - 1) As Double
            For i = _C.Length - 1 To 0 Step -1
                _W(i) = _D(i) - _C(i)
            Next
            Me._W = New TransitionWorkElement(_C, _W)
        End Sub
        ''' <summary>
        ''' Retrieve the current value from the transition using the time difference.
        ''' </summary>
        ''' <param name="time"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private ReadOnly Property Value(time As Double) As Object
            'Using the TransitionWorkElement class, we can add the amount of work from the starting position 
            'according to the amount of time within the transition from 0-1 (percentage done from time difference)
            Get
                Dim _C As Double() = ConvertToRaw(C)
                For i = Me._W.Work.Length - 1 To 0 Step -1
                    _C(i) = (Me._W.StartPosition(i) + (Me._W.Work(i) * time))
                Next
                C = ConvertFromRaw(_C)
                Return C
            End Get
        End Property
        Private Property _isFinished As Boolean = False
        Public ReadOnly Property isFinished As Boolean
            Get
                Return _isFinished
            End Get
        End Property
        Private Sub Complete()
            _Enabled = False
            RaiseEvent OnFinish(Me)
            _isFinished = True
        End Sub
        ''' <summary>
        ''' Retrieve the current value from the transition.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Value As Object
            Get
                Dim difference As Long = GetDifference()
                Dim v As Double = difference / (If(ReverseUsesDuration And Reverse, Duration.Ticks / 2, Duration.Ticks))
                If (Enabled And Not Paused) AndAlso v < 1 Then
                    Return Value(v)
                ElseIf (Enabled And Not Paused) AndAlso v >= 1 Then
                    'If we are running a normal transition (A->B) OR if we have finished the reverse process without repition, stop everything
                    If Not Reverse And Not Repeat Or (Reverse AndAlso Not Repeat AndAlso _Direction = TransitionDirection.Backward) Then
                        Complete()
                        Return lastValue
                    ElseIf Not Reverse And Repeat Then
                        'But, if we aren't reversing but we're repeating, start again from the first value
                        C = A
                        RaiseEvent OnRepeat(Me)
                    Else
                        RaiseEvent OnReverse(Direction, If(Direction = TransitionDirection.Forward, TransitionDirection.Backward, TransitionDirection.Forward))
                        swapDirections()
                    End If
                    Evaluate()
                    startTick = Now.Ticks()
                End If
                Return lastValue
            End Get
        End Property
        ''' <summary>
        ''' Retrieve the last value that from the transition
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property lastValue As Object
            Get
                Return C
            End Get
        End Property
#End Region
    End Class
End Namespace
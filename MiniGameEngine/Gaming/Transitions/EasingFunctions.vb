Namespace Transitions
    Public Class EasingFunctions
        ' Easing functions according to https://www.w3.org/TR/css-easing-1/#cubic-bezier-easing-functions

        Public Delegate Function [Delegate](A As Double, B As Double, T As Double) As Double

        Public Shared Function Linear(A As Double, B As Double, T As Double) As Double
            Return Lerp(A, B, T)
        End Function

        Public Shared Function Ease(A As Double, B As Double, T As Double) As Double
            Return Lerp(A, B, CubicBezierLerp(0.25, 0.1, 0.25, 1, T))
        End Function

        Public Shared Function EaseIn(A As Double, B As Double, T As Double) As Double
            Return Lerp(A, B, CubicBezierLerp(0.42, 0, 1, 1, T))
        End Function

        Public Shared Function EaseOut(A As Double, B As Double, T As Double) As Double
            Return Lerp(A, B, CubicBezierLerp(0, 0, 0.58, 1, T))
        End Function

        Public Shared Function EaseInOut(A As Double, B As Double, T As Double) As Double
            Return Lerp(A, B, CubicBezierLerp(1, 0.5, 0, 0, T))
        End Function
    End Class
End Namespace

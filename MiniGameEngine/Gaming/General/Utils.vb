Imports System.Drawing

Public Module Utils
    Public Function Lerp(Start As Double, [End] As Double, T As Double) As Double
        Return (1 - T) * Start + T * [End]
    End Function

    Public Function LerpQuadracticBezier(A As Double, B As Double, C As Double, T As Double) As Double
        Return Lerp(Lerp(A, C, T), Lerp(C, B, T), T)
    End Function

    Public Function CubicBezierLerp(X1 As Double, Y1 As Double, X2 As Double, Y2 As Double, T As Double) As Double
        ' As according to the W3 spec using X1/Y1 and X2/Y2 graph points
        ' with help from Freya Holma's 'The Ever So Lovely Bezier Curve' article
        ' Source: https://acegikmo.medium.com/the-ever-so-lovely-b%C3%A9zier-curve-eb27514da3bf

        ' Calculate graph points from control points based on time
        Dim PX1 As Double = Lerp(0, X1, T)
        Dim PX2 As Double = Lerp(X1, X2, T)
        Dim PX3 As Double = Lerp(X2, 1, T)
        Dim PY1 As Double = Lerp(0, Y1, T)
        Dim PY2 As Double = Lerp(Y1, Y2, T)
        Dim PY3 As Double = Lerp(Y2, 1, T)

        ' Calculate curve based on calculated control points
        Dim CX1 As Double = Lerp(PX1, PX2, T)
        Dim CX2 As Double = Lerp(PX2, PX3, T)
        Dim CY1 As Double = Lerp(PY1, PY2, T)
        Dim CY2 As Double = Lerp(PY2, PY3, T)

        Return Lerp(Lerp(CX1, CX2, T), Lerp(CY1, CY2, T), T)
    End Function

    Public Function LerpQuadracticBezier(A As PointF, B As PointF, C As PointF, T As Double) As PointF
        Dim P As New PointF(0, 0) With {
            .X = LerpQuadracticBezier(A.X, B.X, C.X, T),
            .Y = LerpQuadracticBezier(A.Y, B.Y, C.Y, T)
        }

        P.X = Lerp(P.X, B.X, T)
        P.Y = Lerp(P.Y, B.Y, T)

        Return P
    End Function

    Public Function Clamp(Value As Double, Min As Double, Max As Double) As Double
        Return Math.Min(Max, Math.Max(Min, Value))
    End Function

    Public Function Clamp(Value As Integer, Min As Integer, Max As Integer) As Integer
        Return Math.Min(Max, Math.Max(Min, Value))
    End Function
End Module


Namespace General.Threading
    Partial Public NotInheritable Class ThreadWork
        Public Delegate Sub Work()
        Public Shared Sub Start(Action As Work)
            Action.BeginInvoke(Nothing, Nothing)
        End Sub
    End Class
End Namespace

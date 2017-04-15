Imports Microsoft.VisualBasic

Public Class PageSizeChangeEventArgs
    Private _PageSize As Integer
    Public Sub New(ByVal pageSize As Integer)
        _PageSize = pageSize
    End Sub
    Public ReadOnly Property PageSize() As Integer
        Get
            Return Me._PageSize
        End Get
    End Property
End Class

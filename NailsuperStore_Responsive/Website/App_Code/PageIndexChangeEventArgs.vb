Imports Microsoft.VisualBasic

Public Class PageIndexChangeEventArgs
    Private _PageIndex As Integer
    Public Sub New(ByVal pageIndex As Integer)
        _PageIndex = pageIndex
    End Sub
    Public ReadOnly Property PageIndex() As Integer
        Get
            Return Me._PageIndex
        End Get
    End Property

End Class

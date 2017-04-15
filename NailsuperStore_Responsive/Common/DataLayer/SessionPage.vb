
Namespace DataLayer
    Public Class SessionPageRow
        Inherits SessionPageRowBase
    End Class

    Public Class SessionPageRowBase
        Private m_SessionName As String = Nothing
        Private m_PageList As List(Of String) = Nothing
        Private m_TypePage As String
        Public Property TypePage() As String
            Get
                Return m_TypePage
            End Get
            Set(ByVal value As String)
                m_TypePage = value
            End Set
        End Property

        Public Property SessionName() As String
            Get
                Return m_SessionName
            End Get
            Set(ByVal value As String)
                m_SessionName = value
            End Set
        End Property

        Public Property PageList() As List(Of String)
            Get
                Return m_PageList
            End Get
            Set(ByVal value As List(Of String))
                m_PageList = value
            End Set
        End Property
    End Class

    Public Class SessionPageCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal session As SessionPageRow)
            Me.List.Add(session)
        End Sub
        Default Public Property Item(ByVal Index As Integer) As SessionPageRow
            Get
                Return CType(Me.List.Item(Index), SessionPageRow)
            End Get

            Set(ByVal Value As SessionPageRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal session As SessionPageRow)
            Me.List.Remove(session)
        End Sub
    End Class


End Namespace


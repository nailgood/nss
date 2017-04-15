
Namespace DataLayer
    Public Class CssPageRow
        Inherits CssPageRowBase
    End Class

    Public Class CssPageRowBase
        Private m_CssName As String = Nothing
        Private m_PageList As List(Of NodeAttribute) = Nothing
        Private m_TypePage As String
        Public Property TypePage() As String
            Get
                Return m_TypePage
            End Get
            Set(ByVal value As String)
                m_TypePage = value
            End Set
        End Property

        Public Property CssName() As String
            Get
                Return m_CssName
            End Get
            Set(ByVal value As String)
                m_CssName = value
            End Set
        End Property

        Public Property PageList() As List(Of NodeAttribute)
            Get
                Return m_PageList
            End Get
            Set(ByVal value As List(Of NodeAttribute))
                m_PageList = value
            End Set
        End Property
    End Class

    Public Class CssPageCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Css As CssPageRow)
            Me.List.Add(Css)
        End Sub
        Default Public Property Item(ByVal Index As Integer) As CssPageRow
            Get
                Return CType(Me.List.Item(Index), CssPageRow)
            End Get

            Set(ByVal Value As CssPageRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal Css As CssPageRow)
            Me.List.Remove(Css)
        End Sub
    End Class


End Namespace


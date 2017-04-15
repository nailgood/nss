
Namespace DataLayer
    Public Class ScriptPageRow
        Inherits ScriptPageRowBase
    End Class

    Public Class NodeAttribute
        Public URL As String
        Public Defer As String
        Public Custom As Boolean
        Public Async As String
    End Class

    Public Class ScriptPageRowBase
        Private m_ScriptName As String = Nothing
        Private m_PageList As List(Of NodeAttribute) = Nothing
        Private m_TypePage As String
        Private m_defer As String = Nothing
        Private m_async As String = Nothing

        Public Property Defer() As String
            Get
                Return m_defer
            End Get
            Set(ByVal value As String)
                m_defer = value
            End Set
        End Property

        Public Property Async() As String
            Get
                Return m_async
            End Get
            Set(value As String)
                m_async = value
            End Set
        End Property

        Public Property TypePage() As String
            Get
                Return m_TypePage
            End Get
            Set(ByVal value As String)
                m_TypePage = value
            End Set
        End Property

        Public Property ScriptName() As String
            Get
                Return m_ScriptName
            End Get
            Set(ByVal value As String)
                m_ScriptName = value
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

    Public Class ScriptPageCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Script As ScriptPageRow)
            Me.List.Add(Script)
        End Sub
        Default Public Property Item(ByVal Index As Integer) As ScriptPageRow
            Get
                Return CType(Me.List.Item(Index), ScriptPageRow)
            End Get

            Set(ByVal Value As ScriptPageRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal Script As ScriptPageRow)
            Me.List.Remove(Script)
        End Sub
    End Class


End Namespace


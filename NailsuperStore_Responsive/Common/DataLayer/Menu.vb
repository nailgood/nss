Namespace DataLayer
    Public Class MenuRow
        Inherits MenuRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

    End Class

    Public Class MenuRowBase
        Private m_Id As Integer = 0
        Private m_MenuName As String = Nothing
        Private m_Href As String = Nothing
        Private m_IsActive As Boolean = True
        Private m_Level As Integer = Nothing
        Private m_AllowGroup As Boolean = False
        Private m_Lastitem As Boolean = False
        Private m_ParentId As Integer = 0

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal value As Integer)
                m_Id = value
            End Set
        End Property

        Public Property MenuName() As String
            Get
                Return m_MenuName
            End Get
            Set(ByVal value As String)
                m_MenuName = value
            End Set
        End Property

        Public Property Href() As String
            Get
                Return m_Href
            End Get
            Set(ByVal value As String)
                m_Href = value
            End Set
        End Property

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal value As Boolean)
                m_IsActive = value
            End Set
        End Property

        Public Property Level() As Integer
            Get
                Return m_Level
            End Get
            Set(ByVal value As Integer)
                m_Level = value
            End Set
        End Property

        Public Property AllowGroup() As Boolean
            Get
                Return m_AllowGroup
            End Get
            Set(ByVal value As Boolean)
                m_AllowGroup = value
            End Set
        End Property

        Public Property LastItem() As Boolean
            Get
                Return m_Lastitem
            End Get
            Set(ByVal value As Boolean)
                m_Lastitem = value
            End Set
        End Property

        Public Property ParentId() As Integer
            Get
                Return m_ParentId
            End Get
            Set(ByVal value As Integer)
                m_ParentId = value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

    End Class

    Public Class MenuCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Menu As MenuRow)
            Me.List.Add(Menu)
        End Sub

        Public Function Contains(ByVal Menu As MenuRow) As Boolean
            Return Me.List.Contains(Menu)
        End Function

        Public Function IndexOf(ByVal Menu As MenuRow) As Integer
            Return Me.List.IndexOf(Menu)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal Menu As MenuRow)
            Me.List.Insert(Index, Menu)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As MenuRow
            Get
                Return CType(Me.List.Item(Index), MenuRow)
            End Get

            Set(ByVal Value As MenuRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal Menu As MenuRow)
            Me.List.Remove(Menu)
        End Sub
    End Class


End Namespace


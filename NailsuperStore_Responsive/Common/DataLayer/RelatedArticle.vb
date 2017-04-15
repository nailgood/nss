Namespace DataLayer
    Public Class RelatedArticleRow
        Inherits RelatedArticleRowBase

        Public Sub New()
            MyBase.New()
        End Sub

    End Class

    Public Class RelatedArticleRowBase
        Private m_Id As Integer
        Private m_CategoryId As Integer
        Private m_Title As String
        Private m_Description As String
        Private m_ShortDescription As String
        Private m_Type As Integer

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal value As Integer)
                m_Id = value
            End Set
        End Property

        Public Property CategoryId() As Integer
            Get
                Return m_CategoryId
            End Get
            Set(ByVal value As Integer)
                m_CategoryId = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set(ByVal value As String)
                m_Title = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return m_Description
            End Get
            Set(ByVal value As String)
                m_Description = value
            End Set
        End Property
        Public Property ShortDescription() As String
            Get
                Return m_ShortDescription
            End Get
            Set(ByVal value As String)
                m_ShortDescription = value
            End Set
        End Property

        Public Property Type() As Integer
            Get
                Return m_Type
            End Get
            Set(ByVal value As Integer)
                m_Type = value
            End Set
        End Property

        Public Sub New()
        End Sub
    End Class

    Public Class RelatedArticleCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Menu As RelatedArticleRow)
            Me.List.Add(Menu)
        End Sub

        Public Function Contains(ByVal RelatedArticle As RelatedArticleRow) As Boolean
            Return Me.List.Contains(RelatedArticle)
        End Function

        Public Function IndexOf(ByVal RelatedArticle As RelatedArticleRow) As Integer
            Return Me.List.IndexOf(RelatedArticle)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal RelatedArticle As RelatedArticleRow)
            Me.List.Insert(Index, RelatedArticle)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As RelatedArticleRow
            Get
                Return CType(Me.List.Item(Index), RelatedArticleRow)
            End Get

            Set(ByVal Value As RelatedArticleRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal RelatedArticle As RelatedArticleRow)
            Me.List.Remove(RelatedArticle)
        End Sub
    End Class
End Namespace
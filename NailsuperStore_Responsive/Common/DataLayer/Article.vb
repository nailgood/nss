Option Explicit On


Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Namespace DataLayer

    Public Class ArticleRow
        Inherits ArticleRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ArticleId As Integer)
            MyBase.New(database, ArticleId)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal ArticleId As Integer) As ArticleRow
            Dim row As ArticleRow

            row = New ArticleRow(_Database, ArticleId)
            row.Load()

            Return row
        End Function

        Public Shared Sub RemoveRow(ByVal _Database As Database, ByVal ArticleId As Integer)
            Dim row As ArticleRow

            row = New ArticleRow(_Database, ArticleId)
            row.Remove()
        End Sub
        

        'Custom Methods
        Public Shared Function GetCurrentNews(ByVal DB As Database) As DataSet
            Dim SQL As String = String.Empty

            SQL &= " select ArticleId, Title, OtherTitle, PostDate, [Image], Link, ShortAbstract, ShortVersion, FullVersion, HasFullVersion from Article"
            SQL &= " where PastNewsDate > DateAdd(d,-1,getdate()) and IsActive = 1"
            SQL &= " union all"
            SQL &= " select ArticleId, Title, OtherTitle, PostDate, [Image], Link, ShortAbstract, ShortVersion, FullVersion, HasFullVersion from Article"
            SQL &= " where PastNewsDate is null and IsActive = 1 "
            SQL &= " order by PostDate Desc"

            Return DB.GetDataSet(SQL)
        End Function

        Public Shared Function GetPastNews(ByVal DB As Database) As DataSet
            Dim SQL As String = String.Empty

            SQL &= " select ArticleId, Title, OtherTitle, PostDate, [Image], Link, ShortAbstract, ShortVersion, FullVersion, HasFullVersion from Article "
            SQL &= " where PastNewsDate <= DateAdd(d,1,getdate()) and IsActive = 1"
            SQL &= " order by PostDate Desc"

            Return DB.GetDataSet(SQL)
        End Function

    End Class

    Public MustInherit Class ArticleRowBase
        Private m_DB As Database
        Private m_ArticleId As Integer = Nothing
        Private m_Title As String = Nothing
        Private m_OtherTitle As String = Nothing
        Private m_ShortAbstract As String = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_PostDate As DateTime = Nothing
        Private m_PastNewsDate As DateTime = Nothing
        Private m_Image As String = Nothing
        Private m_Link As String = Nothing
        Private m_HasFullVersion As Boolean = Nothing
        Private m_ShortVersion As String = Nothing
        Private m_FullVersion As String = Nothing


        Public Property ArticleId() As Integer
            Get
                Return m_ArticleId
            End Get
            Set(ByVal Value As Integer)
                m_ArticleId = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return m_Title
            End Get
            Set(ByVal Value As String)
                m_Title = value
            End Set
        End Property

        Public Property OtherTitle() As String
            Get
                Return m_OtherTitle
            End Get
            Set(ByVal Value As String)
                m_OtherTitle = value
            End Set
        End Property

        Public Property ShortAbstract() As String
            Get
                Return m_ShortAbstract
            End Get
            Set(ByVal Value As String)
                m_ShortAbstract = value
            End Set
        End Property

        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal Value As Boolean)
                m_IsActive = value
            End Set
        End Property

        Public Property PostDate() As DateTime
            Get
                Return m_PostDate
            End Get
            Set(ByVal Value As DateTime)
                m_PostDate = value
            End Set
        End Property

        Public Property PastNewsDate() As DateTime
            Get
                Return m_PastNewsDate
            End Get
            Set(ByVal Value As DateTime)
                m_PastNewsDate = value
            End Set
        End Property

        Public Property Image() As String
            Get
                Return m_Image
            End Get
            Set(ByVal Value As String)
                m_Image = value
            End Set
        End Property

        Public Property Link() As String
            Get
                Return m_Link
            End Get
            Set(ByVal Value As String)
                m_Link = Value
            End Set
        End Property

        Public Property HasFullVersion() As Boolean
            Get
                Return m_HasFullVersion
            End Get
            Set(ByVal Value As Boolean)
                m_HasFullVersion = value
            End Set
        End Property

        Public Property ShortVersion() As String
            Get
                Return m_ShortVersion
            End Get
            Set(ByVal Value As String)
                m_ShortVersion = value
            End Set
        End Property

        Public Property FullVersion() As String
            Get
                Return m_FullVersion
            End Get
            Set(ByVal Value As String)
                m_FullVersion = value
            End Set
        End Property

        Public Property DB() As Database
            Get
                DB = m_DB
            End Get
            Set(ByVal Value As DataBase)
                m_DB = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal ArticleId As Integer)
            m_DB = database
            m_ArticleId = ArticleId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Article WHERE ArticleId = " & DB.Quote(ArticleId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Email.SendError("ToError500", "Article.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
        End Sub


        Protected Overridable Sub Load(ByVal r As SqlDataReader)
            Try
                m_Title = Convert.ToString(r.Item("Title"))
                If IsDBNull(r.Item("OtherTitle")) Then
                    m_OtherTitle = Nothing
                Else
                    m_OtherTitle = Convert.ToString(r.Item("OtherTitle"))
                End If
                If IsDBNull(r.Item("ShortAbstract")) Then
                    m_ShortAbstract = Nothing
                Else
                    m_ShortAbstract = Convert.ToString(r.Item("ShortAbstract"))
                End If
                m_IsActive = Convert.ToBoolean(r.Item("IsActive"))
                If IsDBNull(r.Item("PostDate")) Then
                    m_PostDate = Nothing
                Else
                    m_PostDate = Convert.ToDateTime(r.Item("PostDate"))
                End If
                If IsDBNull(r.Item("PastNewsDate")) Then
                    m_PastNewsDate = Nothing
                Else
                    m_PastNewsDate = Convert.ToDateTime(r.Item("PastNewsDate"))
                End If
                If IsDBNull(r.Item("Image")) Then
                    m_Image = Nothing
                Else
                    m_Image = Convert.ToString(r.Item("Image"))
                End If
                If IsDBNull(r.Item("Link")) Then
                    m_Link = Nothing
                Else
                    m_Link = Convert.ToString(r.Item("Link"))
                End If
                m_HasFullVersion = Convert.ToBoolean(r.Item("HasFullVersion"))
                If IsDBNull(r.Item("ShortVersion")) Then
                    m_ShortVersion = Nothing
                Else
                    m_ShortVersion = Convert.ToString(r.Item("ShortVersion"))
                End If
                If IsDBNull(r.Item("FullVersion")) Then
                    m_FullVersion = Nothing
                Else
                    m_FullVersion = Convert.ToString(r.Item("FullVersion"))
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub 'Load

        Public Overridable Function Insert() As Integer
            Dim SQL As String

            SQL = " INSERT INTO Article (" _
             & " Title" _
             & ",OtherTitle" _
             & ",ShortAbstract" _
             & ",IsActive" _
             & ",PostDate" _
             & ",PastNewsDate" _
             & ",Image" _
             & ",Link" _
             & ",HasFullVersion" _
             & ",ShortVersion" _
             & ",FullVersion" _
             & ") VALUES (" _
             & m_DB.Quote(Title) _
             & "," & m_DB.Quote(OtherTitle) _
             & "," & m_DB.Quote(ShortAbstract) _
             & "," & CInt(IsActive) _
             & "," & m_DB.Quote(PostDate) _
             & "," & m_DB.NullQuote(PastNewsDate) _
             & "," & m_DB.Quote(Image) _
             & "," & m_DB.Quote(Link) _
             & "," & CInt(HasFullVersion) _
             & "," & m_DB.Quote(ShortVersion) _
             & "," & m_DB.Quote(FullVersion) _
             & ")"

            ArticleId = m_DB.InsertSQL(SQL)

            Return ArticleId
        End Function

        Public Overridable Sub Update()
            Dim SQL As String

            SQL = " UPDATE Article SET " _
             & " Title = " & m_DB.Quote(Title) _
             & ",OtherTitle = " & m_DB.Quote(OtherTitle) _
             & ",ShortAbstract = " & m_DB.Quote(ShortAbstract) _
             & ",IsActive = " & CInt(IsActive) _
             & ",PostDate = " & m_DB.Quote(PostDate) _
             & ",PastNewsDate = " & m_DB.NullQuote(PastNewsDate) _
             & ",Image = " & m_DB.Quote(Image) _
             & ",Link = " & m_DB.Quote(Link) _
             & ",HasFullVersion = " & CInt(HasFullVersion) _
             & ",ShortVersion = " & m_DB.Quote(ShortVersion) _
             & ",FullVersion = " & m_DB.Quote(FullVersion) _
             & " WHERE ArticleId = " & m_DB.Quote(ArticleId)

            m_DB.ExecuteSQL(SQL)

        End Sub 'Update

        Public Sub Remove()
            Dim SQL As String

            SQL = "DELETE FROM Article WHERE ArticleId = " & m_DB.Quote(ArticleId)
            m_DB.ExecuteSQL(SQL)
        End Sub 'Remove
    End Class

    Public Class ArticleCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal Article As ArticleRow)
            Me.List.Add(Article)
        End Sub

        Public Function Contains(ByVal Article As ArticleRow) As Boolean
            Return Me.List.Contains(Article)
        End Function

        Public Function IndexOf(ByVal Article As ArticleRow) As Integer
            Return Me.List.IndexOf(Article)
        End Function

        Public Sub Insert(ByVal Index As Integer, ByVal Article As ArticleRow)
            Me.List.Insert(Index, Article)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ArticleRow
            Get
                Return CType(Me.List.Item(Index), ArticleRow)
            End Get

            Set(ByVal Value As ArticleRow)
                Me.List(Index) = Value
            End Set
        End Property

        Public Sub Remove(ByVal Article As ArticleRow)
            Me.List.Remove(Article)
        End Sub
        Public ReadOnly Property Clone() As ArticleCollection
            Get
                If Me.List Is Nothing Then
                    Return Nothing
                End If
                Dim result As New ArticleCollection
                For Each obj As ArticleRow In Me.List
                    result.Add(CloneObject.Clone(obj))
                Next
                Return result
            End Get
        End Property
    End Class

End Namespace



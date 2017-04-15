Imports System.Data.SqlClient
Imports Components

Namespace DataLayer
    Public Class LikeReviewRow
        Inherits LikeReviewRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal ReviewId As Integer, ByVal MemberId As Integer)
            MyBase.New(database, ReviewId, MemberId)
        End Sub 'New

        Public Shared Function GetRow(ByVal _Database As Database, ByVal ReviewId As Integer, ByVal MemberId As Integer) As LikeReviewRow
            Dim row As LikeReviewRow
            row = New LikeReviewRow(_Database, ReviewId, MemberId)
            row.Load()
            Return row
        End Function
        Public Shared Function UpdateLike(ByVal _Database As Database, ByVal ReviewId As Integer, ByVal MemberId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_LikeReview_UpdateLike"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("ReviewId", SqlDbType.Int, 0, ReviewId))
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function UpdateUnLike(ByVal _Database As Database, ByVal ReviewId As Integer, ByVal MemberId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_LikeReview_UpdateUnLike"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("ReviewId", SqlDbType.Int, 0, ReviewId))
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
    End Class
    Public MustInherit Class LikeReviewRowBase
        Private m_DB As Database
        Private m_ReviewId As Integer
        Private m_MemberId As Integer
        Private m_Like As Boolean
        Private m_CreatedDate As DateTime
        Private m_ModifiedDate As DateTime

        Public Property ReviewId() As Integer
            Get
                Return m_ReviewId
            End Get
            Set(ByVal value As Integer)
                m_ReviewId = value
            End Set
        End Property

        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(ByVal value As Integer)
                m_MemberId = value
            End Set
        End Property

        Public Property [Like]() As Boolean
            Get
                Return m_Like
            End Get
            Set(ByVal value As Boolean)
                m_Like = value
            End Set
        End Property

        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(ByVal value As DateTime)
                m_CreatedDate = value
            End Set
        End Property

        Public Property ModifiedDate() As DateTime
            Get
                Return m_ModifiedDate
            End Get
            Set(ByVal value As DateTime)
                m_ModifiedDate = value
            End Set
        End Property

        Public Property DB() As Database
            Get
                Return m_DB
            End Get
            Set(ByVal value As Database)
                m_DB = value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal DB As Database)
            m_DB = DB
        End Sub 'New

        Public Sub New(ByVal DB As Database, ByVal ReviewId As Integer, ByVal MemberId As Integer)
            m_DB = DB
            m_ReviewId = ReviewId
            m_MemberId = MemberId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM LikeReview WHERE ReviewId = " & m_DB.Number(ReviewId) & " MemberId = " & m_DB.Number(MemberId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("LikeReview.vb", "Load", ex)
            End Try
        End Sub
        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("Reviewid"))) Then
                        m_ReviewId = Convert.ToInt32(reader("Reviewid"))
                    Else
                        m_ReviewId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MemberId"))) Then
                        m_MemberId = Convert.ToInt32(reader("MemberId"))
                    Else
                        m_MemberId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Like"))) Then
                        m_Like = Convert.ToBoolean(reader("Like"))
                    Else
                        m_Like = False
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                        m_CreatedDate = Convert.ToDateTime(reader("CreatedDate"))
                    Else
                        m_CreatedDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))) Then
                        m_ModifiedDate = Convert.ToDateTime(reader("ModifiedDate"))
                    Else
                        m_ModifiedDate = Nothing
                    End If

                End If

            Catch ex As Exception
                Throw ex
            End Try
        End Sub
    End Class

    Public Class LikeReviewCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As LikeReviewRowBase)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As LikeReviewRowBase
            Get
                Return CType(Me.List.Item(Index), LikeReviewRowBase)
            End Get

            Set(ByVal Value As LikeReviewRowBase)
                Me.List(Index) = Value
            End Set
        End Property
    End Class

End Namespace


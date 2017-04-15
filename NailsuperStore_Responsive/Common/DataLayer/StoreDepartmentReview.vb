Imports Components
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Utility
Imports Database

Namespace DataLayer

    Public Class StoreDepartmentReviewRow
        Inherits StoreDepartmentReviewBase
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub

        Public Sub New(ByVal database As Database, ByVal DepartmentId As Integer)
            MyBase.New(database, DepartmentId)
        End Sub

        Public Shared Function ListByDepartmentId(ByVal DepartmentId As Integer) As List(Of StoreDepartmentReviewRow)
            Dim result As New List(Of StoreDepartmentReviewRow)
            Dim dr As SqlDataReader = Nothing
            Dim sp As String = "sp_StoreDepartmentReview_ListByDepartmentId"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Try
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
                dr = db.ExecuteReader(cmd)

                If dr.HasRows Then
                    result = mapList(Of StoreDepartmentReviewRow)(dr)
                End If

            Catch ex As Exception
                Email.SendError("ToError500", "ListByDepartmentId", ex.ToString())
            Finally
                Core.CloseReader(dr)
            End Try

            Return result
        End Function

        Public Shared Function Insert(ByVal item As StoreDepartmentReviewRow) As Boolean
            Dim result As Integer = 0
            Dim sp As String = "sp_StoreDepartmentReview_Insert"
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "DepartmentId", DbType.Int32, item.DepartmentId)
            db.AddInParameter(cmd, "ItemReviewId", DbType.Int32, item.ItemReviewId)
            db.AddInParameter(cmd, "CreatedDate", DbType.DateTime, item.CreatedDate)
            result = db.ExecuteNonQuery(cmd)
            If result > 0 Then
                CacheUtils.RemoveCacheItemWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function Delete(ByVal DepartmentId As Integer, ByVal ItemReviewId As Integer) As Boolean
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_StoreDepartmentReview_Delete"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
            db.AddInParameter(cmd, "ItemReviewId", DbType.Int32, ItemReviewId)
            Dim result As Integer = db.ExecuteNonQuery(cmd)
            If result > 0 Then
                CacheUtils.RemoveCacheItemWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function

        Public Shared Function ChangeArrange(ByVal DepartmentId As Integer, ByVal ItemReviewId As Integer, ByVal IsUp As Boolean) As Boolean
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim sp As String = "sp_StoreDepartmentReview_ChangeArrange"
            Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
            db.AddInParameter(cmd, "DepartmentId", DbType.Int32, DepartmentId)
            db.AddInParameter(cmd, "ItemReviewId", DbType.Int32, ItemReviewId)
            db.AddInParameter(cmd, "IsUp", DbType.Boolean, IsUp)
            Dim result As Integer = db.ExecuteNonQuery(cmd)
            If result > 0 Then
                CacheUtils.RemoveCacheItemWithPrefix(cachePrefixKey)
                Return True
            End If
            Return False
        End Function
    End Class

    Public MustInherit Class StoreDepartmentReviewBase
        Private m_DB As Database
        Private m_DepartmentId As Integer = Nothing
        Private m_ItemReviewId As Integer = Nothing
        Private m_Arrange As Integer = Nothing
        Private m_CreatedDate As DateTime = Nothing
        Private m_ModifiedDate As DateTime = Nothing

        ''property use in admin
        Private m_ItemId As Integer = Nothing
        Private m_MemberId As Integer = Nothing
        Private m_NumStars As Integer = Nothing
        Private m_ReviewTitle As String = Nothing
        Private m_ReviewerName As String = Nothing
        Private m_IsActive As Boolean = Nothing
        Private m_ItemName As String = Nothing
        Private m_URLCode As String = Nothing
        Private m_DateAdded As DateTime = Nothing
        Private m_Comment As String = Nothing

        Public Shared cachePrefixKey As String = "StoreDepartmentReview_"

        Public Property DB() As Database
            Get
                Return m_DB
            End Get
            Set(value As Database)
                m_DB = value
            End Set
        End Property
        Public Property DepartmentId() As Integer
            Get
                Return m_DepartmentId
            End Get
            Set(value As Integer)
                m_DepartmentId = value
            End Set
        End Property
        Public Property ItemReviewId() As Integer
            Get
                Return m_ItemReviewId
            End Get
            Set(value As Integer)
                m_ItemReviewId = value
            End Set
        End Property
        Public Property Arrange() As Integer
            Get
                Return m_Arrange
            End Get
            Set(value As Integer)
                m_Arrange = value
            End Set
        End Property
        Public Property CreatedDate() As DateTime
            Get
                Return m_CreatedDate
            End Get
            Set(value As DateTime)
                m_CreatedDate = value
            End Set
        End Property
        Public Property ModifiedDate() As DateTime
            Get
                Return m_ModifiedDate
            End Get
            Set(value As DateTime)
                m_ModifiedDate = value
            End Set
        End Property
        Public Property ItemId() As Integer
            Get
                Return m_ItemId
            End Get
            Set(value As Integer)
                m_ItemId = value
            End Set
        End Property
        Public Property MemberId() As Integer
            Get
                Return m_MemberId
            End Get
            Set(value As Integer)
                m_MemberId = value
            End Set
        End Property
        Public Property NumStars() As Integer
            Get
                Return m_NumStars
            End Get
            Set(value As Integer)
                m_NumStars = value
            End Set
        End Property
        Public Property ReviewTitle() As String
            Get
                Return m_ReviewTitle
            End Get
            Set(value As String)
                m_ReviewTitle = value
            End Set
        End Property
        Public Property ReviewerName() As String
            Get
                Return m_ReviewerName
            End Get
            Set(value As String)
                m_ReviewerName = value
            End Set
        End Property
        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(value As Boolean)
                m_IsActive = value
            End Set
        End Property
        Public Property Comment() As String
            Get
                Return m_Comment
            End Get
            Set(value As String)
                m_Comment = value
            End Set
        End Property
        Public Property ItemName() As String
            Get
                Return m_ItemName
            End Get
            Set(value As String)
                m_ItemName = value
            End Set
        End Property
        Public Property URLCode() As String
            Get
                Return m_URLCode
            End Get
            Set(value As String)
                m_URLCode = value
            End Set
        End Property
        Public Property DateAdded() As DateTime
            Get
                Return m_DateAdded
            End Get
            Set(value As DateTime)
                m_DateAdded = value
            End Set
        End Property
        Public Sub New()
        End Sub

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub

        Public Sub New(ByVal database As Database, ByVal DepartmentId As Integer)
            m_DB = database
            m_DepartmentId = DepartmentId
        End Sub

    End Class

    Public Class StoreDepartmentReviewCollection
        Inherits CollectionBase
        Public Sub New()
        End Sub

        Public Sub Add(ByVal StoreDepartmentReview As StoreDepartmentReviewRow)
            Me.List.Add(StoreDepartmentReview)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As StoreDepartmentReviewRow
            Get
                Return CType(Me.List.Item(Index), StoreDepartmentReviewRow)
            End Get
            Set(ByVal value As StoreDepartmentReviewRow)
                Me.List(Index) = value
            End Set
        End Property

        Public Sub Insert(ByVal index As Integer, ByVal StoreDepartmentReview As StoreDepartmentReviewRow)
            Me.List.Insert(index, StoreDepartmentReview)
        End Sub
    End Class
End Namespace
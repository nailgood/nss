Imports System.Data.SqlClient
Imports Components
Imports Microsoft.Practices
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Utility.Common

Namespace DataLayer
    Public Class ReviewRow
        Inherits ReviewRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New
        Public Sub New(ByVal database As Database, ByVal ReviewId As Integer)
            MyBase.New(database, ReviewId)
        End Sub 'New

        Public Shared Function GetRow(ByVal _Database As Database, ByVal ReviewId As Integer) As ReviewRow
            Dim row As ReviewRow
            row = New ReviewRow(_Database, ReviewId)
            row.Load()
            Return row
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As ReviewRow) As Integer
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Review_insert"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("Type", SqlDbType.Int, 0, data.Type))
                cmd.Parameters.Add(_Database.InParam("ItemReviewId", SqlDbType.Int, 0, data.ItemReviewId))
                cmd.Parameters.Add(_Database.InParam("ParentReviewId", SqlDbType.Int, 0, data.ParentReviewId))
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, data.MemberId))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("IsFacebook", SqlDbType.Bit, 0, data.IsFacebook))
                cmd.Parameters.Add(_Database.InParam("Comment", SqlDbType.NVarChar, 0, data.Comment))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            Return result
        End Function
        Public Shared Function Update(ByVal _Database As Database, ByVal data As ReviewRow) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Review_Update"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("ReviewId", SqlDbType.Int, 0, data.ReviewId))
                cmd.Parameters.Add(_Database.InParam("Type", SqlDbType.Int, 0, data.Type))
                cmd.Parameters.Add(_Database.InParam("ItemReviewId", SqlDbType.Int, 0, data.ItemReviewId))
                cmd.Parameters.Add(_Database.InParam("ParentReviewId", SqlDbType.Int, 0, data.ParentReviewId))
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, data.MemberId))
                cmd.Parameters.Add(_Database.InParam("IsActive", SqlDbType.Bit, 0, data.IsActive))
                cmd.Parameters.Add(_Database.InParam("IsFacebook", SqlDbType.Bit, 0, data.IsFacebook))
                cmd.Parameters.Add(_Database.InParam("Comment", SqlDbType.NVarChar, 0, data.Comment))
                cmd.Parameters.Add(_Database.InParam("ModifiedBy", SqlDbType.NVarChar, 0, data.ModifiedBy))
                cmd.Parameters.Add(_Database.InParam("IsReportSpam", SqlDbType.NVarChar, 0, data.IsReportSpam))
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
        Public Shared Function Delete(ByVal _Database As Database, ByVal ReviewId As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Review_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("ReviewId", SqlDbType.BigInt, 0, ReviewId))
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

        Public Shared Function GetListByItemReviewId(ByVal Type As ReviewType, ByVal ItemReviewId As Integer, ByVal sortField As String, ByVal sortExp As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef TotalRecords As Integer) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Review_GetListByItemReviewId")
                db.AddInParameter(cmd, "Type", DbType.Int32, Type)
                db.AddInParameter(cmd, "ItemReviewId", DbType.Int32, ItemReviewId)
                db.AddInParameter(cmd, "OrderBy", DbType.String, sortField)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pageSize)
                db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 1)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                If result IsNot Nothing AndAlso result.Tables.Count > 0 Then
                    TotalRecords = CInt(cmd.Parameters("@TotalRecords").Value)
                    Return result.Tables(0)
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                Components.Email.SendError("ToError500", "GetListByItemReviewId(ByVal Type=" & Type.ToString() & " As Integer, ByVal ItemReviewId=" & ItemReviewId & " As Integer, ByVal sortField=" & sortField & " As String, ByVal sortExp=" & sortExp & " As String, ByVal pageIndex=" & pageIndex & " As Integer, ByVal pageSize=" & pageSize & " As Integer, ByRef total As Integer) As DataTable", ex.Message & ",Stack trace:" & ex.StackTrace)
                Return Nothing
            End Try
            Return New DataTable
        End Function
        Public Shared Function GetListChildByParentReviewId(ByVal ParentReviewId As Integer) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Review_GetListChildByParentReviewId")
                db.AddInParameter(cmd, "ParentReviewId", DbType.Int32, ParentReviewId)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                Return result.Tables(0)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "GetListChildByParentReviewId(ByVal ParentReviewId=" & ParentReviewId & " As Integer) As DataTable", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return New DataTable
        End Function
        Public Shared Function GetTotalReviewByItemReviewId(ByVal Type As ReviewType, ByVal ItemReviewId As Integer) As Integer
            Dim result As Integer = 0
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Review_GetTotalReviewByItemReviewId")
                db.AddInParameter(cmd, "Type", DbType.Int32, CInt(Type))
                db.AddInParameter(cmd, "ItemReviewId", DbType.Int32, ItemReviewId)
                result = CInt(db.ExecuteScalar(cmd))
            Catch ex As Exception
                Components.Email.SendError("ToError500", "GetTotalReviewByItemReviewId(ByVal Type=" & Type & " As Integer,ByVal ItemReviewId=" & ItemReviewId & " As Integer) As DataTable", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function

        Public Shared Function GetCustomerByReviewId(ByVal ReviewId As Integer) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Review_GetCustomerByReviewId")
                db.AddInParameter(cmd, "ReviewId", DbType.Int32, ReviewId)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                Return result.Tables(0)
            Catch ex As Exception
                Components.Email.SendError("ToError500", "GetCustomerByReviewId(ByVal ReviewId=" & ReviewId & " As Integer) As DataTable", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return New DataTable
        End Function

    End Class

    Public MustInherit Class ReviewRowBase
        Private m_DB As Database
        Private m_ReviewId As Integer
        Private m_Type As Integer
        Private m_ItemReviewId As Integer
        Private m_parentReviewId As Integer
        Private m_MemberId As Integer
        Private m_IsActive As Boolean
        Private m_Comment As String
        Private m_IsFacebook As Boolean
        Private m_CreatedDate As Date
        Private m_modifiedDate As Date
        Private m_ModifiedBy As String
        Private m_IsReportSpam As Boolean

        Public Property ReviewId() As Integer
            Get
                Return m_ReviewId
            End Get
            Set(ByVal value As Integer)
                m_ReviewId = value
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
        Public Property ItemReviewId() As Integer
            Get
                Return m_ItemReviewId
            End Get
            Set(ByVal value As Integer)
                m_ItemReviewId = value
            End Set
        End Property
        Public Property ParentReviewId() As Integer
            Get
                Return m_parentReviewId
            End Get
            Set(ByVal value As Integer)
                m_parentReviewId = value
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
        Public Property IsActive() As Boolean
            Get
                Return m_IsActive
            End Get
            Set(ByVal value As Boolean)
                m_IsActive = value
            End Set
        End Property
        Public Property Comment() As String
            Get
                Return m_Comment
            End Get
            Set(ByVal value As String)
                m_Comment = value
            End Set
        End Property
        Public Property IsFacebook() As Boolean
            Get
                Return m_IsFacebook
            End Get
            Set(ByVal value As Boolean)
                m_IsFacebook = value
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
                Return m_modifiedDate
            End Get
            Set(ByVal value As DateTime)
                m_modifiedDate = value
            End Set
        End Property
        Public Property ModifiedBy() As String
            Get
                Return m_ModifiedBy
            End Get
            Set(ByVal value As String)
                m_ModifiedBy = value
            End Set
        End Property
        Public Property IsReportSpam() As String
            Get
                Return m_IsReportSpam
            End Get
            Set(ByVal value As String)
                m_IsReportSpam = value
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

        Public Sub New(ByVal DB As Database, ByVal ReviewId As Integer)
            m_DB = DB
            m_ReviewId = ReviewId
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Review WHERE ReviewId = " & m_DB.Number(ReviewId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("Review.vb", "Load", ex)
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
                    If (Not reader.IsDBNull(reader.GetOrdinal("Type"))) Then
                        m_Type = Convert.ToInt32(reader("Type"))
                    Else
                        m_Type = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ItemReviewId"))) Then
                        m_ItemReviewId = Convert.ToInt32(reader("ItemReviewId"))
                    Else
                        m_ItemReviewId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ParentReviewId"))) Then
                        m_parentReviewId = Convert.ToInt32(reader("ParentReviewId"))
                    Else
                        m_parentReviewId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("MemberId"))) Then
                        m_MemberId = Convert.ToInt32(reader("MemberId"))
                    Else
                        m_MemberId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                        m_IsActive = Convert.ToBoolean(reader("IsActive"))
                    Else
                        m_IsActive = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("Comment"))) Then
                        m_Comment = reader("Comment").ToString()
                    Else
                        m_Comment = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsFacebook"))) Then
                        m_IsFacebook = Convert.ToBoolean(reader("IsFacebook"))
                    Else
                        m_IsFacebook = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                        m_CreatedDate = Convert.ToDateTime(reader("CreatedDate"))
                    Else
                        m_CreatedDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))) Then
                        m_modifiedDate = Convert.ToDateTime(reader("ModifiedDate"))
                    Else
                        m_modifiedDate = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("ModifiedBy"))) Then
                        m_ModifiedBy = reader("ModifiedBy").ToString()
                    Else
                        m_ModifiedBy = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsReportSpam"))) Then
                        m_IsReportSpam = Convert.ToBoolean(reader("IsReportSpam"))
                    Else
                        m_IsReportSpam = False
                    End If
                End If

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

    End Class
    Public Class ReviewCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As ReviewRowBase)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ReviewRowBase
            Get
                Return CType(Me.List.Item(Index), ReviewRowBase)
            End Get

            Set(ByVal Value As ReviewRowBase)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace


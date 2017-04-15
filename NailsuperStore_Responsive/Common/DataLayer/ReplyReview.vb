
Imports System
Imports Components
Imports System.Data
Imports System.Collections
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Text
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Utility

Namespace DataLayer
    Public Class ReplyReviewRow
        Inherits ReplyReviewRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New


     
        Public Shared Function GetRowByReviewId(ByVal ReviewId As Integer, ByVal typeReview As Integer) As ReplyReviewRow
            Dim dr As SqlDataReader = Nothing
            Dim result As New ReplyReviewRow
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim sp As String = "sp_ReplyReview_GetRowByReviewId"
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ReviewId", DbType.Int32, ReviewId)
                db.AddInParameter(cmd, "TypeReview", DbType.Int32, typeReview)
                dr = db.ExecuteReader(cmd)
                If dr.Read Then
                    result = GetDataFromReader(dr)
                End If
                Core.CloseReader(dr)
            Catch ex As Exception
                Core.CloseReader(dr)
                Email.SendError("ToError500", "ReplyReviewRow.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function

        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As ReplyReviewRow
            Dim result As New ReplyReviewRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("ReplyReviewId"))) Then
                    result.ReplyReviewId = Convert.ToInt32(reader("ReplyReviewId"))
                Else
                    result.ReplyReviewId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ReviewId"))) Then
                    result.ReviewId = Convert.ToInt32(reader("ReviewId"))
                Else
                    result.ReviewId = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Content"))) Then
                    result.Content = reader("Content").ToString()
                Else
                    result.Content = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("TypeReply"))) Then
                    result.TypeReply = Convert.ToInt32(reader("TypeReply"))
                Else
                    result.TypeReply = 0
                End If


                If (Not reader.IsDBNull(reader.GetOrdinal("TypeReview"))) Then
                    result.TypeReview = Convert.ToInt32(reader("TypeReview"))
                Else
                    result.TypeReview = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CreateDate"))) Then
                    result.CreateDate = Convert.ToDateTime(reader("CreateDate").ToString())
                Else
                    result.CreateDate = ""
                End If
            Catch ex As Exception
                ''  Email.SendError("ToError500", "AdminLogDetail.vb", ex.Message & ",Stack trace:" & ex.StackTrace)
                Throw ex
            End Try
            Return result
        End Function
    
        Public Shared Function Insert(ByVal data As ReplyReviewRow) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_ReplyReview_Insert"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ReviewId", DbType.Int32, data.ReviewId)
                db.AddInParameter(cmd, "Content", DbType.String, data.Content)
                db.AddInParameter(cmd, "TypeReview", DbType.Int32, data.TypeReview)
                db.AddInParameter(cmd, "TypeReply", DbType.Int32, data.TypeReply)
                db.AddInParameter(cmd, "CreateDate", DbType.DateTime, data.CreateDate)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                result = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
        Public Shared Function UpdateContent(ByVal data As ReplyReviewRow) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_ReplyReview_UpdateContent"
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand(sp)
                db.AddInParameter(cmd, "ReplyReviewId", DbType.Int32, data.ReplyReviewId)
                db.AddInParameter(cmd, "Content", DbType.String, data.Content)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                result = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
            Catch ex As Exception

            End Try
            If result = 1 Then
                Return True
            End If
            Return False
        End Function
    End Class


    Public MustInherit Class ReplyReviewRowBase

        Private m_ReplyReviewId As Integer = Nothing
        Private m_ReviewId As Integer = Nothing
        Private m_Content As String = Nothing
        Private m_CreateDate As DateTime = Nothing
        Private m_TypeReview As Integer = Nothing
        Private m_TypeReply As Integer = Nothing
       
        Public Property ReplyReviewId() As Integer
            Get
                Return m_ReplyReviewId
            End Get
            Set(ByVal Value As Integer)
                m_ReplyReviewId = Value
            End Set
        End Property
        Public Property ReviewId() As Integer
            Get
                Return m_ReviewId
            End Get
            Set(ByVal Value As Integer)
                m_ReviewId = Value
            End Set
        End Property
        Public Property Content() As String
            Get
                Return m_Content
            End Get
            Set(ByVal Value As String)
                m_Content = Value
            End Set
        End Property

        Public Property TypeReview() As Integer
            Get
                Return m_TypeReview
            End Get
            Set(ByVal Value As Integer)
                m_TypeReview = Value
            End Set
        End Property

        Public Property TypeReply() As Integer
            Get
                Return m_TypeReply
            End Get
            Set(ByVal Value As Integer)
                m_TypeReply = Value
            End Set
        End Property
        Public Property CreateDate() As DateTime
            Get
                Return m_CreateDate
            End Get
            Set(ByVal Value As DateTime)
                m_CreateDate = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New
    End Class

    Public Class ReplyReviewRowCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As ReplyReviewRow)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As ReplyReviewRow
            Get
                Return CType(Me.List.Item(Index), ReplyReviewRow)
            End Get

            Set(ByVal Value As ReplyReviewRow)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace




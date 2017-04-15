Imports System.Data.SqlClient
Imports Components
Imports System.Data.Common
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class SurveyResultRow
    Inherits SurveyResultRowBase
    Public Sub New()
        MyBase.New()
    End Sub
    Public Sub New(ByVal DB As Database)
        MyBase.New(DB)
    End Sub
    Public Sub New(ByVal DB As Database, ByVal Id As Integer)
        MyBase.New(DB, Id)
    End Sub
    Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer)
        Dim row As SurveyResultRow
        row = New SurveyResultRow(DB, Id)
        row.Load()
        Return row
    End Function
    Public Shared Function Insert(ByVal _Database As Database, ByVal data As SurveyResultRow) As Integer
        Dim result As Integer = 0
        Try
            Dim sp As String = "sp_SurveyResult_Insert"
            Dim cmd As SqlCommand = _Database.CreateCommand(sp)
            cmd.Parameters.Add(_Database.InParam("SurveyId", SqlDbType.Int, 0, data.SurveyId))
            cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, data.MemberId))
            cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, data.OrderId))
            cmd.Parameters.Add(_Database.InParam("CustomerName", SqlDbType.NVarChar, 0, data.CustomerName))
            cmd.Parameters.Add(_Database.InParam("CustomerEmail", SqlDbType.NVarChar, 0, data.CustomerEmail))
            cmd.Parameters.Add(_Database.InParam("Comment", SqlDbType.NVarChar, 0, data.Comment))
            cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
            cmd.ExecuteNonQuery()
            result = CInt(cmd.Parameters("result").Value)
        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function
    Public Shared Function CheckDuplicateEmail(ByVal _Database As Database, ByVal SurveyId As Integer, ByVal Email As String, ByVal MemberId As Integer) As Boolean
        Dim result As Integer = 0
        Try
            Dim sp As String = "sp_SurveyResult_CheckDuplicateEmail"
            Dim cmd As SqlCommand = _Database.CreateCommand(sp)
            cmd.Parameters.Add(_Database.InParam("SurveyId", SqlDbType.Int, 0, SurveyId))
            cmd.Parameters.Add(_Database.InParam("Email", SqlDbType.NVarChar, 0, Email))
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

    Public Shared Function CheckSurveyByOrderId(ByVal _Database As Database, ByVal SurveyId As Integer, ByVal MemberId As Integer, ByVal OrderId As String) As Integer
        Dim result As Integer = 0
        Try
            Dim sp As String = "sp_SurveyResult_CheckSurveyByOrderId"
            Dim cmd As SqlCommand = _Database.CreateCommand(sp)
            cmd.Parameters.Add(_Database.InParam("SurveyId", SqlDbType.Int, 0, SurveyId))
            cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, MemberId))
            cmd.Parameters.Add(_Database.InParam("OrderId", SqlDbType.Int, 0, OrderId))
            cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
            cmd.ExecuteNonQuery()
            result = CInt(cmd.Parameters("result").Value)
        Catch ex As Exception
        End Try
        Return result
    End Function

End Class
Public MustInherit Class SurveyResultRowBase
    Private m_DB As Database
    Private m_Id As Integer = Nothing
    Private m_SurveyId As Integer = Nothing
    Private m_MemberId As Integer = Nothing
    Private m_OrderId As Integer = Nothing
    Private m_CustomerName As String = Nothing
    Private m_CustomerEmail As String = Nothing
    Private m_Comment As String = Nothing
    Private m_CreatedDate As DateTime = Nothing

    Public Property Id() As Integer
        Get
            Return m_Id
        End Get
        Set(ByVal value As Integer)
            m_Id = value
        End Set
    End Property
    Public Property SurveyId() As Integer
        Get
            Return m_SurveyId
        End Get
        Set(ByVal value As Integer)
            m_SurveyId = value
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
    Public Property OrderId() As Integer
        Get
            Return m_OrderId
        End Get
        Set(ByVal value As Integer)
            m_OrderId = value
        End Set
    End Property
    Public Property CustomerName() As String
        Get
            Return m_CustomerName
        End Get
        Set(ByVal value As String)
            m_CustomerName = value
        End Set
    End Property
    Public Property CustomerEmail() As String
        Get
            Return m_CustomerEmail
        End Get
        Set(ByVal value As String)
            m_CustomerEmail = value
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
    Public Property CreatedDate() As DateTime
        Get
            Return m_CreatedDate
        End Get
        Set(ByVal value As DateTime)
            m_CreatedDate = value
        End Set
    End Property

    Public Sub New()
    End Sub

    Public Sub New(ByVal DB As Database)
        m_DB = DB
    End Sub

    Public Sub New(ByVal DB As Database, ByVal Id As Integer)
        m_DB = DB
        m_Id = Id
    End Sub

    Protected Overridable Sub Load()
        Dim r As SqlDataReader = Nothing
        Try
            Dim SQL As String
            SQL = "SELECT * FROM SurveyResult WHERE Id = " & m_DB.Number(Id)
            r = m_DB.GetReader(SQL)
            If Not r Is Nothing Then
                If r.Read Then
                    Me.Load(r)
                End If
            End If
            Core.CloseReader(r)
        Catch ex As Exception
            Core.CloseReader(r)
            Core.LogError("SurveyResult.vb", "Load", ex)
        End Try
    End Sub
    Protected Overridable Sub Load(ByVal reader As SqlDataReader)
        Try
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    m_Id = Convert.ToInt32(reader("Id"))
                Else
                    m_Id = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SurveyId"))) Then
                    m_SurveyId = Convert.ToInt32(reader("SurveyId"))
                Else
                    m_SurveyId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("MemberId"))) Then
                    m_MemberId = Convert.ToInt32(reader("MemberId"))
                Else
                    m_MemberId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("OrderId"))) Then
                    m_OrderId = Convert.ToInt32(reader("OrderId"))
                Else
                    m_OrderId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CustomerName"))) Then
                    m_CustomerName = reader("CustomerName").ToString()
                Else
                    m_CustomerName = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CustomerEmail"))) Then
                    m_CustomerEmail = reader("CustomerEmail").ToString()
                Else
                    m_CustomerEmail = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Comment"))) Then
                    m_Comment = reader("Comment").ToString()
                Else
                    m_Comment = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                    m_CreatedDate = Convert.ToDateTime(reader("CreatedDate"))
                Else
                    m_CreatedDate = DateTime.MinValue
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class

Public Class SurveyResultCollection
    Inherits CollectionBase
    Public Sub New()

    End Sub

    Public Sub Add(ByVal data As SurveyAnswerRow)
        Me.List.Add(data)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As SurveyResultRow
        Get
            Return CType(Me.List.Item(Index), SurveyResultRow)
        End Get

        Set(ByVal Value As SurveyResultRow)
            Me.List(Index) = Value
        End Set
    End Property
End Class

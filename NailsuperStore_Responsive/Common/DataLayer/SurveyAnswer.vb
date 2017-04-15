Imports System.Data.SqlClient
Imports Components
Imports Microsoft.Practices
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class SurveyAnswerRow
    Inherits SurveyAnswerRowBase

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
        Dim row As SurveyAnswerRow
        row = New SurveyAnswerRow(DB, Id)
        row.Load()
        Return row
    End Function
    Public Shared Function GetAnswersByQuestionId(ByVal QuestionId As Integer) As SurveyAnswerCollection
        Dim lstSurveyAnswer As New SurveyAnswerCollection
        Dim dr As SqlDataReader = Nothing
        Try
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_SurveyAnswer_GetAnswerByQuestionId")
            db.AddInParameter(cmd, "QuestionId", DbType.Int32, QuestionId)
            dr = db.ExecuteReader(cmd)
            While dr.Read
                Dim row As SurveyAnswerRow = LoadByDataReader(dr)
                lstSurveyAnswer.Add(row)
            End While
        Catch ex As Exception
        End Try
        Core.CloseReader(dr)
        Return lstSurveyAnswer
    End Function

    Public Shared Function GetlistAnswerSelectedBySurveyResult(ByVal SurveyResultId As Integer, ByVal QuestionId As Integer) As DataTable
        Try
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_SurveyAnswer_GetListAnswerSelectedBySurveyResult")
            db.AddInParameter(cmd, "SurveyResultId", DbType.Int32, SurveyResultId)
            db.AddInParameter(cmd, "QuestionId", DbType.Int32, QuestionId)

            Dim result As DataSet = db.ExecuteDataSet(cmd)
            Return result.Tables(0)
        Catch ex As Exception

        End Try
        Return New DataTable
    End Function

    Protected Shared Function LoadByDataReader(ByVal reader As SqlDataReader) As SurveyAnswerRow
        Dim row As New SurveyAnswerRow
        Try
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    row.Id = Convert.ToInt32(reader("Id"))
                Else
                    row.Id = 0
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Answer"))) Then
                    row.Answer = reader("Answer").ToString()
                Else
                    row.Answer = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("QuestionId"))) Then
                    row.QuestionId = Convert.ToInt32(reader("QuestionId"))
                Else
                    row.QuestionId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("isDefaultSelect"))) Then
                    row.IsDefaultSelect = Convert.ToBoolean(reader("isDefaultSelect"))
                Else
                    row.IsDefaultSelect = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    row.IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    row.IsActive = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                    row.Arrange = Convert.ToInt32(reader("Arrange"))
                Else
                    row.Arrange = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                    row.CreatedDate = Convert.ToDateTime(reader("CreatedDate"))
                Else
                    row.CreatedDate = DateTime.MinValue
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))) Then
                    row.ModifiedDate = Convert.ToDateTime(reader("ModifiedDate"))
                Else
                    row.ModifiedDate = DateTime.MinValue
                End If

            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return row
    End Function
End Class
Public MustInherit Class SurveyAnswerRowBase
    Private m_DB As Database
    Private m_Id As Integer = Nothing
    Private m_Answer As String = Nothing
    Private m_QuestionId As Integer = Nothing
    Private m_IsDefaultSelect As Boolean = False
    Private m_Arrange As Integer = Nothing
    Private m_IsActive As Boolean = False
    Private m_CreatedDate As DateTime = Nothing
    Private m_ModifiedDate As DateTime = Nothing

    Public Property Id() As Integer
        Get
            Return m_Id
        End Get
        Set(ByVal value As Integer)
            m_Id = value
        End Set
    End Property

    Public Property Answer() As String
        Get
            Return m_Answer
        End Get
        Set(ByVal value As String)
            m_Answer = value
        End Set
    End Property

    Public Property QuestionId() As Integer
        Get
            Return m_QuestionId
        End Get
        Set(ByVal value As Integer)
            m_QuestionId = value
        End Set
    End Property

    Public Property IsDefaultSelect() As Boolean
        Get
            Return m_IsDefaultSelect
        End Get
        Set(ByVal value As Boolean)
            m_IsDefaultSelect = value
        End Set
    End Property

    Public Property Arrange() As Integer
        Get
            Return m_Arrange
        End Get
        Set(ByVal value As Integer)
            m_Arrange = value
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
            SQL = "SELECT * FROM SurveyAnswer WHERE Id = " & m_DB.Number(Id)
            r = m_DB.GetReader(SQL)
            If Not r Is Nothing Then
                If r.Read Then
                    Me.Load(r)
                End If
            End If
            Core.CloseReader(r)
        Catch ex As Exception
            Core.CloseReader(r)
            Core.LogError("SurveyAnswer.vb", "Load", ex)
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

                If (Not reader.IsDBNull(reader.GetOrdinal("Answer"))) Then
                    m_Answer = reader("Answer").ToString()
                Else
                    m_Answer = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("QuestionId"))) Then
                    m_QuestionId = Convert.ToInt32(reader("QuestionId"))
                Else
                    m_QuestionId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("isDefaultSelect"))) Then
                    m_IsDefaultSelect = Convert.ToBoolean(reader("isDefaultSelect"))
                Else
                    m_IsDefaultSelect = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = False
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Arrange"))) Then
                    m_Arrange = Convert.ToInt32(reader("Arrange"))
                Else
                    m_Arrange = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("CreatedDate"))) Then
                    m_CreatedDate = Convert.ToDateTime(reader("CreatedDate"))
                Else
                    m_CreatedDate = DateTime.MinValue
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("ModifiedDate"))) Then
                    m_ModifiedDate = Convert.ToDateTime(reader("ModifiedDate"))
                Else
                    m_ModifiedDate = DateTime.MinValue
                End If

            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
Public Class SurveyAnswerCollection
    Inherits CollectionBase
    Public Sub New()

    End Sub

    Public Sub Add(ByVal data As SurveyAnswerRow)
        Me.List.Add(data)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As SurveyAnswerRow
        Get
            Return CType(Me.List.Item(Index), SurveyAnswerRow)
        End Get

        Set(ByVal Value As SurveyAnswerRow)
            Me.List(Index) = Value
        End Set
    End Property
End Class
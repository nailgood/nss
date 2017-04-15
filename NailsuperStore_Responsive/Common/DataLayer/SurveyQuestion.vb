Imports System.Data.SqlClient
Imports Components
Imports Microsoft.Practices
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class SurveyQuestionRow
    Inherits SurveyQuestionRowBase

    Public Sub New()
        MyBase.New()
    End Sub 'New

    Public Sub New(ByVal DB As Database)
        MyBase.New(DB)
    End Sub 'New

    Public Sub New(ByVal DB As Database, ByVal Id As Integer)
        MyBase.New(DB, Id)
    End Sub 'New

    Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer)
        Dim row As SurveyQuestionRow
        row = New SurveyQuestionRow(DB, Id)
        row.Load()
        Return row
    End Function

    Public Shared Function GetQuestionBySurveyId(ByVal SurveyId As Integer) As SurveyQuestionCollection
        Dim lstSurveyQuestion As New SurveyQuestionCollection
        Dim dr As SqlDataReader = Nothing
        Try
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_SurveyQuestion_GetQuestionBySurveyId")
            db.AddInParameter(cmd, "SurveyId", DbType.Int32, SurveyId)
            dr = db.ExecuteReader(cmd)
            While dr.Read
                Dim row As SurveyQuestionRow = LoadByDataReader(dr)
                lstSurveyQuestion.Add(row)
            End While
        Catch ex As Exception
        End Try
        Core.CloseReader(dr)
        Return lstSurveyQuestion
    End Function

    Protected Shared Function LoadByDataReader(ByVal reader As SqlDataReader) As SurveyQuestionRow
        Dim row As New SurveyQuestionRow
        Try
            If (Not reader Is Nothing And Not reader.IsClosed) Then
                If (Not reader.IsDBNull(reader.GetOrdinal("Id"))) Then
                    row.Id = Convert.ToInt32(reader("Id"))
                Else
                    row.Id = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("SurveyId"))) Then
                    row.SurveyId = Convert.ToInt32(reader("SurveyId"))
                Else
                    row.SurveyId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Question"))) Then
                    row.Question = reader("Question").ToString()
                Else
                    row.Question = ""
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
                If (Not reader.IsDBNull(reader.GetOrdinal("IsShowNote"))) Then
                    row.IsShowNote = Convert.ToBoolean(reader("IsShowNote"))
                Else
                    row.IsShowNote = False
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return row
    End Function

End Class

Public MustInherit Class SurveyQuestionRowBase
    Private m_DB As Database
    Private m_Id As Integer = Nothing
    Private m_SurveyId As Integer = Nothing
    Private m_Question As String = Nothing
    Private m_IsActive As Boolean = False
    Private m_Arrange As Integer = Nothing
    Private m_CreatedDate As DateTime = Nothing
    Private m_ModifiedDate As DateTime = Nothing
    Private m_IsShowNote As Boolean = False

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

    Public Property Question() As String
        Get
            Return m_Question
        End Get
        Set(ByVal value As String)
            m_Question = value
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

    Public Property Arrange() As Integer
        Get
            Return m_Arrange
        End Get
        Set(ByVal value As Integer)
            m_Arrange = value
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
    Public Property IsShowNote() As Boolean
        Get
            Return m_IsShowNote
        End Get
        Set(ByVal value As Boolean)
            m_IsShowNote = value
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
            SQL = "SELECT * FROM SurveyQuestion WHERE Id = " & m_DB.Number(Id)
            r = m_DB.GetReader(SQL)
            If Not r Is Nothing Then
                If r.Read Then
                    Me.Load(r)
                End If
            End If
            Core.CloseReader(r)
        Catch ex As Exception
            Core.CloseReader(r)
            Core.LogError("SurveyQuestion.vb", "Load", ex)
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
                If (Not reader.IsDBNull(reader.GetOrdinal("Question"))) Then
                    m_Question = reader("Question").ToString()
                Else
                    m_Question = ""
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
                If (Not reader.IsDBNull(reader.GetOrdinal("IsShowNote"))) Then
                    m_IsShowNote = Convert.ToBoolean(reader("IsShowNote"))
                Else
                    m_IsShowNote = False
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class

Public Class SurveyQuestionCollection
    Inherits CollectionBase
    Public Sub New()

    End Sub

    Public Sub Add(ByVal data As SurveyQuestionRow)
        Me.List.Add(data)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As SurveyQuestionRow
        Get
            Return CType(Me.List.Item(Index), SurveyQuestionRow)
        End Get

        Set(ByVal Value As SurveyQuestionRow)
            Me.List(Index) = Value
        End Set
    End Property
End Class

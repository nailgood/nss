Imports System.Data.SqlClient
Imports Components
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class SurveyResultDetailRow
    Inherits SurveyResultDetailRowBase
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
        Dim row As SurveyResultDetailRow
        row = New SurveyResultDetailRow(DB, Id)
        row.Load()
        Return row
    End Function
    Public Shared Function Insert(ByVal _Database As Database, ByVal data As SurveyResultDetailRow) As Integer
        Dim result As Integer = 0
        Try
            Dim sp As String = "sp_SurveyResultDetail_Insert"
            Dim cmd As SqlCommand = _Database.CreateCommand(sp)
            cmd.Parameters.Add(_Database.InParam("SurveyResultId", SqlDbType.Int, 0, data.SurveyResultId))
            cmd.Parameters.Add(_Database.InParam("QuestionId", SqlDbType.Int, 0, data.QuestionId))
            cmd.Parameters.Add(_Database.InParam("AnswerId", SqlDbType.Int, 0, data.AnswerId))
            cmd.Parameters.Add(_Database.InParam("Note", SqlDbType.NVarChar, 0, data.Note))
            cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
            cmd.ExecuteNonQuery()
            result = CInt(cmd.Parameters("result").Value)
        Catch ex As Exception
            Throw ex
        End Try
        Return result
    End Function
    Public Shared Function GetQuestionNoteBySurveyResultId(ByVal SurveyResultId As Integer, ByVal QuestionId As Integer) As String
        If SurveyResultId < 1 Then
            Return Nothing
        End If
        Dim result As String = ""
        Dim sql As String = "Select Note from SurveyResultDetail where SurveyResultId=" & SurveyResultId & " and QuestionId=" & QuestionId
        Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
        Dim reader As SqlDataReader = Nothing
        Try
            reader = db.ExecuteReader(CommandType.Text, sql)
            If reader.Read Then
                result = reader.GetValue(0).ToString()
            End If
            Core.CloseReader(reader)
        Catch ex As Exception
            Core.CloseReader(reader)
        End Try
        Return result
    End Function
End Class

Public MustInherit Class SurveyResultDetailRowBase
    Private m_DB As Database
    Private m_Id As Integer = Nothing
    Private m_SurveyResultId As Integer = Nothing
    Private m_QuestionId As Integer = Nothing
    Private m_AnswerId As Integer = Nothing
    Private m_Note As String = Nothing

    Public Property Id() As Integer
        Get
            Return m_Id
        End Get
        Set(ByVal value As Integer)
            m_Id = value
        End Set
    End Property
    Public Property SurveyResultId() As Integer
        Get
            Return m_SurveyResultId
        End Get
        Set(ByVal value As Integer)
            m_SurveyResultId = value
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
    Public Property AnswerId() As Integer
        Get
            Return m_AnswerId
        End Get
        Set(ByVal value As Integer)
            m_AnswerId = value
        End Set
    End Property
    Public Property Note() As String
        Get
            Return m_Note
        End Get
        Set(ByVal value As String)
            m_Note = value
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
            SQL = "SELECT * FROM SurveyResultDetail WHERE Id = " & m_DB.Number(Id)
            r = m_DB.GetReader(SQL)
            If Not r Is Nothing Then
                If r.Read Then
                    Me.Load(r)
                End If
            End If
            Core.CloseReader(r)
        Catch ex As Exception
            Core.CloseReader(r)
            Core.LogError("SurveyResultDetail.vb", "Load", ex)
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
                If (Not reader.IsDBNull(reader.GetOrdinal("SurveyResultId"))) Then
                    m_SurveyResultId = Convert.ToInt32(reader("SurveyResultId"))
                Else
                    m_SurveyResultId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("QuestionId"))) Then
                    m_QuestionId = Convert.ToInt32(reader("QuestionId"))
                Else
                    m_QuestionId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("AnswerId"))) Then
                    m_AnswerId = Convert.ToInt32(reader("AnswerId"))
                Else
                    m_AnswerId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("Note"))) Then
                    m_Note = reader("Note").ToString()
                Else
                    m_Note = ""
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


End Class

Public Class SurveyResultDetailCollection
    Inherits CollectionBase
    Public Sub New()

    End Sub

    Public Sub Add(ByVal data As SurveyResultDetailRow)
        Me.List.Add(data)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As SurveyResultDetailRow
        Get
            Return CType(Me.List.Item(Index), SurveyResultDetailRow)
        End Get

        Set(ByVal Value As SurveyResultDetailRow)
            Me.List(Index) = Value
        End Set
    End Property
End Class
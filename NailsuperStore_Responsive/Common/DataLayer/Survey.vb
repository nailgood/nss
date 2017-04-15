Imports System.Data.SqlClient
Imports Components
Imports Utility
Imports Microsoft.Practices
Imports System.Data.Common
Imports Microsoft.Practices.EnterpriseLibrary.Data

Public Class SurveyRow
    Inherits SurveyRowBase

    Public Sub New()
        MyBase.New()
    End Sub 'New

    Public Sub New(ByVal DB As Database)
        MyBase.New(DB)
    End Sub 'New

    Public Sub New(ByVal DB As Database, ByVal Id As Integer)
        MyBase.New(DB, Id)
    End Sub 'New
    Public Sub New(ByVal DB As Database, ByVal Code As String)
        MyBase.New(DB, Code)
    End Sub 'New

    'Shared function to get one row
    Public Shared Function GetRow(ByVal DB As Database, ByVal Id As Integer) As SurveyRow
        Dim row As SurveyRow
        'Dim key As String = String.Format(cachePrefixKey & "GetRow_{0}", Id)
        'row = CType(CacheUtils.GetCache(key), SurveyRow)
        'If Not row Is Nothing Then
        '    Return row
        'End If
        row = New SurveyRow(DB, Id)
        row.Load()
        'CacheUtils.SetCache(key, row)
        Return row
    End Function

    Public Shared Function GetRowByCode(ByVal DB As Database, ByVal Code As String) As SurveyRow
        Dim row As SurveyRow
        row = New SurveyRow(DB, Code)
        row.LoadByCode()
        Return row
    End Function
End Class

Public MustInherit Class SurveyRowBase
    Private m_DB As Database
    Private m_Id As Integer = Nothing
    Private m_Code As String = Nothing
    Private m_Name As String = Nothing
    Private m_IsActive As Boolean = False
    Private m_CreatedDate As DateTime = Nothing
    Private m_ModifiedDate As DateTime = Nothing
    Private m_Description As String = Nothing
    Private m_IsComment As Boolean = False

    Public Shared cachePrefixKey As String = "Survey_"

    Public Property Id() As Integer
        Get
            Return m_Id
        End Get
        Set(ByVal value As Integer)
            m_Id = value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return m_Code
        End Get
        Set(ByVal value As String)
            m_Code = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return m_Name
        End Get
        Set(ByVal value As String)
            m_Name = value
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

    Public Property Description() As String
        Get
            Return m_Description
        End Get
        Set(ByVal value As String)
            m_Description = value
        End Set
    End Property
    Public Property IsComment() As Boolean
        Get
            Return m_IsComment
        End Get
        Set(ByVal value As Boolean)
            m_IsComment = value
        End Set
    End Property

    Public Sub New()
    End Sub 'New

    Public Sub New(ByVal database As Database)
        m_DB = database
    End Sub 'New

    Public Sub New(ByVal database As Database, ByVal Id As Integer)
        m_DB = database
        m_Id = Id
    End Sub 'New

    Public Sub New(ByVal database As Database, ByVal Code As String)
        m_DB = database
        m_Code = Code
    End Sub 'New

    Protected Overridable Sub Load()
        Dim r As SqlDataReader = Nothing
        Try
            Dim SQL As String
            SQL = "SELECT * FROM Survey WHERE Id = " & m_DB.Number(Id)
            r = m_DB.GetReader(SQL)
            If Not r Is Nothing Then
                If r.Read Then
                    Me.Load(r)
                End If
            End If
            Core.CloseReader(r)
        Catch ex As Exception
            Core.CloseReader(r)
            Core.LogError("Survey.vb", "Load", ex)
        End Try
    End Sub
    Protected Overridable Sub LoadByCode()
        Dim r As SqlDataReader = Nothing
        Try
            Dim SQL As String
            SQL = "SELECT * FROM Survey WHERE IsActive = 1 AND Code = " & m_DB.Quote(Code)
            r = m_DB.GetReader(SQL)
            If Not r Is Nothing Then
                If r.Read Then
                    Me.Load(r)
                End If
            End If
            Core.CloseReader(r)
        Catch ex As Exception
            Core.CloseReader(r)
            Core.LogError("Survey.vb", "LoadByCode", ex)
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
                If (Not reader.IsDBNull(reader.GetOrdinal("Code"))) Then
                    m_Code = reader("Code").ToString()
                Else
                    m_Code = ""
                End If

                If (Not reader.IsDBNull(reader.GetOrdinal("Name"))) Then
                    m_Name = reader("Name").ToString()
                Else
                    m_Name = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsActive"))) Then
                    m_IsActive = Convert.ToBoolean(reader("IsActive"))
                Else
                    m_IsActive = False
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
                If (Not reader.IsDBNull(reader.GetOrdinal("Description"))) Then
                    m_Description = reader("Description").ToString()
                Else
                    m_Description = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("IsComment"))) Then
                    m_IsComment = Convert.ToBoolean(reader("IsComment"))
                Else
                    m_IsComment = False
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function GetList() As DataTable
        Try
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Survey_GetList")
            Dim result As DataSet = db.ExecuteDataSet(cmd)
            Return result.Tables(0)
        Catch ex As Exception

        End Try
        Return New DataTable
    End Function
End Class

Public Class SurveyCollection
    Inherits CollectionBase

    Public Sub New()
    End Sub

    Public Sub Add(ByVal data As SurveyRow)
        Me.List.Add(data)
    End Sub

    Default Public Property Item(ByVal Index As Integer) As SurveyRow
        Get
            Return CType(Me.List.Item(Index), SurveyRow)
        End Get

        Set(ByVal Value As SurveyRow)
            Me.List(Index) = Value
        End Set
    End Property
End Class
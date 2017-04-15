Imports System.Data.Common
Imports System.Data.SqlClient
Imports Microsoft.Practices
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Utility

Public Class KeywordFilter
#Region "Properties"
    Public _Id As Integer
    Public Property ID() As Integer
        Get
            Return _Id
        End Get
        Set(ByVal value As Integer)
            _Id = value
        End Set
    End Property

    Private _keywordName As String
    Public Property KeyWordName() As String
        Get
            Return _keywordName
        End Get
        Set(ByVal value As String)
            _keywordName = value
        End Set
    End Property

    Private _filterProperty As String
    Public Property FilterProperty() As String
        Get
            Return _filterProperty
        End Get
        Set(ByVal value As String)
            _filterProperty = value
        End Set
    End Property

    Private _filterValue As String
    Public Property FilterValue() As String
        Get
            Return _filterValue
        End Get
        Set(ByVal value As String)
            _filterValue = value
        End Set
    End Property

    Private _originalKeyword As String = String.Empty
    Public Property OriginalKeyword() As String
        Get
            Return _originalKeyword
        End Get
        Set(ByVal value As String)
            _originalKeyword = value
        End Set
    End Property
    Private _fiterType As String = String.Empty
    Public Property FilterType() As String
        Get
            Return _fiterType
        End Get
        Set(ByVal value As String)
            _fiterType = value
        End Set
    End Property
    Private _keywordId As Integer
    Public Property KeywordId() As Int64
        Get
            Return _keywordId
        End Get
        Set(ByVal value As Int64)
            _keywordId = value
        End Set
    End Property
    Private _originalKwId As Int64
    Public Property OriginalKeywordId() As Int64
        Get
            Return _originalKwId
        End Get
        Set(ByVal value As Int64)
            _originalKwId = value
        End Set
    End Property
#End Region

    Public Shared filterProperties = New String() {"Categories", "Brands", "Collection", "Tones", "Shades"}

    Public Shared Function GetAll(Optional ByVal createNew As Boolean = False) As List(Of KeywordFilter)
        Dim result As List(Of KeywordFilter) = New List(Of KeywordFilter)()

        Dim r As SqlDataReader = Nothing
        Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
        Try
            Dim cmd As SqlCommand
            If Not createNew Then
                cmd = db.GetSqlStringCommand("SELECT c.*, d.KeywordName, d.OriginalKeyword FROM KeywordFilter c inner join (select b.KeywordName, a.KeywordName as OriginalKeyword, a.KeywordId, b.KeywordId as ReplaceKeywordId from Keyword a inner join Keyword b on a.KeywordId = b.ReplaceKeywordId) d on c.KeywordId = d.ReplaceKeywordId and c.OriginalKeywordId = d.KeywordId")
            Else
                cmd = db.GetSqlStringCommand("select a.KeywordName, b.KeywordName as OriginalKeyword, a.KeywordId, b.KeywordId as ReplaceKeywordId from Keyword a inner join Keyword b on b.KeywordId = a.ReplaceKeywordId")
            End If

            r = db.ExecuteReader(cmd)
            If r.HasRows Then
                result = Database.mapList(Of KeywordFilter)(r)
            End If
        Catch ex As Exception
            Components.Email.SendError("ToError500", "KeywordFilter.GetAll", "Exception" & ex.Message & ", Stack trace: " & ex.StackTrace)
        End Try

        Return result
    End Function
    Public Shared Function GetRow(ByVal Id As Integer) As KeywordFilter
        Dim result As KeywordFilter = Nothing

        Dim r As SqlDataReader = Nothing
        Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
        Try
            Dim cmd As SqlCommand = db.GetSqlStringCommand("select a.*, bb.KeywordName, bb.OriginalKeyword from KeywordFilter a inner join (select a.KeywordName, b.KeywordName as OriginalKeyword, a.KeywordId, b.KeywordId as ReplaceKeywordId from Keyword a inner join Keyword b on a.KeywordId = b.ReplaceKeywordId) bb  on a.KeywordId = bb.ReplaceKeywordId and a.OriginalKeywordId = bb.KeywordId WHERE a.Id=" & Id)
            r = db.ExecuteReader(cmd)
            If r.HasRows Then
                result = Database.mapList(Of KeywordFilter)(r)(0)
            End If
        Catch ex As Exception
            Components.Email.SendError("ToError500", "KeywordFilter.GetRow", "Exception" & ex.Message & ", Stack trace:" & ex.StackTrace)
        End Try

        Return result
    End Function

    Public Shared Function Insert(ByVal objKeyword As KeywordFilter) As Integer
        Try

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetSqlStringCommand("INSERT INTO KeywordFilter(KeywordId, FilterProperty, FilterValue, OriginalKeywordId, FilterType) select a.KeywordId, @FilterProperty, @FilterValue, b.KeywordId, @FilterType from Keyword a inner join Keyword b on a.ReplaceKeywordId = b.KeywordId where b.KeywordName = @OriginalKeywordName and a.KeywordName = @KeywordName")
            db.AddInParameter(cmd, "FilterProperty", DbType.String, objKeyword.FilterProperty)
            db.AddInParameter(cmd, "FilterValue", DbType.String, objKeyword.FilterValue)
            db.AddInParameter(cmd, "OriginalKeywordName", DbType.String, objKeyword.OriginalKeyword)
            db.AddInParameter(cmd, "FilterType", DbType.String, objKeyword.FilterType)
            db.AddInParameter(cmd, "KeywordName", DbType.String, objKeyword.KeyWordName)
            db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
            db.ExecuteNonQuery(cmd)
            Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
            Return result
        Catch ex As Exception
            Components.Email.SendError("ToError500", "KeywordFilter >> Insert", "KeywordName=" & objKeyword.KeyWordName & "<br>Exception: " & ex.ToString() + "")
        End Try
        Return -1
    End Function

    Public Shared Sub Update(objkeyword As KeywordFilter)
        Try

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetSqlStringCommand("update KeywordFilter set KeywordId = @KeywordId, FilterProperty = @FilterProperty, FilterValue = @FilterValue where id = @ID")
            db.AddInParameter(cmd, "Id", DbType.Int16, objkeyword.ID)
            db.AddInParameter(cmd, "KeywordId", DbType.String, objkeyword.KeywordId)
            db.AddInParameter(cmd, "FilterProperty", DbType.String, objkeyword.FilterProperty)
            db.AddInParameter(cmd, "FilterValue", DbType.String, objkeyword.FilterValue)
            db.ExecuteNonQuery(cmd)
        Catch ex As Exception
            Components.Email.SendError("ToError500", "KeywordFilter >> Insert", "KeywordName=" & objkeyword.KeyWordName & "<br>Exception: " & ex.ToString() + "")
        End Try
    End Sub
    Public Shared Sub Delete(ByVal kwFilterId As Integer)
        Try

            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As DbCommand = db.GetSqlStringCommand("delete KeywordFilter where id = @ID")
            db.AddInParameter(cmd, "Id", DbType.Int16, kwFilterId)

            db.ExecuteNonQuery(cmd)
        Catch ex As Exception
            Components.Email.SendError("ToError500", "KeywordFilter >> Delete", "Id=" & kwFilterId.ToString() & "<br>Exception: " & ex.ToString() + "")
        End Try
    End Sub

    Public Shared Function BuildQueryString(ByVal kwName As String, Optional ByVal kwId As Int32 = -1) As DataTable
        Dim dt As DataTable = New DataTable()
        Dim r As SqlDataReader = Nothing
        Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
        Try
            Dim cmd As SqlCommand = db.GetStoredProcCommand("sp_KeywordFilter_BuildQueryString")
            db.AddInParameter(cmd, "KeywordName", DbType.String, kwName)
            db.AddInParameter(cmd, "OriginalKeywordId", DbType.Int32, kwId)
            r = db.ExecuteReader(cmd)
            If r.HasRows Then
                dt.Load(r)
            End If
        Catch ex As Exception
            Components.Email.SendError("ToError500", "KeywordFilter.BuildQueryString", "Exception" & ex.Message & ", Stack trace:" & ex.StackTrace)
        End Try

        Return dt
    End Function
End Class

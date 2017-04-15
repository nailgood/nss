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
    Public Class KeywordRow
        Inherits KeywordRowBase

        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            MyBase.New(database)
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal id As Integer)
            MyBase.New(database, id)
        End Sub 'New

        'Shared function to get one row
        Public Shared Function GetRow(ByVal _Database As Database, ByVal id As Int64) As KeywordRow
            Dim row As KeywordRow
            row = New KeywordRow(_Database, id)
            row.Load()
            Return row
        End Function
        Private Shared Function GetDataFromReader(ByVal reader As SqlDataReader) As KeywordRow
            Dim result As New KeywordRow
            Try
                If (Not reader.IsDBNull(reader.GetOrdinal("KeywordId"))) Then
                    result.KeywordId = Convert.ToInt64(reader("KeywordId"))
                Else
                    result.KeywordId = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("KeywordName"))) Then
                    result.KeywordName = reader("KeywordName").ToString()
                Else
                    result.KeywordName = ""
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("TotalAddCart"))) Then
                    result.TotalAddCart = Convert.ToInt64(reader("TotalAddCart"))
                Else
                    result.TotalAddCart = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("TotalDetail"))) Then
                    result.TotalDetail = Convert.ToInt64(reader("TotalDetail"))
                Else
                    result.TotalDetail = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("TotalPoint"))) Then
                    result.TotalPoint = Convert.ToInt64(reader("TotalPoint"))
                Else
                    result.TotalPoint = 0
                End If
                If (Not reader.IsDBNull(reader.GetOrdinal("TotalSearch"))) Then
                    result.TotalSearch = Convert.ToInt64(reader("TotalSearch"))
                Else
                    result.TotalSearch = 0
                End If
            Catch ex As Exception
                Throw ex
            End Try
            Return result
        End Function

        Public Shared Function Delete(ByVal _Database As Database, ByVal Id As Integer) As Boolean
            Dim result As Integer = 0
            Try
                Dim sp As String = "sp_Keyword_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("KeywordId", SqlDbType.BigInt, 0, Id))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
                ''Components.Email.SendError("ToError500", "KeywordRowBase.GetDataReportByDate", "Id=" & Id & "<br>Exception: " & ex.ToString() + "")
                Core.LogError("Keyword.vb", "Delete(Id=" & Id & ")", ex)
            End Try
            Return False
        End Function

        Public Shared Function Insert(ByVal _Database As Database, ByVal data As KeywordRow, ByVal MemberId As Integer, Optional ByVal resultCount As Integer = -1) As Integer
            Dim result As Integer = 0
            Dim cmd As DbCommand
            Try
                If _Database Is Nothing Then
                    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                    cmd = db.GetStoredProcCommand("sp_Keyword_Insert")
                    db.AddInParameter(cmd, "KeywordName", DbType.String, data.KeywordName)
                    db.AddInParameter(cmd, "MemberId", DbType.Int32, MemberId)
                    db.AddInParameter(cmd, "SearchResult", DbType.Int32, resultCount)
                    db.AddParameter(cmd, "result", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                    db.ExecuteNonQuery(cmd)
                    result = Convert.ToInt32(db.GetParameterValue(cmd, "result"))
                    Return result
                End If

                Dim sp As String = "sp_Keyword_Insert"
                cmd = _Database.CreateCommand(sp)
                cmd.Parameters.Add(_Database.InParam("KeywordName", SqlDbType.NVarChar, 0, data.KeywordName))
                cmd.Parameters.Add(_Database.InParam("MemberId", SqlDbType.Int, 0, MemberId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
                Core.LogError("Keyword.vb", "Insert", ex)
            End Try
            Return result
        End Function
        Public Shared Function InsertReplaceKeyword(ByVal mainKeywordId As Integer, ByVal replaceKeywordName As String) As Integer

            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Keyword_InsertReplaceKeyword")
                db.AddInParameter(cmd, "KeywordId", DbType.Int64, mainKeywordId)
                db.AddInParameter(cmd, "ReplaceKeywordName", DbType.String, replaceKeywordName)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                Return result
            Catch ex As Exception
                Core.LogError("Keyword.vb", "InsertReplaceKeyword(mainKeywordId=" & mainKeywordId & ",replaceKeywordName=" & replaceKeywordName & ")", ex)
            End Try
            Return 0
        End Function
        Public Shared Function GetDataReportByDate(ByVal condition As String, ByVal sortField As String, ByVal sortExp As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef toltal As Integer) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Keyword_ReportByDateV1")
                db.AddInParameter(cmd, "Condition", DbType.String, condition)
                db.AddInParameter(cmd, "OrderBy", DbType.String, sortField)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pageSize)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                If Not result Is Nothing Then
                    If result.Tables.Count > 0 Then
                        toltal = result.Tables(0).Rows(0)("Total")
                    End If
                End If
                Return result.Tables(0)
            Catch ex As Exception
                Core.LogError("Keyword.vb", "GetDataReportByDate(condition=" & condition & ",sortField=" & sortField & ",sortExp=" & sortExp & ",pageIndex=" & pageIndex & ",pageSize=" & pageSize & ")", ex)
            End Try
            Return New DataTable
        End Function
        Public Shared Function GetDataReportByListExport(ByVal condition As String, ByVal pageIndex As Integer, ByVal pageSize As Integer) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeyWord_GetListDataKeywordByListExport")
                db.AddInParameter(cmd, "Condition", DbType.String, condition)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pageSize)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                Return result.Tables(0)
            Catch ex As Exception
                Core.LogError("Keyword.vb", "GetDataReportByListExport(condition=" & condition & ",pageIndex=" & pageIndex & ",pageSize=" & pageSize & ")", ex)
            End Try
            Return New DataTable
        End Function

        Public Shared Function GetReplaceKeyword(ByVal keyword As String) As String
            Dim reader As SqlDataReader = Nothing
            Dim result As String = String.Empty
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Keyword_GetReplaceKeyword")
                db.AddInParameter(cmd, "KeywordName", DbType.String, keyword)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                If (reader.Read()) Then
                    result = reader.GetValue(0)
                End If
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Core.LogError("Keyword.vb", "GetReplaceKeyword(KeywordName=" & keyword & ")", ex)
            End Try
            Return result
        End Function
        Public Shared Function GetReplaceKeywordWithFilter(ByVal keyword As String) As DataSet
            Dim reader As SqlDataReader = Nothing
            Dim result As DataSet = New DataSet()
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Keyword_GetReplaceKeyword")
                db.AddInParameter(cmd, "KeywordName", DbType.String, keyword)
                db.AddInParameter(cmd, "GetFilter", DbType.Boolean, True)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                result.Load(reader, LoadOption.PreserveChanges, "1,2".Split(","))
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Core.LogError("Keyword.vb", "GetReplaceKeyword(KeywordName=" & keyword & ")", ex)
            End Try
            Return result
        End Function

        Public Shared Function GetListDataKeywordByDate(ByVal SearchedDate As DateTime, ByVal sortField As String, ByVal sortExp As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef toltal As Integer) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Keyword_GetListDataKeywordByDate")
                db.AddInParameter(cmd, "SearchedDate", DbType.DateTime, SearchedDate)
                db.AddInParameter(cmd, "OrderBy", DbType.String, sortField)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pageSize)
                ''db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 0)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                toltal = result.Tables(1).Rows(0)(0).ToString()
                Return result.Tables(0)
            Catch ex As Exception
                Core.LogError("Keyword.vb", "GetListDataKeywordByDate(SearchedDate=" & SearchedDate.ToLongDateString() & ",sortField=" & sortField & ",sortExp=" & sortExp & ",pageIndex=" & pageIndex & ",pageSize=" & pageSize & ")", ex)
            End Try
            Return New DataTable
        End Function
        Public Shared Function GetListDataKeywordByDateExport(ByVal SearchedDate As DateTime) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Keyword_GetListDataKeywordByDateExport")
                db.AddInParameter(cmd, "SearchedDate", DbType.DateTime, SearchedDate)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                Return result.Tables(0)
            Catch ex As Exception
                Core.LogError("Keyword.vb", "GetListDataKeywordByDateExport(SearchedDate=" & SearchedDate.ToLongDateString() & ")", ex)
            End Try
            Return New DataTable
        End Function
        Public Shared Function GetListKeyword(ByVal KeywordName As String, ByVal SynonymStatus As Integer, ByVal ReplaceKeywordStatus As Integer, ByVal sortField As String, ByVal sortExp As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef total As Integer, Optional ByVal synonymType As String = "") As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Keyword_GetList")
                db.AddInParameter(cmd, "KeywordName", DbType.String, KeywordName)
                db.AddInParameter(cmd, "SynonymStatus", DbType.Int32, SynonymStatus)
                db.AddInParameter(cmd, "ReplaceStatus", DbType.Int32, ReplaceKeywordStatus)
                db.AddInParameter(cmd, "OrderBy", DbType.String, sortField)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pageSize)
                db.AddInParameter(cmd, "SynonymType", DbType.String, synonymType)
                ''db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 0)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                total = result.Tables(1).Rows(0)(0).ToString()
                Return result.Tables(0)
            Catch ex As Exception
                Core.LogError("Keyword.vb", "GetListKeyword(KeywordName=" & KeywordName & ",sortField=" & sortField & ",sortExp=" & sortExp & ",pageIndex=" & pageIndex & ",pageSize=" & pageSize & ")", ex)

            End Try
            Return New DataTable
        End Function
        Public Shared Function GetListKeywordNoResult(ByVal KeywordName As String, ByVal SynonymStatus As Integer, ByVal ReplaceKeywordStatus As Integer, ByVal sortField As String, ByVal sortExp As String, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByRef total As Integer) As DataTable
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Keyword_GetListNoResult")
                db.AddInParameter(cmd, "KeywordName", DbType.String, KeywordName)
                db.AddInParameter(cmd, "SynonymStatus", DbType.Int32, SynonymStatus)
                db.AddInParameter(cmd, "ReplaceStatus", DbType.Int32, ReplaceKeywordStatus)
                db.AddInParameter(cmd, "OrderBy", DbType.String, sortField)
                db.AddInParameter(cmd, "OrderDirection", DbType.String, sortExp)
                db.AddInParameter(cmd, "CurrentPage", DbType.Int32, pageIndex)
                db.AddInParameter(cmd, "PageSize", DbType.Int32, pageSize)
                ''db.AddOutParameter(cmd, "TotalRecords", DbType.Int32, 0)
                Dim result As DataSet = db.ExecuteDataSet(cmd)
                total = result.Tables(1).Rows(0)(0).ToString()
                Return result.Tables(0)
            Catch ex As Exception
                Core.LogError("Keyword.vb", "GetListKeyword(KeywordName=" & KeywordName & ",sortField=" & sortField & ",sortExp=" & sortExp & ",pageIndex=" & pageIndex & ",pageSize=" & pageSize & ")", ex)

            End Try
            Return New DataTable
        End Function

        Private Shared Property KeywordSearchCache As Dictionary(Of Tuple(Of String, Boolean), String)
            Get
                If System.Web.HttpContext.Current.Application("KeywordSearchCache") Is Nothing Then
                    Dim dr As SqlDataReader = Nothing
                    Try
                        Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                        Dim cmd As DbCommand = db.GetSqlStringCommand("select keywordName, isnull(AutoComplete, 0) as AutoComplete, Query from SearchQuery")

                        dr = db.ExecuteReader(cmd)
                        If (dr.HasRows) Then
                            Dim dic As Dictionary(Of Tuple(Of String, Boolean), String) = New Dictionary(Of Tuple(Of String, Boolean), String)
                            While dr.Read()
                                dic.Add(New Tuple(Of String, Boolean)(dr("KeywordName"), dr("AutoComplete")), dr("Query"))
                            End While
                            System.Web.HttpContext.Current.Application("KeywordSearchCache") = dic
                            Return dic
                        End If

                    Catch ex As Exception
                        If dr IsNot Nothing AndAlso Not dr.IsClosed Then
                            dr.Close()
                        End If
                        Core.LogError("Keyword.vb", "KeywordSearchCach", ex)
                    End Try
                Else
                    Return System.Web.HttpContext.Current.Application("KeywordSearchCache")
                End If
                Return New Dictionary(Of Tuple(Of String, Boolean), String)
            End Get
            Set(value As Dictionary(Of Tuple(Of String, Boolean), String))
                System.Web.HttpContext.Current.Application("Keyword_KeywordSearchCache") = value
            End Set
        End Property

        Public Shared Function getQueryCacheSearch(ByVal kw As String, ByVal autocomplete As Boolean) As String
            Dim key As Tuple(Of String, Boolean) = New Tuple(Of String, Boolean)(kw, autocomplete)
            If KeywordSearchCache.ContainsKey(key) Then
                Return KeywordSearchCache(key)
            End If
            Return String.Empty
        End Function
        Public Shared Sub setQueryCacheSearch(ByVal kw As String, ByVal autocomplete As Boolean, ByVal query As String)
            Return
            Dim key As Tuple(Of String, Boolean) = New Tuple(Of String, Boolean)(kw, autocomplete)
            If Not KeywordSearchCache.ContainsKey(key) Then
                Try
                    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                    Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Keyword_insertCacheSearch")
                    db.AddInParameter(cmd, "KeywordName", DbType.String, kw)
                    db.AddInParameter(cmd, "AutoComplete", DbType.Boolean, autocomplete)
                    db.AddInParameter(cmd, "Query", DbType.String, query)
                    If db.ExecuteNonQuery(cmd) > 0 Then
                        If Not KeywordSearchCache.ContainsKey(key) Then
                            KeywordSearchCache.Add(key, query)
                        End If
                    End If
                Catch ex As Exception

                End Try
            End If
        End Sub
    End Class


    Public MustInherit Class KeywordRowBase
        Private m_DB As Database
        Private m_KeywordId As Int64 = Nothing
        Private m_KeywordName As String = Nothing
        Private m_TotalPoint As Int64 = Nothing
        Private m_TotalSearch As Int64 = Nothing
        Private m_TotalDetail As Int64 = Nothing
        Private m_TotalAddCart As Int64 = Nothing
        Public Property KeywordId() As Long
            Get
                Return m_KeywordId
            End Get
            Set(ByVal Value As Long)
                m_KeywordId = Value
            End Set
        End Property
        Public Property KeywordName() As String
            Get
                Return m_KeywordName
            End Get
            Set(ByVal Value As String)
                m_KeywordName = Value
            End Set
        End Property
        Public Property TotalPoint() As Int64
            Get
                Return m_TotalPoint
            End Get
            Set(ByVal Value As Int64)
                m_TotalPoint = Value
            End Set
        End Property
        Public Property TotalSearch() As Int64
            Get
                Return m_TotalSearch
            End Get
            Set(ByVal Value As Int64)
                m_TotalSearch = Value
            End Set
        End Property
        Public Property TotalDetail() As Int64
            Get
                Return m_TotalDetail
            End Get
            Set(ByVal Value As Int64)
                m_TotalDetail = Value
            End Set
        End Property
        Public Property TotalAddCart() As Int64
            Get
                Return m_TotalAddCart
            End Get
            Set(ByVal Value As Int64)
                m_TotalAddCart = Value
            End Set
        End Property
       
        Public Sub New()
        End Sub 'New

        Public Sub New(ByVal database As Database)
            m_DB = database
        End Sub 'New

        Public Sub New(ByVal database As Database, ByVal Id As Integer)
            m_DB = database
            m_KeywordId = Id
        End Sub 'New

        Protected Overridable Sub Load()
            Dim r As SqlDataReader = Nothing
            Try
                Dim SQL As String
                SQL = "SELECT * FROM Keyword WHERE KeywordId = " & m_DB.Number(KeywordId)
                r = m_DB.GetReader(SQL)
                If Not r Is Nothing Then
                    If r.Read Then
                        Me.Load(r)
                    End If
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Core.LogError("Keyword.vb", "Load", ex)
            End Try
        End Sub
        Protected Overridable Sub Load(ByVal reader As SqlDataReader)
            Try
                If (Not reader Is Nothing And Not reader.IsClosed) Then
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordId"))) Then
                        m_KeywordId = Convert.ToInt64(reader("KeywordId"))
                    Else
                        m_KeywordId = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordName"))) Then
                        m_KeywordName = reader("KeywordName").ToString()
                    Else
                        m_KeywordName = ""
                    End If

                    If (Not reader.IsDBNull(reader.GetOrdinal("TotalAddCart"))) Then
                        m_TotalAddCart = Convert.ToInt64(reader("TotalAddCart"))
                    Else
                        m_TotalAddCart = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("TotalDetail"))) Then
                        m_TotalDetail = Convert.ToInt64(reader("TotalDetail"))
                    Else
                        m_TotalDetail = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("TotalPoint"))) Then
                        m_TotalPoint = Convert.ToInt64(reader("TotalPoint"))
                    Else
                        m_TotalPoint = 0
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("TotalSearch"))) Then
                        m_TotalSearch = Convert.ToInt64(reader("TotalSearch"))
                    Else
                        m_TotalSearch = 0
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Sub
    End Class

    Public Class KeywordCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As KeywordRowBase)
            Me.List.Add(data)
        End Sub
        Public Sub Insert(ByVal index As Integer, ByVal data As KeywordRow)
            If (index < 0) Then
                Exit Sub
            End If
            If (index >= Me.List.Count) Then
                Exit Sub
            End If
            Me.List.Insert(index, data)
        End Sub
        Default Public Property Item(ByVal Index As Integer) As KeywordRowBase
            Get
                Return CType(Me.List.Item(Index), KeywordRowBase)
            End Get

            Set(ByVal Value As KeywordRowBase)
                Me.List(Index) = Value
            End Set
        End Property
    End Class
End Namespace



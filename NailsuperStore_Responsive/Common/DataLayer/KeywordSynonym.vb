Imports Components
Imports System.Data.SqlClient
Imports System.Data.Common
Imports Microsoft.Practices
Imports Utility
Imports System.Linq
Imports Microsoft.Practices.EnterpriseLibrary.Data

Namespace DataLayer
    Public Class KeywordSynonymRow
        Inherits KeywordSynonymRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Private Shared Function getKeywordSynonymAll() As KeywordSynonymCollection
            Dim resultSyn As KeywordSynonymCollection
            Dim key As String = String.Format(cachePrefixKey & "Synonym")

            resultSyn = CType(CacheUtils.GetCache(key), KeywordSynonymCollection)
            If resultSyn IsNot Nothing Then
                Return resultSyn
            End If

            Dim reader As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Keyword_GetListSynonymSearchNew")
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)

                resultSyn = New KeywordSynonymCollection()

                While (reader.Read())
                    Dim objKeyword As New KeywordSynonymRow
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordName"))) Then
                        objKeyword.KeywordName = reader("KeywordName").ToString().ToLower()
                    Else
                        objKeyword.KeywordName = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordNameSyn"))) Then
                        objKeyword.KeywordSynonymName = reader("KeywordNameSyn").ToString().ToLower().Trim().Replace("-", " ")
                    Else
                        objKeyword.KeywordSynonymName = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsRoundTrip"))) Then
                        objKeyword.IsRoundTrip = reader("IsRoundTrip")
                    Else
                        objKeyword.IsRoundTrip = False
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("SynonymType"))) Then
                        objKeyword.SynonymType = reader("SynonymType")
                    Else
                        objKeyword.SynonymType = ""
                    End If
                    resultSyn.Add(objKeyword)
                End While
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Core.LogError("KeywordSynonym.vb", "getKeywordSynonymAll", ex)
            End Try

            CacheUtils.SetCache(key, resultSyn, Utility.ConfigData.TimeCacheData)
            Return resultSyn
        End Function

        Public Shared Function getSynonymInKw(ByVal keyword As String, ByVal SynonymType As String) As Dictionary(Of String, String)
            Dim result As Dictionary(Of String, String) = New Dictionary(Of String, String)()
            keyword = keyword.Replace("-", " ")
            Dim kwSyn = getKeywordSynonymAll()
            If Not kwSyn Is Nothing Then
                Dim listResult As IEnumerable(Of KeywordSynonymRow) = kwSyn.OfType(Of KeywordSynonymRow).Where(Function(i) i.SynonymType = SynonymType AndAlso keyword.Contains(i.KeywordSynonymName))
                If listResult.Count() > 0 Then
                    For index = 0 To listResult.Count() - 1
                        If Not result.ContainsKey(listResult(index).KeywordName) Then
                            result.Add(listResult(index).KeywordName, listResult(index).KeywordSynonymName) 'tone
                        Else
                            Dim kwSynBeforeLght = result(listResult(index).KeywordName).Length
                            Dim length = listResult(index).KeywordSynonymName.Length
                            If length > kwSynBeforeLght Then
                                result(listResult(index).KeywordName) = listResult(index).KeywordSynonymName
                            End If
                        End If
                    Next
                    Return result
                End If
            End If
            Return New Dictionary(Of String, String)()
        End Function
        Private Shared Function getSynonymKeyword(ByVal keyword As String, Optional SynonymType As String = "") As KeywordCollection
            Dim result As KeywordCollection = New KeywordCollection()
            Dim kwSyn = getKeywordSynonymAll()
            If Not kwSyn Is Nothing Then
                Dim listResult As IEnumerable(Of KeywordSynonymRow) = kwSyn.OfType(Of KeywordSynonymRow).Where(Function(i) i.SynonymType = SynonymType AndAlso keyword.Contains(i.KeywordName))
                If listResult.Count() > 0 Then
                    For index = 0 To listResult.Count() - 1
                        Dim objKeyword As New KeywordRow
                        objKeyword.KeywordName = keyword.Replace(listResult(index).KeywordName, listResult(index).KeywordSynonymName)
                        result.Add(objKeyword)
                    Next
                End If
            End If
            Return result
        End Function

        Public Shared cachePrefixKey As String = "KeywordSynonym_"

        Public Shared Function GetListSynonymSearchNew(ByVal Keyword As String, Optional SynonymType As String = "") As KeywordCollection
            Return getSynonymKeyword(Keyword, SynonymType)
        End Function
        Public Shared Function CheckBuyInBulkSynonym(ByVal Keyword As String) As Boolean
            Dim kwSyn = getKeywordSynonymAll()
            Return kwSyn.OfType(Of KeywordSynonymRow).Any(Function(i) i.SynonymType = "buyinbulk" AndAlso (" " + Keyword.Trim() + " ").Contains(" " + i.KeywordSynonymName + " "))
        End Function
        Public Shared Function GetBuyInBulkSynonym(ByVal Keyword As String) As IEnumerable(Of KeywordSynonymRow)
            Dim kwSyn = getKeywordSynonymAll()
            Return kwSyn.OfType(Of KeywordSynonymRow).Where(Function(i) i.SynonymType = "buyinbulk" AndAlso (Keyword.Trim().Replace(" ", "")).Contains(i.KeywordSynonymName.Trim().Replace(" ", "")))
        End Function

        Public Shared Function GetListSynonymSearch(ByVal Keyword As String) As KeywordCollection
            Dim result As KeywordCollection
            Dim key As String = String.Format(cachePrefixKey & Keyword)
            result = CType(CacheUtils.GetCache(key), KeywordCollection)
            If Not result Is Nothing Then
                Return result
            End If

            Dim reader As SqlDataReader = Nothing
            Try
                result = New KeywordCollection
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Keyword_GetListSynonymSearch")
                db.AddInParameter(cmd, "KeywordName", DbType.String, Keyword)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                While (reader.Read())
                    Dim objKeyword As New KeywordRow
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordName"))) Then
                        objKeyword.KeywordName = reader("KeywordName").ToString()
                    Else
                        objKeyword.KeywordName = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordId"))) Then
                        objKeyword.KeywordId = CInt(reader("KeywordId").ToString())
                    Else
                        objKeyword.KeywordId = Nothing
                    End If
                    result.Add(objKeyword)
                End While
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Core.LogError("KeywordSynonym.vb", "GetListSynonymSearch(KeywordName=" & Keyword & ")", ex)
            End Try

            CacheUtils.SetCache(key, result, Utility.ConfigData.TimeCacheData)
            Return result
        End Function

        Public Shared Function GetListKeywordSynonym(ByVal Keyword As String, Optional ByVal synonymType As String = "") As KeywordSynonymCollection
            Dim reader As SqlDataReader = Nothing
            Dim result As New KeywordSynonymCollection
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_Keyword_GetListSynonym")
                db.AddInParameter(cmd, "KeywordName", DbType.String, Keyword)
                db.AddInParameter(cmd, "SynonymType", DbType.String, synonymType)
                reader = CType(db.ExecuteReader(cmd), SqlDataReader)
                While (reader.Read())
                    Dim objKeyword As New KeywordSynonymRow
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordName"))) Then
                        objKeyword.KeywordSynonymName = reader("KeywordName").ToString()
                    Else
                        objKeyword.KeywordSynonymName = ""
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("KeywordId"))) Then
                        objKeyword.KeywordId = CInt(reader("KeywordId").ToString())
                    Else
                        objKeyword.KeywordId = Nothing
                    End If
                    If (Not reader.IsDBNull(reader.GetOrdinal("IsRoundTrip"))) Then
                        objKeyword.IsRoundTrip = CBool(reader("IsRoundTrip").ToString())
                    Else
                        objKeyword.IsRoundTrip = False
                    End If
                    result.Add(objKeyword)
                End While
                Core.CloseReader(reader)
            Catch ex As Exception
                Core.CloseReader(reader)
                Core.LogError("KeywordSynonym.vb", "GetListKeywordKeywordSynonym(KeywordName=" & Keyword & ")", ex)
            End Try
            Return result
        End Function
        Public Shared Function ChangeArrange(ByVal keywordId As Integer, ByVal KeywordSynonymId As Integer, ByVal IsUp As Boolean) As Boolean
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeywordSynonym_ChangeArrange")
                db.AddInParameter(cmd, "KeywordId", DbType.Int32, keywordId)
                db.AddInParameter(cmd, "KeywordSynonymId", DbType.Int32, KeywordSynonymId)
                db.AddInParameter(cmd, "IsUp", DbType.Boolean, IsUp)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception
                Core.LogError("KeywordSynonym.vb", "ChangeArrange(keywordId=" & keywordId & ",KeywordSynonymId=" & KeywordSynonymId & ",IsUp=" & IsUp & ")", ex)
            End Try
            Return False
        End Function
        Public Shared Function Delete(ByVal keywordId As Integer, ByVal KeywordSynonymId As Integer) As Boolean
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeywordSynonym_Delete")
                db.AddInParameter(cmd, "KeywordId", DbType.Int32, keywordId)
                db.AddInParameter(cmd, "KeywordSynonymId", DbType.Int32, KeywordSynonymId)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception
                Core.LogError("KeywordSynonym.vb", "Delete(keywordId=" & keywordId & ",KeywordSynonymId=" & KeywordSynonymId & ")", ex)
            End Try
            Return False
        End Function
        Public Shared Function UpdateRoundTrip(ByVal keywordId As Integer, ByVal KeywordSynonymId As Integer) As Boolean
            Try

                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeywordSynonym_UpdateRoundTrip")
                db.AddInParameter(cmd, "KeywordId", DbType.Int32, keywordId)
                db.AddInParameter(cmd, "KeywordSynonymId", DbType.Int32, KeywordSynonymId)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                If result = 1 Then
                    Return True
                End If
            Catch ex As Exception
                Core.LogError("KeywordSynonym.vb", "UpdateRoundTrip(keywordId=" & keywordId & ",KeywordSynonymId=" & KeywordSynonymId & ")", ex)
            End Try
            Return False
        End Function
        Public Shared Function Insert(ByVal objKeyword As KeywordSynonymRow, ByVal db As EnterpriseLibrary.Data.Database, Optional ByVal synonymType As String = "") As Integer
            Try

                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeywordSynonym_Insert")
                db.AddInParameter(cmd, "KeywordId", DbType.Int64, objKeyword.KeywordId)
                db.AddInParameter(cmd, "KeywordSynonymName", DbType.String, objKeyword.KeywordSynonymName)
                db.AddInParameter(cmd, "IsRoundTrip", DbType.Boolean, objKeyword.IsRoundTrip)
                db.AddInParameter(cmd, "SynonymType", DbType.String, synonymType)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                Return result
            Catch ex As Exception
                '' Core.LogError("KeywordSynonym.vb", "Insert(keywordId=" & objKeyword.KeywordId & ",KeywordSynonymName=" & objKeyword.KeywordSynonymName & ",IsRoundTrip=" & objKeyword.IsRoundTrip & ")", ex)
            End Try
            Return 0
        End Function
        Public Shared Function InsertMainKeyword(ByVal objKeyword As KeywordRow, ByVal db As EnterpriseLibrary.Data.Database) As Integer

            Try


                Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeywordSynonym_InsertMainKeyword")
                db.AddInParameter(cmd, "KeywordName", DbType.String, objKeyword.KeywordName)
                db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                db.ExecuteNonQuery(cmd)
                Dim result As Integer = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                Return result
            Catch ex As Exception

            End Try
            Return 0
        End Function
        Public Shared Function InsertListKeywordSynonym(ByVal objMainKeyword As KeywordRow, ByVal lstOneway As KeywordSynonymCollection, ByVal lstRoundTrip As KeywordSynonymCollection, Optional ByVal synonymType As String = "") As Integer
            Dim _Connection As DbConnection = Nothing
            Dim _Transaction As DbTransaction = Nothing
            Dim result As Integer = 0
            Try
                Dim sql As String = String.Empty
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                _Connection = db.CreateConnection
                _Connection.Open()
                _Transaction = _Connection.BeginTransaction()
                If (objMainKeyword.KeywordId < 1) Then ''insert new keyword

                    objMainKeyword.KeywordId = InsertMainKeyword(objMainKeyword, db)
                Else ''delete old date
                    sql = "Delete from KeywordSynonym where KeywordId=" & objMainKeyword.KeywordId
                    db.ExecuteNonQuery(CommandType.Text, sql)
                End If
                If Not lstOneway Is Nothing Then
                    For Each keyword As KeywordSynonymRow In lstOneway
                        keyword.KeywordId = objMainKeyword.KeywordId
                        Insert(keyword, db, synonymType)
                    Next
                End If
                If Not lstRoundTrip Is Nothing Then
                    For Each keyword As KeywordSynonymRow In lstRoundTrip
                        keyword.KeywordId = objMainKeyword.KeywordId
                        Insert(keyword, db, synonymType)
                    Next
                End If
                result = objMainKeyword.KeywordId
                _Transaction.Commit()
            Catch ex As Exception
                _Transaction.Rollback()
                Core.LogError("KeywordSynonym.vb", "InsertListKeywordSynonym(mainKeywordName=" & objMainKeyword.KeywordName & ")", ex)
            Finally


                If _Connection.State = ConnectionState.Open Then
                    _Connection.Close()

                End If
                _Connection = Nothing
                _Transaction = Nothing
            End Try
            Return result
        End Function
    End Class

    Public MustInherit Class KeywordSynonymRowBase
        Private m_DB As Database
        Private m_KeywordId As Int64 = Nothing
        Private m_KeywordSynonymName As String = Nothing
        Private m_KeywordSynonymId As Int64 = Nothing
        Private m_IsRoundTrip As Boolean = False
        Private m_keywordName As String = Nothing

        Private m_Arrange As Integer = Nothing
        Private m_synonymType As String
        Public Property SynonymType() As String
            Get
                Return m_synonymType
            End Get
            Set(ByVal value As String)
                m_synonymType = value
            End Set
        End Property

        Public Property KeywordName() As String
            Get
                Return m_keywordName
            End Get
            Set(ByVal Value As String)
                m_keywordName = Value
            End Set
        End Property

        Public Property KeywordId() As Int64
            Get
                Return m_KeywordId
            End Get
            Set(ByVal Value As Int64)
                m_KeywordId = Value
            End Set
        End Property
        Public Property KeywordSynonymId() As Int64
            Get
                Return m_KeywordSynonymId
            End Get
            Set(ByVal Value As Int64)
                m_KeywordSynonymId = Value
            End Set
        End Property
        Public Property IsRoundTrip() As Boolean
            Get
                Return m_IsRoundTrip
            End Get
            Set(ByVal Value As Boolean)
                m_IsRoundTrip = Value
            End Set
        End Property
        Public Property KeywordSynonymName() As String
            Get
                Return m_KeywordSynonymName
            End Get
            Set(ByVal Value As String)
                m_KeywordSynonymName = Value
            End Set
        End Property
        Public Property Arrange() As Integer
            Get
                Return m_Arrange
            End Get
            Set(ByVal Value As Integer)
                m_Arrange = Value
            End Set
        End Property

        Public Sub New()
        End Sub 'New

    End Class

    Public Class KeywordSynonymCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As KeywordSynonymRowBase)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As KeywordSynonymRowBase
            Get
                Return CType(Me.List.Item(Index), KeywordSynonymRowBase)
            End Get

            Set(ByVal Value As KeywordSynonymRowBase)
                Me.List(Index) = Value
            End Set
        End Property
        Public Sub Insert(ByVal index As Integer, ByVal data As KeywordSynonymRow)
            If (index < 0) Then
                Exit Sub
            End If
            If (index >= Me.List.Count) Then
                Exit Sub
            End If
            Me.List.Insert(index, data)
        End Sub
    End Class
End Namespace



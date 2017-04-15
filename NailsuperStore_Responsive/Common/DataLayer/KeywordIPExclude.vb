

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
Imports Database

Namespace DataLayer
    Public Class KeywordIPExcludeRow
        Inherits KeywordIPExcludeRowBase
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Shared Function GetRow(ByVal Id As Integer) As KeywordIPExcludeRow
            'Get cache
            Dim key As String = cachePrefixKey & "GetRow_" & Id
            Dim result As KeywordIPExcludeRow = CType(CacheUtils.GetCache(key), KeywordIPExcludeRow)
            If Not result Is Nothing Then
                Return result
            Else
                result = New KeywordIPExcludeRow
            End If

            'Get db
            Dim r As SqlDataReader = Nothing
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Try
                Dim cmd As SqlCommand = db.GetSqlStringCommand("SELECT * FROM KeywordIPExclude WHERE Id=" & Id)
                r = db.ExecuteReader(cmd)
                If r.HasRows Then
                    result = mapList(Of KeywordIPExcludeRow)(r)(0)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "GetRow", "Exception" & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try

            Return result
        End Function

        Public Shared Function ListIP() As List(Of KeywordIPExcludeRow)
            Dim result As New List(Of KeywordIPExcludeRow)
            Dim r As SqlDataReader = Nothing
            Try
                Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                Dim cmd As SqlCommand = db.GetSqlStringCommand("SELECT * FROM KeywordIPExclude")
                r = db.ExecuteReader(cmd)

                If r.HasRows Then
                    result = mapList(Of KeywordIPExcludeRow)(r)
                End If
                Core.CloseReader(r)
            Catch ex As Exception
                Core.CloseReader(r)
                Components.Email.SendError("ToError500", "ListIP", "Exception" & ex.Message & ", Stack trace:" & ex.StackTrace)
            End Try
            Return result
        End Function

        Public Shared Function Insert(ByVal item As KeywordIPExcludeRow) As Integer
            Dim key As String = CacheUtils.RemoveCacheItemWithPrefix(cachePrefixKey)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As SqlCommand = db.GetStoredProcCommand("sp_KeywordIPExclude_Insert")
            db.AddInParameter(cmd, "IP", DbType.String, item.IP)
            Dim result As Integer = db.ExecuteNonQuery(cmd)
            Return result
        End Function

        Public Shared Function Update(ByVal item As KeywordIPExcludeRow) As Integer
            Dim key As String = CacheUtils.RemoveCacheItemWithPrefix(cachePrefixKey)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As SqlCommand = db.GetStoredProcCommand("sp_KeywordIPExclude_Update")
            db.AddInParameter(cmd, "Id", DbType.Int32, item.Id)
            db.AddInParameter(cmd, "IP", DbType.String, item.IP)
            Dim result As Integer = db.ExecuteNonQuery(cmd)
            Return result
        End Function

        Public Shared Function Delete(ByVal Id As Integer) As Boolean
            Dim key As String = CacheUtils.RemoveCacheItemWithPrefix(cachePrefixKey)
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
            Dim cmd As SqlCommand = db.GetSqlStringCommand("DELETE FROM KeywordIPExclude WHERE Id=" & Id)
            Dim result As Integer = db.ExecuteNonQuery(cmd)
            If result > 0 Then
                Return True
            End If
            Return False
        End Function

        Public Shared Function CheckNotAllowTrackingKeyword(ByVal ip As String) As Boolean
            Dim result As Integer = 0
            Try
                Dim key As String = String.Format(cachePrefixKey & "CheckAllowTrackingKeyword_{0}", ip)
                Dim resultCache As Object = CacheUtils.GetCache(key)
                If (Not resultCache Is Nothing) Then
                    result = CInt(resultCache)
                Else
                    Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()
                    Dim cmd As DbCommand = db.GetStoredProcCommand("sp_KeywordIPExclude_IsNotAllowTrackingKeyword")
                    db.AddInParameter(cmd, "IP", DbType.String, ip)
                    db.AddParameter(cmd, "return_value", DbType.Int32, ParameterDirection.ReturnValue, Nothing, DataRowVersion.Default, Nothing)
                    db.ExecuteNonQuery(cmd)
                    result = Convert.ToInt32(db.GetParameterValue(cmd, "return_value"))
                    CacheUtils.SetCache(key, result)
                End If
            Catch ex As Exception
                Core.LogError("KeywordIPExclude.vb", "CheckAllowTrackingKeyword(ip=" & ip & ")", ex)
            End Try
            If (result = 1) Then
                Return True
            End If
            Return False
        End Function


    End Class

    Public MustInherit Class KeywordIPExcludeRowBase

        Private m_Id As Integer = Nothing
        Private m_IP As String = Nothing
        Public Shared cachePrefixKey As String = "KeywordIPExclude_"

        Public Property Id() As Integer
            Get
                Return m_Id
            End Get
            Set(ByVal Value As Integer)
                m_Id = Value
            End Set
        End Property

        Public Property IP() As String
            Get
                Return m_IP
            End Get
            Set(ByVal Value As String)
                m_IP = Value
            End Set
        End Property


        Public Sub New()
        End Sub 'New


    End Class

    Public Class KeywordIPExcludeRowCollection
        Inherits CollectionBase

        Public Sub New()
        End Sub

        Public Sub Add(ByVal data As KeywordIPExcludeRowBase)
            Me.List.Add(data)
        End Sub

        Default Public Property Item(ByVal Index As Integer) As KeywordIPExcludeRowBase
            Get
                Return CType(Me.List.Item(Index), KeywordIPExcludeRowBase)
            End Get

            Set(ByVal Value As KeywordIPExcludeRowBase)
                Me.List(Index) = Value
            End Set
        End Property

    End Class
End Namespace



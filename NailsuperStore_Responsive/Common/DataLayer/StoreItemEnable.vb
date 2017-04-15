Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Components
Imports Components.Core
Imports System.Data
Imports System.Data.Common
Imports Microsoft.Practices
Imports Utility
Imports System.Data.SqlClient
Namespace DataLayer

    Public Class StoreItemEnable
        Public Sub New()
            MyBase.New()
        End Sub 'New

        Public Shared Function ListBrands(ByVal CustomerId As Integer) As DataTable
            'If CustomerId = 0 Then
            '    Return New DataTable
            'End If
            Dim db As EnterpriseLibrary.Data.Database = DatabaseFactory.CreateDatabase()

            Dim SP_GETLIST As String = "sp_StoreItemEnable_ListBrands"

            Dim cmd As DbCommand = db.GetStoredProcCommand(SP_GETLIST)
            db.AddInParameter(cmd, "MemberId", DbType.Int32, CustomerId)

            Return db.ExecuteDataSet(cmd).Tables(0)

        End Function
        Public Shared Function Remove(ByVal _Database As Database, ByVal EnableId As String) As Boolean
            Dim result As Integer = 0
            Try
                Dim SP_STOREITEMENABLE_DELETE As String = "sp_StoreItemEnable_Delete"
                Dim cmd As SqlCommand = _Database.CreateCommand(SP_STOREITEMENABLE_DELETE)
                cmd.Parameters.Add(_Database.InParam("EnableId", SqlDbType.Int, 0, EnableId))
                cmd.Parameters.Add(_Database.ReturnParam("result", SqlDbType.Int))
                cmd.ExecuteNonQuery()
                result = CInt(cmd.Parameters("result").Value)
            Catch ex As Exception
            End Try
            If result = 1 Then
                '' CacheUtils.ClearCacheWithPrefix(StoreItemRow.cachePrefixKey)
                CacheUtils.ClearCacheWithPrefix(StoreItemRowBase.cachePrefixKey, DepartmentTabItemRowBase.cachePrefixKey, ShopSaveItemRowBase.cachePrefixKey, SalesCategoryItemRow.cachePrefixKey)

                Return True
            End If
            Return False
        End Function
    End Class
End Namespace
Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer
Imports Utility

Partial Class admin_store_salescategory_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim SalesCategoryId As Integer = Convert.ToInt32(Request("SalesCategoryId"))
        Try
            DB.BeginTransaction()
            SalesCategoryRow.RemoveRow(DB, SalesCategoryId)
            ''Utility.CacheUtils.RemoveCache(enmCache.SaleMenu)
            DB.CommitTransaction()

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class


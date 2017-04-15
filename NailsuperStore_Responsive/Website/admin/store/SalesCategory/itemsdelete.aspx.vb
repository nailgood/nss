Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_store_salescategory_itemsdelete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim SalesCategoryId As Integer = Convert.ToInt32(Request("SalesCategoryId"))
        Dim ItemId As Integer = Convert.ToInt32(Request("Id"))
        Try
            DB.BeginTransaction()
            SalesCategoryItemRow.RemoveRow(DB, SalesCategoryId, ItemId)
            DB.CommitTransaction()

            Response.Redirect("items.aspx?SalesCategoryId=" & SalesCategoryId & "&" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class


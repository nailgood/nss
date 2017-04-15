Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_store_items_SpecificDelete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim Id As Integer = Convert.ToInt32(Request("Id"))
        Dim ItemGroupId As Integer = Convert.ToInt32(Request("ItemGroupId"))
        Try
            DB.BeginTransaction()
            Dim row As StoreItemRow = StoreItemRow.GetRow(DB, ItemGroupId)
            StoreItemRow.DeleteAllStoreItemGroupChoiceRel(DB, Id)
            row.DeleteStoreItemGroupRel(Id)
            DB.CommitTransaction()
            Response.Redirect("groupitems.aspx?ItemGroupId=" & ItemGroupId & "&" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class


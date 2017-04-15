Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_itemrelate_delete
    Inherits AdminPage

    Private ITEM_ID As Integer
    Private IMAGE As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        ITEM_ID = Convert.ToInt32(Request("ItemId"))
        Try
            DB.BeginTransaction()

            IMAGE = DB.ExecuteScalar("SELECT Coalesce(Image,'') AS IMAGE FROM StoreItem WHERE ItemId=" & ITEM_ID)

            SQL = "delete from StoreCollectionItem where ItemId=" & ITEM_ID
            DB.ExecuteSQL(SQL)

            SQL = "delete from StoreToneItem where ItemId=" & ITEM_ID
            DB.ExecuteSQL(SQL)

            SQL = "delete from RelatedItem where itemId = " & ITEM_ID
            DB.ExecuteSQL(SQL)

            SQL = "delete from storeitemfeature where itemId = " & ITEM_ID
            DB.ExecuteSQL(SQL)

            StoreItemRow.RemoveRow(DB, ITEM_ID)
            DB.CommitTransaction()

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Finally
        End Try
    End Sub
End Class
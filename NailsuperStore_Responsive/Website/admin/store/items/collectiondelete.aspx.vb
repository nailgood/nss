Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_items_collectiondelete
    Inherits AdminPage

    Private ItemId As Integer
    Private ParentId As Integer
    Private sType As String

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        ParentId = Convert.ToInt32(Request("ParentId"))
        ItemId = Convert.ToInt32(Request("ItemId"))
        sType = Convert.ToString(Request("sType"))


        Try
            DB.BeginTransaction()
            If sType = "Collection" Then
                StoreItemRow.RemoveCollectionItem(DB, ParentId, ItemId)
            ElseIf sType = "Related" Then
                StoreItemRow.RemoveRelatedItem(DB, ParentId, ItemId)
            ElseIf sType = "Swatch" Then
                StoreItemRow.RemoveRelatedSwatch(DB, ParentId, ItemId)
            End If
            DB.CommitTransaction()

            If sType = "Collection" Then
                Response.Redirect("Collection.aspx?ItemId=" & ParentId & "&" & GetPageParams(FilterFieldType.All))
            ElseIf sType = "Related" Then
                Response.Redirect("related.aspx?ItemId=" & ParentId & "&" & GetPageParams(FilterFieldType.All))
            ElseIf sType = "Swatch" Then
                Response.Redirect("related-swatch.aspx?ItemId=" & ParentId & "&" & GetPageParams(FilterFieldType.All))
            End If

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        Finally
        End Try
    End Sub
End Class
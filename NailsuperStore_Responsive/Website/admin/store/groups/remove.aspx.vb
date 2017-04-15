Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_store_groups_remove
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim ItemGroupId As Integer = Convert.ToInt32(Request("ItemGroupId"))
        Dim ItemId As Integer = Convert.ToInt32(Request("ItemId"))
        Try
            If (StoreItemRow.RemoveOptionChoices(ItemGroupId, ItemId)) Then
                StoreItemRow.ClearItemCache(ItemId)
                Response.Redirect("items.aspx?ItemGroupId=" & ItemGroupId & "&" & GetPageParams(FilterFieldType.All))
            Else
                AddError("An error occured during this operation. Please try again later.")
            End If
        Catch ex As SqlException

            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class


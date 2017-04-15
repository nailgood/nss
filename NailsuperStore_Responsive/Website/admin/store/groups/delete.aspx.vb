Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_store_groups_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim ItemGroupId As Integer = Convert.ToInt32(Request("ItemGroupId"))
        Try
            Dim result As Integer = StoreItemGroupRow.Delete(ItemGroupId)
            If (result = 1) Then
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            ElseIf (result = -1) Then
                AddError("Item Group is currently in use. Please try again later.")
            Else
                AddError("An error occured during this operation. Please try again later.")
            End If

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class


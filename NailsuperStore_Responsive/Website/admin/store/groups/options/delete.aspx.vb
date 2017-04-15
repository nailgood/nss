Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_store_groups_options_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim OptionId As Integer = Convert.ToInt32(Request("OptionId"))
        Try
            Dim result As Integer = StoreItemGroupOptionRow.Delete(OptionId)
            If (result = 1) Then
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            ElseIf (result = -1) Then
                AddError("Item Group Option Choice is currently in use. Please try again later.")
            Else
                AddError("An error occured during this operation. Please try again later.")
            End If

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class


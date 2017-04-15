Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_admins_groups_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim GroupId As Integer = Convert.ToInt32(Request("GroupId"))
        Try
            AdminAccessRow.RemoveByGroup(GroupId)
            AdminGroupRow.RemoveRow(GroupId)
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

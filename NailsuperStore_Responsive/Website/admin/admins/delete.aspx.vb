Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_admins_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim AdminId As Integer = Convert.ToInt32(Request("AdminId"))
        Dim dbAdmin As AdminRow = AdminRow.GetRow(DB, AdminId)
        If (Not dbAdmin.Username = CType(Context.User.Identity, AdminIdentity).Username) Then
            Try
                AdminRow.RemoveRow(DB, AdminId)
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            Catch ex As SqlException
                AddError(ErrHandler.ErrorText(ex))
            End Try
        Else
            Response.Redirect("default.aspx")
        End If
    End Sub
End Class

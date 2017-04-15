Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Partial Class admin_admins_password
    Inherits AdminPage

    Protected AdminId As Integer
    Protected sUsername As String = ""
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        AdminId = Convert.ToInt32(Request("AdminId"))

        If Not IsPostBack Then
            If AdminId <> 0 Then
                Dim dbAdmin As AdminRow = AdminRow.GetRow(DB, AdminId)
                sUsername = dbAdmin.Username
            Else
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            End If

        End If

    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Save.Click
        If AdminId <> 0 Then
            Dim dbAdmin As AdminRow = AdminRow.GetRow(DB, AdminId)
            dbAdmin.Password = PASSWORD1.Text
            dbAdmin.Update()
            WriteLogDetail("Update Admin User", dbAdmin)
        End If
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

    End Sub
End Class

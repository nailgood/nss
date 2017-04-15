Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Partial Class admin_admins_AdminIPAccess_Default
    Inherits AdminPage
    Private TotalRecords As Integer
    Private Type As Integer = 1
    Dim ToTal As Integer
    Protected NewsId As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindList()
        End If
    End Sub

    Private Sub BindList()
        Dim lstData As AdminIPAccessCollection = AdminIPAccessRow.ListAllByUsername(DB, Username)
        gvList.DataSource = lstData
        gvList.DataBind()
    End Sub

    Protected Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim ltrIP As Literal = CType(e.Row.FindControl("ltrIP"), Literal)
            Dim objData As AdminIPAccessRow = DirectCast(DirectCast(e.Row.DataItem, System.Object), DataLayer.AdminIPAccessRow)
            ltrIP.Text = "<a href='Edit.aspx?username=" & UserName() & "&Id=" & objData.Id & "'>" & objData.IP & "</a>"
        End If

    End Sub

    Protected Sub gvList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvList.RowCommand
        If e.CommandName = "Delete" Then
            AdminIPAccessRow.Delete(DB, e.CommandArgument)
            Response.Redirect("Default.aspx?username=" & Username & "&" & GetPageParams(FilterFieldType.All))
        End If
    End Sub
 

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Response.Redirect("../../admins/default.aspx")
    End Sub
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Response.Redirect("Edit.aspx?username=" & Username)
    End Sub
    Public Function UserName() As String
        Try
            Return Request("username")
        Catch ex As Exception

        End Try
        Response.Redirect("../admins/default.aspx")
        Return String.Empty
    End Function

End Class

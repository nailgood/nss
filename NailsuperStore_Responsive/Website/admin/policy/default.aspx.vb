Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components
Imports Utility.Common

Public Class admin_PolicyDefault
    Inherits AdminPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            LoadList()
        End If
    End Sub

    Private Sub LoadList()
        Dim collection As PolicyCollection = PolicyRow.ListAll()

        If collection.TotalRecords > 0 Then
            rptPolicy.DataSource = collection
        Else
            rptPolicy.DataSource = Nothing
        End If
        rptPolicy.DataBind()

    End Sub

    Protected Sub btnAddNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddNew.Click
        Response.Redirect("edit.aspx")
    End Sub


    Protected Sub rptPolicy_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptPolicy.ItemCommand
        If e.CommandName = "Delete" Then
            If PolicyRow.Delete(e.CommandArgument) Then
                LoadList()
            End If
        ElseIf e.CommandName = "Active" Then
            If PolicyRow.ChangeIsActive(e.CommandArgument) Then
                LoadList()
            End If

        End If
    End Sub

    Protected Sub rptPolicy_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPolicy.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim p As PolicyRow = CType(e.Item.DataItem, PolicyRow)

            Dim litName As Literal = CType(e.Item.FindControl("litName"), Literal)
            litName.Text = String.Format("<a href=""edit.aspx?id={0}"">{1}</a>", p.PolicyId, p.Title)

            Dim imbActive As ImageButton = CType(e.Item.FindControl("imbActive"), ImageButton)
            If p.IsActive Then
                imbActive.ImageUrl = "/includes/theme-admin/images/active.png"
            Else
                imbActive.ImageUrl = "/includes/theme-admin/images/inactive.png"
            End If
        End If
    End Sub
End Class

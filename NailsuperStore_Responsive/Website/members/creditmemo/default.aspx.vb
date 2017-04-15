Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Public Class Member_creditmemo_Default
    Inherits SitePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HasAccess() Then
            DB.Close()
            Response.Redirect("/members/login.aspx")
        End If
        Dim dbMember As MemberRow = MemberRow.GetRow(Session("memberId"))

        'ltlPageTitle.Text = "My Credit History"
        'ltlMemberNavigation.Text = MemberRow.MemberNavigationString

        Dim dt As DataTable = dbMember.GetCreditMemoHistory()

        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
            rptOrderHistory.DataSource = dt
            rptOrderHistory.DataBind()

        Else
            rptOrderHistory.DataSource = Nothing
            rptOrderHistory.DataBind()

            divEmpty.Visible = True
        End If

    End Sub

    Protected Sub rptOrderHistory_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptOrderHistory.ItemCommand
        If e.CommandName = "Details" Then
            DB.Close()
            Response.Redirect("/members/creditmemo/view.aspx?MemoId=" & e.CommandArgument)
        End If
    End Sub

    Protected Sub rptOrderHistory_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptOrderHistory.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            Dim btnDetails As Button = CType(e.Item.FindControl("btnDetails"), Button)
            Dim ltlOrderNo As Literal = CType(e.Item.FindControl("ltlOrderNo"), Literal)
            Dim ltlBillingName As Literal = CType(e.Item.FindControl("ltlBillingName"), Literal)
            Dim ltlTotal As Literal = CType(e.Item.FindControl("ltlTotal"), Literal)
            Dim ltlStatus As Literal = CType(e.Item.FindControl("ltlStatus"), Literal)
            Dim ltlPurchaseDate As Literal = CType(e.Item.FindControl("ltlPurchaseDate"), Literal)
            Dim ltlTrackingNo As Literal = e.Item.FindControl("ltlTrackingNo")

            btnDetails.CommandArgument = e.Item.DataItem("MemoId")
            ltlOrderNo.Text = e.Item.DataItem("No")

            If Not IsDBNull(e.Item.DataItem("selltocustomername")) Then ltlBillingName.Text = e.Item.DataItem("selltocustomername") & " " & e.Item.DataItem("selltocustomername2")
            If Not IsDBNull(e.Item.DataItem("Total")) Then ltlTotal.Text = FormatCurrency(-1 * e.Item.DataItem("Total"), 2, TriState.UseDefault, TriState.False, TriState.UseDefault)
            If Not IsDBNull(e.Item.DataItem("posting")) Then ltlPurchaseDate.Text = e.Item.DataItem("posting")
        End If
    End Sub
End Class
Imports DataLayer
Imports Components
Imports Utility
Partial Class admin_store_items_AmazonHistory
    Inherits AdminPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gvList.BindList = AddressOf LoadList
        If Not IsPostBack Then
            LoadList()
        End If
    End Sub
    Private Sub LoadList()
        Dim sku As String = String.Empty
        If Request.QueryString("sku") <> Nothing AndAlso Request.QueryString("sku").Length > 0 Then
            sku = Request.QueryString("sku")
        End If
        If Not sku Is Nothing Then
            Dim item As StoreItemRow = StoreItemRow.GetRowSku(DB, sku)
            ltrHeader.Text &= item.ItemName
            Dim data As DataTable = DB.GetDataTable("exec sp_AmazonItemHistory " & item.SKU)
            gvList.DataSource = data
            gvList.DataBind()
        End If
    End Sub
    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If
        Dim lbWeightPound As Label = e.Row.FindControl("lbWeightPound")
        Dim ltTitle As Literal = e.Row.FindControl("ltTitle")
        lbWeightPound.Text = e.Row.DataItem("Weight").ToString & " lbs"
        Dim ltPrice As Literal = e.Row.FindControl("ltPrice")
        ltPrice.Text = FormatCurrency(e.Row.DataItem("Price")).ToString
        ltTitle.Text = String.Format("<a target='_blank' href='" & Utility.ConfigData.AmazonLinkItemDetail & "'>{1}</a>", e.Row.DataItem("ASIN"), e.Row.DataItem("Title").ToString)
    End Sub

    Protected Sub Cancel2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancel2.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

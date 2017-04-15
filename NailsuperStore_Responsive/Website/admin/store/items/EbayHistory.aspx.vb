Imports DataLayer
Imports Components
Imports Utility
Partial Class admin_store_items_EbayHistory
    Inherits AdminPage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        gvList.BindList = AddressOf LoadList
        If Not IsPostBack Then
            LoadList()
        End If
    End Sub
    Private Sub LoadList()
        Dim id As Int32 = 0
        If Request.QueryString("id") <> Nothing AndAlso Request.QueryString("id").Length > 0 Then
            id = CInt(Request.QueryString("id"))
        End If
        If (id > 0) Then
            Dim item As StoreItemRow = StoreItemRow.GetRow(DB, id)
            ltrHeader.Text &= item.ItemName
            Dim data As DataTable = DB.GetDataTable("exec sp_EbayItemHistory " & item.ItemId)
            gvList.DataSource = data
            gvList.DataBind()
        End If
    End Sub
    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel2.Click
        
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))


    End Sub

    Private Sub gvList_ItemDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If Not (e.Row.RowType = DataControlRowType.DataRow) Then
            Exit Sub
        End If
        Dim lbDuration As Label = e.Row.FindControl("lbDuration")
        Dim lbWeightPound As Label = e.Row.FindControl("lbWeightPound")
        Dim ltTitle As Literal = e.Row.FindControl("ltTitle")
        lbDuration.Text = e.Row.DataItem("Duration").ToString.Replace("Days_", "")
        lbWeightPound.Text = e.Row.DataItem("WeightPound").ToString & " lbs"
        ltTitle.Text = String.Format("<a target='_blank' href='" & Utility.ConfigData.EbayLinkItemDetail & "'>{1}</a>", e.Row.DataItem("EbayId"), e.Row.DataItem("Title").ToString)
    End Sub
End Class

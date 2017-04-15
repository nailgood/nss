Imports Components
Imports DataLayer
Partial Class store_main_shop_save
    Inherits SitePage
    Private imgShopsave As String = "<img alt=""{1}"" src=""" & Utility.ConfigData.CDNMediaPath & "/assets/shopsave/home/{0}"">"
    Private Sub LoadData()

        rptShopSave.DataSource = ShopSaveRow.ListShopSave(DB, "") '1,2: Shop Now & save now
        rptShopSave.DataBind()
        If rptShopSave.Items.Count = 0 Then
            rptShopSave.DataSource = Nothing
            rptShopSave.DataBind()
        End If
    End Sub
    Protected Sub rptShopSave_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptShopSave.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ltrLink As Literal = CType(e.Item.FindControl("ltrLink"), Literal)
            Dim tab As ShopSaveRow = e.Item.DataItem
            If tab.Url = String.Empty Then
                ltrLink.Text = "<a href=""/shop-now/" & HttpUtility.UrlEncode(RewriteUrl.ReplaceUrl(tab.Name.ToLower())) & "/" & tab.ShopSaveId & """>" & imgShopsave & "<p>" & tab.Name & "</p></a>"
            Else
                ltrLink.Text = "<a href=""" & tab.Url & """>" & imgShopsave & "<p>" & tab.Name & "</p></a>"
            End If
            ltrLink.Text = String.Format(ltrLink.Text, tab.HomeBanner, tab.Name)
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            setMeataTag()
            LoadData()
        End If
    End Sub
    Private Sub setMeataTag()
        Dim row As ContentToolPageRow = ContentToolPageRow.GetRowByURL("/store/main-shop-save.aspx")
        If row.PageId > 0 Then
            Dim objMeta As New MetaTag
            objMeta.PageTitle = row.Title
            objMeta.MetaDescription = row.MetaDescription
            objMeta.MetaKeywords = row.MetaKeywords
            Dim page As System.Web.UI.Page = Me.Page
            Dim sp As New SitePage()
            objMeta.Canonical = Utility.ConfigData.GlobalRefererName & "deals-center"
            SetPageMetaSocialNetwork(page, objMeta)
        End If
    End Sub
End Class

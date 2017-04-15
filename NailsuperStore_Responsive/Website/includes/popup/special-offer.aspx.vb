Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Namespace Components

    Partial Class Popup_SpecialOffer
        Inherits SitePage
        Protected Promotion As PromotionRow
        Private mixmatchId As Integer = 0
        Public MediaUrl As String = Utility.ConfigData.MediaUrl
        Private isAllowLink As Boolean = False
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            If Not Page.IsPostBack Then
                isAllowLink = True

                If (Request.UrlReferrer IsNot Nothing AndAlso Request.UrlReferrer.OriginalString.Contains("/store/revise-cart.aspx")) Then
                    isAllowLink = False
                End If

                If Not Request("mmId") Is Nothing AndAlso IsNumeric(Request("mmId")) Then
                    mixmatchId = Request("mmId")

                    If mixmatchId > 0 Then
                        LoadDataBonousOffer(mixmatchId)
                    End If
                End If
            End If
        End Sub

        Protected Sub rptPurchase_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptPurchase.ItemDataBound, prtDiscount.ItemDataBound
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim ltrItem As System.Web.UI.WebControls.Literal = DirectCast(e.Item.FindControl("ltrItem"), System.Web.UI.WebControls.Literal)
                If Not ltrItem Is Nothing Then
                    Dim drv As DataRowView = e.Item.DataItem
                    Dim img As String = drv("image")
                    Dim itemId As Integer = drv("ItemId")
                    Dim urlCode As String = drv("URLCode")
                    Dim itemName As String = drv("itemname")
                    Dim link As String = URLParameters.ProductUrl(urlCode, itemId)

                    ltrItem.Text = "<a class='" & IIf(isAllowLink, "", "adiable") & "' href='javascript:void(0);' onclick=""gotoLinkPopupReviseFreeItem('" & link & "')"">"
                    If (String.IsNullOrEmpty(img)) Then
                        img = Utility.ConfigData.NoImageItem
                    End If
                    ltrItem.Text &= "<img src='" & MediaUrl & "/items/featured/" & img & "' alt='" & itemName & "'/>"
                    ltrItem.Text &= "</a>"
                    ltrItem.Text &= "<div class='name'><a class='" & IIf(isAllowLink, "", "adiable") & "' href='javascript:void(0);' onclick=""gotoLinkPopupReviseFreeItem('" & link & "')"">"
                    ltrItem.Text &= itemName & "</a></div>"
                End If
            End If
        End Sub

        Public Sub LoadDataBonousOffer(ByVal mmId As Integer)
            Try
                Promotion = PromotionRow.GetDistinctRow(DB, mmId)
                If Promotion Is Nothing Then
                    Exit Sub
                End If
                Dim MixMatch As MixMatchRow = DataLayer.MixMatchRow.GetRow(DB, mmId)
                If MixMatch Is Nothing Then
                    Exit Sub
                End If
                Dim dv As DataView = New DataView
                dv.Table = Promotion.PurchaseItems.Table.Copy
                dv.RowFilter = "(percentoff = 0 or percentoff is null) and (setprice = 0 or setprice is null)"
                If dv.Count > 0 Then
                    divPurchase.Visible = True
                    dv.Sort = "itemname"
                    rptPurchase.DataSource = dv
                    rptPurchase.DataBind()
                Else
                    divPurchase.Visible = False
                End If

                If Promotion.Type = PromotionType.LeastExpensive Then
                    divDiscount.Visible = False
                Else
                    dv.Table = Promotion.PurchaseItems.Table.Copy
                    dv.RowFilter = "percentoff > 0 or setprice > 0"
                    Dim dv2 As New DataView
                    dv2.Table = Promotion.GetItems.Table.Copy
                    If dv.Count > 0 Then
                        For i As Integer = 0 To dv.Count - 1
                            Dim tmp As New DataView
                            tmp.Table = dv2.Table.Copy
                            tmp.RowFilter = "itemid = " & dv(i)("ItemId")
                            If tmp.Count = 0 Then
                                Dim r As DataRow = dv2.Table.NewRow
                                r("ItemId") = dv(i)("ItemId")
                                r("PercentOff") = dv(i)("PercentOff")
                                r("SetPrice") = dv(i)("SetPrice")
                                r("ItemName") = System.Web.HttpUtility.UrlEncode(dv(i)("ItemName").ToString.Replace("/", "_"))
                                r("ItemName") = RewriteUrl.ReplaceUrl(r("ItemName"))
                                r("ItemNameNew") = dv(i)("ItemNameNew")
                                If dv(i)("ItemNameNew").ToString <> "" And IsDBNull(dv(i)("ItemNameNew")) = False Then
                                    r("ItemName") = System.Web.HttpUtility.UrlEncode(dv(i)("ItemNameNew").ToString.Replace("/", "_"))
                                    r("ItemName") = RewriteUrl.ReplaceUrl(r("ItemName"))
                                End If
                                r("Image") = dv(i)("Image")
                                r("SKU") = dv(i)("SKU")
                                r("Price") = dv(i)("Price")
                                r("HighPrice") = dv(i)("HighPrice")
                                r("LowPrice") = dv(i)("LowPrice")
                                r("DepartmentId") = dv(i)("DepartmentId")
                                dv2.Table.Rows.Add(r)
                            End If
                        Next
                    End If
                    If dv2.Count > 0 Then
                        dv2.Sort = "itemname"
                        prtDiscount.DataSource = dv2
                        prtDiscount.DataBind()
                        divDiscount.Visible = True
                    Else
                        divDiscount.Visible = False
                    End If
                End If
            Catch ex As Exception
            End Try

        End Sub
    End Class
End Namespace
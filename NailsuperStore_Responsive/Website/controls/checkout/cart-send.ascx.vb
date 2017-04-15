Imports Components
Imports DataLayer
Partial Class controls_checkout_cart_send
    Inherits BaseControl

    Private c As ShoppingCart
    Private _db As Database
    Private o As StoreOrderRow
    Private m_Member As MemberRow
    Protected LiftGateService As Double
    Protected InsideDeliveryService As Double
    Protected CODThreshold As Double
    Protected CODFee As Double
    Protected EbayOrder As Boolean = False
    Public m_LoggedInName As String = String.Empty
    Public weebRoot As String = System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName")
    Public Property Cart() As ShoppingCart
        Get
            Return c
        End Get
        Set(ByVal value As ShoppingCart)
            c = value
        End Set
    End Property

    Public Property Dab() As Database
        Get
            Return _db
        End Get
        Set(ByVal value As Database)
            _db = value
        End Set
    End Property
    Private Function CheckEbayOrder(ByVal orderNo As String) As Boolean
        Dim prefix As String = ""
        Try
            prefix = orderNo.Substring(0, 1)
        Catch ex As Exception

        End Try
        If (prefix = "E") Then
            Return True
        End If
        Return False
    End Function
    Public isShipInt As Boolean = False
    Protected sUrl As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If c Is Nothing Then Exit Sub

        If weebRoot = "" Then
            weebRoot = "https://www.nss.com/"
        End If

        CODThreshold = SysParam.GetValue("FreeCODThreshold")
        CODFee = SysParam.GetValue("CODFee")

        o = c.Order
        sUrl = weebRoot & "/store/cart.aspx?oid=" & o.OrderId
        m_LoggedInName = c.Order.BillToName & " " & c.Order.BillToName2
       
        LiftGateService = SysParam.GetValue("LiftGateService")
        InsideDeliveryService = SysParam.GetValue("InsideDeliveryService")

        If Not IsPostBack Then
            LoadFromDb()
        End If

    End Sub
    Private Function CheckHTMLAddress(ByVal value As String) As String
        Dim index As Integer = value.IndexOf("<br />")
        Dim result As String = value
        If (index = 0) Then
            result = value.Substring(6, value.Length - 6)
        End If
        Return result
    End Function
    Private Sub LoadFromDb()
        isShipInt = MemberRow.CheckMemberIsInternational(o.MemberId, o.OrderId)
        rptCartItems.DataSource = c.GetCartItems()
        rptCartItems.DataBind()



        Dim amt As Double = o.RawPriceDiscountAmount()
        Dim MerchSubTotal As Double = o.BaseSubTotal() + IIf(amt > 0, amt, 0)
        Dim subtotal As Double = o.SubTotal
        lblMerchSubTotal.Text = FormatCurrency(MerchSubTotal, 2)
        If Cart.Order.TotalDiscount > 0 OrElse amt > 0 Then lblPromotionalDiscount.Text = "-" & FormatCurrency(Cart.Order.TotalDiscount + IIf(amt > 0, amt, 0), 2) Else trPromotionalDiscount.Visible = False
        lblSubTotal.Text = FormatCurrency(o.SubTotal)
        If Utility.Common.CheckShippingInternational(DB, o) = True Then
            If o.ShipToCountry <> "PR" And o.ShipToCountry <> "VI" And o.ShipToCountry <> "HI" And o.ShipToCountry <> "AK" Then
                litInter.Visible = True
                litInter.Text = "<tr><td colpan=""2""><span style=""font-size: 12px;color: #be048d;line-height: 14px;"">Taxes & duties are not included</span><td></tr>"
            End If
        End If
     
     
    End Sub
    Private Sub rptCartItems_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptCartItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim ci As StoreCartItemRow = e.Item.DataItem
                Dim lnkImg As Literal = CType(e.Item.FindControl("lnkImg"), Literal)
                Dim sm As ShipmentMethodRow = ShipmentMethodRow.GetRow(DB, ci.CarrierType)
                Dim litSelected As Literal = CType(e.Item.FindControl("litSelected"), Literal)
                Dim litDetails As Literal = CType(e.Item.FindControl("litDetails"), Literal)
                Dim litCoupon As Literal = e.Item.FindControl("litCoupon")
                If ci.CouponMessage <> Nothing Then
                    Dim strMsg As String = ci.CouponMessage
                    If strMsg.Contains("This promotion has a minimum") Or strMsg.Contains("This promotion has a maximum") Or strMsg.Contains("The promotion code you entered") Then
                        litCoupon.Text = ""
                    Else
                        litCoupon.Text = "<div style=""color: #bb0008; font:12px/18px Open Sans;"">" & ci.CouponMessage & "</div>"
                    End If
                End If
                Dim itemCode As String = DB.ExecuteScalar("Select  URLCode from StoreItem where ItemId=" & ci.ItemId & "")
                Dim sURL As String = weebRoot & URLParameters.ProductUrl(itemCode, ci.ItemId) '' String.Format("/nail-products/{0}", itemCode)

                litDetails.Text = "<div><b><a href='" & sURL & "' style='text-decoration:none;color:#333333;'>" & Server.HtmlEncode(IIf(ci.IsFreeSample = True, ci.ItemName, ci.ItemName)) & "</a></b></div>"
                litDetails.Text &= "<div style=""margin-top:2px;"">Item# " & ci.SKU & "</div>" & IIf(ci.Attributes <> Nothing, "<div style=""color:#BE048D;line-height: 14px"">" & ci.AttributeSKU & "</div>", "") & vbCrLf
                If ci.IsFreeItem AndAlso Not String.IsNullOrEmpty(ci.FreeItemIds) Then
                    Dim warningMsg As String = String.Empty
                    If isShipInt AndAlso ci.IsFlammable Then
                        warningMsg = Resources.Msg.FreeItemFlammable
                    End If
                    If Not String.IsNullOrEmpty(ci.CouponMessage) Then
                        warningMsg = IIf(String.IsNullOrEmpty(warningMsg), ci.CouponMessage, warningMsg & "<br/>" & ci.CouponMessage)
                    End If
                    litDetails.Text &= "<div style=""margin-top:2px;color:red"">" & warningMsg & "</div>"


                End If

                If ci.Swatches <> Nothing Then litDetails.Text &= "<div style=""margin-top:6px;font-size: 12px;"" >" & ci.Swatches.Replace(vbCrLf, "<br />") & "</div>"
                'If ci.Attributes <> Nothing Then litDetails.Text &= "<div style=""margin-top:6px;font-size: 11px;"" >" & ci.Attributes.Replace(vbCrLf, "<br />") & "</div>"
                'litDetails.Text &= "<div style=""margin-top:6px;"" class=""mag"">" & p.Text & "</div>"
                Dim freeText As String = String.Empty
                If ci.IsFreeItem Then
                    If Not ci.FreeItemIds Is Nothing Then
                        If ci.Total > 0 Then
                            Dim mmdesc As String = MixMatchRow.GetRow(DB, ci.MixMatchId).Description
                            litDetails.Text = "<b style='color: #be048d;line-height: 14px;'>" & mmdesc & "</b><br />" & litDetails.Text
                        Else
                            litDetails.Text = "<b style='color: #be048d;line-height: 14px;'>FREE Item</b><br />" & litDetails.Text
                        End If

                    Else

                        If ci.PromotionID > 0 Then
                            Dim objPromotion As StorePromotionRow = StorePromotionRow.GetRow(DB, ci.PromotionID)
                            If Not objPromotion Is Nothing Then
                                If Not objPromotion.IsProductCoupon Then
                                    freeText = "FREE Item"
                                End If
                            End If
                        End If
                        litDetails.Text = "<b style='color: #be048d;line-height: 14px;'>" & freeText & "</b><br />" & litDetails.Text
                    End If
                End If
                If ci.IsFreeGift Then
                    freeText = "FREE Gift"
                    litDetails.Text = "<b style='color: #be048d;line-height: 14px;'>" & freeText & "</b><br />" & litDetails.Text
                End If
                lnkImg.Text = "<a href='" & sURL & "'><img border=""0"" style=""border-style:none;"" src="""
                lnkImg.Text &= IIf(ci.Image <> Nothing, weebRoot & "/assets/items/cart/" & ci.Image, weebRoot & "/assets/items/cart/na.jpg")
                lnkImg.Text &= """></a>"

                '''''''''''''''''''''''''''''''''''''
                Dim ltrTotal As Literal = e.Item.FindControl("ltrTotal")
                ltrTotal.Text = FormatCurrency(ci.Total)

            End If
        End If
    End Sub
End Class


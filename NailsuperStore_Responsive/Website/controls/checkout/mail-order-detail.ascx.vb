

Imports Components
Imports DataLayer
Partial Class controls_checkout_mail_order_detail
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If c Is Nothing Then Exit Sub
        'Cart.RecalculateTotalUpdate()
        Dim fromMobile As String = String.Empty
        Try
            fromMobile = Request.QueryString("mb")
        Catch ex As Exception

        End Try
        If weebRoot = "" Then
            weebRoot = "https://www.nss.com/"
        End If
        CODThreshold = SysParam.GetValue("FreeCODThreshold")
        CODFee = SysParam.GetValue("CODFee")

        o = c.Order
        m_LoggedInName = c.Order.BillToName & " " & c.Order.BillToName2
        If o.OrderNo <> Nothing Then
            lit.Text = o.OrderNo
        End If
        EbayOrder = CheckEbayOrder(o.OrderNo)
        If Not EbayOrder Then
            ltrShipToTitle.Text = "Ship to:"
            ltrBillingToTitle.Text = "Bill to:"
        Else
            ltrShipToTitle.Text = "Ship to:"
            ltrEbayCustomerMail.Text = o.Email
            ltrEbayOrderData.Text = String.Format("{0:g}", o.OrderDate)
        End If
        LiftGateService = SysParam.GetValue("LiftGateService")
        InsideDeliveryService = SysParam.GetValue("InsideDeliveryService")
        If o.Notes <> Nothing Then
            divNotesHead.Visible = True
            divNotes.InnerHtml = o.Notes.Replace("\r\n", "<br/>").Replace("\n", "<br/>")
        Else
            divNotesHead.Visible = False
            divNotes.Visible = False
        End If
        If (fromMobile = "1") Then
            '' divComment.InnerText = "Order via mobile"
            divComment.Visible = True
        Else
            divComment.Visible = False
        End If
        litBilling.Text = Utility.Common.GetHTMLMailOrderBillingAddress(DB, o)
        If o.IsSameAddress Then
            litShipping.Text = litBilling.Text
        Else
            litShipping.Text = Utility.Common.GetHTMLMailOrderShippingAddress(DB, o)
        End If

        If Left(Request.Path, 7) = "/admin/" Then
            If c.Order.MemberId <> Nothing Then
                litBilling.Text &= "<br /><br /><a href=""/admin/members/edit.aspx?MemberId=" & c.Order.MemberId & """>View Customer " & DB.ExecuteScalar("select top 1 customerno from customer where customerid = " & c.Order.SellToCustomerId) & "</a>"
            Else
                litBilling.Text &= "<br /><br />Customer " & DB.ExecuteScalar("select top 1 customerno from customer where customerid = " & c.Order.SellToCustomerId) & " <span style=""font-weight:normal"">(not a web member)</span>"
            End If
        End If

        Try
            If o.OrderNo <> Nothing Then
                dvOrderNo.Visible = True
                dvNotOrderNo.Visible = False
            Else
                dvOrderNo.Visible = False
                dvNotOrderNo.Visible = True
                litBillingNotOrder.Text = litBilling.Text
            End If
        Catch ex As Exception

        End Try

        If Not IsPostBack Then
            LoadFromDb()
        End If
        litBilling.Text = CheckHTMLAddress(litBilling.Text)
        litShipping.Text = CheckHTMLAddress(litShipping.Text)

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
        If (o.Tax > 0) Then
            lblSalesTax.Text = FormatCurrency(o.Tax, 2)
            trSalesTax.Visible = True
        Else
            trSalesTax.Visible = False
        End If

        'Long add show coupon discount
        If Cart.CoupontDiscount > 0 Then
            trCouponDiscount.Visible = True
            lblCouponDiscount.Text = "-" & FormatCurrency(Cart.CoupontDiscount, 2)
        End If
        lblSubTotal.Text = FormatCurrency(o.Total)

        Dim HasOversize As Boolean = c.HasOversizeItems()
        If HasOversize Then
            divOversize.Visible = True
        Else
            divOversize.Visible = False
        End If

        litShippingDetails.Text = c.GetMailShippingLines()

        Dim exempt As Double = c.GetAmountExempt()
        lblSubjectToTax.Text = FormatCurrency(subtotal - exempt, 2)
        lblExempt.Text = FormatCurrency(exempt, 2)
        'lblExempt.Text = FormatCurrency(exempt, 2)

        If Utility.Common.CheckShippingInternational(DB, o) = True Then
            If o.ShipToCountry <> "PR" And o.ShipToCountry <> "VI" And o.ShipToCountry <> "HI" And o.ShipToCountry <> "AK" And Not (o.ShipToCountry = "US" AndAlso o.ShipToCounty = "HI") And Not (o.ShipToCountry = "US" AndAlso o.ShipToCounty = "AK") Then
                litInter.Visible = True
                litInter.Text = "<tr><td colpan=""2""><span style=""font-size: 12px;color: #be048d;line-height: 14px;"">Taxes & duties are not included</span><td></tr>"
            End If
        End If
        Select Case c.Order.PaymentType
            Case "CC"
                litPayment.Text = "Payment Method: <span style=""color: #b70072; font: bold 14px Open Sans;"">Credit Card</span><br />"
                litPayment.Text &= "Name on Card: <span style=""color: #b70072; font: bold 14px Open Sans;"">" & c.Order.CardHolderName & "</span><br />"
                litPayment.Text &= "Card Type: <span style=""color: #b70072; font: bold 14px Open Sans;"">" & CardTypeRow.GetRow(DB, c.Order.CardTypeId).Name & "</span><br />"
                litPayment.Text &= "Card Number: <span style=""color: #b70072; font: bold 14px Open Sans;"">" & c.Order.StarredCardNumber & "</span><br />"
                litPayment.Text &= "Expiration Date: <span style=""color: #b70072; font: bold 14px Open Sans;"">" & IIf(c.Order.ExpirationDate.Month.ToString.Length = 1, "0", "") & c.Order.ExpirationDate.Month & "/" & c.Order.ExpirationDate.Year & "</span><br />"
            Case "CHECK"
                litPayment.Text = "Payment Method: <span style=""color: #b70072; font: bold 14px Open Sans;"">Check</span><br />"
                litPayment.Text &= "Bank Name: <span style=""color: #b70072; font: bold 14px Open Sans;"">" & c.Order.BankName & "</span><br />"
                litPayment.Text &= "Routing Number: <span style=""color: #b70072; font: bold 14px Open Sans;"">" & c.Order.StarredRoutingNumber & "</span><br />"
                litPayment.Text &= "Account Type: <span style=""color: #b70072; font: bold 14px Open Sans;"">" & c.Order.AccountType & "</span><br />"
                litPayment.Text &= "Account Number: <span style=""color: #b70072; font: bold 14px Open Sans;"">" & c.Order.StarredAccountNumber & "</span><br />"
                litPayment.Text &= "Check Number: <span style=""color: #b70072; font: bold 14px Open Sans;"">" & c.Order.CheckNumber & "</span><br />"
                litPayment.Text &= "Drivers License #: <span style=""color: #b70072; font: bold 14px Open Sans;"">" & c.Order.DLNumber & "</span><br />"
            Case "COD"
                litPayment.Text = "Payment Method: <span style=""color: #b70072; font: bold 14px Open Sans;"">COD</span><br />"
                trCODFee.Visible = True
                lblCODFee.Text = FormatCurrency(IIf(c.Order.Total >= CODThreshold AndAlso Not c.Order.Total - CODFee < CODThreshold, 0, CODFee))
            Case "PAYPAL"
                litPayment.Text = "Payment Method: <span style=""color: #b70072; font: bold 14px Open Sans;"">PayPal</span><br />"
            Case Else
                litPayment.Text = "Payment Method: <span style=""color: #b70072; font: bold 14px Open Sans;""><i>Invalid Payment Method</i></span><br />"
        End Select

        If exempt > 0 Then
            litPayment.Text &= "Tax Exempt Id: <span style=""color: #b70072; font: bold 14px Open Sans;"">" & c.Order.TaxExemptId & "</span><br />"
        End If
        If (o.ShipToAddressType = 2 Or o.ShipToAddressType = 0) And o.ResidentialFee > 0 Then
            trResidential.Visible = True
            lblResidentil.Text = FormatCurrency(o.ResidentialFee, 2)
        Else
            trResidential.Visible = False
        End If
        If o.PurchasePoint > 0 Then
            lblPurchasePoint.Text = "-" & FormatCurrency(o.TotalPurchasePoint)
        Else
            trPoint.Visible = False
        End If
        If o.PointAmountDiscount > 0 Then
            lblDiscPoint.Text = "-" & FormatCurrency(o.PointAmountDiscount)
            lblMLevelPoint.Text = o.PointLevelMessage
        Else
            trLevelPoint.Visible = False
        End If
        If o.TotalSpecialHandlingFee > 0 Then
            lblHandlingFee.Text = FormatCurrency(o.TotalSpecialHandlingFee, 2)
        Else
            trHandlingFee.Visible = False
        End If
        If o.HazardousMaterialFee > 0 Then
            lblHazardousMaterialFee.Text = FormatCurrency(o.HazardousMaterialFee, 2)
        Else
            trHazardousMaterialFee.Visible = False
        End If
    End Sub

    Private Function GetPaymentType(ByVal type As String) As String
        Select Case type
            Case "CC"
                Return "Credit Card"
            Case "CHECK"
                Return "Check"
            Case "COD"
                Return type
            Case "PAYPAL"
                Return "PayPal"
            Case Else
                Return ""
        End Select
    End Function

    Private Sub rptCartItems_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptCartItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim ci As StoreCartItemRow = e.Item.DataItem
                Dim lnkImg As Literal = CType(e.Item.FindControl("lnkImg"), Literal)
                Dim sm As ShipmentMethodRow = ShipmentMethodRow.GetRow(DB, ci.CarrierType)
                Dim imgShipping As WebControls.Image = CType(e.Item.FindControl("imgShipping"), WebControls.Image)
                Dim litSelected As Literal = CType(e.Item.FindControl("litSelected"), Literal)
                Dim pnlSelect As Panel = CType(e.Item.FindControl("pnlSelect"), Panel)
                Dim rbLiftGate As RadioButton = CType(e.Item.FindControl("rbLiftGate"), RadioButton)
                Dim rbInsideDelivery As RadioButton = CType(e.Item.FindControl("rbInsideDelivery"), RadioButton)
                Dim rbAppointment As RadioButton = CType(e.Item.FindControl("rbAppointment"), RadioButton)
                Dim litDetails As Literal = CType(e.Item.FindControl("litDetails"), Literal)
                Dim pnlRush As Panel = e.Item.FindControl("pnlRush")
                'Dim chkIsRushDelivery As CheckBox = e.Item.FindControl("chkIsRushDelivery")
                Dim ltlrush As Literal = e.Item.FindControl("ltlrush")
                Dim pnlOversize As Panel = e.Item.FindControl("pnlOversize")
                'Dim chkIsLiftGate As CheckBox = e.Item.FindControl("chkIsLiftGate")
                Dim ltlLiftGate As Literal = e.Item.FindControl("ltlLiftGate")
                Dim ltlQtyShipped As Literal = e.Item.FindControl("ltlQtySHipped")
                Dim litUnit As Literal = e.Item.FindControl("litUnit")
                Dim trLiftGate As HtmlTableRow = e.Item.FindControl("trLiftGate")
                Dim trInsideDelivery As HtmlTableRow = e.Item.FindControl("trInsideDelivery")
                Dim trScheduleDelivery As HtmlTableRow = e.Item.FindControl("trScheduleDelivery")
                'Dim chkScheduleDelivery As CheckBox = e.Item.FindControl("chkScheduleDelivery")
                Dim ltlScheduleDelivery As Literal = e.Item.FindControl("ltlScheduleDelivery")
                ' Dim chkInsideDelivery As CheckBox = e.Item.FindControl("chkInsideDelivery")
                Dim ltlInsideDelivery As Literal = e.Item.FindControl("ltlInsideDelivery")
                Dim litCoupon As Literal = e.Item.FindControl("litCoupon")
                'ltlQtyShipped.Text = DB.ExecuteScalar("select top 1 totalqtyshipped from storeordershipmentline where cartitemid = " & ci.CartItemId)
                Dim SQL As String = "SELECT coalesce(SUM(coalesce(totalqtyshipped,0)),0) FROM StoreOrderShipmentLine WHERE OrderId = " & ci.OrderId & " AND CartItemid = " & ci.CartItemId
                ltlQtyShipped.Text = DB.ExecuteScalar(SQL)
                If ltlQtyShipped.Text = Nothing Then ltlQtyShipped.Text = "&nbsp;"

                pnlRush.Visible = ci.IsRushDelivery
                Dim surcharge As Double = Nothing
                If pnlRush.Visible Then
                    ltlrush.Text = "You requested rush delivery on this product. " & IIf(ci.RushDeliveryCharge > 0, " A surcharge of " & FormatCurrency(ci.RushDeliveryCharge) & " was added to shipping costs for this item.", "")
                End If

                pnlOversize.Visible = ci.IsOversize
                If pnlOversize.Visible Then
                    trLiftGate.Visible = ci.IsLiftGate
                    If trLiftGate.Visible Then
                        surcharge = SysParam.GetValue("LiftGateCharge")
                        ltlLiftGate.Text = "You requested a lift gate for this product. " & IIf(surcharge > 0, "A single surcharge of " & FormatCurrency(surcharge) & " was added to shipping costs for your order.", "")
                    End If

                    trInsideDelivery.Visible = ci.IsInsideDelivery
                    If trInsideDelivery.Visible Then
                        surcharge = SysParam.GetValue("InsideDeliveryService")
                        ltlInsideDelivery.Text = "You requested an inside delivery. " & IIf(surcharge > 0, " A single surcharge of " & FormatCurrency(surcharge) & " was added to your order.", "")
                    End If

                    trScheduleDelivery.Visible = ci.IsScheduleDelivery
                    If trScheduleDelivery.Visible Then
                        surcharge = SysParam.GetValue("ScheduleDeliveryCharge")
                        ltlScheduleDelivery.Text = "You requested a scheduled delivery. " & IIf(surcharge > 0, " A single surcharge of " & FormatCurrency(surcharge) & " was added to your order.", "")
                    End If

                End If
                If ci.CouponMessage <> Nothing Then
                    Dim strMsg As String = ci.CouponMessage
                    If strMsg.Contains("This promotion has a minimum") Or strMsg.Contains("This promotion has a maximum") Or strMsg.Contains("The promotion code you entered") Then
                        litCoupon.Text = ""
                    Else
                        litCoupon.Text = "<div style=""color: #bb0008; font:12px/18px Open Sans;"">" & ci.CouponMessage & "</div>"
                    End If
                End If
                Dim UrlCode As String = StoreItemRow.GetRowURLCodeById(ci.ItemId)
                Dim sURL As String = weebRoot
                If String.IsNullOrEmpty(UrlCode) Then
                    sURL &= URLParameters.ProductUrl(URLParameters.ReplaceUrl(HttpUtility.UrlEncode(ci.ItemName.ToLower())), ci.ItemId)
                Else
                    sURL &= URLParameters.ProductUrl(UrlCode, ci.ItemId)
                End If

                litDetails.Text = "<div><b><a href='" & sURL & "' style='text-decoration:none;color:#333333;'>" & Server.HtmlEncode(ci.ItemName) & "</a></b></div>"
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

                If ci.AddType = "2" Then
                    Dim si As StoreItemRow = StoreItemRow.GetRow(DB, ci.ItemId, 0, True, False)
                    litUnit.Text &= "Case of " & si.CaseQty
                Else
                    litUnit.Text = ci.PriceDesc
                End If
                If ci.Swatches <> Nothing Then litDetails.Text &= "<div style=""margin-top:6px;font-size: 12px;"" >" & ci.Swatches.Replace(vbCrLf, "<br />") & "</div>"
                If ci.IsFreeItem = True Then
                    If Not ci.FreeItemIds Is Nothing Then
                        If ci.LineDiscountAmount < ci.SubTotal Then
                            If String.IsNullOrEmpty(ci.MixMatchDescription) Then
                                litDetails.Text = "<b style='color: #be048d;line-height: 14px;'>Discount Item</b><br />" & litDetails.Text
                            Else
                                litDetails.Text = "<b style='color: #be048d;line-height: 14px;'>" & ci.MixMatchDescription & "</b><br />" & litDetails.Text
                            End If
                        Else
                            litDetails.Text = "<b style='color: #be048d;line-height: 14px;'>FREE Item</b><br />" & litDetails.Text
                        End If
                    Else
                        Dim freeText As String = String.Empty
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
                ElseIf ci.IsFreeGift Then
                    litDetails.Text = "<b style='color: #be048d;line-height: 14px;'>FREE Gift</b><br />" & litDetails.Text
                End If

                If sm.Image <> Nothing AndAlso Cart.Order.BillToCountry = "US" Then
                    If Cart.CheckShippingSpecialUS = True Then
                        If ci.IsOversize = False Then
                            imgShipping.ImageUrl = weebRoot & "/includes/theme/images/ico_international.gif"
                        Else
                            imgShipping.ImageUrl = weebRoot & "/includes/theme/images/" & sm.Image
                        End If
                    Else
                        imgShipping.ImageUrl = weebRoot & "/includes/theme/images/" & sm.Image
                    End If

                ElseIf sm.Image <> Nothing AndAlso Cart.Order.BillToCountry <> "US" Then
                    If Cart.CheckShippingSpecialUS = True Then
                        If ci.IsOversize = False Then
                            imgShipping.ImageUrl = weebRoot & "/includes/theme/images/ico_international.gif"
                        Else
                            imgShipping.ImageUrl = weebRoot & "/includes/theme/images/" & sm.Image
                        End If
                    Else
                        imgShipping.ImageUrl = weebRoot & "/includes/theme/images/" & sm.Image
                    End If
                Else
                    imgShipping.ImageUrl = weebRoot & "/includes/theme-admin/images/spacer.gif"
                End If

                lnkImg.Text = "<a href='" & sURL & "'><img border=""0"" style=""border-style:none;"" src="""
                lnkImg.Text &= IIf(ci.Image <> Nothing, weebRoot & "/assets/items/cart/" & ci.Image, weebRoot & "/assets/items/cart/na.jpg")
                lnkImg.Text &= """></a>"

                Dim isWebTemplate As Boolean = StoreItemRow.IsWebItemplateItem(ci.SKU)
                If (isWebTemplate Or ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                    litSelected.Text = String.Empty
                    imgShipping.Visible = False
                Else
                    If Cart.Order.HazardousMaterialFee > 0 And Utility.Common.InternationalShippingId().Contains(Cart.Order.CarrierType) Then
                        imgShipping.Visible = False
                    End If

                    Dim CountryCode As String = IIf(Cart.Order.IsSameAddress, Cart.Order.BillToCountry, Cart.Order.ShipToCountry)
                    Dim shippingName As String = ShipmentMethodRow.GetRow(DB, ci.CarrierType).Name
                    If CountryCode <> "US" Then
                        If Utility.Common.InternationalShippingId.Contains(ci.CarrierType.ToString()) Then
                            litSelected.Text = "<div style=""margin-top:3px;"">International Shipping</div>"
                        Else
                            litSelected.Text = "<div style=""margin-top:3px;"">" & shippingName & "</div>"
                        End If
                    Else
                        If Cart.CheckShippingSpecialUS = True Then
                            If Utility.Common.InternationalShippingId.Contains(ci.CarrierType.ToString()) Then
                                litSelected.Text = "<div style=""margin-top:3px;"">International Shipping</div>"
                            Else
                                litSelected.Text = "<div style=""margin-top:3px;"">" & shippingName & "</div>"
                            End If
                        Else
                            litSelected.Text = "<div style=""margin-top:3px;"">" & shippingName & IIf(ci.IsFreeShipping, "<br /><span style='color:#bb0008; font-size:12px;'>FREE Shipping</span>", "") & "</div>"
                        End If
                    End If
                End If

                Dim ltrTotal As Literal = e.Item.FindControl("ltrTotal")
                If Not ltrTotal Is Nothing Then
                    If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                        ltrTotal.Text = Utility.Common.FormatPointPrice(ci.SubTotalPoint)
                    Else
                        ltrTotal.Text = FormatCurrency(ci.Total)
                    End If
                End If
            End If
        End If
    End Sub

End Class

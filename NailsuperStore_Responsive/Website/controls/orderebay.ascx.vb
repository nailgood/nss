Imports Components
Imports DataLayer
Imports Utility
Partial Class controls_orderebay
    Inherits BaseControl
    Private lstCartItem As StoreCartItemCollection
    Private c As ShoppingCart
    Private _db As Database
    Private o As StoreOrderRow
    Private m_Member As MemberRow
    Protected LiftGateService As Double
    Protected InsideDeliveryService As Double
    Protected CODThreshold As Double
    Protected CODFee As Double
    Protected EbayOrder As Boolean = False
    Public linkTracking As String = ""
    Public linkEdit As String = ""
    Public formWidth As Integer = 754
    Public m_LoggedInName As String
    Public Property Order() As StoreOrderRow
        Get
            Return o
        End Get
        Set(ByVal value As StoreOrderRow)
            o = value
        End Set
    End Property
    Public Property Cart() As ShoppingCart
        Get
            Return c
        End Get
        Set(ByVal value As ShoppingCart)
            c = value
        End Set
    End Property
    Public Property CartItemList() As StoreCartItemCollection
        Get
            Return lstCartItem
        End Get
        Set(ByVal value As StoreCartItemCollection)
            lstCartItem = value
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If c Is Nothing Then Exit Sub

        If Request.RawUrl.Contains("members/orderhistory/view.aspx") Then
            formWidth = 754
        ElseIf Request.RawUrl.Contains("store/vieworder.aspx") Then
            formWidth = 754
        End If
        'If c Is Nothing Then
        ' c = New ShoppingCart(DB, Session("adminOrderId"), True)
        ' End If
        ' Dim Cart1 As ShoppingCart = New ShoppingCart(DB, Session("adminOrderId"), True)

        CODThreshold = SysParam.GetValue("FreeCODThreshold")
        CODFee = SysParam.GetValue("CODFee")
        o = Order

        m_LoggedInName = o.BillToName & " " & o.BillToName2
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
            If Left(Request.Path, 7) = "/admin/" And c.Order.MemberId <> Nothing Then

                ltrEbayCustomerMail.Text &= "<br /><a href=""/admin/members/edit.aspx?MemberId=" & c.Order.MemberId & """>View Customer " & DB.ExecuteScalar("select top 1 customerno from customer where customerid = " & c.Order.SellToCustomerId) & "</a>"

            End If
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

        litBilling.Text = Utility.Common.GetHTMLOrderBillingAddress(DB, o)
        If o.IsSameAddress Then
            litShipping.Text = litBilling.Text
        Else
            litShipping.Text = Utility.Common.GetHTMLOrderShippingAddress(DB, o)
        End If
        If Not EbayOrder Then
            If Left(Request.Path, 7) = "/admin/" Then
                If c.Order.MemberId <> Nothing Then
                    litBilling.Text &= "<br /><br /><a href=""/admin/members/edit.aspx?MemberId=" & c.Order.MemberId & """>View Customer " & DB.ExecuteScalar("select top 1 customerno from customer where customerid = " & c.Order.SellToCustomerId) & "</a>"
                Else
                    litBilling.Text &= "<br /><br />Customer " & DB.ExecuteScalar("select top 1 customerno from customer where customerid = " & c.Order.SellToCustomerId) & " <span style=""font-weight:normal"">(not a web member)</span>"
                End If
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

        'If Not IsPostBack Then
        LoadFromDb()
        'End If
        litBilling.Text = CheckHTMLAddress(litBilling.Text)
        litShipping.Text = CheckHTMLAddress(litShipping.Text)
        Dim StrUrl As String = Request.RawUrl
        If StrUrl.Contains("/admin/store/orders/edit.aspx") Then
            GetTrackingNumber()
        Else
            linkTracking = String.Empty
        End If
    End Sub
    Public Sub GetTrackingNumber()
        Dim dt As DataTable = DB.GetDataTable("select TrackingId,trackingno from StoreOrderShipmentTracking where shipmentid in (select shipmentid from storeordershipment where orderid = " & o.OrderId & ")")
        If dt.Rows.Count = 0 Then
            dt = DB.GetDataTable("select trackingid,trackingno from StoreOrderShipmentTracking where  orderid = " & o.OrderId)
        End If
        Dim smt As ShipmentMethodRow = ShipmentMethodRow.GetRow(DB, o.CarrierType)
        Dim result As String = String.Empty
        If LCase(Left(smt.Code, 3)) = "ups" Then
            linkTracking = "<a href='http://wwwapps.ups.com/WebTracking/processRequest?HTMLVersion=5.0&Requester=NES&AgreeToTermsAndConditions=yes&loc=en_US&tracknum={0}' target='_blank' >{0}"
        ElseIf o.CarrierType = 15 Then
            linkTracking = "<a href='https://tools.usps.com/go/TrackConfirmAction?qtc_tLabels1={0}' target='_blank' >{0}"
        End If
        Dim TrackingId As Integer = 0
        Try
            If dt.Rows.Count > 0 Then
                result = dt.Rows(0)("TrackingNo").ToString()
                TrackingId = CInt(dt.Rows(0)("TrackingId"))
            End If
            linkTracking = String.Format(linkTracking, result)
            If result = "" Then
                linkTracking = "<a href ='/admin/store/orders/AddTrackingNumber.aspx?OrderId=" & o.OrderId & "'>Add Tracking Number</a>"
            Else
                linkEdit = "<a href ='/admin/store/orders/AddTrackingNumber.aspx?OrderId=" & o.OrderId & "&TrackingId=" & TrackingId & "'>Edit Tracking Number</a>"
            End If
            Exit Sub
        Catch ex As Exception

        End Try
        linkTracking = String.Empty
    End Sub
    Private Function CheckHTMLAddress(ByVal value As String) As String
        Dim index As Integer = value.IndexOf("<br />")
        Dim result As String = value
        If (index = 0) Then
            result = value.Substring(6, value.Length - 6)
        End If
        Return result
    End Function
    Private Function IsLoadCart() As Boolean
        If lstCartItem Is Nothing Then
            Return True
        End If
        If lstCartItem.Count < 1 Then
            Return True
        End If
        Return False
    End Function
    Private Sub LoadFromDb()
        If IsLoadCart() Then
            lstCartItem = c.GetCartItems()
        End If
        rptCartItems.DataSource = lstCartItem
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

        litShippingDetails.Text = c.GetShippingLines()

        Dim exempt As Double = c.GetAmountExempt()
        lblSubjectToTax.Text = FormatCurrency(subtotal - exempt, 2)
        lblExempt.Text = FormatCurrency(exempt, 2)

        If Utility.Common.CheckShippingInternational(DB, o) = True Then
            If o.ShipToCountry <> "PR" And o.ShipToCountry <> "VI" And o.ShipToCountry <> "HI" And o.ShipToCountry <> "AK" And Not (o.ShipToCountry = "US" AndAlso o.ShipToCounty = "HI") And Not (o.ShipToCountry = "US" AndAlso o.ShipToCounty = "AK") Then
                litInter.Visible = True
                litInter.Text = "<tr><td colpan=""2""><span class=""smallermag"">Taxes & duties are not included</span><td></tr>"
            End If
        End If
        Select Case c.Order.PaymentType
            Case "CC"
                litPayment.Text = "Payment Method: <span id=""mag"">Credit Card</span><br />"
                litPayment.Text &= "Name on Card: <span id=""mag"">" & c.Order.CardHolderName & "</span><br />"
                litPayment.Text &= "Card Type: <span id=""mag"">" & CardTypeRow.GetRow(DB, c.Order.CardTypeId).Name & "</span><br />"
                litPayment.Text &= "Card Number: <span id=""mag"">" & c.Order.StarredCardNumber & "</span><br />"
                litPayment.Text &= "Expiration Date: <span id=""mag"">" & IIf(c.Order.ExpirationDate.Month.ToString.Length = 1, "0", "") & c.Order.ExpirationDate.Month & "/" & c.Order.ExpirationDate.Year & "</span><br />"
            Case "CHECK"
                litPayment.Text = "Payment Method: <span id=""mag"">Check</span><br />"
                litPayment.Text &= "Bank Name: <span id=""mag"">" & c.Order.BankName & "</span><br />"
                litPayment.Text &= "Routing Number: <span id=""mag"">" & c.Order.StarredRoutingNumber & "</span><br />"
                litPayment.Text &= "Account Type: <span id=""mag"">" & c.Order.AccountType & "</span><br />"
                litPayment.Text &= "Account Number: <span id=""mag"">" & c.Order.StarredAccountNumber & "</span><br />"
                litPayment.Text &= "Check Number: <span id=""mag"">" & c.Order.CheckNumber & "</span><br />"
                litPayment.Text &= "Drivers License #: <span id=""mag"">" & c.Order.DLNumber & "</span><br />"
            Case "COD"
                litPayment.Text = "Payment Method: <span id=""mag"">COD</span><br />"
                trCODFee.Visible = True
                lblCODFee.Text = FormatCurrency(IIf(c.Order.Total >= CODThreshold AndAlso Not c.Order.Total - CODFee < CODThreshold, 0, CODFee))
            Case "PAYPAL"
                litPayment.Text = "Payment Method: <span id=""mag"">PayPal</span><br />"
            Case Else
                litPayment.Text = "Payment Method: <span id=""mag""><i>Invalid Payment Method</i></span><br />"
        End Select

        If exempt > 0 Then
            litPayment.Text &= "Tax Exempt Id: <span id=""mag"">" & c.Order.TaxExemptId & "</span><br />"
        End If
        If (o.ShipToAddressType = 2 Or o.ShipToAddressType = 0) And o.ResidentialFee > 0 Then
            trResidential.Visible = True
            lblResidentil.Text = FormatCurrency(o.ResidentialFee, 2)
        Else
            trResidential.Visible = False
        End If
        If o.PurchasePoint > 0 Then
            lblPurchasePoint.Text = "-" & FormatCurrency(Cart.Order.TotalPurchasePoint)
        Else
            trPoint.Visible = False
        End If
        If o.PointAmountDiscount > 0 Then
            lblDiscPoint.Text = "-" & FormatCurrency(Cart.Order.PointAmountDiscount)
            lblMLevelPoint.Text = o.PointLevelMessage
        Else
            trLevelPoint.Visible = False
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
                Dim chkIsRushDelivery As CheckBox = e.Item.FindControl("chkIsRushDelivery")
                Dim ltlrush As Literal = e.Item.FindControl("ltlrush")
                Dim pnlOversize As Panel = e.Item.FindControl("pnlOversize")
                Dim chkIsLiftGate As CheckBox = e.Item.FindControl("chkIsLiftGate")
                Dim ltlLiftGate As Literal = e.Item.FindControl("ltlLiftGate")
                Dim ltlQtyShipped As Literal = e.Item.FindControl("ltlQtySHipped")
                Dim trLiftGate As HtmlTableRow = e.Item.FindControl("trLiftGate")
                Dim trInsideDelivery As HtmlTableRow = e.Item.FindControl("trInsideDelivery")
                Dim trScheduleDelivery As HtmlTableRow = e.Item.FindControl("trScheduleDelivery")
                Dim chkScheduleDelivery As CheckBox = e.Item.FindControl("chkScheduleDelivery")
                Dim ltlScheduleDelivery As Literal = e.Item.FindControl("ltlScheduleDelivery")
                Dim chkInsideDelivery As CheckBox = e.Item.FindControl("chkInsideDelivery")
                Dim ltlInsideDelivery As Literal = e.Item.FindControl("ltlInsideDelivery")
                Dim litCoupon As Literal = e.Item.FindControl("litCoupon")
                'ltlQtyShipped.Text = DB.ExecuteScalar("select top 1 totalqtyshipped from storeordershipmentline where cartitemid = " & ci.CartItemId)
                Dim SQL As String = "SELECT coalesce(SUM(coalesce(totalqtyshipped,0)),0) FROM StoreOrderShipmentLine WHERE OrderId = " & ci.OrderId & " AND CartItemid = " & ci.CartItemId
                ltlQtyShipped.Text = DB.ExecuteScalar(SQL)
                If ltlQtyShipped.Text = Nothing Then ltlQtyShipped.Text = "&nbsp;"

                pnlRush.Visible = ci.IsRushDelivery
                Dim surcharge As Double = Nothing
                If pnlRush.Visible Then
                    chkIsRushDelivery.Checked = True
                    chkIsRushDelivery.Enabled = False
                    ltlrush.Text = "You requested rush delivery on this product. " & IIf(ci.RushDeliveryCharge > 0, " A surcharge of " & FormatCurrency(ci.RushDeliveryCharge) & " was added to shipping costs for this item.", "")
                End If

                pnlOversize.Visible = ci.IsOversize
                If pnlOversize.Visible Then
                    trLiftGate.Visible = ci.IsLiftGate
                    If trLiftGate.Visible Then
                        surcharge = SysParam.GetValue("LiftGateCharge")
                        chkIsLiftGate.Checked = True
                        chkIsLiftGate.Enabled = False
                        ltlLiftGate.Text = "You requested a lift gate for this product. " & IIf(surcharge > 0, "A single surcharge of " & FormatCurrency(surcharge) & " was added to shipping costs for your order.", "")
                    End If

                    trInsideDelivery.Visible = ci.IsInsideDelivery
                    If trInsideDelivery.Visible Then
                        surcharge = SysParam.GetValue("InsideDeliveryService")
                        chkInsideDelivery.Checked = True
                        chkInsideDelivery.Enabled = False
                        ltlInsideDelivery.Text = "<label for=""" & chkInsideDelivery.ClientID & """>You requested an inside delivery. " & IIf(surcharge > 0, " A single surcharge of " & FormatCurrency(surcharge) & " was added to your order.", "") & "</label>"
                    End If

                    trScheduleDelivery.Visible = ci.IsScheduleDelivery
                    If trScheduleDelivery.Visible Then
                        surcharge = SysParam.GetValue("ScheduleDeliveryCharge")
                        chkScheduleDelivery.Checked = True
                        chkScheduleDelivery.Enabled = False
                        ltlScheduleDelivery.Text = "<label for=""" & chkScheduleDelivery.ClientID & """>You requested a scheduled delivery. " & IIf(surcharge > 0, " A single surcharge of " & FormatCurrency(surcharge) & " was added to your order.", "") & "</label>"
                    End If

                End If
                If ci.CouponMessage <> Nothing Then
                    Dim strMsg As String = ci.CouponMessage
                    If strMsg.Contains("This promotion has a minimum") Or strMsg.Contains("This promotion has a maximum") Or strMsg.Contains("The promotion code you entered") Then
                        litCoupon.Text = ""
                    Else
                        litCoupon.Text = ci.CouponMessage
                    End If
                End If
                litDetails.Text = "<div><b>" & Server.HtmlEncode(IIf(ci.IsFreeSample = True, ci.ItemName, ci.ItemName)) & "</b></div>"
                litDetails.Text &= "<div style=""margin-top:2px;"">Item# " & IIf(ci.AttributeSKU = Nothing, ci.SKU, ci.AttributeSKU) & "</div>"
                If ci.Swatches <> Nothing Then litDetails.Text &= "<div style=""margin-top:6px;"" class=""smaller"">" & ci.Swatches.Replace(vbCrLf, "<br />") & "</div>"
                If ci.Attributes <> Nothing Then litDetails.Text &= "<div style=""margin-top:6px;"" class=""smaller"">" & ci.Attributes.Replace(vbCrLf, "<br />") & "</div>"

                If ci.IsFreeItem = True Then
                    If Not ci.FreeItemIds Is Nothing Then
                        litDetails.Text = "<b class=""mag"">FREE Item</b><br />" & litDetails.Text
                    Else
                        litDetails.Text = "<b class=""mag"">FREE Gift</b><br />" & litDetails.Text
                    End If
                End If
                If sm.Image <> Nothing AndAlso Cart.Order.BillToCountry = "US" Then
                    If Cart.CheckShippingSpecialUS = True Then
                        If ci.IsOversize = False Then
                            imgShipping.ImageUrl = ConfigData.GlobalRefererName & "/includes/theme-admin/images/global/ico_international.gif"
                        Else
                            imgShipping.ImageUrl = ConfigData.GlobalRefererName & "/includes/theme-admin/images/global/" & sm.Image
                        End If
                    Else
                        imgShipping.ImageUrl = ConfigData.GlobalRefererName & "/includes/theme-admin/images/global/" & sm.Image
                    End If

                ElseIf sm.Image <> Nothing AndAlso Cart.Order.BillToCountry <> "US" Then
                    If Cart.CheckShippingSpecialUS = True Then
                        If ci.IsOversize = False Then
                            imgShipping.ImageUrl = ConfigData.GlobalRefererName & "/includes/theme-admin/images/global/ico_international.gif"
                        Else
                            imgShipping.ImageUrl = ConfigData.GlobalRefererName & "/includes/theme-admin/images/global/" & sm.Image
                        End If
                        '''''''''''''''''''''''''''''''''''''''
                    Else
                        imgShipping.ImageUrl = ConfigData.GlobalRefererName & "/includes/theme-admin/images/global/" & sm.Image
                    End If
                Else
                    imgShipping.ImageUrl = ConfigData.GlobalRefererName & "/includes/theme-admin/images/spacer.gif"
                End If


                If SitePage.GetQueryString("print") <> Nothing Then
                    If SitePage.GetQueryString("print") = "y" Then
                        lnkImg.Text = "<img border=""0"" style=""border-style:none;"" src="""
                        lnkImg.Text &= IIf(ci.Image <> Nothing, ConfigData.GlobalRefererName & "/assets/items/cart/" & ci.Image, ConfigData.GlobalRefererName & "/assets/items/cart/na.jpg")
                        lnkImg.Text &= """>"
                    End If
                Else
                    lnkImg.Text = "<div style=""height: 58px; width:58px; background-image: url(" & ConfigData.GlobalRefererName & "/assets/items/cart/" & IIf(String.IsNullOrEmpty(ci.Image), "na.jpg", ci.Image) & ");""><img src=""" & ConfigData.GlobalRefererName & "/assets/nobg.gif"" height=""100%"" width=""100%"" style=""border-style:none"" alt="""" />"
                End If
                Dim isWebTemplate As Boolean = StoreItemRow.IsWebItemplateItem(ci.SKU)
                If (isWebTemplate Or ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                    litSelected.Text = String.Empty
                    imgShipping.Visible = False
                Else
                    Dim CountryCode As String = IIf(Cart.Order.IsSameAddress, Cart.Order.BillToCountry, Cart.Order.ShipToCountry)
                    If CountryCode <> "US" Then
                        litSelected.Text = "<div style=""margin-top:3px;"">" & IIf(ci.IsHazMat, "<div class=""smallerred"">Flammable Liquid</div>", "") & "Best Way</div>"
                        If Utility.Common.CheckShippingInternational(DB, Cart.Order) = True Then
                            'Long Note 30/01/2012
                            litSelected.Text = "<div style=""margin-top:3px;"">" & IIf(ci.IsHazMat, "<div class=""smallerred"">Flammable Liquid</div>", "") & ShipmentMethodRow.GetRow(DB, ci.CarrierType).Name & "</div>"
                        Else
                            litSelected.Text = "<div style=""margin-top:3px;"">" & IIf(ci.IsHazMat, "<div class=""smallerred"">Flammable Liquid</div>", "") & "Best Way</div>"
                        End If
                    Else
                        If Cart.CheckShippingSpecialUS = True Then
                            'Long Note 30/01/2012
                            litSelected.Text = "<div style=""margin-top:3px;"">" & IIf(ci.IsHazMat, "<div class=""smallerred"">Flammable Liquid</div>", "") & ShipmentMethodRow.GetRow(DB, ci.CarrierType).Name & "</div>"
                        Else
                            litSelected.Text = "<div style=""margin-top:3px;"">" & IIf(ci.IsHazMat, "<div class=""smallerred"">Flammable Liquid</div>", "") & ShipmentMethodRow.GetRow(DB, ci.CarrierType).Name & IIf(ci.IsFreeShipping, "<br /><span class=""smallerred"">FREE Shipping</span>", "") & "</div>"
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

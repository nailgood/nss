Imports Components
Imports DataLayer
Imports Utility
Imports ShippingValidator
Partial Class controls_tracking
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
    Public weebRoot As String = Utility.ConfigData.GlobalRefererName ' System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName")
    Protected TrackingId As String
    'Private trackDetail As FedExGetTracking.TrackDetail
    Private trackDetail As FedExGetTracking.TrackDetail
    Protected m_UrlTracking As String
    Private WebRoot As String = Utility.ConfigData.CDNMediaPath
    Private m_OrderId As Integer = 0
    Public Property Cart() As ShoppingCart
        Get
            Return c
        End Get
        Set(ByVal value As ShoppingCart)
            c = value
        End Set
    End Property
    Public Property m_TrackingId() As String
        Get
            Return TrackingId
        End Get
        Set(ByVal value As String)
            TrackingId = value
        End Set
    End Property
    Public Property UrlTracking() As String
        Get
            Return m_UrlTracking
        End Get
        Set(ByVal value As String)
            m_UrlTracking = value
        End Set
    End Property
    Public Property OrderId() As Integer
        Get
            Return m_OrderId
        End Get
        Set(ByVal value As Integer)
            m_OrderId = value
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
    Private GuestStatus As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If c Is Nothing Then Exit Sub
        o = c.Order
        m_LoggedInName = o.BillToName & " " & o.BillToName2
        m_Member = MemberRow.GetRow(o.MemberId)
        GuestStatus = IIf(m_Member.GuestStatus = 2, True, False)
        If o.OrderNo <> Nothing Then
            lit.Text = o.OrderNo
        End If
        Try
            Dim tr As StoreOrderShipmentTrackingRow = StoreOrderShipmentTrackingRow.GetRowFromTrackingNo(DB, TrackingId)
            Dim ShipingType As String
            If tr.OrderId = 0 Then
                tr.OrderId = OrderId
            End If
            Dim o As StoreOrderRow = StoreOrderRow.GetRow(DB, tr.OrderId)

            Dim ShipBeginDate As String = o.ProcessDate.ToString("MM/dd/yyyy")
            If tr.ServiceType = Nothing Then
                ShipingType = NavisionCodes.ShippingTypeFedex(o.CarrierType) 'DB.ExecuteScalar("select Name from ShipmentMethod sm left join StoreOrder so on sm.MethodId = so.CarrierType where OrderId = " & tr.OrderId)
            Else
                ShipingType = tr.ServiceType
            End If
            Dim trinfo As TrackingInfoRow = TrackingInfoRow.GetRow(DB, TrackingId)
            If trinfo.ActualDeliveryTimestam.ToString.Contains("0001") = False Then
                ShowInfoDB(trinfo)
            Else
                'Dim getTracking As New FedexTracking(TrackingId, ShipBeginDate, ShipingType)
                'ShowInfoAPI(getTracking)

                Dim getTracking2 As New FedexTracking(TrackingId, ShipBeginDate, ShipingType)
                ShowInfoAPI(getTracking2)
            End If
        Catch ex As Exception

        End Try

        'Cart.RecalculateTotalUpdate()
        Dim fromMobile As String = String.Empty
        Try
            fromMobile = Request.QueryString("mb")
        Catch ex As Exception

        End Try

        CODThreshold = SysParam.GetValue("FreeCODThreshold")
        CODFee = SysParam.GetValue("CODFee")


        EbayOrder = CheckEbayOrder(o.OrderNo)

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
        'litBilling.Text = Utility.Common.GetHTMLOrderBillingAddress(DB, o)
        If o.IsSameAddress Then
            litShipping.Text = Utility.Common.GetHTMLOrderBillingAddressTracking(DB, o) ' litBilling.Text
        Else
            litShipping.Text = Utility.Common.GetHTMLOrderShippingAddressTracking(DB, o)
        End If

        'If Left(Request.Path, 7) = "/admin/" Then
        '    If c.Order.MemberId <> Nothing Then
        '        litBilling.Text &= "<br /><br /><a href=""/admin/members/edit.aspx?MemberId=" & c.Order.MemberId & """>View Customer " & DB.ExecuteScalar("select top 1 customerno from customer where customerid = " & c.Order.SellToCustomerId) & "</a>"
        '    Else
        '        litBilling.Text &= "<br /><br />Customer " & DB.ExecuteScalar("select top 1 customerno from customer where customerid = " & c.Order.SellToCustomerId) & " <span style=""font-weight:normal"">(not a web member)</span>"
        '    End If
        'End If

        Try
            If o.OrderNo <> Nothing Then
                dvOrderNo.Visible = True

            Else
                dvOrderNo.Visible = False

                ' litBillingNotOrder.Text = litBilling.Text
            End If
        Catch ex As Exception

        End Try

        If Not IsPostBack Then
            LoadFromDb()
        End If
        ' litBilling.Text = CheckHTMLAddress(litBilling.Text)
        litShipping.Text = CheckHTMLAddress(litShipping.Text)

    End Sub
    Private Sub ShowInfoDB(ByVal trinfo As TrackingInfoRow)
        Dim Status As String = Trim(trinfo.StatusCode)
        If Status <> "DL" And Status <> "DE" Then
            ltEstimatedDelivery.Text = "Your estimated delivery date is:<br />" & trinfo.ActualDeliveryTimestam.ToString("dddd, MMMM dd, yyyy")
            tbBeginTracking.Visible = True
            ltHello.Text = IIf(GuestStatus = True, Resources.Msg.HelloBeginSendTrackingByGuest, Resources.Msg.HelloBeginSendTracking)
        Else
            If Status = "DL" Then
                ltHello.Text = Resources.Msg.HelloTrackingDelivered
            Else
                ltSeeMore.Text = "<p style=""margin:0px; padding:0px"">" & Resources.Msg.ReasonDeliveryException & "</p>"
                ltHello.Text = Resources.Msg.HelloTrackingDeliveryException
            End If
            lbShipTimestamp.Text = trinfo.ShipTimestamp.ToString("ddd MM/dd/yyyy")
            Dim statusImg As String = ""
            statusImg = "<div><img src='" & WebRoot & "/includes/theme/images/" & ReturnImgStatus(Trim(trinfo.StatusCode), LCase(trinfo.StatusDescription)) & "' alt='" & trinfo.StatusDescription & "'></div>"
            If trinfo.StatusDescription.Contains("Delivered") Then
                ltStatus.Text = statusImg _
                & "<div style='font-weight: bold;padding-right: 20px; text-align: center;color: #008000;font-size: 16px; font-weight: bold;'>" & trinfo.StatusDescription & "</div><div class=""ship-status-italic"">Signed for by: " & trinfo.DeliverySignatureName & "</div>"
                lbStatus.Text = "Actual delivery :"
                lbActualDeliveryTimestamp.Text = trinfo.ActualDeliveryTimestam.ToString("ddd MM/dd/yyyy hh:mm tt")
            Else
                ltStatus.Text = statusImg _
                 & "<div style='color: #800080;font-size: 16px;font-weight: bold;padding-top: 10px; font-weight: bold; padding-right: 20px;text-align: center;'>" & trinfo.StatusDescription & "</div><div class=""ship-status-italic"">" & trinfo.StatusAddress & "</div>"
                lbStatus.Text = "Estimated delivery :"
                lbActualDeliveryTimestamp.Text = trinfo.ActualDeliveryTimestam.ToString("ddd MM/dd/yyyy")
            End If
            ltSeeMore.Text &= String.Format(Resources.Msg.SeeMoreTracking, "&nbsp;<a href='" & WebRoot & String.Format(Resources.Msg.LinkTrackingNailSuperStore, TrackingId) & "' target='_blank'>click here</a>")
            lbActualAddress.Text = UCase(trinfo.ActualAddress)

            tbDelivery.Visible = True
        End If
    End Sub
    Private Sub ShowInfoAPI(ByVal getTracking As FedexTracking)
        hTrackingId.Value = TrackingId

        For Each trackDetail In getTracking.replyTracking.TrackDetails
            Dim Status As String = Trim(trackDetail.StatusCode)
            If Status <> "DL" And Status <> "DE" Then
                ltEstimatedDelivery.Text = "Your estimated delivery date is:<br />"
                If trackDetail.EstimatedDeliveryTimestamp.ToString.Contains("0001") = False Then
                    ltEstimatedDelivery.Text &= trackDetail.EstimatedDeliveryTimestamp.ToString("dddd, MMMM dd, yyyy")
                End If
                If trackDetail.ActualDeliveryTimestamp.ToString.Contains("0001") = False Then
                    ltEstimatedDelivery.Text = "Your estimated delivery date is:<br />"
                    ltEstimatedDelivery.Text &= trackDetail.ActualDeliveryTimestamp.ToString("dddd, MMMM dd, yyyy")
                End If
                If ltEstimatedDelivery.Text.Contains("0001") Then
                    ltEstimatedDelivery.Visible = False
                End If
                tbBeginTracking.Visible = True
                ltHello.Text = IIf(GuestStatus = True, Resources.Msg.HelloBeginSendTrackingByGuest, Resources.Msg.HelloBeginSendTracking)
            Else
                If Status = "DL" Then
                    ltHello.Text = Resources.Msg.HelloTrackingDelivered
                Else
                    ltSeeMore.Text = Resources.Msg.ReasonDeliveryException & "<br>"
                    ltHello.Text = Resources.Msg.HelloTrackingDeliveryException
                End If
                If trackDetail.EstimatedDeliveryTimestamp.ToString = "1/1/0001 12:00:00 AM" Then
                    lbShipTimestamp.Text = trackDetail.ActualDeliveryTimestamp.ToString("dddd, MMMM dd, yyyy")
                Else
                    lbShipTimestamp.Text = trackDetail.EstimatedDeliveryTimestamp.ToString("dddd, MMMM dd, yyyy")
                End If
                Dim Address As String = ""
                Dim i As Integer = 0
                For Each trackevent As FedExGetTracking.TrackEvent In trackDetail.Events
                    If (trackevent.TimestampSpecified) Then
                        If i = 0 Then
                            Address = trackevent.Address.City & " " & trackevent.Address.StateOrProvinceCode
                        End If
                        i = i + 1
                    End If
                Next
                Dim statusImg As String = ""
                statusImg = "<div><img src='" & WebRoot & "/includes/theme/images/" & ReturnImgStatus(Trim(trackDetail.StatusCode), LCase(trackDetail.StatusDescription)) & "'></div>"
                If trackDetail.StatusDescription.Contains("Delivered") Then
                    ltStatus.Text = statusImg _
                    & "<div class=""ship-status delivered"">" & trackDetail.StatusDescription & "</div><div class=""ship-status-italic"">" & trackDetail.DeliverySignatureName & "</div>"
                    lbStatus.Text = "Actual delivery :"

                Else
                    ltStatus.Text = statusImg _
                     & "<div class=""ship-status intransit"">" & trackDetail.StatusDescription & "</div><div class=""ship-status-italic"">" & Address & "</div>"
                    lbStatus.Text = "Estimated delivery :"
                End If
                If (trackDetail.DestinationAddress IsNot Nothing) Then
                    lbActualAddress.Text = trackDetail.DestinationAddress.City & ", " & trackDetail.DestinationAddress.StateOrProvinceCode & " " & trackDetail.DestinationAddress.CountryCode
                    lbActualDeliveryTimestamp.Text = trackDetail.EstimatedDeliveryTimestamp.ToString("ddd MM/dd/yyyy hh:mm tt")
                End If

                If (trackDetail.ActualDeliveryTimestampSpecified) Then
                    lbActualAddress.Text = trackDetail.ActualDeliveryAddress.City & ", " & trackDetail.ActualDeliveryAddress.StateOrProvinceCode & " " & trackDetail.ActualDeliveryAddress.CountryCode
                    lbActualDeliveryTimestamp.Text = trackDetail.ActualDeliveryTimestamp.ToString("ddd MM/dd/yyyy hh:mm tt")
                End If
                ltSeeMore.Text &= String.Format(Resources.Msg.SeeMoreTracking, "&nbsp;<a href='" & WebRoot & String.Format(Resources.Msg.LinkTrackingNailSuperStore, TrackingId) & "' target='_blank'>click here</a>")
            End If

        Next
    End Sub

    'Private Sub ShowInfoAPI(ByVal getTracking As FedexTracking2)
    '    hTrackingId.Value = TrackingId

    '    'For Each trackDetail In getTracking.replyTracking.CompletedTrackDetails
    '    For Each CompletedTrackDetail As TrackServiceDefinitions.CompletedTrackDetail In getTracking.replyTracking.CompletedTrackDetails
    '        For Each trackDetail In CompletedTrackDetail.TrackDetails
    '            Dim Status As String = Trim(trackDetail.StatusDetail.Code)
    '            If Status <> "DL" And Status <> "DE" Then
    '                ltEstimatedDelivery.Text = "Your estimated delivery date is:<br />"
    '                If trackDetail.EstimatedDeliveryTimestamp.ToString.Contains("0001") = False Then
    '                    ltEstimatedDelivery.Text &= trackDetail.EstimatedDeliveryTimestamp.ToString("dddd, MMMM dd, yyyy")
    '                End If
    '                If trackDetail.ActualDeliveryTimestamp.ToString.Contains("0001") = False Then
    '                    ltEstimatedDelivery.Text = "Your estimated delivery date is:<br />"
    '                    ltEstimatedDelivery.Text &= trackDetail.ActualDeliveryTimestamp.ToString("dddd, MMMM dd, yyyy")
    '                End If
    '                If ltEstimatedDelivery.Text.Contains("0001") Then
    '                    ltEstimatedDelivery.Visible = False
    '                End If
    '                tbBeginTracking.Visible = True
    '                ltHello.Text = IIf(GuestStatus = True, Resources.Msg.HelloBeginSendTrackingByGuest, Resources.Msg.HelloBeginSendTracking)
    '            Else
    '                If Status = "DL" Then
    '                    ltHello.Text = Resources.Msg.HelloTrackingDelivered
    '                Else
    '                    ltSeeMore.Text = Resources.Msg.ReasonDeliveryException & "<br>"
    '                    ltHello.Text = Resources.Msg.HelloTrackingDeliveryException
    '                End If
    '                If trackDetail.EstimatedDeliveryTimestamp.ToString = "1/1/0001 12:00:00 AM" Then
    '                    lbShipTimestamp.Text = trackDetail.ActualDeliveryTimestamp.ToString("dddd, MMMM dd, yyyy")
    '                Else
    '                    lbShipTimestamp.Text = trackDetail.EstimatedDeliveryTimestamp.ToString("dddd, MMMM dd, yyyy")
    '                End If
    '                Dim Address As String = ""
    '                Dim i As Integer = 0
    '                For Each trackevent As TrackServiceDefinitions.TrackEvent In trackDetail.Events
    '                    If (trackevent.TimestampSpecified) Then
    '                        If i = 0 Then
    '                            Address = trackevent.Address.City & " " & trackevent.Address.StateOrProvinceCode
    '                        End If
    '                        i = i + 1
    '                    End If
    '                Next
    '                Dim statusImg As String = ""
    '                statusImg = "<div><img src='" & WebRoot & "/includes/theme/images/" & ReturnImgStatus(Trim(trackDetail.StatusDetail.Code), LCase(trackDetail.StatusDetail.Description)) & "'></div>"
    '                If trackDetail.StatusDetail.Description.Contains("Delivered") Then
    '                    ltStatus.Text = statusImg _
    '                    & "<div class=""ship-status delivered"">" & trackDetail.StatusDetail.Description & "</div><div class=""ship-status-italic"">" & trackDetail.DeliverySignatureName & "</div>"
    '                    lbStatus.Text = "Actual delivery :"

    '                Else
    '                    ltStatus.Text = statusImg _
    '                     & "<div class=""ship-status intransit"">" & trackDetail.StatusDetail.Description & "</div><div class=""ship-status-italic"">" & Address & "</div>"
    '                    lbStatus.Text = "Estimated delivery :"
    '                End If
    '                If (trackDetail.DestinationAddress IsNot Nothing) Then
    '                    lbActualAddress.Text = trackDetail.DestinationAddress.City & ", " & trackDetail.DestinationAddress.StateOrProvinceCode & " " & trackDetail.DestinationAddress.CountryCode
    '                    lbActualDeliveryTimestamp.Text = trackDetail.EstimatedDeliveryTimestamp.ToString("ddd MM/dd/yyyy hh:mm tt")
    '                End If

    '                If (trackDetail.ActualDeliveryTimestampSpecified) Then
    '                    lbActualAddress.Text = trackDetail.ActualDeliveryAddress.City & ", " & trackDetail.ActualDeliveryAddress.StateOrProvinceCode & " " & trackDetail.ActualDeliveryAddress.CountryCode
    '                    lbActualDeliveryTimestamp.Text = trackDetail.ActualDeliveryTimestamp.ToString("ddd MM/dd/yyyy hh:mm tt")
    '                End If
    '                ltSeeMore.Text &= String.Format(Resources.Msg.SeeMoreTracking, "&nbsp;<a href='" & WebRoot & String.Format(Resources.Msg.LinkTrackingNailSuperStore, TrackingId) & "' target='_blank'>click here</a>")
    '            End If
    '        Next
    '    Next
    'End Sub

    Private Function CheckHTMLAddress(ByVal value As String) As String
        Dim index As Integer = value.IndexOf("<br />")
        Dim result As String = value
        If (index = 0) Then
            result = value.Substring(6, value.Length - 6)
        End If
        Return result
    End Function
    Private Sub LoadFromDb()
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


        If Utility.Common.CheckShippingInternational(DB, o) = True Then
            If o.ShipToCountry <> "PR" And o.ShipToCountry <> "VI" And o.ShipToCountry <> "HI" And o.ShipToCountry <> "AK" And Not (o.ShipToCountry = "US" AndAlso o.ShipToCounty = "HI") And Not (o.ShipToCountry = "US" AndAlso o.ShipToCounty = "AK") Then
                litInter.Visible = True
                litInter.Text = "<tr><td colpan=""2""><span style=""font-size: 11px;color: #be048d;line-height: 13px;"">Taxes & duties are not included</span><td></tr>"
            End If
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
        If o.TotalSpecialHandlingFee > 0 Then
            lblHandlingFee.Text = FormatCurrency(Cart.Order.TotalSpecialHandlingFee, 2)
            trHandlingFee.Visible = True
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
                Dim tdIconShipping As HtmlTableCell = e.Item.FindControl("tdIconShipping")
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

                Dim memberID As Integer = 0
                If Not (Session("MemberId") Is Nothing) Then
                    memberID = CInt(Session("MemberId"))
                End If
                Dim Item As StoreItemRow = StoreItemRow.GetRowInCart(DB, ci.ItemId, memberID)


                Dim sUrl As String = URLParameters.ProductUrl(Item.URLCode, Item.ItemId)
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
                        litCoupon.Text = "<div style=""color: #BE048C; font: 13px Arial; color: #BE048C; font: 12px/18px Arial;"">" & ci.CouponMessage & "</div>"
                    End If
                End If
                litDetails.Text = "<div><b><a href='" & weebRoot & sUrl & "' style='color:#454545'>" & Server.HtmlEncode(IIf(ci.IsFreeSample = True, ci.ItemName, ci.ItemName)) & "</a></b></div>"
                litDetails.Text &= "<div style=""margin-top:2px;"">Item# " & IIf(ci.AttributeSKU = Nothing, ci.SKU, ci.AttributeSKU) & "</div>"
                If ci.Swatches <> Nothing Then litDetails.Text &= "<div style=""margin-top:6px;font-size: 11px;"" >" & ci.Swatches.Replace(vbCrLf, "<br />") & "</div>"
                If ci.Attributes <> Nothing Then litDetails.Text &= "<div style=""margin-top:6px;font-size: 11px;"" >" & ci.Attributes.Replace(vbCrLf, "<br />") & "</div>"
                'litDetails.Text &= "<div style=""margin-top:6px;"" class=""mag"">" & p.Text & "</div>"
                If ci.IsFreeItem = True Then
                    If Not ci.FreeItemIds Is Nothing Then
                        litDetails.Text = "<b style='color: #be048d;line-height: 13px;'>FREE Item</b><br />" & litDetails.Text
                    Else
                        litDetails.Text = "<b style='color: #be048d;line-height: 13px;'>FREE Gift</b><br />" & litDetails.Text
                    End If
                End If
                Dim iconShipping As String = String.Empty
                If sm.Image <> Nothing AndAlso Cart.Order.BillToCountry = "US" Then
                    If Cart.CheckShippingSpecialUS = True Then
                        If ci.IsOversize = False Then
                            iconShipping = "/includes/theme-admin/images/global/ico_international.gif"
                        Else
                            iconShipping = "/includes/theme-admin/images/global/" & sm.Image
                        End If
                    Else
                        iconShipping = "/includes/theme-admin/images/global/" & sm.Image
                    End If

                ElseIf sm.Image <> Nothing AndAlso Cart.Order.BillToCountry <> "US" Then
                    If Cart.CheckShippingSpecialUS = True Then

                        If ci.IsOversize = False Then
                            iconShipping = "/includes/theme-admin/images/global/ico_international.gif"
                        Else
                            iconShipping = "/includes/theme-admin/images/global/" & sm.Image
                        End If
                        '''''''''''''''''''''''''''''''''''''''
                    Else
                        iconShipping = "/includes/theme-admin/images/global/" & sm.Image
                    End If

                Else
                    iconShipping = "/includes/theme-admin/images/spacer.gif"
                End If


                'If Not Request.QueryString("print") = Nothing Then
                '    If Request.QueryString("print") = "y" Then
                '        lnkImg.Text = "<a href='" & weebRoot & sUrl & "'><img border=""0"" style=""border-style:none;"" src="""
                '        lnkImg.Text &= IIf(ci.Image <> Nothing, weebRoot & "/assets/items/cart/" & ci.Image, weebRoot & "/assets/items/cart/na.jpg")
                '        lnkImg.Text &= """></a>"
                '    End If
                'Else
                '    'lnkImg.Text = "<div style=""height: 58px; width:58px; background-image: url(" & weebRoot & "/assets/items/cart/" & IIf(String.IsNullOrEmpty(ci.Image), "na.jpg", ci.Image) & ");""><a href='" & weebRoot & sUrl & "' target='_blank'><img src=""" & weebRoot & "/assets/nobg.gif"" height=""100%"" width=""100%"" style=""border-style:none"" alt="""" /></a>"
                '    lnkImg.Text = "<a href='" & weebRoot & sUrl & "' target='_blank'><img src=""" & weebRoot & "/assets/items/cart/" & IIf(String.IsNullOrEmpty(ci.Image), "na.jpg", ci.Image) & " style=""border-style:none;height: 58px; width:58px;"" alt="""" /></a>"
                'End If
                lnkImg.Text = "<a href='" & weebRoot & sUrl & "' target='_blank'><img border=""0"" style=""border-style:none;"" src="""
                lnkImg.Text &= IIf(ci.Image <> Nothing, weebRoot & "/assets/items/cart/" & ci.Image, WebRoot & "/assets/items/cart/na.jpg")
                lnkImg.Text &= """></a>"

                'off icon shipping if exist hazardousfee
                If o IsNot Nothing AndAlso (o.HazardousMaterialFee > 0 And Common.InternationalShippingId().Contains(o.CarrierType)) Then
                    tdIconShipping.Visible = False
                Else
                    tdIconShipping.InnerHtml = "<img src='" & WebRoot & iconShipping & "' Width='26' Height='25' />"
                End If
                Dim isWebTemplate As Boolean = StoreItemRow.IsWebItemplateItem(ci.SKU)
                If (isWebTemplate Or ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                    litSelected.Text = String.Empty
                    tdIconShipping.Visible = False
                Else
                    '''''''''''code vuphuong add: 03/09/2009
                    Dim CountryCode As String = IIf(Cart.Order.IsSameAddress, Cart.Order.BillToCountry, Cart.Order.ShipToCountry)
                    If CountryCode <> "US" Then
                        ''litSelected.Text = "<div style=""margin-top:3px;"">" & IIf(ci.IsHazMat Or ci.IsFlammable, "<div style='color:Red; font-size:11px;'>" & Resources.Alert.ItemFlammable & "</div>", "") & "Best Way</div>"
                        ''litFlammable.Text = "<div  style=""margin-top:-2px; line-height:16px; padding-top: 0px; "">" & IIf(ci.IsHazMat Or ci.IsFlammable, "<div style=""color: Red;font-size: 11px;"">" & Resources.Alert.ItemFlammable & "</div>", "") & "Best Way</div>"

                        '''''vuphuong add:21/10/2009
                        If Utility.Common.CheckShippingInternational(DB, Cart.Order) = True Then
                            litSelected.Text = "<div style=""margin-top:3px;"">International Shipping</div>"
                            ''''''''''''vuphuong add: 09/12/2009
                            If ci.IsOversize = False Then
                                litSelected.Text = "<div style=""margin-top:3px;"">International Shipping</div>"
                            Else
                                litSelected.Text = "<div style=""margin-top:3px;"">" & ShipmentMethodRow.GetRow(DB, ci.CarrierType).Name & "</div>"
                            End If
                            ''''''''''''''''''''''''''''''''''

                        End If
                        ''''''''''''''''''''''''''''
                    Else
                        If Cart.CheckShippingSpecialUS = True Then
                            ''''''''''''vuphuong add: 09/12/2009
                            If ci.IsOversize = False Then
                                litSelected.Text = "<div style=""margin-top:3px;"">International Shipping</div>"
                                '' litFlammable.Text = "<div  style=""margin-top:-2px; line-height:16px; padding-top: 0px; "">" & IIf(ci.IsHazMat Or ci.IsFlammable, "<div style='color:Red; font-size:11px;'>" & Resources.Alert.ItemFlammable & "</div>", "")


                            Else
                                litSelected.Text = "<div style=""margin-top:3px;"">" & ShipmentMethodRow.GetRow(DB, ci.CarrierType).Name & "</div>"
                                ''  litFlammable.Text = "<div  style=""margin-top:-2px; line-height:16px; padding-top: 0px; "">" & IIf(ci.IsHazMat Or ci.IsFlammable, "<div style='color:Red; font-size:11px;'>" & Resources.Alert.ItemFlammable & "</div>", "")


                            End If
                            ''''''''''''''''''''''''''''''''''
                            'litSelected.Text = "<div style=""margin-top:3px;"">" & IIf(ci.IsHazMat, "<div class=""smaller red"">Flammable Liquid</div>", "") & "USPS Priority</div>"
                        Else
                            litSelected.Text = "<div style=""margin-top:3px;"">" & ShipmentMethodRow.GetRow(DB, ci.CarrierType).Name & IIf(ci.IsFreeShipping, "<br /><span style='color:Red; font-size:11px;'>FREE Shipping</span>", "") & "</div>"
                           

                        End If

                    End If
                End If

                'litSelected.Text = "<div style=""margin-top:3px;"">" & ShipmentMethodRow.GetRow(DB, ci.CarrierType).Name & "</div>"
                '''''''''''''''''''''''''''''''''''''
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
    Private Function ReturnImgStatus(ByVal StatusCode As String, ByVal StatusDescription As String) As String
        Dim img As String = ""
        Select Case StatusCode
            Case "DL"
                img = "the-nailsuperstore-delivered.jpg"
            Case "IT"
                img = "the-nailsuperstore-intransit.jpg"
            Case "PU"
                img = "the-nailsuperstore-pickedup.jpg"
            Case "DE"
                img = "the-nailsuperstore-exception.jpg"
            Case Else
                img = "the-nailsuperstore-first.jpg"
        End Select
        If StatusDescription = "in transit" Then
            img = "the-nailsuperstore-intransit.jpg"
        End If
        Return img
    End Function
End Class

Imports Components
Imports DataLayer
Imports Utility
Partial Class controls_product_order_review_detail
    Inherits BaseControl
    Private c As ShoppingCart
    Private _db As Database
    Private o As StoreOrderRow

    Protected LiftGateService As Double
    Protected InsideDeliveryService As Double
    Protected CODThreshold As Double
    Protected CODFee As Double
    Public weebRoot As String = System.Configuration.ConfigurationManager.AppSettings("GlobalRefererName")
    Public MemberId As Integer
    Public countReview As Integer = 0
    Public pointReview As Integer = 0
    Public memberName As String = String.Empty
    Private WebRoot As String = Utility.ConfigData.CDNMediaPath
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If weebRoot = "" Then
            weebRoot = "https://www.nss.com/"
        End If
        If c Is Nothing Then Exit Sub
        'Cart.RecalculateTotalUpdate()

        CODThreshold = SysParam.GetValue("FreeCODThreshold")
        CODFee = SysParam.GetValue("CODFee")
        pointReview = SysParam.GetValue("ProductReviewPoint")
        o = c.Order
        If o.OrderNo <> Nothing Then
            lit.Text = o.OrderNo
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

        If Not IsPostBack Then
            LoadCustomerName(o.MemberId)
            LoadFromDb()
        End If
    End Sub
    Private Sub LoadCustomerName(ByVal memberId As Integer)
        memberName = MemberRow.GetNameForSendMail(Dab, memberId)
    End Sub
    Private Sub LoadFromDb()
        rptCartItems.DataSource = c.GetListProductReview()
        rptCartItems.DataBind()

    End Sub

    Private Sub rptCartItems_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptCartItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim ci As StoreCartItemRow = e.Item.DataItem
                Dim storeItem As New StoreItemRow
                Dim ltrLinkReview As Literal = CType(e.Item.FindControl("ltrLinkReview"), Literal)
                If (ci.IsRewardPoints Or ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                    ltrLinkReview.Text = ""
                ElseIf ci.IsFreeItem = True Then
                    ltrLinkReview.Text = ""
                ElseIf ci.IsFreeSample = True Then
                    ltrLinkReview.Text = ""
                Else
                    countReview = storeItem.GetCountItemReviewByMember(Dab, ci.ItemId, MemberId)
                    If countReview < 1 Then
                        'ltrLinkReview.Text = "<a style='color:#0605ff;' href=" & weebRoot & "/store/review/product-write.aspx?id=" & HttpUtility.UrlEncode(Utility.Crypt.EncryptTripleDes(ci.CartItemId)) & ">Review this product </a>"
                        ltrLinkReview.Text = "<a style='color:#0605ff;' href=" & weebRoot & "/store/review/product-write.aspx?id=" & ci.CartItemId & ">{0} </a>"
                        If ci.isFirstReview Then
                            ltrLinkReview.Text = String.Format(ltrLinkReview.Text, Resources.Msg.review_First)
                        Else
                            ltrLinkReview.Text = String.Format(ltrLinkReview.Text, Resources.Msg.review_Old)
                        End If

                    Else
                        ltrLinkReview.Text = "Reviewed"
                    End If
                End If

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
                        litCoupon.Text = "<div style=""color: #be048c; font-size: 13px;"">" & ci.CouponMessage & "</div>"

                    End If
                End If
                litDetails.Text = "<div><b>" & Server.HtmlEncode(IIf(ci.IsFreeSample = True, ci.ItemName, ci.ItemName)) & "</b></div>"
                litDetails.Text &= "<div style=""margin-top:2px;"">Item# " & IIf(ci.AttributeSKU = Nothing, ci.SKU, ci.AttributeSKU) & "</div>"
                If ci.Swatches <> Nothing Then litDetails.Text &= "<div style=""margin-top:6px;"">" & ci.Swatches.Replace(vbCrLf, "<br />") & "</div>"
                If ci.Attributes <> Nothing Then litDetails.Text &= "<div style=""margin-top:6px;"" >" & ci.Attributes.Replace(vbCrLf, "<br />") & "</div>"
                'litDetails.Text &= "<div style=""margin-top:6px;"" class=""mag"">" & p.Text & "</div>"
                If ci.IsFreeItem = True Then
                    If Not ci.FreeItemIds Is Nothing Then
                        litDetails.Text = "<b style=""color: #be048c;font-size: 13px;"">FREE Item</b><br />" & litDetails.Text
                    Else
                        litDetails.Text = "<b  style=""color: #be048c;font-size: 13px;"">FREE Gift</b><br />" & litDetails.Text
                    End If
                End If
                Dim iconShipping As String = String.Empty
                If sm.Image <> Nothing AndAlso Cart.Order.BillToCountry = "US" Then
                    If Cart.CheckShippingSpecialUS = True Then
                        ''''''''''''''''''''vuphuong add: 09/12/2009
                        If ci.IsOversize = False Then
                            iconShipping = "/includes/theme/images/ico_international.gif"
                        Else
                            iconShipping = "/includes/theme/images/" & sm.Image
                        End If
                        '''''''''''''''''''''''''''''''''''''''
                    Else
                        iconShipping = "/includes/theme/images/" & sm.Image
                    End If

                ElseIf sm.Image <> Nothing AndAlso Cart.Order.BillToCountry <> "US" Then
                    If Cart.CheckShippingSpecialUS = True Then
                        ''''''''''''''''''''vuphuong add: 09/12/2009
                        If ci.IsOversize = False Then
                            iconShipping = "/includes/theme/images/ico_international.gif"
                        Else
                            iconShipping = "/includes/theme/images/" & sm.Image
                        End If
                        '''''''''''''''''''''''''''''''''''''''
                    Else
                        iconShipping = "/includes/theme/images/" & sm.Image
                    End If

                Else
                    iconShipping = "/includes/theme/images/spacer.gif"
                End If


                'If Not SitePage.GetQueryString("print") = Nothing Then
                '    If SitePage.GetQueryString("print") = "y" Then
                '        lnkImg.Text = "<img border=""0"" style=""border-style:none;"" src="""
                '        lnkImg.Text &= IIf(ci.Image <> Nothing, weebRoot & "/assets/items/cart/" & ci.Image, weebRoot & "/assets/items/cart/na.jpg")
                '        lnkImg.Text &= """>"
                '    End If
                'Else

                '    lnkImg.Text = "<img width='58px' height='58px' src='" & weebRoot & "" & IIf(String.IsNullOrEmpty(ci.Image), weebRoot & "/assets/items/cart/na.jpg", "/assets/items/cart/" & ci.Image) & "' style=""border-style:none"" alt="""" />"

                '    ''lnkImg.Text = "<div style=""height: 58px; width:58px; background-image: url(" & weebRoot & "/assets/items/cart/" & IIf(String.IsNullOrEmpty(ci.Image), weebRoot & "na.jpg", ci.Image) & ");""><img src=""" & weebRoot & "/assets/nobg.gif"" height=""100%"" width=""100%"" style=""border-style:none"" alt="""" />"
                'End If
                lnkImg.Text = "<img border=""0"" style=""border-style:none;"" src="""
                lnkImg.Text &= IIf(ci.Image <> Nothing, WebRoot & "/assets/items/cart/" & ci.Image, WebRoot & "/assets/items/cart/na.jpg")
                lnkImg.Text &= """>"
                If o IsNot Nothing AndAlso (o.HazardousMaterialFee > 0 And Common.InternationalShippingId().Contains(o.CarrierType)) Then
                    tdIconShipping.Visible = False
                Else
                    tdIconShipping.InnerHtml = "<img src='" & WebRoot & iconShipping & "' Width='26' Height='25' />" 'imgShipping.ToString
                End If
                Dim isWebTemplate As Boolean = StoreItemRow.IsWebItemplateItem(ci.SKU)
                If (isWebTemplate Or ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                    litSelected.Text = String.Empty
                    tdIconShipping.Visible = False

                Else
                    Dim CountryCode As String = IIf(Cart.Order.IsSameAddress, Cart.Order.BillToCountry, Cart.Order.ShipToCountry)
                    If CountryCode <> "US" Then
                        '''''vuphuong add:21/10/2009
                        If Utility.Common.CheckShippingInternational(DB, Cart.Order) = True Then
                            litSelected.Text = "<div style=""margin-top:3px;"">" & "International Shipping</div>"
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
                                '' litFlammable.Text = "<div style=""margin-top:-2px; line-height:16px; padding-top: 0px; "">" & IIf(ci.IsHazMat Or ci.IsFlammable, "<div style=""color: Red;font-size: 11px;"">" & Resources.Alert.ItemFlammable & "</div>", "")


                            Else
                                litSelected.Text = "<div style=""margin-top:3px;"">" & ShipmentMethodRow.GetRow(DB, ci.CarrierType).Name & "</div>"
                                ''litFlammable.Text = "<div style=""margin-top:-2px; line-height:16px; padding-top: 0px; "">" & IIf(ci.IsHazMat Or ci.IsFlammable, "<div style=""color: Red;font-size: 11px;"">" & Resources.Alert.ItemFlammable & "</div>", "")
                            End If
                            ''''''''''''''''''''''''''''''''''
                            'litSelected.Text = "<div style=""margin-top:3px;"">" & IIf(ci.IsHazMat, "<div class=""smaller red"">Flammable Liquid</div>", "") & "USPS Priority</div>"
                        Else
                            litSelected.Text = "<div style=""margin-top:3px;"">" & ShipmentMethodRow.GetRow(DB, ci.CarrierType).Name & IIf(ci.IsFreeShipping, "<br /><span class=""smallerred"">FREE Shipping</span>", "") & "</div>"


                        End If

                    End If
                End If

                '''''''''''code vuphuong add: 03/09/2009

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

End Class

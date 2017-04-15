

Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_product_order_detail
    Inherits ModuleControl
    Public m_DataSource As StoreCartItemCollection
    Protected EbayOrder As Boolean = False
    Private Property DataSource() As StoreCartItemCollection
        Set(ByVal value As StoreCartItemCollection)
            m_DataSource = value
        End Set
        Get
            Return m_DataSource
        End Get
    End Property
    Private m_Cart As ShoppingCart = Nothing
    Public Property Cart() As ShoppingCart
        Set(ByVal value As ShoppingCart)
            m_Cart = value
        End Set
        Get
            Return m_Cart
        End Get
    End Property
    Private m_Order As StoreOrderRow = Nothing
    Public Property Order() As StoreOrderRow
        Set(ByVal value As StoreOrderRow)
            m_Order = value
        End Set
        Get
            Return m_Order
        End Get
    End Property
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Public linkTracking As String = String.Empty
    Public linkEdit As String = String.Empty
    Public isShipInt As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        BindData()
    End Sub
    Private Sub rptCartItems_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptCartItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ci As StoreCartItemRow = e.Item.DataItem
            Dim si As StoreItemRow = StoreItemRow.GetRow(DB, ci.ItemId, 0, True, False)

            Dim p As PromotionRow = ci.Promotion
            Dim sBrand As String = String.Empty
            Dim strItemName As String = ci.ItemName
            Dim urlCode As String = si.URLCode
            Dim sUrl As String = URLParameters.ProductUrl(urlCode, si.ItemId)
            Dim sm As ShipmentMethodRow = ShipmentMethodRow.GetRow(DB, ci.CarrierType)
            Dim ltrName As Literal = CType(e.Item.FindControl("ltrName"), Literal)
            Dim ltrImageShipping As Literal = CType(e.Item.FindControl("ltrImageShipping"), Literal)
            Dim ltrSKU As Literal = CType(e.Item.FindControl("ltrSKU"), Literal)
            ltrName.Text = strItemName
            If sm.Image <> Nothing AndAlso Cart.Order.BillToCountry = "US" Then
                If Cart.CheckShippingSpecialUS = True Then
                    If ci.IsOversize = False Then
                        ltrImageShipping.Text = "ico_international.gif"
                    Else
                        ltrImageShipping.Text = sm.Image
                    End If
                Else
                    ltrImageShipping.Text = sm.Image
                End If

            ElseIf sm.Image <> Nothing AndAlso Cart.Order.BillToCountry <> "US" Then
                If Cart.CheckShippingSpecialUS = True Then
                    If ci.IsOversize = False Then
                        ltrImageShipping.Text = "ico_international.gif"
                    Else
                        ltrImageShipping.Text = sm.Image
                    End If
                Else
                    ltrImageShipping.Text = sm.Image
                End If
            Else
                ltrImageShipping.Text = "spacer.gif"
            End If

            'HazMat order doesn't show icon
            If m_Order IsNot Nothing AndAlso (m_Order.HazardousMaterialFee > 0 And Utility.Common.InternationalShippingId().Contains(m_Order.CarrierType)) Then
                ltrImageShipping.Text = String.Empty
            Else
                ltrImageShipping.Text = "<img src='" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/" & ltrImageShipping.Text & "'/>"
            End If

            ltrSKU.Text = ci.SKU
            If (Request.RawUrl.Contains("/store/payment.aspx") Or Request.RawUrl.Contains("store/confirmation.aspx")) AndAlso ci.IsFreeItem AndAlso Not String.IsNullOrEmpty(ci.FreeItemIds) Then
                Dim warningMsg As String = String.Empty
                If isShipInt AndAlso ci.IsFlammable Then
                    warningMsg = Resources.Msg.FreeItemFlammable
                End If
                If Not String.IsNullOrEmpty(ci.CouponMessage) Then
                    warningMsg = IIf(String.IsNullOrEmpty(warningMsg), ci.CouponMessage, warningMsg & "<br/>" & ci.CouponMessage)
                End If
                Dim divWarning As HtmlGenericControl = CType(e.Item.FindControl("divWarning"), HtmlGenericControl)
                If Not divWarning Is Nothing Then
                    divWarning.InnerHtml = warningMsg
                    divWarning.Visible = True
                End If
            End If

            If ci.IsFreeItem = True Then
                If Not ci.FreeItemIds Is Nothing Then
                    If ci.LineDiscountAmount < ci.SubTotal Then
                        If String.IsNullOrEmpty(ci.MixMatchDescription) Then
                            ltrName.Text = "<b class=""mag"">Discount Item</b><br />" & ltrName.Text
                        Else
                            ltrName.Text = "<b class=""mag"">" & ci.MixMatchDescription & "</b><br />" & ltrName.Text
                        End If
                    Else
                        ltrName.Text = "<b class=""mag"">FREE Item</b><br />" & ltrName.Text
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
                    ltrName.Text = "<b class=""mag"">" & freeText & "</b><br />" & ltrName.Text
                End If
            ElseIf ci.IsFreeGift Then
                ltrName.Text = "<b class=""mag"">FREE Gift</b><br />" & ltrName.Text
            End If


            If Not String.IsNullOrEmpty(ltrName.Text) AndAlso Not String.IsNullOrEmpty(ci.PriceDesc) Then
                ltrName.Text = ltrName.Text & "<span class='unit'> - " & ci.PriceDesc & "</span>"
            End If

            Dim isWebTemplate As Boolean = StoreItemRow.IsWebItemplateItem(ci.SKU)
            Dim tdShipping As HtmlGenericControl = CType(e.Item.FindControl("tdShipping"), HtmlGenericControl)
            Dim divFreeShip As HtmlGenericControl = CType(e.Item.FindControl("divFreeShip"), HtmlGenericControl)
            If (isWebTemplate Or ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                tdShipping.Visible = False
            Else
                Dim ltrShippingName As Literal = CType(e.Item.FindControl("ltrShippingName"), Literal)
                Dim ltrSmallShipment As Literal = CType(e.Item.FindControl("ltrSmallShipment"), Literal)
                Dim shippingName As String = ShipmentMethodRow.GetRow(DB, ci.CarrierType).Name
                Dim CountryCode As String = IIf(Cart.Order.IsSameAddress, Cart.Order.BillToCountry, Cart.Order.ShipToCountry)
                If CountryCode <> "US" Then
                    If Utility.Common.InternationalShippingId.Contains(ci.CarrierType) Then
                        ltrShippingName.Text = "International Shipping" 'shippingName
                        ltrSmallShipment.Text = "International Shipping" 'shippingName
                    Else
                        ltrShippingName.Text = shippingName
                        ltrSmallShipment.Text = shippingName
                    End If
                Else
                    If Cart.CheckShippingSpecialUS = True Then
                        If Utility.Common.InternationalShippingId.Contains(ci.CarrierType) Then
                            ltrShippingName.Text = "International Shipping" 'shippingName
                            ltrSmallShipment.Text = "International Shipping" 'shippingName
                        Else
                            ltrShippingName.Text = shippingName
                            ltrSmallShipment.Text = shippingName
                        End If
                    Else
                        ltrSmallShipment.Text = IIf(ci.IsFreeShipping, "<span class=""free"">Free</span>", "") & "<span class=""name"">" & shippingName & "</span>"
                        ltrShippingName.Text = shippingName
                        If ci.IsFreeShipping Then
                            divFreeShip.Visible = True
                        End If
                    End If
                End If
            End If

            Dim ltrTotal As Literal = CType(e.Item.FindControl("ltrTotal"), Literal)
            Dim ltrImage As Literal = CType(e.Item.FindControl("ltrImage"), Literal)
            Dim ltrQty As Literal = CType(e.Item.FindControl("ltrQty"), Literal)
            Dim ltrSmallQty As Literal = CType(e.Item.FindControl("ltrSmallQty"), Literal)
            Dim ltrQtyShip As Literal = CType(e.Item.FindControl("ltrQtyShip"), Literal)
            Dim ltrSmallQtyShip As Literal = CType(e.Item.FindControl("ltrSmallQtyShip"), Literal)
            Dim ltrUnit As Literal = CType(e.Item.FindControl("ltrUnit"), Literal)
            If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                ltrTotal.Text = Utility.Common.FormatPointPrice(ci.SubTotalPoint)
            Else
                ltrTotal.Text = NSS.DisplayCartPricing(DB, ci, True, False)
            End If
            ''ci.Image = "119019.jpg"
            ltrImage.Text = IIf(String.IsNullOrEmpty(si.Image), "na.jpg", si.Image)
            ltrImage.Text = "<img src='" & Utility.ConfigData.GlobalRefererName & "/assets/items/cart/" & ltrImage.Text & "' alt='" & ci.ItemName & "'/>"
            ltrQty.Text = ci.Quantity.ToString
            ltrSmallQty.Text = ci.Quantity
            Dim SQL As String = "SELECT coalesce(SUM(coalesce(totalqtyshipped,0)),0) FROM StoreOrderShipmentLine WHERE OrderId = " & ci.OrderId & " AND CartItemid = " & ci.CartItemId
            ltrQtyShip.Text = DB.ExecuteScalar(SQL)
            ltrSmallQtyShip.Text = ltrQtyShip.Text

            If ci.AddType = 2 Then
                ltrUnit.Text &= "Case of " & si.CaseQty
            Else
                ltrUnit.Text = ci.PriceDesc
            End If

            Dim divRow As HtmlGenericControl = CType(e.Item.FindControl("divRow"), HtmlGenericControl)
            If e.Item.ItemIndex Mod 2 = 0 Then
                divRow.Attributes.Add("class", "cart-row ")
            Else
                divRow.Attributes.Add("class", "cart-row cart-alt-row")
            End If
            Dim divPromotion As HtmlGenericControl = CType(e.Item.FindControl("divPromotion"), HtmlGenericControl)
            divPromotion.Visible = False
            If Not (ci.IsRewardPoints) Then
                If ci.CouponMessage <> Nothing Then
                    Dim dbStoPro As StorePromotionRow = StorePromotionRow.GetRow(DB, ci.PromotionID)
                    Dim Msg As String = String.Empty
                    If StorePromotionRow.ValidateProductCoupon(DB, dbStoPro, dbStoPro.PromotionCode, Msg, Session("MemberId"), ci.CartItemId) = True Then
                        divPromotion.InnerText = dbStoPro.Message
                        divPromotion.Visible = True
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub BindData()
        If Cart Is Nothing AndAlso Not Session("cartRender") Is Nothing Then
            Cart = Session("cartRender")
            Dim OrderId As Integer = Utility.Common.GetCurrentOrderId()
            If orderId <= 0 Then
                OrderId = Cart.Order.OrderId
            End If

            DataSource = StoreCartItemRow.GetCartItems(DB, OrderId)
        End If

        Try
            Order = Cart.Order
        Catch ex As Exception
            Me.Visible = False
            Exit Sub
        End Try

        If Order Is Nothing Then
            Me.Visible = False
            Exit Sub
        End If

        If Request.Path = "/store/confirmation.aspx" Or Request.Path = "/members/orderhistory/view.aspx" Then
            If Not String.IsNullOrEmpty(Cart.Order.Notes) Then
                'divNoteContent.InnerText = Cart.Order.Notes
                divNoteContent.InnerHtml = Cart.Order.Notes
                divNote.Visible = True
            End If
            If Request.Path = "/store/confirmation.aspx" Then
                divTitle.InnerText = "Order Confirmation: #" & Order.OrderNo
                DataSource = Cart.GetCartItems()
            Else
                divTitle.InnerText = "View Order: #" & Order.OrderNo
                DataSource = Cart.GetCartItems()
            End If
            If Utility.Common.IsEbayOrder(Order.OrderNo) Then

                ltrBillingAddress.Text = "<ul><li class='title'>ebay Customer ID</li>"
                ltrBillingAddress.Text &= "<li>" & Order.Email.Trim & "</li>"
                ltrBillingAddress.Text &= "</ul>"
                ltrBillingAddress.Text &= "<ul><li class='title'>Order Date</li>"
                ltrBillingAddress.Text &= "<li>" & String.Format("{0:g}", Order.OrderDate) & "</li>"
                ltrBillingAddress.Text &= "</ul>"
            Else
                ltrBillingAddress.Text = Utility.Common.GetFullHTMLOrderBillingAddress(DB, Cart.Order)
            End If


            ltrShippingAddress.Text = Utility.Common.GetFullHTMLOrderShippingAddress(DB, Cart.Order)
            ulAddress.Visible = True
            divTitle.Visible = True
            If Utility.Common.IsViewFromAdmin Then
                divCustomer.Visible = True
                divCustomer.InnerHtml = "<a onclick='parent.ViewCustomer(" & Cart.Order.MemberId & ");' href='javascript:void(0);'>View Customer " & DB.ExecuteScalar("select top 1 customerno from customer where customerid = " & Cart.Order.SellToCustomerId) & "</a>"
                GetTrackingNumber()
                If Not String.IsNullOrEmpty(linkTracking) Then
                    ulAddTracking.Visible = True
                End If

                If Order.RemoteIP IsNot Nothing AndAlso Order.RemoteIP.Length > 8 Then 'IP: ::1
                    IPLocation.Visible = True
                    IPLocation.InnerHtml = "<ul><li>IP: <a href=""http://www.checkip.com/ip/" & Order.RemoteIP & """ target=""_blank"">" & Order.RemoteIP & "</a></li>" & IIf(String.IsNullOrEmpty(Order.IPLocation), "", "<li> IP Location: " & Order.IPLocation & "</li>") & "</ul>"
                End If

            End If
        Else
            ulAddress.Visible = False
            divTitle.Visible = False
        End If
        If Not DataSource Is Nothing AndAlso DataSource.Count > 0 Then
            isShipInt = MemberRow.CheckMemberIsInternational(Cart.Order.MemberId, Cart.Order.OrderId)
            rptCartItems.DataSource = DataSource
            rptCartItems.DataBind()
        End If
    End Sub
    Public Sub GetTrackingNumber()
        Dim o As StoreOrderRow = Cart.Order
        Dim dt As DataTable = DB.GetDataTable("select TrackingId,trackingno,coalesce(ShipmentType,0) as ShipmentType,coalesce(Note,'') as Note from StoreOrderShipmentTracking where shipmentid in (select shipmentid from storeordershipment where orderid = " & o.OrderId & ")")
        If dt.Rows.Count = 0 Then
            dt = DB.GetDataTable("select trackingid,trackingno,coalesce(ShipmentType,0) as ShipmentType,coalesce(Note,'') as Note from StoreOrderShipmentTracking where  orderid = " & o.OrderId)
        End If
        Dim CarrierType As Integer = DB.ExecuteScalar("select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = " & o.OrderId & " and  type = 'item' and LEFT(Code,3)='UPS'")

        ''Dim dtShipping As DataTable = DB.GetDataTable("Select MethodId,Name from ShipmentMethod where MethodId in (select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = " & OrderId & " and  type = 'item' and LEFT(Code,3)='UPS'))")

        If CarrierType < 1 Then
            CarrierType = DB.ExecuteScalar("select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = " & o.OrderId & " and  type = 'item'")
        End If
        If CarrierType < 1 Then
            CarrierType = o.CarrierType
        End If
        Dim smt As ShipmentMethodRow = ShipmentMethodRow.GetRow(DB, CarrierType)
        Dim result As String = String.Empty

        Dim TrackingId As Integer = 0
        Dim ShipmentType As Integer = 0
        Try
            If dt.Rows.Count > 0 Then
                result = Trim(dt.Rows(0)("TrackingNo").ToString())
                TrackingId = CInt(dt.Rows(0)("TrackingId"))
                ShipmentType = CInt(dt.Rows(0)("ShipmentType"))
            End If
            If result = "" Then
                linkTracking = "<a onclick='parent.AddTracking(" & o.OrderId & ")' href ='javascript:void(0);'>Add Tracking Number</a>"
            Else
                linkEdit = "<a onclick='parent.EditTracking(" & o.OrderId & "," & TrackingId & ")' href='javascript:void(0);'>Edit Tracking Number</a>"
                If LCase(Left(smt.Code, 3)) = "ups" Or LCase(Left(smt.Code, 3)) = "fed" Or CarrierType = 15 Then
                    If (ShipmentType = Utility.Common.StandardShippingMethod.Truck) Then
                        linkTracking &= "<a target='_blank'   href='" & result & "'>" & Trim(dt.Rows(0)("Note")) & "</a>"
                    Else
                        If (ShipmentType = Utility.Common.StandardShippingMethod.USPS) Then
                            linkTracking = Resources.Msg.LinkTrackingNumberUSPS
                        ElseIf (ShipmentType = Utility.Common.StandardShippingMethod.FedEx) Then
                            linkTracking = Resources.Msg.LinkTrackingNailSuperStore
                        ElseIf ShipmentType = Utility.Common.StandardShippingMethod.DHL Then
                            linkTracking = Resources.Msg.LinkTrackingNumberDHL
                        ElseIf (ShipmentType = Utility.Common.StandardShippingMethod.UPS) Then
                            linkTracking = Resources.Msg.LinkTrackingNumberUPS
                        ElseIf CarrierType = 15 Then
                            linkTracking = Resources.Msg.LinkTrackingNumberUSPSPriority
                        End If
                        Try
                            If result.Contains("/") Then
                                Dim strTracking As String = String.Empty
                                Dim lstTracking As String() = result.Split("/")
                                For i As Integer = 0 To lstTracking.Length - 1
                                    strTracking &= "<a href='" & String.Format(linkTracking, Trim(lstTracking(i))) & "' target='_blank' >" & lstTracking(i) & "</a><br>"
                                Next
                                linkTracking = strTracking
                            Else
                                linkTracking = String.Format(linkTracking, result)
                                linkTracking = "<a href='" & linkTracking & "' target='_blank' >" & result & "</a>"
                            End If
                        Catch

                        End Try
                        'linkTracking = "<a href='" & linkTracking & "' target='_blank' >" & result & "</a>"
                    End If

                ElseIf CarrierType = Utility.Common.StandardShippingMethod.Truck Then
                    linkTracking &= "<a target='_blank'   href='" & result & "'>" & Trim(dt.Rows(0)("Note")) & "</a>"
                Else
                    linkTracking &= result
                End If
            End If
            Exit Sub
        Catch ex As Exception

        End Try
        linkTracking = String.Empty
    End Sub
End Class

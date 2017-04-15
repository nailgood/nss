
Imports Components
Imports DataLayer
Imports PayPalHandler
Imports PayPal.Payments.DataObjects
Imports PayPal.Payments.Transactions
Imports PayPal.Payments.Common.Utility
Imports Utility
Imports ShippingValidator
Imports System.Collections.Generic

Partial Class Store_Payment
    Inherits SitePage
    Protected o As StoreOrderRow
    Private Member As MemberRow
    Private MoneyEachPoint As Double = SysParam.GetValue("MoneyEachPoint")
    Private MaxMoneySpent As Double = SysParam.GetValue("MaxMoneySpend")
    Private GetPoint As Integer = SysParam.GetValue("GetPoint")

    Public SignatureMoney As Double = 0

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
        If memberId <= 0 And HasAccess() = False Then
            Response.Redirect("/members/login.aspx")
        End If

        If Not Page.IsPostBack Then
            If Cart Is Nothing Then
                Response.Redirect("/store/revise-cart.aspx")
            End If
            o = Cart.Order

            'Check Valid address
            Dim isAllowResetAddress As Boolean = True
            If o.ShippingAddressId < 1 AndAlso o.IsSameAddress = False AndAlso o.BillingAddressId > 0 Then
                isAllowResetAddress = False
            End If

            'Finish OrderNo
            If Not String.IsNullOrEmpty(o.OrderNo) Then
                DB.Close()
                Response.Redirect("/store/confirmation.aspx?OrderId=" & o.OrderId)
            End If

            'Check Flammable
            Dim flammable As Common.FlammableCart = Cart.HasFlammableCartItem()
            If flammable = Common.FlammableCart.BlockedHazMat AndAlso Common.InternationalShippingId().Contains(o.CarrierType) Then
                ShowError(Resources.Alert.CartItemBlockedHazmat2, True)
                Utility.Common.OrderLog(o.OrderId, "Error PageLoad", {Resources.Alert.CountryBlockedHazMat})
                Exit Sub
            ElseIf flammable = Common.FlammableCart.HazMat AndAlso Not Cart.HasCountryHazMat(o.ShipToCountry) Then
                ShowError(Resources.Alert.CountryBlockedHazMat, True)
                Utility.Common.OrderLog(o.OrderId, "Error PageLoad", {Resources.Alert.CountryBlockedHazMat})
                Exit Sub
                'ElseIf flammable = Common.FlammableCart.HazMat AndAlso Cart.CheckShippingSpecialUS() Then
                '    ShowError(Resources.Alert.CountryBlockedHazMat, True)
                '    Utility.Common.OrderLog(o.OrderId, "Error PageLoad", {Resources.Alert.CountryBlockedHazMat})
                '    Exit Sub
            End If

            If flammable = Common.FlammableCart.BlockedHazMat AndAlso (Common.UPSNextDayShippingId() = o.CarrierType Or Common.UPS2DayShippingId() = o.CarrierType) Then
                ShowError(Resources.Alert.CartItemBlockedHazMat, False)
            End If

            If ((o.BillingAddressId < 1 Or o.ShippingAddressId < 1) And String.IsNullOrEmpty(o.OrderNo) And isAllowResetAddress) Then
                If (StoreOrderRow.UpdateValidAddress(o.OrderId)) Then
                    Utility.Common.OrderLog(o.OrderId, "Error PageLoad", {"UpdateValidAddress"})
                    DB.Close()
                    Response.Redirect("/store/payment.aspx?act=address-notvalid")
                End If
            End If

            Dim isNotCompleteAddress As Boolean = MemberAddressRow.IsNotCompleteAddress(DB, memberId)
            If isNotCompleteAddress AndAlso isAllowResetAddress Then
                Dim result As String = String.Empty
                Dim country As String = DB.ExecuteScalar("Select COALESCE(Country,'') from MemberAddress where MemberId=" & memberId & " and AddressType='Billing'")
                If country = "" Or country = "US" Then
                    result = "/store/billing.aspx?type=Billing"
                Else
                    result = "/store/billingint.aspx"
                End If
                Response.Redirect(result)
            End If

            Cart.ResetAllCartData(DB, Cart)
            If MemberRow.MemberInGroupWHS(o.MemberId) Then
                ltrSecPointTitle.Text = "Coupon"
                divCashPoint.Visible = False
            End If

            If Cart.CheckTotalFreeSample(o.OrderId) Then
                Cart.RemoveFreeSample()
            End If

            Dim MinFreeGiftLevelInvalid As Double = ShoppingCart.GetMinFreeGiftLevelInValid(o.OrderId)
            If MinFreeGiftLevelInvalid > 0 Then
                Cart.RemoveFreeGift()
            End If

            Member = MemberRow.GetRow(o.MemberId)
            If CheckOverrideAddress(o) Then ''not have billing address
                StoreOrderRow.CopyBillShipAddressFromMemberAddress(DB, o.MemberId)
                Cart.Order = StoreOrderRow.GetRow(DB, o.OrderId)
                o = Cart.Order
            End If

            CheckResidentialAddress(o)
            BindData()
            BindAddress()
            LoadDrpCashPoint()
            BindCardMonth()
            BindCardYear()
            Utility.Common.DeleteCachePopupCart(o.OrderId)
            ucCouponList.Cart = Cart

            If Utility.Common.CheckOrderShippingIsCompleteIntAddress(DB, Cart.Order) Then
                liDiffAddress.Attributes.Add("style", "display:none")
            End If

            'Khoa tat: Hazardous Material Fee
            'If Member.IsInternational Then
            '    Dim SKUFlammable As String = Utility.Common.GetFlammableSKU(DB, o.OrderId)
            '    If Not String.IsNullOrEmpty(SKUFlammable) Then
            '        ShowError("The item " & SKUFlammable & " is not available for customer outsite of 48 states within continental USA. Please go back to <a href=""revise-cart.aspx"">shopping cart</a> and remove them before proceeding to check out.", True)
            '        Exit Sub
            '    End If
            'End If

            ''Check ship to Russia
            'If Utility.Common.IsShipToRussia(o) Then
            '    ShowError(Resources.Alert.Russia, True)
            '    Utility.Common.OrderLog(o.OrderId, "Error Page Load", {Resources.Alert.Russia})
            '    Exit Sub
            'End If

            'If Utility.Common.IsOrderShippingPOBoxAddress(o) Then
            '    ShowError(Resources.Alert.POBoxShippingAddress, False)
            '    Utility.Common.OrderLog(o.OrderId, "Error Page Load", {Resources.Alert.POBoxShippingAddress})
            '    Exit Sub
            'End If

            Dim message As String = String.Empty
            message = Cart.CheckOrderPriceMin(o, Resources.Alert.OrderMin)
            If Not String.IsNullOrEmpty(message) Then
                ShowError(message, True)
                Utility.Common.OrderLog(o.OrderId, "Error Page Load", {message})
                Exit Sub
            End If

            Dim shippingTBD As String = StoreOrderRow.GetShippingTBDName(o.OrderId)
            If Not String.IsNullOrEmpty(shippingTBD) Then
                message = String.Format(Resources.Alert.ShippingInvalid, shippingTBD)
                ShowError(message, False)
            End If
            LoadMetaData(DB, "/store/payment.aspx")

            Utility.Common.OrderLog(o.OrderId, "Page Load", Nothing)
        End If
    End Sub
    Private Sub BindCardMonth()
        drlCardExpireMonth.Items.Add(New ListItem(" ", ""))
        For i As Integer = 0 To 11
            Dim j As Integer = i + 1
            Dim monthname As String = New System.Globalization.DateTimeFormatInfo().MonthNames(i).ToString()
            Dim item As ListItem = New ListItem(monthname, j.ToString())
            drlCardExpireMonth.Items.Add(item)
        Next
    End Sub
    Private Sub BindCardYear()
        Dim StartYear As Integer = Year(Now)
        Dim EndYear As Integer = Year(Now) + 10
        drlCardExpireYear.Items.Add(New ListItem(" ", ""))
        For i As Integer = StartYear To EndYear
            Dim item As ListItem = New ListItem(i.ToString(), i.ToString())
            drlCardExpireYear.Items.Add(item)
        Next
    End Sub
    Private Sub BindAddress()
        divBillingAddress.InnerHtml = Utility.Common.GetHTMLOrderBillingAddress(DB, o)
        ucListBillingAddress.Cart = Cart
        ucListBillingAddress.CurrentAddressType = Utility.Common.MemberAddressType.Billing.ToString()
        ucListShippingAddress.Cart = Cart
        ucListShippingAddress.CurrentAddressType = Utility.Common.MemberAddressType.Shipping.ToString()
        If (o.IsSameAddress) Then
            rdoSameAddress.Checked = True
            divListShippingAddress.Attributes.Add("style", "display:none")
        Else
            rdoDiffAddress.Checked = True
        End If

        If String.IsNullOrEmpty(o.Email) Then
            Try
                If Not StoreOrderRow.UpdateEmailNull(DB, o.OrderId) Then
                    Email.SendError("ToError500", "UpdateEmailNull", "OrderId: " & o.OrderId & GetSessionList())
                End If
            Catch ex As Exception
                Email.SendError("ToError500", "UpdateEmailNull Error", "OrderId: " & o.OrderId & GetSessionList())
            End Try

        End If
    End Sub


    Private Sub LoadDrpCashPoint()
        Dim j As Integer = 1
        Dim i As Integer = 0
        Dim TotalPointEarned As Integer = Cart.GetCurrentBalancePoint(PointAvailable)
        ltMsgPoint.Text = "You currently have " & SitePage.NumberToString(PointAvailable) & " redeemable reward points"
        If PointAvailable <= 0 Then
            ulPointReward.Visible = False
            'Exit Sub
        End If
        Dim PurchasePoint As Integer = Cart.Order.PurchasePoint
        Dim MoneyEarned As Double = (TotalPointEarned + PurchasePoint) * MoneyEachPoint
        Dim StepMoney As Integer = Utility.ConfigData.StepMoney
        Dim LimitSpend As Integer = 0
        If MaxMoneySpent >= MoneyEarned Then
            LimitSpend = Math.Floor(MoneyEarned)
        Else
            LimitSpend = CInt(MaxMoneySpent)
        End If
        If Cart.SubTotalPuChasePoint - Cart.Order.PointAmountDiscount <= LimitSpend And LimitSpend > 0 Then
            LimitSpend = Math.Floor((Cart.SubTotalPuChasePoint - Cart.Order.PointAmountDiscount) / GetPoint)
        End If
        drpCashPoint.Items.Insert(0, New ListItem("Please select one", 0))
        If MoneyEarned < 1 Then
            If MoneyEarned > 0 Then
                ltrMsgPointMin.Visible = True
            End If
            ulPointReward.Visible = False
        Else
            If MoneyEarned > 0 And MoneyEarned < StepMoney Then
                StepMoney = 1
                LimitSpend = Math.Floor(MoneyEarned / GetPoint)
            End If
            For i = StepMoney To LimitSpend Step 0
                If i < LimitSpend Then
                    drpCashPoint.Items.Insert(j, New ListItem(SitePage.NumberToString(i / MoneyEachPoint) & " points save $" & FormatNumber(i), CInt(i / MoneyEachPoint)))
                ElseIf i >= LimitSpend Then
                    drpCashPoint.Items.Insert(j, New ListItem(SitePage.NumberToString(LimitSpend / MoneyEachPoint) & " points save $" & FormatNumber(LimitSpend), CInt(LimitSpend / MoneyEachPoint)))
                    Exit For
                End If
                If i + StepMoney > LimitSpend Then
                    i = LimitSpend
                Else
                    i = i + StepMoney
                End If
                j = j + 1
            Next
        End If
        If PurchasePoint > 0 Then
            drpCashPoint.SelectedValue = PurchasePoint
        End If
    End Sub


    Private Sub RecalculateShipping(ByVal selectedMethod As Integer)
        If (o.ShipToCountry = "US" OrElse (o.IsSameAddress AndAlso o.BillToCountry = "US")) AndAlso Not Cart.CheckShippingSpecialUS Then

            If selectedMethod = Nothing Or selectedMethod = Utility.Common.USPSPriorityShippingId Then
                selectedMethod = Utility.Common.DefaultShippingId
            ElseIf selectedMethod = Utility.Common.TruckShippingId.ToString() Then
                'Kiem tra xem CarrierType = 4 Freight Delivery nhung ko co item Freight
                Dim countRestrict As Integer = DB.ExecuteScalar("SELECT COUNT(CartItemId) FROM StoreCartItem WHERE [Type] = 'item' AND IsOversize=1 AND OrderId = " & o.OrderId)
                If countRestrict = 0 Then
                    selectedMethod = Utility.Common.DefaultShippingId
                End If
            ElseIf selectedMethod = Utility.Common.PickupShippingId.ToString() AndAlso Not (o.ShipToCountry = "US" AndAlso o.ShipToCounty = "IL") Then
                selectedMethod = Utility.Common.DefaultShippingId
            ElseIf selectedMethod = Common.FirstClassShippingId Or selectedMethod = Common.USPSPriorityShippingId Then
                selectedMethod = Utility.Common.DefaultShippingId
            Else
                Dim countRestrict As Integer = DB.ExecuteScalar("Select count(*) from ShippingFedExRestricted where MethodId=" & selectedMethod & " and '" & o.ShipToZipcode & "' between LowValue and HighValue")
                If (countRestrict > 0) Then
                    selectedMethod = Utility.Common.DefaultShippingId
                Else
                    'Khoa tat: Hazardous Material Fee
                    'If Cart.HasFlammableCartItem() Then
                    '    If (selectedMethod <> Utility.Common.PickupShippingId) Then
                    '        selectedMethod = Utility.Common.DefaultShippingId
                    '    End If
                    'End If
                End If
            End If

            Dim SQL As String
            If selectedMethod > 0 Then
                If selectedMethod = Utility.Common.PickupShippingId.ToString() Then
                    SQL = "update storecartitem set carriertype = " & selectedMethod & " where type = 'item' and orderid = " & Cart.Order.OrderId
                    ''SQL = "update storecartitem set carriertype = case when isoversize = 1 then " & Utility.Common.TruckShippingId & " else " & DB.Number(selectedMethod) & " end where type = 'item' and orderid = " & Cart.Order.OrderId
                ElseIf selectedMethod = Utility.Common.TruckShippingId.ToString() Then
                    SQL = "update storecartitem set carriertype=" & Utility.Common.TruckShippingId & " where type = 'item' and orderid = " & Cart.Order.OrderId
                Else
                    SQL = "update storecartitem set carriertype = case when isoversize = 1 and " & DB.NullNumber(selectedMethod) & " not in (" & Utility.Common.PickupShippingId & ") then " & Utility.Common.TruckShippingId & " else case when ishazmat = 1 then case when " & DB.Number(selectedMethod) & " in " & DB.NumberMultiple(Utility.Common.NonExpeditedShippingIds) & " then " & DB.Number(selectedMethod) & "	else carriertype end else " & DB.Number(selectedMethod) & " end end where type = 'item' and orderid = " & Cart.Order.OrderId
                End If
            Else
                SQL = "update storecartitem set carriertype = case when isoversize = 1 and " & DB.NullNumber(selectedMethod) & " not in (" & Utility.Common.PickupShippingId & ") then " & Utility.Common.TruckShippingId & " else case when ishazmat = 1 then case when " & DB.Number(selectedMethod) & " in " & DB.NumberMultiple(Utility.Common.NonExpeditedShippingIds) & " then " & DB.Number(selectedMethod) & "	else carriertype end else " & DB.Number(selectedMethod) & " end end where type = 'item' and orderid = " & Cart.Order.OrderId
            End If
            ''''''''End'''''''''
            DB.ExecuteSQL(SQL)
            o.CarrierType = IIf(selectedMethod < 1, Utility.Common.DefaultShippingId, selectedMethod)
            o.Update()
        Else
            o.CarrierType = Utility.Common.USPSPriorityShippingId
        End If

        If selectedMethod = Utility.Common.DefaultShippingId AndAlso GetQueryString("act") = "newaddress" Then
            StoreOrderRow.ResetCartItemHandlingFee(DB, Utility.Common.GetCurrentOrderId())
        End If
    End Sub
    Private Sub ShowError(ByVal msg As String, ByVal bFix As Boolean)
        ltrLoadMsg.Text = String.Format("<span style='display:none;' id='loadMsg{0}'>{1}</span>", IIf(bFix, "Fix", ""), msg)
    End Sub
    Private Sub BindData()
        If Session("CheckOutGuest") IsNot Nothing AndAlso Session("CheckOutGuest") = "1" Then
            divCashPoint.Visible = False
            If Member IsNot Nothing AndAlso Member.GuestStatus = 2 Then
                divCashPoint.Visible = True
            End If
        End If

        Dim CarrierType As Integer = Common.GetDefaultShippingByOrderId(DB, o.OrderId)
        RecalculateShipping(CarrierType)
        CarrierType = o.CarrierType
        ucShippingList.Cart = Cart
        Dim ds As DataSet
        If (o.ShipToCountry = "US" AndAlso Cart.CheckShippingSpecialUS = False) OrElse (o.BillToCountry = "US" AndAlso o.IsSameAddress AndAlso Cart.CheckShippingSpecialUS = False) Then
            ds = Cart.GetShippingMethods()

            If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
                ucShippingList.ShippingMethod = CarrierType
                ucShippingList.ListShippingMethod = ds
                ''RadShipVia.SelectedValue = CarrierType
                Dim isHasSelect As Boolean = False
                For Each row As DataRow In ds.Tables(0).Rows
                    If (CarrierType = row("MethodId")) Then
                        isHasSelect = True
                        Exit For
                    End If
                Next

                If (Not isHasSelect) Then
                    CarrierType = ds.Tables(0).Rows(0)("MethodId")
                    RecalculateShipping(CarrierType)

                    If (o.CarrierType = Utility.Common.DefaultShippingId) Then
                        'Reset Handling Fee
                        DB.ExecuteSQL("UPDATE StoreCartItem SET SpecialHandlingFee = CASE WHEN CarrierType=" & Utility.Common.DefaultShippingId & " THEN [dbo].[fc_StoreCartItem_GetSpecialHandlingFee](CartItemId,ItemId,AddType) else 0 end where OrderId=" & o.OrderId & " and Type<>'carrier'")
                    Else
                        DB.ExecuteSQL("UPDATE StoreCartItem SET CarrierType= " & CarrierType & ", SpecialHandlingFee=0 WHERE OrderId=" & o.OrderId & ";UPDATE StoreOrder SET CarrierType= " & CarrierType & " WHERE OrderId=" & o.OrderId)
                    End If
                    Session.Remove("CheckResidential")
                    Response.Redirect("/store/payment.aspx?act=shipping-" & CarrierType)
                End If
            Else
                RecalculateShipping(CarrierType)
                Email.SendError("ToErrorPayment", "[SubmitOrder] o.CarrierType <> RadShipVia.SelectedValue", "OrderId=" & o.OrderId & "<br>ShipToCode=" & o.ShipToCode & "<br>o.CarrierType=" & o.CarrierType & "<br>RadShipVia.SelectedValue=" & CarrierType)
                Response.Redirect("/store/payment.aspx?act=shipping-us-null")
            End If

            Cart.RecalculateOrderSignatureConfirmation(o, True)
            o.Update()
            ucShippingOption.Cart = Cart
            ucShippingOption.Order = o
            ucShippingOption.ShippingMethod = CarrierType
        Else
            DB.ExecuteSQL("delete from storecartitem where type = 'carrier' and orderid = " & o.OrderId)
            divShippingOption.InnerHtml = String.Empty
            Cart.RecalculateOrderSignatureConfirmation(o, False)
            If (o.BillToCountry <> "US") Then
                o.IsSameAddress = True
            Else
                o.ShipToCountry = o.BillToCountry
            End If
            DB.ExecuteSQL("Update StoreCartItem set SpecialHandlingFee = 0 where OrderId=" & o.OrderId & "  and Type<>'carrier'; Update StoreOrder set TotalSpecialHandlingFee=0 where OrderId=" & o.OrderId)
        End If

        litOrderEmail.Text = String.Format("{0} <span class=""help"" onclick=""ShowTipEmailOrder();"">&nbsp;</span>", o.Email)

        If Not Page.IsPostBack Then
            Cart.RecalculateOrderDetail("submitorder.BindData")
        End If

        Dim subtotal As Double = o.SubTotal
        Dim HasOversize As Boolean = Cart.HasOversizeItems()
        If HasOversize Then
            dvFreightOption.Visible = True
            ucFreightOption.Cart = Cart
        End If

        Dim ucCartSummary As controls_checkout_cart_summary = Me.Master.FindControl("ucCartSummary")
        If Not ucCartSummary Is Nothing Then
            ucCartSummary.Cart = Cart
        End If

        Dim ucPointSummary As controls_checkout_reward_point_summary = Me.Master.FindControl("ucPointSummary")
        If Not ucPointSummary Is Nothing Then
            ucPointSummary.Cart = Cart
        End If
    End Sub
    Private Sub CheckResidentialAddress(ByVal o As StoreOrderRow)
        If o.ShipToCountry <> "US" OrElse (o.ShipToCountry = "US" AndAlso Common.IsCheckShippingSpecialUSState(o.ShipToCounty)) Then
            Exit Sub
        End If

        If Session("CheckResidential") Is Nothing Then
            Dim xav As Validator = CheckAddressType(DB, o)
            If Not xav Is Nothing Then
                SQL = String.Format("UPDATE StoreOrder SET ShipToAddressType = {0}, IsSignatureConfirmation = {1}, SignatureConfirmation = {2} WHERE OrderId = {3}", o.ShipToAddressType, CInt(o.IsSignatureConfirmation), o.SignatureConfirmation, o.OrderId)
                DB.ExecuteSQL(SQL)
            End If
            Session("CheckResidential") = 1
        End If
    End Sub
    Private Function CheckOverrideAddress(ByVal currentOrder As StoreOrderRow) As Boolean
        If (String.IsNullOrEmpty(currentOrder.BillToAddress)) Then 'not have billing address
            Return True
        End If
        Return False
    End Function

    Protected Sub ltrPaypalCheckout_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ltrPaypalCheckout.Click
        PayPalCheckOut()
    End Sub
    Public Sub PayPalCheckOut()

        Dim strError As String = String.Empty
        Dim linkRedirect As String = String.Empty
        Dim memberId As Integer = Common.GetMemberIdFromCartCookie()
        Dim arrMail As New ArrayList
        Dim orderId As Integer = 0

        Try
            Dim objOrder As StoreOrderRow = Cart.Order
            Command.logOrderDevice(objOrder.OrderId, Request.UserAgent)
            CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & objOrder.OrderId)
            Dim strException As String = String.Empty
            Dim strOrder As String = objOrder.OrderId
            Dim sb As New StringBuilder()
            Try
                Dim IsLive As String = SysParam.GetValue("API_IS_LIVE")
                Dim sActURL As String = String.Empty
                If Not String.IsNullOrEmpty(IsLive) AndAlso IsLive = "1" Then
                    sActURL = "https://www.paypal.com/cgi-bin/webscr?"
                Else
                    sActURL = "https://www.sandbox.paypal.com/us/cgi-bin/webscr?"
                End If

                Dim PayPalEmail As String = SysParam.GetValue("PP_Email")
                Dim objPTRS As New PostToRemoteServer(sActURL)
                Dim sci As StoreCartItemCollection = Cart.GetCartItemsForPaypalProcess()

                'Insert cart vao bang StoreCartItemTmp de lam co so revise sau nay
                'StoreCartItemRow.InsertItemCartRevise(DB, Cart.Order.OrderId)
                Dim i As Integer = 0
                Dim tmpCartItem As String = String.Empty
                For i = 0 To sci.Count - 1
                    sci.Item(i).Price = CDbl(NSS.GetCartPricing(DB, sci.Item(i), True))
                    tmpCartItem &= sci.Item(i).SKU & " | " & sci.Item(i).ItemName & " | Qty=" & sci.Item(i).Quantity & "<br/>"
                Next

                'Send email warning admin
                Dim AllDiscount As Double
                If Cart.Order.TotalPurchasePoint > 0 Then
                    AllDiscount = Cart.Order.Discount + objOrder.TotalPurchasePoint 'include cash point
                Else
                    AllDiscount = Cart.Order.Discount  'only promotion
                End If

                'Tracking by email
                sb.Append("Date: " & DateTime.Now.ToString())
                sb.Append("<br>Total Price: $" & Cart.Order.Total.ToString())
                sb.Append("<br>OrderId: " & Cart.Order.OrderId.ToString())
                sb.Append("<br>MemberId: " & Cart.MemberId)
                sb.Append("<br>Shipping: $" & Cart.Order.Shipping.ToString())
                sb.Append("<br>Special Handling Fee: $" & Cart.Order.TotalSpecialHandlingFee.ToString())
                sb.Append("<br>Hazardous Material Fee: $" & Cart.Order.HazardousMaterialFee.ToString())
                sb.Append("<br>Tax: $" & Cart.Order.Tax.ToString())
                If Cart.Order.TotalPurchasePoint > 0 Then
                    Dim moneyEachPoint As Double = SysParam.GetValue("MoneyEachPoint")
                    Dim resultPoint As Double = objOrder.TotalPurchasePoint * moneyEachPoint
                    sb.Append("<br>Cashpoint: " & objOrder.TotalPurchasePoint & " pts ($" & resultPoint & ")")
                End If
                If Cart.Order.Discount > 0 Then
                    sb.Append("<br>Promotion: $" & Cart.Order.Discount)
                End If
                sb.Append("<br>Total Discount: $" & AllDiscount.ToString())
                sb.Append("<br><br/>Cart Items:<br/>")
                sb.Append(tmpCartItem)

                Dim bSend As Boolean = False
                Dim lstPaypalPost As String = String.Empty
                Common.OrderLog(orderId, "PayPal Checkout", Nothing)
                Try
                    lstPaypalPost = PayPalHelper.PaypalItemListNew(sci, Cart.Order.Shipping + objOrder.TotalSpecialHandlingFee + objOrder.HazardousMaterialFee, Cart.Order.Tax, AllDiscount, PayPalEmail, Cart.Order.OrderId.ToString, Cart.Order.MemberId.ToString)
                    objPTRS.Post(lstPaypalPost)
                Catch ex As Exception
                    If Not ex.ToString().Contains("Thread was being aborted") Then
                        strException = ex.ToString()
                        sb.Append(strException)
                    End If

                    If Not String.IsNullOrEmpty(strException) Then
                        Email.SendError("ToErrorPayment", "[Paypal] Error", "Order = " & strOrder & "<br>Exception = " & strException)
                    Else
                        Email.SendReport("ToReportPayment", "[Paypal] Click", sb.ToString() & "<br/>Data Post Paypal=" & lstPaypalPost)
                    End If

                    bSend = True
                End Try

                If Not bSend Then
                    Email.SendReport("ToReportPayment", "[Paypal] Click No Catch Exception", "" & sb.ToString())
                End If
            Catch ex As Exception
                If Not ex.ToString() <> strException Then
                    strException &= ex.ToString()
                End If
            End Try

            If Not String.IsNullOrEmpty(strException) Then
                Email.SendError("ToErrorPayment", "Paypal Error", "Order = " & strOrder & "<br>Exception = " & strException)
            End If

        Catch ex As Exception
            strError = ex.Message
        End Try

        If String.IsNullOrEmpty(strError) Then
            AddError(strError)
        End If
    End Sub
End Class

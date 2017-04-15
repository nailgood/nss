Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_cart_summary
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Private m_Cart As ShoppingCart = Nothing
    Private m_TotalPointAvailable As Integer
    Public m_BuyPointTotal As Integer = 0
    Public m_DisplayRowBuyPoint As String = String.Empty
    Public m_TotalRewardsPoint As Integer
    Public m_PurchasePoint As Integer
    Public CssPadding As String = String.Empty
    Public Property Cart() As ShoppingCart
        Set(ByVal value As ShoppingCart)
            m_Cart = value
        End Set
        Get
            Return m_Cart
        End Get
    End Property

    Public Property TotalPointAvailable() As Integer
        Set(ByVal value As Integer)
            m_TotalPointAvailable = value
        End Set
        Get
            Return m_TotalPointAvailable
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Session("cartRender") Is Nothing AndAlso Cart Is Nothing Then
            Cart = Session("cartRender")
        Else
            If Request.RawUrl.Contains("/store/cart.aspx") AndAlso Cart IsNot Nothing AndAlso Cart.Order IsNot Nothing Then
                If Not String.IsNullOrEmpty(Cart.Order.ShipToZipcode) AndAlso Cart.Order.SubTotal > 0 Then
                    Exit Sub
                End If
            End If
        End If

        ltrTitleTotal.Text = "Order SubTotal"
        If Cart Is Nothing Then
            Cart = New ShoppingCart(DB)
        End If
        If Not Cart Is Nothing AndAlso Not Cart.Order Is Nothing AndAlso Cart.GetCartItemCount() > 0 Then
            If HttpContext.Current.Request.RawUrl.Contains("revise-cart.aspx") Then
                BindDataInCartPage(Cart, Cart.Order)
                trCheckOut.Visible = True
                divContinueShipping.Visible = False
                divReviseCart.Visible = False
            ElseIf HttpContext.Current.Request.RawUrl.Contains("cart.aspx") Or HttpContext.Current.Request.RawUrl.Contains("/includes/popup/free-item.aspx") Then
                If Session("CartRenderEstimate") IsNot Nothing Then
                    ltrTitleTotal.Text = "Order Total"
                End If

                BindDataInCartPage(Cart, Cart.Order)
                trCheckOut.Visible = True
                divContinueShipping.Visible = True
                divReviseCart.Visible = False
            ElseIf (HttpContext.Current.Request.RawUrl.Contains("payment.aspx") Or HttpContext.Current.Request.RawUrl.Contains("confirmation.aspx") Or HttpContext.Current.Request.RawUrl.Contains("orderhistory/view.aspx")) Then
                ltrTitleTotal.Text = "Order Total"
                BindData(Cart, Cart.Order)
                trCheckOut.Visible = False
                divContinueShipping.Visible = False
                If Not (HttpContext.Current.Request.RawUrl.Contains("confirmation.aspx") Or HttpContext.Current.Request.RawUrl.Contains("orderhistory/view.aspx")) Then
                    divReviseCart.Visible = True
                Else
                    divReviseCart.Visible = False
                End If
            Else
                Me.Visible = False
            End If
        Else
            Me.Visible = False
        End If

    End Sub
    Public Sub BindDataInCartPage(ByVal Cart As ShoppingCart, ByVal o As StoreOrderRow)
        trSalesTax.Visible = False
        trHeader.Visible = False
        trPoint.Visible = False
        trLevelPoint.Visible = False
        trHandlingFee.Visible = False
        trHazardous.Visible = False
        Dim amt As Double = o.RawPriceDiscountAmount()
        lblMerchSubTotal.Text = FormatCurrency(o.BaseSubTotal + IIf(amt > 0, amt, 0), 2)
        If o.TotalDiscount > 0 OrElse amt > 0 Then lblPromotionalDiscount.Text = "-" & FormatCurrency(o.TotalDiscount + IIf(amt > 0, amt, 0), 2) Else trPromotionalDiscount.Visible = False

        If Cart.CoupontDiscount > 0 Then
            trCouponDiscount.Visible = True
            lblCouponDiscount.Text = "-" & FormatCurrency(Cart.CoupontDiscount, 2)
        End If

        If Session("CartRenderEstimate") IsNot Nothing OrElse ltrTitleTotal.Text = "Order Total" Then
            Dim flammable As Utility.Common.FlammableCart = Cart.HasFlammableCartItem()
            If flammable = Utility.Common.FlammableCart.HazMat And (Cart.IsHazardousMaterialFee()) Then
                trHazardous.Visible = True
                lblHazardous.Text = FormatCurrency(ShipmentMethod.GetValue(o.CarrierType, Utility.Common.ShipmentValue.HazMatFee), 2)
                o.Total += CDbl(ShipmentMethod.GetValue(o.CarrierType, Utility.Common.ShipmentValue.HazMatFee))
            End If

            lblSubTotal.Text = FormatCurrency(o.Total, 2)
        Else
            lblSubTotal.Text = FormatCurrency(o.SubTotal, 2)
        End If

        If o.PurchasePoint > 0 Then
            trPoint.Visible = True
            lblPurchasePoint.Text = "-" & FormatCurrency(o.TotalPurchasePoint)
        End If

        If o.PointAmountDiscount > 0 Then
            lblDiscPoint.Text = "-" & FormatCurrency(o.PointAmountDiscount, 2)
        End If

        Dim htmlShipping As String = String.Empty
        If (Not Session("cartRenderShipping") Is Nothing) Then '' render data when Estimate shipping in cart page
            htmlShipping = Session("cartRenderShipping")
            litShippingDetails.Text = htmlShipping
            trSalesTax.Visible = False

            If (o.Tax > 0) Then
                trSalesTax.Visible = True
                lblSalesTax.Text = FormatCurrency(o.Tax, 2)
            End If

            If htmlShipping.Contains("Standard Shipping") AndAlso (o.TotalSpecialHandlingFee > 0) Then
                trHandlingFee.Visible = True
                lblHandlingFee.Text = FormatCurrency(o.TotalSpecialHandlingFee, 2)
            Else
                trHandlingFee.Visible = False
            End If
        End If

        If Not String.IsNullOrEmpty(litShippingDetails.Text) AndAlso Not litShippingDetails.Text.Contains("FREE") AndAlso o.CarrierType = Utility.Common.DefaultShippingId() Then
            Dim dSpendFreeShipping As Double = CDbl(SysParam.GetValue("FreeShippingOrderAmount"))
            dSpendFreeShipping -= o.SubTotal
            lblSpendFreeShipping.Text = String.Format(Resources.Msg.SpendFreeShipping, dSpendFreeShipping)
            trSpendFreeShipping.Visible = True
        End If

        If o.Total >= 100 Then
            CssPadding = "tbl-summary-stretch"
        End If
    End Sub

    Public Sub BindData(ByVal Cart As ShoppingCart, ByVal o As StoreOrderRow)

        Dim amt As Double = o.RawPriceDiscountAmount()
        Dim MerchSubTotal As Double = o.BaseSubTotal() + IIf(amt > 0, amt, 0)
        Dim subtotal As Double = o.SubTotal
        lblMerchSubTotal.Text = FormatCurrency(MerchSubTotal, 2)
        If o.TotalDiscount > 0 OrElse amt > 0 Then lblPromotionalDiscount.Text = "-" & FormatCurrency(o.TotalDiscount + IIf(amt > 0, amt, 0), 2) Else trPromotionalDiscount.Visible = False

        'Long add show coupon discout
        If Cart.CoupontDiscount > 0 Then
            trCouponDiscount.Visible = True
            lblCouponDiscount.Text = "-" & FormatCurrency(Cart.CoupontDiscount, 2)
        End If 'End

        Dim Additional As Double = Cart.GetAdditionalFreightCharges()
        Dim HasOversize As Boolean = Cart.HasOversizeItems()
        Dim exempt As Double = Cart.GetAmountExempt()
        If o.PurchasePoint > 0 Then
            lblPurchasePoint.Text = "-" & FormatCurrency(o.TotalPurchasePoint)
        Else
            trPoint.Visible = False
        End If

        If o.PointAmountDiscount > 0 Then
            lblMLevelPoint.Text = o.PointLevelMessage
            lblDiscPoint.Text = "-" & FormatCurrency(o.PointAmountDiscount)
        Else
            trLevelPoint.Visible = False
        End If

        lblSubTotal.Text = FormatCurrency(o.Total)

        trSalesTax.Visible = False
        If o.BillToCountry = "US" Then
            If o.ShipToCounty = "IL" OrElse (o.BillToCounty = "IL" AndAlso o.IsSameAddress) Then
                trSalesTax.Visible = True
                lblSalesTax.Text = FormatCurrency(o.Tax, 2)
            End If
        End If

        trHazardous.Visible = False
        If o.OrderNo IsNot Nothing AndAlso (o.OrderNo.Contains("A") Or o.OrderNo.Contains("E")) Then
            trHazardous.Visible = False
        Else
            Dim flammable As Utility.Common.FlammableCart = Cart.HasFlammableCartItem()
            If flammable = Utility.Common.FlammableCart.HazMat And (Cart.IsHazardousMaterialFee() OrElse o.HazardousMaterialFee > 0) Then
                trHazardous.Visible = True
                lblHazardous.Text = FormatCurrency(ShipmentMethod.GetValue(o.CarrierType, Utility.Common.ShipmentValue.HazMatFee), 2)
            End If
        End If


        If (o.ShipToAddressType = 2 Or o.ShipToAddressType = 0) AndAlso o.ResidentialFee > 0 Then
            trResidential.Visible = True
            lblResidentil.Text = FormatCurrency(o.ResidentialFee, 2)
        Else
            trResidential.Visible = False
        End If

        If (o.TotalSpecialHandlingFee > 0) Then
            trHandlingFee.Visible = True
            lblHandlingFee.Text = FormatCurrency(o.TotalSpecialHandlingFee, 2)
        Else
            trHandlingFee.Visible = False
        End If

        If (o.IsSameAddress = False AndAlso o.ShipToZipcode = Nothing) OrElse (o.IsSameAddress = True AndAlso o.BillToZipcode = Nothing AndAlso o.BillToCountry = "US") Then
            ReFreshSummaryUS(o, amt)
        Else
            litShippingDetails.Text = Cart.GetShippingLines()
        End If

        If o.Total >= 100 Then
            CssPadding = "tbl-summary-stretch"
        End If
    End Sub
    Private Sub ReFreshSummaryUS(ByVal o As StoreOrderRow, ByVal amt As Double)
        lblMerchSubTotal.Text = FormatCurrency(Cart.Order.BaseSubTotal + IIf(amt > 0, amt, 0), 2)
        If Cart.Order.TotalDiscount > 0 OrElse amt > 0 Then lblPromotionalDiscount.Text = "-" & FormatCurrency(Cart.Order.TotalDiscount + IIf(amt > 0, amt, 0), 2) Else trPromotionalDiscount.Visible = False
        If Cart.CoupontDiscount > 0 Then
            trCouponDiscount.Visible = True
            lblCouponDiscount.Text = "-" & FormatCurrency(Cart.CoupontDiscount, 2)
        End If
        lblSubTotal.Text = FormatCurrency(Cart.Order.SubTotal, 2)
        If Cart.Order.PurchasePoint > 0 Then
            trPoint.Visible = True
            lblPurchasePoint.Text = "-" & FormatCurrency(Cart.Order.TotalPurchasePoint)
        End If

        If Cart.Order.PointAmountDiscount > 0 Then
            lblDiscPoint.Text = "-" & FormatCurrency(Cart.Order.PointAmountDiscount, 2)
        End If
        litShippingDetails.Text = String.Empty
    End Sub
End Class

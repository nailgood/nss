Option Strict Off
Imports Components
Imports DataLayer
Imports System.Data
Imports System.IO
Imports System.Web
Imports System.Net
Imports Utility

Partial Class controls_PopupCart
    Inherits ModuleControl

    Public m_Cart As ShoppingCart
    Public subTotal As Double
    Public merchandiseSubTotal As Double
    Public cartItemCount As Integer = 0
    Public cartList As New StoreCartItemCollection
    Public isAllowShowPopupCart As Boolean = True
    Protected CartItemId As String
    Protected textFreeShipping As String = String.Empty
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property


    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Utility.Common.IsViewFromAdmin() Then
            Me.Visible = False
            Exit Sub
        End If

        If m_Cart Is Nothing Then
            If HttpContext.Current.Session("cartRender") Is Nothing Then
                m_Cart = New ShoppingCart(DB, Utility.Common.GetCurrentOrderId())
            Else
                m_Cart = HttpContext.Current.Session("cartRender")
            End If
        End If

        If m_Cart IsNot Nothing Then
            LoadCartData(m_Cart)
        End If

    End Sub
    Public Sub LoadCartData(ByVal objCart As ShoppingCart)
        If objCart IsNot Nothing Then
            If objCart.Order IsNot Nothing Then
                Dim so As StoreOrderRow = objCart.Order
                cartItemCount = objCart.GetCartItemCount()

                Dim amt As Double = so.RawPriceDiscountAmount()
                merchandiseSubTotal = so.BaseSubTotal + IIf(amt > 0, amt, 0)
                If so.TotalDiscount > 0 OrElse amt > 0 Then
                    lblPromotionalDiscount.Text = "-" & FormatCurrency(so.TotalDiscount + IIf(amt > 0, amt, 0), 2)
                    divsave.Visible = True
                Else
                    divsave.Visible = False
                End If

                If so.SubTotal = 0 AndAlso cartItemCount > 0 AndAlso Utility.Common.GetCurrentMemberId() = 0 Then
                    Dim o As StoreOrderRow = StoreOrderRow.GetRow(DB, so.OrderId)
                    If o.CreateDate = DateTime.MinValue AndAlso o.SubTotal = 0 Then
                        Dim orderId As Integer = StoreOrderRow.InsertUniqueOrder(DB, HttpContext.Current.Request.ServerVariables("REMOTE_ADDR"), 0, HttpContext.Current.Session.SessionID)
                        Utility.Common.SetOrderToCartCookie(orderId)
                        Session("OrderId") = orderId
                        DB.ExecuteScalar("UPDATE StoreCartItem SET OrderId = " & orderId & " WHERE OrderId = " & so.OrderId)
                        objCart = New ShoppingCart(DB, orderId)
                        objCart.RecalculateOrderDetail("Reset Order Guest")
                        Response.Redirect(Request.RawUrl)
                    End If
                End If

                subTotal = so.SubTotal
                If so.PurchasePoint > 0 Then
                    divPoint.Visible = True
                    lblPurchasePoint.Text = "-" & FormatCurrency(so.TotalPurchasePoint)
                Else
                    divPoint.Visible = False
                End If

                If objCart.CoupontDiscount > 0 Then
                    divDiscount.Visible = True
                    lblCouponDiscount.Text = "-" & FormatCurrency(objCart.CoupontDiscount, 2)
                Else
                    divDiscount.Visible = False
                End If

                cartList = StoreCartItemRow.GetListPopupCart(so.OrderId)
                rptCartItems.DataSource = cartList
                rptCartItems.DataBind()

                If Not String.IsNullOrEmpty(so.ShipToCountry) AndAlso (so.ShipToCountry <> "US" Or (so.ShipToCountry = "US" AndAlso Common.CheckShippingSpecialUS(so))) Then
                    Exit Sub
                End If

                textFreeShipping = "<li>" & SitePage.getTextFreeShip(so.SubTotal) & "</li>"
            End If
        End If
    End Sub
    'Private Sub getTextFreeShip()
    '    Try
    '        Dim orderPriceMin As Double = 0
    '        'If m_Cart.Order.BillToCountry = "" Then
    '        orderPriceMin = SysParam.GetValue("USOrderPriceMin")
    '        'Else
    '        '    orderPriceMin = SysParam.GetValue("InternationalOrderPriceMin")
    '        'End If
    '        orderPriceMin = SysParam.GetValue("USOrderPriceMin") - m_Cart.Order.SubTotal
    '        If orderPriceMin > 0 Then
    '            textFreeShip = String.Format(Resources.Msg.miniCart_addPriceFreeShip, FormatCurrency(orderPriceMin))
    '        Else
    '            textFreeShip = Resources.Msg.miniCart_FreeShip
    '        End If
    '    Catch

    '    End Try
    'End Sub
    Public itemImage As String = String.Empty
    Private Sub rptCartItems_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptCartItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ci As StoreCartItemRow = e.Item.DataItem

            Dim ltr As Literal = CType(e.Item.FindControl("litCartItem"), Literal)
            If ltr IsNot Nothing Then
                ltr.Text = ci.CartItemId.ToString()
            End If

            ltr = CType(e.Item.FindControl("ltrName"), Literal)

            Dim linkItemDetail As String = URLParameters.ProductUrl(StoreItemRow.GetRowURLCodeById(ci.ItemId), ci.ItemId)
            If (ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                ltr.Text = "" & ci.ItemName & ""
            Else
                If ci.AddType = 2 Then
                    Dim si As StoreItemRow = StoreItemRow.GetRow(DB, ci.ItemId)
                    ltr.Text = String.Format("<a href='{0}'>{1}{2}</a>", linkItemDetail, ci.ItemName, IIf(ci.AddType = 2, " - " & si.CaseQty & " pieces/case", ""))
                Else
                    ltr.Text = String.Format("<a href='{0}'>{1}</a>", linkItemDetail, ci.ItemName)
                End If
            End If

            Dim ltrRemove As Literal = CType(e.Item.FindControl("ltrRemove"), Literal)
            If (ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                ltrRemove.Text = String.Format("<span class=""c-remove"" href=""#"" onclick=""mainScreen.ExecuteCommand('RemoveCartItemBuyPoint', 'methodHandlers.ShowPopupCart', ['{0}', 0]);"">Remove</span>", m_Cart.Order.OrderId)
            Else
                ltrRemove.Text = String.Format("<span class=""c-remove"" href=""#"" onclick=""mainScreen.ExecuteCommand('RemoveCartItem', 'methodHandlers.ShowPopupCart', ['{0}', '{1}']);"">Remove</span>", ci.CartItemId, ci.ItemId)
            End If

            ltr = CType(e.Item.FindControl("ltrFree"), Literal)
            If ci.IsFreeItem = True Then
                If ci.IsFreeItem Then
                    If ci.Total > 0 Then
                        ltr.Text = "<div class='free'>Discounted Item</div>"
                        ltrRemove.Visible = True
                    Else
                        ltr.Text = "<div class='free'>FREE Item</div>"
                        ltrRemove.Visible = False
                    End If

                Else
                    Dim freeText As String = "FREE Gift"
                    If ci.PromotionID > 0 Then
                        Dim objPromotion As StorePromotionRow = StorePromotionRow.GetRow(DB, ci.PromotionID)
                        If Not objPromotion Is Nothing Then
                            If Not objPromotion.IsProductCoupon Then
                                freeText = "FREE Item"
                                ltrRemove.Visible = False
                            End If
                        End If
                    End If

                    ltr.Text = "<div class='free'>" & freeText & "</div>"
                End If
                ltr.Visible = True
            Else
                ltr.Visible = False
            End If

            ltr = CType(e.Item.FindControl("ltrSKU"), Literal)
            ltr.Visible = True
            ltr.Text = ci.SKU
            ltr = CType(e.Item.FindControl("ltrQty"), Literal)
            ltr.Text = ci.Quantity
            ltr = CType(e.Item.FindControl("lnkImg"), Literal)

            Dim imgSrc As String = Utility.ConfigData.CDNmediapath & "/assets/items/cart/" & IIf(String.IsNullOrEmpty(ci.Image), "na.jpg", ci.Image)
            If (ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                ltr.Text &= "<img src=""" & imgSrc & """ class=""img"" alt=""" & ci.ItemName & """ width=""58px"" height=""58px"" />"
            Else
                ltr.Text &= "<a href='" & linkItemDetail & "'><img src=""" & imgSrc & """ class=""img"" alt=""" & ci.ItemName & """ width=""58px"" height=""58px"" /></a>"
            End If

            ltr = CType(e.Item.FindControl("ltrPrice"), Literal)
            If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                ltr.Text = Utility.Common.FormatPointPrice(ci.SubTotalPoint)
            Else
                If (ci.SubTotal <> ci.Total) Then
                    ltr.Text = "<span class='price1'>" & FormatCurrency(ci.SubTotal) & "</span> <span class='price2'>" & FormatCurrency(ci.Total) & "</span>"
                Else
                    ltr.Text = "<span class='price3'>" & FormatCurrency(ci.SubTotal) & "</span>"
                End If
            End If
        End If
    End Sub










End Class

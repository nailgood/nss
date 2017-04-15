Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Imports Utility.Common
Imports System.Web.Services

Partial Class store_cart
    Inherits SitePage
    Dim OrderId As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        Try
            OrderId = GetQueryString("oid") 'request orderid from email 
            If IsNumeric(OrderId) And OrderId > 0 Then
                Cart = New ShoppingCart(DB, OrderId, False)
                If Cart.Order Is Nothing Then
                    Session.Remove("OrderId")
                    Response.Redirect("/")
                Else
                    Session("OrderId") = OrderId
                End If
            End If
        Catch ex As Exception
        End Try

        If Not IsPostBack Then

            If Session("ShowCartMessage") IsNot Nothing Then
                AddError(Session("ShowCartMessage"))
            End If

            LoadMetaData(DB, "/store/cart.aspx")
            If Cart Is Nothing Then
                OrderId = Utility.Common.GetOrderIdFromCartCookie()
                If OrderId > 0 Then
                    Session("OrderId") = OrderId
                ElseIf Session("OrderId") IsNot Nothing AndAlso CInt(Session("OrderId")) > 0 Then
                    OrderId = CInt(Session("OrderId"))
                End If

                If OrderId > 0 Then
                    Utility.Common.SetOrderToCartCookie(OrderId)
                    Cart = New ShoppingCart(DB, OrderId, False)
                Else
                    Exit Sub
                End If
            End If

            Dim couponCode As String = String.Empty
            If Not Session("LandingPageCouponCode") Is Nothing Then
                couponCode = Session("LandingPageCouponCode")
                Session.Remove("LandingPageCouponCode")
            End If

            If HasOrder Then
                Utility.Common.DeleteCachePopupCart(OrderId)
                Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & OrderId)

                Cart.RecalculateOrderDetail("Cart.aspx.Page_Load")
                uCart.Cart = Cart
                divSendCart.Visible = Cart.GetCartItemCount() > 0
                Dim ucEstimateShipping As controls_checkout_estimate_shipping = Me.Master.FindControl("ucEstimateShippingSummary")
                If Not ucEstimateShipping Is Nothing Then
                    ucEstimateShipping.Cart = Cart
                End If

                Dim ucSummary As controls_checkout_cart_summary = Me.Master.FindControl("ucCartSummary")
                If Not ucSummary Is Nothing Then
                    ucSummary.Cart = Cart
                End If

                ucFreeGift.Cart = Cart
                Dim message As String = String.Empty
                If Cart.CheckTotalFreeSample(Cart.Order.OrderId) Then
                    message = String.Format(Resources.Alert.FreeSamplesMin, CDbl(SysParam.GetValue("FreeSampleOrderMin")))
                End If

                Dim MinFreeGiftLevelInvalid As Double = ShoppingCart.GetMinFreeGiftLevelInValid(Cart.Order.OrderId)
                If MinFreeGiftLevelInvalid > 0 Then
                    If Not String.IsNullOrEmpty(message) Then
                        message = message & "<br/><br/>"
                    End If
                    If MinFreeGiftLevelInvalid > 150 Then
                        message = message & String.Format(Resources.Alert.FreeGiftMin2, MinFreeGiftLevelInvalid)
                    Else
                        message = message & String.Format(Resources.Alert.FreeGiftMin, MinFreeGiftLevelInvalid)
                    End If
                End If

                If Not String.IsNullOrEmpty(message) Then
                    ltrScript.Text = "<script>LoadError('" & message & "')</script>"
                End If
            Else
                Response.Redirect("/")
            End If

        End If
    End Sub


    <WebMethod()> _
    Public Shared Function ValidateCheckOut() As String()
        Dim ucCart As New controls_checkout_cart
        Dim result As String() = ucCart.ValidateCheckOut()
        Return result
    End Function
End Class

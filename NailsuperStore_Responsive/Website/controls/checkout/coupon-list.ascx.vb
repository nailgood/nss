
Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_coupon_list
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
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
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Cart Is Nothing Then
            Cart = Session("cartRender")
        End If
        If Cart Is Nothing Then
            Dim orderId As Integer = Session("OrderId")
            If orderId < 1 Then
                orderId = Utility.Common.GetOrderIdFromCartCookie()
            End If
            If orderId > 0 Then
                Cart = New ShoppingCart(DB, orderId, False)
            End If
        End If
        If Not Cart Is Nothing AndAlso Not Cart.Order Is Nothing AndAlso Cart.GetCartItemCount() > 0 Then
            LoadAllCoupon()
        End If
    End Sub
    Private Sub LoadAllCoupon()
        Dim StrCoupon As String = ""
        Dim ArrCoupon As String()
        Dim GProductCoupon As String = ""
        Dim GOrderCoupon As String = ""
        StorePromotionRow.GetAllCoupon(Cart.Order.OrderId, GProductCoupon, GOrderCoupon)
     
        Dim i As Integer
        If String.IsNullOrEmpty(GProductCoupon) And String.IsNullOrEmpty(GOrderCoupon) Then
            Exit Sub
        End If
        Dim ProCode As String = Nothing
        Dim ProMessage As String = Nothing
        Dim IsProductCoupon As Boolean = Nothing
        If GProductCoupon <> "" Then
            StrCoupon = GProductCoupon
        End If
        If GOrderCoupon <> "" Then
            If StrCoupon = "" Then
                StrCoupon = GOrderCoupon
            Else
                StrCoupon &= "," & GOrderCoupon
            End If
        End If
        ArrCoupon = StrCoupon.Split(",")
        For i = 0 To ArrCoupon.Length - 1
            ProCode = ArrCoupon(i)
            Dim isError As Boolean = False
            StorePromotionRow.GetMessageByCode(DB, ProCode, ProMessage, IsProductCoupon)
            If Not ProCode Is Nothing Then
                StrCoupon = ""
                
                If IsProductCoupon = True Then
                    StrCoupon = Cart.GetProductCouponMessage(ProCode) & ""
                Else
                    StrCoupon = Cart.Order.PromotionMessage
                End If
                If Not ProMessage.Equals(StrCoupon) Then '' coupon invalid
                    isError = True
                    If StrCoupon.Contains("This promotion has a minimum") Or StrCoupon.Contains("This promotion has a maximum") Or StrCoupon.Contains("The promotion code you entered") Or StrCoupon.Contains("The promotion code you entered is only valid from") Then
                        If IsProductCoupon Then
                            If StrCoupon.Contains("This promotion has a minimum purchase amount of") Or StrCoupon.Contains("This promotion has a maximum purchase amount of") Then
                                Dim subtotal As Double = 0
                                If Not String.IsNullOrEmpty(ProCode) Then
                                    subtotal = DB.ExecuteScalar("Select Total from StoreCartItem where OrderId=" & Cart.Order.OrderId & " and PromotionID=(Select PromotionId from StorePromotion where PromotionCode='" & ProCode & "')")
                                End If

                                Dim subtotalMsg As String = String.Format(Resources.Alert.ItemSubTotal, subtotal)
                                StrCoupon = StrCoupon & "." & subtotalMsg
                            End If

                        Else
                            If StrCoupon.Contains("This promotion has a minimum") Or StrCoupon.Contains("This promotion has a maximum") Then
                                Dim subtotalMsg As String = String.Format(Resources.Alert.OrderSubTotal, Cart.Order.SubTotal)
                                StrCoupon = StrCoupon & "." & subtotalMsg
                            End If
                        End If

                        ''  Lit.Text = "<span class='CouponRed'>" & StrCoupon & "</span>"
                        '' Else

                    End If
                    If String.IsNullOrEmpty(StrCoupon) Then
                        StrCoupon = "Although you've entered a valid promo code " & ProCode & ", your order does not currently meet the code's usage criteria."

                    End If


                End If
                ltrData.Text &= CreateHTMLCouponRow(ProCode, StrCoupon, isError)
            End If
        Next

    End Sub
    Private Function CreateHTMLCouponRow(ByVal code As String, ByVal msg As String, ByVal isMsgError As Boolean) As String
        Dim result As String = "<ul>"
        result &= "<li class='code'><span>" & code & "</span><span class='remove'><a onclick=""DeleteCoupon('" & code & "')"" href='javascript:void(0);'>Remove</a></span></li>"
        If isMsgError Then
            result &= "<li class='msgError'>" & msg & "</li></ul>"
            hidCouponIsNotValid.Value = hidCouponIsNotValid.Value & "," & code
        Else
            result &= "<li class='msg'>" & msg & "</li></ul>"
        End If
        Return result
    End Function
End Class

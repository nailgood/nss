

Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_cart_free_gift
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
    Protected carttotal As Double = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadFreeGift()
    End Sub
    Private Sub LoadFreeGift()

      
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
            carttotal = Cart.Order.SubTotal
            Dim lstFreeGiftLevel As FreeGiftLevelCollection = FreeGiftLevelRow.GetListActive()
            If Not lstFreeGiftLevel Is Nothing AndAlso lstFreeGiftLevel.Count > 0 Then
                rptFreeGift.DataSource = lstFreeGiftLevel
                rptFreeGift.DataBind()
            Else
                Me.Visible = False
            End If
        Else
            Me.Visible = False
        End If



    End Sub
    Protected Sub rptFreeGift_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptFreeGift.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim objLevel As FreeGiftLevelRow = e.Item.DataItem
            Dim ltrData As Literal = CType(e.Item.FindControl("ltrData"), Literal)
            Dim msg As String = String.Empty
            Dim disableCSS As String = "message"
            If (Cart.Order.SubTotal < objLevel.MinValue) Then
                msg = "Purchase " & FormatCurrency(CInt(objLevel.MinValue) - Cart.Order.SubTotal) & " more to receive"
            Else
                msg = "Choose Your Free Gift At Check Out"
                disableCSS = disableCSS & " disable"
            End If

            Dim strHTML As String = "<li class='free-gift-banner'>"
            strHTML &= "                <div class='inner-content'>"
            strHTML &= "                   <div class='level-content-contianner1'>"
            strHTML &= "                      <div class='level-content-contianner2'>"
            strHTML &= "                         <ul class='level-content'><li>Free Gift </li><li>for orders over </li><li>$" & CInt(objLevel.MinValue) & " </li></ul>"
            strHTML &= "                      </div>"
            strHTML &= "                    </div>"
            strHTML &= "                    <div class='level-banner'>"
            If Not String.IsNullOrEmpty(objLevel.Banner) Then
                strHTML &= "<img src='" & Utility.ConfigData.PathFreeGiftLevelBanner & "/thumb/" & objLevel.Banner & "'/>"
            End If
            strHTML &= "                    </div>"
            strHTML &= "                 </div>"
            strHTML &= "                 <ul class='" & disableCSS & "'><li class='icon'><span>&nbsp;</span></li>"
            strHTML &= "                    <li>" & msg & "</li>"
            strHTML &= "                 </ul>"
            strHTML &= "              </li>"
            ltrData.Text = strHTML
        End If
    End Sub
End Class

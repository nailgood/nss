


Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System
Imports Utility.Common
Imports System.Web.Services
Partial Class store_revise_cart
    Inherits SitePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
        If memberId <= 0 And HasAccess() = False Then
            Response.Redirect("/members/login.aspx")
        End If

        If Cart Is Nothing Then
            Response.Redirect("/store/cart.aspx")
        End If

        Dim o As StoreOrderRow = Cart.Order
        If o Is Nothing Then
            Response.Redirect("/store/cart.aspx")
        End If

        If Not Page.IsPostBack Then
            If Cart.GetCartItemCount() = 0 Then
                DB.Close()
                Response.Redirect("/store/cart.aspx")
            End If

            uCart.Cart = Cart
            Dim ucCartSummary As controls_checkout_cart_summary = Me.Master.FindControl("ucCartSummary")
            If Not ucCartSummary Is Nothing Then
                ucCartSummary.Cart = Cart
            End If
            LoadMetaData(DB, "/store/revise-cart.aspx")
            'ucFreeGift.Cart = Cart
            If Cart.CheckTotalFreeSample(Cart.Order.OrderId) Then
                Dim message As String = String.Format(Resources.Alert.FreeSamplesMin, CDbl(SysParam.GetValue("FreeSampleOrderMin")))
                AddError(message)
                Exit Sub
            End If

            Utility.Common.OrderLog(o.OrderId, "Page Load", Nothing)
        End If

    End Sub
    <WebMethod()> _
    Public Shared Function ValidateCheckOut() As String()
        Dim ucCart As New controls_checkout_cart
        Dim result As String() = ucCart.ValidateCheckOut()
        Return result
    End Function
End Class

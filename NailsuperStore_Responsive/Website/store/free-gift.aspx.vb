Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System.IO
Partial Class store_free_gift
    Inherits SitePage

    Private MinTotal As Double = 0
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            If Request.RawUrl.Contains("?act=checkout") Then
                Dim memberId = Utility.Common.GetCurrentMemberId()
                If memberId <= 0 And HasAccess() = False Then
                    Response.Redirect("/members/login.aspx?act=checkout")
                End If
                If Cart Is Nothing Then
                    Response.Redirect("/store/cart.aspx")
                End If
                Dim o As StoreOrderRow = Cart.Order
                If o Is Nothing Then
                    Response.Redirect("/store/cart.aspx")
                End If
                hTitle.Visible = False
            End If
            LoadList()
            ucListItem.Cart = Cart
            Dim pageDB As ContentToolPageRow = ContentToolPageRow.GetRowByURL("/store/free-gift.aspx")
            ucListItem.Description = String.Format(pageDB.Content, MinTotal)
            LoadMetaData(DB, pageDB)
        End If

    End Sub
    Private Sub LoadList()
        Dim lstFreeGiftLevel As FreeGiftLevelCollection = FreeGiftLevelRow.GetListActive()
        If (Not lstFreeGiftLevel Is Nothing AndAlso lstFreeGiftLevel.Count > 0) Then
            MinTotal = lstFreeGiftLevel(0).MinValue
            Session("FreeGiftLevelGender") = lstFreeGiftLevel(0).Id
            Dim orderId As Integer = 0
            If Not Cart Is Nothing AndAlso Not Cart.Order Is Nothing Then
                orderId = Cart.Order.OrderId
            End If

            Dim lstItem As StoreItemCollection = StoreItemRow.GetFreeGiftColectionByLevel(orderId, lstFreeGiftLevel(0).Id)
            ucListItem.ListItem = lstItem
        End If
    End Sub

End Class

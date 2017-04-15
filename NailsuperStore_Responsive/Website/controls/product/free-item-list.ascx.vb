
Imports Components
Imports DataLayer
Imports Utility

Partial Class controls_product_free_item_list
    Inherits BaseControl
    Private m_ListItem As StoreItemCollection = Nothing
    Public Property ListItem() As StoreItemCollection
        Set(ByVal value As StoreItemCollection)
            m_ListItem = value
        End Set
        Get
            Return m_ListItem
        End Get
    End Property
    Private m_isCustomerInternational As Boolean = False
    Private m_Cart As ShoppingCart
    Public Property Cart() As ShoppingCart
        Set(ByVal value As ShoppingCart)
            m_Cart = value
        End Set
        Get
            Return m_Cart
        End Get
    End Property
    Private m_isAllowAddCart As Boolean = False
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Request.FilePath.Contains("/free-sample.aspx") Then
            If ListItem Is Nothing AndAlso Not Session("ListFreeGiftRender") Is Nothing Then
                ListItem = Session("ListFreeGiftRender")
            End If

            If Not Session("cartRender") Is Nothing AndAlso Cart Is Nothing Then
                Cart = Session("cartRender")
            End If

            If Not Cart Is Nothing Then
                Dim freeGiftLevel As Integer = 0
                If Not Session("FreeGiftLevelGender") Is Nothing Then
                    freeGiftLevel = Session("FreeGiftLevelGender")
                End If
                m_isAllowAddCart = FreeGiftLevelRow.CheckAllowAddCart(Cart.Order.OrderId, freeGiftLevel)
            End If
        End If

        If Not Cart Is Nothing Then
            m_isCustomerInternational = MemberRow.CheckMemberIsInternational(Cart.Order.MemberId, Cart.Order.OrderId)
        End If

        rptData.DataSource = ListItem
        rptData.DataBind()
    End Sub


    'Private Function isCheckOut() As Boolean
    '    Dim act As String = String.Empty
    '    If Not Request.QueryString("act") Is Nothing Then
    '        act = Request.QueryString("act")
    '        If act = "checkout" Then
    '            Return True
    '        End If
    '    End If
    '    If Not Session("checkOut") Is Nothing Then
    '        Return CBool(Session("checkOut"))
    '    End If
    '    Return False
    'End Function
    Protected Sub rptData_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptData.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim objItem As StoreItemRow = CType(e.Item.DataItem, StoreItemRow)
            hidListId.Value &= objItem.ItemId & ","
            Dim ucFreeItem As controls_product_free_sample_product = DirectCast(e.Item.FindControl("ucFreeItem"), controls_product_free_sample_product)
            ucFreeItem.Item = objItem
            ucFreeItem.IsCustomerInternational = m_isCustomerInternational
            ucFreeItem.AllowAddCart = m_isAllowAddCart
        End If
    End Sub
    
End Class

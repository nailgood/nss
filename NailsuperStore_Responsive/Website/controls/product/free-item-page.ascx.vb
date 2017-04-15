
Imports Components
Imports DataLayer
Imports Utility

Partial Class controls_product_free_item_page
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

    Private m_Description As String = String.Empty
    Public Property Description() As String
        Set(ByVal value As String)
            m_Description = value
        End Set
        Get
            Return m_Description
        End Get
    End Property

    Private m_Cart As ShoppingCart
    Public Property Cart() As ShoppingCart
        Set(ByVal value As ShoppingCart)
            m_Cart = value
        End Set
        Get
            Return m_Cart
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.FilePath.Contains("/free-sample.aspx") Then
            Dim orderId As Integer = Common.GetCurrentOrderId()
            If orderId > 0 Then
                btnAddCartTop.Text = "Add to Cart"
                btnAddCartBottom.Text = "Add to Cart"
                hidContinueLink.Value = "/store/cart.aspx"
                btnAddCartBottom.Attributes.Add("onclick", "return AddToCart();")
                btnAddCartTop.Attributes.Add("onclick", "return AddToCart();")

                divCartTop.Visible = True
                divCartBottom.Visible = True
            End If
            ucFreeGiftLevel.Visible = False
        Else
            If ListItem Is Nothing AndAlso Not Session("ListFreeGiftRender") Is Nothing Then
                ListItem = Session("ListFreeGiftRender")
            End If
            If Not Session("cartRender") Is Nothing AndAlso Cart Is Nothing Then
                Cart = Session("cartRender")
            End If
            hidContinueLink.Value = GetCheckOutPage()
            If Not Cart Is Nothing Then
                Dim freeGiftLevel As Integer = 0
                If Not Session("FreeGiftLevelGender") Is Nothing Then
                    freeGiftLevel = Session("FreeGiftLevelGender")
                End If

            End If

            'If Not isCheckOut() Then
            '    divCartTop.Visible = False
            '    divCartBottom.Visible = False
            '    ' topbarContainer.Attributes.Add("style", "margin-right:0px !important")
            'Else
            'btnAddCartBottom.Attributes.Add("onclick", "return ContinueCheckOut();")
            'btnAddCartTop.Attributes.Add("onclick", "return ContinueCheckOut();")
            'End If
        End If

        Description = RewriteUrl.StripTags(Description).Trim()
        freeItemList.ListItem = ListItem
        freeItemList.Cart = Cart

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

    Private Function GetCheckOutPage() As String
        Dim result As String = "/store/payment.aspx"
        Dim memberId As Integer = Utility.Common.GetCurrentMemberId()
        If memberId <= 0 Then
            Return result
        End If
        Dim isNotCompleteAddress As Boolean = MemberAddressRow.IsNotCompleteAddress(DB, memberId)
        If isNotCompleteAddress Then
            Dim country As String = DB.ExecuteScalar("Select COALESCE(Country,'') from MemberAddress where MemberId=" & memberId.ToString() & "  and AddressType='Billing'")
            If country = "" Or country = "US" Then
                result = "/store/billing.aspx?type=Billing"
            Else
                result = "/store/billingint.aspx"
            End If
        End If
        Return result
    End Function
End Class

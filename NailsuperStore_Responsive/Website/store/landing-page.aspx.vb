Imports Components
Imports DataLayer
Imports System.IO


Partial Class store_landing_page
    Inherits SitePage

    Private AddCartLogin As Boolean = False
    Private AddCartRelated As Boolean = False
    Protected isShowRelatedItem As Boolean = False
    Private ItemGroupId As Integer = 0

    Public Property ItemId() As Integer
        Get
            Dim o As Object = ViewState("ItemId")
            If o IsNot Nothing Then
                Return DirectCast(o, Integer)
            End If
            Return 0
        End Get

        Set(ByVal value As Integer)
            ViewState("ItemId") = value
        End Set
    End Property

    Public Property SKU() As String
        Get
            Dim o As Object = ViewState("SKU")
            If o IsNot Nothing Then
                Return DirectCast(o, String)
            End If
            Return String.Empty
        End Get

        Set(ByVal value As String)
            ViewState("SKU") = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim code As String = GetQueryString("LandingCode")
        If String.IsNullOrEmpty(code) Then
            Response.Redirect("/home.aspx")
        End If

        If Not IsPostBack Then
            If Session("AddCartRelated") IsNot Nothing Then
                If Session("AddCartRelated").ToString() = "1" Then
                    AddCartRelated = True
                    ShowPopupCart(False, True)
                End If
                Session.Remove("AddCartRelated")
            End If
            If Session("Username") IsNot Nothing AndAlso Session("AutoWishlist") IsNot Nothing Then
                AutoWishlist()
            End If
            Dim LandingPage As New LandingPageRow()
            Dim CustPriceGroupId As Integer = 0
            If LoggedInMemberId > 0 Then
                CustPriceGroupId = Utility.Common.GetCurrentCustomerPriceGroupId()
            End If
            ''kiem tra thoi han landing page va co ton tai khong
            LandingPage = LandingPageRow.CheckLandingPageByUrlCode(code, CustPriceGroupId)
            ItemId = LandingPage.ItemId
            Dim item As New StoreItemRow
            If ItemId > 0 Then
                item = StoreItemRow.GetRow(DB, ItemId)
            End If

            If (LandingPage Is Nothing Or LandingPage.Id = 0) Then
                ''get landing page theo url de lau url return hay itemId cho redirect
                Dim obj As LandingPageRow = LandingPageRow.GetRowByUrlcode(DB, code)
                If Not String.IsNullOrEmpty(obj.UrlReturn) Then
                    Redirect301(obj.UrlReturn)
                End If
                If obj.ItemId > 0 Then
                    Redirect301(URLParameters.ProductUrlByID(obj.ItemId))
                Else
                    Response.Redirect("/home.aspx")
                End If
                Exit Sub
            End If

            If ItemId > 0 Then
                If Not item Is Nothing Then
                    SKU = item.SKU
                    hdVideoId.Value = DB.ExecuteScalar("select Top(1) ItemVideoId from StoreItemVideo where IsActive = 1 and ItemId = " & ItemId & " Order by Arrange asc")
                    ItemGroupId = item.ItemGroupId
                Else
                    Response.Redirect("/home.aspx")
                End If
            End If
            LoadHtml(LandingPage.FileLocation)

            Dim ltrCanonical As Literal = CType(Page.FindControl("LinkCanonical"), Literal)
            If Not ltrCanonical Is Nothing Then
                ltrCanonical.Text = "<link rel=""canonical"" href=""" & GlobalSecureName & Me.Request.RawUrl & """ />"
            End If

            '''''Add Google Analytics Content Experience'''''
            Dim ltrGoogleAnalyticExp As Literal = CType(Page.FindControl("GoogleAnalyticExp"), Literal)
            If Not ltrGoogleAnalyticExp Is Nothing Then
                ltrGoogleAnalyticExp.Text = LandingPage.GoogleABCode
            End If

            'PageTitle = LandingPage.PageTitle
            'MetaDescription = LandingPage.MetaDescription
            'MetaKeywords = LandingPage.MetaKeywords
        End If

        ''load related item
        If ItemId > 0 AndAlso litHTML.Text.Contains("<span class=""related"">Related</span>") Then
            Dim total As Integer = DB.ExecuteScalar("SELECT COUNT(*) FROM RelatedItem ri INNER JOIN StoreItem si ON (ri.ItemId = si.ItemID and si.IsFreeGift<2 and si.IsFreeSample=0) WHERE si.IsActive = 1 AND ri.ParentID = " & ItemId)
            If total > 0 Then
                isShowRelatedItem = True
                'RelateItemsNew1.ItemGroup = ItemGroupId
                'RelateItemsNew1.ItemId = ItemId
            End If
        End If
    End Sub

    Private Sub LoadHtml(ByVal path As String)
        Dim objReader As FileStream

        Try
            objReader = New FileStream(Server.MapPath(path), FileMode.Open)
            Dim sr As New StreamReader(objReader, Encoding.Default)
            Dim strTemplate As String = sr.ReadToEnd
            litHTML.Text = ReplaceSrc(strTemplate, path)
            sr.Dispose()
            sr.Close()
        Catch ex As Exception
            Response.Write(ex.ToString())
        Finally
            If Not objReader Is Nothing Then
                objReader.Close()
                objReader.Dispose()
            End If

        End Try
    End Sub

    Private Function ReplaceSrc(ByVal src As String, ByVal path As String) As String
        Dim result As String = src
        Dim prefixUrl As String = String.Empty

        Dim i As Integer = path.LastIndexOf("/")
        If i > 0 Then
            prefixUrl = path.Substring(0, i + 1)
            result = src.Replace("src='", "src='" & prefixUrl)
            result = result.Replace("src=""", "src=""" & prefixUrl)

            result = result.Replace("url(""", "url(""" & prefixUrl)
            result = result.Replace("url('", "url('" & prefixUrl)
            result = ReplaceLinkCSS(result, prefixUrl)
        End If

        Return result
    End Function

    Protected Sub lnkAddCart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkAddCart.Click
        ShoppingCart.GetShoppingCartFromCookie(DB, False, Cart)
        If IsNumeric(hdItemId.Value) Then
            If hdItemId.Value > 0 Then
                ItemId = hdItemId.Value
            End If
        End If
        If Not String.IsNullOrEmpty(hdShowPopupCart.Value) Then
            If hdShowPopupCart.Value = 1 Then
                Session("isShowPopup") = 1
            Else
                Session("isShowPopup") = 0
            End If
        End If
        If Not String.IsNullOrEmpty(hdItemSKU.Value) Then
            Dim si As StoreItemRow = StoreItemRow.GetRow(DB, hdItemSKU.Value.ToString())
            If si IsNot Nothing AndAlso si.ItemId > 0 Then
                ItemId = si.ItemId
            End If
        End If

        If String.IsNullOrEmpty(hdMultiItem.Value) Then
            If Not String.IsNullOrEmpty(hdCouponCode.Value) Then
                Session("LandingPageCouponCode") = hdCouponCode.Value
            End If
            AddItemToCart()
        ElseIf (Not String.IsNullOrEmpty(hdMultiItem.Value) AndAlso Not hdMultiItem.Value.Equals(",")) Then
            AddMultiItemToCart()
        End If

    End Sub

    Private bErrorMutiCart As Boolean = False
    Private bMultiAddCart As Boolean = False
    Private Sub AddMultiItemToCart()
        Dim lstItem() As String
        Dim bMultiQtyitem As Boolean = False
        If (hdMultiItem.Value.IndexOf(":") > 0 AndAlso hdMultiItem.Value.IndexOf("|")) Then
            Session("txtQty") = Nothing
            lstItem = hdMultiItem.Value.Split("|")
            bMultiQtyitem = True
        Else
            lstItem = hdMultiItem.Value.Split(",")
            bMultiQtyitem = False
        End If


        If lstItem.Length > 0 Then
            bMultiAddCart = True
            For i As Integer = 0 To lstItem.Length - 1
                If Not String.IsNullOrEmpty(lstItem(i)) Then
                    If bMultiQtyitem Then
                        Dim tmp() As String = lstItem(i).Split(":")
                        If tmp.Length >= 2 Then
                            SKU = tmp(0)
                            hdQuantity.Value = tmp(1)
                        End If
                    Else
                        SKU = lstItem(i)
                    End If
                    AddItemToCart()
                End If
            Next
            hdMultiItem.Value = String.Empty
            hdQuantity.Value = 0
            ShowPopupCart(True, False)
        End If
    End Sub

    Private Sub AddItemToCart()
        Dim isShowPopup As Boolean = False
        If Session("isShowPopup") Is Nothing = False Then
            If Session("isShowPopup").ToString = "1" Then
                isShowPopup = True
            End If
        End If
        If Session("txtQty") Is Nothing = False Then
            If Session("txtQty").ToString <> "" Then
                hdQuantity.Value = Session("txtQty").ToString
            End If
        End If
        Dim bAddCart As Boolean = False
        Dim sUrl As String = Me.Request.RawUrl
        Try
            Dim si As New StoreItemRow
            If ItemId > 0 Then
                si = StoreItemRow.GetRow(DB, ItemId)
            ElseIf Not String.IsNullOrEmpty(SKU) Then
                si = StoreItemRow.GetRow(DB, SKU)
            Else
                Exit Sub
            End If
            Dim MemberBillingAddress As MemberAddressRow = MemberAddressRow.GetAddressByType(DB, LoggedInMemberId, Utility.Common.MemberAddressType.Billing.ToString())
            Dim Qty As Integer = hdQuantity.Value

            Dim departmentID As Integer = Utility.ConfigData.RootDepartmentID
            Dim bError As Boolean = False
            Dim ItemError As String = "This item "
            If bMultiAddCart Then
                ItemError = "<B>Item #" & si.SKU & "</B> "
            End If
            If si.IsFlammable And MemberBillingAddress.Country <> "US" Then
                ShowMessageError(ItemError & "is flammable and hazardous materials, it can not be shipped by air.")
                bError = True
                bErrorMutiCart = True
            End If
            DB.BeginTransaction()
            If Not bError Then
                If (Utility.Common.IsItemAcceptingOrder(si.AcceptingOrder) Or si.IsSpecialOrder) Then
                    Cart.Add2Cart(si.ItemId, Nothing, Qty, departmentID, "Myself")
                    bAddCart = True
                Else
                    Dim TotalInCart As Integer = DB.ExecuteScalar("select coalesce(sum(Quantity),0) from StoreCartItem where OrderId = " & DB.Number(Session("OrderId")) & " and ItemId = " & si.ItemId)
                    Dim MaximumQty As Integer = si.MaximumQuantity
                    If MaximumQty > 0 AndAlso MaximumQty < Qty Then
                        ShowMessageError("Item " & si.ItemName & " has a maximum purchase quantity of " & MaximumQty & " per order.<br />")
                        bError = True
                        bErrorMutiCart = True
                    ElseIf TotalInCart + Qty > si.QtyOnHand Then
                        If si.QtyOnHand > 0 Then
                            ShowMessageError("<B>Item #" & si.SKU & "</B>, We only have " & si.QtyOnHand & " units in stock for this item" & IIf(TotalInCart > 0, ", and there are already " & TotalInCart & " in your <a href=""/store/cart.aspx"">shopping cart</a>", "") & ".<br />")
                        Else
                            ShowMessageError(ItemError & "is out of stock.")
                        End If
                        bError = True
                        bErrorMutiCart = True
                    ElseIf Qty < 1 Then
                        ShowMessageError("Please select at least one item to add to your shopping cart.")
                        bError = True
                        bErrorMutiCart = True
                    Else
                        Cart.Add2Cart(si.ItemId, Nothing, Qty, departmentID, "Myself")
                        bAddCart = True
                    End If
                End If
            End If
            If Not bError Then
                If bAddCart Then
                    If Not DB.Transaction Is Nothing Then DB.CommitTransaction()
                    Cart.RecalculateOrderDetail("landing-page.AddItemToCart")
                    'RefreshPopupCart(Cart)
                Else
                    If Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                    ShowMessageError("An error occurred while adding the items to your cart.")
                End If
            End If

        Catch ex As Exception
            If Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            ShowMessageError("An error occurred while adding the items to your cart.")
        End Try
        Session.Remove("txtQty")
        Session.Remove("AddtoCartLogin")
        Session.Remove("isShowPopup")
        Session.Remove("AddMultiToCartLogin")
        If bAddCart Then
            If Not bMultiAddCart Then
                DB.Close()
                ShowPopupCart(False, isShowPopup)
            Else
                SKU = String.Empty
                ItemId = 0
            End If
        End If
    End Sub

    Private Sub ShowPopupCart(ByVal isMultiItem As Boolean, ByVal isShowPopup As Boolean)
        If isMultiItem Then
            If bErrorMutiCart Then
                If AddCartLogin Then
                    ltrScript.Text = "<script>javascript:ResetCart(" & Cart.GetCartItemCount() & ",'" & FormatCurrency(Cart.Order.SubTotal()) & "');</script>"
                Else
                    ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "resetcart", "ResetCart(" & Cart.GetCartItemCount() & ",'" & FormatCurrency(Cart.Order.SubTotal()) & "');", True)
                End If
                Exit Sub
            End If
        End If
        If isShowPopup Then
            If AddCartLogin Or AddCartRelated Then
                ltrScript.Text = "<script>javascript:ResetCart(" & Cart.GetCartItemCount() & ",'" & FormatCurrency(Cart.Order.SubTotal()) & "');javascript:ShowPopupCart();scrollTop();checkShowRelatedItem();</script>"
            Else
                ScriptManager.RegisterClientScriptBlock(Me, Me.GetType(), "resetcart", "ResetCart(" & Cart.GetCartItemCount() & ",'" & FormatCurrency(Cart.Order.SubTotal()) & "');", True)
                ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShowPopupCart", "ShowPopupCart();scrollTop();checkShowRelatedItem();", True)
            End If
        Else
            Response.Redirect("/store/cart.aspx")
        End If

    End Sub


    Private Sub ShowMessageError(ByVal msg As String)
        If AddCartLogin Then
            ltrScript.Text = "<script>javascript:ShowAddCardError('" & msg & "');</script>"
        Else
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "showError", "ShowAddCardError('" & msg & "')", True)
        End If
    End Sub

    Private Function ReplaceLinkCSS(ByVal source As String, ByVal prefixUrl As String) As String
        Dim result As String = String.Empty
        Dim src As String = source
        Dim oTag As Integer = src.IndexOf("<link")
        Dim tmp As String = String.Empty
        Dim tmp2 As String = String.Empty

        While oTag >= 0
            result &= src.Substring(0, oTag)
            tmp = src.Substring(oTag)
            Dim cTag As Integer = tmp.IndexOf("</link>")
            Dim compare As String = String.Empty
            Dim link As String = String.Empty
            If cTag >= 0 Then
                compare = tmp.Substring(0, cTag + 9)
                tmp2 = tmp.Substring(cTag + 9)
                If compare.ToLower().Contains(".css") Then
                    compare = compare.Replace("href=""", "href=""" & prefixUrl)
                End If
            End If
            src = tmp2
            oTag = src.IndexOf("<link")
            If oTag >= 0 Then
                tmp2 = String.Empty
            End If
            result &= compare & tmp2

        End While

        If String.IsNullOrEmpty(result) Then
            result = src
        End If
        Return result
    End Function


    Private Sub Redirect301(ByVal strUrl As String)
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Status = "301 Moved Permanently"
        HttpContext.Current.Response.AddHeader("Location", strUrl)
        HttpContext.Current.Response.End()
    End Sub

    'Long edit 30/11/2009
    Protected Sub lnkAdd2Wishlist_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkAdd2Wishlist.Click

        If hdQuantity.Value <> "" Then
            Session("txtQtyWL") = hdQuantity.Value
        End If
        Dim si As New StoreItemRow
        If ItemId < 1 Then
            If Not String.IsNullOrEmpty(hdItemSKU.Value) Then
                SKU = hdItemSKU.Value
                si = StoreItemRow.GetRow(DB, SKU)
                ItemId = si.ItemId
            End If
        End If
        If Not HasAccess() Then
            Session("AutoWishlist") = ItemId
            Response.Redirect("/members/login.aspx")
        End If
        AutoWishlist()
    End Sub
    Private Sub AutoWishlist()
        If Session("AutoWishlist") IsNot Nothing Then
            If Session("AutoWishlist").ToString <> "" Then
                ItemId = Convert.ToInt32(Session("AutoWishlist"))
            End If
        End If
        If ItemId < 0 Then
            Exit Sub
        End If
        Dim Item As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
        Session("AutoWishlist") = Nothing
        If Session("txtQtyWL") Is Nothing = False Then
            If Session("txtQtyWL").ToString <> "" Then
                hdQuantity.Value = Session("txtQtyWL").ToString
            End If
        Else
            hdQuantity.Value = hdQuantity.Value
        End If
        If Not IsNumeric(hdQuantity.Value) OrElse CInt(hdQuantity.Value) < 1 Then
            Session("txtQtyWL") = Nothing
            ''Session("AutoWishlist") = Nothing
            ShowMessageError("Please provide a valid quantity")
            Exit Sub
        Else
            Session("txtQtyWL") = hdQuantity.Value
        End If
        Dim swatches As String = String.Empty
        Dim AttributeSKU As String = ""
        Dim Attributes As String = String.Empty
        If Item.Pricing Is Nothing Then NSS.FillPricing(DB, Item)
        If Item.Pricing Is Nothing Then Response.Redirect("/500.aspx")
        Try

            If Session("swatches") Is Nothing = True Then
                Session("swatches") = swatches
            Else
                swatches = Session("swatches")
            End If
            If Session("AttributeSKU") Is Nothing = True Then
                Session("AttributeSKU") = AttributeSKU
            Else
                AttributeSKU = Session("AttributeSKU")
            End If
            If Session("Attributes") Is Nothing = True Then
                Session("Attributes") = Attributes
            Else
                Attributes = Session("Attributes")
            End If

            Dim dbWishlistItem As MemberWishlistItemRow = MemberWishlistItemRow.GetRow(DB, Session("MemberId"), ItemId, Attributes, swatches)

            If dbWishlistItem.WishlistItemId > 0 AndAlso Session("MemberId") <> Nothing Then
                dbWishlistItem.Quantity += hdQuantity.Value
                dbWishlistItem.Update()
                Session("txtQtyWL") = Nothing
                '' Session("AutoWishlist") = Nothing
                Session("AttributeSKU") = Nothing
                Session("Attributes") = Nothing
                Session("swatches") = Nothing
                Session("txtQty9999") = Nothing
                Response.Redirect("/members/wishlist/default.aspx", True)
            ElseIf Session("MemberId") <> Nothing Then
                dbWishlistItem = New MemberWishlistItemRow(DB)
                dbWishlistItem.Quantity = hdQuantity.Value
                dbWishlistItem.ItemId = Item.ItemId
                dbWishlistItem.MemberId = Session("MemberId")
                dbWishlistItem.Attributes = Attributes
                dbWishlistItem.AttributeSKU = AttributeSKU
                dbWishlistItem.Swatches = swatches
                dbWishlistItem.Insert()
                Session("txtQtyWL") = Nothing
                ''Session("AutoWishlist") = Nothing
                Session("AttributeSKU") = Nothing
                Session("Attributes") = Nothing
                Session("swatches") = Nothing
                Session("txtQty9999") = Nothing
                Response.Redirect("/members/wishlist/default.aspx", True)
            End If
        Catch ex As Exception
            ShowMessageError("There was a problem adding your item to your wish list.")
        End Try
    End Sub

End Class

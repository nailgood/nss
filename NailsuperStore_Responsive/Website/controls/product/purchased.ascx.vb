Imports Components
Imports DataLayer
Imports Utility

Partial Class controls_product_purchased
    Inherits BaseControl
    Private c As ShoppingCart
    Private o As StoreOrderRow
    Protected EbayOrder As Boolean = False
    Private lstCartItem As StoreCartItemCollection
    Private CountData As Integer = 0
    Protected itemindex As Integer = 0
    Public IsFirstLoad As Boolean
    Public isInternational As Boolean
    Public isProductReview As Boolean = False
    Private webRoot As String = Utility.ConfigData.CDNMediaPath
    Private weblink As String = Utility.ConfigData.GlobalRefererName
    Public isReview As Boolean = False
    Public isShowReviewCart As Boolean = False
    Public Property CartItemList() As StoreCartItemCollection
        Get
            Return lstCartItem
        End Get
        Set(ByVal value As StoreCartItemCollection)
            lstCartItem = value
        End Set
    End Property
    Private Function IsLoadCart() As Boolean
        If lstCartItem Is Nothing Then
            Return True
        End If
        If lstCartItem.Count < 1 Then
            Return True
        End If
        Return False
    End Function
    Public Sub Fill(ByVal OrderId As Integer, ByVal index As Integer, ByVal isFirst As Boolean, ByVal International As Boolean)
        itemindex = index
        IsFirstLoad = isFirst
        isInternational = International
        c = New ShoppingCart(DB, OrderId, True)
        If c Is Nothing Then Exit Sub
        o = c.Order
        If o.OrderNo <> Nothing Then
            If isProductReview Then
                If o.IsShowReviewCart Then
                    trCartItems.Visible = False
                End If
                If isReview = True And isShowReviewCart = True Then
                    lit.Visible = False
                    trCartItems.Visible = False
                Else
                    'Dim CrOrderId As String = System.Web.HttpUtility.UrlEncode(CryptData.Crypt.EncryptTripleDes(o.OrderId.ToString()))
                    Dim CrOrderId As String = o.OrderId
                    lit.Text = o.OrderNo & IIf(isReview = False, " <span class=""small review-link""><a href='" & ConfigData.OrderReviewUrl() & "?Id=" & CrOrderId & "'>Review this order</a></span>", "")
                    LoadFromDb()
                End If

            Else
                lit.Text = o.OrderNo
                LoadFromDb()
            End If
            lit.Text = "Order #" & lit.Text
        End If
    End Sub
    Private Sub LoadFromDb()
        If isProductReview Then
            lstCartItem = StoreCartItemRow.GetCartReview(DB, o.OrderId, o.MemberId)
        Else
            lstCartItem = c.GetCartItems()
        End If
        CountData = lstCartItem.Count
        For i As Integer = 0 To CountData - 1

        Next
        rptCartItems.DataSource = lstCartItem
        rptCartItems.DataBind()
    End Sub
    Public Sub rptCartItems_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptCartItems.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim ci As StoreCartItemRow = e.Item.DataItem
                Dim si As StoreItemRow = StoreItemRow.GetRow1(DB, ci.ItemId, o.MemberId, Utility.Common.GetOrderIdFromCartCookie(), ci.IsRewardPoints)

                Dim trCart As HtmlControl = CType(e.Item.FindControl("trCart"), HtmlControl)
                Dim objPrice As Object = BaseShoppingCart.GetItemPrice(DB, si, 1, 0, 0, False)

                If ci.IsFreeItem Or si.IsFreeGift Or si.IsFreeSample Or si.PermissionBuyBrand = False Or (si.IsFlammable And isInternational) Or ci.Type = Common.CartItemTypeBuyPoint Then
                    trCart.Visible = False
                    Exit Sub
                End If

                Dim lnkImg As Literal = CType(e.Item.FindControl("lnkImg"), Literal)
                Dim litDetails As Literal = CType(e.Item.FindControl("litDetails"), Literal)
                Dim ltAddCart As Literal = e.Item.FindControl("ltaddCart")
                Dim litCoupon As Literal = e.Item.FindControl("litCoupon")
                Dim ltPrice As Literal = e.Item.FindControl("ltPrice")
                Dim ltPrice1 As Literal = e.Item.FindControl("ltPrice1")
                Dim SQL As String = "SELECT coalesce(SUM(coalesce(totalqtyshipped,0)),0) FROM StoreOrderShipmentLine WHERE OrderId = " & ci.OrderId & " AND CartItemid = " & ci.CartItemId
                If si.MixMatchDescription <> Nothing Then
                    litCoupon.Text = "<div class=""mag"">" & si.MixMatchDescription & "</div>"
                End If
                litDetails.Text = "<div><b>" & Server.HtmlEncode(si.ItemName) & IIf(isProductReview, "", "<span class=""xs-unit""></span>") & "</b></div>"
                litDetails.Text &= "<div style=""margin-top:2px;"">Item# " & si.SKU & IIf(isProductReview, "  -  " & si.PriceDesc, "") & "</div>" & IIf(ci.Attributes <> Nothing, "<div class=""mag"">" & ci.AttributeSKU & "</div>", "") & vbCrLf
                If ci.Swatches <> Nothing Then litDetails.Text &= "<div style=""margin-top:6px;"" class=""smaller"">" & ci.Swatches.Replace(vbCrLf, "<br />") & "</div>"
                lnkImg.Text = "<img src=""" & ConfigData.CDNMediaPath & "/assets/items/cart/" & IIf(String.IsNullOrEmpty(ci.Image), "na.jpg", ci.Image) & """ alt="""" />"

                If ci.AddType = 2 Then
                    ltPrice.Text = "<span class=""bold"">" & FormatCurrency(si.CasePrice) & " (Case)</span>"
                Else
                    If (si.IsRewardPoints And si.RewardPoints > 0 And ci.IsRewardPoints) Then
                        ltPrice.Text = Utility.Common.FormatPointPrice(si.RewardPoints)
                    Else
                        ltPrice.Text = StoreItemRow.ReturnPrice(objPrice)
                    End If
                End If
                
                ltPrice1.Text = ltPrice.Text
                'btn add cart
                Dim isIncart As Boolean = si.IsInCart
                Dim strAddCartEvent As String = "mainScreen.ExecuteCommand('AddCart', 'methodHandlers.ShowCart', ['{0}', GetQty('{0}'),false]);"
                If si.IsRewardPoints And si.RewardPoints > 0 And ci.IsRewardPoints Then
                    strAddCartEvent = "mainScreen.ExecuteCommand('AddCartRewardPoint', 'methodHandlers.AddCartRewardPointCallBack', ['{0}', GetQtyPoint('{0}')]);"
                    litCoupon.Visible = False
                End If
                Dim btnAddcart As String = "<input value=""Add to Cart"" type=""button"" class=""bt-add-cart bg-add-cart"" onclick=""{0}"" />"
                btnAddcart = String.Format(btnAddcart, strAddCartEvent)
                ' Dim btnAddCart As String = "<input value=""Add to Cart"" type=""button"" class=""bt-add-cart bg-add-cart"" onclick=""mainScreen.ExecuteCommand('AddCart', 'methodHandlers.ShowCart', ['{0}', GetQty('{0}')]);"" />"
                btnAddcart = String.Format(btnAddcart, ci.ItemId)
                Dim txtQty As String = String.Format("<div class=""qty""><div class=""plus""><a href=""javascript:void(IncreaseMulti('txtQtyItem{1}{0}'))"">+</a></div><div class=""min""><a href=""javascript:void(DecreaseMulti('txtQtyItem{1}{0}',0))"">&ndash;</a></div><div class=""pull-left""><input type=""tel"" class=""txt-qty"" name=""txtQtyItem{1}{0}"" id=""txtQtyItem{1}{0}"" value=""0"" onkeypress=""return numbersonly();"" maxlength=""4"" /></div>" & _
                                                   "<div id=""arrow-qty"" class=""bao-arrow""><ul><a href=""javascript:void(IncreaseMulti('txtQtyItem{1}{0}'))""><li><b class=""arrow-up arrow-down-act"" id=""imgIntxtQty{0}""></b></li></a>" & _
                                                   "<a href=""javascript:void(DecreaseMulti('txtQtyItem{1}{0}',0))""><li class=""bd-qty""><b class=""arrow-down arrow-down-act"" id=""imgDetxtQty{0}""></b></li></a></ul></div></div>", ci.ItemId, IIf(ci.IsRewardPoints, "Point", ""))

                If ci.AddType = 2 Then
                    ltAddCart.Text = String.Format("<div class=""add-cart""><input value=""More Info"" type=""button"" class=""bt-add-cart bg-more-info"" onclick=""window.location.href='{0}';""></div>", URLParameters.ProductUrl(si.URLCode, si.ItemId))
                Else
                    ltAddCart.Text = "<div class=""add-cart"">" & txtQty & btnAddcart & "</div> <div name=""lblInCart" & IIf(ci.IsRewardPoints, "Point", "") & ci.ItemId & """ id=""lblInCart" & IIf(ci.IsRewardPoints, "Point", "") & ci.ItemId & """ class=""incart"">" & IIf(isIncart, "Added to your cart", "") & "</div>"
                End If

                If isProductReview Then
                    If ci.IsRewardPoints Then
                        trCart.Visible = False
                        Exit Sub
                    End If
                    litCoupon.Visible = False
                    ltAddCart.Text = String.Empty
                    ltPrice.Text = String.Empty
                    Dim sm As ShipmentMethodRow = ShipmentMethodRow.GetRow(DB, ci.CarrierType)
                    Dim litImageShipping As Literal = CType(e.Item.FindControl("litImageShipping"), Literal)
                    Dim litSelected As Literal = CType(e.Item.FindControl("litSelected"), Literal)
                    Dim ltCartReview As Literal = e.Item.FindControl("ltCartReview")
                    Dim ltrTotal As Literal = e.Item.FindControl("ltrTotal")
                    Dim ltrTotal1 As Literal = e.Item.FindControl("ltrTotal1")
                    If sm.Image <> Nothing AndAlso o.BillToCountry = "US" Then
                        If Utility.Common.CheckShippingSpecialUS(o) = True Then
                            If ci.IsOversize = False Then
                                litImageShipping.Text = "/includes/theme-admin/images/global/ico_international.gif"
                            Else
                                litImageShipping.Text = "/includes/theme-admin/images/global/" & sm.Image
                            End If
                        Else
                            litImageShipping.Text = "/includes/theme-admin/images/global/" & sm.Image
                        End If
                    ElseIf sm.Image <> Nothing AndAlso o.BillToCountry <> "US" Then
                        If Utility.Common.CheckShippingSpecialUS(o) = True Then
                            If ci.IsOversize = False Then
                                litImageShipping.Text = "/includes/theme-admin/images/global/ico_international.gif"
                            Else
                                litImageShipping.Text = "/includes/theme-admin/images/global/" & sm.Image
                            End If
                        Else
                            litImageShipping.Text = "/includes/theme-admin/images/global/" & sm.Image
                        End If
                    Else
                        litImageShipping.Text = "/includes/theme-admin/images/spacer.gif"
                    End If

                    If o IsNot Nothing AndAlso (o.HazardousMaterialFee > 0 And Common.InternationalShippingId().Contains(o.CarrierType)) Then
                        litImageShipping.Text = String.Empty
                    Else
                        litImageShipping.Text = "<li style=""width:27px;vertical-align:top""><img src='" & webRoot & litImageShipping.Text & "' style=""width:25px;height:26px""/></li>"
                    End If

                    Dim isWebTemplate As Boolean = StoreItemRow.IsWebItemplateItem(ci.SKU)
                    If (isWebTemplate Or ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                        litSelected.Text = String.Empty
                        litImageShipping.Text = String.Empty
                    Else
                        Dim CountryCode As String = IIf(o.IsSameAddress, o.BillToCountry, o.ShipToCountry)
                        Dim shippingName As String = ShipmentMethodRow.GetRow(DB, ci.CarrierType).Name
                        If CountryCode <> "US" Then
                            If Common.InternationalShippingId.Contains(o.CarrierType) Then
                                litSelected.Text = "<div style=""margin-top:3px;"">International Shipping</div>"
                            Else
                                litSelected.Text = "<div style=""margin-top:3px;"">" & shippingName & "</div>"
                            End If
                        Else
                            If Utility.Common.CheckShippingSpecialUS(o) = True Then
                                If Common.InternationalShippingId.Contains(o.CarrierType) Then
                                    litSelected.Text = "<div style=""margin-top:3px;"">International Shipping</div>"
                                Else
                                    litSelected.Text = "<div style=""margin-top:3px;"">" & shippingName & "</div>"
                                End If
                            Else
                                litSelected.Text = "<div style=""margin-top:3px;"">" & shippingName & IIf(ci.IsFreeShipping, "<br /><span class=""small smallerred"">FREE Shipping</span>", "") & "</div>"
                            End If

                        End If
                    End If
                    If (ci.Type = Common.CartItemTypeBuyPoint Or ci.IsRewardPoints) Then
                        ltCartReview.Text = String.Empty
                    ElseIf ci.IsFreeItem = True Then
                        ltCartReview.Text = String.Empty
                    ElseIf ci.IsFreeSample = True Then
                        ltCartReview.Text = String.Empty
                    Else
                        ltCartReview.Text = IIf(ci.IsReviewed, "Reviewed", "<a href=" & weblink & ConfigData.ProductReviewUrl() & "?id=" & ci.CartItemId & " target='_blank'>" & IIf(ci.isFirstReview, Resources.Msg.review_First, Resources.Msg.review_Old) & "</a>")
                    End If
                    If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                        ltrTotal.Text = Utility.Common.FormatPointPrice(ci.SubTotalPoint)
                    Else
                        ltrTotal.Text = FormatCurrency(ci.Total)
                    End If
                    ltrTotal1.Text = ltrTotal.Text
                End If
            End If
        End If
    End Sub
End Class

Imports Components
Imports DataLayer
Partial Class controls_product_product
    Inherits BaseControl
    Private LastDepartment As String
    Private reviewCount As Integer
    Private averageStars As Double
    Private ItemPrice As String
    Public Property cartItemId() As Integer
        Get
            Return ViewState("cartItemId")
        End Get
        Set(ByVal value As Integer)
            ViewState("cartItemId") = value
        End Set
    End Property
    Public Property ItemID() As Integer
        Get
            Return ViewState("ItemID")
        End Get
        Set(ByVal value As Integer)
            ViewState("ItemID") = value
        End Set
    End Property

    Public Property CountDiv() As Integer
        Get
            Return ViewState("CountDiv")
        End Get
        Set(ByVal value As Integer)
            ViewState("CountDiv") = value
        End Set
    End Property
    Public Property Image() As String
        Get
            Return ViewState("Image")
        End Get
        Set(ByVal value As String)
            ViewState("Image") = value
        End Set
    End Property
    Public Property ItemName() As String
        Get
            Return ViewState("ItemName")
        End Get
        Set(ByVal value As String)
            ViewState("ItemName") = value
        End Set
    End Property
    Public Property PromotionText() As String
        Get
            Return ViewState("PromotionText")
        End Get
        Set(ByVal value As String)
            ViewState("PromotionText") = value
        End Set
    End Property
    Public Property isIncart() As Boolean
        Get
            Return ViewState("isIncart")
        End Get
        Set(ByVal value As Boolean)
            ViewState("isIncart") = value
        End Set
    End Property
    Public Property IsFirstLoad() As Boolean
        Get
            Return ViewState("IsFirstLoad")
        End Get
        Set(ByVal value As Boolean)
            ViewState("IsFirstLoad") = value
        End Set
    End Property
    Public Sub New()

    End Sub

    Public Sub New(ByVal labelText As String, ByVal howManyTimes As Integer)
        MyBase.New()
        Dim sb As New System.Text.StringBuilder()
        For i As Integer = 1 To howManyTimes
            sb.Append(labelText)
            sb.Append("")
        Next
        litBtnAddCart.Text = sb.ToString()
    End Sub


    Public Sub Fill(ByVal CountDiv As Integer, ByVal StoreItem As StoreItemRow, ByVal CurrentQtyAddCardID As String, ByVal CurrentQtyAddCardValue As Integer, ByVal isInternational As Boolean)
        Me.CountDiv = CountDiv
        Fill(StoreItem, CurrentQtyAddCardID, CurrentQtyAddCardValue, isInternational)
    End Sub

    Public Sub Fill(ByVal StoreItem As StoreItemRow, ByVal CurrentQtyAddCardID As String, ByVal CurrentQtyAddCardValue As Integer, ByVal isInternational As Boolean)
        Dim si As New StoreItemRow
        si = StoreItem
        Dim objecPrice As Object = BaseShoppingCart.GetItemPrice(DB, si, 1, 0, 0, False)
        ItemID = si.ItemId
        ItemName = si.ItemName2
        If String.IsNullOrEmpty(LastDepartment) Then
            LastDepartment = StoreItemRow.GetDefaultDepartmentNameByItemId(si.ItemId)
        End If
        ItemName = ItemName & " " & LastDepartment

        Image = si.Image
        Try
            Dim orderid As Integer = Utility.Common.GetOrderIdFromCartCookie()
            If orderid > 0 Then
                isIncart = DB.ExecuteScalar("SELECT  dbo.fc_StoreCartItem_CheckItemInCart(" & Utility.Common.GetOrderIdFromCartCookie() & ", " & si.ItemId & "," & IIf(si.IsRewardPoints, 1, 0) & ")")
            End If
        Catch ex As Exception
            isIncart = False
        End Try
        Dim sURL As String = URLParameters.ProductUrl(si.URLCode, si.ItemId)
        aItem1.HRef = sURL
        aItem2.InnerHtml = "<h2>" & si.ItemName2 & "</h2>"
        aItem2.HRef = sURL
        If Me.CountDiv = 0 Then
            CountDiv = si.itemIndex
        End If

        Dim strAddCartEvent As String = "mainScreen.ExecuteCommand('AddCart', 'methodHandlers.ShowCart', ['{0}', GetQty('{0}'),false]);"
        litBtnAddCart.Text = "<input value=""{text}"" type=""button"" class=""{css}"" onclick=""{event}"" />"

        'Promotion

        PromotionText = si.MixMatchDescription
        reviewCount = si.CountReview
        averageStars = si.AverageReview

        Dim isPageItemPoint As Boolean = False
        Try
            isPageItemPoint = HttpContext.Current.Request.Url.ToString.Contains("reward-point.aspx")
        Catch
        End Try
        Dim imgReview As String = String.Empty
        If (reviewCount >= 1) Then
            If averageStars.ToString().Contains(".5") Then
                averageStars = averageStars.ToString().Replace(".", "")
            Else
                averageStars = averageStars.ToString().Replace(".", "") & "0"
            End If
            If isPageItemPoint Then
                litReview.Text = String.Format("<div class=""review""><img alt=""{3}"" src=""" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/star{0}.png"" /><a style='text-decoration:none;' >{1} review{2}</a></div>", averageStars, reviewCount, IIf(reviewCount > 1, "s", ""), "")
            Else
                litReview.Text = String.Format("<div class=""review""><img alt=""{3}"" src=""" & Utility.ConfigData.CDNMediaPath & "/includes/theme/images/star{0}.png"" /><a href=""{4}#review-section"">{1} review{2}</a></div>", averageStars, reviewCount, IIf(reviewCount > 1, "s", ""), "", URLParameters.ProductUrl(si.URLCode, si.ItemId))

            End If

        Else
            litReview.Visible = False
        End If

        'icon new, hot, bestseller
        'Dim strImg As String = "<img src=""/includes/theme/images/{0}"" alt=""New!"" />&nbsp;"
        Dim icContent As String = ""
        If si.IsFreeShipping Then
            icContent = String.Format("<span class='ico-freeshipping'></span>")
        ElseIf si.IsNew Then
            icContent = String.Format("<span class='ico-new'></span>")
        ElseIf si.IsBestSeller Then
            icContent = String.Format("<span class='ico-bestseller'></span>")
        ElseIf si.IsHot Then
            icContent = String.Format("<span class='ico-hot'></span>")
        End If
        lticItem.Text = IIf(icContent <> "", "<div class=""ic-item"">" & icContent & "</div>", "")

        If si.IsHot Or si.IsNew Or si.IsBestSeller Or si.IsFreeShipping Then
            lticItem.Visible = True
        Else
            lticItem.Visible = False
        End If



        If isPageItemPoint Then
            aItem1.HRef = "javascript:void(0)"
            aItem2.HRef = "javascript:void(0)"
            aItem1.Attributes.Add("class", "disableLink")
            aItem2.Attributes.Add("class", "disableLink")
            strAddCartEvent = "mainScreen.ExecuteCommand('AddCartRewardPoint', 'methodHandlers.AddCartRewardPointCallBack', ['{0}', GetQty('{0}')]);"
            PromotionText = String.Empty
        End If

        'Price
        ItemPrice = StoreItem.ReturnPrice(objecPrice)
        lblPrice.Text = ItemPrice
        If si.youSave > 0 And ItemPrice.Contains("As low as") = False Then
            ltsave.Text = "<li class=""save"">You Save: <span class=""yousave"">" & FormatCurrency(si.youSave) & "</span> (<span class=""savepercent"">" & si.savePercent & "</span>%)</li>"
        Else
            ltsave.Visible = False
        End If

        If Not (isPageItemPoint) Then
            si.IsRewardPoints = False
            si.RewardPoints = 0
        End If
        Dim IsActive, IsRecent As Boolean
        Try
            If Request.Url.ToString.Contains("recently-viewed.aspx") Then
                IsRecent = True
            End If
            IsActive = si.IsActive
        Catch ex As Exception
            IsActive = False
            IsRecent = False
        End Try
        If IsActive = False And IsRecent Then
            lblPrice.Text = "<span class=""red""> It is no longer available. Please select a different item.</span>"
            divCart.Visible = False
            litBtnAddCart.Text = String.Empty
            ltsave.Text = String.Empty
        ElseIf si.IsFreeSample Or si.IsFreeGift >= 2 Then
            divCart.Visible = False
            litBtnAddCart.Text = String.Empty 'btnAddCart.Text = String.Empty
            lblPrice.Visible = False
            ltsave.Visible = False
        ElseIf si.PermissionBuyBrand = False AndAlso Utility.Common.GetCurrentMemberId > 0 Then
            lblPrice.Text = "<span class=""red"">Available only in store or by phone order</span>"
            divCart.Visible = False
            litBtnAddCart.Text = String.Empty
            ltrQty.Text = String.Empty
            ltsave.Text = String.Empty
        ElseIf si.IsFlammable = True And isInternational Then
            divCart.Visible = False
            litBtnAddCart.Text = String.Empty 'btnAddCart.Visible = False
            ltrQty.Text = String.Empty
            lblPrice.Text = "<span class=""red"">This item is not available for customer outside of 48 states within continental USA.</span>"
            ltsave.Visible = False
        Else
            If si.IsInCart Then
                If (isPageItemPoint) Then
                    divCart.Visible = True
                End If
            Else
                divCart.Visible = False
            End If

            If si.QtyOnHand <= 0 And Not Utility.Common.IsItemAcceptingOrder(si.AcceptingOrder) And Not si.IsSpecialOrder Then
                litBtnAddCart.Text = String.Empty 'btnAddCart.Visible = False
                divCart.Text = String.Format("<div id=""divCart"" class=""red"">Currently Out of Stock <br><a href=""#"" onclick=""NotifyInStock('{0}');"">Notify me when in stock</a></div>", si.ItemId)
                ltrQty.Text = String.Empty
                divCart.Visible = True
                If (isPageItemPoint) Then
                    lblPrice.Text = "<span class='pointPrice'>Redeem Points: " & Utility.Common.FormatPointPrice(si.RewardPoints) & "</span>"
                Else
                    If (si.IsRewardPoints And si.RewardPoints > 0) Then
                        Dim priceTable As String = "<table class='itemprice' cellpadding='0' cellspacing='0'>"
                        priceTable &= "<tr><td> " & "Redeem Points: " & Utility.Common.FormatPointPrice(si.RewardPoints) & "</td></tr>"
                        priceTable &= "<tr><td> " & ItemPrice & "</td></tr></table>"
                        lblPrice.Text = priceTable
                    Else
                        lblPrice.Text = ItemPrice
                    End If

                End If
            Else
                Dim isItemMulti As Boolean
                Try
                    isItemMulti = StoreItemRow.IsItemMultiPrice(si.ItemId, Session("MemberId"))
                Catch ex As Exception
                    isItemMulti = False
                End Try

                If si.IsVariance Or isItemMulti Then
                    If (isPageItemPoint) Then
                        litBtnAddCart.Text = litBtnAddCart.Text.Replace("{css}", "bt-add-cart bg-add-cart")  'btnAddCart.Attributes.Add("class", "bt-add-cart bg-add-cart")
                        litBtnAddCart.Text = litBtnAddCart.Text.Replace("{text}", "Add to Cart") 'btnAddCart.Attributes.Add("value", "Add to Cart")
                        litBtnAddCart.Text = litBtnAddCart.Text.Replace("{event}", String.Format(strAddCartEvent, si.ItemId.ToString())) 'btnAddCart.Attributes.Add("onclick", "AddToCart(" & si.ItemId & ", 'ctl00_ContentPlaceHolder1_ctl0" & itemID & "_btnAddCart_txtQtyItem');")
                        lblPrice.Text = "<span class='pointPrice'>Redeem Points: " & Utility.Common.FormatPointPrice(si.RewardPoints) & "</span>"


                        ltrQty.Text = String.Format("<div class=""qty""><div class=""plus""><a href=""javascript:void(Increase('txtQtyItem{0}'))"">+</a></div><div class=""min""><a href=""javascript:void(Decrease('txtQtyItem{0}',1))"">&ndash;</a></div><div class=""pull-left""><input type=""tel"" class=""txt-qty"" name=""txtQtyItem{0}"" id=""txtQtyItem{0}"" value=""1"" onkeypress=""return numbersonly();"" maxlength=""4"" /></div>" & _
                                                    "<div class=""bao-arrow""><ul><a href=""javascript:void(Increase('txtQtyItem{0}'))""><li><b class=""arrow-up arrow-down-act"" id=""""></b></li></a>" & _
                                                    "<a href=""javascript:void(Decrease('txtQtyItem{0}',1))""><li class=""bd-qty""><b class=""arrow-down arrow-down-act"" id=""imgDetxtQty{0}""></b></li></a></ul></div></div>", ItemID)
                        'End If
                    Else
                        litBtnAddCart.Text = litBtnAddCart.Text.Replace("{css}", "bt-add-cart bg-more-info") 'btnAddCart.Attributes.Add("class", "bt-add-cart bg-more-info")
                        litBtnAddCart.Text = litBtnAddCart.Text.Replace("{text}", "More Info") 'btnAddCart.Attributes.Add("value", "More Info")
                        litBtnAddCart.Text = litBtnAddCart.Text.Replace("{event}", "window.location.href='" & sURL & "';") 'btnAddCart.Attributes.Add("onclick", "window.location.href='" & sURL & "';")
                        ltrQty.Text = String.Empty
                        If (si.IsRewardPoints And si.RewardPoints > 0) Then
                            Dim priceTable As String = "<table class='itemprice' cellpadding='0' cellspacing='0'>"
                            priceTable &= "<tr ><td> " & "Redeem Points: " & Utility.Common.FormatPointPrice(si.RewardPoints) & "</td></tr>"
                            priceTable &= "<tr ><td> " & ItemPrice & "</td></tr></table>"
                            lblPrice.Text = priceTable
                        Else
                            lblPrice.Text = ItemPrice '"As low as $" & objecPrice.MinMultiPrice
                        End If
                    End If

                Else
                    litBtnAddCart.Text = litBtnAddCart.Text.Replace("{css}", "bt-add-cart bg-add-cart") 'btnAddCart.Attributes.Add("class", "bt-add-cart bg-add-cart")
                    litBtnAddCart.Text = litBtnAddCart.Text.Replace("{text}", "Add to Cart") 'btnAddCart.Attributes.Add("value", "Add to Cart")
                    divCart.Visible = False
                    If (isPageItemPoint) Then
                        lblPrice.Text = "<span class='pointPrice'>Redeem Points: " & Utility.Common.FormatPointPrice(si.RewardPoints) & "</span'>"
                        If (divCart.Visible = True) Then
                            litBtnAddCart.Text = String.Empty 'btnAddCart.Visible = False
                        Else
                            litBtnAddCart.Text = litBtnAddCart.Text.Replace("{event}", String.Format(strAddCartEvent, si.ItemId.ToString()))
                            'btnAddCart.Attributes.Add("onclick", "AddToCart(" & si.ItemId & ", 'ctl00_ContentPlaceHolder1_ctl0" & CountDiv & "_btnAddCart_txtQtyItem');")
                        End If

                    Else
                        lblPrice.Text = ItemPrice
                        litBtnAddCart.Text = litBtnAddCart.Text.Replace("{event}", String.Format(strAddCartEvent, si.ItemId.ToString())) 'btnAddCart.Attributes.Add("onclick", "AddToCart(" & si.ItemId & ", 'ctl00_ContentPlaceHolder1_ctl0" & CountDiv & "_btnAddCart_txtQtyItem');")
                    End If

                    If isInternational And si.IsFlammable Then
                        litBtnAddCart.Text = String.Empty 'btnAddCart.Visible = False
                        ltrQty.Text = String.Empty
                    Else
                        ltrQty.Text = String.Format("<div class=""qty""><div class=""plus""><a href=""javascript:void(Increase('txtQtyItem{0}'))"">+</a></div><div class=""min""><a href=""javascript:void(Decrease('txtQtyItem{0}',1))"">&ndash;</a></div><div class=""pull-left""><input type=""tel"" class=""txt-qty"" name=""txtQtyItem{0}"" id=""txtQtyItem{0}"" value=""1"" onkeypress=""return numbersonly();"" maxlength=""4"" /></div>" & _
                                                    "<div class=""bao-arrow""><ul><li><a href=""javascript:void(Increase('txtQtyItem{0}'))""><b class=""arrow-up arrow-down-act"" id=""imgIntxtQty{0}""></b></a></li>" & _
                                                    "<li class=""bd-qty""><a href=""javascript:void(Decrease('txtQtyItem{0}',1))""><b class=""arrow-down arrow-down-act"" id=""imgDetxtQty{0}""></b></a></li></ul></div></div>", ItemID)
                    End If
                End If
            End If
        End If
        divRemoveCart.Attributes.Add("style", "display:none")
        If (isPageItemPoint) Then
            divRemoveCart.ID = divRemoveCart.ID & ItemID
            divCartWrapper.ID = divCartWrapper.ID & ItemID

            If Not (Session("MemberId") Is Nothing) Then

                cartItemId = DB.ExecuteScalar("Select [dbo].[fc_StoreCartItem_GetCartItemID](" & Session("MemberId") & "," & si.ItemId & ",1)")
                If (cartItemId > 0) Then
                    divCartWrapper.Attributes.Add("style", "display:none")
                    divRemoveCart.Attributes.Add("style", "display:")
                    divRemoveCart.InnerHtml = "<a href='javascript:void(0)' onclick=""RemoveCartItemRewardPoint(" & cartItemId & "," & si.ItemId & ");"">Remove</a>"
                End If
            End If
        End If

    End Sub
End Class

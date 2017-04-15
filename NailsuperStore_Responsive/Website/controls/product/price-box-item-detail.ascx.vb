Imports Components
Imports DataLayer
Partial Class controls_product_price_box_item_detail
    Inherits BaseControl
    Private m_Item As StoreItemRow = Nothing
    Public AcceptingOrderMessage As String = String.Empty
    Public AcceptingOrderName As String = String.Empty
    Public isAcceptingOrder As Boolean = False
    Protected typeOffer As String = "Offer"
    Public linkMSDS As String = String.Empty
    Public Property Item() As StoreItemRow
        Set(ByVal value As StoreItemRow)
            m_Item = value
        End Set
        Get
            Return m_Item
        End Get
    End Property
    Private currentMemberId As Integer = 0
    Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Item Is Nothing AndAlso Not Session("itemRender") Is Nothing Then
            Item = Session("itemRender")
        End If
        If Item Is Nothing Then
            Me.Visible = False
            Exit Sub
        End If

        If Item.IsLoginViewPrice Then
            divLoginViewPrice.Visible = True
            divPriceBox.Visible = False
            Exit Sub
        End If

        If Item.CaseQty > 0 And Item.CasePrice > 0 Then
            ucBuyInBulk.Item = Item
        Else
            ucBuyInBulk.Visible = False
        End If

        txtQty.Attributes.Add("onKeypress", " return numbersonly(txtQty,event);")
        If Item.IsActive = False Then
            cartWrapper.Visible = False
        End If
        currentMemberId = Utility.Common.GetCurrentMemberId()
        btnAddCart.Attributes.Add("onclick", "return AddCartDetail(" & Item.ItemId & ");")

        LoadPromotion(Item)
        LoadMessage(Item)
        LoadAttibuteGroup()
        LoadIcon()

        CheckFlammable(Item)
        LoadPriceItem(Item)
        If Not String.IsNullOrEmpty(linkMSDS) Then
            divMSDS.Visible = True
        End If
    End Sub
    Private Sub AllowShowAddCart(ByVal objItem As StoreItemRow)

        If Not (objItem.IsActive) Then
            Exit Sub
        End If
        If objItem.IsFreeSample = True Or objItem.IsFreeGift >= 2 Then
            cartWrapper.Visible = False
            Exit Sub
        End If
        cartWrapper.Visible = True
    End Sub
    Private Sub LoadMessage(ByVal objItem As StoreItemRow)

        If objItem.IsFreeSample = True Or objItem.IsFreeGift >= 2 Then
            cartWrapper.Visible = False
        Else
            If objItem.IsActive = True Then
                cartWrapper.Visible = True
            End If
        End If

        divInCart.Visible = False
        Dim orderId As Integer = Utility.Common.GetOrderIdFromCartCookie()
        If orderId > 0 Then
            divInCart.Visible = CDbl(DB.ExecuteScalar("SELECT dbo.fc_StoreCartItem_CheckItemInCart(" & orderId.ToString() & ", " & objItem.ItemId & " ,0) "))
        End If


        'Check OPI
        'Khoa tat vi no ko co su dung nua
        'Dim isOPI As Boolean = False
        'If currentMemberId > 0 Then
        '    'Kiem tra xem item co thuoc Brand ko duoc phep ban
        '    Dim sie As New StoreItemEnable()

        '    Dim dt As DataTable = sie.ListBrands(currentMemberId)
        '    If Not IsDBNull(dt) AndAlso dt.Rows.Count > 0 And cartWrapper.Visible = True Then
        '        Dim brands As String = dt.Rows(0)("Brands").ToString()
        '        Dim memberBrands As String = dt.Rows(0)("MemberBrands").ToString()

        '        If memberBrands.Contains("," & objItem.BrandId & ",") Then
        '            isOPI = False
        '        Else
        '            If brands.Contains("," & objItem.BrandId & ",") Then
        '                isOPI = True
        '            Else
        '                isOPI = False
        '            End If
        '        End If
        '        dt.Dispose()
        '    End If
        '    'End
        'End If

        'If isOPI Then
        '    divMessage.InnerHtml = Resources.Msg.OPI 'OPI products are available only in store or by phone order
        '    divMessage.Visible = True
        '    cartWrapper.Visible = False
        '    divOutStock.Visible = False
        '    divInStock.Visible = False
        '    divStockWarning.InnerText = String.Empty
        '    divNotify.Visible = False
        '    Exit Sub
        'End If

        isAcceptingOrder = False
        AcceptingOrderMessage = ""
        divOutStock.Visible = False
        divInStock.Visible = False
        divStockWarning.InnerText = String.Empty
        divNotify.Visible = False

        If (objItem.IsSpecialOrder) Then
            AcceptingOrderMessage = DataLayer.SysParam.GetValue("SpecialOrderMessage")
        ElseIf Utility.Common.IsItemAcceptingOrder(objItem.AcceptingOrder) Then
            isAcceptingOrder = True
            AcceptingOrderName = Utility.Common.ConvertItemAcceptingStatusToName(objItem.AcceptingOrder)
            If objItem.AcceptingOrder = Utility.Common.ItemAcceptingStatus.AcceptingOrder Then
                AcceptingOrderMessage = Resources.Msg.AceptingOrder
            End If
        Else
            If objItem.QtyOnHand > 0 AndAlso objItem.IsActive Then
                AllowShowAddCart(objItem)
                divInStock.Visible = True

                If objItem.LowStockThreshold > 0 AndAlso objItem.QtyOnHand <= objItem.LowStockThreshold Then
                    If objItem.LowStockMsg <> Nothing Then
                        divStockWarning.InnerText &= objItem.LowStockMsg.Replace("[QTY]", objItem.QtyOnHand)
                    Else
                        divStockWarning.InnerText = SysParam.GetValue("HurryMessage").Replace("[QTY]", objItem.QtyOnHand)
                    End If
                ElseIf objItem.LowStockThreshold = 0 AndAlso SysParam.GetValue("HurryMessageThreshold") <> Nothing AndAlso objItem.QtyOnHand <= SysParam.GetValue("HurryMessageThreshold") Then
                    If objItem.LowStockMsg <> Nothing Then
                        divStockWarning.InnerText = objItem.LowStockMsg.Replace("[QTY]", objItem.QtyOnHand)
                    End If
                    divStockWarning.InnerText &= SysParam.GetValue("HurryMessage").Replace("[QTY]", objItem.QtyOnHand)
                End If
                If (String.IsNullOrEmpty(divStockWarning.InnerText.Trim())) Then
                    divInStock.Visible = False
                End If
            Else
                divOutStock.Visible = True
                cartWrapper.Visible = False
            End If
            If objItem.QtyOnHand < 1 OrElse objItem.Status = "DC" Then
                If objItem.Status <> "DC" Then
                    divNotify.Visible = True
                    lnkNotify.Attributes.Add("onclick", "NotifyInStock('" & objItem.ItemId & "');")
                End If
            End If
        End If

        If Not String.IsNullOrEmpty(AcceptingOrderMessage) Then
            divMessage.InnerHtml = AcceptingOrderMessage
            divMessage.Visible = True
        Else
            divMessage.Visible = False
        End If
    End Sub

    Public hasIcon As Boolean = False
    Public iconClass As String = String.Empty
    Public isHasAttribute As Boolean = False
    Public listOptionId As String = String.Empty
    Public listChoiceId As String = String.Empty
    Public hidUnitPoint As String = String.Empty
    Public hidPricePoint As String = String.Empty

    Public Sub LoadAttibuteGroup()
        listOptionId = String.Empty
        listChoiceId = String.Empty
        listOptionId = String.Empty
        lstChoiceCheckItem = String.Empty
        Dim lstAtt As StoreItemGroupOptionCollection = StoreItemGroupOptionRow.GetListByItemGroupId(Item.ItemGroupId)
        If Not lstAtt Is Nothing AndAlso lstAtt.Count() > 0 Then

            rptAttribute.Visible = True
            isHasAttribute = True
            rptAttribute.DataSource = lstAtt
            rptAttribute.DataBind()
            For Each item As RepeaterItem In rptAttribute.Items
                Dim ltrAttribute As Literal = DirectCast(item.FindControl("ltrAttribute"), Literal)
                ltrAttribute.Text = ltrAttribute.Text.Replace("{0}", listOptionId)
                ltrAttribute.Text = ltrAttribute.Text.Replace("{1}", listChoiceId)
            Next
        Else
            rptAttribute.Visible = False
        End If
    End Sub
    Public lstChoiceCheckItem As String = String.Empty
    Protected Sub rptAttribute_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptAttribute.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim ltrAttribute As Literal = DirectCast(e.Item.FindControl("ltrAttribute"), Literal)
            If Not ltrAttribute Is Nothing Then
                Dim choiceActiveId As Integer = 0
                Dim objOption As StoreItemGroupOptionRow = CType(e.Item.DataItem, StoreItemGroupOptionRow)
                If Not (objOption Is Nothing) Then

                    listOptionId = listOptionId & objOption.OptionId & ","
                    Dim strAttribute As String = String.Empty
                    Dim strOption As String = "<li class=""{0}"">Your " & objOption.OptionName.Trim().ToLower() & " is <span> {1}</span>"
                    Dim strOptionClass = String.Empty
                    Dim strAttributeSelect = String.Empty
                    Dim sql As String = String.Empty
                    Dim lstChoice As New DataTable
                    If e.Item.ItemIndex = 0 Then
                        strOptionClass = "first"
                        sql = "Select ChoiceId,ChoiceName,ThumbImage,[dbo].[fc_StoreItemGroupChoice_IsItemSelect](" & Item.ItemId & ",ChoiceId) as Active from StoreItemGroupChoice where OptionId=" & objOption.OptionId & "  and ChoiceId in( Select ChoiceId from StoreItemGroupChoiceRel ic left join  StoreItem si on(si.ItemId=ic.ItemId)  where  OptionId=" & objOption.OptionId & " and si.IsActive=1 and ItemGroupId=" & Item.ItemGroupId & ") order by SortOrder ASC"
                    Else
                        sql = "Select ChoiceId,ChoiceName,ThumbImage,[dbo].[fc_StoreItemGroupChoice_IsItemSelect](" & Item.ItemId & ",ChoiceId) as Active from StoreItemGroupChoice where OptionId=" & objOption.OptionId & "  and ChoiceId in( Select ChoiceId from StoreItemGroupChoiceRel ic left join  StoreItem si on(si.ItemId=ic.ItemId)  where  OptionId=" & objOption.OptionId & "  and ItemGroupId=" & Item.ItemGroupId & ") order by SortOrder ASC"
                    End If
                    Dim choiceNameActive As String = String.Empty
                    lstChoice = DB.GetDataTable(sql)
                    If (Not lstChoice Is Nothing AndAlso lstChoice.Rows.Count() > 0) Then
                        strAttribute = "<ul class=""sub"">"
                        Dim thumbImg As String = String.Empty
                        Dim Active As Integer = 0
                        Dim itemClass As String = String.Empty
                        Dim ChoiceId As Integer = 0
                        Dim isHasThumb As Boolean = False
                        Dim choiceName As String = String.Empty
                        Dim choiceIndex As Integer = 0
                        Dim itemTmpId As Integer = 0
                        For Each r As DataRow In lstChoice.Rows
                            thumbImg = r("ThumbImage").ToString()
                            choiceName = r("ChoiceName").ToString()
                            ChoiceId = CInt(r("ChoiceId").ToString())
                            If String.IsNullOrEmpty(lstChoiceCheckItem) Then
                                itemTmpId = StoreItemRow.GetItemIdByChoices(DB, Item.ItemGroupId, ChoiceId.ToString())
                            Else
                                itemTmpId = StoreItemRow.GetItemIdByChoices(DB, Item.ItemGroupId, lstChoiceCheckItem & "," & ChoiceId.ToString())
                            End If

                            If choiceIndex = 0 Then
                                If String.IsNullOrEmpty(thumbImg) Then
                                    strOptionClass &= " text"
                                Else
                                    isHasThumb = True
                                End If
                            End If
                            choiceIndex = choiceIndex + 1
                            Active = CInt(r("Active").ToString())
                            If Active = 1 Then
                                itemClass = "select"
                                choiceNameActive = choiceName
                                If (String.IsNullOrEmpty(listChoiceId)) Then
                                    listChoiceId = ";" & objOption.OptionId & "," & ChoiceId & ";"
                                Else
                                    listChoiceId = listChoiceId & objOption.OptionId & "," & ChoiceId & ";"
                                End If
                                choiceActiveId = ChoiceId
                            Else
                                If (itemTmpId > 0) Then
                                    itemClass = ""
                                Else
                                    itemClass = "deny"
                                End If


                            End If
                            If itemClass = "deny" Then
                                strAttribute = strAttribute & "<li class=" & itemClass & "> <div>"
                            Else
                                Dim paramDefault As String = String.Empty
                                If String.IsNullOrEmpty(lstChoiceCheckItem) Then
                                    paramDefault = ChoiceId
                                Else
                                    paramDefault = ";" & lstChoiceCheckItem & ";" & ChoiceId & ";"
                                End If
                                strAttribute = strAttribute & "<li class=" & itemClass & "> <div onclick=""SelectItemGroup(" & Item.ItemGroupId & "," & objOption.OptionId & ",'" & paramDefault & "','{0}','{1}');"">"

                            End If

                            If (isHasThumb) Then
                                Dim path As String = Utility.ConfigData.GroupChoiceThumbPath & "/" & thumbImg
                                strAttribute = strAttribute & "<a href='javascript:void(0)' ><img src='" & path & "' alt='" & choiceName & "' /></a>"
                            Else
                                strAttribute = strAttribute & "" & choiceName
                            End If
                            strAttribute = strAttribute & "</div></li>"
                        Next
                        strAttribute = strAttribute & "</ul></li>"
                    End If
                    strOption = String.Format(strOption, strOptionClass, choiceNameActive)

                    ltrAttribute.Text = ltrAttribute.Text & strOption & strAttribute
                    lstChoiceCheckItem = lstChoiceCheckItem & "," & choiceActiveId
                End If

            End If
        End If
    End Sub
    Private Sub LoadIcon()
        If ((Item.IsNew Or Item.NewUntil >= Date.Now) OrElse Item.IsHot OrElse Item.IsBestSeller OrElse Item.IsFreeShipping) Then
            hasIcon = True
        End If
        If Not hasIcon Then
            hasIcon = isAcceptingOrder
        End If
        If Not rptAttribute.Visible Then ''has Attribute
            iconClass = "icon none-att"
        Else
            iconClass = "icon"
        End If
        If Not hasIcon Then
            ltrIcon.Visible = False
        Else
            ltrIcon.Visible = True
            ltrIcon.Text = "  <div id='secIcon' class='" & iconClass & "'>"
            If (Item.IsFreeShipping) Then
                ltrIcon.Text &= "<span class='ico-freeshipping'>Free Shipping</span>"
            ElseIf Item.IsNewTrue Then
                ltrIcon.Text &= "<span class='ico-new'>New!</span>"
            ElseIf (Item.IsBestSeller) Then
                ltrIcon.Text &= "<span class='ico-bestseller'>Best Seller</span>"
            ElseIf (Item.IsHot) Then
                ltrIcon.Text &= "<span class='ico-hot'>Hot!</span>"
            End If

            If Not String.IsNullOrEmpty(AcceptingOrderName) Then
                ltrIcon.Text &= "<div id='instock' itemtype='http://schema.org/Offer'><link href='http://schema.org/InStock' itemprop='availability' /><span>" & AcceptingOrderName & " </span></div>"
            End If
            ltrIcon.Text &= "</div>"
        End If

    End Sub
    Private Sub CheckFlammable(ByVal objItem As StoreItemRow)
        If objItem.IsHazMat Then
            divFlammableWarning.Visible = True
            'cartWrapper.Visible = True
        End If

        'If MemberRow.CheckMemberIsInternational(currentMemberId, Session("OrderId")) And objItem.IsFlammable = True Then
        '    cartWrapper.Visible = False
        '    divMessage.InnerText = "This item is not available for customer outside of 48 states within continental USA."
        '    divMessage.Visible = True
        '    divMessage.Attributes.Add("class", "flammablemsg")
        'End If
    End Sub

    Public Sub LoadPromotion(ByVal objItem As StoreItemRow)
        divPromotion.Visible = False
        Dim PromotionDetails As String = String.Empty
        If Not objItem.Promotion Is Nothing AndAlso objItem.Promotion.MixMatchId > 0 AndAlso (objItem.Promotion.PurchaseItems.Count > 0 OrElse objItem.Promotion.GetItems.Count > 0) AndAlso Not String.IsNullOrEmpty(objItem.Promotion.Description) AndAlso (objItem.Promotion.MixMatchType = Utility.Common.MixmatchType.Normal) Then
            ltrMM.Text = NSS.ShowPromotionDetails(objItem, currentMemberId, Utility.Common.GetCurrentOrderId())
            If Not String.IsNullOrEmpty(ltrMM.Text) Then
                If Not objItem.Promotion.EndingDate = Nothing Then spanGoodThru.InnerHtml = " (Good thru " & objItem.Promotion.EndingDate.ToShortDateString & ")"
                divPromotion.Visible = True
            End If

        End If
    End Sub

    Private Sub LoadPriceItem(ByVal objItemShowPrice As StoreItemRow)

        hidPricePoint = String.Empty
        hidUnitPoint = SysParam.GetValue("GetPoint").ToString()

        Dim objItemPrice As ItemPrice = BaseShoppingCart.GetItemPrice(DB, objItemShowPrice, 1, 0, 0, False)
        Dim priceConvertPoint As Double = 0
        If (objItemPrice.MinMultiPrice > 0) Then
            priceConvertPoint = objItemPrice.MinMultiPrice
            If Not objItemPrice.MultiPriceColection Is Nothing Then
                Dim roundPrice As Double = 0
                For Each objMultiPrice As ItemMultiPrice In objItemPrice.MultiPriceColection
                    roundPrice = Utility.Common.RoundCurrency(objMultiPrice.Price)
                    hidPricePoint &= objMultiPrice.MinQty & "-" & objMultiPrice.MaxQty & "-" & roundPrice & ","
                Next
            End If

        ElseIf (objItemPrice.SalePrice > 0) Then
            priceConvertPoint = objItemPrice.SalePrice
        Else
            priceConvertPoint = objItemPrice.NormalPrice
        End If
        priceConvertPoint = Utility.Common.RoundCurrency(priceConvertPoint)
        Dim pointPurchaseItem As Double = CDbl(hidUnitPoint) * priceConvertPoint
        If pointPurchaseItem < 1 Then
            pointPurchaseItem = 1
        Else
            pointPurchaseItem = Math.Ceiling(pointPurchaseItem)
        End If
        If String.IsNullOrEmpty(hidPricePoint) Then
            hidPricePoint = priceConvertPoint.ToString()
        End If

        'Show price

        If (Not String.IsNullOrEmpty(objItemPrice.MultiPriceHTML)) Then
            If (Item.CaseQty > 0 And Item.CasePrice > 0) AndAlso Item.QtyOnHand >= Item.CaseQty Then
                ltrPrice.Text &= "<ul class='sale-price'><li class='price' itemprop=""price"">" & Utility.Common.ViewCurrency(Item.Price) & "</li><li><meta itemprop=""priceCurrency"" content=""USD"" /></li></ul>"
            Else
                ltrPrice.Text = objItemPrice.MultiPriceHTML
            End If
        Else
            ltrPrice.Text = "<ul class='sale-price'>"
            If (objItemPrice.SalePrice > 0) Then
                'ltrPrice.Text &= "<li class='regular'>Regular price <span itemprop=""highPrice"">" & Utility.Common.ViewCurrency(objItemPrice.RegularPrice) & "</span></li>"
                'ltrPrice.Text &= "<li class='save'>You Save <span>-" & Utility.Common.ViewCurrency(objItemPrice.YouSave) & " (" & objItemPrice.PercentSave & "%)</span></li>"
                'ltrPrice.Text &= "<li class='price' itemprop=""lowPrice"">" & Utility.Common.ViewCurrency(objItemPrice.SalePrice) & "</li>"
                ltrPrice.Text &= "<li class='regular'>Regular price <span itemprop=""highPrice"">" & String.Format("{0:0.00}", objItemPrice.RegularPrice) & "</span><span>$</span></li>" ' tach dau $ ra khoi value cho rich snippet
                ltrPrice.Text &= "<li class='save'>You Save <span>-" & Utility.Common.ViewCurrency(objItemPrice.YouSave) & " (" & objItemPrice.PercentSave & "%)</span></li>"
                ltrPrice.Text &= "<li class='price'>$<span itemprop=""lowPrice"">" & String.Format("{0:0.00}", objItemPrice.SalePrice) & "</span></li>"
            Else
                ltrPrice.Text &= "<li class='price'>$<span itemprop=""price"">" & String.Format("{0:0.00}", objItemPrice.NormalPrice) & "</span></li>"
            End If
            ltrPrice.Text &= "<li><meta itemprop=""priceCurrency"" content=""USD"" /></li></ul>"
        End If

        If ltrPrice.Text.Contains("Regular price") Then 'schema
            typeOffer = "AggregateOffer"
        End If

        divPriceBox.Attributes.Item("itemtype") = "http://schema.org/" & typeOffer
        divPoint.InnerHtml = String.Format(Resources.Msg.ItemPoint, pointPurchaseItem, Resources.Msg.PopupCashPoint)
        divHandlingFee.Visible = False
        If Not objItemShowPrice.IsOversize AndAlso objItemShowPrice.Weight >= 8 Then 'Tam thoi hardcode 8 lbs de giam truy xuat db

            Dim handlingFee As Double = StoreItemRow.GetHandlingFee(DB, objItemShowPrice.ItemId, False, True)
            If handlingFee > 0 Then
                ltrHandlingFee.Text = FormatCurrency(handlingFee)
                divHandlingFee.Visible = True
            End If
        End If

        txtQty.Attributes.Add("onblur", "CalculatePoint('" & hidUnitPoint & "','" & hidPricePoint & "')")
        ucPolicy.ItemId = Item.ItemId
    End Sub
End Class

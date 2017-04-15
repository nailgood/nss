Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports Utility

Partial Class controls_checkout_cart
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Public Cart As ShoppingCart = Nothing
    Public firstItemIdError As Integer = 0
    Private DoRecalculate As Boolean = False
    Private CheckedSKUs As String() = {}
    Private CheckedSKUQuantities As Integer() = {}
    Private Errors As Integer() = {}
    Private scic As StoreCartItemCollection
    Protected DoNotUnderline As Boolean = False
    Protected ReWriteURL As RewriteUrl
    Dim OrderTotalFreeSample As Double = CDbl(SysParam.GetValue("FreeSampleOrderMin"))

    Private isShowItemFee As Boolean = False
    Private isRemoveItemPoint As Boolean
    Private isShowErrorBindData As Boolean
    Private DefaultFreeItemIdSelect As Integer = 0
    Public customerPriceGroup As Integer = 0
    Dim lstEstimateShippingAvailable As New List(Of Integer)
    Public PointAvailable As Integer = 0

    Public Sub New(ByVal objCart As ShoppingCart)
        Cart = objCart
    End Sub
    Public Sub New()

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        customerPriceGroup = Utility.Common.GetCurrentCustomerPriceGroupId()
        If Not IsPostBack Then
            If Cart Is Nothing AndAlso Session("CartRender") IsNot Nothing Then
                Cart = Session("CartRender")
            End If

            Dim orderId As Integer = Utility.Common.GetCurrentOrderId()
            If Cart Is Nothing AndAlso orderId > 0 Then
                Cart = New ShoppingCart(DB, orderId, False)
            End If

            If (orderId > 0) Then
                Dim orderNo As String = DB.ExecuteScalar("Select COALESCE (OrderNo,'') from StoreOrder where OrderId=" & orderId)
                If Not String.IsNullOrEmpty(orderNo) Then
                    ShowEmpty()
                    Exit Sub
                End If
            End If

            BindData()

            If Not Request.QueryString("act") Is Nothing Then
                If Request.QueryString("act") = "checkoutcart" Then
                    If firstItemIdError > 0 Then
                        hidFirstItemErrorId.Value = firstItemIdError.ToString
                        hidFirstItemErrorId.ClientIDMode = UI.ClientIDMode.Static
                    Else
                        Dim linkredirect As String = "/store/payment.aspx"
                        If HttpContext.Current.Request.RawUrl.Contains("/store/revise-cart.aspx") Then
                            linkredirect = "/store/payment.aspx"
                        Else
                            If Not HttpContext.Current.Session("isUpdateAddress") Is Nothing Then
                                If HttpContext.Current.Session("isUpdateAddress") = 1 Then
                                    linkredirect = "/store/billing.aspx?type=Shipping"
                                End If
                            End If
                        End If
                        Response.Redirect(linkredirect)
                    End If

                End If
            End If
        End If
    End Sub
    Private Sub ShowEmpty()

        'If Session("LastDepartmentUrl") <> Nothing Then
        '    link = Session("LastDepartmentUrl")
        'Else
        '    link = "/"
        'End If

        'divEmpty.InnerHtml = divEmpty.InnerHtml & "<br/><a href='" & link & "'>Click here to continue shopping</a>"
        divEmpty.Style.Add("display", "block")
    End Sub

    Private Sub BindData()
        If Cart Is Nothing Then
            ShowEmpty()
            rptCartItems.Visible = False
            Exit Sub
        End If

        firstItemIdError = 0
        scic = StoreCartItemRow.GetCartItemsForCart(Cart.Order.OrderId)
        Dim memberID As Integer = Utility.Common.GetCurrentMemberId()
        If memberID > 0 Then
            isCustomerInternational = MemberRow.CheckMemberIsInternational(memberID, Cart.Order.OrderId)
        End If
        If Request.Path.Contains("/store/revise-cart.aspx") Then
            isAllowLinkItem = False
        End If

        isRemoveItemPoint = False
        rptCartItems.DataSource = scic
        rptCartItems.DataBind()
        If rptCartItems.Items.Count > 0 Then
            rptCartItems.Visible = True
        Else
            divEmpty.Style.Add("display", "block")
            rptCartItems.Visible = False
            ShowEmpty()
        End If
    End Sub


    Private isCustomerInternational As Boolean = False
    Private isAllowReviseFree As Boolean
    Private isAllowLinkItem As Boolean = True
    Private isMixmatchDiscountPercent As Boolean
    Private Sub rptCartItems_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rptCartItems.ItemDataBound

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            isMixmatchDiscountPercent = False
            Dim mixMatchId As Integer = 0
            Dim ci As StoreCartItemRow = e.Item.DataItem
            Dim memberId As Integer = Utility.Common.GetMemberIdFromCartCookie()
            Dim Item As StoreItemRow = StoreItemRow.GetRowFromCart(DB, ci.ItemId, memberId, ci.AddType, ci.Quantity)

            Dim sURL As String = URLParameters.ProductUrl(Item.URLCode, Item.ItemId)
            Dim ltrImage As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrImage"), System.Web.UI.WebControls.Literal)
            Dim ltrArrow As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrArrow"), System.Web.UI.WebControls.Literal)
            Dim ltrArrowUpdate As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrArrowUpdate"), System.Web.UI.WebControls.Literal)
            Dim textBoxId As String = "txtQty_" & ci.CartItemId
            If Not ltrArrow Is Nothing Then
                ltrArrow.Text = String.Format("<div class=""plus""><a href=""javascript:void(IncreaseCart({2},'{0}'))"">+</a></div><div class=""min""><a href=""javascript:void(DecreaseCart({2},'{0}'))"">&ndash;</a></div><input type='text' onkeypress=""InputCartQty('{2}','{0}',event);"" id=""{0}"" maxlength=""4"" value=""{1}"" name=""{0}"" />", textBoxId, ci.Quantity, ci.CartItemId)
                ltrArrow.Text &= "<ul class='arrow'><li class='up'><a href=""javascript:void(IncreaseCart(" & ci.CartItemId & ",'" & textBoxId & "'));""><b></b></a></li>"
                ltrArrow.Text &= "<li class='down'><a href=""javascript:void(DecreaseCart(" & ci.CartItemId & ",'" & textBoxId & "'))""><b></b></a></li></ul>"
            End If
            If Not ltrArrowUpdate Is Nothing Then
                ltrArrowUpdate.Text = "<ul class='update-ctr' id='pnUpdate_" & ci.CartItemId & "' style='display:none;'><li class='update'><a href='javascript:void(0)' onclick='return OnChangeQty(" & ci.CartItemId & ")'>Update</a></li>"
                ltrArrowUpdate.Text &= "<li class='cancel'><a  onclick=""return CancelUpdate(" & ci.CartItemId & ",'" & textBoxId & "'," & ci.Quantity & ");"" href='javascript:void(0)'> | Cancel </a></li></ul>"
                If ci.AddType = 2 Then
                    ltrArrowUpdate.Text &= "<ul class=""cases"" style=""max-width: 200px;""><li>Case of " & Item.CaseQty & "</li></ul>"
                End If
            End If
            If Not ltrImage Is Nothing Then
                If String.IsNullOrEmpty(ci.Image) Then
                    ci.Image = "na.jpg"
                End If

                If ci.Type <> Utility.Common.CartItemTypeBuyPoint AndAlso isAllowLinkItem Then
                    ltrImage.Text = "<a href='" & sURL & "'><img src='" & Utility.ConfigData.CDNMediaPath & "/assets/items/featured/" & ci.Image & "'/></a>"
                Else
                    ltrImage.Text = "<a class='adisable' href='javascript:void(0);'><img src='" & Utility.ConfigData.CDNMediaPath & "/assets/items/featured/" & ci.Image & "'/></a>"
                End If

            End If
            Dim hplRemove As System.Web.UI.WebControls.HyperLink = CType(e.Item.FindControl("hplRemove"), System.Web.UI.WebControls.HyperLink)
            If Not hplRemove Is Nothing Then
                If (ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                    hplRemove.Attributes.Add("OnClick", "OnRemoveBuyPointOrder('" & ci.OrderId & "');")
                Else
                    hplRemove.Attributes.Add("OnClick", "OnRemoveCartItem('" & ci.CartItemId & "');")
                End If
            End If


            Dim hplSave As System.Web.UI.WebControls.HyperLink = CType(e.Item.FindControl("hplSave"), System.Web.UI.WebControls.HyperLink)
            If Not hplSave Is Nothing Then
                If ci.IsFreeGift Or ci.IsFreeSample Or ci.IsRewardPoints Then
                    hplSave.Visible = False
                ElseIf memberId > 0 Then
                    hplSave.Attributes.Add("OnClick", "OnSaveCartItem('" & ci.CartItemId & "');")
                Else
                    hplSave.Attributes.Add("OnClick", "OnSaveCartLoginFirst(" & ci.CartItemId & ");")
                End If
            End If

            Dim strItemName As String = ci.ItemName
            Dim strPriceDesc As String = IIf(Item.PriceDesc <> Nothing, " - " & Item.PriceDesc, "")
            Dim measure As String = SitePage.ShowMeasurement(Item.PriceDesc, Item.Measurement)
            strPriceDesc &= IIf(measure.Length > 0, " (" & measure & ")", "")

            If Item.IsActive = False Then
                Item.QtyOnHand = 0
            End If

            Dim divItemName As HtmlGenericControl = CType(e.Item.FindControl("divItemName"), HtmlGenericControl)
            If divItemName IsNot Nothing Then
                If ci.Type <> Utility.Common.CartItemTypeBuyPoint AndAlso isAllowLinkItem Then
                    divItemName.InnerHtml = String.Format("<a href='{0}'>{1} {2}</a>", sURL, Server.HtmlEncode(strItemName), strPriceDesc)
                Else
                    divItemName.InnerHtml = "<a class='adisable' href='javascript:void(0);'>" & Server.HtmlEncode(strItemName) & strPriceDesc & "</a>"
                End If
            End If

            Dim divSKU As HtmlGenericControl = CType(e.Item.FindControl("divSKU"), HtmlGenericControl)
            If divSKU IsNot Nothing Then
                divSKU.InnerHtml = String.Format("<span class=""sku"">Item# {0}", ci.SKU)
            End If

            Dim divSubSKU As HtmlGenericControl = CType(e.Item.FindControl("divSubSKU"), HtmlGenericControl)
            If divSubSKU IsNot Nothing Then
                divSubSKU.InnerHtml = String.Format("#{0}", ci.SKU)
            End If

            Dim tdPrice As HtmlTableCell = CType(e.Item.FindControl("tdPrice"), HtmlTableCell)
            Dim tdTotal As HtmlTableCell = CType(e.Item.FindControl("tdTotal"), HtmlTableCell)
            If tdPrice IsNot Nothing And tdTotal IsNot Nothing Then
                If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
                    tdPrice.InnerHtml = Utility.Common.FormatPointPrice(ci.RewardPoints)
                    tdTotal.InnerHtml = Utility.Common.FormatPointPrice(ci.SubTotalPoint)
                Else
                    If Not ci.IsFreeGift AndAlso (ci.AddType <> 2 AndAlso (Item.Price <> ci.Price Or (ci.LineDiscountAmount > 0 AndAlso ci.CustomerPrice <> Item.LowSalePrice) Or (ci.LineDiscountAmount = 0 And Item.LowSalePrice > 0)) Or
                        (ci.AddType = 2 AndAlso (Item.CasePrice <> ci.Price Or (ci.LineDiscountAmount > 0 AndAlso ci.CustomerPrice <> Item.LowSalePrice) Or (ci.LineDiscountAmount = 0 And Item.LowSalePrice > 0)))) Then
                        Cart.RecalculateCartItem(StoreCartItemRow.GetRow(DB, ci.CartItemId), True)
                    End If

                    tdPrice.InnerHtml = NSS.DisplayCartPricing(DB, ci, True, True)
                    tdTotal.InnerHtml = NSS.DisplayCartPricing(DB, ci, True, False)
                End If
            End If


            Dim divSmallPrice As HtmlGenericControl = CType(e.Item.FindControl("divSmallPrice"), HtmlGenericControl)
            If divSmallPrice IsNot Nothing Then
                divSmallPrice.InnerHtml = tdPrice.InnerHtml
            End If

            Dim divItemError As HtmlGenericControl = CType(e.Item.FindControl("divItemError"), HtmlGenericControl)
            Dim divQty As HtmlGenericControl = CType(e.Item.FindControl("divQty"), HtmlGenericControl)

            Dim dbStoPro As StorePromotionRow = Nothing
            If ci.PromotionID > 0 Then
                dbStoPro = StorePromotionRow.GetRow(DB, ci.PromotionID)
            End If

            If (ci.IsFreeItem AndAlso ci.PromotionID > 0) Then
                ci.MixMatchId = 0
                ci.MixMatchDescription = String.Empty
            End If

            Dim isLastItemMM As Boolean = True
            If ci.MixMatchId = 0 Then
                If ci.PromotionID > 0 Then
                    If dbStoPro.IsProductCoupon Then
                        If Utility.Common.IsPromotionFreeItem(dbStoPro.PromotionType) Then
                            If StorePromotionRow.ValidateProductPromotion(DB, dbStoPro.PromotionCode, memberId, ci.CartItemId) = 1 Then
                                ci.HasFreeItems = True
                                ci.MixMatchId = dbStoPro.MixmatchId
                            Else
                                ci.HasFreeItems = False
                                ci.PromotionID = 0
                            End If
                        End If
                    Else
                        ci.HasFreeItems = False
                    End If
                Else
                    ci.HasFreeItems = False
                End If

            ElseIf e.Item.ItemIndex < scic.Count - 1 AndAlso (ci.MixMatchId = scic(e.Item.ItemIndex + 1).MixMatchId And scic(e.Item.ItemIndex + 1).IsFreeGift = False) Then
                Dim currentMixMatchIsAvailablePurchaseItem As Boolean = DB.ExecuteScalar("select [dbo].[fc_MixMatch_IsAvailablePurchaseItem](" & ci.MixMatchId & "," & ci.ItemId & ")")
                'Dim nextMixMatchIsAvailablePurchaseItem As Boolean = DB.ExecuteScalar("select [dbo].[fc_MixMatch_IsAvailablePurchaseItem](" & scic(e.Item.ItemIndex + 1).MixMatchId & "," & scic(e.Item.ItemIndex + 1).ItemId & ")")
                Dim nextMixMatchIsAvailablePurchaseItem As Boolean = scic(e.Item.ItemIndex + 1).MixMatchId > 0 And Not String.IsNullOrEmpty(scic(e.Item.ItemIndex + 1).FreeItemIds)
                If (currentMixMatchIsAvailablePurchaseItem And nextMixMatchIsAvailablePurchaseItem) Then
                    ci.HasFreeItems = False
                End If
                isLastItemMM = False
            End If

            Dim countFreeItemOfMM As Integer = 0
            If (ci.HasFreeItems AndAlso ci.MixMatchId > 0) Then
                countFreeItemOfMM = MixMatchRow.CountFreeItem(DB, ci.MixMatchId, memberId, ci.OrderId)
                If countFreeItemOfMM < 1 Then
                    ci.HasFreeItems = False
                End If
            End If

            e.Item.FindControl("phdFreeItems").Visible = False
            Dim rowCss As String = "row-item"
            Dim trRow As HtmlTableRow = CType(e.Item.FindControl("trRow"), HtmlTableRow)
            trRow.ID = "trRow_" & ci.CartItemId.ToString
            trRow.ClientIDMode = UI.ClientIDMode.Static
            isAllowReviseFree = False
            If ci.HasFreeItems Then 'And ci.GetFreeItems.Count > 1 Then              
                Dim rptFreeItem As Repeater = e.Item.FindControl("rptFreeItem")
                Dim freeItemList As StoreCartItemCollection = ci.GetFreeItems(DB, ci.OrderId, ci.CartItemId, ci.MixMatchId)
                If freeItemList IsNot Nothing AndAlso freeItemList.Count > 0 Then
                    If isLastItemMM Then
                        ''if has free item , check Qty for free item
                        Dim lstFreeItemInvalid As New StoreItemCollection()
                        lstFreeItemInvalid = MixMatchRow.GetListFreeItemInvalid(ci.OrderId, ci.MixMatchId, memberId, 1)
                        Dim lstSKU As String = String.Empty
                        If (Not lstFreeItemInvalid Is Nothing AndAlso lstFreeItemInvalid.Count > 0) Then
                            lstSKU = ConvertListSKU2String(lstFreeItemInvalid)
                            divItemError.InnerHtml = String.Format(Resources.Alert.QtyFreeItemNotValid, GetQtyOnHand(lstFreeItemInvalid, lstSKU), lstSKU)
                        Else 'Chek permission by brand
                            '[Khoa] Hazardous Material Fee
                            'lstFreeItemInvalid = MixMatchRow.GetListFreeItemInvalid(ci.OrderId, ci.MixMatchId, memberId, 2)
                            'If (Not lstFreeItemInvalid Is Nothing AndAlso lstFreeItemInvalid.Count > 0) Then
                            '    lstSKU = ConvertListSKU2String(lstFreeItemInvalid)
                            '    divItemError.InnerHtml = String.Format(Resources.Alert.PermissonBuyBrandFreeItem, lstSKU)
                            'Else ''check flammable
                            '    If isCustomerInternational = True Then
                            '        lstFreeItemInvalid = MixMatchRow.GetListFreeItemInvalid(ci.OrderId, ci.MixMatchId, memberId, 3)
                            '        If (Not lstFreeItemInvalid Is Nothing AndAlso lstFreeItemInvalid.Count > 0) Then
                            '            lstSKU = ConvertListSKU2String(lstFreeItemInvalid)
                            '            divItemError.InnerHtml = String.Format(Resources.Alert.FlammableFreeItem, lstSKU)
                            '        End If
                            '    End If
                            'End If
                        End If
                    End If

                    rowCss = rowCss & " has-free"
                    e.Item.FindControl("phdFreeItems").Visible = True
                    'Check next item
                    If scic.Count = e.Item.ItemIndex + 1 Then
                        Dim trFreeItem As HtmlTableRow = CType(e.Item.FindControl("phdFreeItems").FindControl("trFreeItem"), HtmlTableRow)
                        trFreeItem.Attributes.Add("class", "free-item free-item-last")
                    End If

                    Dim freeItem As StoreCartItemRow = freeItemList(0)
                    If MixMatchRow.IsDiscountPercent(ci.MixMatchId) Then
                        Dim spTitle As HtmlGenericControl = CType(e.Item.FindControl("spTitle"), HtmlGenericControl)
                        spTitle.InnerText = "Discounted Items"
                        isMixmatchDiscountPercent = True

                        If countFreeItemOfMM > 1 Then
                            Dim ltrLinkChangeFreeItem As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrLinkChangeFreeItem"), System.Web.UI.WebControls.Literal)
                            ltrLinkChangeFreeItem.Text = "<span class='revise-link'><a href='javascript:void(0)' onClick=showDiscountItem(" + ci.MixMatchId.ToString() + "," + ci.OrderId.ToString() + ")>Click here to revise</a></span>"
                        End If
                    Else
                        If (Not String.IsNullOrEmpty(freeItem.FreeItemIds) AndAlso freeItem.FreeItemIds.Contains(",")) Then
                            Dim isDefaultAllItem As Boolean = MixMatchRow.IsDefaultAllFreeItem(ci.MixMatchId) ''CBool(DB.ExecuteScalar("select [dbo].[fc_MixMatch_IsDefaultAllItem](" & p.MixMatchId & ")"))

                            If Not isDefaultAllItem Then

                                DefaultFreeItemIdSelect = DB.ExecuteScalar("Select ItemId from MixMatchLine where MixMatchId=" & ci.MixMatchId & " and Value=100 and IsDefaultSelect=1")
                                If DefaultFreeItemIdSelect > 0 Then
                                    If countFreeItemOfMM > 2 Then
                                        isAllowReviseFree = True
                                    Else
                                        isAllowReviseFree = False
                                    End If
                                Else
                                    If countFreeItemOfMM > 1 Then
                                        isAllowReviseFree = True
                                    End If
                                End If

                                If isAllowReviseFree Then
                                    Dim ltrLinkChangeFreeItem As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrLinkChangeFreeItem"), System.Web.UI.WebControls.Literal)
                                    ltrLinkChangeFreeItem.Text = "<span class='revise-link'><a href='javascript:void(0)' onClick=ChangeFreeItem(" + ci.OrderId.ToString() + "," + ci.MixMatchId.ToString() + "," + ci.CartItemId.ToString() + ")>Click here to revise</a></span>"
                                End If
                            End If

                        End If
                    End If
                    AddHandler rptFreeItem.ItemDataBound, AddressOf rptFreeItem_ItemDataBound
                    rptFreeItem.DataSource = freeItemList
                    rptFreeItem.DataBind()
                End If
            End If

            If ci.IsFreeItem Then
                If Not String.IsNullOrEmpty(ci.FreeItemIds) Then
                    hplRemove.Visible = False
                    divItemName.InnerHtml = "<span class='gift-label'>FREE Item</span><br>" & ci.ItemName
                Else
                    'Free gift item
                    hplRemove.Attributes.Add("OnClick", "OnRemoveCartFreeGift('" & ci.CartItemId & "');")
                    divItemName.InnerHtml = "<span class='gift-label'>FREE Gift</span><br>" & ci.ItemName
                End If

                divQty.InnerHtml = " <span class='text'>QTY: </span>1"
                divQty.Attributes.Add("class", "qty qty-disable")

            ElseIf ci.IsFreeGift Then 'New condition for FreeGift
                hplRemove.Attributes.Add("OnClick", "OnRemoveCartFreeGift('" & ci.CartItemId & "');")
                divItemName.InnerHtml = "<span class='gift-label'>FREE Gift</span><br>" & ci.ItemName

                divQty.InnerHtml = " <span class='text'>QTY: </span>1"
                divQty.Attributes.Add("class", "qty qty-disable")
            End If

            Dim divPromotion As HtmlGenericControl = CType(e.Item.FindControl("divPromotion"), HtmlGenericControl)
            Dim divCoupon As HtmlGenericControl = CType(e.Item.FindControl("divCoupon"), HtmlGenericControl)

            'Check item has mixmatch to show message & link
            If ci.MixMatchId > 0 Then
                Dim arr As String() = MixMatchRow.CheckMixmatchLineType(ci.MixMatchId, ci.ItemId)
                Dim iMixmatchLineType As Common.MixmatchLineType = Common.MixmatchLineType.BuyFreeItem
                Dim iDefaultType As Common.MixmatchDefaulType = Common.MixmatchDefaulType.NoneDefault
                If arr IsNot Nothing Then
                    Try
                        iMixmatchLineType = CType(arr(0), Common.MixmatchLineType)
                        iDefaultType = CType(arr(1), Common.MixmatchDefaulType)
                    Catch ex As Exception
                    End Try
                End If

                If iMixmatchLineType = Common.MixmatchLineType.BuyDiscounted Or iMixmatchLineType = Common.MixmatchLineType.GetDiscounted Then
                    divPromotion.InnerHtml = String.Format("<a class=""maglnk"" href=""javascript:void(0)"" onclick=""ShowBonusOffer({0});"">{1}</a>", ci.MixMatchId, ci.MixMatchDescription)

                    If iMixmatchLineType = Common.MixmatchLineType.BuyDiscounted Then
                        divPromotion.InnerHtml &= String.Format("<span class=""revisediscountpercent""><a href=""javascript:void(0)"" onclick=""showDiscountItem({0},{1})"">Revise your discounted items</a></span>", ci.MixMatchId, ci.OrderId)
                    End If
                ElseIf iMixmatchLineType = Common.MixmatchLineType.BuyFreeItem Or iMixmatchLineType = Common.MixmatchLineType.GetFreeItem Then
                    divPromotion.InnerHtml = "<a class=""maglnk"" href=""javascript:void(0)"" onclick=""ShowBonusOffer(" & ci.MixMatchId & ");"">" & ci.MixMatchDescription & "</a>"

                    If iDefaultType <> Common.MixmatchDefaulType.AllDefault AndAlso iMixmatchLineType = Common.MixmatchLineType.BuyFreeItem AndAlso StoreCartItemRow.ValidateFreeItem(ci.OrderId, ci.MixMatchId) Then
                        divPromotion.InnerHtml &= String.Format("<span class=""revisediscountpercent""><a href=""javascript:void(0)"" onclick=""onClick=ChangeFreeItem({0},{1},{2})"">Revise your free items</a></span>", ci.OrderId, ci.MixMatchId, ci.CartItemId)
                    End If
                End If
                mixMatchId = ci.MixMatchId
            End If


            'Check MM Expire
            If Not String.IsNullOrEmpty(divPromotion.InnerHtml.Trim()) Then
                If BaseShoppingCart.CheckCartMixmatchValid(DB, ci.MixMatchId, customerPriceGroup) = False Then
                    Dim countFreeItem As Integer = DB.ExecuteScalar("Select count(*) from StoreCartItem where OrderId=" & ci.OrderId & " and MixMatchId=" & ci.MixMatchId & " and IsFreeItem=1")
                    If countFreeItem > 0 Then
                        divItemError.InnerHtml = "<div class=""error"">" & Resources.Alert.CartMixmatchNotValid & "</div>"
                        'divPromotion.InnerHtml =
                    End If
                End If
            Else
                divPromotion.Visible = False
            End If

            'Show error
            Dim bOutOfStock As Boolean = False
            If (Item.QtyOnHand <= 0 Or Item.QtyOnHand < ci.Quantity) And (ci.IsFreeItem = False) Then
                If Not ((Utility.Common.IsItemAcceptingOrder(Item.AcceptingOrder) And Item.IsActive = True) OrElse (Item.IsSpecialOrder And Item.IsActive = True) OrElse ci.AttributeSKU = "FREE") Then
                    If Item.QtyOnHand <= 0 Then
                        divItemError.InnerHtml = "Currently Out of Stock."
                        divQty.Visible = False
                        bOutOfStock = True
                    Else
                        divItemError.InnerHtml = "Only " & Item.QtyOnHand & " in stock. Please revise your order quantity."
                    End If
                End If
            ElseIf (Item.PermissionBuyBrand = False) Then
                divItemError.InnerHtml = "Available only in store or by phone order."
                divQty.Visible = False
            Else
                'If isCustomerInternational = True And ci.IsFlammable = True Then
                '    divItemError.InnerHtml = "Item is not available for customer outsite of 48 states within continental USA."
                '    divQty.Visible = False
                'End If
                If (ci.IsRewardPoints) Then
                    If (Item.IsRewardPoints) Then
                        If (Item.RewardPoints <> ci.RewardPoints) Then ''update point cho item
                            Cart.RemoveCartItem(ci.CartItemId)
                            isRemoveItemPoint = True
                        End If
                    Else
                        e.Item.FindControl("tblQty").Visible = False
                        divQty.Visible = False
                    End If
                End If
            End If

            If (ci.IsRewardPoints) Then
                divCoupon.InnerHtml = String.Empty
            Else
                If (ci.IsFreeSample) Then
                    divQty.InnerHtml = " <span class='text'>QTY: </span>1"
                    divQty.Attributes.Add("class", "qty qty-disable")
                Else
                    If Not dbStoPro Is Nothing Then
                        If dbStoPro.IsProductCoupon Then
                            Dim sCouponPrice As String = ""
                            If Utility.Common.IsPromotionFreeItem(dbStoPro.PromotionType) Then
                                If StorePromotionRow.ValidateProductPromotion(DB, dbStoPro.PromotionCode, Session("MemberId"), ci.CartItemId) = 1 Then
                                    divCoupon.InnerHtml = "<span>" & dbStoPro.Message & "</span>"
                                    mixMatchId = dbStoPro.MixmatchId
                                Else
                                    divCoupon.InnerHtml = String.Empty
                                End If
                            Else
                                If dbStoPro.PromotionType = "Monetary" Then
                                    sCouponPrice = "$" & dbStoPro.Discount
                                Else
                                    sCouponPrice = dbStoPro.Discount & "%"
                                End If
                                If StorePromotionRow.ValidateProductPromotion(DB, dbStoPro.PromotionCode, Session("MemberId"), ci.CartItemId) = 1 Then
                                    If ci.PromotionID <> Nothing And ci.PromotionID <> 0 And ci.Price < ci.CouponPrice Then
                                        If dbStoPro.PromotionType = "Monetary" Then
                                            divCoupon.InnerHtml = "<span >Discount Coupon Code - $" & ci.Price.ToString() & " Off" & "</span>" ' & ci.Quantity.ToString() & " = $" & PriceSaleoff.ToString() 
                                        Else
                                            divCoupon.InnerHtml = "<span >Discount Coupon Code - " & sCouponPrice & " Off" & "</span>" ' & ci.Quantity.ToString() & " = $" & PriceSaleoff.ToString() 
                                        End If
                                    Else
                                        divCoupon.InnerHtml = "<span >Discount Coupon Code - " & sCouponPrice & " Off" & "</span>" ' & ci.Quantity.ToString() & " = $" & PriceSaleoff.ToString() 
                                    End If
                                Else
                                    divCoupon.InnerHtml = String.Empty
                                End If
                            End If
                        End If
                    End If

                End If
            End If

            If Item.IsSpecialOrder Then
                divSKU.InnerHtml &= String.Format(" &nbsp;|&nbsp;</span> Availability: Drop Shipping &nbsp;<a class=""dropshippingtip"" id=""aAvailability{0}"" title=""""><img src=""{1}/includes/theme/images/icon-question-1.png""></a><div class=""ShowTipDropShipping"">This item will be shipped directly from the manufacturer.</div>", ci.CartItemId, Utility.ConfigData.CDNMediaPath)
            Else
                divSKU.InnerHtml &= String.Format(" &nbsp;|&nbsp;</span> Availability: In Stock")
            End If

            If ci.IsFlammable Then
                rowCss &= " blockedflammable"

            ElseIf ci.IsHazMat Then
                rowCss &= " flammable"
            End If

            'hidListMixMatchId.Value = hidListMixMatchId.Value & mixMatchId & ";"
            If Not String.IsNullOrEmpty(divItemError.InnerHtml.Trim()) Then
                divItemError.Visible = True
                rowCss &= " error"
                If firstItemIdError < 1 Then
                    firstItemIdError = ci.CartItemId
                End If
            End If

            trRow.Attributes.Add("class", rowCss)
        End If
    End Sub
    Private Function ConvertListSKU2String(ByVal lst As StoreItemCollection) As String
        Dim result As String = String.Empty
        For Each item As StoreItemRow In lst
            If (String.IsNullOrEmpty(result)) Then
                result = item.SKU
            Else
                result = result & "," & item.SKU
            End If
        Next
        Return result
    End Function

    Private Function GetQtyOnHand(ByVal lst As StoreItemCollection, ByVal SKU As String) As Integer
        Dim result As Integer = 0
        For Each item As StoreItemRow In lst
            If item.SKU = SKU Then
                result = item.QtyOnHand
                Exit For
            End If
        Next

        Return result
    End Function

    Private Sub rptFreeItem_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ci As StoreCartItemRow = e.Item.DataItem
            Dim Item As StoreItemRow = StoreItemRow.GetRow(DB, ci.ItemId)
            Dim trFreeItem As HtmlTableRow = CType(e.Item.FindControl("trFreeItem"), HtmlTableRow)
            trFreeItem.ID = "trRow_" & ci.CartItemId.ToString
            trFreeItem.ClientIDMode = UI.ClientIDMode.Static
            If ci.IsFlammable Then
                trFreeItem.Attributes.Add("class", "free-row-item last blockedflammable")
            ElseIf ci.IsHazMat Then
                trFreeItem.Attributes.Add("class", "free-row-item last flammable")
            End If

            Dim itemURLCode As String = Item.URLCode
            Dim sURL As String = URLParameters.ProductUrl(itemURLCode, ci.ItemId)
            Dim tdFreeItemQty As HtmlTableCell = CType(e.Item.FindControl("tdFreeItemQty"), HtmlTableCell)
            Dim divFreeItemName As HtmlGenericControl = CType(e.Item.FindControl("divFreeItemName"), HtmlGenericControl)
            Dim divFreeItemSKU As HtmlGenericControl = CType(e.Item.FindControl("divFreeItemSKU"), HtmlGenericControl)
            Dim divFreeItemSmallQty As HtmlGenericControl = CType(e.Item.FindControl("divFreeItemSmallQty"), HtmlGenericControl)
            Dim divFreeError As HtmlGenericControl = CType(e.Item.FindControl("divFreeError"), HtmlGenericControl)

            tdFreeItemQty.InnerText = ci.Quantity
            divFreeItemSmallQty.InnerHtml = "QTY:" & ci.Quantity
            Dim ltrFreeImage As System.Web.UI.WebControls.Literal = CType(e.Item.FindControl("ltrFreeImage"), System.Web.UI.WebControls.Literal)
            If Not ltrFreeImage Is Nothing Then
                If String.IsNullOrEmpty(ci.Image) Then
                    ci.Image = "na.jpg"
                End If

                If isAllowLinkItem Then
                    ltrFreeImage.Text = "<a href='" & sURL & "'><img src='" & Utility.ConfigData.CDNmediapath & "/assets/items/featured/" & ci.Image & "'/></a>"
                Else
                    ltrFreeImage.Text = "<a class='adisable' href='javascript:void(0)'><img src='" & Utility.ConfigData.CDNmediapath & "/assets/items/featured/" & ci.Image & "'/></a>"
                End If

            End If

            If Not divFreeItemName Is Nothing Then
                If isAllowLinkItem Then
                    divFreeItemName.InnerHtml = "<a href='" & sURL & "'>" & Server.HtmlEncode(ci.ItemName) & "</a>"
                Else
                    divFreeItemName.InnerHtml = "<a class='adisable' href='javascript:void(0)'>" & Server.HtmlEncode(ci.ItemName) & "</a>"
                End If
            End If
            If Not divFreeItemSKU Is Nothing Then
                divFreeItemSKU.InnerHtml = "Item # " & ci.SKU
            End If
            Dim divFreeItemPromotion As HtmlGenericControl = CType(e.Item.FindControl("divFreeItemPromotion"), HtmlGenericControl)
            NSS.FillPricing(DB, ci)

            Dim tdFreeItemPrice As HtmlTableCell = CType(e.Item.FindControl("tdFreeItemPrice"), HtmlTableCell)
            Dim tdFreeItemTotal As HtmlTableCell = CType(e.Item.FindControl("tdFreeItemTotal"), HtmlTableCell)
            Dim divFreeItemSmallPrice As HtmlGenericControl = CType(e.Item.FindControl("divFreeItemSmallPrice"), HtmlGenericControl)

            If (ci.IsRewardPoints And ci.IsRewardPoints > 0) Then
                tdFreeItemPrice.InnerHtml = Utility.Common.FormatPointPrice(ci.RewardPoints)
            Else
                tdFreeItemPrice.InnerHtml &= NSS.DisplayPriceCartItem(ci)
                tdFreeItemTotal.InnerHtml = NSS.DisplayCartPricing(DB, ci, True, False)
            End If

            divFreeItemSmallPrice.InnerHtml = tdFreeItemTotal.InnerHtml
            Dim hplRemove As System.Web.UI.WebControls.HyperLink = CType(e.Item.FindControl("hplRemove"), System.Web.UI.WebControls.HyperLink)
            If isMixmatchDiscountPercent Then
                If Not hplRemove Is Nothing Then
                    hplRemove.Attributes.Add("OnClick", "OnRemoveCartFreeItem('" & ci.CartItemId & "');")
                End If
            Else
                hplRemove.Visible = False
            End If
        End If
    End Sub
    Private Function CreateItemMsg(ByVal itemName As String, ByVal sku As String, ByVal errormsg As String, ByVal cartItemId As Integer, ByRef firstItemErrorId As String) As String
        If lstSKUError.Contains(sku) Then
            Return String.Empty
        End If
        lstSKUError.Add(sku)
        If String.IsNullOrEmpty(firstItemErrorId) Then
            firstItemErrorId = cartItemId
        End If
        Return "Item <b>" & itemName & "</b>, Item#: <b>" & sku & "</b> " & errormsg
    End Function
    Private Function CreateFreeItemMsg(ByVal itemName As String, ByVal sku As String, ByVal errormsg As String, ByVal cartItemId As Integer, ByRef firstItemErrorId As String) As String
        If String.IsNullOrEmpty(firstItemErrorId) Then
            firstItemErrorId = cartItemId
        End If
        Return "Free item <b>" & itemName & "</b>, Item#: <b>" & sku & "</b> " & errormsg
    End Function
    Private Sub MergeError(ByVal errorMsg As String, ByRef result As String)
        result &= IIf(Not String.IsNullOrEmpty(errorMsg), errorMsg & "<br><br>", "")
    End Sub
    Private lstSKUError As List(Of String)
    Public Function ValidateCheckOut() As String()
        ''ClearError()
        Dim lstError As String = String.Empty
        lstSKUError = New List(Of String)
        Dim result As String() = New String(2) {}
        result(0) = String.Empty
        result(1) = String.Empty
        result(2) = String.Empty
        Dim linkredirect As String = String.Empty
        Dim orderId As Integer = Utility.Common.GetCurrentOrderId()
        Dim memberId As Integer = Utility.Common.GetCurrentMemberId()

        If memberId < 1 Then
            HttpContext.Current.Session("checkoutcart") = 1
            linkredirect = "/members/login.aspx?act=checkout"
        ElseIf orderId < 1 Then
            HttpContext.Current.Session("checkoutcart") = 1
            linkredirect = "/members/login.aspx?act=checkout"
            Email.SendError("ToError500", "ValidateCheckOut orderId < 1", "memberId: " & memberId)
        Else
            Dim DB As New Database()
            DB.Open(ConfigurationManager.ConnectionStrings("NSSDB").ConnectionString)
            Dim log As String = String.Empty
            Try
                If Cart Is Nothing Then
                    Cart = New ShoppingCart(DB)
                    log &= "<br>Cart = New ShoppingCart(DB)"
                End If

                Dim o As StoreOrderRow = Cart.Order
                log &= "<br>Dim o As StoreOrderRow = Cart.Order"
                log &= "<br>memberId=" & memberId & " | orderId=" & orderId
                PointAvailable = CashPointRow.GetTotalCashPointByMember(DB, memberId, orderId)
                log &= "<br>PointAvailable"
                Dim isCustomerInternational As Boolean = False
                Dim errorMsg As String = String.Empty
                Dim sci As New StoreCartItemCollection()
                log &= "<br>Dim sci As New StoreCartItemCollection()"

                Dim subtotal As Double = 0
                If o IsNot Nothing Then
                    log &= "<br>o <> Nothing"
                    If o.SubTotal > 0 Then
                        subtotal = o.SubTotal
                    Else
                        If StoreCartItemRow.CountCartRow(DB, o.OrderId) <= 0 Then
                            linkredirect = "/store/cart.aspx"
                            Exit Try
                        Else
                            linkredirect = "/store/cart.aspx"
                            Email.SendError("ToError500", "ValidateCheckOut subtotal=0", "OrderId: " & orderId & "<br>memberId: " & memberId)
                            Exit Try
                        End If
                    End If
                Else
                    log &= "<br>o=Nothing"
                End If

                log &= "<br>subtotal=" & subtotal

                Dim CountItemHazMat As Integer = 0
                Dim CountBlockedItemHazMat As Integer = 0

                log &= "<br>Dim"
                isCustomerInternational = MemberRow.CheckMemberIsInternational(memberId, orderId)
                If isCustomerInternational Then
                    Dim InternationalOrderPriceMin As Double = SysParam.GetValue("InternationalOrderPriceMin")
                    If InternationalOrderPriceMin <> Nothing AndAlso subtotal < InternationalOrderPriceMin Then
                        errorMsg = String.Format(Resources.Alert.OrderMinOutsideUS, InternationalOrderPriceMin, subtotal)
                        MergeError(errorMsg, lstError)
                        'arrId = {} 'Ko can chay vao vong lap for
                    End If
                Else
                    Dim USOrderPriceMin As Double = SysParam.GetValue("USOrderPriceMin")
                    If USOrderPriceMin <> Nothing AndAlso subtotal < USOrderPriceMin Then
                        errorMsg = String.Format(Resources.Alert.OrderMin, USOrderPriceMin, subtotal)
                        MergeError(errorMsg, lstError)
                        'arrId = {} 'Ko can chay vao vong lap for
                    End If
                End If

                log &= "<br>OrderPriceMin"
                If String.IsNullOrEmpty(errorMsg) Then
                    sci = StoreCartItemRow.GetCartItems(DB, orderId)
                End If
                log &= "<br>StoreCartItemRow"

                For Each ci As StoreCartItemRow In sci
                    log &= "<br>ItemName=" & ci.ItemName
                    If (ci.Type = Utility.Common.CartItemTypeBuyPoint) Then
                        Continue For
                    End If

                    Dim item As StoreItemRow = StoreItemRow.GetRowFromCart(DB, ci.ItemId, memberId, ci.AddType, ci.Quantity)
                    log &= "<br>StoreItemRow=" & item.ItemName

                    Dim bNeedUpdate As Boolean = False
                    If ci.IsHazMat <> item.IsHazMat Then
                        ci.IsHazMat = item.IsHazMat
                        bNeedUpdate = True
                    End If

                    If ci.IsFlammable <> item.IsFlammable Then
                        ci.IsFlammable = item.IsFlammable
                        bNeedUpdate = True
                    End If

                    log &= "<br>bNeedUpdate"
                    If Not bNeedUpdate AndAlso (ci.IsFreeItem And Not ci.FreeItemIds Is Nothing) Then
                        Continue For
                    End If

                    If item.IsActive = False Then
                        item.QtyOnHand = 0
                    End If

                    If item.IsHazMat Then CountItemHazMat += 1
                    If item.IsFlammable Then CountBlockedItemHazMat += 1
                    Dim totalCartQty As Integer = DB.ExecuteScalar("Select dbo.[fc_StoreCartItem_SumQuantityByItemId](" & ci.OrderId & "," & ci.ItemId & ")")
                    log &= "<br>totalCartQty"
                    If (item.QtyOnHand <= 0 Or item.QtyOnHand < totalCartQty) Then
                        If ((Utility.Common.IsItemAcceptingOrder(item.AcceptingOrder) And item.IsActive = True) OrElse (item.IsSpecialOrder And item.IsActive = True) OrElse ci.AttributeSKU = "FREE") Then
                            '' Continue For
                        Else
                            If item.QtyOnHand <= 0 Then
                                errorMsg = CreateItemMsg(ci.ItemName, ci.SKU, "is currently out of stock", ci.CartItemId, result(1))
                            Else
                                errorMsg = CreateItemMsg(ci.ItemName, ci.SKU, "is only " & item.QtyOnHand & " in stock. Please revise your order quantity.", ci.CartItemId, result(1))
                            End If
                            MergeError(errorMsg, lstError)
                        End If
                    End If
                    log &= "<br>item.QtyOnHand"

                    If ci.IsFreeGift = False Then
                        Dim isPermissionByBrand As Boolean = DB.ExecuteScalar("Select dbo.fc_CheckPermissionBuyBrand(" & memberId & ", " & item.BrandId & ")")
                        If isPermissionByBrand = False Then
                            errorMsg = CreateItemMsg(ci.ItemName, ci.SKU, "is available only in store or by phone order.", ci.CartItemId, result(1))
                            MergeError(errorMsg, lstError)
                        End If
                    End If
                    log &= "<br>ci.IsFreeGift = False"

                    If (ci.MixMatchId > 0) Then
                        'Check xem item nay da bao msg error chua, neu chua moi bao tiep msg ve qty,flammable cua free item
                        If Not lstSKUError.Contains(ci.SKU) Then
                            Dim lstFreeItemInvalid As New StoreItemCollection()
                            Dim lstSKU As String = String.Empty
                            lstFreeItemInvalid = MixMatchRow.GetListFreeItemInvalid(ci.OrderId, ci.MixMatchId, memberId, 1)
                            If (Not lstFreeItemInvalid Is Nothing AndAlso lstFreeItemInvalid.Count > 0) Then
                                lstSKU = ConvertListSKU2String(lstFreeItemInvalid)
                                errorMsg = String.Format(Resources.Alert.QtyFreeItemNotValid, item.QtyOnHand, lstSKU)
                                MergeError(errorMsg, lstError)
                                If String.IsNullOrEmpty(result(1)) Then
                                    result(1) = ci.CartItemId
                                End If
                            Else
                                'Chek permission by brand
                                'lstFreeItemInvalid = MixMatchRow.GetListFreeItemInvalid(ci.OrderId, ci.MixMatchId, memberId, 2)
                                'If (Not lstFreeItemInvalid Is Nothing AndAlso lstFreeItemInvalid.Count > 0) Then
                                '    lstSKU = ConvertListSKU2String(lstFreeItemInvalid)
                                '    errorMsg = String.Format(Resources.Alert.PermissonBuyBrandFreeItem, lstSKU)
                                '    MergeError(errorMsg, lstError)
                                '    If String.IsNullOrEmpty(result(1)) Then
                                '        result(1) = ci.CartItemId
                                '    End If

                                'Check flammable
                                '[Khoa]: Hazardous Material Fee

                                If isCustomerInternational = True Then
                                    lstFreeItemInvalid = MixMatchRow.GetListFreeItemInvalid(ci.OrderId, ci.MixMatchId, memberId, 3)
                                    If (Not lstFreeItemInvalid Is Nothing AndAlso lstFreeItemInvalid.Count > 0) Then
                                        'lstSKU = ConvertListSKU2String(lstFreeItemInvalid)
                                        'errorMsg = String.Format(Resources.Alert.FlammableFreeItem, lstSKU)
                                        'MergeError(errorMsg, lstError)
                                        'If String.IsNullOrEmpty(result(1)) Then
                                        '    result(1) = ci.CartItemId
                                        'End If
                                        CountBlockedItemHazMat += 1
                                        'End If
                                    End If
                                End If
                            End If
                            lstSKUError.Add(ci.SKU)
                        End If
                    End If
                    log &= "<br>ci.MixMatchId > 0"

                    '[Khoa] Hazardous Material Fee
                    'If isCustomerInternational = True And item.IsFlammable = True Then
                    'errorMsg = CreateItemMsg(ci.ItemName, ci.SKU, "is not available for customer outsite of 48 states within continental USA. Please remove them before proceeding to check out.", ci.CartItemId, result(1))
                    'MergeError(errorMsg, lstError)
                    'End If

                    If (ci.IsRewardPoints) Then
                        If (item.IsRewardPoints) Then
                            If (item.RewardPoints <> ci.RewardPoints) Then 'Update point cho item
                                Cart.RemoveCartItem(ci.CartItemId)
                                DB.Close()
                                linkredirect = "/store/cart.aspx"
                                Exit Try
                            End If
                        Else
                            errorMsg = CreateItemMsg(ci.ItemName, item.SKU, "is not allow by w/Point. Please remove it from your shopping cart and proceed to check out.", ci.CartItemId, result(1))
                            MergeError(errorMsg, lstError)
                        End If
                    ElseIf ci.IsFreeGift Then
                        If ci.Total > 0 Then
                            Email.SendError("ToError500", "ValidateCheckOut > FreeGift has fee", "OrderId: " & orderId.ToString() & SitePage.GetSessionList())
                            ci.Total = 0
                            ci.SubTotal = ci.Price * ci.Quantity
                            ci.LineDiscountAmount = ci.Price * ci.Quantity
                            ci.Update()
                            Continue For
                        ElseIf ci.SubTotal = 0 Then 'Khoa:tam thoi update lai data
                            ci.Total = 0
                            ci.SubTotal = ci.Price * ci.Quantity
                            ci.LineDiscountAmount = ci.Price * ci.Quantity
                            ci.Update()
                        End If
                    ElseIf bNeedUpdate Then
                        ci.Update()
                    End If

                    Dim Others As Integer = DB.ExecuteScalar("SELECT [dbo].[fc_StoreCartItem_GetOtherQuantity](" & ci.OrderId & "," & ci.CartItemId & "," & ci.ItemId & ")")
                    If item.MaximumQuantity > 0 AndAlso item.MaximumQuantity < ci.Quantity Then
                        errorMsg = CreateItemMsg(item.ItemName, ci.SKU, "has a maximum purchase quantity of " & item.MaximumQuantity & " per order", ci.CartItemId, result(1))
                        MergeError(errorMsg, lstError)
                    ElseIf ci.IsFreeGift = False Then
                        If Utility.Common.IsItemAcceptingOrder(item.AcceptingOrder) OrElse item.IsSpecialOrder OrElse ci.AttributeSKU = "FREE" Then
                            'Khoa dong code lai, vi thay ko can thiet update
                            'ValidateUpdateQtyCart(sp.Cart, DB, ci, qty)
                        Else
                            If item.QtyOnHand - Others >= ci.Quantity Then
                                If (ci.IsRewardPoints) Then
                                    If (item.IsRewardPoints) Then
                                        If (item.RewardPoints <> ci.RewardPoints) Then 'Update point cho item
                                            Cart.RemoveCartItem(ci.CartItemId)
                                        End If
                                    Else
                                        Cart.RemoveCartItem(ci.CartItemId)
                                    End If
                                    'Else
                                    '    If ci.Quantity <> qty Then
                                    '        ValidateUpdateQtyCart(sp.Cart, DB, ci, qty)
                                    '    End If
                                End If
                            Else
                                errorMsg = CreateItemMsg(item.ItemName, ci.SKU, "have " & item.QtyOnHand & " units in stock  and there are already " & ci.Quantity + Others & " in your shopping cart.", ci.CartItemId, result(1))
                                MergeError(errorMsg, lstError)
                            End If
                        End If

                    End If
                Next

                If Not String.IsNullOrEmpty(lstError) Then
                    result(0) = lstError
                    DB.Close()
                    Return result
                End If


                If Cart.CheckTotalFreeSample(orderId) Then
                    Cart.RemoveFreeSample()
                End If
                Dim MinFreeGiftLevelInvalid As Double = ShoppingCart.GetMinFreeGiftLevelInValid(orderId)
                If MinFreeGiftLevelInvalid > 0 Then
                    Cart.RemoveFreeGift()
                End If
                Cart.RecalculateOrderDetail("Check_QuantityShoppingCart")

                'Dim objCart As ShoppingCart = New ShoppingCart(DB, orderId, False)
                Dim customerPriceGroup As Integer = Utility.Common.GetCurrentCustomerPriceGroupId()
                Cart.DeleteCartMixMatchNotValid(DB, orderId, customerPriceGroup)
                Utility.Common.DeleteCachePopupCart(orderId)

                If CountBlockedItemHazMat > 0 AndAlso o.ShipToCountry <> "US" Then
                    'result(0) = Resources.Alert.CartItemBlockedHazMat
                    linkredirect = "/store/billingint.aspx?type=Shipping"
                ElseIf CountItemHazMat > 0 AndAlso (Not Cart.HasCountryHazMat(o.ShipToCountry) Or Cart.CheckShippingSpecialUS()) Then
                    'result(0) = Resources.Alert.CountryBlockedHazMat
                    linkredirect = "/store/billingint.aspx?type=Shipping"
                Else
                    linkredirect = "/store/payment.aspx"
                    If HttpContext.Current.Request.RawUrl.Contains("/store/revise-cart.aspx") Then
                        linkredirect = "/store/payment.aspx"
                    Else
                        If Not HttpContext.Current.Session("isUpdateAddress") Is Nothing Then
                            If HttpContext.Current.Session("isUpdateAddress") = 1 Then
                                If isCustomerInternational Then
                                    linkredirect = "/store/billingint.aspx?type=Shipping"
                                Else
                                    linkredirect = "/store/billing.aspx?type=Shipping"
                                End If

                            End If
                        End If
                    End If
                End If

                Utility.CacheUtils.RemoveCache("StoreCartItem_ItemCount_" & orderId)
                DB.Close()
            Catch ex As Exception
                If Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
                result(0) = ex.Message
                Email.SendError("ToError500", "Cart.ascx > ValidateCheckOut", "OrderId: " & orderId & "<br>MemberId: " & memberId & "<br>Exception: " & ex.ToString() & "<br><br>" & log)
            End Try
        End If

        Utility.Common.OrderLog(orderId, "Secure Checkout Now", Nothing)
        result(2) = linkredirect
        Return result
    End Function
    Private Sub ValidateUpdateQtyCart(ByVal objCart As ShoppingCart, ByVal objDB As Database, ByVal ci As StoreCartItemRow, ByVal qtyUpdate As Integer)

        If (ci.IsRewardPoints And ci.RewardPoints > 0) Then
            StoreCartItemRow.UpdateCartQty(objDB, ci.CartItemId, ci.OrderId, qtyUpdate, True)
        Else
            StoreCartItemRow.UpdateCartQty(objDB, ci.CartItemId, ci.OrderId, qtyUpdate, False)
        End If
        objCart.RecalculateCartItem(ci, True)

    End Sub
End Class

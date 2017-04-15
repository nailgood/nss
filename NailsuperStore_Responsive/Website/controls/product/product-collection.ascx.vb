Imports Components
Imports DataLayer
Partial Class controls_product_product_collection
    Inherits BaseControl

    Protected LastDepartment As String
    Private itemPrice As String
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
    Public Sub Fill(ByVal CountDiv As Integer, ByVal StoreItem As StoreItemRow, ByVal CurrentQtyAddCardID As String, ByVal CurrentQtyAddCardValue As Integer, ByVal isInternational As Boolean)
        Me.CountDiv = CountDiv
        Fill(StoreItem, CurrentQtyAddCardID, CurrentQtyAddCardValue, isInternational)
    End Sub

    Public Sub Fill(ByVal StoreItem As StoreItemRow, ByVal CurrentQtyAddCardID As String, ByVal CurrentQtyAddCardValue As Integer, ByVal isInternational As Boolean)
        Dim si As New StoreItemRow
        si = storeitem
        Dim objecPrice As Object = BaseShoppingCart.GetItemPrice(DB, si, 1, 0, 0, False)
        itemID = si.ItemId
        ItemName = IIf(String.IsNullOrEmpty(si.ItemNameNew), si.ItemName, si.ItemNameNew)
        'If Not Request.RawUrl.Contains("/store/search-result.aspx") AndAlso Session("LastDepartment") IsNot Nothing Then
        'If String.IsNullOrEmpty(LastDepartment) Then
        '    LastDepartment = StoreItemRow.GetDefaultDepartmentNameByItemId(si.ItemId)
        'End If
        Image = si.Image
        'isIncart = si.IsInCart
        Try
            isIncart = DB.ExecuteScalar("SELECT  dbo.fc_StoreCartItem_CheckItemInCart(" & Utility.Common.GetOrderIdFromCartCookie() & ", " & si.ItemId & "," & IIf(si.IsRewardPoints, 1, 0) & ")")
        Catch ex As Exception
            isIncart = False
        End Try
        Dim sURL As String = URLParameters.ProductUrl(si.URLCode, si.ItemId)
        'aItem2.InnerText = si.ItemName2
        'aItem2.HRef = sURL
        If CountDiv = 0 Then
            CountDiv = si.itemIndex
        End If
        PromotionText = si.MixMatchDescription 'NSS.ShowPromotionText(Item) '
        If si.MixMatchId > 0 Then
            PromotionText = "<a onclick=showBonosOffer(" & si.MixMatchId & ") href='javascript:void(0)' class='maglnk'>" & PromotionText & "</a>"
        End If
        itemPrice = StoreItem.ReturnPrice(objecPrice)
        lblPrice.Text = itemPrice
        If si.youSave > 0 And itemPrice.Contains("As low as") = False Then
            ltsave.Text = "<li class=""save"">You Save: <span class=""yousave"">" & FormatCurrency(si.youSave) & "</span> (<span class=""savepercent"">" & si.savePercent & "</span>%)</li>"
        Else
            ltsave.Visible = False
        End If
        'icon new, hot, bestseller
        'Dim strImg As String = "<img src=""/includes/theme/images/{0}"" alt=""New!""/>&nbsp;"
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
        If si.IsFreeSample Or si.IsFreeGift >= 2 Then
            divCart.Visible = False
            lblPrice.Visible = False
            ltsave.Visible = False
        ElseIf si.PermissionBuyBrand = False AndAlso Utility.Common.GetCurrentMemberId > 0 Then
            lblPrice.Text = "<span class=""red"">Available only in store or by phone order</span>"
            ' lblPrice.CssClass = "red"
            divCart.Visible = False
            ltrQty.Text = String.Empty
            ltsave.Text = String.Empty
        ElseIf si.IsFlammable = True And isInternational Then
            divCart.Visible = False
            ltrQty.Text = String.Empty
            lblPrice.Text = "<span class=""red"">This item is not available for customer outside of 48 states within continental USA.</span>"
            ltsave.Visible = False
        Else
            If si.IsInCart Then
                divCart.Visible = False
            End If
            If si.QtyOnHand <= 0 And Not Utility.Common.IsItemAcceptingOrder(si.AcceptingOrder) And Not si.IsSpecialOrder Then
                divCart.Text = String.Format("<div id=""divCart"" class=""red"">Currently Out of Stock <br><a href=""#"" onclick=""NotifyInStock('{0}');"">Notify me when in stock</a></div>", si.ItemId)
                divCart.Visible = True
                lblPrice.Text = itemPrice
                ltrQty.Text = String.Empty
            Else
                ltrQty.Text = String.Format("<div class=""qty""><div class=""plus""><a href=""javascript:void(Increase('txtQtyItem{0}'))"">+</a></div><div class=""min""><a href=""javascript:void(Decrease('txtQtyItem{0}',0))"">&ndash;</a></div><div class=""pull-left""><input type=""tel"" class=""txt-qty"" name=""txtQtyItem{0}"" id=""txtQtyItem{0}"" value=""0"" onkeypress=""return numbersonly();"" maxlength=""4"" /></div>" & _
                                                  "<div class=""bao-arrow""><ul id=""c-qty""><a href=""javascript:void(Increase('txtQtyItem{0}'))""><li><b class=""arrow-up arrow-down-act"" id=""imgIntxtQty{0}""></b></li></a>" & _
                                                  "<a href=""javascript:void(Decrease('txtQtyItem{0}',0))""><li class=""bd-qty""><b class=""arrow-down arrow-down-act"" id=""imgDetxtQty{0}""></b></li></a></ul></div></div>", ItemID)
                divCart.Visible = False
            End If
        End If
    End Sub
End Class

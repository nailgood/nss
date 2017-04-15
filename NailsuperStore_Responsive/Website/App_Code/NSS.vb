Imports Microsoft.VisualBasic
Imports DataLayer
Imports Components

Public Class NSS
    Public ReadOnly DefaultShippingId As Integer = Utility.Common.DefaultShippingId
    Public ReadOnly TruckShippingId As Integer = Utility.Common.TruckShippingId
    Public ReadOnly NonExpeditedShippingIds As String = Utility.Common.NonExpeditedShippingIds
    Public ReadOnly PickupShippingId As Integer = Utility.Common.PickupShippingId
	Public Const LoginToViewPrices As String = "Please <a href=""/members/login.aspx"" class=""maglnk"">login</a> to see prices."

    Public Shared Sub FillPricing(ByVal DB As Database, ByRef o As Object)
        ShoppingCart.FillPricing(DB, o, False, Utility.Common.SalesPriceType.Item)
    End Sub

	Public Shared Function DisplayPricing(ByVal DB As Database, ByVal si As StoreItemRow, ByVal IsVertical As Boolean) As String
		Return DisplayPricing(DB, si, IsVertical, 1, 0)
    End Function
    Public Shared Function DisplayRelatedListPricing(ByVal DB As Database, ByVal si As StoreItemRow, ByVal IsVertical As Boolean, ByVal memberID As Integer) As String
        Return DisplayRelatedListPricing(DB, si, IsVertical, 1, 0, memberID)
    End Function
    Public Shared Function DisplayPriceCartItem(ByVal ci As StoreCartItemRow) As String
        Dim result As String = String.Empty
        If ci.IsFreeItem = True Then
            If Not ci.FreeItemIds Is Nothing Then
                result = "<div class='regular'>" & FormatCurrency(ci.Price) & "</div><div class='current'>" & FormatCurrency(ci.CustomerPrice) & "</div>"

            Else
                ''ltr.Text = "<div class='free'>FREE Gift</div>"
            End If

        Else
            If (ci.Price <> ci.CustomerPrice) Then
                result = "<div class='regular'>" & FormatCurrency(ci.Price) & "</div><div class='current'>" & FormatCurrency(ci.CustomerPrice) & "</div>"
            Else
                result = "<div>" & FormatCurrency(ci.Price) & "</div>"
            End If
        End If
        Return result
    End Function
    Public Shared Function DisplayTotalCartItem(ByVal ci As StoreCartItemRow) As String
        Dim result As String = String.Empty
        If ci.IsFreeItem = True Then
            If Not ci.FreeItemIds Is Nothing Then
                result = "<div class='regular'>" & FormatCurrency(ci.SubTotal) & "</div><div class='current'>" & FormatCurrency(ci.Total) & "</div>"

            Else
                ''ltr.Text = "<div class='free'>FREE Gift</div>"
                If (ci.SubTotal <> ci.Total) Then
                    result = "<div class='regular'>" & FormatCurrency(ci.SubTotal) & "</div><div class='current'>" & FormatCurrency(ci.Total) & "</div>"
                Else
                    result = "<div >" & FormatCurrency(ci.SubTotal) & "</div>"
                End If
            End If

        Else
            If (ci.SubTotal <> ci.Total) Then
                result = "<div class='regular'>" & FormatCurrency(ci.SubTotal) & "</div><div class='current'>" & FormatCurrency(ci.Total) & "</div>"
            Else
                result = "<div>" & FormatCurrency(ci.SubTotal) & "</div>"
            End If
        End If
        Return result
    End Function
    Public Shared Function DisplayCartPricing(ByVal DB As Database, ByVal ci As StoreCartItemRow, ByVal IsVertical As Boolean, ByVal IsSingleItem As Boolean) As String
        Dim s As String
        Dim LowestPrice, LowestPriceNoPromo As Double

        LowestPrice = ci.GetLowestPrice()
        LowestPriceNoPromo = ci.GetLowestPrice(False)
        ''vuphuong add: 30/01/2010
        If Not (ci.Promotion Is Nothing) Then
            If ci.LowSalePrice > 0 And ci.LowSalePrice < LowestPrice Then
                LowestPrice = ci.LowSalePrice
            End If
        End If
        '''''''''''''''''''''''''''
        If ci.IsFreeItem AndAlso (ci.SubTotal = 0 Or ci.PromotionID > 0) Then ''

            Dim tmp As String = StoreItemRow.GetCustomerDiscount(ci.ItemId, System.Web.HttpContext.Current.Session("MemberId"))
            If IsNumeric(tmp) Then LowestPrice = FormatNumber(tmp, 2)
            If LowestPrice > 0 Then
                s = "<div class=""regular"">" & FormatCurrency(IIf(IsSingleItem, LowestPrice, LowestPrice * ci.Quantity)) & "</div>"
            Else
                s = "<div class=""regular"">" & FormatCurrency(IIf(IsSingleItem, ci.Price, ci.Price * ci.Quantity)) & "</div>"
            End If
            s &= "<div class=""current"">" & FormatCurrency(0) & "</div>"
        Else
            If IsSingleItem Then
                If CDbl(ci.Price) > CDbl(LowestPrice) Then
                    s = "<div class=""regular"">" & FormatCurrency(ci.Price) & "</div>"
                    s &= "<div class=""current"">"
                    If ci.PromotionIsLowestPrice AndAlso ci.DiscountQuantity > 0 AndAlso ci.DiscountQuantity <> ci.Quantity Then
                        s &= FormatCurrency(ci.PromotionPrice) & "</span> x " & ci.DiscountQuantity
                        Dim usespan As Boolean = IIf(FormatNumber(ci.Price, 2) > FormatNumber(LowestPriceNoPromo, 2), True, False)
                        s &= IIf(usespan, "<div class=""current"">", "") & FormatCurrency(LowestPriceNoPromo) & IIf(usespan, "</div>", "") & " x " & ci.Quantity - ci.DiscountQuantity
                    Else
                        s &= FormatCurrency(LowestPrice) & "</div>"
                    End If
                Else
                    s = FormatCurrency(ci.Price)
                End If
            Else

                If FormatNumber(ci.Price, 2) > FormatNumber(LowestPrice, 2) Or ci.Total < ci.SubTotal Then
                    If ci.IsFreeItem Then
                        s = "<div class=""regular"">" & FormatCurrency(ci.SubTotal) & "</div>"
                        s &= "<div class=""current"">" & FormatCurrency(ci.Total) & "</div>"
                    Else
                        s = "<div class=""regular"">" & FormatCurrency(ci.Price * ci.Quantity) & "</div>"
                        s &= "<div class=""current"">" & FormatCurrency(ci.Total) & "</div>"
                    End If

                    
                Else
                    s = FormatCurrency(ci.Total)
                End If
            End If
        End If

        Return s
    End Function

    Public Shared Function CheckSalePrice(ByVal PriceText As String) As Boolean
        Return (PriceText.ToLower().Contains("as low as") OrElse PriceText.ToLower().Contains(" - "))
    End Function
  
	Public Shared Function DisplayPricing(ByVal DB As Database, ByVal si As StoreItemRow, ByVal IsVertical As Boolean, ByVal Quantity As Integer, ByVal LineDiscountAmount As Double) As String
		If si Is Nothing Then Return ""

		If si.Pricing Is Nothing Then FillPricing(DB, si)
		If si.Pricing Is Nothing Then Return LoginToViewPrices

		Dim s, stmp As String
		Dim Prefix As String = ""
		Dim LowestPrice As Double = si.LowPrice
		Dim tmpPrice As Double = Nothing

		Dim dt As DataTable = si.Pricing.PPU

		If Not dt Is Nothing Then
			If dt.Rows.Count > 1 Then
                If HttpContext.Current.Request.Path.ToLower = "/store/item.aspx" OrElse HttpContext.Current.Request.Path.ToLower = "/includes/ajax.aspx" Then
                    s = "<div class="""">" & vbCrLf & _
                     "<table cellspacing=""0"" cellpadding=""0"" border=""0"" class=""lnpadbt3"">" & vbCrLf & _
                     "<tr>" & vbCrLf

                    For i As Integer = 0 To dt.Rows.Count - 1
                        Dim qty As Integer = dt.Rows(i)("minimumquantity")
                        s &= "<td class=""ppuhead"">"
                        If i = dt.Rows.Count - 1 Then
                            s &= qty & "+"
                        Else
                            s &= qty & "-" & dt.Rows(i + 1)("minimumquantity") - 1
                        End If
                        s &= "</td>"
                    Next

                    s &= "</tr>" & vbCrLf

                    For i As Integer = 0 To dt.Rows.Count - 1
                        s &= "<td class=""pputd"">" & FormatCurrency(dt.Rows(i)("unitprice")) & "</td>"
                    Next

                    s &= "</table></div></div>" & vbCrLf
                ElseIf System.Web.HttpContext.Current.Request.Path.ToLower = "/store/shop-save.aspx" OrElse System.Web.HttpContext.Current.Request.Path.ToLower = "/home.aspx" OrElse System.Web.HttpContext.Current.Request.Path.ToLower = "/store/default.aspx" OrElse System.Web.HttpContext.Current.Request.Path.ToLower = "/store/sub-category.aspx" OrElse System.Web.HttpContext.Current.Request.Path.ToLower = "/store/main-category.aspx" OrElse System.Web.HttpContext.Current.Request.Path.ToLower = "/store/category.aspx" _
                        OrElse System.Web.HttpContext.Current.Request.Path.ToLower = "/store/recently-viewed.aspx" OrElse System.Web.HttpContext.Current.Request.Path.ToLower = "/store/search-result.aspx" Then 'OrElse System.Web.HttpContext.Current.Request.Path.ToLower = "/store/dealday.aspx"
                    s = "<div class=""mag"" style=""font-weight:normal;"">As low as <strong>" & FormatCurrency(dt.Rows(dt.Rows.Count - 1)("unitprice")) & "</strong></div>"

                Else
                    's = FormatCurrency(dt.Rows(0)("unitprice"))
                    'vuphuong add 28/12/2009
                    s = FormatCurrency(dt.Rows(0)("unitprice") * Quantity)
                    '''''''''''''''''''''''
                End If

				Return s
			Else
				Prefix = "<b>Our Price:</b> "
			End If
		End If

		If si.Pricing.IsRangedPricing Then
			If si.LowPrice <> si.HighPrice Then
				If si.Pricing.LowSellPrice < si.LowPrice Then
					Return Prefix & "<span class=""strike"" style=""font-weight:normal"">" & FormatCurrency(si.Pricing.LowPrice) & " - " & FormatCurrency(si.Pricing.HighPrice) & "</span><br /><span class=""red bold"">" & FormatCurrency(si.Pricing.LowSellPrice) & " - " & FormatCurrency(si.Pricing.HighPrice) & "</span>"
				Else
					Return Prefix & FormatCurrency(si.Pricing.LowPrice) & " - " & FormatCurrency(si.Pricing.HighPrice)
				End If
			Else
				Return Prefix & FormatCurrency(si.LowPrice)
			End If
		End If

		If (si.LowPrice <> si.LowSalePrice AndAlso si.LowSalePrice <> Nothing) OrElse LineDiscountAmount > 0 Then
            s = "<span class=""strike"" style=""font-weight:bold"">" & FormatCurrency(si.Pricing.BasePrice * Quantity) & "</span>" & IIf(IsVertical, "<br />", " ") & "<span class=""red bold"">" & FormatCurrency(si.Pricing.SellPrice * Quantity) & "</span>"
			LowestPrice = si.LowSalePrice
		Else
			s = FormatCurrency((si.LowPrice - LineDiscountAmount) * Quantity)
		End If

		If si.MixMatchId <> Nothing Then
			Dim Promotion As PromotionRow
			If Not si.Promotion Is Nothing Then
                Promotion = si.Promotion
			Else
				Promotion = PromotionRow.GetRow(DB, si.MixMatchId, False)
			End If

			If Promotion.Type = PromotionType.LineSpecific Then
				Dim dv As DataView = New DataView
				dv.Table = Promotion.GetItems.Table.Copy
				dv.RowFilter = "ItemId = " & si.ItemId

				stmp = String.Empty

				If dv.Count <> 0 AndAlso Promotion.LinesToTrigger = Promotion.[Optional] Then
					stmp &= "<span class=""strike"">" & FormatCurrency(si.LowPrice * Quantity) & "</span>" & IIf(IsVertical, "<br />", " ") & "<span class=""red bold"">"
					If Not IsDBNull(dv(0)("SetPrice")) Then
						tmpPrice = dv(0)("SetPrice")
						stmp &= FormatCurrency(tmpPrice * Quantity - LineDiscountAmount)
					ElseIf Not IsDBNull(dv(0)("PercentOff")) Then
						tmpPrice = si.LowPrice * (1 - (dv(0)("PercentOff") / 100))
						stmp &= FormatCurrency(tmpPrice * Quantity - LineDiscountAmount)
					End If
					stmp &= "</span>"
					If tmpPrice < LowestPrice Then
						LowestPrice = tmpPrice
						s = stmp
					End If
				End If
			ElseIf Promotion.LinesToTrigger = Promotion.Optional Then
				LowestPrice = si.LowPrice
				If LineDiscountAmount > 0 Then
                    s = "<span class=""strike"">" & FormatCurrency(LowestPrice * Quantity) & "</span>" & IIf(IsVertical, "<br />", " ") & "<span class=""red bold"">" & FormatCurrency(LowestPrice * Quantity - LineDiscountAmount) & "</span>"
				Else
					s = FormatCurrency(LowestPrice * Quantity)
				End If
			End If
		End If

		Return Prefix & s
    End Function
    Public Shared Function DisplayListPricing(ByVal DB As Database, ByVal si As StoreItemRow, ByVal IsVertical As Boolean, ByVal Quantity As Integer, ByVal LineDiscountAmount As Double, ByVal memberId As Integer) As String
        If si Is Nothing Then Return ""

        If si.Pricing Is Nothing Then FillPricing(DB, si)
        If si.Pricing Is Nothing Then Return LoginToViewPrices

        Dim s, stmp As String
        Dim Prefix As String = ""
        Dim LowestPrice As Double = si.LowPrice
        Dim tmpPrice As Double = Nothing

        Dim dt As DataTable = si.Pricing.PPU

        If Not dt Is Nothing Then
            If dt.Rows.Count > 1 Then
                If (StoreItemRow.IsItemMultiPrice(si.ItemId, memberId)) Then
                    s = "<div class=""mag"" style=""font-weight:normal;"">As low as <strong>" & FormatCurrency(dt.Rows(dt.Rows.Count - 1)("unitprice")) & "</strong></div>"
                Else
                    s = FormatCurrency(dt.Rows(0)("unitprice") * Quantity)
                End If
                Return s
            Else
                Prefix = "<b>Our Price:</b> "
            End If
        End If

        If si.Pricing.IsRangedPricing Then
            If si.LowPrice <> si.HighPrice Then
                If si.Pricing.LowSellPrice < si.LowPrice Then
                    Return Prefix & "<span class=""strike"" style=""font-weight:normal"">" & FormatCurrency(si.Pricing.LowPrice) & " - " & FormatCurrency(si.Pricing.HighPrice) & "</span><br /><span class=""red bold"">" & FormatCurrency(si.Pricing.LowSellPrice) & " - " & FormatCurrency(si.Pricing.HighPrice) & "</span>"
                Else
                    Return Prefix & FormatCurrency(si.Pricing.LowPrice) & " - " & FormatCurrency(si.Pricing.HighPrice)
                End If
            Else
                Return Prefix & FormatCurrency(si.LowPrice)
            End If
        End If

        If (si.LowPrice <> si.LowSalePrice AndAlso si.LowSalePrice <> Nothing) OrElse LineDiscountAmount > 0 Then
            s = "<span class=""strike"" style=""font-weight:bold"">" & FormatCurrency(si.Pricing.BasePrice * Quantity) & "</span>" & IIf(IsVertical, "<br />", " ") & "<span class=""red bold"">" & FormatCurrency(si.Pricing.SellPrice * Quantity) & "</span>"
            LowestPrice = si.LowSalePrice
        Else
            s = FormatCurrency((si.LowPrice - LineDiscountAmount) * Quantity)
        End If

        If si.MixMatchId <> Nothing Then
            Dim Promotion As PromotionRow
            If Not si.Promotion Is Nothing Then
                Promotion = si.Promotion
            Else
                Promotion = PromotionRow.GetRow(DB, si.MixMatchId, False)
            End If

            If Promotion.Type = PromotionType.LineSpecific Then
                Dim dv As DataView = New DataView
                dv.Table = Promotion.GetItems.Table.Copy
                dv.RowFilter = "ItemId = " & si.ItemId

                stmp = String.Empty

                If dv.Count <> 0 AndAlso Promotion.LinesToTrigger = Promotion.[Optional] Then
                    stmp &= "<span class=""strike"">" & FormatCurrency(si.LowPrice * Quantity) & "</span>" & IIf(IsVertical, "<br />", " ") & "<span class=""red bold"">"
                    If Not IsDBNull(dv(0)("SetPrice")) Then
                        tmpPrice = dv(0)("SetPrice")
                        stmp &= FormatCurrency(tmpPrice * Quantity - LineDiscountAmount)
                    ElseIf Not IsDBNull(dv(0)("PercentOff")) Then
                        tmpPrice = si.LowPrice * (1 - (dv(0)("PercentOff") / 100))
                        stmp &= FormatCurrency(tmpPrice * Quantity - LineDiscountAmount)
                    End If
                    stmp &= "</span>"
                    If tmpPrice < LowestPrice Then
                        LowestPrice = tmpPrice
                        s = stmp
                    End If
                End If
            ElseIf Promotion.LinesToTrigger = Promotion.Optional Then
                LowestPrice = si.LowPrice
                If LineDiscountAmount > 0 Then
                    s = "<span class=""strike"">" & FormatCurrency(LowestPrice * Quantity) & "</span>" & IIf(IsVertical, "<br />", " ") & "<span class=""red bold"">" & FormatCurrency(LowestPrice * Quantity - LineDiscountAmount) & "</span>"
                Else
                    s = FormatCurrency(LowestPrice * Quantity)
                End If
            End If
        End If

        Return Prefix & s
    End Function
    Public Shared Function DisplayRelatedListPricing(ByVal DB As Database, ByVal si As StoreItemRow, ByVal IsVertical As Boolean, ByVal Quantity As Integer, ByVal LineDiscountAmount As Double, ByVal memberId As Integer) As String
        If si Is Nothing Then Return ""

        If si.Pricing Is Nothing Then FillPricing(DB, si)
        If si.Pricing Is Nothing Then Return LoginToViewPrices

        Dim s, stmp As String
        Dim Prefix As String = ""
        Dim LowestPrice As Double = si.LowPrice
        Dim tmpPrice As Double = Nothing

        Dim dt As DataTable = si.Pricing.PPU

        If Not dt Is Nothing Then
            If dt.Rows.Count > 1 Then
                If (StoreItemRow.IsItemMultiPrice(si.ItemId, memberId)) Then
                    s = "<div class=""mag"" style=""font-weight:normal;"">As low as <strong>" & FormatCurrency(dt.Rows(dt.Rows.Count - 1)("unitprice")) & "</strong></div>"
                Else
                    s = FormatCurrency(dt.Rows(0)("unitprice") * Quantity)
                End If
                Return s
            Else
                Prefix = "<b>Our Price:</b> "
            End If
        End If

        If si.Pricing.IsRangedPricing Then
            If si.LowPrice <> si.HighPrice Then
                If si.Pricing.LowSellPrice < si.LowPrice Then
                    Return Prefix & "<span class=""strike pputd2"">" & FormatCurrency(si.Pricing.LowPrice) & " - " & FormatCurrency(si.Pricing.HighPrice) & "</span><br /><span class=""red bold"">" & FormatCurrency(si.Pricing.LowSellPrice) & " - " & FormatCurrency(si.Pricing.HighPrice) & "</span>"
                Else
                    Return Prefix & FormatCurrency(si.Pricing.LowPrice) & " - " & FormatCurrency(si.Pricing.HighPrice)
                End If
            Else
                Return Prefix & FormatCurrency(si.LowPrice)
            End If
        End If

        If (si.LowPrice <> si.LowSalePrice AndAlso si.LowSalePrice <> Nothing) OrElse LineDiscountAmount > 0 Then
            s = "<span class=""strike pputd2"">" & FormatCurrency(si.Pricing.BasePrice * Quantity) & "</span>" & IIf(IsVertical, "<br />", " ") & "<span class=""red bold"">" & FormatCurrency(si.Pricing.SellPrice * Quantity) & "</span>"
            LowestPrice = si.LowSalePrice
        Else
            s = "<span class=""pputd2"" style=""font-weight:bold"">" & FormatCurrency((si.LowPrice - LineDiscountAmount) * Quantity) & "</span>"
        End If

        If si.MixMatchId <> Nothing Then
            Dim Promotion As PromotionRow
            If Not si.Promotion Is Nothing Then
                Promotion = si.Promotion
            Else
                Promotion = PromotionRow.GetRow(DB, si.MixMatchId, False)
            End If

            If Promotion.Type = PromotionType.LineSpecific Then
                Dim dv As DataView = New DataView
                dv.Table = Promotion.GetItems.Table.Copy
                dv.RowFilter = "ItemId = " & si.ItemId

                stmp = String.Empty

                If dv.Count <> 0 AndAlso Promotion.LinesToTrigger = Promotion.[Optional] Then
                    stmp &= "<span class=""strike"">" & FormatCurrency(si.LowPrice * Quantity) & "</span>" & IIf(IsVertical, "<br />", " ") & "<span class=""red bold"">"
                    If Not IsDBNull(dv(0)("SetPrice")) Then
                        tmpPrice = dv(0)("SetPrice")
                        stmp &= FormatCurrency(tmpPrice * Quantity - LineDiscountAmount)
                    ElseIf Not IsDBNull(dv(0)("PercentOff")) Then
                        tmpPrice = si.LowPrice * (1 - (dv(0)("PercentOff") / 100))
                        stmp &= FormatCurrency(tmpPrice * Quantity - LineDiscountAmount)
                    End If
                    stmp &= "</span>"
                    If tmpPrice < LowestPrice Then
                        LowestPrice = tmpPrice
                        s = stmp
                    End If
                End If
            ElseIf Promotion.LinesToTrigger = Promotion.Optional Then
                LowestPrice = si.LowPrice
                If LineDiscountAmount > 0 Then
                    s = "<span class=""strike"">" & FormatCurrency(LowestPrice * Quantity) & "</span>" & IIf(IsVertical, "<br />", " ") & "<span class=""red bold"">" & FormatCurrency(LowestPrice * Quantity - LineDiscountAmount) & "</span>"
                Else
                    s = FormatCurrency(LowestPrice * Quantity)
                End If
            End If
        End If

        Return Prefix & s
    End Function

    Public Shared Function ShowPromotionDetails(ByVal o As Object, ByVal memberId As Integer, ByVal orderId As Integer) As String
        If Not (TypeOf o Is StoreItemRow OrElse TypeOf o Is StoreCartItemRow) OrElse o.Promotion Is Nothing Then Return ""
        Dim s As String = String.Empty
        Dim stmp As String = String.Empty
        Dim Promotion As PromotionRow
        Promotion = o.Promotion
        stmp = String.Empty
        Dim objMixmatch As MixMatchRow = MixMatchRow.GetRow(o.DB, Promotion.MixMatchId)
        If objMixmatch.Type = Utility.Common.MixmatchType.Normal Then
            Dim isInternationalCustomer As Boolean = MemberRow.CheckMemberIsInternational(memberId, orderId)
            Dim countItemFreeAvailable As Integer = MixMatchRow.CountFreeItem(o.DB, objMixmatch.Id, memberId, isInternationalCustomer)
            If countItemFreeAvailable < 1 Then
                Return String.Empty
            End If
            Dim countItemPurchaseAvailable As Integer = MixMatchRow.CountPurchaseItem(o.DB, objMixmatch.Id, memberId, isInternationalCustomer)
            If countItemPurchaseAvailable < 1 Then
                Return String.Empty
            End If
            Return "<a class=""maglnk""  href=""javascript:void(0)"" onclick=""ShowBonusOffer(" & Promotion.MixMatchId & ");"">" & objMixmatch.Description & "</a>"
        End If
        Return String.Empty
    End Function

    Public Shared Function ShowPromotionText(ByVal o As Object) As String
        If Not (TypeOf o Is StoreItemRow OrElse TypeOf o Is StoreCartItemRow) OrElse o.Promotion Is Nothing Then Return ""

        Dim Promotion As PromotionRow
        Promotion = o.Promotion

        Return MixMatchRow.GetRow(o.DB, Promotion.MixMatchId).Description
    End Function

    Public Shared Function GetCartPricing(ByVal DB As Database, ByVal ci As StoreCartItemRow, ByVal IsSingleItem As Boolean) As String
        Dim s As String = ""
        Dim LowestPrice, LowestPriceNoPromo As Double

        LowestPrice = ci.GetLowestPrice()
        LowestPriceNoPromo = ci.GetLowestPrice(False)
        If Not (ci.Promotion Is Nothing) Then
            If ci.LowSalePrice > 0 And ci.LowSalePrice < LowestPrice Then
                LowestPrice = ci.LowSalePrice
            End If
        End If

        If ci.IsFreeItem Then
            s = "0"
        Else
            If IsSingleItem Then
                If CDbl(ci.Price) > CDbl(LowestPrice) Then
                    If ci.PromotionIsLowestPrice AndAlso ci.DiscountQuantity > 0 AndAlso ci.DiscountQuantity <> ci.Quantity Then
                        s = LowestPriceNoPromo
                    Else
                        s = LowestPrice
                    End If
                Else
                    s = ci.Price
                End If
            Else
                If FormatNumber(ci.Price, 2) > FormatNumber(LowestPrice, 2) Then
                    s = ci.Total
                Else
                    s = ci.Total
                End If
            End If
        End If

        Return s
    End Function
End Class
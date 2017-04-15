Imports Components
Imports DataLayer

Partial Class quickorder_default
	Inherits SitePage

	Private RowCount As Integer = 20
	Private HiddenCount As Integer = 10
    Private si As StoreItemRow
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("MemberId") = Nothing Then
            ViewedItemRow.updateSearchResult(String.Empty, "/store/quickorder.aspx", "Login")
            Response.Redirect("/members/login.aspx?fromQuickOrderPage=1")
        End If

        Dim dt As New DataTable
        dt.Columns.Add("fake")

        For i As Integer = 0 To RowCount - 1
            dt.Rows.Add(New TableRow)
        Next

        If Not IsPostBack Then
            rpt.DataSource = dt
            rpt.DataBind()
        Else
            Dim HasAtt As Boolean = False
            If hdnMore.Value <> "" Then
                litJS.Text = "<script language=""javascript"" type=""text/javascript""><!--" & vbCrLf & "document.getElementById('tblHidden').style.display='block';document.getElementById('aMore').style.display='none';" & vbCrLf & "//--></script>"
            End If
            Dim bError As Boolean = False
            Dim AddedOne As Boolean = False

            If (CheckBeforeAdd(si)) Then

                DB.BeginTransaction()
                For Each i As RepeaterItem In rpt.Items
                    Dim txt As TextBox = CType(i.FindControl("txtitem"), TextBox)
                    Dim qty As TextBox = CType(i.FindControl("txtqty"), TextBox)
                    Dim sku As String = Trim(txt.Text)
                    'Dim si As StoreItemRow
                    If sku <> String.Empty Then
                        si = StoreItemRow.GetRow(DB, sku)
                        If si.ItemId <> Nothing Then
                            If IsNumeric(qty.Text) Then
                                Dim lit As Literal = i.FindControl("lit")
                                If si.GetCusionColors = String.Empty AndAlso si.GetBaseColors = String.Empty AndAlso si.GetLaminateTrims = String.Empty Then
                                    lit.Text = ""
                                    Dim att As Repeater = i.FindControl("rpt")
                                    Dim ac As StoreAttributeCollection = StoreAttributeRow.GetRowsByItem(DB, si.ItemId)
                                    If Cart Is Nothing Then
                                        Cart = New ShoppingCart(DB, 0, False)
                                    End If
                                    'If ViewState("ProcessedAttributes") Is Nothing OrElse Array.IndexOf(ViewState("ProcessedAttributes").ToString.Split(","), si.ItemId.ToString) = -1 Then
                                    '    AddHandler att.ItemDataBound, AddressOf att_ItemDataBound
                                    '    att.DataSource = ac
                                    '    att.DataBind()
                                    '    ViewState("ProcessedAttributes") &= IIf(ViewState("ProcessedAttributes") = Nothing, "", ",") & si.ItemId.ToString
                                    '    If ac.Count > 0 Then
                                    '        HasAtt = True
                                    '        bError = True
                                    '        'txt.ReadOnly = True
                                    '    Else
                                    '        'Add to cart
                                    '        Try
                                    '            AddToCart(si, qty, att, bError, AddedOne)
                                    '        Catch ex As Exception
                                    '            AddError(ErrHandler.ErrorText(ex))
                                    '            bError = True
                                    '            Exit For
                                    '        End Try
                                    '    End If
                                    'Else
                                    'Add to cart
                                    Try
                                        AddToCart(si, qty, att, bError, AddedOne)
                                    Catch ex As Exception
                                        Email.SendError("ToError500", "Catalog quick order-AddToCart", Request.RawUrl & "-MemberId=" & Session("MemberId") & "<br>:" & ex.ToString())
                                        AddError(ErrHandler.ErrorText(ex))
                                        bError = True
                                        Exit For
                                    End Try
                                    'End If
                                Else
                                    lit.Text = "This item cannot be added to your cart from the quick order page. Please <a href=""/store/item.aspx?ItemId=" & si.ItemId & """ class=""maglnk"">add it to your cart here</a>. To skip adding this item and continue adding the others, click the ""Add To Cart"" button."
                                    If ViewState("IgnoredItems") Is Nothing OrElse Array.IndexOf(ViewState("IgnoredItems").ToString.Split(","), si.ItemId.ToString) = -1 Then
                                        bError = True
                                        ViewState("IgnoredItems") &= IIf(ViewState("IgnoredItems") = Nothing, "", ",") & si.ItemId.ToString
                                    End If
                                End If
                            Else
                                AddError("Please enter a valid quantity for part number " & sku)
                                bError = True
                            End If
                        ElseIf sku <> String.Empty Then
                            AddError("Could not find part number " & sku)
                            bError = True
                        End If
                    End If
                Next

                If Not bError AndAlso AddedOne Then
                    Cart.RecalculateOrderDetail("quickorder.Page_Load")
                    DB.CommitTransaction()
                    Response.Redirect("/store/cart.aspx")
                Else
                    DB.RollbackTransaction()
                End If

                If HasAtt Then
                    litMsg.Visible = True
                Else
                    litMsg.Visible = False
                End If
            End If
        End If
    End Sub

    Private Function CheckBeforeAdd(ByRef si As StoreItemRow) As Boolean
        Dim flag As Boolean = True

        For Each i As RepeaterItem In rpt.Items
            Dim txt As TextBox = CType(i.FindControl("txtitem"), TextBox)
            Dim qty As TextBox = CType(i.FindControl("txtqty"), TextBox)
            Dim dvItem As HtmlGenericControl = CType(i.FindControl("dvItem"), HtmlGenericControl)
            Dim dvQty As HtmlGenericControl = CType(i.FindControl("dvQty"), HtmlGenericControl)
            Dim spclose As HtmlGenericControl = CType(i.FindControl("spclose"), HtmlGenericControl)
            Dim sku As String = Trim(txt.Text)
            Dim flagItem As Boolean = True
            'reset highligh
            dvItem.Attributes("class") = ""
            dvQty.Attributes("class") = ""
            spclose.Attributes("class") = ""

            'Dim si As StoreItemRow
            If sku <> String.Empty Then

                If (String.IsNullOrEmpty(qty.Text)) Then
                    qty.Text = 0
                End If
                'Check Quantity
                If Not IsNumeric(qty.Text) OrElse CInt(qty.Text) < 1 OrElse qty.Text = "" Then
                    AddError("Please enter a valid quantity (Item# " & sku & ")")
                    SetHighlight(dvItem, dvQty, spclose)
                    flag = False
                    'Exit For
                End If

                si = StoreItemRow.GetRow(DB, sku)
                If si.AcceptingOrder = 2 Then
                    si.QtyOnHand = 9999
                End If
                'check item FreeSample
                If si.IsFreeSample = True Then
                    AddError("The item #" & sku & " is free sample that is not permitted to buy. Please select a different item.")
                    SetHighlight(dvItem, dvQty, spclose)
                    flag = False
                    'Exit For
                End If
                'check item Free Gift not add cart
                If si.IsFreeGift = 2 Then
                    AddError("The item #" & sku & " is free gift that is not permitted to buy. Please select a different item.")
                    SetHighlight(dvItem, dvQty, spclose)
                    flag = False
                    'Exit For
                End If
                'Check SKU valid
                If si.ItemId <> Nothing And si.IsActive = True Then
                    Dim memberId As Integer = IIf(Session("MemberId") = Nothing, 0, Convert.ToInt32(Session("MemberId")))

                    'Check Member in list Item Enable
                    Dim sie As New StoreItemEnable()
                    Dim dt As DataTable = sie.ListBrands(memberId)
                    If Not IsDBNull(dt) AndAlso dt.Rows.Count > 0 Then
                        Dim brands As String = dt.Rows(0)("Brands").ToString()
                        Dim memberBrands As String = dt.Rows(0)("MemberBrands").ToString()

                        If Not memberBrands.Contains("," & si.BrandId & ",") Then
                            If brands.Contains("," & si.BrandId & ",") Then
                                AddError("Due to OPI restrictions, all OPI products are now available only in store or by phone order (Item# " & sku & ").")
                                SetHighlight(dvItem, dvQty, spclose)
                                flagItem = False
                                flag = False
                            End If
                        End If

                        dt.Dispose()
                    End If
                    Dim member As MemberRow = MemberRow.GetRow(memberId)
                    If member.IsInternational = True And si.IsFlammable = True Then
                        AddError("The item " & sku & " is not available for customer outsite of 48 states within continental USA. Please remove them for continuing to checkout")
                        SetHighlight(dvItem, dvQty, spclose)
                        flag = False
                    End If
                    'Check total in Cart
                    Dim TotalInCart As Integer
                    TotalInCart = DB.ExecuteScalar("select coalesce(sum(Quantity),0) from StoreCartItem where OrderId = " & DB.Number(Session("OrderId")) & " and ItemId = " & si.ItemId)

                    If si.QtyOnHand < 1 Then
                        AddError("There are no more units for this item (Item# " & sku & ")")
                        SetHighlight(dvItem, dvQty, spclose)
                        flag = False
                    ElseIf flagItem = True And TotalInCart + CInt(qty.Text) > si.QtyOnHand Then
                        AddError("We only have " & si.QtyOnHand & " units in stock for this item (Item# " & sku & ")" & IIf(TotalInCart > 0, ", and there are already " & TotalInCart & " in your <a href=""/store/cart.aspx"">shopping cart</a>", "") & ".")
                        qty.Text = si.QtyOnHand - TotalInCart
                        SetHighlight(dvItem, dvQty, spclose)
                        flag = False
                    End If
                Else
                    AddError("Could not find part number (Item# " & sku & ")")
                    SetHighlight(dvItem, dvQty, spclose)
                    flag = False
                End If
            End If


        Next

        Return flag
    End Function
    Private Sub SetHighlight(ByRef dvItem As HtmlGenericControl, ByRef dvQty As HtmlGenericControl, ByRef spclose As HtmlGenericControl)
        dvItem.Attributes("class") = "has-error has-feedback"
        dvQty.Attributes("class") = "has-error has-feedback"
        spclose.Attributes("class") = "glyphicon glyphicon-remove form-control-feedback"
    End Sub
    Private Sub AddToCart(ByVal si As StoreItemRow, ByRef qty As TextBox, ByRef att As Repeater, ByRef bError As Boolean, ByRef AddedOne As Boolean)
        Dim Quantity As Integer = qty.Text
        'Dim TotalInCart As Integer

        'If Not IsNumeric(qty.Text) OrElse CInt(qty.Text) < 1 Then
        '    AddError("Please enter a valid quantity")
        '    Exit Sub
        'End If

        'TotalInCart = DB.ExecuteScalar("select coalesce(sum(Quantity),0) from StoreCartItem where OrderId = " & DB.Number(Session("OrderId")) & " and ItemId = " & si.ItemId)
        'If TotalInCart + CInt(qty.Text) > si.QtyOnHand Then
        '    AddError("We only have " & si.QtyOnHand & " units in stock for this item" & IIf(TotalInCart > 0, ", and there are already " & TotalInCart & " in your <a href=""/store/cart.aspx"">shopping cart</a>", "") & ".")
        '    qty.Text = si.QtyOnHand - TotalInCart
        '    Exit Sub
        'End If

        'Dim Id As Integer
        ' Dim AttributeSKU As String = ""
        'Dim Attributes As String = String.Empty
        'If si.Pricing Is Nothing Then NSS.FillPricing(DB, si)
        'If si.Pricing Is Nothing Then Response.Redirect("/500.aspx")
        'Dim OriginalPrice As Double = si.Pricing.BasePrice
        ' Dim ExtraPrice As Double = 0
        'Dim ddl As DropDownList
        'Dim swatches As String = String.Empty

        'For Each j As RepeaterItem In att.Items
        '    If j.ItemIndex = 0 Then If si.Prefix <> Nothing Then AttributeSKU = si.Prefix
        '    ddl = CType(j.FindControl("ddlAttribute"), DropDownList)
        '    Id = CType(j.FindControl("lblId"), Label).Text
        '    Dim att1 As StoreAttributeRow = StoreAttributeRow.GetRow(DB, Id)
        '    ExtraPrice += Regex.Split(att1.Price, vbCrLf)(ddl.SelectedIndex)
        '    AttributeSKU &= Regex.Split(att1.SKU, vbCrLf)(ddl.SelectedIndex)
        '    Attributes &= IIf(Attributes <> String.Empty, vbCrLf, "") & att1.Name & ": " & Regex.Split(att1.Value, vbCrLf)(ddl.SelectedIndex)
        'Next

        Cart.Add2Cart(si.ItemId, Nothing, Quantity, Nothing, "Myself", "", "", 0, "", False, True, Nothing)

        AddedOne = True
    End Sub

	Private Sub att_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
		If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
			Dim att As StoreAttributeRow = e.Item.DataItem
			Dim ddl As DropDownList = CType(e.Item.FindControl("ddlAttribute"), DropDownList)
			Dim values As String() = Regex.Split(att.Value, vbCrLf)
			Dim prices As String() = Regex.Split(att.Price, vbCrLf)
			Dim prc As Double, s As String, li As ListItem

			For i As Integer = 0 To UBound(values)
				s = values(i)
				prc = Nothing

				If HasAccess() Then
					If UBound(prices) >= i Then
						If IsNumeric(prices(i)) Then
							If CDbl(prices(i)) <> 0 Then s &= " [add " & FormatCurrency(prices(i)) & "]"
							prc = prices(i)
						End If
					End If
				End If

				li = New ListItem(s, prc & "|" & i)
				If Trim(values(i)) <> String.Empty Then ddl.Items.Add(li)
			Next
		End If
	End Sub

	Private Sub rpt_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles rpt.ItemDataBound
		If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
			Dim trOpen As Literal = e.Item.FindControl("trOpen")
			Dim trClose As Literal = e.Item.FindControl("trClose")

			If e.Item.ItemIndex = HiddenCount Then
                trOpen.Text = "<tr><td colspan=""3""><div style=""text-align:left"" onclick=""this.style.display='none';document.getElementById('hdnMore').value='Y';document.getElementById('tblHidden').style.display='block';return false;""><a class=""more"" href=""#"" id=""aMore"">Add More Items <b class=""glyphicon arrow-right""></b></a></div><table border=""0"" cellspacing=""0"" cellpadding=""0"" id=""tblHidden"" style=""width:150px;display:none;""><tr>"
                trClose.Text = "</tr>"
            ElseIf e.Item.ItemIndex = RowCount - 1 Then
                trOpen.Text = "<tr>"
                trClose.Text = "</tr></table></td></tr>"
			Else
				'If e.Item.ItemIndex Mod 2 = 0 Then
				'	trOpen.Text = "<tr>"
				'Else
				'	trClose.Text = "</tr>"
				'End If
                trOpen.Text = "<tr>"
				trClose.Text = "</tr>"
			End If
		End If
	End Sub
End Class

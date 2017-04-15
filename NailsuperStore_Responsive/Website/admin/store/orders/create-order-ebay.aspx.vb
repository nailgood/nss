Imports Components
Imports DataLayer
Partial Class admin_store_orders_create_order_ebay
    Inherits SitePage
    Private RowCount As Integer = 20
    Private HiddenCount As Integer = 10
    Private MemberId As Integer = 0
    Private OrderId As String = 0
    Protected c As baseShoppingCart
    Protected o As StoreOrderRow
    Private Cart1 As ShoppingCart
    Private Tax As Double = 0
    Private Shipping As Double = 0
    'Protected Cart As ShoppingCart
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'If Session("adminMemberId") = Nothing Then Response.Redirect("/members/")
        'tblOrder.Visible = False
        'dtl.Visible = False
        Tax = CDbl(IIf(IsNumeric(txtTax.Text), txtTax.Text, 0))
        Shipping = CDbl(IIf(IsNumeric(txtShippingCost.Text), txtShippingCost.Text, 0))
        Dim dt As New DataTable
        dt.Columns.Add("fake")

        If Cart1 Is Nothing Then
            Cart1 = New ShoppingCart(DB, Session("adminOrderId"), True)
        End If

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

            If (CheckBeforeAdd()) And Session("adminOrderId") <> Nothing Then

                DB.BeginTransaction()
                For Each i As RepeaterItem In rpt.Items
                    Dim txt As TextBox = CType(i.FindControl("txtitem"), TextBox)
                    Dim qty As TextBox = CType(i.FindControl("txtqty"), TextBox)
                    Dim pri As TextBox = CType(i.FindControl("txtPrice"), TextBox)
                    Dim sku As String = Trim(txt.Text)
                    Dim si As StoreItemRow
                    If sku <> String.Empty Then
                        si = StoreItemRow.GetRow(DB, sku)
                        If si.ItemId <> Nothing Then
                            If IsNumeric(qty.Text) Then
                                Dim lit As Literal = i.FindControl("lit")
                                If si.GetCusionColors = String.Empty AndAlso si.GetBaseColors = String.Empty AndAlso si.GetLaminateTrims = String.Empty Then
                                    lit.Text = ""
                                    Dim att As Repeater = i.FindControl("rpt")
                                    Dim ac As StoreAttributeCollection = StoreAttributeRow.GetRowsByItem(DB, si.ItemId)
                                    If ViewState("ProcessedAttributes") Is Nothing OrElse Array.IndexOf(ViewState("ProcessedAttributes").ToString.Split(","), si.ItemId.ToString) = -1 Then
                                        AddHandler att.ItemDataBound, AddressOf att_ItemDataBound
                                        att.DataSource = ac
                                        att.DataBind()
                                        ViewState("ProcessedAttributes") &= IIf(ViewState("ProcessedAttributes") = Nothing, "", ",") & si.ItemId.ToString
                                        If ac.Count > 0 Then
                                            HasAtt = True
                                            bError = True
                                            'txt.ReadOnly = True
                                        Else
                                            'Add to cart
                                            Try
                                                AddToCart(si, qty, CDbl(pri.Text), att, bError, AddedOne)
                                            Catch ex As Exception
                                                AddError(ErrHandler.ErrorText(ex))
                                                bError = True
                                                Exit For
                                            End Try
                                        End If
                                    Else
                                        'Add to cart
                                        Try
                                            AddToCart(si, qty, CDbl(pri.Text), att, bError, AddedOne)
                                        Catch ex As Exception
                                            AddError(ErrHandler.ErrorText(ex))
                                            bError = True
                                            Exit For
                                        End Try
                                    End If
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
                    'If Cart Is Nothing Then

                    Cart1 = New ShoppingCart(DB, Session("adminOrderId"), True)
                    o = Cart1.Order
                    Cart1.AdminRecalculateOrderDetail() 'RecalculateOrderDetail()
                    CalculateOrder(o)
                    DB.CommitTransaction()
                    dtl.Order = o
                    'c = New ShoppingCart(DB, Session("adminOrderId"), True)
                    'litOrderNo.Text = c.Order.OrderNo
                    dtl.Cart = Cart1
                    tblOrder.Visible = True
                    F_Email.Text = ""
                    F_UserName.Text = ""
                    'Response.Redirect("/store/cart.aspx")
                Else
                    DB.RollbackTransaction()
                End If

                If HasAtt Then
                    litMsg.Visible = True
                Else
                    litMsg.Visible = False
                End If
                Session("adminOrderId") = Nothing
                Session("adminMemberId") = Nothing
            End If
        End If

    End Sub

    Private Sub CalculateOrder(ByVal Order As StoreOrderRow)
        Order.OrderNo = GetMaxOrderEbay()
        Order.OrderDate = Now.Date
        'o.SellToCustomerId = Member.CustomerId
        Order.DoExport = True
        'Dim Shipping As Double = CDbl(IIf(IsNumeric(txtShippingCost.Text), txtShippingCost.Text, 0))
        Order.ProcessDate = Now()
        Order.Shipping = Shipping
        DB.ExecuteSQL("Update StoreCartItem Set subtotal = " & Shipping & ", Total = " & Shipping & " Where Type = 'carrier' and OrderId = " & Order.OrderId)
        Order.ProcessSessionID = Session.SessionID
        Order.PaypalStatus = 2
        Order.Notes = "Ebay Order Create From Admin<br/>" & drEbayShippingType.SelectedItem.Text.ToString()
        Order.CarrierType = Utility.Common.USPSPriorityShippingId

        Order.Shipping = Shipping
        Order.ResidentialFee = 0
        Order.SignatureConfirmation = 0
        Order.Tax = Tax
        Order.PaymentType = "PAYPAL"
        DB.ExecuteSQL("Update StoreCartItem Set Total = " & Shipping & ", SubTotal = " & Shipping & " Where OrderId = " & Order.OrderId & " and Type = 'carrier'")
        DB.ExecuteSQL("Update StoreCartItem Set CarrierType = " & Utility.Common.USPSPriorityShippingId & " Where OrderId = " & Order.OrderId)
        Dim TotalItem As Double = DB.ExecuteScalar("Select isnull(Sum(Price * Quantity),0) From StoreCartItem Where OrderId = " & Order.OrderId & " and Type = 'item'")
        Order.Total = TotalItem + Shipping + Order.Tax
        Order.SubTotal = TotalItem
        Order.BaseSubTotal = TotalItem

        Order.Update()
        Dim CountCarrier As Integer = DB.ExecuteScalar("Select Count(*) From StoreCartItem Where Type = 'carrier' and OrderId = " & Order.OrderId)
        If CountCarrier > 1 Then
            DB.ExecuteSQL("delete StoreCartItem where Type = 'carrier' and OrderId = " & Order.OrderId & " and CartItemId in (select MAX(cartitemid) from StoreCartItem where OrderId = " & Order.OrderId & " and Type = 'carrier')")
        End If
    End Sub
    Private Function GetMaxOrderEbay() As String
        Dim MaxOrder As String = String.Format("E{0:0000000}", (CInt(DB.ExecuteScalar("Select max(OrderId)  from StoreOrder where LEFT(OrderNo,1)='E'")) + 1))
        Return MaxOrder
    End Function
    Private Function CheckBeforeAdd() As Boolean
        Dim flag As Boolean = True

        For Each i As RepeaterItem In rpt.Items
            Dim txt As TextBox = CType(i.FindControl("txtitem"), TextBox)
            Dim qty As TextBox = CType(i.FindControl("txtqty"), TextBox)
            Dim sku As String = Trim(txt.Text)
            Dim flagItem As Boolean = True

            Dim si As StoreItemRow
            If sku <> String.Empty Then


                'Check Quantity
                If Not IsNumeric(qty.Text) OrElse CInt(qty.Text) < 1 OrElse qty.Text = "" Then
                    AddError("Please enter a valid quantity (Item# " & sku & ")")
                    flag = False
                    Exit For
                End If

                si = StoreItemRow.GetRow(DB, sku)
                'check item FreeSample
                If si.IsFreeSample = True Then
                    AddError("The item #" & sku & " is free sample that is not permitted to buy. Please select a different item.")
                    flag = False
                    Exit For
                End If
                'Check SKU valid
                If si.ItemId <> Nothing And si.IsActive = True Then
                    Dim memberId As Integer = IIf(Session("adminMemberId") = Nothing, 0, Convert.ToInt32(Session("adminMemberId")))

                    'Check Member in list Item Enable
                    Dim sie As New StoreItemEnable()
                    Dim dt As DataTable = sie.ListBrands(memberId)
                    If Not IsDBNull(dt) AndAlso dt.Rows.Count > 0 Then
                        Dim brands As String = dt.Rows(0)("Brands").ToString()
                        Dim memberBrands As String = dt.Rows(0)("MemberBrands").ToString()

                        If Not memberBrands.Contains("," & si.BrandId & ",") Then
                            If brands.Contains("," & si.BrandId & ",") Then
                                AddError("Due to OPI restrictions, all OPI products are now available only in store or by phone order (Item# " & sku & ").")
                                flagItem = False
                                flag = False
                            End If
                        End If

                        dt.Dispose()
                    End If
                    Dim member As MemberRow = MemberRow.GetRow(memberId)
                    If member.IsInternational = True And si.IsFlammable = True Then
                        AddError("The item " & sku & " is not available for customer outsite of 48 states within continental USA. Please remove them for continuing to checkout")
                        flag = False
                    End If
                    'Check total in Cart
                    Dim TotalInCart As Integer
                    TotalInCart = DB.ExecuteScalar("select coalesce(sum(Quantity),0) from StoreCartItem where OrderId = " & DB.Number(Session("adminOrderId")) & " and ItemId = " & si.ItemId)

                    If si.QtyOnHand < 1 Then
                        AddError("There are no more units for this item (Item# " & sku & ")")
                        flag = False
                    ElseIf flagItem = True And TotalInCart + CInt(qty.Text) > si.QtyOnHand Then
                        AddError("We only have " & si.QtyOnHand & " units in stock for this item (Item# " & sku & ")" & IIf(TotalInCart > 0, ", and there are already " & TotalInCart & " in your <a href=""/store/cart.aspx"">shopping cart</a>", "") & ".")
                        qty.Text = si.QtyOnHand - TotalInCart
                        flag = False
                    End If
                Else
                    AddError("Could not find part number (Item# " & sku & ")")
                    flag = False
                End If
            End If


        Next

        Return flag
    End Function

    Private Sub AddToCart(ByVal si As StoreItemRow, ByRef qty As TextBox, ByVal pri As Double, ByRef att As Repeater, ByRef bError As Boolean, ByRef AddedOne As Boolean)
        Session("MemberId") = Nothing
        Session("OrderId") = Nothing
        Dim Quantity As Integer = qty.Text
        Dim TotalInCart As Integer

        If Not IsNumeric(qty.Text) OrElse CInt(qty.Text) < 1 Then
            AddError("Please enter a valid quantity")
            Exit Sub
        End If

        TotalInCart = DB.ExecuteScalar("select coalesce(sum(Quantity),0) from StoreCartItem where OrderId = " & DB.Number(Session("adminOrderId")) & " and ItemId = " & si.ItemId)
        If TotalInCart + CInt(qty.Text) > si.QtyOnHand Then
            AddError("We only have " & si.QtyOnHand & " units in stock for this item" & IIf(TotalInCart > 0, ", and there are already " & TotalInCart & " in your <a href=""/store/cart.aspx"">shopping cart</a>", "") & ".")
            qty.Text = si.QtyOnHand - TotalInCart
            Exit Sub
        End If

        Dim Id As Integer
        Dim AttributeSKU As String = ""
        Dim Attributes As String = String.Empty
        If si.Pricing Is Nothing Then NSS.FillPricing(DB, si)
        ' If si.Pricing Is Nothing Then Response.Redirect("/500.aspx")
        Dim OriginalPrice As Double = si.Pricing.BasePrice
        Dim ExtraPrice As Double = 0
        Dim ddl As DropDownList
        Dim swatches As String = String.Empty

        For Each j As RepeaterItem In att.Items
            If j.ItemIndex = 0 Then If si.Prefix <> Nothing Then AttributeSKU = si.Prefix
            ddl = CType(j.FindControl("ddlAttribute"), DropDownList)
            Id = CType(j.FindControl("lblId"), Label).Text
            Dim att1 As StoreAttributeRow = StoreAttributeRow.GetRow(DB, Id)
            ExtraPrice += Regex.Split(att1.Price, vbCrLf)(ddl.SelectedIndex)
            AttributeSKU &= Regex.Split(att1.SKU, vbCrLf)(ddl.SelectedIndex)
            Attributes &= IIf(Attributes <> String.Empty, vbCrLf, "") & att1.Name & ": " & Regex.Split(att1.Value, vbCrLf)(ddl.SelectedIndex)
        Next

        If Cart Is Nothing Then
            Dim Cart1 As ShoppingCart
            If OrderId = 0 Then
                OrderId = Session("adminOrderId")
            End If
            Cart1 = New ShoppingCart(DB, OrderId, True)
            If Cart1.Order.CarrierType = 0 Then
                'If Cart1.Order.BillToCountry <> "US" And Cart1.Order.ShipToCountry <> "US" Then
                'Cart1.Order.CarrierType = Utility.Common.USPSPriorityShippingId
                'Else
                Cart1.Order.CarrierType = Utility.Common.DefaultShippingId
                'End If
                Cart1.Order.Update()
            End If

            Cart1.Add2Cart(si.ItemId, Nothing, Quantity, Nothing, "Admin-" & OrderId & "-" & pri, Attributes, AttributeSKU, ExtraPrice, swatches, False, True, Nothing)
        Else
            Cart.Add2Cart(si.ItemId, Nothing, Quantity, Nothing, "Admin-" & OrderId & "-" & pri, Attributes, AttributeSKU, ExtraPrice, swatches, False, True, Nothing)
        End If
        AddedOne = True
        divCart.Visible = False
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
                trOpen.Text = "<tr><td colspan=""3""><table border=""0"" cellspacing=""0"" cellpadding=""0"" id=""tblHidden"" style=""width:140px;display:none;""><tr>"
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
    Private Sub BindList()
        Dim SQLFields, SQL As String
        Dim Conn As String = " where "
        SQL = "SELECT memberid FROM member,customer  "
        SQL = SQL & Conn & " member.customerid=customer.customerid "
        Conn = " AND "
        If Not F_UserName.Text = String.Empty Then
            SQL = SQL & Conn & "UserName LIKE " & DB.FilterQuote(F_UserName.Text)
            Conn = " AND "
        End If
        If Not F_Email.Text = String.Empty Then
            SQL = SQL & Conn & "Email LIKE " & DB.FilterQuote(F_Email.Text)
            Conn = " AND "
        End If

    End Sub
    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub
        If btnSearch.Text = "Search" Then
            Session("adminMemberId") = Nothing
            Session("adminOrderId") = Nothing
            If F_UserName.Text <> "" Or F_Email.Text <> "" Then
                Dim Conn As String = " where "
                SQL = "SELECT memberid FROM member,customer  "
                SQL = SQL & Conn & " member.customerid=customer.customerid "
                Conn = " AND "
                If Not F_UserName.Text = String.Empty Then
                    SQL = SQL & Conn & "UserName LIKE " & DB.FilterQuote(F_UserName.Text)
                    Conn = " AND "
                End If
                If Not F_Email.Text = String.Empty Then
                    SQL = SQL & Conn & "Email LIKE " & DB.FilterQuote(F_Email.Text)
                    Conn = " AND "
                End If
                MemberId = DB.ExecuteScalar(SQL)
                Session("adminMemberId") = MemberId
                If MemberId <> 0 Then
                    GenerateUniqueOrderId(MemberId)
                    OrderId = DB.ExecuteScalar("Select isnull(OrderId,0) From StoreOrder o, Member m Where m.LastOrderId = o.OrderId and o.MemberId = " & MemberId & " and OrderNo is null")
                    Session("adminOrderId") = OrderId
                    If OrderId <> 0 Then
                        divCart.Visible = True
                    Else
                        divCart.Visible = False
                    End If

                End If
            End If

        End If
        'Response.Redirect("create-order-ebay.aspx")
    End Sub
    Public Function GenerateUniqueOrderId(ByVal MemberId As Integer) As Integer
        Dim SQL As String = String.Empty
        Dim sMemberId As String
        If MemberId > 0 Then sMemberId = DB.Quote(MemberId.ToString) Else sMemberId = "NULL"
        If sMemberId = "NULL" Then
            Throw New ApplicationException("Cannot have an order if you aren't a member!")
        End If
        Dim result As Integer = 0
        Try
            'SQL = "Select top 1 OrderId From StoreOrder Where  OrderNo is null And OrderId <> " & GenerateUniqueOrderId & " And MemberId = " & MemberId & " Order By CreateDate Desc"
            SQL = "Select top 1 OrderId From StoreOrder Where  OrderNo is null And MemberId = " & MemberId & " and CreateDate > (Select top 1 ProcessDate From StoreOrder Where OrderNo is not null And MemberId = " & MemberId & " and OrderId not in (Select so.OrderId From StoreOrder so inner join StoreCartItem si on so.OrderId = si.OrderId Where so.MemberId = " & MemberId & " And OrderNo is null) Order by ProcessDate Desc) Order By CreateDate Desc"
            result = DB.ExecuteScalar(SQL)
        Catch ex As Exception
            result = 0
        End Try
        If result > 0 Then
            GenerateUniqueOrderId = result
        Else
            ''SQL = "INSERT INTO StoreOrder (RemoteIP, Status, MemberId, BaseSubTotal, SubTotal, Shipping, Tax, Total, CreateDate, CreateSessionID) VALUES (" & DB.Quote(HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")) & ",'N'," & sMemberId & ",0,0,0,0,0,GetDate()," & DB.Quote(Session.SessionID) & ")"
            ''GenerateUniqueOrderId = DB.InsertSQL(SQL)
            GenerateUniqueOrderId = StoreOrderRow.InsertUniqueOrder(DB, Context.Request.ServerVariables("REMOTE_ADDR"), MemberId, Session.SessionID)

        End If

        SQL = "UPDATE Member SET LastOrderId = " & DB.Number(GenerateUniqueOrderId) & " where memberid = " & DB.Number(MemberId)
        DB.ExecuteSQL(SQL)
        If result > 0 Then
            'Email.SendError("ToError500", "GenerateUniqueOrderId (Insert New OrderId, OrderNo is null > 2)", "Exception: <br/>" & result & " <br/>" & MemberId)
        End If
    End Function
End Class

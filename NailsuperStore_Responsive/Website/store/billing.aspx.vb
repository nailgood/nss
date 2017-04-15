Imports Components
Imports DataLayer
Imports ShippingValidator
Imports System.Web.Services

Partial Class billing
    Inherits SitePage

    Private Member As MemberRow
    Private MemberId As Integer = 0
    Private bAddressType As Boolean = False
    Protected act As String = String.Empty
    Private ctr As String = String.Empty
    Private bOldGuest As Boolean = False
    Private o As StoreOrderRow

    Public Property IsXav() As Boolean
        Get
            Return CType(ViewState("IsXav"), Boolean)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsXav") = Value
        End Set
    End Property
    Public Property AddressId() As Integer
        Get
            Return CType(ViewState("AddressId"), Integer)
        End Get
        Set(ByVal Value As Integer)
            ViewState("AddressId") = Value
        End Set
    End Property
    Public Property CurrentAddressType() As String
        Get
            Return CType(ViewState("CurrentAddressType"), String)
        End Get
        Set(ByVal Value As String)
            ViewState("CurrentAddressType") = Value
        End Set
    End Property

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Get member
        MemberId = Utility.Common.GetCurrentMemberId()

        If GetQueryString("act") IsNot Nothing Then
            act = GetQueryString("act")
        End If

        Dim orderId As Integer = Utility.Common.GetCurrentOrderId()
        If HasAccess() = False AndAlso Not MemberId > 0 Then
            If act <> "guest" Then
                Response.Redirect("/members/login.aspx")
            ElseIf Not orderId > 0 Then
                Response.Redirect("/")
            End If
        End If

        If Not String.IsNullOrEmpty(GetQueryString("ctr")) Then
            ctr = GetQueryString("ctr")
        End If

        If MemberId > 0 Then
            Member = MemberRow.GetRow(MemberId)
        End If

        If Not Page.IsPostBack Then
            If act = "guest" Then
                If MemberId > 0 OrElse HasAccess() Then
                    Response.Redirect("/store/cart.aspx")
                Else
                    If Cart Is Nothing Then
                        Cart = New ShoppingCart(DB, orderId)
                    End If
                    o = Cart.Order

                    If o Is Nothing Then
                        o = StoreOrderRow.GetRow(DB, orderId)
                        Email.SendError("ToError500", "Billing > PageLoad > o is Nothing", "orderId=" & orderId)
                    End If

                    Dim USOrderPriceMin As Double = SysParam.GetValue("USOrderPriceMin")
                    If USOrderPriceMin <> Nothing AndAlso o.SubTotal < USOrderPriceMin Then
                        Utility.Common.OrderLog(0, "Page Load > Checkout as Guest > US Order Price Min", {"SubTotal=" & o.SubTotal})
                        ShowError(String.Format(Resources.Alert.OrderMin, USOrderPriceMin, o.SubTotal), True)
                        If o.SubTotal = 0 Then
                            Email.SendError("ToError500", "Billing > PageLoad > o.SubTotal = 0 ", "orderId=" & orderId)
                        End If
                    Else
                        Utility.Common.OrderLog(0, "Page Load > Checkout as Guest", Nothing)
                    End If
                End If

                chkDiffAddress.Checked = False 'De KH co the nhap billing/shipping address default
                secShipping.Visible = False
                rowDiffAddress.Visible = True
            End If

            If MemberId > 0 Then
                AddressId = GetQueryString("id")
                CurrentAddressType = GetQueryString("type")

                Try
                    If Cart.CheckTotalFreeSample(Cart.Order.OrderId) Then
                        Cart.RemoveFreeSample()
                    End If
                Catch ex As Exception
                End Try

                If Not HasOrder OrElse Cart.AllFreeItems OrElse Cart.GetCartItemCount() = 0 Then
                    DB.Close()
                    Response.Redirect("/store/cart.aspx")
                End If
            End If

            BindData()
            LoadMetaData(DB, "/store/billing.aspx")
        End If
    End Sub

    Private Sub BindBillingAddress(ByVal address As MemberAddressRow)
        If Not String.IsNullOrEmpty(address.FirstName) Then
            txtBillingFirstName.Text = address.FirstName.Trim
        End If
        If Not String.IsNullOrEmpty(address.LastName) Then
            txtBillingLastName.Text = address.LastName.Trim
        End If
        If Not String.IsNullOrEmpty(address.Company) Then
            txtBillingCompany.Text = address.Company.Trim()
        End If
        If Not String.IsNullOrEmpty(address.Address1) Then
            txtBillingAddress1.Text = address.Address1.Trim()
        End If
        If Not String.IsNullOrEmpty(address.Address2) Then
            txtBillingAddress1.Text &= " " & address.Address2.Trim()
        End If
        If Not String.IsNullOrEmpty(address.City) Then
            txtBillingCity.Text = address.City.Trim()
        End If
        drpBillingState.SelectedValue = address.State

        If Not String.IsNullOrEmpty(address.Phone) Then
            txtBillingPhone.Text = Trim(address.Phone)
            txtBillingPhone.Text = txtBillingPhone.Text.Replace("--", "")
        End If
        If Not String.IsNullOrEmpty(address.PhoneExt) Then
            txtBillingPhone.Text &= " " & Trim(address.PhoneExt)
        End If
        If Not String.IsNullOrEmpty(address.Zip) Then
            txtBillingZip.Text = Trim(address.Zip)
            hidBillingZipCode.Value = Trim(address.Zip)
        End If

        'If Not String.IsNullOrEmpty(address.DaytimePhone) Then
        '    txtBillingDaytimePhone.Text = Trim(address.DaytimePhone)
        '    txtBillingDaytimePhone.Text = txtBillingDaytimePhone.Text.Replace("--", "")
        'End If
        'If Not String.IsNullOrEmpty(address.DaytimePhoneExt) Then
        '    txtBillingDaytimePhone.Text &= " " & Trim(address.DaytimePhoneExt)
        'End If
        If txtEmail.Visible Then
            If Not String.IsNullOrEmpty(address.Email) Then
                txtEmail.Text = address.Email
            Else
                If o Is Nothing Then
                    o = Cart.Order
                End If

                If o IsNot Nothing AndAlso Not String.IsNullOrEmpty(o.Email) Then
                    txtEmail.Text = o.Email
                End If
            End If
        End If
        drpBillingCountry.SelectedValue = IIf(Not String.IsNullOrEmpty(ctr), ctr, address.Country)
    End Sub
    Private Sub BindShippingAddress(ByVal address As MemberAddressRow)
        If Not String.IsNullOrEmpty(address.FirstName) Then
            txtShippingFirstName.Text = Trim(address.FirstName)
        End If

        If Not String.IsNullOrEmpty(address.LastName) Then
            txtShippingLastName.Text = Trim(address.LastName)
        End If

        If Not String.IsNullOrEmpty(address.Address1) Then
            txtShippingAddress1.Text = Trim(address.Address1)
        End If
        If Not String.IsNullOrEmpty(address.Address2) Then
            txtShippingAddress2.Text = Trim(address.Address2)
        End If

        If Not String.IsNullOrEmpty(address.City) Then
            txtShippingCity.Text = Trim(address.City)
        End If

        drpShippingState.SelectedValue = address.State

        If Not String.IsNullOrEmpty(address.Fax) Then
            txtShippingFax.Text = Trim(address.Fax)
            txtShippingFax.Text = txtShippingFax.Text.Replace("--", "")
        End If

        If Not String.IsNullOrEmpty(address.Phone) Then
            txtShippingPhone.Text = Trim(address.Phone)
            txtShippingPhone.Text = txtShippingPhone.Text.Replace("--", "")
        End If

        If Not String.IsNullOrEmpty(address.PhoneExt) Then
            txtShippingPhone.Text &= " " & Trim(address.PhoneExt)
        End If
        If Not String.IsNullOrEmpty(address.Zip) Then
            txtShippingZip.Text = Trim(address.Zip)
            hidShippingZipCode.Value = Trim(address.Zip)
        End If

        If Not String.IsNullOrEmpty(address.Company) Then
            txtShippingCompany.Text = Trim(address.Company)
        End If
        drpShippingCountry.SelectedValue = IIf(Not String.IsNullOrEmpty(GetQueryString("ctr")), GetQueryString("ctr"), address.Country)
    End Sub
    Private Sub LoadEditAddress()
        Dim address As MemberAddressRow = MemberAddressRow.GetRow(DB, AddressId)
        If (CurrentAddressType = Utility.Common.MemberAddressType.Billing.ToString()) Then
            Utility.Common.OrderLog(0, "Page Load > Billing", Nothing)
            BindBillingAddress(address)
            secShipping.Visible = False
            rowDiffAddress.Visible = False

        ElseIf (CurrentAddressType = Utility.Common.MemberAddressType.Shipping.ToString()) Then
            Utility.Common.OrderLog(0, "Page Load > Shipping", Nothing)
            BindShippingAddress(address)
            secBilling.Visible = False
        End If

    End Sub
    Private Sub BindData()
        drpShippingCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
        drpShippingCountry.DataBind()

        drpBillingCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
        drpBillingCountry.DataBind()

        Dim listItem = StateRow.GetStateList()
        drpBillingState.Items.AddRange(listItem.ToArray())
        drpBillingState.DataBind()
        drpBillingState.Items.Insert(0, New ListItem("", ""))


        drpShippingState.Items.AddRange(listItem.ToArray())
        drpShippingState.DataBind()
        drpShippingState.Items.Insert(0, New ListItem("", ""))

        drpBillingState.Items.Remove(drpBillingState.Items.FindByValue("PR"))
        drpShippingState.Items.Remove(drpShippingState.Items.FindByValue("PR"))
        drpBillingState.Items.Remove(drpShippingState.Items.FindByValue("VI"))
        drpShippingState.Items.Remove(drpShippingState.Items.FindByValue("VI"))

        If MemberId > 0 Then
            Dim o As StoreOrderRow = Cart.Order
            If AddressId > 1 Then ''edit address
                LoadEditAddress()
            Else
                If o Is Nothing Then
                    Response.Redirect("/store/cart.aspx")
                End If
                LoadDefaultAddress()
            End If
        Else
            If act = "guest" Then
                drpBillingCountry.SelectedValue = "US"
            End If
        End If

        If String.IsNullOrEmpty(txtBillingZip.Text) Then
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "PageLoad", "$(""#divBillingCity"").hide(); $(""#divBillingState"").hide();", True)
        End If
    End Sub
    Private Sub LoadDefaultAddress()
        Dim Billing As MemberAddressRow = Nothing
        Dim Shipping As MemberAddressRow = Nothing
        If Cart.Order.BillingAddressId > 0 Then
            Billing = MemberAddressRow.GetRow(DB, Cart.Order.BillingAddressId)
        Else
            Billing = MemberAddressRow.GetAddressByType(DB, Member.MemberId, Utility.Common.MemberAddressType.Billing.ToString())
        End If

        If Cart.Order.ShippingAddressId > 0 Then
            Shipping = MemberAddressRow.GetRow(DB, Cart.Order.ShippingAddressId)
        Else
            Shipping = MemberAddressRow.GetAddressByType(DB, Member.MemberId, Utility.Common.MemberAddressType.Shipping.ToString())
        End If

        If Cart.Order.BillToCity = Nothing And Billing.Country = "US" Then
            BindBillingAddress(Billing)
        Else
            Dim tmlAddress As New MemberAddressRow
            tmlAddress.Company = Cart.Order.BillToSalonName
            tmlAddress.FirstName = Cart.Order.BillToName
            tmlAddress.LastName = Cart.Order.BillToName2
            tmlAddress.Address1 = Cart.Order.BillToAddress
            tmlAddress.Address2 = Cart.Order.BillToAddress2
            tmlAddress.City = Cart.Order.BillToCity
            tmlAddress.State = Cart.Order.BillToCounty
            tmlAddress.Zip = Cart.Order.BillToZipcode
            tmlAddress.Fax = Cart.Order.BillToFax
            tmlAddress.Phone = Cart.Order.BillToPhone
            tmlAddress.PhoneExt = Cart.Order.BillToPhoneExt
            'tmlAddress.DaytimePhone = Cart.Order.BillToDaytimePhone
            'tmlAddress.DaytimePhoneExt = Cart.Order.BillToDaytimePhoneExt
            tmlAddress.Email = Cart.Order.Email
            tmlAddress.Country = Cart.Order.BillToCountry
            BindBillingAddress(tmlAddress)
        End If
        If Cart.Order.BillToSalonName = Nothing Then
            Dim address As MemberAddressRow
            If Member.IsSameDefaultAddress Then
                address = Billing
            Else
                address = Shipping
            End If
            BindShippingAddress(address)
        Else
            Dim tmlAddress As New MemberAddressRow
            tmlAddress.Company = Cart.Order.ShipToSalonName
            tmlAddress.FirstName = Cart.Order.ShipToName
            tmlAddress.LastName = Cart.Order.ShipToName2
            tmlAddress.Address1 = Cart.Order.ShipToAddress
            tmlAddress.Address2 = Cart.Order.ShipToAddress2
            tmlAddress.City = Cart.Order.ShipToCity
            tmlAddress.State = Cart.Order.ShipToCounty
            tmlAddress.Zip = Cart.Order.ShipToZipcode
            tmlAddress.Fax = Cart.Order.ShipToFax
            tmlAddress.Phone = Cart.Order.ShipToPhone
            tmlAddress.PhoneExt = Cart.Order.ShipToPhoneExt
            tmlAddress.Country = Cart.Order.ShipToCountry
            BindShippingAddress(tmlAddress)
        End If
        '' ''''''''''''''''''''''
        Dim type As String = String.Empty
        If Not (Request.QueryString("type") Is Nothing) Then
            type = Request.QueryString("type")
        End If



        'Dim message As String = Cart.CheckOrderPriceMin(o, Resources.Alert.OrderMin)
        'If Not String.IsNullOrEmpty(message) Then
        '    AddError(message)
        'End If
        Dim disableCheckDiff As Boolean = False
        If (Billing.AddressId = Shipping.AddressId AndAlso Billing.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString()) Then
            Cart.Order.IsSameAddress = True
            disableCheckDiff = True
        End If
        If Cart.Order.IsSameAddress Then
            secShipping.Visible = False
            rowDiffAddress.Visible = True
            chkDiffAddress.Checked = False
        Else
            secShipping.Visible = True
            rowDiffAddress.Visible = True
            chkDiffAddress.Checked = True
        End If
        If disableCheckDiff Then

            rowDiffAddress.Visible = False
        End If

    End Sub
    Private Sub lkbCandidateCheckout_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lkbCandidateCheckout.Click
        bAddressType = True
        btnContinue_Click(Nothing, Nothing)
    End Sub

    Private Sub btnContinue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnContinue.Click

        If Page.IsValid Then
            If act = "guest" Then
                Dim dt As DataTable = DB.GetDataTable("SELECT MemberId, CreateDate, GuestStatus, IsActive FROM Member WHERE CustomerId IN (select CustomerId from customer where email = '" & txtEmail.Text.Trim() & "')")
                If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        If CInt(dr("GuestStatus")) > 0 And CBool(dr("IsActive")) = False Then '0-registered, 1-guestuncheckout, 2-guestcheckout
                            bOldGuest = True

                            MemberId = CInt(dr("MemberId"))
                            If MemberId > 0 Then
                                Member = MemberRow.GetRow(MemberId)
                            End If
                        Else
                            ShowError(String.Format("The email address you entered is already in use. Please visit the <a href=""/members/forgotpassword.aspx"">forgot your password</a> page if you no longer remember your password, otherwise please use a different email address."), False)
                            Exit Sub
                        End If
                    Next
                End If
            End If

            ''Check Address
            'Dim bCheckAddress As Boolean = True
            'If CurrentAddressType = Nothing Then
            '    bCheckAddress = CheckAddress("")
            'ElseIf CurrentAddressType = "Billing" Then
            '    bCheckAddress = CheckAddress("Billing")
            'ElseIf CurrentAddressType = "Shipping" Then
            '    bCheckAddress = CheckAddress("Shipping")
            'End If
            'If bCheckAddress = False Then
            '    Exit Sub
            'End If

            If o Is Nothing Then
                o = Cart.Order
            End If

            'Checkout as guest
            If act = "guest" AndAlso Not bOldGuest Then
                Try
                    DB.BeginTransaction()
                    ' Create Membership
                    Dim MemberBillingAddress As MemberAddressRow
                    Dim MemberShippingAddress As MemberAddressRow
                    Member = New MemberRow(DB)
                    Member.Username = txtEmail.Text.Trim()
                    Member.Password = IIf(Member.Username.Contains("@"), txtEmail.Text.Trim().Substring(0, txtEmail.Text.Trim().IndexOf("@")), txtBillingCity.Text.Trim())
                    Member.CreateDate = Now
                    Member.IsActive = False
                    MemberBillingAddress = New MemberAddressRow(DB)
                    MemberShippingAddress = New MemberAddressRow(DB)

                    Member.MemberTypeId = Nothing
                    Member.ProfessionalStatus = Nothing
                    Member.IsSameDefaultAddress = Not chkDiffAddress.Checked
                    Member.AuthorizedSignatureName = Nothing
                    Member.AuthorizedSignatureTitle = Nothing
                    Member.AuthorizedSignatureDate = Nothing
                    Member.DeptOfRevenueRegistered = Nothing
                    Member.InformationAccuracyAgreement = Nothing
                    Member.ResaleAcceptance = Nothing
                    Member.SalesTaxExemptionNumber = Nothing
                    Member.LastOrderId = o.OrderId

                    Member.Customer = New CustomerRow()
                    Member.Customer.SalesTaxExemptionNumber = Nothing
                    Member.Customer.Email = txtEmail.Text
                    Member.CustomerContact.Email = txtEmail.Text
                    Member.CustomerContact.CountryCode = drpBillingCountry.SelectedValue


                    MemberId = Member.Insert(True)
                    o.MemberId = MemberId

                    If MemberId > 0 Then

                    Else
                        DB.RollbackTransaction()
                        AddError("An error has occurred while trying to process your request. Please, try again later")
                        Email.SendError("ToError500", "Register Error", Request.Url.AbsoluteUri.ToString() & "<br>MemberId=0")
                        Exit Sub
                    End If

                    GetOrderBillingAddressFromUI(o)
                    o.Email = txtEmail.Text.Trim()
                    o.IsSameAddress = Not chkDiffAddress.Checked
                    If (o.IsSameAddress) Then
                        o.ShipToSalonName = o.BillToSalonName
                        o.ShipToName = o.BillToName
                        o.ShipToName2 = o.BillToName2
                        o.ShipToAddress = o.BillToAddress
                        o.ShipToAddress2 = o.BillToAddress2
                        o.ShipToCity = o.BillToCity
                        o.ShipToCountry = o.BillToCountry
                        o.ShipToCounty = o.BillToCounty
                        o.ShipToFax = o.BillToFax
                        o.ShipToPhone = o.BillToPhone
                        o.ShipToPhoneExt = o.BillToPhoneExt
                        o.ShipToZipcode = o.BillToZipcode
                    Else
                        GetOrderShippingAddressFromUI(o)
                    End If

                    UpdateMemberAddressByOrderAddress(o)

                    If o.BillingAddressId < 1 Or o.ShippingAddressId < 1 Then
                        Dim sLog As String = String.Empty
                        If o.BillingAddressId < 1 And o.ShippingAddressId < 1 Then
                            sLog = "Insert Billing Address Error<br/>" & "Email=" & txtEmail.Text.Trim() & ",Country=" & drpBillingCountry.SelectedValue
                        Else
                            sLog = "Insert Shipping Address Error<br/>" & "Email=" & txtEmail.Text.Trim() & ",Country=" & drpBillingCountry.SelectedValue
                        End If

                        Email.SendError("ToError500", " Register Error", Request.Url.AbsoluteUri.ToString() & "<br>Log: " & sLog)
                        Exit Sub
                    End If



                    Dim MailingStatus As String = ""
                    MailingStatus = "ACTIVE"
                    Dim dbMailingMember As MailingMemberRow
                    dbMailingMember = MailingMemberRow.GetRowByEmail(DB, txtEmail.Text)
                    dbMailingMember.MimeType = "HTML"
                    dbMailingMember.Status = MailingStatus
                    If dbMailingMember.MemberId <> 0 Then
                        dbMailingMember.Update()
                    Else
                        dbMailingMember.Email = txtEmail.Text.Trim()
                        dbMailingMember.Name = txtEmail.Text.Trim()
                        dbMailingMember.Insert()
                    End If

                    If (dbMailingMember.MemberId > 0) Then
                        dbMailingMember.DeleteFromAllLists()
                        dbMailingMember.InsertToList(1)
                    End If

                    DB.CommitTransaction()
                Catch ex As Exception
                    DB.RollbackTransaction()
                    Email.SendError("ToError500", " Register Error", Request.Url.AbsoluteUri.ToString() _
                                    & "<br>Country: " & drpBillingCountry.SelectedValue _
                                    & "<br>Billing First Name: " & txtBillingFirstName.Text _
                                    & "<br>Last Name: " & txtBillingLastName.Text _
                                    & "<br>Email: " & txtEmail.Text _
                                    & "<br>Company/Salon: " & txtBillingCompany.Text _
                                    & "<br>Street: " & txtBillingAddress1.Text _
                                    & "<br>City: " & txtBillingCity.Text _
                                    & "<br>State: " & drpBillingState.SelectedValue _
                                    & "<br>Zip Code: " & txtBillingZip.Text _
                                    & "<br>Is Diffrent: " & chkDiffAddress.Checked.ToString() _
                                    & "<br>Shipping First Name: " & txtShippingFirstName.Text _
                                    & "<br>Last Name: " & txtShippingLastName.Text _
                                    & "<br>Email: " & txtEmail.Text _
                                    & "<br>Company/Salon: " & txtShippingCompany.Text _
                                    & "<br>Street: " & txtShippingAddress1.Text _
                                    & "<br>City: " & txtShippingCity.Text _
                                    & "<br>State: " & drpShippingState.SelectedValue _
                                    & "<br>Zip Code: " & txtShippingZip.Text _
                                    & "<br>Exception: " & ex.Message)
                End Try

                Cart.RecalculateOrderDetail("billing.imgContinueCheckout_Click > guest")
                ValidateAddressType(o)
            Else
                If bOldGuest Then
                    o.MemberId = MemberId
                End If

                o.Email = txtEmail.Text.Trim()
                'Billing
                If (AddressId > 0) Then ''edit address
                    If CurrentAddressType = Utility.Common.MemberAddressType.Billing.ToString() Then
                        GetOrderBillingAddressFromUI(o)

                        If AddressId = o.ShippingAddressId Then
                            o.IsSameAddress = True
                            'Member.IsSameDefaultAddress = True
                        End If

                        If (o.IsSameAddress) Then
                            o.ShipToSalonName = o.BillToSalonName
                            o.ShipToName = o.BillToName
                            o.ShipToName2 = o.BillToName2
                            o.ShipToAddress = o.BillToAddress
                            o.ShipToAddress2 = o.BillToAddress2
                            o.ShipToCity = o.BillToCity
                            o.ShipToCountry = o.BillToCountry
                            o.ShipToCounty = o.BillToCounty
                            o.ShipToFax = o.BillToFax
                            o.ShipToPhone = o.BillToPhone
                            o.ShipToPhoneExt = o.BillToPhoneExt
                            o.ShipToZipcode = o.BillToZipcode
                        End If
                    ElseIf CurrentAddressType = Utility.Common.MemberAddressType.Shipping.ToString() Then
                        GetOrderShippingAddressFromUI(o)
                    End If
                Else
                    GetOrderBillingAddressFromUI(o)
                    GetOrderShippingAddressFromUI(o)
                End If

                'Check PO address
                If (Utility.Common.IsOrderShippingPOBoxAddress(o)) Then
                    ShowError(Resources.Alert.POBoxShippingAddress, False)
                    Exit Sub
                End If

                If AddressId > 0 Then
                    If CurrentAddressType = Utility.Common.MemberAddressType.Shipping.ToString() Then
                        ValidateAddressType(o)
                    Else
                        If o.IsSameAddress Then
                            ValidateAddressType(o)
                        End If
                    End If
                Else
                    ValidateAddressType(o)
                End If

                If CurrentAddressType = "Shipping" AndAlso o.ShipToCountry = "US" AndAlso (o.ShipToCounty = "HI" Or o.ShipToCounty = "AK") AndAlso o.IsSameAddress = False Then
                    o.IsSameAddress = True
                End If

                UpdateMemberAddressByOrderAddress(o)

                If (o.CarrierType = Utility.Common.USPSPriorityShippingId) Then
                    o.CarrierType = Utility.Common.DefaultShippingId
                End If

                Dim sql As String = "UPDATE StoreOrder SET BillingAddressId=" & o.BillingAddressId & ",ShippingAddressId=" & o.ShippingAddressId & ",CarrierType=" & o.CarrierType & ",IsSameAddress=" & (IIf(o.IsSameAddress = True, 1, 0)) & " where OrderId=" & o.OrderId _
                    & "; UPDATE StoreCartItem SET CarrierType=" & o.CarrierType & " where OrderId=" & o.OrderId

                If o.CarrierType <> Utility.Common.TruckShippingId() Then ' Khong update nhung dong CarrierType=4 (truong hop Order co 2 dong CarrierType khac nhau)
                    sql &= " AND CarrierType <> " & Utility.Common.TruckShippingId()
                End If

                If Member IsNot Nothing AndAlso o.OrderId <> Member.LastOrderId Then
                    sql &= String.Format("; UPDATE Member SET LastOrderId = {0} WHERE MemberId = {1}; UPDATE StoreOrder SET MemberId = 0 WHERE OrderId={2} AND OrderNo IS NULL", o.OrderId, MemberId, Member.LastOrderId)
                Else
                    If o.IsSameAddress <> Member.IsSameDefaultAddress Then
                        sql &= String.Format("; UPDATE Member SET IsSameDefaultAddress = {0} WHERE MemberId = {1}; ", IIf(o.IsSameAddress = True, "1", "0"), MemberId)
                    End If
                End If

                DB.ExecuteSQL(sql)

                Cart.RecalculateOrderDetail("billing.btnContinue_Click")
                DB.Close()
            End If

            Utility.CacheUtils.RemoveCache(String.Format(MemberRow.cachePrefixKey & "GetRow_{0}", MemberId))
            Session.Remove("CheckMemberIsInternational")
            If act = "guest" Then
                If MemberId > 0 Then
                    Session("CheckOutGuest") = "1"
                    Session("MemberId") = MemberId
                    Utility.Common.SetCartCookieLogin(MemberId, o.OrderId)
                    Utility.Common.OrderLog(o.OrderId, "Submit > Checkout as Guest", Nothing)
                    ViewedItemRow.UpdateMemberId(Session.SessionID, MemberId)
                End If

                Response.Redirect("/store/payment.aspx")
            Else
                Utility.Common.OrderLog(o.OrderId, "Submit", Nothing)
                Response.Redirect(GetBackURL())
            End If
        End If
    End Sub

    Private Sub ValidateAddressType(ByVal o As StoreOrderRow)
        If o.ShipToCountry = "US" Then
            Dim xav As Validator = CheckAddressType(DB, o)
            Session("CheckResidential") = 1
            If bAddressType = False And Not IsXav And Not xav Is Nothing Then
                Dim bCandidate As Boolean = False
                If Not xav.CandidateList Is Nothing AndAlso xav.CandidateList.Count > 1 Then
                    bCandidate = True
                ElseIf Not xav.CandidateList Is Nothing AndAlso xav.CandidateList.Count = 1 Then

                    Dim can As ShippingValidator.Candidate = xav.CandidateList.Item(0)
                    If o.ShipToAddress.ToUpper().Trim() <> can.Street.Trim() Or o.ShipToCity.ToUpper() <> can.City Or Not can.ZipCode.Contains(o.ShipToZipcode) Then
                        bCandidate = True
                    ElseIf can.Code = 3 Then
                        xav.Type = AddressType.Insufficient
                        litMsg.Text = Resources.Alert.InsufficientAddress
                        bCandidate = True
                    End If
                End If

                If bCandidate Then
                    If xav.Type = AddressType.Insufficient Then
                        rptListConfirmAddress.DataSource = Nothing
                    Else
                        rptListConfirmAddress.DataSource = xav.CandidateList
                    End If

                    rptListConfirmAddress.DataBind()
                    ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "showListAddress", "showListAddress();", True)
                    Exit Sub
                End If

            End If

            If Not xav Is Nothing Then
                If xav.Msg.ToUpper().Contains("ERROR") Then
                    Email.SendReport("ToReportFedEx", "[Billing] Shipping Validator", xav.Msg)
                End If
            Else
                Dim body As String = ""
                body &= "<br>o.OrderId= " & o.OrderId
                body &= "<br>o.ShipToCountry= " & o.ShipToCountry.ToString()
                body &= "<br>o.ShipToCounty= " & o.ShipToCounty.ToString()
                body &= "<br>o.ShipToCity = " & o.ShipToCity.ToString()
                body &= "<br>o.ShipToZipcode= " & o.ShipToZipcode.ToString()
                body &= "<br>o.ShipToAddress= " & o.ShipToAddress.ToString()

                Email.SendReport("ToReportFedEx", "[Billing] Shipping Validator", "XAV Nothing " & body)
            End If

        End If
        o.Update()
    End Sub
    Protected Sub rptListConfirmAddress_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptListConfirmAddress.ItemDataBound
        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim ltrAddress As Literal = DirectCast(e.Item.FindControl("ltrAddress"), Literal)
            If Not ltrAddress Is Nothing Then
                Dim objCandidate As Candidate = CType(e.Item.DataItem, Candidate)
                If Not (objCandidate Is Nothing) Then

                    ltrAddress.Text &= "<li class='street'>" & objCandidate.Street & "</li>"
                    ltrAddress.Text &= "<li class='city'>" & objCandidate.City & "," & objCandidate.State & "," & objCandidate.ZipCode & "</li>"


                End If
            End If
        End If
    End Sub

    Protected Sub rptListConfirmAddress_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptListConfirmAddress.ItemCommand

        If e.CommandName = "Select" Then
            Try
                Dim arr As String() = e.CommandArgument.ToString().Split("|")
                Dim strStreet As String = arr(0)
                Dim strCity As String = arr(1)
                Dim strState As String = arr(2)
                Dim strZipCode As String = arr(3)

                txtShippingAddress1.Text = strStreet
                txtShippingCity.Text = strCity
                txtShippingZip.Text = strZipCode
                drpShippingState.SelectedValue = strState

                If arr(4) = "0" Then
                    IsXav = True
                End If

                'SameBilling
                If Not chkDiffAddress.Checked Then
                    txtBillingAddress1.Text = strStreet
                    txtBillingCity.Text = strCity
                    txtShippingZip.Text = strZipCode
                    drpShippingState.SelectedValue = strState
                End If
            Catch ex As Exception

            End Try

            'Close popup
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "closeListConfirm", "ClosePopupListConfirm();", True)
        End If

    End Sub

    Private Sub UpdateMemberBillingAddressFromOrder(ByVal billingAddress As MemberAddressRow, ByVal order As StoreOrderRow)
        If Not (billingAddress Is Nothing) Then
            billingAddress.Address1 = order.BillToAddress
            billingAddress.Address2 = order.BillToAddress2
            billingAddress.City = order.BillToCity
            billingAddress.Company = order.BillToSalonName
            billingAddress.Country = order.BillToCountry
            ' billingAddress.DaytimePhone = order.BillToDaytimePhone
            'billingAddress.DaytimePhoneExt = order.BillToDaytimePhoneExt
            billingAddress.Email = order.Email
            billingAddress.Fax = order.BillToFax
            billingAddress.FirstName = order.BillToName
            billingAddress.LastName = order.BillToName2
            billingAddress.ModifyDate = DateTime.Now
            billingAddress.Phone = order.BillToPhone
            billingAddress.PhoneExt = order.BillToPhoneExt
            billingAddress.Zip = order.BillToZipcode
            If billingAddress.Country = "US" Then
                billingAddress.Region = ""
                billingAddress.State = order.BillToCounty
            Else
                billingAddress.Region = order.BillToCounty
                billingAddress.State = ""
            End If
            billingAddress.DoExport = True
            billingAddress.DB = DB

            If act = "guest" AndAlso bOldGuest = False Then
                billingAddress.MemberId = MemberId
                billingAddress.DoExport = False
                billingAddress.Label = "Default Billing Address"
                billingAddress.AddressType = "Billing"
                order.BillingAddressId = billingAddress.Insert()
            Else
                billingAddress.Update()
                order.BillingAddressId = billingAddress.AddressId
            End If

            ''Update customer
            Member.DB = DB
            Member.Customer.Address = billingAddress.Address1
            Member.Customer.Address2 = billingAddress.Address2
            Member.Customer.Name = billingAddress.FirstName
            Member.Customer.Name2 = billingAddress.LastName
            Member.Customer.City = billingAddress.City
            Member.Customer.Zipcode = billingAddress.Zip
            Member.Customer.County = billingAddress.State
            Member.Customer.Phone = billingAddress.Phone
            Member.Customer.PhoneExt = billingAddress.PhoneExt
            Member.Customer.DoExport = IIf(act = "guest", False, True)
            Member.Customer.Update()
        End If
    End Sub

    Private Sub UpdateMemberShippingAddressFromOrder(ByVal shippingAddress As MemberAddressRow, ByVal order As StoreOrderRow)
        If Not (shippingAddress Is Nothing) Then
            shippingAddress.Address1 = order.ShipToAddress
            shippingAddress.Address2 = order.ShipToAddress2
            shippingAddress.City = order.ShipToCity
            shippingAddress.Company = order.ShipToSalonName
            shippingAddress.Country = order.ShipToCountry
            shippingAddress.Fax = order.ShipToFax
            shippingAddress.FirstName = order.ShipToName
            shippingAddress.LastName = order.ShipToName2
            shippingAddress.ModifyDate = DateTime.Now
            shippingAddress.Phone = order.ShipToPhone
            shippingAddress.PhoneExt = order.ShipToPhoneExt
            shippingAddress.Zip = order.ShipToZipcode
            If shippingAddress.Country = "US" Then
                shippingAddress.Region = ""
                shippingAddress.State = order.ShipToCounty
            Else
                shippingAddress.Region = order.ShipToCounty
                shippingAddress.State = ""
            End If
            shippingAddress.DB = DB
            shippingAddress.DoExport = True

            If act = "guest" AndAlso bOldGuest = False Then
                shippingAddress.MemberId = MemberId
                shippingAddress.DoExport = False
                shippingAddress.Label = "Default Shipping Address"
                shippingAddress.AddressType = "Shipping"
                order.ShippingAddressId = shippingAddress.Insert()
            Else
                shippingAddress.Update()
                order.ShippingAddressId = shippingAddress.AddressId
            End If
        End If
    End Sub

    Private Sub UpdateMemberAddressByOrderAddress(ByVal order As StoreOrderRow)
        If AddressId > 0 Then
            Dim currentAddress As MemberAddressRow = MemberAddressRow.GetRow(DB, AddressId)
            Dim otherAddress As MemberAddressRow = Nothing
            If CurrentAddressType = Utility.Common.MemberAddressType.Billing.ToString() Then
                If order.IsSameAddress Then
                    If currentAddress.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString() Then ''neu billinh add ress la address book
                        order.ShippingAddressId = currentAddress.AddressId
                    Else ''neu la default billing
                        otherAddress = MemberAddressRow.GetAddressByType(DB, order.MemberId, Utility.Common.MemberAddressType.Shipping.ToString())
                        UpdateMemberShippingAddressFromOrder(otherAddress, order)
                    End If
                End If
                UpdateMemberBillingAddressFromOrder(currentAddress, order)
            ElseIf CurrentAddressType = Utility.Common.MemberAddressType.Shipping.ToString() Then
                If order.IsSameAddress Then
                    If currentAddress.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString() Then ''neu billinh add ress la address book
                        order.BillingAddressId = currentAddress.AddressId
                    Else ''neu la default shipping
                        otherAddress = MemberAddressRow.GetAddressByType(DB, order.MemberId, Utility.Common.MemberAddressType.Billing.ToString())
                        UpdateMemberBillingAddressFromOrder(otherAddress, order)
                    End If
                End If
                UpdateMemberShippingAddressFromOrder(currentAddress, order)
            End If
        Else
            Dim billingAddress As MemberAddressRow = Nothing
            Dim shippingAddress As MemberAddressRow = Nothing
            If order.BillingAddressId > 0 Then
                billingAddress = MemberAddressRow.GetRow(DB, order.BillingAddressId)
            ElseIf act = "guest" AndAlso bOldGuest = False Then
                billingAddress = New MemberAddressRow()
            Else
                billingAddress = MemberAddressRow.GetAddressByType(DB, Member.MemberId, Utility.Common.MemberAddressType.Billing.ToString())
            End If
            UpdateMemberBillingAddressFromOrder(billingAddress, order)

            If order.ShippingAddressId > 0 Then
                shippingAddress = MemberAddressRow.GetRow(DB, order.ShippingAddressId)
            ElseIf act = "guest" AndAlso bOldGuest = False Then
                shippingAddress = New MemberAddressRow()
            Else
                shippingAddress = MemberAddressRow.GetAddressByType(DB, Member.MemberId, Utility.Common.MemberAddressType.Shipping.ToString())
            End If
            UpdateMemberShippingAddressFromOrder(shippingAddress, order)
        End If

    End Sub

    Private Sub ShowError(ByVal msg As String, ByVal bFix As Boolean)
        If bFix Then
            ltrLoadMsg.Text = String.Format("<span style='display:none;' id='loadMsgFix'>{0}</span>", msg)
        Else
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "showError", "ShowError('" & msg & "');", True)
        End If

    End Sub

    Private Sub GetOrderShippingAddressFromUI(ByRef o As StoreOrderRow)
        If AddressId > 0 Or chkDiffAddress.Checked Then
            If AddressId < 1 Then
                o.IsSameAddress = False
            End If
            o.ShipToSalonName = txtShippingCompany.Text.Trim()
            o.ShipToName = txtShippingFirstName.Text.Trim()
            o.ShipToName2 = txtShippingLastName.Text.Trim()
            o.ShipToAddress = txtShippingAddress1.Text.Trim()
            o.ShipToAddress2 = txtShippingAddress2.Text.Trim()
            o.ShipToCity = txtShippingCity.Text.Trim()
            o.ShipToCountry = "US"
            o.ShipToCounty = drpShippingState.SelectedValue
            Utility.Common.GetUSPhoneValueFromUI(txtShippingPhone.Text, o.ShipToPhone, o.ShipToPhoneExt)
            o.ShipToZipcode = txtShippingZip.Text.Trim()
            o.ShipToFax = txtShippingFax.Text
            ''o.ShippingAddressId = AddressId
        Else
            o.IsSameAddress = True
            o.ShipToSalonName = o.BillToSalonName
            o.ShipToName = o.BillToName
            o.ShipToName2 = o.BillToName2
            o.ShipToAddress = o.BillToAddress
            o.ShipToAddress2 = o.BillToAddress2
            o.ShipToCity = o.BillToCity
            o.ShipToCountry = o.BillToCountry
            o.ShipToCounty = o.BillToCounty
            o.ShipToFax = o.BillToFax
            o.ShipToPhone = o.BillToPhone
            o.ShipToPhoneExt = o.BillToPhoneExt
            o.ShipToZipcode = o.BillToZipcode
        End If

    End Sub
    Private Sub GetOrderBillingAddressFromUI(ByRef o As StoreOrderRow)
        o.CheckoutPage = "Billing"
        o.BillToSalonName = txtBillingCompany.Text.Trim()
        o.BillToName = txtBillingFirstName.Text.Trim()
        o.BillToName2 = txtBillingLastName.Text.Trim()
        o.BillToAddress = txtBillingAddress1.Text.Trim()
        o.BillToAddress2 = String.Empty
        o.BillToCity = txtBillingCity.Text.Trim()
        o.BillToCountry = "US"
        o.BillToCounty = drpBillingState.SelectedValue
        o.BillToCustomerId = Member.CustomerId
        Utility.Common.GetUSPhoneValueFromUI(txtBillingPhone.Text, o.BillToPhone, o.BillToPhoneExt)
        'Utility.Common.GetUSPhoneValueFromUI(txtBillingDaytimePhone.Text, o.BillToDaytimePhone, o.BillToDaytimePhoneExt)
        o.BillToZipcode = txtBillingZip.Text.Trim()
        'o.BillToFax = txtBillingFax.Text.Trim()
        If Trim(Member.SalesTaxExemptionNumber) <> String.Empty Then
            o.IsTaxExempt = True
            o.TaxExemptId = Member.SalesTaxExemptionNumber
        Else
            o.IsTaxExempt = False
            o.TaxExemptId = Nothing
        End If
    End Sub
    Private Function GetBackURL() As String
        Dim result As String = String.Empty

        If Not Session("isUpdateAddress") Is Nothing Then
            If Session("isUpdateAddress") = 1 Then
                Session("isUpdateAddress") = Nothing
                Return "/store/reward-point.aspx"
            End If
        End If
        If Not (Request.QueryString("url") Is Nothing) Then
            result = Request.QueryString("url")
        End If
        If (String.IsNullOrEmpty(result)) Then
            result = "/store/payment.aspx"
        End If


        Return result
    End Function

    Protected Sub ServerCheckPhoneUS(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        If Not String.IsNullOrEmpty(e.Value) Then
            e.IsValid = Utility.Common.CheckUSPhoneValid(e.Value)
        End If
    End Sub

    Protected Sub chkDiffAddress_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDiffAddress.CheckedChanged
        If chkDiffAddress.Checked Then
            secShipping.Visible = True
            'ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "ShippingPhone", "$(""#txtShippingPhone"").mask(maskPhoneUSRegular);", True)
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "BindControl", " $('#txtBillingPhone, #txtBillingFax, #txtShippingPhone, #txtShippingFax').keyup(function (e) {fnFormatPhoneUS(this, e, this.id);}); $('#drpBillingCountry, #drpShippingCountry').change(function () {fnSelectCountryCode(this.id);}); $('#divShippingCountry').css('display', 'none'); SetZipTextBox();" _
                                                    & IIf(String.IsNullOrEmpty(txtShippingZip.Text), "$(""#divShippingCity"").hide(); $(""#divShippingState"").hide();", ""), True)
        Else
            secShipping.Visible = False
        End If
    End Sub

    Protected Sub lbtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnBack.Click
        Response.Redirect(GetBackURL())
    End Sub

    <WebMethod()>
    Public Shared Function SetZipCode(ByVal city As String, ByVal zipcode As String, ByVal state As String) As String
        Dim msg As String = ""
        Dim result As Integer = 0
        Dim collection As ZipCodeCollection = ZipCodeRow.GetRow(zipcode)

        Dim country As String = String.Empty
        If collection.Count > 0 Then
            For Each zip As ZipCodeRow In collection
                If zip.StateCode = "VI" Or zip.StateCode = "PR" Then
                    country = zip.StateCode
                Else
                    country = String.Empty
                    Exit For
                End If
            Next
        End If

        If country = String.Empty Then
            Return Newtonsoft.Json.JsonConvert.SerializeObject(collection)
        Else
            Return Newtonsoft.Json.JsonConvert.SerializeObject(country)
        End If
    End Function

    Private Function GetInternationalLink() As String
        Dim url As String = "/store/billingint.aspx"
        If Not String.IsNullOrEmpty(CurrentAddressType) AndAlso AddressId > 0 Then
            url &= "?type=" & CurrentAddressType & "&id=" & AddressId
        End If

        If Not String.IsNullOrEmpty(act) Then
            url &= IIf(url.Contains("?"), "&", "?")
            url &= "act=" & act
        End If

        Return url
    End Function
End Class


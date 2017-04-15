Imports Components
Imports DataLayer
Imports System

Partial Class billingint
	Inherits SitePage
    Private Member As MemberRow
    Private MemberId As Integer = 0
    Private act As String = String.Empty
    Private ctr As String = String.Empty
    Private bOldGuest As Boolean = False
    Private o As StoreOrderRow

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
        If GetQueryString("act") IsNot Nothing AndAlso GetQueryString("act") = "guest" Then
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
                If MemberId AndAlso HasAccess() Then
                    Response.Redirect("/store/cart.aspx")
                Else
                    o = Cart.Order
                    Dim InternationalOrderPriceMin As Double = SysParam.GetValue("InternationalOrderPriceMin")

                    If InternationalOrderPriceMin <> Nothing AndAlso o.SubTotal < InternationalOrderPriceMin Then
                        Utility.Common.OrderLog(0, "Page Load > Checkout as Guest > International Order Price Min", {"SubTotal=" & o.SubTotal})
                        ShowError(String.Format(Resources.Alert.OrderMinOutsideUS, InternationalOrderPriceMin, o.SubTotal), True)
                    Else
                        Utility.Common.OrderLog(0, "Page Load > Checkout as Guest", Nothing)
                    End If
                End If
            Else
                Utility.Common.OrderLog(0, "Page Load", Nothing)
                o = Cart.Order
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
            If Not IsPostBack Then
                BindData()
                LoadMetaData(DB, "/store/billingint.aspx")
            End If
        End If

    End Sub
    Private Sub BindData()

        drpBillingCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
        drpBillingCountry.DataBind()
        ' drpBillingCountry.Items.Remove(drpBillingCountry.Items.FindByValue("US"))
        drpBillingCountry.Items.Remove(drpBillingCountry.Items.FindByValue("AK"))
        drpBillingCountry.Items.Remove(drpBillingCountry.Items.FindByValue("HI"))
        drpBillingCountry.Items.Insert(0, New ListItem("", ""))

        If MemberId > 0 Then
            If AddressId > 1 Then ''edit address
                LoadEditAddress()
            Else
                LoadDefaultAddress()
            End If
        Else
            If act = "guest" Then
                drpBillingCountry.SelectedValue = ctr
            End If
        End If

    End Sub
    Private Sub LoadEditAddress()
        Dim address As MemberAddressRow = MemberAddressRow.GetRow(DB, AddressId)
        BindBillingAddress(address)
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
            txtBillingAddress2.Text = address.Address2.Trim()
        End If

        If Not String.IsNullOrEmpty(address.City) Then
            txtBillingCity.Text = address.City.Trim()
        End If
        If Not String.IsNullOrEmpty(address.Region) Then
            txtBillingRegion.Text = address.Region
        End If

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

        If Not String.IsNullOrEmpty(address.Phone) Then
            txtBillingPhone.Text = Trim(address.Phone)
        End If
        If Not String.IsNullOrEmpty(address.PhoneExt) Then
            txtBillingPhone.Text &= " " & Trim(address.PhoneExt)
        End If
        If Not String.IsNullOrEmpty(address.Zip) Then
            txtBillingZip.Text = Trim(address.Zip)
        End If

        'If Not String.IsNullOrEmpty(address.DaytimePhone) Then
        '    txtBillingDaytimePhone.Text = Trim(address.DaytimePhone)
        'End If
        'If Not String.IsNullOrEmpty(address.DaytimePhoneExt) Then
        '    txtBillingDaytimePhone.Text &= " " & Trim(address.DaytimePhoneExt)
        'End If

        drpBillingCountry.SelectedValue = IIf(Not String.IsNullOrEmpty(ctr), ctr, address.Country)
    End Sub
    Private Sub LoadDefaultAddress()
        Dim Billing As MemberAddressRow = Nothing
        If Cart.Order.BillingAddressId > 0 Then
            Billing = MemberAddressRow.GetRow(DB, Cart.Order.BillingAddressId)
        Else
            Billing = MemberAddressRow.GetAddressByType(DB, Member.MemberId, Utility.Common.MemberAddressType.Billing.ToString())
        End If


        If Cart.Order.BillToCity = Nothing And Billing.Country <> "US" Then
            BindBillingAddress(Billing)
        Else
            Dim tmlAddress As New MemberAddressRow
            tmlAddress.Company = Cart.Order.BillToSalonName
            tmlAddress.FirstName = Cart.Order.BillToName
            tmlAddress.LastName = Cart.Order.BillToName2
            tmlAddress.Address1 = Cart.Order.BillToAddress
            tmlAddress.Address2 = Cart.Order.BillToAddress2
            tmlAddress.Country = Cart.Order.BillToCountry
            tmlAddress.City = Cart.Order.BillToCity
            tmlAddress.Region = Cart.Order.BillToCounty
            tmlAddress.Zip = Cart.Order.BillToZipcode
            tmlAddress.Fax = Cart.Order.BillToFax
            tmlAddress.Phone = Cart.Order.BillToPhone
            tmlAddress.PhoneExt = Cart.Order.BillToPhoneExt
            tmlAddress.DaytimePhone = Cart.Order.BillToDaytimePhone
            tmlAddress.DaytimePhoneExt = Cart.Order.BillToDaytimePhoneExt
            tmlAddress.Email = Cart.Order.Email
            BindBillingAddress(tmlAddress)
        End If
    End Sub
    Private Function GetUSLink() As String
        Dim url As String = "/store/billing.aspx"
        If Not String.IsNullOrEmpty(CurrentAddressType) AndAlso AddressId > 0 Then
            url = url & "?type=" & CurrentAddressType & "&id=" & AddressId
        End If

        If Not String.IsNullOrEmpty(act) Then
            url &= IIf(url.Contains("?"), "&", "?")
            url &= "act=" & act
        End If
        Return url
    End Function

    Private Function GetBackURL() As String
        If Not Session("isUpdateAddress") Is Nothing AndAlso Session("isUpdateAddress") = 1 Then
            Session.Remove("isUpdateAddress")
            Return "/store/payment.aspx"
        End If

        Dim result As String = String.Empty
        If Not (Request.QueryString("url") Is Nothing) Then
            result = Request.QueryString("url")
        End If
        If (String.IsNullOrEmpty(result)) Then
            result = "/store/payment.aspx"
        End If
        Return result
    End Function


    Protected Sub ServerCheckPhoneInternational(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        Utility.Common.CheckPhoneInternational(cusvPhoneBillingInt, e)
    End Sub
    'Protected Sub ServerCheckDayPhoneInternational(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
    '    Utility.Common.CheckPhoneInternational(cusvDayPhoneBillingInt, e)
    'End Sub
    'Protected Sub ServerCheckFaxInternational(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
    '    e.IsValid = Utility.Common.CheckFaxInternational(e.Value)
    'End Sub
    Private Sub GetOrderAddressFromUI(ByRef o As StoreOrderRow)
        o.CheckoutPage = "Billing"
        o.BillToSalonName = txtBillingCompany.Text.Trim
        o.BillToName = txtBillingFirstName.Text.Trim()
        o.BillToName2 = txtBillingLastName.Text.Trim()
        o.BillToAddress = txtBillingAddress1.Text.Trim()
        o.BillToAddress2 = txtBillingAddress2.Text.Trim()
        o.BillToCity = txtBillingCity.Text.Trim()
        o.BillToCountry = drpBillingCountry.SelectedValue
        o.BillToCounty = txtBillingRegion.Text.Trim()
        o.BillToCustomerId = Member.CustomerId
        'o.BillToFax = txtBillingFax.Text.Trim
        o.BillToPhone = txtBillingPhone.Text.Trim
        o.BillToPhoneExt = String.Empty
        'o.BillToDaytimePhone = txtBillingDaytimePhone.Text.Trim
        o.BillToDaytimePhoneExt = String.Empty
        o.BillToZipcode = txtBillingZip.Text

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
        o.ShipToPhoneExt = String.Empty
        o.ShipToZipcode = o.BillToZipcode

        o.IsSameAddress = True
        o.IsSignatureConfirmation = False
        o.ShipToAddressType = 3
        o.SignatureConfirmation = 0
        o.ResidentialFee = 0
        If Trim(Member.SalesTaxExemptionNumber) <> String.Empty Then
            o.IsTaxExempt = True
            o.TaxExemptId = Member.SalesTaxExemptionNumber
        Else
            o.IsTaxExempt = False
            o.TaxExemptId = Nothing
        End If
    End Sub

    Private Sub ShowError(ByVal msg As String, ByVal bFix As Boolean)
        If bFix Then
            ltrLoadMsg.Text = String.Format("<span style='display:none;' id='loadMsgFix'>{0}</span>", msg)
        Else
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType(), "showError", "ShowError('" & msg & "');", True)
        End If

    End Sub
    Protected Sub btnContinue_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinue.Click
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
                    Member.IsSameDefaultAddress = True
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

                    If MemberId <= 0 Then
                        DB.RollbackTransaction()
                        AddError("An error has occurred while trying to process your request. Please, try again later")
                        Email.SendError("ToError500", " Register Error", Request.Url.AbsoluteUri.ToString() & "<br>MemberId=0")
                        Exit Sub
                    End If

                    o.Email = txtEmail.Text.Trim()
                    GetOrderAddressFromUI(o)

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
                    Email.SendError("ToError500", "Register Error", Request.Url.AbsoluteUri.ToString() & "<br>Exception: " & ex.Message)
                End Try

                Cart.RecalculateOrderDetail("billingint.btnContinue_Click > guest")
            Else
                If bOldGuest Then
                    o.MemberId = MemberId
                End If
                o.Email = txtEmail.Text.Trim()
                If (o.CarrierType = Utility.Common.DefaultShippingId) Then
                    o.CarrierType = Utility.Common.USPSPriorityShippingId
                End If

                GetOrderAddressFromUI(o)
                If Utility.Common.IsShipToRussia(o) Then
                    ShowError(Resources.Alert.Russia, False)
                    Exit Sub
                End If

                UpdateMemberAddressByOrderAddress(o)

                Dim sql As String = "" 'String.Format("UPDATE StoreOrder SET BillingAddressId={1}, ShippingAddressId={2} WHERE OrderId={0}", o.OrderId, o.BillingAddressId, o.ShippingAddressId)
                If Member IsNot Nothing AndAlso o.OrderId <> Member.LastOrderId Then
                    sql &= String.Format("; UPDATE Member SET LastOrderId = {0} WHERE MemberId = {1}; UPDATE StoreOrder SET MemberId = 0 WHERE OrderId={2} AND OrderNo IS NULL", o.OrderId, MemberId, Member.LastOrderId)
                End If
                DB.ExecuteSQL(sql)

                Cart.RecalculateOrderDetail("billingint.btnContinue_Click")
                DB.Close()
            End If

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
    Private Sub UpdateMemberBillingAddressFromOrder(ByVal billingAddress As MemberAddressRow, ByVal order As StoreOrderRow)
        If Not (billingAddress Is Nothing) Then
            billingAddress.Address1 = order.BillToAddress
            billingAddress.Address2 = order.BillToAddress2
            billingAddress.City = order.BillToCity
            billingAddress.Company = order.BillToSalonName
            billingAddress.Country = order.BillToCountry
            billingAddress.DaytimePhone = order.BillToDaytimePhone
            billingAddress.DaytimePhoneExt = order.BillToDaytimePhoneExt
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

            'Update customer
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
                If (currentAddress.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString()) Then
                    order.ShippingAddressId = currentAddress.AddressId
                Else
                    otherAddress = MemberAddressRow.GetAddressByType(DB, order.MemberId, Utility.Common.MemberAddressType.Shipping.ToString())
                    UpdateMemberShippingAddressFromOrder(otherAddress, order)
                End If
                UpdateMemberBillingAddressFromOrder(currentAddress, order)
            ElseIf CurrentAddressType = Utility.Common.MemberAddressType.Shipping.ToString() Then
                If (currentAddress.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString()) Then
                    order.BillingAddressId = currentAddress.AddressId
                Else 'Default shipping Adddress
                    otherAddress = MemberAddressRow.GetAddressByType(DB, order.MemberId, Utility.Common.MemberAddressType.Billing.ToString())
                    UpdateMemberBillingAddressFromOrder(otherAddress, order)
                End If
                UpdateMemberShippingAddressFromOrder(currentAddress, order)
            End If
        Else
            Dim shippingAddress As MemberAddressRow = Nothing
            If Cart.Order.ShippingAddressId > 0 Then
                shippingAddress = MemberAddressRow.GetRow(DB, order.ShippingAddressId)
            ElseIf act = "guest" AndAlso bOldGuest = False Then
                shippingAddress = New MemberAddressRow()
            Else
                shippingAddress = MemberAddressRow.GetAddressByType(DB, Member.MemberId, Utility.Common.MemberAddressType.Shipping.ToString())
            End If
            UpdateMemberShippingAddressFromOrder(shippingAddress, order)

            If shippingAddress.AddressType = Utility.Common.MemberAddressType.AddressBook.ToString() Then
                order.BillingAddressId = shippingAddress.AddressId
            Else
                Dim billingAddress As MemberAddressRow = Nothing
                If Cart.Order.BillingAddressId > 0 Then
                    billingAddress = MemberAddressRow.GetRow(DB, order.BillingAddressId)
                ElseIf act = "guest" AndAlso bOldGuest = False Then
                    billingAddress = New MemberAddressRow()
                Else
                    billingAddress = MemberAddressRow.GetAddressByType(DB, Member.MemberId, Utility.Common.MemberAddressType.Billing.ToString())
                End If
                UpdateMemberBillingAddressFromOrder(billingAddress, order)
                order.BillingAddressId = billingAddress.AddressId
            End If
        End If

        If act <> "guest" Then
            Dim objContact As CustomerContactRow = CustomerContactRow.GetRow(DB, Member.Customer.ContactId)
            If Not objContact Is Nothing Then
                objContact.DoExport = True
                objContact.Update()
            End If

            DB.ExecuteSQL("Update Member set IsSameDefaultAddress=" & (IIf(order.IsSameAddress = True, 1, 0)) & " where MemberId=" & order.MemberId)
        End If

        order.Update()
    End Sub
    Protected Sub lbtnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnBack.Click
        Response.Redirect(GetBackURL())
    End Sub

    'Protected Sub lbtnUS_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnUS.Click
    '    'Session("IsInternational") = "N"
    '    Response.Redirect(GetUSLink())
    'End Sub
End Class

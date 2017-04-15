

Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System

Public Class members_address
    Inherits SitePage

    Private Member As MemberRow = Nothing
    Protected css As String = String.Empty
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HasAccess() Then
            Response.Redirect("/members/login.aspx")
        End If

        Member = MemberRow.GetRow(Utility.Common.GetCurrentMemberId())
        If Not IsPostBack Then
            LoadDropdown()
        End If

        If Not IsPostBack Then LoadFormData()
        SwitchValidationControls()

        chkSameAsBilling_CheckedChanged(sender, e)
        drpBillingCountry_SelectedIndexChanged(sender, e)

    End Sub

    Private Sub LoadFormData()
        Dim MemberBillingAddress As MemberAddressRow = MemberAddressRow.GetAddressByType(DB, Member.MemberId, Utility.Common.MemberAddressType.Billing.ToString())
        Dim MemberShippingAddress As MemberAddressRow = MemberAddressRow.GetAddressByType(DB, Member.MemberId, Utility.Common.MemberAddressType.Shipping.ToString())

        txtBillingFirstName.Text = Trim(MemberBillingAddress.FirstName)
        txtBillingLastName.Text = Trim(MemberBillingAddress.LastName)
        txtBillingAddress1.Text = Trim(MemberBillingAddress.Address1)
        txtBillingAddress2.Text = Trim(MemberBillingAddress.Address2)
        txtBillingCompany.Text = Trim(MemberBillingAddress.Company)
        txtBillingCity.Text = Trim(MemberBillingAddress.City)
        txtBillingZip.Text = Trim(MemberBillingAddress.Zip)
        zipcode.InnerHtml = Trim(MemberBillingAddress.Zip) + "|" + Trim(MemberShippingAddress.Zip)
        txtBillingRegion.Text = Trim(MemberBillingAddress.Region)
        txtBillingFax.Text = Trim(MemberBillingAddress.Fax)
        txtBillingEmail.Text = Trim(MemberBillingAddress.Email)
        drpBillingCountry.SelectedValue = Trim(MemberBillingAddress.Country)
        If MemberBillingAddress.Country = "US" Or MemberBillingAddress.Country = "" Then
            txtBillingPhone.Text = Trim(MemberBillingAddress.Phone) & " " & MemberBillingAddress.PhoneExt
            drpBillingState.SelectedValue = MemberBillingAddress.State
        Else
            txtBillingPhone.Text = Trim(MemberBillingAddress.Phone)
        End If

        If Member.IsSameDefaultAddress = True Then chkSameAsBilling.Checked = True Else chkSameAsBilling.Checked = False

        If (MemberShippingAddress.Country = "US") Then
            If (Member.IsSameDefaultAddress) Then
                txtShippingFirstName.Text = Trim(MemberBillingAddress.FirstName)
                txtShippingLastName.Text = Trim(MemberBillingAddress.LastName)
                txtShippingAddress1.Text = Trim(MemberBillingAddress.Address1)
                txtShippingAddress2.Text = Trim(MemberBillingAddress.Address2)
                txtShippingCity.Text = Trim(MemberBillingAddress.City)
                txtShippingZip.Text = Trim(MemberBillingAddress.Zip)
                txtShippingRegion.Text = Trim(MemberBillingAddress.Region)
                txtShippingPhone.Text = Trim(MemberBillingAddress.Phone) & " " & MemberBillingAddress.PhoneExt
                drpShippingCountry.SelectedValue = Trim(MemberBillingAddress.Country)
                If MemberShippingAddress.Country = "US" Then
                    drpShippingState.SelectedValue = drpBillingState.SelectedValue
                End If
                txtShippingCompany.Text = Trim(MemberBillingAddress.Company)
            Else
                txtShippingFirstName.Text = Trim(MemberShippingAddress.FirstName)
                txtShippingLastName.Text = Trim(MemberShippingAddress.LastName)
                txtShippingAddress1.Text = Trim(MemberShippingAddress.Address1)
                txtShippingAddress2.Text = Trim(MemberShippingAddress.Address2)
                txtShippingCity.Text = Trim(MemberShippingAddress.City)
                txtShippingZip.Text = Trim(MemberShippingAddress.Zip)
                txtShippingRegion.Text = Trim(MemberShippingAddress.Region)
                txtShippingPhone.Text = Trim(MemberShippingAddress.Phone) & " " & MemberShippingAddress.PhoneExt
                drpShippingCountry.SelectedValue = Trim(MemberShippingAddress.Country)
                txtShippingCompany.Text = Trim(MemberShippingAddress.Company)
            End If
            drpShippingState.SelectedValue = MemberShippingAddress.State
        Else
            drpShippingState.Visible = False
            dvShippingState.Visible = False
            dvShippingRegion.Visible = True
            txtShippingFirstName.Text = Trim(MemberShippingAddress.FirstName)
            txtShippingLastName.Text = Trim(MemberShippingAddress.LastName)
            txtShippingAddress1.Text = Trim(MemberShippingAddress.Address1)
            txtShippingAddress2.Text = Trim(MemberShippingAddress.Address2)
            txtShippingCity.Text = Trim(MemberShippingAddress.City)
            txtShippingZip.Text = Trim(MemberShippingAddress.Zip)
            txtShippingRegion.Text = Trim(MemberShippingAddress.Region)
            txtShippingPhone.Text = Trim(MemberShippingAddress.Phone)
            drpShippingCountry.SelectedValue = Trim(MemberShippingAddress.Country)
            txtShippingCompany.Text = Trim(MemberShippingAddress.Company)
            'BindShippingPhone(False, Trim(MemberShippingAddress.Phone))
        End If

        Dim MailingMemberId As Integer = DB.ExecuteScalar("SELECT MemberId FROM MailingMember WHERE Email=" & DB.Quote(Member.Customer.Email))
        Dim dbMailingMember As MailingMemberRow
        dbMailingMember = MailingMemberRow.GetRow(DB, MailingMemberId)
        ''If dbMailingMember.Status = "ACTIVE" Then rbtnNewsletterYes.Checked = True Else rbtnNewsletterNo.Checked = True
        'chkDORRegistered.Checked = Member.DeptOfRevenueRegistered
        ''If Member.DeptOfRevenueRegistered Then litJS.Text &= ""
        ''Me.dtLicenseExpirationDate.Value = Member.LicenseExpirationDate
        ''drpLicenseExpirationMonth.SelectedValue = Member.LicenseExpirationDate.Month
        ''drpLicenseExpirationYear.SelectedValue = Member.LicenseExpirationDate.Year

        ''Me.dpExpectedGraduationDate.Value = Member.ExpectedGraduationDate
        ''drpExpectedGraduationMonth.SelectedValue = Member.ExpectedGraduationDate.Month
        ''drpExpectedGraduationYear.SelectedValue = Member.ExpectedGraduationDate.Year

        ''Me.txtLicenseNumber.Text = Member.LicenseNumber
        ''Me.drpLicenseState.SelectedValue = Member.LicenseState
        ''Me.txtTeacherLicenseNumber.Text = Member.LicenseNumber
        ''Me.txtTeacherSchoolContactName.Text = Member.ContactName
        ''Me.txtTeacherSchoolContactPhone.Text = Member.ContactPhone
        ''Me.txtStudentNumber.Text = Member.StudentNumber
        '' '' Me.txtSchoolName.Text = Member.SchoolName
        ''Me.txtSchoolContactName.Text = Member.ContactName
        ''Me.txtSchoolContactPhone.Text = Member.ContactPhone

        'Me.txtAuthorizedSignatureName1.Text = Member.AuthorizedSignatureName
        'Me.txtAuthorizedSignatureName2.Text = Me.txtAuthorizedSignatureName1.Text
        'Me.txtTitle.Text = Member.AuthorizedSignatureTitle
        'Me.lblDate.Text = Member.AuthorizedSignatureDate
        'Me.chkDORRegistered.Checked = Member.DeptOfRevenueRegistered
        'Me.chkInformationAccuracyAcceptance.Checked = Member.InformationAccuracyAgreement
        'Me.chkResaleAgreement.Checked = Member.ResaleAcceptance
        'txtTaxNumber.Text = Member.SalesTaxExemptionNumber
    End Sub

    Private Sub LoadDropdown()
        Dim ds As DataSet = Nothing
        Dim listItem = StateRow.GetStateList()
        drpBillingState.Items.AddRange(listItem.ToArray())
        drpBillingState.DataBind()
        drpBillingState.Items.Insert(0, New ListItem("", ""))


        drpShippingState.Items.AddRange(listItem.ToArray())
        drpShippingState.DataBind()
        drpShippingState.Items.Insert(0, New ListItem("", ""))

        drpBillingState.Items.Remove(drpBillingState.Items.FindByValue("PR"))
        drpShippingState.Items.Remove(drpShippingState.Items.FindByValue("PR"))
        drpBillingState.Items.Remove(drpBillingState.Items.FindByValue("VI"))
        drpShippingState.Items.Remove(drpShippingState.Items.FindByValue("VI"))

        ''drpLicenseState.DataSource = ds
        ''drpLicenseState.DataTextField = "StateName"
        ''drpLicenseState.DataValueField = "StateCode"
        ''drpLicenseState.DataBind()
        ''drpLicenseState.Items.Insert(0, New ListItem("", ""))

        drpBillingCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
        drpBillingCountry.DataBind()
        ''drpBillingCountry.Attributes.Add("onchange", "UpdateStates('','',this)")
        drpBillingCountry.SelectedValue = "US"
        ''''drpBillingCountry.Attributes.Add("onchange", "UpdateStates('" & bartxtBillingRegion.ClientID & "','" & bardrpBillingState.ClientID & "',this)")
        drpBillingCountry.Items.Insert(0, New ListItem("", ""))

        drpShippingCountry.DataSource = ds
        drpShippingCountry.DataTextField = "CountryName"
        drpShippingCountry.DataValueField = "CountryCode"
        drpShippingCountry.DataBind()
        ''drpShippingCountry.Attributes.Add("onchange", "UpdateStates('','',this)")
        drpShippingCountry.SelectedValue = "US"
        ''''drpShippingCountry.Attributes.Add("onchange", "UpdateStates('" & bartxtShippingRegion.ClientID & "','" & bardrpShippingState.ClientID & "',this)")

        drpShippingCountry.Items.Remove(drpShippingCountry.Items.FindByValue("AK"))
        drpShippingCountry.Items.Remove(drpShippingCountry.Items.FindByValue("HI"))
        drpBillingCountry.Items.Remove(drpBillingCountry.Items.FindByValue("AK"))
        drpBillingCountry.Items.Remove(drpBillingCountry.Items.FindByValue("HI"))

        ' ''drpLicenseExpiration
        ''drpLicenseExpirationMonth.Items.Add("")
        ''drpLicenseExpirationYear.Items.Add("")
        ''drpExpectedGraduationMonth.Items.Add("")
        ''drpExpectedGraduationYear.Items.Add("")

        ''Dim d As DateTime = Convert.ToDateTime("01/01/" & Now.Year)
        ''For i As Int16 = 1 To 12
        ''    drpLicenseExpirationMonth.Items.Add(New ListItem(d.ToString("MMMM"), d.Month))
        ''    drpExpectedGraduationMonth.Items.Add(New ListItem(d.ToString("MMMM"), d.Month))
        ''    d = d.AddMonths(1)

        ''    If i <= 11 Then
        ''        drpLicenseExpirationYear.Items.Add(New ListItem(d.ToString("yyyy"), d.Year))
        ''        drpExpectedGraduationYear.Items.Add(New ListItem(d.ToString("yyyy"), d.Year))
        ''        d = d.AddYears(1)
        ''    End If
        ''Next
    End Sub
    ' '''''''''''''''
    Protected Sub ServerCheckPhoneUS(ByVal sender As Object, ByVal e As ServerValidateEventArgs)

        If Not String.IsNullOrEmpty(e.Value) Then
            e.IsValid = Utility.Common.CheckUSPhoneValid(e.Value)
        End If

    End Sub

    Protected Sub ServerCheckPhoneInternational(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        Utility.Common.CheckPhoneInternational(cusvPhoneBillingInt, e)
    End Sub
    Protected Sub ServerCheckFaxInternational(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        e.IsValid = Utility.Common.CheckFaxInternational(e.Value)
    End Sub
    
    Protected Sub ServerCheckEmail(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        Dim r As New System.Text.RegularExpressions.Regex("[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?")
        e.IsValid = r.IsMatch(txtBillingEmail.Text)
    End Sub


    Private Function CheckAddressMsg(ByVal Zip As String, ByVal City As String, ByVal State As String, ByVal AddressType As String) As String
        Dim msg As String = ""
        Dim result As Integer = 0
        Dim collection As ZipCodeCollection = ZipCodeRow.CheckAddress(Zip, City, State, result)
        If result < 0 Then
            msg = String.Format("{0} address is not valid. Please verify State and Zipcode are correct.", AddressType)
        ElseIf result = 0 Then
            For Each z As ZipCodeRow In collection
                msg &= ""
            Next
        End If

        Return msg
    End Function


    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click

        Dim bRedirect As Boolean = True, bError As Boolean = False
        If drpShippingCountry.SelectedValue <> "US" Then rqdrpShippingState.Enabled = False
        If drpBillingCountry.SelectedValue <> "US" Then rqdrpBillingState.Enabled = False

        If (Me.chkSameAsBilling.Checked And drpBillingState.SelectedValue = "IL") _
         Or (Not Me.chkSameAsBilling.Checked And drpShippingState.SelectedValue = "IL") Then

            'If Me.chkDORRegistered.Checked Then

            '    If Not Me.chkResaleAgreement.Checked Then
            '        AddError("You must check all tax agreements - NEEDS COPY ")
            '        bError = True
            '    End If

            '    If Not Me.chkInformationAccuracyAcceptance.Checked Then
            '        AddError("You must check all tax agreements - NEEDS COPY ")
            '        bError = True
            '    End If

            'End If
        End If

        Page.Validate()

        If drpBillingCountry.SelectedValue = "US" Then
            Dim strBillingError As String = CheckAddressMsg(txtBillingZip.Text, txtBillingCity.Text, drpBillingState.SelectedValue, "Billing")
            If Not String.IsNullOrEmpty(strBillingError) Then
                AddError(strBillingError)
                bError = True
            End If

            If chkSameAsBilling.Checked = False Then
                Dim strShippingError As String = CheckAddressMsg(txtShippingZip.Text, txtShippingCity.Text, drpShippingState.SelectedValue, "Shipping")
                If Not String.IsNullOrEmpty(strShippingError) Then
                    AddError(strShippingError)
                    bError = True
                End If
            End If
        End If

        If Not Page.IsValid Or bError Then
            Exit Sub
        End If
        Dim SetupBoards As Boolean = False
        Try
            DB.BeginTransaction()

            ' Create Membership
            Dim CurrentMemberId As Integer
            Dim MemberBillingAddress As MemberAddressRow
            Dim MemberShippingAddress As MemberAddressRow
            Member = MemberRow.GetRow(Session("MemberId"))
            Member.DB = DB
            CurrentMemberId = Session("memberId")
            MemberBillingAddress = MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & CurrentMemberId & " AND Label = 'Default Billing Address'"))
            MemberShippingAddress = MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & CurrentMemberId & " AND Label = 'Default Shipping Address'"))
            If chkSameAsBilling.Checked = True Then Member.IsSameDefaultAddress = True Else Member.IsSameDefaultAddress = False
            Member.ModifyDate = Now

            If chkSameAsBilling.Checked = True Then Member.IsSameDefaultAddress = True Else Member.IsSameDefaultAddress = False
            If drpBillingCountry.SelectedValue = "US" Or drpBillingCountry.SelectedValue = "" Then
                'If chkProfessionalStatus.Checked Then
                '    Member.ProfessionalStatus = 3
                'Else
                '    Member.ProfessionalStatus = Nothing
                'End If
                Member.ProfessionalStatus = 3
            Else
                Member.ProfessionalStatus = Nothing
            End If

            'Tax info check
            If (Me.chkSameAsBilling.Checked And drpBillingState.SelectedValue = "IL") OrElse (Not Me.chkSameAsBilling.Checked And drpShippingState.SelectedValue = "IL") Then
                'Member.AuthorizedSignatureName = Me.txtAuthorizedSignatureName1.Text
                'Member.AuthorizedSignatureTitle = Me.txtTitle.Text
                'Member.AuthorizedSignatureDate = Me.lblDate.Text
                'Member.DeptOfRevenueRegistered = Me.chkDORRegistered.Checked
                'Member.InformationAccuracyAgreement = Me.chkInformationAccuracyAcceptance.Checked
                'Member.ResaleAcceptance = Me.chkResaleAgreement.Checked
                'Member.SalesTaxExemptionNumber = txtTaxNumber.Text
                If Member.MemberId <> Nothing Then
                    Member.Update(DB)
                Else
                    CurrentMemberId = Member.Insert(False)
                    SetupBoards = True
                End If

                Member.Customer.SalesTaxExemptionNumber = Member.SalesTaxExemptionNumber
            Else
                Member.AuthorizedSignatureName = Nothing
                Member.AuthorizedSignatureTitle = Nothing
                Member.AuthorizedSignatureDate = Nothing
                Member.DeptOfRevenueRegistered = Nothing
                Member.InformationAccuracyAgreement = Nothing
                Member.ResaleAcceptance = Nothing
                Member.SalesTaxExemptionNumber = Nothing
                If Member.MemberId <> Nothing Then
                    Member.Update(DB)
                Else
                    CurrentMemberId = Member.Insert(False)
                    SetupBoards = True
                End If
                Member.Customer.SalesTaxExemptionNumber = Nothing
            End If

            ' Create Default Billing Address
            MemberBillingAddress.MemberId = CurrentMemberId
            MemberBillingAddress.Label = "Default Billing Address"
            MemberBillingAddress.AddressType = "Billing"
            MemberBillingAddress.FirstName = txtBillingFirstName.Text
            MemberBillingAddress.LastName = txtBillingLastName.Text
            MemberBillingAddress.Address1 = txtBillingAddress1.Text
            MemberBillingAddress.Address2 = txtBillingAddress2.Text
            MemberBillingAddress.Company = txtBillingCompany.Text
            MemberBillingAddress.City = txtBillingCity.Text
            MemberBillingAddress.Email = txtBillingEmail.Text
            If drpBillingCountry.SelectedValue = "US" Then
                MemberBillingAddress.State = drpBillingState.SelectedValue
                MemberBillingAddress.Region = ""
            Else
                MemberBillingAddress.State = ""
                MemberBillingAddress.Region = txtBillingRegion.Text
            End If
            MemberBillingAddress.Zip = txtBillingZip.Text
            MemberBillingAddress.Country = drpBillingCountry.SelectedValue
            If MemberBillingAddress.Country <> "US" Then
                MemberBillingAddress.Phone = txtBillingPhone.Text
                MemberBillingAddress.PhoneExt = String.Empty
            Else
                Dim array() As String = txtBillingPhone.Text.Trim().Split(" ")
                Try
                    MemberBillingAddress.Phone = array(0)
                    MemberBillingAddress.PhoneExt = array(1).Trim()
                Catch ex As Exception
                End Try
            End If
            MemberBillingAddress.Fax = txtBillingFax.Text
            MemberBillingAddress.IsDefault = True
            ' Update Customer
            Member.Customer.Address = MemberBillingAddress.Address1
            Member.Customer.Address2 = MemberBillingAddress.Address2
            Member.Customer.Name = MemberBillingAddress.FirstName
            Member.Customer.Name2 = MemberBillingAddress.LastName
            Member.Customer.City = MemberBillingAddress.City
            Member.Customer.Zipcode = MemberBillingAddress.Zip
            If MemberBillingAddress.Country <> "US" Then
                Member.Customer.County = MemberBillingAddress.Region
            Else
                Member.Customer.County = MemberBillingAddress.State
            End If
            Member.Customer.Phone = MemberBillingAddress.Phone
            Member.Customer.PhoneExt = MemberBillingAddress.PhoneExt
            Member.Customer.ContactNo = Member.CustomerContact.ContactNo
            Member.Customer.ContactId = Member.CustomerContact.ContactId
            Member.Customer.DoExport = True
            Member.Customer.Update()

            ' Update CustomerContact
            Member.CustomerContact.Address = MemberBillingAddress.Address1
            Member.CustomerContact.Address2 = MemberBillingAddress.Address2
            Member.CustomerContact.CustName = MemberBillingAddress.FirstName
            Member.CustomerContact.CustName2 = MemberBillingAddress.LastName
            Member.CustomerContact.Phone = MemberBillingAddress.Phone
            Member.CustomerContact.City = MemberBillingAddress.City
            Member.CustomerContact.PostCode = MemberBillingAddress.Zip
            If MemberBillingAddress.Country = "US" Then Member.CustomerContact.County = MemberBillingAddress.State
            Member.CustomerContact.CountryCode = MemberBillingAddress.Country
            Member.CustomerContact.Email = MemberBillingAddress.Email
            Member.CustomerContact.DoExport = True
            Member.CustomerContact.Update()

            MemberShippingAddress.MemberId = CurrentMemberId
            MemberShippingAddress.Label = "Default Shipping Address"
            MemberShippingAddress.AddressType = "Shipping"
            If chkSameAsBilling.Checked = True Then
                MemberShippingAddress.FirstName = txtBillingFirstName.Text
                MemberShippingAddress.LastName = txtBillingLastName.Text
                MemberShippingAddress.Address1 = txtBillingAddress1.Text
                MemberShippingAddress.Address2 = txtBillingAddress2.Text
                MemberShippingAddress.City = txtBillingCity.Text
                If drpBillingCountry.SelectedValue = "US" Then
                    MemberShippingAddress.State = drpBillingState.SelectedValue
                    MemberShippingAddress.Region = ""
                Else
                    MemberShippingAddress.State = ""
                    MemberShippingAddress.Region = txtBillingRegion.Text
                End If
                MemberShippingAddress.Zip = txtBillingZip.Text
                MemberShippingAddress.Company = txtBillingCompany.Text
                MemberShippingAddress.Country = drpBillingCountry.SelectedValue
                MemberShippingAddress.Phone = MemberBillingAddress.Phone
                MemberShippingAddress.PhoneExt = MemberBillingAddress.PhoneExt
            Else
                MemberShippingAddress.FirstName = txtShippingFirstName.Text
                MemberShippingAddress.LastName = txtShippingLastName.Text
                MemberShippingAddress.Address1 = txtShippingAddress1.Text
                MemberShippingAddress.Address2 = txtShippingAddress2.Text
                MemberShippingAddress.City = txtShippingCity.Text
                If drpShippingCountry.SelectedValue = "US" Then MemberShippingAddress.State = drpShippingState.SelectedValue
                MemberShippingAddress.Zip = txtShippingZip.Text
                MemberShippingAddress.Region = txtShippingRegion.Text
                MemberShippingAddress.Company = txtShippingCompany.Text
                MemberShippingAddress.Country = drpShippingCountry.SelectedValue
                If MemberShippingAddress.Country <> "US" Then
                    MemberShippingAddress.Phone = txtShippingPhone.Text
                    MemberShippingAddress.PhoneExt = String.Empty
                Else
                    Dim array() As String = txtShippingPhone.Text.Trim().Split(" ")
                    Try
                        MemberShippingAddress.Phone = array(0)
                        MemberShippingAddress.PhoneExt = array(1).Trim()
                    Catch ex As Exception
                    End Try
                End If
            End If
            MemberShippingAddress.DoExport = True
            MemberBillingAddress.DoExport = True
            MemberShippingAddress.Update()
            MemberBillingAddress.Update()
            StoreOrderRow.CopyBillShipAddressFromMemberAddress(DB, Session("memberId"))

            DB.CommitTransaction()
            HttpContext.Current.Session("isUpdateAddress") = Nothing
        Catch ex As Exception
            DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
            bRedirect = False
        End Try

        Dim dbMember As MemberRow = MemberRow.GetRow(Session("MemberId"))
        If Not dbMember.LastOrderId = Nothing Then
            Dim dbOrder As StoreOrderRow = StoreOrderRow.GetRow(DB, dbMember.LastOrderId)
            If dbOrder.ProcessDate = Nothing AndAlso dbOrder.MemberId = dbMember.MemberId Then
                Session("OrderId") = dbOrder.OrderId
                Utility.CookieUtil.SetTripleDESEncryptedCookie("OrderId", dbOrder.OrderId)
                RecalculateShipping(dbOrder)
            Else
                Session("OrderId") = Nothing
                Utility.CookieUtil.SetTripleDESEncryptedCookie("OrderId", Nothing)
                dbMember.LastOrderId = Nothing
                dbMember.Update(DB)
            End If
        Else
            Session("OrderId") = Nothing
            Utility.CookieUtil.SetTripleDESEncryptedCookie("OrderId", Nothing)
        End If


        If bRedirect Then
            DB.Close()
            Dim url As String = String.Empty
            If Not (Request.QueryString("url") Is Nothing) Then
                url = Request.QueryString("url")
            End If
            If (String.IsNullOrEmpty(url)) Then
                url = "/members/"
            End If
            Response.Redirect(url)
        End If
    End Sub

    Private Sub RecalculateShipping(ByVal o As StoreOrderRow)
        Dim selectedMethod As Integer = Utility.Common.GetDefaultShippingByOrderId(DB, o.OrderId)
        If (o.ShipToCountry = "US" OrElse (o.IsSameAddress AndAlso o.BillToCountry = "US")) AndAlso Not Cart.CheckShippingSpecialUS Then

            If selectedMethod = Nothing Or selectedMethod = Utility.Common.USPSPriorityShippingId Then
                selectedMethod = Utility.Common.DefaultShippingId
            ElseIf selectedMethod = Utility.Common.TruckShippingId.ToString() Then
                Dim countRestrict As Integer = DB.ExecuteScalar("SELECT COUNT(CartItemId) FROM StoreCartItem WHERE [Type] = 'item' AND IsOversize=1 AND OrderId = " & o.OrderId)
                If countRestrict = 0 Then
                    selectedMethod = Utility.Common.DefaultShippingId
                End If
            ElseIf selectedMethod = Utility.Common.PickupShippingId.ToString() AndAlso Not (o.ShipToCountry = "US" AndAlso o.ShipToCounty = "IL") Then
                selectedMethod = Utility.Common.DefaultShippingId
            ElseIf selectedMethod = Utility.Common.FirstClassShippingId Or selectedMethod = Utility.Common.USPSPriorityShippingId Then
                selectedMethod = Utility.Common.DefaultShippingId
            Else
                Dim countRestrict As Integer = DB.ExecuteScalar("Select count(*) from ShippingFedExRestricted where MethodId=" & selectedMethod & " and '" & o.ShipToZipcode & "' between LowValue and HighValue")
                If (countRestrict > 0) Then
                    selectedMethod = Utility.Common.DefaultShippingId
                End If
            End If

            Dim SQL As String
            If selectedMethod > 0 Then
                If selectedMethod = Utility.Common.PickupShippingId.ToString() Then
                    SQL = "update storecartitem set carriertype = " & selectedMethod & " where type = 'item' and orderid = " & Cart.Order.OrderId
                ElseIf selectedMethod = Utility.Common.TruckShippingId.ToString() Then
                    SQL = "update storecartitem set carriertype=" & Utility.Common.TruckShippingId & " where type = 'item' and orderid = " & Cart.Order.OrderId
                Else
                    SQL = "update storecartitem set carriertype = case when isoversize = 1 and " & DB.NullNumber(selectedMethod) & " not in (" & Utility.Common.PickupShippingId & ") then " & Utility.Common.TruckShippingId & " else case when ishazmat = 1 then case when " & DB.Number(selectedMethod) & " in " & DB.NumberMultiple(Utility.Common.NonExpeditedShippingIds) & " then " & DB.Number(selectedMethod) & "	else carriertype end else " & DB.Number(selectedMethod) & " end end where type = 'item' and orderid = " & Cart.Order.OrderId
                End If
            Else
                SQL = "update storecartitem set carriertype = case when isoversize = 1 and " & DB.NullNumber(selectedMethod) & " not in (" & Utility.Common.PickupShippingId & ") then " & Utility.Common.TruckShippingId & " else case when ishazmat = 1 then case when " & DB.Number(selectedMethod) & " in " & DB.NumberMultiple(Utility.Common.NonExpeditedShippingIds) & " then " & DB.Number(selectedMethod) & "	else carriertype end else " & DB.Number(selectedMethod) & " end end where type = 'item' and orderid = " & Cart.Order.OrderId
            End If
            ''''''''End'''''''''
            DB.ExecuteSQL(SQL)
            o.CarrierType = IIf(selectedMethod < 1, Utility.Common.DefaultShippingId, selectedMethod)
            o.Update()
        Else
            o.CarrierType = Utility.Common.USPSPriorityShippingId
        End If
    End Sub

    Private Sub EnableCheckPhoneBillingUS(ByVal status As Boolean)
        status = False
        If status Then

            rqtxtBillingPhone.Enabled = False

        Else

            rqtxtBillingPhone.Enabled = True
            ''''''''''
            If drpBillingCountry.SelectedValue = "US" Then
                cusvPhoneBillingUS.Enabled = True

                cusvFaxBillingUS.Enabled = True
                cusvFaxBillingInt.Enabled = False
                cusvPhoneBillingInt.Enabled = False
            Else
                cusvPhoneBillingUS.Enabled = False

                cusvFaxBillingUS.Enabled = False
                cusvFaxBillingInt.Enabled = True
                cusvPhoneBillingInt.Enabled = True
            End If
        End If
    End Sub
    Private Sub SwitchValidationControls()
        'Me.rqtxtTaxNumber.Enabled = False
        'Me.rqtxtAuthorizedSignatureName1.Enabled = False
        'Me.rqtxtAuthorizedSignatureName2.Enabled = False
        'Me.cvAuthorizedSignatureName.Enabled = False
        'Me.rqtxtTitle.Enabled = False
        Me.rqtxtBillingZip.Enabled = False
        Me.rqextxtBillingZip.Enabled = False
        ''divLicenseInfo.Visible = False
        divShippingAddress.Visible = False
        ''divTeacher.Visible = False
        ''divStudentInfomation.Visible = False

        If drpBillingCountry.SelectedValue <> "US" Then
            EnableCheckPhoneBillingUS(False)
            rqtxtBillingRegion.Enabled = True
            rqdrpBillingState.Enabled = False
            If Not Me.chkSameAsBilling.Checked Then
                rqtxtShippingRegion.Enabled = True
                rqdrpShippingState.Enabled = False
            Else
                rqtxtShippingRegion.Enabled = False
            End If
        Else
            EnableCheckPhoneBillingUS(True)
            rqtxtBillingZip.Enabled = True
            rqtxtBillingRegion.Enabled = False
            rqdrpBillingState.Enabled = True
            cusvFaxBillingInt.Enabled = True
            rqextxtBillingZip.Enabled = True
        End If



        If chkSameAsBilling.Checked = False Then

            ''trPhoneShippingOutSite.Visible = True
            ''trShippingPhoneUS.Visible = False
            ''cusvShippingPhoneUS.Enabled = False
            rqtxtShippingPhone.Enabled = True

            If drpShippingCountry.SelectedValue = "US" Then
                cusvShippingPhoneUS.Enabled = True
                cusvShippingPhoneInt.Enabled = False
            Else
                cusvShippingPhoneUS.Enabled = False
                cusvShippingPhoneInt.Enabled = True
            End If
        Else ''same billing address
            ''cusvShippingPhoneUS.Enabled = False
            rqtxtShippingPhone.Enabled = False

        End If
    End Sub

    'Private Sub ShowPanelProfessionalStatus(ByVal show As Boolean)
    '    If Member Is Nothing Then
    '        divProfessionalStatus.Visible = show
    '        Exit Sub
    '    End If
    '    If MemberRow.IsEbayCustomer(DB, Member.MemberId) Then
    '        divProfessionalStatus.Visible = False
    '    Else
    '        If show Then
    '            divProfessionalStatus.Visible = True
    '        Else
    '            divProfessionalStatus.Visible = False
    '        End If
    '    End If

    'End Sub
    Protected Sub drpBillingCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpBillingCountry.SelectedIndexChanged

        If drpBillingCountry.SelectedValue <> "US" Then
            ''SpanBillingtxt.Visible = True
            ''SpanShippingtxt.Visible = True
            chkSameAsBilling.Checked = True
            chkSameAsBilling.Enabled = False
            ''divIntNote.Visible = True
            divShippingAddress.Visible = False
            'divProfessionalStatus.Visible = False
            'ShowPanelProfessionalStatus(False)


            'State
            drpBillingState.Visible = False
            dvBillingState.Visible = False
            rqdrpBillingState.Visible = False

            'Provice
            dvBillingRegion.Visible = True
            txtBillingRegion.Visible = True
            rqtxtBillingRegion.Visible = True
            'chkProfessionalStatus.Checked = False
            'chkProfessionalStatus.Enabled = False
            css = String.Empty
        Else
            'chkProfessionalStatus.Checked = True
            'chkProfessionalStatus.Enabled = False
            'State
            drpBillingState.Visible = True
            drpBillingState.Enabled = True

            txtBillingCity.Enabled = True

            dvBillingState.Visible = True
            rqdrpBillingState.Visible = True

            'Provice
            dvBillingRegion.Visible = False
            txtBillingRegion.Visible = False
            rqtxtBillingRegion.Visible = False

            'divProfessionalStatus.Visible = True
            'ShowPanelProfessionalStatus(True)
            ''SpanBillingtxt.Visible = False
            ''SpanShippingtxt.Visible = False
            chkSameAsBilling.Enabled = True
            ''divIntNote.Visible = False
            css = "required"

        End If

    End Sub

    Protected Sub drpShippingCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpShippingCountry.SelectedIndexChanged
        If drpShippingCountry.SelectedValue <> "US" Then
            drpShippingState.Visible = False
            dvShippingState.Visible = False
            dvShippingRegion.Visible = True

            ''cusvShippingPhoneUS.Enabled = False
            rqtxtShippingPhone.Enabled = True
            ''trPhoneShippingOutSite.Visible = True
            ''trShippingPhoneUS.Visible = False
        Else
            drpShippingState.Visible = True

            dvShippingState.Visible = True

            dvShippingRegion.Visible = False

            rqtxtShippingPhone.Enabled = True

        End If
    End Sub

    Protected Sub chkSameAsBilling_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSameAsBilling.CheckedChanged

        If drpBillingCountry.SelectedValue = "US" Then

            If chkSameAsBilling.Checked = True Then
                divShippingAddress.Visible = False
            Else
                divShippingAddress.Visible = True
                drpShippingCountry.Enabled = False
            End If
            drpShippingCountry.SelectedValue = "US"
            drpShippingCountry_SelectedIndexChanged(Nothing, Nothing)


            'drpBillingState.Enabled = False
        Else
            divShippingAddress.Visible = False
        End If
        zipcode.InnerHtml = zipcode.InnerHtml + "|chkSameAsBillingChange"
    End Sub


End Class

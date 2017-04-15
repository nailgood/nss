Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports Components
Imports DataLayer
Imports System.IO
Partial Class controls_layout_form_contact
    Inherits BaseControl

    Protected ContactId, SubjectId As Integer
    Protected s As ContactUsSubjectRow
    Private c As ShoppingCart
    Dim sp As SitePage
    Public IsError As Boolean = False

    Public Property Cart() As ShoppingCart
        Get
            Return c
        End Get
        Set(ByVal value As ShoppingCart)
            c = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        sp = CType(Me.Page, SitePage)
        s = ContactUsSubjectRow.GetByLink(Request.RawUrl)
        If Not s Is Nothing Then
            SubjectId = s.SubjectId
        End If
        If Not IsPostBack Then
            LoadState()
            LoadCountry()
            LoadMemberInfor()
        End If
        If drCountry.SelectedValue <> "US" Then ' tat bao loi check valid phone int
            cusvPhoneUS.Visible = False
        End If
        If (SubjectId = 2 Or SubjectId = 5 Or SubjectId = 9 Or SubjectId = 6 Or SubjectId = 1 Or SubjectId = 4 Or SubjectId = 7 Or SubjectId = 17) AndAlso drCountry.SelectedValue = "US" Then
            trCountry.Visible = True
            trState.Visible = True
            trZipCode.Visible = True
            trRegion.Visible = False
            trInvoice.Visible = IIf(SubjectId = 9 Or SubjectId = 6, False, True)
        ElseIf SubjectId = 2 Or SubjectId = 5 Or SubjectId = 9 Or SubjectId = 6 Or SubjectId = 1 Or SubjectId = 4 Or SubjectId = 7 Then
            trRegion.Visible = True
            trState.Visible = False
            trZipCode.Visible = False
            trInvoice.Visible = IIf(SubjectId = 9 Or SubjectId = 6, False, True)
        Else ''General Question, Submit An Idea
            trRegion.Visible = False
            trSalon.Visible = False
            trShippingAddress.Visible = False
            trState.Visible = False
            trCity.Visible = False
            trInvoice.Visible = False
            trZipCode.Visible = False
            trCountry.Visible = False
            cusvPhoneUS.Visible = False
        End If
        trDescQty.Visible = IIf(SubjectId = 9, True, False)
        trComments.Visible = IIf(SubjectId = 9, False, True)
        trDescItem.Visible = IIf(SubjectId = 6, True, False)
        trItemNotReceive.Visible = IIf(SubjectId = 1, True, False)
        trDamaged.Visible = IIf(SubjectId = 4, True, False)
        trItemReturn.Visible = IIf(SubjectId = 7, True, False)
        trPriceRequest.Visible = IIf(SubjectId = 17, True, False)
        trTypecode.Visible = IIf(SubjectId = 17, False, True)
        'cusItemNo.Enabled = True
    End Sub

    Private Sub LoadState()
        drState.Items.AddRange(StateRow.GetStateList().ToArray())
        drState.DataBind()
        drState.Items.Insert(0, New ListItem("", ""))
        drState.Items.Add(New ListItem("Other", "Other"))
    End Sub

    Private Sub LoadCountry()
        Try
            drCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
            drCountry.DataBind()
            drCountry.SelectedValue = "US"
        Catch ex As Exception
        Finally
        End Try
    End Sub

    Private Sub LoadMemberInfor()
        If sp.HasAccess() Then
            Dim CurrentMemberId As Integer = 0
            Dim Member As MemberRow = Nothing
            Dim MemberBillingAddress As MemberAddressRow = Nothing
            Dim MemberShippingAddress As MemberAddressRow = Nothing
            'Dim o As StoreOrderRow = Cart.Order

            CurrentMemberId = Session("memberId")
            If Member Is Nothing Then Member = MemberRow.GetRow(Session("MemberId"))
            MemberBillingAddress = MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & CurrentMemberId & " AND Label = 'Default Billing Address'"))
            MemberShippingAddress = IIf(SubjectId <> 3 Or SubjectId <> 8, MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & CurrentMemberId & " AND Label = 'Default Shipping Address'")), Nothing)
            txtFirstName.Text = MemberBillingAddress.FirstName
            txtLastName.Text = MemberBillingAddress.LastName
            txtEmailAddress.Text = MemberBillingAddress.Email
            txtBillingPhone.Text = MemberBillingAddress.Phone
            '' page: Billing Inquiry, Return Order Status
            If SubjectId <> 3 Or SubjectId <> 8 Then
                drCountry.SelectedValue = MemberBillingAddress.Country
                txtCity.Text = MemberBillingAddress.City
                txtSalonName.Text = MemberBillingAddress.Company
                txtZipCode.Text = MemberBillingAddress.Zip
                'txtShipping.Text = o.ShipToAddress
                txtShipping.Text = MemberShippingAddress.Address1
                If drCountry.SelectedValue = "US" Then
                    drState.SelectedValue = Member.Customer.County
                Else
                    txtRegion.Text = Member.Customer.County
                End If
                If drCountry.SelectedValue = "US" Then
                    'rfvTxtState.Enabled = False
                    'rfvZipCode.Enabled = True
                    'rfvSate.Enabled = True
                    'labeldrState.Visible = True
                    drState.Visible = True
                Else
                    'rfvTxtState.Enabled = True
                    'rfvZipCode.Enabled = False
                    'rfvSate.Enabled = False
                    'labeldrState.Visible = False
                    drState.Visible = False
                End If
            End If
        End If

    End Sub

    Protected Sub ServerCheckPhoneUS(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        'If Not String.IsNullOrEmpty(e.Value) Then
        '    e.IsValid = Utility.Common.CheckUSPhoneValid(e.Value)
        'End If
        If Not String.IsNullOrEmpty(txtBillingPhone.Text) Then
            Dim array() As String = txtBillingPhone.Text.Trim().Split(" ")
            Dim phone1 As String = String.Empty
            Dim phone2 As String = String.Empty
            Dim phone3 As String = String.Empty
            Dim phoneExt As String = String.Empty
            Try
                Dim tmp As String = array(0)
                If Not String.IsNullOrEmpty(tmp) Then
                    Dim lstPhone() As String = tmp.Split("-")
                    phone1 = lstPhone(0)
                    phone2 = lstPhone(1)
                    phone3 = lstPhone(2)
                End If
                If array.Length > 1 Then
                    phoneExt = array(1)
                End If

                If (String.IsNullOrEmpty(phone1) And String.IsNullOrEmpty(phone2) And String.IsNullOrEmpty(phone3)) Then
                    e.IsValid = False
                    Exit Sub
                End If
                e.IsValid = Utility.Common.CheckUSPhoneValid(phone1, phone2, phone3)
                If (e.IsValid) Then
                    e.IsValid = CheckPhoneExt(phoneExt.Trim())
                End If
                If (e.IsValid = False) Then
                    cusvPhoneUS.ErrorMessage = "Daytime phone number is invalid"
                End If
            Catch ex As Exception
                e.IsValid = False
                cusvPhoneUS.ErrorMessage = "Daytime phone number is invalid"
            End Try
        End If

    End Sub

    Protected Function CheckPhoneExt(ByVal ext As String) As Boolean
        If (String.IsNullOrEmpty(ext)) Then
            Return True
        End If
        If (ext.Length > 4) Then
            Return False
        Else

        End If
        Dim nPhone As Integer = -1
        Try
            nPhone = CInt(ext)
        Catch ex As Exception
            nPhone = -1
        End Try
        If (nPhone < 0) Then
            Return False
        Else
            Return True
        End If
    End Function

    Protected Sub ServerCheckPhoneInt(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        If Not String.IsNullOrEmpty(txtBillingPhone.Text) Then
            Dim phone As String = txtBillingPhone.Text.Trim().Replace(" ", "")
            phone = phone.Replace(".", "")
            phone = phone.Replace("(", "")
            phone = phone.Replace(")", "")
            phone = phone.Replace("-", "")
            If IsNumeric(phone) Then
                If phone.Length < 10 Then
                    e.IsValid = False
                    'cusvPhoneBillingInt.ErrorMessage = "Billing phone must be at least 10 digit."
                End If
            Else
                e.IsValid = False
            End If

        End If
    End Sub

    Protected Sub ServerCheckValidEmail(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        If Not String.IsNullOrEmpty(txtEmailAddress.Text) Then
            Dim email As String = txtEmailAddress.Text.Trim()
            If Not Utility.Common.CheckValidEmail(email) Then
                e.IsValid = False
            End If
        End If
    End Sub

    Protected Sub CheckRequired()
        If trInvoice.Visible Then
            If txtInvoice.Text = String.Empty Then
                ltrInvoice.Text = "<span class='text-danger'> Invoice Number is required</span>"
                IsError = True
            Else
                ltrInvoice.Text = String.Empty
            End If
        End If
        If trDamaged.Visible Then
            If radDamaged.SelectedValue = String.Empty Then
                lrtradDamaged.Text = "<span class='text-danger'> The merchandise Damaged/Broken or Defective is required</span>"
                IsError = True
            Else
                lrtradDamaged.Text = String.Empty
            End If
            If txtItemDamaged.Text = String.Empty Then
                ltrItemDamaged.Text = "<span class='text-danger'> Item on your order Damaged/Broken or Defective is required</span>"
                IsError = True
            Else
                ltrItemDamaged.Text = String.Empty
            End If
            If txtDescDamaged.Text = String.Empty Then
                ltrDescDamaged.Text = "<span class='text-danger'> Describle how the package was packed is required</span>"
                IsError = True
            Else
                ltrDescDamaged.Text = String.Empty
            End If
            If radYesNoDamaged.SelectedValue = String.Empty Then
                ltrYesNoDamaged.Text = "<span class='text-danger'> There any visible damaged and/or tampering with the carton is required</span>"
                IsError = True
            Else
                ltrYesNoDamaged.Text = String.Empty
            End If
            If radCreditedDamaged.SelectedValue = String.Empty Then
                ltrCreditedDamaged.Text = "<span class='text-danger'> Your merchandise reshipped or credited is required</span>"
                IsError = True
            Else
                ltrCreditedDamaged.Text = String.Empty
            End If
        End If
        If trItemNotReceive.Visible Then
            If radCreditedReceived.SelectedValue = String.Empty Then
                ltrCreditedReceived.Text = "<span class='text-danger'> Your merchandise reshipped or credited is required</span>"
                IsError = True
            Else
                ltrCreditedReceived.Text = String.Empty
            End If
            If txtItemNotReceive.Text = String.Empty Then
                ltrItemNotReceive.Text = "<span class='text-danger'> 'Item not received' is required</span>"
                IsError = True
            Else
                ltrItemNotReceive.Text = String.Empty
            End If
        End If
        If trDescItem.Visible Then
            If txtDescItem.Text = String.Empty Then
                ltrDescItem.Text = "<span class='text-danger'> Item # or Product Description is required</span>"
                IsError = True
            Else
                ltrDescItem.Text = String.Empty
            End If
        End If
        If trDescQty.Visible Then
            If txtDescQty.Text = String.Empty Then
                ltrDescQty.Text = "<span class='text-danger'> Describe product & quantity is required</span>"
                IsError = True
            Else
                ltrDescQty.Text = String.Empty
            End If
        End If
        If trPriceRequest.Visible Then
            If radAdjustmentType.SelectedValue = String.Empty Then
                ltrAdjustmentType.Text = "<span class='text-danger'>Adjustment Type is required</span>"
                IsError = True
            Else
                ltrAdjustmentType.Text = String.Empty
            End If
            If txtItemAdjust.Text = String.Empty Then
                ltrItemAdjust.Text = "<span class='text-danger'>Item(s) to Adjust is required</span>"
                IsError = True
            Else
                ltrItemAdjust.Text = String.Empty
            End If
        End If
        If txtComments.Text = String.Empty And Request.RawUrl.Contains("wholesaleinquiry.aspx") = False Then
            ltrComments.Text = "<span class='text-danger'> Comments is required</span>"
            IsError = True
        Else
            ltrComments.Text = String.Empty
        End If
        If trTypecode.Visible AndAlso Not sp.CheckCaptcha(txtCaptcha.Text.Trim()) Then
            ltCapcha.Text = "<span class='text-danger'> Please try the code shown instead again</span>"
            txtCaptcha.Text = ""
            IsError = True
        Else
            ltCapcha.Text = String.Empty
        End If

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not sp.IsValid Then
            txtCaptcha.Text = String.Empty
            Exit Sub
        End If
        CheckRequired()
        If IsError Then Exit Sub
        Try
            Dim sMsg As String = String.Empty
            Dim sName As String = String.Empty
            Dim sSubject As String = String.Empty
            Dim phone As String = String.Empty
            Dim State As String = String.Empty
            Dim strTypeShipping As String = String.Empty
            Dim strYesNo1 As String = String.Empty
            Dim strYesNo2 As String = String.Empty
            Dim strItem As String = "<table width='800px' border='1' style='border-color:#FFC; border-style:solid'><tr style='background-color:#CCC; padding:2px; color:black'><td>Item No </td><td>Product Description </td><td>Quantity </td><td>Reason For Return </td></tr>"

            phone = txtBillingPhone.Text
            State = IIf(drState.SelectedValue <> String.Empty, drState.SelectedValue, txtRegion.Text)
            If SubjectId = 1 Then
                strTypeShipping = radCreditedReceived.SelectedItem.Text
            End If
            If SubjectId = 4 Then
                strTypeShipping = radCreditedDamaged.SelectedItem.Text
            End If
            If radCreditedDamaged.SelectedValue <> String.Empty AndAlso radCreditedDamaged.SelectedValue = True Then
                strYesNo1 = "Damaged"
            Else
                strYesNo1 = "Defective"
            End If
            If radYesNoDamaged.SelectedValue <> String.Empty AndAlso radYesNoDamaged.SelectedValue = True Then
                strYesNo2 = "Yes"
            Else
                strYesNo2 = "No"
            End If

            If txtItem1.Text.Trim <> "" Then
                strItem = strItem & "<tr><td>" & txtItem1.Text & "</td><td>" & txtItemDesc1.Text & "</td><td>" & txtQty1.Text & "</td><td>" & txtReason1.Text & "</td></tr>"
            End If
            If txtItem2.Text.Trim <> "" Then
                strItem = strItem & "<tr><td>" & txtItem2.Text & "</td><td>" & txtItemDesc2.Text & "</td><td>" & txtQty2.Text & "</td><td>" & txtReason2.Text & "</td></tr>"
            End If
            If txtItem3.Text.Trim <> "" Then
                strItem = strItem & "<tr><td>" & txtItem3.Text & "</td><td>" & txtItemDesc3.Text & "</td><td>" & txtQty3.Text & "</td><td>" & txtReason3.Text & "</td></tr>"
            End If
            If txtItem4.Text.Trim <> "" Then
                strItem = strItem & "<tr><td>" & txtItem4.Text & "</td><td>" & txtItemDesc4.Text & "</td><td>" & txtQty4.Text & "</td><td>" & txtReason4.Text & "</td></tr>"
            End If

            If txtItem5.Text.Trim <> "" Then
                strItem = strItem & "<tr><td>" & txtItem5.Text & "</td><td>" & txtItemDesc5.Text & "</td><td>" & txtQty5.Text & "</td><td>" & txtReason5.Text & "</td></tr>"
            End If
            If txtItem6.Text.Trim <> "" Then
                strItem = strItem & "<tr><td>" & txtItem6.Text & "</td><td>" & txtItemDesc6.Text & "</td><td>" & txtQty6.Text & "</td><td>" & txtReason6.Text & "</td></tr>"
            End If
            If txtItem7.Text.Trim <> "" Then
                strItem = strItem & "<tr><td>" & txtItem7.Text & "</td><td>" & txtItemDesc7.Text & "</td><td>" & txtQty7.Text & "</td><td>" & txtReason7.Text & "</td></tr>"
            End If
            If txtItem8.Text.Trim <> "" Then
                strItem = strItem & "<tr><td>" & txtItem8.Text & "</td><td>" & txtItemDesc8.Text & "</td><td>" & txtQty8.Text & "</td><td>" & txtReason8.Text & "</td></tr>"
            End If

            Dim dbEmailTemplet As DataTable = EmailTempletRow.GetSubjectEmailTemplets(DB, SubjectId)
            If dbEmailTemplet.Rows.Count > 0 Then
                sName = dbEmailTemplet.Rows(0)("Name")
                sSubject = dbEmailTemplet.Rows(0)("Subject")
                sMsg = dbEmailTemplet.Rows(0)("Contents")
                If SubjectId = 2 Or SubjectId = 5 Then
                    EmailTempletRow.FormatContentsEmailTemplet("", "", "", "", txtFirstName.Text, txtLastName.Text, txtEmailAddress.Text, phone, txtComments.Text, drCountry.SelectedItem.Text, txtSalonName.Text, txtShipping.Text, txtCity.Text, State, txtZipCode.Text, txtInvoice.Text, "", "", "", "", "", "", "", "", "", "", sMsg)
                ElseIf SubjectId = 9 Then
                    EmailTempletRow.FormatContentsEmailTemplet("", "", "", "", txtFirstName.Text, txtLastName.Text, txtEmailAddress.Text, phone, txtDescQty.Text, drCountry.SelectedItem.Text, txtSalonName.Text, txtShipping.Text, txtCity.Text, State, txtZipCode.Text, "", "", "", "", "", "", "", "", "", "", "", sMsg)
                ElseIf SubjectId = 6 Then
                    EmailTempletRow.FormatContentsEmailTemplet("", "", "", "", txtFirstName.Text, txtLastName.Text, txtEmailAddress.Text, phone, txtComments.Text, drCountry.SelectedItem.Text, txtSalonName.Text, txtShipping.Text, txtCity.Text, State, txtZipCode.Text, "", "", "", "", "", "", "", txtDescItem.Text, "", "", "", sMsg)
                ElseIf SubjectId = 1 Then
                    EmailTempletRow.FormatContentsEmailTemplet("", "", "", "", txtFirstName.Text, txtLastName.Text, txtEmailAddress.Text, phone, txtComments.Text, drCountry.SelectedItem.Text, txtSalonName.Text, txtShipping.Text, txtCity.Text, State, txtZipCode.Text, txtInvoice.Text, txtItemNotReceive.Text, "", "", "", "", strTypeShipping, "", "", "", "", sMsg)
                ElseIf SubjectId = 4 Then
                    EmailTempletRow.FormatContentsEmailTemplet("", "", "", "", txtFirstName.Text, txtLastName.Text, txtEmailAddress.Text, phone, txtComments.Text, drCountry.SelectedItem.Text, txtSalonName.Text, txtShipping.Text, txtCity.Text, State, txtZipCode.Text, txtInvoice.Text, "", strYesNo1, txtItemDamaged.Text, txtDescDamaged.Text, strYesNo2, strTypeShipping, "", "", "", "", sMsg)
                ElseIf SubjectId = 7 Then
                    EmailTempletRow.FormatContentsEmailTemplet("", "", "", "", txtFirstName.Text, txtLastName.Text, txtEmailAddress.Text, phone, txtComments.Text, drCountry.SelectedItem.Text, txtSalonName.Text, txtShipping.Text, txtCity.Text, State, txtZipCode.Text, txtInvoice.Text, "", "", "", "", "", "", "", strItem, "", "", sMsg)
                ElseIf SubjectId = 17 Then
                    EmailTempletRow.FormatContentsEmailTemplet("", "", "", "", txtFirstName.Text, txtLastName.Text, txtEmailAddress.Text, phone, txtComments.Text.Replace(vbCrLf, "<br/>"), drCountry.SelectedItem.Text, txtSalonName.Text, txtShipping.Text, txtCity.Text, State, txtZipCode.Text, txtInvoice.Text, "", "", "", "", "", "", txtItemAdjust.Text.Replace(vbCrLf, "<br/>"), radAdjustmentType.SelectedItem.Text, "", "", sMsg)
                Else
                    EmailTempletRow.FormatContentsEmailTemplet("", "", "", "", txtFirstName.Text, txtLastName.Text, txtEmailAddress.Text, phone, txtComments.Text, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", sMsg)
                End If

            ElseIf SubjectId = 3 Or SubjectId = 8 Then
                sMsg = "First Name: " & txtFirstName.Text & vbCrLf & _
              "Last Name: " & txtLastName.Text & vbCrLf & _
              "Email: " & txtEmailAddress.Text & vbCrLf & _
              "Phone: " & phone & vbCrLf & _
              "Comments: " & vbCrLf & txtComments.Text
                If Not s Is Nothing AndAlso s.SubjectId > 0 Then
                    sSubject = s.Subject
                ElseIf SubjectId = 2 Then
                    sSubject = "General Question"
                Else
                    sSubject = "Submit An Idea"
                End If
            Else
                sMsg = "Country: " & drCountry.SelectedItem.Text & vbCrLf & _
                           "First Name: " & txtFirstName.Text & vbCrLf & _
                             "Last Name: " & txtLastName.Text & vbCrLf & _
                             "Salon Name: " & txtSalonName.Text & vbCrLf & _
                             "Email: " & txtEmailAddress.Text & vbCrLf & _
                             "Daytime Phone: " & phone & vbCrLf & _
                             IIf(SubjectId = 6, "Item # or Product Description:" & txtDescItem.Text, "Invoice Number: " & txtInvoice.Text) & vbCrLf & _
                             "Shipping Address: " & txtShipping.Text & vbCrLf & _
                             "City : " & txtCity.Text & vbCrLf & vbCrLf & _
                            IIf(drCountry.SelectedValue = "US", "State: ", "Province/Region: ") & State & vbCrLf & _
                            IIf(drCountry.SelectedValue = "US", "Zip Code: " & txtZipCode.Text, "") & vbCrLf & _
                            IIf(SubjectId = 1, "Once the issue is resolved, would you like your merchandise reshipped or credited : " & radCreditedReceived.SelectedValue & vbCrLf & _
                             "What item on your order were not recieved? " & txtItemNotReceive.Text, "") & vbCrLf & _
                         IIf(SubjectId = 4, "Is the merchandise Damaged/Broken or Defective?" & strYesNo1 & vbCrLf & _
                           "What item on your order Damaged/Broken or Defective?" & txtItemDamaged.Text & vbCrLf & _
                           "Is there any visible damaged and/or tampering with the carton?" & strYesNo2 & vbCrLf & _
                           "Please describle how the package was packed?" & txtDescDamaged.Text & vbCrLf & vbCrLf & _
                           "Once the issue is resolved, would you like your merchandise reshipped or credited : " & strTypeShipping, "") & vbCrLf & _
                       IIf(SubjectId = 7, "Item not received?:" & strItem, "") & vbCrLf & _
                IIf(SubjectId = 17, "Adjustment Type:" & radAdjustmentType.SelectedItem.Text & vbCrLf & "Item(s) to Adjust: " & vbCrLf & txtItemAdjust.Text, "") & vbCrLf & _
                IIf(SubjectId = 9, "Comments: " & txtDescQty.Text, "Comments: " & txtComments.Text)
                If Not s Is Nothing AndAlso s.SubjectId > 0 Then
                    sSubject = s.Subject
                ElseIf SubjectId = 2 Then
                    sSubject = "Billing Inquiry"
                ElseIf SubjectId = 6 Then
                    sSubject = "Product Warranty Information"
                ElseIf SubjectId = 1 Then
                    sSubject = "Item Not Received"
                ElseIf SubjectId = 4 Then
                    sSubject = "Damaged Shipment"
                ElseIf SubjectId = 7 Then
                    sSubject = "Return Authorization Number Request"
                ElseIf SubjectId = 9 Then
                    sSubject = "Wholesale Inquiry"
                ElseIf SubjectId = 17 Then
                    sSubject = "Price Adjustment request"
                Else
                    sSubject = "Return Order Status"
                End If
            End If

            Dim dbContact As New ContactUsRow(DB)
            dbContact.FirstName = txtFirstName.Text
            dbContact.LastName = txtLastName.Text
            dbContact.Phone = phone
            dbContact.Comments = IIf(SubjectId = 9, txtDescQty.Text, txtComments.Text)
            dbContact.EmailAddress = txtEmailAddress.Text
            dbContact.SubjectId = SubjectId
            dbContact.TypeContact = IIf(SubjectId = 3, "General Question", "Submit An Idea")
            If SubjectId <> 3 Or SubjectId <> 8 Then
                dbContact.ShippingAddress = txtShipping.Text
                dbContact.SalonName = txtSalonName.Text
                dbContact.City = txtCity.Text
                dbContact.State = State
                dbContact.ZipCode = txtZipCode.Text
                dbContact.InvoiceNumber = IIf(SubjectId <> 9, txtInvoice.Text, "")
                dbContact.TypeContact = IIf(SubjectId = 2, "Billing Inquiry", IIf(SubjectId = 5, "Return Order Status", IIf(SubjectId = 6, "Product Warranty Information", "Wholesale Inquiry")))
                If SubjectId = 1 Then
                    dbContact.TypeShipping = strTypeShipping
                    dbContact.TypeContact = "Item Not Received"
                End If
                If SubjectId = 4 Then
                    dbContact.DamagedCarton = strYesNo2
                    dbContact.ItemDamaged = txtItemDamaged.Text
                    dbContact.PieceOfMerchandise = strYesNo1
                    dbContact.TypeShipping = strTypeShipping
                    dbContact.DscPackaging = txtDescDamaged.Text
                End If
                If SubjectId = 17 Then
                    dbContact.AdjustmentType = radAdjustmentType.SelectedValue
                    dbContact.ProductDescription = txtItemAdjust.Text
                End If
                dbContact.ProductDescription = IIf(SubjectId = 7, strItem, "")
            End If
            dbContact.Insert()

            EmailTempletRow.SendEmailList(DB, SubjectId, txtFirstName.Text, txtLastName.Text, txtEmailAddress.Text, sSubject, sMsg)
            Dim au As New AutoRespondRow
            If au.CheckDate(DB, DateTime.Now) Then
                au.SendAuto(txtEmailAddress.Text, txtFirstName.Text & " " & txtLastName.Text)
            End If
            Response.Redirect("thankyou.aspx")
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            sp.AddError(sp.ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub drCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drCountry.SelectedIndexChanged
        If drCountry.SelectedValue = "US" Then
            drState.Visible = True
            txtZipCode.Visible = True
            txtRegion.Visible = False
        Else
            drState.Visible = False
            txtZipCode.Visible = False
            txtRegion.Visible = True
        End If
        trInvoice.Visible = IIf(SubjectId = 9 Or SubjectId = 6, False, True)
        trComments.Visible = IIf(SubjectId = 9, False, True)
        trDescQty.Visible = IIf(SubjectId = 9, True, False)
        trItemNotReceive.Visible = IIf(SubjectId = 1, True, False)
        trDamaged.Visible = IIf(SubjectId = 4, True, False)
        trItemReturn.Visible = IIf(SubjectId = 7, True, False)
        trDescItem.Visible = IIf(SubjectId = 6, True, False)
        trPriceRequest.Visible = IIf(SubjectId = 17, True, False)
    End Sub

End Class


Imports System.IO
Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports System

Public Class members_register
    Inherits SitePage
    Private Member As MemberRow = Nothing
    Public PointUseReferFriend As Integer = 0

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If HasAccess() Then Response.Redirect("/members/address.aspx")
        litJS.Text = "<script language=""javascript"">" & vbCrLf & "<!--" & vbCrLf
        Dim strPointUseReferFriend As String = SysParam.GetValue("PointUseReferFriend")
        If Not String.IsNullOrEmpty(strPointUseReferFriend) Then
            PointUseReferFriend = CInt(strPointUseReferFriend)
        End If
        If Not IsPostBack Then
            cusvProfessionalStatus.ErrorMessage = Resources.Alert.RegisterAgreePolicyError
            txtEmail.Focus()
            LoadCountry()

            'LoadMemberTypes()
            'GetReferData()
        End If

        ''ltlBreadcrumb.Text = "<span class=""bcative"">New Member Registration</span>"
        register_form.Visible = True
        cvtxtPassword.Enabled = True
        rqtxtPassword.Enabled = True
        cusvPassword.Enabled = True
        litJS.Text &= "//-->" & vbCrLf & "</script>"
    End Sub

    'Private Sub GetReferData()
    '    Dim referCode As String = GetQueryString("refercode")

    '    'Check refer code is valid
    '    txtReferCode.Text = referCode
    'End Sub

    'Private Sub BindMemberTypeDataSource(ByVal source As DataSet)
    '    If Not source Is Nothing Then
    '        Dim MemberTypeID As String
    '        Dim membertype As String
    '        Dim active As Boolean
    '        Dim code As String
    '        For Each row As DataRow In source.Tables(0).Rows
    '            MemberTypeID = row("MemberTypeId")
    '            membertype = row("MemberType")
    '            active = row("Active")
    '            code = row("NavisionCode")
    '            If (code <> "CON" And active = "1") Then
    '                optMemberType.Items.Add(New ListItem(membertype, MemberTypeID))
    '            End If
    '        Next
    '    End If
    'End Sub

    'Private Sub LoadMemberTypes()
    '    BindMemberTypeDataSource(DataLayer.MemberTypeRow.GetMemberTypes(DB))
    '    If Not Member Is Nothing Then
    '        optMemberType.SelectedValue = Member.MemberTypeId
    '    End If
    'End Sub
    'Protected Sub drpCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles drpCountry.SelectedIndexChanged
    '    If drpCountry.SelectedValue <> "US" Then
    '        ShowPanelProfessionalStatus(False)
    '    Else
    '        ShowPanelProfessionalStatus(True)
    '    End If
    'End Sub
    Private Sub LoadCountry()
        drpCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
        drpCountry.DataBind()
        drpCountry.SelectedValue = "US"
    End Sub
    Protected Sub ServerCheckPasswordValid(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        e.IsValid = Utility.Common.IsPasswordValid(e.Value)
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click
        Dim bRedirect As Boolean = True, bError As Boolean = False, bAtLeastOne As Boolean = False
        Dim sRightsOnlyItems As String = ""
        Page.Validate()

        'Check country
        If String.IsNullOrEmpty(drpCountry.SelectedValue) Then
            AddError("You must provide an country for your membership")
            Exit Sub
        End If

        'If drpCountry.SelectedValue = "US" AndAlso String.IsNullOrEmpty(optMemberType.SelectedValue) Then
        '    AddError("You must select an Professional Status for your membership")
        '    Exit Sub
        'End If

        'Validate refer code is valid
        Dim MemberReferId As Integer = 0
        If Not String.IsNullOrEmpty(GetQueryString("refercode")) Then
            MemberReferId = DB.ExecuteScalar("Select MemberId from Member where ReferCode='" & GetQueryString("refercode").Trim() & "'")
            'If (MemberReferId < 1) Then
            '    AddError("Referral code is not valid.")
            '    bError = True
            'End If
        End If

        If Not chkProfessionalStatus.Checked AndAlso drpCountry.SelectedValue = "US" Then
            AddError(cusvProfessionalStatus.ErrorMessage)
            bError = True
        End If
        If Page.IsValid Then
            Dim sCheckUsername As String = DB.ExecuteScalar("SELECT Username FROM Member WHERE Username = " & DB.Quote(txtEmail.Text))
            If sCheckUsername <> "" Then
                bError = True
                AddError("The email address you entered is already in use.  Please visit the <a href='/forgotpassword.aspx'>forgot your password</a> page if you no longer remember your password, otherwise please use a different email address.")
            End If

            If Not bError Then
                Dim sCheckEmail As String = DB.ExecuteScalar("SELECT Username FROM Member WHERE CustomerId = (select top 1 customerid from customer where email = " & DB.Quote(txtEmail.Text) & ")")
                If sCheckEmail <> "" Then
                    bError = True
                    AddError("The email address you entered is already in use.  Please visit the <a href='/forgotpassword.aspx'>forgot your password</a> page if you no longer remember your password, otherwise please use a different email address.")
                End If
            End If
        End If

        If Not Page.IsValid Or bError Then Exit Sub
        Dim CurrentMemberId As Integer = 0
        Try
            DB.BeginTransaction()
            ' Create Membership

            Dim MemberBillingAddress As MemberAddressRow
            Dim MemberShippingAddress As MemberAddressRow
            Member = New MemberRow(DB)
            Member.Username = txtEmail.Text.Trim()
            Member.Password = txtPassword.Text
            Member.CreateDate = Now
            Member.IsActive = False
            MemberBillingAddress = New MemberAddressRow(DB)
            MemberShippingAddress = New MemberAddressRow(DB)

            'If Not MemberRow.IsEbayCustomer(DB, Member.MemberId) Then
            '    Member.MemberTypeId = optMemberType.SelectedValue
            'End If
            'If drpCountry.SelectedValue = "US" Or drpCountry.SelectedValue = "" Then
            '    If chkProfessionalStatus.Checked Then
            '        Member.ProfessionalStatus = 3
            '    Else
            '        Member.ProfessionalStatus = Nothing
            '    End If
            'Else
            'End If

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
            CurrentMemberId = Member.Insert(False)

            If CurrentMemberId <= 0 Then
                DB.RollbackTransaction()
                AddError("An error has occurred while trying to process your request. Please, try again later")
                Email.SendError("ToError500", " Register Error", Request.Url.AbsoluteUri.ToString() & "<br>CurrentMemberId=0<br>" & GetFormData())
                Exit Sub
            End If

            MemberBillingAddress.DoExport = False
            MemberBillingAddress.MemberId = CurrentMemberId
            MemberBillingAddress.Label = "Default Billing Address"
            MemberBillingAddress.AddressType = "Billing"
            MemberBillingAddress.Country = drpCountry.SelectedValue
            MemberBillingAddress.Email = txtEmail.Text.Trim()
            MemberBillingAddress.IsDefault = True
            MemberBillingAddress.Address1 = String.Empty
            MemberBillingAddress.City = String.Empty

            'Truong hop dac biet Alaska & Hawai phai la Country US va State Alaska or Hawai 
            If MemberBillingAddress.Country = "AK" Or MemberBillingAddress.Country = "HI" Then
                MemberBillingAddress.State = MemberBillingAddress.Country
                MemberBillingAddress.Country = "US"
            End If

            MemberShippingAddress.MemberId = CurrentMemberId
            MemberShippingAddress.Label = "Default Shipping Address"
            MemberShippingAddress.AddressType = "Shipping"
            MemberShippingAddress.Address1 = String.Empty
            MemberShippingAddress.City = String.Empty
            MemberShippingAddress.DoExport = False
            MemberShippingAddress.Country = MemberBillingAddress.Country
            Dim billingAddressId As Integer = MemberShippingAddress.Insert()
            Dim shippingAddressId As Integer = MemberBillingAddress.Insert()

            If billingAddressId < 1 Or shippingAddressId < 1 Then
                
                Dim sLog As String = String.Empty
                If billingAddressId < 1 And shippingAddressId < 1 Then
                    sLog = "Insert Billing Address Error<br/>" & "Email=" & txtEmail.Text.Trim() & ",Country=" & drpCountry.SelectedValue
                Else
                    sLog = "Insert Shipping Address Error<br/>" & "Email=" & txtEmail.Text.Trim() & ",Country=" & drpCountry.SelectedValue
                End If

                Email.SendError("ToError500", " Register Error", Request.Url.AbsoluteUri.ToString() & "<br>Log: " & sLog & "<br>" & GetFormData())
                Exit Sub
            End If

            Member.Customer.Email = MemberBillingAddress.Email
            Member.Customer.ContactNo = Member.CustomerContact.ContactNo
            Member.Customer.ContactId = Member.CustomerContact.ContactId
            Member.Customer.DoExport = False
            Member.Customer.Update()

            Member.CustomerContact.CountryCode = drpCountry.SelectedValue
            Member.CustomerContact.Email = MemberBillingAddress.Email
            Member.CustomerContact.DoExport = False
            Member.CustomerContact.Update()

            Member.Customer.SalesTaxExemptionNumber = Nothing
            Dim MailingStatus As String = ""
            MailingStatus = "ACTIVE"
            Dim dbMailingMember As MailingMemberRow
            dbMailingMember = MailingMemberRow.GetRowByEmail(DB, txtEmail.Text)
            ''dbMailingMember.MemberId = CurrentMemberId
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

            If MemberReferId > 0 Then
                Dim objRerer As New MemberReferRow
                objRerer.TypeRefer = Utility.Common.ReferType.ReferFriends
                Try
                    objRerer.Source = GetQueryString("src")
                Catch ex As Exception

                End Try
                objRerer.Email = txtEmail.Text
                objRerer.MemberRefer = MemberReferId
                objRerer.MemberUseRefer = Member.MemberId
                objRerer.Status = Utility.Common.ReferFriendStatus.Register
                MemberReferRow.UpdateRegisterReferFriend(DB, objRerer)
            End If
            DB.CommitTransaction()

        Catch ex As Exception
            DB.RollbackTransaction()
            Email.SendError("ToError500", " Register Error", Request.Url.AbsoluteUri.ToString() & "Exception: " & ex.Message & "<br>" & GetFormData())
            bRedirect = False
        End Try

        If Not bRedirect Then
            AddError("An error has occurred while trying to process your request. Please, try again later")
            Exit Sub
        End If

        Dim dbMember As MemberRow = MemberRow.GetRow(CurrentMemberId)
        If Not dbMember.LastOrderId = Nothing Then
            Dim dbOrder As StoreOrderRow = StoreOrderRow.GetRow(DB, dbMember.LastOrderId)
            If dbOrder.ProcessDate = Nothing AndAlso dbOrder.MemberId = dbMember.MemberId Then
                ''
            Else
                dbMember.LastOrderId = Nothing
                dbMember.Update(DB)
            End If
        Else
            Session("OrderId") = Nothing
        End If

        Dim sActiveCode As String = Guid.NewGuid().ToString()
        DB.ExecuteSQL("UPDATE Member SET ActiveCode='" & sActiveCode & "' where memberid = " & CurrentMemberId.ToString())

        Dim sName As String = "The Nail Superstore"
        Dim sSubject As String = "Activate Your Account"
        Dim strContents As String = String.Empty
        Dim objReader As StreamReader

        Try
            objReader = New StreamReader(Utility.ConfigData.ActiveAccountTemplatePath)
            strContents = objReader.ReadToEnd()
            objReader.Close()

            strContents = String.Format(strContents, txtEmail.Text, Utility.ConfigData.GlobalRefererName, sActiveCode, txtPassword.Text)
            Email.SendHTMLMail(FromEmailType.NoReply, txtEmail.Text, txtEmail.Text, sSubject, strContents)
        Catch
        End Try

        If bRedirect Then
            DB.Close()
            Response.Redirect("/members/thankyouregister.aspx")
        End If


    End Sub
    Private Function GetFormData() As String
        Dim result As String = String.Empty
        result = "Country:" & drpCountry.SelectedValue.ToString()
        result &= "<br>Email:" & txtEmail.Text
        result &= "<br>Password:" & txtPassword.Text
        result &= "<br>Confirm Password:" & txtConfirmpassword.Text
        'result &= "<br>Professional Status:" & optMemberType.SelectedValue.ToString()
        result &= "<br>chkProfessionalStatus:" & chkProfessionalStatus.Checked
        result &= "<br>Referral code:" & GetQueryString("refercode")
        ''  result &= "<br>Confirm Password:" & txtConfirmpassword.Text
        Return result
    End Function
    Private Sub SendEmailList(ByVal sSubject As String, ByVal Msg As String)
        Dim res As DataTable = DB.GetDataTable("select * from StorePromotion where IsRegisterSend='true'")
        If res.Rows.Count > 0 Then
            Dim i As Integer
            Dim PromotionCode As String = ""
            Msg = Msg & " Congratulation!. Now, we have The Special Discounts for new member : " & vbCrLf
            For i = 0 To res.Rows.Count - 1
                PromotionCode = res.Rows(i)("PromotionCode")
                If PromotionCode <> "" Then
                    Msg = Msg & " PromotionCode :" & PromotionCode & vbCrLf
                End If
            Next
        End If
        Email.SendHTMLMail(FromEmailType.NoReply, txtEmail.Text, txtEmail.Text, sSubject, Msg)
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

End Class

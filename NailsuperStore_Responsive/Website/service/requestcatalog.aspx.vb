Imports Components
Imports DataLayer

Partial Class service_requestcatalog
    Inherits SitePage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BindData()
            LoadMemberInfor()
        End If

        If ddlCountry.SelectedValue = "US" Then
            trState.Visible = True
            trZipCode.Visible = True
            trRegion.Visible = False
            cusvPhoneInt.Visible = False
            cusvPhoneUS.Visible = True
        Else
            trState.Visible = False
            trZipCode.Visible = False
            trRegion.Visible = True
            cusvPhoneUS.Visible = False
        End If
    End Sub

    Private Sub LoadMemberInfor()
        If HasAccess() Then
            Dim CurrentMemberId As Integer = 0
            Dim Member As MemberRow = Nothing
            Dim MemberBillingAddress As MemberAddressRow = Nothing

            CurrentMemberId = Session("memberId")
            If Member Is Nothing Then Member = MemberRow.GetRow(Session("MemberId"))
            MemberBillingAddress = MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & CurrentMemberId & " AND Label = 'Default Billing Address'"))
            ddlCountry.SelectedValue = MemberBillingAddress.Country
            txtFirstName.Text = Member.Customer.Name
            txtLastName.Text = Member.Customer.Name2
            txtCity.Text = Member.Customer.City
            txtBillingPhone.Text = Member.Customer.Phone
            txtEmailAddress.Text = Member.Customer.Email

            txtZip.Text = Member.Customer.Zipcode
            txtAddress1.Text = Member.Customer.Address
            'txtAddress2.Text = Member.Customer.Address2
            ddlState.SelectedValue = Member.Customer.County

            If ddlCountry.SelectedValue = "US" Then
                ddlState.SelectedValue = Member.Customer.County
            Else
                txtState.Text = Member.Customer.County
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
                    cusvPhoneInt.ErrorMessage = "Phone must be at least 10 digit."
                End If
            Else
                e.IsValid = False
            End If

        End If
    End Sub

    Protected Sub ServerCheckPhoneUS(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
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
                    cusvPhoneUS.ErrorMessage = "Phone number is invalid"
                End If
            Catch ex As Exception
                e.IsValid = False
                cusvPhoneUS.ErrorMessage = "Phone number is invalid"
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

    Private Sub BindData()
        ddlState.Items.AddRange(StateRow.GetStateList().ToArray())
        ddlState.DataBind()
        ddlState.Items.Insert(0, New ListItem("", ""))
        ddlState.Items.Add(New ListItem("Other", "Other"))
        ddlState.SelectedValue = ""

        ddlCountry.Items.AddRange(CountryRow.GetCountries().ToArray())
        ddlCountry.DataBind()
        ddlCountry.Items.Insert(0, New ListItem("", ""))
        ddlCountry.SelectedValue = "US"

    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        'End 04/12/2009
        If Not IsValid Then
            Exit Sub
        End If
        Try
            Dim dbRequestCatalog As StoreCatalogRequestRow = New StoreCatalogRequestRow(DB)
            dbRequestCatalog.Address1 = txtAddress1.Text
            'dbRequestCatalog.Address2 = txtAddress2.Text
            dbRequestCatalog.City = txtCity.Text
            If Not ddlCountry.SelectedValue = "US" Then
                dbRequestCatalog.State = txtState.Text
            Else
                dbRequestCatalog.State = ddlState.SelectedValue
            End If
            dbRequestCatalog.Zip = txtZip.Text
            dbRequestCatalog.Country = ddlCountry.SelectedValue
            'dbRequestCatalog.Company = txtCompany.Text
            dbRequestCatalog.Phone = txtBillingPhone.Text
            dbRequestCatalog.Email = txtEmailAddress.Text
            dbRequestCatalog.DateRequested = Date.Now
            dbRequestCatalog.FirstName = txtFirstName.Text
            dbRequestCatalog.LastName = txtLastName.Text
            'dbRequestCatalog.PhoneExt = Ext.Text
            dbRequestCatalog.Insert()
            contact_form.Visible = False
            phConfirm.Visible = True
            SendCatalogRequestEmail()

        Catch ex As Exception

        End Try
    End Sub

    Private Function GetMailBody() As String
        Dim result As String = String.Empty
        Try
            Dim path As String = Server.MapPath("~/includes/MailTemplate/RequestCatalog.txt")
            result = My.Computer.FileSystem.ReadAllText(path)
        Catch ex As Exception

        End Try
        Return result
    End Function

    Protected Sub SendCatalogRequestEmail()
        Dim body As String = GetMailBody()
        If body <> String.Empty Then
            body = String.Format(body, txtFirstName.Text & " " & txtLastName.Text).Replace(Environment.NewLine, "<br/>")
            Email.SendReport("ToEmailCatalogRequest", "Catalog Request Email", body)
            Email.SendHTMLMail(Utility.ConfigData.RequestCatalogEmail, "Customer Sevice", txtEmailAddress.Text, txtFirstName.Text & " " & txtLastName.Text, "Catalog Request", body, "")
        End If
    End Sub


    Protected Sub ddlCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlCountry.SelectedIndexChanged
        If ddlCountry.SelectedValue = "US" Then
            trState.Visible = True
            trZipCode.Visible = True
            trRegion.Visible = False
        Else
            trState.Visible = False
            trZipCode.Visible = False
            trRegion.Visible = True
        End If
    End Sub

End Class

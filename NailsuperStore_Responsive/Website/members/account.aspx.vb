Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Public Class Members_Account
    Inherits SitePage

    Private dbMember As MemberRow

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not HasAccess() Then
            DB.Close()
            Response.Redirect("/members/login.aspx")
        End If
        dbMember = MemberRow.GetRow(Convert.ToInt32(Session("memberId")))

        ''ltlMemberNavigation.Text = MemberRow.MemberNavigationString

        If Not IsPostBack Then LoadFormData()
    End Sub
    Protected Sub ServerCheckPasswordValid(ByVal sender As Object, ByVal e As ServerValidateEventArgs)
        e.IsValid = Utility.Common.IsPasswordValid(e.Value)
    End Sub

    Private Sub LoadFormData()
        txtEmail.Text = dbMember.Customer.Email
    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click, btnSubmit1.Click
        Dim bRedirect As Boolean = True, bError As Boolean = False, bAtLeastOne As Boolean = False, bUpdateMailingMember As Boolean = False

        Dim btn As Button = DirectCast(sender, Button)
        If btn.ID = "btnSubmit" Then
            
            If txtNewPassword.Text = String.Empty Then
                bError = True
                'AddError("New Password  is required")
            End If
            If txtConfirmPassword.Text = String.Empty Then
                bError = True
                'AddError("Confirm New Password  is required")
            End If
        End If
        If bError = True Then
            Exit Sub
        End If

        Dim isUpdateUsername As Boolean = False
        If dbMember.Username.ToLower().Trim() = dbMember.Customer.Email.ToLower().Trim() Then
            isUpdateUsername = True
        End If

        'Page.Validate()

        If txtEmail.Text <> dbMember.Customer.Email And Page.IsValid And Not bError Then
            Dim dbExistingMember As MemberRow = MemberRow.GetRowByEmail(DB, txtEmail.Text)
            If Not dbExistingMember.MemberId = dbMember.MemberId AndAlso dbExistingMember.MemberId <> 0 Then
                txtEmail.Text = dbMember.Customer.Email
                AddError("The email address you selected is currently in use on the system already.  If you have forgotten your password, please <a href='/forgotpassword.aspx'>goto this page</a> to retrieve it.")
                bError = True
            ElseIf (isUpdateUsername) Then
                Dim sCheckUsername As String = DB.ExecuteScalar("SELECT Username FROM Member WHERE Username = " & DB.Quote(txtEmail.Text))
                If sCheckUsername <> "" Then
                    bError = True
                    AddError("The email address you selected is currently in use on the system already.  If you have forgotten your password, please <a href='/forgotpassword.aspx'>goto this page</a> to retrieve it.")

                End If
            Else

                bUpdateMailingMember = True
            End If
        End If



        If Not Page.IsValid Or bError Then Exit Sub

        Try
            DB.BeginTransaction()

            If bUpdateMailingMember Then
                Dim dbMailingMember As MailingMemberRow = MailingMemberRow.GetRowByEmail(DB, dbMember.Customer.Email)
                If dbMailingMember.MemberId <> Nothing Then
                    dbMailingMember.Email = txtEmail.Text.Trim()
                    dbMailingMember.Update()
                End If
            End If
            If dbMember.DB Is Nothing Then
                dbMember.DB = DB
            End If
            dbMember.Customer.Email = txtEmail.Text.Trim()
            dbMember.Customer.Update()
            If (isUpdateUsername) Then
                dbMember.Username = txtEmail.Text.Trim()
            End If
            If Not Trim(txtNewPassword.Text) = String.Empty Then dbMember.Password = txtNewPassword.Text
            dbMember.Update(DB)
            Dim orderId As Integer = 0
            If Not Session("OrderId") Is Nothing Then
                orderId = Session("OrderId")
            End If

            ''DB.ExecuteSQL("Update StoreOrder set Email='" & dbMember.Customer.Email & "' where OrderId=" & Session("OrderId") & " and MemberId=" & Session("memberId"))
            DB.ExecuteSQL("exec sp_Member_UpdateChangeEmailAccount " & orderId & "," & Session("memberId") & ",'" & dbMember.Customer.Email & "'")

            ' Create Membership
            DB.CommitTransaction()
        Catch ex As Exception
            DB.RollbackTransaction()
            AddError("There was a problem processing your email preferences.  Please call us @ 800-417-3261 or visit our <a href='/service/'>customer service page</a> if the problem persists.")
            bRedirect = False
        End Try
        If btn.ID = "btnSubmit" Then
            Try
                Dim sMsg As String
                Dim sName As String = ""
                Dim sSubject As String = "Your password for the nss.com website"
                Dim dbEmailTemplet As DataTable
                Dim dbCustomer As CustomerRow = CustomerRow.GetRow(DB, dbMember.CustomerId)
                Dim MemberBillingAddress As MemberAddressRow = MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & dbMember.MemberId & " AND Label = 'Default Billing Address'"))
                Dim MemberShippingAddress As MemberAddressRow = MemberAddressRow.GetRow(DB, DB.ExecuteScalar("SELECT AddressId FROM MemberAddress WHERE MemberId=" & dbMember.MemberId & " AND Label = 'Default Shipping Address'"))
                dbEmailTemplet = EmailTempletRow.GetOutboundEmailTemplets(DB, "14")

                If dbEmailTemplet.Rows.Count > 0 Then
                    sName = dbEmailTemplet.Rows(0)("Name")
                    sSubject = dbEmailTemplet.Rows(0)("Subject")
                    sMsg = dbEmailTemplet.Rows(0)("Contents")
                    If dbCustomer.Email <> "" Then
                        EmailTempletRow.FormatOutboundEmailTemplet(dbCustomer.Name, dbCustomer.Name2, dbMember.Username, dbMember.Password, dbCustomer.Email, dbCustomer.Name, dbCustomer.Name2, dbCustomer.Address, dbCustomer.Address2, MemberBillingAddress.Phone, MemberBillingAddress.Country, MemberBillingAddress.Company, MemberBillingAddress.City, MemberBillingAddress.State, MemberBillingAddress.Region, MemberBillingAddress.Zip, MemberBillingAddress.Fax, MemberShippingAddress.FirstName, MemberShippingAddress.LastName, MemberShippingAddress.Address1, MemberShippingAddress.Address2, MemberShippingAddress.Phone, MemberShippingAddress.Country, MemberShippingAddress.Company, MemberShippingAddress.City, MemberShippingAddress.State, MemberShippingAddress.Region, MemberShippingAddress.Zip, "", "", sMsg, "", "")
                        Email.SendHTMLMail(FromEmailType.NoReply, dbCustomer.Email, dbCustomer.Name & " " & dbCustomer.Name2, sSubject, sMsg)
                    End If
                End If

            Catch ex As Exception

            End Try
        End If


        If bRedirect Then
            DB.Close()
            Response.Redirect("/members/")
        End If
    End Sub
End Class
Imports Components
Imports DataLayer
Imports Utility

Partial Class SSPForgotpassword
    Inherits SitePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ltlResult.Text = ""
        If HasAccess() Then
            DB.Close()
            Response.Redirect("/members/default.aspx")
        End If
    End Sub

    Private Function CheckEmail() As Boolean
        Dim bcheck As Boolean
        Dim dsEmail As DataSet = DB.GetDataSet("SELECT * FROM Member WHERE CustomerId in (select customerid from customer where email = " & DB.Quote(txtEmail.Text) & ")")
        If dsEmail.Tables(0).Rows.Count > 1 Then
            ddlUser.DataSource = dsEmail
            ddlUser.DataTextField = "username"
            ddlUser.DataValueField = "password"
            ddlUser.DataBind()
            bcheck = True
            PChooseUser.Visible = True
        Else
            bcheck = False
            PChooseUser.Visible = True = False
        End If
        Return bcheck
    End Function

    Protected Sub btnRetrieve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetrieve.Click
        If Page.IsValid Then
            If Not CheckCaptcha(txtCaptcha.Text.Trim()) Then
                'AddError("Please try the code shown instead again")
                reqTxtCaptcha.Visible = False
                ltCapcha.Text = "<span class=""text-danger"">Please try the code shown instead again</span>"
                txtCaptcha.Text = ""
                Exit Sub
            End If

            Try
                Dim dbMember As MemberRow = MemberRow.GetRowByEmail(DB, txtEmail.Text)

                Dim dbCustomer As CustomerRow = CustomerRow.GetRow(DB, dbMember.CustomerId)
                Dim sMsg As String
                Dim sName As String = "forgotpassword@nss.com"
                Dim sSubject As String = "Your password for the nss.com website"
                Dim dbEmailTemplet As DataTable = EmailTempletRow.GetOutboundEmailTemplets(DB, "12")

                If dbEmailTemplet.Rows.Count > 0 Then
                    sName = dbEmailTemplet.Rows(0)("Name")
                    sSubject = dbEmailTemplet.Rows(0)("Subject")
                    sMsg = dbEmailTemplet.Rows(0)("Contents")
                    Dim cusName As String = String.Empty
                    If String.IsNullOrEmpty(dbCustomer.Name) Then
                        cusName = dbCustomer.Email
                    Else
                        cusName = dbCustomer.Name
                    End If
                    EmailTempletRow.FormatContentsEmailTemplet(cusName, dbCustomer.Name2, dbCustomer.Email, dbMember.Password, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", sMsg)


                Else ' send default
                    sMsg = "We have included your login information as you requested from the nss.com website.  Here are your login details: " & vbCrLf & vbCrLf & _
                "Username: " & dbMember.Username & vbCrLf & _
                "Password: " & dbMember.Password & vbCrLf & vbCrLf & _
                "Thank you," & vbCrLf & _
                "nss.com Customer Support"
                End If
                If dbMember.Customer.Email = "" Then
                    ltlResult.Text = "<span style='color: red;'>The email you entered could not be found in our system.  Please try again, or you may also create a new account.</span>"
                    txtCaptcha.Text = ""
                    Exit Sub
                End If
                If CheckEmail() = True Then
                    pnlSearch.Visible = False
                    PChooseUser.Visible = True
                    Exit Sub
                Else
                    Email.SendHTMLMail(FromEmailType.NoReply, dbMember.Customer.Email, dbMember.Customer.Email, sSubject, sMsg)
                End If
                pnlSearch.Visible = False
                PChooseUser.Visible = False
                Session("Captcha") = Nothing
                GoMsg("Forgot your password##We have sent an email message with your account details to the email address you specified.")



            Catch ex As Exception
                ltlResult.Text = "<span style='color: red;'>The email you entered could not be found in our system.  Please try again, or you may also create a new account.</span>"
            End Try
        End If
    End Sub

    Protected Sub btnRetrieve2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetrieve2.Click
        Try
            Dim dbMember As MemberRow = MemberRow.GetRowByEmail(DB, txtEmail.Text)
            Dim dbCustomer As CustomerRow = CustomerRow.GetRow(DB, dbMember.CustomerId)
            Dim sMsg As String
            Dim sName As String = "forgotpassword@nss.com"
            Dim sSubject As String = "Your password for the nss.com website"
            Dim dbEmailTemplet As DataTable = EmailTempletRow.GetOutboundEmailTemplets(DB, "12")

            If dbEmailTemplet.Rows.Count > 0 Then
                sName = dbEmailTemplet.Rows(0)("Name")
                sSubject = dbEmailTemplet.Rows(0)("Subject")
                sMsg = dbEmailTemplet.Rows(0)("Contents")
                Dim password As String = CryptData.Crypt.DecryptTripleDes(ddlUser.SelectedValue).ToString()
                EmailTempletRow.FormatContentsEmailTemplet(dbCustomer.Name, dbCustomer.Name2, ddlUser.SelectedItem.Text, password, "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", sMsg)

            Else ' send default
                sMsg = "We have included your login information as you requested from the nss.com website.  Here are your login details: " & vbCrLf & vbCrLf & _
            "Username: " & dbMember.Username & vbCrLf & _
            "Password: " & dbMember.Password & vbCrLf & vbCrLf & _
            "Thank you," & vbCrLf & _
            "nss.com Customer Support"
            End If
            If dbMember.Customer.Email = "" Then
                ltlResult.Text = "<span style='color: red;'>The username you entered could not be found in our system.  Please try again, or you may also create a new account.</span>"
                txtCaptcha.Text = ""
                Exit Sub
            End If

            Call Email.SendHTMLMail("forgotpassword@nss.com", sName, dbMember.Customer.Email, dbMember.Customer.Email, sSubject, sMsg)
            ltlResult.Text = "<span style='color: green;'>We have sent an email message with your account details to the email address you specified.</span><br><br><span style='color: Gray;'>After 5 minutes, if you don't receive an email with subject line ""Your password for the nss.com website"" from us, please check your Bulk or Spam mail folder. Or you can always contact our customer support at webmaster@nss.com</span>"

            pnlSearch.Visible = False
            PChooseUser.Visible = False
            Session("Captcha") = Nothing
        Catch ex As Exception
            ltlResult.Text = "<span style='color: red;'>The email you entered could not be found in our system.  Please try again, or you may also create a new account.</span>"
        End Try

    End Sub
End Class

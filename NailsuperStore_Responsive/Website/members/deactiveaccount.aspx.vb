Imports Components
Imports DataLayer
Imports Utility
Imports System.Text

Partial Class members_deactiveaccount
    Inherits SitePage
    Private DeactiveCode As String = ""
    Private Member As MemberRow = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DeactiveCode = Request("deactivedcode")
        If DeactiveCode <> "" Then
            Dim DeActive As Boolean
            Dim memberid As Integer
            Dim CusEmail As String = ""
            DeactiveCode = CryptData.Crypt.DecryptTripleDes(DeactiveCode)
            Dim res As DataTable = DB.GetDataTable("select m.*, c.Name, c.Name2, c.Email from Member m inner join Customer c on m.CustomerId = c.CustomerId where Memberid='" & DeactiveCode & "'")
            Try
                DeActive = CBool(res.Rows(0)("DeActive"))
            Catch ex As Exception
                DeActive = False
            End Try
            Try
                memberId = CInt(res.Rows(0)("MemberId"))
            Catch ex As Exception

            End Try
            If Not res Is Nothing Then
                If res.Rows.Count > 0 Then
                    CusEmail = res.Rows(0)("Email")
                    If Not DeActive And Request("send") <> "1" Then
                        SQL = "UPDATE Member SET Deactive ='true'  where memberid='" & DeactiveCode & "'"
                        If DB.ExecuteSQL(SQL) < 1 Then
                            Response.Redirect("/members/login.aspx")
                        End If
                        SQL = "UPDATE MailingMember SET Status='DELETED' WHERE Email = '" & CusEmail & "'"
                        DB.ExecuteSQL(SQL)
                        Email.SendHTMLMail(FromEmailType.NoReply, SysParam.GetValue("ToReportUnsubscribe"), "WebMaster", "Deactivate Account Report", "Email:" & CusEmail)
                    End If
                    ltlMsg.Text = "<div>Your account " & CusEmail & " is deactived successfull.</div>"
                    dvDeactive.Visible = False
                    'Send mail for admin
                End If
            End If
            If Request("send") <> "1" Then
                Session("MemberId") = Nothing
                Session("Username") = Nothing
                'Session("CheckoutType") = ""

                'Session("MemberInfo") = Nothing
                'Session("AddressInfo") = Nothing
                'Session("CustomerInfo") = Nothing
                ''Session("PointEarned") = Nothing
                Session("OrderId") = Nothing
                CookieUtil.SetTripleDESEncryptedCookie("OrderId", Nothing)
                CookieUtil.SetTripleDESEncryptedCookie("MemberId", Nothing)
                'CookieUtil.SetTripleDESEncryptedCookie("IdMember", Nothing)
                Response.Redirect(Request.RawUrl & "&send=1")
            End If
           
        Else
            If HasAccess() Then Member = MemberRow.GetRow(Session("MemberId"))
        End If

    End Sub
    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
        If Not CheckCaptcha(txtCaptcha.Text.Trim()) Then
            'AddError("Please try the code shown instead again")
            reqTxtCaptcha.Visible = False
            ltCapcha.Text = "<span style='color: red;'>Please try the code shown instead again</span>"
            txtCaptcha.Text = ""
            Exit Sub
        End If

        If Not Member.DeActive And ltlMsg.Text = "" Then
            DeactiveCode = CryptData.Crypt.EncryptTripleDes(Member.MemberId)
            DeactiveCode = Server.UrlEncode(DeactiveCode)
            Dim strMsg As String = ""
            strMsg = "Dear " & Member.Customer.Name & "," & _
            "<br><br>You have sent a request to deactivate your account " & Member.Customer.Email & "! To deactivate your account, please click on the link below:" & _
            "<br><br><a href='" & ConfigurationManager.AppSettings("GlobalRefererName") & "/members/deactiveaccount.aspx?deactivedcode=" & DeactiveCode & "'>Deactivate Your Account</a><br><br>If the above link does not work, please type in or copy and paste this link to your web browser:" & _
            "<br><a href='" & ConfigurationManager.AppSettings("GlobalRefererName") & "/members/deactiveaccount.aspx?deactivedcode=" & DeactiveCode & "'>" & ConfigurationManager.AppSettings("GlobalRefererName") & "/members/deactiveaccount.aspx?deactivedcode=" & DeactiveCode & "</a> <br><br>If you don't want to deactivate your account, please don't click on the link above and remove this email.<br><br>Best Regards, <br><br>The Nail Superstore<br>support@nss.com<br>www.nss.com<br>3804 Carnation St<br>Franklin Park, IL 60131, USA"
            'Send mail for customer
            Email.SendHTMLMail(FromEmailType.NoReply, Member.Customer.Email, Member.Customer.Name, "Deactivate Account", strMsg, SysParam.GetValue("ToReportUnsubscribe"))
            ltlMsg.Text = "<div class=""red"">To deactivate your account please check your email " & Member.Customer.Email & " and click the link on email.</div>"
            dvDeactive.Visible = False
        End If

    End Sub

End Class

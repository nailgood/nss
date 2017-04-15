Imports Components
Imports DataLayer
Imports System.Data.SqlClient
Imports Utility
Imports System.IO

Public Class Members_ActivedAccount
    Inherits SitePage
    Private logSubject As String = String.Empty
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        logSubject = "Log Active Account-" & Request.Url.AbsoluteUri.ToString()

        Dim sbJavascript As System.Text.StringBuilder = New System.Text.StringBuilder()
        sbJavascript.Append("<script language='javascript'>")
        sbJavascript.Append("function SetFocusOnUsername() {")
        sbJavascript.Append("var txtUN = document.getElementById('")
        sbJavascript.Append(txtEmail.ClientID)
        sbJavascript.Append("');")
        sbJavascript.Append("if (txtUN != null)txtUN.focus()}")
        sbJavascript.Append("</script>")

        If (Not ClientScript.IsClientScriptBlockRegistered("JSScript")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "JSScript", sbJavascript.ToString())
        End If

        Dim ActiveCode As String = String.Empty
        ActiveCode = Request("ActivedCode")
        If ActiveCode <> "" Then
            If ActiveCode = "newaccount" Then

                ltlMsg.Text = "<div class=""page-title"">Create Member Account</div>" & _
                                "<p>Thank you for registering with The Nail Superstore</p>" & _
                                "<p>To activate your account, please do the followings:</p>" & _
                                "<ol><li>Check your email and locate the email from The Nail Superstore with subject: ""nss.com Account Activation""</li>" & _
                                "<li>Click the link provided in the email.</li>" & _
                                "<li>Make sure your account is activated.</li></ol>" & _
                                "<p>If you do not confirm your account within 48 hours, your account will be cancelled.  You will not be able to access your account until you have completed the account activation steps.</p>" & _
                                "<p>If you have any problems activating your account, please contact a member of our support staffs at <a href=""mailto:support@nss.com"">support@nss.com</a></p>" & _
                                "<p>Best Regards,</p><br/>" & _
                                "<p>The Nail Superstore<br /><a href=""mailto:support@nss.com"">support@nss.com</a><br/>" & _
                                "<a href=""https://www.nss.com"">www.nss.com</a><br />" & _
                                "3804 Carnation St<br />" & _
                                "Franklin Park, IL 60131, USA<br />" & _
                                "Phone: 847-260-4000</p>"

                DivMsg.Visible = True
                DivActiveCode.Visible = False
                Exit Sub
            End If

            Dim linkRedirect As String = "/members/default.aspx"
            Try
                Dim sSQL As String = ""
                Dim isActive As Boolean = False
                Dim memberId As Integer = 0
                Dim useremail As String = ""
                Dim UseReferCode As String = ""
                Dim userName As String = String.Empty

                Dim res As DataTable = DB.GetDataTable("select m.isActive,m.MemberId,c.Email,m.password,m.UserName,[dbo].[fc_Member_UseReferCode](m.MemberId,c.Email) as UseReferCode, c.Name, c.Name2, c.Email from Member m inner join Customer c on m.CustomerId = c.CustomerId where activecode='" & ActiveCode & "'")
                If Not res Is Nothing Then
                    If res.Rows.Count > 0 Then
                        Try
                            isActive = CBool(res.Rows(0)("isActive"))
                        Catch ex As Exception
                            isActive = False
                        End Try
                        Try
                            memberId = CInt(res.Rows(0)("MemberId"))
                        Catch ex As Exception

                        End Try
                        useremail = res.Rows(0)("Email")
                        UseReferCode = res.Rows(0)("UseReferCode")
                        userName = res.Rows(0)("UserName")
                        If Not isActive Then
                            SQL = "UPDATE Member SET isActive ='true'  where activecode='" & ActiveCode & "'"
                            Dim key As String = String.Format(MemberRow.cachePrefixKey & "GetRow_{0}", memberId)
                            CacheUtils.RemoveCache(key)

                            If DB.ExecuteSQL(SQL) < 1 Then
                                Response.Redirect("/members/default.aspx")
                            End If

                            Dim mem As MemberRow = MemberRow.GetRow(memberId)
                            BaseShoppingCart.AwardedPoint(DB, mem.Customer.CustomerNo, memberId)

                            If Not (String.IsNullOrEmpty(UseReferCode)) Then
                                ''Add point for this user if has refer code
                                Utility.Common.AddPointReferActiveAccount(DB, memberId, useremail, UseReferCode, userName)
                            End If
                            ltlMsg.Text = "<br><br><span style=""color:#be048c; font-size:16px"">Thank for your activecode!<br><br> Your account is actived successfull. <br><br><br>Please <a href=""/members/default.aspx"" class=""bold"" style=""color:#be048c; font-size:16px""> click here </a> to sign in</span>"
                            DivActiveCode.Visible = False
                            Session("MemberId") = memberId
                            Session("Username") = res.Rows(0)("username")

                            Dim cookieOrderId As Integer = Utility.Common.GetOrderIdFromCartCookie()

                            If cookieOrderId > 0 Then
                                ''GENERATE last orderId
                                Dim lastOrderId As Integer = DB.ExecuteScalar("Select COALESCE(LastOrderId,0) from Member where Username='" & Session("Username") & "'")
                                If lastOrderId < 1 Then
                                    lastOrderId = StoreOrderRow.InsertUniqueOrder(DB, Context.Request.ServerVariables("REMOTE_ADDR"), memberId, Session.SessionID)

                                End If
                                If cookieOrderId > 0 And cookieOrderId <> lastOrderId Then
                                    StoreOrderRow.MergerCartItem(DB, cookieOrderId, lastOrderId)
                                    Session("OrderId") = lastOrderId
                                    Dim objCart As New ShoppingCart(DB, lastOrderId)
                                    objCart.Order.MemberId = memberId
                                    DB.ExecuteScalar("Update Member set LastOrderId=" & lastOrderId & " where MemberId=" & memberId & " ; Update StoreOrder set MemberId=" & memberId & " where OrderId=" & objCart.Order.OrderId)
                                    Cart.ResetAllCartDataLogin(DB, objCart)
                                    Utility.Common.DeleteCachePopupCart(Cart.Order.OrderId)

                                Else
                                    Session("OrderId") = lastOrderId
                                End If

                                Utility.Common.SetCartCookieLogin(memberId, lastOrderId)
                            Else
                                Utility.Common.SetCartCookieLogin(memberId, 0)
                            End If

                            If MemberRow.IsEbayCustomer(DB, memberId) Then
                                Dim dbMember As MemberRow = MemberRow.GetRow(memberId)
                                If Not dbMember.LastOrderId = Nothing Then
                                    Dim dbOrder As StoreOrderRow = StoreOrderRow.GetRow(DB, dbMember.LastOrderId)
                                    If dbOrder.ProcessDate = Nothing AndAlso dbOrder.MemberId = memberId Then
                                        Session("OrderId") = dbOrder.OrderId
                                        Utility.CookieUtil.SetTripleDESEncryptedCookie("OrderId", dbOrder.OrderId)
                                    End If
                                Else
                                    Session("OrderId") = Nothing
                                    Utility.CookieUtil.SetTripleDESEncryptedCookie("OrderId", Nothing)
                                End If
                                linkRedirect = "/members/register.aspx"
                            End If

                        End If
                    End If
                End If

            Catch ex As Exception
                Email.SendError("ToError500", logSubject, "Active Code: " & ActiveCode & ",Active Error:" & ex.Message)
            End Try

            Response.Redirect(linkRedirect)
            'ElseIf ActiveCode = "" Then
            '    Dim email As String
            '    email = txtEmail.Text
            '    GetActiveCode(email)
        Else
            DivActiveCode.Visible = True
            DivMsg.Visible = False
            ltlMsg.Text = "<br><br><span style=""color:#be048c; font-size:14px"">Require active code?<br><br></span>"
        End If
    End Sub

    Private Sub GetActiveCode(ByVal strEmail As String)
        Dim res As DataTable = DB.GetDataTable("SELECT ActiveCode, Password, IsActive FROM Member WHERE Username = '" & strEmail & "'")
        If res.Rows.Count > 0 Then
            Dim i As Integer
            Dim ActiveCode As String = ""
            Dim Password As String = ""
            Dim IsActive As Integer = 0
            For i = 0 To res.Rows.Count - 1
                ActiveCode = res.Rows(i)("ActiveCode")
                Password = res.Rows(i)("Password")
                IsActive = res.Rows(i)("IsActive")
                If ActiveCode <> "" And IsActive = 0 Then
                    Dim sActiveCode As String = ActiveCode
                    Dim sPassword As String = Password
                    Dim Decrypted = CryptData.Crypt.DecryptTripleDes(sPassword)
                    sPassword = Decrypted
                    Dim sName As String = "The Nail Superstore"
                    Dim sSubject As String = "Activate Your Account"
                    Dim strContents As String = String.Empty
                    Dim objReader As StreamReader
                    Try
                        objReader = New StreamReader(Utility.ConfigData.ActiveAccountTemplatePath)
                        strContents = objReader.ReadToEnd()
                        objReader.Close()
                        strContents = String.Format(strContents, txtEmail, Utility.ConfigData.GlobalRefererName, sActiveCode, sPassword)
                        If Email.SendHTMLMail(FromEmailType.NoReply, strEmail, strEmail, sSubject, strContents) = True Then
                            DivActiveCode.Visible = False
                            ltlCompleteMsg.Text = " <h1>Activate Your Account</h1> " & _
                                                  " <p>We have sent an email message with your active code to the email address you specified.</p> " & _
                                                  " <p><span style='color: #F00;'>After 5 minutes, if you don't receive an email with subject line ""Account Activation"" from us, please check your Bulk or Spam mail folder.</span></p>"
                        End If
                    Catch ex As Exception
                        Email.SendError("ToError500", "ActivedAccount > GetActiveCode", "Email: " & strEmail & "<br>Exception: " & ex.ToString())
                    End Try
                Else
                    ltlResult.Text = "<span style='color: #333;'>The Email you entered had registered by other. <br/> If you forgotpassword, please <a style='color: #3b76ba' href=""/members/forgotpassword.aspx""> click here </a> to retrieve your password. </span>"
                End If
            Next
        End If
    End Sub

    Private Sub GetUser(ByVal activeCode As String)
        Try
            Dim sSQL As String = ""
            Dim res As DataTable = DB.GetDataTable("select * from Member where  activecode='" & activeCode & "'")
            If Not res Is Nothing Then
                Dim id As String = ""
                Dim username As String = ""
                If res.Rows.Count > 0 Then
                    Try
                        id = res.Rows(0)("MemberId")
                    Catch ex As Exception
                        Email.SendError("ToError500", logSubject, "GetUser Funtion: get MemberId Error")
                    End Try
                    Try
                        username = res.Rows(0)("username")
                    Catch ex As Exception
                        Email.SendError("ToError500", logSubject, "GetUser Funtion: get username Error")
                    End Try
                    Email.SendError("ToError500", logSubject, "GetUser Funtion: user name:& " & username & ",id:" & id)

                Else
                    Email.SendError("ToError500", logSubject, "GetUser Funtion: count row table=0")

                End If
            Else
                Email.SendError("ToError500", logSubject, "GetUser Funtion: table is nothing ")
            End If
        Catch ex As Exception
            Email.SendError("ToError500", logSubject, "GetUser Funtion,Catch ex As Exception: " & ex.Message)
        End Try
    End Sub

    Private Sub GetCoupon(ByRef Msg As String)
        Dim res As DataTable = DB.GetDataTable("select * from StorePromotion where IsRegisterSend='true'")
        If res.Rows.Count > 0 Then
            Dim i As Integer
            Dim PromotionCode As String = ""
            'Msg = Msg & " Congratulation!. Now, we have The Special Discounts for new member : " & vbCrLf
            For i = 0 To res.Rows.Count - 1
                PromotionCode = res.Rows(i)("PromotionCode")
                If PromotionCode <> "" Then
                    Msg = Msg & " PromotionCode :" & PromotionCode & " "
                End If
            Next
        End If

    End Sub

    Protected Sub btnRetrieve_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRetrieve.Click

        If Page.IsValid Then

            If Not CheckCaptcha(txtCaptcha.Text.Trim()) Then
                ' AddError("Please try the code shown instead again")
                ltCapcha.Text = "<span class=""text-danger"">Please try the code shown instead again</span>"
                txtCaptcha.Text = ""
                Exit Sub
            End If

            Try
                txtCaptcha.Text = ""

                Dim dbMember As MemberRow = MemberRow.GetRowByEmail(DB, txtEmail.Text)
                Dim dbCustomer As CustomerRow = CustomerRow.GetRow(DB, dbMember.CustomerId)
                Dim sMsg As String = ""
                Dim sCoupon As String = ""
                Dim sActiveCode As String = ""
                Dim sName As String = "activedcode@nss.com"
                Dim sSubject As String = "Your Activedcode for the nss.com website"

                If dbMember.Customer.Email = "" Then
                    'AddError("The Email you entered could not be found in our system. Please try again, or you may also create a new account.")
                    ltlResult.Text = "<span style='color: red;'>The Email you entered could not be found in our system. Please try again, or you may also create a new account.</span>"
                    Exit Sub
                End If

                'Author: Cuong
                GetActiveCode(txtEmail.Text)
                
            Catch ex As Exception
            'AddError("The Email you entered could not be found in our system. Please try again, or you may also create a new account.")
            ltlResult.Text = "<span style='color: red;'>The Email you entered could not be found in our system. Please try again, or you may also create a new account.</span>"
        End Try
        End If
    End Sub
End Class

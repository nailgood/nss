Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Public Class Members_Login
    Inherits SitePage

    Private act As String = ""
    Dim iTemplate As Integer = Integer.MinValue
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        CheckLoginFrom()

        If Not Page.IsPostBack Then
            LoadMetaData(DB, "/members/login.aspx")
            If Request.UrlReferrer IsNot Nothing Then
                If (Request.QueryString("fromQuickOrderPage") Is Nothing) Then
                    ViewedItemRow.updateSearchResult(String.Empty, Request.UrlReferrer.PathAndQuery, "Login")
                End If
            End If
        End If

        CheckRedirectFromTemplate()
        If Not Session("MemberId") Is Nothing Then
            Response.Redirect("/members/")
        End If

        If Request.QueryString("err") = "1" Then AddError("Invalid login. Please try again")
    End Sub

    Private Sub CheckRedirectFromTemplate()
        Try
            iTemplate = Request.QueryString("template")
        Catch ex As Exception
        End Try

        If (iTemplate > 0) Then
            AddError("To complete your purchase, please sign in or register a new account. Thank you!")
        End If
    End Sub

    Private Sub CheckLoginFrom()
        Try
            act = Request.QueryString("act")
        Catch ex As Exception
        End Try

        If (act = "checkout") Then
            litTitle1.Text = Resources.Msg.LoginTitle1
            btnLogin.Text = Resources.Msg.ButtonLogin1
            btnRegister.Text = Resources.Msg.ButtonRegister1
            pCreateAccount.Visible = True
        Else
            btnLogin.Text = Resources.Msg.ButtonLogin2
            litTitle1.Text = Resources.Msg.LoginTitle2
            btnRegister.Text = Resources.Msg.ButtonRegister2
            pCreateAccount.Visible = False
        End If
    End Sub

    Protected Sub btnRegister_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRegister.Click
        If (act = "checkout") Then
            Utility.Common.OrderLog(0, "Checkout as Guest", Nothing)
            Response.Redirect("/store/billing.aspx?act=guest")
        Else
            Response.Redirect("/members/register.aspx")
        End If
    End Sub

    Protected Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Page.Validate()

        If Page.IsValid Then
            Login(0)
        End If
    End Sub

    Private Sub Login(ByVal MemberId As Integer)
        If (MemberId < 1) Then
            MemberId = MemberRow.ValidateMemberCredentials(DB, txtUsername.Text, txtPassword.Text)
        End If

        If MemberId > 0 Then
            Dim dbMember As MemberRow = MemberRow.GetRowLogin(MemberId)

            If dbMember.IsActive = False Then
                AddError("The username you entered has not been activated.")
                Exit Sub
            End If
            If dbMember.DeActive = True Then
                AddError("The account you entered has been deactivated.")
                Exit Sub
            End If

            ViewedItemRow.UpdateMemberId(Session.SessionID, MemberId)

            Dim isMergerCart As Boolean = False
            Dim countItemCartBeforeMerger As Integer = 0
            Dim cookieOrderId As Integer = Utility.Common.GetOrderIdFromCartCookie()
            Dim cookieMemberId As Integer = Utility.Common.GetMemberIdFromCartCookie()
            If dbMember.LastOrderId = Nothing AndAlso cookieOrderId > 0 Then
                dbMember.LastOrderId = StoreOrderRow.InsertUniqueOrder(DB, Context.Request.ServerVariables("REMOTE_ADDR"), dbMember.MemberId, Session.SessionID)
            End If

            If Not dbMember.LastOrderId = Nothing Then
                ''merger cart
                Dim isSameAccountLogin As Boolean = False
                If (cookieMemberId > 0) Then
                    If (cookieMemberId = dbMember.MemberId) Then
                        isSameAccountLogin = True
                    Else
                        isSameAccountLogin = False
                    End If
                Else
                    isSameAccountLogin = True
                End If
                If cookieOrderId > 0 And cookieOrderId <> dbMember.LastOrderId And isSameAccountLogin Then
                    countItemCartBeforeMerger = DB.ExecuteScalar("Select COALESCE(sum(Quantity),0) from StoreCartItem where OrderId=" & dbMember.LastOrderId & " and type='item'")
                    StoreOrderRow.MergerCartItem(DB, cookieOrderId, dbMember.LastOrderId)
                    Dim countItemCartAfterMerger As Integer = DB.ExecuteScalar("Select COALESCE(sum(Quantity),0) from StoreCartItem where OrderId=" & dbMember.LastOrderId & " and type='item'")

                    If countItemCartBeforeMerger <> countItemCartAfterMerger Then
                        isMergerCart = True
                    End If
                End If

                Dim dbOrder As StoreOrderRow = StoreOrderRow.GetRow(DB, dbMember.LastOrderId)
                If dbOrder.OrderNo = Nothing Then 'Update Order PurchasePoint
                    StoreOrderRow.RemoveItemPointByOrder(DB, dbOrder.OrderId)
                    dbOrder.PurchasePoint = 0
                    dbOrder.TotalPurchasePoint = 0
                    dbOrder.PointMessage = ""
                    Dim isNotCompleteAddress As Boolean = MemberAddressRow.IsNotCompleteAddress(DB, dbMember.MemberId)
                    If isNotCompleteAddress Then
                        dbOrder.IsSameAddress = True
                    End If
                    dbOrder.PointAmountDiscount = 0
                    dbOrder.PointLevelMessage = ""
                    'get iplocation
                    dbOrder.IPLocation = GetCityLocation(dbOrder.RemoteIP)
                    dbOrder.Update()
                    Session("MemberId") = MemberId
                    Session("OrderId") = dbOrder.OrderId

                    Dim objCart As New ShoppingCart(DB, dbOrder.OrderId)
                    objCart.Order.MemberId = dbMember.MemberId
                    DB.ExecuteScalar("UPDATE StoreOrder SET MemberId=" & dbMember.MemberId & " WHERE OrderId=" & dbOrder.OrderId)
                    Cart.DeleteMixMatchFreeItemIsNotValid(DB, dbOrder.OrderId)
                    Dim isAllowReset As Boolean = True
                    If (isMergerCart) Then
                        If (countItemCartBeforeMerger < 1) Then
                            isAllowReset = False
                        End If
                    End If
                    If (isAllowReset) Then
                        Cart.ResetAllCartDataLogin(DB, objCart)
                    End If

                End If

                'End Update Order PurchasePoint
                If dbOrder.ProcessDate = Nothing AndAlso dbOrder.MemberId = MemberId Then
                    Session("OrderId") = dbOrder.OrderId
                Else
                    Session.Remove("OrderId")
                    dbMember.LastOrderId = Nothing
                    DB.ExecuteSQL("Update Member set LastOrderId=0 where MemberId=" & dbMember.MemberId)
                End If
            Else
                Session.Remove("OrderId")
            End If

            Dim CustomerPriceGroupId As Integer = MemberRow.GetCustomerPriceGroupIdByMember(MemberId)
            Session("CustomerPriceGroupId") = CustomerPriceGroupId

            ' Valid login
            Session("MemberId") = MemberId
            Session("Username") = txtUsername.Text
            Session("Language") = LanguageCode.GetLanguageCode(dbMember.Customer.LanguageCode)

            'set LastLoginDate
            MemberRow.SetLastLoginDate(DB, MemberId)
            Utility.Common.SetCartCookieLogin(MemberId, dbMember.LastOrderId)

            Dim sRedirect As String = String.Empty
            If Not Request.QueryString("act") Is Nothing AndAlso Request.QueryString("act") = "checkout" Then
                If (isMergerCart AndAlso countItemCartBeforeMerger > 0) Then
                    Response.Redirect("/store/cart.aspx?act=mergecart")
                Else
                    Response.Redirect("/store/cart.aspx?act=checkout")
                End If
            ElseIf Request.QueryString("url") IsNot Nothing Then
                Response.Redirect(Request.QueryString("url"))
            End If

            sRedirect = ViewedItemRow.getLastUrlByPageType("Login", True)
            If String.IsNullOrEmpty(sRedirect) Then
                sRedirect = "/"
            End If
            'If Not Trim(Session("LastWebsiteURL")).Contains("msg.aspx") Then
            '    sRedirect = Trim(Session("LastWebsiteURL"))
            'End If
            If Not String.IsNullOrEmpty(sRedirect) Then
                If (sRedirect.Contains("/home.aspx")) Then
                    sRedirect = "/"
                End If
            Else
                sRedirect = "/members/default.aspx"
            End If

            Try
                If Request("act") = "r" Then
                    Dim arrItem As String() = sRedirect.Split("/")
                    Dim ItemId As Integer = 0
                    If arrItem(arrItem.Length - 1) <> Nothing Then
                        ItemId = DB.ExecuteScalar("Select ItemId From StoreItem where UrlCode = '" & arrItem(arrItem.Length - 1) & "'")
                        sRedirect = "/store/review.aspx?ItemId=" & ItemId
                    End If
                End If
            Catch ex As Exception
                Response.Redirect(ConfigurationManager.AppSettings("GlobalRefererName"))
            End Try
            DB.Close()

            If iTemplate > 0 Then
                sRedirect = "/store/AddTemplateCart.aspx?Id=" & iTemplate
            End If

            Response.Redirect(sRedirect)
        ElseIf MemberId = 0 Then
            AddError("The password you entered does not match the one for this account. Please try again, or go to the <a href='/members/forgotpassword.aspx'>forgot your password</a> page to retrieve it.")
        Else
            If txtUsername.Text.Contains("@") Then
                AddError("Your email is not found in our system database. Please try it again or create a new account.")
            Else
                AddError("Your username is not found in our system database. Please try it again or create a new account.")
            End If

        End If

    End Sub
End Class
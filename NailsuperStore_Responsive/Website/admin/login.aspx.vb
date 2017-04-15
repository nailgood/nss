Imports Components
Imports DataLayer

Partial Class admin_Login
    Inherits BasePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        
    End Sub

    Protected Sub Login_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Dim ip As String = HttpContext.Current.Request.ServerVariables("REMOTE_ADDR").ToString

        ' Validate Login
        Dim newUser As AdminPrincipal = AdminPrincipal.ValidateLogin(DB, Username.Value, UserPass.Value, System.Configuration.ConfigurationManager.AppSettings("DomainGroupNames"), ip)

        ' If Login/Password correct then replace Context.User
        If newUser Is Nothing Then
            Msg.Text = "Invalid Credentials. Please try again."
        ElseIf Not newUser.IsIpAccessValid Then
            Msg.Text = "You are not authorized to access Admin pages."
        Else
            Context.User = newUser
            SetAuthCookie(newUser.Username, PersistCookie.Checked)
            ''log action
            Dim dbAdminLog As New AdminLogRow()
            dbAdminLog.AdminId = CType(newUser.Identity, AdminIdentity).AdminId
            dbAdminLog.Username = Username.Value
            dbAdminLog.RemoteIP = Request.ServerVariables("REMOTE_ADDR")
            dbAdminLog.LoginDate = Now()
            dbAdminLog.ComputerName = System.Net.Dns.GetHostName()
            dbAdminLog.LanIP = Utility.Common.GetComputerLanIP()
            Dim logId As Integer = AdminRow.DoLogin(DB, dbAdminLog)
            Session("AdminId") = CType(newUser.Identity, AdminIdentity).AdminId
            Session("LogId") = logId
            Session("Track_AdminName") = Username.Value
            '' Dim fram As String = Session("FrameURL")
            Dim lastURL As String = Session("LastAdminURL")
            Try
                If Not String.IsNullOrEmpty(lastURL) Then
                    lastURL = lastURL.Replace("//admin", "/admin")
                End If
            Catch
            End Try


            If String.IsNullOrEmpty(lastURL) Then
                lastURL = "/admin/default.aspx"
            End If
            Response.Redirect(lastURL)
        End If
    End Sub

    Sub SetAuthCookie(ByVal username As String, ByVal Persist As Boolean)
        If Persist Then
            Dim ticket As FormsAuthenticationTicket = New FormsAuthenticationTicket(2, username, DateTime.Now, DateTime.Now.AddYears(1), True, "", FormsAuthentication.FormsCookiePath)
            Dim cookie As HttpCookie = New HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))

            cookie.HttpOnly = True
            cookie.Path = FormsAuthentication.FormsCookiePath
            cookie.Secure = FormsAuthentication.RequireSSL
            cookie.Expires = ticket.Expiration

            Response.Cookies.Add(cookie)
        Else
            FormsAuthentication.SetAuthCookie(username, False)
        End If
    End Sub

End Class

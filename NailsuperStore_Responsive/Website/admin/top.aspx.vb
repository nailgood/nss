Imports Components
Imports DataLayer

Partial Class admin_TopPage
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If TypeOf Context.User Is AdminPrincipal Then
            ltlFullName.Text = CType(Context.User.Identity, AdminIdentity).FirstName & " " & CType(Context.User.Identity, AdminIdentity).LastName
        End If
        If "admin" <> LoggedInUsername Then
            ltlChangePassword.Text = AdminMenu.MenuTopEmptyRoot("/admin/password/index.aspx", "Change Password")
        End If
        If HasRights("USERS") Then
            ltlSystemParameters.Text = AdminMenu.MenuTopEmptyRoot("/admin/settings/", "System Setting")
            ltlLog.Text = AdminMenu.MenuTopEmptyRoot("/admin/adminlog/LogAction.aspx", "Logs")
        End If
        ltlLogout.Text = AdminMenu.MenuTopEmptyRoot("/admin/logout.aspx", "Sign out")
    End Sub

End Class

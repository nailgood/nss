Imports Components

Partial Class admin_logout
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FormsAuthentication.SignOut()
        Session.Abandon()
        Response.Redirect("/admin/")
    End Sub
End Class

Imports Components
Imports DataLayer

Partial Class admin_password_PasswordChange
    Inherits AdminPage

    Private Sub SubmitButton_onClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles submitButton.Click
        Dim dbAdmin As AdminRow = AdminRow.GetRow(DB, LoggedInAdminId)

        Dim isValid As Boolean = (dbAdmin.Password = PASSWORD_OLD.Text)
        If isValid Then
            DB.BeginTransaction()
            dbAdmin.Password = PASSWORD_NEW.Text
            dbAdmin.Update()
            DB.CommitTransaction()

            Session("Confirmation") = "Your password has been successfully changed."
            Response.Redirect("/admin/confirm.aspx")
        Else
            AddError("The Old Password does not match your current password.  Please try again.")
        End If
    End Sub

    Private Sub Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel.Click
        Response.Redirect("/admin/main.aspx")
    End Sub

End Class

Imports DataLayer
Imports System.Data
Imports Components

Partial Class AdminLastLoginActivity
    Inherits BaseControl

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        BindLastActivityRepeater()
    End Sub

    Private Sub BindLastActivityRepeater()
        Dim ds As DataSet = AdminLogRow.GetLast10Logins()

        LastActivity.DataSource = ds
        LastActivity.DataBind()

        If ds.Tables(0).Rows.Count = 0 Then
            LastActivity.Visible = False
        Else
            LastActivity.Visible = True
        End If
    End Sub
End Class

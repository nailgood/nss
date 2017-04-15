Imports Components

Partial Class admin_store_cart__default
    Inherits AdminPage

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    Private Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSearch.Click
        If Not IsValid Then Exit Sub
        Response.Redirect("abandoned.aspx?Start=" & F_OrderDateLbound.Value & "&End=" & F_OrderDateUbound.Value & "&Username=" & F_Username.Text)
    End Sub
End Class

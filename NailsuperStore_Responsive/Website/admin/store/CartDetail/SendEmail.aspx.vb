Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Imports System.IO

Partial Class admin_store_cartDetail_sendEmail
    Inherits AdminPage
    Protected OrderId As Integer
    Protected c As ShoppingCart
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        OrderId = CType(Request("OrderId"), Integer)
        If Not IsPostBack Then
            Try
                c = New ShoppingCart(DB, OrderId, True)
                If c.Order.Email <> "" Then
                    F_Email.Text = c.Order.Email
                Else
                    Dim dbmember As MemberRow = MemberRow.GetRow(c.Order.MemberId)
                    Dim dbcustomer As CustomerRow = CustomerRow.GetRow(DB, dbmember.CustomerId)
                    F_Email.Text = dbcustomer.Email
                End If
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Response.Redirect(Me.Request.RawUrl)
    End Sub
    Private Sub btnSendEmail_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSendEmail.Click
        If Not IsValid Then Exit Sub
        Try
            c = New ShoppingCart(DB, OrderId, True)
            Dim dbmember As MemberRow = MemberRow.GetRow(c.Order.MemberId)
            Dim dbcustomer As CustomerRow = CustomerRow.GetRow(DB, dbmember.CustomerId)
            Email.SendHTMLMail(FromEmailType.NoReply, F_Email.Text, dbcustomer.Name, F_Subject.Text, F_Content.Text)

        Catch ex As Exception
        End Try
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class


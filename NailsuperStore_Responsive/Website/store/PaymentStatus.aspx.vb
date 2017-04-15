Imports Components
Imports DataLayer
Imports Utility
Partial Class PaymentStatus
    Inherits SitePage
    Private Member As MemberRow
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request("status") = "success" And Request("tx") IsNot Nothing Then
            If Not IsWebTest() AndAlso Not (Request("st") = "Completed") Then
                Response.Redirect("/store/cart.aspx")
                Exit Sub
            End If

            Dim requestOrderId As Integer = 0
            If Session("MemberId") = Nothing Then
                Session("MemberId") = Request("MemberId")
            End If

            If Request("OrderId") IsNot Nothing Then
                requestOrderId = Request("OrderId")
            Else
                requestOrderId = Session("OrderId")
            End If

            'Dim dtOrder As DataTable = DB.GetDataTable("Select coalesce(Email,'') as Email,coalesce(BillToCountry,'') as BillToCountry from StoreOrder where OrderId=" & requestOrderId)
            'Dim OrderBillToCountry As String = String.Empty
            'Dim OrderEmail As String = String.Empty
            'If Not dtOrder Is Nothing AndAlso dtOrder.Rows.Count > 0 Then
            '    OrderBillToCountry = dtOrder.Rows(0)("BillToCountry")
            '    OrderEmail = dtOrder.Rows(0)("Email")
            'End If

            'If OrderEmail = Nothing Then
            '    If OrderBillToCountry = Nothing OrElse OrderBillToCountry = "US" Then
            '        Response.Redirect("/store/billing.aspx")
            '    Else
            '        Response.Redirect("/store/billingint.aspx")
            '    End If
            'End If

            'Send email to backup
            Dim strContent As String = "Date: " + DateTime.Now.ToString()
            strContent += "<br>OrderId: " + requestOrderId.ToString()
            strContent += "<br>MemberId: " + Request("MemberId")
            Email.SendReport("ToReportPayment", "[Paypal] Redirect", strContent)

            Dim OrderId As String = requestOrderId 'System.Web.HttpUtility.UrlEncode(Utility.Crypt.EncryptTripleDes(requestOrderId))

            DB.ExecuteSQL(String.Format("UPDATE StoreOrder SET PaymentType = 'PAYPAL' WHERE OrderId = {0}; Update Member set LastOrderId=0 where MemberId={1}", OrderId, Common.GetCurrentMemberId()))
            Utility.Common.SetOrderToCartCookie(0)
            Session.Remove("OrderId")
            Response.Redirect("/store/confirmation.aspx?OrderId=" & OrderId)
        Else
            Response.Redirect("/store/cart.aspx")
        End If
    End Sub
End Class

Imports Components

Partial Class admin_store_cart_abandoned
    Inherits AdminPage

    Protected TotalOrders As Integer = 0
    Protected TotalSubtotal As Double = 0
    Protected TotalIncomplete As Integer = 0
    Protected TotalCompleted As Integer = 0
    Protected TotalCompleteSubtotal As Double = 0
    Protected TotalIncompleteSubtotal As Double = 0
    Protected dStart, dEnd As DateTime
    Protected Username As String = Nothing

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            dStart = Request("Start")
            dEnd = Request("End")
            Username = Request("Username")
            If dStart = Nothing OrElse dEnd = Nothing Then Throw New ApplicationException("")

            Dim SQL As String = "select CheckoutPage, count(*) as iCount, sum(basesubtotal) as subtotal, avg(basesubtotal) as average, case when processdate is not null then 1 else 0 end as Completed, case when coalesce(createsessionid,'') <> coalesce(processsessionid,'') and processdate is not null then 1 else 0 end as IsDifferentSession from storeorder where createdate >= " & DB.Quote(dStart) & " and createdate < " & DB.Quote(dEnd.AddDays(1)) & IIf(Username = Nothing, "", " and memberid = " & DB.Number(DB.ExecuteScalar("select top 1 memberid from member where username = " & DB.Quote(Username)))) & " group by case when processdate is not null then 1 else 0 end, CheckoutPage, case when coalesce(createsessionid,'') <> coalesce(processsessionid,'') and processdate is not null then 1 else 0 end order by completed, isdifferentsession"
            Dim dt As DataTable = DB.GetDataTable(SQL)
            gvList.DataSource = dt
            gvList.DataBind()

        Catch ex As Exception
            Response.Write(ex.ToString)
            'Response.Redirect("default.aspx")
        End Try
    End Sub

    Private Sub gvList_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvList.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            TotalOrders += e.Row.DataItem("iCount")
            TotalSubtotal += e.Row.DataItem("subtotal")
            If CBool(e.Row.DataItem("Completed")) Then
                TotalCompleteSubtotal += e.Row.DataItem("subtotal")
                TotalCompleted += e.Row.DataItem("iCount")
            Else
                TotalIncompleteSubtotal += e.Row.DataItem("subtotal")
                TotalIncomplete += e.Row.DataItem("iCount")
            End If
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            CType(e.Row.FindControl("ltlOrdersTotal"), Literal).Text = FormatNumber(TotalIncomplete, 0) & "<br />" & FormatNumber(TotalCompleted, 0) & "<br />" & FormatNumber(TotalOrders, 0) & "<br />" & FormatPercent(TotalCompleted / TotalOrders, 2)
            CType(e.Row.FindControl("ltlAverageTotal"), Literal).Text = FormatCurrency(TotalSubtotal / TotalOrders)
            CType(e.Row.FindControl("ltlAverageComplete"), Literal).Text = FormatCurrency(TotalCompleteSubtotal / TotalOrders)
            CType(e.Row.FindControl("ltlAverageIncomplete"), Literal).Text = FormatCurrency(TotalIncompleteSubtotal / TotalOrders)
        End If
    End Sub
End Class

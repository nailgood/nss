Imports Components
Imports DataLayer
Imports System.IO
Partial Class admin_store_orders_reviews_view
    Inherits AdminPage

    Private OrderId As Integer = 0
    Private sor As StoreOrderReviewRow
    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        OrderId = Request.QueryString("OrderId")
        If Not Page.IsPostBack Then
            If OrderId <> Nothing Then
                sor = StoreOrderReviewRow.GetRow(DB, CInt(OrderId))
                OrderId = sor.OrderId
                LoadItem()
            End If
        End If
    End Sub
    Private Sub LoadItem()
        Try
            If sor.IsActive = True Then
                btnActive.Visible = False
            End If
            If sor.IsShared = True Then
                btnShare.Visible = False
            End If
            lblname.InnerText = DB.ExecuteScalar("Select isnull(c.Name,so.BillToName) as NameReview From StoreOrderReview sor inner join StoreOrder so on sor.OrderId = so.OrderId inner join Member m on m.MemberId = so.MemberId inner join Customer c on c.CustomerId = m.CustomerId Where sor.OrderId = " & OrderId)
            lblarrived.InnerText = IIf(sor.ItemArrived = True, "Yes", "No")
            lbldescibled.InnerText = IIf(sor.ItemDescribed = True, "Yes", "No")
            Select Case sor.ServicePrompt
                Case 0
                    lblservice.InnerText = "No"
                Case 1
                    lblservice.InnerText = "Yes"
                Case 2
                    lblservice.InnerText = "Did not contact"
            End Select
            ltImage.Text = "<img src=""/includes/theme/images/star" & sor.NumStars & "0.png"" style=""border-style:none"">"
            lblContent.InnerText = BBCodeHelper.ConvertBBCodeToHTML(sor.Comment)
        Catch ex As Exception

        End Try
    End Sub
    

    Protected Sub btnActive_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActive.Click
        If OrderId > 0 Then
            Dim logDetail As New AdminLogDetailRow
            sor = StoreOrderReviewRow.GetRow(DB, CInt(OrderId))
            sor.IsActive = True
            logDetail.Message = "IsActive|False|True[br]"
            If sor.IsActive = True Then
                Dim o As StoreOrderRow = StoreOrderRow.GetRow(DB, OrderId)
                Dim cp As CashPointRow = New CashPointRow(DB)
                cp = cp.SetValueCashPoint(cp, o, CInt(Session("Admin")), 1)
                If cp.CheckTransactionNoExists(DB, cp.MemberId, cp.TransactionNo) = False Then
                    cp.Insert()
                    logDetail.Message &= "AddPoint|False|True[br]"
                End If
            End If
            sor.Update()

            logDetail.ObjectType = Utility.Common.ObjectType.OrderReview.ToString()
            logDetail.ObjectId = sor.OrderId
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            ''Response.Write("<script language='javascript'> {  window.opener.SetValue('1') ; window.close();}</script>")
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "ActiveReview", "ReloadParent();", True)



        End If
    End Sub

    Protected Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        '' Response.Write("<script language='javascript'> { window.close();}</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "ClosePopup", "ClosePopup();", True)

    End Sub

    Protected Sub btnEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        ''Response.Write("<script language='javascript'> {  window.opener.SetValue('edit.aspx?OrderId=" & OrderId & "') ; window.close();}</script>")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "EditReview", "EditReview('edit.aspx?OrderId=" & OrderId & "');", True)

    End Sub

    Protected Sub btnShare_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShare.Click
        If OrderId > 0 Then
            sor = StoreOrderReviewRow.GetRow(DB, CInt(OrderId))
            Dim sorBefore As StoreOrderReviewRow = CloneObject.Clone(sor)
            Dim logDetail As New AdminLogDetailRow
            sor.IsShared = True
            sor.IsActive = True
            logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.OrderReview, sorBefore, sor)
            '' sor.Update()
            Dim o As StoreOrderRow = StoreOrderRow.GetRow(DB, OrderId)
            Dim cp As CashPointRow = New CashPointRow(DB)
            cp = cp.SetValueCashPoint(cp, o, CInt(Session("Admin")), 1)
            If cp.CheckTransactionNoExists(DB, cp.MemberId, cp.TransactionNo) = False Then
                cp.Insert()
                logDetail.Message &= "AddPoint|False|True[br]"
            End If
            sor.Update()

            logDetail.ObjectId = sor.OrderId
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            logDetail.ObjectType = Utility.Common.ObjectType.OrderReview.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            ''Response.Write("<script language='javascript'> {  window.opener.SetValue('1') ; window.close();}</script>")
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "ShareReview", "ReloadParent();", True)
        End If
    End Sub
End Class

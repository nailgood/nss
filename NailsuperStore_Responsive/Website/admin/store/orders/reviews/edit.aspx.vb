Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_orders_reviews_Edit
    Inherits AdminPage

    Protected OrderId As Integer
    Protected ItemId As Integer
    Protected EditFromEmail As Boolean
    Public transExists As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        OrderId = Convert.ToInt32(Request("OrderId"))

        If Not IsPostBack Then
            EditFromEmail = False
            If Not Request.QueryString("editFromEmail") Then
                If Request.QueryString("editFromEmail") = "1" Then
                    EditFromEmail = True
                End If
            End If
            LoadFromDB()
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "InitEditor", "InitEditor();", True)
        End If
    End Sub

    Private Sub LoadFromDB()
       
        If OrderId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreOrderReview As StoreOrderReviewRow = StoreOrderReviewRow.GetRow(DB, OrderId)
        Dim Member As MemberRow = MemberRow.GetRow(dbStoreOrderReview.MemberId)
        Session("MemberId") = dbStoreOrderReview.MemberId
        hplEmail.NavigateUrl = "../../../members/edit.aspx?MemberId=" & dbStoreOrderReview.MemberId
        hplOrder.NavigateUrl = "../edit.aspx?OrderId=" & dbStoreOrderReview.OrderId
        hplEmail.Text = Member.Username
        hplOrder.Text = dbStoreOrderReview.OrderNo
        drArrived.SelectedValue = dbStoreOrderReview.ItemArrived
        drDescribled.SelectedValue = dbStoreOrderReview.ItemDescribed
        drService.SelectedValue = dbStoreOrderReview.ServicePrompt
        drpStar.SelectedValue = dbStoreOrderReview.NumStars
        lblDateAdded.Text = dbStoreOrderReview.DateAdded
        txtComment.InnerText = dbStoreOrderReview.Comment
        If EditFromEmail Then
            chkPost.Checked = True
            chkIsActive.Checked = True
        Else
            chkIsActive.Checked = dbStoreOrderReview.IsActive
            chkPost.Checked = dbStoreOrderReview.IsShared
        End If
        
        'chkIsFeatured.Checked = dbStoreItemReview.IsFeatured
        Dim tranNO As String = DB.ExecuteScalar("Select TransactionNo From CashPoint Where OrderId = " & OrderId & " and MemberId = " & dbStoreOrderReview.MemberId & " and Left(TransactionNo,2) = 'RO'")
        If CashPointRow.CheckTransactionNoExists(DB, Member.MemberId, tranNO) Then
            ltrCastPointLink.Text = "<a href='/admin/members/addpoint.aspx?transid=" & tranNO & "&MemberId=" & Member.MemberId & "'>" & tranNO & "</a>"
            transExists = True
        Else
            transExists = False
        End If
        ''get Admin Reply
        Dim reply As ReplyReviewRow = ReplyReviewRow.GetRowByReviewId(dbStoreOrderReview.OrderId, CInt(Utility.Common.TypeReview.OrderReview))
        txtAdminReply.Text = reply.Content
    End Sub
    Private Function ProductReviewTranId(ByVal itemid As Integer, ByVal reviewId As Integer) As String
        Dim item As StoreItemRow = StoreItemRow.GetRow(DB, itemid)
        Return "PR" & item.SKU
    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            DB.BeginTransaction()

            Dim dbStoreOrderReview As StoreOrderReviewRow
            dbStoreOrderReview = StoreOrderReviewRow.GetRow(DB, OrderId)

            Dim dbStoreOrderReviewBefore As StoreOrderReviewRow = CloneObject.Clone(dbStoreOrderReview)
            Dim logDetail As New AdminLogDetailRow
            If (dbStoreOrderReview.OrderId < 1) Then
                DB.CommitTransaction()
                Exit Sub
            End If

            dbStoreOrderReview.ItemArrived = drArrived.SelectedValue
            dbStoreOrderReview.ItemDescribed = drDescribled.SelectedValue
            dbStoreOrderReview.ServicePrompt = drService.SelectedValue
            dbStoreOrderReview.NumStars = drpStar.SelectedValue
            dbStoreOrderReview.Comment = txtComment.InnerText 'litComment.Text
            'dbStoreItemReview.IsRecommendFriend = chkIsRecommendFriend.Checked
            dbStoreOrderReview.IsShared = chkPost.Checked
            dbStoreOrderReview.IsActive = chkIsActive.Checked
            'dbStoreItemReview.IsFeatured = chkIsFeatured.Checked


            dbStoreOrderReview.Update()
            logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.OrderReview, dbStoreOrderReviewBefore, dbStoreOrderReview)
            If dbStoreOrderReview.IsActive Then
                Dim o As StoreOrderRow = StoreOrderRow.GetRow(DB, OrderId)
                Dim cp As CashPointRow = New CashPointRow(DB)
                cp = cp.SetValueCashPoint(cp, o, CInt(Session("Admin")), 1)
                If cp.CheckTransactionNoExists(DB, cp.MemberId, cp.TransactionNo) = False Then
                    cp.Insert()
                    logDetail.Message &= "AddPoint|False|True[br]"
                End If
            End If

            DB.CommitTransaction()
            ''insert admin reply
            Dim reply As ReplyReviewRow = ReplyReviewRow.GetRowByReviewId(dbStoreOrderReview.OrderId, CInt(Utility.Common.TypeReview.OrderReview))
            Dim replyBefore As ReplyReviewRow = CloneObject.Clone(reply)
            reply.Content = txtAdminReply.Text.Trim()
            If reply.ReplyReviewId > 0 Then
                ReplyReviewRow.UpdateContent(reply)
            Else
                If txtAdminReply.Text <> String.Empty Then
                    reply.CreateDate = DateTime.Now
                    reply.TypeReply = Utility.Common.TypeReply.AdminReply
                    reply.TypeReview = Utility.Common.TypeReview.OrderReview
                    reply.ReviewId = dbStoreOrderReview.OrderId
                    ReplyReviewRow.Insert(reply)
                End If
            End If
            logDetail.Message &= AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.OrderReview, replyBefore, reply)
            logDetail.ObjectId = dbStoreOrderReview.OrderId
            logDetail.ObjectType = Utility.Common.ObjectType.OrderReview.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)


            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?OrderId=" & OrderId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class


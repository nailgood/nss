Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_store_items_reviews_Edit
    Inherits AdminPage

    Protected ReviewId As Integer
    Protected ItemId As Integer
    Protected EditFromEmail As Boolean = False
    Public transExists As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ReviewId = Convert.ToInt32(Request("ReviewId"))
        If Not Request("ItemId") Is Nothing Then
            ItemId = Convert.ToInt32(Request("ItemId"))
        End If
        EditFromEmail = False
        If Not Request.QueryString("editEmail") Is Nothing Then
            If Request.QueryString("editEmail") = "1" Then
                EditFromEmail = True
            End If
        End If
        If Not IsPostBack Then
            If Not Request.QueryString("editcomment") Is Nothing Then
                If Request.QueryString("editcomment") = "1" Then
                    Response.Redirect("/store/review/product-write.aspx?ReviewId=" & ReviewId & "&ItemId=" & ItemId)
                End If
            End If
            LoadFromDB()
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "InitEditor", "InitEditor();", True)
        End If

    End Sub

    Private Sub LoadFromDB()
        drpItemId.Datasource = StoreItemRow.GetAllStoreItems(DB)
        drpItemId.DataValueField = "ItemId"
        drpItemId.DataTextField = "ItemName"
        drpItemId.Databind()
        drpItemId.Items.Insert(0, New ListItem("", ""))

        If ReviewId = 0 Then
            btnDelete.Visible = False
            Exit Sub
        End If

        Dim dbStoreItemReview As StoreItemReviewRow = StoreItemReviewRow.GetRow(DB, ReviewId)

        Dim Member As MemberRow = MemberRow.GetRow(dbStoreItemReview.MemberId)
        'txtMemberId.Text = dbStoreItemReview.MemberId
        Session("MemberId") = dbStoreItemReview.MemberId
        hplEmail.NavigateUrl = "../../../members/edit.aspx?MemberId=" & dbStoreItemReview.MemberId
        hplEmail.Text = Member.Username
        lblDateAdded.Text = dbStoreItemReview.DateAdded
        drpItemId.SelectedValue = dbStoreItemReview.ItemId
        ItemId = dbStoreItemReview.ItemId

        chkIsActive.Checked = dbStoreItemReview.IsActive
        If Not chkIsActive.Checked Then
            If EditFromEmail Then
                chkIsActive.Checked = True
                txtAdminReply.Focus()
            End If
        End If
        Dim tranNO As String = ProductReviewTranId(ItemId, dbStoreItemReview.ReviewId)
        If CashPointRow.CheckTransactionNoExists(DB, Member.MemberId, tranNO) Then
            ltrCastPointLink.Text = "<a href='/admin/members/addpoint.aspx?transid=" & tranNO & "&MemberId=" & Member.MemberId & "'>" & tranNO & "</a>"
            transExists = True
        Else
            transExists = False
        End If
        ''get Admin Reply
        Dim reply As ReplyReviewRow = ReplyReviewRow.GetRowByReviewId(dbStoreItemReview.ReviewId, CInt(Utility.Common.TypeReview.ProductReview))
        txtAdminReply.Text = reply.Content

    End Sub
    Private Function ProductReviewTranId(ByVal itemid As Integer, ByVal reviewId As Integer) As String
        Dim item As StoreItemRow = StoreItemRow.GetRow(DB, itemid)
        Return "PR" & item.SKU
    End Function
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try

            Dim logDetail As New AdminLogDetailRow
            Dim dbStoreItemReview As StoreItemReviewRow
            Dim dbStoreItemReviewBefore As New StoreItemReviewRow
            dbStoreItemReview = StoreItemReviewRow.GetRow(DB, ReviewId)
            If (dbStoreItemReview.ItemId < 1) Then
                Exit Sub
            End If
            dbStoreItemReviewBefore = CloneObject.Clone(dbStoreItemReview)

            dbStoreItemReview.ItemId = drpItemId.SelectedValue
            dbStoreItemReview.IsActive = chkIsActive.Checked
            dbStoreItemReview.Update()
            logDetail.ObjectId = dbStoreItemReview.ReviewId
            logDetail.ObjectType = Utility.Common.ObjectType.ProductReview.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.ProductReview, dbStoreItemReviewBefore, dbStoreItemReview)
            Dim addPoint As Integer = IIf(dbStoreItemReview.IsFirstReview, Utility.ConfigData.PointFirstReview, Utility.ConfigData.PointOldReview)
            If dbStoreItemReview.IsActive And dbStoreItemReview.CartItemId > 0 Then
                Dim addPointResult As Boolean = CashPointRow.AddPointProductReview(DB, dbStoreItemReview.MemberId, dbStoreItemReview.ItemId, addPoint)
                If (addPointResult) Then
                    logDetail.Message &= "AddPoint|False|True[br]"
                End If
            End If
            ''insert admin reply
            Dim reply As ReplyReviewRow = ReplyReviewRow.GetRowByReviewId(dbStoreItemReview.ReviewId, CInt(Utility.Common.TypeReview.ProductReview))
            Dim replyBefore As ReplyReviewRow = CloneObject.Clone(reply)
            reply.Content = txtAdminReply.Text.Trim()
            If reply.ReplyReviewId > 0 Then
                ReplyReviewRow.UpdateContent(reply)
            Else
                If txtAdminReply.Text <> String.Empty Then
                    reply.CreateDate = DateTime.Now
                    reply.TypeReply = Utility.Common.TypeReply.AdminReply
                    reply.TypeReview = Utility.Common.TypeReview.ProductReview
                    reply.ReviewId = dbStoreItemReview.ReviewId
                    replyBefore.ReviewId = dbStoreItemReview.ReviewId
                    ReplyReviewRow.Insert(reply)
                End If
            End If
            logDetail.Message &= AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.ProductReview, replyBefore, reply)
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException

            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
  
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Response.Redirect("delete.aspx?ReviewId=" & ReviewId & "&" & GetPageParams(FilterFieldType.All))
    End Sub
End Class


Imports System.Web.UI
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer
Partial Class admin_shopdesign_reviews_edit
    Inherits AdminPage

    Public ReviewId As Integer
    Public ParentReviewId As Integer
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ReviewId = Request("ReviewId")
        ParentReviewId = Request("ParentReviewId")
        If Not IsPostBack Then
            LoadDetail()
        End If
    End Sub

    Private Sub LoadDetail()
        Dim review As ReviewRow = ReviewRow.GetRow(DB, ReviewId)
        If Not review Is Nothing Then
            ParentReviewId = review.ParentReviewId
            If ParentReviewId > 0 Then
                lblUserName.Text = "User Reply"
                lblLabelDate.Text = "Date Reply"
                lblComment.Text = "Comment Reply"
                trParentComment.Visible = True
                lblParentComment.Text = review.Comment
                pageTitle.InnerText = "Shop Design Review Reply"
            Else
                lblUserName.Text = "User"
                lblLabelDate.Text = "Date "
                lblComment.Text = "Comment"
                trParentComment.Visible = False
                pageTitle.InnerText = "Shop Design Review"
            End If
            lblDate.Text = review.CreatedDate
            txtComment.InnerText = review.Comment
            chkIsActive.Checked = review.IsActive
            Dim shopdesign As ShopDesignRow = ShopDesignRow.GetRow(DB, review.ItemReviewId)
            Dim member As MemberRow = MemberRow.GetRow(review.MemberId)
            hplShopDesign.Text = shopdesign.Title
            hplShopDesign.NavigateUrl = "/admin/shopdesign/edit.aspx?ShopDesignId=" & shopdesign.ShopDesignId & "&Type=" & Utility.Common.ReviewType.ShopDesign
            hplEmail.Text = member.Username
            hplEmail.NavigateUrl = "/admin/members/edit.aspx?MemberId=" & member.MemberId
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            Dim review As ReviewRow = ReviewRow.GetRow(DB, ReviewId)
            Dim oldreview As ReviewRow = CloneObject.Clone(review)
            review.Comment = txtComment.InnerText
            review.IsActive = chkIsActive.Checked
            ReviewRow.Update(DB, review)

            Dim logdetail As New AdminLogDetailRow
            logdetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.ShopDesignReview, oldreview, review)
            logdetail.ObjectId = review.ItemReviewId
            logdetail.ObjectType = Utility.Common.ObjectType.ShopDesignReview
            logdetail.Action = Utility.Common.AdminLogAction.Update
            AdminLogHelper.WriteLuceneLogDetail(logdetail)
            If review.ParentReviewId > 0 Then
                Response.Redirect("reply.aspx?ReviewId=" & review.ParentReviewId & "&" & GetPageParams(FilterFieldType.All))
            Else
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            End If
        Catch ex As Exception
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim review As ReviewRow = ReviewRow.GetRow(DB, ReviewId)
        ReviewRow.Delete(DB, ReviewId)
        Dim logdetail As New AdminLogDetailRow
        logdetail.Action = Utility.Common.AdminLogAction.Delete
        logdetail.ObjectId = review.ItemReviewId
        logdetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(review, Utility.Common.ObjectType.ShopDesignReview)
        logdetail.ObjectType = Utility.Common.ObjectType.ShopDesignReview
        AdminLogHelper.WriteLuceneLogDetail(logdetail)
        If review.ParentReviewId > 0 Then
            Response.Redirect("reply.aspx?ReviewId=" & review.ParentReviewId & "&" & GetPageParams(FilterFieldType.All))
        Else
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If ParentReviewId > 0 Then
            Response.Redirect("reply.aspx?ReviewId=" & ParentReviewId & "&" & GetPageParams(FilterFieldType.All))
        Else
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        End If
    End Sub
End Class

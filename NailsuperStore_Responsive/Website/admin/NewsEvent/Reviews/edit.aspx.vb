Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Partial Class admin_NewsEvent_Reviews_edit
    Inherits AdminPage

    Public Property ReviewId As Integer
        Get
            If ViewState("ReviewId") Is Nothing Then
                Return 0
            End If
            Return ViewState("ReviewId")
        End Get
        Set(ByVal value As Integer)
            ViewState("ReviewId") = value
        End Set
    End Property
    Public Property ReviewParentId As Integer
        Get
            If ViewState("ReviewParentId") Is Nothing Then
                Return 0
            End If
            Return ViewState("ReviewParentId")
        End Get
        Set(ByVal value As Integer)
            ViewState("ReviewParentId") = value
        End Set
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ReviewId = Convert.ToInt32(Request("ReviewId"))
        ReviewParentId = Request("ParentReviewId")
        If Not IsPostBack Then
            LoadFromDB()
        End If
    End Sub

    Private Sub LoadFromDB()
        Dim review As ReviewRow = ReviewRow.GetRow(DB, ReviewId)
        ReviewParentId = review.ParentReviewId
        If ReviewParentId > 0 Then
            pageTitle.InnerText = "News Review Reply"
            lblComment.Text = "Comment Reply"
            lblLabelDate.Text = "Date Reply"
            lblUserName.Text = "User Reply"
            trParentComment.Visible = True
            Dim parentReview As ReviewRow = ReviewRow.GetRow(DB, ReviewParentId)
            lblParentComment.Text = parentReview.Comment.Replace(Environment.NewLine, "<br/>")
        Else
            pageTitle.InnerText = "News Review"
            lblComment.Text = "Comment"
            lblLabelDate.Text = "Date"
            lblUserName.Text = "User"
            trParentComment.Visible = False
        End If
        Dim member As MemberRow = MemberRow.GetRow(review.MemberId)
        Dim news As NewsRow = NewsRow.GetRow(DB, review.ItemReviewId)
        hplNews.NavigateUrl = "/admin/newsevent/news/default.aspx?NewsId=" & review.ItemReviewId & "&Type=2"
        hplNews.Text = news.Title
        hplEmail.NavigateUrl = "/admin/members/edit.aspx?MemberId=" & review.MemberId
        hplEmail.Text = member.Username
        lblDate.Text = review.CreatedDate
        txtComment.InnerText = review.Comment
        chkIsActive.Checked = review.IsActive
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            Dim review As ReviewRow = ReviewRow.GetRow(DB, ReviewId)
            Dim dbReviewBefore As ReviewRow = CloneObject.Clone(review)
            Dim logDetail As New AdminLogDetailRow
            If (review.ReviewId < 1) Then
                Exit Sub
            End If
            review.Comment = txtComment.InnerText
            review.IsActive = chkIsActive.Checked

            ReviewRow.Update(DB, review)

            Dim logMessage As String = Utility.Common.ObjectToString(review)
            'If review.IsActive Then
            '    CashPointRow.AddPointReview(DB, review.ReviewId)
            'End If

            logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.NewsReview, dbReviewBefore, review)
            logDetail.ObjectId = review.ReviewId
            logDetail.ObjectType = Utility.Common.ObjectType.NewsReview.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            btnCancel_Click(sender, e)

        Catch ex As Exception
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        If ReviewParentId > 0 Then
            Response.Redirect("reply.aspx?reviewId=" & ReviewParentId & "&" & GetPageParams(FilterFieldType.All))
        Else
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        End If
    End Sub
End Class

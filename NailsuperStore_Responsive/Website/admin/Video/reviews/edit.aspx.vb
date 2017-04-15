
Imports System.Web
Imports System.Data.SqlClient
Imports System.Text
Imports System.Data
Imports DataLayer
Imports Components

Public Class admin_Video_reviews_edit
    Inherits AdminPage
    Public Property ReviewId() As Integer
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
    Public Property ReviewParentId() As Integer
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

    Protected ItemId As Integer
    Protected EditFromEmail As Boolean
    Public transExists As Boolean
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ReviewId = Convert.ToInt32(Request("ReviewId"))
        ReviewParentId = Request("ParentReviewId")
        If Not IsPostBack Then
            ReviewParentId = 0
            LoadFromDB()

        End If
    End Sub

    Private Sub LoadFromDB()

        Dim review As ReviewRow = ReviewRow.GetRow(DB, ReviewId)
        ReviewParentId = review.ParentReviewId
        If ReviewParentId > 0 Then
            pageTitle.InnerText = " Video Reviews Reply"
            lblComment.Text = "Comment Reply"
            lblLabelDate.Text = "Date Reply"
            lblUserName.Text = "User Reply"
            trParentComment.Visible = True
            Dim parentReview As ReviewRow = ReviewRow.GetRow(DB, ReviewParentId)
            lblParentComment.Text = parentReview.Comment.Replace(Environment.NewLine, "<br/>")
        Else
            lblUserName.Text = "User "
            lblLabelDate.Text = "Date"
            pageTitle.InnerText = " Video Reviews "
            trParentComment.Visible = False
            lblComment.Text = "Comment"
        End If
        Dim Member As MemberRow = MemberRow.GetRow(review.MemberId)
        Dim video As VideoRow = VideoRow.GetRow(DB, review.ItemReviewId)
        hplEmail.NavigateUrl = "/admin/members/edit.aspx?MemberId=" & review.MemberId
        hplVideo.NavigateUrl = "/admin/Video/Video/edit.aspx?id=" & review.ItemReviewId
        hplEmail.Text = Member.Username
        hplVideo.Text = video.Title
        lblDate.Text = review.CreatedDate
        txtComment.InnerText = review.Comment
        chkIsActive.Checked = review.IsActive

    End Sub
   
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Not IsValid Then Exit Sub
        Try
            Dim dbReview As ReviewRow = ReviewRow.GetRow(DB, ReviewId)
            Dim dbReviewBefore As ReviewRow = CloneObject.Clone(dbReview)
            Dim logDetail As New AdminLogDetailRow
            If (dbReview.ReviewId < 1) Then
                Exit Sub
            End If
            dbReview.Comment = txtComment.InnerText
            dbReview.IsActive = chkIsActive.Checked

            ReviewRow.Update(DB, dbReview)

            Dim logMessage As String = Utility.Common.ObjectToString(dbReview)
            If dbReview.IsActive Then
                CashPointRow.AddPointReview(DB, dbReview.ReviewId)
            End If

            logDetail.Message = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.VideoReview, dbReviewBefore, dbReview)
            logDetail.ObjectId = dbReview.ReviewId
            logDetail.ObjectType = Utility.Common.ObjectType.VideoReview.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            GoBack()
        Catch ex As SqlException

            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        GoBack()
    End Sub
    Private Sub GoBack()
        If ReviewParentId > 0 Then
            Response.Redirect("reply.aspx?reviewid=" & ReviewParentId & "&" & GetPageParams(FilterFieldType.All))
        Else
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        End If
    End Sub
  
End Class


Imports Components
Imports DataLayer
Imports System.Text
Imports System.IO
Partial Class news_access
    Inherits SitePage
    Private changeLog As String = String.Empty
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim ReviewId As Integer = 0
            Dim act As String = String.Empty
            Dim email As String = String.Empty
            If Not Request.QueryString("ReviewId") Is Nothing Then
                ReviewId = Request("ReviewId")
            End If
            If Not Request.QueryString("act") Is Nothing Then
                act = Request("act")
            End If
            If Not Request.QueryString("email") Is Nothing Then
                email = Request("email")
            End If
            If ReviewId < 1 Or (act <> "0" And act <> "1" And act <> "2") Then
                ltrMess.Text = "Review is not valid"
                Exit Sub
            End If
            ltrMess.Text = AccessReview(ReviewId, act, email)
        End If
    End Sub

    Private Function AccessReview(ByVal reviewId As Integer, ByVal act As String, ByVal email As String) As String
        Dim messageResult As String = String.Empty
        Try
            Dim rv As ReviewRow = ReviewRow.GetRow(DB, reviewId)
            Dim rvBefore As ReviewRow = CloneObject.Clone(rv)
            Dim logDetail As New AdminLogDetailRow
            If rv Is Nothing Then
                Return "Review is not valid"
            End If
            If (act = "1") Then ''active this item
                ActiveReview(rv)
                messageResult = "The review is activated."
            ElseIf (act = "0") Then ''Delete this item
                ReviewRow.Delete(DB, reviewId)
                messageResult = "The review is deleted."
            ElseIf (act = "2") Then ''Active and post FB this item
                Dim resultPost As Integer = SocialHelper.PostNewsReviewToFB(DB, rv)
                If (resultPost <> 0) Then
                    messageResult = "The review is activated and posted to Facebook."
                    rv.IsFacebook = True
                    ActiveReview(rv)
                Else
                    messageResult = "Can not post this review to Facebook."
                End If
            ElseIf (act = "3") Then ''Add point for this review
                AddPointReview(rv)
                messageResult = "Addpoint successfull."
            End If

            If (act = "0") Then
                logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
                changeLog = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(rv, Utility.Common.ObjectType.ProductReview)
            Else
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                changeLog &= AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.ProductReview, rvBefore, rv)
            End If
            logDetail.Message = changeLog
            logDetail.LoggedEmail = email
            logDetail.ObjectId = reviewId
            logDetail.ObjectType = Utility.Common.ObjectType.VideoReview.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        Catch ex As Exception
            messageResult = ex.Message.ToString()
        End Try
        Return messageResult
    End Function
    Private Sub ActiveReview(ByVal rv As ReviewRow)
        rv.IsActive = True
        ReviewRow.Update(DB, rv)
        AddPointReview(rv)
    End Sub
    Private Sub AddPointReview(ByVal rv As ReviewRow)
        'Dim resultAddPoint As Boolean = CashPointRow.AddPointReview(DB, rv.ReviewId)
        'If (resultAddPoint) Then
        '    changeLog &= "AddPoint|False|True[br]"
        'End If
    End Sub
End Class

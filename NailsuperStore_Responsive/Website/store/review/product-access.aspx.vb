Imports Components
Imports DataLayer
Imports System.Text
Imports System.IO
Partial Class ReviewProductAccess
    Inherits SitePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            Dim ReviewId As Integer = 0
            Dim act As String = String.Empty
            Dim email As String = String.Empty
            If Request.QueryString("ReviewId") <> Nothing Then
                ReviewId = Request("ReviewId")
            End If
            If Request.QueryString("act") <> Nothing Then
                act = Request("act")
            End If
            If Request.QueryString("email") <> Nothing Then
                email = Request("email")
            End If
            If ReviewId < 0 Or (act <> "0" And act <> "1" And act <> "2" And act <> "3") Then
                ltrMess.Text = "Item review is not valid"
                Exit Sub
            End If
            ltrMess.Text = AccessreviewItem(ReviewId, act, email)
        End If
    End Sub

    Private changeLog As String = String.Empty
    Private Function AccessreviewItem(ByVal reviewId As Integer, ByVal act As String, ByVal email As String) As String
        Dim messageResult As String = String.Empty
        Try
            Dim sir As StoreItemReviewRow = StoreItemReviewRow.GetRow(DB, reviewId)
            Dim sirBefore As StoreItemReviewRow = CloneObject.Clone(sir)
            Dim logDetail As New AdminLogDetailRow
            If sir Is Nothing Then
                Return "Item review is not valid"
            End If
            If (act = "1") Then ''active this item
                ActiveReview(sir)
                messageResult = "The review is activated."
            ElseIf (act = "0") Then ''Delete this item
                StoreItemReviewRow.RemoveRow(DB, reviewId)
                Dim adminPage As New AdminPage
                adminPage.WriteLogDetail("Delete Item Review(" & Me.Request.RawUrl & ")", sir)
                messageResult = "The review is deleted."
            ElseIf (act = "2") Then ''Active and post FB this item
                Dim resultPost As Integer = SocialHelper.PostItemReviewToFB(DB, sir)
                If (resultPost <> 0) Then
                    messageResult = "The review is activated and posted to Facebook."
                    sir.IsFacebook = True
                    ActiveReview(sir)
                Else
                    messageResult = "Can not post this review to Facebook."
                End If
            ElseIf (act = "3") Then ''Add point for this review
                AddPointReview(sir)
                messageResult = "Addpoint successfull."
            End If

            If (act = "0") Then
                logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
                changeLog = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(sir, Utility.Common.ObjectType.ProductReview)
            Else
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                changeLog &= AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.ProductReview, sirBefore, sir)
            End If
            logDetail.Message = changeLog
            logDetail.LoggedEmail = email
            logDetail.ObjectId = reviewId
            logDetail.ObjectType = Utility.Common.ObjectType.ProductReview.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        Catch ex As Exception
            messageResult = ex.Message.ToString()
        End Try
        Return messageResult
    End Function
    Private Sub ActiveReview(ByVal item As StoreItemReviewRow)
        item.IsActive = True
        item.Update()
        Dim addPoint As Integer = IIf(item.IsFirstReview, Utility.ConfigData.PointFirstReview, Utility.ConfigData.PointOldReview)
        Dim resultAddPoint As Boolean = CashPointRow.AddPointProductReview(DB, item.MemberId, item.ItemId, addPoint)
        If (resultAddPoint) Then
            changeLog &= "AddPoint|False|True[br]"
        End If
        Dim adminPage As New AdminPage
    End Sub
    Private Sub AddPointReview(ByVal item As StoreItemReviewRow)
        Dim addPoint As Integer = IIf(item.IsFirstReview, Utility.ConfigData.PointFirstReview, Utility.ConfigData.PointOldReview)
        Dim resultAddPoint As Boolean = CashPointRow.AddPointProductReview(DB, item.MemberId, item.ItemId, addPoint)
        If (resultAddPoint) Then
            changeLog &= "AddPoint|False|True[br]"
        End If
        Dim adminPage As New AdminPage
    End Sub

End Class

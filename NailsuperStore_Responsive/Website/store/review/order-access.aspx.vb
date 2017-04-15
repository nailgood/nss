Imports Components
Imports DataLayer
Imports System.Text
Imports System.IO

Partial Class ReviewOrderAccess
    Inherits SitePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            Dim orderId As Integer = 0
            Dim act As String = String.Empty
            Dim email As String = String.Empty
            If Request.QueryString("OrderId") <> Nothing Then
                orderId = Request("OrderId")
            End If
            If Request.QueryString("act") <> Nothing Then
                act = Request("act")
            End If
            If Request.QueryString("email") <> Nothing Then
                email = Request("email")
            End If
            If orderId < 0 Or (act <> "0" And act <> "1" And act <> "2" And act <> "3") Then
                ltrMess.Text = "Order review is not valid"
                Exit Sub
            End If
            ltrMess.Text = AccessreviewItem(orderId, act, email)
        End If
    End Sub
    Private changeLog As String = String.Empty
    Private Function AccessreviewItem(ByVal orderId As Integer, ByVal act As String, ByVal email As String) As String
        Dim messageResult As String = String.Empty
        Try
            Dim sor As StoreOrderReviewRow = StoreOrderReviewRow.GetRow(DB, orderId)
            Dim sorBefore As StoreOrderReviewRow = CloneObject.Clone(sor)
            Dim logDetail As New AdminLogDetailRow
            If sor.MemberId < 1 Then
                Return "Order review is not valid"
            End If
            If (act = "1") Then ''active this item
                ActiveReview(sor, False)
                messageResult = "The review is activated."
            ElseIf (act = "0") Then ''Delete this item
                StoreOrderReviewRow.RemoveRow(DB, orderId)
                Dim adminPage As New AdminPage
                adminPage.WriteLogDetail("Delete Order Review(" & Me.Request.RawUrl & ")", sor)
                messageResult = "The review is deleted."
            ElseIf (act = "2") Then ''Active and post web live
                ActiveReview(sor, True)
                messageResult = "The review is activated and shared."
            ElseIf (act = "3") Then ''Active and post FB this item
                Dim resultPost As Integer = SocialHelper.PostOrderReviewToFB(DB, sor)
                If resultPost <> 0 Then
                    sor.IsFacebook = True
                    ActiveReview(sor, True)
                End If
                messageResult = "The review is activated and posted to Facebook."
            End If

            If (act = "0") Then
                logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
                changeLog = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(sor, Utility.Common.ObjectType.OrderReview)
            Else
                logDetail.Action = Utility.Common.AdminLogAction.Update.ToString()
                changeLog &= AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.OrderReview, sorBefore, sor)
            End If
            logDetail.Message = changeLog
            logDetail.LoggedEmail = email
            logDetail.ObjectId = orderId
            logDetail.ObjectType = Utility.Common.ObjectType.OrderReview.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
        Catch ex As Exception
            messageResult = ex.Message.ToString()
        End Try
        Return messageResult
    End Function
    Private Sub ActiveReview(ByVal sor As StoreOrderReviewRow, ByVal share As Boolean)
        Try
            Dim logMessage As String = String.Empty
            Dim o As StoreOrderRow = StoreOrderRow.GetRow(DB, sor.OrderId)
            sor.IsActive = True
            If share Then
                sor.IsShared = True
            End If
            sor.Update()
            Dim cp As CashPointRow = New CashPointRow(DB)
            Dim CashPointId As String = DB.ExecuteScalar("Select isnull(CashPointId,0) From CashPoint Where left(TransactionNo,2) = 'RO' and MemberId = " & o.MemberId & " and OrderId = " & o.OrderId)
            If CashPointId = 0 Then
                cp = cp.SetValueCashPoint(cp, o, CInt(Session("AdminId")), 1)
                cp.Insert()
                changeLog &= "AddPoint|False|True[br]"
            End If
            Dim adminPage As New AdminPage
        Catch ex As Exception

        End Try
    End Sub

End Class


Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_Video_reviews_delete
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ReviewId = Convert.ToInt32(Request("ReviewId"))
        Try
            Dim rv As ReviewRow = ReviewRow.GetRow(DB, ReviewId)
            If rv.ParentReviewId > 0 Then
                ReviewParentId = rv.ParentReviewId
            End If
            'Dim logDetail As New AdminLogDetailRow
            'logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(sir, Utility.Common.ObjectType.ProductCoupon)
            ReviewRow.Delete(DB, ReviewId)
            'WriteLogDetail("Delete Item Review", "ReviewId=" & ReviewId)

            'logDetail.ObjectId = ReviewId
            'logDetail.ObjectType = Utility.Common.ObjectType.ProductReview.ToString()
            'logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            'AdminLogHelper.WriteLuceneLogDetail(logDetail)
            If ReviewParentId > 0 Then
                Response.Redirect("reply.aspx?reviewid=" & ReviewParentId & "&" & GetPageParams(FilterFieldType.All))
            Else
                Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
            End If

        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class


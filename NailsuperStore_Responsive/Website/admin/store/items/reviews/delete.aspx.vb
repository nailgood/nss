Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_store_items_reviews_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim ReviewId As Integer = Convert.ToInt32(Request("ReviewId"))
        Try
            Dim sir As StoreItemReviewRow = StoreItemReviewRow.GetRow(DB, ReviewId)
            Dim logDetail As New AdminLogDetailRow
            logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(sir, Utility.Common.ObjectType.ProductCoupon)
            StoreItemReviewRow.RemoveRow(DB, ReviewId)

            logDetail.ObjectId = ReviewId
            logDetail.ObjectType = Utility.Common.ObjectType.ProductReview.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class


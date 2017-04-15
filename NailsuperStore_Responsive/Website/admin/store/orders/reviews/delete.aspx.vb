Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_store_orders_reviews_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim OrderId As Integer = Convert.ToInt32(Request("Orderid"))
        Try
            Dim sor As StoreOrderReviewRow = StoreOrderReviewRow.GetRow(DB, OrderId)
            Dim logDetail As New AdminLogDetailRow
            logDetail.Message = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(sor, Utility.Common.ObjectType.OrderReview)
            StoreOrderReviewRow.RemoveRow(DB, OrderId)

            logDetail.ObjectId = OrderId
            logDetail.ObjectType = Utility.Common.ObjectType.OrderReview.ToString()
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString()
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException

            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class


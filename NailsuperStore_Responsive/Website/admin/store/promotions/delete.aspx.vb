Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer
Imports Controls
Partial Class delete
    Inherits AdminPage
    Dim f As New FileUpload
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim PromotionId As Integer = Convert.ToInt32(Request("PromotionId"))
        Dim pro As StorePromotionRow

        Try
            DB.BeginTransaction()
            pro = StorePromotionRow.GetRow(DB, PromotionId)
            StorePromotionRow.RemoveRow(DB, PromotionId)
            DB.ExecuteSQL("Delete from StorePromotionItem where PromotionId=" & PromotionId)
            f.RemoveFileName(ConfigurationManager.AppSettings("PathCoupon"), pro.Image)
            DB.CommitTransaction()
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = PromotionId
            logDetail.ObjectType = Utility.Common.ObjectType.OrderCoupon.ToString()
            Dim changeLog As String = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(pro, Utility.Common.ObjectType.OrderCoupon)
            logDetail.Message = changeLog
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString
            AdminLogHelper.WriteLuceneLogDetail(logDetail)
            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

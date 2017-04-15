Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer
Imports Controls
Partial Class admin_store_freegift_delete
    Inherits AdminPage
    Dim f As New FileUpload
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim FreeGiftId As Integer = Convert.ToInt32(Request("FreeGiftId"))
        Try
            DB.BeginTransaction()
            Dim FG As FreeGiftRow = FreeGiftRow.GetRow(DB, FreeGiftId)
            Dim si As StoreItemRow = StoreItemRow.GetRow(DB, FG.ItemId)
            si.CheckFreeGiftItem(Nothing, FG, Utility.Common.AdminLogAction.Delete.ToString())
            FreeGiftRow.Delete(FreeGiftId)
            si.IsFreeGift = 0
            DB.ExecuteScalar("Update StoreItem set IsFreeGift=" & si.IsFreeGift & " where ItemId=" & si.ItemId)
            StoreItemRowBase.ClearItemCache(si.ItemId)

            ' DB.ExecuteSQL("delete StoreCartItem where CartItemId  in (select  c.CartItemId from StoreOrder o inner join StoreCartItem c on o.OrderId = c.OrderId where o.OrderNo is null and o.SellToCustomerId = 0 and ItemId = " & FG.ItemId & " and IsFreeItem = 1 and freeitemids is null )")
            f.RemoveFileName(ConfigurationManager.AppSettings("PathFreeGift"), FG.Banner)
            f.RemoveFileName(ConfigurationManager.AppSettings("PathFreeGift"), FG.Image)
            DB.CommitTransaction()
            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = FreeGiftId
            logDetail.ObjectType = Utility.Common.ObjectType.FreeGift.ToString()
            Dim changeLog As String = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(FG, Utility.Common.ObjectType.FreeGift)
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


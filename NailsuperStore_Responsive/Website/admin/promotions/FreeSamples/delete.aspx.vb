Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class delete
	Inherits AdminPage

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim ItemId As Integer = Convert.ToInt32(Request("ItemId"))
        Dim si As StoreItemRow = StoreItemRow.GetRow(DB, ItemId)
        Dim siold As StoreItemRow = CloneObject.Clone(si)
		Try
            DB.BeginTransaction()
            si.IsFreeSample = False
            si.IPNUpdateFreeSamples()
            DB.ExecuteSQL("delete StoreCartItem where CartItemId  in (select  c.CartItemId from StoreOrder o inner join StoreCartItem c on o.OrderId = c.OrderId where o.OrderNo is null and o.SellToCustomerId = 0 and ItemId = " & ItemId & " and IsFreeSample = 1 )")
			DB.CommitTransaction()

            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = ItemId
            logDetail.ObjectType = Utility.Common.ObjectType.FreeSample.ToString()
            Dim changeLog As String = AdminLogHelper.ConvertObjectUpdateToLogMesssageString(DB, Utility.Common.ObjectType.FreeSample, siold, si)
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


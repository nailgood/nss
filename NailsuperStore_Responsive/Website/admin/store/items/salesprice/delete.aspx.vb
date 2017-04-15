Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_store_items_salesprice_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        

        Try
            Dim SalesPriceId As Integer = Convert.ToInt32(Request("SalesPriceId"))
            Dim SalesType As Integer = Convert.ToInt32(Request("salestype"))
            Dim dbSalesPrice As SalesPriceRow = SalesPriceRow.GetRowById(DB, SalesPriceId)
            Dim ItemId As Integer = dbSalesPrice.ItemId

            DB.BeginTransaction()
            SalesPriceRow.RemoveRow(DB, SalesPriceId)
            DB.CommitTransaction()

            Dim logDetail As New AdminLogDetailRow
            logDetail.ObjectId = SalesPriceId
            logDetail.ObjectType = Utility.Common.ObjectType.SalesPrice.ToString()
            Dim changeLog As String = AdminLogHelper.ConvertObjectDeleteToLogMesssageString(dbSalesPrice, Utility.Common.ObjectType.StoreItem)
            logDetail.Message = changeLog
            logDetail.Action = Utility.Common.AdminLogAction.Delete.ToString
            AdminLogHelper.WriteLuceneLogDetail(logDetail)

            Response.Redirect("default.aspx?salestype=" & SalesType & "&ItemId=" & dbSalesPrice.ItemId & "&" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

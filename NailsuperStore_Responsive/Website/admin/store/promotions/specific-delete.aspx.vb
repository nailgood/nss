Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class SpecificDelete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim Id As Integer = Convert.ToInt32(Request("Id"))
        Dim PromotionId As Integer = Convert.ToInt32(Request("PromotionId"))
        Try
            DB.BeginTransaction()
            Dim dbPromotion As StorePromotionRow = StorePromotionRow.GetRow(DB, PromotionId)
            dbPromotion.RemoveRelatedItem(Id)
            DB.CommitTransaction()
            Response.Redirect("specific-items.aspx?PromotionId=" & PromotionId & "&" & GetPageParams(FilterFieldType.All))
        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class


Imports Components
Imports System.Data
Imports System.Data.SqlClient
Imports DataLayer

Partial Class admin_store_ordersremove_delete
    Inherits AdminPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        Dim OrderId As Integer = Convert.ToInt32(Request("OrderId"))

        Dim iCount As Integer = 0
        Dim dbStoreCartItem As StoreCartItemCollection
        Dim dbStoreItem As StoreItemRow
        Dim iQuatity As Integer = 0
        Dim iItemId As Integer = 0
        Try
            DB.BeginTransaction()
            ''''''''''update quantity on storeitem

            dbStoreCartItem = StoreCartItemRow.GetCartItems(DB, OrderId)
            For iCount = 0 To dbStoreCartItem.Count - 1
                iQuatity = dbStoreCartItem.Item(iCount).Quantity
                iItemId = dbStoreCartItem.Item(iCount).ItemId

                dbStoreItem = StoreItemRow.GetRow(DB, iItemId)
                dbStoreItem.QtyOnHand = dbStoreItem.QtyOnHand + iQuatity
                dbStoreItem.Update()

                'SQL = "update from Storeitem set QtyOnHand=QtyOnHand+" & DB.Quote(iQuatity.ToString()) & " where ItemId = " & DB.Quote(iItemId)
                'DB.ExecuteSQL(SQL)
            Next
            CashPointRow.RemoveByOrderId(DB, OrderId)
            StoreOrderCommentRow.RemoveByOrderId(DB, OrderId)
            StoreOrderPayflowRow.RemoveByOrderId(DB, OrderId)
            StoreCartItemRow.RemoveByOrderId(DB, OrderId)
            StoreOrderRow.RemoveRow(DB, OrderId)
            DB.CommitTransaction()

            Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))

        Catch ex As SqlException
            If Not DB Is Nothing AndAlso Not DB.Transaction Is Nothing Then DB.RollbackTransaction()
            AddError(ErrHandler.ErrorText(ex))
        End Try
    End Sub
End Class

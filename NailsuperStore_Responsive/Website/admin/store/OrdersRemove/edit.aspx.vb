Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient
Imports Components
Imports Controls
Imports System.Data
Imports DataLayer

Partial Class admin_store_ordersremove_order_view
    Inherits AdminPage

    Protected params As String
    Protected OrderId As Integer
    Protected c As ShoppingCart
    Protected o As StoreOrderRow
    Protected LiftGateService As Double = SysParam.GetValue("LiftGateService")
    Protected InsideDeliveryService As Double = SysParam.GetValue("InsideDeliveryService")

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load


        OrderId = CType(Request("OrderId"), Integer)
        c = New ShoppingCart(DB, OrderId, True)
        litOrderNo.Text = c.Order.OrderNo
        dtl.Cart = c
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("default.aspx?" & GetPageParams(FilterFieldType.All))
    End Sub
End Class

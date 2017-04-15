
Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Public Class store_ViewOrder
    Inherits SitePage

    Protected OrderId As Integer

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        Exit Sub

        If GetQueryString("OrderId") <> Nothing Then
            OrderId = DB.ExecuteScalar("SELECT OrderId FROM StoreOrder WHERE processdate is not null and OrderId=" & Convert.ToInt32(GetQueryString("OrderId")))
        End If

        If OrderId = 0 Then
            Exit Sub
        End If

        Dim c As ShoppingCart = New ShoppingCart(DB, OrderId, True)
        dtl.Cart = c
        ltl.Text = DB.ExecuteScalar("SELECT top 1 Name FROM StoreOrderStatus sos where sos.Code = " & DB.Quote(c.Order.Status))
        Dim Shipment As StoreOrderShipmentRow = StoreOrderShipmentRow.GetRow(DB, CInt(DB.ExecuteScalar("select top 1 shipmentid from storeordershipment where orderid = " & OrderId)))
        If Not Shipment.ShipmentId = Nothing AndAlso Not DB.ExecuteScalar("select top 1 shipmentlineid from storeordershipmentline where shipmentid = " & Shipment.ShipmentId) = Nothing Then
            If Shipment.CompletelyShipped Then
                ltl.Text = "Shipped"
            Else
                ltl.Text = "Partially Shipped"
            End If
        End If

        ltlPageTitle.Text = "Order Details"

        Dim dt As DataTable = Shipment.GetTrackingNumbers()
        If dt.Rows.Count > 0 Then
            ltl2.Text &= "<br />Tracking No: <span class=""mag"">"
            For Each row As DataRow In dt.Rows
                ltl2.Text &= Trim(row("TrackingNo")) & "<br />"
            Next
            ltl2.Text &= "</span>"
        Else
            ltl2.Text &= "&nbsp;"
        End If
    End Sub
End Class
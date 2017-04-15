Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Public Class Member_orderhistory_view
    Inherits SitePage

	Protected OrderId As Integer

	Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadData()
    End Sub
    Private Sub LoadData()
        Dim isViewAdmin As Boolean = Utility.Common.IsViewFromAdmin
        If isViewAdmin Then
            OrderId = GetQueryString("OrderId")
            divDetail.Visible = False
            ulStatus.Visible = False
        Else
            If Not HasAccess() AndAlso (GetQueryString("e") = Nothing OrElse GetQueryString("orderno") = Nothing) Then
                Response.Redirect("/members/login.aspx")
            End If
            Dim dbMember As New MemberRow
            If Session("memberId") <> Nothing Then dbMember = MemberRow.GetRow(Session("memberId"))

            If GetQueryString("OrderId") <> Nothing AndAlso Not dbMember Is Nothing Then
                OrderId = DB.ExecuteScalar("SELECT OrderId FROM StoreOrder WHERE processdate is not null and MemberId=" & dbMember.MemberId & " AND OrderId=" & Convert.ToInt32(GetQueryString("OrderId")))
            ElseIf GetQueryString("e") <> Nothing AndAlso GetQueryString("orderno") <> Nothing Then
                OrderId = DB.ExecuteScalar("select orderid from storeorder where processdate is not null and email = " & DB.Quote(Utility.Crypt.DecryptTripleDes(GetQueryString("e"))) & " and orderno = " & DB.Quote(Utility.Crypt.DecryptTripleDes(GetQueryString("OrderNo"))) & IIf(dbMember Is Nothing, "", " and memberid = " & dbMember.MemberId))
            End If
            If OrderId = 0 Then
                Response.Redirect("/members/orderhistory/")
            End If

        End If
      
        Dim objCart As ShoppingCart = New ShoppingCart(DB, OrderId, True)
        ucOrderDetail.Cart = objCart
        Dim ucCartSummary As controls_checkout_cart_summary = Me.Master.FindControl("ucCartSummary")
        If Not ucCartSummary Is Nothing Then
            ucCartSummary.Cart = objCart
        End If
        Dim ucPayment As controls_checkout_payment_infor = Me.Master.FindControl("ucPayment")
        If Not ucPayment Is Nothing Then
            ucPayment.Cart = objCart
        End If
        If Not isViewAdmin Then

            spStatus.InnerText = DB.ExecuteScalar("SELECT top 1 Name FROM StoreOrderStatus sos where sos.Code = " & DB.Quote(objCart.Order.Status))
            Dim Shipment As StoreOrderShipmentRow = StoreOrderShipmentRow.GetRow(DB, CInt(DB.ExecuteScalar("select top 1 shipmentid from storeordershipment where orderid = " & OrderId)))
            If Not Shipment.ShipmentId = Nothing AndAlso Not DB.ExecuteScalar("select top 1 shipmentlineid from storeordershipmentline where shipmentid = " & Shipment.ShipmentId) = Nothing Then
                If Shipment.CompletelyShipped Then
                    spStatus.InnerText = "Shipped"
                Else
                    spStatus.InnerText = "Partially Shipped"
                End If
            End If
            Dim dt As DataTable = DB.GetDataTable("SELECT TrackingNo,Note,coalesce(ShipmentType,0) as ShipmentType  FROM StoreOrderShipmentTracking WHERE ShipmentId = " & Shipment.ShipmentId & " or OrderId=" & objCart.Order.OrderId)

            If dt.Rows.Count > 0 Then
                Dim ShipmentType As Integer = 0

                For Each row As DataRow In dt.Rows
                    ShipmentType = CInt(dt.Rows(0)("ShipmentType"))
                    If (ShipmentType = Utility.Common.StandardShippingMethod.Truck) Then
                        spTrackingNo.InnerHtml &= Trim(row("Note")) & "<br />"
                    Else
                        Dim trackingNumber As String = Trim(row("TrackingNo"))
                        Try
                            If trackingNumber.Contains("/") Then

                                Dim lstTracking As String() = trackingNumber.Split("/")
                                For i As Integer = 0 To lstTracking.Length - 1
                                    spTrackingNo.InnerHtml &= lstTracking(i) & "<br>"
                                Next
                            Else
                                spTrackingNo.InnerHtml = trackingNumber
                            End If
                        Catch

                        End Try
                        'spTrackingNo.InnerHtml &= Trim(row("TrackingNo")) & "<br />"
                    End If

                Next

            Else
                liTrackingNo.Visible = False
            End If
        End If
        LoadMetaData(DB, "/members/orderhistory/view.aspx")
    End Sub
End Class
Imports Components
Imports DataLayer
Imports System.Data.SqlClient

Public Class Member_OrderHistory_Default
    Inherits SitePage

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not HasAccess() Then
            DB.Close()
            Response.Redirect("/members/login.aspx")
        End If
        Dim dbMember As MemberRow = MemberRow.GetRow(Session("memberId"))

        'ltlPageTitle.Text = "My Order History"
        'ltlMemberNavigation.Text = MemberRow.MemberNavigationString

        rptOrderHistory.DataSource = dbMember.GetMemberOrderHistory()
        rptOrderHistory.DataBind()
    End Sub

    Protected Sub rptOrderHistory_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptOrderHistory.ItemCommand
        If e.CommandName = "Details" Then
            DB.Close()
            Response.Redirect("/members/orderhistory/view.aspx?OrderId=" & e.CommandArgument)
        End If
    End Sub

    Protected Sub rptOrderHistory_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptOrderHistory.ItemDataBound
        If e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item Then
            ''  Dim CarrierType As Integer = e.Item.DataItem("CarrierType")
            Dim btnDetails As LinkButton = CType(e.Item.FindControl("btnDetails"), LinkButton)
            Dim ltlOrderNo As Literal = CType(e.Item.FindControl("ltlOrderNo"), Literal)
            Dim ltlBillingName As Literal = CType(e.Item.FindControl("ltlBillingName"), Literal)
            Dim ltlTotal As Literal = CType(e.Item.FindControl("ltlTotal"), Literal)
            Dim ltlStatus As Literal = CType(e.Item.FindControl("ltlStatus"), Literal)
            Dim ltlPurchaseDate As Literal = CType(e.Item.FindControl("ltlPurchaseDate"), Literal)
            Dim ltlTrackingNo As Literal = e.Item.FindControl("ltlTrackingNo")
            Dim ltrIconShipping As Literal = e.Item.FindControl("ltrIconShipping")
            Dim orderId As Integer = e.Item.DataItem("OrderId")
            btnDetails.CommandArgument = e.Item.DataItem("OrderId")
            Dim CarrierType As Integer = DB.ExecuteScalar("select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = " & orderId & " and  type = 'item' and LEFT(Code,3)='UPS'")
            If CarrierType < 1 Then
                CarrierType = DB.ExecuteScalar("select top 1  carriertype from storecartitem item left join ShipmentMethod sp on(sp.MethodId=item.CarrierType) where orderid = " & orderId & " and  type = 'item'")
            End If
            If CarrierType < 1 Then
                CarrierType = e.Item.DataItem("CarrierType")
            End If
            Dim url As String = "<a href=""/members/orderhistory/view.aspx?OrderId={1}"">{0}</a>"
            If Not IsDBNull(e.Item.DataItem("OrderNo")) Then
                ltlOrderNo.Text = String.Format(url, e.Item.DataItem("OrderNo"), e.Item.DataItem("OrderId"))
            ElseIf Not IsDBNull(e.Item.DataItem("NavisionOrderNo")) Then
                ltlOrderNo.Text = String.Format(url, e.Item.DataItem("NavisionOrderNo"), e.Item.DataItem("OrderId"))
            End If

            If Not IsDBNull(e.Item.DataItem("BillingName")) Then ltlBillingName.Text = e.Item.DataItem("BillingName")
            If Not IsDBNull(e.Item.DataItem("Total")) Then ltlTotal.Text = FormatCurrency(e.Item.DataItem("Total"))
            If Not IsDBNull(e.Item.DataItem("Status")) Then ltlStatus.Text = e.Item.DataItem("Status")
            'Dim Shipment As StoreOrderShipmentRow = StoreOrderShipmentRow.GetRow(DB, CInt(DB.ExecuteScalar("select top 1 coalesce(shipmentid,0) from storeordershipment where orderid = " & e.Item.DataItem("OrderId") & " order by case when completelyshipped = 1 then 0 else 1 end")))
            'If Not Shipment.ShipmentId = Nothing AndAlso Not DB.ExecuteScalar("select top 1 shipmentlineid from storeordershipmentline where shipmentid = " & Shipment.ShipmentId) = Nothing Then
            '    If Shipment.CompletelyShipped Then
            '        ltlStatus.Text = "Shipped"
            '    Else
            '        ltlStatus.Text = "Partially Shipped"
            '    End If
            'End If
            
            If Not IsDBNull(e.Item.DataItem("ProcessDate")) Then ltlPurchaseDate.Text = e.Item.DataItem("ProcessDate")

            Dim dt As DataTable = DB.GetDataTable("select sost.trackingno,sost.note from TrackingInfo tinfo inner join StoreOrderShipmentTracking sost on tinfo.TrackingNo = sost.TrackingNo where orderid = " & e.Item.DataItem("orderid"))
            If dt.Rows.Count = 0 Then
                dt = DB.GetDataTable("select trackingno,note from StoreOrderShipmentTracking where shipmentid in (select shipmentid from storeordershipment where orderid = " & e.Item.DataItem("orderid") & ")")
            End If

            Dim ShipmentType As Integer = DB.ExecuteScalar("Select coalesce(ShipmentType,0) from StoreOrderShipmentTracking where OrderId=" & e.Item.DataItem("orderid"))
            Dim smt As ShipmentMethodRow = ShipmentMethodRow.GetRow(DB, CarrierType)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                If LCase(Left(smt.Code, 3)) = "ups" Or LCase(Left(smt.Code, 3)) = "fed" Or CarrierType = 15 Then
                    If (ShipmentType = Utility.Common.StandardShippingMethod.Truck) Then
                        ltlTrackingNo.Text &= "<a target='_blank' style='text-decoration:none;color:#0000ed'  href='" & Trim(dt.Rows(0)("TrackingNo")) & "'>" & Trim(dt.Rows(0)("Note")) & "</a>"
                    Else
                        Dim linkTracking As String = String.Empty
                        Dim trackingNumber As String = Trim(dt.Rows(0)("TrackingNo"))
                        If (ShipmentType = Utility.Common.StandardShippingMethod.USPS) Then
                            linkTracking = Resources.Msg.LinkTrackingNumberUSPS
                        ElseIf (ShipmentType = Utility.Common.StandardShippingMethod.FedEx) Then
                            linkTracking = Resources.Msg.LinkTrackingOnWeb 'Resources.Msg.LinkTrackingNumberFedEx
                        ElseIf (ShipmentType = Utility.Common.StandardShippingMethod.UPS) Then
                            linkTracking = Resources.Msg.LinkTrackingNumberUPS
                        ElseIf CarrierType = 15 Then
                            linkTracking = Resources.Msg.LinkTrackingNumberUSPSPriority
                        End If
                        Try
                            If trackingNumber.Contains("/") Then
                                Dim strTracking As String = ""
                                Dim lstTracking As String() = trackingNumber.Split("/")
                                For i As Integer = 0 To lstTracking.Length - 1
                                    strTracking &= "<a href='" & String.Format(linkTracking, Trim(lstTracking(i))) & "' target='_blank' >" & lstTracking(i) & "</a><br>"
                                Next
                                ltlTrackingNo.Text = strTracking
                            Else
                                linkTracking = String.Format(linkTracking, trackingNumber)
                                ltlTrackingNo.Text = "<a href='" & linkTracking & "' target='_blank' >" & trackingNumber & "</a>"
                            End If
                        Catch

                        End Try

                        'linkTracking = String.Format(linkTracking, trackingNumber)
                        'ltlTrackingNo.Text &= "<a href='" & linkTracking & "' target='_blank' style='text-decoration:none; color:#0000ed'>" & trackingNumber & "</a><br />"
                    End If

                ElseIf CarrierType = Utility.Common.StandardShippingMethod.Truck Then
                    ltlTrackingNo.Text &= "<a target='_blank' style='text-decoration:none;color:#0000ed'  href='" & Trim(dt.Rows(0)("TrackingNo")) & "'>" & Trim(dt.Rows(0)("Note")) & "</a>"
                    '' ltlTrackingNo.Text &= "<a href='" & String.Format(Resources.Msg.LinkTrackingNumberUSPSPriority, Trim(dt.Rows(0)("TrackingNo"))) & "' target='_blank' style='text-decoration:none'>" & Trim(dt.Rows(0)("TrackingNo")) & "</a><br />"
                End If
                ltrIconShipping.Text = "<img src=""/includes/theme/images/" & smt.Image & """/>"
            Else
                ltlTrackingNo.Text &= "&nbsp;"
            End If

        End If
    End Sub
End Class
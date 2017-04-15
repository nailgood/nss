Imports Components
Imports DataLayer
Imports System.Net
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices
Imports System.Data.Common
Imports ShippingValidator

Partial Class members_orderhistory_tracking
    Inherits SitePage
    Private Message As String
    Public trackDetail As FedExGetTracking.TrackDetail
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim TrackingId As String = Request("id")
        Try
            lbviewfedex.Text = "<a href='" & String.Format(Resources.Msg.LinkTrackingNumberFedEx, TrackingId) & "' target='_blank'>view on Fedex.com</a>"
            lbNumber.Text = TrackingId
            Dim tr As StoreOrderShipmentTrackingRow = StoreOrderShipmentTrackingRow.GetRowFromTrackingNo(DB, TrackingId)
            Dim ShipingType As String
            Dim o As StoreOrderRow = StoreOrderRow.GetRow(DB, tr.OrderId)
            Dim ShipBeginDate As String = o.ProcessDate.ToString("MM/dd/yyyy")
            If tr.ServiceType = Nothing Then
                ShipingType = NavisionCodes.ShippingTypeFedex(o.CarrierType) 'DB.ExecuteScalar("select Name from ShipmentMethod sm left join StoreOrder so on sm.MethodId = so.CarrierType where OrderId = " & tr.OrderId)
            Else
                ShipingType = tr.ServiceType
            End If
            'ShipingType = "fedex ground"

            If CheckTrackingNo(TrackingId) = False Then
                ordertracking.Visible = False
                dNorecord.Visible = True
                lbmsgError.Text = String.Format(Resources.Alert.TrackingNorecord, "<a href=""/contact/GeneralQuestion.aspx"">customer service</a>")
            Else
                Dim trinfo As TrackingInfoRow = TrackingInfoRow.GetRow(DB, TrackingId)
                If trinfo.TravelHistory.Contains("/App_Themes/Default/images") Then
                    trinfo.TravelHistory = trinfo.TravelHistory.Replace("/App_Themes/Default/images", "/includes/theme/images")
                End If
                ' If trinfo.TrackingNo <> Nothing Then
                If trinfo.ActualDeliveryTimestam.ToString.Contains("0001") = False Then
                    ShowTrackDetail(trinfo)

                Else
                    Dim getTracking As New FedexTracking(TrackingId, ShipBeginDate, ShipingType)
                    If getTracking.msgError = "ERROR" Then
                        dNorecord.Visible = True
                        ordertracking.Visible = False
                        lbmsgError.Text = String.Format(Resources.Alert.TrackingServiceError, "<a href='" & String.Format(Resources.Msg.LinkTrackingNumberFedEx, TrackingId) & "' target='_blank'>FedEx.com</a>", "<a href=""/contact/GeneralQuestion.aspx"">customer service</a>")
                    Else
                        ShowTrackReply(getTracking.replyTracking)
                    End If

                End If
            End If

        Catch ex As Exception
            Email.SendError("ToError500", "Order-Tracking page", "TrackingNo:" & TrackingId & "<br>" & ex.ToString)
        End Try

    End Sub
    Private Function CheckTrackingNo(ByVal TrackingNo As String) As Boolean
        Dim countTrackingNo As Integer = DB.ExecuteScalar("Select COUNT(*) from StoreOrderShipmentTracking where TrackingNo = '" & TrackingNo & "'")
        If countTrackingNo > 0 Then
            Return True
        End If
        Return False
    End Function
    Sub ShowTrackReply(ByRef reply As FedExGetTracking.TrackReply)

        Dim Address As String
        If (Not reply.TrackDetails Is Nothing) Then
            For Each trackDetail In reply.TrackDetails
                Dim contentDay As String = ""
                Dim d As String = ""

                If (trackDetail.Events IsNot Nothing) Then


                    lthistory.Text &= "<table class=""tb-track"" border=""0"" cellpadding=""0"" cellspacing=""0"">" _
                    & "<tr><td style=""font-weight:bold"">Date/Time</td><td style=""font-weight:bold"">Activity</td><td style=""font-weight:bold"">Location</td></tr>"
                    Dim i As Integer = 0

                    For Each trackevent As FedexGetTracking.TrackEvent In trackDetail.Events
                        If (trackevent.TimestampSpecified) Then
                            d = trackevent.Timestamp.Day
                            If contentDay.ToString.Contains(d) = False Then
                                contentDay &= d & "-"
                                lthistory.Text &= "<tr class=""header-time""><td border=""0"">&nbsp;&nbsp;<img src=""/includes/theme/images/icon_bullet2.gif"" />&nbsp;" & trackevent.Timestamp.ToString("MM/dd/yyyy") & "</td><td border=""0"">" & trackevent.Timestamp.ToString("dddd") & "</td><td border=""0""></td></tr>" _
                                & "<tr><td class=""list-info"">" & trackevent.Timestamp.ToString("hh.mm tt") & "</td><td>" & trackevent.EventDescription & "</td><td>" & trackevent.Address.City & " " & trackevent.Address.StateOrProvinceCode & "</td></tr>"
                            Else
                                lthistory.Text &= "<tr><td class=""list-info"">" & trackevent.Timestamp.ToString("hh.mm tt") & "</td><td>" & trackevent.EventDescription & "</td><td>" & trackevent.Address.City & " " & trackevent.Address.StateOrProvinceCode & "</td></tr>"
                            End If
                            If i = 0 Then
                                Address = trackevent.Address.City & " " & trackevent.Address.StateOrProvinceCode
                            End If
                            i = i + 1
                        End If
                    Next
                    lthistory.Text &= "</table>"
                End If
                lbNumber.Text = trackDetail.TrackingNumber
                lbNumer2.Text = trackDetail.TrackingNumber
                lbShipTimestamp.Text = trackDetail.ShipTimestamp.ToString("ddd MM/dd/yyyy")
                If (trackDetail.OtherIdentifiers IsNot Nothing) Then
                    For Each identifier As FedexGetTracking.TrackPackageIdentifier In trackDetail.OtherIdentifiers
                        ' Message &= "Other Identifier: " & identifier.Type & " " & identifier.Value
                        If identifier.Type = FedexGetTracking.TrackIdentifierType.CUSTOMER_REFERENCE Then
                            lbReference.Text = identifier.Value
                        End If
                        If identifier.Type = FedexGetTracking.TrackIdentifierType.INVOICE Then
                            lbInvoicenumber.Text = identifier.Value
                        End If
                        If identifier.Type = FedexGetTracking.TrackIdentifierType.PURCHASE_ORDER Or lbInvoicenumber.Text = Nothing Then
                            lbordernumber.Text = identifier.Value
                        End If
                        If identifier.Type = FedexGetTracking.TrackIdentifierType.GROUND_SHIPMENT_ID Then
                            lbShipmentID.Text = identifier.Value
                        End If
                    Next
                End If

                If (trackDetail.ServiceInfo IsNot Nothing) Then
                    lbService.Text = trackDetail.ServiceInfo
                End If

                If (trackDetail.PackageWeight IsNot Nothing) Then
                    lbWeight.Text = trackDetail.PackageWeight.Value & " lbs" '& trackDetail.PackageWeight.Units
                End If
                lbTotalpieces.Text = trackDetail.PackageCount
                If (trackDetail.ShipmentWeight IsNot Nothing) Then
                End If
                If (trackDetail.Packaging IsNot Nothing) Then
                    lbPackaging.Text = trackDetail.Packaging
                End If
                'If (trackDetail.ShipTimestampSpecified) Then
                '    'Message &= "note 7--Ship timestamp: " & trackDetail.ShipTimestamp
                'End If

                If (trackDetail.DestinationAddress IsNot Nothing) Then
                    lbActualAddress.Text = trackDetail.DestinationAddress.City & ", " & trackDetail.DestinationAddress.StateOrProvinceCode & " " & trackDetail.DestinationAddress.CountryCode
                    lbActualDeliveryTimestamp.Text = IIf(trackDetail.EstimatedDeliveryTimestamp.ToString.Contains("0001"), "N/A", trackDetail.EstimatedDeliveryTimestamp.ToString("ddd MM/dd/yyyy hh:mm tt"))
                End If

                If (trackDetail.ActualDeliveryTimestampSpecified) Then
                    lbActualAddress.Text = trackDetail.ActualDeliveryAddress.City & ", " & trackDetail.ActualDeliveryAddress.StateOrProvinceCode & " " & trackDetail.ActualDeliveryAddress.CountryCode
                    lbActualDeliveryTimestamp.Text = IIf(trackDetail.ActualDeliveryTimestamp.ToString.Contains("0001"), "N/A", trackDetail.ActualDeliveryTimestamp.ToString("ddd MM/dd/yyyy hh:mm tt"))
                End If
                If Not String.IsNullOrEmpty(lbActualDeliveryTimestamp.Text) Then
                    lbActualDeliveryTimestamp.Text = ReturnMessage(lbActualDeliveryTimestamp.Text)
                End If

                'If (trackDetail.SignatureProofOfDeliveryAvailableSpecified) Then
                '    'Message &= "note 10--SPOD availability: " & trackDetail.SignatureProofOfDeliveryAvailable
                'End If

                'If (trackDetail.NotificationEventsAvailable IsNot Nothing) Then
                '    For Each notificationEventType As FedexGetTracking.EMailNotificationEventType In trackDetail.NotificationEventsAvailable
                '        ' Message &= "note 11--EmailNotificationEvent type : " & notificationEventType
                '    Next
                'End If
                Dim statusImg As String = ""
                statusImg = "<div><img src='" & WebRoot & "/includes/theme/images/" & ReturnImgStatus(Trim(trackDetail.StatusCode), LCase(trackDetail.StatusDescription)) & "'></div>"
                If trackDetail.StatusDescription.Contains("Delivered") Then
                    ltStatus.Text = statusImg _
                    & "<div class=""ship-status delivered"">" & trackDetail.StatusDescription & "</div><div class=""ship-status-italic"">" & trackDetail.DeliverySignatureName & "</div>"
                    lbStatus.Text = "Actual delivery :"
                    lbDimensions.Text = trackDetail.PackageDimensions.Length & "x" & trackDetail.PackageDimensions.Width & "x" & trackDetail.PackageDimensions.Height & " in."
                Else
                    liDimension.Visible = False
                    liDimension1.Visible = False
                    liInvoice.Visible = False
                    liInvoice1.Visible = False
                    ltStatus.Text = statusImg _
                     & "<div class=""ship-status intransit"">" & trackDetail.StatusDescription & "</div><div class=""ship-status-italic"">" & Address & "</div>"
                    lbStatus.Text = "Estimated delivery :"
                End If
            Next
        End If

    End Sub

    Sub ShowTrackDetail(ByRef trackDetail As TrackingInfoRow)

        Dim Address As String

        Dim contentDay As String = ""
        Dim d As String = ""



        lthistory.Text = trackDetail.TravelHistory

        lbNumber.Text = trackDetail.TrackingNo
        lbNumer2.Text = trackDetail.TrackingNo
        lbShipTimestamp.Text = trackDetail.ShipTimestamp.ToString("ddd MM/dd/yyyy")

        lbReference.Text = trackDetail.Reference
        lbordernumber.Text = trackDetail.OrderNumber
        lbShipmentID.Text = trackDetail.ShipmentId


        lbService.Text = trackDetail.Service



        lbWeight.Text = trackDetail.Weight & " lbs" '& trackDetail.PackageWeight.Units

        lbTotalpieces.Text = trackDetail.Pieces
        'If (trackDetail.ShipmentWeight IsNot Nothing) Then
        'End If
        If (trackDetail.Packaging IsNot Nothing) Then
            lbPackaging.Text = trackDetail.Packaging
        End If
        'If (trackDetail.ShipTimestampSpecified) Then
        '    'Message &= "note 7--Ship timestamp: " & trackDetail.ShipTimestamp
        'End If

        'If (trackDetail.DestinationAddress IsNot Nothing) Then
        '    lbActualAddress.Text = trackDetail.DestinationAddress
        '    lbActualDeliveryTimestamp.Text = trackDetail.EstimatedDeliveryTimestamp.ToString("ddd MM/dd/yyyy hh:mm tt")
        'End If


        lbActualAddress.Text = trackDetail.ActualAddress
        lbActualDeliveryTimestamp.Text = IIf(trackDetail.ActualDeliveryTimestam.ToString.Contains("0001"), "N/A", trackDetail.ActualDeliveryTimestam.ToString("ddd MM/dd/yyyy hh:mm tt"))
        lbActualDeliveryTimestamp.Text = ReturnMessage(lbActualDeliveryTimestamp.Text)


        'If (trackDetail.SignatureProofOfDeliveryAvailableSpecified) Then
        '    'Message &= "note 10--SPOD availability: " & trackDetail.SignatureProofOfDeliveryAvailable
        'End If

        'If (trackDetail.NotificationEventsAvailable IsNot Nothing) Then
        '    For Each notificationEventType As FedexGetTracking.EMailNotificationEventType In trackDetail.NotificationEventsAvailable
        '        ' Message &= "note 11--EmailNotificationEvent type : " & notificationEventType
        '    Next
        'End If
        Address = trackDetail.StatusAddress
        If Trim(trackDetail.Dimensions) <> Nothing Then
            lbDimensions.Text = trackDetail.Dimensions
        Else
            liDimension1.Visible = False
        End If
        If Trim(trackDetail.InvoiceNumber) <> Nothing Then
            lbInvoicenumber.Text = trackDetail.InvoiceNumber
        Else
            liInvoice.Visible = False
            liInvoice1.Visible = False
        End If
        If Trim(trackDetail.DoorTagNumber) <> Nothing Then
            lbDoorTagNumber.Text = trackDetail.DoorTagNumber
        Else
            liDoorTagNumber.Visible = False
            liDoorTagNumber1.Visible = False
        End If
        If Trim(trackDetail.StatusExceptionDescription) <> Nothing And Trim(trackDetail.StatusCode) = "DE" Then
            dvError.Visible = True
            lbExceptionDescription.Text = trackDetail.StatusExceptionDescription
        End If


        Dim statusImg As String = ""
        statusImg = "<div><img src='" & WebRoot & "/includes/theme/images/" & ReturnImgStatus(Trim(trackDetail.StatusCode), LCase(trackDetail.StatusDescription)) & "'></div>"
        If trackDetail.StatusDescription.Contains("Delivered") Then
            ltStatus.Text = statusImg _
            & "<div class=""ship-status delivered"">" & trackDetail.StatusDescription & "</div><div class=""ship-status-italic"">" & trackDetail.DeliverySignatureName & "</div>"
            lbStatus.Text = "Actual delivery :"
            ' lbDimensions.Text = trackDetail.Dimensions
        Else
            ' liDimension.Visible = False
            ltStatus.Text = statusImg _
             & "<div class=""ship-status intransit"">" & trackDetail.StatusDescription & "</div><div class=""ship-status-italic"">" & Address & "</div>"
            lbStatus.Text = "Estimated delivery :"
        End If


    End Sub
    Private Function ReturnImgStatus(ByVal StatusCode As String, ByVal StatusDescription As String) As String
        Dim img As String = ""
        Select Case StatusCode
            Case "DL"
                img = "the-nailsuperstore-delivered.jpg"
            Case "IT"
                img = "the-nailsuperstore-intransit.jpg"
            Case "PU"
                img = "the-nailsuperstore-pickedup.jpg"
            Case "DE"
                img = "the-nailsuperstore-exception.jpg"
            Case Else
                img = "the-nailsuperstore-first.jpg"
        End Select
        If StatusDescription = "in transit" Then
            img = "the-nailsuperstore-intransit.jpg"
        End If
        Return img
    End Function
    Private Function ReturnMessage(str As String) As String
        str = str.Replace("12:00 AM", "by end of day")
        Return str
    End Function
End Class

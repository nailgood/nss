Imports DataLayer
Imports Components
Partial Class store_send_tracking
    Inherits System.Web.UI.Page

    Protected OrderId As Integer = Nothing
    Public webRoot As String

    Protected dbOrder As StoreOrderRow
    Protected TrackingId As String
    Private UrlTracking As String = ""
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        webRoot = Utility.ConfigData.GlobalRefererName

        If Not Request("OrderId") = Nothing Then
            Try
                OrderId = Request("OrderId")
                TrackingId = Request("trackingid")
                'OrderId = Utility.Crypt.DecryptTripleDes(Request("OrderId"))
                'TrackingId = Utility.Crypt.DecryptTripleDes(Request("trackingid"))
                tracking.m_TrackingId = TrackingId
                tracking.OrderId = OrderId
            Catch ex As Exception
                OrderId = Nothing
            End Try
        End If

        If Not IsPostBack Then
            Dim redirect As Boolean = False
            Try
                BindData()
            Catch ex As Exception
                redirect = True
            End Try
        End If
    End Sub

    Private Sub BindData()
        Dim sp As New SitePage()
        tracking.Cart = New ShoppingCart(sp.DB, OrderId, True)

        Dim ShippingType As Integer = 0
        Try
            ShippingType = CInt(tracking.Cart.Order.CarrierType)
        Catch ex As Exception
            ShippingType = 16
            Email.SendError("ToError500", "SEND TRACKING NUMBER", "OrderId = " & OrderId & "<br>TrackingId = " & TrackingId)
        End Try
        ltLinkTracking.Text = "<a href='" & ReturnLinkTracking(ShippingType) & "' target='_blank'>" & TrackingId & "</a>"
        tracking.UrlTracking = UrlTracking
    End Sub
    Private Function ReturnLinkTracking(ByVal ShipmentType As Integer) As String

        Select Case ShipmentType
            Case 1, 2, 3, 14 'UPS
                UrlTracking = String.Format(Resources.Msg.LinkTrackingNumberUPS, TrackingId)
            Case 16, 17, 18 'FedEx
                UrlTracking = webRoot & String.Format(Resources.Msg.LinkTrackingOnWeb, TrackingId)
            Case Utility.Common.USPSPriorityShippingId
                UrlTracking = String.Format(Resources.Msg.LinkTrackingNumberUSPS, TrackingId)
            Case Utility.Common.TruckShippingId
                UrlTracking = webRoot & String.Format(Resources.Msg.LinkTrackingTruck, OrderId)
            Case Else
                UrlTracking = Resources.Msg.LinkTrackingTruck
        End Select

        Return UrlTracking
    End Function
End Class

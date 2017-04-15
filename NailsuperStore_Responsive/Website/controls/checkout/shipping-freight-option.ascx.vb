

Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_shipping_freight_option
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Private m_Cart As ShoppingCart = Nothing
    Private m_Order As StoreOrderRow

    Public Property Cart() As ShoppingCart
        Set(ByVal value As ShoppingCart)
            m_Cart = value
        End Set
        Get
            Return m_Cart
        End Get
    End Property
    Public Property Order() As StoreOrderRow
        Set(ByVal value As StoreOrderRow)
            m_Order = value
        End Set
        Get
            Return m_Order
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Cart Is Nothing Then
            Cart = Session("cartRender")

        End If

        If Not Cart Is Nothing Then
            If Order Is Nothing Then
                Order = Cart.Order

            End If
        End If
        If Not Cart Is Nothing AndAlso Not Order Is Nothing AndAlso Cart.GetCartItemCount() > 0 Then
            Dim CarrierType As Integer = Utility.Common.GetDefaultShippingByOrderId(Cart.DB, Order.OrderId)
            If Cart.HasOversizeItems AndAlso CarrierType <> Utility.Common.PickupShippingId Then

                If (Cart.Order.ShipToCountry = "US" AndAlso Cart.CheckShippingSpecialUS = False) OrElse (Cart.Order.BillToCountry = "US" AndAlso Cart.Order.IsSameAddress AndAlso Cart.CheckShippingSpecialUS = False) Then
                    Dim isLiftGate As Boolean = False
                    Dim IsScheduleDelivery As Boolean = False
                    Dim IsInsideDelivery As Boolean = False
                    StoreOrderRow.GetFreightShippingOption(Cart.Order.OrderId, isLiftGate, IsScheduleDelivery, IsInsideDelivery)
                    If (isLiftGate) Then
                        chkLiftGate.Checked = True
                    End If
                    If (IsInsideDelivery) Then
                        chkInsideDelivery.Checked = True
                    End If
                    If (IsScheduleDelivery) Then
                        chkScheduleDelivery.Checked = True
                    End If
                    Dim surcharge As Double = SysParam.GetValue("LiftGateCharge")
                    '' chkIsLiftGate.Attributes.Add("onChange", "RequestOversizeFee(" & ci.CartItemId & ",this,1);"))
                    lblLiftGate.InnerHtml = "Click here to request a lift gate. " & IIf(surcharge > 0, " A single surcharge of " & FormatCurrency(surcharge) & " will be added to your order.", "") & ""

                    surcharge = SysParam.GetValue("ScheduleDeliveryCharge")
                    lblScheduleDelivery.InnerHtml = "Click here to request a scheduled delivery. " & IIf(surcharge > 0, " A single surcharge of " & FormatCurrency(surcharge) & " will be added to your order.", "")


                    surcharge = SysParam.GetValue("InsideDeliveryService")
                    lblInsideDelivery.InnerHtml = "Click here to request an inside delivery. " & IIf(surcharge > 0, " A single surcharge of " & FormatCurrency(surcharge) & " will be added to your order.", "")

                Else
                    Me.Visible = False
                End If


            Else
                Me.Visible = False
            End If
        End If

    End Sub

End Class




Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports System.Net
Imports System.IO

Partial Class controls_checkout_shipping_list
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
    Private m_ListShippingMethod As DataSet
    Private m_ShippingMethod As Integer = 0
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
    Public Property ListShippingMethod() As DataSet
        Set(ByVal value As DataSet)
            m_ListShippingMethod = value
        End Set
        Get
            Return m_ListShippingMethod
        End Get
    End Property
    Public Property ShippingMethod() As Integer
        Set(ByVal value As Integer)
            m_ShippingMethod = value
        End Set
        Get
            Return m_ShippingMethod
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

            If (Order.ShipToCountry = "US" AndAlso Cart.CheckShippingSpecialUS = False) OrElse (Order.BillToCountry = "US" AndAlso Order.IsSameAddress AndAlso Cart.CheckShippingSpecialUS = False) Then
                If ListShippingMethod Is Nothing Then
                    ListShippingMethod = Cart.GetShippingMethods()
                End If
                If ShippingMethod < 1 Then
                    ShippingMethod = Utility.Common.GetDefaultShippingByOrderId(DB, Order.OrderId)
                End If

                If ListShippingMethod IsNot Nothing AndAlso ListShippingMethod.Tables.Count > 0 Then
                    BindShippingMethod(ListShippingMethod, ShippingMethod)
                End If
            Else
                BindInternationShippingMethod()
            End If
        End If

    End Sub
    Private Sub BindShippingMethod(ByVal ds As DataSet, ByVal defaultSelect As Integer)
        If ds IsNot Nothing AndAlso ds.Tables.Count > 0 Then
            Dim pickupMsg As String = String.Empty
            Dim methodid As Integer = 0
            Dim Name As String = String.Empty
            Dim shippingRow As String = String.Empty
            For Each dtRow As DataRow In ds.Tables(0).Rows
                methodid = dtRow("methodid")
                Name = dtRow("Name")
                shippingRow = "<li><input type='radio' onclick='ChangeShippingMethod(" & methodid & ")'  {0} name='rdoShipping' id='rdoShipping_" & methodid & "' class='radio-node'/>" &
                    "<label for='rdoShipping_" & methodid & "' class='radio-label rdoShipping'></label><span class='radio-text' onclick='ChangeShippingMethod(" & methodid & ")'>" & Name & "<span> " &
                     Command.calculateDateShipping(DateTime.Now, dtRow("Days"), True, True) & "</li>"
                If defaultSelect = methodid Then
                    shippingRow = String.Format(shippingRow, "checked='checked'")
                    hidShippingSelect.Value = defaultSelect
                Else
                    shippingRow = String.Format(shippingRow, "")
                End If
                ltrListShippingMethod.Text &= shippingRow
                If methodid = Utility.Common.PickupShippingId Then
                    If defaultSelect = Utility.Common.PickupShippingId Then 'Pickup from warehouse
                        pickupMsg = "<li class='msg' style='display:block;' id='msgPickup'>Must come to pick up your order at our Franklin Park warehouse within 2 days." & Utility.Common.RenderUserControl("~/controls/checkout/pickup-from-warehouse.ascx", False) & "</li>"
                        rdoRadShipVia.Attributes.Add("style", "width:100%;")
                    ElseIf Utility.Common.PickupShippingId = methodid Then
                        pickupMsg = "<li class='msg' id='msgPickup'>Must come to pick up your order at our Franklin Park warehouse within 2 days." + Utility.Common.RenderUserControl("~/controls/checkout/pickup-from-warehouse.ascx", False) + "</li>"
                    End If
                End If
            Next
            ltrListShippingMethod.Text = ltrListShippingMethod.Text & pickupMsg

        End If
    End Sub
    Private Sub BindInternationShippingMethod()

        If Cart.OnlyOversizeItems = True Then
            ltrListShippingMethod.Text = "<li><input type='radio' param='" & Utility.Common.TruckShippingId & "' checked='true' name='rdoShipping' id='rdoShipping_" & Utility.Common.TruckShippingId & "' class='radio-node'/><label for='rdoShipping_" & Utility.Common.TruckShippingId & "' class='radio-label rdoShipping'></label>" & Utility.Common.FreightDeliveryShippingName() & " </li>"
        Else
            ltrListShippingMethod.Text = "<li><input type='radio' param='" & Utility.Common.USPSPriorityShippingId & "' checked='true' name='rdoShipping' id='rdoShipping_" & Utility.Common.USPSPriorityShippingId & "' class='radio-node'/><label for='rdoShipping_" & Utility.Common.USPSPriorityShippingId & "' class='radio-label rdoShipping'></label>" & Utility.Common.GetShippingInternationalMethodName() & " </li>"

        End If
    End Sub
End Class

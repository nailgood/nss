
Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_confirm_freight_option
    Inherits ModuleControl

    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Private m_OrderId As Integer
    Public Property OrderId() As Integer
        Set(ByVal value As Integer)
            m_OrderId = value
        End Set
        Get
            Return m_OrderId
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If OrderId < 1 Then
            OrderId = System.Web.HttpContext.Current.Session("orderIdRender")
        End If
        liLiftGate.Visible = False
        liScheduleDelivery.Visible = False
        liInsideDelivery.Visible = False
        Dim isLiftGate As Boolean = False
        Dim IsScheduleDelivery As Boolean = False
        Dim IsInsideDelivery As Boolean = False
        StoreOrderRow.GetFreightShippingOption(OrderId, isLiftGate, IsScheduleDelivery, IsInsideDelivery)
        Dim surcharge As Double = 0
        If (isLiftGate) Then
            liLiftGate.Visible = True
            surcharge = SysParam.GetValue("LiftGateCharge")
            ltrLiftGate.Text = "You requested a lift gate for this product. A single surcharge of " & FormatCurrency(surcharge) & " was added to shipping costs for your order."
        End If
        If (IsScheduleDelivery) Then
            liScheduleDelivery.Visible = True
            surcharge = SysParam.GetValue("ScheduleDeliveryCharge")
            ltrScheduleDelivery.Text = " You requested a scheduled delivery. A single surcharge of " & FormatCurrency(surcharge) & " was added to your order."
        End If
        If (IsInsideDelivery) Then
            liInsideDelivery.Visible = True
            surcharge = SysParam.GetValue("InsideDeliveryService")
            ltrInsideDelivery.Text = " You requested an inside delivery. A single surcharge of " & FormatCurrency(surcharge) & " was added to your order."
        End If
        If isLiftGate = False AndAlso IsInsideDelivery = False AndAlso IsScheduleDelivery = False Then
            Me.Visible = False
        End If
    End Sub


End Class

Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_reward_point_summary
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Public Cart As ShoppingCart = Nothing
    Private m_TotalPointAvailable As Integer
    Private m_PendingPoint As Integer
    'Public m_TotalRewardsPoint As Integer
    Public m_PurchasePoint As Integer
    Public Property TotalPointAvailable() As Integer
        Set(ByVal value As Integer)
            m_TotalPointAvailable = value
        End Set
        Get
            Return m_TotalPointAvailable
        End Get
    End Property
    Public Property PendingPoint() As Integer
        Set(ByVal value As Integer)
            m_PendingPoint = value
        End Set
        Get
            Return m_PendingPoint
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("CheckOutGuest") = "1" Then
            Me.Visible = False
            Exit Sub
        End If

        If Not IsPostBack Then
            If Cart Is Nothing Then
                Cart = Session("CartRender")
            End If
            If Cart Is Nothing Then
                Dim orderId As Integer = Session("OrderId")
                If orderId < 1 Then
                    orderId = Utility.Common.GetOrderIdFromCartCookie()
                End If
                If orderId > 0 Then
                    Cart = New ShoppingCart(DB, orderId, False)
                End If
            End If
            
            BindData()

        End If
    End Sub
    Private Sub BindData()
        Dim MoneyEachPoint As Double = SysParam.GetValue("MoneyEachPoint")
        TotalPointAvailable = CashPointRow.GetTotalCashPointByMember(DB, Cart.Order.MemberId, Cart.Order.OrderId)
        PendingPoint = CashPointRow.GetPendingCashPoint(DB, Cart.Order.MemberId)

        tdRewardsPoint.Text = String.Format("<div class=""point-icon"">My Cash Rewards Points <a href=""javascript:void(ShowCashPointTip('{0}'));""><span class=""question""></span></a></div><div class=""point-detail""><a onclick=""showPointBalancePopup();"">Detail</a></div>", Resources.Msg.PopupCashPoint)
        divAvailablePoints.Text = String.Format("${0} ({1} pts)", TotalPointAvailable * MoneyEachPoint, TotalPointAvailable)
        tdPendingPoints.Text = String.Format("${0} ({1} pts)", PendingPoint * MoneyEachPoint, PendingPoint)
        'm_TotalRewardsPoint = Cart.Order.TotalRewardPoint
        m_PurchasePoint = Cart.Order.PurchasePoint
        Dim itemBuyPoint As StoreCartItemRow = StoreCartItemRow.GetRowBuyPointByOrderId(DB, Cart.Order.OrderId)
        divPointPurchase.Visible = True
        m_PurchasePoint = Cart.Order.PurchasePoint
        If Not (itemBuyPoint Is Nothing) AndAlso itemBuyPoint.ItemId > 0 Then
            trBuyPoint.Visible = True
            If (Request.RawUrl.Contains("payment.aspx")) Then
                divPointPurchase.Visible = False
            End If
        Else
            trBuyPoint.Visible = False
        End If

    End Sub
End Class

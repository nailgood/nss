Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic

Partial Class controls_checkout_rewards_points_cart
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Private m_Cart As ShoppingCart = Nothing
    Public Property Cart() As ShoppingCart
        Set(ByVal value As ShoppingCart)
            m_Cart = value
        End Set
        Get
            Return m_Cart
        End Get
    End Property
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Cart Is Nothing Then
                Cart = Session("cartRender")
            End If

            Dim OrderId As Integer = Utility.Common.GetCurrentOrderId()
            Dim SubTotal As Double = 0
            If OrderId > 0 Then
                If Cart Is Nothing Then
                    SubTotal = StoreOrderRow.GetRow(DB, OrderId).SubTotal
                    ShowMessage(SubTotal)
                ElseIf Cart.Order IsNot Nothing
                    ShowMessage(Cart.Order.SubTotal)
                Else
                    Me.Visible = False
                End If
            Else
                Me.Visible = False
            End If
        End If
    End Sub

    Private Sub ShowMessage(ByVal SubTotal As Double)
        Dim MemberId As Integer = Utility.Common.GetCurrentMemberId()
        Dim OrderId As Integer = Utility.Common.GetCurrentOrderId()
        Dim MoneyEachPoint As Double = SysParam.GetValue("MoneyEachPoint")
        Try
            If MemberId > 0 Then
                Dim member As MemberRow = MemberRow.GetRow(MemberId)
                Dim TotalPointEarned As Double = CashPointRow.GetTotalCashPointByMember(DB, MemberId, OrderId)
                Dim MoneyDebit As Double = TotalPointEarned * MoneyEachPoint
                Dim MoneyEarn As Double = Math.Round(SubTotal) * MoneyEachPoint
                litMsg.Text = String.Format(Resources.Msg.RewardsPointCartLogin, MoneyEarn, SubTotal, MoneyDebit, TotalPointEarned)
            Else
                Dim MoneyEarn As Double = Math.Round(SubTotal) * MoneyEachPoint
                litMsg.Text = String.Format(Resources.Msg.RewardsPointCartUnLogin, MoneyEarn, SubTotal)
            End If
        Catch ex As Exception

        End Try
        
    End Sub
    
End Class

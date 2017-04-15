Imports DataLayer
Imports Components
Imports System.Data.SqlClient
Imports System.Collections.Generic
Partial Class controls_checkout_buy_point
    Inherits ModuleControl
    Public Overrides Property Args() As String
        Get
            Return ""
        End Get
        Set(ByVal Value As String)
        End Set
    End Property
    Public Cart As ShoppingCart = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
           
            If Cart Is Nothing Then
                Dim orderId As Integer = Session("OrderId")
                If orderId < 1 Then
                    orderId = Utility.Common.GetOrderIdFromCartCookie()
                End If
                If orderId > 0 Then
                    Cart = New ShoppingCart(DB, orderId, False)
                End If
            End If
            BindItemPoint()

        End If
    End Sub
    Private Sub BindItemPoint()
        Dim lstItemPoint As StoreItemCollection = StoreItemRow.ListItemBuyPoint()
        Dim itemNull As New ListItem("Please select", String.Empty)
        drpPoint.Items.Add(itemNull)
        For Each itemPoint As StoreItemRow In lstItemPoint
            Dim item As New ListItem(itemPoint.ItemName.Replace("points", "pts"), itemPoint.ItemId)
            drpPoint.Items.Add(item)
        Next
        If Not (Cart.Order Is Nothing) Then
            Dim itemBuyPoint As StoreCartItemRow = StoreCartItemRow.GetRowBuyPointByOrderId(DB, Cart.Order.OrderId)
            If Not (itemBuyPoint Is Nothing) Then
                Try
                    drpPoint.SelectedValue = itemBuyPoint.ItemId
                Catch ex As Exception

                End Try

            End If
        End If
      
    End Sub
End Class

Imports DataLayer
Imports Components

Partial Class store_ConfirmReview
    Inherits System.Web.UI.Page
    Protected OrderId As Integer = Nothing
    Protected dbOrder As StoreOrderRow
    Private Sub GetOrderId()

        Try
            OrderId = Request("OrderId")
        Catch ex As Exception

        End Try
       
        ''OrderId = 30690
    End Sub
    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        GetOrderId()
        If OrderId < 1 Then
            Exit Sub
        End If

        Dim sp As New SitePage()

        If Not IsPostBack Then
            If Not OrderId = Nothing Then
                dbOrder = StoreOrderRow.GetRow(sp.DB, OrderId)

            End If
            Dim redirect As Boolean = False
            Try
                BindData()
            Catch ex As Exception
                redirect = True
            End Try
            'If redirect Then Response.Redirect(GlobalRefererName & "/")
        End If
    End Sub

    Private Sub BindData()
        Dim sp As New SitePage()
        od.Dab = sp.DB
        od.MemberId = dbOrder.MemberId
        od.Cart = New ShoppingCart(sp.DB, OrderId, True)

        Try
            If od.Cart.Order.OrderNo <> Nothing Then
                '' lit.Text = od.Cart.Order.OrderNo
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class

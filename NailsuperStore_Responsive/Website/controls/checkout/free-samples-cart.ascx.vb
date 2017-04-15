
Imports Components
Imports DataLayer
Imports Utility

Partial Class controls_checkout_free_samples_cart
    Inherits BaseControl
    Public Cart As ShoppingCart = Nothing
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Cart Is Nothing Then
                Cart = Session("cartRender")
            End If

            Dim orderId As Integer = Common.GetCurrentOrderId()
            If orderId > 0 Then
                If Cart Is Nothing Then
                    Cart = New ShoppingCart(DB, orderId, False)
                End If

                If Cart IsNot Nothing AndAlso Cart.Order IsNot Nothing Then
                    ShowFreeSamples(Cart.Order.SubTotal)
                Else
                    Me.Visible = False
                End If
            Else
                Me.Visible = False
            End If
        End If
    End Sub

    Private Sub ShowFreeSamples(ByVal SubTotal As Double)
        Dim str As String = String.Empty
        Dim QtySample As Integer = CInt(SysParam.GetValue("FreeSampleQty"))
        Dim FreeSamplesTotal As Double = CDbl(SysParam.GetValue("FreeSampleOrderMin"))

        If SubTotal >= FreeSamplesTotal Then
            litFreeSamplesMsg.Text = String.Format("<a href=""javascript:OpenFreeSamples();"" class=""link"">Choose {0} <span class=""lblmain"">FREE samples</span></a>", QtySample)
        Else
            litFreeSamplesMsg.Text = String.Format("<a href=""javascript:OpenFreeSamples();"" class=""link"">Purchase {0:C2} more to receive {1} <span class=""lblmain"">FREE Samples!</span></a>", FreeSamplesTotal - SubTotal, QtySample)
        End If

    End Sub

End Class

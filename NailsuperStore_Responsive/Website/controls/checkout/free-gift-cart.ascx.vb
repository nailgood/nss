
Imports Components
Imports DataLayer
Imports Utility

Partial Class controls_checkout_free_gift_cart
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
                    ShowFreeGift(Cart.Order.SubTotal)
                Else
                    Me.Visible = False
                End If
            Else
                Me.Visible = False
            End If

        End If

    End Sub

    Private Sub ShowFreeGift(ByVal SubTotal As Double)
        Dim str As String = String.Empty
        Dim lstFreeGiftLevel As FreeGiftLevelCollection = FreeGiftLevelRow.GetListActive()
        If lstFreeGiftLevel.Count > 0 Then
            For Each fg As FreeGiftLevelRow In lstFreeGiftLevel
                If SubTotal >= fg.MinValue Then
                    litFreeGiftMsg.Text = String.Format("<span class=""lblbackup"">FREE Gift</span><a href=""javascript:OpenFreeGift();"" class=""link"">{0}</a>", fg.Name)
                    hidFreeGiftLevelId.Value = fg.Id
                End If
            Next

            If String.IsNullOrEmpty(litFreeGiftMsg.Text) Then
                litFreeGiftMsg.Text = String.Format("<a href=""javascript:OpenFreeGift();"" class=""link"">Purchase {0}  more to receive <span class=""lblmain"">FREE Gift!</span></a>", FormatCurrency(CInt(lstFreeGiftLevel(0).MinValue) - Cart.Order.SubTotal))
                hidFreeGiftLevelId.Value = lstFreeGiftLevel(0).Id
            End If
        End If

    End Sub

End Class
